<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="AdminTemplateQuestion.aspx.cs" Inherits="SuverySystem.SystemAdmin.TemplateQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>常用問題管理</h2>
    <asp:TextBox ID="txtQuestionName" runat="server" TextMode="SingleLine" placeholder="問題集名稱"></asp:TextBox>
    <asp:DropDownList ID="ddlQuestionType" runat="server" OnSelectedIndexChanged="dllQuestionType_SelectedIndexChanged" AutoPostBack="true" placeholder="問題種類">
        <asp:ListItem Value="QT1">文字方塊-文字</asp:ListItem>
        <asp:ListItem Value="QT2">文字方塊-數字</asp:ListItem>
        <asp:ListItem Value="QT3">文字方塊-Email</asp:ListItem>
        <asp:ListItem Value="QT4">文字方塊-日期</asp:ListItem>
        <asp:ListItem Value="QT5">單選方塊</asp:ListItem>
        <asp:ListItem Value="QT6">複選方塊</asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox runat="server" ID="txtItemName" placeholder="請輸入項目名稱且用  ' , ' 隔開 " />

    <asp:CheckBox ID="MustKeyIn" runat="server" Text="必填" />
    <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" OnClientClick="javascript:return confirm('確定執行？');" />
    <br />
    <div id="NewQ"></div>
    <div class="row">
        <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <div class="row">
                    <div class="col-1">刪除</div>
                    <div class="col-1">#</div>
                    <div class="col-5">問題</div>
                    <div class="col-3">種類</div>
                    <div class="col-2">必填</div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="row">
                    <div class="col-1">
                        <asp:HiddenField ID="HiddenField1" runat="server"/>
                        <asp:Button Text="刪除" runat="server" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('確定刪除？')"/>
                    </div>
                    <div class="col-1"><%# Eval("QuestionTemplateNo") %></div>
                    <div class="col-5"><%# Eval("QuestionTemplateName") %></div>
                    <div class="col-3"><%# Eval("QuestionTemplateType") %></div>
                    <div class="col-2"><%# Eval("QuestionTemplateMustKeyIn") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
    <%--    <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
    <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />--%>
    <script>
    //$(function () {
    //    $(function () {
    //        $("#ContentPlaceHolder1_btnAdd").click(function () {
    //            var QuestionName = $("#ContentPlaceHolder1_txtQuestionName").val();
    //            var QuestionType = $("#ContentPlaceHolder1_dllQuestionType").val();
    //            switch (QuestionType) {
    //                case "QT1":
    //                    QuestionType = "文字方塊-文字";
    //                    break;
    //                case "QT2":
    //                    QuestionType = "文字方塊-數字";
    //                    break;
    //                case "QT3":
    //                    QuestionType = "文字方塊-E-Mail";
    //                    break;
    //                case "QT4":
    //                    QuestionType = "文字方塊-日期";
    //                    break;
    //                case "QT5":
    //                    QuestionType = "單選方塊";
    //                    break;
    //                case "QT6":
    //                    QuestionType = "多選方塊";
    //                    break;
    //                default:
    //            }
    //            var MustKeyIn = $("#ContentPlaceHolder1_MustKeyIn").val();
    //            if (MustKeyIn) {
    //                MustKeyIn = "Yes";
    //            }
    //            else {
    //                MustKeyIn = "No";
    //            }
    //            var ItenName = $("#ContentPlaceHolder1_txtItemName").val();
    //            var OutputString = "";
    //            if (ItenName) {
    //                OutputString =`標題:${QuestionName}\n格式:${QuestionType}\n必填:${MustKeyIn}\n選項名稱:${ItenName}\n按下確定新增常用問題`;
    //            }
    //            else {
    //                OutputString = `標題:${QuestionName}\n格式:${QuestionType}\n必填:${MustKeyIn}\n按下確定新增常用問題`;
    //            }
    //            var yes = confirm(OutputString);
    //            if (yes) {
    //                $("#hfBool").val = true;
    //            }
    //            else {
    //                $("#hfBool").val = false;
    //            }

    //        })
    //    });
    //});
    </script>
</asp:Content>
