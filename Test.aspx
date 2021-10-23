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
        <%--        <div>
            <h3>CheckBoxList Example </h3>

            Select items from the CheckBoxList.

      <br />
            <br />

            <asp:CheckBoxList ID="checkboxlist1"
                AutoPostBack="True"
                CellPadding="5"
                CellSpacing="5"
                RepeatColumns="2"
                RepeatDirection="Vertical"
                RepeatLayout="Flow"
                TextAlign="Right"
                OnSelectedIndexChanged="Check_Clicked"
                runat="server">

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
        </div>--%>
        <div runat="server" id="TestArea"></div>
        <asp:Label Text="" runat="server" ID="lbl" />
        <asp:Button Text="送出" runat="server" ID="btnCheck" OnClick="btnCheck_Click" />
        <div>
            <asp:CheckBoxList runat="server">
                <asp:ListItem Text="text1" />
                <asp:ListItem Text="text2" />
            </asp:CheckBoxList>
        </div>
        <%--   <div>

          <div id="QuestionArea" class="col-12">
                <span>問題區</span><br />
                <span>全名</span><input name="Q1" type="text" id="Q1" class="Answer" /><span></br></span><span>性別</span><table id="Q2" class="Answer">
                    <tr>
                        <td>
                            <input id="Q2_0" type="radio" name="Q2" value="男" /><label for="Q2_0">男</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q2_1" type="radio" name="Q2" value="女" /><label for="Q2_1">女</label></td>
                    </tr>
                </table>
                <span></br></span><span>年齡區間</span><table id="Q3" class="Answer">
                    <tr>
                        <td>
                            <input id="Q3_0" type="radio" name="Q3" value="成年" /><label for="Q3_0">成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q3_1" type="radio" name="Q3" value="未成年" /><label for="Q3_1">未成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q3_2" type="radio" name="Q3" value="不顯示" /><label for="Q3_2">不顯示</label></td>
                    </tr>
                </table>
                <span></br></span><span>性別</span><table id="Q4" class="Answer">
                    <tr>
                        <td>
                            <input id="Q4_0" type="checkbox" name="Q4$0" value="男" /><label for="Q4_0">男</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q4_1" type="checkbox" name="Q4$1" value="女" /><label for="Q4_1">女</label></td>
                    </tr>
                </table>
                <span></br></span><span>年齡區間</span><table id="Q5" class="Answer">
                    <tr>
                        <td>
                            <input id="Q5_0" type="checkbox" name="Q5$0" value="成年" /><label for="Q5_0">成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q5_1" type="checkbox" name="Q5$1" value="未成年" /><label for="Q5_1">未成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q5_2" type="checkbox" name="Q5$2" value="不顯示" /><label for="Q5_2">不顯示</label></td>
                    </tr>
                </table>
                <span></br></span><span>性別(必填)</span><table id="Q6" class="isRequired Answer">
                    <tr>
                        <td>
                            <input id="Q6_0" type="radio" name="Q6" value="男" /><label for="Q6_0">男</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q6_1" type="radio" name="Q6" value="女" /><label for="Q6_1">女</label></td>
                    </tr>
                </table>
                <span></br></span><span>年齡區間(必填)</span><table id="Q7" class="isRequired Answer">
                    <tr>
                        <td>
                            <input id="Q7_0" type="radio" name="Q7" value="成年" /><label for="Q7_0">成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q7_1" type="radio" name="Q7" value="未成年" /><label for="Q7_1">未成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q7_2" type="radio" name="Q7" value="不顯示" /><label for="Q7_2">不顯示</label></td>
                    </tr>
                </table>
                <span></br></span><span>性別(必填)</span><table id="Q8" class="isRequired Answer">
                    <tr>
                        <td>
                            <input id="Q8_0" type="checkbox" name="Q8$0" value="男" /><label for="Q8_0">男</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q8_1" type="checkbox" name="Q8$1" value="女" /><label for="Q8_1">女</label></td>
                    </tr>
                </table>
                <span></br></span><span>年齡區間(必填)</span><table id="Q9" class="isRequired Answer">
                    <tr>
                        <td>
                            <input id="Q9_0" type="checkbox" name="Q9$0" value="成年" /><label for="Q9_0">成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q9_1" type="checkbox" name="Q9$1" value="未成年" /><label for="Q9_1">未成年</label></td>
                    </tr>
                    <tr>
                        <td>
                            <input id="Q9_2" type="checkbox" name="Q9$2" value="不顯示" /><label for="Q9_2">不顯示</label></td>
                    </tr>
                </table>
                <span></br></span>
            </div>

        </div>--%>
    </form>

</body>
</html>
