<%@ Page Title="" Language="C#" MasterPageFile="~/XtMain.Master" AutoEventWireup="true" CodeBehind="ShowingOrder.aspx.cs" Inherits="XT.ShowingOrder.ShowingOrder" %>

<%@ Register Src="~/MainFooter.ascx" TagPrefix="uc1" TagName="MainFooter" %>


<asp:Content ID="ShowingOrder" ContentPlaceHolderID="ContentXT" runat="server">
    <script type="text/javascript" src="../Scripts/ShowingOrder/ShowingOrder.js"></script>

     <div class="mainM">
        
        <div class="comment" id="divComment">
            
        </div>
        
    </div>
    
    <uc1:MainFooter runat="server" id="MainFooterAscx" />
</asp:Content>

