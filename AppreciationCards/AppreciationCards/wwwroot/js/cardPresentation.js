const toArray = [];
const fromArray = [];
const contentArray = [];
const valuesArray = [];
const datesArray = [];
var cardIndex = 0;
var canvas;
var canvasWrapper;
var card;
var index = 0;

function hideCanvasLoader() {
    const canvasLoader = document.getElementById('canvasLoader');
    canvasLoader.style.display = 'none';
}

function showCanvasLoader() {
    const canvasLoader = document.getElementById('canvasLoader');
    canvasLoader.style.display = 'block';
}

document.addEventListener("DOMContentLoaded", function () {

    const aspectRatio = 1280 / 720;
    canvas = document.getElementById('canvas');
    canvasWrapper = document.getElementById("canvasWrapper");
    card = new AppreciationCard(canvas, aspectRatio);

    /* Initially hide the card and show a loader while we are waiting for the font to load */
    card.setDisplayCard(false);
    showCanvasLoader();

    window.addEventListener('show-card', () => {
        hideCanvasLoader();
        card.setDisplayCard(true);
        card.render();
    });

    fetch('GetWeeksMessagesAsList')
        .then((resp) => resp.json()) // Transform the data into json
        .then((data) => {

            for (const entry of data) {
                if (index > 0) {
                    checkLastEntriesSimilarity(entry.content, entry.toName, entry.fromName, entry.value.valueName, entry.messageDate);
                } else {
                    splitDataIntoArrays(entry.content, entry.toName, entry.fromName, entry.value.valueName, entry.messageDate);
                }
            }
            window.dispatchEvent(new Event('resize'));
            setCardCounter();
        })

    window.addEventListener('resize', () => {
        showCard(cardIndex);
        card.resizeCanvas(canvasWrapper);
        card.render();
    });

    window.dispatchEvent(new Event('resize'));
});

function nextCard() {
    if (!(cardIndex == index - 1)) {
        cardIndex++;
        document.getElementById("canvasWrapper").classList.add("fadeInRight");
        document.getElementById("canvasWrapper").classList.add("animated");
        setTimeout(function () {
            document.getElementById("canvasWrapper").classList.remove("fadeInRight");
            document.getElementById("canvasWrapper").classList.remove("animated");
        }, 1000);
        showCard(cardIndex);
    }
}

function previousCard() {
    if (!cardIndex == 0) {
        cardIndex--;
        document.getElementById("canvasWrapper").classList.add("fadeInLeft");
        document.getElementById("canvasWrapper").classList.add("animated");
        setTimeout(
            function () {
                document.getElementById("canvasWrapper").classList.remove("fadeInLeft");
                document.getElementById("canvasWrapper").classList.remove("animated");
            }, 1000);
        showCard(cardIndex);
    }
}

function showCard(n) {
    card.setFromText(fromArray[n]);
    card.setToText(toArray[n]);
    card.setMessage(contentArray[n]);
    card.setXeroValueHashtag(valuesArray[n]);
    card.render();
    if (toArray[n] != undefined) { 
    markCardAsRead(toArray[n], datesArray[n]);
    }
}

function markCardAsRead(name, date) {
    console.log(name);
    console.log(date);
    $.ajax({
        url: 'MarkAsRead',
        data: {
            toName: name,
            dateTime: date
        },
        type: "POST",
    });
}

function checkLastEntriesSimilarity(content, toName, fromName, value, date) {
    if (content == contentArray[index - 1] && fromName == fromArray[index - 1]) {
        toArray[index - 1] += ", " + toName;
    } else {
        splitDataIntoArrays(content, toName, fromName, value, date);
    }
}

function splitDataIntoArrays(content, toName, fromName, value, date) {
    contentArray[index] = content;
    toArray[index] = toName;
    fromArray[index] = fromName;
    valuesArray[index] = value;
    datesArray[index] = date.replace(/ |-|T|:/gi, "");
    index = index + 1;
}

function setCardCounter() {
    document.getElementById("cardCount").innerHTML = (cardIndex + 1) + "/" + index;
}
