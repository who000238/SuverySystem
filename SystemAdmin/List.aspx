<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="SuverySystem.SystemAdmin.List1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="border: dashed 1px">
        <asp:TextBox ID="SuveryTitle" runat="server" placeholder="問卷標題" Width="408px" TextMode="Search"></asp:TextBox><br />
        <asp:TextBox ID="StartDate" runat="server" placeholder="開始日期" TextMode="Date"></asp:TextBox>
        <asp:TextBox ID="EndDate" runat="server" placeholder="結束日期" TextMode="Date"></asp:TextBox><br />
        <asp:Button ID="btnSreach" runat="server" Text="Sreach" />
    </div>
    <div>
        <asp:Button ID="btnDelete" runat="server" Text="刪除問卷" />
        &nbsp;&nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="新增問券" />
    </div>
    <div>
        <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <div class="row">
                    <div class="col-1">勾選</div>
                    <div class="col-1">編號</div>
                    <div class="col-4">標題</div>
                    <div class="col-1">狀態</div>
                    <div class="col-2">開始時間</div>
                    <div class="col-2">結束時間</div>
                    <div class="col-1">觀看統計</div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="row">
                    <div class="col-1"><asp:CheckBox ID="CheckBox1" runat="server" /></div>
                    <div class="col-1"><%#Eval("No") %></div>
                    <div class="col-4"><a href="#"><%#Eval("SuveryTitle") %></a></div>
                    <div class="col-1"><%#Eval("Status") %></div>
                    <div class="col-2"><%#Eval("StartDate") %></div>
                    <div class="col-2"><%#Eval("EndDate") %></div>
                    <div class="col-1"><a href="#">觀看統計</a></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
