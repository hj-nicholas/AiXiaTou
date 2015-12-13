//made by samuel

//根据窗口大小获取对应css以及顶部标题显示
var sw,sh;

$(function () {
	window_meet();
});
$(window).resize(function(){
	window_meet();
});

function window_meet(){
	var fh,bg_w,bg_h,f_top,f_left;
	sw=$(window).width();
	sh=$(window).height();
	
	var sc = (sw>960 && 960) || (sw>720 && 720) || (sw>520 && 520) || 320;
	document.getElementById("mediaWidth").setAttribute("href","../../Content/css/page"+sc+".css");
	
}

//屏幕加载样式结束
//点击通用效果
$(document).ready(function(){
  $(".Animate_pulse").click(function(){
/*		  var str=$(this);
	$(this).addClass("swing animated");
	setTimeout("$('str').removeClass('swing animated')",1200);*/
	$(this).addClass("pulse animated");
	var $this=$(this);
	t2=setTimeout(function(){$this.removeClass('pulse animated');},1200);

	
  });
});
$(document).ready(function(){
	  $(".Animate_shake").click(function(){
		$(this).addClass("shake animated");
		var $this=$(this);
		t2=setTimeout(function(){$this.removeClass('shake animated');},1200);
	  });
	});
$(document).ready(function(){
  $(".Animate_bounce").click(function(){
/*		  var str=$(this);
	$(this).addClass("swing animated");
	setTimeout("$('str').removeClass('swing animated')",1200);*/
	$(this).addClass("bounce animated");
	var $this=$(this);
	t2=setTimeout(function(){$this.removeClass('bounce animated');},1200);

	
  });
});

//点击通用效结束
//输入框提示语
$(function() {
	  
	  	   $(".Ntext").each(function(index, element) {
            
			$(this).val($(this).next().val());
			$(this).css("color","#999999");
        	});
	  
          $(".Ntext").focus(function() {

              if ($(this).val() == $(this).next().val()) {
                  $(this).val('');
				  $(this).css("color","#333333");
              }
          }).blur(function() {
              if ($(this).val() == "") {
				   $(this).css("color","#999999");
                  $(this).val($(this).next().val());
              }else if($(this).val!=$(this).next().val()){
				  
				  $(this).css("color","#333333");
				  }
				 /* else if($(this).val==$(this).next().val())
				  {
					  $(this).css("color","#999999");
					  }*/
          });
		  
      }
  );

//输入框提示语结束

$(function() {
$("#index_more").toggle(function() {
	x=$(this).attr("name");
	$(this).html("收起 &nbsp;&nbsp;&and;");
	$(".none").show();
}, function() {
	$(this).html(x);
	$(".none").hide();
});
})
//还款table

$(document).ready(function(){
    $("table tr.repay_B1").click(function(){
		$("table tr.repay_B1").next("tr").removeClass("tableblock").addClass("none");
		c=$(this).next("tr").attr("class")
		if(c=="tableblock"){
            $(this).next("tr").removeClass("tableblock").addClass("none");
        }
        else{
            $(this).next("tr").removeClass("none").addClass("tableblock")
        }
		
		
    });
});


//获取验证码倒计时
$(document).ready(function(){
	
	$(".login_T a").click(function (){
	var i = 60;
	alert("aaa")
	setTimeout('shownum()',0);
	
	})
})
 

function shownum(){
	i=i-1
	$(".login_T a").html(i)	   
	setTimeout('shownum()',1000);
}


//切换页面
function selectTag(showContent,selfObj,a,b){
    // 操作标签
    var tag = document.getElementById(a).getElementsByTagName("li");
    var taglength = tag.length;
    for(i=0; i<taglength; i++){
        tag[i].className = "";
    }
    selfObj.parentNode.className = "hover";
    // 操作内容
    for(i=0; j=document.getElementById(b+i); i++){
        j.style.display = "none";
    }
    document.getElementById(showContent).style.display = "block";	
}

//add by hoo 2015-12-13
$(function() {
    //$("i[type='footerBtn']").click(function() {
    //    if($(this).hasClass("shaidan"))
    //});
});