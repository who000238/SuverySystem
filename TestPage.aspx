<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="SuverySystem.TestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="css/bootstrap.css" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="Scripts/jQuery-min-3.6.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="HFdata" runat="server" />
        <div runat="server" id="div">
        </div>
        <asp:Button ID="btn" runat="server" Text="Button" OnClick="btn_Click" />
        <asp:Button Text="跳頁" runat="server" OnClick="Unnamed_Click" OnClientClick="javascript:return aa();" />
        <script>    


            function aa() {
                var result = $("#HFdata").val();
                if (result == "Y") {
                    if (confirm('yes or no？')) {
                        alert('yes');
                        return true;
                    } else {
                        alert('no');
                        return false;
                    }
                }
            }
            $(function () {
                alert(result)



                //if (result == "Y") {
                //    var answer = confirm("確認跳頁?");
                //    if (answer)
                //        return true;
                //    else
                //        return false;
                //}
            });
        </script>
    </form>
</body>
</html>
