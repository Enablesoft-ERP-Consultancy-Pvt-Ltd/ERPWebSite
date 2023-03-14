<%@ Page Language="C#" AutoEventWireup="true" CodeFile="purchase_material_reprt.aspx.cs"
    MasterPageFile="~/ERPmaster.master" ViewStateMode="Enabled" EnableEventValidation="false"
    Inherits="Masters_Purchase_purchase_material_reprt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px">
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="Tdcomp" runat="server" class="tdstyle">
                            Company Name<br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdcustomer" runat="server">
                            <asp:Label ID="lblcusomername" class="tdstyle" runat="server" Text="Customer Code"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcustomer" runat="server" Width="250px" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="7" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomer"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdorder" runat="server" class="tdstyle">
                            Order No.<br />
                            <asp:DropDownList ID="ddOrderno" runat="server" Width="150px" TabIndex="8" AutoPostBack="True"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddOrderno"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdsupply" runat="server">
                            <asp:Label ID="lblemp" runat="server" class="tdstyle" Text="SUPPLIER NAME"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dsuppl" runat="server" Width="200px" TabIndex="9" CssClass="dropdown"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="dsuppl"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="Tr7">
                        <td colspan="4" align="right">
                            <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" TabIndex="22" CssClass="buttonnorm"
                                OnClick="btnpreview_Click" />
                            &nbsp;<asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                TabIndex="23" CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
