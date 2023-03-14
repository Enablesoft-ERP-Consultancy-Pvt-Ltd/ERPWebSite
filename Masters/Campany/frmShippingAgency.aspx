<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmShippingAgency.aspx.cs"
    Inherits="Masters_Campany_frmShippingAgency" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title>Shipping Agency</title>
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_btnAgencyClose').click();
            self.close();
        }
        function CloseForm() {
            window.opener.document.getElementById('btnAgencyClose').click();
            self.close();
        }
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_btnAgencyCloseFormCustomer').click();
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAgencyName" runat="server" Text="Shipping Agency" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAgencyName" runat="server" Width="250px" CssClass="textboxtranswithborder"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Req1" runat="server" ControlToValidate="txtAgencyName"
                        SetFocusOnError="true" ErrorMessage="Please Enter Agency Name" ValidationGroup="m"
                        Font-Size="20px" ForeColor="Red">*
                    
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" Width="300px" CssClass="textboxtranswithborder"
                        TextMode="MultiLine" Height="44px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Phone No." CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPhoneNo" runat="server" Width="150px" CssClass="textboxtranswithborder"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="150px" CssClass="textboxtranswithborder"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFax" runat="server" Text="Fax No." CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFaxno" runat="server" Width="150px" CssClass="textboxtranswithborder"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="8">
                    <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                        ValidationGroup="m" />
                    &nbsp;<asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close"
                        OnClientClick="return CloseForm();" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <div>
            <asp:GridView ID="gdShippingAgency" runat="server" Width="708px" AllowPaging="True"
                CellPadding="4" PageSize="6" ForeColor="#333333" DataKeyNames="AgencyId" AutoGenerateColumns="False"
                CssClass="grid-view" OnPageIndexChanging="gdShippingAgency_PageIndexChanging"
                OnSelectedIndexChanged="gdShippingAgency_SelectedIndexChanged" OnRowDataBound="gdShippingAgency_RowDataBound"
                OnRowDeleting="gdShippingAgency_RowDeleting">
                <HeaderStyle CssClass="gvheader" />
                <AlternatingRowStyle CssClass="gvalt" />
                <RowStyle CssClass="gvrow" HorizontalAlign="Center" VerticalAlign="Middle" />
                <PagerStyle CssClass="PagerStyle" />
                <EmptyDataRowStyle CssClass="gvemptytext" />
                <Columns>
                    <asp:BoundField DataField="AgencyName" HeaderText="Agency Name" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                    <asp:BoundField DataField="FaxNo" HeaderText="FaxNo" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:CommandField ShowDeleteButton="True">
                        <ItemStyle ForeColor="Blue" />
                    </asp:CommandField>
                </Columns>
                <SelectedRowStyle BackColor="Yellow" />
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
