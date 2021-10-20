<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/SysterAdmin.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="SuverySystem.SystemAdmin.Detail1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {
            $("#tabs").tabs();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">問卷</a></li>
            <li><a href="#tabs-2">問題</a></li>
            <li><a href="#tabs-3">填寫資料</a></li>
            <li><a href="#tabs-4">統計</a></li>
        </ul>
        <div id="tabs-1">
            <asp:TextBox ID="txtSuveryTitle" runat="server" placeholder="問卷名稱" TextMode="SingleLine"></asp:TextBox><br />
            <asp:TextBox ID="txtInner" runat="server" placeholder="描述內容" TextMode="MultiLine"></asp:TextBox><br />
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox><br />
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" Text="已啟用" />
            <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
            <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
        </div>
        <div id="tabs-2">
            種類
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList><br />
            問題<asp:TextBox ID="txtQuestion" runat="server"></asp:TextBox>
            <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
            <asp:CheckBox ID="CheckBox2" runat="server" Text="必填" /><br />
            回答<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>(多個答案以，分隔)
            <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" /><br />
            <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" />
            <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                    <div class="row">
                        <div class="col-1">勾選</div>
                        <div class="col-2">編號</div>
                        <div class="col-4">問題</div>
                        <div class="col-2">種類</div>
                        <div class="col-1">必填</div>
                        <div class="col-2">編輯</div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="row">
                        <div class="col-1"></div>
                        <div class="col-2"></div>
                        <div class="col-4"></div>
                        <div class="col-2"></div>
                        <div class="col-1"></div>
                        <div class="col-2"></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
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
</asp:Content>
