<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBuyingHouse.aspx.cs" Inherits="Masters_Campany_AddBuyingHouse"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/SearchA.ascx" TagName="SearchA" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_btnbuyinghouseClose').click();
            self.close();
        }
        function CloseForm() {
            window.opener.document.getElementById('btnbuyinghouseClose').click();
            self.close();
        }
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_btnbuyinghouseCloseFormCustomer').click();
            self.close();
        }
        function validate() {
            if (Page_ClientValidate())
                return confirm('Do you Want to Save Data ?');
        }
    
    </script>
</head>
<body>
    <form id="frmbuyinghouse" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="height: 500px;">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr align="left">
                        <td class="tdstyle">
                            <b>Name of Buying House</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtnameofbuyinghouse" runat="server" CssClass="textb" Width="150px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="Req2" runat="server" ControlToValidate="txtnameofbuyinghouse"
                                ErrorMessage="Plz Select Buyinghouse" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                        <td class="tdstyle">
                            <b>Type of Buying House</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txttypeofbuyinghouse" runat="server" CssClass="textb" Width="150px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="Req3" runat="server" ControlToValidate="txttypeofbuyinghouse"
                                SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr align="left">
                        <td class="tdstyle">
                            <b>Address</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtaddress" runat="server" CssClass="textb" Width="150px" TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <b>Contact No</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcontactno" runat="server" CssClass="textb" Width="150px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td class="tdstyle">
                            <b>Fax No</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFaxNo" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <b>Email</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            <b>Contact Person</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcontactperson" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <b>Email of Contact Person</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtemailofcontactperson" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            <b>Mobile Number</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmobilenumber" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="2">
                        </td>
                        <td colspan="2" align="right">
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonnorm" Text="New" Visible="false" />
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" Text="Save" Width="65px"
                                OnClientClick="return validate();" OnClick="btnSave_Click" />
                            <asp:Button ID="Button1" runat="server" Text="Preview" CssClass="buttonnorm" Visible="false" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="4">
                            <asp:Label ID="leblrr" Font-Size="Medium" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="Gdbuyinghouse" runat="server" Width="350px" AutoGenerateColumns="false"
                                AllowPaging="True" PageSize="8" CssClass="grid-view" OnRowCreated="Gdbuyinghouse_RowCreated"
                                OnRowDataBound="Gdbuyinghouse_RowDataBound" DataKeyNames="buyinghouseId" OnSelectedIndexChanged="Gdbuyinghouse_SelectedIndexChanged">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <Columns>
                                    <asp:BoundField DataField="Name_buying_house" HeaderText="Name_Buying_House" />
                                    <asp:BoundField DataField="Type_buying_house" HeaderText="Type_Buying_House" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                    <asp:BoundField DataField="Contactno" HeaderText="Contactno" />
                                    <asp:BoundField DataField="Faxno" HeaderText="Faxno" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="Contactperson" HeaderText="Contactperson" />
                                    <asp:BoundField DataField="Emailofcontactperson" HeaderText="Emailofcontactperson" />
                                    <asp:BoundField DataField="MobileNumber" HeaderText="MobileNumber" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
