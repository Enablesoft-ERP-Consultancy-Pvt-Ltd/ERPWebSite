<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="GenrateInDentNew.aspx.cs" Inherits="Masters_Process_GenrateInDentNew"
    EnableEventValidation="false" ViewStateMode="Enabled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
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
            window.location.href = "GenrateInDentNew.aspx";
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
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }

        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }
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
            else {
                return confirm('Do you want to save data?')
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
                                <table>
                                    <tr>
                                        <td colspan="4" align="right" id="TDChkForOrder" runat="server" visible="true">
                                            <asp:CheckBox ID="ChkForEdit" runat="server" Text="For Edit" ForeColor="Black" AutoPostBack="True"
                                                CssClass="checkboxbold" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                                            <%--    <asp:CheckBox ID="CheckBox1" runat="server" Text="Check For OrderWise" ForeColor="Red"
                                                AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />--%>
                                            <asp:Label ID="lblMessage1" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" id="TDBtnAddEmp" runat="server">
                                            <asp:Button ID="BtnAddEmp" runat="server" Text="Add Vendor" CssClass="buttonnorm"
                                                OnClientClick="return AddAddEmp()" Width="90px" />
                                        </td>
                                        <td colspan="4" align="center" id="TDLblReqDate" runat="server" visible="false">
                                            <asp:Label ID="lblfinalDate" runat="server" Text="Stock At PH Date:" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label1" runat="server" Text="CompanyName" Font-Bold="true"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="110" runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged"
                                                TabIndex="0">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDCustCode" visible="false" runat="server" class="tdstyle">
                                            <asp:Label ID="Label2" runat="server" Text="CustomerCode" Font-Bold="true"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged"
                                                TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDOrderNo" runat="server" visible="false" class="tdstyle">
                                            <asp:Label ID="Label3" runat="server" Text=" OrderNo" Font-Bold="true"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="110" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label4" runat="server" Text="ProcessName" Font-Bold="true"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DDProcessName"
                                                ErrorMessage="please Select Process" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="110" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label5" runat="server" Text="Vendor Name" Font-Bold="true"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="DDPartyName"
                                                ErrorMessage="please Select PartyName" ForeColor="Red" SetFocusOnError="true"
                                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="110" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" TabIndex="4">
                                            </asp:DropDownList>
                                            <asp:Button ID="refreshEmp2" runat="server" Visible="true" Text="" BorderWidth="0px"
                                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                ForeColor="White" OnClick="refreshEmp_Click" />
                                        </td>
                                        <td id="TDppno" runat="server" class="tdstyle">
                                            <asp:Label ID="Label6" runat="server" Text="PPNo" Font-Bold="true"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="DDProcessProgramNo"
                                                ErrorMessage="please Select ProcessProgramNo" ForeColor="Red" SetFocusOnError="true"
                                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDProcessProgramNo" Width="110" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDProcessProgramNo_SelectedIndexChanged"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDIndentNo" runat="server" class="tdstyle" visible="false">
                                            <asp:Label ID="Label10" runat="server" Text="IndentNo" Font-Bold="true"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDIndentNo"
                                                ErrorMessage="please Select Indent No" ForeColor="Red" SetFocusOnError="true"
                                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDIndentNo" Width="110" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDIndentNo_SelectedIndexChanged" TabIndex="5">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdindent" runat="server" class="tdstyle">
                                            <asp:Label ID="Label7" runat="server" Text=" Indent No." Font-Bold="true"></asp:Label>
                                            <br />
                                            <%--<asp:TextBox CssClass="textb" ID="TxtIndentNo" Width="110" runat="server"  TabIndex="6"></asp:TextBox>--%>
                                            <asp:TextBox CssClass="textb" ID="TxtIndentNo" Width="110" runat="server" AutoPostBack="true"
                                                OnTextChanged="TxtIndentNo_TextChanged" TabIndex="6"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label8" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtDate"
                                                ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator><b
                                                    style="color: Red"> &nbsp; *</b>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px" TabIndex="7"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label9" runat="server" Text=" TargetDate" CssClass="labelbold"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtDate"
                                                ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                            <b style="color: Red">&nbsp; *</b>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="100px" AutoPostBack="false"
                                                TabIndex="8" BackColor="#7b96bb "></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtReqDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="Label26" runat="server" Text="Issue Details" CssClass="labelbold"
                                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <div id="Div2" runat="server" style="max-height: 400px; overflow: auto">
                                                    <asp:GridView ID="DGIndent" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                        OnRowDataBound="DGIndent_RowDataBound">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" />
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ItemDescription">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("Description") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="DDUnit" CssClass="dropdown" Width="80px" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CaTypel">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="DDCalType" CssClass="dropdown" Width="80px" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lot No.">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="120px" runat="server" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tag No.">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTagNo" Width="70px" BackColor="White" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock Qty">
                                                                <ItemTemplate>
                                                                    <%-- <asp:TextBox ID="txtStockQty" Width="70px" BackColor="White" runat="server" onkeypress="return isNumberKey(event);" />--%>
                                                                    <asp:Label ID="lblTotalStockQty" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Loss %" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLoss" Width="70px" BackColor="White" runat="server" ReadOnly="true"
                                                                        onkeypress="return isNumberKey(event);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Issue Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalQty" Text='<%#Bind("QTY") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pre Qty">
                                                                <ItemTemplate>
                                                                    <%-- <asp:TextBox ID="txtPreQty" Width="70px" BackColor="White" runat="server" Text="0" ReadOnly="true" onkeypress="return isNumberKey(event);" />--%>
                                                                    <asp:Label ID="lblPreQty" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Issue Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQty" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                                                        OnTextChanged="txtQty_TextChanged" onkeypress="return isNumberKey(event);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Extra Qty" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtExtraQty" Width="50px" BackColor="White" runat="server" onkeypress="return isNumberKey(event);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRate" Width="50px" BackColor="White" runat="server" onkeypress="return isNumberKey(event);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField HeaderText="Remark">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemark" Width="70px" BackColor="White" runat="server" TextMode="MultiLine" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Item Remark">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtItemRemark" Width="70px" BackColor="White" runat="server" TextMode="MultiLine" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPPId" Text='<%#Bind("PPId") %>' runat="server" />
                                                                    <asp:Label ID="lblIFinishedId" Text='<%#Bind("IFinishedId") %>' runat="server" />
                                                                    <asp:Label ID="lblOFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                                    <asp:Label ID="lblUnitTypeId" Text='<%#Bind("UnitTypeID") %>' runat="server" />
                                                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server" />
                                                                    <asp:Label ID="lblOrderDetailId" Text='<%#Bind("OrderDetailId") %>' runat="server" />
                                                                    <asp:Label ID="lblTotalStock" runat="server" />
                                                                    <%-- <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    <asp:Label ID="lblissuemasterdetailid" Text='<%#Bind("Detailid") %>' runat="server" />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <table>
                                    <tr id="TRdyingTypes" runat="server" visible="false">
                                        <td class="tdstyle" id="TDDyeingMatch" runat="server">
                                            <asp:Label ID="lblmatch" runat="server" Text="Dyeing Match" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDDyeingMatch" runat="server" CssClass="dropdown" Enabled="false">
                                                <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                                <asp:ListItem Value="Side">Side</asp:ListItem>
                                                <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle" id="TDDyeing" runat="server">
                                            <asp:Label ID="Label23" runat="server" Text="Dyeing" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDDyeing" runat="server" CssClass="dropdown" Enabled="false">
                                                <asp:ListItem Value="Boarder">Boarder</asp:ListItem>
                                                <asp:ListItem Value="Ground">Ground</asp:ListItem>
                                                <asp:ListItem Value="Ascent">Ascent</asp:ListItem>
                                                <asp:ListItem Value="Ground/Boarder">Ground/Boarder</asp:ListItem>
                                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle" id="TDDyingType" runat="server">
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
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                                            <asp:Label ID="Label21" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtremarks" runat="server" Width="250px" Height="70px" CssClass="textb"
                                                TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                        </td>
                                        <%--  <td class="tdstyle" id="TDitemremark" runat="server" colspan="3">
                                            <asp:Label ID="Label22" runat="server" Text="Item Remarks" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtitemremark" runat="server" Width="250px" Height="70px" CssClass="textb"
                                                TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="7" align="right">
                                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" TabIndex="26"
                                                OnClientClick="return ClickNew();" />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                                ValidationGroup="f1" OnClientClick="return Validation();" TabIndex="26" Width="50px" />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreview" Enabled="false" runat="server"
                                                Text="Preview" TabIndex="27" OnClick="BtnPreview_Click" />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                                runat="server" Text="Close" TabIndex="28" Width="50px" />
                                            &nbsp;<asp:Button ID="Btnorder" runat="server" Text="WorkLoad" Visible="false" CssClass="buttonnorm"
                                                OnClientClick="return OrderDetail()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="TxtFinishedid" runat="server" Height="0px" Width="0px" BorderStyle="None"></asp:TextBox>
                                            <asp:Label ID="lblMessage" Font-Bold="true" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="ValidationSummary1" ForeColor="RED" ValidationGroup="f1"
                                                ShowMessageBox="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <div id="gride" runat="server" style="height: 300px">
                                                <asp:GridView ID="DGIndentDetail" AutoGenerateColumns="false" runat="server" DataKeyNames="IndentDetailId"
                                                    CssClass="grid-views" OnRowDataBound="DGIndentDetail_RowDataBound1" OnRowCancelingEdit="DGIndentDetail_RowCancelingEdit"
                                                    OnRowEditing="DGIndentDetail_RowEditing" OnRowUpdating="DGIndentDetail_RowUpdating"
                                                    OnRowDeleting="DGIndentDetail_RowDeleting">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="IndentDetailID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentDetailId" runat="server" Text='<%#Eval("IndentDetailId") %>'></asp:Label>
                                                                <asp:Label ID="lblIndentId" runat="server" Text='<%#Eval("IndentId") %>'></asp:Label>
                                                                <asp:Label ID="lblOFinishedId" runat="server" Text='<%#Eval("OFinishedId") %>'></asp:Label>
                                                                <asp:Label ID="lblIFinishedId" runat="server" Text='<%#Eval("IFinishedId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="IndentDetailId" HeaderText="IndentDetailId" Visible="false" />--%>
                                                        <asp:BoundField DataField="IndentNo" HeaderText="IndentNo" />
                                                        <asp:BoundField DataField="PPNo" HeaderText="PPNo" />
                                                        <asp:BoundField DataField="InDescription" HeaderText="InDescription" />
                                                        <asp:BoundField DataField="OutDescription" HeaderText="OutDescription" />
                                                        <asp:TemplateField HeaderText="Rate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#Eval("Rate") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- <asp:BoundField DataField="Rate" HeaderText="Rate" />--%>
                                                        <%--<asp:BoundField DataField="Quantity" HeaderText="Qty" />--%>
                                                        <asp:TemplateField HeaderText="Qty">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQty" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtQty" runat="server" Text='<%#Eval("Quantity") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="ExtraQty" HeaderText="ExtraQty" />--%>
                                                        <asp:TemplateField HeaderText="Lot No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLotNo" runat="server" Text='<%#Eval("LotNo") %>'></asp:Label>
                                                                <asp:Label ID="lbltxtQty" runat="server" Text='<%#Eval("Quantity") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="LotNo" HeaderText="LotNo" />--%>
                                                        <%--<asp:BoundField DataField="TagNo" HeaderText="TagNo" />--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <span style="margin-left: 10px">
                                                                    <asp:LinkButton ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                                                                    <%--<asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />--%>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                        CommandName="Delete"></asp:LinkButton></span>
                                                                <%-- <asp:Button ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete this data?");' Text="Delete" CommandName="Delete"/>  --%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <span style="padding-left: 10px">
                                                                    <asp:LinkButton ID="btn_Update" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                                                    <asp:LinkButton ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton></span>
                                                                <%--<asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update"/>  
                                                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"/>  --%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        <td colspan="3" runat="server" visible="false" id="tdordergrid">
                                            <div id="Div1" runat="server" style="height: 200px; overflow: auto">
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
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hncomp" runat="server" />
                                            <asp:HiddenField ID="hnorderid" runat="server" Visible="false" />
                                            <asp:HiddenField ID="HNPENQTY" runat="server" Visible="false" />
                                            <asp:HiddenField ID="Hnqty" runat="server" Visible="false" />
                                            <asp:HiddenField ID="HnTotalStock" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
