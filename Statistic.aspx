<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistic.aspx.cs" Inherits="SuverySystem.Statistic" %>

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
            border: 1px solid
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>前台</h1>
            <div class="col-12">
                統計頁面
            </div>
            <div class="col-12">
                <h3 runat="server" id="h3Title"></h3>
            </div>
            <div runat="server" id="TestArea"></div>
        </div>
    </form>
</body>
</html>
