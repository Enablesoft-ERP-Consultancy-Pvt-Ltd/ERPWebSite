<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmRecipeSlipGeneration.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Recipe_FrmRecipeSlipGeneration"
    EnableEventValidation="false" Title="Recipe Slip Generation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmRecipeSlipGeneration.aspx";
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
                        <td id="TDSlipNoForEdit" runat="server" visible="false">
                            <asp:Label ID="lbl" Text="Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtSlipNoForEdit" runat="server" Width="70px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtSlipNoForEdit_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Company Name" runat="server" CssClass="labelbold" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="ChkForEdit" Font-Bold="True" runat="server" Text=" For Edit" AutoPostBack="True"
                                CssClass="checkboxbold" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="True">
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
                        <td id="TDDDSlipNo" runat="server" visible="false">
                            <asp:Label ID="Label5" Text="Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDSlipNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDSlipNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDSlipNo" runat="server" TargetControlID="DDSlipNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text=" Recipe Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDRecipeName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDRecipeName"
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
                            <asp:TextBox ID="TxtSlipDate" runat="server" Width="90px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtSlipDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label9" Text="Slip No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtSlipNo" runat="server" Width="70px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td id="selectall" runat="server" colspan="2" visible="false">
                            <asp:CheckBox ID="ChkForAllSelect" runat="server" Text="Select All" ForeColor="Red"
                                onclick="return CheckAllCheckBoxes();" CssClass="checkboxnormal" AutoPostBack="True" />
                            <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForRawMaterial" runat="server" Text="For Raw Material" Checked="true"
                                            ForeColor="Red" CssClass="checkboxnormal" AutoPostBack="True" OnCheckedChanged="ChkForRawMaterial_CheckedChanged" />--%>
                        </td>
                        <td colspan="5" align="right">
                            <asp:Button ID="BtnShowData" runat="server" Text="Show Data" ForeColor="White" CssClass="buttonnorm"
                                UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to Show Data?')) return; this.disabled=true;this.value = 'wait ...';"
                                OnClick="BtnShowData_Click" Visible="False" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnDelete" Text="Delete" runat="server"
                                OnClick="BtnDelete_Click" OnClientClick="return ValidationDelete();" Visible="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div id="DivDGDetail" style="max-height: 350px; width: 800px; overflow: scroll">
                            <asp:GridView ID="DGDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                OnRowDataBound="DGDetail_RowDataBound">
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
                                    <asp:BoundField DataField="Item" HeaderText="Item" />
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ItemStyle Width="500px" />
                                    </asp:BoundField>
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
                                    <asp:TemplateField HeaderText="Recipe Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecipeName" Text='<%#Bind("RecipeName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblflag" Text='<%#Bind("Flag") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecipeNameID" runat="server" Text='<%#Bind("RecipeNameID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
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
                        <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
