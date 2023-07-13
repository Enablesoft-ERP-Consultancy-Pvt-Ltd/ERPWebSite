<%@ Page Title="Vendor Allocation Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="VendorAllocationReport.aspx.cs" Inherits="Masters_ReportForms_VendorAllocationReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=ddlCompanyname.ClientID %>").value <= "0") {
                alert("Plz select Company Name");
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 80%; margin-left: 25%">
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label Text="Company Name" ID="lblCompany" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompanyname" runat="server" Width="298px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblvendor" runat="server" CssClass="labelbold" Text="Vendor Name" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddvendorname" runat="server" CssClass="dropdown" Width="298px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" OnClick="btnprint_Click"
                                    OnClientClick="return validate()" Text="Print" />
                            </td>
                        </tr>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
