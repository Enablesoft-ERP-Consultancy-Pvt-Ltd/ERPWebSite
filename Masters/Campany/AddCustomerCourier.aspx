<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomerCourier.aspx.cs"
    Inherits="Masters_Campany_AddCustomerCourier" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closesCourier() {

            self.close();
        }
        
    </script>
    <script type="text/javascript">


        function getbacktostepone() {
            window.location = "AddSeaport.aspx";
        }
        function onSuccess() {
            ta
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-left: 15%; margin-right: 15%">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Courier Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtcourier" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Courier Name"
                                ControlToValidate="txtcourier" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            A/C No
                        </td>
                        <td>
                            <asp:TextBox ID="txtacno" runat="server" Width="150px" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter A/C No Name"
                                ControlToValidate="txtacno" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="2">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="DGcourier" runat="server" Width="280px" DataKeyNames="detailid"
                                    CssClass="grid-view" AutoGenerateColumns="False" OnRowCreated="DGcourier_RowCreated"
                                    OnRowDataBound="DGcourier_RowDataBound" OnSelectedIndexChanged="DGcourier_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="CourierName">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCourierName" runat="server" Text='<%#Bind("CourierName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ac/No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcNo" runat="server" Text='<%#Bind("AcNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" ValidationGroup="f1"
                                OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" ValidationGroup="f1"
                                CausesValidation="False" OnClientClick="return closesCourier();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                Visible="False" OnClick="btndelete_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
