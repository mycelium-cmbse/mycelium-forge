# Forge's Keycloak (DD-20): one image for every environment. The realm, client, roles and theme are
# baked in so dev and prod can never drift; environments differ only through environment variables
# (see the ${...} placeholders in forge-realm.json). Build context: docker/keycloak.
FROM quay.io/keycloak/keycloak:26.0

COPY themes/forge /opt/keycloak/themes/forge
COPY forge-realm.json /opt/keycloak/data/import/forge-realm.json

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]
