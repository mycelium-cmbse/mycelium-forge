package com.starion.forge.keycloak;

import org.keycloak.Config;
import org.keycloak.models.KeycloakSession;
import org.keycloak.models.KeycloakSessionFactory;
import org.keycloak.validate.Validator;
import org.keycloak.validate.ValidatorFactory;

/**
 * Registers the {@link ScopeShortNameUniqueValidator} with Keycloak so it can be configured on the
 * username attribute in the realm's User Profile settings.
 */
public class ScopeShortNameUniqueValidatorFactory implements ValidatorFactory {

    /**
     * Singleton instance of the validator.
     */
    private static final ScopeShortNameUniqueValidator VALIDATOR = new ScopeShortNameUniqueValidator();

    /**
     * Gets the unique provider identifier matching {@link ScopeShortNameUniqueValidator#ID}.
     *
     * @return The provider identifier.
     */
    @Override
    public String getId() {
        return ScopeShortNameUniqueValidator.ID;
    }

    /**
     * Creates or returns the validator instance for the current Keycloak session.
     *
     * @param session The current Keycloak session.
     * @return The {@link Validator} instance.
     */
    @Override
    public Validator create(KeycloakSession session) {
        return VALIDATOR;
    }

    /**
     * Initializes the validator factory with configuration parameters.
     *
     * @param config The configuration scope.
     */
    @Override
    public void init(Config.Scope config) {
    }

    /**
     * Performs post-initialization tasks once the session factory has been created.
     *
     * @param factory The Keycloak session factory.
     */
    @Override
    public void postInit(KeycloakSessionFactory factory) {
    }

    /**
     * Closes resources associated with the validator factory when shutting down.
     */
    @Override
    public void close() {
    }
}
