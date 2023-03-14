<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PackingCost.aspx.cs" EnableEventValidation="false"
    EnableViewState="true" EnableSessionState="True" Inherits="Masters_Carpet_PackingCost" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function RefreshCombo() {
            var PackingType = document.getElementById('TxtPackingType').value;
            if (PackingType == 1) {
                window.opener.document.getElementById('txtInnerPackingCost').value = document.getElementById('TxtAmount').value;

                self.close();
            }
            else if (PackingType == 2) {
                window.opener.document.getElementById('txtMiddlePackingCost').value = document.getElementById('TxtAmount').value;
                self.close();
            }
            else if (PackingType == 3) {
                window.opener.document.getElementById('txtMasterPackingCost').value = document.getElementById('TxtAmount').value;
                window.opener.document.getElementById('txtContainerPackingCost').value = document.getElementById('TxtFrtAmt').value;
                self.close();
            }
            else if (PackingType == 4) {
                window.opener.document.getElementById('txtOtherPackingCost').value = document.getElementById('TxtAmount').value;
                self.close();
            }
            else if (PackingType == 5) {
                window.opener.document.getElementById('txtContainerPackingCost').value = document.getElementById('TxtAmount').value;
                self.close();
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .style2
        {
            width: 72px;
        }
        .style3
        {
            width: 46px;
        }
    </style>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="750px">
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Unit
                </td>
                <td>
                    <asp:DropDownList ID="DDunit" runat="server" Width="80px" AutoPostBack="True" CssClass="dropdown">
                        <asp:ListItem Value="1">Cms</asp:ListItem>
                        <asp:ListItem Value="0">Inch</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td id="tdlblProdCode" align="center" class="tdstyle" visible="false" runat="server">
                    <asp:Label ID="lblProdCode" runat="server" Text="Prod Code"></asp:Label>
                </td>
                <td id="tdddProdCode" visible="false" runat="server">
                    <asp:DropDownList ID="ddProdCode" runat="server" Width="150px" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="TxtPackingType" runat="server" Height="0px" Width="0px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Length
                </td>
                <td>
                    <asp:TextBox ID="TxtLength" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtLength_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Width
                </td>
                <td class="style3">
                    <asp:TextBox ID="TxtWidth" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtWidth_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle" style="width: 80px">
                    Height
                </td>
                <td>
                    <asp:TextBox ID="TxtHeight" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtHeight_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Waste-1
                </td>
                <td>
                    <asp:TextBox ID="TxtWaste1" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtWaste1_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Waste-2
                </td>
                <td class="style3">
                    <asp:TextBox ID="TxtWaste2" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtWaste2_TextChanged"> </asp:TextBox>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblArea" runat="server" Text="lblArea"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Ply
                </td>
                <td>
                    <asp:TextBox ID="TxtPly" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtPly_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Craft
                </td>
                <td class="style3">
                    <asp:TextBox ID="TxtCraft" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtCraft_TextChanged"></asp:TextBox>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblGSMC1" runat="server" Text="lblGSMC1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    GSM-1
                </td>
                <td>
                    <asp:TextBox ID="TxtGSM1" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtGSM1_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    GSM-2
                </td>
                <td class="style3">
                    <asp:TextBox ID="TxtGSM2" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtGSM2_TextChanged"></asp:TextBox>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblGSMC2" runat="server" Text="lblGSMC2"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Rate-1
                </td>
                <td>
                    <asp:TextBox ID="TxtRate1" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtRate1_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Rate-2
                </td>
                <td class="style3">
                    <asp:TextBox ID="TxtRate2" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtRate2_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Pcs
                </td>
                <td>
                    <asp:TextBox ID="TxtPCS" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtPCS_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:TextBox ID="TxtFrtAmt" runat="server" Width="0px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="TxtAmount" runat="server" Width="0px"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    Net Cost
                </td>
                <td>
                    <asp:TextBox ID="TxtNetcost" runat="server"></asp:TextBox>
                </td>
                <td colspan="4">
                    <asp:Button ID="BtnCalculate" runat="server" Text="Calculate" OnClick="BtnCalculate_Click"
                        CssClass="buttonnorm" Width="20%" />
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClientClick="return confirm('Do You Want To Save Data?')"
                        OnClick="BtnSave_Click" CssClass="buttonnorm" Width="15%" />
                    <asp:Button ID="BtnClose" OnClientClick="RefreshCombo();" runat="server" Text="Close"
                        CssClass="buttonnorm" Width="15%" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" Visible="false" OnClientClick="return confirm('Do You Want To Delete Data?')"
                        OnClick="btnDelete_Click" CssClass="buttonnorm" Width="15%" />
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <asp:GridView ID="DG" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                        OnSelectedIndexChanged="DG_SelectedIndexChanged" OnRowDataBound="DG_RowDataBound"
                        CssClass="grid-view" OnRowCreated="DG_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="LENGTH" HeaderText="LENGTH">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WIDTH" HeaderText="WIDTH">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="HEIGHT" HeaderText="HEIGHT">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WASTE1" HeaderText="WASTE1">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WASTE2" HeaderText="WASTE2">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PLY" HeaderText="PLY">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CRAFT" HeaderText="CRAFT">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GSM1" HeaderText="GSM1">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GSM2" HeaderText="GSM2">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RATE1" HeaderText="RATE1">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RATE2" HeaderText="RATE2">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PCS" HeaderText="PCS">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NETCOST" HeaderText="NETCOST">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sr_No" HeaderText="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" Width="0px" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
