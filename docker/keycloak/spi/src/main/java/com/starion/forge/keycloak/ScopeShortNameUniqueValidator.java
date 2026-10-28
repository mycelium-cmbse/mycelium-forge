package com.starion.forge.keycloak;

import java.io.IOException;
import java.net.URI;
import java.net.URLEncoder;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.time.Duration;
import java.util.Locale;

import org.jboss.logging.Logger;
import org.keycloak.models.UserModel;
import org.keycloak.userprofile.UserProfileAttributeValidationContext;
import org.keycloak.validate.AbstractSimpleValidator;
import org.keycloak.validate.ValidationContext;
import org.keycloak.validate.ValidationError;
import org.keycloak.validate.ValidatorConfig;

/**
 * Rejects a username that equals the short name of an existing Forge scope (an Account or an Organization).
 * The check runs when the user is created and whenever the username of an existing user changes.
 * It queries Forge through <code>GET /api/v1/scopes/shortnames/{shortName}</code>: 200 means the short name
 * is taken, 404 means it is free. Any other outcome rejects the value (fail closed).
 */
public class ScopeShortNameUniqueValidator extends AbstractSimpleValidator {

    /**
     * The unique identifier of this validator SPI.
     */
    public static final String ID = "forge-scope-shortname-unique";

    /**
     * Message key when the username is already taken by a scope.
     */
    public static final String MESSAGE_TAKEN = "usernameNotAvailable";

    /**
     * Message key when Forge is unreachable or encounters an error.
     */
    public static final String MESSAGE_UNAVAILABLE = "usernameCheckFailed";

    /**
     * Logger instance for validator events.
     */
    private static final Logger LOGGER = Logger.getLogger(ScopeShortNameUniqueValidator.class);

    /**
     * Environment variable containing the Forge HTTP API base URL.
     */
    private static final String BASE_URL_VARIABLE = "FORGE_SPI_BASE_URL";

    /**
     * Environment variable containing the shared API key for Forge API authentication.
     */
    private static final String API_KEY_VARIABLE = "FORGE_SPI_API_KEY";

    /**
     * Request header name carrying the shared API key.
     */
    private static final String API_KEY_HEADER = "X-Forge-Api-Key";

    /**
     * HTTP connection and request timeout when calling Forge.
     */
    private static final Duration TIMEOUT = Duration.ofSeconds(5);

    /**
     * HTTP client used to perform scope lookups against Forge.
     */
    private final HttpClient httpClient = HttpClient.newBuilder().connectTimeout(TIMEOUT).build();

    /**
     * Gets the unique provider identifier.
     *
     * @return The validator ID.
     */
    @Override
    public String getId() {
        return ID;
    }

    /**
     * Determines whether validation should be skipped for the given attribute value.
     *
     * @param value  The value to evaluate.
     * @param config The validator configuration.
     * @return {@code true} if validation should be skipped; otherwise {@code false}.
     */
    @Override
    protected boolean skipValidation(Object value, ValidatorConfig config) {
        return value == null || value.toString().isBlank();
    }

    /**
     * Validates that the requested username does not collide with an existing scope short name.
     *
     * @param value     The username value to validate.
     * @param inputHint The attribute name or input hint.
     * @param context   The validation context.
     * @param config    The validator configuration.
     */
    @Override
    protected void doValidate(Object value, String inputHint, ValidationContext context, ValidatorConfig config) {
        var username = value.toString().trim().toLowerCase(Locale.ROOT);

        if (isUnchanged(username, context)) {
            return;
        }

        switch (lookup(username)) {
            case TAKEN -> context.addError(new ValidationError(ID, inputHint, MESSAGE_TAKEN));
            case UNAVAILABLE -> context.addError(new ValidationError(ID, inputHint, MESSAGE_UNAVAILABLE));
            case FREE -> {
            }
        }
    }

    /**
     * Determines whether the username being validated is identical to the current user's existing username.
     *
     * @param username The normalized username to check.
     * @param context  The validation context.
     * @return {@code true} if the username is unchanged for an existing user; otherwise {@code false}.
     */
    private static boolean isUnchanged(String username, ValidationContext context) {
        if (!(context instanceof UserProfileAttributeValidationContext attributeContext)) {
            return false;
        }

        UserModel user = attributeContext.getAttributeContext().getUser();

        return user != null && username.equalsIgnoreCase(user.getUsername());
    }

    /**
     * Executes an HTTP GET request against the Forge short name endpoint to determine availability.
     *
     * @param username The normalized short name to look up.
     * @return A {@link Lookup} status indicating whether the name is free, taken, or lookup failed.
     */
    private Lookup lookup(String username) {
        var baseUrl = System.getenv(BASE_URL_VARIABLE);
        var apiKey = System.getenv(API_KEY_VARIABLE);

        if (baseUrl == null || baseUrl.isBlank() || apiKey == null || apiKey.isBlank()) {
            LOGGER.errorf("%s and %s must be set to validate usernames against Forge", BASE_URL_VARIABLE, API_KEY_VARIABLE);
            return Lookup.UNAVAILABLE;
        }

        var encoded = URLEncoder.encode(username, StandardCharsets.UTF_8).replace("+", "%20");
        var uri = URI.create(baseUrl.replaceAll("/+$", "") + "/api/v1/scopes/shortnames/" + encoded);
        var request = HttpRequest.newBuilder(uri)
                .GET()
                .header(API_KEY_HEADER, apiKey)
                .timeout(TIMEOUT)
                .build();

        try {
            var status = httpClient.send(request, HttpResponse.BodyHandlers.discarding()).statusCode();

            return switch (status) {
                case 200 -> Lookup.TAKEN;
                case 404 -> Lookup.FREE;
                default -> {
                    LOGGER.errorf("Forge answered %d when looking up the short name '%s'", status, username);
                    yield Lookup.UNAVAILABLE;
                }
            };
        } catch (IOException exception) {
            LOGGER.error("Forge could not be reached to look up a short name", exception);
            return Lookup.UNAVAILABLE;
        } catch (InterruptedException exception) {
            Thread.currentThread().interrupt();
            return Lookup.UNAVAILABLE;
        }
    }
}
