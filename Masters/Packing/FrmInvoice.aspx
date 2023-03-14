<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoice.aspx.cs" Inherits="Masters_Packing_FrmInvoice"
    EnableSessionState="True" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>INVOICE</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript">      
    </script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            $(':text').bind('keydown', function (e) {
                //on keydown for all textboxes

                if (e.target.className != "searchtextbox") {

                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;

            });
        });</script>
    <script type="text/javascript">


        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function PopUpTermsBankDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceTermsBankDetail.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }
        function PopUpOtherDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceOtherDetail.aspx?ID=' + varcode + '', '', 'width=700px,Height=400px,menubar=no');
        }
        function PopUpItemDetail() {
            var varcode = document.getElementById('TxtInvoiceId').value;
            window.open('FrmInvoiceItemDetail.aspx?ID=' + varcode + '', '', 'width=500px,Height=300px,menubar=no');
        }
        function PopUpRate() {
            var varcode = document.getElementById('TxtInvoiceId').value;
           
            window.open('FrmInvoiceRate.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }
        function PopUpRateNew() {
            var varcode = document.getElementById('TxtInvoiceId').value;

            window.open('FrmInvoiceRateNew.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }
        function RollsWeightDetail() {
            var varcode = document.getElementById('DDInvoiveNo').value;
            window.open('FrmRollWeight.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }
        function RollsWeightDetailSamara() {
            var varcode = document.getElementById('DDInvoiveNo').value;
            window.open('FrmRollWeightNew.aspx?ID=' + varcode + '', '', 'width=800px,Height=500px,menubar=no');
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 238px;
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
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em>
                    </strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                </i>
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
                                        <asp:CheckBox ID="ChkInvoice" runat="server" AutoPostBack="True" Text="Check Invoice"
                                            OnCheckedChanged="ChkInvoice_CheckedChanged" Font-Bold="True" CssClass="checkboxbold" />
                                        <br />
                                        <asp:Label ID="Label51" Text="Invoice Year" runat="server" CssClass="labelbold" />
                                        <br />
                                          <asp:DropDownList ID="DDInvoiceYear" runat="server" Width="120px" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="DDInvoiceYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="lbl" Text="Invoice No" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDInvoiveNo" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDInvoiveNo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RadioButton ID="RDUnToOrder" runat="server" Text="Unto Order" AutoPostBack="True"
                                            OnCheckedChanged="RDUnToOrder_CheckedChanged" Font-Bold="True" GroupName="m"
                                            CssClass="radiobuttonnormal" />
                                        <br />
                                        <asp:RadioButton ID="RDCustomer" runat="server" Text="Customer" AutoPostBack="True"
                                            OnCheckedChanged="RDCustomer_CheckedChanged" Font-Bold="True" GroupName="m" CssClass="radiobuttonnormal" />
                                        <br />
                                        <asp:RadioButton ID="RDBank" runat="server" Text="Bank" AutoPostBack="True" OnCheckedChanged="RDBank_CheckedChanged"
                                            Font-Bold="True" GroupName="m" CssClass="radiobuttonnormal" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label1" Text="Consignor" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtConsignor" runat="server" CssClass="textb" Width="250px" Height="100px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label2" Text="Consignee" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtConsignee" runat="server" Height="100px" Width="250px" CssClass="textb"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label3" Text=" Buyer(If Other Than Consignee)" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtBuyerOtherThanConsignee" Width="250px" runat="server" Height="100px"
                                            CssClass="textb" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label39" Text="Ship To Address" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtShipToAddress" Width="250px" runat="server" Height="100px" CssClass="textb"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label4" Text=" Currency" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDCurrency" runat="server" Width="175px" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Label ID="Label5" Text="Invoice Date" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtInvoiceId" runat="server" ForeColor="White" Height="0px" Width="0px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label6" Text=" Buyer Other Than Consignee" runat="server" CssClass="labelbold" />
                                        <asp:DropDownList ID="DDBuyerOtherThanConsignee" runat="server" Width="170px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDBuyerOtherThanConsignee_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2" id="TDExportIGST" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkExportIGST" class="tdstyle" runat="server" Text="Export Againt IGST"
                                            Font-Bold="True" />
                                        <asp:CheckBox ID="ChkExportLUT" class="tdstyle" runat="server" Text="Export Against LUT"
                                            Font-Bold="True" />
                                    </td>
                                    <td id="TDButtonShow" colspan="3" align="center" runat="server" visible="false">
                                        <%-- <asp:LinkButton ID="BtnTermsBankDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpTermsBankDetail()"
                 ForeColor="#0000CC">Terms & Bank Detail</asp:LinkButton>
             &nbsp;&nbsp;&nbsp;&nbsp;--%>
                                        <asp:LinkButton ID="BtnItemDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpItemDetail()"
                                            ForeColor="#0000CC">Item Detail</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnOtherDetail" runat="server" Font-Bold="True" OnClientClick=" return PopUpOtherDetail()"
                                            ForeColor="#0000CC">Goods Description</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnRate" runat="server" Font-Bold="True" OnClientClick=" return PopUpRate()"
                                            ForeColor="#0000CC" Visible="true">Rate</asp:LinkButton>
                                              <asp:LinkButton ID="BtnRateNew" runat="server" Font-Bold="True" OnClientClick=" return PopUpRateNew()"
                                            ForeColor="#0000CC" Visible="false">Rate</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnQualityWiseWeight" runat="server" Font-Bold="True" Visible="false"
                                            ForeColor="#0000CC">Quality Wise Weight</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="BtnShippingDtl" runat="server" Font-Bold="True" Visible="false"
                                            ForeColor="#0000CC">Shipping Details</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label7" Text="Shipping Agent" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDShippingAgent" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDShippingAgent"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label8" Text="Pre-Carriage" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDPreCarriage" runat="server" Width="175px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPreCarriage"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label9" Text="Receipt At" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDReceiptAt" runat="server" Width="175px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDReceiptAt"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label10" Text="By Air/Sea" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDByAirSea" runat="server" Width="170px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDByAirSea"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label11" Text=" Port Load" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:DropDownList ID="DDPortLoad" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDPortLoad"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label12" Text="Port DisCharge" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtPortDisCharge" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label13" Text=" Delivery" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <font color="red">
                                            *</font>
                                        <br />
                                        <asp:DropDownList ID="DDDelivery" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDDelivery"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle" >
                                        <asp:Label ID="Label14" Text="Terms" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDTerms" runat="server" Width="175px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDTerms"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle" id="TDDays" runat="server" visible="true">
                                        <asp:Label ID="Label15" Text="Days" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtDays" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDFreightTerms" runat="server" visible="true">
                                        <asp:Label ID="Label16" Text="Freight Terms" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtFreightTerms" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ChkBilling" class="tdstyle" runat="server" AutoPostBack="True"
                                            Text="From Billing Date" Font-Bold="True" />
                                        <br />
                                        <asp:CheckBox ID="ChkShipmentDays" class="tdstyle" runat="server" AutoPostBack="True"
                                            Text="After Shipment Date" Font-Bold="True" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label17" Text="Bank" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDBank" runat="server" CssClass="dropdown" Width="175px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDBank"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label18" Text="Roll Mark Head" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtRollMarkHead" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" runat="server" visible="false">
                                        <asp:Label ID="Label19" Text="Roll No" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtRollNo" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label20" Text=" Final Destination" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">*</font>
                                        <br />
                                        <asp:TextBox ID="TxtFinalDestination" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label21" Text=" Discount" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtDiscount" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label22" Text="Gross Weight" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtGrossWeight" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label23" Text="Net Weight" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtNetWeight" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label24" Text="C.B.M" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtcbm" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label25" Text="Pre. Advance" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtPreAdvance" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDAdvanceRec" runat="server" visible="true">
                                        <asp:Label ID="Label26" Text="Advance Rec" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtAdvanceRec" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDInsurance" runat="server" visible="true">
                                        <asp:Label ID="Label27" Text="Add Insurance" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtAddInsurance" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label28" Text=" Add Frieght" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtAddFrieght" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label29" Text=" Invoice Amt" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtInvoiceAmt" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label30" Text="Extra Charge Amt" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtExtraChargeAmt" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label Text="CGST(%)" ID="lblCGST" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtCgst" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="SGST(%)" ID="Label33" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtSgst" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="IGST(%)" ID="Label34" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtIgst" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="Currency Conversion Rate" ID="Label35" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtInrRate" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="Gstin Type" ID="Label37" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtGSTINType" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="End Use" ID="Label38" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtEndUse" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label31" Text="Documents Send Through" runat="server" CssClass="labelbold" />
                                    </td>
                                     <td>
                                        <asp:Label Text="Compisition" ID="Label50" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtComposition" runat="server" CssClass="textb" TextMode="MultiLine" Height="80px" Width="250px"></asp:TextBox>
                                    </td>
                                      <td>
                                        <asp:Label Text="GST Compensation Cess" ID="lblgstcess" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtgstcess" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdstyle">
                                        <asp:Label ID="Label32" Text="Extra Charge Remark" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="TxtExtraChargeRemark" CssClass="textboxremark" runat="server" Width="450px"
                                            TextMode="MultiLine" Height="75px"></asp:TextBox>
                                    </td>
                                    <td colspan="1" class="tdstyle">
                                        <asp:Label ID="Label40" Text="Discount Remark" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtDiscountRemark" CssClass="textboxremark" runat="server" Width="200px"
                                            TextMode="MultiLine" Height="75px"></asp:TextBox>
                                    </td>
                                    <td colspan="0">
                                        <asp:Label ID="Label36" Text="Goods Description" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDGoodsDescription" runat="server" CssClass="dropdown" Width="175px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDGoodsDescription_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDGoodsDescription"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="LblGoods" runat="server" CssClass="labelbold" Text="Description of Good"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtDescriptionofGood" CssClass="textboxremark" runat="server" Width="450px"
                                            Height="75px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr id="TRPackingCharges" runat="server" visible="false">
                                    <td>
                                        <asp:Label Text="SU Qty" ID="Label47" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtSUQty" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="DEUPA" ID="Label48" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtDeupa" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label Text="Pallet/Packing Charges" ID="Label49" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtPackingCharges" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    </tr>

                                <tr id="TRPileBase" runat="server" visible="false">
                                    <td colspan="1" class="tdstyle">
                                        <asp:Label ID="Label41" Text="Raw Material Pile" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtRawMaterialPile" CssClass="textboxremark" runat="server" Width="250px"
                                            TextMode="MultiLine" Height="75px"></asp:TextBox>
                                    </td>
                                    <td colspan="1" class="tdstyle">
                                        <asp:Label ID="Label42" Text="Raw Material Base" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtRawMaterialBase" CssClass="textboxremark" runat="server" Width="200px"
                                            TextMode="MultiLine" Height="75px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label43" Text=" Payment Term Custom" runat="server" CssClass="labelbold" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <font color="red">
                                            *</font>
                                        <br />
                                        <asp:DropDownList ID="DDPaymentTermCustom" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDPaymentTermCustom"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label44" Text="Country Of Final Destination" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtCountryOfFinalDestination" CssClass="textboxremark" runat="server"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label45" Text="Place Of Delivery" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtPlaceOfDelivery" CssClass="textboxremark" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label46" Text="WareHouse Name" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtWareHouseName" CssClass="textboxremark" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="LblErrorMessage" runat="server" Text="ErrorMessage" Visible="false"
                                            ForeColor="Red"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Button ID="BtnRollWt" runat="server" Text="Roll Weight" OnClick="BtnSave_Click"
                                            OnClientClick="return RollsWeightDetail();" CssClass="buttonnorm " Width="100px" />
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                                            CssClass="buttonnorm" />
                                        <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                            CssClass="buttonnorm" />
                                        <asp:Button ID="BtnRollWeightNew" runat="server" Text="Roll Weight" OnClientClick="return RollsWeightDetailSamara();"
                                            CssClass="buttonnorm " Width="100px" Visible="false" />
                                         <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" CssClass="buttonnorm" Visible="false" /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <font color="red">*&nbsp;&nbsp; Mandatory Fields</font>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                         <Triggers>                           
                            <asp:PostBackTrigger ControlID="BtnPreview" />
                        </Triggers>
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
