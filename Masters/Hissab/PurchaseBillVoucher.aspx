<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBillVoucher.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_Hissab_PurchaseBillVoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function validate() {
            var pp = document.getElementById('<%=DDPartyName.ClientID %>');
            var bill = document.getElementById('<%=ddbillno.ClientID %>');
            var cr = document.getElementById('<%=DDcr.ClientID %>');
            var payment = document.getElementById('<%=ddpaymentby.ClientID %>');
            var pay = document.getElementById('<%=TxtPayment.ClientID %>');
            if (pp.value == 0) {
                alert("PLZ Select Party Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            else if (bill.value == 0) {
                alert("PLZ Select Bill No");
                document.getElementById("<%=ddbillno.ClientID %>").focus();
                return false;
            }
            else if (cr.value == 0) {
                alert("PLZ Select Cr or Dr");
                document.getElementById("<%=DDcr.ClientID %>").focus();
                return false;
            }
            else if (payment.value == 0) {
                alert("PLZ Select PaymentBy");
                document.getElementById("<%=ddpaymentby.ClientID %>").focus();
                return false;
            }
            else if (pay.value == 0) {
                alert("Payment IS Zero");
                return false;
            }



            else if (payment.value == 1) {
                var bank = document.getElementById('<%=Txtbank.ClientID %>');
                var cheque = document.getElementById('<%=Txtcheque.ClientID %>');
                var narr = document.getElementById('<%=Txtnarration.ClientID %>');
                if (bank.value == "") {
                    alert("Bank Name cannot Be Blank ");
                    document.getElementById("<%=Txtbank.ClientID %>").focus();
                    return false;
                }
                else if (cheque.value == "") {
                    alert("Cheque No. cannot Be Blank ");
                    document.getElementById("<%=Txtcheque.ClientID %>").focus();
                    return false;
                }
                else if (narr.value == "") {
                    alert("Narration cannot Be Blank ");
                    document.getElementById("<%=Txtnarration.ClientID %>").focus();
                    return false;
                }
            }

            else {
                return confirm('Do You Want To Save?')
            }
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        } 
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            CompanyName<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="DDCompanyName" ErrorMessage="please fill Company......." ForeColor="Red"
                                SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" TabIndex="2">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            PartyName
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged"
                                Width="200px" TabIndex="3" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Bill No.
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="ddbillno" runat="server" OnSelectedIndexChanged="ddbillno_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="3">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddbillno"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Date
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            Voucher No.
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtvoucherNo" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Cr/Dr
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcr" runat="server" Width="100px" TabIndex="2">
                                <asp:ListItem Value="0">--Cr/Dr--</asp:ListItem>
                                <asp:ListItem Value="1" Selected>Cr</asp:ListItem>
                                <asp:ListItem Value="2">Dr</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            Payment
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtPayment" Enabled="false" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            T.D.S
                            <br />
                            <asp:TextBox CssClass="textb" ID="Txttds" runat="server" Width="100px" AutoPostBack="True"
                                OnTextChanged="Txttds_TextChanged"></asp:TextBox>
                        </td>
                        <td colspan="2" class="tdstyle">
                            Amount
                            <br />
                            <asp:TextBox CssClass="textb" ID="Txtamount" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Payment By
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="ddpaymentby" runat="server" Width="100px"
                                TabIndex="2" OnSelectedIndexChanged="ddpaymentby_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">--Payment By--</asp:ListItem>
                                <asp:ListItem Value="1">Cheque</asp:ListItem>
                                <asp:ListItem Value="2">Cash</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="tdbank" visible="false" runat="server" class="tdstyle">
                            Bank Name
                            <br />
                            <asp:TextBox CssClass="textb" ID="Txtbank" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td id="tdcheque" visible="false" runat="server" class="tdstyle">
                            Cheque No.
                            <br />
                            <asp:TextBox CssClass="textb" ID="Txtcheque" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td colspan="2" id="tdnarr" visible="false" runat="server" class="tdstyle">
                            Narration
                            <br />
                            <asp:TextBox CssClass="textb" ID="Txtnarration" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text=""
                                Visible="false"></asp:Label>
                        </td>
                        <td align="right" colspan="2">
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Submit" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return validate()" ValidationGroup="f1" TabIndex="45" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                            <asp:Button CssClass="buttonnorm" ID="btnprivew" runat="server" Enabled="false" Text="Preivew"
                                OnClientClick="return Preview();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:GridView ID="DGVOUCHERDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                OnSelectedIndexChanged="DGVOUCHERDetail_SelectedIndexChanged" OnRowCommand="DGVOUCHERDetail_RowCommand"
                                OnRowDataBound="DGVOUCHERDetail_RowDataBound" CssClass="grid-view" OnRowCreated="DGVOUCHERDetail_RowCreated">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="vouchorno" HeaderText="Voucher NO" />
                                    <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                    <asp:BoundField DataField="date" HeaderText="Date" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
