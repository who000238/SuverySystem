<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SuverySystem.Test.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script language="javascript" type="text/javascript">
        function buttonClick() {
            alert("我是客戶端點選事件");
            return false;
        }
    </script> 
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn" Text="text" runat="server" OnClick="btn_Click" OnClientClick="return buttonClick();" />
        </div>
     
    </form>
</body>
</html>
