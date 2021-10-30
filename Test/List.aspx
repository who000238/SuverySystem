<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="SuverySystem.List" %>

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
            <div><a href="TryList.aspx">前往</a></div>
            <h1>前台</h1>
            <div runat="server" id="TestArea"></div>
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
                                <div class="col-4"><a href="Form.aspx?ID=<%#Eval("Guid") %>"><%#Eval("Title") %></a></div>
                                <div class="col-2"><%#((int)Eval("Status")==0) ? "關閉中":"開放中" %></div>
                                <div class="col-2"><%#Eval("StartDate") %></div>
                                <div class="col-2"><%#Eval("EndDate") %></div>
                                <%--<div class="col-1"><a href="Statistic.aspx?ID=<%#Eval("Guid") %>">前往</a></div>--%>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
