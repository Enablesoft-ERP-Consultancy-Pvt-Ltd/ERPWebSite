<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterFourNewParameter.ascx.cs"
    Inherits="UserControls_MasterFourNewParameter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
<link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function ColorSelected(source, eventArgs) {
        document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
    }
    function CloseForm() {
        var objParent = window.opener;
        if (objParent != null) {
            if (window.opener.document.getElementById('CPH_Form_refreshcolor')) {
                window.opener.document.getElementById('CPH_Form_refreshcolor').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshcolor')) {
                window.opener.document.getElementById('refreshcolor').click();
                self.close();
            }
        }
        else {
            window.location.href = "../../main.aspx";
        }
    }

    function addpriview() {
        window.open("../../ReportViewer.aspx")
    }        
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="margin: 1% 20% 0% 20%">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox CssClass="textb" ID="TxtType" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle" width="30%" style="text-align: right">
                        <asp:Label ID="lblcolorname" runat="server" Text="Name" CssClass="labelbold"></asp:Label>
                    </td>
                    <td width="50%" style="text-align: left; padding-left: 10px">
                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox CssClass="textb" ID="txtName" runat="server"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtcolor_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                            CompletionInterval="20" Enabled="True" ServiceMethod="GetColorName" CompletionSetCount="20"
                            OnClientItemSelected="ColorSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtName"
                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                        </cc1:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td style="text-align: left">
                        <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                            OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" Width="53px"
                            OnClientClick="return CloseForm();" />
                        <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
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
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        <div style="height: 400px; overflow: auto">
                            <asp:GridView ID="gdcolor" runat="server" DataKeyNames="Sr_No" OnRowDataBound="gdcolor_RowDataBound"
                                OnSelectedIndexChanged="gdcolor_SelectedIndexChanged" Width="400px">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
