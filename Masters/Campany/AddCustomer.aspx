<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomer.aspx.cs" Inherits="Masters_Campany_AddCustomer"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <title>Customer</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validateSave() {
            var varcustomercode = document.getElementById('txtCustCode').value.toString;
            if (varcustomercode == "") {
                alert('Please select Country ...........');
            }
        }
        function CloseForm() {
            window.opener.document.getElementById('BtnRefereceCustomer').click();
            self.close();
        }
        function AddItum() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('CountryMaster.aspx', '', 'width=500px,Height=400px');
            }
        }
        function AddBank() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddBank.aspx', '', 'width=950px,Height=500px');
            }
        }
        function AddCurrency() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddCurrencies.aspx', '', 'width=701px,Height=501px');
            }
        }
        function AddPrecarriage() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddCarriage.aspx', '', 'width=601px,Height=401px');
            }
        }

        function Addcarriage() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddGoodReceipt.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }
        }
        function Addshiping() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShipping.aspx', '', 'width=901px,Height=401px');
            }
        }
        function Addair() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShipping.aspx', '', 'width=901px,Height=401px');
            }
        }
        function Addtransmode() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddTransMode.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }


        }
        function addgoodreceipt() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('addgood.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }

        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
        function addbyingagent() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddBuyingAgent.aspx', '', 'width=901px,Height=501px', 'resizeable=yes');
            }
        }
        function AddDeliveryTerms() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddTerm.aspx', '', 'width=901px,Height=501px', 'resizeable=yes');
            }
        }
        function AddPaymentMode() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddPaymentDetail.aspx', '', 'width=901px,Height=501px', 'resizeable=yes');
            }
        }

    </script>
