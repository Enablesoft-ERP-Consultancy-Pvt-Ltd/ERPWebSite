<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmHomeFurnishingPanelMakingHissab.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_HomeFurnishing_FrmHomeFurnishingPanelMakingHissab"
    EnableEventValidation="false" Title="HISSAB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmHomeFurnishingPanelMakingHissab.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx");
        }
        function CheckAllCheckBoxes() {
            if (document.getElementById('CPH_Form_ChkForAllSelect').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_DGDetail');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_DGDetail');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchemp.ClientID %>').click();
            }
        }
        function ValidationSaveNew() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name ....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            return confirm('Do You Want To Save? Please check date range')
        }
        function ValidationSave() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name ....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDEmployerName') != null) {
                if (document.getElementById('CPH_Form_DDEmployerName').options.length == 0) {
                    alert("Employee name must have a value....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                    alert("Please select employee name ....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ChkForEdit').checked == true) {
                if (document.getElementById('CPH_Form_DDSlipNo') != null) {
                    if (document.getElementById('CPH_Form_DDSlipNo').options.length == 0) {
                        alert("Slip no must have a value....!");
                        document.getElementById("CPH_Form_DDSlipNo").focus();
                        return false;
                    }
                    else if (document.getElementById('CPH_Form_DDSlipNo').options[document.getElementById('CPH_Form_DDSlipNo').selectedIndex].value == 0) {
                        alert("Please select slip no ....!");
                        document.getElementById("CPH_Form_DDSlipNo").focus();
                        return false;
                    }
                }
            }
            var k = 0;
            for (i = 1; i < document.getElementById('CPH_Form_DGDetail').rows.length; i++) {
                var inputs = document.getElementById('CPH_Form_DGDetail').rows[i].getElementsByTagName('input');
                if (inputs[0].checked == true) {
                    k = k + 1;
                    i = document.getElementById('CPH_Form_DGDetail').rows.length;
                }
            }
            if (k == 0) {
                alert("Please select atleast one check box....!");
                return false;
            }
            return confirm('Do You Want To Save?')
        }
        function ValidationDelete() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name ....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDEmployerName') != null) {
                if (document.getElementById('CPH_Form_DDEmployerName').options.length == 0) {
                    alert("Employee name must have a value....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                    alert("Please select employee name ....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ChkForEdit').checked == true) {
                if (document.getElementById('CPH_Form_DDSlipNo') != null) {
                    if (document.getElementById('CPH_Form_DDSlipNo').options.length == 0) {
                        alert("Slip no must have a value....!");
                        document.getElementById("CPH_Form_DDSlipNo").focus();
                        return false;
                    }
                    else if (document.getElementById('CPH_Form_DDSlipNo').options[document.getElementById('CPH_Form_DDSlipNo').selectedIndex].value == 0) {
                        alert("Please select slip no ....!");
                        document.getElementById("CPH_Form_DDSlipNo").focus();
                        return false;
                    }
                }
            }
            return confirm('Are you sure to delete this slip number ?')
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset>
                <legend>
                    <asp:Label Text="..." CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkForEdit" Font-Bold="True" runat="server" Text=" For Edit" AutoPostBack="True"
                                CssClass="checkboxbold" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label14" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                            <asp:Button Text="employee" ID="btnsearchemp" CssClass="buttonnorm" runat="server"
                                OnClick="btnsearchemp_Click" Style="display: none" />
                        </td>
                    </tr>
                    <tr>
                        <td id="TDSlipNoForEdit" runat="server">
                            <asp:Label ID="lbl" Text="Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtSlipNo" runat="server" Width="70px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtSlipNo_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDCompanyName" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text=" Process Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDProcessName" runat="server" TargetControlID="DDProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Party Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDEmployerName" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDEmployerName" runat="server" TargetControlID="DDEmployerName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDDDSlipNo" runat="server">
                            <asp:Label ID="Label5" Text="Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDSlipNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDSlipNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDSlipNo" runat="server" TargetControlID="DDSlipNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDFFromDate" runat="server" visible="true" class="style4">
                            <asp:Label ID="Label6" Text="From Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtFromDate" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtFromDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="TDFToDate" runat="server" visible="true" style="width: 102px">
                            <asp:Label ID="Label7" Text=" To Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtToDate" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtToDate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtToDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="TDDate" runat="server">
                            <asp:Label ID="Label8" Text="Slip Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label9" Text=" Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtHissabNo" runat="server" Width="70px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return ValidationSave();" />
                        <asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="BtnPriview_Click" />
                        <asp:Button ID="btnprintvoucher" runat="server" Text="Print Voucher" CssClass="buttonnorm" Visible ="false" 
                            OnClick="btnprintvoucher_Click" />
                        <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
