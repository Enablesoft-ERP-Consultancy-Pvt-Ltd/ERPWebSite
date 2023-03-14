<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmFiller_Shell.aspx.cs"
    Inherits="Masters_Carpet_frmFiller_Shell" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filler_Shell</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Iterate through each Textbox and add keyup event handler
            $(".ShellTxtToCalculate").each(function () {
                $(this).keyup(function () {
                    //Initialize total to 0
                    var total = 0;
                    var txttotalamount = getNum(parseFloat($("#txttotalamount").val()));
                    if (txttotalamount <= 0) {
                        alert("Please enter Total amount first!!");
                        this.value = "";
                        return false;
                    }
                    $(".ShellTxtToCalculate").each(function () {
                        // Sum only if the text entered is number and greater than 0
                        if (!isNaN(this.value) && this.value.length != 0) {
                            total += parseFloat(this.value);
                        }
                    });
                    //Assign the total to label
                    //.toFixed() method will roundoff the final sum to 2 decimal places
                    $('#<%=lblTotalShell.ClientID %>').html(total.toFixed(2));
                    var txtCurrencyShell = $("#txtCurrencyShell").val();
                    var shell = (total - txtCurrencyShell) / 100;
                    var amount = (txttotalamount + txttotalamount * shell) / getNum(parseFloat(txtCurrencyShell));
                    amount = amount == "Infinity" ? 0 : amount;
                    document.getElementById("txtQuote").value = (getNum(parseFloat(amount))).toFixed(2);
                });
            });
        });
    </script>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            //Iterate through each Textbox and add keyup event handler
            $(".FillerTxtToCalculate").each(function () {
                $(this).keyup(function () {
                    //Initialize total to 0
                    var total = 0;
                    var txtFilleramount = getNum(parseFloat($("#txtFilleramount").val()));
                    if (txtFilleramount <= 0) {
                        alert("Please enter Filler amount first!!");
                        this.value = "";
                        return false;
                    }

                    $(".FillerTxtToCalculate").each(function () {
                        // Sum only if the text entered is number and greater than 0
                        if (!isNaN(this.value) && this.value.length != 0) {
                            total += parseFloat(this.value);
                        }
                    });
                    //Assign the total to label
                    //.toFixed() method will roundoff the final sum to 2 decimal places
                    $('#<%=lblTotalFiller.ClientID %>').html(total.toFixed(2));
                });
            });
        });
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lbltotalamt" Text="Total Amount" CssClass="labelbold" runat="server" /><br />
                    <asp:TextBox ID="txttotalamount" CssClass="textb" runat="server" Width="100px" />
                </td>
                <td id="TDFillerAmount" runat="server" visible="false">
                    <asp:Label ID="lblFilleramount" Text="Total Filler Amount" CssClass="labelbold" runat="server" /><br />
                    <asp:TextBox ID="txtFilleramount" CssClass="textb" runat="server" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblCurrentCosting" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblcurrency" CssClass="labelbold" Text="Currency" runat="server" />
                    <asp:DropDownList ID="DDcurrency" runat="server" CssClass="dropdown" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table border="1px" cellpadding="5" style="border-collapse: collapse;">
            <tr style="text-align: left;">
                <th>
                    No.
                </th>
                <th>
                    Parameter
                </th>
                <th>
                    Shell
                </th>
                <%--  <th>
                    Filler
                </th>--%>
            </tr>
            <tr>
                <td>
                    1
                </td>
                <td>
                    Margin:
                </td>
                <td>
                    <asp:TextBox ID="txtmarginshell" Width="80px" CssClass="ShellTxtToCalculate" runat="server"></asp:TextBox>
                </td>
                <%--<td>
                    <asp:TextBox ID="txtmarginFiller" Width="80px" CssClass="FillerTxtToCalculate" runat="server"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>
                    2
                </td>
                <td>
                    Currency:
                </td>
                <td>
                    <asp:TextBox ID="txtCurrencyShell" Width="80px" CssClass="ShellTxtToCalculate" runat="server"></asp:TextBox>
                </td>
                <%--<td>
                    <asp:TextBox ID="txtcurrencyFiller" Width="80px" CssClass="FillerTxtToCalculate"
                        runat="server"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>
                    3
                </td>
                <td>
                    Overheads:
                </td>
                <td>
                    <asp:TextBox ID="txtoverheadsshell" Width="80px" CssClass="ShellTxtToCalculate" runat="server"></asp:TextBox>
                </td>
                <%--   <td>
                    <asp:TextBox ID="txtoverheadsFiller" Width="80px" CssClass="FillerTxtToCalculate"
                        runat="server"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>
                    4
                </td>
                <td>
                    Finance:
                </td>
                <td>
                    <asp:TextBox ID="txtfinanceshell" Width="80px" CssClass="ShellTxtToCalculate" runat="server"></asp:TextBox>
                </td>
                <%--  <td>
                    <asp:TextBox ID="txtFinanceFiller" Width="80px" CssClass="FillerTxtToCalculate" runat="server"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>
                    5
                </td>
                <td>
                    Commission:
                </td>
                <td>
                    <asp:TextBox ID="txtcommissionShell" Width="80px" CssClass="ShellTxtToCalculate"
                        runat="server"></asp:TextBox>
                </td>
                <%-- <td>
                    <asp:TextBox ID="txtcommissionFiller" Width="80px" CssClass="FillerTxtToCalculate"
                        runat="server"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <b>Total</b>
                </td>
                <td>
                    <asp:Label ID="lblTotalShell" runat="server"></asp:Label>
                </td>
                <%-- <td>
                    <asp:Label ID="lblTotalFiller" runat="server"></asp:Label>
                </td>--%>
            </tr>
        </table>
        <table>
            <tr style="display: none">
                <td>
                    <asp:Label ID="lblFiller" Text="Filler : " CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtFilleramt" Width="100px" Enabled="false" CssClass="textb" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblquote" Text="Quote : " CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtQuote" Width="100px" CssClass="textb" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblmessage" CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