</head>
<body>
    <form id="CompInfo" runat="server">
    <%--Page Design--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="1">
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtid" runat="server" CssClass="textb" Visible="False"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Country <b style="color: Red">*</b><asp:Button ID="country" runat="server" BackColor="White"
                                                        ForeColor="White" Height="1px" OnClick="country_Click" Width="1px" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddCountry" runat="server" CssClass="dropdown" Width="102px"
                                                        OnSelectedIndexChanged="ddCountry_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCountry"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Button2" runat="server" Text="...." OnClientClick="return AddItum();"
                                                        Style="margin-top: 0px" CssClass="buttonsmall" />
                                                </td>
                                                <td class="tdstyle">
                                                    Customer Company Name<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="textb" text-transform="uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" runat="server"
                                                        ErrorMessage="please Enter CompanyName" ControlToValidate="txtcompName" ValidationGroup="f1"
                                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Customer Code<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="textb" text-transform="uppercase"
                                                        ForeColor="Black"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                                        ErrorMessage="please Enter CustomerCode" ControlToValidate="txtcustcode" ValidationGroup="f1"
                                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td id="pincode" runat="server" visible="false" class="tdstyle">
                                                    Pin Code
                                                </td>
                                                <td id="pincode1" runat="server" visible="false">
                                                    <asp:TextBox ID="txtPin" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Concern Person
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Phone No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="3" class="tdstyle">
                                                    Customer Address<b style="color: Red">*</b>
                                                </td>
                                                <td rowspan="3">
                                                    <asp:TextBox ID="txtCustAddress" runat="server" TextMode="MultiLine" CssClass="textb"
                                                        Height="55px" Width="158px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" runat="server"
                                                        ControlToValidate="txtCustAddress" ErrorMessage="please Enter Customer Address"
                                                        ForeColor="Red" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td class="tdstyle">
                                                    Mobile No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMob" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Fax No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="textb"></asp:TextBox>
                                                    <%--<asp:CompareValidator ID="CompareValidator3" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txtFax" Text="Text must be a number." ForeColor="Red" SetFocusOnError="true" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Tin No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTinNo" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    E-Mail
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server" Width="350px"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Final Destination<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinelDest" runat="server" CssClass="textb"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" runat="server"
                                                        ControlToValidate="txtFinelDest" ErrorMessage="please Enter Final Destination"
                                                        ForeColor="Red" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Mark
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMark" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Sea Port<b style="color: Red"></b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSeaPort" runat="server" CssClass="textb"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                     ControlToValidate="txtSeaPort" ErrorMessage="please Enter Sea Port" 
                                     ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Currency <b style="color: Red">*</b><asp:Button ID="currency" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="currency_Click" Width="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="dropdown" ViewStateMode="Enabled"
                                                        Width="102px">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddlCurrency"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Button4" runat="server" CssClass="buttonsmall" OnClientClick="return AddCurrency();"
                                                        Text="...." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlCurrency"
                                                        ErrorMessage="please Enter Currency" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlCurrency"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Air Port<b style="color: Red"></b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAirPort" runat="server" CssClass="textb"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                     ControlToValidate="txtAirPort" ErrorMessage="please Enter AirPort" 
                                     ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Bank <b style="color: Red">*</b><asp:Button ID="Button3" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="Button3_Click" Text="Button" Width="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown" Width="102px">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddlBank"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="btnAddNew0" runat="server" CssClass="buttonsmall" OnClientClick=" return AddBank();"
                                                        Text="...." />
                                                    <%-- <cc1:ModalPopupExtender ID="btnAddNew0_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="modalBackground" CancelControlID="btnCancel" Drag="true" 
                                OkControlID="btnOkay" OnOkScript="okay()" PopupControlID="Panel1" 
                                TargetControlID="btnAddNew0">
                            </cc1:ModalPopupExtender>--%>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBank"
                                                        ErrorMessage="please Enter Bank" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlBank"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    A/C No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAccNo" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Receipt At By Pre<asp:Button ID="cariage" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" OnClick="cariage_Click"
                                                        Width="1px" />
                                                    Carrier<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlReceiptPreCar" runat="server" CssClass="dropdown" Width="102px">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddlReceiptPreCar"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Button5" runat="server" CssClass="buttonsmall" OnClientClick="return Addcarriage();"
                                                        Text="...." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlReceiptPreCar"
                                                        ErrorMessage="please Enter Receipt At By Pre Carrier" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlReceiptPreCar"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    By Air/Sea<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlByAirSea" runat="server" CssClass="dropdown" Width="102px">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddlByAirSea"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Air" runat="server" CssClass="buttonsmall" OnClientClick="return Addtransmode();"
                                                        Text="...." />
                                                    <asp:Button ID="transmode" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="transmode_Click"
                                                        Text="." Width="1px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlByAirSea"
                                                        ErrorMessage="please Enter ByAirSea" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddlByAirSea"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Pre Carriage By<b style="color: Red">*</b><asp:Button ID="PreCarriageBy" runat="server"
                                                        BackColor="White" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                                        ForeColor="White" Height="1px" OnClick="PreCarriageBy_Click" Width="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPreCarr" runat="server" CssClass="dropdown" Width="102px">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddlPreCarr"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Button8" runat="server" CssClass="buttonsmall" OnClientClick="return AddPrecarriage();"
                                                        Text="...." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlPreCarr"
                                                        ErrorMessage="please Enter Pre Carriage By" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlPreCarr"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Port Of Loading<b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPortOfLoading" runat="server" CssClass="dropdown" Width="102px">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="notifybyair" runat="server" CssClass="buttonsmall" OnClientClick="return addgoodreceipt();"
                                                        Text="...." />
                                                    <asp:Button ID="addgood" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="Button7_Click1"
                                                        Width="1px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlPortOfLoading"
                                                        ErrorMessage="please Enter PortOfLoading" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlPortOfLoading"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Buyer Other Than Consignee Air
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtByerOthCons" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Notify By Air
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotifyByAir" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Buyer Other Than Consignee Sea
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtByerOtherThanConsSea" runat="server" CssClass="textb" Height="50px"
                                                        TextMode="MultiLine" Width="350px"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Notify By Sea
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotifyBySea" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Shipping Agent
                                                    <asp:Button ID="shipping0" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="shipping0_Click"
                                                        Text="Button" Width="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddShipping" runat="server" Width="102px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button6" runat="server" CssClass="buttonsmall" OnClientClick="return Addshiping(); "
                                                        Text="...." />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddShipping"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Rate Type
                                                    <asp:Button ID="BtnRefDeliveryTerms" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" Text="Button"
                                                        Width="1px" OnClick="BtnRefDeliveryTerms_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddDeliveryTerms" runat="server" Width="102px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="BtnAddDeliveryTerms" runat="server" CssClass="buttonsmall"
                                                        OnClientClick="return AddDeliveryTerms(); " Text="...." />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddDeliveryTerms"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Buying Agent
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddBuyingAgent" runat="server" Width="102px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button7" runat="server" CssClass="buttonsmall" OnClientClick="return addbyingagent();"
                                                        Text="...." />
                                                    &nbsp;<asp:Button ID="addbying" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" OnClick="addbying_Click"
                                                        Width="1px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddBuyingAgent"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Payment Terms
                                                    <asp:Button ID="BtnRefPaymentMode" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" Text="Button"
                                                        Width="1px" OnClick="BtnRefPaymentMode_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddPaymentMode" runat="server" Width="102px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender17" runat="server" TargetControlID="ddPaymentMode"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    &nbsp;<asp:Button ID="BtnAddPaymentMode" runat="server" CssClass="buttonsmall" OnClientClick="return AddPaymentMode(); "
                                                        Text="...." />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddPaymentMode"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    &nbsp;
                                                    <asp:Button ID="cmdCancel" runat="server" CssClass="buttonnorm" OnClick="cmdCancel_Click"
                                                        Text="New" ValidationGroup="f1" />
                                                    <asp:Button ID="Rpt" runat="server" OnClick="Rpt_Click" OnClientClick="return priview();"
                                                        Text="Preview" CssClass="buttonnorm" />
                                                    <asp:Button ID="cmdSave" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                                        OnClick="cmdSave_Click" Text="Save" ValidationGroup="f1" />
                                                    <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();"
                                                        Text="Close" />
                                                    <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                                                        OnClientClick="return confirm('Do you want to Delete data?')" Text="Delete" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="RED" ShowMessageBox="true"
                                                        ValidationGroup="f1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblErr" runat="server" CssClass="errormsg" ForeColor="Red"></asp:Label>
                                                    <div style="width: 1000px; height: 200px; overflow: scroll">
                                                        <asp:GridView ID="GvCustomer" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            CellPadding="4" DataKeyNames="Customerid" ForeColor="#333333" OnPageIndexChanging="GvCustomer_PageIndexChanging"
                                                            OnRowDataBound="GvCustomer_RowDataBound" OnSelectedIndexChanged="GvCustomer_SelectedIndexChanged"
                                                            PageSize="4" CssClass="grid-view" OnRowCreated="GvCustomer_RowCreated">
                                                            <HeaderStyle CssClass="gvheader" />
                                                            <AlternatingRowStyle CssClass="gvalt" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            <Columns>
                                                                <asp:BoundField DataField="CustomerId" HeaderText="Sr.No." />
                                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                                <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" />
                                                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                                                <asp:BoundField DataField="BuyerOtherThanConsigneeAir" HeaderText="BuyerOtherThanConsigneeAir" />
                                                                <asp:BoundField DataField="BuerOtherThanConsigneeSea" HeaderText="BuerOtherThanConsigneeSea" />
                                                                <asp:BoundField DataField="Mark" HeaderText="Mark" />
                                                                <asp:BoundField DataField="CountryName" HeaderText="CountryName" />
                                                                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                                                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                                                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                                                <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                                                <asp:BoundField DataField="PinCode" HeaderText="PinCode" />
                                                                <asp:BoundField DataField="BankName" HeaderText="BankName" />
                                                                <asp:BoundField DataField="CurrencyName" HeaderText="CurrencyName" />
                                                                <asp:BoundField DataField="DestinationPlace" HeaderText="DestinationPlace" />
                                                                <asp:BoundField DataField="CarriageName" HeaderText="CarriageName" />
                                                                <asp:BoundField DataField="transmodeName" HeaderText="transmodeName" />
                                                                <asp:BoundField DataField="StationName" HeaderText="StationName" />
                                                                <asp:BoundField DataField="AirPort" HeaderText="AirPort" />
                                                                <asp:BoundField DataField="AcNo" HeaderText="AcNo" />
                                                                <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" />
                                                                <asp:BoundField DataField="TinNo" HeaderText="TinNo" />
                                                                <asp:BoundField DataField="AgentName" HeaderText="AgentName" />
                                                                <asp:BoundField DataField="BuyeingAgentName" HeaderText="BuyeingAgentName" />
                                                                <asp:BoundField DataField="PaymentName" HeaderText="PaymentName" />
                                                                <asp:BoundField DataField="TermName" HeaderText="TermName" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:CheckBox ID="Chkvalidate" runat="server" AutoPostBack="True" OnCheckedChanged="Chkvalidate_CheckedChanged" />
                                                    <br />
                                                    Buyer's Label List
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <div id="ch" runat="server" visible="false">
                                                        <asp:GridView ID="Gvchklist" runat="server" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="Chkbox" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="category_name" HeaderText="categoryname" />
                                                                <asp:BoundField DataField="item_name" HeaderText="itemname" />
                                                                <asp:BoundField DataField="item_id" HeaderText="Sr.No" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
