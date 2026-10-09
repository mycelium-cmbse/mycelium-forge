// ------------------------------------------------------------------------------------------------
// <copyright file="lcovToSonarConverter.mjs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

import { readFileSync, writeFileSync } from 'node:fs';
import { isAbsolute, normalize, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

/**
 * Escapes special XML characters in attribute values.
 * @param {string} value The string to escape.
 * @returns {string} The escaped string.
 */
export function escapeXmlAttribute(value) {
    return value
        .replaceAll('&', '&amp;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&apos;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;');
}

/**
 * Normalizes file paths to POSIX format relative to repository root.
 * @param {string} filePath The path to normalize.
 * @returns {string} The normalized path.
 */
function normalizePath(filePath) {
    return normalize(filePath).replaceAll('\\', '/').replace(/^\.\//, '');
}

/**
 * Converts LCOV coverage string to SonarQube generic coverage XML.
 * @param {string} lcov The raw LCOV string.
 * @returns {string} The Sonar generic coverage XML.
 */
export function convertLcovToSonarXml(lcov) {
    if (typeof lcov !== 'string') {
        throw new TypeError('LCOV input must be a string.');
    }

    const records = new Map();
    let currentRecord = null;

    const lines = lcov.replace(/^\uFEFF/, '').split(/\r?\n/);
    for (let i = 0; i < lines.length; i++) {
        const line = lines[i].trim();
        if (!line) {
            continue;
        }

        if (line.startsWith('SF:')) {
            const rawPath = line.slice(3).trim();
            if (isAbsolute(rawPath)) {
                throw new Error(`LCOV line ${i + 1}: absolute SF path is not allowed: ${rawPath}`);
            }

            const normalized = normalizePath(rawPath);
            currentRecord = records.get(normalized) ?? { lines: new Map(), branches: new Map() };
            records.set(normalized, currentRecord);
        } else if (line.startsWith('DA:')) {
            if (!currentRecord) {
                throw new Error(`LCOV line ${i + 1}: DA appears outside an SF record.`);
            }

            const [lineNumStr, countStr] = line.slice(3).split(',');
            const lineNumber = parseInt(lineNumStr, 10);
            const count = parseInt(countStr, 10);

            if (!Number.isInteger(lineNumber) || lineNumber <= 0 || !Number.isInteger(count) || count < 0) {
                throw new Error(`LCOV line ${i + 1}: malformed DA record: ${line}`);
            }

            currentRecord.lines.set(lineNumber, (currentRecord.lines.get(lineNumber) ?? false) || count > 0);
        } else if (line.startsWith('BRDA:')) {
            if (!currentRecord) {
                throw new Error(`LCOV line ${i + 1}: BRDA appears outside an SF record.`);
            }

            const [lineNumStr, blockStr, branchStr, takenStr] = line.slice(5).split(',');
            const lineNumber = parseInt(lineNumStr, 10);
            const block = parseInt(blockStr, 10);
            const branch = parseInt(branchStr, 10);
            const covered = takenStr !== '-' && parseInt(takenStr, 10) > 0;

            if (!Number.isInteger(lineNumber) || !Number.isInteger(block) || !Number.isInteger(branch)) {
                throw new Error(`LCOV line ${i + 1}: malformed BRDA record: ${line}`);
            }

            const branchMap = currentRecord.branches.get(lineNumber) ?? new Map();
            branchMap.set(`${block}:${branch}`, (branchMap.get(`${block}:${branch}`) ?? false) || covered);
            currentRecord.branches.set(lineNumber, branchMap);
        } else if (line === 'end_of_record') {
            currentRecord = null;
        }
    }

    const output = [
        '<?xml version="1.0" encoding="UTF-8"?>',
        '<coverage version="1">'
    ];

    const sortedPaths = [...records.keys()].sort((a, b) => a.localeCompare(b));
    for (const filePath of sortedPaths) {
        const record = records.get(filePath);
        output.push(`  <file path="${escapeXmlAttribute(filePath)}">`);

        const sortedLines = [...record.lines.keys()].sort((a, b) => a - b);
        for (const lineNum of sortedLines) {
            const covered = record.lines.get(lineNum);
            const branches = record.branches.get(lineNum);
            let branchAttr = '';

            if (branches && branches.size > 0) {
                const coveredCount = [...branches.values()].filter(Boolean).length;
                branchAttr = ` branchesToCover="${branches.size}" coveredBranches="${coveredCount}"`;
            }

            output.push(`    <lineToCover lineNumber="${lineNum}" covered="${covered}"${branchAttr} />`);
        }

        output.push('  </file>');
    }

    output.push('</coverage>');
    return `${output.join('\n')}\n`;
}

/**
 * Reads an LCOV file, converts it, and writes out Sonar generic coverage XML.
 * @param {string} inputPath Path to the input LCOV file.
 * @param {string} outputPath Path to the output XML file.
 * @returns {string} The generated XML content.
 */
export function convertLcovFile(inputPath, outputPath) {
    const lcov = readFileSync(inputPath, 'utf8');
    const xml = convertLcovToSonarXml(lcov);
    writeFileSync(outputPath, xml, 'utf8');
    return xml;
}

const isMainModule = process.argv[1] !== undefined
    && pathToFileURL(resolve(process.argv[1])).href === import.meta.url;

if (isMainModule) {
    const [inputPath, outputPath, ...unexpected] = process.argv.slice(2);

    if (!inputPath || !outputPath || unexpected.length > 0) {
        console.error('Usage: node lcovToSonarConverter.mjs <input-lcov> <output-xml>');
        process.exitCode = 1;
    } else {
        try {
            convertLcovFile(inputPath, outputPath);
            console.log(`Generated Sonar generic coverage report: ${outputPath}`);
        } catch (error) {
            console.error(error instanceof Error ? error.message : error);
            process.exitCode = 1;
        }
    }
}
