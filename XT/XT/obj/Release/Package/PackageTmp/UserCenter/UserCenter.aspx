<%@ Page Title="" Language="C#" MasterPageFile="~/XtMain.Master" AutoEventWireup="true" CodeBehind="UserCenter.aspx.cs" Inherits="XT.UserCenter.UserCenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentXT" runat="server">
    <div class="mainM mine">
    	<div class="mineT white_bg">
        	<a href="" class="green_bg">
            <img src="images/user_in.png" class="fl"  />
            <i class="arrow_s fr icon_s "></i>
            <p>
            	<span class="f16 oh">余小卡</span>
                <span class="f14 oh">150****9460</span>
                
            </p>
            
            </a>     
        </div>
        <div class="mineM white_bg margin_t15">
        	<ul class="oh">
            	<li><a href="我的-支出明细.html"><h3><span class="f20 fn">99</span><i class="fn gray9"> 只</i></h3><p class="gray6">虾米</p></a></li>
                <li><a href="我的参与（一元）.html"><h3><span class="f20 fn">5</span><i class="fn gray9"> 个</i></h3><p class="gray6">参与</p></a></li>
                <li><a href="我的收藏.html"><h3><span class="f20 fn">2</span><i class="fn gray9"> 个</i></h3><p class="gray6">收藏</p></a></li>
            </ul>
        </div>
        <div class="mineB margin_t15">
        	<ul class="white_bg">
            	<li>
                <a href="分享有礼.html" class="oh">
                <i class="share_s icon_s fl margin_r10"></i>
                <p class="fl f16">免费拿虾米</p>
                </a>
                </li>
                <!--
                <li>
                <a class="oh">
                <i class="shopping_s icon_s fl margin_r10"></i>
                <p class="fl f16">海货市场</p>
                </a>
                </li>
                -->
                <li>
                <a href="虾米送礼.html" class="oh">
                <i class="gift_s icon_s fl margin_r10"></i>
                <p class="fl f16">虾米送礼</p>
                <span class="fr red_bg white br4">首单立减20</span>
                </a>
                </li>
                <li>
                <a href="收货地址.html" class="oh">
                <i class="locate_s icon_s fl margin_r10"></i>
                <p class="fl f16">收货地址</p>
                </a>
                </li>
                
            </ul>
            <ul class="margin_t15 white_bg">
                <li>
                <a class="oh">
                <i class="telephone_s icon_s fl margin_r10"></i>
                <p class="fl f16">服务电话</p>
                </a>
                </li>
                <li>
                <a class="oh">
                <i class="paper_s icon_s fl margin_r10"></i>
                <p class="fl f16">合作协议</p>
                </a>
                </li>
                
            </ul>
        </div>
        
        
    </div>
    
</asp:Content>
