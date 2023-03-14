<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmorderwisecapacitydistrpt.aspx.cs" Inherits="Masters_ReportForms_frmorderwisecapacitydistrpt" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
   
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 400px; margin-left: 220px; height: 300px; margin-top: 20px">
                <asp:Panel runat="server" ID="pp" Style="border: 1px groove Teal; background-color: #D3D3D3"
                    Width="400px" Height="400px">
                    <div style="padding-left: 5px">
                        <table style="width: 335px; height: 168px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcategoryname" runat="server" Text="Department" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDcategory" runat="server" Width="150px" CssClass="dropdown"
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
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="cmdorders" Text="Get Orders" CssClass="buttonnorm" runat="server"
                                        OnClick="cmdorders_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="Label1" runat="server" Text="OrderNo." CssClass="labelbold"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <div style="overflow: auto; height: 140px; width: 300PX">
                                        <asp:CheckBoxList ID="Chkorderno" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                            RepeatColumns="2" ForeColor="Blue">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                        OnClick="btnprint_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
