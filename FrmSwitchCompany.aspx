<%@ Page Title="Switch Company" Language="C#" AutoEventWireup="true" CodeFile="FrmSwitchCompany.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="FrmSwitchCompany" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 20%">
                <table>
                    <tr>
                        <td colspan="2">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="DGSwitchCompany" runat="server" DataKeyNames="CompanyID" OnRowDataBound="DGSwitchCompany_RowDataBound"
                                    OnSelectedIndexChanged="DGSwitchCompany_SelectedIndexChanged" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                     <td colspan="2">
                     <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                     </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
