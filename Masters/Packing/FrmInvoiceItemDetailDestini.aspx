<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceItemDetailDestini.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceItemDetailDestini" %>

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
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>
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
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:GridView ID="GDItemDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                CssClass="grid-view" OnRowCreated="GDItemDetail_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                    <asp:BoundField DataField="QualityName" HeaderText="QualityName" />
                                    <asp:BoundField DataField="Pcs" HeaderText="Pcs" />
                                    <asp:BoundField DataField="Area" HeaderText="Area" />
                                    <asp:BoundField DataField="Price" HeaderText="Price" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
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
