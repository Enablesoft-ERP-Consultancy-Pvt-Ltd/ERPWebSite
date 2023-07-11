<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCurrencies.aspx.cs" Inherits="Masters_Campany_AddCurrencies"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CurrencyMaster</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            var opener = window.opener;
            if (opener != null) {
                window.opener.document.getElementById('CPH_Form_currency').click();
                self.close();
            }
            else {
                window.opener.document.getElementById('currency').click();
                self.close();
            }

        }
     
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManger1" runat="server">
    </asp:ScriptManager>
    <table border="1">
        <tr>
            <td>
                <asp:UpdatePanel ID="Updatepanel1" runat="server">
                    <ContentTemplate>
                        <div style="height: 480px">
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lbl1" Text="Currency Name" runat="server" CssClass="label" />
                                    </td>
                                    <td colspan="3" class="tdstyle">
                                        <asp:TextBox CssClass="textb" ID="TxtCurrencyName" runat="server" Width="508px"></asp:TextBox>
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
                                        <asp:Label ID="Label1" Text="Rupees" runat="server" CssClass="label" />
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox CssClass="textb" ID="txtrupees" runat="server" Width="170"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtrupees"
                                            ErrorMessage="Please enter the Rupees" runat="server" ValidationGroup="m" ForeColor="Red">*
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label2" Text="Paise(P)" runat="server" CssClass="label" />
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
                                    <br />
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" Text="Current Conversion Rate As Per Rs." runat="server" CssClass="label"
                                                margin="auto" />
                                        </td>
                                        <td colspan="1" align="right">
                                            <asp:TextBox CssClass="textb" ID="txtConversionRateAsPerRs" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tr>
                                <tr id="trPAYINSTRUCTION" runat="server" visible="false">
                                    <td class="tdstyle">
                                        <asp:Label ID="Label4" Text="PAYMENT INSTRUCTION" runat="server" CssClass="label" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTPAYMENTINSTRUCTION" runat="server" Height="80px" TextMode="MultiLine"
                                            Width="375px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trBENEFICIARYBANK" runat="server" visible="false">
                                    <td class="tdstyle">
                                        <asp:Label ID="Label5" Text="BENEFICIARY BANK" runat="server" CssClass="label" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTBENEFICIARYBANK" runat="server" Height="80px" TextMode="MultiLine"
                                            Width="375px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trRemarks" runat="server" visible="false">
                                    <td class="tdstyle">
                                        <asp:Label ID="Label6" Text="Remarks" runat="server" CssClass="label" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="490px"></asp:TextBox>
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
                                                OnSelectedIndexChanged="dgcurrency_SelectedIndexChanged" DataKeyNames="Sr.No"
                                                CssClass="grid-views" OnRowCreated="dgcurrency_RowCreated">
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
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
