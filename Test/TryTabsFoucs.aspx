<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryTabsFoucs.aspx.cs" Inherits="SuverySystem.TryTabsFoucs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="JS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="JS/jquery-ui.min.js" type="text/javascript"></script>
    <script src="https://cdn.staticfile.org/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
    <script src="Scripts/custom.min.js"></script>
     <script type="text/javascript">
         $(function () {

             //tabs頁簽 使用cookie記住最後開啟的頁簽
             $("#tabs").tabs({

                 //起始頁active: 導向cookie("tabs")所指頁簽，如果空白導向tabs:0
                 active: ($.cookie("tabs") || 0),

                 //換頁動作activate 
                 activate: function (event, ui) {
                     //取得選取的頁簽編號
                     var newIndex = ui.newTab.parent().children().index(ui.newTab);

                     //記錄到cookie   
                     $.cookie("tabs", newIndex, { expires: 1 });
                 }
             });
         })
     </script>
   
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidCurrentTab" runat="server" />
        <div>
            <div id="tabs">
                <ul>
                    <li><a href="#Tab1"><span>Tab1</span></a></li>
                    <li><a href="#Tab2"><span>Tab2</span></a></li>
                    <li><a href="#Tab3"><span>Tab3</span></a></li>
                </ul>
                <div id="Tab1">
                    This is Tabl1.                
                </div>
                <div id="Tab2">
                    This is Tabl2.
                    <asp:Button ID="Button1" runat="server" Text="Tab2 Button" />
                </div>
                <div id="Tab3">
                    This is Tabl3.
                    <asp:Button ID="Button2" runat="server" Text="Tab3 Button" />
                </div>
            </div>
        </div>
    <%--     <script>
             $(document).ready(function () {
                 var tabIndex = $("#hidCurrentTab").val();
                 alert(tabIndex);
                 $("#tabs").tabs({
                     select: function (event, ui) {
                         $("#hidCurrentTab").val(ui.index);
                     }
                     , selected: tabIndex
                 });
             });
        //$(document).ready(function () {
        //    $("#tabs").tabs();
        //});
        //$(function () {
        //    $('#tabs').tabs({ cookie: { expires: 30 } });
        //});

         </script>--%>
    </form>
</body>
</html>
