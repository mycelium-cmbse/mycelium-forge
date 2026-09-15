import path from 'node:path';
import { fileURLToPath } from 'node:url';

/**
 * The current directory of this module, resolved from the file URL.
 */
const currentDirectory = path.dirname(fileURLToPath(import.meta.url));

/**
 * Parses command-line arguments formatted as --key=value into a key-value dictionary.
 * @param {string[]} args - Process argument array.
 * @returns {Record<string, string>} Parsed arguments dictionary.
 */
function parseRawArguments(args) {
  const result = {};
  for (const argument of args) {
    if (argument.startsWith('--')) {
      const [key, ...valueParts] = argument.slice(2).split('=');
      result[key] = valueParts.join('=') || 'true';
    }
  }
  return result;
}

/**
 * Parses and resolves command-line configuration options for design token generation.
 * @param {string[]} args - Process argument array.
 * @returns {{ target: string, tokensDirectory: string, outputDirectory: string }} Resolved configuration options.
 */
export function parseCommandLineArguments(args) {
  const rawArguments = parseRawArguments(args);
  const target = rawArguments.target || 'all';

  const defaultTokensDirectory = path.resolve(currentDirectory, '..', 'Styles', 'tokens');
  const tokensDirectory = rawArguments.tokensDir
    ? path.resolve(process.cwd(), rawArguments.tokensDir)
    : defaultTokensDirectory;

  const defaultOutputDirectory = path.resolve(currentDirectory, '..', 'Styles', 'dist');
  const outputDirectory = rawArguments.outDir
    ? path.resolve(process.cwd(), rawArguments.outDir)
    : defaultOutputDirectory;

  return {
    target,
    tokensDirectory,
    outputDirectory
  };
}
