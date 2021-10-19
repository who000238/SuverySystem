<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="SuverySystem.SystemAdmin.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>後台 - 問卷管理</h1>
        <div>
            <a href="/SystemAdmin/Detail.aspx">問卷管理</a><br />
            <a href="#">常用問題管理</a>
        </div>
        <div style="border: dashed 1px">
            <asp:TextBox ID="SuveryTitle" runat="server" placeholder="問卷標題" Width="408px" TextMode="Search"></asp:TextBox><br />
            <asp:TextBox ID="StartDate" runat="server" placeholder="開始日期" TextMode="Date"></asp:TextBox>
            <asp:TextBox ID="EndDate" runat="server" placeholder="結束日期" TextMode="Date"></asp:TextBox><br />
            <asp:Button ID="btnSreach" runat="server" Text="Sreach" />
        </div>
        <div>
            <asp:Button ID="btnDelete" runat="server" Text="刪除問卷" /> &nbsp;&nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="新增問券" />
        </div>
    </form>
</body>
</html>
