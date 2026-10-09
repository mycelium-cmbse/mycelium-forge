// ------------------------------------------------------------------------------------------------
// <copyright file="convertLcovToSonar.mjs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

import {
    existsSync,
    readFileSync,
    realpathSync,
    statSync,
    writeFileSync
} from 'node:fs';
import {
    isAbsolute,
    posix,
    relative,
    resolve,
    sep,
    win32
} from 'node:path';
import { pathToFileURL } from 'node:url';

const maximumSafeInteger = BigInt(Number.MAX_SAFE_INTEGER);

/**
 * Converts LCOV format coverage data into Sonar generic coverage XML.
 * @param {string} lcov - The raw LCOV coverage report string.
 * @param {object} [options={}] - Options for conversion including repositoryRoot.
 * @returns {string} The Sonar generic coverage XML string.
 */
export function convertLcovToSonarXml(lcov, options = {}) {
    const records = parseLcov(lcov, options);
    const output = [
        '<?xml version="1.0" encoding="UTF-8"?>',
        '<coverage version="1">'
    ];

    for (const record of records.toSorted((left, right) => compareText(left.path, right.path))) {
        output.push(`  <file path="${escapeXmlAttribute(record.path)}">`);

        for (const [lineNumber, covered] of [...record.lines.entries()].toSorted(compareLineEntries)) {
            const branches = record.branches.get(lineNumber);
            let branchAttributes = '';

            if (branches !== undefined) {
                const coveredBranches = [...branches.values()].filter(Boolean).length;
                branchAttributes = ` branchesToCover="${branches.size}" coveredBranches="${coveredBranches}"`;
            }

            output.push(
                `    <lineToCover lineNumber="${lineNumber}" covered="${covered}"${branchAttributes} />`
            );
        }

        output.push('  </file>');
    }

    output.push('</coverage>');
    return `${output.join('\n')}\n`;
}

/**
 * Parses LCOV data into source coverage records.
 * @param {string} lcov - The raw LCOV string.
 * @param {object} [options={}] - Parser options.
 * @returns {Array<object>} The parsed records.
 */
export function parseLcov(lcov, options = {}) {
    if (typeof lcov !== 'string') {
        throw new TypeError('LCOV input must be a string.');
    }

    const parser = createLcovParser(options.repositoryRoot ?? process.cwd());
    const inputLines = lcov.replace(/^\uFEFF/, '').split(/\r?\n/);

    for (const [index, line] of inputLines.entries()) {
        parseLcovLine(parser, line, index + 1);
    }

    return completeLcovParsing(parser);
}

/**
 * Escapes characters for XML attributes.
 * @param {string} value - The input string to escape.
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
 * Reads an LCOV file, converts it, and writes out Sonar generic coverage XML.
 * @param {string} inputPath - Path to input LCOV file.
 * @param {string} outputPath - Path to output XML file.
 * @param {object} [options={}] - Conversion options.
 * @returns {string} The generated XML content.
 */
export function convertLcovFile(inputPath, outputPath, options = {}) {
    const lcov = readFileSync(inputPath, 'utf8');
    const xml = convertLcovToSonarXml(lcov, options);
    writeFileSync(outputPath, xml, 'utf8');
    return xml;
}

function createLcovParser(repositoryRootPath) {
    const repositoryRoot = resolve(repositoryRootPath);

    if (!existsSync(repositoryRoot) || !statSync(repositoryRoot).isDirectory()) {
        throw new Error(`Repository root does not exist: ${repositoryRoot}`);
    }

    return {
        currentRecord: undefined,
        realRepositoryRoot: realpathSync(repositoryRoot),
        records: [],
        repositoryRoot,
        seenSources: new Set()
    };
}

