var emojiPicker = {
    emojiData: {},
    initialize: function (emojiData) {
        this.emojiData = emojiData;
        return this;
    },
    parseUnicode: function (emojiUnocode) {
        return emojiUnocode
        .replace(/u[+]/gi, '&#x')
        .replace(/ /gi, ';')
        + ';';
    },
    renderEmojiGroup: function (emojiGroupName) {
        var $emojis = $('<div />', { class: 'emoji-group' });
        $emojiCategory = $('<div />', { class: 'emoji-category' });
        $emojiCategory.text(emojiGroupName);
        $emojis.append($emojiCategory);
        return $emojis;
    },
    renderEmoji: function (emoji) {
        var $emoji = $('<div />', { class: 'emoji' });
        var emojiCode = this.parseUnicode(emoji.code);
        $emoji.html(emojiCode);
        $emoji.attr('title', emoji.no + ' : ' + emoji.code);
        if (emoji.hasOwnProperty('types')) {
            var $emojiTypes = $('<div />', { class: 'emoji-types' });
            $emojiTypes.append($emoji.clone());
            for (var type in emoji.types) {
                var $emojiType = $('<div />', { class: 'emoji' });
                var emojiCode = this.parseUnicode(emoji.types[type]);
                $emojiType.html(emojiCode);
                $emojiTypes.append($emojiType);
            }
            $emoji.append($emojiTypes);
            $emoji.addClass('emoji-with-types');
        }
        return $emoji;
    },
    render: function () {
        var $emojiPicker = $('<div />', { class: 'emoji-picker' });
        for (var emojiGroup in this.emojiData) {
            var $emojiGroup = this.renderEmojiGroup(emojiGroup);
            for (var emoji in this.emojiData[emojiGroup]) {
                if (this.emojiData[emojiGroup][emoji].flagged || this.emojiData[emojiGroup][emoji].no === 18) continue;

                var $emoji = this.renderEmoji(this.emojiData[emojiGroup][emoji])
                $emojiGroup.append($emoji);
            }
            $emojiPicker.append($emojiGroup);
        }
        return $emojiPicker;
    }
};
$(function () {
    $.getJSON('https://api.myjson.com/bins/4sz7d', function (emojiData) {
        var $emojiPicker = emojiPicker.initialize(emojiData).render();
        $('#emojiselector').append($emojiPicker);
    });
    var $input = $('#messageInput');
    $('body').on('click', '.emoji-picker>.emoji-group>.emoji:not(.emoji-with-types), .emoji-picker>.emoji-group>.emoji.emoji-with-types>.emoji-types>.emoji', function () {
        $input.val($input.val() + $(this).text());
        $input.focus();
        // Dispatch a key event, to simulate the user typing an emoji
        // Need to use get(0) to get the DOM node instead of the jQuery Object
        $input.get(0).dispatchEvent(new Event('keyup'));
    }).on('click', '.emoji-picker>.emoji-group>.emoji.emoji-with-types', function () {
        $('.emoji-types.visible').not($(this).find('.emoji-types')).toggleClass('visible');
        $(this).find('.emoji-types').toggleClass('visible');
    });
});
function DisplayEmojiPicker() {
    var x = document.getElementById("emojiselector");
    if (x.style.display === 'none') {
        x.style.display = 'block';
    }
    else
        if (x.style.display === 'block') {
            x.style.display = 'none';
        }
    else{
            x.style.display = 'none';
        }
}