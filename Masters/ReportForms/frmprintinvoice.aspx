<%@ Page Title="PRINT INVOICE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmprintinvoice.aspx.cs" Inherits="Masters_ReportForms_frmprintinvoice" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="LbCompanyName" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInvoiceYear" runat="server" Text="Invoice Year" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDInvoiceYear" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDInvoiceYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Invoice No" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDinvoiceNo" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblDocumentType" runat="server" Text="Document Type" Width="150px"
                                CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDDocumentType" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDocumentType_SelectedIndexChanged">
                                <asp:ListItem Value="1">INVOICE</asp:ListItem>
                                <asp:ListItem Value="2">PACKING LIST</asp:ListItem>
                                <asp:ListItem Value="3">Duty Draw Back LIST</asp:ListItem>
                                <asp:ListItem Value="4">Shipping Instruction</asp:ListItem>
                                <asp:ListItem Value="5">Letter To Bank</asp:ListItem>
                                <asp:ListItem Value="6">Bill Of Exchange</asp:ListItem>
                                <asp:ListItem Value="7">BRC</asp:ListItem>
                                <asp:ListItem Value="8">Shipping Advice</asp:ListItem>
                                <asp:ListItem Value="9">Packing List Summary</asp:ListItem>
                                <asp:ListItem Value="10">Express B/L</asp:ListItem>
                                <asp:ListItem Value="11">Export Value Declaration</asp:ListItem>
                                <asp:ListItem Value="12">Single country Declaration</asp:ListItem>
                                <asp:ListItem Value="13">GR Release</asp:ListItem>
                                <asp:ListItem Value="14">Annexure-I</asp:ListItem>
                                <asp:ListItem Value="15">Weight List</asp:ListItem>
                                <asp:ListItem Value="16">ACD</asp:ListItem>
                                <asp:ListItem Value="17">Form Sdf </asp:ListItem>
                                <asp:ListItem Value="18">Annexure-A </asp:ListItem>
                                <asp:ListItem Value="19">VDF </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblPrintType" runat="server" Text="Print Type" Width="150px" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDPrintType" runat="server" Width="250px" CssClass="dropdown">
                                <asp:ListItem Value="1">Type 1</asp:ListItem>
                                <asp:ListItem Value="2">Type 2</asp:ListItem>
                                <asp:ListItem Value="3">Type 3</asp:ListItem>
                                <asp:ListItem Value="4">Type 4</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:CheckBox ID="chkexport" Text="Export" runat="server" Visible="false" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
