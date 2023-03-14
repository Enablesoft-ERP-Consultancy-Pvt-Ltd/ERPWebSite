<%@ Page Title="BranchMaster" Language="C#" AutoEventWireup="true" CodeFile="frmBranchMaster.aspx.cs"
    Inherits="Masters_Campany_frmBranchMaster" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <asp:UpdatePanel ID="UpdataPanel1" runat="server">
        <ContentTemplate>
            <table style="width: 65%; margin: 5%">
                <tr>
                    <asp:TextBox ID="TxtId" runat="server" Visible="false"></asp:TextBox>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lbl5" Text="Company" runat="server" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCompany" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="cmbCompany"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" Text=" Branch Name" runat="server" Font-Bold="true" />
                    </td>
                    <td class="style10">
                        <asp:TextBox ID="txtBranchName" runat="server" CssClass="textb"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqBranch" runat="server" ControlToValidate="txtBranchName"
                            ErrorMessage="Please Enter The Branch Name" ValidationGroup="M" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" Text=" Address" runat="server" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" CssClass="textb" runat="server"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label3" Text=" Phone No" runat="server" Font-Bold="true" />
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtPhoneNo" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label4" Text="Fax No" runat="server" Font-Bold="true" />
                    </td>
                    <td class="style12">
                        <asp:TextBox ID="txtFaxNo" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label5" Text="Contact Person" runat="server" Font-Bold="true" />
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtCPerson" CssClass="textb" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="true"
                            ShowMessageBox="false" Font-Bold="true" Font-Italic="true" Font-Names="Times new Roman"
                            Font-Overline="false" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="dgBranch" runat="server" EmptyDataText="No Data Found !" OnRowDataBound="dgBranch_RowDataBound"
                            Width="513px" PageSize="6" CellPadding="4" ForeColor="#333333" GridLines="None"
                            AllowPaging="true" OnSelectedIndexChanged="dgBranch_SelectedIndexChanged" DataKeyNames="SrNo"
                            CssClass="grid-views" OnRowCreated="dgBranch_RowCreated">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                            Text="Save" ValidationGroup="M" CssClass="buttonnorm" TabIndex="7" />
                        <asp:Button ID="Button2" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="buttonnorm" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
