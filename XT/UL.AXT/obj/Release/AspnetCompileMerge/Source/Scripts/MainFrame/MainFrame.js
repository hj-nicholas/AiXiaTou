$(function () {

    //设置底部菜单高亮显示
    if (getCookie("footerBtn") != null) {
        $("#" + getCookie("footerBtn")).addClass("icon_H");
    }
    
});