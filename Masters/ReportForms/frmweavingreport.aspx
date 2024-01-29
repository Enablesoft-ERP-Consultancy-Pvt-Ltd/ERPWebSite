<%@ Page Title="Weaving Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmweavingreport.aspx.cs" Inherits="Masters_ReportForms_frmweavingreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }            
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "16")
            {
                TDChampoPNMAmtDifference.Visible = true;
                TDFolioWiseConsumptionReport.Visible = true;
                TDDesignWiseFolioMaterialIssueStatus.Visible = true;
                TRReceiveHoldDetail.Visible = true;
            }
            else
            {
                TDChampoPNMAmtDifference.Visible = false;
                TDFolioWiseConsumptionReport.Visible = false;
                TDDesignWiseFolioMaterialIssueStatus.Visible = false;
                TRReceiveHoldDetail.Visible = false;
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "41")
            {
                TDInternalBucket.Visible = false;
                TDIssRecConsumpSummary.Visible = false;
                Td1.Visible = false;
                TDWeaverOrderStatus.Visible = false;
            }
            else
            {
                TDInternalBucket.Visible = true;
                TDIssRecConsumpSummary.Visible = true;
                Td1.Visible = true;
                TDWeaverOrderStatus.Visible = true;
            }

            TDChkReceiveSummaryQualityDesignSizeWise.Visible = false;
            TRForWithoutTDS.Visible = false;
            TROpenIssRecConDetail.Visible = false;
            TDChkForWeavingPendingQtyWithAreaSize.Visible = false;
            TDChkForWeavingPendingQtyWithExcelReport.Visible = false;
            TDWeaverRawMaterialIssRecWithConsumption.Visible = true;
            TDChkForWeavingPendingQtyWithArea.Visible = false;
            TRChkBazaarReturnGatePassSummary.Visible = false;
            ChkBazaarReturnGatePassSummary.Checked = false;
            TRChkBazaarReceiveOnLoom.Visible = false;
            ChkBazaarReceiveOnLoom.Checked = false;
            TRReturnGatePassNo.Visible = false;
            Trunitname.Visible = false;
            TRChkWeavingReport.Visible = false;
            TRChkUnpaidApprovalNo.Visible = false;
            TDchksummary.Visible = false;
            TRchkforshadewise.Visible = false;
            lblfromdt.Text = "From Date";
            Tdtodate.Visible = true;
            Tdtodatelabel.Visible = true;
            Tdselectdate.Visible = true;

            trProductionStatus.Visible = true;
            trFolioType.Visible = true;
            TRCustomerCode.Visible = true;
            TROrderNo.Visible = true;
            trCategoryName.Visible = true;
            trItemName.Visible = true;
            trReportType.Visible = false;
            Trproductiontype.Visible = false;
            TDchkpbarcode.Visible = false;
            chkpbarcode.Checked = false;

            TDInternalExternalBazarSummary.Visible = false;
            TDForInternalBazaarDetail.Visible = false;
            TDChkReceiveSummaryFinishedItemWise.Visible = false;
            TDChkForStockDetail.Visible = false;
            TDChkForDepartmentRawDetail.Visible = false;
            TRReceiveHoldDetail.Visible = false;
            if (Session["varMasterCompanyIDForERP"].ToString() == "42")
            {
                TDDouraReport.Visible = true;
            }
            else
            {
                TDDouraReport.Visible = false;
            }

            if (RDAll.Checked == true)
            {
                Trproductiontype.Visible = true;
                Trunitname.Visible = true;
                if (variable.VarLoomNoGenerated == "1")
                {
                    TDchkpbarcode.Visible = true;
                }
                if (Session["VarCompanyNo"].ToString() == "38")
                {
                    TDChkForWeavingPendingQtyWithArea.Visible = true;
                }
                else
                {
                    TDChkForWeavingPendingQtyWithArea.Visible = false;
                }
                if (Session["VarCompanyNo"].ToString() == "42")
                {
                    TDChkForWeavingPendingQtyWithAreaSize.Visible = true;
                }
                else
                {
                    TDChkForWeavingPendingQtyWithAreaSize.Visible = false;
                }

                if (Session["VarCompanyNo"].ToString() == "31")
                {
                    TDChkForWeavingPendingQtyWithExcelReport.Visible = true;
                }
                else
                {
                    TDChkForWeavingPendingQtyWithExcelReport.Visible = false;
                }

            }
            if (RDOrder.Checked == true)
            {
                ChkWeavingReport.Checked = false;
                Trproductiontype.Visible = true;
                Trunitname.Visible = true;
                if (Session["varMasterCompanyIDForERP"].ToString() == "22")
                {
                    TRChkWeavingReport.Visible = true;
                }

            }
            if (RDReceive.Checked == true)
            {
                ChkWeavingReport.Checked = false;
                TDchksummary.Visible = true;
                Trproductiontype.Visible = true;
                Trunitname.Visible = true;
                TDChkReceiveSummaryFinishedItemWise.Visible = true;
                TDChkReceiveSummaryQualityDesignSizeWise.Visible = true;

                if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
                {
                    TDInternalExternalBazarSummary.Visible = true;
                }
                if (Session["varMasterCompanyIDForERP"].ToString() == "28")
                {
                    TDForInternalBazaarDetail.Visible = true;
                }
                if (Session["varMasterCompanyIDForERP"].ToString() == "22")
                {
                    TRChkWeavingReport.Visible = true;
                }

            }
            if (RDweaverrawbalance.Checked == true)
            {
                lblfromdt.Text = "As on Date";
                Tdtodate.Visible = false;
                Tdtodatelabel.Visible = false;
                Tdselectdate.Visible = false;
                if (Session["varMasterCompanyIDForERP"].ToString() == "27")
                {
                    TRchkforshadewise.Visible = true;
                }


            }
            if (RDWeaverHissabReport.Checked == true)
            {
                DDWeaver.SelectedIndex = 0;
                DDFolioNo.Items.Clear();
                DDReportType.SelectedValue = "1";
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                TDchksummary.Visible = false;
                trReportType.Visible = true;
                Trorderstatus.Visible = false;
                TRChkUnpaidApprovalNo.Visible = true;

            }
            if (RDWeaverHissabReport.Checked == false)
            {
                DDWeaver.SelectedIndex = 0;
                DDFolioNo.Items.Clear();
                Label3.Text = "Folio No";
                Trorderstatus.Visible = false;

            }
            if (Rdweaverorderstatus.Checked == true)
            {
                Trorderstatus.Visible = true;

            }
            if (Rdweaverorderstatus.Checked == false)
            {
                Trorderstatus.Visible = false;
                DDorderstatus.SelectedIndex = 0;

            }
            if (RDInternalBucketDetail.Checked == true)
            {
                trFolioType.Visible = false;

            }
            trWeaver.Visible = true;
            trFolioNo.Visible = true;
            DDCategory.SelectedValue = "0";
            DDQtype.Items.Clear();
            if (RDIssRecConsumpSummary.Checked == true)
            {
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = false;
                trFolioNo.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                Tdselectdate.Visible = true;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;

            }
            DDQuality.Items.Clear();
            if (RDItemQualityWiseWeavingPaymentSummary.Checked == true)
            {
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = true;
                Trproductiontype.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = false;
                trFolioNo.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                Tdselectdate.Visible = true;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;

            }

            if (RDBazaarReturnGatePassDetail.Checked == true)
            {
                DDReturnGatePassNo.Items.Clear();
                DDWeaver.SelectedIndex = 0;
                trProductionStatus.Visible = true;
                DDProductionstatus.SelectedItem.Text = "Pending";
                DDProductionstatus.Enabled = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                TDchksummary.Visible = false;
                trReportType.Visible = false;
                Trorderstatus.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TrSelectDate.Visible = false;
                TRReturnGatePassNo.Visible = true;
                TRChkBazaarReturnGatePassSummary.Visible = true;
            }
            if (RDBazaarReturnGatePassDetail.Checked == false)
            {
                DDProductionstatus.SelectedItem.Text = "ALL";
                DDProductionstatus.Enabled = true;
                DDWeaver.SelectedIndex = 0;
                DDFolioNo.Items.Clear();
                Label3.Text = "Folio No";
                Trorderstatus.Visible = false;
                TrSelectDate.Visible = true;
                TRReturnGatePassNo.Visible = false;
                TRChkBazaarReturnGatePassSummary.Visible = false;
                ChkBazaarReturnGatePassSummary.Checked = false;
            }

            if (RDWeaverRawMaterialOnLoom.Checked == true)
            {
                TRChkBazaarReceiveOnLoom.Visible = true;
                DDReturnGatePassNo.Items.Clear();
                DDWeaver.SelectedIndex = 0;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                TDchksummary.Visible = false;
                trReportType.Visible = false;
                Trorderstatus.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TrSelectDate.Visible = true;
                TRReturnGatePassNo.Visible = false;
            }
            if (RDWeaverRawMaterialOnLoom.Checked == false)
            {
                TRChkBazaarReceiveOnLoom.Visible = false;
                ChkBazaarReceiveOnLoom.Checked = false;
                DDWeaver.SelectedIndex = 0;
                DDFolioNo.Items.Clear();
                Label3.Text = "Folio No";
                Trorderstatus.Visible = false;
            }

            if (RDWithTagNoTracking.Checked == true)
            {
                //TRCompanyName.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = false;
                trFolioNo.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                Tdselectdate.Visible = true;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                TRTagNo.Visible = true;
                if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["UserType"].ToString() == "1")
                {
                    TDChkForStockDetail.Visible = true;
                }
                else
                {
                    TDChkForStockDetail.Visible = false;
                }
            }
            if (RDWithTagNoTracking.Checked == false)
            {
                //TRCompanyName.Visible = true;
                TRTagNo.Visible = false;
                TDChkForStockDetail.Visible = false;
            }

            if (RDChampoPNMAmtDifference.Checked == true)
            {
                TRCompanyName.Visible = true;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = true;
                trFolioNo.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                Tdselectdate.Visible = true;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                TRTagNo.Visible = false;
                DDWeaver.SelectedValue = "890";
                DDWeaver.Enabled = false;
            }
            if (RDChampoPNMAmtDifference.Checked == false)
            {
                DDWeaver.Enabled = true;
            }
            if (RDChampoExternalWeaverConsumption.Checked == true)
            {
                TRCompanyName.Visible = true;
                trReportType.Visible = false;
                trProductionStatus.Visible = true;
                trFolioType.Visible = false;
                Trproductiontype.Visible = true;
                TRCustomerCode.Visible = true;
                TROrderNo.Visible = true;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                Tdselectdate.Visible = true;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                TRTagNo.Visible = false;
                Trunitname.Visible = true;
                DDproductiontype.SelectedValue = "0";
                TDChkForDepartmentRawDetail.Visible = true;
                //DDproductiontype.Enabled = false;
            }
            if (RDChampoExternalWeaverConsumption.Checked == false)
            {
                DDproductiontype.Enabled = true;
            }
            if (RDWeavingOrderRecBalWithAmountDetail.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = true;
                TROrderNo.Visible = true;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;
            }
            if (RDWeaverRawMaterialIssueReport.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = true;
                TROrderNo.Visible = true;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = true;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;
            }

            if (RDWeavingReceiveWithTDS.Checked == true)
            {
                ChkWeavingReport.Checked = false;
                TDchksummary.Visible = false;
                Trproductiontype.Visible = false;
                Trunitname.Visible = false;
                TDChkReceiveSummaryFinishedItemWise.Visible = false;

            }
            if (RDWeaverRawMaterialIssRecWithConsumption.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = true;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;

                if (Session["VarCompanyNo"].ToString() == "42")
                {
                    ChkselectDate.Checked = false;
                    TDchksummary.Visible = true;
                }
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    trFolioNo.Visible = false;
                    trCategoryName.Visible = false;
                    trItemName.Visible = false;
                    Trquality.Visible = false;
                    Trshadecolor.Visible = false;
                    TDchksummary.Visible = true;
                    TROpenIssRecConDetail.Visible = true;
                }
            }
            if (RDWeaverReceivePaymentSummary.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = true;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;
            }
            if (RDWeaverAdvancePaymentFolioWise.Checked == true)
            {
                DDWeaver.SelectedIndex = 0;
                DDFolioNo.Items.Clear();
                DDReportType.SelectedValue = "1";
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                Trquality.Visible = false;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = false;
                TDchksummary.Visible = false;
                trReportType.Visible = false;
                Trorderstatus.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
            }
            if (RDinternalFolioDetail.Checked == true)
            {
                TRReceiveHoldDetail.Visible = true;
            }
            if (RDQualityWiseProductionHissabSummary.Checked == true)
            {
                Trunitname.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    Trproductiontype.Visible = true;                    
                }
                else
                {
                    Trproductiontype.Visible = false;                   
                }
            }
            if (RDWeaverRawMaterialIssueSummary.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = false;
                Trcolor.Visible = false;
                Trsize.Visible = false;
                Trshadecolor.Visible = true;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;              

            }
            if (RDDouraReport.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = true;
                TROrderNo.Visible = true;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Trunitname.Visible = false;
                trReportType.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = true;
                trItemName.Visible = true;
                Trquality.Visible = true;
                Trdesign.Visible = true;
                Trcolor.Visible = true;
                Trsize.Visible = true;
                Trshadecolor.Visible = false;
                Trorderstatus.Visible = false;
                TRReturnGatePassNo.Visible = false;
                TRTagNo.Visible = false;
                TDchksummary.Visible = false;
                TDchkpbarcode.Visible = false;
                TRchkforshadewise.Visible = false;
                TRChkUnpaidApprovalNo.Visible = false;
                TRChkWeavingReport.Visible = false;
                ChkselectDate.Checked = true;
               
            }
            if (RDWeavingRecSummaryWithTDS.Checked == true)
            {
                Trunitname.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    Trproductiontype.Visible = true;
                    TRForWithoutTDS.Visible = true;
                }
                else
                {
                    Trproductiontype.Visible = false;
                    TRForWithoutTDS.Visible = false;
                }
            }
            if (RDBazaarWiseWeavingRegisterSummary.Checked == true)
            {
                Trunitname.Visible = false;
                trProductionStatus.Visible = false;
                trFolioType.Visible = false;
                TRCustomerCode.Visible = false;
                TROrderNo.Visible = false;
                Trproductiontype.Visible = false;
                trCategoryName.Visible = false;
                trItemName.Visible = false;
                trFolioNo.Visible = false;
                Trquality.Visible = false;
                Trshadecolor.Visible = false;
                TRForWithoutTDS.Visible = true;
                
            }
        }        
    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 5%;">
                <div style="width: 100%">
                    <div style="float: left; width: 30%">
                        <asp:Panel ID="pnl1" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDAll" Text="All" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDOrder" Text="Order" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDReceive" Text="Receive" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverRawBalance" runat="server">
                                        <asp:RadioButton ID="RDweaverrawbalance" Text="Weaver Raw Balance" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td1" runat="server" visible="false">
                                        <asp:RadioButton ID="RDinternalFolioDetail" Text="Internal Folio Detail" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td2" runat="server">
                                        <asp:RadioButton ID="RDWeaverHissabReport" Text="Weaver Hissab Payment" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverOrderStatus" runat="server" visible="false">
                                        <asp:RadioButton ID="Rdweaverorderstatus" Text="Weaver order Status" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RdConsumptionreport" Text="Consumption Report" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDInternalBucket" runat="server" visible="false">
                                        <asp:RadioButton ID="RDInternalBucketDetail" Text="Internal Bucket Detail" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDIssRecConsumpSummary" runat="server" visible="false">
                                        <asp:RadioButton ID="RDIssRecConsumpSummary" Text="Iss/Rec Consumption Summary" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDItemQualityWiseWeavingPaymentSummary" runat="server">
                                        <asp:RadioButton ID="RDItemQualityWiseWeavingPaymentSummary" Text="ItemQuality Wise Payment Summary"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TD3" runat="server">
                                        <asp:RadioButton ID="RDCommissionDetail" Text="Commission Detail" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDBazaarReturnGatePassDetail" runat="server">
                                        <asp:RadioButton ID="RDBazaarReturnGatePassDetail" Text="Bazaar Return GatePass"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverRawMaterialOnLoom" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeaverRawMaterialOnLoom" Text="Raw Material OnLoom" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWithTagNoTracking" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWithTagNoTracking" Text="With TagNo Tracking" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChampoPNMAmtDifference" runat="server" visible="false">
                                        <asp:RadioButton ID="RDChampoPNMAmtDifference" Text="Champo PNM Amt Difference" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChampoExternalWeaverConsumption" runat="server" visible="false">
                                        <asp:RadioButton ID="RDChampoExternalWeaverConsumption" Text="External Internal Consumption Detail"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDRDWeavingOrderRecBalWithAmountDetail" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeavingOrderRecBalWithAmountDetail" Text="Weaving Order Rec Bal With Amount"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDFolioWiseConsumptionReport" runat="server" visible="false">
                                        <asp:RadioButton ID="RDFolioWiseConsumptionReport" Text="Folio Wise Consumption Report"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverRawMaterialIssueReport" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeaverRawMaterialIssueReport" Text="Weaver Raw Material Issue"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDDesignWiseFolioMaterialIssueStatus" runat="server" visible="false">
                                        <asp:RadioButton ID="RDDesignWiseFolioMaterialIssueStatus" Text="Design Wise Folio Material Issue Status"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeavingReceiveWithTDS" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeavingReceiveWithTDS" Text="Weaving Receive With TDS" runat="server"
                                            CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverRawMaterialIssRecWithConsumption" runat="server" visible="true">
                                        <asp:RadioButton ID="RDWeaverRawMaterialIssRecWithConsumption" Text="Weaver Raw Material Iss Rec With Consumption"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverReceivePaymentSummary" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeaverReceivePaymentSummary" Text="Weaver Receive Payment Summary"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverAdvancePaymentFolioWise" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeaverAdvancePaymentFolioWise" Text="Weaver Advance Payment FolioWise"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDQualityWiseProductionHissabSummary" runat="server" visible="false">
                                        <asp:RadioButton ID="RDQualityWiseProductionHissabSummary" Text="Quality Wise Production & Consump Summary"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeaverRawMaterialIssueSummary" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeaverRawMaterialIssueSummary" Text="Weaver Raw Material Iss Summary"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>

                                 <tr>
                                    <td id="TDDouraReport" runat="server" visible="false">
                                        <asp:RadioButton ID="RDDouraReport" Text="Doura Report"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWeavingRecSummaryWithTDS" runat="server" visible="false">
                                        <asp:RadioButton ID="RDWeavingRecSummaryWithTDS" Text="Weaving Rec Summary With TDS"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDBazaarWiseWeavingRegisterSummary" runat="server" visible="false">
                                        <asp:RadioButton ID="RDBazaarWiseWeavingRegisterSummary" Text="Bazaar Wise Weaving Register Summary"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="float: right; width: 70%">
                        <asp:Panel ID="pnl2" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr id="TRCompanyName" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trunitname" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label16" runat="server" CssClass="labelbold" Text="Unit Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDUnitname" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trReportType" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label14" Text="Report Type." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDReportType" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDReportType_SelectedIndexChanged">
                                            <asp:ListItem Value="0">ApprovalNo Wise</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">Folio Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trProductionStatus" runat="server">
                                    <td>
                                        <asp:Label ID="Label1" Text="Production Status" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDProductionstatus" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Pending</asp:ListItem>
                                            <asp:ListItem>Complete</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trFolioType" runat="server">
                                    <td>
                                        <asp:Label ID="Label11" Text="Folio Type" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDFoliotype" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem Value="-1">ALL</asp:ListItem>
                                            <asp:ListItem Value="0">Production</asp:ListItem>
                                            <asp:ListItem Value="1">Purchase</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trproductiontype" runat="server">
                                    <td>
                                        <asp:Label ID="Label15" Text="Production Type" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDproductiontype" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem Value="-1">ALL</asp:ListItem>
                                            <asp:ListItem Value="0">External</asp:ListItem>
                                            <asp:ListItem Value="1">Internal</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRCustomerCode" runat="server">
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="300px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TROrderNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="300px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trWeaver" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" Text="Weaver/Contractor Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDWeaver" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDWeaver_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trFolioNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" Text="Folio No." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trCategoryName" runat="server">
                                    <td>
                                        <asp:Label ID="Label8" Text="Category Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trItemName" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" Text="Item Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQtype" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDQtype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trquality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label9" Text="Quality" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trdesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trcolor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trsize" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label7" Text="Size" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="200px">
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
                                        <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
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
                                <tr id="TRReturnGatePassNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label17" Text="Return GatePass No" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDReturnGatePassNo" CssClass="dropdown" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRTagNo" runat="server" visible="false">
                                    <td id="Td4" runat="server">
                                        <asp:Label ID="Label20" Text="Lot No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtTagNo" CssClass="textb" runat="server" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="TrSelectDate" runat="server" visible="true">
                                    <td id="Tdselectdate" runat="server">
                                        <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold" />
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
                                <tr>
                                    <td id="TDchksummary" runat="server" visible="false">
                                        <asp:CheckBox ID="chksummary" Text="For Summary" runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDInternalExternalBazarSummary" runat="server" visible="false" colspan="2">
                                        <asp:CheckBox ID="ChkInternalExternalBazarSummary" Text="For Internal External Bazar Summary"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDchkpbarcode" runat="server">
                                        <asp:CheckBox ID="chkpbarcode" Text="For Pending Bar Code" runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr id="TRchkforshadewise" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="chkforshadewise" Text="For ShadeWise" runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr id="TRChkUnpaidApprovalNo" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="ChkUnpaidApprovalNo" Text="For Unpaid ApprovalNo" runat="server"
                                            CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="ChkUnpaidApprovalNo_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="TRChkWeavingReport" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="ChkWeavingReport" Text="For Weaving Report" runat="server" CssClass="checkboxbold"
                                            AutoPostBack="true" OnCheckedChanged="ChkWeavingReport_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="TRChkBazaarReceiveOnLoom" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="ChkBazaarReceiveOnLoom" Text="For Bazaar Receive OnLoom" runat="server"
                                            CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="ChkBazaarReceiveOnLoom_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr id="TRChkBazaarReturnGatePassSummary" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="ChkBazaarReturnGatePassSummary" Text="Return GatePass Summary"
                                            runat="server" CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="ChkBazaarReturnGatePassSummary_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDForInternalBazaarDetail" runat="server" visible="false" colspan="2">
                                        <asp:CheckBox ID="ChkForInternalBazaarDetail" Text="For Internal Bazaar Detail" runat="server"
                                            CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkReceiveSummaryFinishedItemWise" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkReceiveSummaryFinishedItemWise" Text="For Summary FinishedItem Wise"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForStockDetail" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForStockDetails" Text="For Stock Details" runat="server" CssClass="checkboxbold"
                                            Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForDepartmentRawDetail" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForDepartmentRawDetail" Text="For Department Consmp Detail"
                                            runat="server" CssClass="checkboxbold" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForWeavingPendingQtyWithArea" colspan="2" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForWeavingPendingQtyWithArea" Text="For Weaving Pending Qty With Area"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForWeavingPendingQtyWithExcelReport" colspan="2" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForWeavingPendingQtyWithExcelReport" Text="For Weaving Pending Qty Excel Report"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForWeavingPendingQtyWithAreaSize" colspan="2" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForWeavingPendingQtyWithAreaSize" Text="For Weaving Pending Qty With Area Size"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr id="TRReceiveHoldDetail" runat="server" visible="false">
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkReceiveHoldDetail" runat="server" CssClass="checkboxbold" Text="Receive Hold Detail" />
                                    </td>
                                </tr>
                                <tr id="TROpenIssRecConDetail" runat="server" visible="false">
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkOpenIssRecConDetail" runat="server" CssClass="checkboxbold" Text="Opening Iss Rec Consmp Detail" />
                                    </td>
                                </tr>
                                 <tr id="TRForWithoutTDS" runat="server" visible="false">
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkForWithoutTDS" runat="server" CssClass="checkboxbold" Text="For Without TDS" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td id="TDChkReceiveSummaryQualityDesignSizeWise" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkReceiveSummaryQualityDesignSizeWise" Text="Rec Summary QualityDesignSize Wise"
                                            runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
