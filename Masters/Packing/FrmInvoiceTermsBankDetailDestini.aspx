<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceTermsBankDetailDestini.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceTermsBankDetailDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            //            window.location.href = "../../main.aspx";
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="70%">
                    <tr>
                        <td>
                            <asp:Label ID="LblTerms" runat="server" Text="Terms" Font-Bold="True" Enabled="false"
                                ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtInvoiceId" runat="server" BorderWidth="0px" ForeColor="White"
                                Height="0px" Width="0px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Delivery&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *
                            <br />
                            <asp:DropDownList ID="DDDelivery" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDDelivery"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Terms
                            <br />
                            <asp:DropDownList ID="DDTerms" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDTerms"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Days
                            <br />
                            <asp:TextBox ID="TxtDays" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Freight Terms
                            <br />
                            <asp:TextBox ID="TxtFreightTerms" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkBilling" runat="server" AutoPostBack="True" Text="From Billing Date"
                                Font-Bold="True" />
                            <br />
                            <asp:CheckBox ID="ChkShipmentDays" runat="server" AutoPostBack="True" Text="After Shipment Date"
                                Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:RadioButton ID="RDDeclaration1" runat="server" Text="Declaration1" AutoPostBack="True"
                                Font-Bold="True" OnCheckedChanged="RDDeclaration1_CheckedChanged" />
                            <br />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDDeclaration2" runat="server" Text="Declaration2" AutoPostBack="True"
                                Font-Bold="True" OnCheckedChanged="RDDeclaration2_CheckedChanged" />
                        </td>
                        <td colspan="2" class="tdstyle">
                            Declaration
                            <br />
                            <asp:TextBox ID="TxtDeclaration" runat="server" Height="100px" TextMode="MultiLine"
                                Width="250px"></asp:TextBox>
                        </td>
                        <td colspan="2" class="tdstyle">
                            Notify
                            <br />
                            <asp:TextBox ID="TxtNotify" runat="server" Height="100px" TextMode="MultiLine" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Label ID="LblBankDetail" class="tdstyle" runat="server" Text="Bank Detail" Font-Bold="True"
                                Enabled="false" ForeColor="#0000CC"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Gri No
                            <br />
                            <asp:TextBox ID="TxtGriNo" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Gsp No
                            <br />
                            <asp:TextBox ID="TxtGspNo" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            BL/AWB No
                            <br />
                            <asp:TextBox ID="TxtAWBNo" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            BL/AWB Date
                            <br />
                            <asp:TextBox ID="TxtBlAwbDate" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Ex Control No
                            <br />
                            <asp:TextBox ID="TxtExControlNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Ex Control Date
                            <br />
                            <asp:TextBox ID="TxtExControlDate" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtExControlDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            Drawback Amount
                            <br />
                            <asp:TextBox ID="TxtDrawBackAmount" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            LC No
                            <br />
                            <asp:TextBox ID="TxtLCNo" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            LC Date
                            <br />
                            <asp:TextBox ID="TxtLCDate" runat="server" Width="90px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtLCDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            Ins Policy
                            <br />
                            <asp:TextBox ID="txtInsPolicy" runat="server" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="ErrorMessage" ForeColor="Red"></asp:Label>
                        </td>
                        <td colspan="2" align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                                CssClass="buttonnorm" />
                            &nbsp;
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                Text="Close" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
