<%@ Page Title="CategorySeprate" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="CategorySeparate.aspx.cs" Inherits="CategorySeparate" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <div id="maindiv" runat="server" style="margin-left: 15%; margin-top: 0px;">
        <asp:UpdatePanel ID="Panal1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblcategorytype" runat="server" Text="Category Type " Font-Bold="true"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDcategorytype" CssClass="dropdown" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDcategorytype_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="~~Select Value~~">--Select Value--</asp:ListItem>
                                <asp:ListItem Value="1">Raw Material</asp:ListItem>
                                <asp:ListItem Value="0">Finished Item</asp:ListItem>
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDcategorytype"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="style2">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" Font-Bold="true"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="DDcategoryName" CssClass="dropdown" runat="server" Width="150px"
                                OnSelectedIndexChanged="DDcategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDcategoryName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" />
                            <asp:Button ID="btnclose" runat="server" Text="Clsoe" OnClientClick="return CloseForm()"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height: 200px">
                                <asp:GridView ID="Gvcategory" runat="server" AutoGenerateColumns="true" DataKeyNames="Sr_No"
                                    AllowPaging="true" PageSize="10" OnPageIndexChanging="Gvcategory_PageIndexChanging"
                                    OnRowDataBound="Gvcategory_RowDataBound" OnSelectedIndexChanged="Gvcategory_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
