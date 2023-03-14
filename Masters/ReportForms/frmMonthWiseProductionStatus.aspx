<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmMonthWiseProductionStatus.aspx.cs"
    Inherits="Masters_ReportForms_frmMonthWiseProductionStatus" MasterPageFile="~/ERPmaster.master"
    Title="MONTHLY PRODUCTION STATUS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../FrmCmpRawMaterialStock.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
      
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table style="width: 89%">
                <tr style="width: 100%">
                    <td style="width: 80px">
                    </td>
                    <td>
                        <div style="width: 800px; height: 300px;">
                            <div style="width: 700px; height: auto; margin-left: 70px; margin-top: 20px">
                                <div style="float: left">
                                    <asp:Panel ID="Panel2" runat="server" Style="border: 1px groove Teal; border: 1px Solid black"
                                        Width="250px" Height="156px">
                                        <table style="height: 80px">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="RDProduction" runat="server" Text="Production Order Status"
                                                        GroupName="M" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDProduction_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="RDOrder" runat="server" Text="Customer Order Status" GroupName="M"
                                                        CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDOrder_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="RDCustmonthlyStatus" runat="server" Text="Customer Monthly Order Status"
                                                        GroupName="M" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDCustmonthlyStatus_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div style="float: right">
                                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                                        Width="400px">
                                        <div style="padding: 0px 0px 0px 20px">
                                            <table>
                                                <tr id="TRDDCompany" runat="server">
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                            Width="250px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDCustName" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                                                    </td>
                                                    <td colspan="3" class="style2">
                                                        <asp:DropDownList ID="DDcustomer" runat="server" CssClass="dropdown" Width="250px"
                                                            OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDOrder" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblOrder" runat="server" CssClass="labelbold" Text="Order"></asp:Label>
                                                    </td>
                                                    <td colspan="3" class="style2">
                                                        <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TRCategory" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRddItemName" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="ddItemName" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDQuality" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDDesign" runat="server">
                                                    <td>
                                                        <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDColor" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDShadeColor" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDShape" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDShape" runat="server" CssClass="dropdown" Width="150px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                        <asp:CheckBox ID="chkmtr" runat="server" Text="Check For Mtr" CssClass="checkboxbold"
                                                            OnCheckedChanged="chkmtr_CheckedChanged" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr id="TRDDSize" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDLotNo" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="LblLotNo" runat="server" CssClass="labelbold" Text="Lot No"></asp:Label>
                                                    </td>
                                                    <td colspan="3" class="style2">
                                                        <asp:DropDownList ID="DDLotNo" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TRDDGodow" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblGudownname" runat="server" CssClass="labelbold" Width="90px" Text="Godown Name"></asp:Label>
                                                    </td>
                                                    <td colspan="3" class="style2">
                                                        <asp:DropDownList ID="DDGudown" runat="server" CssClass="dropdown" Width="250px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TRDate" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="TxtDate">
                                                        </asp:CalendarExtender>
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
                                                    <td colspan="4">
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="right">
                                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                                            OnClick="BtnPreview_Click" />
                                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                                            OnClientClick="return  CloseForm();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
