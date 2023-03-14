<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmProcessHissab.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Hissab_FrmProcessHissab"
    EnableEventValidation="false" Title="PROCESS HISSAB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProcessHissab.aspx";
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
                        <td id="TDRadioButton" runat="server" visible="false" colspan="2">
                            <asp:RadioButton ID="RDStockWise" runat="server" Text="Stock Wise" Font-Bold="True"
                                GroupName="OrderType" Visible="false" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDQtyWise" runat="server" Text="Qty Wise" Font-Bold="True" GroupName="OrderType"
                                Visible="false" />
                        </td>
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
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDCompanyName" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDDDYear" runat ="server" visible ="false" >
                            <asp:Label ID="Label21" Text=" Year" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDYear" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDYear"
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
                        <td id="TDPoOrderNo" runat="server" visible="false">
                            <asp:Label ID="Label4" Text="PO OrderNO." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDPOOrderNo" runat="server" Width="100px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDPOOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPOOrderNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDsrno" runat="server" visible="false">
                            <asp:Label ID="lblsrno" Text="Sr No." CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDsrno" CssClass="dropdown" Width="100px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDsrno_SelectedIndexChanged">
                            </asp:DropDownList>
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
                    <tr>
                        <td id="selectall" runat="server" colspan="2">
                            <asp:CheckBox ID="ChkForAllSelect" runat="server" Text="Select All" ForeColor="Red"
                                onclick="return CheckAllCheckBoxes();" CssClass="checkboxnormal" AutoPostBack="True" />
                            <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForRawMaterial" runat="server" Text="For Raw Material" Checked="true"
                                            ForeColor="Red" CssClass="checkboxnormal" AutoPostBack="True" OnCheckedChanged="ChkForRawMaterial_CheckedChanged" />--%>
                        </td>
                         <td id="TDItemDescription" runat="server" visible="false">
                            <asp:Label ID="Label17" Text="Description." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDItemDescription" runat="server" Width="100px" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="DDItemDescription_SelectedIndexChanged" >
                            </asp:DropDownList>
                          <%--<cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDPOOrderNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>
                        <td colspan="5" align="right">
                        <asp:Button CssClass="buttonnorm" ID="BtnShowItemDescription" Text="Show ItemDescription"
                                runat="server" OnClick="BtnShowItemDescription_Click" Visible="false"  UseSubmitBehavior="false" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSaveAllProcessWise" Text="Save Process Wise"
                                runat="server" OnClick="BtnSaveAllProcessWise_Click" Visible="false" OnClientClick="return ValidationSaveNew();" />
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="BtnShowData" runat="server" Text="Show Data" ForeColor="White"
                                CssClass="buttonnorm" UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to Show Data?')) return; this.disabled=true;this.value = 'wait ...';"
                                OnClick="BtnShowData_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSaveAllOneTime" Text="Save All One Time"
                                runat="server" OnClick="BtnSaveAllOneTime_Click" OnClientClick="return ValidationSave();"
                                Visible="false" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnDelete" Text="Delete" runat="server"
                                OnClick="BtnDelete_Click" OnClientClick="return ValidationDelete();" Visible="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div id="DivDGDetail" style="max-height: 500px; width: 1000px; overflow: scroll">
                            <asp:GridView ID="DGDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False" OnRowDataBound="DGDetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StockNo" HeaderText="Stock No" />
                                    <%--<asp:BoundField DataField="Category"  HeaderText="Category" />--%>
                                    <asp:BoundField DataField="Item" HeaderText="Item" />
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ItemStyle Width="400px" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="Qty" HeaderText="Qty" />--%>
                                    <%--<asp:BoundField DataField="Area" HeaderText="Area" />--%>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblqty" Text='<%#Bind("qty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Area">
                                        <ItemTemplate>
                                            <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtRate" runat="server" Width="50px" Text='<%# Bind("Rate") %>'
                                                AutoPostBack="true" CssClass="textb" ReadOnly="true" BackColor="Yellow"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Penality">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPenality" runat="server" Width="50px" Text='<%# Bind("Penality") %>'
                                                AutoPostBack="true" CssClass="textb" ReadOnly="true" BackColor="Yellow"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comm Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcommamount" Text='<%#Bind("commAmount") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ReqWeight" HeaderText="ReqWeight" />--%>
                                    <asp:TemplateField HeaderText="Req. weight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreqweight" Text='<%#Bind("ReqWeight") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weight">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtWeight" runat="server" Width="50px" Text='<%# Bind("Weight") %>'
                                                AutoPostBack="true" CssClass="textb" ReadOnly="true" BackColor="Yellow"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Bonus Amt" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBonusAmt" runat="server" Width="50px" Text='<%# Bind("BonusAmt") %>'
                                                 CssClass="textb" ReadOnly="true" BackColor="Yellow"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Material Amt" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMaterialAmt" runat="server" Width="50px" Text='<%# Bind("MaterialAmt") %>'
                                                 CssClass="textb" ReadOnly="true" BackColor="Yellow"></asp:TextBox>
                                                 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PRemark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPRemark" runat="server" Width="200px" Text='<%# Bind("PRemark") %>'
                                                AutoPostBack="true" CssClass="textb" BackColor="Yellow"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblflag" Text='<%#Bind("Flag") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitid" runat="server" Text='<%#Bind("unitid") %>'></asp:Label>
                                            <asp:Label ID="lblcaltype" runat="server" Text='<%#Bind("caltype") %>'></asp:Label>
                                            <asp:Label ID="lblfinishedid" runat="server" Text='<%#Bind("Item_Finished_id") %>'></asp:Label>
                                            <asp:Label ID="lblcommrate" runat="server" Text='<%#Bind("comm") %>'></asp:Label>
                                            <asp:Label ID="lblIssueOrderID" runat="server" Text='<%#Bind("ISSUEORDERID") %>'></asp:Label>
                                            <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus") %>'></asp:Label>
                                             <asp:Label ID="lblFRRate2" runat="server" Text='<%#Bind("FRRate2") %>'></asp:Label>
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
                        <asp:Label Text="Total Pcs" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalpcs" CssClass="textb" Width="100px" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label10" Text="Total Area" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalarea" CssClass="textb" Width="120px" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label11" Text="Total Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtamount" CssClass="textb" Width="120px" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label12" Text="Total Commission" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalcommission" CssClass="textb" Width="120px" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label13" Text="Total Penality" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalpenality" CssClass="textb" Width="120px" runat="server" />
                    </td>
                    <td id="TDAdditionAmt" runat="server" visible="false">
                        <asp:Label ID="Label15" Text="Addition Amt" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtAdditionAmt" CssClass="textb" Width="100px" runat="server" onkeypress="return isNumber(event);" />
                    </td>
                    <td id="TDDeductionAmt" runat="server" visible="false">
                        <asp:Label ID="Label16" Text="Deduction Amt" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtDeductionAmt" CssClass="textb" Width="100px" runat="server" onkeypress="return isNumber(event);" />
                    </td>
                     <td id="TDBonusAmt" runat="server" visible="false">
                        <asp:Label ID="Label18" Text="Total Bonus Amt" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtTotalBonusAmt" CssClass="textb" Width="120px" runat="server" />
                    </td>
                     <td id="TDMaterialDeductionAmt" runat="server" visible="false">
                        <asp:Label ID="Label19" Text="Material Deduction Amt" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtMaterialDeductionAmt" CssClass="textb" Width="120px" runat="server" />
                    </td>
                   
                </tr>
                <tr>
                 <td id="TDGST" runat="server" visible="false" colspan="8">
                        <asp:Label ID="Label20" Text="GST" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtGST" CssClass="textb" Width="120px" runat="server" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:CheckBox ID="ChkForRateWise" Text="Rate Wise" runat="server" CssClass="labelbold" />
                        <asp:CheckBox ID="chkstocknowise" Text="Stock No Wise Report" runat="server" CssClass="labelbold" />
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return ValidationSave();" />
                        <asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="BtnPriview_Click" />
                        <asp:Button ID="btnprintvoucher" runat="server" Text="Print Voucher" CssClass="buttonnorm"
                            OnClick="btnprintvoucher_Click" />
                        <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
