<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBuyingAgent.aspx.cs" Inherits="BuyingAgent"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--User controls regertration--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_addbying').click();
            self.close();
        }
        function CloseForm() {
            window.opener.document.getElementById('addbying').click();
            self.close();
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


                if (document.getElementById('ddbuyinghouse').options.length == 0) {
                    alert("Buyinghouse Name must have a value....!");
                    document.getElementById("ddbuyinghouse").focus();
                    return false;
                }
                else if (document.getElementById('ddbuyinghouse').options[document.getElementById('ddbuyinghouse').selectedIndex].value == 0) {
                    alert("Please select Buyinghouse Name....!");
                    document.getElementById("ddbuyinghouse").focus();
                    return false;
                }
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <style type="text/css">
        .style2
        {
            width: 152px;
        }
        .textbox
        {
        }
        .button
        {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <%--Page Working table--%>
    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="tdstyle">
                        <asp:Label Text=" Buying House" runat="server" ID="lbl2" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddbuyinghouse" runat="server" CssClass="dropdown" Width="172px">
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="addbuyinghouse" runat="server" CssClass="buttonsmalls" OnClientClick="return Addbuyinghouse();"
                            Text="&#43;" Width="35px" />
                        <asp:Button ID="btnbuyinghouseClose" runat="server" Style="display: none" EnableTheming="true"
                            OnClick="btnbuyinghouseClose_Click" />
                    </td>
                    <td class="tdstyle">
                        Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="textb" Width="172px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAgentName"
                            ErrorMessage="please enter AgentName" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="tdstyle">
                        Address
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="345px">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        Phone
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textb" Width="172px"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        Email
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textb" Width="172px"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="8">
                        <br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                            Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="8">
                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="gdBuyingAgent" runat="server" Width="840px" AllowPaging="True"
                            PageSize="6" OnRowDataBound="gdBuyingAgent_RowDataBound" OnSelectedIndexChanged="gdBuyingAgent_SelectedIndexChanged"
                            OnPageIndexChanging="gdBuyingAgent_PageIndexChanging" AutoGenerateColumns="False"
                            CssClass="grid-view" OnRowCreated="gdBuyingAgent_RowCreated">
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
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
                <tr>
                    <td colspan="2">
                    </td>
                    <td colspan="4" align="right">
                        <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                            Visible="false" OnClick="btndelete_Click" />
                        <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                            OnClick="btnsave_Click" ValidationGroup="m" />
                    </td>
                    <td>
                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();"
                            OnClick="btnclose_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Page Design--%>
    </form>
</body>
</html>