function parseLcovLine(parser, line, reportLineNumber) {
    if (line.startsWith('SF:')) {
        startSourceRecord(parser, line.slice('SF:'.length), reportLineNumber);
        return;
    }

    if (line.startsWith('DA:')) {
        addLineCoverage(parser, line, reportLineNumber);
        return;
    }

    if (line.startsWith('BRDA:')) {
        addBranchCoverage(parser, line, reportLineNumber);
        return;
    }

    if (line === 'end_of_record') {
        finishSourceRecord(parser, reportLineNumber);
    }
}

function startSourceRecord(parser, sourcePath, reportLineNumber) {
    if (parser.currentRecord !== undefined) {
        throw new Error(`LCOV line ${reportLineNumber}: source record is missing end_of_record.`);
    }

    const source = resolveSource(
        sourcePath,
        parser.repositoryRoot,
        parser.realRepositoryRoot,
        reportLineNumber
    );
    const duplicateKey = getDuplicateSourceKey(source.realPath);

    if (parser.seenSources.has(duplicateKey)) {
        throw new Error(`LCOV line ${reportLineNumber}: duplicate source record for ${source.path}.`);
    }

    parser.seenSources.add(duplicateKey);
    parser.currentRecord = {
        branches: new Map(),
        lines: new Map(),
        path: source.path
    };
}

function addLineCoverage(parser, line, reportLineNumber) {
    requireCurrentRecord(parser.currentRecord, reportLineNumber, 'DA');
    parseLineCoverage(parser.currentRecord, line, reportLineNumber);
}

function addBranchCoverage(parser, line, reportLineNumber) {
    requireCurrentRecord(parser.currentRecord, reportLineNumber, 'BRDA');
    parseBranchCoverage(parser.currentRecord, line, reportLineNumber);
}

function finishSourceRecord(parser, reportLineNumber) {
    requireCurrentRecord(parser.currentRecord, reportLineNumber, 'end_of_record');
    finalizeRecord(parser.currentRecord, reportLineNumber);
    parser.records.push(parser.currentRecord);
    parser.currentRecord = undefined;
}

function completeLcovParsing(parser) {
    if (parser.currentRecord !== undefined) {
        throw new Error('LCOV input ended before end_of_record.');
    }

    if (parser.records.length === 0) {
        throw new Error('LCOV input contains no source records.');
    }

    return parser.records;
}

function getDuplicateSourceKey(realPath) {
    return process.platform === 'win32' ? realPath.toLowerCase() : realPath;
}

function resolveSource(sourcePath, repositoryRoot, realRepositoryRoot, reportLineNumber) {
    if (sourcePath.length === 0) {
        throw new Error(`LCOV line ${reportLineNumber}: SF path is empty.`);
    }

    if (
        isAbsolute(sourcePath)
        || posix.isAbsolute(sourcePath)
        || win32.isAbsolute(sourcePath)
        || sourcePath.startsWith('file:')
    ) {
        throw new Error(`LCOV line ${reportLineNumber}: absolute SF path is not allowed: ${sourcePath}`);
    }

    const normalizedPath = posix.normalize(sourcePath.replaceAll('\\', '/'));
    const resolvedPath = resolve(repositoryRoot, ...normalizedPath.split('/'));

    if (isOutside(repositoryRoot, resolvedPath)) {
        throw new Error(`LCOV line ${reportLineNumber}: SF path is outside the repository: ${sourcePath}`);
    }

    if (!existsSync(resolvedPath) || !statSync(resolvedPath).isFile()) {
        throw new Error(`LCOV line ${reportLineNumber}: SF source file does not exist: ${normalizedPath}`);
    }

    const realPath = realpathSync(resolvedPath);

    if (isOutside(realRepositoryRoot, realPath)) {
        throw new Error(`LCOV line ${reportLineNumber}: SF path resolves outside the repository: ${sourcePath}`);
    }

    return { path: normalizedPath, realPath };
}

