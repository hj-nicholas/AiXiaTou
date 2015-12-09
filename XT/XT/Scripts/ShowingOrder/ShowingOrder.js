$(function() {

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
                    html += "<div class='commentT'><p class='oh'><span class='fl'>" + showOrder[index].UserName + "</span><span class='fr'>2人次</span></p>";
                    html += " <p class='oh'><span class='fl red'>" +  showOrder[index].ProductName + " " +  showOrder[index].PeriodNum + "</span><span class='fr gray9'>2015-11-25</span></p></div>";

                    html += "<div class='commentM oh'>";
                    html += "<p class='block'>" +  showOrder[index].CommentContent + "</p>";

                    html += "<div class='commentB oh'>";
                    html += "<a class='fl' href='评论.html'><i class='liaotian_s block icon_s fl '></i><p class='fl'>评论 " +  showOrder[index].CommentNum + "</p></a>";
                    html += " <a class='fl'><i class='dianzan_s block icon_s fl '></i><p class='fl'>羡慕 " +  showOrder[index].SupportNum + "</p></a>";

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