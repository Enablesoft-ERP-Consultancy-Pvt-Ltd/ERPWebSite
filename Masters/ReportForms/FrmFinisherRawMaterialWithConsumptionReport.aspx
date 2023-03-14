<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmFinisherRawMaterialWithConsumptionReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmFinisherRawMaterialWithConsumptionReport" MasterPageFile="~/ERPmaster.master"
    Title="Finisher Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }

        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }

        var atLeast = 1
        function Validate() {
            var CHK = document.getElementById("<%=chekboxlist.ClientID%>");
            var checkbox = CHK.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {

                if (checkbox[i].checked) {

                    counter++;
                }

            }

            if (atLeast > counter) {
                alert("Please select atleast " + atLeast + " QualityType item(s)");
                return false;
            }
            return true;

        }
    </script>
    <%-- <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=BtnPreview.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDJobName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Job Name!!\n";
                    }                    
                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>--%>
    <%--<script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //DDItemName.Visible = true;
            //TRToDate.Visible = false;
            //ChkForFt.Visible = true;
            TRReportType.Visible = false;
            TRItemName.Visible = false;
            TRChkForSummary.Visible = false;
            TRQualityType.Visible = false;
            TRQualityTypeDropDown.Visible = false;
            TRGodown.Visible = false;
            if (RDWeaverRawMaterial.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = true;
                TRReportType.Visible = false;
                TRQualityType.Visible = true;
                TRQualityTypeDropDown.Visible = false;
                TRGodown.Visible = false;
            }
            else if (RDWeaverRawLedger.Checked == true)
            {
                TRItemName.Visible = false;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = false;
                TRQualityType.Visible = true;
                TRQualityTypeDropDown.Visible = false;
                TRGodown.Visible = false;
            }
            else if (RDWeaverRawMaterialIssueReceive.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = true;
                TRQualityType.Visible = false;
                TRQualityTypeDropDown.Visible = true;
                TRGodown.Visible = true;
            }
            else if (RDFinisherRawMaterialIssueReceive.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = true;
                TRQualityType.Visible = false;
                TRQualityTypeDropDown.Visible = true;
                TRGodown.Visible = true;
            }        
        }
    </script>--%>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript" language="javascript">
             Sys.Application.add_load(Jscriptvalidate);
            </script>--%>
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <br />
                            &nbsp;&nbsp;
                            <%-- <asp:RadioButton ID="RDWeaverRawMaterial" runat="server" Text="Weaver Raw Material" GroupName="OrderType" AutoPostBack="true"
                                    CssClass="labelbold" OnCheckedChanged="RDWeaverRawMaterial_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;--%>
                            <asp:RadioButton ID="RDFinisherRawLedger" Text="Finisher Raw Ledger" runat="server"
                                AutoPostBack="true" GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDFinisherRawLedger_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <%--<asp:RadioButton ID="RDWeaverRawMaterialIssueReceive" Text="Weaver Raw Material Issue/Receive" runat="server" AutoPostBack="true"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDWeaverRawMaterialIssueReceive_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;  
                                 <asp:RadioButton ID="RDFinisherRawMaterialIssueReceive" Text="Finisher Raw Material Issue/Receive" runat="server" AutoPostBack="true"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDFinisherRawMaterialIssueReceive_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp; --%>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div style="float: left; width: 450px;">
                                <%--<div style="float: left; width: 450px; height: 500px;">--%>
                                <%--<asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="100%">--%>
                                <div style="padding: 0px 0px 0px 20px">
                                    <table style="height: 150px">
                                        <%-- <tr id="TRReportType" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblReportType" Text="Report Type" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDReportType" runat="server" Width="350px" CssClass="dropdown">
                                        <asp:ListItem Value="0" Text="Issue">Issue</asp:ListItem>
                                        <asp:ListItem Value="1" Text="Receive">Receive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDCompany" runat="server" Width="350px" CssClass="dropdown">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" Text="Job Type" runat="server" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDJobName" runat="server" Width="350px" CssClass="dropdown"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDJobName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDJobName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblWeaverName" Text="Finisher Name" runat="server" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDWeaverName" runat="server" Width="350px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDWeaverName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRQualityType" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblQualityType" runat="server" Text="Quality Type" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td class="tdstyle">
                                                <div style="height: 120px; width: 80%; overflow: scroll">
                                                    <b>
                                                        <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" Text="Check All" /></b>
                                                    <br />
                                                    <asp:CheckBoxList ID="chekboxlist" runat="server" AutoPostBack="True" Width="600px"
                                                        CssClass="checkboxnormal" OnSelectedIndexChanged="chekboxlist_SelectedIndexChanged"
                                                        RepeatDirection="Horizontal" RepeatColumns="5">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- <tr id="TRQualityTypeDropDown" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblQualityTypeDD" Text="Quality Type" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQualityType" runat="server" Width="350px" CssClass="dropdown">                                        
                                        </asp:DropDownList>
                                    </td>
                                </tr> --%>
                                        <tr id="TRItemName" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblItemName" Text="Item Name" runat="server" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDItemName" runat="server" Width="350px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItemName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <%-- <tr id="TRGodown" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblGodownName" Text="Godown Name" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDGodownName" runat="server" Width="350px" CssClass="dropdown">                                        
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                        <tr>
                                            <td id="Tdselectdate" runat="server">
                                                <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold"
                                                    Checked="true" Enabled="false" />
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                                            <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td id="Tdtodatelabel" runat="server">
                                                            <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td id="Tdtodate" runat="server">
                                                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                                            <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="TRChkForSummary" runat="server" visible="false">
                                            <td id="Td1" runat="server">
                                                <asp:CheckBox ID="ChkForSummary" Text="For Summary" runat="server" CssClass="checkboxbold" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                                    OnClick="BtnPreview_Click" OnClientClick="return Validate()" />
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
                                <%--</asp:Panel>--%>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
