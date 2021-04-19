var chat;

$(document).ready(function () {
    ScrollDown();

    //start connection
    chat = $.connection.chatHub;
    //upon receiving message append it and scroll down
    chat.client.message = function (user, msg, footer) {
        //create div that will be appent to dom
        var $div = $('<div><hr/></div>')
            .append($('<h4></h4>').text(user))
            .append(
                $('<p></p>').text(msg)
                    .append('<br>')
                    .append(
                        $('<small></small>').text(footer).addClass('text-muted')
                    )
        );

        var message_holder = $('#msg-list');

        //display on screen
        message_holder.append($div);

        //remove old elements if there are more than 50
        KeepOnlyNElements(message_holder, 50);

        // clear input
        $('#msg-input').val('').focus();

        ScrollDown();
    };
    $.connection.hub.start();
});

function KeepOnlyNElements(elem, n) {
    while (elem.children().length > n) { //iterate until children count is the number we want
        elem.find('div:first').remove(); //on each iteration remove the first child (oldest element)
    }
}


function ScrollDown() {
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
}