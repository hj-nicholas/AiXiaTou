$(function () {

    //弹出地址选择
    $("#alertRevAddrs").click(function() {

        $("#addrList").remove();
        $.ajax({
            url: "/User/GetAddrs",
            type: "POST",
            data: { userId: getCookie("UserId") },
            success: function (data) {
                
                var html = '';
                if (data != null) {
                    html += ' <div class="addressB" id="addrList"> <ul>';
                    $(data).each(function (index) {
                        //alert(data[index].AddressDesc);
                                html += '<li class="white_bg">';
                                html += '   <input type="radio" class="fl" name="rdAddr" id="chk' + data[index].AddressId + '"/>';
                                html += '   <a  class="block">';
                                html += '    <i class="pen_s icon_s pa margin_t15"></i>';
                                html += '     <p class="oh lh25"><b class="fl f16" name="receiver">' + data[index].Receiver + '</b><i class="fr f14"  name="phone">' + data[index].Phone + '</i></p>';
                                html += '   <p class="lh20 gray9"  name="addressDesc">' + data[index].AddressDesc + '</p>';
                                html += '   </a> </li>';

                    });

                    html += ' </ul></div>';
                    $("#addrsMain").append(html);
                }
            },
            error:function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });

        AlertDiv('#alert_address');
    });

   
});