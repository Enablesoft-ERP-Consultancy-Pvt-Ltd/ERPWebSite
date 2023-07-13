<%@ Page Title="Vendor Allocation Month Wise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmVendorAllocationMonthwise.aspx.cs" Inherits="Masters_ReportForms_frmVendorAllocationMonthwise" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
   
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 400px; margin-left: 220px; height: 300px; margin-top: 20px">
                <asp:Panel runat="server" ID="pp" Style="border: 1px groove Teal; background-color: #D3D3D3"
                    Width="400px" Height="200px">
                    <div style="padding-left: 5px">
                        <table style="width: 335px; height: 168px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblVendor" runat="server" Text="VendorName" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDVendor" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblmonth" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="checkmonth" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                        RepeatColumns="3" ForeColor="Blue">
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown" Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" OnClick="btnprint_Click"
                                        Text="Print" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
