# Forge's Keycloak (DD-20): one image for every environment. The realm, client, roles and theme are
# baked in so dev and prod can never drift; environments differ only through environment variables
# (see the ${...} placeholders in forge-realm.json). Build context: docker/keycloak.
FROM maven:3.9-eclipse-temurin-21 AS spi
WORKDIR /build
COPY spi/pom.xml .
RUN mvn -q -B dependency:go-offline
COPY spi/src src
RUN mvn -q -B package -DskipTests

FROM quay.io/keycloak/keycloak:26.0

COPY --from=spi /build/target/forge-keycloak-spi.jar /opt/keycloak/providers/forge-keycloak-spi.jar
COPY themes/forge /opt/keycloak/themes/forge
COPY forge-realm.json /opt/keycloak/data/import/forge-realm.json

USER 1000

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]
