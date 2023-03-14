<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="ParameterSetting.aspx.cs" Inherits="Masters_ParameterSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closeForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="formPanal" runat="server">
        <ContentTemplate>
            <div id="div" runat="server" align="center">
                <table>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblCategory" runat="server" Text="CATEGORY" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtCategory" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblItem" runat="server" Text="ITEM" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtItem" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblQuality" runat="server" Text="QUALITY" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtQuality" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblDesign" runat="server" Text="DESIGN" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtDesign" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblColor" runat="server" Text="COLOR" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtColor" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblColorShade" runat="server" Text="SHADECOLOR" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtShadeColor" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblShape" runat="server" Text="SHAPE" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtShape" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tdstyle">
                            <asp:Label ID="LblSize" runat="server" Text="SIZE" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtSize" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Label ID="Lblmsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return closeForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
