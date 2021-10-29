<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="SeeAnswerDetail.aspx.cs" Inherits="SuverySystem.SystemAdmin.SeeAnswerDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                    <input type="button" onclick="javascript:history.back();" value="上一頁"/>
    <div class="row">
        <asp:TextBox ID="tbName" runat="server" Enabled="false"></asp:TextBox>
        <asp:TextBox ID="tbPhone" runat="server" Enabled="false"></asp:TextBox>
        <asp:TextBox ID="tbEMail" runat="server" Enabled="false"></asp:TextBox>
        <asp:TextBox ID="tbAge" runat="server" Enabled="false"></asp:TextBox>
    </div>
    <div class="row">

    </div>
</asp:Content>
