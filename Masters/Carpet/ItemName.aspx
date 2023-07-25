<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemName.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Campany_ItemName" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function ItemSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function getbacktostepone() {
            window.location = "ItemName.aspx.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function AddJobs() {
            var answer = confirm("Do you want to ADD?");
            if (answer) {
                var a = document.getElementById('CPH_Form_txtid').value;

                if (a == "" || a == "0") {
                    alert('Plz Select or Insert Item');
                    return false;
                }
                var left = (screen.width / 2) - (650 / 2);
                var top = (screen.height / 2) - (400 / 2);

                //                window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
                window.open('frmAddItemProcess.aspx?a=' + a, 'Add Jobs', 'width=650px, height=400px, top=' + top + ', left=' + left);
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="height: 500px">
                <asp:TextBox ID="txtid" runat="server" Width="0px" Text="0"></asp:TextBox>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="CATEGORY" Font-Bold="true"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="ddCategory" runat="server" Width="150px"
                                OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="Plese Select Category" ControlToValidate="ddCategory"
                                runat="server" ForeColor="red" InitialValue="0">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td align="right" class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="ITEM NAME" Font-Bold="true"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtItemName" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemName"
                                ErrorMessage="Please Enter Item Name" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:AutoCompleteExtender ID="txtItemName_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetItemName" CompletionSetCount="20"
                                OnClientItemSelected="ItemSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtItemName"
                                UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:CheckBox ID="ChkFlagFixWeight" CssClass="checkboxbold" runat="server" Text="For Fix Or Weight" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="chkpretreat" Visible="false" CssClass="checkboxbold" runat="server"
                                Text="Receipe Pre Treatment" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="UNIT TYPE" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="ddUnit" runat="server" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddUnit"
                                ErrorMessage="Please Select Unit Type" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddUnit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td align="right" class="tdstyle">
                            <asp:Label ID="lblitemcode" runat="server" Text="ITEM CODE" Font-Bold="true"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCode" runat="server" CssClass="textb" onkeypress="return isNumberKey(event)"
                                onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                        <td id="TDkatiwithexportsize" runat="server" visible="false">
                            <asp:CheckBox ID="chkkatiwithexportsize" CssClass="checkboxbold" runat="server" Text="Kati With Export Size" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="chkchem" Visible="false" CssClass="checkboxbold" runat="server"
                                Text="Dye Bath" />
                        </td>
                    </tr>
                    <tr id="tr2" runat="server" visible="true">
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text="ITEM TYPE" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="ddlItemType" runat="server" Width="150px" />
                        </td>
                        <td id="TDMasterQualityType" align="left" class="tdstyle" runat="server" visible="false">
                            <asp:Label ID="Label3" runat="server" Text="Master QualityType" Font-Bold="true"></asp:Label>
                        </td>
                        <td id="TDMasterQualityType2" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDMasterQualityType" runat="server" Width="150px" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkForCushionTypeItem" CssClass="checkboxbold" runat="server" Text="Cushion Type Item" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkForAllDesignAllColorAndSizeWiseConsumption" CssClass="checkboxbold" runat="server"
                                Text="For All Design All Color And Size Wise Consumption" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkForAllDesignAllColorAllSizeWiseConsumption" CssClass="checkboxbold" runat="server"
                                Text="For All Design All Color All Size Wise Consumption" />
                        </td>
                        

                        <%-- <td class="tdstyle">
                            <asp:CheckBox ID="chkdyesstuff" Visible="false" CssClass="checkboxbold" runat="server" Text="Dyes Stuff" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="4" class="style1" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                            <asp:ValidationSummary ID="vs" runat="server" HeaderText="Mandatory fields:" ShowMessageBox="true"
                                ShowSummary="false" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                            <asp:Button CssClass="buttonnorm" ID="btnAddItem_Process" runat="server" OnClientClick=" return AddJobs();"
                                Text="Add Jobs" Width="70" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" OnClientClick="CloseForm();"
                                Text="Close" ValidationGroup="l" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="4">
                            <div style="height: 250px; overflow: scroll; width: 100%;">
                                <asp:Label ID="Lblerr" runat="server" Text="" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="gdItem" runat="server" Width="100%" OnSelectedIndexChanged="gdItem_SelectedIndexChanged"
                                    OnRowDataBound="gdItem_RowDataBound" PageSize="4" DataKeyNames="Sr_No" CssClass="grid-views"
                                    OnRowCreated="gdItem_RowCreated">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
