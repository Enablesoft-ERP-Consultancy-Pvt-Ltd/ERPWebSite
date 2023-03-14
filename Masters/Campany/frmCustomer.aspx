<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCustomer.aspx.cs" Inherits="Masters_Campany_frmCustomer"
    EnableEventValidation="false" Title="CUSTOMER MASTER" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('BtnRefereceCustomer')) {
                window.opener.document.getElementById('BtnRefereceCustomer').click();
                self.close();
            }
        }
    </script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmCustomer.aspx";
        }
        function validateSave() {
            var message = "";

            var hnmastercompanyid = document.getElementById('<%= hnmastercompanyid.ClientID %>');
            if (hnmastercompanyid.value == "9") {
                var continent = document.getElementById('<%= ddContinent.ClientID %>');
                var Subcontinent = document.getElementById('<%= DDSubcontinent.ClientID %>');
                var Country = document.getElementById('<%= ddCountry.ClientID %>');
                var State = document.getElementById('<%= DDState.ClientID %>');
                if (continent.value <= 0) {
                    message = message + "Please select Continent\n";
                }
                if (Subcontinent.value <= 0) {
                    message = message + "Please select SubContinent\n";
                }
                if (Country.value <= 0) {
                    message = message + "Please select Country\n";
                }
                if (State.value <= 0) {
                    message = message + "Please select State\n";
                }
            }
            if (message != "") {
                alert(message);
                return false;
            }
            else {
                confirm('Do you want to save Data?');
            }
        }

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddItum() {
            window.open('CountryMaster.aspx', '', 'width=500px,Height=400px');
        }
        function AddItum1() {
            window.open('ContinentMaster.aspx', '', 'width=500px,Height=400px');
        }
        function Addcustomerbank() {
            var a = document.getElementById('CPH_Form_txtid').value;

            if (a == "" || a == "0") {
                alert('Plz Select or Insert customer');
                return false;
            }
            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (300 / 2);

            //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
            window.open('Customer_Vender_Dyerbank.aspx?a=' + a + '&b=1', 'ADD Customer Bank', 'width=500px, height=300px, top=' + top + ', left=' + left);
        }
        function AddcustomerType() {

            var left = (screen.width / 2) - (350 / 2);
            var top = (screen.height / 2) - (350 / 2);

            //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
            window.open('AddCustomerType.aspx', 'ADD Customer Type', 'width=350px, height=350px, top=' + top + ', left=' + left);
        }
        function Addcourier() {
            var a = document.getElementById('CPH_Form_txtid').value;

            if (a == "" || a == "0") {
                alert('Plz Select or Insert customer');
                return false;
            }
            window.open('AddCustomerCourier.aspx?a=' + a, '', 'width=500px,Height=400px');
        }
        function AddSubcontinent() {
            var selectedIndex = document.getElementById('CPH_Form_ddContinent').selectedIndex;
            if (selectedIndex == 0) {
                alert("Plz Select Continent First...");
                return false;
            }

            var a = document.getElementById('CPH_Form_ddContinent');

            var id = a.options[a.selectedIndex].value;
            window.open('frmSubcontinentmaster.aspx?a=' + id, '', 'width=500px,Height=400px');
        }

        function AddState() {
            var selectedIndex = document.getElementById('CPH_Form_ddCountry').selectedIndex;
            if (selectedIndex == 0) {
                alert("Plz Select Country First...");
                return false;
            }
            var a = document.getElementById('CPH_Form_ddCountry');

            var id = a.options[a.selectedIndex].value;
            window.open('frmstatemaster.aspx?a=' + id, '', 'width=500px,Height=400px');
        }

        function Addseaport() {
            var selectedIndex = document.getElementById('CPH_Form_ddCountry').selectedIndex;
            if (selectedIndex == 0) {
                alert("Plz Select Country First...");
                return false;
            }
            var a = document.getElementById('CPH_Form_ddCountry');

            var id = a.options[a.selectedIndex].value;
            window.open('AddSeaport.aspx?a=' + id, '', 'width=500px,Height=400px');
        }

        function AddAirPort() {
            var selectedIndex = document.getElementById('CPH_Form_ddCountry').selectedIndex;
            if (selectedIndex == 0) {
                alert("Plz Select Country First...");
                return false;
            }
            var a = document.getElementById('CPH_Form_ddCountry');

            var id = a.options[a.selectedIndex].value;
            window.open('AddAirport.aspx?a=' + id, '', 'width=500px,Height=400px');
        }

        function AddBank() {
            window.open('AddBank.aspx', '', 'width=950px,Height=500px');
        }

        function AddCurrency() {
            window.open('AddCurrencies.aspx', '', 'width=701px,Height=501px');
        }

        function AddPrecarriage() {
            window.open('AddCarriage.aspx', '', 'width=601px,Height=401px');
        }

        function Addcarriage() {
            window.open('AddGoodReceipt.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
        }

        function Addshiping() {
            window.open('AddShipping.aspx', '', 'width=900px,Height=401px');
        }

        function Addair() {
            window.open('AddShipping.aspx', '', 'width=901px,Height=401px');
        }

        function Addtransmode() {
            window.open('AddTransMode.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
        }

        function addgoodreceipt() {
            window.open('addgood.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
        }

        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }

        function addbyingagent() {
            window.open('AddBuyingAgent.aspx', '', 'width=901px,Height=501px', 'resizeable=yes');
        }

        function AddDeliveryTerms() {
            window.open('AddTerm.aspx', '', 'width=501px,Height=401px', 'resizeable=yes');
        }

        function AddPaymentMode() {
            window.open('AddPaymentDetail.aspx', '', 'width=501px,Height=401px', 'resizeable=yes');
        }

        function AddShippingAgency() {

            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var left = (screen.width / 2) - (750 / 2);
                var top = (screen.height / 2) - (500 / 2);

                window.open('frmShippingAgency.aspx', 'ADD Shipping Agency', 'width=740px, height=400px, top=' + top + ', left=' + left);
            }
        }

        function Addbuyinghouse() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var left = (screen.width / 2) - (800 / 2);
                var top = (screen.height / 2) - (500 / 2);
                //window.open('AddBuyingHouse.aspx', '', 'width=950px,Height=500px');
                window.open('AddBuyingHouse.aspx', 'ADD BUYING HOUSE', 'width=900px, height=400px, top=' + top + ', left=' + left);
            }
        }

    </script>
    <%--Page Design--%>
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
                                                <td class="tdstyle">
                                                    <asp:Label Text="Customer Company Name" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="textb" Width="300px" text-transform="uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" runat="server"
                                                        ErrorMessage="please Enter CompanyName" ControlToValidate="txtcompName" ValidationGroup="f1"
                                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label1" Text="Continent" runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="Continent" runat="server" BackColor="White" ForeColor="White" Height="1px"
                                                        Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" OnClick="Continent_Click"
                                                        CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddContinent" runat="server" CssClass="dropdown" Width="150px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddContinent_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="btnaddcontinent" runat="server" Text="&#43;" Width="35px" CssClass="buttonsmalls"
                                                        OnClientClick="return AddItum1();" Style="margin-top: 0px" ToolTip="Click For Add New Continent" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddContinent"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label2" Text="Country " runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="country" runat="server" BackColor="White" ForeColor="White" Height="1px"
                                                        OnClick="country_Click" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                                        CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddCountry" runat="server" CssClass="dropdown" Width="150px"
                                                        OnSelectedIndexChanged="ddCountry_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button2" runat="server" Text="&#43;" Width="35px" CssClass="buttonsmalls"
                                                        OnClientClick="return AddItum();" Style="margin-top: 0px" ToolTip="Click For Add New Country" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCountry"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label3" Text="Customer Code " runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="textb" text-transform="uppercase"
                                                        ForeColor="Black" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                                        ErrorMessage="please Enter CustomerCode" ControlToValidate="txtcustcode" ValidationGroup="f1"
                                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label4" Text=" Subcontinent" runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="Subcontinent" runat="server" BackColor="White" ForeColor="White"
                                                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                                        OnClick="Subcontinent_Click" CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDSubcontinent" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                        Width="150px" OnSelectedIndexChanged="DDSubcontinent_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button11" runat="server" Text="&#43;" Width="35px" CssClass="buttonsmalls"
                                                        OnClientClick="return AddSubcontinent();" Style="margin-top: 0px" ToolTip="Click For Add New Subcontinent" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="DDSubcontinent"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label5" Text="  State" runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="State" runat="server" BackColor="White" ForeColor="White" Height="1px"
                                                        Width="1px" BorderColor="White" BorderWidth="0px" ToolTip="Click For Add New State"
                                                        EnableTheming="true" OnClick="State_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDState" runat="server" CssClass="dropdown" Width="150px" OnSelectedIndexChanged="DDState_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button9" runat="server" Text="&#43;" Width="35px" CssClass="buttonsmalls"
                                                        OnClientClick="return AddState();" Style="margin-top: 0px" ToolTip="Click For Add New State" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDState"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label6" Text=" Concern Person" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustName" runat="server" Width="300px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label7" Text=" Phone No." runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td id="pincode" runat="server" visible="false" class="tdstyle">
                                                    <asp:Label ID="Label8" Text="Pin Code" runat="server" Font-Bold="true" />
                                                </td>
                                                <td id="pincode1" runat="server" visible="false">
                                                    <asp:TextBox ID="txtPin" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="3" class="tdstyle">
                                                    <asp:Label ID="Label9" Text=" Customer Address1" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td rowspan="3">
                                                    <asp:TextBox ID="txtCustAddress" runat="server" TextMode="MultiLine" CssClass="textb"
                                                        Height="55px" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" runat="server"
                                                        ControlToValidate="txtCustAddress" ErrorMessage="please Enter Customer Address"
                                                        ForeColor="Red" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label10" Text=" Address2" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustAdd2" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label11" Text=" Address3" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustAddr3" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label12" Text="Fax No." runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    <%--<asp:CompareValidator ID="CompareValidator3" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txtFax" Text="Text must be a number." ForeColor="Red" SetFocusOnError="true" />--%>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label13" Text="Mobile No." runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMob" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label14" Text="Tin No" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTinNo" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label15" Text="E-Mail" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" CssClass="textb" Style="text-transform: none"
                                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label16" Text="Final" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinelDest" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" runat="server"
                                                        ControlToValidate="txtFinelDest" ErrorMessage="please Enter Final Destination"
                                                        ForeColor="Red" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label17" Text="Mark" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMark" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label18" Text="Sea Port" runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="seaport2" runat="server" BackColor="White" ForeColor="White" Height="1px"
                                                        Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" OnClick="seaport2_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDSeaport" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Seaport" runat="server" CssClass="buttonsmalls" OnClientClick="return Addseaport();"
                                                        Text="&#43;" Width="35px" />
                                                    <cc1:ListSearchExtender ID="seaport1" runat="server" TargetControlID="DDSeaport"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                     ControlToValidate="txtSeaPort" ErrorMessage="please Enter Sea Port" 
                                     ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label19" Text="Currency" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b><asp:Button ID="currency" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="currency_Click" Width="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="dropdown" ViewStateMode="Enabled"
                                                        Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button4" runat="server" CssClass="buttonsmalls" OnClientClick="return AddCurrency();"
                                                        Text="&#43;" Width="35px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlCurrency"
                                                        ErrorMessage="please Enter Currency" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlCurrency"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label20" Text="Air Port" runat="server" Font-Bold="true" />
                                                    <b style="color: Red"></b>
                                                    <asp:Button ID="Airport2" runat="server" BackColor="White" ForeColor="White" Height="1px"
                                                        Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" OnClick="Airport2_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDAirport" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Airport" runat="server" CssClass="buttonsmalls" OnClientClick="return AddAirPort();"
                                                        Text="&#43;" Width="35px" />
                                                    <cc1:ListSearchExtender ID="Airport1" runat="server" TargetControlID="DDAirport"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                     ControlToValidate="txtAirPort" ErrorMessage="please Enter AirPort" 
                                     ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr id="TRBankAndAcNo" runat="server">
                                                <td class="tdstyle" runat="server" id="TDBankLabel">
                                                    <asp:Label ID="Label21" Text=" Bank " runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b><asp:Button ID="Button3" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="Button3_Click" Text="Button" Width="1px" CssClass="buttonsmalls" />
                                                </td>
                                                <td id="TDBank" runat="server">
                                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="btnAddNew0" runat="server" CssClass="buttonsmalls" OnClientClick=" return AddBank();"
                                                        Text="&#43;" Width="35px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlBank"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <%-- <cc1:ModalPopupExtender ID="btnAddNew0_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="modalBackground" CancelControlID="btnCancel" Drag="true" 
                                OkControlID="btnOkay" OnOkScript="okay()" PopupControlID="Panel1" 
                                TargetControlID="btnAddNew0">
                            </cc1:ModalPopupExtender>--%>
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBank"
                                                        ErrorMessage="please Enter Bank" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                    <td class="tdstyle" runat="server" visible="false">
                                                        <asp:Label ID="Label22" Text="A/C No." runat="server" Font-Bold="true" />
                                                    </td>
                                                    <td runat="server" visible="false">
                                                        <asp:TextBox ID="txtAccNo" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle" id="TDlblReceiptAtPreCarrier" runat="server">
                                                    <asp:Label ID="Label23" Text="Receipt At By Pre Carrier" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b><asp:Button ID="cariage" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="cariage_Click" Width="1px" />
                                                </td>
                                                <td id="TDddlReceiptPreCar" runat="server">
                                                    <asp:DropDownList ID="ddlReceiptPreCar" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button5" runat="server" CssClass="buttonsmalls" OnClientClick="return Addcarriage();"
                                                        Text="&#43;" Width="35px" />
                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlReceiptPreCar"
                                                        ErrorMessage="please Enter Receipt At By Pre Carrier" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlReceiptPreCar"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label24" Text="By Air/Sea" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlByAirSea" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Air" runat="server" CssClass="buttonsmalls" OnClientClick="return Addtransmode();"
                                                        Text="&#43;" Width="35px" />
                                                    <asp:Button ID="transmode" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="transmode_Click"
                                                        Text="." Width="1px" CssClass="buttonsmalls" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlByAirSea"
                                                        ErrorMessage="please Enter ByAirSea" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlByAirSea"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle" id="TDlblPreCarrierBy" runat="server">
                                                    <asp:Label ID="Label25" Text="Pre Carriage By" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b><asp:Button ID="PreCarriageBy" runat="server" BackColor="White"
                                                        BorderColor="White" BorderWidth="0px" EnableTheming="true" ForeColor="White"
                                                        Height="1px" OnClick="PreCarriageBy_Click" Width="1px" />
                                                </td>
                                                <td id="TDddlPreCarr" runat="server">
                                                    <asp:DropDownList ID="ddlPreCarr" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button8" runat="server" CssClass="buttonsmalls" OnClientClick="return AddPrecarriage();"
                                                        Text="&#43;" Width="35px" />
                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlPreCarr"
                                                        ErrorMessage="please Enter Pre Carriage By" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlPreCarr"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label26" Text="Port Of Loading" runat="server" Font-Bold="true" />
                                                    <b style="color: Red">*</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPortOfLoading" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="notifybyair" runat="server" CssClass="buttonsmalls" OnClientClick="return addgoodreceipt();"
                                                        Text="&#43;" Width="35px" />
                                                    <asp:Button ID="addgood" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="Button7_Click1"
                                                        Width="1px" CssClass="buttonsmalls" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlPortOfLoading"
                                                        ErrorMessage="please Enter PortOfLoading" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddlPortOfLoading"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="lblOtherThanConsigneeAir" runat="server" Text="Buyer Other Than Consignee Address(Air)"
                                                        Font-Bold="true"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtByerOthCons" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label27" Text="Notify By Air" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotifyByAir" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label45" runat="server" Text="Consignee Country" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConsigneeCountry" runat="server" CssClass="textb" Width="200px"
                                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label46" runat="server" Text="Consignee State" Font-Bold="true"></asp:Label>
                                                    <asp:TextBox ID="txtConsigneeState" runat="server" CssClass="textb" Width="100px"
                                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    <asp:Label ID="Label47" runat="server" Text="Consignee StateCode" Font-Bold="true"></asp:Label>
                                                    <asp:TextBox ID="txtConsigneeStateCode" runat="server" CssClass="textb" Width="100px"
                                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle" id="TDlblBuyerOtherThanConsigneeSea" runat="server">
                                                    <asp:Label ID="Label28" Text="Buyer Other Than Consignee Address(Sea)" runat="server"
                                                        Font-Bold="true" />
                                                </td>
                                                <td id="TDTxtByerOtherThanConsSea" runat="server">
                                                    <asp:TextBox ID="TxtByerOtherThanConsSea" runat="server" CssClass="textb" Height="50px"
                                                        TextMode="MultiLine" Width="350px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label29" Text="Notify By Sea" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotifyBySea" runat="server" CssClass="textb" Height="50px" TextMode="MultiLine"
                                                        Width="350px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label30" Text=" Sample Delivery Address" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSampleAddress" runat="server" CssClass="textb" Width="350px"
                                                        TextMode="MultiLine" Height="50px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label31" Text="  Priority" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddPriority" runat="server" CssClass="dropdown" Width="80px">
                                                        <asp:ListItem Value="1">HIGH</asp:ListItem>
                                                        <asp:ListItem Value="2">MEDIUM</asp:ListItem>
                                                        <asp:ListItem Value="3">LOW</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label32" Text=" Type Of Customer" runat="server" Font-Bold="true" />
                                                    <asp:DropDownList ID="ddTypeofCustomer" runat="server" CssClass="dropdown" Width="100px">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="BtnAddCustomerType" runat="server" CssClass="buttonsmalls" OnClientClick="return AddcustomerType(); "
                                                        Text="&#43;" Width="35px" Height="20px" />
                                                    <asp:Button ID="btnCustomerType" runat="server" BackColor="White" ForeColor="White"
                                                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                                        OnClick="btnCustomerType_Click" CssClass="buttonsmalls" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label33" Text=" Delivery Terms" runat="server" Font-Bold="true" />
                                                    <asp:Button ID="BtnRefDeliveryTerms" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" Text="Button"
                                                        Width="1px" OnClick="BtnRefDeliveryTerms_Click" CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddDeliveryTerms" runat="server" Width="150px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="BtnAddDeliveryTerms" runat="server" CssClass="buttonsmalls"
                                                        OnClientClick="return AddDeliveryTerms(); " Text="&#43;" Width="35px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddDeliveryTerms"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label34" Text=" Payment_Terms Bank" runat="server" Font-Bold="true" />
                                                    <asp:Button ID="BtnRefPaymentMode" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" Text="Button"
                                                        Width="1px" OnClick="BtnRefPaymentMode_Click" CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddPaymentMode" runat="server" Width="150px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="BtnAddPaymentMode" runat="server" CssClass="buttonsmalls" OnClientClick="return AddPaymentMode(); "
                                                        Text="&#43;" Width="35px" />&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtid" runat="server" CssClass="textb" Style="display: none"></asp:TextBox>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddPaymentMode"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label35" Text=" Shipping Agency" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDShippingAgency" runat="server" CssClass="dropdown" Width="150px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDShippingAgency_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="btnaddagency" runat="server" Text="&#43;" CssClass="buttonsmalls"
                                                        OnClientClick="return AddShippingAgency();" Style="margin-top: 0px" ToolTip="Click For Add New Agency"
                                                        Width="30px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="DDShippingAgency"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="btnAgencyCloseFormCustomer" runat="server" BackColor="White" ForeColor="White"
                                                        Height="1px" Width="0px" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                                        OnClick="btnAgencyCloseFormCustomer_Click" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label48" Text=" Payment_Terms Custom" runat="server" Font-Bold="true" />
                                                    <asp:Button ID="BtnRefPaymentModeCustom" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" Text="Button"
                                                        Width="1px" OnClick="BtnRefPaymentModeCustom_Click" CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddPaymentModeCustom" runat="server" Width="150px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="BtnAddPaymentModeCustom" runat="server" CssClass="buttonsmalls"
                                                        OnClientClick="return AddPaymentMode(); " Text="&#43;" Width="35px" />&nbsp;&nbsp;&nbsp;
                                                    <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddPaymentModeCustom"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label37" Text="Shipping Agent" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <div style="overflow: scroll; width: 300px; height: 122px">
                                                        <asp:CheckBoxList ID="ChklistShippingAgent" runat="server" Width="300px" Height="122px">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </td>
                                                <%-- <td>
                                                    Buying Agent
                                                </td>
                                                <td>
                                                    <div style="overflow: scroll; width: 300px; height: 122px">
                                                        <asp:CheckBoxList ID="chklistBuyingAgent" runat="server" Width="300px" Height="122px">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </td>--%>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label38" Text="Buying Agent" runat="server" Font-Bold="true" />
                                                    <br></br>
                                                    <br></br>
                                                    <asp:Label ID="Label36" Text="Buying House" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddBuyingAgent" runat="server" Width="150px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button7" runat="server" CssClass="buttonsmalls" OnClientClick="return addbyingagent();"
                                                        Text="&#43;" Width="35px" Visible="false" />
                                                    &nbsp;<asp:Button ID="addbying" runat="server" BackColor="White" BorderColor="White"
                                                        BorderWidth="0px" EnableTheming="true" ForeColor="White" Height="1px" OnClick="addbying_Click"
                                                        Width="1px" CssClass="buttonsmalls" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddBuyingAgent"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <br></br>
                                                    <br></br>
                                                    <asp:DropDownList ID="DDBuyingHouse1" runat="server" CssClass="dropdown" Width="150px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDBuyingHouse1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="addbuyinghouse" runat="server" CssClass="buttonsmalls" OnClientClick="return Addbuyinghouse();"
                                                        Text="&#43;" Width="35px" />
                                                    <asp:Button ID="btnbuyinghouseCloseFormCustomer" runat="server" Style="display: none"
                                                        EnableTheming="true" OnClick="btnbuyinghouseCloseFormCustomer_Click" />
                                                </td>
                                            </tr>
                                            <tr id="Tr1" runat="server">
                                                <td>
                                                    <asp:Label ID="Label43" Text="Merchant Name" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmerchantname" runat="server" CssClass="textb" Width="350px"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label42" Text="Customer GSTIN" runat="server" Font-Bold="true" />
                                                    <asp:TextBox ID="txtgstin" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                    <asp:Label ID="Label44" Text="Consignee GSTIN" runat="server" Font-Bold="true" />
                                                    <asp:TextBox ID="txtConsigneeGSTIN" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false">
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label39" Text=" Shipping Agent" runat="server" Font-Bold="true" />
                                                    <asp:Button ID="shipping0" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                        EnableTheming="true" ForeColor="White" Height="1px" OnClick="shipping0_Click"
                                                        Text="Button" Width="1px" CssClass="buttonsmalls" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddShipping" runat="server" Width="150px" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="Button6" runat="server" CssClass="buttonsmalls" OnClientClick="return Addshiping(); "
                                                        Text="&#43;" Width="35px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddShipping"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="Label40" Text=" Buying House" runat="server" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDbuyinghouse" runat="server" CssClass="dropdown" Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    &nbsp;<asp:Button ID="cmdSave0" runat="server" CssClass="buttonnorm" OnClientClick="return validateSave();"
                                                        OnClick="cmdSave_Click" Text="Save" ValidationGroup="f1" />
                                                    &nbsp;<asp:Button ID="cmdCancel" runat="server" CssClass="buttonnorm" Text="New"
                                                        OnClientClick="return NewForm()" />
                                                    &nbsp;<asp:Button ID="Rpt" runat="server" OnClick="Rpt_Click" CssClass="buttonnorm"
                                                        Text="Preview" Width="70px" />
                                                    &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();"
                                                        Text="Close" />
                                                    &nbsp;<asp:Button ID="Btnaddcustomerbank" runat="server" CssClass="buttonnorm" Text="ADD Customer Bank"
                                                        OnClientClick="return Addcustomerbank();" />
                                                    &nbsp;<asp:Button ID="Btnaddcustomercourier" runat="server" CssClass="buttonnorm"
                                                        Text="ADD Customer Courier" OnClientClick="return Addcourier();" />
                                                    &nbsp;<asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
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
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <div style="width: 1000px; max-height: 300px; overflow: auto; margin-left: 5px; float: left"">                                                    
                                                        <asp:GridView ID="GvCustomer" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            DataKeyNames="Customerid" ForeColor="#333333" OnPageIndexChanging="GvCustomer_PageIndexChanging"
                                                            OnRowDataBound="GvCustomer_RowDataBound" OnSelectedIndexChanged="GvCustomer_SelectedIndexChanged"
                                                            PageSize="40" OnRowCreated="GvCustomer_RowCreated">
                                                            <HeaderStyle CssClass="gvheaders" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <PagerStyle CssClass="PagerStyle" />
                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sr No.">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CustomerId" HeaderText="CustomerId" Visible="false" />
                                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                                <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" />
                                                                <asp:BoundField DataField="Address" HeaderText="Address1" />
                                                                <asp:BoundField DataField="CustAdd1" HeaderText="Address2" />
                                                                <asp:BoundField DataField="CustAdd2" HeaderText="Address3" />
                                                                <asp:BoundField DataField="BuyerOtherThanConsigneeAir" HeaderText="BuyerOtherThanConsigneeAir" />
                                                                <asp:BoundField DataField="BuerOtherThanConsigneeSea" HeaderText="BuerOtherThanConsigneeSea" />
                                                                <asp:BoundField DataField="Mark" HeaderText="Mark" />
                                                                <asp:BoundField DataField="CountryName" HeaderText="CountryName" />
                                                                <asp:BoundField DataField="StateName" HeaderText="StateName" />
                                                                <asp:BoundField DataField="ContinentName" HeaderText="ContinentName" />
                                                                <asp:BoundField DataField="SubcontinentName" HeaderText="SubcontinentName" />
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
                                                                <asp:BoundField DataField="AirPortName" HeaderText="AirPortName" />
                                                                <asp:TemplateField HeaderText="AirPort" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAirport" runat="server" Text='<%#Bind("AirPort") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SeaPortName" HeaderText="SeaPortName" />
                                                                <asp:TemplateField HeaderText="SeaPort" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSeaPort" runat="server" Text='<%#Bind("SeaPort") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="AcNo" HeaderText="AcNo" />
                                                                <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" />
                                                                <asp:BoundField DataField="TinNo" HeaderText="TinNo" />
                                                                <asp:BoundField DataField="AgentName" HeaderText="AgentName" />
                                                                <asp:BoundField DataField="BuyeingAgentName" HeaderText="BuyeingAgentName" />
                                                                <asp:BoundField DataField="Name_buying_house" HeaderText="BuyingHouse" />
                                                                <asp:BoundField DataField="PaymentName" HeaderText="PaymentName" />
                                                                <asp:BoundField DataField="TermName" HeaderText="TermName" />
                                                                <asp:BoundField DataField="MerchantName" HeaderText="Merchant Name" />
                                                                <asp:BoundField DataField="GSTIN" HeaderText="GSTIN" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:CheckBox ID="Chkvalidate" runat="server" AutoPostBack="True" OnCheckedChanged="Chkvalidate_CheckedChanged" />
                                                    <br />
                                                    <asp:Label ID="Label41" Text=" Buyer's Label List" runat="server" Font-Bold="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <div id="ch" runat="server" visible="false">
                                                        <asp:GridView ID="Gvchklist" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                                            OnRowCreated="Gvchklist_RowCreated">
                                                            <HeaderStyle CssClass="gvheaders" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
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
                                <asp:HiddenField ID="hnmastercompanyid" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
