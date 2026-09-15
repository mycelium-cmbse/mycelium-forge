import {
  collidingSemanticGroups,
  dereferenceCollidingTokens,
  getCachedPrimitives,
  normalizeDimensionPrimitives
} from './dtcg-normalizers.mjs';
import { getFileHeader, mapTokenToTailwindThemeProperty } from './tailwind-theme-mapper.mjs';

/**
 * Registers custom Style Dictionary parsers and formats for DTCG tokens and Tailwind v4.
 * @param {object} StyleDictionary - The StyleDictionary class or instance.
 */
export function registerCustomPlugins(StyleDictionary) {
  /**
   * Custom Style Dictionary parser for DTCG dimension primitives (mycelium-primitives.json).
   */
  StyleDictionary.registerParser({
    name: 'dtcg-primitives-shim',
    pattern: /mycelium-primitives\.json$/,
    parser: ({ contents }) => {
      const parsed = JSON.parse(contents);
      normalizeDimensionPrimitives(parsed);
      return parsed;
    }
  });

  /**
   * Custom Style Dictionary parser for mode-specific semantic tokens (mycelium-semantics.json).
   */
  StyleDictionary.registerParser({
    name: 'dtcg-semantics-shim',
    pattern: /mycelium-semantics\.json$/,
    parser: ({ contents, filePath }) => {
      const parsed = JSON.parse(contents);
      const primitives = getCachedPrimitives(filePath);

      for (const groupName of collidingSemanticGroups) {
        dereferenceCollidingTokens(parsed[groupName], primitives[groupName]);
      }

      delete parsed.$extensions;
      return parsed;
    }
  });

  /**
   * Custom Style Dictionary parser for DTCG typography definitions (typography.json).
   */
  StyleDictionary.registerParser({
    name: 'dtcg-typography-curated-filter',
    pattern: /typography\.json$/,
    parser: ({ contents }) => {
      const parsed = JSON.parse(contents);
      if (parsed?.typography?.typography?.raw) {
        delete parsed.typography.typography.raw;
      }
      return parsed;
    }
  });

  /**
   * Custom Style Dictionary format generating Tailwind v4 @theme inline block from DTCG tokens.
   */
  StyleDictionary.registerFormat({
    name: 'css/tailwind-theme',
    format: ({ dictionary, file }) => {
      const fileName = file?.destination || 'theme.css';
      const lines = [
        getFileHeader(fileName),
        '',
        '@theme inline {'
      ];

      for (const token of dictionary.allTokens) {
        lines.push(...mapTokenToTailwindThemeProperty(token));
      }

      lines.push('}', '');
      return lines.join('\n');
    }
  });
}
