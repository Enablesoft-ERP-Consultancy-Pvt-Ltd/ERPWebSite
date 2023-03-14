<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceDestini.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceDestini" EnableSessionState="True" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function PopUpTermsBankDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceTermsBankDetailDestini.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }
        function PopUpOtherDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceOtherDetailDestini.aspx?ID=' + varcode + '', '', 'width=1000px,Height=500px,menubar=no');
        }
        function PopUpItemDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceItemDetailDestini.aspx?ID=' + varcode + '', '', 'width=500px,Height=300px,menubar=no');
        }
        function PopUpRate() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceRateDestini.aspx?ID=' + varcode + '', '', 'width=900px,Height=500px,menubar=no');
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 238px;
        }
        .style2
        {
            width: 218px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                    <strong><em><i><font size="4" face="GEORGIA">
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td width="75%">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                            Text="Logout" OnClick="BtnLogout_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td height="inherit" valign="top" class="style1" colspan="2">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="ChkInvoice" runat="server" Text="Check Invoice" AutoPostBack="true"
                                            Font-Bold="True" OnCheckedChanged="ChkInvoice_CheckedChanged" />
                                        <br />
                                        Invoive No
                                        <br />
                                        <asp:DropDownList ID="DDInvoiveNo" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDInvoiveNo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDInvoiveNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                        <br />
                                        <asp:RadioButton ID="RDUnToOrder" runat="server" Text="Unto Order" AutoPostBack="True"
                                            OnCheckedChanged="RDUnToOrder_CheckedChanged" Font-Bold="True" />
                                        <br />
                                        <asp:RadioButton ID="RDCustomer" runat="server" Text="Customer" AutoPostBack="True"
                                            OnCheckedChanged="RDCustomer_CheckedChanged" Font-Bold="True" />
                                        <br />
                                        <asp:RadioButton ID="RDBank" runat="server" Text="Bank" AutoPostBack="True" OnCheckedChanged="RDBank_CheckedChanged"
                                            Font-Bold="True" />
                                    </td>
                                    <td class="tdstyle">
                                        Consignor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtConsignor" runat="server" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Consignee&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtConsignee" runat="server" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td colspan="2" class="tdstyle">
                                        Buyer(If Other Than Consignee)
                                        <br />
                                        <asp:TextBox ID="TxtBuyerOtherThanConsignee" runat="server" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Currency&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDCurrency" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <br />
                                        Invoice Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtInvoiceId" runat="server" ForeColor="White" Height="0px" Width="0px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdstyle">
                                        Buyer Other Than Consignee
                                        <asp:DropDownList ID="DDBuyerOtherThanConsignee" runat="server" Width="170px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDBuyerOtherThanConsignee_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDBuyerOtherThanConsignee"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDButtonShow" colspan="4" align="center" runat="server" visible="false">
                                        <asp:LinkButton ID="BtnTermsBankDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpTermsBankDetail()"
                                            ForeColor="#0000CC">Terms & Bank Detail</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnItemDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpItemDetail()"
                                            ForeColor="#0000CC">Item Detail</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnOtherDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpOtherDetail()"
                                            ForeColor="#0000CC">Other Detail</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnRate" runat="server" Font-Bold="True" OnClientClick=" return PopUpRate()"
                                            ForeColor="#0000CC">Rate</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnQualityWiseWeight" runat="server" Font-Bold="True" Visible="false"
                                            ForeColor="#0000CC">Quality Wise Weight</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Shipping Agent&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDShippingAgent" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDShippingAgent"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Pre-Carriage&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDPreCarriage" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDPreCarriage"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Receipt At&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDReceiptAt" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDReceiptAt"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        By Air/Sea&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDByAirSea" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDByAirSea"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Port Load&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:DropDownList ID="DDPortLoad" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDPortLoad"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Port DisCharge&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtPortDisCharge" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Shipping Bill No
                                        <br />
                                        <asp:TextBox ID="TxtShippingBillNo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Mark Head&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtRollMarkHead" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" runat="server" visible="false">
                                        Roll No
                                        <br />
                                        <asp:TextBox ID="TxtRollNo" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Final Destination&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*
                                        <br />
                                        <asp:TextBox ID="TxtFinalDestination" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Vessal Name
                                        <br />
                                        <asp:TextBox ID="TxtVessalName" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Gross Weight
                                        <br />
                                        <asp:TextBox ID="TxtGrossWeight" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Net Weight
                                        <br />
                                        <asp:TextBox ID="TxtNetWeight" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Shipping Bill Date
                                        <br />
                                        <asp:TextBox ID="TxtShippingBillDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtShippingBillDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Advance
                                        <br />
                                        <asp:TextBox ID="TxtPreAdvance" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" runat="server" visible="false">
                                        Advance Rec
                                        <br />
                                        <asp:TextBox ID="TxtAdvanceRec" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Add Insurance
                                        <br />
                                        <asp:TextBox ID="TxtAddInsurance" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Add Frieght
                                        <br />
                                        <asp:TextBox ID="TxtAddFrieght" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Invoice Amt
                                        <br />
                                        <asp:TextBox ID="TxtInvoiceAmt" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Extra Charge Amt
                                        <br />
                                        <asp:TextBox ID="TxtExtraChargeAmt" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="tdstyle">
                                        Extra Charge Remark
                                        <br />
                                        <asp:TextBox ID="TxtExtraChargeRemark" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="LblErrorMessage" runat="server" Text="ErrorMessage" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td colspan="3" align="right">
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                                            CssClass="buttonnorm" />
                                        <asp:Button ID="BtnDelete" runat="server" CssClass="buttonnorm" Visible="false" Text="Delete"
                                            OnClick="BtnDelete_Click" />
                                        <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                            Text="Close" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
