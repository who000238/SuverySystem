<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="SuverySystem.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>前台</h1>

        <div id="testarea" runat="server"></div>

        <div style="border:dashed 1px">
               <asp:TextBox ID="txtSuveryTitle" runat="server" placeholder="問卷標題" Width="408px" TextMode="Search"></asp:TextBox><br />
            <asp:TextBox ID="txtStartDate" runat="server" placeholder="開始日期" TextMode="Date"></asp:TextBox>
            <asp:TextBox ID="txtEndDate" runat="server" placeholder="結束日期" TextMode="Date"></asp:TextBox><br />
            <asp:Button ID="btnSreach" runat="server" Text="搜尋" OnClick="btnSreach_Click" />
        </div>
        <div style="border:dashed 1px">
            <asp:GridView ID="gvSuveryList" AllowPaging="true" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvSuveryList_RowDataBound" OnPageIndexChanging="gvSuveryList_PageIndexChanging">
                 <PagerSettings Mode="NumericFirstLast"
                    FirstPageText="<<" LastPageText=">>"
                    PreviousPageText="<" NextPageText=">"
                     PageButtonCount="5"
                    Position="Bottom"/>
                <Columns>
                    <asp:BoundField DataField="No" HeaderText="編號" />
                    <asp:TemplateField HeaderText="標題">
                        <ItemTemplate>
                            <a href="Form.aspx?ID=<%#Eval("Guid") %>"><%#Eval("Title") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="狀態"> 
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStatus"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="StartDate" HeaderText="開始日期" />
                    <asp:BoundField DataField="EndDate" HeaderText="結束日期" />
                    <asp:TemplateField HeaderText="觀看統計">
                        <ItemTemplate>
                            <a href="Statistic.aspx?ID=<%#Eval("Guid") %>">前往</a>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
           <%-- <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                    <div>#</div>
                    <div>問卷</div>
                    <div>狀態</div>
                    <div>開始時間</div>
                    <div>結束時間</div>
                    <div>觀看統計</div>
                </HeaderTemplate>
                <ItemTemplate>
                      <div><%#Eval("No") %></div>
                    <div><a href="#"><%#Eval("Title") %></a></div>
                    <div><%#Eval("Status") %></div>
                    <div><%#Eval("StartDate") %></div>
                    <div><%#Eval("EndDate") %></div>
                    <div><a href="#">觀看統計</a></div>
                </ItemTemplate>
            </asp:Repeater>--%>
        </div>
    </form>
</body>
</html>
