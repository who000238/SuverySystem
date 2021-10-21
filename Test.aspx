<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="SuverySystem.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox runat="server" required="required" aria-required="true" TextMode="SingleLine"/>
            <asp:Button Text="submit" runat="server" />
            <asp:CheckBox ID="CheckBox1" runat="server" /><asp:CheckBox ID="CheckBox2" runat="server" /><br />
            <asp:CheckBoxList ID="CheckBoxList1" runat="server"></asp:CheckBoxList>
        </div>
    </form>
</body>
</html>
