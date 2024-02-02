<%@ Page Title="Partner Employee Vendor Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPropertyPartnerDetail.aspx.cs"
    Inherits="Masters_ZDevi_FrmPropertyPartnerDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function New() {
            window.location.href = "FrmPartnerPropertyTransactionDetail.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="From Partner Name" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDFromPartnerName" runat="server" Width="300px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDFromPartnerName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label6" Text="Property Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDPropertyName" runat="server" Width="300px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" Text="Payment Mode" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDPaymentMode" runat="server" Width="300px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDPaymentMode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label11" Text="Transaction Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtTransactionDate" CssClass="textb" runat="server" Width="100px" />
                    </td>
                </tr>
                <tr id="trBankDetail" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label3" Text="From Bank Account No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDFromBankAccountNo" runat="server" Width="300px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label4" Text="Chq/Transaction No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtChequeTransactionNo" CssClass="textb" runat="server" Width="250px" />
                    </td>
                    <td>
                        <asp:Label ID="Label5" Text="Chq/Transaction Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtChequeTransactionDate" CssClass="textb" runat="server" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" Text="To Partner Name" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                        <br />
                        <asp:DropDownList ID="DDToPartnerName" runat="server" Width="300px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDToPartnerName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="tdToBankAccountNo" runat="server" visible="false">
                        <asp:Label ID="Label8" Text="To Bank Account No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDToBankAccountNo" runat="server" Width="300px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label9" Text="Amount" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtAmount" CssClass="textb" runat="server" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label10" Text="Remark" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtRemark" CssClass="textb" runat="server" Width="500px" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" UseSubmitBehavior="false"
                            OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                            CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="Btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" Width="70px" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
