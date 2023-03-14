<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmWeavingQualityWiseProductionArea.aspx.cs"
    Inherits="Masters_ReportForms_FrmWeavingQualityWiseProductionArea" MasterPageFile="~/ERPmaster.master"
    Title="Quality Wise Production Area Report" EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <td>
                        <%--<td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                                border-width: thin">
                                &nbsp;&nbsp;
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDWeaverRawMaterial" runat="server" Text="Weaver Raw Material" GroupName="OrderType" AutoPostBack="true"
                                    CssClass="labelbold" OnCheckedChanged="RDWeaverRawMaterial_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDWeaverRawLedger" Text="Weaver Raw Ledger" runat="server" AutoPostBack="true"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDWeaverRawLedger_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp; 
                                 <asp:RadioButton ID="RDWeaverRawMaterialIssueReceive" Text="Weaver Raw Material Issue/Receive" runat="server" AutoPostBack="true"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDWeaverRawMaterialIssueReceive_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;  
                                 <asp:RadioButton ID="RDFinisherRawMaterialIssueReceive" Text="Finisher Raw Material Issue/Receive" runat="server" AutoPostBack="true"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDFinisherRawMaterialIssueReceive_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;          
                            </div>
                        </td>--%>
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: none;">
                            &nbsp;&nbsp;
                        </div>
                        <div>
                            <div style="float: left; width: 450px;">
                                <%--<div style="float: left; width: 450px; height: 500px;">--%>
                                <%--<asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="100%">--%>
                                <div style="padding: 0px 0px 0px 20px">
                                    <table style="height: 150px">
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
                                                <asp:Label ID="lblWeaverName" Text="Weaver/ContractorName" runat="server" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDWeaverName" runat="server" Width="350px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDWeaverName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRQualityType" runat="server" visible="true">
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
                                                        RepeatDirection="Horizontal" RepeatColumns="4">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </tr>
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
