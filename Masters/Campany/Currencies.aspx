<%@ Page Language="C#" Title="CurrencyMaster" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="Currencies.aspx.cs" Inherits="Currencies" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="height: 480px">
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Currency Name" runat="server" ID="lbl" Font-Bold="true" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox CssClass="textb" ID="TxtCurrencyName" runat="server" Width="490px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TxtCurrencyName"
                                ErrorMessage="Please enter the Currency Name" runat="server" ValidationGroup="m"
                                ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text=" Rupees" runat="server" ID="Label1" Font-Bold="true" />
                        </td>
                        <td class="style1">
                            <asp:TextBox CssClass="textb" ID="txtrupees" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtrupees"
                                ErrorMessage="Please enter the Rupees" runat="server" ValidationGroup="m" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="  Paise(P)" runat="server" ID="Label2" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtpaise" runat="server" Width="171px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Regularfieldvalidater1" ControlToValidate="txtpaise"
                                ErrorMessage="Please enter the Paise:" runat="server" ValidationGroup="m" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2" class="tdstyle">
                            <asp:Label Text="  Current Conversion Rate As Per Rs." runat="server" ID="Label3"
                                Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtConversionRateAsPerRs" runat="server" onkeydown="return (event.keyCode!=13)"
                                Width="171px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trPAYINSTRUCTION" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label Text=" PAYMENT INSTRUCTION" runat="server" ID="Label4" Font-Bold="true" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="TXTPAYMENTINSTRUCTION" runat="server" Height="80px" TextMode="MultiLine"
                                onkeydown="return (event.keyCode!=13);" Width="375px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trBENEFICIARYBANK" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label Text=" BENEFICIARY BANK" runat="server" ID="Label5" Font-Bold="true" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="TXTBENEFICIARYBANK" runat="server" Height="80px" TextMode="MultiLine"
                                Width="375px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trRemarks" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label Text="Remarks" runat="server" ID="Label6" Font-Bold="true" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="490px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                                Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                                ShowSummary="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClick="btndelete_Click"
                                Visible="False" OnClientClick="return confirm('Do you want to Delete data?')" />
                            <asp:Button ID="BtnNew" Text="New" Width="64px" runat="server" CssClass="buttonnorm"
                                OnClick="BtnNew_Click" />
                            <asp:Button ID="BtnSave" Text="Save" runat="server" Width="64px" CssClass="buttonnorm"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="BtnSave_Click"
                                ValidationGroup="m" />
                            <asp:Button ID="BtnClose" Text="Close" runat="server" Width="64px" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" OnClick="BtnClose_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                            <div id="gridediv" runat="server" style="width: 100%; height: 100px; overflow: auto">
                                <asp:GridView ID="dgcurrency" Width="609px" runat="server" OnRowDataBound="dgcurrency_RowDataBound1"
                                    OnSelectedIndexChanged="dgcurrency_SelectedIndexChanged" DataKeyNames="Sr.No">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
