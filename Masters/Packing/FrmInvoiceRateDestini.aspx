<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceRateDestini.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceRateDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            //window.location.href = "../../main.aspx";
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
                        <td>
                            <asp:TextBox ID="TxtInvoiceId" runat="server" BorderWidth="0px" ForeColor="White"
                                Height="0px" Width="0px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td align="right" class="tdstyle">
                            <asp:RadioButton ID="RDAreaWise" runat="server" Checked="true" Text="Area Wise" AutoPostBack="True"
                                OnCheckedChanged="RDAreaWise_CheckedChanged" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <td class="tdstyle">
                                <asp:RadioButton ID="RDPcsWise" runat="server" Checked="false" Text="Pcs Wise" AutoPostBack="True"
                                    OnCheckedChanged="RDPcsWise_CheckedChanged" />
                    </tr>
                    <tr>
                        <td class="style2" colspan="4">
                            <asp:GridView ID="GDItemDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                OnRowCommand="GDItemDetail_RowCommand" CssClass="grid-view" OnRowCreated="GDItemDetail_RowCreated">
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
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                                CssClass="buttonnorm" />
                            &nbsp;
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                Text="Close" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
