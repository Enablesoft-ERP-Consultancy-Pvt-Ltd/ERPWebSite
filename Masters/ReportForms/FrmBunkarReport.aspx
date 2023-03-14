<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmBunkarReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmBunkarReport" MasterPageFile="~/ERPmaster.master"
    Title="Bunkar Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top" align="left">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDBunkarDetail" runat="server" Text="Bunkar Detail" GroupName="OrderType"
                                CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDBunkarDetail_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDBunkarWise" runat="server" Text="Bunkar Wise" GroupName="OrderType"
                                CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDBunkarWise_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDQualityWise" runat="server" Text="Quality Wise" GroupName="OrderType"
                                CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDQualityWise_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDVoucherSummary" runat="server" Text="Voucher Summary" GroupName="OrderType"
                                CssClass="labelbold" />
                            <br />
                            <%--  &nbsp;&nbsp;
                                 <asp:RadioButton ID="RDFinishingReceiveDetail" runat="server" Text="Finishing Receive Detail"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingReceiveDetail_CheckedChanged" />                               
                                &nbsp;&nbsp; 
                            --%>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div style="float: left; width: 450px; vertical-align: top;">
                                <%--<div style="float: left; width: 450px; height: 500px;">--%>
                                <%--<asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="100%">--%>
                                <div style="padding: 0px 0px 0px 20px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRcustcode" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Contractor Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDContractorName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDContractorName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TR2" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Bunkar Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDBunkarName" runat="server" CssClass="dropdown" Width="300px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDBunkarName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRCategory" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRddItemName" runat="server">
                                            <td>
                                                <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
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
                                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDDesign" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShape" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDSize" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="250px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                                <%--  <asp:CheckBox ID="chkmtr" runat="server" Text="For Mtr." Font-Bold="true" OnCheckedChanged="chkmtr_CheckedChanged"
                                                AutoPostBack="true" />--%>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShadeColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDShadeColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="tr3" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Month"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDMonth" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Year" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDYear" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TR4" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Report Type"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDReportType" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                                                    <asp:ListItem Value="1">--Production--</asp:ListItem>
                                                    <asp:ListItem Value="2">--Sample--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="ChkForVoucher" runat="server" Text="Check For Voucher" Visible="true"
                                                    CssClass="checkboxbold" />
                                            </td>
                                        </tr>
                                        <%--  <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="ChkForFinisherNillDetail" runat="server" Text="Finisher Nill Detail" Visible="false"
                                                CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForFinisherNillDetail_CheckedChanged" />
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td align="right" colspan="4">
                                                &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                    OnClientClick="return Validate();" Text="Preview" />
                                                &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                    Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
