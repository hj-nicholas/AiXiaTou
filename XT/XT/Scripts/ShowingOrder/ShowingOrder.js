/// <reference path="F:\XiaTou\Project\AiXiaTou.git\XT\XT\ShowingOrder/CommentList.aspx" />
$(function() {

    //进入首页，展示晒单页面
    $.ajax({
        type: "POST",
        url: "../ASHX/ShowOrder.ashx",
        data: { action: "ShowOrder" },
        async: false,
        success: function (datas) {
            if (datas != null) {
                var showOrder = eval(datas);
                var html = "<ul>";
                $(showOrder).each(function (index) {
                    html += "<li>";
                    html += "<div class='commentT'><img src='../Uploads/UserImages/img01.jpg' class='fl'/><p class='oh'><span class='fl'>" + showOrder[index].UserName + "</span><span class='fr'>2人次</span></p>";
                    html += " <p class='oh'><span class='fl red'>" + showOrder[index].ProductName + " " + showOrder[index].PeriodNum + "</span><span class='fr gray9'>" + jsonDateFormat(showOrder[index].CommentDate) + " </span></p></div>";

                    html += "<div class='commentM oh'>";
                    html += "<p class='block'>" + showOrder[index].CommentContent + "</p>";
                    html += "<a class='fl'><img src='../Uploads/CommentPhotos/20151209/img01.jpg'/></a>";
                    html += "</div>";

                    html += "<div class='commentB oh'>";
                    html += "<a class='fl'  href='CommentList.aspx?peroid=" + showOrder[index].PeriodID + "'><i class='liaotian_s block icon_s fl '></i><p class='fl'>评论 " + showOrder[index].CommentNum + "</p></a>";
                    html += " <a class='fl'><i class='dianzan_s block icon_s fl '></i><p class='fl'>羡慕 " +  showOrder[index].SupportNum + "</p></a>";
                    html += "</div>";
                });
                html += "</ul>";
                //$("#divComment").removeChild();
                $("#divComment").append(html);
            }
            
        },
        error: function (msg) {
            alert("暂无数据");
        },
        complete: function () {

        }

    });

    

});

function jsonDateFormat(jsonDate) {//json日期格式转换为正常格式
    try {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds + "." + milliseconds;
    } catch (ex) {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
        return "";
    }
}