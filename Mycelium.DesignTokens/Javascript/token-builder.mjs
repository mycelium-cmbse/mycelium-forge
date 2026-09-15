import fs from 'node:fs';
import path from 'node:path';
import StyleDictionary from 'style-dictionary';
import { registerCustomPlugins } from './style-dictionary-plugins.mjs';
import { formatCombinedVariablesContent } from './tailwind-theme-mapper.mjs';

// Register custom DTCG parsers and Tailwind @theme format.
registerCustomPlugins(StyleDictionary);

/**
 * Validates that a required token directory exists on disk.
 * @param {string} directoryPath - The path to check.
 */
function validateDirectoryExists(directoryPath) {
  if (!fs.existsSync(directoryPath)) {
    throw new Error(`Required tokens directory does not exist: ${directoryPath}`);
  }
}

/**
 * Creates a StyleDictionary instance configured for CSS custom properties.
 * @param {string} product - 'forge' or 'bloom'
 * @param {string} mode - 'light' or 'dark'
 * @param {string} selector - CSS rule selector (':root' or '.dark')
 * @param {string} tokensDirectory - Path to tokens directory
 * @returns {StyleDictionary} Configured StyleDictionary instance
 */
function createVariablesInstance(product, mode, selector, tokensDirectory) {
  const semanticsDir = path.join(tokensDirectory, `${product}-${mode}`);
  validateDirectoryExists(semanticsDir);

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
  const semanticsDir = path.join(tokensDirectory, `${product}-light`);
  validateDirectoryExists(semanticsDir);

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
            destination: 'theme-mycelium.css',
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
async function buildProductTokens(product, tokensDirectory, outputDirectory) {
  console.log(`Building tokens for ${product}...`);

  const combinedFileName = `tokens-${product}.css`;

  const styleDictionaryLight = createVariablesInstance(product, 'light', ':root', tokensDirectory);
  const styleDictionaryDark = createVariablesInstance(product, 'dark', '.dark', tokensDirectory);
  const styleDictionaryTailwind = createTailwindThemeInstance(product, tokensDirectory, outputDirectory);

  const [[lightResult], [darkResult]] = await Promise.all([
    styleDictionaryLight.formatPlatform('css'),
    styleDictionaryDark.formatPlatform('css'),
    styleDictionaryTailwind.buildAllPlatforms()
  ]);

  const combinedContent = formatCombinedVariablesContent(combinedFileName, lightResult.output, darkResult.output);
  fs.writeFileSync(path.join(outputDirectory, combinedFileName), combinedContent, 'utf8');

  console.log(`Generated: ${combinedFileName}, theme-mycelium.css`);
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

  if (target === 'all') {
    await Promise.all([
      buildProductTokens('forge', tokensDirectory, outputDirectory),
      buildProductTokens('bloom', tokensDirectory, outputDirectory)
    ]);
  } else if (target === 'forge' || target === 'bloom') {
    await buildProductTokens(target, tokensDirectory, outputDirectory);
  }

  console.log('Design token generation completed successfully.');
}
