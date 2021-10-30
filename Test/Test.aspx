<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="SuverySystem.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script runat="server">

//void Check_Clicked(Object sender, EventArgs e) 
//{

//   Message.Text = "Selected Item(s):<br /><br />";

//   // Iterate through the Items collection of the CheckBoxList 
//   // control and display the selected items.
//   for (int i=0; i<checkboxlist1.Items.Count; i++)
//   {

//      if (checkboxlist1.Items[i].Selected)
//      {

//         Message.Text += checkboxlist1.Items[i].Text + "<br />";

//      }

//   }

//}

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="TestArea"></div>
        <div>
            <h3>CheckBoxList Example </h3>

            Select items from the CheckBoxList.

      <br />
            <br />
            <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                <asp:ListItem>Item 1</asp:ListItem>
                <asp:ListItem>Item 2</asp:ListItem>
                <asp:ListItem>Item 3</asp:ListItem>
                <asp:ListItem>Item 4</asp:ListItem>
                <asp:ListItem>Item 5</asp:ListItem>
                <asp:ListItem>Item 6</asp:ListItem>
            </asp:CheckBoxList>

            <br />
            <br />

            <asp:Label ID="Message" runat="server" AssociatedControlID="checkboxlist1" />
        </div>
        <asp:Button ID="btnCheck" runat="server" Text="Button" OnClick="btnCheck_Click" />
    </form>

</body>
</html>
