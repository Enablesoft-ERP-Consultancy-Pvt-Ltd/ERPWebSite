<%@ Page Title="Stock With Rate Amount Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmCompanyStockWithRateDetail.aspx.cs" Inherits="Masters_ReportForms_FrmCompanyStockWithRateDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../FrmCompanyStockWithRateDetail.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="width: 100%;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%" valign="top">
                        </td>
                        <td style="width: 70%" valign="top">
                            <div style="float: left; width: 90%; max-height: 600px;">
                                <table border="1" cellspacing="2">
                                    <tr id="TRDDCompany" runat="server">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDBranchName" runat="server" visible="true">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblBranchName" runat="server" CssClass="labelbold" Text="Branch Name"></asp:Label>
                                        </td>
                                        <td class="style2" colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDBranchName_SelectedIndexChanged"
                                                AutoPostBack="true" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDGodow" runat="server">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblGudownname" runat="server" CssClass="labelbold" Text="Godown Name"
                                                Width="90px"></asp:Label>
                                        </td>
                                        <td class="style2" colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDGodownName" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRcategory" runat="server">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDCategory_SelectedIndexChanged" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRddItemName" runat="server">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="ddItemName_SelectedIndexChanged" Width="250px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" PromptCssClass="labelbold"
                                                PromptPosition="Bottom" TargetControlID="ddItemName" ViewStateMode="Disabled">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDQuality" runat="server" visible="false">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDDesign" runat="server" visible="false">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" PromptCssClass="labelbold"
                                                PromptPosition="Bottom" TargetControlID="DDDesign" ViewStateMode="Disabled">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDColor" runat="server" visible="false">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDShadeColor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDShape" runat="server" visible="false">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDSize" runat="server" visible="false">
                                        <td style="border-style: dotted">
                                            <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                        </td>
                                        <td colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="border-style: dotted">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td align="right" colspan="2" style="border-style: dotted">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkRawMaterialDetail" CssClass="checkboxbold" Text="Raw Material as on date"
                                                            runat="server" AutoPostBack="true" OnCheckedChanged="chkallstockno_CheckedChanged" />
                                                    </td>
                                                    <td id="TDstockupto" runat="server" visible="false">
                                                        <asp:Label ID="lblstockupto" Text="Stock up to" CssClass="labelbold" runat="server" /><br />
                                                        <asp:TextBox ID="txtstockupto" CssClass="textb" Width="100px" runat="server" />
                                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtstockupto">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                            Text="Preview" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                            Text="Close" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
