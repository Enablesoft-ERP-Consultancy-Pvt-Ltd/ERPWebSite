<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceRate.aspx.cs" Inherits="Masters_Packing_FrmInvoiceRate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   <%-- <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />--%>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:TextBox ID="TxtInvoiceId" runat="server" BorderWidth="0px" ForeColor="White"
                                Height="0px" Width="0px"></asp:TextBox>
                            <asp:RadioButton ID="RDAreaWise" Text="Area Wise" runat="server" 
                                GroupName="OrderType" oncheckedchanged="RDAreaWise_CheckedChanged" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RDPcsWise" Text="Pcs Wise" runat="server" 
                                GroupName="OrderType" oncheckedchanged="RDPcsWise_CheckedChanged" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="GDItemDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                OnRowCommand="GDItemDetail_RowCommand" CssClass="grid-view" OnRowCreated="GDItemDetail_RowCreated">
                                 <HeaderStyle CssClass="gvheader" />
            <AlternatingRowStyle CssClass="gvalt" />
            <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                                    <asp:BoundField DataField="QualityName" HeaderText="QualityName" />
                                    <asp:BoundField DataField="DesignName" HeaderText="DesignName" />
                                    <asp:BoundField DataField="ColorName" HeaderText="ColorName" />
                                    <asp:TemplateField HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPcs" runat="server" Text='<%# Bind("Pcs") %>' Width="70px" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Area">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtArea" runat="server" Text='<%# Bind("Area") %>' Width="70px"
                                                Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPrice" runat="server" Text='<%# Bind("Price") %>' Width="70px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtAmt" runat="server" Text='<%# Bind("Amt") %>' Width="70px" Enabled="false"
                                                AutoPostBack="True"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Show">
                                        <ItemTemplate>
                                            <asp:Button ID="BTNShowAmt" CssClass="buttonnorm" runat="server" Text="Show" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                OnClick="BTNShowAmt_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td colspan="3" align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
