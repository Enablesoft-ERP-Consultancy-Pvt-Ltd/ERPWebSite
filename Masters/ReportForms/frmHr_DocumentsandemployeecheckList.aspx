<%@ Page Title="Documents & Employee Check List" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmHr_DocumentsandemployeecheckList.aspx.cs"
    Inherits="Masters_ReportForms_frmHr_DocumentsandemployeecheckList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function empselected(source, eventArgs) {
            document.getElementById('<%=txtempid.ClientID%>').value = eventArgs.get_value();
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" cellspacing="2" cellpadding="5" width="100%">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="90%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Emp Code / Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:TextBox ID="txtempcode_name" Width="95%" CssClass="textb" placeholder="Type here Emp. Code / Employee Name"
                                runat="server" />
                            <asp:TextBox ID="txtempid" runat="server" Style="display: none"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtempcode_name_AutoCompleteExtender" runat="server"
                                BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeAll_HR"
                                EnableCaching="true" CompletionSetCount="20" ServicePath="~/Autocomplete.asmx"
                                OnClientItemSelected="empselected" TargetControlID="txtempcode_name" UseContextKey="True"
                                ContextKey="0#0#0" MinimumPrefixLength="1">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" Text="Documents" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDDocuments" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; border-style: dotted" colspan="2" align="right">
                            <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; border-style: dotted" colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
