//made by samuel

//根据窗口大小获取对应css以及顶部标题显示
var sw, sh;

$(function () {
    window_meet();
});
$(window).resize(function () {
    window_meet();
});

function window_meet() {
    var fh, bg_w, bg_h, f_top, f_left;
    sw = $(window).width();
    sh = $(window).height();

    var sc = (sw > 960 && 960) || (sw > 720 && 720) || (sw > 520 && 520) || 320;
    document.getElementById("mediaWidth").setAttribute("href", "../../Content/css/page" + sc + ".css");

}

//屏幕加载样式结束
//点击通用效果
$(document).ready(function () {
    $(".Animate_pulse").click(function () {
        /*		  var str=$(this);
            $(this).addClass("swing animated");
            setTimeout("$('str').removeClass('swing animated')",1200);*/
        $(this).addClass("pulse animated");
        var $this = $(this);
        t2 = setTimeout(function () { $this.removeClass('pulse animated'); }, 1200);


    });
});
$(document).ready(function () {
    $(".Animate_shake").click(function () {
        $(this).addClass("shake animated");
        var $this = $(this);
        t2 = setTimeout(function () { $this.removeClass('shake animated'); }, 1200);
    });
});
$(document).ready(function () {
    $(".Animate_bounce").click(function () {
        /*		  var str=$(this);
            $(this).addClass("swing animated");
            setTimeout("$('str').removeClass('swing animated')",1200);*/
        $(this).addClass("bounce animated");
        var $this = $(this);
        t2 = setTimeout(function () { $this.removeClass('bounce animated'); }, 1200);


    });
});

//点击通用效结束
//输入框提示语
$(function () {

    $(".Ntext").each(function (index, element) {

        $(this).val($(this).next().val());
        $(this).css("color", "#999999");
    });

    $(".Ntext").focus(function () {

        if ($(this).val() == $(this).next().val()) {
            $(this).val('');
            $(this).css("color", "#333333");
        }
    }).blur(function () {
        if ($(this).val() == "") {
            $(this).css("color", "#999999");
            $(this).val($(this).next().val());
        } else if ($(this).val != $(this).next().val()) {

            $(this).css("color", "#333333");
        }
        /* else if($(this).val==$(this).next().val())
         {
             $(this).css("color","#999999");
             }*/
    });

}
  );

//输入框提示语结束

$(function () {
    $("#index_more").toggle(function () {
        x = $(this).attr("name");
        $(this).html("收起 &nbsp;&nbsp;&and;");
        $(".none").show();
    }, function () {
        $(this).html(x);
        $(".none").hide();
    });
})
//还款table

$(document).ready(function () {
    $("table tr.repay_B1").click(function () {
        $("table tr.repay_B1").next("tr").removeClass("tableblock").addClass("none");
        c = $(this).next("tr").attr("class")
        if (c == "tableblock") {
            $(this).next("tr").removeClass("tableblock").addClass("none");
        }
        else {
            $(this).next("tr").removeClass("none").addClass("tableblock")
        }


    });
});


//获取验证码倒计时
$(document).ready(function () {

    $(".login_T a").click(function () {
        var i = 60;
        alert("aaa")
        setTimeout('shownum()', 0);

    })
})


function shownum() {
    i = i - 1
    $(".login_T a").html(i)
    setTimeout('shownum()', 1000);
}


//切换页面
function selectTag(showContent, selfObj, a, b) {
    // 操作标签
    var tag = document.getElementById(a).getElementsByTagName("li");
    var taglength = tag.length;
    for (i = 0; i < taglength; i++) {
        tag[i].className = "";
    }
    selfObj.parentNode.className = "hover";
    // 操作内容
    for (i = 0; j = document.getElementById(b + i) ; i++) {
        j.style.display = "none";
    }
    document.getElementById(showContent).style.display = "block";
}

//弹出
function AlertDiv(seletor) {
    $(seletor).css("display", "block");
    var y1 = $(window).height();
    var x1 = $(window).width();
    var y2 = $(seletor).find(".alertC").height();
    var x2 = $(seletor).find(".alertC").width();
    var y = (y1 - y2) / 2;
    var x = (x1 - x2) / 2;
    $(seletor).find(".alertC").css("top", y)
    $(seletor).find(".alertC").css("left", x)
}
function AlertClose(seletor) {
    $(seletor).closest(".alert").css("display", "none")
}

function AlertDivD(seletor) {
    $(seletor).css("display", "block");
    var y1 = $(window).height();
    var x1 = $(window).width();
    var y2 = $(seletor).find(".DalertC").height();
    var x2 = $(seletor).find(".DalertC").width();
    var y = (y1 - y2) / 2;
    var x = (x1 - x2) / 2;
    $(seletor).find(".DalertC").css("top", y)
    $(seletor).find(".DalertC").css("left", x)
}
function AlertCloseD(seletor) {
    $(seletor).closest(".Dalert").css("display", "none");
}


//add by hoo 2015-12-13
$(function () {

    //判断是否微信登录
    //if (!is_weixn()) {
    //    window.location.href = "http://www.uuulink.com/ixiatou/";
    //}

    //切换底部菜单高亮显示
    $("i[type='footerBtn']").click(function () {
        var a_H = $(this).parent();
        if (!a_H.hasClass("icon_H")) {
            a_H.siblings().removeClass("icon_H");
            a_H.addClass("icon_H");
            SetCookie("footerBtn", a_H.prop("id"));

        }
    });

    $("#smile").click(function() {
        var userId = getCookie("UserId");
        window.location.href="/User/Index?userId="+userId;
    });
});

//判断是否微信登录
function is_weixn() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}

/*
功能：保存cookies函数 
参数：name，cookie名字；value，值
*/
function SetCookie(name, value) {
    var cookieDays = 30 * 12;   //cookie 将被保存一年
    var exp = new Date();  //获得当前时间
    exp.setTime(exp.getTime() + cookieDays * 24 * 60 * 60 * 1000);  //换成毫秒
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
}
/*
功能：获取cookies函数 
参数：name，cookie名字
*/
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) {
        return unescape(arr[2]);
    } else {
        return null;
    }
}
/*
功能：删除cookies函数 
参数：name，cookie名字
*/

function delCookie(name) {
    var exp = new Date();  //当前时间
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

//保存cookie信息
function saveToCookie(cookieName, val) {
    if (val != "") {
        SetCookie(cookieName, val);
    }
}

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
