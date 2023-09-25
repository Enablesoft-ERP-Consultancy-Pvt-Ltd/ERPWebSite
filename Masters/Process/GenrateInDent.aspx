<%@ Page Title="Genrate Indent" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="GenrateInDent.aspx.cs" Inherits="Masters_Process_GenrateInDent" EnableEventValidation="false"
    ViewStateMode="Enabled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function ClickNew() {
            window.location.href = "GenrateInDent.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "GenrateIndentReport");
        }
        function AddDyeingRat() {
            var a3 = document.getElementById("<%=TxtFinishedid.ClientID %>").value;

            window.open('AddDyeingRat.aspx?' + a3);
        }
        function OrderDetail() {
            var e = document.getElementById("<%=DDPartyName.ClientID %>");
            var varcode = e.options[e.selectedIndex].value;
            if (varcode > 0) {
                window.open('../order/frmorderdetail.aspx?Vendor=' + varcode + '&Type=JW', '', 'width=950px,Height=500px');
            }
            else {
                alert("Plz select Vendor name");
            }
        }
        function AddAddEmp() {
            window.open('../Campany/frmWeaver.aspx?ABC=1', '', 'Height=600px,width=1000px');
        }
        function Validation() {

            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Please Select Company");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TDCustCode.ClientID %>")) {
                if (document.getElementById("<%=DDCustomerCode.ClientID %>").value <= "0") {
                    alert("Please Select Customer Code");
                    document.getElementById("<%=DDCustomerCode.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDOrderNo.ClientID %>")) {
                if (document.getElementById("<%=DDOrderNo.ClientID %>").value <= "0") {
                    alert("Please Select Order No.");
                    document.getElementById("<%=DDOrderNo.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDProcessName.ClientID %>").value <= "0") {
                alert("Please Select Process No.");
                document.getElementById("<%=DDProcessName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDPartyName.ClientID %>").value <= "0") {
                alert("Please Select Vendor Name.");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TDppno.ClientID %>")) {
                if (document.getElementById("<%=DDProcessProgramNo.ClientID %>").value <= "0") {
                    alert("Please Select ProcessProgram No.");
                    document.getElementById("<%=DDProcessProgramNo.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDLotNo.ClientID %>")) {
                if (document.getElementById("<%=ddllotno.ClientID %>").value <= "0") {
                    alert("Please Select lot No.");
                    document.getElementById("<%=ddllotno.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDTagNo.ClientID %>")) {
                if (document.getElementById("<%=txtTagno.ClientID %>").value == "" && document.getElementById("<%=hntagnoautogen.ClientID %>").value == "0") {
                    alert("Please Fill Tag No.");
                    document.getElementById("<%=txtTagno.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtQty.ClientID %>").value == "" || document.getElementById("<%=txtQty.ClientID %>").value == "0") {
                alert("Quantity Cann't be blank Or Zero");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
        
    </script>
    <script language="JavaScript" type="text/javascript">

        function confirmSubmit() {

            var returnvalue;
            var agree = confirm("Are you sure you wish to continue with zero Loss Percentage value?");
            if (agree) {
                document.getElementById('<%=hnRetunTypeValue.ClientID %>').value = "1";
                returnvalue = 1;
                //return true;
            }
            else {
                document.getElementById('<%=hnRetunTypeValue.ClientID %>').value = "0";
                returnvalue = 0;
                //document.getElementById('<%=BtnSave.ClientID %>').style.display = 'none';                           
                //return false;
            }
        }

    </script>
    <table width="100%" border="1">
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <fieldset>
                                    <legend>
                                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                                    </legend>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="right" id="TDChkForOrder" runat="server" visible="false">
                                                <asp:CheckBox ID="ChkForOrder" runat="server" Text="Check For OrderWise" ForeColor="Red"
                                                    AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                                            </td>
                                            <td>
                                            </td>
                                            <td id="TDReDyeing" runat="server" visible="false">
                                                <asp:CheckBox ID="chkredyeing" runat="server" Text="Check For RE-DYEING" ForeColor="Red"
                                                    AutoPostBack="true" CssClass="checkboxbold" OnCheckedChanged="chkredyeing_CheckedChanged" />
                                            </td>
                                            <td colspan="4" align="center" id="TDLblReqDate" runat="server" visible="false">
                                                <asp:Label ID="lblfinalDate" runat="server" Text="Stock At PH Date:" CssClass="labelbold"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label29" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDCustCode" visible="false" runat="server" class="tdstyle">
                                                <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="DDCustomerCode" runat="server" Width="130px" CssClass="dropdown"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDOrderNo" runat="server" visible="false" class="tdstyle">
                                                <asp:Label ID="Label3" runat="server" Text=" OrderNo" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="110" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label4" runat="server" Text="ProcessName" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DDProcessName"
                                                    ErrorMessage="please Select Process" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDppno" runat="server" class="tdstyle">
                                                <asp:Label ID="Label6" runat="server" Text="PPNo" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="DDProcessProgramNo"
                                                    ErrorMessage="please Select ProcessProgramNo" ForeColor="Red" SetFocusOnError="true"
                                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDProcessProgramNo" Width="130px" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDProcessProgramNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="DDPartyName"
                                                                ErrorMessage="please Select PartyName" ForeColor="Red" SetFocusOnError="true"
                                                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="150" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="refreshEmp2" runat="server" Visible="true" Text="" BorderWidth="0px"
                                                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                                ForeColor="White" OnClick="refreshEmp_Click" />
                                                        </td>
                                                        <td align="center" id="TDBtnAddEmp" runat="server">
                                                            <br />
                                                            <asp:Button ID="BtnAddEmp" runat="server" Text="+" CssClass="buttonnorm" ToolTip="Add Vendor"
                                                                OnClientClick="return AddAddEmp()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="tdindent" runat="server" class="tdstyle">
                                                <asp:Label ID="Label7" runat="server" Text=" Indent No." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtIndentNo" Width="90" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label8" runat="server" Text="Date" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtDate"
                                                    ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator><b
                                                        style="color: Red"> &nbsp; *</b>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="90px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label9" runat="server" Text=" Req.Date" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtDate"
                                                    ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <b style="color: Red">&nbsp; *</b>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="90px" AutoPostBack="false"
                                                    BackColor="#7b96bb "></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtReqDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>
                                        <asp:Label Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                                    </legend>
                                    <table>
                                        <tr>
                                            <td id="procode" runat="server" visible="false" class="tdstyle">
                                                <asp:Label ID="Label10" runat="server" Text=" ProdCode" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtProdCode" Width="100px" runat="server" OnTextChanged="TxtProdCode_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                                    UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblCategory" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                                                    ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="150" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblItemName" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                                                    ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="150" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdQuality" runat="server" visible="false">
                                                <asp:Label ID="LblQuality" CssClass="labelbold" runat="server" Text="Label"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdDesign" runat="server" visible="false">
                                                <asp:Label ID="LblDesign" CssClass="labelbold" runat="server" Text="Label"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDDesign" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdColor" runat="server" visible="false">
                                                <asp:Label ID="LblColor" CssClass="labelbold" runat="server" Text="Label"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDColor" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdColorShade" runat="server" visible="false">
                                                <asp:Label ID="LblColorShade" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDColorShade" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDISSUESHADE" runat="server" visible="false">
                                                <asp:Label ID="Label27" runat="server" Text="ISSUE SHADE" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDISSUESHADE" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDISSUESHADE_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdShape" runat="server" visible="false">
                                                <asp:Label ID="LblShape" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDShape" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TdSize" runat="server" visible="false">
                                                <asp:Label ID="LblSize" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                                <%--<asp:CheckBox ID="ChkFt" runat="server" AutoPostBack="True" Text="Ft" OnCheckedChanged="ChkFt_CheckedChanged"
                                                Visible="false" />--%>
                                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                                    <%--   <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                                <asp:ListItem Value="1">MTR</asp:ListItem>
                                                <asp:ListItem Value="2">Inch</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDSize" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label11" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddUnit" runat="server" CssClass="dropdown" Width="70px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle" id="TDDyeingMatch" runat="server" visible="false">
                                                <asp:Label ID="lblmatch" runat="server" Text="Dyeing Match" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="DDDyeingMatch" runat="server" CssClass="dropdown" Enabled="false"
                                                    Width="110px">
                                                    <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                                    <asp:ListItem Value="Side">Side</asp:ListItem>
                                                    <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle" id="TDDyeing" runat="server" visible="false">
                                                <asp:Label ID="Label23" runat="server" Text="Dyeing" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDDyeing" runat="server" CssClass="dropdown" Enabled="false"
                                                    Width="150px">
                                                    <asp:ListItem Value="Boarder">Boarder</asp:ListItem>
                                                    <asp:ListItem Value="Ground">Ground</asp:ListItem>
                                                    <asp:ListItem Value="Ascent">Ascent</asp:ListItem>
                                                    <asp:ListItem Value="Ground/Boarder">Ground/Boarder</asp:ListItem>
                                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle" id="TDDyingType" runat="server" visible="false">
                                                <asp:Label ID="Label24" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDDyingType" runat="server" CssClass="dropdown" Enabled="false">
                                                    <asp:ListItem Value="Shaded">Shaded</asp:ListItem>
                                                    <asp:ListItem Value="Natural">Natural</asp:ListItem>
                                                    <asp:ListItem Value="Plain">Plain</asp:ListItem>
                                                    <asp:ListItem Value="Gabbeh">Gabbeh</asp:ListItem>
                                                    <asp:ListItem Value="Multi Dyeing">Multi Dyeing</asp:ListItem>
                                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle" id="TDitemremark" runat="server" colspan="5">
                                                <asp:Label ID="Label22" runat="server" Text="Item Remark" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtitemremark" runat="server" Width="90%" Height="33px" CssClass="textb"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="Label28" Text="..." CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                                    <table>
                                        <tr>
                                            <td id="TDCaltype" runat="server" class="tdstyle">
                                                <asp:Label ID="Label12" runat="server" Text="CalType" CssClass="labelbold"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtQty"
                                                    ErrorMessage="please Select CalType" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="90px"
                                                    OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDGodownName" runat="server" class="tdstyle" visible="false">
                                                <asp:Label ID="Label30" runat="server" Text=" Godown Name" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="DDGodownName" CssClass="dropdown" Width="130px" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDGodownName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDLotNo" runat="server" class="tdstyle">
                                                <asp:Label ID="Label13" runat="server" Text=" LotNo." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddllotno" CssClass="dropdown" Width="130px" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddllotno_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDTagNo" runat="server" visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label25" runat="server" Text="Tag No." CssClass="labelbold"></asp:Label>
                                                            <asp:CheckBox ID="chkoldtagno" Text="For Old Tag No." CssClass="checkboxbold" ForeColor="Red"
                                                                runat="server" Visible="false" />
                                                            <br />
                                                            <asp:TextBox ID="txtTagno" CssClass="textb" Width="120px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="TDDDTagNo" runat="server" visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label31" runat="server" Text="UCN No." CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="TDStockQty" runat="server" class="tdstyle">
                                                <asp:Label ID="Label14" runat="server" Text="   Stock Qty" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtstock" CssClass="textb" Width="75px" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td id="TDLoss" runat="server" class="tdstyle">
                                                <asp:Label ID="Label15" runat="server" Text=" Loss%" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="TxtLoss" CssClass="textb" Width="50px" runat="server" Enabled="false"
                                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="TDTotalQty" runat="server" class="tdstyle">
                                                <asp:Label ID="Label16" runat="server" Text="TotalQty." CssClass="labelbold"></asp:Label>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTotalQty"
                                                ErrorMessage="TotalQty can not be null..." ForeColor="Red" SetFocusOnError="true"
                                                ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                                <br />
                                                <asp:TextBox CssClass="textb" Enabled="false" ID="txtTotalQty" Width="75px" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="TDPreQty" runat="server" class="tdstyle">
                                                <asp:Label ID="Label17" runat="server" Text=" PreQty." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox CssClass="textb" Enabled="false" ID="TxtPreQty" Width="75px" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="TDPqty" runat="server" class="tdstyle">
                                                <asp:Label ID="Label26" runat="server" Text=" Pend.Qty." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox CssClass="textb" Enabled="false" ID="txtpqty" Width="75px" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label18" runat="server" Text="Qty" CssClass="labelbold"></asp:Label>
                                                <asp:Label ID="LblKg" runat="server" Text="(kg.)" CssClass="labelbold" Visible="false"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtQty"
                                                    ErrorMessage="please Enter Qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="txtQty" Width="70px" runat="server" AutoPostBack="True"
                                                    OnTextChanged="TxtQty_TextChanged" BackColor="#7b96bb " onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label19" runat="server" Text="Extra Qty" CssClass="labelbold"></asp:Label><asp:Label
                                                    ID="LblKg1" runat="server" Text="(Kg.)" CssClass="labelbold" Visible="false"></asp:Label>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="txtextraQty" Width="70px" runat="server" onkeydown="return (event.keyCode!=13);"
                                                    onkeypress="return isNumberKey(event);" BackColor="#7b96bb " AutoPostBack="true"
                                                    OnTextChanged="txtextraQty_TextChanged"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle" runat="server" id="tdrate" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text=" Rate" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox CssClass="textb" ID="TxtRate" Width="70px" runat="server" Enabled="False"
                                                                OnTextChanged="TxtRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td id="TDBtnAddRate" runat="server" visible="true" valign="bottom">
                                                            <asp:Button ID="BtnAddRate" runat="server" CssClass="buttonnorm" OnClientClick="return AddDyeingRat();"
                                                                Text="+" ToolTip="Add Rate" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:Label Text="Amount" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:TextBox ID="txtamt" CssClass="textb" Width="80px" runat="server" Enabled="false"
                                                    BackColor="LightGray" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 60%" id="TDtxtremarks" runat="server">
                                            <asp:Label ID="Label21" runat="server" Text="Remark" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtremarks" runat="server" Width="90%" Height="33px" CssClass="textb"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td style="width: 40%; text-align: right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnEmployeeWisePPDetail" runat="server" Text="PP Detail"
                                                OnClick="BtnEmployeeWisePPDetail_Click" Visible="false" />
                                            <asp:CheckBox ID="ChkForWithoutRate" runat="server" Text="Without Rate Print" class="tdstyle"
                                                Visible="false" CssClass="checkboxbold" />
                                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew();" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                                ValidationGroup="f1" OnClientClick="return Validation();" Width="50px" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnPreview" Enabled="false" runat="server"
                                                Text="Preview" OnClick="BtnPreview_Click" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                                runat="server" Text="Close" Width="50px" />
                                            <asp:Button ID="Btnorder" runat="server" Text="WorkLoad" Visible="false" CssClass="buttonnorm"
                                                OnClientClick="return OrderDetail()" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnMaterialIssueOnIndent" runat="server" Text="Material IssueOnDyeing"
                                                OnClick="BtnMaterialIssueOnIndent_Click" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtFinishedid" runat="server" Height="0px" Width="0px" BorderStyle="None"></asp:TextBox>
                                            <asp:Label ID="lblMessage" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                                            <asp:Label ID="lblMessage1" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="ValidationSummary1" ForeColor="RED" ValidationGroup="f1"
                                                ShowMessageBox="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 70%">
                                            <div id="gride" runat="server" style="max-height: 300px; width: 95%">
                                                <asp:GridView ID="DGIndentDetail" AutoGenerateColumns="false" runat="server" DataKeyNames="IndentDetailId"
                                                    CssClass="grid-views" OnRowDataBound="DGIndentDetail_RowDataBound" Width="100%">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="IndentDetailId" HeaderText="IndentDetailId" Visible="false" />
                                                        <asp:BoundField DataField="IndentNo" HeaderText="IndentNo" />
                                                        <asp:BoundField DataField="PPNo" HeaderText="PPNo" />
                                                        <asp:BoundField DataField="InDescription" HeaderText="InDescription" />
                                                        <asp:BoundField DataField="OutDescription" HeaderText="OutDescription" />
                                                        <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                                        <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                                                        <asp:BoundField DataField="ExtraQty" HeaderText="ExtraQty" />
                                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" />
                                                        <asp:BoundField DataField="TagNo" HeaderText="TagNo" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        <td style="width: 30%" runat="server" visible="false" id="tdordergrid">
                                            <div id="Div1" runat="server" style="max-height: 200px; overflow: auto">
                                                <asp:GridView ID="dgorder" AutoGenerateColumns="false" runat="server" CssClass="grid-views"
                                                    OnSelectedIndexChanged="dgorder_SelectedIndexChanged" OnRowDataBound="dgorder_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Description" HeaderText="Order Description" />
                                                        <asp:BoundField DataField="Qty" HeaderText="Ordered Qty" />
                                                        <asp:TemplateField HeaderText="Balance to issue">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("ShadecolorId") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblorderdet" runat="server" Text='<%# Bind("orderdetailid") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="LBLUNIT" runat="server" Text='<%# Bind("unit") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "orderdetailid").ToString(),DataBinder.Eval(Container.DataItem, "Qty").ToString()) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hncomp" runat="server" />
                                            <asp:HiddenField ID="hnorderid" runat="server" Visible="false" />
                                            <asp:HiddenField ID="HNPENQTY" runat="server" Visible="false" />
                                            <asp:HiddenField ID="Hnqty" runat="server" Visible="false" />
                                            <asp:HiddenField ID="hntagnoautogen" runat="server" Visible="false" />
                                            <asp:HiddenField ID="hnRetunTypeValue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnEmployeeWisePPDetail" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
