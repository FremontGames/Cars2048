/**
 * Photoshop script to translate text layers based on lang 
 * JSON file, then export project to PNG images for each 
 * language.
 * 
 * Usage: generate screenshots files for your app (ex: 
 * Googleplay, etc).
 * 
 * @param langs array of languages, linked to translation 
 * files named like '<photoshopprojectname>-<lang>.json'
 * @param langFallback language to use when json file does
 * not exist.
 * @param (input) Current Photoshop project
 * @param (output) Current Photoshop project
 */

var langs = ["en", "ja", "ko", "zh-TW", "de", "fr", "pt", "es", "it", "ru", "hi"];
var langFallback = "en";
// get the path of currently opened photoshop document
var inputPath = app.activeDocument.path;
var outputPath = app.activeDocument.path;

/* UTILS *************************************************** */

// Because Photoshop doesn't
function JSON_parse(fileContent) {
    /// Extend script doesn't support JSON. Are you fucking kidding?
    // obj = JSON.parse(file_to_read .read());
    // Worry not. We got potentially harmful, yet useful, `eval` function.
    var jsonFile = eval("(" + fileContent + ")");
    return jsonFile;
}

function FILE_toString(fileToRead) {
    if (fileToRead === false) {
        alert("File read error:" + jsonFilePath);
    }
    fileToRead.open('r');
    var fileContent = fileToRead.read();
    fileToRead.close();
    return fileContent;
}

/* EXECUTION *************************************************** */

//Get the currently opened Photoshop document
var doc = app.activeDocument;

// get filename without extension
var filename = doc.name;
filename = filename.slice(0, filename.lastIndexOf(".")); //just add this line to the construction.      

// execute for each group
for (var y = 0; y < doc.layers.length; y++) {
    var currentGroup = doc.layers[y];

    // check layer is visible
    if (currentGroup.kind != undefined) {
        break;
    }

    // Pass sub layers to core script
    var groupLayers = currentGroup.layers;
    // hide others groups
    for (var t = 0; t < doc.layers.length; t++)
        doc.layers[t].visible = false;
    currentGroup.visible = true;

    // execute for each languages
    for (var t = 0; t < langs.length; t++) {
        var currentLang = langs[t];
        var fileContent;

        // Use absolute path for the JSON file.
        var langFilePath = inputPath + "/" + filename + "-" + currentLang + ".json";
        var fileToRead = File(langFilePath);
        fileContent = FILE_toString(fileToRead);

        // Use default language if file not found!
        if (fileContent === "") {
            var langFilePath2 = inputPath + "/" + filename + "-" + langFallback + ".json";
            var fileToRead2 = File(langFilePath2);
            fileContent = FILE_toString(fileToRead2);
        }

        var messages = JSON_parse(fileContent);

        // Translate each text layers and sub layers
        for (var i = 0; i < groupLayers.length; i++) {
            var currLayer = groupLayers[i];

            // check layer is visible
            if (currLayer.visible === false) {
                break;
            }

            // check layer is text
            if (currLayer.kind != LayerKind.TEXT) {
                break;
            }

            // get translation
            var translation = messages[currLayer.name];

            // check translation founded
            if (!translation) {
                break;
            }

            // set new text
            currLayer.textItem.contents = translation;
        }

        // save
        // TODO: create outputPath
        var file = new File(outputPath + "/" + currentLang + "-" + (y + 1) + ".jpg");
        var saveOptions = new JPEGSaveOptions();
        doc.saveAs(file, saveOptions, true, Extension.LOWERCASE);
    }
}