function hideCanvasLoader() {
    const canvasLoader = document.getElementById('canvasLoader');
    canvasLoader.style.display = 'none';
}

function showCanvasLoader() {
    const canvasLoader = document.getElementById('canvasLoader');
    canvasLoader.style.display = 'block';
}

function initializeCanvas() {
    const canvas = document.getElementById('canvas');
    const canvasWrapper = document.getElementById("canvasWrapper");
    const toInput = document.getElementById('toInput');
    const fromInput = document.getElementById('fromInput');
    const messageInput = document.getElementById('messageInput');
    const valueSelection = document.getElementById('ValueId');
    const fontFamilySelector = document.getElementById('fontFamilySelector');
    const fontSizeInput = document.getElementById('fontSizeInput');
    const printButton = document.getElementById('printButton')
    const aspectRatio = 148 / 105;

    const card = new AppreciationCard(canvas, aspectRatio);

    /* Depending on whether the font has loaded, show/hide the loader and card */
    card.setDisplayCard(AppreciationCard.fontLoaded);
    if (AppreciationCard.fontLoaded) {
        hideCanvasLoader();
    } else {
        showCanvasLoader();
    }

    /* Initialize event listeners */
    window.addEventListener('resize', () => {
        card.resizeCanvas(canvasWrapper);
        card.render();
    });

    window.addEventListener('show-card', () => {
        hideCanvasLoader();
        card.setDisplayCard(true);
        card.render();
    });

    toInput.addEventListener('keyup', (e) => {
        card.setToText(toInput.value);
        card.render();
    });

    fromInput.addEventListener('keyup', (e) => {
        card.setFromText(fromInput.value);
        card.render();
    });

    messageInput.addEventListener('keyup', (e) => {
        card.setMessage(messageInput.value);
        card.render();
    });

    valueSelection.addEventListener('change', (e) => {
        card.setXeroValueHashtag(valueSelection[valueSelection.value - 1].text);
        card.render();
    });

    printButton.addEventListener('click', (e) => {
        e.preventDefault();
        card.print();
    });

    fontSizeInput.addEventListener('keyup', (e) => {
        card.setFontSize(fontSizeInput.value);
        card.render();
    });

    fontFamilySelector.addEventListener('change', (e) => {
        card.setCustomFont(fontFamilySelector.value);
        card.render();
    });

    /* Dispatch initial events */
    window.dispatchEvent(new Event('resize'));
    valueSelection.dispatchEvent(new Event('change'));
}

document.addEventListener("DOMContentLoaded", function () {
    initializeCanvas();
});
