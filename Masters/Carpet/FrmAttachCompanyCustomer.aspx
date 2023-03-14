<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAttachCompanyCustomer.aspx.cs"
    Inherits="Masters_Carpet_FrmAttachCompanyCustomer" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%;">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50;">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblCompany" runat="server" Text="Company Name" CssClass="labelbold"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptCssClass="labelbold"
                                PromptPosition="Bottom" TargetControlID="DDCompanyName" ViewStateMode="Disabled">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close"
                                OnClientClick="return CloseForm()" />
                        </td>
                    </tr>
                </table>
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkForAllSelect" Text="Select For All Customer" CssClass="checkboxbold"
                                    runat="server" AutoPostBack="True" OnCheckedChanged="ChkForAllSelect_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle" id="TDCustomerCode" runat="server">
                                <div style="height: 200px; width: 100%; overflow: scroll">
                                    <asp:Label ID="LblCustomerCode" runat="server" Text="Customer Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:CheckBoxList ID="ChkBoxListCustomerCode" runat="server" Width="200px" CssClass="checkboxnormal">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
