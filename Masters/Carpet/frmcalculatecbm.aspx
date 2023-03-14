<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmcalculatecbm.aspx.cs"
    Inherits="Masters_Carpet_frmcalculatecbm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calculate CBM</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
        function TextChanged() {
            var txtLength = getNum(parseFloat(document.getElementById('txtLength').value));
            var txtWidth = getNum(parseFloat(document.getElementById('txtWidth').value));
            var txtHeigth = getNum(parseFloat(document.getElementById('txtHeight').value));
            var txtpc_Box = getNum(parseFloat(document.getElementById('txtpcperbox').value));
            var txtcbm_pc = document.getElementById('txtcbm_pc');
            var txtrate_pc = document.getElementById('txtrate_pc');
            var txtrate_cbm = document.getElementById('txtrate_cbm');
            var txtboxprice = document.getElementById('txtboxprice');
            var txtbox_pc = document.getElementById('txtbox_pc');
            var txtboxply = getNum(parseFloat(document.getElementById('txtboxply').value));

            var CBM = (getNum(parseFloat(((txtLength * txtWidth * txtHeigth) / 1000000) / txtpc_Box))).toFixed(4);
            txtcbm_pc.value = CBM;
            //Rate/pc
            txtrate_pc.value = (getNum(parseFloat(((txtLength * txtWidth * txtHeigth) / 1000000) / txtpc_Box)) * getNum(parseFloat(txtrate_cbm.value))).toFixed(2);
            //Box price
            txtboxprice.value = (((((txtLength + txtWidth + 7.62) * (txtLength + txtHeigth)) / 6.4516) * 0.05) * (txtboxply / 9)).toFixed(2);
            //Box/pc
            txtbox_pc.value = (getNum(parseFloat(txtboxprice.value)) / getNum(parseFloat(txtpc_Box))).toFixed(2);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 70%">
        <table width="60%">
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblsize" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblboxsize" Text="Box Size" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtBoxsize" CssClass="textb" Width="70px" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblL" Text="L" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtLength" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblW" Text="W" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtWidth" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblH" Text="H" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtHeight" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblPcperbox" Text="Pc/Box" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtpcperbox" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblBoxply" Text="Calculation" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtboxply" CssClass="textb" Width="70px" Text="8.3" runat="server"
                        onchange="javascript:TextChanged();" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCBM_Pc" Text="CBM/Pc" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtcbm_pc" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblRate_cbm" Text="Rate/CBM" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtrate_cbm" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblRate_pc" Text="Rate/Pc" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtrate_pc" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblBoxprice" Text="Box Price" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtboxprice" CssClass="textb" Width="70px" runat="server" onchange="javascript:TextChanged();" />
                </td>
                <td>
                    <asp:Label ID="lblBox_pc" Text="Box/Pc" runat="server" CssClass="labelbold" /><br />
                    <asp:TextBox ID="txtbox_pc" CssClass="textb" Width="70px" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="6" align="right">
                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label ID="lblmessgae" ForeColor="Red" Text="" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
