<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBank.aspx.cs" Inherits="Masters_Campany_frmBank1"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "AddBank.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
            //parent.document.location.reload();
        }
        function RefreshCombo() {
            if (window.opener.document.getElementById('CPH_Form_x')) {
                window.opener.document.getElementById('CPH_Form_x').click();
                self.close();
            }
            else if (window.opener.document.getElementById('CPH_Form_Button3')) {
                window.opener.document.getElementById('CPH_Form_Button3').click();
                self.close();
            }
            else if (window.opener.document.getElementById('Button3')) {
                window.opener.document.getElementById('Button3').click();
                self.close();
            }
        }
        function validate() {
            //            if (Page_ClientValidate())
            //                return confirm('Do you Want to Save Data ?');

            if (typeof (Page_ClientValidate) == 'function') {
                var isPageValid = Page_ClientValidate('m');
                if (isPageValid) {
                    return confirm('Do you Want to Save Data ?');
                }
            }

        }
    </script>
</head>
<body>
    <form id="frmBank1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbl1" Text="Bank Name" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="textb" Width="305px" Height="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqUser" runat="server" ControlToValidate="txtBankName"
                            CssClass="errormsg" ErrorMessage="Please, Enter Bank Name!" ValidationGroup="m"
                            SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Text="Address" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="550px" Height="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress"
                            ErrorMessage="Please,Enter Bank Address.." ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="m">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label2" Text="City" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtCity" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label3" Text="State" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtState" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label4" Text="Country" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtCountry" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label5" Text="Fax No." runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtFaxNo" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label6" Text="Phone No." runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtPhn" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label7" Text="Currency" runat="server" CssClass="label" />
                        <br />
                        <asp:DropDownList ID="DDcurrency" runat="server" Width="170px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label8" Text="A/C No." runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtAcc" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label9" Text="ADI CODE" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="TxtAdCode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label10" Text="Swift Code" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtCode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label11" Text="E-Mail" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtEmail" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label12" Text="Contact Person" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtMisc" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label13" Text="IBAN" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtiban" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label14" Text="BIC" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtbic" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label15" Text="IFS CODE" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtifscode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label16" Text="AD CODE" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtacode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label17" Text="BRANCH CODE" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtbranchcode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label18" Text="MICR CODE" runat="server" CssClass="label" />
                        <br />
                        <asp:TextBox ID="txtmicrcode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label19" Text="A/c Type" runat="server" CssClass="label" />
                        <br />
                        <asp:DropDownList ID="DDActype" runat="server" Width="120px" CssClass="dropdown">
                            <asp:ListItem Value="1">CURRENT</asp:ListItem>
                            <asp:ListItem Value="2">SAVING</asp:ListItem>
                            <asp:ListItem Value="3">DRAWBACK</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label21" runat="server" Text=" Account Name" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtaccountname" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label22" runat="server" Text=" Post Code" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtPostcode" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label20" Text="CATEGORIES" runat="server" CssClass="label" />
                        <br />
                        <div style="overflow: scroll; width: 160px; border: 1px Solid Black">
                            <asp:CheckBoxList ID="chkBankCategory" runat="server" Width="150px" Height="77px">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr align="left">
                    <td colspan="5" align="right">
                        <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="NewForm()" />
                        <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            Text="Save" ValidationGroup="m" OnClientClick="return validate();" />
                        <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview" OnClick="BtnPreview_Click" />
                        <asp:Button ID="BtnClose" OnClientClick="return RefreshCombo();" CssClass="buttonnorm"
                            runat="server" Text="Close" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errormsg"
                            ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormsg"
                            ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lbblerr" runat="server" ForeColor="Red"></asp:Label>
                        <div style="width: 870px; height: 300px; overflow: scroll">
                            <asp:GridView ID="dgBank" DataKeyNames="BankId" runat="server" Width="1200px" EmptyDataText="No Data Found!"
                                OnRowDataBound="dgBank_RowDataBound" OnSelectedIndexChanged="dgBank_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views" OnRowCreated="dgBank_RowCreated"
                                OnRowDeleting="dgBank_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="BankId" HeaderText="Sr.No." />
                                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                    <asp:BoundField DataField="Street" HeaderText="Street" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="State" HeaderText="State" />
                                    <asp:BoundField DataField="Country" HeaderText="Country" />
                                    <asp:BoundField DataField="ACNo" HeaderText="AC No" />
                                    <asp:BoundField DataField="SwiftCode" HeaderText="Swift Code" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" />
                                    <asp:BoundField DataField="FaxNo" HeaderText="Fax No" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="ContectPerson" HeaderText="Contect Person" />
                                    <asp:BoundField DataField="ADICode" HeaderText="ADI Code" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
