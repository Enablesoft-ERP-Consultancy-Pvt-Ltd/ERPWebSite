<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportJobWise_Rec_Pending.aspx.cs"
    Inherits="Masters_ReportForms_frmReportJobWise_Rec_Pending" MasterPageFile="~/ERPmaster.master"
    Title="JOB WISE RECEIVE/PENDING/PACKING" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
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
        function validate() {

            if (document.getElementById("<%=TRLoomBal.ClientID %>")) {
                if (document.getElementById("<%=RDLoombal.ClientID %>").checked) {
                    if (document.getElementById("<%=DDjob.ClientID %>").selectedIndex <= "0") {
                        alert('Plz Select Job...')
                        document.getElementById("<%=DDjob.ClientID %>").focus()
                        return false;
                    }
                }
            }
            if (document.getElementById("<%=DDUnitName.ClientID %>").selectedIndex < "0") {
                alert('Plz Select Unit...')
                document.getElementById("<%=DDUnitName.ClientID %>").focus()
                return false;
            }
            if (document.getElementById("<%=DDArticle.ClientID %>").selectedIndex < "0") {
                alert('Plz Select Article...')
                document.getElementById("<%=DDArticle.ClientID %>").focus()
                return false;
            }
        }
        function Toggle(sender, CName) {


            if (CName == "RDPendingQty") {
                document.getElementById('CPH_Form_TRJob').style.visibility = sender.checked ? "visible" : "hidden";
                document.getElementById('CPH_Form_TRDate').style.visibility = sender.checked ? "hidden" : "visible";
                alert(sender.checked)


            }
            else if (CName == "RDRecReport") {

                document.getElementById('CPH_Form_TRDate').style.visibility = sender.checked ? "visible" : "hidden";
                document.getElementById('CPH_Form_TRJob').style.visibility = sender.checked ? "visible" : "hidden";
            }
            else if (CName == "RDPacking") {

                document.getElementById('CPH_Form_TRDate').style.visibility = sender.checked ? "visible" : "hidden";
                document.getElementById('CPH_Form_TRJob').style.visibility = sender.checked ? "hidden" : "visible";
            }
        }
    </script>
    <script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TDForLoomStockNo.Visible = false;
            TRFolioNo.Visible = false;
            TRItemName.Visible = false;
            TRDDQuality.Visible = false;
            TRDDDesign.Visible = false;
            TRDDColor.Visible = false;
            TRDDShadeColor.Visible = false;
            TRDDShape.Visible = false;
            TRDDSize.Visible = false;
            TRDate.Visible = false;
            TRJob.Visible = false;
            TRUnitName.Visible = false;
            TDlabeltodate.Visible = true;
            TDtxttodate.Visible = true;
            lblFromDate.Text = "From Date";
            TrLoomNo.Visible = false;
            DDjob.Enabled = true;
            chkexcelexport.Checked = false;
            chkexcelexport.Visible = false;
            TDcheckwithstockdetail.Visible = false;
            chkwithstockdetail.Checked = false;
            Trarticles.Visible = true;
            TDChkWithLotTag.Visible = false;
            if (RDRecReport.Checked == true)
            {
                TRDate.Visible = true;
                TRJob.Visible = true;
                TRUnitName.Visible = true;
            }
            if (RDPendingQty.Checked == true)
            {
                TRDate.Visible = true;
                TRJob.Visible = true;
                TRUnitName.Visible = true;
                TDlabeltodate.Visible = false;
                TDtxttodate.Visible = false;
                lblFromDate.Text = "Up to";
                if (Session["varcompanyId"].ToString() != "8")
                {
                    chkexcelexport.Visible = true;
                }
            }
            if (RDPacking.Checked == true)
            {
                TRJob.Visible = false;
                TRDate.Visible = true;
                TRUnitName.Visible = false;
            }
            if (RDIssueDetail.Checked == true)
            {
                TRDate.Visible = true;
                TRJob.Visible = true;
                TRUnitName.Visible = true;
                if (Session["varcompanyId"].ToString() != "8")
                {
                    chkexcelexport.Visible = true;
                }

                if (Session["varCompanyNo"].ToString() == "22")
                {
                    TDForLoomStockNo.Visible = true;
                }
            }
            if (RDLoombal.Checked == true)
            {
                TRJob.Visible = true;
                TRUnitName.Visible = true;
                TRDate.Visible = true;
                TrLoomNo.Visible = true;
                TDlabeltodate.Visible = false;
                TDtxttodate.Visible = false;
                lblFromDate.Text = "Up to";
                DDjob.Enabled = false;
                DDjob.SelectedValue = "1";
            }
            if (RDMaterial.Checked == true)
            {
                TRDate.Visible = true;
                TRJob.Visible = true;
                TRUnitName.Visible = true;
            }
            if (RDRMLoomBal.Checked == true)
            {
                Trcategory.Visible = false;
                TRJob.Visible = false;
                TRUnitName.Visible = true;
                TrLoomNo.Visible = true;
                TRDate.Visible = true;
                Trarticles.Visible = false;

                if (Session["varcompanyId"].ToString() == "21")
                {
                    TDChkWithLotTag.Visible = true;
                }
            }
            if (RDFolioWiseMaterialIssWithConsumption.Checked == true)
            {
                DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
                Trcategory.Visible = true;
                TRJob.Visible = false;
                TRUnitName.Visible = true;
                TrLoomNo.Visible = true;
                TRDate.Visible = true;
                Trarticles.Visible = false;
                TRItemName.Visible = true;
                DDjob.SelectedValue = "0";
                TRFolioNo.Visible = true;
            }
        }
    </script>
    <script runat="server">
        protected void Checkexportchecked(object sender, System.EventArgs e)
        {
            TDcheckwithstockdetail.Visible = false;
            chkwithstockdetail.Checked = false;
            if (chkexcelexport.Checked == true)
            {
                if (RDPendingQty.Checked == true || RDIssueDetail.Checked == true)
                {
                    TDcheckwithstockdetail.Visible = true;
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div style="width: 1000px; height: 1000px;">
                <div style="width: 900px; height: 1000px">
                    <div style="width: 600px; max-height: 500px; margin-left: 200px; margin-top: 20px">
                        <div style="float: left">
                            <asp:Panel runat="server" Style="border: 1px groove Teal; background-color: #DEB887;
                                max-height: 500px" Width="200px">
                                <table style="height: 130px">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RDIssueDetail" runat="server" GroupName="M" Text="Job/Article Wise Issue"
                                                Font-Bold="true" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RDRecReport" runat="server" Text="Job/Article Wise Receive"
                                                GroupName="M" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RDPendingQty" runat="server" Text="Job/Article Wise Pending"
                                                GroupName="M" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RDPacking" runat="server" GroupName="M" Text="Packed Qty." Font-Bold="true"
                                                AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="TRLoomBal" runat="server" visible="false">
                                        <td>
                                            <asp:RadioButton ID="RDLoombal" runat="server" GroupName="M" Text="Loom Balance"
                                                Font-Bold="true" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr id="TRMaterial" runat="server" visible="false">
                                        <td>
                                            <asp:RadioButton ID="RDMaterial" runat="server" GroupName="M" Text="Material Report"
                                                Font-Bold="true" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr id="TrRMLoomBal" runat="server">
                                        <td>
                                            <asp:RadioButton ID="RDRMLoomBal" runat="server" GroupName="M" Text="RM Loom Balance"
                                                Font-Bold="true" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr id="TRFolioWiseMaterialIssWithConsumption" runat="server" visible="false">
                                        <td>
                                            <asp:RadioButton ID="RDFolioWiseMaterialIssWithConsumption" runat="server" GroupName="M"
                                                Text="Folio Wise Material Iss/Consumption" Font-Bold="true" OnCheckedChanged="RadioButton_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <div style="float: right">
                            <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; background-color: #DEB887"
                                Width="340px">
                                <div style="padding-left: 20px">
                                    <table style="height: 158px; position: relative">
                                        <tr runat="server" id="TrCustomerCode">
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDCustomerOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TrOrderNo">
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TRJob">
                                            <td>
                                                <asp:Label ID="lblJob" runat="server" Text="Job" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DDjob_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TRUnitName">
                                            <td>
                                                <asp:Label ID="lblUnits" runat="server" Text="UnitName" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDUnitName" runat="server" Width="150px" AutoPostBack="true"
                                                    CssClass="dropdown" OnSelectedIndexChanged="DDUnitName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TrLoomNo" visible="false">
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDLoomNo" runat="server" Width="150px" CssClass="dropdown"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TRFolioNo" visible="false">
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="FolioNo." CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDFolioNo" runat="server" Width="150px" CssClass="dropdown">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="Trcategory" visible="false">
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDcategory" runat="server" Width="150px" CssClass="dropdown"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDcategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="Trarticles" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="lblArticle" runat="server" Text="Articles" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDArticle" runat="server" Width="150px" CssClass="dropdown">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRItemName" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="250px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDQuality" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="250px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDDesign" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShadeColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShape" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="250px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRDDSize" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="TRDate">
                                            <td id="TDlabelfromdate" runat="server">
                                                <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFromdate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalenderExtendertxtdate" runat="server" TargetControlID="txtFromdate"
                                                                Format="dd-MMM-yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td id="TDlabeltodate" runat="server">
                                                            <asp:Label ID="LblTodate" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td id="TDtxttodate" runat="server">
                                                            <asp:TextBox ID="txtToDate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                                Format="dd-MMM-yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkexcelexport" Text="Excel Export" runat="server" CssClass="labelbold"
                                                                Visible="false" AutoPostBack="true" OnCheckedChanged="Checkexportchecked" />
                                                        </td>
                                                        <td id="TDcheckwithstockdetail" runat="server" visible="false">
                                                            <asp:CheckBox ID="chkwithstockdetail" Text="Check With Stock Detail" runat="server"
                                                                CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDChkWithLotTag" runat="server" visible="false">
                                                            <asp:CheckBox ID="ChkWithLotTagNo" Text="Check With Lot Tag" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDForLoomStockNo" runat="server" visible="false">
                                                            <asp:CheckBox ID="ChkForLoomStock" Text="For LoomStock" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" OnClientClick="return validate();"
                                                                OnClick="btnprint_Click" />
                                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
