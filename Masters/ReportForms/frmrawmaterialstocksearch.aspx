<%@ Page Title="Raw Material Stock search" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmrawmaterialstocksearch.aspx.cs" Inherits="Masters_ReportForms_frmrawmaterialstocksearch" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmrawmaterialstocksearch.aspx";
        }
       
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkstockqtyzero" Text="Having Stock Qty. Zero" runat="server" CssClass="checkboxbold"
                            Font-Size="Small" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label Text="Lot No." CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 13%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtlotno" CssClass="textb" runat="server" Width="90%" placeholder="Type here to search" />
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label1" Text="Tag No." CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 13%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txttagno" CssClass="textb" runat="server" Width="90%" placeholder="Type here to search" />
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label2" Text="Bin No." CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 13%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBinNo" CssClass="textb" runat="server" Width="90%" placeholder="Type here to search" />
                    </td>
                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label3" Text="Purchase Bill No" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtPurchaseReceiveBillNo" CssClass="textb" runat="server" Width="90%"
                            placeholder="Type here to search" />
                    </td>
                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label4" Text="Dyeing Receive Bill No" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtDyeingReceiveBillNo" CssClass="textb" runat="server" Width="90%"
                            placeholder="Type here to search" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="1" cellspacing="0">
                <tr>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label8" Text="Quality" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                        <asp:DropDownList ID="DDQuality" CssClass="dropdown" runat="server" Width="95%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label6" Text="Shade Color" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                        <asp:DropDownList ID="DDshadeno" CssClass="dropdown" runat="server" Width="95%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label7" Text="Godown" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                        <asp:DropDownList ID="DDgodown" CssClass="dropdown" runat="server" Width="95%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 8%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label5" Text="Company Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%;">
                        <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="95%" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 35%">
                        <asp:Button Text="Show Detail" CssClass="buttonnorm" ID="btnshowdetail" runat="server"
                            OnClick="btnshowdetail_Click" />
                        <asp:Button Text="Clear Search" ID="btnnewsearch" CssClass="buttonnorm" runat="server"
                            OnClientClick="return ClickNew();" />
                        <asp:LinkButton ID="lnkshowdetailedreport" CssClass="labelbold" ForeColor="Blue"
                            Font-Size="Small" Text="Show Detailed Report" runat="server" OnClick="lnkshowdetailedreport_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <table style="width: 80%">
                <tr>
                    <td style="width: 100%">
                        <div style="width: 100%; overflow: auto; max-height: 500px">
                            <asp:GridView ID="DgDetail" runat="server" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
