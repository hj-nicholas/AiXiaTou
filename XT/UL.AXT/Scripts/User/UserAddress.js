$(function() {

    $("#newAddr").click(function() {

        AlertDiv("#alert_address_new");
    });

    $("a[name='editAddr']").click(function () {
        var obj = $(this);
        var receiver = obj.find("b[name='receiver']").text();
        var phone = obj.find("i[name='phone']").text();
        var address = obj.find("p[name='address']").text();
        var post = obj.find("input[name='post']").val();

        $("#txtRecieverEdit").val(receiver);
        $("#txtPhoneEdit").val(phone);
        $("#txtAddrEdit").val(address);
        $("#txtPostCodeEdit").val(post);
        $("#txtAddrId").val(obj.attr("id"));

        AlertDiv('#alert_address_edit');
       
    });

       //验证提示
       $.formValidator.initConfig({ validatorGroup:"2", formID: "form2", onError: function () { alert("请查看错误信息"); } });
        $("#txtPhoneEdit").formValidator({validatorGroup:"2", onShow: "", onfocus: "", onCorrect: "", empty: false }).regexValidator({ regExp: "mobile", dataType: "enum", onError: "手机号码格式不正确" });
        $("#txtRecieverEdit").formValidator({ validatorGroup: "2", onShow: "", onfocus: "", onCorrect: "", empty: false });
        $("#txtAddrEdit").formValidator({ validatorGroup: "2", onShow: "", onfocus: "", onCorrect: "", empty: false });
        $("#txtPostCodeEdit").formValidator({ validatorGroup: "2", onShow: "", onfocus: "", onCorrect: "", empty: false });
        
        $.formValidator.initConfig({ validatorGroup:"3", formID: "form3", onError: function () { alert("请查看错误信息"); } });
        $("#txtPhoneEditN").formValidator({validatorGroup:"3", onShow: "", onfocus: "", onCorrect: "", empty: false }).regexValidator({ regExp: "mobile", dataType: "enum", onError: "手机号码格式不正确" });
        $("#txtRecieverEditN").formValidator({ validatorGroup: "3", onShow: "", onfocus: "", onCorrect: "", empty: false });
        $("#txtAddrEditN").formValidator({ validatorGroup: "3", onShow: "", onfocus: "", onCorrect: "", empty: false });
        $("#txtPostCodeEditN").formValidator({ validatorGroup: "3", onShow: "", onfocus: "", onCorrect: "", empty: false });

});

function EdidAddress(type) {
    var addrId = 0;
    var addrDesc = "";
    var reciever = "";
    var phone = "";
    var post = "";
    //校验填写信息
    if (type == 0) {
        addrId = $("#txtAddrIdN").val();
        addrDesc = $("#txtAddrEditN").val();
        reciever = $("#txtRecieverEditN").val();
        phone = $("#txtPhoneEditN").val();
        post = $("#txtPostCodeEditN").val();
        if (!$.formValidator.pageIsValid('3'))
            return false;
    } else {
        addrId = $("#txtAddrId").val();
        addrDesc = $("#txtAddrEdit").val();
        reciever = $("#txtRecieverEdit").val();
        phone = $("#txtPhoneEdit").val();
        post = $("#txtPostCodeEdit").val();
        if (!$.formValidator.pageIsValid('2'))
            return false;
    }
    

    $.ajax({
        url: "/User/EditAddr",
        type: "Post",
        data: { AddressId: addrId, AddressDesc: addrDesc, Receiver: reciever, Phone: phone, PostCode: post, type: type, UserID: getCookie("UserId") },
        success: function () {
            window.location.href = "/User/ReceiveAddrs?userId=" + getCookie("UserId");
        }

    });
}