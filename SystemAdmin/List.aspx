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
            <asp:Button ID="btnDelete" runat="server" Text="刪除問卷" /> &nbsp;&nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="新增問券" />
        </div>
</asp:Content>
