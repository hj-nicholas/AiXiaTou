/*
 *  used by samuel
 *	#example:
 *	$("#test").touchwipe({
 *			min_move_x: 40, //横向灵敏度
 *			min_move_y: 40, //纵向灵敏度
 *			wipeLeft: function() {$("#val").append("左，");}, //左侧滑动事件
 *			wipeRight: function() { $("#val").append("右，");}, //右侧滑动事件
 *			wipeUp: function() { $("#val").append("上，");}, //向上滑动事件
 *			wipeDown: function() { $("#val").append("下，");}, //向下滑动事件
 *			wipe:function(){$("#val").append("点击，");}, //触摸事件
 *			wipehold:function(){$("#val").append("保持，");}, //触摸保持事件
 *			preventDefaultEvents: true //阻止默认事件
 *		});
 *
 */
(function(a){a.fn.touchwipe=function(c){var b={min_move_x:20,min_move_y:20,wipeLeft:function(){},wipeRight:function(){},wipeUp:function(){},wipeDown:function(){},wipe:function(){},wipehold:function(){},preventDefaultEvents:true};if(c){a.extend(b,c)}this.each(function(){var h;var g;var j=false;var i=false;var e;function m(){this.removeEventListener("touchmove",d);h=null;j=false;clearTimeout(e)}function d(q){if(b.preventDefaultEvents){q.preventDefault()}if(j){var n=q.touches[0].pageX;var r=q.touches[0].pageY;var p=h-n;var o=g-r;if(Math.abs(p)>=b.min_move_x){m();if(p>0){b.wipeLeft()}else{b.wipeRight()}}else{if(Math.abs(o)>=b.min_move_y){m();if(o>0){b.wipeUp()}else{b.wipeDown()}}}}}function k(){clearTimeout(e);if(!i&&j){b.wipe()}i=false}function l(){i=true;b.wipehold()}function f(n){if(n.touches.length==1){h=n.touches[0].pageX;g=n.touches[0].pageY;j=true;this.addEventListener("touchmove",d,false);e=setTimeout(l,750)}}if("ontouchstart" in document.documentElement){this.addEventListener("touchstart",f,false);this.addEventListener("touchend",k,false)}});return this};a.extend(a.fn.touchwipe,{version:"2.0",author:""})})(jQuery);