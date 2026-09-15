import fs from 'node:fs';
import path from 'node:path';

/**
 * Standard CSS dimension value for full border radius.
 */
export const fullRadiusDimension = '999px';

/**
 * Semantic groups that collide with primitive collections.
 */
export const collidingSemanticGroups = ['radius', 'spacing', 'text'];

/**
 * Cache of parsed shared primitives indexed by resolved file path.
 */
const primitivesCache = new Map();

/**
 * Loads and caches parsed JSON primitives from disk.
 * @param {string} filePath - Path to a semantics file in a sibling directory.
 * @returns {object} Parsed primitives JSON tree.
 */
export function getCachedPrimitives(filePath) {
  const primitivesFilePath = path.resolve(path.dirname(filePath), '..', 'shared', 'mycelium-primitives.json');
  if (!primitivesCache.has(primitivesFilePath)) {
    primitivesCache.set(primitivesFilePath, JSON.parse(fs.readFileSync(primitivesFilePath, 'utf8')));
  }
  return primitivesCache.get(primitivesFilePath);
}

/**
 * Normalizes a primitive scalar value into a CSS dimension string.
 * @param {string} key - Token key name.
 * @param {object} token - Token definition object.
 * @returns {string|*} The normalized dimension string or original token value.
 */
export function normalizeDimensionValue(key, token) {
  if (!token) {
    return undefined;
  }

  // Style Dictionary may store transformed token output in .value, while raw DTCG source defines .$value.
  const rawValue = token.value ?? token.$value;

  if (token.$type === 'number' && typeof rawValue === 'number') {
    return key === 'full' ? fullRadiusDimension : `${rawValue}px`;
  }

  if (typeof rawValue === 'object' && rawValue !== null && 'value' in rawValue) {
    return `${rawValue.value}px`;
  }

  return rawValue;
}

/**
 * Determines whether a token node is a raw DTCG dimension primitive.
 * @param {object} node - Token node to evaluate.
 * @returns {boolean} True if the node is a numeric primitive; otherwise, false.
 */
export function isDimensionPrimitive(node) {
  return node.$type === 'number' && typeof node.$value === 'number';
}

/**
 * Recursively normalizes dimension primitives in a DTCG token tree.
 * @param {object} tokenTree - Object tree containing token definitions.
 */
export function normalizeDimensionPrimitives(tokenTree) {
  for (const [key, value] of Object.entries(tokenTree)) {
    if (!value || typeof value !== 'object') {
      continue;
    }

    if (!isDimensionPrimitive(value)) {
      normalizeDimensionPrimitives(value);
      continue;
    }

    const normalized = normalizeDimensionValue(key, value);
    value.$type = 'dimension';
    value.$value = normalized;
  }
}

/**
 * Determines whether a token node represents a leaf token definition.
 * @param {object} node - Token node to evaluate.
 * @returns {boolean} True if the node defines a value or type; otherwise, false.
 */
export function isLeafToken(node) {
  return node.$value !== undefined || node.$type !== undefined;
}

/**
 * Applies resolved primitive values to a semantic token.
 * @param {object} token - Target semantic token.
 * @param {string} key - Token key name.
 * @param {object} primitive - Source primitive definition.
 */
export function applyPrimitiveToToken(token, key, primitive) {
  if (!primitive) {
    return;
  }

  token.$type = 'dimension';
  token.$value = normalizeDimensionValue(key, primitive);

  if (!token.$description && primitive.$description) {
    token.$description = primitive.$description;
  }
}

/**
 * Recursively dereferences colliding semantic token groups against shared primitives.
 * @param {object} semanticGroup - Group of semantic tokens.
 * @param {object} primitiveGroup - Group of primitive tokens.
 */
export function dereferenceCollidingTokens(semanticGroup, primitiveGroup) {
  if (!semanticGroup || !primitiveGroup) {
    return;
  }

  for (const [key, token] of Object.entries(semanticGroup)) {
    if (!token || typeof token !== 'object') {
      continue;
    }

    const primitive = primitiveGroup[key];

    if (isLeafToken(token)) {
      applyPrimitiveToToken(token, key, primitive);
      continue;
    }

    if (primitive && typeof primitive === 'object') {
      dereferenceCollidingTokens(token, primitive);
    }
  }
}
