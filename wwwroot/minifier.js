const fs = require('fs');
const path = require('path');
const UglifyJS = require('uglify-js');

function minifyFile(inputFile, outputFile) {
    const code = fs.readFileSync(inputFile, 'utf8');
    const result = UglifyJS.minify(code, { compress: false, mangle: true });

    if (result.error) {
        console.error(`Error minifying ${inputFile}: ${result.error.message}`);
        return;
    }

    fs.writeFileSync(outputFile, result.code);
    console.log(`Minified ${inputFile} to ${outputFile}`);
}

function processFiles(files) {
    files.forEach((file) => {
        const inputFile = file.inputfile;
        const outputFile = file.outputfile;

        if (!inputFile || !outputFile) {
            console.error('Invalid input JSON format. Each object should have "inputfile" and "outputfile" properties.');
            return;
        }
        minifyFile(inputFile, outputFile);
    });
}

function main() {
    const inputJsonFile = 'wwwroot/minifymapping.json';
    const jsonData = fs.readFileSync(inputJsonFile, 'utf8');

    try {
        const files = JSON.parse(jsonData);
        processFiles(files);
    } catch (error) {
        console.error(`Error parsing JSON file: ${error.message}`);
    }
}

main();
