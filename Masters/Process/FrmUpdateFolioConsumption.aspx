<%@ Page Title="Update Folio Consumption" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmUpdateFolioConsumption.aspx.cs" Inherits="Masters_Process_FrmUpdateFolioConsumption"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 300px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Folio No" runat="server" CssClass="labelbold" Width="100px" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtFolioNo" runat="server" CssClass="textb" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="ChkForDyeingConsumption" runat="server" Text="Check For Dyeing Consumption"
                                class="tdstyle" CssClass="checkboxbold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Button ID="btnupdateconsmp" runat="server" Text="Update Folio Consumption" CssClass="buttonnorm"
                                 OnClientClick="return cancelvalidation();" OnClick="btnupdateconsmp_Click" />
                        </td>
                        <td>
                            <asp:Button ID="BtnUpdateFolioReceiveConsumption" runat="server" Text="Update Receive Consumption" CssClass="buttonnorm"
                                 OnClientClick="return cancelvalidation();" OnClick="BtnUpdateFolioReceiveConsumption_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
