<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessType.aspx.cs" Inherits="Masters_Carpet_ProcessType"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="form" runat="server" ContentPlaceHolderID="CPH_Form">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript"></script>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    Process Name
                </td>
                <td>
                    <asp:DropDownList ID="ddprocessname" runat="server" Width="150px" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddprocessname"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    Process Type
                </td>
                <td>
                    <asp:TextBox ID="txtprocestype" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="Gvprocesstype" runat="server" CssClass="grid-view" OnRowCreated="Gvprocesstype_RowCreated">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" />
                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
