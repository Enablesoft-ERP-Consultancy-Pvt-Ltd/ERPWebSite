<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmRawMaterialIssueDetailProcessWise.aspx.cs"
    Inherits="Masters_ReportForms_FrmRawMaterialIssueDetailProcessWise" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
//        function ShowDates() {

//            var Trdates = document.getElementById('<%=trDates.ClientID %>');
//            if (document.getElementById('<%=ChkForDate.ClientID %>').checked = true) {
//                Trdates.style.display = 'block';
//            }
//            else {
//                Trdates.style.display = 'none';
//            }

//        }
    </script>  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: none;
                            border-width: thin">
                            <%--<table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDAll" runat="server" Text="All" GroupName="OrderType" CssClass="labelbold"
                                            AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDOrder" runat="server" Text="Order" GroupName="OrderType" CssClass="labelbold"
                                            AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDReceive" runat="server" Text="Receive" GroupName="OrderType"
                                            CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDRawMaterial" runat="server" Text="Raw Material" GroupName="OrderType"
                                            CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDRawMaterialSupervisorWise" runat="server" Text="Raw Material Supervisor Wise"
                                            GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>--%>
                        </div>
                    </td>
                    <td>
                        <div style="float: left; width: 450px; height: 500px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>  
                                <tr id="TRUnits" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Units Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDUnits" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRProcessName" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                                <%--<tr id="TREmpName" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>--%>
                                <tr id="TRCategoryName" runat="server">
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
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
                                    </td>
                                </tr>
                                <tr id="TRDDColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDShape" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDSize" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
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
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" AutoPostBack="true"
                                            CssClass="checkboxbold" OnCheckedChanged="ChkForDate_CheckedChanged" Visible="false" />
                                    </td>
                                </tr>
                                <tr id="trDates" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtFromDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="To Date" Width="80px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtToDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm"
                                            Text="Preview" OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                            </table>
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
