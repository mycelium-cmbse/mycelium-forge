import fs from 'node:fs';
import path from 'node:path';
import StyleDictionary from 'style-dictionary';

/**
 * Standard CSS dimension value for full border radius.
 */
const fullRadiusDimension = '999px';

/**
 * Semantic groups that collide with primitive collections.
 */
const collidingSemanticGroups = ['radius', 'spacing', 'text'];

/**
 * Normalizes a primitive scalar value into a CSS dimension string.
 * @param {string} key - Token key name.
 * @param {object} token - Token definition object.
 * @returns {string|*} The normalized dimension string or original token value.
 */
function normalizeDimensionValue(key, token) {
  if (!token) {
    return undefined;
  }

  if (token.$type === 'number' && typeof token.$value === 'number') {
    return key === 'full' ? fullRadiusDimension : `${token.$value}px`;
  }

  if (typeof token.$value === 'object' && token.$value !== null && 'value' in token.$value) {
    return `${token.$value.value}px`;
  }

  return token.$value;
}

/**
 * Recursively normalizes dimension primitives in a DTCG token tree.
 * @param {object} tokenTree - Object tree containing token definitions.
 */
function normalizeDimensionPrimitives(tokenTree) {
  for (const [key, value] of Object.entries(tokenTree)) {
    if (value && typeof value === 'object') {
      if (value.$type === 'number' && typeof value.$value === 'number') {
        value.$type = 'dimension';
        value.$value = normalizeDimensionValue(key, value);
      } else {
        normalizeDimensionPrimitives(value);
      }
    }
  }
}

/**
 * Dereferences colliding semantic token groups against shared primitives.
 * @param {object} semanticGroup - Group of semantic tokens.
 * @param {object} primitiveGroup - Group of primitive tokens.
 */
function dereferenceCollidingTokens(semanticGroup, primitiveGroup) {
  if (!semanticGroup || !primitiveGroup) {
    return;
  }

  for (const [key, token] of Object.entries(semanticGroup)) {
    if (token && typeof token === 'object') {
      const primitiveToken = primitiveGroup[key];
      if (primitiveToken) {
        token.$type = 'dimension';
        token.$value = normalizeDimensionValue(key, primitiveToken);
        if (!token.$description && primitiveToken.$description) {
          token.$description = primitiveToken.$description;
        }
      }
    }
  }
}


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
    const primitivesFilePath = path.resolve(path.dirname(filePath), '..', 'shared', 'mycelium-primitives.json');
    const primitives = JSON.parse(fs.readFileSync(primitivesFilePath, 'utf8'));

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
 * Maps a design token to its corresponding Tailwind @theme custom property declaration.
 * @param {object} token - Style Dictionary token.
 * @returns {string|null} CSS variable declaration line or null if not applicable.
 */
function mapTokenToTailwindThemeProperty(token) {
  const comment = token.$description ? ` /** ${token.$description} */` : '';

  if (token.$type === 'shadow') {
    return `  --${token.name}: var(--${token.name});${comment}`;
  }

  if (token.$type === 'color' && token.path[0] !== 'color') {
    return `  --color-${token.name}: var(--${token.name});${comment}`;
  }

  return null;
}

/**
 * Generates the standard file header comment for auto-generated files.
 * @param {string} fileName - Destination file name.
 * @returns {string} File header comment block.
 */
function getFileHeader(fileName) {
  return [
    '/**',
    ' * Do not edit directly, this file was auto-generated.',
    ` * ${fileName}`,
    ' */'
  ].join('\n');
}

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
      const propertyDeclaration = mapTokenToTailwindThemeProperty(token);
      if (propertyDeclaration) {
        lines.push(propertyDeclaration);
      }
    }

    lines.push('}', '');
    return lines.join('\n');
  }
});

/**
 * Creates a StyleDictionary instance configured for CSS custom properties.
 * @param {string} product - 'forge' or 'bloom'
 * @param {string} mode - 'light' or 'dark'
 * @param {string} selector - CSS rule selector (':root' or '.dark')
 * @param {string} tokensDirectory - Path to tokens directory
 * @returns {StyleDictionary} Configured StyleDictionary instance
 */
