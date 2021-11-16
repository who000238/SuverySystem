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
    <asp:TextBox runat="server" ID="txtItemName" placeholder="請輸入項目名稱且用  ' , ' 隔開 最多四個選項 " Width="40%" />

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
                         <input type="hidden" class="hfQuestionTemplateNo" value="<%#Eval("QuestionTemplateNo") %>"/>
                        <asp:Button Text="刪除" runat="server" CssClass="btnDelete"/>
                    </div>
                    <div class="col-1"><%# Eval("QuestionTemplateNo") %></div>
                    <div class="col-5"><%# Eval("QuestionTemplateName") %></div>
                    <div class="col-3"><%# Eval("QuestionTemplateType") %></div>
                    <div class="col-2"><%# Eval("QuestionTemplateMustKeyIn") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
    <script>
        $(document).on("click", ".btnDelete", function () {
            if (confirm('確定刪除 ? ')) {
                var td = $(this).closest("div");
                var hf = td.find("input.hfQuestionTemplateNo");

                var QuestionTemplateNo = hf.val();
                $.ajax({
                    url: "http://localhost:50503/SystemAdmin/AdminTemplateQuestionHandler.ashx?actionName=Delete",
                    type: "POST",
                    data: {
                        "ID": QuestionTemplateNo,
                    },
                    success: function (result) {
                        alert('刪除成功');
                        location.reload()
                    }
                });
            }
        });
    </script>
</asp:Content>
