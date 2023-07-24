<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterDesign.ascx.cs"
    Inherits="UserControls_MasterDesign" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function DesignSelected(source, eventArgs) {
        document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
    }
    function closeForm() {

        var objParent = window.opener;
        if (objParent != null) {
            if (window.opener.document.getElementById('refreshdesign')) {
                window.opener.document.getElementById('refreshdesign').click();
                self.close();
            }

        }
        else {
            window.location.href = "../../main.aspx";
        }



    }
    function priview() {
        window.open("../../ReportViewer.aspx");
    }
    function KeyDownHandler(btn) {

        var objParent = window.opener;
        if (objParent != null) {
            btn = "usercontrol_btnsearch";
        }
        else {
            btn = "CPH_Form_usercontrol_btnsearch"
        }
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;
            window.document.getElementById(btn).click();
        }
    }
</script>
<div style="margin-left: 15%; margin-right: 15%">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<asp:UpdatePanel ID="updatepanel1" runat="server">
    <ContentTemplate>
        <div style="margin: 1% 20% 0% 20%">
            <table>
                <tr>
                    <td class="tdstyle">
                    </td>
                    <td>
                        <asp:TextBox ID="txtsearchdesign" runat="server" CssClass="textb" placeholder="Enter design to search"
                            Width="200px" AutoPostBack="true" onKeyDown="KeyDownHandler('btnsearch');"></asp:TextBox>
                        <asp:Button ID="btnsearch" runat="server" Style="display: none" OnClick="txtsearchdesign_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lbldesignname" runat="server" Text="DesignName" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtDesign" runat="server" CssClass="textb" ValidationGroup="a" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Design Name"
                            ControlToValidate="txtDesign" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <cc1:AutoCompleteExtender ID="txtDesign_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                            CompletionInterval="20" Enabled="True" ServiceMethod="GetDesignName" CompletionSetCount="20"
                            OnClientItemSelected="DesignSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDesign"
                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                        </cc1:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lblDesignCode" runat="server" Text="DesignCode" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesignCode" runat="server" CssClass="textb" ValidationGroup="a"
                            Width="200px"></asp:TextBox>
                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Design Code"
                            ControlToValidate="txtDesignCode" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:Button ID="Button1" runat="server" CssClass="buttonnorm preview_width" OnClick="Button1_Click"
                            Text="Preview" UseSubmitBehavior="False" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                            OnClick="btnsave_Click" ValidationGroup="a" />
                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                            UseSubmitBehavior="False" />
                        <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                            CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                        <div style="max-height: 500px; overflow: auto">
                            <asp:GridView ID="gdDesign" runat="server" Width="400px" AllowPaging="True" PageSize="30"
                                AutoGenerateColumns="false" DataKeyNames="Sr_No" OnPageIndexChanging="gdDesign_PageIndexChanging"
                                OnRowDataBound="gdDesign_RowDataBound" OnSelectedIndexChanged="gdDesign_SelectedIndexChanged"
                                OnInit="gdDesign_Init">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Design Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesign" Text='<%#Bind("DesignName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Design Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignCode" Text='<%#Bind("DesignCode") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enable/Disable" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDesign_ED" Text='<%#Bind("Status") %>' runat="server" OnClick="lnkDesign_ED"
                                                OnClientClick="return confirm('Do you want to Enable_Disable Design')" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignenable_disable" Text='<%#Bind("Enable_Disable") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
