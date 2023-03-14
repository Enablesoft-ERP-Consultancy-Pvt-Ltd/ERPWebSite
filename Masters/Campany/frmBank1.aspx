<%@ Page Title="Bank" Language="C#" AutoEventWireup="true" CodeFile="frmBank1.aspx.cs"
    Inherits="Masters_Campany_frmBank1" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmBank1.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function validate() {

            if (Page_ClientValidate())
                return confirm('Do you Want to Save Data ?');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="LblItem" runat="server" Text=" Bank Name" Font-Bold="true"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="textb" Width="300px" Height="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankName"
                            CssClass="errormsg" ErrorMessage="Please, Enter Bank Name!" ValidationGroup="m"
                            SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=" Address" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="550px" Height="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddress"
                            ErrorMessage="Please,Enter Bank Address" ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="m">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" runat="server" Text=" City" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtCity" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCity"
                            ErrorMessage="Please,Enter City" ForeColor="Red" SetFocusOnError="true" ValidationGroup="m">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label3" runat="server" Text="  State" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtState" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label4" runat="server" Text="  Country" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtCountry" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label5" runat="server" Text=" Fax No." Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtFaxNo" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label6" runat="server" Text="  Phone No." Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtPhn" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle" runat="server" visible="false">
                        <asp:Label ID="Label7" runat="server" Text=" Currency Name" Font-Bold="true" />
                        <br />
                        <asp:DropDownList ID="DDcurrency" runat="server" Width="170px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle" runat="server" visible="true">
                        <asp:Label ID="Label8" runat="server" Text=" A/C. No." Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtAcc" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label9" runat="server" Text=" ADI Code" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="TxtAdCode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label10" runat="server" Text="Swift Code" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtCode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCode"
                            ErrorMessage="Please,Enter Swift Code " ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="m">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label11" runat="server" Text=" E-Mail" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtEmail" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label12" runat="server" Text="Contact Person" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtMisc" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" visible="false">
                        <asp:Label ID="Label13" runat="server" Text="IBAN" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtiban" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label14" runat="server" Text=" BIC" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtbic" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label15" runat="server" Text="  IFS CODE" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtifscode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label16" runat="server" Text="  AD CODE" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtacode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label17" runat="server" Text="BRANCH CODE" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtbranchcode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label18" runat="server" Text=" MICR CODE" Font-Bold="true" />
                        <br />
                        <asp:TextBox ID="txtmicrcode" runat="server" Width="170px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label19" runat="server" Text=" A/c Type" Font-Bold="true" />
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
                        <asp:Label ID="Label20" runat="server" Text="CATEGORIES" Font-Bold="true" />
                        <br />
                        <div style="overflow: scroll; width: 160px; border: 1px Solid Black">
                            <asp:CheckBoxList ID="chkBankCategory" runat="server" Width="150px" Height="77px">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right">
                        <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="NewForm();"
                            Width="70px" />
                        <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            Text="Save" ValidationGroup="m" OnClientClick="return validate();" Width="70px" />
                        <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview" OnClick="BtnPreview_Click"
                            Width="70px" />
                        <asp:Button ID="BtnClose" OnClientClick="CloseForm();" CssClass="buttonnorm" runat="server"
                            Text="Close" Width="70px" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormsg"
                            ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbblerr" runat="server" ForeColor="Red"></asp:Label>
                        <div style="width: 870px; height: 300px; overflow: scroll">
                            <asp:GridView ID="dgBank" DataKeyNames="BankId" runat="server" Width="1200px" EmptyDataText="No Data Found!"
                                OnRowDataBound="dgBank_RowDataBound" OnSelectedIndexChanged="dgBank_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views" OnRowCreated="dgBank_RowCreated"
                                OnRowDeleting="dgBank_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="BankId" HeaderText="Sr.No." />
                                    <asp:BoundField DataField="BankName" HeaderText="Bank_Name" />
                                    <asp:BoundField DataField="Street" HeaderText="Street" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="State" HeaderText="State" />
                                    <asp:BoundField DataField="Country" HeaderText="Country" />
                                    <asp:BoundField DataField="ACNo" HeaderText="AC No" />
                                    <asp:BoundField DataField="SwiftCode" HeaderText="Swift_Code" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="Phone_No" />
                                    <asp:BoundField DataField="FaxNo" HeaderText="Fax_No" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="ContectPerson" HeaderText="Contact_Person" />
                                    <asp:BoundField DataField="ADICode" HeaderText="ADI_Code" />
                                    <asp:BoundField DataField="iban" Visible="false" HeaderText="Iban" />
                                    <asp:BoundField DataField="Bic" HeaderText="Bic" />
                                    <asp:BoundField DataField="Ifscode" HeaderText="Ifscode" />
                                    <asp:BoundField DataField="Adcode" HeaderText="Adcode" />
                                    <asp:BoundField DataField="Branchcode" HeaderText="Branchcode" />
                                    <asp:BoundField DataField="Micrcode" HeaderText="Micrcode" />
                                    <asp:BoundField DataField="currencyname" HeaderText="CurrencyName" />
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
</asp:Content>
