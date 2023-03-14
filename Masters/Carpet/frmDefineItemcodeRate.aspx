<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDefineItemcodeRate.aspx.cs"
    Inherits="Masters_Carpet_frmDefineItemcodeRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top: 20px; margin-left: 20%; margin-right: 20%">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblrate" Text="Rate" CssClass="labelbold" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtrate" CssClass="textb" Width="150px" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblunit" Text="Unit" CssClass="labelbold" runat="server" />
                </td>
                <td>
                    <asp:DropDownList ID="DDunit" CssClass="dropdown" Width="150px" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div style="max-height: 200px; overflow: auto">
                        <asp:GridView ID="GDView" AutoGenerateColumns="False" runat="server" EmptyDataText="No records found.">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <Columns>
                                <asp:TemplateField HeaderText="Item Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemDesc" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dateadded">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateadded" Text='<%#Bind("Dateadded") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
