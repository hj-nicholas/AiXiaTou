/// <reference path="F:\XiaTou\Project\AiXiaTou.git\XT\XT\ShowingOrder/CommentList.aspx" />
$(function () {


    CheckMenu();

    $(".dianzan_s").click( function () {
        var userId = getCookie("UserId");
        var obj = $(this);
        if(userId != "");
        {
            $.ajax({
                type: "POST",
                url: "/ShowOrder/AddSupport",
                data: { userId: userId, periodId: $(this).prop("id") },
                success: function (result) {
                    if (result != null) {
                        obj.next("p").next("p").text(result.ResultId);
                        obj.parent("a").addClass("icon_H");
                    }
                }
            });
            
        }
       
    });

    $("a[name='showReply']").click(function () {
        //$("#divReply").show();
        AlertDiv('#alert_pinglun');
        //$("#replyContent").focus();
        $("#txtCommentId").val($(this).prop("id"));
    });

    $("#commShowUser").click(function() {
        AlertDiv('#alert_pinglun');
         $("#txtCommentId").val(0);
    });

    $("#btnReply").click(function () {
        var userId = getCookie("UserId");
        var commRefId = 0;
        var periodId = 0;
        if ($("#txtCommentId").val() == 0) {
            periodId = $("#commShowUser").attr("tagPeriod");
            commRefId = 0;
        } else {
            var objCom = $(eval("'#" + $("#txtCommentId").val() + "'"));
            periodId = objCom.attr("tagPeriod");
            commRefId = objCom.attr("tagCommref");
        }
        $.ajax({
            type: "POST",
            url: "/ShowOrder/AddReply",
            data: { userId: getCookie("UserId"), commRefId: commRefId, periodId: periodId, replyContent: $("#replyContent").val() },
            success: function (result) {
                if (result != null) {
                    //$(".dianzan_s").next("p").text(result.ResultId);
                    AlertClose(this);
                    window.location.reload();
                    //href("/ShowOrder/CommentList?periodId=" + objCom.attr("tagPeriod") + "&commNum=0&suppNum=0");
                    
                }
            }
        });
    });


});

function CheckMenu() {
    var a_H = $("#shaidan");
    if (!a_H.hasClass("icon_H")) {
        a_H.siblings().removeClass("icon_H");
        a_H.addClass("icon_H");
        SetCookie("footerBtn", a_H.prop("id"));

    }
}