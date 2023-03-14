<%@ Page Title="PACKING UNIQUE BARCODE REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPackingUniqueBarCodeNoReport.aspx.cs" Inherits="Masters_ReportForms_FrmPackingUniqueBarCodeNoReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 20%">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCustCode" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRddItemName" runat="server">
                        <td>
                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDQuality" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDDesign" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDColor" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDShape" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDSize" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                            <br />
                            <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TrSelectDate" runat="server" visible="true">
                        <td>
                            <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                            <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                            <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
