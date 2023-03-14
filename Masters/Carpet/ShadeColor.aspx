<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShadeColor.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_ShadeColor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ShadeColorSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
        } function addpriview() {
            window.open("../../ReportViewer.aspx")
        }
        function KeyDownHandler(btn) {

            var objParent = window.opener;
            if (objParent != null) {
                btn = "usercontrol_btnsearch";
            }
            else {
                btn = "CPH_Form_btnsearch"
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
                <td class="tdstyle">
                </td>
                <td>
                    <asp:TextBox ID="txtsearchdesign" runat="server" CssClass="textb" placeholder="Enter Shade color to search"
                        Width="200px" AutoPostBack="true" onKeyDown="KeyDownHandler('btnsearch');"></asp:TextBox>
                    <asp:Button ID="btnsearch" runat="server" Style="display: none" OnClick="txtsearchdesign_TextChanged" />
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label1" runat="server" Text="Color Box" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtColorBox" runat="server" onkeydown="return (event.keyCode!=13);"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="lblshadecolorname" runat="server" Text="Shade Color Name" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                 <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                    <asp:TextBox CssClass="textb" ID="txtcolor" runat="server" onkeydown="return (event.keyCode!=13);"
                        Width="200px"></asp:TextBox>
                         <cc1:AutoCompleteExtender ID="txtcolor_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetShadeColorName" CompletionSetCount="20"
                                                            OnClientItemSelected="ShadeColorSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtcolor"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </cc1:AutoCompleteExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label2" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtShadeColor" runat="server" onkeydown="return (event.keyCode!=13);"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right">
                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="buttonnorm" />
                    <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnnew_Click" CssClass="buttonnorm" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                        CssClass="buttonnorm" OnClick="btnclose_Click" />
                    <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                        CssClass="buttonnorm" />
                    <asp:Button ID="btnrpt" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                        OnClick="btnrpt_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 90%; height: 400px; overflow: scroll;">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="gdshadecolor" Width="100%" runat="server" OnRowDataBound="gdshadecolor_RowDataBound"
                            DataKeyNames="Sr_No" OnSelectedIndexChanged="gdshadecolor_SelectedIndexChanged"
                            PageSize="50" AllowPaging="true" OnPageIndexChanging="gdshadecolor_PageIndexChanging1">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
