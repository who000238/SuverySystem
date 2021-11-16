<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SuverySystem.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="Scripts/jQuery-min-3.6.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtAcc" runat="server" placeholder="帳號" TextMode="SingleLine"></asp:TextBox>
            <asp:TextBox ID="txtPwd" runat="server" placeholder="密碼" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnLogin" runat="server" Text="登入" OnClick="btnLogin_Click" />
            <asp:Button ID="btnCreateAcc" runat="server" Text="申請" OnClick="btnCreateAcc_Click" style="height: 32px" />
        </div>
    </form>
</body>
</html>
