﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="AdminDetail.aspx.cs" Inherits="SuverySystem.SystemAdmin.Detail1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {
            $("#tabs").tabs();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input type="hidden" id="hfSuveryID" runat="server" />


    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">問卷</a></li>
            <%--<li><a href="#tabs-2">問題</a></li>--%>
            <li><a href="#tabs-3">填寫資料</a></li>
            <li><a href="#tabs-4">統計</a></li>
        </ul>
        <div id="tabs-1">
            <asp:TextBox ID="txtSuveryTitle" runat="server" placeholder="問卷名稱" TextMode="SingleLine"></asp:TextBox><br />
            <asp:TextBox ID="txtSummary" runat="server" placeholder="描述內容" TextMode="MultiLine"></asp:TextBox><br />
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
            ~
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:CheckBox ID="StatusCheck" runat="server" Checked="True" Text="已啟用" />
            <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
            <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
            <br />
            <hr />
            <%-- </div>
        <div id="tabs-2">--%>
            種類
            <asp:DropDownList ID="TemplateQddl" runat="server" OnSelectedIndexChanged="TemplateQddl_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList><br />
            問題<asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>



            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


            <asp:DropDownList ID="QTypeddl" runat="server" OnSelectedIndexChanged="QTypeddl_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="QT1">文字方塊(文字)</asp:ListItem>
                <asp:ListItem Value="QT2">文字方塊(數字)</asp:ListItem>
                <asp:ListItem Value="QT3">文字方塊(E-Mail)</asp:ListItem>
                <asp:ListItem Value="QT4">文字方塊(日期)</asp:ListItem>
                <asp:ListItem Value="QT5">單選方塊</asp:ListItem>
                <asp:ListItem Value="QT6">多選方塊</asp:ListItem>
            </asp:DropDownList>




            <asp:CheckBox ID="QMustKeyIn" runat="server" Text="必填" /><br />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    回答<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>
                    (多個答案以，分隔)

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="QTypeddl" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>



            <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" /><br />
            <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" />

            <div runat="server" id="QuestionArea">
            </div>

            <asp:Button ID="btnCancle2" runat="server" Text="取消" OnClick="btnCancle2_Click" />
            <asp:Button ID="btnSubmit2" runat="server" Text="送出" OnClick="btnSubmit2_Click" />
        </div>
        <div id="tabs-3">
            <asp:Repeater ID="Repeater2" runat="server">
                <HeaderTemplate>
                    <div class="row">
                        <div>
                            <div class="col-1">編號</div>
                            <div class="col-4">姓名</div>
                            <div class="col-5">填寫時間</div>
                            <div class="col-2">觀看細節</div>
                        </div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="row">
                        <div class="col-1">#</div>
                        <div class="col-4">姓名</div>
                        <div class="col-5">填寫時間</div>
                        <div class="col-2">觀看細節</div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div id="tabs-4">
            <span>統計頁面</span>
        </div>
    </div>



    <script>
        $(function () {

            $("#ContentPlaceHolder1_btnAdd").click(function () {
                var SuveryID = $("#ContentPlaceHolder1_hfSuveryID").val();
                var DetailTitle = $("#ContentPlaceHolder1_txtQuestion").val();
                var DetailType = $("#ContentPlaceHolder1_QTypeddl").val();
                var DetailMustKeyin = $("#ContentPlaceHolder1_QMustKeyIn").val();
                var ItemName = $("#ContentPlaceHolder1_txtAnswer").val();
                $.ajax({
                    url: "http://localhost:50503/SystemAdmin/TemplateQuestionHandler.ashx?actionName=Create",
                    type: "POST",
                    data: {
                        "SuveryID": SuveryID,
                        "DetailTitle": DetailTitle,
                        "DetailType": DetailType,
                        "DetailMustKeyin": DetailMustKeyin,
                        "ItemName": ItemName

                    },
                    success: function (result) {
                    }
                });
            });
            var SuveryID = $("#hfSuveryID").val();
            $.ajax({
                url: "http://localhost:50503/SystemAdmin/TemplateQuestionHandler.ashx?actionName=Load",
                type: "GET",
                data: {},
                success: function (result) {

                    var table = '<table class="table table-striped table-hover" id="QuestionTable">';
                    table += '<tr><th>勾選</th><th>編號</th><th>問題標題</th><th>種類</th><th>必填</th><th>編輯</th></tr>';
                    for (var i = 0; i < result.length; i++) {
                        var obj = result[i];
                        var htmlText =
                            `<tr>
                                    <td  style="width:5%">
                                        <asp:CheckBox  runat="server" />
                                    </td>
                                    <td  style="width:5%">
                                            <input type="hidden" class="hfRowID" value="${obj.QuestionNo}"/>
                                            <span>${obj.QuestionNo}</span>
                                    </td>
                                    <td  style="width:50%">${obj.DetailTitle}</td>
                                    <td  style="width:25%">${obj.DetailType}</td>
                                    <td  style="width:5%">
                                        <asp:CheckBox  runat="server" />
                                    </td>
                                    <td  style="width:10%">
                                        <button type="button" class="Update" text="編輯">
                                    </td>
                            </tr>`;
                        table += htmlText;
                    }
                    table += "</table>";
                    $("#ContentPlaceHolder1_QuestionArea").append(table);

                }
            });
        });
    </script>
</asp:Content>
