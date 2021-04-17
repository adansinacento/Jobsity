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

        //display on screen
        $('#msg-list').append($div);

        // clear input
        $('#msg-input').val('').focus();

        ScrollDown();
    };
    $.connection.hub.start();
});

function ScrollDown() {
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
}