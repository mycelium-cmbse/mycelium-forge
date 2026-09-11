import { parseCommandLineArguments } from './argument-parser.mjs';
import { buildTokens } from './token-builder.mjs';

/**
 * Main application entry point for design token compilation.
 * @returns {Promise<void>}
 */
async function main() {
  try {
    const options = parseCommandLineArguments(process.argv.slice(2));
    await buildTokens(options);
  } catch (error) {
    console.error('Error generating design tokens:', error);
    process.exit(1);
  }
}

await main();