function createVariablesInstance(product, mode, selector, tokensDirectory) {
  const sharedGlob = path.posix.join(tokensDirectory.replace(/\\/g, '/'), 'shared/**/*.json');
  const semanticsGlob = path.posix.join(tokensDirectory.replace(/\\/g, '/'), `${product}-${mode}/**/*.json`);

  return new StyleDictionary({
    parsers: ['dtcg-primitives-shim', 'dtcg-semantics-shim', 'dtcg-typography-curated-filter'],
    source: [sharedGlob, semanticsGlob],
    platforms: {
      css: {
        transformGroup: 'css',
        files: [
          {
            format: 'css/variables',
            options: {
              selector: selector,
              outputReferences: true,
              showFileHeader: false
            }
          }
        ]
      }
    }
  });
}

/**
 * Creates a StyleDictionary instance configured for Tailwind v4 @theme inline generation.
 * @param {string} product - 'forge' or 'bloom'
 * @param {string} tokensDirectory - Path to tokens directory
 * @param {string} outputDirectory - Path to output directory
 * @returns {StyleDictionary} Configured StyleDictionary instance
 */
function createTailwindThemeInstance(product, tokensDirectory, outputDirectory) {
  const sharedGlob = path.posix.join(tokensDirectory.replace(/\\/g, '/'), 'shared/**/*.json');
  const semanticsGlob = path.posix.join(tokensDirectory.replace(/\\/g, '/'), `${product}-light/**/*.json`);

  return new StyleDictionary({
    parsers: ['dtcg-primitives-shim', 'dtcg-semantics-shim', 'dtcg-typography-curated-filter'],
    source: [sharedGlob, semanticsGlob],
    platforms: {
      tailwind: {
        transforms: ['name/kebab'],
        buildPath: `${outputDirectory.replace(/\\/g, '/')}/`,
        files: [
          {
            destination: `theme-${product}.css`,
            format: 'css/tailwind-theme'
          }
        ]
      }
    }
  });
}

/**
 * Builds the combined CSS variables and Tailwind theme stylesheets for a product.
 * @param {string} product - 'forge' or 'bloom'
 * @param {string} tokensDirectory - Path to tokens directory
 * @param {string} outputDirectory - Path to output directory
 * @returns {Promise<void>}
 */
/**
 * Formats the combined CSS stylesheet containing light and dark mode variable blocks.
 * @param {string} fileName - Destination file name.
 * @param {string} lightOutput - Light mode CSS declarations.
 * @param {string} darkOutput - Dark mode CSS declarations.
 * @returns {string} The full combined CSS content.
 */
function formatCombinedVariablesContent(fileName, lightOutput, darkOutput) {
  const header = getFileHeader(fileName);
  return `${header}\n\n${lightOutput.trim()}\n\n${darkOutput.trim()}\n`;
}

/**
 * Builds the combined CSS variables and Tailwind theme stylesheets for a product.
 * @param {string} product - 'forge' or 'bloom'
 * @param {string} tokensDirectory - Path to tokens directory
 * @param {string} outputDirectory - Path to output directory
 * @returns {Promise<void>}
 */
export async function buildProductTokens(product, tokensDirectory, outputDirectory) {
  console.log(`Building tokens for ${product}...`);

  const combinedFileName = `tokens-${product}.css`;
  const themeFileName = `theme-${product}.css`;

  const styleDictionaryLight = createVariablesInstance(product, 'light', ':root', tokensDirectory);
  const styleDictionaryDark = createVariablesInstance(product, 'dark', '.dark', tokensDirectory);
  const styleDictionaryTailwind = createTailwindThemeInstance(product, tokensDirectory, outputDirectory);

  const [lightResult] = await styleDictionaryLight.formatPlatform('css');
  const [darkResult] = await styleDictionaryDark.formatPlatform('css');
  await styleDictionaryTailwind.buildAllPlatforms();

  const combinedContent = formatCombinedVariablesContent(combinedFileName, lightResult.output, darkResult.output);
  fs.writeFileSync(path.join(outputDirectory, combinedFileName), combinedContent, 'utf8');

  console.log(`Generated: ${combinedFileName}, ${themeFileName}`);
}

/**
 * Executes token generation based on target and directory configuration.
 * @param {{ target: string, tokensDirectory: string, outputDirectory: string }} options - Build options.
 * @returns {Promise<void>}
 */
export async function buildTokens(options) {
  const { target, tokensDirectory, outputDirectory } = options;

  if (!fs.existsSync(outputDirectory)) {
    fs.mkdirSync(outputDirectory, { recursive: true });
  }

  if (target === 'forge' || target === 'all') {
    await buildProductTokens('forge', tokensDirectory, outputDirectory);
  }
  if (target === 'bloom' || target === 'all') {
    await buildProductTokens('bloom', tokensDirectory, outputDirectory);
  }
  console.log('Design token generation completed successfully.');
}
