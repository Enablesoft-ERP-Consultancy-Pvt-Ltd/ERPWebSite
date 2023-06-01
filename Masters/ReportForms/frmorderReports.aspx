<%@ Page Title="ORDER REPORTS" Language="C#" AutoEventWireup="true" CodeFile="frmorderReports.aspx.cs"
    Inherits="Masters_ReportForms_frmorderReports" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            var message = "";
            if (document.getElementById('<%= RDOrderConsumption.ClientID %>').checked) {
                if (document.getElementById('<%= DDCustCode.ClientID %>').value <= 0) {
                    message = message + "Please select customer code..\n";
                }
                if (document.getElementById('<%= DDOrderNo.ClientID %>').value <= 0) {
                    message = message + "Please select customer Order No..\n";
                }
            }
            else if (document.getElementById('<%= RDVendorWiseDetail.ClientID %>').checked) {
                if (document.getElementById('<%=txtlocalOrderNo.ClientID  %>').value == "") {
                    message = message + "Please Enter Srno..\n";
                }
            }
            else if (document.getElementById('<%= RDProductionSummary.ClientID %>').checked) {
                if (document.getElementById('<%= DDOrderNo.ClientID %>').value <= 0) {
                    message = message + "Please select customer Order No..\n";
                }
            }
            else if (document.getElementById('<%= RDDesignWiseConsmpDetail.ClientID %>').checked) {
                if (document.getElementById('<%= DDOrderNo.ClientID %>').value <= 0) {
                    message = message + "Please select customer Order No..\n";
                }
            }
            else if (document.getElementById('<%= RDProductionReport.ClientID %>').checked) {
                if (document.getElementById('<%= DDCategory.ClientID %>').value <= 0) {
                    message = message + "Please select category..\n";
                }
            }
            else if (document.getElementById('<%= RDFinishingStatus.ClientID %>').checked) {
                var isChecked = $('#<%= chkexcelexport.ClientID %>').is(":checked");
                if (isChecked) {
                    if (document.getElementById('<%= DDjobname.ClientID %>').value <= 0) {
                        message = message + "Please select Job Name..\n";
                    }
                }
            }
            else if (document.getElementById('<%= RDBazaarCompleteStatus.ClientID %>').checked) {
                var isChecked = $('#<%= chkexcelexport.ClientID %>').is(":checked");
                if (isChecked) {
                    if (document.getElementById('<%= DDjobname.ClientID %>').value <= 0) {
                        message = message + "Please select Job Name..\n";
                    }
                }
            }
            if (message != "") {
                alert(message);
                return false;
            }
        }
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TROrderType.Visible = false;
            TRFolioWiseConsumptionSummary.Visible = false;
            TREmployeeName.Visible = false;
            TRFolioWiseConsumptionReportType2.Visible = false;
            TRPOStatusReportType2.Visible = false;
            TDWithOrderValue.Visible = false;
            TDPaid.Visible = false;
            chkpaidunpaid.Checked = false;
            TRMonthyear.Visible = false;
            TRcategory.Visible = false;
            TritemName.Visible = false;
            TrProcessname.Visible = false;
            TRfinishingstatusExcel.Visible = false;
            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRCustomerCode.Visible = false;
            }
            else
            {
                TRCustomerCode.Visible = true;
            }

            TROrderNo.Visible = true;
            TRSRNo.Visible = true;
            Trsizeunit.Visible = false;
            Trorderstatus.Visible = false;
            TRcheckdate.Visible = false;
            if (RDVendorWiseDetail.Checked == true)
            {
                TDPaid.Visible = true;
            }
            if (RDCustomeropenorderstatus.Checked == true)
            {
                TRMonthyear.Visible = true;
            }
            if (RDFinishingStatus.Checked == true)
            {
                TRcategory.Visible = true;
                TritemName.Visible = true;
                TrProcessname.Visible = true;
                TRfinishingstatusExcel.Visible = true;
            }
            if (RDProductionReport.Checked == true)
            {
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                TRcategory.Visible = true;
                TritemName.Visible = true;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                TRSRNo.Visible = false;
                TRMonthyear.Visible = true;
            }
            if (RDOrderstatus.Checked == true)
            {
                Trsizeunit.Visible = true;

                if (Session["varCompanyNo"].ToString() == "4")
                {
                    TROrderType.Visible = true;
                }
                else
                {
                    TROrderType.Visible = false;
                }
            }
            if (RDPOSTATUSAGNI.Checked == true)
            {
                TRcategory.Visible = false;
                TritemName.Visible = false;
                TRMonthyear.Visible = false;
                TRSRNo.Visible = false;
                TRPODETAIL.Visible = true;
            }
            if (RDPOSTATUS.Checked == true || RDPOSTATUSInHouse.Checked == true || RDPOSTATUSOutSide.Checked == true)
            {
                TRcategory.Visible = true;
                TritemName.Visible = true;
                TRMonthyear.Visible = true;
                if (Session["varCompanyNo"].ToString() == "4")
                {
                    Trorderstatus.Visible = false;
                }
                else
                {
                    Trorderstatus.Visible = true;
                }
                if (Session["varCompanyNo"].ToString() == "30")
                {
                    TDWithOrderValue.Visible = true;
                }
                else
                {
                    TDWithOrderValue.Visible = false;
                }

                if (RDPOSTATUS.Checked == true)
                {
                    if (Session["varCompanyNo"].ToString() == "16" || Session["varCompanyNo"].ToString() == "28")
                    {
                        TRPOStatusReportType2.Visible = true;
                    }
                    else
                    {
                        TRPOStatusReportType2.Visible = false;
                    }
                }
               
            }
            if (RDinternalfoliodetail.Checked == true)
            {
                TRcategory.Visible = true;
                TritemName.Visible = true;
            }
            if (RDProcessWiseReport.Checked == true)
            {
                TRcheckdate.Visible = true;
            }
            if (RDCustomerOrderInvoiceStatus.Checked == true)
            {
                Trsizeunit.Visible = true;
            }
            //TROrderNo.Visible = true;
            //TRSRNo.Visible = true;
            //Trsizeunit.Visible = false;
            //Trorderstatus.Visible = false;
            //TRcheckdate.Visible = false;
            if (RDCustomerOrderInternalOC.Checked == true)
            {
                TROrderNo.Visible = true;
                TRSRNo.Visible = false;
            }
            if (RDBazaarCompleteStatus.Checked == true)
            {
                TRcategory.Visible = true;
                TritemName.Visible = true;
                TrProcessname.Visible = false;
                TRfinishingstatusExcel.Visible = true;
                TRcheckdate.Visible = true;
                ChkForRecQty.Visible = false;
            }
            if (RDFolioWiseDetail.Checked == true)
            {
                if (Session["varCompanyNo"].ToString() == "16" || Session["varCompanyNo"].ToString() == "28")
                {
                    TRMonthyear.Visible = true;
                    TRFolioWiseConsumptionReportType2.Visible = true;
                    TREmployeeName.Visible = true;
                    TRFolioWiseConsumptionSummary.Visible = true;
                }
                else
                {
                    TRMonthyear.Visible = false;
                    TRFolioWiseConsumptionReportType2.Visible = false;
                    TREmployeeName.Visible = false;
                    TRFolioWiseConsumptionSummary.Visible = false;
                }
            }
            if (RDOrderSummaryWithAllProcess.Checked == true)
            {
                TRMonthyear.Visible = true;
                TRSRNo.Visible = false;
            }
            if (RDOrderDispatchSummary.Checked == true)
            {
                TRMonthyear.Visible = true;
                TRSRNo.Visible = false;
            }
            if (RDOrderDetailWIP.Checked == true)
            {
                TRMonthyear.Visible = true;
                TRSRNo.Visible = false;
            }
            if (RDOrderShippedInvoiceWiseDetail.Checked==true)
            {
                TRcheckdate.Visible = true;
                ChkForRecQty.Visible = false;
                TRCustomerCode.Visible = true;
                TRcategory.Visible = true;
                TritemName.Visible = true;
                TRMonthyear.Visible = true;
                Trorderstatus.Visible = false;
                TDWithOrderValue.Visible = false;
                TRSRNo.Visible = false;
                

            }
             if (RDProcessDetails.Checked==true)
            {
                TRCustomerCode.Visible = true;
                TRcategory.Visible = true;
                TritemName.Visible = true;
               
                TROrderNo.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = true;
                Trcolor.Visible = true;
                
                TRSRNo.Visible = false;
                
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 90%; height: 100%;">
                <tr style="width: 100%">
                    <td style="width: 80px">
                    </td>
                    <td>
                        <div style="width: 250px; max-height: 100%; float: left; border-style: solid; border-width: thin">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDOrderConsumption" Text="Order Consumption Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDVendorWiseDetail" Text="Vendor Wise Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDProductionSummary" Text="Production Summary" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDCancelPo" Text="Cancel Po Detail" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDOrderstatus" Text="Order Status" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDFinishingStatus" Text="Finishing Status" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDCustomeropenorderstatus" Text="Customer Open Order Status"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDFolioWiseDetail" Text="Folio Wise Consmp Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDDesignWiseConsmpDetail" Text="Design Wise Consmp Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDProductionReport" Text="Production Report" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPOSTATUS" Text="PO Status" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPOSTATUSAGNI" Text="PO Status" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPOSTATUSInHouse" Text="PO Status InHouse" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPOSTATUSOutSide" Text="PO Status OutSide" runat="server" Font-Bold="true"
                                            GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDinternalfoliodetail" runat="server" visible="false">
                                        <asp:RadioButton ID="RDinternalfoliodetail" Text="Internal Folio Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDProcessWiseReport" runat="server" visible="false">
                                        <asp:RadioButton ID="RDProcessWiseReport" Text="Process Wise Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TD1" runat="server">
                                        <asp:RadioButton ID="RDInternalprodstockno" Text="Internal Prod. Stock No." runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDCustomerOrderInvoiceStatus" runat="server">
                                        <asp:RadioButton ID="RDCustomerOrderInvoiceStatus" Text="CustomerOrder Invoice Status"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDCustomerOrderInternalOC" Text="Customer Order Internal OC"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDBazaarCompleteStatus" runat="server" visible="false">
                                        <asp:RadioButton ID="RDBazaarCompleteStatus" Text="Bazaar Complete Status" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>

                                 <tr id="TROrderSummaryWithAllProcess" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderSummaryWithAllProcess" Text="Order Summary With All Jobs"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>

                                <tr id="TROrderDispatchSummary" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderDispatchSummary" Text="Order Dispatch Summary"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>

                                <tr  id="TROrderConsumptionWithIndentIssRec" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderConsumptionWithIndentIssRec" Text="Order Consumption With Indent Iss/Rec" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>

                                 <tr id="TROrderDetailWIP" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderDetailWIP" Text="Order Detail WIP"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                 <tr  id="TROrderConsumptionSummaryWithWeaverIssRec" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderConsumptionSummaryWithWeaverIssRec" Text="Order Consumption Summary With Weaver Iss/Rec" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr  id="TROrderConsumptionRecMaterialPending" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderConsumptionRecMaterialPending" Text="Order Consumption Rec Material Pending Detail" runat="server"
                                            Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="TROrderShippedInvoiceWiseDetail" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDOrderShippedInvoiceWiseDetail" Text="Order Shipped Invoice Wise Detail"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="TRProcessDetails" runat="server" visible="true">
                                    <td>
                                        <asp:RadioButton ID="RDProcessDetails" Text="Order Process Details"
                                            runat="server" Font-Bold="true" GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: left; width: 350px; max-height: 400px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrProcessname" visible="false">
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Job Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDjobname" runat="server" Width="250px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRCustomerCode" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TROrderNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" AutoPostBack="true"
                                            CssClass="dropdown" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                                <tr id="TREmployeeName" runat="server" visible = "false">
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDEmployeeName" runat="server" Width="250px" CssClass="dropdown" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr id="TRcategory" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TritemName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trquality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label9" Text="Quality" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="250px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trdesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trcolor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trsize" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label7" Text="Size" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="150px">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="Chkmtrsize" Text="Mtr Size" runat="server" AutoPostBack="true"
                                            CssClass="checkboxbold" OnCheckedChanged="Chkmtrsize_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="Trshadecolor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label10" Text="Shadecolor" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trsizeunit" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label8" Text="Size Unit" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDsizeunit" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRSRNo" runat="server">
                                    <td>
                                        <asp:Label ID="lblLocalOrderNo" runat="server" Text="SR No." CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlocalOrderNo" CssClass="textb" runat="server" />
                                    </td>
                                </tr>
                                <tr id="TRfinishingstatusExcel" runat="server" visible="false">
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkexcelexport" CssClass="checkboxbold" Text="Excel Export" runat="server" />
                                    </td>
                                </tr>
                                <tr id="Trorderstatus" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblorderstatus" Text="Order Status" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDorderstatus" CssClass="dropdown" runat="server">
                                            <asp:ListItem Text="ALL" Value="-1" />
                                            <asp:ListItem Text="Running" Value="0" />
                                            <asp:ListItem Text="Complete" Value="1" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                  <tr id="TROrderType" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label13" Text="Order Type" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderType" CssClass="dropdown" runat="server">                                                                                     
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRcheckdate" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                            OnCheckedChanged="ChkForDate_CheckedChanged" AutoPostBack="true" />
                                        <asp:CheckBox ID="ChkForRecQty" runat="server" Text="Check For RecQty" CssClass="checkboxbold"
                                            OnCheckedChanged="ChkForRecQty_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                  <tr id="TRPOStatusReportType2" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForPOStatusReportType2" runat="server" Text="Check For Report Type2" CssClass="checkboxbold"/>
                                       
                                    </td>
                                </tr>
                                 <tr id="TRFolioWiseConsumptionReportType2" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForFolioWiseConsReporType2" runat="server" Text="Check For Report Type2" CssClass="checkboxbold"/>
                                       
                                    </td>
                                </tr>
                                 <tr id="TRFolioWiseConsumptionSummary" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForFolioWiseConReportType2Summary" runat="server" Text="Check For Report Summary" CssClass="checkboxbold"/>
                                       
                                    </td>
                                </tr>
                                 <tr id="TRPODETAIL" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="chkforpodetail" runat="server" Text="For Details" CssClass="checkboxbold"/>
                                       
                                    </td>
                                </tr>

                                <tr id="TRMonthyear" runat="server" visible="false">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblmonth" Text="From Date" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                                    <asp:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtfromdate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" Text="To Date" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDPaid" runat="server" visible="false">
                                        <asp:CheckBox ID="chkpaidunpaid" Text="Paid/Unpaid" runat="server" CssClass="checkboxbold" />
                                    </td>
                                    <td id="TDWithOrderValue" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkWithOrderValue" Text="With Order Value" runat="server" CssClass="checkboxbold" />
                                    </td>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                            OnClientClick="return Validate();" CssClass="buttonnorm" />
                                        <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
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