function parseLineCoverage(record, line, reportLineNumber) {
    const fields = line.slice('DA:'.length).split(',');

    if (fields.length < 2 || fields.length > 3) {
        throw new Error(`LCOV line ${reportLineNumber}: malformed DA record.`);
    }

    const lineNumber = parseLineNumber(fields[0], 'DA line number', reportLineNumber);
    const executionCount = parseUnsignedInteger(fields[1], 'DA execution count', reportLineNumber);
    const covered = executionCount > 0n;
    record.lines.set(lineNumber, (record.lines.get(lineNumber) ?? false) || covered);
}

function parseBranchCoverage(record, line, reportLineNumber) {
    const fields = line.slice('BRDA:'.length).split(',');

    if (fields.length !== 4) {
        throw new Error(`LCOV line ${reportLineNumber}: malformed BRDA record.`);
    }

    const lineNumber = parseLineNumber(fields[0], 'BRDA line number', reportLineNumber);
    const blockNumber = parseUnsignedInteger(fields[1], 'BRDA block number', reportLineNumber);
    const branchNumber = parseUnsignedInteger(fields[2], 'BRDA branch number', reportLineNumber);
    const covered = fields[3] === '-'
        ? false
        : parseUnsignedInteger(fields[3], 'BRDA taken count', reportLineNumber) > 0n;
    const branchKey = `${blockNumber}:${branchNumber}`;
    const branches = record.branches.get(lineNumber) ?? new Map();

    branches.set(branchKey, (branches.get(branchKey) ?? false) || covered);
    record.branches.set(lineNumber, branches);
}

function finalizeRecord(record, reportLineNumber) {
    if (record.lines.size === 0) {
        throw new Error(`LCOV line ${reportLineNumber}: source record ${record.path} has no DA entries.`);
    }

    for (const branchLine of record.branches.keys()) {
        if (!record.lines.has(branchLine)) {
            throw new Error(
                `LCOV line ${reportLineNumber}: BRDA line ${branchLine} has no matching DA entry in ${record.path}.`
            );
        }
    }
}

function requireCurrentRecord(record, reportLineNumber, recordType) {
    if (record === undefined) {
        throw new Error(`LCOV line ${reportLineNumber}: ${recordType} appears outside an SF record.`);
    }
}

function parseLineNumber(value, fieldName, reportLineNumber) {
    const parsed = parseUnsignedInteger(value, fieldName, reportLineNumber);

    if (parsed === 0n || parsed > maximumSafeInteger) {
        throw new Error(`LCOV line ${reportLineNumber}: malformed ${fieldName}: ${value}`);
    }

    return Number(parsed);
}

function parseUnsignedInteger(value, fieldName, reportLineNumber) {
    if (!/^\d+$/.test(value)) {
        throw new Error(`LCOV line ${reportLineNumber}: malformed ${fieldName}: ${value}`);
    }

    return BigInt(value);
}

function isOutside(rootPath, candidatePath) {
    const relativePath = relative(rootPath, candidatePath);
    return relativePath === '..'
        || relativePath.startsWith(`..${sep}`)
        || isAbsolute(relativePath);
}

function compareText(left, right) {
    if (left < right) {
        return -1;
    }

    if (left > right) {
        return 1;
    }

    return 0;
}

function compareLineEntries([left], [right]) {
    return left - right;
}

const isMainModule = process.argv[1] !== undefined
    && pathToFileURL(resolve(process.argv[1])).href === import.meta.url;

if (isMainModule) {
    const [inputPath, outputPath, ...unexpectedArguments] = process.argv.slice(2);

    if (inputPath === undefined || outputPath === undefined || unexpectedArguments.length > 0) {
        console.error('Usage: node convertLcovToSonar.mjs <input-lcov> <output-xml>');
        process.exitCode = 1;
    } else {
        try {
            convertLcovFile(inputPath, outputPath, { repositoryRoot: process.cwd() });
            console.log(`Generated Sonar generic coverage report: ${outputPath}`);
        } catch (error) {
            console.error(error instanceof Error ? error.message : error);
            process.exitCode = 1;
        }
    }
}
