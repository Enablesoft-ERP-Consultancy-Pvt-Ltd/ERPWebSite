<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmsalereport.aspx.cs" Inherits="Masters_ReportForms_frmsalereport"
    MasterPageFile="~/ERPmaster.master" Title="Sale Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 366px; margin-left: 250px">
                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="350px">
                        <div style="padding: 0px 0px 0px 20px">
                            <table style="height: 150px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="200px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMonthYaer" runat="server" Text="From Month/Year" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDFMonth" runat="server" CssClass="dropdown" Width="100px">
                                            <asp:ListItem Value="1">JAN</asp:ListItem>
                                            <asp:ListItem Value="2">FEB</asp:ListItem>
                                            <asp:ListItem Value="3">MAR</asp:ListItem>
                                            <asp:ListItem Value="4">APR</asp:ListItem>
                                            <asp:ListItem Value="5">MAY</asp:ListItem>
                                            <asp:ListItem Value="6">JUN</asp:ListItem>
                                            <asp:ListItem Value="7">JUL</asp:ListItem>
                                            <asp:ListItem Value="8">AUG</asp:ListItem>
                                            <asp:ListItem Value="9">SEP</asp:ListItem>
                                            <asp:ListItem Value="10">OCT</asp:ListItem>
                                            <asp:ListItem Value="11">NOV</asp:ListItem>
                                            <asp:ListItem Value="12">DEC</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:DropDownList ID="DDFyear" runat="server" CssClass="dropdown" widh="100px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblToMonthyear" runat="server" Text="To Month/Year" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDToMonth" runat="server" CssClass="dropdown" Width="100px">
                                            <asp:ListItem Value="1">JAN</asp:ListItem>
                                            <asp:ListItem Value="2">FEB</asp:ListItem>
                                            <asp:ListItem Value="3">MAR</asp:ListItem>
                                            <asp:ListItem Value="4">APR</asp:ListItem>
                                            <asp:ListItem Value="5">MAY</asp:ListItem>
                                            <asp:ListItem Value="6">JUN</asp:ListItem>
                                            <asp:ListItem Value="7">JUL</asp:ListItem>
                                            <asp:ListItem Value="8">AUG</asp:ListItem>
                                            <asp:ListItem Value="9">SEP</asp:ListItem>
                                            <asp:ListItem Value="10">OCT</asp:ListItem>
                                            <asp:ListItem Value="11">NOV</asp:ListItem>
                                            <asp:ListItem Value="12">DEC</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:DropDownList ID="DDToyear" runat="server" CssClass="dropdown" widh="100px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:CheckBox ID="chkForCompare" runat="server" Text=" Check For Compare" CssClass="checkboxbold" />
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
