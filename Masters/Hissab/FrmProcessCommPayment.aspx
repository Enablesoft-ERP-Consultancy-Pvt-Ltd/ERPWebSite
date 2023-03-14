<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmProcessCommPayment.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_Hissab_FrmProcessCommPayment"
    Title="Process Commission Hissab" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProcessCommPayment.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx");
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
            if (document.getElementById('CPH_Form_DDProcessOrderNo') != null) {
                if (document.getElementById('CPH_Form_DDProcessOrderNo').options.length == 0) {
                    alert("Process order no name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessOrderNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessOrderNo').options[document.getElementById('CPH_Form_DDProcessOrderNo').selectedIndex].value == 0) {
                    alert("Please select process order no ....!");
                    document.getElementById("CPH_Form_DDProcessOrderNo").focus();
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
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td align="right" colspan="3">
                        <asp:CheckBox ID="ChkForEdit" Font-Bold="True" runat="server" Text=" For Edit" AutoPostBack="True"
                            OnCheckedChanged="ChkForEdit_CheckedChanged" CssClass="checkboxbold" />
                    </td>
                </tr>
                <tr>
                    <td id="TDSlipNoForEdit" runat="server">
                        <span class="labelbold">Slip No</span>
                        <br />
                        <asp:TextBox ID="TxtSlipNo" runat="server" Width="70px" CssClass="textb" AutoPostBack="True"
                            OnTextChanged="TxtSlipNo_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Company Name</span>
                        <br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown"
                            OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDCompanyName" runat="server" TargetControlID="DDCompanyName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <span class="labelbold">Process Name </span>
                        <br />
                        <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDProcessName" runat="server" TargetControlID="DDProcessName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <span class="labelbold">Party Name </span>
                        <br />
                        <asp:DropDownList ID="DDEmployerName" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDEmployerName" runat="server" TargetControlID="DDEmployerName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TDDDSlipNo" runat="server">
                        <span class="labelbold">Slip No </span>
                        <br />
                        <asp:DropDownList ID="DDSlipNo" runat="server" Width="100px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDSlipNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDSlipNo" runat="server" TargetControlID="DDSlipNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <span class="labelbold">Process Order No </span>
                        <br />
                        <asp:DropDownList ID="DDProcessOrderNo" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDProcessOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDProcessOrderNo" runat="server" TargetControlID="DDProcessOrderNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TDDate" runat="server">
                        <span class="labelbold">Date </span>
                        <br />
                        <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span class="labelbold">Slip No </span>
                        <br />
                        <asp:TextBox ID="TxtHissabNo" runat="server" Width="70px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div style="width: 100%; height: 300px; overflow: scroll">
                            <asp:GridView ID="DGDetail" Width="100%" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="Item" HeaderText="Item">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Area" HeaderText="Area">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Comm.Rate">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Sr_No") %>' runat="server" />
                                            <asp:Label ID="lblqty" Text='<%#Bind("qty") %>' runat="server" />
                                            <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                            <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                                            <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                            <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                            <asp:Label ID="lbltdspercentage" Text='<%#Bind("tdspercentage") %>' runat="server" />
                                            <asp:Label ID="lblcommpaymentflag" Text='<%#Bind("commpaymentflag") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="right">
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return ValidationSave();" />
                        &nbsp;<asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm"
                            OnClick="BtnPriview_Click" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                            OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
