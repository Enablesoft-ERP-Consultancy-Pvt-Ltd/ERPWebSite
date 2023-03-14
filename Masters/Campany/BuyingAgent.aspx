<%@ Page Title="BuyingAgent" Language="C#" AutoEventWireup="true" CodeFile="BuyingAgent.aspx.cs"
    Inherits="BuyingAgent" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       
    </script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Addbuyinghouse() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var left = (screen.width / 2) - (800 / 2);
                var top = (screen.height / 2) - (500 / 2);
                //window.open('AddBuyingHouse.aspx', '', 'width=950px,Height=500px');
                window.open('AddBuyingHouse.aspx', 'ADD BUYING HOUSE', 'width=900px, height=400px, top=' + top + ', left=' + left);
            }
        }
        function validate() {
            var isPageValid = Page_ClientValidate('s');
            if (isPageValid) {


                if (document.getElementById('CPH_Form_ddbuyinghouse').options.length == 0) {
                    alert("Buyinghouse Name must have a value....!");
                    document.getElementById("CPH_Form_ddbuyinghouse").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddbuyinghouse').options[document.getElementById('CPH_Form_ddbuyinghouse').selectedIndex].value == 0) {
                    alert("Please select Buyinghouse Name....!");
                    document.getElementById("CPH_Form_ddbuyinghouse").focus();
                    return false;
                }
                return confirm('Do You Want To Save?')
            }
        }
       

    </script>
    <div style="height: 485px">
        <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text=" Buying House" runat="server" ID="lbl2" Font-Bold="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="addbuyinghouse"
                                runat="server" CssClass="buttonsmalls" OnClientClick="return Addbuyinghouse();"
                                Text="&#43;" Width="35px" />
                            <br />
                            <asp:DropDownList ID="ddbuyinghouse" runat="server" CssClass="dropdown" Width="172px">
                            </asp:DropDownList>
                            <asp:Button ID="btnbuyinghouseClose" runat="server" Style="display: none" EnableTheming="true"
                                OnClick="btnbuyinghouseClose_Click" />
                        </td>
                        <td>
                            <asp:Label Text="Buying Agent Name" runat="server" ID="Label1" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="txtAgentName" runat="server" CssClass="textb" Width="172px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAgentName"
                                ErrorMessage="please enter AgentName" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label Text=" Address" runat="server" ID="Label3" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="300px" TextMode="MultiLine"
                                onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text=" Email" runat="server" ID="Label5" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textb" Width="172px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Phone" runat="server" ID="Label4" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="textb" Width="172px"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" Operator="DataTypeCheck"
                                Type="Integer" ControlToValidate="txtPhone" Text="Text must be a number." ForeColor="Red"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="left">
                        <td class="style2" colspan="4">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                                Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                                ShowSummary="true" />
                        </td>
                    </tr>
                </table>
                <div>
                    <table width="83%">
                        <tr align="right">
                            <td colspan="4" align="right">
                                <asp:Button ID="btndelete" runat="server" Width="50px" Text="Delete" CssClass="buttonnorm"
                                    OnClientClick="return confirm('Do you want to Delete data?')" Visible="false"
                                    OnClick="btndelete_Click" />
                                <asp:Button ID="Button2" runat="server" Width="50px" Text="New" CssClass="buttonnorm"
                                    OnClick="Button2_Click" />
                                <asp:Button ID="btnsave" runat="server" Width="50px" CssClass="buttonnorm" Text="Save"
                                    OnClientClick="return validate();" OnClick="btnsave_Click" ValidationGroup="s" />
                                <asp:Button ID="Button1" runat="server" Width="70px" Text="Preview" CssClass="buttonnorm"
                                    OnClick="Button1_Click" />
                                <asp:Button ID="btnclose" runat="server" Width="50px" CssClass="buttonnorm" Text="Close"
                                    OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <table>
                    <tr align="left">
                        <td class="style2" colspan="8">
                            <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdBuyingAgent" runat="server" Width="840px" AllowPaging="True"
                                CellPadding="4" PageSize="6" ForeColor="#333333" GridLines="both" OnRowDataBound="gdBuyingAgent_RowDataBound"
                                OnSelectedIndexChanged="gdBuyingAgent_SelectedIndexChanged" OnPageIndexChanging="gdBuyingAgent_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="grid-views" OnRowCreated="gdBuyingAgent_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuyeingAgentId" runat="server" Text='<%#Bind("BuyeingAgentId")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BuyeingAgentName" HeaderText="BuyeingAgent Name" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuyingHouseId" runat="server" Text='<%#Bind("BuyingHouseId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
