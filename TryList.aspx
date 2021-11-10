<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryList.aspx.cs" Inherits="SuverySystem.TryList" %>

<%@ Register Src="~/UserControls/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


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

        a.disabled {
            pointer-events: none;
            cursor: default;
            color:black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div><a href="SystemAdmin/AdminList.aspx">前往後台</a></div>
            <div><a href="SystemAdmin/AdminList.aspx" class="disabled">前往後台</a></div>
            <a href="TestPage.aspx">Test.aspx</a>
            <h1>前台</h1>
            <div style="border: dashed 1px">
                <asp:TextBox ID="txtSuveryTitle" runat="server" placeholder="問卷標題" Width="408px" TextMode="Search"></asp:TextBox><br />
                <asp:TextBox ID="txtStartDate" runat="server" placeholder="開始日期" TextMode="Date"></asp:TextBox>
                <asp:TextBox ID="txtEndDate" runat="server" placeholder="結束日期" TextMode="Date"></asp:TextBox><br />
                <asp:Button ID="btnSreach" runat="server" Text="搜尋" OnClick="btnSreach_Click" />
            </div>
            <div style="border: dashed 1px">
                <div runat="server" class="row">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <div class="row">
                                <div class="col-1">#</div>
                                <div class="col-4">標題</div>
                                <div class="col-2">狀態</div>
                                <div class="col-2">開始時間</div>
                                <div class="col-2">結束時間</div>
                                <div class="col-1">觀看統計</div>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-1"><%#Eval("No") %></div>
                                <div class="col-4"><a href="TryForm.aspx?ID=<%#Eval("SuveryID") %>" class="<%#Eval("ClassName") %>"><%#Eval("Title") %></a></div>
                                <div class="col-2"><%#Eval("Status") %></div>
                                <div class="col-2"><%#Eval("SDate") %></div>
                                <div class="col-2"><%#Eval("EDate") %></div>
                                <div class="col-1"><a href="Statistic.aspx?ID=<%#Eval("SuveryID") %>">前往</a></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div style="background-color: orange; opacity: 0.8; text-align: center">
                        <uc1:ucPager runat="server" ID="ucPager" PageSize="10" Url="/TryList.aspx" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
