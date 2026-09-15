/**
 * Base spacing unit in pixels used by Tailwind CSS.
 */
export const TailwindSpacingBasePx = 4;

/**
 * Token categories mapped directly to Tailwind @theme properties with identical names.
 */
export const directMappedThemeCategories = ['shadow', 'radius', 'text'];

/**
 * Maps a spacing design token to its corresponding Tailwind @theme custom property declaration.
 * @param {object} token - Style Dictionary token.
 * @param {string} comment - Formatted description comment.
 * @returns {string[]} CSS variable declaration lines.
 */
export function mapSpacingToken(token, comment) {
  if (token.path[1].startsWith('overlap')) {
    // Style Dictionary may store transformed token output in .value, while raw DTCG source defines .$value.
    const raw = token.value ?? token.$value;
    const numValue = typeof raw === 'object' && raw !== null && 'value' in raw ? raw.value : parseFloat(raw);
    return [`  --spacing-${token.path[1]}: ${numValue}px;${comment}`];
  }

  const numValue = parseFloat(token.path[1]);
  if (!isNaN(numValue) && numValue > 0) {
    const step = numValue / TailwindSpacingBasePx;
    const tailwindKey = Number.isInteger(step) ? step.toString() : step.toString().replace('.', '\\.');
    return [`  --spacing-${tailwindKey}: var(--${token.name});${comment}`];
  }

  return [];
}

/**
 * Maps a typography design token to its corresponding Tailwind @theme custom property declarations.
 * @param {object} token - Style Dictionary token.
 * @param {string} comment - Formatted description comment.
 * @returns {string[]} CSS variable declaration lines.
 */
export function mapTypographyToken(token, comment) {
  // Style Dictionary may store transformed token output in .value, while raw DTCG source defines .$value.
  const value = token.value ?? token.$value;
  if (!value || typeof value !== 'object') {
    return [];
  }

  const { fontSize, lineHeight, fontWeight, fontFamily } = value;
  const scale = token.path.slice(2).join('-');
  const declarations = [];

  if (fontSize?.value) {
    declarations.push(`  --text-${scale}: ${fontSize.value}px;${comment}`);
  }
  if (lineHeight?.value) {
    declarations.push(`  --leading-${scale}: ${lineHeight.value}px;${comment}`);
  }
  if (fontWeight) {
    declarations.push(`  --font-weight-${scale}: ${fontWeight};${comment}`);
  }
  if (fontFamily) {
    const family = typeof fontFamily === 'string' && fontFamily.includes(' ') && !fontFamily.startsWith("'")
      ? `'${fontFamily}'`
      : fontFamily;
    declarations.push(`  --font-${scale}: ${family};${comment}`);
  }

  return declarations;
}

/**
 * Maps a design token to its corresponding Tailwind @theme custom property declarations.
 * @param {object} token - Style Dictionary token.
 * @returns {string[]} CSS variable declaration lines.
 */
export function mapTokenToTailwindThemeProperty(token) {
  const comment = token.$description ? ` /** ${token.$description} */` : '';

  if (token.$type === 'color' && token.path[0] !== 'color') {
    return [`  --color-${token.name}: var(--${token.name});${comment}`];
  }

  if (token.path[0] === 'spacing') {
    return mapSpacingToken(token, comment);
  }

  // Check top-level group (token.path[0]) for semantic namespaces (e.g., 'text')
  // as well as DTCG token types (token.$type) for primitive types (e.g., 'shadow', 'radius').
  if (directMappedThemeCategories.includes(token.path[0]) || directMappedThemeCategories.includes(token.$type)) {
    return [`  --${token.name}: var(--${token.name});${comment}`];
  }

  if (token.$type === 'typography') {
    return mapTypographyToken(token, comment);
  }

  return [];
}

/**
 * Generates the standard file header comment for auto-generated files.
 * @param {string} fileName - Destination file name.
 * @returns {string} File header comment block.
 */
export function getFileHeader(fileName) {
  return [
    '/**',
    ' * Do not edit directly, this file was auto-generated.',
    ` * ${fileName}`,
    ' */'
  ].join('\n');
}

/**
 * Formats the combined CSS stylesheet containing light and dark mode variable blocks.
 * @param {string} fileName - Destination file name.
 * @param {string} lightOutput - Light mode CSS declarations.
 * @param {string} darkOutput - Dark mode CSS declarations.
 * @returns {string} The full combined CSS content.
 */
export function formatCombinedVariablesContent(fileName, lightOutput, darkOutput) {
  const header = getFileHeader(fileName);
  return `${header}\n\n${lightOutput.trim()}\n\n${darkOutput.trim()}\n`;
}
