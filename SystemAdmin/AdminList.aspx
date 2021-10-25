<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="AdminList.aspx.cs" Inherits="SuverySystem.SystemAdmin.List1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div style="border: dashed 1px">
            <asp:TextBox ID="txtSuveryTitle" runat="server" placeholder="問卷標題" Width="408px" TextMode="Search"></asp:TextBox><br />
            <asp:TextBox ID="txtStartDate" runat="server" placeholder="開始日期" TextMode="Date"></asp:TextBox>
            <asp:TextBox ID="txtEndDate" runat="server" placeholder="結束日期" TextMode="Date"></asp:TextBox><br />
            <asp:Button ID="btnSreach" runat="server" Text="搜尋" OnClick="btnSreach_Click" />
        </div>
        <div>
            <asp:Button ID="btnDelete" runat="server" Text="刪除問卷" OnClick="btnDelete_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="新增問卷" OnClick="btnAdd_Click" />
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
                            <div class="col-1"><%#Eval("SuveryNo") %></div>
                            <div class="col-4"><a href="TryForm.aspx?ID=<%#Eval("SuveryID") %>"><%#Eval("Title") %></a></div>
                            <div class="col-2"><%#(Eval("Status")=="N") ? "關閉中":"開放中" %></div>
                            <div class="col-2"><%#Eval("StartDate") %></div>
                            <div class="col-2"><%#Eval("EndDate") %></div>
                            <div class="col-1"><a href="Statistic.aspx?ID=<%#Eval("SuveryID") %>">前往</a></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
</asp:Content>
