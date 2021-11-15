<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryForm.aspx.cs" Inherits="SuverySystem.TryForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="css/bootstrap.css" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="Scripts/jQuery-min-3.6.0.js"></script>
    <style>
        div {
            border: 1px solid
        }
        .ErrorMSG{
            color: red
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div>
                    <asp:HiddenField runat="server" ID="HFData" />
                </div>
                <div class="col-6">
                    <h1>前台</h1>
                </div>
                <div class="col-6">
                    <asp:Literal ID="ltlStatusAndDate" runat="server"></asp:Literal>
                </div>
                <div class="col-12">
                    <h3 runat="server" id="h3Title"></h3>
                </div>
                <div class="col-12">
                    <asp:Literal ID="ltlInnerText" runat="server"></asp:Literal>
                </div>
                <div class="col-12" runat="server" id="UserInformation">
                    <span>個人資料填寫區 (必填)</span><br />
                    <asp:TextBox runat="server" ID="UserName" placeholder="姓名" TextMode="SingleLine" required="required" aria-required="true" /><br />
                    <asp:TextBox runat="server" ID="UserPhone" placeholder="電話" TextMode="Phone" required="required" aria-required="true" /><br />
                    <asp:TextBox runat="server" ID="UserMail" placeholder="E-Mail" TextMode="Email" required="required" aria-required="true" /><br />
                    <asp:TextBox runat="server" ID="UserAge" placeholder="年齡" TextMode="Number" min="0" max="130" required="required" aria-required="true" />


                </div>
                <div class="col-12" runat="server" id="QuestionArea">
                    <span>問題區</span><br />
                </div>
                <div class="col-12">
                    <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
        <script>

            $(function () {
                //$("#btnSubmit").click(function () {
                //    var SelectQuestion = document.getElementsByClassName("MustKeyIn");
                //    var haveValueOrNot - SelectQuestion.selectedIndex;
                //var temp = $(".MustKeyInRB").children("input").attr("id");
                //alert(temp);
                //$(`input[id='${temp}']`).attr("class", "180");
                    $("#btnSubmit").click(function () {
                        //alert('hoeel');
                        //var RBObj = $('.MustKeyInRB').children("input radio");
                        //alert(RBObj);
                        //alert(RBObj.length);
                        //RBObj.attr(required,"required")

                        ////RBObj.required = true;
                        ////var CBLObj = $('.MustKeyInRB input: checkbox:');
                        ////alert(CBLObj.val);
                        ////CBLObj.required = true;


                        ////var MustKeyInRBExist = $(".MustKeyInRB").val;
                        //var MustKeyInRBExist = document.getElementsByClassName("MustKeyInRB");
                        //alert(MustKeyInRBExist);
                        //if (MustKeyInRBExist > 0) {
                        //    var RBSelected = $('.MustKeyInRB input:radio:checked').val();
                        //    if (RBSelected == null) {
                        //        alert("好像還有必填問題沒填寫/選取!");
                        //        return false;
                        //    }
                        //}
                        ////var MustKeyInCBLExist = $(".MustKeyInCBL").val;
                        //var MustKeyInCBLExist = document.getElementsByClassName("MustKeyInCBL");
                        //alert(MustKeyInCBLExist);
                        //if (MustKeyInCBLExist > 0) {
                        //    var CBLSelected = $('.MustKeyInCBL input:checkbox:checked').val();
                        //    if (CBLSelected == null) {
                        //        alert("好像還有必填問題沒填寫/選取!");
                        //        return false;
                        //    }
                        //}


                        //var RBSelected = $('.MustKeyIn input:radio:checked').val();
                        //if (RBSelected == null) {
                        //    alert("好像還有必填問題沒填寫/選取!");
                        //    return false;
                        //}

                        //var MustKeyInRBExist = this.getElementsByClassName("MustKeyInRB");
                        //if (MustKeyInRBExist) {
                        //    var RBSelected = $('.MustKeyInRB input:radio:checked').val();
                        //    if (RBSelected == null) {
                        //        alert("好像還有必填問題沒填寫/選取!");
                        //        return false;
                        //    }
                        //}

                        //var MustKeyInCBLExist = this.getElementsByClassName("MustKeyInCBL");
                        //if (MustKeyInCBLExist) {
                        //    var CBLSelected = $('.MustKeyInCBL input:checkbox:checked').val();
                        //    if (CBLSelected == null) {
                        //        alert("好像還有必填問題沒填寫/選取!");
                        //        return false;
                        //    }
                        //}

                    });
            });
        </script>
    </form>
</body>
</html>
