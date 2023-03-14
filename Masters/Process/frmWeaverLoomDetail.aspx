<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmWeaverLoomDetail.aspx.cs"
    Inherits="Masters_Process_frm_WeaverLoomDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function CloseForm() {
            self.close();
        }
        function ConfirmSave() {
            var Ok = confirm('Are you sure want to save ?');
            if (Ok) {
                if (document.getElementById('hnEditflag').value == "1") {
                    var yes = confirm('Are you sure want to Delete existing Daura Data ?');
                    if (yes)
                        return true;
                    else
                        return false;
                }
                return true;
            }
            else {
                return false;
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 564px; margin-left: 20px; height: 288px;">
        <table style="width: 564px">
            <tr>
                <td>
                    <div style="overflow: scroll">
                        <asp:GridView ID="GDLoomDetail" runat="server" AutoGenerateColumns="False" Width="560px">
                            <Columns>
                                <asp:TemplateField HeaderText="Quality">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Design">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Size">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loom">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtloomDetail" runat="server" Width="65px" BackColor="Yellow"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessId" runat="server" Text='<%#Bind("ProcessId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("QualityId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignId" runat="server" Text='<%#Bind("DesignId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSizeid" runat="server" Text='<%#Bind("SizeId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitId" runat="server" Text='<%#Bind("UnitId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvheader" Height="25px" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 564px">
            <tr>
                <td align="right" colspan="7">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="75px" CssClass="buttonnorm"
                        OnClick="btnSave_Click" OnClientClick="return ConfirmSave();" />
                    &nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" Width="75px" CssClass="buttonnorm"
                        OnClientClick="CloseForm();" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hnEditflag" runat="server" />
    </div>
    </form>
</body>
</html>
