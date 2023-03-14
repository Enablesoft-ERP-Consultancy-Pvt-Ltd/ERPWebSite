<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customer_Vender_Dyerbank.aspx.cs"
    Inherits="Masters_Campany_Customer_vender_Dyerbank" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
            //parent.document.location.reload();
        }
        function RefreshCombo() {
            if (window.opener.document.getElementById('x')) {
                window.opener.document.getElementById('x').click();
                self.close();
            }
            else if (window.opener.document.getElementById('Button3')) {
                window.opener.document.getElementById('Button3').click();
                self.close();
            }

        }
        function closepartybank() {
            self.close();
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function validateSave() {
            alert('hi')
            var varacno = document.getElementById('txtacno').value;
            if (varacno == "") {
                alert('Please select Acno ...........');

            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 60px">
                <table>
                    <tr>
                        <td class="tdstyle">
                            Bank<br />
                            <asp:DropDownList ID="DDbank" runat="server" Width="170px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ReqBank" runat="server" ControlToValidate="DDbank"
                                ForeColor="Red" ValidationGroup="f1" SetFocusOnError="true">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="tdstyle">
                            A/c Type<br />
                            <asp:DropDownList ID="DDactype" runat="server" Width="170px" CssClass="dropdown">
                                <asp:ListItem Value="1">CURRENT</asp:ListItem>
                                <asp:ListItem Value="2">SAVING</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ReqAcType" ControlToValidate="DDactype" runat="server"
                                ForeColor="Red" ValidationGroup="f1" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Currency<br />
                            <asp:DropDownList ID="DDcurrency" runat="server" Width="170px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ReqDDCurrency" runat="server" ControlToValidate="DDCurrency"
                                SetFocusOnError="true" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="tdstyle">
                            A/c No.<br />
                            <asp:TextBox ID="txtacno" runat="server" Width="170px" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldtxtAcno" runat="server" ControlToValidate="txtacno"
                                ErrorMessage="Plz Enter Ac/No" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Iban
                            <br />
                            <asp:TextBox ID="txtibanno" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldtxtibanno" runat="server" ControlToValidate="txtibanno"
                                ErrorMessage="Plz Enter Iban" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-left: 60px">
                <table>
                    <tr>
                        <td style="width: 350px" align="right">
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" Text="Save" ValidationGroup="f1"
                                OnClick="BtnSave_Click" />
                            <asp:Button ID="BtnClose" CssClass="buttonnorm" runat="server" Text="Close" OnClientClick="return closepartybank();" />
                            <asp:Button ID="Btndelete" CssClass="buttonnorm" runat="server" Text="Delete" OnClick="Btndelete_Click"
                                OnClientClick="return confirm('Do you want to Delete data?')" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="width: 396px; height: 149px; overflow: scroll">
                                <asp:GridView ID="Gvcvd" runat="server" CssClass="grid-view" DataKeyNames="Detailid"
                                    OnRowCreated="Gvcvd_RowCreated" OnRowDataBound="Gvcvd_RowDataBound" AutoGenerateColumns="false"
                                    Width="373px" OnSelectedIndexChanged="Gvcvd_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                        <asp:BoundField DataField="AcType" HeaderText="Ac Type" />
                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" />
                                        <asp:BoundField DataField="AcNo" HeaderText="AcNo" />
                                        <asp:BoundField DataField="Iban" HeaderText="IBAN" />
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
