package com.starion.forge.keycloak;

/**
 * Result of checking whether a scope short name exists in Forge.
 */
public enum Lookup {

    /**
     * The short name does not belong to any existing account or organization.
     */
    FREE,

    /**
     * The short name is already in use by an existing account or organization.
     */
    TAKEN,

    /**
     * Forge could not be reached or answered with an unexpected response status.
     */
    UNAVAILABLE
}
