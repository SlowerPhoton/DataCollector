function help(elementName) {
    var display = elementName;
    if (elementName == '') {
        display = 'No help for you.';
    }
    if (elementName == 'final_inspection')
        display = 'Use the <b>DD.MM.YYYY</b> format. This field is required.';
    document.getElementById("help").innerHTML = display;
}