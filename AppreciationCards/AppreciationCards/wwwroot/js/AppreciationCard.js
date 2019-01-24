/* Wait for the required font to load before rendering the appreciation card */
WebFontConfig = {
    google: {
        families: [
            'Amatic SC:400,700',
            'Gloria Hallelujah',
            'IBM+Plex+Sans:100,100i,400,400i,700,700i',
            'Major+Mono+Display',
            'Permanent+Marker',
            'Homemade Apple',
            'Indie Flower',
            'Allura',
            'Cabin Sketch',
        ]
    },
    active: () => {
        AppreciationCard.fontLoaded = true;
        window.dispatchEvent(new Event('show-card'));
    },
};
(() => {
    var wf = document.createElement("script");
    wf.src = 'https://ajax.googleapis.com/ajax/libs/webfont/1.5.10/webfont.js';
    wf.async = 'true';
    document.head.appendChild(wf);
})();

class AppreciationCard {

    constructor(canvas, aspectRatio) {
        this.canvas = canvas;
        this.aspectRatio = aspectRatio;
        this.defaultFont = 'IBM Plex Sans';
        this.fontSize = 32;
    }

    setCustomFont(value) {
        this.customFont = value;
    }

    setFontSize(value) {
        this.fontSize = value;
    }

    setDisplayCard(value) {
        this.display = value;
    }

    setMessage(value) {
        this.message = value;
    }

    setToText(value) {
        this.toText = value;
    }

    setFromText(value) {
        this.fromText = value;
    }

    setXeroValueHashtag(value) {
        this.hashtag = value;
    }

    print() {
        const dataUrl = this.canvas.toDataURL();
        const img = new Image();
        img.src = dataUrl;

        const printWindow = window.open('', '', 'width=1014,height=615');
        printWindow.document.body.append(img);

        img.onload = () => {
            printWindow.focus();
            printWindow.print();
        }
    }

    render() {
        if (!this.display) return;

        const ctx = this.canvas.getContext('2d');
        const width = this.canvas.clientWidth;
        const height = this.canvas.clientHeight;
        const messageCharacterLimit = 800;
        const toCharacterLimit = 20;
        const fromCharacterLimit = 20;

        /* Paint Background and Card Border*/
        ctx.rect(0, 0, width, height);
        ctx.fillStyle = "#fff";
        ctx.fill();
        ctx.lineWidth = 1;
        ctx.strokeStyle = "#bababa";
        ctx.stroke();

        /* Paint Main Rectangle */
        ctx.fillStyle = "#e8e8e8";
        const mainRectOffset = width * 0.05;
        const mainRectWidth = width - mainRectOffset * 2;
        const mainRectHeight = height * 0.6;
        ctx.fillRect(mainRectOffset, mainRectOffset, mainRectWidth, mainRectHeight);

        const fontSize = width / 75;
        const fontOffset = width / 75;
        const textOffset = width / 45;
        ctx.fillStyle = "#4c4c4c";
        ctx.font = `${fontSize}px ${this.defaultFont}`;
        ctx.fillText('STORY', mainRectOffset + fontOffset, mainRectOffset + fontSize + fontOffset);

        /* Paint Small Rectangles */
        ctx.fillStyle = "#e8e8e8";
        const smallRectWidth = mainRectWidth * 0.4;
        const smallRectHeight = mainRectHeight * 0.12;
        const smallRectTopOffset = mainRectOffset + mainRectHeight + smallRectHeight;
        const smallRectLeftOffset = mainRectOffset + (0.6 * mainRectWidth);
        const spacingBetweenRecetangles = smallRectHeight * 1.3;
        ctx.fillRect(smallRectLeftOffset, smallRectTopOffset, smallRectWidth, smallRectHeight);
        ctx.fillRect(smallRectLeftOffset, smallRectTopOffset + spacingBetweenRecetangles, smallRectWidth, smallRectHeight);
        ctx.fillStyle = "#4c4c4c";
        ctx.font = `${fontSize}px ${this.defaultFont}`;
        ctx.fillText('TO', smallRectLeftOffset + fontOffset, smallRectTopOffset + fontSize + fontOffset);
        ctx.fillStyle = "#4c4c4c";
        ctx.font = `${fontSize}px ${this.defaultFont}`;
        ctx.fillText('FROM', smallRectLeftOffset + fontOffset, smallRectTopOffset + spacingBetweenRecetangles + fontSize + fontOffset);

        /* Render Dynamic Text Content */
        if (this.toText) {
            const text = this.toText.substring(0, toCharacterLimit);
            ctx.fillText(text, smallRectLeftOffset + fontOffset + textOffset, smallRectTopOffset + fontSize + fontOffset);
        }

        if (this.fromText) {
            const text = this.fromText.substring(0, fromCharacterLimit);
            ctx.fillText(text, smallRectLeftOffset + fontOffset + (textOffset * 2), smallRectTopOffset + spacingBetweenRecetangles + fontSize + fontOffset);
        }

        if (this.message) {
            const messageFontSize = fontSize * 1.5;
            ctx.font = `${messageFontSize}px ${this.customFont || this.defaultFont}`;
            const text = this.message.substring(0, messageCharacterLimit);

            CanvasTextWrapper(canvas, text, {
                font: `${this.fontSize}px ${this.customFont || this.defaultFont}`,
                paddingX: mainRectOffset + fontOffset,
                paddingY: mainRectOffset + (fontOffset * 2.5),
                maxWidth: mainRectWidth * 0.97,
                maxHeight: mainRectOffset + mainRectHeight,
            });
        }

        if (this.hashtag) {
            ctx.font = `700 ${fontSize * 5}px ${this.defaultFont}`;
            ctx.fillStyle = '#e0e0e0';
            if (this.hashtag[0] === '#') {
                ctx.fillText(`${this.hashtag}`, mainRectOffset, smallRectTopOffset + fontSize + (fontOffset * 3));
            } else {
                ctx.fillText(`#${this.hashtag}`, mainRectOffset, smallRectTopOffset + fontSize + (fontOffset * 3));
            }
        }
    }

    resizeCanvas(canvasWrapper) {
        const styles = window.getComputedStyle(canvasWrapper);
        const padding = parseFloat(styles.paddingLeft) + parseFloat(styles.paddingRight);
        this.canvas.width = canvasWrapper.clientWidth - padding; // Need to set width here (instead of in css) to avoid stretching 
        this.canvas.height = this.canvas.width / this.aspectRatio;
    }
}