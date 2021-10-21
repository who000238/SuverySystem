<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmPage.aspx.cs" Inherits="SuverySystem.ConfirmPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="css/bootstrap.css" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="Scripts/jQuery-min-3.6.0.js"></script>
    <style>
        div {
            border: 1px solid;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-6">
                    <h1>前台-確認頁面</h1>
                </div>
              <div class="col-6">
                     <asp:Literal ID="ltlStatusAndDate" runat="server"></asp:Literal>
                </div>
                <div class="col-12">
                    <h3 runat="server" id="h3Title"></h3>
                </div>
                <div class="col-12" id="AnsArea" runat="server">

                </div>
                <div class="col-12">
                    <asp:Button ID="btnModify" runat="server" Text="修改" OnClick="btnModify_Click" />
                    <input type="button" onclick="javascript:history.back();" value="修改JS"/>
                    <asp:Button ID="btnSubmit" runat="server" Text="送出" onclientclick="javascript:return confirm('確定執行？');" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
