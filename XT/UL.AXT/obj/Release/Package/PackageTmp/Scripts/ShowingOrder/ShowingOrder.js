/// <reference path="F:\XiaTou\Project\AiXiaTou.git\XT\XT\ShowingOrder/CommentList.aspx" />
$(function () {

    $(".dianzan_s").click( function () {
        var userId = getCookie("UserId");
        if(userId != "");
        {
            $.ajax({
                type: "POST",
                url: "/ShowOrder/AddSupport",
                data: { userId: userId, periodId: $(this).prop("id") },
                success: function (result) {
                    if (result != null) {
                        $(".dianzan_s").next("p").text(result.ResultId);
                    }
                }
            });
            
        }
       
    });

    $("a[name='showReply']").click(function () {
        $("#divReply").show();
        $("#replyContent").focus();
        $("#txtCommentId").val($(this).prop("id"));
    });

    $("#btnReply").click(function () {
            var objCom = $(eval("'#" + $("#txtCommentId").val() + "'"));
        $.ajax({
            type: "POST",
            url: "/ShowOrder/AddReply",
            data: { userId: getCookie("UserId"), commRefId: objCom.attr("tagCommref"), periodId: objCom.attr("tagPeriod"), replyContent: $("#replyContent").val() },
            success: function (result) {
                if (result != null) {
                    //$(".dianzan_s").next("p").text(result.ResultId);
                    window.location.reload();
                    //href("/ShowOrder/CommentList?periodId=" + objCom.attr("tagPeriod") + "&commNum=0&suppNum=0");
                }
            }
        });
    });


});

