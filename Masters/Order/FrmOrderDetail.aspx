<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="FrmOrderDetail.aspx.cs"
    Inherits="Masters_Order_FrmOrderDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr align="center">
                <td>
                    <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" OnRowDataBound="DG_RowDataBound"
                        CssClass="grid-view" OnRowCreated="DG_RowCreated" ShowFooter="true">
                        <HeaderStyle CssClass="gvheader" Font-Bold="true" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="OrderDescription">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbltot" runat="server" BackColor="#BED30B" Font-Names="Verdana" ForeColor="Black"
                                        Text='Total' /></FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Description" HeaderText="OrderDescription">
                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                            </asp:BoundField>--%>
                            <%--<asp:BoundField DataField="Qty" HeaderText="IssueQty">
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="IssueQty">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueQty" runat="server" Text='<%#Bind("Qty") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbltotissue" runat="server" BackColor="#BED30B" Font-Names="Verdana"
                                        ForeColor="Black" Text='<%# gettotissue().ToString() %>' /></FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RecQty">
                                <ItemTemplate>
                                    <asp:Label ID="lblRec" runat="server" Text='<%#Bind("RecQty") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbltotRec" runat="server" BackColor="#BED30B" Font-Names="Verdana"
                                        ForeColor="Black" Text='<%# gettotRec().ToString() %>' /></FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <%-- <asp:BoundField DataField="RecQty" HeaderText="RecQty">
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Pending">
                                <ItemTemplate>
                                    <asp:Label ID="lblpending" runat="server" Text='<%# getpend(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "RecQty").ToString()) %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbltotpending" runat="server" BackColor="#BED30B" Font-Names="Verdana"
                                        ForeColor="Black" Text='<%# gettotpend().ToString() %>' /></FooterTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage1" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hnissueqty" runat="server" />
                    <asp:HiddenField runat="server" ID="hnrecqty" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
