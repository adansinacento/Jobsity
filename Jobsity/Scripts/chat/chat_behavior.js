$(document).ready(function () {
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
});

function SendMessageSuccess(data) {
    //clear the input
    $('#msg-input').val('');

    //append the msg to the list
    $('#msg-list').append(data);
    //console.log(data);
}