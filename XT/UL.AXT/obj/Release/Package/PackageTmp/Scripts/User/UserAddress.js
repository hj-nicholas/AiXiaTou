$(function() {

    $("#newAddr").click(function() {

        AlertDiv("#alert_address_edit");
    });
});

function EdidAddress(type) {
    $.ajax({
        url: "/User/EditAddr",
        type: "Post",
        data: { AddressId: $("#txtAddrId").val(), AddressDesc: $("#txtAddrEdit").val(), Receiver: $("#txtRecieverEdit").val(), Phone: $("#txtPhoneEdit").val(), PostCode: $("#txtPostCodeEdit").val(), type: type, UserID: getCookie("UserId") },
        success: function () {
            window.location.href = "/User/ReceiveAddrs?userId=" + getCookie("UserId");
        }

    });
}