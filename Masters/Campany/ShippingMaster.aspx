<%@ Page Title="ShippingAgent" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="ShippingMaster.aspx.cs" Inherits="ShippingMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
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


                if (document.getElementById('CPH_Form_DDAgencyName').options.length == 0) {
                    alert("Agency Name must have a value....!");
                    document.getElementById("CPH_Form_DDAgencyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDAgencyName').options[document.getElementById('CPH_Form_DDAgencyName').selectedIndex].value == 0) {
                    alert("Please select Agency Name....!");
                    document.getElementById("CPH_Form_DDAgencyName").focus();
                    return false;
                }
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <div style="height: 450px">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
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
                            <asp:Label ID="lblAgent" runat="server" Text="Agent Name" CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter CompanyName"
                                ControlToValidate="txtCompanyName" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label4" runat="server" Text="Address" CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="300px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text=" Contact Person" CssClass="labelbold"
                                Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 148px;">
                            <asp:Label ID="LblPhone" runat="server" Text="Phone  No." CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox><br />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                                Type="Integer" ControlToValidate="txtPhone" Text="Text must be a number." ForeColor="Red"
                                SetFocusOnError="true" />
                        </td>
                        <td style="width: 148px;">
                            <asp:Label ID="Label3" runat="server" Text="Mobile" CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox><br />
                            <asp:CompareValidator ID="CompareValidator2" runat="server" Operator="DataTypeCheck"
                                Type="Integer" ControlToValidate="txtMobile" Text="Text must be a number." ForeColor="Red"
                                SetFocusOnError="true" />
                        </td>
                        <td style="width: 148px;">
                            <asp:Label ID="Label5" runat="server" Text="Email" CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                            <br />
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Fax" CssClass="labelbold" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox><br />
                            <asp:CompareValidator ID="CompareValidator3" runat="server" Operator="DataTypeCheck"
                                Type="Integer" ControlToValidate="txtFax" Text="Text must be a number." ForeColor="Red"
                                SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 148px;">
                            <asp:Label Text="Define Company" runat="server" ID="Label7" Font-Bold="true" />
                            <br />
                            <asp:DropDownList ID="DDdefinecompany" runat="server" CssClass="dropdown" Width="148px">
                                <asp:ListItem Value="1">CHA</asp:ListItem>
                                <asp:ListItem Value="2">FORWARDER</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                        <td style="width: 148px;">
                            <asp:Label Text="Mode Of Transaction" runat="server" ID="Label8" Font-Bold="true" />
                            <br />
                            <asp:DropDownList ID="DDmodeoftranaction" runat="server" CssClass="dropdown" Width="148px">
                                <asp:ListItem Value="1">SEA</asp:ListItem>
                                <asp:ListItem Value="2">AIR</asp:ListItem>
                                <asp:ListItem Value="3">DOORDELIVERY</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                        <td style="width: 148px;">
                            <asp:Label Text="Bank Information" runat="server" ID="Label9" Font-Bold="true" />
                            <br />
                            <asp:DropDownList ID="DDbankinformation" runat="server" CssClass="dropdown" Width="148px">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="8">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                                Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                                ShowSummary="true" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="8">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdShippingMaster" runat="server" OnRowDataBound="gdShippingMaster_RowDataBound"
                                OnSelectedIndexChanged="gdShippingMaster_SelectedIndexChanged" Width="840px"
                                AllowPaging="True" CellPadding="4" PageSize="6" ForeColor="#333333" OnPageIndexChanging="gdShippingMaster_PageIndexChanging"
                                DataKeyNames="Agentid" AutoGenerateColumns="False" CssClass="grid-views" OnRowCreated="gdShippingMaster_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
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
                                    <%--  <asp:TemplateField  Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgencyId" runat="server" Text='<%#Bind("AgencyId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="linktoCertainCustomer" runat="server" CommandName="LinktoCertainCustomer"
                                                Text="Customer Linking" OnClick="lnkbtnName_Click" Font-Size="12px">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="8">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return validate();"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close"
                                OnClientClick="return CloseForm();" OnClick="btnclose_Click" />
                            &nbsp;<asp:Button ID="Button1" runat="server" Text="Preview" OnClick="Button1_Click"
                                CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnModelPopup"
                        TargetControlID="btnModalPopUp" DropShadow="true" BackgroundCssClass="modalBackground"
                        CancelControlID="btnCancel" PopupDragHandleControlID="pnModelPopup" OnOkScript="onOk()">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                        border-radius: 12px; padding: 0" Height="241px" Width="330px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAgentName" runat="server" Text="" ForeColor="#cc3300" CssClass="labelbold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="overflow: auto; width: 320px; height: 100px; margin-top: 10px; height: 180px">
                            <asp:GridView ID="GDLinkedtoCustomer" runat="server" Width="290px" Style="margin-left: 10px"
                                ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" Height="20px" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" />
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <table width="300px">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" Width="100px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
