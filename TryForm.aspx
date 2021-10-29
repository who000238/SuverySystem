<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryForm.aspx.cs" Inherits="SuverySystem.TryForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
       <link rel="stylesheet" href="css/bootstrap.css" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="Scripts/jQuery-min-3.6.0.js"></script>
    <style>
        div{
            border:1px solid
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
                    <span>個人資料填寫區</span><br />
                    <asp:TextBox runat="server" id="UserName" placeholder="姓名" TextMode="SingleLine"  required="required" aria-required="true"/><br />
                    <asp:TextBox runat="server" id="UserPhone" placeholder="電話" TextMode="Phone" required="required" aria-required="true" /><br />
                    <asp:TextBox runat="server" id="UserMail" placeholder="E-Mail" TextMode="Email" required="required" aria-required="true" /><br />
                    <asp:TextBox runat="server" id="UserAge" placeholder="年齡" TextMode="Number" required="required" aria-required="true" />


                </div>
                <div class="col-12" runat="server" id="QuestionArea">
                    <span>問題區</span><br />
                </div>
                <div class="col-12">
                    <asp:Button ID="btnCancle" runat="server" Text="取消" OnClick="btnCancle_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click"/>
                </div>
            </div>
        </div>
        <script>
    
            //$(function () {
            //    $("#btnSubmit").click(function () {
            //        alert('Hello');
            //        var QCount = document.getElementsByClassName('Answer')
            //        alert(QCount.length);

            //        //字串版本
            //        //var AnswerData = $(".Answer").serialize(); //文字方塊可讀取，單多選無法取值
            //        //alert(AnswerData); 

            //        //陣列版本
            //        //var AnswerData = $(".Answer").val(); //文字方塊可讀取，單多選無法取值
            //        //for (var i = 0; i < AnswerData.length; i++) {
            //        //    alert(AnswerData[i].value);
            //        //}


            //        var AnswerData = $(".Answer").val(); //文字方塊可讀取，單多選無法取值
            //        alert(AnswerData); 

            //        //var AnswerString;
            //        //for (var i = 0; i < QCount.length; i++) {
            //        //    AnswerString = AnswerString + ",";
            //        //    if (QCount.length - i == 1) {
            //        //        alert(AnswerString);
            //        //    }
            //        //}
            //        //var QuestionArea = document.getElementById('QuestionArea');
            //        //var QCount = getElementsByTagName('input');
            //        //alert(QCount);
              
            //    });
            //});
        </script>
    </form>
</body>
</html>