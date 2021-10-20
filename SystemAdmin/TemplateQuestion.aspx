<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="TemplateQuestion.aspx.cs" Inherits="SuverySystem.SystemAdmin.TemplateQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {
            $("#btnAdd").click(function () {
                var QuestionName = $("#txtQuestionName").val();
                var QuestionType = $("#dllQuestionType").val();
                var isRequire = $("#isRequire").val();
                alert("Hello");
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>常用問題管理</h2>
    問題集名稱<asp:TextBox ID="txtQuestionName" runat="server"></asp:TextBox><br />
    問題種類<asp:DropDownList ID="dllQuestionType" runat="server">
        <asp:ListItem Value="1">文字方塊-文字</asp:ListItem>
        <asp:ListItem Value="2">文字方塊-數字</asp:ListItem>
        <asp:ListItem Value="3">文字方塊-Email</asp:ListItem>
        <asp:ListItem Value="4">文字方塊-日期</asp:ListItem>
        <asp:ListItem Value="5">單選方塊</asp:ListItem>
        <asp:ListItem Value="6">複選方塊</asp:ListItem>
    </asp:DropDownList>
    <asp:CheckBox ID="isRequire" runat="server" Text="必填" />
    <button type="button" id="btnAdd">加入</button><br />
    <button type="button" id="btnDelete">刪除</button><br />
    <div id="NewQ"></div>
    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <div class="row">
                <div class="row-1"></div>
                <div class="row-1">#</div>
                <div class="row-5">問題</div>
                <div class="row-3">種類</div>
                <div class="row-2">必填</div>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row">
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
    <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
</asp:Content>
