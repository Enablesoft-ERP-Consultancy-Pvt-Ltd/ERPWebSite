var oldgridSelectedColor;

function setMouseOverColor(element) {
    oldgridSelectedColor = element.style.backgroundColor;
    element.style.backgroundColor = 'yellow';
    element.style.cursor = 'hand';
    element.style.textDecoration = 'underline';
}

function setMouseOutColor(element) {
    element.style.backgroundColor = oldgridSelectedColor;
    element.style.textDecoration = 'none';
}