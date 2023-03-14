<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmHomeFurnishingIssueProcessReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmHomeFurnishingIssueProcessReport" EnableEventValidation="false"
    Title="Home Furnishing Issue Process Report" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
           

            if (document.getElementById('CPH_Form_RDPendingQty').checked == true) {
                if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }


            }

        }
        
    </script>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="85%">
                    <tr>
                        <td style="width: 300px" valign="top">
                            <div style="width: 287px; padding-top: 5px; height: 350px; float: left; border-style:none;
                                border-width: thin">
                                &nbsp;&nbsp;
                                <br />                               
                               
                               <%-- &nbsp;&nbsp;
                                <asp:RadioButton ID="RDHomeFurnishingRecDetail" Text="Home Furnishing Receive Detail" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDHomeFurnishingRecDetail_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;--%>
                                
                            </div>
                        </td>
                        <td style="vertical-align:top">
                            <div style="float: left; width: 450px; max-height: 500px;">
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
                                    <tr id="TRProcessName" runat="server">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRcustcode" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Customer code"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDcustcode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRorderno" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Order No."></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TREmpName" runat="server" visible="true">
                                        <td>
                                            <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged" Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRRecChallan" runat="server">
                                        <td>
                                            <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Issue Challan No"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDChallanNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDChallanNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
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
                                                Width="300px">
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
                                                Width="300px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
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
                                            <asp:CheckBox ID="chkmtr" runat="server" Text="For Mtr." Font-Bold="true" OnCheckedChanged="chkmtr_CheckedChanged"
                                                AutoPostBack="true" />
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
                                    <tr id="TRlotNo" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="LblLotNo" runat="server" CssClass="labelbold" Text="Lot No."></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDLotNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDLotNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                 
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold" />
                                        </td>
                                    </tr>
                                    <tr id="trDates" runat="server">
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
                                            <asp:CheckBox ID="ChkForIssRecSummary" runat="server" Text="For Iss Rec Summary" CssClass="checkboxbold" />
                                        </td>
                                    
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                  
                                    <tr>                                       
                                       
                                        <td align="right" colspan="4">
                                            &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" 
                                                OnClick="BtnPreview_Click" OnClientClick="return Validate();" Text="Preview" />
                                            &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" 
                                                OnClick="BtnClose_Click" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
        <style type="text/css">
            #mask
            {
                position: fixed;
                left: 0px;
                top: 0px;
                z-index: 4;
                opacity: 0.4;
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
                filter: alpha(opacity=40); /* second!*/
                background-color: Gray;
                display: none;
                width: 100%;
                height: 100%;
            }
        </style>
        <script type="text/javascript" language="javascript">
            function ShowPopup() {
                $('#mask').show();
                $('#<%=pnlpopup.ClientID %>').show();
            }
            function HidePopup() {
                $('#mask').hide();
                $('#<%=pnlpopup.ClientID %>').hide();
            }
            $(".btnPwd").live('click', function () {
                HidePopup();
            });
        </script>
        <div id="mask">
        </div>
        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
            Style="z-index: 111; background-color: White; position: absolute; left: 35%;
            top: 40%; border: outset 2px gray; padding: 5px; display: none">
            <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
                <tr style="background-color: #8B7B8B; height: 1px">
                    <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                        align="center">
                        ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                            class="btnPwd" href="#">X</a>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Enter Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                            ValidationGroup="m" OnClick="btnCheck_Click" />
                        <input type="button" value="Cancel" class="btnPwd" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
