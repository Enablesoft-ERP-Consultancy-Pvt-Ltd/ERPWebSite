<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddShipping.aspx.cs" Inherits="AddShipping"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function CloseForm() {
        window.opener.document.getElementById('CPH_Form_shipping0').click();
        self.close();
    }
    function CloseForm() {
        window.opener.document.getElementById('shipping0').click();
        self.close();
    }

    function AddShippingAgency() {

        var answer = confirm("Do you want to ADD?")

        if (answer) {

            var left = (screen.width / 2) - (750 / 2);
            var top = (screen.height / 2) - (500 / 2);


            //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
            window.open('frmShippingAgency.aspx', 'ADD Shipping Agency', 'width=740px, height=400px, top=' + top + ', left=' + left);
        }
    }

    function validate() {
        var isPageValid = Page_ClientValidate('m');
        if (isPageValid) {


            if (document.getElementById('DDAgencyName').options.length == 0) {
                alert("Agency Name must have a value....!");
                document.getElementById("DDAgencyName").focus();
                return false;
            }
            else if (document.getElementById('DDAgencyName').options[document.getElementById('DDAgencyName').selectedIndex].value == 0) {
                alert("Please select Agency Name....!");
                document.getElementById("DDAgencyName").focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 90%">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text="Shipping Agency" runat="server" ID="lbl2" Font-Bold="true" />
                            <br />
                            <asp:DropDownList ID="DDAgencyName" runat="server" CssClass="dropdown" Width="150px"
                                OnSelectedIndexChanged="DDAgencyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="btnaddagency" runat="server" Text="...." CssClass="buttonsmalls"
                                OnClientClick="return AddShippingAgency();" Style="margin-top: 0px" ToolTip="Click For Add New Agency"
                                Width="30px" />
                            <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDAgencyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            <asp:Button ID="btnAgencyClose" runat="server" BackColor="White" ForeColor="White"
                                Height="1px" Width="0px" BorderColor="White" BorderWidth="0px" EnableTheming="true"
                                OnClick="btnAgencyClose_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblAgent" runat="server" Text="Agent Name" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter CompanyName"
                                ControlToValidate="txtCompanyName" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Address" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="345px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text=" Contact Person" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="LblPhone" runat="server" Text="Phone  No." CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" runat="server" Text="Mobile" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="trdstyle">
                            <asp:Label ID="Label5" runat="server" Text="Email" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" runat="server" Text="Fax" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 148px;">
                            Define Company<br />
                            <asp:DropDownList ID="DDdefinecompany" runat="server" CssClass="dropdown" Width="148px">
                                <asp:ListItem Value="1">CHA</asp:ListItem>
                                <asp:ListItem Value="2">FORWARDER</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                        <td style="width: 148px;">
                            Mode Of Tranaction<br />
                            <asp:DropDownList ID="DDmodeoftranaction" runat="server" CssClass="dropdown" Width="148px">
                                <asp:ListItem Value="1">SEA</asp:ListItem>
                                <asp:ListItem Value="2">AIR</asp:ListItem>
                                <asp:ListItem Value="3">DOORDELIVERY</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                        <td style="width: 148px;">
                            Bank Information<br />
                            <asp:DropDownList ID="DDbankinformation" runat="server" CssClass="dropdown" Width="148px">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="3">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                                Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                                ShowSummary="true" />
                            <br />
                        </td>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="4">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdShippingMaster" runat="server" AllowPaging="True" CssClass="grid-view"
                                AutoGenerateColumns="false" DataKeyNames="Agentid" OnPageIndexChanging="gdShippingMaster_PageIndexChanging"
                                OnRowCreated="gdShippingMaster_RowCreated" OnRowDataBound="gdShippingMaster_RowDataBound"
                                OnSelectedIndexChanged="gdShippingMaster_SelectedIndexChanged" PageSize="6" Width="840px">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="AgentId" HeaderText="Sr.No." />
                                    <asp:BoundField DataField="AgentName" HeaderText="Agent Name" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                    <asp:BoundField DataField="ContactPerson" HeaderText="ContactPerson" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                                    <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="definecompany" HeaderText="definecompany" />
                                    <asp:BoundField DataField="modeoftranaction" HeaderText="modeoftranaction" />
                                    <asp:TemplateField HeaderText="bankid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbankid" runat="server" Text='<%#Bind("bankid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="bankname" HeaderText="BankName" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td colspan="3" align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                Text="Save" ValidationGroup="m" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClick="btnclose_Click"
                                OnClientClick="return CloseForm();" Text="Close" />
                            <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                                OnClientClick="return confirm('Do you want to Delete data?')" Text="Delete" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
