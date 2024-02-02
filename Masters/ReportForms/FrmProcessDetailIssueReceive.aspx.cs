using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;
using ClosedXML.Excel;
public partial class Masters_ReportForms_FrmProcessDetailIssueReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TRlotNo.Visible = false;
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
                        Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By PROCESS_NAME
                        select CI.CustomerId,CI.CustomerCode from customerinfo  CI order by CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Select--");
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtIssueFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtIssueToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            txtAsOnDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            int varcompanyNo = Convert.ToInt16(Session["varMasterCompanyIDForERP"].ToString());
            switch (varcompanyNo)
            {
                case 8:
                    RDProcessOrderFolio.Visible = true;
                    RDProcessIssRecDetail.Visible = false;
                    RDCommDetail.Visible = false;
                    RDGatePass.Visible = false;
                    RDProcessIssRecDetail.Visible = false;
                    RDProcessIssRecDetailWithConsumpton.Visible = false;
                    RDStockNoTobeIssued.Visible = false;
                    ChkForProcessIssRecSummary.Visible = false;
                    ChkSummary.Visible = false;
                    break;
                case 9:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    RDWeaverRawMaterialReceiveDetail.Visible = true;
                    break;
                case 16:
                    RDProcessOrderFolio.Visible = false;
                    RDCommDetail.Visible = false;
                    RDStockNoTobeIssued.Visible = false;
                    RDPendingQty.Visible = false;
                    RDStockRecQithwt.Visible = false;
                    RDTasselIssueReceiveSummary.Visible = true;
                    RDTasselPartnerIssueSummary.Visible = true;
                    RDTasselPartnerReceiveSummary.Visible = true;
                    RDTasselMakingRawIssueDetail.Visible = true;
                    break;
                case 28:
                    if (Convert.ToInt16(Session["varSubCompanyId"]) == 285)
                    {
                        RDTasselIssueReceiveSummary.Visible = true;
                        RDTasselPartnerIssueSummary.Visible = true;
                        RDTasselPartnerReceiveSummary.Visible = true;
                        RDTasselMakingRawIssueDetail.Visible = true;
                    }
                    break;
                case 31:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    //TRQualityWiseSummary.Visible = true;
                    break;
                case 38:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    //RDCommDetail.Visible = false;
                    //RDStockNoTobeIssued.Visible = false;
                    //RDPendingQty.Visible = false;
                    //RDStockRecQithwt.Visible = false;
                    ////TRQualityWiseSummary.Visible = true;
                    break;
                case 39:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    break;
                case 42:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    break;
                case 43:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    RDFinishingRecSummaryWithTDS.Visible = true;
                    RDFinishingHissabSummary.Visible = true;
                    RDFinishingNillBalance.Visible = true;
                    RDFinishingRegisterSummary.Visible = true;
                    break;
                case 44:
                    RDTasselIssueReceiveSummary.Visible = true;
                    RDTasselPartnerIssueSummary.Visible = true;
                    RDTasselPartnerReceiveSummary.Visible = true;
                    RDTasselMakingRawIssueDetail.Visible = true;
                    RDProcessIssRecDetail.Visible = true;
                    RDFinishingpending.Visible = true;
                    RDFinishingIssueDetail.Visible = true;
                    RDProcessIssueReceiveSummary.Visible = true;
                    RDProcessWiseAdvancePayment.Visible = true;
                    RDProcessOrderFolio.Visible = false;
                    RDCommDetail.Visible = false;
                    RDStockNoTobeIssued.Visible = false;
                    RDPendingQty.Visible = false;
                    RDStockRecQithwt.Visible = false;

                    RDGatePass.Visible = false;
                    RDDailyfinreport.Visible = false;
                    RDProcessIssRecDetailWithConsumpton.Visible = false;
                    RDWeaverRawMaterialIssueDetail.Visible = false;
                    RDWeaverRawMaterialReceiveDetail.Visible = false;

                    ChkForProcessIssRecSummary.Visible = false;
                    ChkSummary.Visible = false;
                    RDPerday.Visible = false;
                    chksumm.Visible = true;
                    chkall.Visible = true;


                    break;
                case 247:
                    RDWeaverRawMaterialIssueDetail.Visible = true;
                    break;

            }
            if (RDProcessIssRecDetail.Visible == true)
            {
                RDProcessIssRecDetail.Checked = true;
                RDProcessIssRecDetail_CheckedChanged(sender, new EventArgs());
            }

            if (RDProcessIssRecDetail.Checked == true)
            {
                TRIssueNo.Visible = true;

            }
            else
            {
                Chkissueno.Checked = false;
                TRIssueNo.Visible = false;
                TDexcelExport.Visible = false;
            }
            if (RDProcessIssRecDetail.Checked == true || RDFinishingIssueDetail.Checked == true)
            {
                TDexcelExport.Visible = true;
            }
            else
            {
                TDexcelExport.Visible = false;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarFinishingNewModuleWise == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EmpName", true, "--Select--");
        }
        //  EmpSelectedChanged();
        if (RDFinishingpending.Checked == true || RDFinishingIssueDetail.Checked == true || RDProcessIssRecDetail.Checked == true || RDProcessIssRecDetail.Checked == true || RDPerday.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");
        }
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanged();
    }
    private void EmpSelectedChanged()
    {
        string Str = "";
        if (DDProcessName.SelectedIndex > 0)
        {
            if (ChkForProcessIssRecSummary.Checked == true || RDProcessOrderFolio.Checked == true || RDPendingQty.Checked == true || RDStockRecQithwt.Checked == true || RDFinishingIssueDetail.Checked == true || RDCommDetail.Checked == true || RDWeaverRawMaterialIssueDetail.Checked == true || RDWeaverRawMaterialReceiveDetail.Checked == true || RDProcessIssueReceiveSummary.Checked == true)
            {
                Str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 
                    From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(Nolock) 
                    join EmpInfo ei on pim.Empid=ei.EmpId 
                    WHERE PIm.Companyid=" + DDCompany.SelectedValue + "";
                if (ChkForComplete.Checked == true)
                {
                    Str = Str + " And  PIM.Status = 'Complete' ";
                }
                else
                {
                    Str = Str + " And PIM.Status = 'Pending' ";
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    Str = Str + " And ei.EmpId=" + DDEmpName.SelectedValue;
                }
                Str = Str + " UNION  ";
                Str = Str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 
                From Process_issue_Master_" + DDProcessName.SelectedValue + @" pim(Nolock) 
                join employee_processorderno emp(Nolock) on pim.issueorderid=emp.issueorderid and emp.ProcessId=" + DDProcessName.SelectedValue + @"";
                if (DDEmpName.SelectedIndex > 0)
                {
                    Str = Str + " And Emp.EmpId=" + DDEmpName.SelectedValue;
                }

                Str = Str + " Where pim.Empid=0 And PIm.Companyid=" + DDCompany.SelectedValue + "";

                if (ChkForComplete.Checked == true)
                {
                    Str = Str + " And  PIM.Status = 'Complete' ";
                }
                else
                {
                    Str = Str + " And PIM.Status = 'Pending' ";
                }

                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
                Label4.Text = "Iss Challan No.";
            }
            else
            {
                if (RDProcessIssRecDetail.Checked == true)
                {
                    Str = "Select Process_Rec_Id,isnull(ChallanNo,Process_Rec_Id) as Process_Rec_Id from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "(Nolock) Where CompanyId=" + DDCompany.SelectedValue;
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        Str = Str + " And EmpId=" + DDEmpName.SelectedValue;
                    }
                    Str = Str + " UNION  ";
                    Str = Str + @" select Distinct PRM.Process_Rec_Id,isnull(PRM.ChallanNo,PRM.Process_Rec_Id) as Process_Rec_Id1 
                        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PRM(Nolock) 
                        JOIN Employee_ProcessReceiveNo Emp(Nolock) on PRM.Process_Rec_Id=Emp.Process_Rec_Id and Emp.ProcessId=" + DDProcessName.SelectedValue + @"";
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        Str = Str + " And Emp.EmpId=" + DDEmpName.SelectedValue;
                    }
                    Str = Str + " Where PRM.Companyid=" + DDCompany.SelectedValue + "";
                }
                else
                {

                    Str = "Select Process_Rec_Id,isnull(ChallanNo,Process_Rec_Id) as Process_Rec_Id from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "(Nolock) Where CompanyId=" + DDCompany.SelectedValue;
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        Str = Str + " And EmpId=" + DDEmpName.SelectedValue;
                    }
                }

                //Str = "Select Process_Rec_Id,Process_Rec_Id from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where CompanyId=" + DDCompany.SelectedValue;

                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
                Label4.Text = "Rec Challan No.";
            }

            //ChallanNoSelectedIndexChange();
            UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");
        }
        if (RDGatePass.Checked == true)
        {
            //Str = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME from View_StockTranGetPassDetail PM,V_FinishedItemDetail VF Where PM.finishedid=VF.Item_finished_id And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    Str = Str + " AND PM.Partyid =" + DDEmpName.SelectedValue + " Order BY VF.CATEGORY_NAME";
            //}
            Str = @"SELECT CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER order by CATEGORY_NAME";
            UtilityModule.ConditionalComboFill(ref DDCategory, Str, true, "--Select--");
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedIndexChange();
    }
    private void ChallanNoSelectedIndexChange()
    {
        string Str = "";
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && RDFinishingpending.Checked == false && RDFinishingIssueDetail.Checked == false && RDWeaverRawMaterialIssueDetail.Checked == false && RDWeaverRawMaterialReceiveDetail.Checked == false)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str = "Select replace(convert(varchar(11),AssignDate,106), ' ','-') As Date From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + "(Nolock) Where 1=1";
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str = Str + " and IssueOrderID=" + DDChallanNo.SelectedValue + "";
                }

                Str1 = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME 
                    from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM(Nolock)
                        JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD(Nolock) ON PM.IssueOrderID=PD.IssueOrderID 
                        JOIN V_FinishedItemDetail VF Where PD.Item_Finished_Id=VF.Item_Finished_Id 
                        Where PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str = "Select replace(convert(varchar(11),ReceiveDate,106), ' ','-') As Date From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where CompanyId=" + DDCompany.SelectedValue;
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str = Str + " And Process_Rec_Id= " + DDChallanNo.SelectedValue;
                }

                Str1 = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDCategory, Str1, true, "--Select--");

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0 && DDChallanNo.SelectedIndex > 0)
            {
                TxtFromDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
                TxtToDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            }
        }
    }
    protected void ChkForProcessIssRecSummary_CheckedChanged(object sender, EventArgs e)
    {
        ProcessIssRecCheckedChanged();
    }
    private void ProcessIssRecCheckedChanged()
    {
        if (ChkForProcessIssRecSummary.Checked == true)
        {
            Label4.Text = "Po No/Folio No";
            ChkSummary.Visible = false;
            ChkForPendingStockNo.Visible = true;
        }
        else
        {
            Label4.Text = "Rec Challan No";
            ChkSummary.Visible = true;
            ChkForPendingStockNo.Visible = false;
        }
        EmpSelectedChanged();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (RDProcessIssRecDetail.Checked == true && chkexcelexport.Checked == true && Session["varMasterCompanyIDForERP"].ToString() != "16")
        {
            Processreceiveexcelexport();
            return;
        }
        if (RDFinishingIssueDetail.Checked == true && chkexcelexport.Checked == true && Session["varMasterCompanyIDForERP"].ToString() != "16")
        {
            ProcessIssueExcelExport();
            return;
        }
        if (RDProcessIssRecDetail.Checked == true && chkjobwisesummary.Checked == true && Session["varMasterCompanyIDForERP"].ToString() != "16")
        {
            if (lblMessage.Text == "")
            {
                CHECKVALIDCONTROL();
                SqlConnection con2 = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                con2.Open();
                SqlTransaction tran2 = con2.BeginTransaction();
                try
                {
                    if (RDProcessIssRecDetail.Checked == true && chkjobwisesummary.Checked == true && Session["varMasterCompanyIDForERP"].ToString() != "16")
                    {
                        if (Session["varMasterCompanyIDForERP"].ToString() == "27")
                        {
                            JobWiseProcessReceiveSummaryFolioWise(tran2);
                            return;
                        }
                        if (Session["varMasterCompanyIDForERP"].ToString() == "42")
                        {
                            JobWiseProcessReceiveSummaryVikramMirzapur();
                            return;
                        }
                        else
                        {
                            JobWiseProcessReceiveSummary();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
                    tran2.Rollback();
                    lblMessage.Text = ex.Message;
                    lblMessage.Visible = true;
                }
                finally
                {
                    con2.Close();
                    con2.Dispose();
                }
            }
        }
        if (RDProcessIssRecDetail.Checked == true && ChkForIssueDate.Checked == true && Session["varMasterCompanyIDForERP"].ToString() != "16")
        {
            if (ChkForIssueDate.Checked == true && ChkSummary.Checked == true)
            {
                ProcessIssueReceiveExcelExport();
                return;
            }
            else if (ChkForIssueDate.Checked == true && ChkSummary.Checked == false)
            {
                Session["ReportPath"] = "Reports/RptProcessIssueDetailNewIssueNoWiseRecQty.rpt";
                ProcessIssueReceiveExcelExport();
                return;
            }
        }
        if (RDWeaverRawMaterialIssueDetail.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "31")
            {
                if (TRQualityWiseSummary.Visible == true && ChkQualityWiseSummary.Checked == true)
                {
                    WeaverRawMaterialIssueWithConsumptionSummaryReport();
                    return;
                }
                else
                {
                    WeaverRawMaterialIssueWithConsumptionDetailReport();
                    return;
                }
            }
            else if (Session["VarCompanyNo"].ToString() == "38")
            {
                WeaverRawMaterialIssueWithConsumptionDetailReportVikram();
                return;
            }
            else
            {
                WeaverRawMaterialIssueDetailReport();
                return;
            }
        }
        if (RDWeaverRawMaterialReceiveDetail.Checked == true)
        {
            WeaverRawMaterialReceiveDetailReport();
            return;
        }
        if (RDProcessIssRecDetail.Checked == true)
        {
            //********Finishing New Module Wise
            if (variable.VarFinishingNewModuleWise == "1" && (Convert.ToInt16(DDProcessName.SelectedIndex <= 0 ? "0" : DDProcessName.SelectedValue) > 1) || chksizesummary.Checked == true || ChkBuyerItemSizeWiseSummary.Checked == true || ChkQualityDesignSizeWiseSummary.Checked == true || ChkQualitySizeWiseHissabSummary.Checked== true)
            {
                if (Chkissueno.Checked == true)
                {
                    if (txtissueno.Text.Trim() == "")
                    {
                        lblMessage.Text = "Please Enter Issue No.";

                        return;
                    }
                    //***********For Champo Carpets
                    switch (Session["varMasterCompanyIDForERP"].ToString())
                    {
                        case "16":
                            if (UtilityModule.Temp_ReportCheck(Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(txtissueno.Text), "ProcessReceiveDetail", Convert.ToInt32(Session["varuserid"])) == false)
                            {

                                txtpwd.Focus();
                                Popup(true);
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                    //***********
                }
                if (chkexcelexport.Checked == true)
                {
                    Finishinghissab_ExcelExport();
                }
                else
                {
                    if (ChkSummary.Checked == true || ChkBuyerItemSizeWiseSummary.Checked == true)
                    {
                        if (Session["varMasterCompanyIDForERP"].ToString() == "22")
                        {
                            ProcessReceiveDetailSummaryDiamondExport();
                            return;
                        }
                        else if (Session["VarCompanyNo"].ToString() == "38")
                        {
                            JobWiseProcessReceiveSummary_Vikram();
                            return;
                        }
                        else if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            JobWiseProcessReceiveSummary_CI();
                            return;
                        }
                        else
                        {
                            ProcessReceiveDetailSummary();
                            return;
                        }

                    }
                    else
                    {
                        if (Session["varMasterCompanyIDForERP"].ToString() == "22")
                        {
                            if (ChkWithRecTime.Checked == true)
                            {
                                Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWiseDiamond.rpt";
                            }
                            else
                            {
                                Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise.rpt";
                            }
                        }
                        else if (Session["varMasterCompanyIDForERP"].ToString() == "38")
                        {
                            Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise_VikramKM.rpt";
                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise.rpt";
                        }
                        Finishinghissab();
                    }

                }

                return;
            }
        }
        if (RDTasselIssueReceiveSummary.Checked == true)
        {
            TassleRawMaterialReceiveDetailReport();
            return;
        }
        else if (RDTasselPartnerIssueSummary.Checked == true)
        {
            TassleRawPartnerIssueReport();
            return;

        }
        else if (RDTasselPartnerReceiveSummary.Checked == true)
        {
            TassleRawPartnerReceiveReport();
            return;

        }
        else if (RDTasselMakingRawIssueDetail.Checked == true)
        {
            TasselMakingRawIssueDetailReport();
            return;
        }

        if (RDFinishingBalance.Checked == true)
        {
            FinishingReceiveBalanceReport();
            return;
        }
        if (RDProcessWiseAdvancePayment.Checked == true)
        {
            ProcessWiseAdvancePaymentDetail();
            return;
        }
        if (RDFinishingRecSummaryWithTDS.Checked == true)
        {
            FinishingReceiveSummaryWithTDSReport_CI();
            return;
        }
        if (RDFinishingHissabSummary.Checked == true)
        {
            FinishingHissabSummaryReport_CI();
            return;
        }
        if (RDFinishingNillBalance.Checked == true)
        {
            FinishingReceiveNillBalanceReport();
            return;
        }

        if (RDFinishingRegisterSummary.Checked == true)
        {
            FinishingRegisterSummaryReport_CI();
            return;
        }

        if (lblMessage.Text == "")
        {
            CHECKVALIDCONTROL();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (RDProcessIssRecDetail.Checked == true)
                {

                    if (ChkForProcessIssRecSummary.Checked == true)
                    {
                        ProcessIssRecSummary(tran);
                        if (ChkForPendingStockNo.Checked == true)
                        {
                            Session["ReportPath"] = "Reports/RptProcessIssRecSummaryWithStockNoNEW.rpt";
                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/RptProcessIssRecSummaryNEW.rpt";
                        }
                    }
                    else
                    {
                        //NextProcessRecDetail(tran);
                        if (ChkSummary.Checked == true)
                        {
                            Session["ReportPath"] = "Reports/RptProcessSummaryNEW.rpt";
                        }
                        else if (Chkissueno.Checked == true)
                        {
                            Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise.rpt";
                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/RptProcessDetailNEW.rpt";
                        }
                    }
                    Session["CommanFormula"] = "";
                    Report1(tran);

                }

                if (RDGatePass.Checked == true)
                {
                    GateINOrPassDetail(tran);
                }
                if (RDProcessIssRecDetailWithConsumpton.Checked == true)
                {
                    ProcessIssRecDetailWithConsumpton(tran);
                }
                if (RDCommDetail.Checked == true)
                {
                    CommissionDetail(tran);
                }
                if (RDStockNoTobeIssued.Checked == true)
                {
                    StockNo_TobeIssued(tran);
                }

                if (RDProcessOrderFolio.Checked == true)
                {
                    ProcessOrderFolio(tran);
                }
                if (RDPendingQty.Checked == true)
                {
                    ProcessLatePendingQty(tran);
                }
                if (RDStockRecQithwt.Checked == true)
                {
                    StockRecDetailWithWeight(tran);
                }
                else if (RDPerday.Checked == true)
                {
                    Perdayproductionstatus(tran);
                }
                else if (RDFinishingpending.Checked == true)
                {
                    FinishingPendingPcs(tran);
                }
                else if (RDFinishingIssueDetail.Checked == true)
                {
                    if (ChkSummary.Checked == true)
                    {
                        if (Session["VarCompanyNo"].ToString() == "38")
                        {
                            ProcessIssueSummaryExcelExportVikramKM();
                            return;
                        }
                        else if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            FinishingIssueDetailSummary(tran);
                        }
                        else
                        {
                            ProcessIssueSummaryExcelExport();
                            return;
                        }
                    }
                    else
                    {
                        FinishingIssueDetail(tran);
                    }
                }
                else if (RDDailyfinreport.Checked == true)
                {
                    Finishinghissab_ExcelExport();
                }
                else if (RDProcessIssueReceiveSummary.Checked == true)
                {
                    if (Session["VarCompanyNo"].ToString() == "44")
                    {
                        FinishingIssueReceiveSummaryagni(tran);
                    }
                    else
                    {
                        FinishingIssueReceiveSummary(tran);

                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
                tran.Rollback();
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
    }
    protected void Processreceiveexcelexport()
    {
        if (chkexcelexport.Checked == true)
        {
            string str = "";
            DataSet ds = new DataSet();

            //str = @" select PNM.PROCESS_NAME as JobName,OM.LocalOrder as SRNO,OM.CustomerOrderNo as [ORDER No],Ei.EmpName as [Contractor Name],PD.IssueOrderId as FolioNo,
            //         PM.ChallanNo,vf.ITEM_NAME,vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+pd.width+'x'+pd.length as ItemDescription 
            //         ,Sum(PD.Qty) as Qty,Sum(PD.qty*PD.Area) as Area,PD.Rate,sum(pd.amount) as Amount 
            //         From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.Process_Rec_Id=PD.Process_Rec_Id
            //         inner Join V_FinishedItemDetail  vf on PD.Item_Finished_Id=vf.ITEM_FINISHED_ID
            //         inner Join ordermaster om on PD.OrderId=om.orderid
            //         inner join empinfo ei on PM.empid=ei.EmpId
            //         inner join Process_name_master PNM on PNM.PROCESS_NAME_ID=" + DDProcessName.SelectedValue + " Where PM.CompanyId=" + DDCompany.SelectedValue;

            //Check Conditions
            if (ChkForDate.Checked == true)
            {
                str = str + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }

            ////            str = str + @"  group by  PNM.PROCESS_NAME,OM.LocalOrder,OM.CustomerOrderNo,Ei.EmpName,PD.IssueOrderId ,PM.ChallanNo,vf.ITEM_NAME,
            ////                                         vf.QualityName,vf.designName,vf.ColorName,vf.ShapeName,pd.width,pd.length,pd.Rate";

            //SqlParameter[] param = new SqlParameter[5];
            //param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            //param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            //param[2] = new SqlParameter("@where", str);
            //param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //param[4] = new SqlParameter("MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetprocessreceiveExcel", param);

            //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("Pro_GetprocessreceiveExcel", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                //Export to excel
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                string filename = "";
                filename = (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedItem.Text : "") + " ReceiveDetail";
                if (ChkForDate.Checked == true)
                {
                    filename = filename + "_From" + TxtFromDate.Text + "_To " + TxtToDate.Text;
                }
                filename = filename + ".xls";

                Response.AddHeader("content-disposition",
                 "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        }
    }
    protected void ProcessIssueExcelExport()
    {
        if (chkexcelexport.Checked == true)
        {
            string str = "";

            //str = @" select PNM.PROCESS_NAME as JobName,OM.LocalOrder as SRNO,OM.CustomerOrderNo as [ORDER No],Ei.EmpName as [Contractor Name],PD.IssueOrderId as FolioNo,
            //         PM.ChallanNo,vf.ITEM_NAME,vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+pd.width+'x'+pd.length as ItemDescription 
            //         ,Sum(PD.Qty) as Qty,Sum(PD.qty*PD.Area) as Area,PD.Rate,sum(pd.amount) as Amount 
            //         From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.Process_Rec_Id=PD.Process_Rec_Id
            //         inner Join V_FinishedItemDetail  vf on PD.Item_Finished_Id=vf.ITEM_FINISHED_ID
            //         inner Join ordermaster om on PD.OrderId=om.orderid
            //         inner join empinfo ei on PM.empid=ei.EmpId
            //         inner join Process_name_master PNM on PNM.PROCESS_NAME_ID=" + DDProcessName.SelectedValue + " Where PM.CompanyId=" + DDCompany.SelectedValue;

            //Check Conditions
            if (ChkForDate.Checked == true)
            {
                str = str + " And PIM.AssignDate>='" + TxtFromDate.Text + "' And PIM.AssignDate<='" + TxtToDate.Text + "'";
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And PIM.EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " And PIM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }


            //str = str + @"  group by  PNM.PROCESS_NAME,OM.LocalOrder,OM.CustomerOrderNo,Ei.EmpName,PD.IssueOrderId ,PM.ChallanNo,vf.ITEM_NAME,
            //                                         vf.QualityName,vf.designName,vf.ColorName,vf.ShapeName,pd.width,pd.length,pd.Rate";

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetProcessIssueExcel", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Export to excel
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                string filename = "";
                filename = (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedItem.Text : "") + " IssueDetail";
                if (ChkForDate.Checked == true)
                {
                    filename = filename + "_From" + TxtFromDate.Text + "_To " + TxtToDate.Text;
                }
                filename = filename + ".xls";

                Response.AddHeader("content-disposition",
                 "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        }
    }
    public void ProcessIssRecDetailWithConsumpton(SqlTransaction Tran)
    {
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);

        array[0].Value = DDCompany.SelectedValue;
        if (DDProcessName.SelectedIndex > 0)
        {
            array[1].Value = DDProcessName.SelectedValue;
        }
        else
        {
            array[1].Value = 0;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            array[2].Value = DDEmpName.SelectedValue;
        }
        else
        {
            array[2].Value = 0;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            array[3].Value = DDChallanNo.SelectedValue;
        }
        else
        {
            array[3].Value = 0;
        }
        DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_ProcessIssRecDetailWithConsumpton", array);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessIssRecWithConsumption.xsd";
            if (Session["varMasterCompanyIDForERP"].ToString() == "27")
            {
                Session["rptFileName"] = "Reports/RptProcessIssRecWithConsumptionAntique.rpt";
            }
            else
            {
                Session["rptFileName"] = "Reports/RptProcessIssRecWithConsumption.rpt";
            }

            Session["GetDataset"] = Ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }

    }
    public void GateINOrPassDetail(SqlTransaction Tran)
    {
        DataSet DS = new DataSet();
        string Str = "";
        string ViewGatepass = "";
        Session["CommanFormula"] = "";
        if (Session["varCompanyNo"].ToString() == "15")
        {
            Session["ReportPath"] = "Reports/RptGateInPassDetailEMBHHeadOffice.rpt";
        }
        else
        {
            Session["ReportPath"] = "Reports/RptGateInPassDetail.rpt";
        }

        if (Session["varCompanyNo"].ToString() == "14")
        {
            ViewGatepass = "VIEW_STOCKTRANGETPASSDETAIL_EASTERNIKEA";
        }
        else if (Session["varCompanyNo"].ToString() == "9")
        {
            ViewGatepass = "VIEW_STOCKTRANGETPASSDETAIL_Hafizia";
        }
        else
        {
            ViewGatepass = "View_StockTranGetPassDetail";
        }

        if (Session["varCompanyNo"].ToString() == "9")
        {
            Str = @"Select ReceiveDate Date,V1.Category_Name,V1.Item_Name,V1.QualityName,V1.designName,V1.ColorName,V1.ShadeColorName,V1.ShapeName,V1.SizeMtr,V2.GateInNo+' / '+V2.ReceiveNo GateInNo,V2.LotNo,Sum(V2.Qty) Qty,e.EmpName IssuedTo,V2.TranType,V2.Remarks ReasonToIssue,V2.FolioNo,
                 CI.CompanyName,Ci.Compaddr1,Ci.gstno,v2.rate,V2.GateInNo  as GateInoutNo,V2.ReceiveNo as GateinoutrecNo,V1.designName+' '+V1.ColorName+' '+V1.ShadeColorName+' '+V1.ShapeName+' '+V1.SizeMtr as Description,V2.Billdate,V2.MainRemarks,V2.TagNo
                 From V_FinishedItemDetail V1(NoLock) 
                JOIN " + ViewGatepass + @" V2(NoLock)  ON V1.Item_Finished_ID=V2.finishedID
                JOIN Empinfo E(NoLock)  ON V2.Partyid=e.empid
                JOIN Companyinfo Ci(NoLock)  ON V2.CompanyId=ci.companyId
                Where V1.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            if (DDCompany.SelectedIndex != -1)
            {
                Str = Str + " and V2.CompanyId=" + DDCompany.SelectedValue;
            }
            if (DDgateinpasstype.SelectedValue != "-1")
            {
                Str = Str + " and V2.TranType=" + DDgateinpasstype.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                Str = Str + " And ReceiveDate>='" + TxtFromDate.Text + "' And ReceiveDate<='" + TxtToDate.Text + "'";
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                Str = Str + " And V2.PartyID=" + DDEmpName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                Str = Str + " And V1.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str = Str + " And V1.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                Str = Str + " And V1.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                Str = Str + " And V1.ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (DDLotNo.SelectedIndex > 0 && TRlotNo.Visible == true)
            {
                Str = Str + " And V2.LotNo='" + DDLotNo.SelectedValue + "'";
            }
            if (TxtGateINPassNo.Text != "")
            {
                Str = Str + " AND V2.GateInNo='" + TxtGateINPassNo.Text.Trim() + "'";
            }
            Str = Str + " Group BY  ReceiveDate, CI.CompanyName,Ci.Compaddr1,Ci.gstno,V1.Category_Name,V1.Item_Name,V1.QualityName,V1.designName,V1.ColorName,V1.ShadeColorName,V1.ShapeName,V1.SizeMtr,V2.GateInNo,V2.LotNo,e.EmpName,V2.TranType,V2.Remarks,V2.ReceiveNo,V2.FolioNo,v2.Rate,v2.billdate,V2.MainRemarks,V2.TagNo  Order By GateInNo";
        }
        else
        {
            Str = @"Select ReceiveDate Date,V1.Category_Name,V1.Item_Name,V1.QualityName,V1.designName,V1.ColorName,V1.ShadeColorName,V1.ShapeName,V1.SizeMtr,V2.GateInNo+' / '+V2.ReceiveNo GateInNo,V2.LotNo,Sum(V2.Qty) Qty,e.EmpName IssuedTo,V2.TranType,V2.Remarks ReasonToIssue,V2.FolioNo,
                 CI.CompanyName,Ci.Compaddr1,Ci.gstno,v2.rate,V2.GateInNo  as GateInoutNo,V2.ReceiveNo as GateinoutrecNo,V1.designName+' '+V1.ColorName+' '+V1.ShadeColorName+' '+V1.ShapeName+' '+V1.SizeMtr as Description,V2.Billdate,V2.MainRemarks
                From V_FinishedItemDetail V1," + ViewGatepass + " V2, Empinfo e,Companyinfo Ci Where V1.Item_Finished_ID=V2.finishedID AND V2.Partyid=e.empid and V2.CompanyId=ci.companyId And V1.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (DDCompany.SelectedIndex != -1)
            {
                Str = Str + " and V2.CompanyId=" + DDCompany.SelectedValue;
            }
            if (DDgateinpasstype.SelectedValue != "-1")
            {
                Str = Str + " and V2.TranType=" + DDgateinpasstype.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                Str = Str + " And ReceiveDate>='" + TxtFromDate.Text + "' And ReceiveDate<='" + TxtToDate.Text + "'";
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                Str = Str + " And V2.PartyID=" + DDEmpName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                Str = Str + " And V1.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str = Str + " And V1.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                Str = Str + " And V1.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                Str = Str + " And V1.ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (DDLotNo.SelectedIndex > 0 && TRlotNo.Visible == true)
            {
                Str = Str + " And V2.LotNo='" + DDLotNo.SelectedValue + "'";
            }
            if (TxtGateINPassNo.Text != "")
            {
                Str = Str + " AND V2.GateInNo='" + TxtGateINPassNo.Text.Trim() + "'";
            }
            Str = Str + " Group BY  ReceiveDate, CI.CompanyName,Ci.Compaddr1,Ci.gstno,V1.Category_Name,V1.Item_Name,V1.QualityName,V1.designName,V1.ColorName,V1.ShadeColorName,V1.ShapeName,V1.SizeMtr,V2.GateInNo,V2.LotNo,e.EmpName,V2.TranType,V2.Remarks,V2.ReceiveNo,V2.FolioNo,v2.Rate,v2.billdate,V2.MainRemarks Order By GateInNo";
        }



        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);

        if (DS.Tables[0].Rows.Count > 0)
        {
            if (chkexcelexport.Checked == true)
            {
                if (Session["VarCompanyNo"].ToString() == "9")
                {
                    GateInPassDetail_Excel_Hafizia(DS);
                }
                else
                {
                    GateInPassDetail_Excel(DS);
                }
            }
            else
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = DS;
                Session["dsFileName"] = "~\\ReportSchema\\GateInPassDetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void GateInPassDetail_Excel(DataSet ds)
    {
        string Path = "";
        string label = "", labelH = "";
        switch (DDgateinpasstype.SelectedValue)
        {
            case "0":
                label = "GATE OUT DETAIL";
                labelH = "Gate Out No.";
                break;
            case "1":
                label = "GATE IN DETAIL";
                labelH = "Gate In No.";
                break;
            default:
                label = "GATE IN / OUT DETAIL";
                labelH = "Gate In / Out No.";
                break;
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("sheet1");
        //*************
        sht.Range("A1:M1").Merge();
        sht.Range("A1:M1").Style.Font.FontSize = 11;
        sht.Range("A1:M1").Style.Font.Bold = true;
        sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        label = label + "-" + (ChkForDate.Checked == true ? ("From Date " + TxtFromDate.Text + "  ToDate " + TxtToDate.Text + "") : "");
        sht.Range("A1").SetValue(label);
        sht.Row(1).Height = 21.75;

        sht.Range("A2").SetValue("Date");
        sht.Range("B2").SetValue("Employee Name");
        sht.Range("C2").SetValue(labelH);
        sht.Range("D2").SetValue("Bill No.");
        sht.Range("E2").SetValue("Lot No.");
        sht.Range("F2").SetValue("Item Name");
        sht.Range("G2").SetValue("Quality");
        sht.Range("H2").SetValue("Description");
        sht.Range("I2").SetValue("Quantity");
        sht.Range("J2").SetValue("Rate");
        sht.Range("K2").SetValue("Amount");
        sht.Range("L2").SetValue("Tran Type");
        sht.Range("M2").SetValue("Bill Date");
        sht.Range("N2").SetValue("Remark");

        sht.Range("A2:N2").Style.Font.Bold = true;

        int row = 3;
        int rowfrom = 0;
        int rowto = 0;

        DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Date", "Issuedto", "gateinoutrecno");

        DataView dv1 = new DataView(dtdistinct);
        dv1.Sort = "Date,Issuedto,gateinoutrecno";

        DataTable dt = dv1.ToTable();

        foreach (DataRow dr in dt.Rows)
        {
            DataView dvitemdesc = new DataView(ds.Tables[0]);
            dvitemdesc.RowFilter = "Date='" + dr["Date"] + "' and Issuedto='" + dr["issuedto"] + "' and Gateinoutrecno='" + dr["Gateinoutrecno"] + "'";
            dvitemdesc.Sort = "Issuedto";
            DataSet dsitemdesc = new DataSet();
            dsitemdesc.Tables.Add(dvitemdesc.ToTable());

            rowfrom = row;

            for (int i = 0; i < dsitemdesc.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(dr["Date"]);
                sht.Range("B" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["issuedto"]);
                sht.Range("C" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["gateinoutNo"]);
                sht.Range("D" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["gateinoutrecno"]);
                sht.Range("E" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["item_name"]);
                sht.Range("G" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["qualityname"]);
                sht.Range("H" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Description"]);
                sht.Range("I" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["qty"]);
                sht.Range("J" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Rate"]);
                sht.Range("K" + row).FormulaA1 = "=I" + row + '*' + "$J$" + row;                //=I3*J3
                sht.Range("L" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["TranType"].ToString() == "1" ? "IN" : "OUT");
                sht.Range("M" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["billdate"]);
                sht.Range("N" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["ReasonToIssue"]);
                row = row + 1;
            }

            rowto = row;
            sht.Range("H" + row).SetValue("TOTAL");
            sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (rowto - 1) + ")";
            sht.Range("K" + row).FormulaA1 = "=SUM(K" + rowfrom + ":K" + (rowto - 1) + ")";
            sht.Range("A" + row + ":N" + row).Style.Font.Bold = true;
            row = row + 1;

        }
        using (var a = sht.Range("A2" + ":N" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }
        //**********
        sht.Columns(1, 20).AdjustToContents();
        //**********Save File
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename(label + "-" + DateTime.Now + "." + Fileextension);
        Path = Server.MapPath("~/Tempexcel/" + filename);
        xapp.SaveAs(Path);
        xapp.Dispose();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.WriteFile(Path);
        Response.End();
        return;
    }
    private void Finishinghissab()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.customerid=" + DDcustcode.SelectedValue;
            where = where + " Cust Code:" + DDcustcode.SelectedItem.Text;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
            where = where + " Order NO:" + DDorderno.SelectedItem.Text;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        int ReportType = 0;
        if (chksizesummary.Checked == true || ChkQualityDesignSizeWiseSummary.Checked==true || ChkQualitySizeWiseHissabSummary.Checked==true)
        {
            ReportType = 2;
        }


        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}

        //SqlCommand cmd = new SqlCommand("FinishinghissabchallanWise", con);
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandTimeout = 30000;

        //cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        //cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        //cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        //cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        //cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        //cmd.Parameters.AddWithValue("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        //cmd.Parameters.AddWithValue("@Where", strCondition);
        //cmd.Parameters.AddWithValue("@ReportType", ReportType);

        //SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //cmd.ExecuteNonQuery();
        //ad.Fill(ds);

        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);
        param[7] = new SqlParameter("@ReportType", ReportType);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FinishinghissabchallanWise", param);

        Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNEW.xsd";

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chksizesummary.Checked == true)
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    Session["rptFileName"] = "~\\Reports\\RptQualitySizeWiseFinishingProcessSummaryCI.rpt";                    
                    Session["GetDataset"] = ds;
                    //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
                    StringBuilder stb2 = new StringBuilder();
                    stb2.Append("<script>");
                    stb2.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb2.ToString(), false);
                }
                else
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    int row = 0;

                    //******************

                    sht.Range("A1").Value = where;
                    sht.Range("A1:C1").Style.Alignment.SetWrapText();
                    sht.Range("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:C1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1:C1").Merge();
                    sht.Row(1).Height = 44;

                    sht.Range("A2").Value = "Size";
                    sht.Range("B2").Value = "Qty";
                    sht.Range("C2").Value = "Area";
                    sht.Range("B2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Width", "Length", "ShapeName");
                    DataView dv1 = new DataView(dtdistinct);
                    dv1.Sort = "Width,Length,ShapeName";
                    DataTable dtdistinct1 = dv1.ToTable();
                    row = 3;

                    foreach (DataRow dr in dtdistinct1.Rows)
                    {
                        sht.Range("A" + row).SetValue(dr["width"] + "x" + dr["Length"] + "  (" + dr["ShapeName"] + ")");
                        var qty = ds.Tables[0].Compute("sum(qty)", "Width='" + dr["width"] + "' and Length='" + dr["Length"] + "' And ShapeName = '" + dr["ShapeName"] + "'");
                        var Area = ds.Tables[0].Compute("sum(Area)", "Width='" + dr["width"] + "' and Length='" + dr["Length"] + "' And ShapeName = '" + dr["ShapeName"] + "'");
                        sht.Range("B" + row).SetValue(qty == DBNull.Value ? 0 : qty);
                        sht.Range("C" + row).SetValue(Area == DBNull.Value ? 0 : Area);
                        row = row + 1;

                    }
                    //********Grand TOtal
                    sht.Range("A" + row).Value = "GRAND TOTAL";
                    sht.Range("B" + row).FormulaA1 = "=SUM(B3:$B$" + (row - 1) + ")";
                    sht.Range("C" + row).FormulaA1 = "=SUM(C3:$C$" + (row - 1) + ")";
                    sht.Columns(1, 10).AdjustToContents();
                    //sht.Rows().AdjustToContents();

                    using (var a = sht.Range("A2" + ":C" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("Processreceivesummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                    return;
                }


            }
            else if (ChkQualityDesignSizeWiseSummary.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptQualityDesignSizeWiseFinishingProcessSummary.rpt";
                Session["GetDataset"] = ds;
                //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
                StringBuilder stb2 = new StringBuilder();
                stb2.Append("<script>");
                stb2.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb2.ToString(), false);
            }
            else if (ChkQualitySizeWiseHissabSummary.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptQualitySizeWiseProcessHissabSummaryCI.rpt";
                Session["GetDataset"] = ds;
                //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
                StringBuilder stb3 = new StringBuilder();
                stb3.Append("<script>");
                stb3.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb3.ToString(), false);
            }
            else
            {
                //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void Finishinghissab_ExcelExport()
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);

        ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FinishinghissabchallanWise", param);
        DataView dv = new DataView(ds1.Tables[0]);
        dv.Sort = "Receivedate";
        ds.Tables.Add(dv.ToTable());


        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            string label = "JOB - " + DDProcessName.SelectedItem.Text;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("FINISHERRECEIVEDETAIL");
            //*************
            sht.Range("A1:N1").Merge();
            sht.Range("A1:N1").Style.Font.FontSize = 11;
            sht.Range("A1:N1").Style.Font.Bold = true;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            label = label + "-" + (ChkForDate.Checked == true ? ("From Date " + TxtFromDate.Text + "  ToDate " + TxtToDate.Text + "") : "");
            sht.Range("A1").SetValue("FINISHER RECEIVE DETAIL " + label);
            sht.Row(1).Height = 21.75;
            //Header
            sht.Range("A2:N2").Style.Font.FontSize = 11;
            sht.Range("A2:N2").Style.Font.Bold = true;
            sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(2).Height = 18.00;
            //
            sht.Range("A2").SetValue("Employee Name");
            sht.Range("B2").SetValue("Challan No.");
            sht.Range("C2").SetValue("ReceiveDate");
            sht.Range("D2").SetValue("Carpet_No.");
            sht.Range("E2").SetValue("Quality");
            sht.Range("F2").SetValue("Design");
            sht.Range("G2").SetValue("Color");
            sht.Range("H2").SetValue("Shape");
            sht.Range("I2").SetValue("Size");
            if (RDDailyfinreport.Checked == false)
            {
                sht.Range("J2").SetValue("Area");
                sht.Range("K2").SetValue("Rate");
                sht.Range("L2").SetValue("Amount");
                sht.Range("M2").SetValue("Penality");
                sht.Range("N2").SetValue("Net_Amount");
            }
            int Row = 3;
            decimal amount = 0, penality = 0, Tamount = 0, Tpenality = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + Row + ":N" + Row).Style.Font.FontSize = 11;
                sht.Range("J" + Row + ":N" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["Empname"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["Issueorderid"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);

                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["Shapename"]);
                sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["sizeft"].ToString());
                if (RDDailyfinreport.Checked == false)
                {
                    sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                    sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["Area"]);
                    sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    amount = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"]), 2, MidpointRounding.AwayFromZero);
                    sht.Range("L" + Row).SetValue(amount);
                    penality = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Penality"]), 2, MidpointRounding.AwayFromZero);
                    sht.Range("M" + Row).SetValue(penality);
                    sht.Range("N" + Row).SetValue(amount - penality);
                }
                Row = Row + 1;
            }
            //********
            if (RDDailyfinreport.Checked == false)
            {
                Tamount = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(amount)", "")), 2, MidpointRounding.AwayFromZero);
                Tpenality = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(Penality)", "")), 2, MidpointRounding.AwayFromZero);
                sht.Range("L" + Row).SetValue(Tamount);
                sht.Range("M" + Row).SetValue(Tpenality);
                sht.Range("N" + Row).SetValue(Tamount - Tpenality);
            }

            //**********
            sht.Columns(1, 20).AdjustToContents();
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FINISHER CARPET RECEIVE DETAIL_" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void FinishingIssueDetail(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Issueorderid=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);
        //param[7] = new SqlParameter("@ChkForSummary", ChkSummary.Checked==true ? "1" : "0");

        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "FinishingIssueDetail", param);


        if (ds.Tables[0].Rows.Count > 0)
        {

            if (Session["varMasterCompanyIDForERP"].ToString() == "22")
            {
                if (ChkWithRecTime.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptfinishingissuedetailDiamond.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\Rptfinishingissuedetail.rpt";
                }

            }            
            else
            {
                Session["rptFileName"] = "~\\Reports\\Rptfinishingissuedetail.rpt";
            }
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rptfinishingissuedetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void Report1(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.EmpId=" + DDEmpName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And Om.customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PD.orderid=" + DDorderno.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        if (Convert.ToString(Session["ReportPath"]) == "Reports/RptProcessIssRecSummaryWithStockNoNEW.rpt")
        {
            string qry = @" SELECT TEMP_PROCESS_ISS_REC_DETAIL.IssueOrderId,TEMP_PROCESS_ISS_REC_DETAIL.Qty,TEMP_PROCESS_ISS_REC_DETAIL.RQty,TEMP_PROCESS_ISS_REC_DETAIL.PQty,V_FinishedItemDetail.QualityName,V_FinishedItemDetail.designName,
V_FinishedItemDetail.ColorName,V_FinishedItemDetail.SizeMtr,TEMP_PROCESS_ISS_REC_DETAIL.UnitId,V_FinishedItemDetail.SizeFt,TEMP_PROCESS_ISS_REC_DETAIL.Empid,EmpInfo.EmpName,TEMP_PROCESS_ISS_REC_DETAIL.TStockNo,
TEMP_PROCESS_ISS_REC_DETAIL.AssignDate,CancelQty
 FROM   TEMP_PROCESS_ISS_REC_DETAIL INNER JOIN EmpInfo ON TEMP_PROCESS_ISS_REC_DETAIL.Empid=EmpInfo.EmpId And EmpInfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"  INNER JOIN V_FinishedItemDetail ON TEMP_PROCESS_ISS_REC_DETAIL.Item_Finished_id=V_FinishedItemDetail.ITEM_FINISHED_ID
 ORDER BY TEMP_PROCESS_ISS_REC_DETAIL.Empid,TEMP_PROCESS_ISS_REC_DETAIL.IssueOrderId";
            ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessIssRecSummaryWithStockNoNEW.xsd";
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptProcessIssRecSummaryNEW.rpt")
        {
            string qry = @" SELECT TEMP_PROCESS_ISS_REC_DETAIL.IssueOrderId,TEMP_PROCESS_ISS_REC_DETAIL.Qty,TEMP_PROCESS_ISS_REC_DETAIL.RQty,TEMP_PROCESS_ISS_REC_DETAIL.PQty,V_FinishedItemDetail.QualityName,
V_FinishedItemDetail.designName,V_FinishedItemDetail.ColorName,V_FinishedItemDetail.SizeMtr,TEMP_PROCESS_ISS_REC_DETAIL.UnitId,V_FinishedItemDetail.SizeFt,TEMP_PROCESS_ISS_REC_DETAIL.Empid,EmpInfo.EmpName
 ,CancelQty FROM   TEMP_PROCESS_ISS_REC_DETAIL INNER JOIN EmpInfo ON TEMP_PROCESS_ISS_REC_DETAIL.Empid=EmpInfo.EmpId And Empinfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN V_FinishedItemDetail ON TEMP_PROCESS_ISS_REC_DETAIL.Item_Finished_id=V_FinishedItemDetail.ITEM_FINISHED_ID
 ORDER BY TEMP_PROCESS_ISS_REC_DETAIL.Empid,TEMP_PROCESS_ISS_REC_DETAIL.IssueOrderId";
            ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessIssRecSummaryNEW.xsd";
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptProcessSummaryNEW.rpt")
        {
            string qry = "";
            qry = @" Select CI.CompanyName,PD.Area*PD.Qty Area,PD.Rate,PD.Amount,PD.Qty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate,EI.EmpName,
                     (select Process_Name from Process_name_Master Where Process_name_id=" + DDProcessName.SelectedValue + @") PROCESS_NAME,
                     PD.Length,PD.Width,vf.CATEGORY_NAME,vf.ITEM_NAME,vf.QualityName,vf.designName,vf.ColorName,vf.ShadeColorName,
                     vf.ShapeName,PM.UnitId,vf.QualityId,vf.designId,vf.SizeId,vf.ITEM_ID,PD.Penality,PD.Comm,PD.CommAmt,isnull(TDSPercentage,0) TDSPercentage,PM.Process_Rec_Id,ReceiveDate,ChallanNo,U.UnitName
                     From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF,Companyinfo CI,Empinfo EI,Unit U,ordermaster om
                     Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id And PM.Companyid=CI.COmpanyid And U.unitId=PM.UnitId And Pd.orderid=Om.orderid and
                     PM.Empid=EI.Empid And PD.Qty<>0 And PD.Qualitytype<>3  And VF.MasterCompanyId=" + Session["varcompanyNo"];
            qry = qry + strCondition;
            ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessSummaryNEW.xsd";

        }
        else if ((Convert.ToString(Session["ReportPath"]) == "Reports/RptProcessDetailNEW.rpt") || (Convert.ToString(Session["ReportPath"]) == "Reports/RptProcessDetailNEWIssueNoWise.rpt"))
        {
            string qry = "";
            qry = @"Select CI.CompanyName,(Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise]
                    (PD.Process_Rec_Detail_Id," + DDProcessName.SelectedValue + @",Issue_Detail_Id)) TStockNo,PD.Area*PD.Qty Area,PD.Rate,PD.Amount,
                    PD.Qty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate,EI.EmpName,(select Process_Name from Process_name_Master Where Process_name_id=" + DDProcessName.SelectedValue + @") PROCESS_NAME,
                    PD.Length,PD.Width,vf.CATEGORY_NAME,vf.ITEM_NAME,vf.QualityName,vf.designName,vf.ColorName,vf.ShadeColorName,
                    vf.ShapeName,PM.UnitId,PD.Penality," + (ChkForDate.Checked == true ? "1" : "0") + @" as Dateflag,PM.Receivedate,PD.issueorderid,VF.MasterCompanyId
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF,Companyinfo CI,Empinfo EI,ordermaster om
                    Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id And PM.Companyid=CI.COmpanyid And  Pd.orderid=Om.orderid and
                    PM.Empid=EI.Empid And PD.Qty<>0 And PD.Qualitytype<>3  And VF.MasterCompanyId=" + Session["varcompanyNo"];
            if (Chkissueno.Checked == true)
            {
                if (txtissueno.Text != "")
                {
                    strCondition = strCondition + "  and PD.issueorderid=" + txtissueno.Text;
                }
            }
            qry = qry + strCondition;

            ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNEW.xsd";
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void NextProcessRecDetail(SqlTransaction tran)
    {
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table Temp_StockNo_Wise_Process_Detail");
        string Str = @"Insert into Temp_StockNo_Wise_Process_Detail Select 0 StockNo,";
        if (ChkSummary.Checked == true)
        {
            Str = Str + " '' TStockNo,";
        }
        else
        {
            Str = Str + " (Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise](PD.Process_Rec_Detail_Id," + DDProcessName.SelectedValue + @",Issue_Detail_Id)) TStockNo,";
        }
        Str = Str + @" PD.Orderid,PD.Item_Finished_id,PD.Length,PD.Width,PD.Area*PD.Qty Area,PD.Rate,PD.Amount,PD.Qty,PM.CompanyId,PM.EmpId,PM.ReceiveDate,PM.Process_Rec_Id,
                      " + DDProcessName.SelectedValue + "," + Session["varuserid"] + @"," + Session["varMasterCompanyIDForERP"] + ",'" + TxtFromDate.Text + "','" + TxtToDate.Text + @"',PM.UnitId,PD.Penality,Comm,CommAmt,Isnull(TDSPercentage,0) TDSPercentage,ChallanNo
                      From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF
                      Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id And PD.Qty<>0 And PD.Qualitytype<>3 And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ChkForDate.Checked == true)
        {
            Str = Str + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            Str = Str + " And PM.EmpId=" + DDEmpName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            Str = Str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Str = Str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            Str = Str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            Str = Str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            Str = Str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            Str = Str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            Str = Str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            Str = Str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
    }
    private void ProcessIssRecSummary(SqlTransaction tran)
    {
        string Str = "";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table TEMP_PROCESS_ISS_REC_DETAIL");

        Str = @"Insert into TEMP_PROCESS_ISS_REC_DETAIL SELECT PM.IssueOrderId,PM.Empid,PM.UnitId,PM.CompanyId,PM.AssignDate,PD.Item_Finished_id,Sum(PD.Qty) Qty,
               Sum((PD.Qty-isnull(cancelQty,0))*PD.Area) Area,Sum(PD.Qty-PD.PQty) RQty,Sum(PD.PQty)-isnull(Sum(CancelQty),0) PQty,PD.Orderid,PD.ReqByDate,DateDiff(Day,PD.ReqByDate,PM.AssignDate) LateDays,
               " + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ",'',isnull(Sum(cancelQty),0) cancelQty  FROM PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
               PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderId=PD.IssueOrderId And PD.Item_Finished_Id=VF.Item_Finished_Id And 
               PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ChkForDate.Checked == true)
        {
            Str = Str + " And PM.AssignDate>='" + TxtFromDate.Text + "' And PM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            Str = Str + " And PM.Empid=" + DDEmpName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And PM.IssueOrderId=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            Str = Str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Str = Str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            Str = Str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            Str = Str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            Str = Str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            Str = Str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            Str = Str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            Str = Str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        Str = Str + " Group By PM.IssueOrderId,PM.Empid,PM.UnitId,PM.CompanyId,PM.AssignDate,PD.Item_Finished_id,PD.Orderid,PD.ReqByDate";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        if (ChkForPendingStockNo.Checked == true)
        {
            Str = @"Update TEMP_PROCESS_ISS_REC_DETAIL Set TStockNo=(Select * From [dbo].[Get_Pending_StockNo" + DDProcessName.SelectedValue + "](IssueOrderId," + DDProcessName.SelectedValue + ",Item_Finished_id)) Where PQty>0";
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (TRProcessName.Visible == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() != "44")
            {
                if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
                {
                    goto a;
                }
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS(sender);
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.QualityId,VF.QualityName from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.QualityId,VF.QualityName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            Str1 = Str1 + " Order By VF.QualityName ";
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        else
        {
            Str1 = @"Select Distinct VF.Qualityid,VF.QualityNAME from V_FinishedItemDetail VF Where  VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and vf.qualityname<>''";
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.Category_id=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            Str1 = Str1 + " Order BY VF.QualityNAME ";
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        if (DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedIndex = 0;
            DDQuality_SelectedIndexChanged(sender, new EventArgs());
        }
        //************
        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName From Shape", true, "--Plz Select--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDGatePass.Checked != true)
        {
            QDCSDDFill(DDDesign, DDColor, DDShape, DDShadeColor);
        }
    }
    private void CATEGORY_DEPENDS_CONTROLS(object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");

        }
        if (RDGatePass.Checked == true)
        {
            TRlotNo.Visible = true;

            UtilityModule.ConditionalComboFill(ref DDLotNo, "select  Distinct LotNo,LotNo from stock Where CompanyId=" + DDCompany.SelectedValue, true, "--Select--");

            //            Str1 = @"Select Distinct VF.Item_id,VF.Item_NAME from View_StockTranGetPassDetail PM,
            //                        V_FinishedItemDetail VF Where PM.finishedid=VF.Item_finished_id And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            //            if (DDCategory.SelectedIndex > 0)
            //            {
            //                Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            //            }
            //            Str1 = Str1 + " Order BY VF.Item_NAME ";
            Str1 = @"select ITEM_ID,ITEM_NAME From ITEM_MASTER Im  WHere 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " and Im.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            Str1 = Str1 + " order by ITEM_NAME";

            UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");
        }
        else
        {
            TRlotNo.Visible = false;
        }
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;

                        //                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from View_StockTranGetPassDetail PM,
                        //                        V_FinishedItemDetail VF Where PM.finishedid=VF.Item_finished_id And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                        //                        if (DDCategory.SelectedIndex > 0)
                        //                        {
                        //                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        //                        }
                        //                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and vf.shadecolorname<>''";
                        if (DDCategory.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        }
                        if (ddItemName.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Item_id=" + ddItemName.SelectedValue;
                        }
                        if (DDQuality.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Qualityid=" + DDQuality.SelectedValue;

                        }
                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
                        break;
                }
            }
        }
        if (ddItemName.Items.Count > 0)
        {
            ddItemName.SelectedIndex = 0;
            if (sender != null)
            {
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    private void QDCSDDFill(DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.designId,VF.designName from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.designId,VF.designName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Designname";
            UtilityModule.ConditionalComboFill(ref DDDesign, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.ColorId,VF.ColorName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.ColorId,VF.ColorName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Colorname";
            UtilityModule.ConditionalComboFill(ref DDColor, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }

            UtilityModule.ConditionalComboFill(ref DDShape, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
        }

    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblshadename.Text = ParameterList[7];
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        string strSize = "Sizeft";
        if (chkmtr.Checked == true)
        {
            strSize = "Sizemtr";
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            if (ChkForProcessIssRecSummary.Checked == true)
            {
                Str1 = @"Select Distinct VF.SizeId,VF." + strSize + " as Size From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.IssueOrderID=PD.IssueOrderID And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else
            {
                Str1 = @"Select Distinct VF.SizeId,VF." + strSize + " as size From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                }
            }
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            //}
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by size";
            UtilityModule.ConditionalComboFill(ref DDSize, Str1, true, "--Select--");
        }
    }

    protected void RDGatePass_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDGatePass.Checked == true)
        {
            TDexcelExport.Visible = true;
            TRlotNo.Visible = true;
            TRGatePass.Visible = true;
            TRRecChallan.Visible = false;
            TRProcessName.Visible = false;
            string str = @"select Distinct LotNo,LotNo from stock(Nolock) Where CompanyId=" + DDCompany.SelectedValue + @"
            Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end EmpNameNew from Empinfo EI Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EmpNameNew";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDLotNo, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmpName, ds, 1, true, "--Select--");
        }
        else
        {
            TRlotNo.Visible = false;
            TRGatePass.Visible = false;
            TRProcessName.Visible = true;

        }
    }
    protected void RDProcessIssRecDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        trIssueDate.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
        if (RDProcessIssRecDetail.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = true;
            }
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            TDJobWiseSummary.Visible = true;
            TRChkBoxIssueDate.Visible = true;
            TRlotNo.Visible = false;
            TRGatePass.Visible = false;
            TRRecChallan.Visible = true;
            TRProcessName.Visible = true;
            ChkForDate.Checked = false;
            TDexcelExport.Visible = true;
            TDsizesummary.Visible = true;
            if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["usertype"].ToString() == "28")
            {
                TRBuyerItemSizeWiseSummary.Visible = true;
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "43")
            {
                TRQualityDesignSizeWiseSummary.Visible = true;
                TRQualitySizeWiseHissabSummary.Visible = true;
            }

            UtilityModule.ConditionalComboFill(ref DDEmpName, @"Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end EmpName 
            from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName", true, "--Select--");
        }
    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
        }
        else
        {
            trDates.Visible = false;
        }
    }
    protected void ChkForIssueDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForIssueDate.Checked == true)
        {
            trIssueDate.Visible = true;
        }
        else
        {
            trIssueDate.Visible = false;
        }

    }
    protected void RDProcessIssRecDetailWithConsumpton_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDProcessIssRecDetailWithConsumpton.Checked == true)
        {
            ChkForProcessIssRecSummary.Checked = true;
            ProcessIssRecCheckedChanged();
            ChkForDate.Checked = false;
            //ChkForDate_CheckedChanged(sender, e);
        }
    }
    protected void StockNo_TobeIssued(SqlTransaction Tran)
    {
        String str = @"select  Distinct PM.IssueOrderId As Foliono,E.EmpName,(select * from dbo.Get_Pending_StockNoTobeIssued_" + DDProcessName.SelectedValue + "(PM.IssueOrderId)) As StockNo,Process_Name from Process_stock_detail PSD,CarpetNumber CN,Process_Issue_Master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + " PD,Process_Name_Master PNM,Empinfo E Where CN.StockNo=PSD.StockNo And Currentprostatus=" + DDProcessName.SelectedValue + @"
                    And Pack=0 And E.EmpId=PM.EmpId And IssRecStatus=0 And PM.IssueOrderId=PD.IssueOrderId And PSD.IssueDetailId=PD.Issue_Detail_Id And PNM.PROCESS_NAME_ID=CurrentProStatus";

        if (DDCompany.SelectedIndex != -1)
        {
            str = str + " And Pm.CompanyId=" + DDCompany.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + " And Pm.EmpId=" + DDEmpName.SelectedValue + "";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And Pm.IssueOrderId= " + DDChallanNo.SelectedValue + "";
        }
        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptStockNoToBe_Issued.xsd";
            Session["rptFileName"] = "Reports/RptStockNoToBe_Issued.rpt";
            Session["GetDataSet"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No record Found!');", true);
        }
    }
    protected void CommissionDetail(SqlTransaction Tran)
    {

        //Execute Through Procedure
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Companyid", DDCompany.SelectedIndex != -1 ? DDCompany.SelectedValue : "0");
        param[2] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[3] = new SqlParameter("@fromdate", TxtFromDate.Text);
        param[4] = new SqlParameter("@Todate", TxtToDate.Text);
        param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
        param[6] = new SqlParameter("@Issueorderid", DDChallanNo.SelectedIndex > 0 ? DDChallanNo.SelectedValue : "0");
        param[7] = new SqlParameter("@EMpid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
        //************
        DataSet ds;
        #region
        //        string str;

        //        int Dateflag = 0;
        //        if (ChkForDate.Checked == true)
        //        {
        //            Dateflag = 1;
        //        }

        //        str = @"select CI.CompanyName,EI.EmpName,IssueOrderId,PROCESS_NAME,ReceiveDate,Sum(CommAmt) As CommAmt,0 As PaidAmt,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + Dateflag + @" As Dateflag,Ci.Compaddr1,CI.gstno from VIEW_PROCESS_RECEIVE_MASTER VPM,VIEW_PROCESS_RECEIVE_DETAIL VPD,CompanyInfo CI,Empinfo EI,Process_Name_Master PM
        //              Where VPM.Process_Rec_Id=VPD.Process_Rec_Id And VPM.CompanyId=CI.CompanyId And VPM.EmpId=EI.EMpId And PM.PROCESS_NAME_ID=VPM.Processid And VPM.ProcessId=" + DDProcessName.SelectedValue + " And Process_Rec_Detail_Id in(select Process_Rec_Detail_Id From VIEW_PROCESS_RECEIVE_DETAIL Where ProcessId=" + DDProcessName.SelectedValue + @" And QualityType<>3)";

        //        if (DDCompany.SelectedIndex != -1)
        //        {
        //            str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
        //        }
        //        if (DDEmpName.SelectedIndex > 0)
        //        {
        //            str = str + " And EI.Empid=" + DDEmpName.SelectedValue;
        //        }
        //        if (ChkForDate.Checked == true)
        //        {
        //            str = str + " and ReceiveDate>='" + TxtFromDate.Text + "' And ReceiveDate<='" + TxtToDate.Text + "'";
        //        }
        //        str = str + " group by CI.CompanyName,Ci.Compaddr1,CI.gstno,EI.EmpName,IssueOrderId,PROCESS_NAME,ReceiveDate having Sum(CommAmt)<>0";
        //        str = str + " Union";

        //        str = str + @" select CI.CompanyName,EI.EMpname,ProcessOrderNo,PROCESS_NAME,Date,0 As CommAmount,Sum(Amount) As PaidAmount,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + Dateflag + @" As Dateflag,Ci.Compaddr1,CI.gstno from PROCESS_HISSAB PH,Companyinfo CI,Empinfo EI,Process_Name_Master PM  where PH.CompanyId=CI.CompanyId And PH.EmpId=EI.EmpId And PH.ProcessId=Pm.PROCESS_NAME_ID And  CommPaymentFlag=1
        //                        And ProcessId=" + DDProcessName.SelectedValue;
        //        if (DDCompany.SelectedIndex != -1)
        //        {
        //            str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
        //        }
        //        if (DDEmpName.SelectedIndex > 0)
        //        {
        //            str = str + " And Ei.EmpId=" + DDEmpName.SelectedValue + "";
        //        }
        //        if (ChkForDate.Checked == true)
        //        {
        //            str = str + " and Date>='" + TxtFromDate.Text + "' And Date<='" + TxtToDate.Text + "'";
        //        }
        //        str = str + " group by CI.CompanyName,Ci.Compaddr1,CI.gstno,ProcessOrderNo,EI.EMpname,PROCESS_NAME,date";

        //        str = str + " Union";

        //        str = str + @" select CI.CompanyName,EI.EMpname,PHP.FolioNo,PROCESS_NAME,Date,0 As CommAmount,isnull(Sum(DrAmt),0) As PaidAmount,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + Dateflag + @" As Dateflag,Ci.Compaddr1,CI.gstno from PROCESSHISSABPAYMENT PHP,Companyinfo CI,Empinfo EI,Process_Name_Master PM  where PHP.CompanyId=CI.CompanyId And PHP.PartyId=EI.EmpId And PHP.ProcessId=Pm.PROCESS_NAME_ID And  CommPaymentFlag=1
        //                        And ProcessId=" + DDProcessName.SelectedValue;
        //        if (DDCompany.SelectedIndex != -1)
        //        {
        //            str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
        //        }
        //        if (DDEmpName.SelectedIndex > 0)
        //        {
        //            str = str + " And Ei.EmpId=" + DDEmpName.SelectedValue + "";
        //        }
        //        if (ChkForDate.Checked == true)
        //        {
        //            str = str + " and Date>='" + TxtFromDate.Text + "' And Date<='" + TxtToDate.Text + "'";
        //        }
        //        str = str + " group by CI.CompanyName,Ci.Compaddr1,CI.gstno,PHP.FolioNo,EI.EMpname,PROCESS_NAME,date";
        #endregion
        //ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETCOMMDETAILREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessCommissionDetail.xsd";
            if (Session["varMasterCompanyIDForERP"].ToString() == "15")
            {
                Session["rptFileName"] = "Reports/RptProcessCommissionDetailEMHD.rpt";
            }
            else
            {
                Session["rptFileName"] = "Reports/RptProcessCommissionDetail.rpt";
            }
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }


    protected void RDProcessOrderFolio_CheckedChanged(object sender, EventArgs e)
    {
        TRAsOnDate.Visible = false;
        Label4.Text = "Iss Challan No";
        EmpSelectedChanged();
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRQualityWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRQualitySizeWiseHissabSummary.Visible = false;

    }
    protected void ProcessOrderFolio(SqlTransaction Tran)
    {
        SqlParameter[] _array = new SqlParameter[7];
        _array[0] = new SqlParameter("@Companyid", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        _array[3] = new SqlParameter("@IssueOrderid", SqlDbType.Int);

        _array[0].Value = DDCompany.SelectedValue;
        _array[1].Value = DDProcessName.SelectedValue;
        _array[2].Value = 0;// DDEmpName.SelectedValue;
        _array[3].Value = DDChallanNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_ForOrderFolio", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            // Table 3 For Show image in crystal Report
            ds.Tables[3].Columns.Add("Image", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[3].Rows)
            {

                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["Image"] = img_Byte;
                    }
                }
            }
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessOrderFolio.xsd";
            Session["rptFileName"] = "Reports/RptProcessOrderFolio.rpt";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void ProcessLatePendingQty(SqlTransaction Tran)
    {
        SqlParameter[] _array = new SqlParameter[8];
        _array[0] = new SqlParameter("@Companyid", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        _array[3] = new SqlParameter("@FromDate", SqlDbType.SmallDateTime);
        _array[4] = new SqlParameter("@ToDate", SqlDbType.SmallDateTime);
        _array[5] = new SqlParameter("@ProcessName", SqlDbType.VarChar, 10);
        _array[6] = new SqlParameter("@Userid", SqlDbType.Int);
        _array[7] = new SqlParameter("@IssueOrderId", SqlDbType.Int);

        // _array[3] = new SqlParameter("@IssueOrderid", SqlDbType.Int);

        _array[0].Value = DDCompany.SelectedValue;
        _array[1].Value = DDProcessName.SelectedValue;
        _array[2].Value = DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue;
        _array[3].Value = TxtFromDate.Text;
        _array[4].Value = TxtToDate.Text;
        _array[5].Value = DDProcessName.SelectedItem.Text;
        _array[6].Value = Session["varuserid"];
        _array[7].Value = DDChallanNo.SelectedIndex <= 0 ? "0" : DDChallanNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetLatePendingQty", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptLateProcessPendingQty.xsd";
            Session["rptFileName"] = "Reports/RptLateProcessPendingQty.rpt";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void StockRecDetailWithWeight(SqlTransaction Tran)
    {
        int Dateflag = 0;
        if (ChkForDate.Checked == true)
        {
            Dateflag = 1;
        }
        string Str = @"select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as Todate," + Dateflag + " as dateflag from v_getWeaverBazarwithweight V where V.Companyid=" + DDCompany.SelectedValue;
        if (DDEmpName.SelectedIndex > 0)
        {
            Str = Str + " And V.empid= " + DDEmpName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And V.issueOrderId= " + DDChallanNo.SelectedValue;
        }
        if (ChkForDate.Checked == true)
        {
            Str = Str + " And V.Receivedate>= '" + TxtFromDate.Text + "'  and V.Receivedate<='" + TxtToDate.Text + "'";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            Str = Str + " And V.Category_Id=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Str = Str + " And V.Item_Id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And V.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Str = Str + " And V.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Str = Str + " And V.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Str = Str + " And V.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Str = Str + " And V.SizeId=" + DDSize.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptStockRecDetailWithWeight.xsd";
            Session["rptFileName"] = "Reports/RptStockRecDetailWithWeight.rpt";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }


    protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }
    protected void Perdayproductionstatus(SqlTransaction Tran)
    {
        string str = "";
        str = " Where PRM.companyId=" + DDCompany.SelectedValue;
        str = str + " And PRM.ReceiveDate>='" + TxtFromDate.Text + "' and PRM.ReceiveDate<='" + TxtToDate.Text + "'";
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + " and PRM.empid=" + DDEmpName.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        }

        //****************
        SqlParameter[] arr = new SqlParameter[3];
        arr[0] = new SqlParameter("@where", str);
        arr[1] = new SqlParameter("@fromdate", TxtFromDate.Text);
        arr[2] = new SqlParameter("@Todate", TxtToDate.Text);

        //***************
        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_PerdayproductionstatusOther", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatusother.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptperdayproductionstatusother.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void FinishingPendingPcs(SqlTransaction Tran)
    {
        string str = "";
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " and om.customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " and om.orderid=" + DDorderno.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        }
        //****************
        SqlParameter[] arr = new SqlParameter[8];
        arr[0] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
        arr[1] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
        arr[2] = new SqlParameter("@fromdate", TxtFromDate.Text);
        arr[3] = new SqlParameter("@Todate", TxtToDate.Text);
        arr[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        arr[5] = new SqlParameter("@Where", str);
        arr[6] = new SqlParameter("@IssueOrderId", txtissueno.Text);
        arr[7] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        //***************
        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_getFinisherPendingpcs", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "38" || Session["VarCompanyNo"].ToString() == "42")
            {
                if (ChkSummary.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingPendingPcsWithoutStockNoWise.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingpendingpcs.rpt";
                }
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingpendingpcs.rpt";
            }
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingpendingpcs.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "FinishingPendingPcs", "alert('No records found!!!');", true);
        }
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (variable.VarReportPwd == txtpwd.Text)
        {
            Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise.rpt";
            Finishinghissab();
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please Enter Correct Password..";

        }

    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void Chkissueno_CheckedChanged(object sender, EventArgs e)
    {
        BtnPreview.Visible = true;
        BtnPreview1.Visible = false;

        if (Chkissueno.Checked == true)
        {
            BtnPreview1.Visible = true;
            BtnPreview.Visible = false;
        }
    }
    protected void RDFinishingIssueDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDFinishingIssueDetail.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = true;
            }
            TDexcelExport.Visible = true;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
        }
    }
    protected void RDCommDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
    }
    protected void RDStockNoTobeIssued_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDPendingQty_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDStockRecQithwt_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDPerday_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = false;
        TRorderno.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDFinishingpending_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = true;
        TRorderno.Visible = true;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDDailyfinreport_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRChkBoxIssueDate.Visible = false;
        trIssueDate.Visible = false;
        TRcustcode.Visible = true;
        TRorderno.Visible = true;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
    }
    protected void RDProcessIssueReceiveSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDProcessIssueReceiveSummary.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = false;
            }
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
        }
    }
    protected void RDTasselIssueReceiveSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
        if (RDTasselIssueReceiveSummary.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = false;
            }
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
        }
    }
    protected void RDTasselPartnerIssueSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDTasselPartnerIssueSummary.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = false;
            }
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
        }
    }
    protected void RDTasselPartnerReceiveSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
        if (RDTasselPartnerReceiveSummary.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" && Session["usertype"].ToString() == "1")
            {
                TRCheckWithTime.Visible = false;
            }
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
        }
    }
    protected void RDFinishingBalance_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
        TRRecChallan.Visible = false;
        trDates.Visible = false;
        TR1.Visible = false;
        TRIssueNo.Visible = false;
        ChkForComplete.Visible = false;
        TRAsOnDate.Visible = false;
        ChkForDate.Visible = false;
        TRCategoryName.Visible = true;
        TRddItemName.Visible = true;

        if (RDFinishingBalance.Checked == true)
        {
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
            TRRecChallan.Visible = false;
            trDates.Visible = false;
            TR1.Visible = false;
            TRIssueNo.Visible = false;
            ChkForComplete.Visible = false;
            TRAsOnDate.Visible = true;
            ChkForDate.Visible = false;
        }
    }
    protected void RDFinishingNillBalance_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;
        TRRecChallan.Visible = false;
        trDates.Visible = false;
        TR1.Visible = false;
        TRIssueNo.Visible = false;
        ChkForComplete.Visible = false;
        TRAsOnDate.Visible = false;
        ChkForDate.Visible = false;
        TRCategoryName.Visible = true;
        TRddItemName.Visible = true;

        if (RDFinishingNillBalance.Checked == true)
        {
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            trIssueDate.Visible = false;
            TRcustcode.Visible = false;
            TRorderno.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            ChkSummary.Visible = false;
            TRRecChallan.Visible = false;
            trDates.Visible = false;
            TR1.Visible = false;
            TRIssueNo.Visible = false;
            ChkForComplete.Visible = false;
            TRAsOnDate.Visible = false;
            ChkForDate.Visible = false;
        }
    }
    protected void ProcessIssueReceiveExcelExport()
    {
        if (ChkForIssueDate.Checked == true)
        {
            string str = "";
            string filterby = "From : " + txtIssueFromDate.Text + "  To : " + txtIssueToDate.Text;

            //Check Conditions
            if (ChkForIssueDate.Checked == true)
            {
                str = str + " And PM.AssignDate>='" + txtIssueFromDate.Text + "' And PM.AssignDate<='" + txtIssueToDate.Text + "'";
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And PIM.EmpId=" + DDEmpName.SelectedValue;
                filterby = filterby + " Emp : " + DDEmpName.SelectedItem.Text;
            }
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " And PIM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                filterby = filterby + " Challan No : " + DDChallanNo.SelectedItem.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                filterby = filterby + " Category : " + DDCategory.SelectedItem.Text;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
                filterby = filterby + " Item : " + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
                filterby = filterby + " Quality : " + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
                filterby = filterby + " Design : " + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
                filterby = filterby + " Color : " + DDColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
                filterby = filterby + " ShapeId : " + DDShape.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
                filterby = filterby + " SizeId : " + DDSize.SelectedItem.Text;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                filterby = filterby + " ShadecolorId : " + DDShadeColor.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
                filterby = filterby + " Localorder : " + txtlocalorderno.Text;
            }

            SqlParameter[] arr = new SqlParameter[9];
            arr[0] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            arr[1] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            arr[2] = new SqlParameter("@fromdate", txtIssueFromDate.Text);
            arr[3] = new SqlParameter("@Todate", txtIssueToDate.Text);
            arr[4] = new SqlParameter("@Dateflag", ChkForIssueDate.Checked == true ? "1" : "0");
            arr[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
            arr[6] = new SqlParameter("@Where", str);
            arr[7] = new SqlParameter("@userid", Session["varuserid"]);
            arr[8] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FinishingIssueReceiveDetailExcelReport", arr);

            Session["dsFileName"] = "~\\ReportSchema\\RptProcessIssueDetailNewIssueNoWiseRecQty.xsd";

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkSummary.Checked == true)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    Decimal TQty = 0, TAmount = 0;
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    int row = 0;
                    //***********
                    sht.Range("A1:I1").Merge();
                    sht.Range("A1").Value = "Process Issue Received Details " + "For - " + DDCompany.SelectedItem.Text;
                    sht.Range("A2:I2").Merge();
                    sht.Range("A2").Value = "Filter By :  " + filterby;
                    sht.Row(2).Height = 30;
                    sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:I2").Style.Font.Bold = true;
                    //***********Filter By Item_Name

                    sht.Range("A3").Value = "Emp Name";
                    sht.Range("B3").Value = "Quality";
                    sht.Range("C3").Value = "Design";
                    sht.Range("D3").Value = "Color";
                    sht.Range("E3").Value = "Shape";

                    sht.Range("F3").Value = "Size";
                    sht.Range("G3").Value = "Rec Qty";
                    //sht.Range("A" + row).Style.Font.SetBold();
                    sht.Range("A3:G3").Style.Font.SetBold();

                    row = 3;
                    Decimal tqty = 0;
                    string empname = "";
                    int rowfrom = 0, rowto = 0;

                    DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "NoofEmp");

                    foreach (DataRow dr in dtdistinct.Rows)
                    {
                        DataView dv = new DataView(ds.Tables[0]);
                        dv.RowFilter = "NoofEmp='" + dr["NoofEmp"] + "'";
                        DataSet ds1 = new DataSet();
                        ds1.Tables.Add(dv.ToTable());

                        DataTable dtdistinct1 = ds1.Tables[0].DefaultView.ToTable(true, "EmpName");

                        foreach (DataRow dr2 in dtdistinct1.Rows)
                        {
                            DataView dv1 = new DataView(ds.Tables[0]);
                            dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                            DataSet ds2 = new DataSet();
                            ds2.Tables.Add(dv1.ToTable());

                            //row = row + 1;

                            //sht.Range("A" + row).Value = "Emp Name";
                            //sht.Range("B" + row).Value = "Quality";
                            //sht.Range("C" + row).Value = "Design";
                            //sht.Range("D" + row).Value = "Color";
                            //sht.Range("E" + row).Value = "Shape";

                            //sht.Range("F" + row).Value = "Size";
                            //sht.Range("G" + row).Value = "Rec Qty";                      
                            //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                            DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "Item_Finished_Id");

                            rowfrom = row + 1;
                            foreach (DataRow dritem in dtdistinctItem.Rows)
                            {
                                row = row + 1;
                                sht.Range("A" + row).SetValue(dritem["empname"]);
                                sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                                sht.Range("C" + row).SetValue(dritem["DesignName"]);
                                sht.Range("D" + row).SetValue(dritem["ColorName"]);
                                sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                                sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                                var qty = ds.Tables[0].Compute("sum(Recqty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "'");
                                tqty = tqty + Convert.ToDecimal(qty);
                                sht.Range("G" + row).SetValue(qty);

                                rowto = row;
                            }
                            row = row + 1;
                            sht.Range("F" + row).Value = "Total";
                            sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
                            sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
                            row = row + 1;

                            //using (var a = sht.Range("A" + (rowfrom-1) + ":G" + (rowto+1)))
                            //{
                            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //}
                        }
                    }

                    //*************GRAND TOTAL
                    sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                    sht.Range("F" + row).Value = "Grand Total";
                    sht.Range("G" + row).SetValue(tqty);
                    sht.Range("F" + row + ":G" + row).Style.Font.SetBold();

                    using (var a = sht.Range("A3:G" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    }
                    //*************
                    sht.Columns(1, 10).AdjustToContents();
                    //********************
                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("ProcessReceiveSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                }
                else
                {
                    Session["rptFileName"] = Session["ReportPath"];
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        }
    }
    protected void ProcessIssueSummaryExcelExport()
    {
        if (ChkForDate.Checked == true)
        {
            string str = "";
            string filterby = "From : " + TxtFromDate.Text + "  To : " + TxtToDate.Text;

            //Check Conditions
            if (ChkForDate.Checked == true)
            {
                str = str + " And PM.AssignDate>='" + TxtFromDate.Text + "' And PM.AssignDate<='" + TxtToDate.Text + "'";
            }
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    str = str + " And PIM.EmpId=" + DDEmpName.SelectedValue;
            //    filterby = filterby + " Emp : " + DDEmpName.SelectedItem.Text;
            //}
            if (DDChallanNo.SelectedIndex > 0)
            {
                //str = str + " And PIM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                str = str + " And PM.IssueOrderId=" + DDChallanNo.SelectedValue;
                filterby = filterby + " Challan No : " + DDChallanNo.SelectedItem.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                filterby = filterby + " Category : " + DDCategory.SelectedItem.Text;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
                filterby = filterby + " Item : " + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
                filterby = filterby + " Quality : " + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
                filterby = filterby + " Design : " + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
                filterby = filterby + " Color : " + DDColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
                filterby = filterby + " ShapeId : " + DDShape.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
                filterby = filterby + " SizeId : " + DDSize.SelectedItem.Text;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                filterby = filterby + " ShadecolorId : " + DDShadeColor.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
                filterby = filterby + " Localorder : " + txtlocalorderno.Text;
            }
            if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
            {
                str = str + " And OM.customerid=" + DDcustcode.SelectedValue;
                filterby = filterby + " Cust code : " + DDcustcode.SelectedItem.Text;
            }
            if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
            {
                str = str + " And OM.orderid=" + DDorderno.SelectedValue;
                filterby = filterby + " Order No. : " + DDorderno.SelectedItem.Text;

            }
            SqlParameter[] arr = new SqlParameter[9];
            arr[0] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            arr[1] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            arr[2] = new SqlParameter("@fromdate", TxtFromDate.Text);
            arr[3] = new SqlParameter("@Todate", TxtToDate.Text);
            arr[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
            arr[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
            arr[6] = new SqlParameter("@Where", str);
            arr[7] = new SqlParameter("@userid", Session["varuserid"]);
            arr[8] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FinishingIssueReceiveDetailExcelReport", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:I1").Merge();
                sht.Range("A1").Value = "Process Issue Summary Details " + "For - " + DDCompany.SelectedItem.Text;
                sht.Range("A2:I2").Merge();
                sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:I2").Style.Font.Bold = true;
                //***********Filter By Item_Name

                sht.Range("A3").Value = "Emp Name";
                sht.Range("B3").Value = "Quality";
                sht.Range("C3").Value = "Design";
                sht.Range("D3").Value = "Color";
                sht.Range("E3").Value = "Shape";

                sht.Range("F3").Value = "Size";
                sht.Range("G3").Value = "Issue Qty";
                //sht.Range("A" + row).Style.Font.SetBold();
                sht.Range("A3:G3").Style.Font.SetBold();

                row = 3;
                Decimal tqty = 0;
                string empname = "";
                string itemfinsihedid = "0";
                int rowfrom = 0, rowto = 0;



                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "NoofEmp");

                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "NoofEmp='" + dr["NoofEmp"] + "'";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());

                    DataTable dtdistinct1 = ds1.Tables[0].DefaultView.ToTable(true, "EmpName");

                    foreach (DataRow dr2 in dtdistinct1.Rows)
                    {
                        DataView dv1 = new DataView(ds.Tables[0]);
                        dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                        DataSet ds2 = new DataSet();
                        ds2.Tables.Add(dv1.ToTable());

                        //row = row + 1;

                        //sht.Range("A" + row).Value = "Emp Name";
                        //sht.Range("B" + row).Value = "Quality";
                        //sht.Range("C" + row).Value = "Design";
                        //sht.Range("D" + row).Value = "Color";
                        //sht.Range("E" + row).Value = "Shape";

                        //sht.Range("F" + row).Value = "Size";
                        //sht.Range("G" + row).Value = "Issue Qty";

                        ////sht.Range("A" + row).Style.Font.SetBold();
                        //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                        DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "Item_Finished_Id");

                        rowfrom = row + 1;
                        foreach (DataRow dritem in dtdistinctItem.Rows)
                        {
                            row = row + 1;
                            sht.Range("A" + row).SetValue(dritem["empname"]);
                            sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                            sht.Range("C" + row).SetValue(dritem["DesignName"]);
                            sht.Range("D" + row).SetValue(dritem["ColorName"]);
                            sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                            sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                            var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "'");
                            tqty = tqty + Convert.ToDecimal(qty);
                            sht.Range("G" + row).SetValue(qty);

                            rowto = row;
                        }
                        row = row + 1;
                        sht.Range("F" + row).Value = "Total";
                        sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
                        sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
                        row = row + 1;

                    }
                }

                //*************GRAND TOTAL
                sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("G" + row).SetValue(tqty);
                sht.Range("F" + row + ":G" + row).Style.Font.SetBold();


                using (var a = sht.Range("A3:G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 10).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessIssueSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        }
    }

    private void ProcessReceiveDetailSummary()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
            where = where + " Cust Code:" + DDcustcode.SelectedItem.Text + ",";
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
            where = where + " Order No:" + DDorderno.SelectedItem.Text + ",";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);

        if (ChkBuyerItemSizeWiseSummary.Checked == true)
        {
            param[7] = new SqlParameter("@ReportType", 1);
        }
        else if (chkjobwisesummary.Checked == true)
        {
            param[7] = new SqlParameter("@ReportType", 2);
        }
        else
        {
            param[7] = new SqlParameter("@ReportType", 0);
        }

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FinishinghissabchallanWise", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:I1").Merge();
                sht.Range("A1").Value = "Process Received Details Summary " + "For - " + DDCompany.SelectedItem.Text;
                sht.Range("A2:I2").Merge();
                sht.Range("A2").Value = "Filter By :  " + where;
                sht.Row(2).Height = 30;
                sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:I2").Style.Font.Bold = true;
                //***********Filter By Item_Name

                sht.Range("A3").Value = "Emp Name";
                sht.Range("B3").Value = "Quality";
                sht.Range("C3").Value = "Design";
                sht.Range("D3").Value = "Color";
                sht.Range("E3").Value = "Shape";

                sht.Range("F3").Value = "Size";
                sht.Range("G3").Value = "Rec Qty";
                //sht.Range("A" + row).Style.Font.SetBold();
                sht.Range("A3:G3").Style.Font.SetBold();

                row = 3;
                Decimal tqty = 0;
                int rowfrom = 0, rowto = 0;

                DataTable dtdistinct1 = ds.Tables[0].DefaultView.ToTable(true, "EmpName");

                foreach (DataRow dr2 in dtdistinct1.Rows)
                {
                    DataView dv1 = new DataView(ds.Tables[0]);
                    dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                    DataSet ds2 = new DataSet();
                    ds2.Tables.Add(dv1.ToTable());

                    //row = row + 1;

                    //sht.Range("A" + row).Value = "Emp Name";
                    //sht.Range("B" + row).Value = "Quality";
                    //sht.Range("C" + row).Value = "Design";
                    //sht.Range("D" + row).Value = "Color";
                    //sht.Range("E" + row).Value = "Shape";

                    //sht.Range("F" + row).Value = "Size";
                    //sht.Range("G" + row).Value = "Rec Qty";                      
                    //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                    DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "item_finished_id");

                    rowfrom = row + 1;
                    foreach (DataRow dritem in dtdistinctItem.Rows)
                    {
                        row = row + 1;
                        sht.Range("A" + row).SetValue(dritem["empname"]);
                        sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                        sht.Range("C" + row).SetValue(dritem["DesignName"]);
                        sht.Range("D" + row).SetValue(dritem["ColorName"]);
                        sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                        sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                        var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "'");
                        tqty = tqty + Convert.ToDecimal(qty);
                        sht.Range("G" + row).SetValue(qty);

                        rowto = row;
                    }
                    row = row + 1;
                    sht.Range("F" + row).Value = "Total";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
                    sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
                    row = row + 1;

                    //using (var a = sht.Range("A" + (rowfrom-1) + ":G" + (rowto+1)))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}
                }

                //*************GRAND TOTAL
                sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("G" + row).SetValue(tqty);
                sht.Range("F" + row + ":G" + row).Style.Font.SetBold();

                using (var a = sht.Range("A3:G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                }
                //*************
                sht.Columns(1, 10).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessReceiveDetailSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else if (ChkBuyerItemSizeWiseSummary.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:K1").Merge();
                sht.Range("A1").Value = "Process Received Customer Item Size Wise Summary " + "For - " + DDCompany.SelectedItem.Text;
                sht.Range("A2:K2").Merge();
                sht.Range("A2").Value = "Filter By :  " + where;
                sht.Row(2).Height = 30;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:K2").Style.Font.Bold = true;
                //***********Filter By Item_Name

                sht.Range("A3").Value = "Customer Code";
                sht.Range("B3").Value = "PO No";
                sht.Range("C3").Value = "Item Name";
                sht.Range("D3").Value = "Quality";
                sht.Range("E3").Value = "Design";
                sht.Range("F3").Value = "Color";
                sht.Range("G3").Value = "Size";
                sht.Range("H3").Value = "Qty";
                sht.Range("I3").Value = "Rate";
                sht.Range("J3").Value = "Amount";
                sht.Range("K3").Value = "NoofEmp";
                sht.Range("A3:K3").Style.Font.SetBold();

                row = 3;

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    row = row + 1;
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["NoofEmp"]);
                }
                row = row + 1;
                sht.Range("G" + row).Value = "Total";
                sht.Range("H" + row).FormulaA1 = "=SUM(H4:$H$" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J4:$J$" + (row - 1) + ")";
                sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
                row = row + 1;

                using (var a = sht.Range("A3:K" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 10).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessReceiveItemSizeWiseSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void JobWiseProcessReceiveSummary()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }

        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chkjobwisesummary.Checked == true && ChkForDate.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                //******************

                sht.Range("A1").Value = where;
                sht.Range("A1:J1").Style.Alignment.SetWrapText();
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:J1").Merge();
                sht.Row(1).Height = 44;

                sht.Range("A2").Value = "FolioNo";
                sht.Range("B2").Value = "ItemName";
                sht.Range("C2").Value = "QualityName";
                sht.Range("D2").Value = "DesignName";
                sht.Range("E2").Value = "ColorName";
                sht.Range("F2").Value = "Size";
                sht.Range("G2").Value = "Qty";
                sht.Range("H2").Value = "Rate";
                sht.Range("I2").Value = "Area";
                sht.Range("J2").Value = "Amount";
                sht.Range("B2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A2:J2").Style.Font.Bold = true;

                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "item_finished_Id", "Rate");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "item_finished_Id,Rate";
                DataTable dtdistinct1 = dv1.ToTable();
                row = 3;
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and Rate='" + dr["Rate"] + "'";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "FolioNo", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate");
                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                    {

                        sht.Range("A" + row).SetValue(dritemdesc["FolioNo"]);
                        sht.Range("B" + row).SetValue(dritemdesc["Item_Name"]);
                        sht.Range("C" + row).SetValue(dritemdesc["QualityName"]);
                        sht.Range("D" + row).SetValue(dritemdesc["DesignName"]);
                        sht.Range("E" + row).SetValue(dritemdesc["ColorName"]);
                        sht.Range("F" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
                        var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "'");
                        var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "'");
                        var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "'");
                        sht.Range("G" + row).SetValue(qty == DBNull.Value ? 0 : qty);
                        sht.Range("H" + row).SetValue(dritemdesc["Rate"]);
                        sht.Range("I" + row).SetValue(Area == DBNull.Value ? 0 : Area);
                        sht.Range("J" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);
                        row = row + 1;

                    }

                    ////********Grand TOtal
                    //sht.Range("E" + row).Value = "GRAND TOTAL";
                    //sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
                    //sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
                    //sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                    //sht.Range("E" + row + ":J" + row).Style.Font.Bold = true;
                    //sht.Columns(1, 10).AdjustToContents();
                    ////sht.Rows().AdjustToContents();

                }
                //********Grand TOtal
                sht.Range("E" + row).Value = "GRAND TOTAL";
                sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                sht.Range("E" + row + ":J" + row).Style.Font.Bold = true;
                sht.Columns(1, 10).AdjustToContents();
                //sht.Rows().AdjustToContents();

                using (var a = sht.Range("A2" + ":J" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }


                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Jobwisereceivesummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
                return;
            }
            //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    public void JobWiseProcessReceiveSummaryFolioWise(SqlTransaction Tran2)
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }

        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        ////End Conditions

        //SqlParameter[] param = new SqlParameter[20];
        //param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        //param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        //param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        //param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        //param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        //param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        //param[6] = new SqlParameter("@Where", strCondition);

        //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary", param);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("JobWiseProcessReceiveSummaryAntique", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        cmd.Parameters.AddWithValue("@Where", strCondition);

        //DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();


        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true)
            {
                JobWiseProcessReceiveSummaryFolioNoWise(ds, where);
                return;
            }
            else
            {
                JobWiseProcessReceiveSummaryFolioAndRecChallanNoWise(ds, where);
                return;
            }
        }

    }
    private void JobWiseProcessReceiveSummaryFolioAndRecChallanNoWise(DataSet ds, string where)
    {

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chkjobwisesummary.Checked == true && ChkForDate.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                //******************

                sht.Range("A1").Value = where;
                sht.Range("A1:M1").Style.Alignment.SetWrapText();
                sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:M1").Merge();
                sht.Row(1).Height = 44;

                //sht.Range("A2").Value = "FolioNo";
                //sht.Range("B2").Value = "ItemName";
                //sht.Range("C2").Value = "QualityName";
                //sht.Range("D2").Value = "DesignName";
                //sht.Range("E2").Value = "ColorName";
                //sht.Range("F2").Value = "Size";

                //sht.Range("G2").Value = "Size Mtr";
                //sht.Range("H2").Value = "Qty";
                //sht.Range("I2").Value = "Rate";
                //sht.Range("J2").Value = "Area";
                //sht.Range("K2").Value = "Amount";
                //sht.Range("L2").Value = "ChallanNo";
                //sht.Range("M2").Value = "POrderNo";

                sht.Range("A2").Value = "POrderNo";
                sht.Range("B2").Value = "ChallanNo";
                sht.Range("C2").Value = "FolioNo";
                sht.Range("D2").Value = "ItemName";
                sht.Range("E2").Value = "QualityName";
                sht.Range("F2").Value = "DesignName";
                sht.Range("G2").Value = "ColorName";
                sht.Range("H2").Value = "Size";

                sht.Range("I2").Value = "Size Mtr";
                sht.Range("J2").Value = "Qty";
                sht.Range("K2").Value = "Rate";
                sht.Range("L2").Value = "Area";
                sht.Range("M2").Value = "Amount";


                sht.Range("B2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A2:M2").Style.Font.Bold = true;

                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "item_finished_Id", "Rate");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "item_finished_Id,Rate";
                DataTable dtdistinct1 = dv1.ToTable();
                row = 3;
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and Rate='" + dr["Rate"] + "'";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    if (Session["varCompanyNo"].ToString() == "27")
                    {
                        DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "IssueOrderId", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate", "ChallanNo", "POrderNo", "sizemtr");
                        foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                        {

                            sht.Range("A" + row).SetValue(dritemdesc["POrderNo"]);
                            sht.Range("B" + row).SetValue(dritemdesc["ChallanNo"]);

                            sht.Range("C" + row).SetValue(dritemdesc["IssueOrderId"]);
                            sht.Range("D" + row).SetValue(dritemdesc["Item_Name"]);
                            sht.Range("E" + row).SetValue(dritemdesc["QualityName"]);
                            sht.Range("F" + row).SetValue(dritemdesc["DesignName"]);
                            sht.Range("G" + row).SetValue(dritemdesc["ColorName"]);
                            sht.Range("H" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
                            sht.Range("I" + row).SetValue(dritemdesc["sizemtr"]);
                            var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");
                            var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");
                            var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");

                            sht.Range("J" + row).SetValue(qty == DBNull.Value ? 0 : qty);
                            sht.Range("K" + row).SetValue(dritemdesc["Rate"]);
                            sht.Range("L" + row).SetValue(Area == DBNull.Value ? 0 : Area);
                            sht.Range("M" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);


                            //sht.Range("A" + row).SetValue(dritemdesc["IssueOrderId"]);
                            //sht.Range("B" + row).SetValue(dritemdesc["Item_Name"]);
                            //sht.Range("C" + row).SetValue(dritemdesc["QualityName"]);
                            //sht.Range("D" + row).SetValue(dritemdesc["DesignName"]);
                            //sht.Range("E" + row).SetValue(dritemdesc["ColorName"]);
                            //sht.Range("F" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
                            //sht.Range("G" + row).SetValue(dritemdesc["sizemtr"]);
                            //var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");
                            //var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");
                            //var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "'");

                            //sht.Range("H" + row).SetValue(qty == DBNull.Value ? 0 : qty);
                            //sht.Range("I" + row).SetValue(dritemdesc["Rate"]);
                            //sht.Range("J" + row).SetValue(Area == DBNull.Value ? 0 : Area);
                            //sht.Range("K" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);
                            //sht.Range("L" + row).SetValue(dritemdesc["ChallanNo"]);
                            //sht.Range("M" + row).SetValue(dritemdesc["POrderNo"]);

                            row = row + 1;

                        }
                    }

                    ////********Grand TOtal
                    //sht.Range("E" + row).Value = "GRAND TOTAL";
                    //sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
                    //sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
                    //sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                    //sht.Range("E" + row + ":J" + row).Style.Font.Bold = true;
                    //sht.Columns(1, 10).AdjustToContents();
                    ////sht.Rows().AdjustToContents();

                }
                //********Grand Total
                sht.Range("H" + row).Value = "GRAND TOTAL";
                sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                sht.Range("L" + row).FormulaA1 = "=SUM(L3:$L$" + (row - 1) + ")";
                sht.Range("M" + row).FormulaA1 = "=SUM(M3:$M$" + (row - 1) + ")";
                sht.Range("E" + row + ":M" + row).Style.Font.Bold = true;
                sht.Columns(1, 16).AdjustToContents();
                //sht.Rows().AdjustToContents();

                using (var a = sht.Range("A2" + ":M" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                if (Session["varCompanyNo"].ToString() != "27")
                {
                    sht.Column(11).Delete();
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("JobWiseRecSummaryFolioAndRecChallanWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
                return;
            }
            //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void JobWiseProcessReceiveSummaryFolioNoWise(DataSet ds, string where)
    {

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true && ChkForDate.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                //******************

                sht.Range("A1").Value = where;
                sht.Range("A1:K1").Style.Alignment.SetWrapText();
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Merge();
                sht.Row(1).Height = 44;

                //sht.Range("A2").Value = "POrderNo";
                //sht.Range("B2").Value = "ChallanNo";
                //sht.Range("C2").Value = "FolioNo";
                //sht.Range("D2").Value = "ItemName";
                //sht.Range("E2").Value = "QualityName";
                //sht.Range("F2").Value = "DesignName";
                //sht.Range("G2").Value = "ColorName";
                //sht.Range("H2").Value = "Size";
                //sht.Range("I2").Value = "Size Mtr";
                //sht.Range("J2").Value = "Qty";
                //sht.Range("K2").Value = "Rate";
                //sht.Range("L2").Value = "Area";
                //sht.Range("M2").Value = "Amount";

                sht.Range("A2").Value = "POrderNo";
                sht.Range("B2").Value = "ItemName";
                sht.Range("C2").Value = "QualityName";
                sht.Range("D2").Value = "DesignName";
                sht.Range("E2").Value = "ColorName";
                sht.Range("F2").Value = "Size";

                sht.Range("G2").Value = "Size Mtr";
                sht.Range("H2").Value = "Qty";
                sht.Range("I2").Value = "Rate";
                sht.Range("J2").Value = "Area";
                sht.Range("K2").Value = "Amount";


                sht.Range("B2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A2:K2").Style.Font.Bold = true;

                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "item_finished_Id", "Rate");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "item_finished_Id,Rate";
                DataTable dtdistinct1 = dv1.ToTable();
                row = 3;
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and Rate='" + dr["Rate"] + "'";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    if (Session["varCompanyNo"].ToString() == "27")
                    {
                        DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate", "POrderNo", "sizemtr");
                        foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                        {

                            sht.Range("A" + row).SetValue(dritemdesc["POrderNo"]);
                            sht.Range("B" + row).SetValue(dritemdesc["Item_Name"]);
                            sht.Range("C" + row).SetValue(dritemdesc["QualityName"]);
                            sht.Range("D" + row).SetValue(dritemdesc["DesignName"]);
                            sht.Range("E" + row).SetValue(dritemdesc["ColorName"]);
                            sht.Range("F" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
                            sht.Range("G" + row).SetValue(dritemdesc["sizemtr"]);
                            var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "'  and POrderNo='" + dritemdesc["POrderNo"] + "' and width='" + dritemdesc["width"] + "'  and Length='" + dritemdesc["Length"] + "' and sizemtr='" + dritemdesc["sizemtr"] + "' ");
                            var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "' and width='" + dritemdesc["width"] + "'  and Length='" + dritemdesc["Length"] + "' and sizemtr='" + dritemdesc["sizemtr"] + "'");
                            var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and POrderNo='" + dritemdesc["POrderNo"] + "' and width='" + dritemdesc["width"] + "'  and Length='" + dritemdesc["Length"] + "' and sizemtr='" + dritemdesc["sizemtr"] + "'");

                            sht.Range("H" + row).SetValue(qty == DBNull.Value ? 0 : qty);
                            sht.Range("I" + row).SetValue(dritemdesc["Rate"]);
                            sht.Range("J" + row).SetValue(Area == DBNull.Value ? 0 : Area);
                            sht.Range("K" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);

                            row = row + 1;

                        }
                    }

                    ////********Grand TOtal
                    //sht.Range("E" + row).Value = "GRAND TOTAL";
                    //sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
                    //sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
                    //sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                    //sht.Range("E" + row + ":J" + row).Style.Font.Bold = true;
                    //sht.Columns(1, 10).AdjustToContents();
                    ////sht.Rows().AdjustToContents();

                }
                //********Grand Total
                sht.Range("G" + row).Value = "GRAND TOTAL";
                sht.Range("H" + row).FormulaA1 = "=SUM(H3:$H$" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
                sht.Range("K" + row).FormulaA1 = "=SUM(K3:$K$" + (row - 1) + ")";
                sht.Range("C" + row + ":K" + row).Style.Font.Bold = true;
                sht.Columns(1, 16).AdjustToContents();
                //sht.Rows().AdjustToContents();

                using (var a = sht.Range("A2" + ":K" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                if (Session["varCompanyNo"].ToString() != "27")
                {
                    sht.Column(9).Delete();
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("JobwisereceivesummaryFolioWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
                return;
            }
            //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Om.OrderId,Om.CustomerOrderNo From OrderMaster OM where CompanyId=" + DDCompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + "  and  Om.Status=0 order by CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDorderno, str, true, "--Select--");
    }

    protected void RDWeaverRawMaterialIssueDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDWeaverRawMaterialIssueDetail.Checked == true)
        {
            TREmpName.Visible = true;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForPendingStockNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            trIssueDate.Visible = false;
            TDexcelExport.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TDJobWiseSummary.Visible = false;
            TRCheckWithTime.Visible = false;
            ChkSummary.Visible = false;
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;

            if (Session["VarCompanyNo"].ToString() == "31")
            {
                TRQualityWiseSummary.Visible = true;
            }
            else
            {
                TRQualityWiseSummary.Visible = false;
            }

        }
    }
    protected void WeaverRawMaterialIssueDetailReport()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Issueorderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}

            if (ChkForDate.Checked == true)
            {
                str2 = str2 + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueReceiveDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 0);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "MATERIAL ISSUE";
                sht.Range("A1:K1").Style.Font.FontName = "Calibri";
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Merge();

                //*******Header
                sht.Range("A2").Value = "Job Name";
                sht.Range("B2").Value = "Issue ChallanDate";
                sht.Range("C2").Value = "Issue ChallanNo";
                sht.Range("D2").Value = "Local Orderno";
                sht.Range("E2").Value = "Folio No";
                sht.Range("F2").Value = "Contractor Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Sub Item";
                sht.Range("I2").Value = "Shade Color";
                sht.Range("J2").Value = "Lot No";
                sht.Range("K2").Value = "Tag No";
                sht.Range("L2").Value = "Weight(K.G)";
                sht.Range("M2").Value = "Remark";


                sht.Range("A2:M2").Style.Font.FontName = "Calibri";
                sht.Range("A2:M2").Style.Font.FontSize = 11;
                sht.Range("A2:M2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChalanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ProrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ISSUERECEIVEQTY"]));
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "M")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    protected void WeaverRawMaterialIssueWithConsumptionDetailReport()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Issueorderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }
            if (ChkForDate.Checked == true)
            {
                str = str + " and PIM.AssignDate>='" + TxtFromDate.Text + "' and PIM.AssignDate<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }

            ////if (DDQtype.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            ////}
            ////if (DDQuality.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            ////}
            ////if (DDshade.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            ////}

            //if (ChkForDate.Checked == true)
            //{
            //    str2 = str2 + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
            //    FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            //}



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueWithConsumptionDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 0);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "CONSUMPTION WITH RAW MATERIAL ISSUE";
                sht.Range("A1:K1").Style.Font.FontName = "Calibri";
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Merge();

                //*******Header
                sht.Range("A2").Value = "Process Name";
                sht.Range("B2").Value = "Folio Date";
                sht.Range("C2").Value = "ChallanNo";
                sht.Range("D2").Value = "Local Orderno";
                sht.Range("E2").Value = "Folio No";
                sht.Range("F2").Value = "Contractor Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Quality";
                sht.Range("I2").Value = "Shade Color";
                sht.Range("J2").Value = "Consumption Qty";
                sht.Range("K2").Value = "Issue Qty";
                sht.Range("L2").Value = "Balance Qty";


                sht.Range("A2:L2").Style.Font.FontName = "Calibri";
                sht.Range("A2:L2").Style.Font.FontSize = 11;
                sht.Range("A2:L2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQTY"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ConsumptionQTY"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]));


                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "L")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssueWithConsumptionDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void RDWeaverRawMaterialReceiveDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        TDJobWiseSummary.Visible = false;
        TDsizesummary.Visible = false;
        TRBuyerItemSizeWiseSummary.Visible = false;
        TRCheckWithTime.Visible = false;
        TRQualityWiseSummary.Visible = false;

        if (RDWeaverRawMaterialReceiveDetail.Checked == true)
        {
            TREmpName.Visible = true;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForPendingStockNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            trIssueDate.Visible = false;
            TDexcelExport.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TDJobWiseSummary.Visible = false;
            TRCheckWithTime.Visible = false;
            ChkSummary.Visible = false;
            TDexcelExport.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = true;
            TRorderno.Visible = true;
        }
    }

    protected void WeaverRawMaterialReceiveDetailReport()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Issueorderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}

            if (ChkForDate.Checked == true)
            {
                str2 = str2 + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueReceiveDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 1);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "MATERIAL RECEIVE";
                sht.Range("A1:M1").Style.Font.FontName = "Calibri";
                sht.Range("A1:M1").Style.Font.Bold = true;
                sht.Range("A1:M1").Style.Font.FontSize = 12;
                sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:M1").Merge();

                //*******Header
                sht.Range("A2").Value = "Job Name";
                sht.Range("B2").Value = "Receive ChallanDate";
                sht.Range("C2").Value = "Receive ChallanNo";
                sht.Range("D2").Value = "Local Orderno";
                sht.Range("E2").Value = "Folio No";
                sht.Range("F2").Value = "Contractor Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Sub Item";
                sht.Range("I2").Value = "Shade Color";
                sht.Range("J2").Value = "Lot No";
                sht.Range("K2").Value = "Tag No";
                sht.Range("L2").Value = "Weight(K.G)";
                sht.Range("M2").Value = "Remark";


                sht.Range("A2:M2").Style.Font.FontName = "Calibri";
                sht.Range("A2:M2").Style.Font.FontSize = 11;
                sht.Range("A2:M2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChalanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ProrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ISSUERECEIVEQTY"]));
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "M")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialReceiveDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void TassleRawMaterialReceiveDetailReport()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and OD.orderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}

            if (ChkForDate.Checked == true)
            {
                str2 = str2 + " and OM.OrderDate>='" + TxtFromDate.Text + "' and OM.OrderDate<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetTassleIssueReceiveDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 1);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "TASSAL REPORT";
                sht.Range("A1:S1").Style.Font.FontName = "Calibri";
                sht.Range("A1:S1").Style.Font.Bold = true;
                sht.Range("A1:S1").Style.Font.FontSize = 12;
                sht.Range("A1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:S1").Merge();

                //*******Header
                sht.Range("A2").Value = "BUYER CODE";
                sht.Range("A2:B2").Merge();
                sht.Range("C2").Value = "PO";
                sht.Range("C2:D2").Merge();
                sht.Range("E2").Value = "ORDER DESC.";
                sht.Range("E2:H2").Merge();
                sht.Range("I2").Value = "SIZE";
                sht.Range("J2").Value = "ORDER QTY.";

                sht.Range("K2").Value = "TASSAL ITEM DESC.";
                sht.Range("K2:N2").Merge();
                sht.Range("O2").Value = "REQ. TASSAL QTY.";
                sht.Range("P2").Value = "TASSLE ORDER QTY.";
                sht.Range("Q2").Value = "BAL. ORDER";
                sht.Range("R2").Value = "REC QTY.";
                sht.Range("S2").Value = "PEND. QTY.";
                //sht.Range("L2").Value = "Weight(K.G)";
                //sht.Range("M2").Value = "Remark";


                sht.Range("A2:S2").Style.Font.FontName = "Calibri";
                sht.Range("A2:S2").Style.Font.FontSize = 11;
                sht.Range("A2:S2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row + ":D" + row).Merge();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["OrderDescription"]);
                    sht.Range("E" + row + ":H" + row).Merge();
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Itemsize"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["OrderedQty"]);


                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("K" + row + ":N" + row).Merge();
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["nooftassal"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["OrderedQtyTassal"]);
                    sht.Range("Q" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["nooftassal"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["OrderedQtyTassal"]));
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["RecQtyTassal"]);
                    sht.Range("S" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["OrderedQtyTassal"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["RecQtyTassal"]));
                    //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    //sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ISSUERECEIVEQTY"]));
                    //sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "S")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TassalReceiveDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void TassleRawPartnerIssueReport()
    {
        lblMessage.Text = "";
        try
        {
            //string str = "", FilterBy = "", str2 = "";
            //if (DDChallanNo.SelectedIndex > 0)
            //{
            //    str = str + " and OD.orderid=" + DDChallanNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            //}
            //if (DDcustcode.SelectedIndex > 0)
            //{
            //    str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
            //    FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            //}
            //if (DDorderno.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDorderno.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            //}
            //if (txtlocalorderno.Text != "")
            //{
            //    str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            //}

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}

            //if (ChkForDate.Checked == true)
            //{
            //    str2 = str2 + " and OM.OrderDate>='" + TxtFromDate.Text + "' and OM.OrderDate<='" + TxtToDate.Text + "'";
            //    FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            //}



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetTassleIssueReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@CustomerID", DDcustcode.SelectedIndex > 0 ? DDcustcode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", DDorderno.SelectedIndex > 0 ? DDorderno.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@DateType", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@ReportType", 1);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "TASSAL PARTNER ISSUE REPORT";
                sht.Range("A1:Q1").Style.Font.FontName = "Calibri";
                sht.Range("A1:Q1").Style.Font.Bold = true;
                sht.Range("A1:Q1").Style.Font.FontSize = 12;
                sht.Range("A1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Q1").Merge();

                //*******Header
                sht.Range("A2").Value = "BUYER CODE";
                sht.Range("A2:B2").Merge();
                sht.Range("C2").Value = "PO";
                sht.Range("C2:D2").Merge();
                sht.Range("E2").Value = "VENDOR NAME";
                sht.Range("E2:H2").Merge();
                sht.Range("I2").Value = "ISSUE CHALLAN NO";
                sht.Range("J2").Value = "ISSUE DATE";

                sht.Range("K2").Value = "TASSAL ITEM DESC.";
                sht.Range("K2:N2").Merge();
                sht.Range("O2").Value = "TASSAL ISSUE QTY.";
                //sht.Range("P2").Value = "TASSLE ORDER QTY.";
                //sht.Range("Q2").Value = "BAL. ORDER";
                sht.Range("P2").Value = "REC QTY.";
                sht.Range("Q2").Value = "PEND. QTY.";
                //sht.Range("L2").Value = "Weight(K.G)";
                //sht.Range("M2").Value = "Remark";


                sht.Range("A2:Q2").Style.Font.FontName = "Calibri";
                sht.Range("A2:Q2").Style.Font.FontSize = 11;
                sht.Range("A2:Q2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row + ":D" + row).Merge();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("E" + row + ":H" + row).Merge();
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["issueorderid"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["issuedate"]);


                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("K" + row + ":N" + row).Merge();
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["IssQty"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                    sht.Range("Q" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["IssQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["RecQty"]));
                    //  sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["RecQtyTassal"]);
                    // sht.Range("S" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["OrderedQtyTassal"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["RecQtyTassal"]));
                    //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    //sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ISSUERECEIVEQTY"]));
                    //sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "Q")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TassalPartnerIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void TassleRawPartnerReceiveReport()
    {
        lblMessage.Text = "";
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetTassleIssueReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@CustomerID", DDcustcode.SelectedIndex > 0 ? DDcustcode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", DDorderno.SelectedIndex > 0 ? DDorderno.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@DateType", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@ReportType", 2);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "TASSAL PARTNER RECEIVE REPORT";
                sht.Range("A1:T1").Style.Font.FontName = "Calibri";
                sht.Range("A1:T1").Style.Font.Bold = true;
                sht.Range("A1:T1").Style.Font.FontSize = 12;
                sht.Range("A1:T1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:T1").Merge();

                //*******Header
                sht.Range("A2").Value = "BUYER CODE";
                sht.Range("A2:B2").Merge();
                sht.Range("C2").Value = "PO";
                sht.Range("C2:D2").Merge();
                sht.Range("E2").Value = "VENDOR NAME";
                sht.Range("E2:H2").Merge();
                sht.Range("I2").Value = "REC CHALLAN NO.";
                sht.Range("J2").Value = "REC DATE";

                sht.Range("K2").Value = "TASSAL ITEM DESC.";
                sht.Range("K2:N2").Merge();
                sht.Range("O2").Value = "REC QTY.";
                sht.Range("P2").Value = "WEIGHT REC.";
                sht.Range("Q2").Value = "LOSS";
                sht.Range("R2").Value = "PARTY CHALLAN";
                sht.Range("S2").Value = "REMARK";
                sht.Range("T2").Value = "RATE";

                sht.Range("A2:T2").Style.Font.FontName = "Calibri";
                sht.Range("A2:T2").Style.Font.FontSize = 11;
                sht.Range("A2:T2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row + ":D" + row).Merge();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("E" + row + ":H" + row).Merge();
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["challanno"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("K" + row + ":N" + row).Merge();
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);

                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["WEIGHT"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["LOSS"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["PARTYCHALLANNO"]);
                    sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["REMARKS"]);
                    sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["RATE"]);

                    row = row + 1;
                }
                row = row - 1;
                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "T")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TassalPartnerReceiveDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "status", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    protected void WeaverRawMaterialIssueWithConsumptionSummaryReport()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Issueorderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }
            if (ChkForDate.Checked == true)
            {
                str = str + " and PIM.AssignDate>='" + TxtFromDate.Text + "' and PIM.AssignDate<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }

            ////if (DDQtype.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            ////}
            ////if (DDQuality.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            ////}
            ////if (DDshade.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            ////}

            //if (ChkForDate.Checked == true)
            //{
            //    str2 = str2 + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
            //    FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            //}



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueWithConsumptionSummaryReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 0);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "CONSUMPTION WITH RAW MATERIAL ISSUE";
                sht.Range("A1:K1").Style.Font.FontName = "Calibri";
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Merge();

                //*******Header
                sht.Range("A2").Value = "Process Name";
                sht.Range("B2").Value = "Folio Date";
                sht.Range("C2").Value = "ChallanNo";
                sht.Range("D2").Value = "Local Orderno";
                sht.Range("E2").Value = "Folio No";
                sht.Range("F2").Value = "Contractor Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Quality";
                sht.Range("I2").Value = "Consumption Qty";
                sht.Range("J2").Value = "Issue Qty";
                sht.Range("K2").Value = "Balance Qty";


                sht.Range("A2:L2").Style.Font.FontName = "Calibri";
                sht.Range("A2:L2").Style.Font.FontSize = 11;
                sht.Range("A2:L2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":K" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":K" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    /////sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQTY"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("K" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ConsumptionQTY"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]));

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "L")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssueWithConsumptionSummaryreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    private void GateInPassDetail_Excel_Hafizia(DataSet ds)
    {
        string Path = "";
        string label = "", labelH = "";
        switch (DDgateinpasstype.SelectedValue)
        {
            case "0":
                label = "GATE OUT DETAIL";
                labelH = "Gate Out No.";
                break;
            case "1":
                label = "GATE IN DETAIL";
                labelH = "Gate In No.";
                break;
            default:
                label = "GATE IN / OUT DETAIL";
                labelH = "Gate In / Out No.";
                break;
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("sheet1");
        //*************
        sht.Range("A1:M1").Merge();
        sht.Range("A1:M1").Style.Font.FontSize = 11;
        sht.Range("A1:M1").Style.Font.Bold = true;
        sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        label = label + "-" + (ChkForDate.Checked == true ? ("From Date " + TxtFromDate.Text + "  ToDate " + TxtToDate.Text + "") : "");
        sht.Range("A1").SetValue(label);
        sht.Row(1).Height = 21.75;

        sht.Range("A2").SetValue("Date");
        sht.Range("B2").SetValue("Employee Name");
        sht.Range("C2").SetValue(labelH);
        sht.Range("D2").SetValue("Bill No.");
        sht.Range("E2").SetValue("Lot No.");

        sht.Range("F2").SetValue("Tag No.");

        sht.Range("G2").SetValue("Item Name");
        sht.Range("H2").SetValue("Quality");
        sht.Range("I2").SetValue("Description");
        sht.Range("J2").SetValue("Quantity");
        sht.Range("K2").SetValue("Rate");
        sht.Range("L2").SetValue("Amount");
        sht.Range("M2").SetValue("Tran Type");
        sht.Range("N2").SetValue("Bill Date");
        sht.Range("O2").SetValue("Remark");

        sht.Range("A2:O2").Style.Font.Bold = true;

        int row = 3;
        int rowfrom = 0;
        int rowto = 0;

        DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Date", "Issuedto", "gateinoutrecno");

        DataView dv1 = new DataView(dtdistinct);
        dv1.Sort = "Date,Issuedto,gateinoutrecno";

        DataTable dt = dv1.ToTable();

        foreach (DataRow dr in dt.Rows)
        {
            DataView dvitemdesc = new DataView(ds.Tables[0]);
            dvitemdesc.RowFilter = "Date='" + dr["Date"] + "' and Issuedto='" + dr["issuedto"] + "' and Gateinoutrecno='" + dr["Gateinoutrecno"] + "'";
            dvitemdesc.Sort = "Issuedto";
            DataSet dsitemdesc = new DataSet();
            dsitemdesc.Tables.Add(dvitemdesc.ToTable());

            rowfrom = row;

            for (int i = 0; i < dsitemdesc.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(dr["Date"]);
                sht.Range("B" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["issuedto"]);
                sht.Range("C" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["gateinoutNo"]);
                sht.Range("D" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["gateinoutrecno"]);
                sht.Range("E" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["TagNo"]);


                sht.Range("G" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["item_name"]);
                sht.Range("H" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["qualityname"]);
                sht.Range("I" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Description"]);
                sht.Range("J" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["qty"]);
                sht.Range("K" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["Rate"]);
                sht.Range("L" + row).FormulaA1 = "=J" + row + '*' + "$K$" + row;                //=I3*J3
                sht.Range("M" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["TranType"].ToString() == "1" ? "IN" : "OUT");
                sht.Range("N" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["billdate"]);
                sht.Range("O" + row).SetValue(dsitemdesc.Tables[0].Rows[i]["ReasonToIssue"]);
                row = row + 1;
            }

            //rowto = row;
            //sht.Range("H" + row).SetValue("TOTAL");
            //sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (rowto - 1) + ")";
            //sht.Range("K" + row).FormulaA1 = "=SUM(K" + rowfrom + ":K" + (rowto - 1) + ")";
            //sht.Range("A" + row + ":N" + row).Style.Font.Bold = true;
            //row = row + 1;

        }
        using (var a = sht.Range("A2" + ":O" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }
        //**********
        sht.Columns(1, 26).AdjustToContents();
        //**********Save File
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename(label + "-" + DateTime.Now + "." + Fileextension);
        Path = Server.MapPath("~/Tempexcel/" + filename);
        xapp.SaveAs(Path);
        xapp.Dispose();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.WriteFile(Path);
        Response.End();
        return;
    }
    private void FinishingIssueReceiveSummaryagni(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        string strCondition = "And IM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And  IM.ASSIgndate>='" + TxtFromDate.Text + "' And  IM.ASSIgndate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And ISD.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And ISD.Issueorderid=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId =" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        string sp = string.Empty;
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);
        if (chksumm.Checked)
        {
            sp = "PRO_FINISHINGISSUERECEIVESUMMARYAGNI";

        }
        else if (chkall.Checked)
        {
            sp = "PRO_FINISHINGISSUERECEIVESUMMARYALLAGNI";
        }
        else { sp = "PRO_FINISHINGISSUERECEIVESUMMARYREPORTAGNI"; }
        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, sp, param);
        //ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, sp, param);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (Session["VarCompanyNo"].ToString() == "22")
            {
                ProcessIssueReceiveSummaryReportDiamondExport(ds);
            }
            if (chksumm.Checked)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummaryonlyagni.rpt";
            }
            else if (chkall.Checked)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummaryallagni.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummaryagni.rpt";
            }
            // if (Session["VarCompanyNo"].ToString() == "44")
            //{
            //   Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummaryagni.rpt";
            // }
            //else
            // {
            //Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummary.rpt"; 

            // }

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingIssueReceiveSummary.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void FinishingIssueReceiveSummary(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Issueorderid=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);

        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_FINISHINGISSUERECEIVESUMMARYREPORT", param);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (Session["VarCompanyNo"].ToString() == "22")
            {
                ProcessIssueReceiveSummaryReportDiamondExport(ds);
            }

            Session["rptFileName"] = "~\\Reports\\RptFinishingIssueReceiveSummary.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingIssueReceiveSummary.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void ProcessReceiveDetailSummaryDiamondExport()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
            where = where + " Cust Code:" + DDcustcode.SelectedItem.Text + ",";
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
            where = where + " Order No:" + DDorderno.SelectedItem.Text + ",";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);
        param[7] = new SqlParameter("@ReportType", ChkBuyerItemSizeWiseSummary.Checked == true ? 1 : 0);  ////ChkBuyerItemSizeWiseSummary.checked == true then 1 else 0 

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FinishinghissabchallanWise", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:I1").Merge();
                sht.Range("A1").Value = "Process Received Details Summary " + "For - " + DDCompany.SelectedItem.Text;
                sht.Range("A2:I2").Merge();
                sht.Range("A2").Value = "Filter By :  " + where;
                sht.Row(2).Height = 30;
                sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:I2").Style.Font.Bold = true;
                //***********Filter By Item_Name

                sht.Range("A3").Value = "Emp Name";
                sht.Range("B3").Value = "Quality";
                sht.Range("C3").Value = "Design";
                sht.Range("D3").Value = "Color";
                sht.Range("E3").Value = "Shape";

                sht.Range("F3").Value = "Size";
                sht.Range("G3").Value = "Rec Qty";
                sht.Range("H3").Value = "Failed Pcs";
                sht.Range("I3").Value = "FAILED %";
                sht.Range("J3").Value = "KPI %";
                //sht.Range("A" + row).Style.Font.SetBold();
                sht.Range("A3:J3").Style.Font.SetBold();

                row = 3;
                Decimal tqty = 0;
                Decimal tfailqty = 0;
                Decimal TotalFailQtyPER = 0;
                Decimal TotalKPIPER = 0;
                int rowfrom = 0, rowto = 0;

                DataTable dtdistinct1 = ds.Tables[0].DefaultView.ToTable(true, "EmpName");

                foreach (DataRow dr2 in dtdistinct1.Rows)
                {
                    DataView dv1 = new DataView(ds.Tables[0]);
                    dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                    DataSet ds2 = new DataSet();
                    ds2.Tables.Add(dv1.ToTable());

                    //row = row + 1;

                    //sht.Range("A" + row).Value = "Emp Name";
                    //sht.Range("B" + row).Value = "Quality";
                    //sht.Range("C" + row).Value = "Design";
                    //sht.Range("D" + row).Value = "Color";
                    //sht.Range("E" + row).Value = "Shape";

                    //sht.Range("F" + row).Value = "Size";
                    //sht.Range("G" + row).Value = "Rec Qty";                      
                    //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                    DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "item_finished_id");

                    rowfrom = row + 1;
                    foreach (DataRow dritem in dtdistinctItem.Rows)
                    {
                        row = row + 1;
                        sht.Range("A" + row).SetValue(dritem["empname"]);
                        sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                        sht.Range("C" + row).SetValue(dritem["DesignName"]);
                        sht.Range("D" + row).SetValue(dritem["ColorName"]);
                        sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                        sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                        var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' ");
                        tqty = tqty + Convert.ToDecimal(qty);
                        sht.Range("G" + row).SetValue(qty);


                        int FailQty = 0;
                        if (!DBNull.Value.Equals(ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' and QualityType=2")))
                        {
                            FailQty = Convert.ToInt32(ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' and QualityType=2"));
                        }

                        tfailqty = tfailqty + Convert.ToDecimal(FailQty);
                        sht.Range("H" + row).SetValue(FailQty);
                        sht.Range("I" + row).SetValue(Math.Round((Convert.ToDecimal(FailQty) / Convert.ToDecimal(qty)) * 100, 2));
                        sht.Range("J" + row).SetValue(Math.Round((Convert.ToDecimal(100) - Convert.ToDecimal((Convert.ToDecimal(FailQty) / Convert.ToDecimal(qty)) * 100)), 2));

                        rowto = row;
                    }
                    row = row + 1;
                    sht.Range("F" + row).Value = "Total";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
                    sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":$H$" + rowto + ")";
                    sht.Range("I" + row).FormulaA1 = "=SUM(H" + rowfrom + ":$H$" + rowto + ")/SUM(G" + rowfrom + ":$G$" + rowto + ")*100";
                    sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("J" + row).FormulaA1 = "=100-(SUM(H" + rowfrom + ":$H$" + rowto + ")/SUM(G" + rowfrom + ":$G$" + rowto + ")*100)";
                    sht.Range("J" + row).Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("F" + row + ":J" + row).Style.Font.SetBold();


                    //sht.Range("J" + row).FormulaA1 = ("=Round(SUM(J6" + ':' + "J" + (row - 1) + "),2)");
                    //sht.Range("J" + row).Style.NumberFormat.Format = "#,##0.00";

                    row = row + 1;

                    //using (var a = sht.Range("A" + (rowfrom-1) + ":G" + (rowto+1)))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}
                }

                //*************GRAND TOTAL
                sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("G" + row).SetValue(tqty);
                sht.Range("H" + row).SetValue(tfailqty);
                TotalFailQtyPER = Math.Round((Convert.ToDecimal(tfailqty) / Convert.ToDecimal(tqty)) * 100, 2);
                sht.Range("I" + row).SetValue(TotalFailQtyPER);
                TotalKPIPER = Math.Round((Convert.ToDecimal(100) - Convert.ToDecimal((Convert.ToDecimal(tfailqty) / Convert.ToDecimal(tqty)) * 100)), 2);
                sht.Range("J" + row).SetValue(TotalKPIPER);
                sht.Range("F" + row + ":G" + row).Style.Font.SetBold();

                using (var a = sht.Range("A3:J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                }
                //*************
                sht.Columns(1, 10).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessReceiveDetailSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            //else if (ChkBuyerItemSizeWiseSummary.Checked == true)
            //{
            //    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            //    {
            //        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            //    }
            //    string Path = "";
            //    var xapp = new XLWorkbook();
            //    var sht = xapp.Worksheets.Add("sheet1");
            //    int row = 0;
            //    //***********
            //    sht.Range("A1:G1").Merge();
            //    sht.Range("A1").Value = "Process Received Customer Item Size Wise Summary " + "For - " + DDCompany.SelectedItem.Text;
            //    sht.Range("A2:G2").Merge();
            //    sht.Range("A2").Value = "Filter By :  " + where;
            //    sht.Row(2).Height = 30;
            //    sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            //    sht.Range("A1:G2").Style.Font.Bold = true;
            //    //***********Filter By Item_Name

            //    sht.Range("A3").Value = "Customer Code";
            //    sht.Range("B3").Value = "PO No";
            //    sht.Range("C3").Value = "Item Name";
            //    sht.Range("D3").Value = "Size";
            //    sht.Range("E3").Value = "Qty";
            //    sht.Range("F3").Value = "Rate";
            //    sht.Range("G3").Value = "Amount";
            //    sht.Range("A3:G3").Style.Font.SetBold();

            //    row = 3;

            //    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        row = row + 1;
            //        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
            //        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
            //        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
            //        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
            //        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
            //        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
            //        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
            //    }
            //    row = row + 1;
            //    sht.Range("D" + row).Value = "Total";
            //    sht.Range("E" + row).FormulaA1 = "=SUM(E4:$E$" + (row - 1) + ")";
            //    sht.Range("G" + row).FormulaA1 = "=SUM(G4:$G$" + (row - 1) + ")";
            //    sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
            //    row = row + 1;

            //    using (var a = sht.Range("A3:G" + row))
            //    {
            //        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //    }
            //    //*************
            //    sht.Columns(1, 10).AdjustToContents();
            //    //********************
            //    string Fileextension = "xlsx";
            //    string filename = UtilityModule.validateFilename("ProcessReceiveItemSizeWiseSummaryReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            //    Path = Server.MapPath("~/Tempexcel/" + filename);
            //    xapp.SaveAs(Path);
            //    xapp.Dispose();
            //    //Download File
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    // Response.Clear();
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //    Response.WriteFile(Path);
            //    // File.Delete(Path);
            //    Response.End();
            //}
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void ProcessIssueReceiveSummaryReportDiamondExport(DataSet ds)
    {

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:I1").Merge();
            sht.Range("A1").Value = "Process Issue Received Summary " + "For - " + DDCompany.SelectedItem.Text;
            sht.Range("A2:I2").Merge();
            sht.Range("A2").Value = "Process Name :  " + DDProcessName.SelectedItem.Text + " " + "Summary From" + " " + TxtFromDate.Text + " To " + TxtToDate.Text;
            sht.Row(2).Height = 30;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:I2").Style.Font.Bold = true;
            //***********Filter By Item_Name

            sht.Range("A3").Value = "Emp Name";
            sht.Range("B3").Value = "Quality";
            sht.Range("C3").Value = "Design";
            sht.Range("D3").Value = "Color";
            sht.Range("E3").Value = "Shape";

            sht.Range("F3").Value = "Size";
            sht.Range("G3").Value = "Issue Qty";
            sht.Range("H3").Value = "Receive Qty";
            sht.Range("I3").Value = "Pending Qty";
            //sht.Range("A" + row).Style.Font.SetBold();
            sht.Range("A3:I3").Style.Font.SetBold();

            row = 3;
            int TotalIssueQty = 0;
            int TotalReceiveQty = 0;
            int TotalPendingQty = 0;
            int rowfrom = 0, rowto = 0;

            DataTable dtdistinct1 = ds.Tables[0].DefaultView.ToTable(true, "EmpName");

            foreach (DataRow dr2 in dtdistinct1.Rows)
            {
                DataView dv1 = new DataView(ds.Tables[0]);
                dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                DataSet ds2 = new DataSet();
                ds2.Tables.Add(dv1.ToTable());

                //row = row + 1;

                //sht.Range("A" + row).Value = "Emp Name";
                //sht.Range("B" + row).Value = "Quality";
                //sht.Range("C" + row).Value = "Design";
                //sht.Range("D" + row).Value = "Color";
                //sht.Range("E" + row).Value = "Shape";

                //sht.Range("F" + row).Value = "Size";
                //sht.Range("G" + row).Value = "Rec Qty";                      
                //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "item_finished_id");

                rowfrom = row + 1;
                foreach (DataRow dritem in dtdistinctItem.Rows)
                {
                    row = row + 1;
                    sht.Range("A" + row).SetValue(dritem["empname"]);
                    sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                    sht.Range("C" + row).SetValue(dritem["DesignName"]);
                    sht.Range("D" + row).SetValue(dritem["ColorName"]);
                    sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                    sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                    var issueqty = ds.Tables[0].Compute("sum(IssueQty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' ");
                    TotalIssueQty = TotalIssueQty + Convert.ToInt32(issueqty);
                    sht.Range("G" + row).SetValue(issueqty);

                    var Recqty = ds.Tables[0].Compute("sum(ReceiveQty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' ");
                    TotalReceiveQty = TotalReceiveQty + Convert.ToInt32(Recqty);
                    sht.Range("H" + row).SetValue(Recqty);
                    sht.Range("I" + row).SetValue(Convert.ToInt32(issueqty) - Convert.ToInt32(Recqty));

                    TotalPendingQty = TotalPendingQty + Convert.ToInt32(issueqty) - Convert.ToInt32(Recqty);

                    rowto = row;
                }
                row = row + 1;
                sht.Range("F" + row).Value = "Total";
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":$H$" + rowto + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":$I$" + rowto + ")";
                sht.Range("F" + row + ":I" + row).Style.Font.SetBold();
                row = row + 1;

                //using (var a = sht.Range("A" + (rowfrom-1) + ":G" + (rowto+1)))
                //{
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
            }

            //*************GRAND TOTAL
            sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
            sht.Range("F" + row).Value = "Grand Total";
            sht.Range("G" + row).SetValue(TotalIssueQty);
            sht.Range("H" + row).SetValue(TotalReceiveQty);
            sht.Range("I" + row).SetValue(TotalPendingQty);
            sht.Range("F" + row + ":I" + row).Style.Font.SetBold();

            using (var a = sht.Range("A3:I" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            }
            //*************
            sht.Columns(1, 10).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessIssueReceiveSummaryReportDiamond_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();


        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void WeaverRawMaterialIssueWithConsumptionDetailReportVikram()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Issueorderid=" + DDChallanNo.SelectedValue;
                FilterBy = FilterBy + ", Folio No. -" + DDChallanNo.SelectedItem.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDcustcode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + " and OM.orderid=" + DDorderno.SelectedValue;
                FilterBy = FilterBy + ", Order No. -" + DDorderno.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
            }
            if (ChkForDate.Checked == true)
            {
                str = str + " and PIM.AssignDate>='" + TxtFromDate.Text + "' and PIM.AssignDate<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }

            ////if (DDQtype.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;
            ////}
            ////if (DDQuality.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            ////}
            ////if (DDshade.SelectedIndex > 0)
            ////{
            ////    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            ////}

            //if (ChkForDate.Checked == true)
            //{
            //    str2 = str2 + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
            //    FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            //}



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueWithConsumptionDetailReport_VCKM", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@TranType", 0);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "CONSUMPTION WITH RAW MATERIAL ISSUE";
                sht.Range("A1:K1").Style.Font.FontName = "Calibri";
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Merge();

                //*******Header
                sht.Range("A2").Value = "Process Name";
                sht.Range("B2").Value = "Folio Date";
                sht.Range("C2").Value = "ChallanNo";
                sht.Range("D2").Value = "Customer Orderno";
                sht.Range("E2").Value = "Folio No";
                sht.Range("F2").Value = "Contractor Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Quality";
                sht.Range("I2").Value = "Shade Color";
                sht.Range("J2").Value = "Consumption Qty";
                sht.Range("K2").Value = "Issue Qty";
                sht.Range("L2").Value = "Balance Qty";
                sht.Range("M2").Value = "Rec Qty";
                sht.Range("N2").Value = "Order Status";
                sht.Range("O2").Value = "Folio Status";


                sht.Range("A2:O2").Style.Font.FontName = "Calibri";
                sht.Range("A2:O2").Style.Font.FontSize = 11;
                sht.Range("A2:O2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQTY"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ConsumptionQTY"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]));
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["OrderStatus"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["FolioStatus"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssueWithConsumptionDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }



    //private void JobWiseProcessReceiveSummaryFolioWiseAntique()
    //{
    //    DataSet ds = new DataSet();
    //    string where = DDProcessName.SelectedItem.Text + "  Summary ";
    //    string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
    //    //Check Conditions
    //    if (ChkForDate.Checked == true)
    //    {
    //        strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
    //        where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
    //    }

    //    if (DDChallanNo.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
    //        where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
    //    }
    //    if (DDCategory.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
    //    }
    //    if (ddItemName.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
    //        where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
    //    }
    //    if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
    //        where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
    //    }
    //    if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
    //        where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
    //    }
    //    if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
    //    }
    //    if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
    //    }
    //    if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
    //    }
    //    if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
    //    }
    //    //End Conditions

    //    //SqlParameter[] param = new SqlParameter[20];
    //    //param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
    //    //param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
    //    //param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
    //    //param[3] = new SqlParameter("@Todate", TxtToDate.Text);
    //    //param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
    //    //param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
    //    //param[6] = new SqlParameter("@Where", strCondition);

    //    //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary", param);

    //     SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //        if (con.State == ConnectionState.Closed)
    //        {
    //            con.Open();
    //        }
    //        SqlCommand cmd = new SqlCommand("JobWiseProcessReceiveSummary", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandTimeout = 300;

    //        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
    //        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
    //        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
    //        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
    //        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
    //        cmd.Parameters.AddWithValue("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
    //        cmd.Parameters.AddWithValue("@Where", strCondition);

    //        //DataSet ds = new DataSet();
    //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //        cmd.ExecuteNonQuery();
    //        ad.Fill(ds);
    //        //*************
    //        con.Close();
    //        con.Dispose();    

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        if (chkjobwisesummary.Checked == true && ChkForDate.Checked == true)
    //        {
    //            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
    //            {
    //                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
    //            }
    //            string Path = "";
    //            var xapp = new XLWorkbook();
    //            var sht = xapp.Worksheets.Add("sheet1");
    //            int row = 0;

    //            //******************

    //            sht.Range("A1").Value = where;
    //            sht.Range("A1:J1").Style.Alignment.SetWrapText();
    //            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //            sht.Range("A1:J1").Merge();
    //            sht.Row(1).Height = 44;

    //            sht.Range("A2").Value = "FolioNo";
    //            sht.Range("B2").Value = "ItemName";
    //            sht.Range("C2").Value = "QualityName";
    //            sht.Range("D2").Value = "DesignName";
    //            sht.Range("E2").Value = "ColorName";
    //            sht.Range("F2").Value = "Size";
    //            sht.Range("G2").Value = "Qty";
    //            sht.Range("H2").Value = "Rate";
    //            sht.Range("I2").Value = "Area";
    //            sht.Range("J2").Value = "Amount";
    //            sht.Range("K2").Value = "ChallanNo";

    //            sht.Range("B2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //            sht.Range("A2:K2").Style.Font.Bold = true;

    //            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "item_finished_Id", "Rate");
    //            DataView dv1 = new DataView(dtdistinct);
    //            dv1.Sort = "item_finished_Id,Rate";
    //            DataTable dtdistinct1 = dv1.ToTable();
    //            row = 3;
    //            foreach (DataRow dr in dtdistinct1.Rows)
    //            {
    //                DataView dvitemdesc = new DataView(ds.Tables[0]);
    //                dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and Rate='" + dr["Rate"] + "'";
    //                DataSet dsitemdesc = new DataSet();
    //                dsitemdesc.Tables.Add(dvitemdesc.ToTable());
    //                //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
    //                if (Session["varCompanyNo"].ToString() == "27")
    //                {
    //                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "IssueOrderId", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate", "ChallanNo");
    //                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
    //                    {

    //                        sht.Range("A" + row).SetValue(dritemdesc["IssueOrderId"]);
    //                        sht.Range("B" + row).SetValue(dritemdesc["Item_Name"]);
    //                        sht.Range("C" + row).SetValue(dritemdesc["QualityName"]);
    //                        sht.Range("D" + row).SetValue(dritemdesc["DesignName"]);
    //                        sht.Range("E" + row).SetValue(dritemdesc["ColorName"]);
    //                        sht.Range("F" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
    //                        var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "'");
    //                        var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "'");
    //                        var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "' and ChallanNo='" + dritemdesc["ChallanNo"] + "'");
    //                        sht.Range("G" + row).SetValue(qty == DBNull.Value ? 0 : qty);
    //                        sht.Range("H" + row).SetValue(dritemdesc["Rate"]);
    //                        sht.Range("I" + row).SetValue(Area == DBNull.Value ? 0 : Area);
    //                        sht.Range("J" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);
    //                        sht.Range("K" + row).SetValue(dritemdesc["ChallanNo"]);
    //                        row = row + 1;

    //                    }
    //                }
    //                else
    //                {
    //                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "IssueOrderId", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate");
    //                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
    //                    {

    //                        sht.Range("A" + row).SetValue(dritemdesc["IssueOrderId"]);
    //                        sht.Range("B" + row).SetValue(dritemdesc["Item_Name"]);
    //                        sht.Range("C" + row).SetValue(dritemdesc["QualityName"]);
    //                        sht.Range("D" + row).SetValue(dritemdesc["DesignName"]);
    //                        sht.Range("E" + row).SetValue(dritemdesc["ColorName"]);
    //                        sht.Range("F" + row).SetValue(dritemdesc["width"] + "x" + dritemdesc["Length"]);
    //                        var qty = ds.Tables[0].Compute("sum(qty)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "'");
    //                        var Area = ds.Tables[0].Compute("sum(ARea)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "'");
    //                        var Amount = ds.Tables[0].Compute("sum(Amount)", "item_finished_Id='" + dritemdesc["item_finished_Id"] + "' and Rate='" + dritemdesc["Rate"] + "' and IssueOrderId='" + dritemdesc["IssueOrderId"] + "'");
    //                        sht.Range("G" + row).SetValue(qty == DBNull.Value ? 0 : qty);
    //                        sht.Range("H" + row).SetValue(dritemdesc["Rate"]);
    //                        sht.Range("I" + row).SetValue(Area == DBNull.Value ? 0 : Area);
    //                        sht.Range("J" + row).SetValue(Amount == DBNull.Value ? 0 : Amount);
    //                        row = row + 1;

    //                    }
    //                }



    //                ////********Grand TOtal
    //                //sht.Range("E" + row).Value = "GRAND TOTAL";
    //                //sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
    //                //sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
    //                //sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
    //                //sht.Range("E" + row + ":J" + row).Style.Font.Bold = true;
    //                //sht.Columns(1, 10).AdjustToContents();
    //                ////sht.Rows().AdjustToContents();

    //            }
    //            //********Grand TOtal
    //            sht.Range("E" + row).Value = "GRAND TOTAL";
    //            sht.Range("G" + row).FormulaA1 = "=SUM(G3:$G$" + (row - 1) + ")";
    //            sht.Range("I" + row).FormulaA1 = "=SUM(I3:$I$" + (row - 1) + ")";
    //            sht.Range("J" + row).FormulaA1 = "=SUM(J3:$J$" + (row - 1) + ")";
    //            sht.Range("E" + row + ":K" + row).Style.Font.Bold = true;
    //            sht.Columns(1, 10).AdjustToContents();
    //            //sht.Rows().AdjustToContents();

    //            using (var a = sht.Range("A2" + ":K" + row))
    //            {
    //                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            }

    //            if (Session["varCompanyNo"].ToString() != "27")
    //            {

    //                sht.Column(11).Delete();
    //            }

    //            string Fileextension = "xlsx";
    //            string filename = UtilityModule.validateFilename("JobwisereceivesummaryFolioWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
    //            Path = Server.MapPath("~/Tempexcel/" + filename);
    //            xapp.SaveAs(Path);
    //            xapp.Dispose();
    //            //Download File
    //            Response.ClearContent();
    //            Response.ClearHeaders();
    //            // Response.Clear();
    //            Response.ContentType = "application/vnd.ms-excel";
    //            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //            Response.WriteFile(Path);
    //            // File.Delete(Path);
    //            Response.End();
    //            return;
    //        }
    //        //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
    //        Session["rptFileName"] = Session["ReportPath"];
    //        Session["GetDataset"] = ds;
    //        //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }
    //}
    protected void RDTasselMakingRawIssueDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        if (RDTasselMakingRawIssueDetail.Checked == true)
        {
            TDexcelExport.Visible = false;
            TDsizesummary.Visible = false;
            TDJobWiseSummary.Visible = false;
            TRCheckWithTime.Visible = false;
            TRQualityWiseSummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            ChkForPendingStockNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRRecChallan.Visible = false;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
        }
    }
    protected void TasselMakingRawIssueDetailReport()
    {
        lblMessage.Text = "";
        try
        {
            int ChkDateFlag = 0;
            if (ChkForDate.Checked == true)
            {
                ChkDateFlag = 1;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetTassleRawMaterialDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@CustomerID", DDcustcode.SelectedIndex > 0 ? DDcustcode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", DDorderno.SelectedIndex > 0 ? DDorderno.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@EmpID", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ChkDateFlag", ChkDateFlag);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@TranType", 0);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "TASSAL RAWMATERIAL REPORT";
                sht.Range("A1:L1").Style.Font.FontName = "Calibri";
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Font.FontSize = 12;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Merge();

                //*******Header
                sht.Range("A2").Value = "ISSUE DATE";
                sht.Range("B2").Value = "ISSUE CHALLAN NO";
                sht.Range("C2").Value = "BUYER CODE";
                sht.Range("D2").Value = "BUYER ORDER NO";
                sht.Range("E2").Value = "FOLIO NO";
                sht.Range("F2").Value = "PARTY NAME";
                sht.Range("G2").Value = "YARN ITEM NAME";
                sht.Range("H2").Value = "YARN QUALITY";
                sht.Range("I2").Value = "SHADE COLOR";
                sht.Range("J2").Value = "LOT NO";
                sht.Range("K2").Value = "TAG NO";
                sht.Range("L2").Value = "ISSUE QTY";

                sht.Range("A2:L2").Style.Font.FontName = "Calibri";
                sht.Range("A2:L2").Style.Font.FontSize = 11;
                sht.Range("A2:L2").Style.Font.Bold = true;

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IssueChallanNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["BuyerCode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["BuyerOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["FolioNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["IssueQuantity"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "L")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TassalRawMaterialReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    private void JobWiseProcessReceiveSummary_Vikram()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }

        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary_Vikram", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true)
            {
                if (ChkForDate.Checked == true)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    int row = 0;

                    //******************

                    sht.Range("A1").Value = where;
                    sht.Range("A1:J1").Style.Alignment.SetWrapText();
                    sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1:J1").Merge();
                    sht.Row(1).Height = 44;

                    sht.Range("A2").Value = "EmpName";
                    sht.Range("B2").Value = "Quality Name";
                    sht.Range("C2").Value = "Design Name";
                    sht.Range("D2").Value = "Size";
                    sht.Range("E2").Value = "Qty";
                    sht.Range("F2").Value = "Area";
                    sht.Range("G2").Value = "Rate";
                    sht.Range("H2").Value = "Amount";
                    sht.Range("I2").Value = "Party ChallanNo";

                    sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A2:I2").Style.Font.Bold = true;

                    row = 3;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["WIDTH"] + "x" + ds.Tables[0].Rows[i]["LENGTH"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Area"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["PartyChallanNo"]);

                        row = row + 1;
                    }

                    //********Grand TOtal
                    sht.Range("B" + row).Value = "GRAND TOTAL";
                    sht.Range("E" + row).FormulaA1 = "=SUM(E3:$E$" + (row - 1) + ")";
                    sht.Range("F" + row).FormulaA1 = "=SUM(F3:$F$" + (row - 1) + ")";
                    sht.Range("H" + row).FormulaA1 = "=SUM(H3:$H$" + (row - 1) + ")";
                    sht.Range("B" + row + ":H" + row).Style.Font.Bold = true;
                    sht.Columns(1, 10).AdjustToContents();
                    //sht.Rows().AdjustToContents();

                    using (var a = sht.Range("A2" + ":I" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }


                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("Jobwisereceivesummary_Vikram" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                    return;

                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptProcessDetailNewRecSummary.rpt";

                    Session["Getdataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNewRecSummary.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void RDProcessWiseAdvancePayment_CheckedChanged(object sender, EventArgs e)
    {
        TRQualitySizeWiseHissabSummary.Visible = false;
        TRQualityDesignSizeWiseSummary.Visible = false;
        TRForWithoutTDS.Visible = false;
        TRAsOnDate.Visible = false;
        if (RDProcessWiseAdvancePayment.Checked == true)
        {
            TDJobWiseSummary.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = false;
            TRorderno.Visible = false;
            TRCheckWithTime.Visible = false;
            TRQualityWiseSummary.Visible = false;
            TRRecChallan.Visible = false;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TR1.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            ChkForPendingStockNo.Visible = false;
            TDexcelExport.Visible = false;
            ChkSummary.Visible = false;
            ChkForComplete.Visible = false;
        }

    }
    protected void ProcessWiseAdvancePaymentDetail()
    {
        try
        {
            //string str = "";
            //string Column = ""; string Column2 = "";
            ////if (DDProductionstatus.SelectedIndex > 0)
            ////{
            ////    str = str + " and Status='" + DDProductionstatus.SelectedItem.Text + "'";
            ////}           

            //str = str + " and AAFW.CompanyId=" + DDCompany.SelectedValue;

            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    str = str + " and AAFW.EmpID=" + DDEmpName.SelectedValue;
            //}           
            //if (ChkForDate.Checked == true)
            //{
            //    str = str + " and AAFW.DATE>='" + TxtFromDate.Text + "' and AAFW.DATE<='" + TxtToDate.Text + "'";
            //}
            //////str = str + " and " + Column2 + "";
            ////*****************

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@EmpId", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
            param[4] = new SqlParameter("@ToDate", TxtToDate.Text);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[6] = new SqlParameter("@ChkForDate", ChkForDate.Checked == true ? 1 : 0);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPROCESSWISEADVANCEPAYMENTREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptProcessEmpWiseAdvancePayment.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessEmpWiseAdvancePayment.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    private void JobWiseProcessReceiveSummaryVikramMirzapur()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }

        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
        param[6] = new SqlParameter("@Where", strCondition);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary_VikramMirzapur", param);

        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptProcessDetailNEWIssueNoWiseVikramMirzapur.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNEWIssueNoWiseVikramMirzapur.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }

    }

    protected void ProcessIssueSummaryExcelExportVikramKM()
    {
        if (ChkForDate.Checked == true)
        {
            string str = "";
            string filterby = "From : " + TxtFromDate.Text + "  To : " + TxtToDate.Text;

            str = "And PM.CompanyId=" + DDCompany.SelectedValue;

            //Check Conditions
            if (ChkForDate.Checked == true)
            {
                str = str + " And PM.AssignDate>='" + TxtFromDate.Text + "' And PM.AssignDate<='" + TxtToDate.Text + "'";
            }
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    str = str + " And PIM.EmpId=" + DDEmpName.SelectedValue;
            //    filterby = filterby + " Emp : " + DDEmpName.SelectedItem.Text;
            //}
            if (DDChallanNo.SelectedIndex > 0)
            {
                //str = str + " And PIM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
                str = str + " And PM.IssueOrderId=" + DDChallanNo.SelectedValue;
                filterby = filterby + " Challan No : " + DDChallanNo.SelectedItem.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                filterby = filterby + " Category : " + DDCategory.SelectedItem.Text;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
                filterby = filterby + " Item : " + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
                filterby = filterby + " Quality : " + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
                filterby = filterby + " Design : " + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
                filterby = filterby + " Color : " + DDColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
                filterby = filterby + " ShapeId : " + DDShape.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
                filterby = filterby + " SizeId : " + DDSize.SelectedItem.Text;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                filterby = filterby + " ShadecolorId : " + DDShadeColor.SelectedItem.Text;
            }
            if (txtlocalorderno.Text != "")
            {
                str = str + " And OM.Localorder='" + txtlocalorderno.Text + "'";
                filterby = filterby + " Localorder : " + txtlocalorderno.Text;
            }
            if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
            {
                str = str + " And OM.customerid=" + DDcustcode.SelectedValue;
                filterby = filterby + " Cust code : " + DDcustcode.SelectedItem.Text;
            }
            if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
            {
                str = str + " And OM.orderid=" + DDorderno.SelectedValue;
                filterby = filterby + " Order No. : " + DDorderno.SelectedItem.Text;

            }
            SqlParameter[] arr = new SqlParameter[9];
            arr[0] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            arr[1] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            arr[2] = new SqlParameter("@fromdate", TxtFromDate.Text);
            arr[3] = new SqlParameter("@Todate", TxtToDate.Text);
            arr[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
            arr[5] = new SqlParameter("@Issueorderid", Chkissueno.Checked == true ? txtissueno.Text : "");
            arr[6] = new SqlParameter("@Where", str);
            arr[7] = new SqlParameter("@userid", Session["varuserid"]);
            arr[8] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FinishingIssueReceiveDetailExcelReport", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:J1").Merge();
                sht.Range("A1").Value = "Process Issue Summary Details " + "For - " + DDCompany.SelectedItem.Text;
                sht.Range("A2:J2").Merge();
                sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:J2").Style.Font.Bold = true;
                //***********Filter By Item_Name

                sht.Range("A3").Value = "Emp Name";
                sht.Range("B3").Value = "Quality";
                sht.Range("C3").Value = "Design";
                sht.Range("D3").Value = "Color";
                sht.Range("E3").Value = "Shape";

                sht.Range("F3").Value = "Size";
                sht.Range("G3").Value = "Issue Qty";

                sht.Range("H3").Value = "Area";
                sht.Range("I3").Value = "Rate";
                sht.Range("J3").Value = "Amount";


                //sht.Range("A" + row).Style.Font.SetBold();
                sht.Range("A3:J3").Style.Font.SetBold();

                row = 3;
                Decimal tqty = 0;
                Decimal tArea = 0;
                Decimal tAmt = 0;
                string empname = "";
                string itemfinsihedid = "0";
                int rowfrom = 0, rowto = 0;



                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "NoofEmp");

                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "NoofEmp='" + dr["NoofEmp"] + "'";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());

                    DataTable dtdistinct1 = ds1.Tables[0].DefaultView.ToTable(true, "EmpName");

                    foreach (DataRow dr2 in dtdistinct1.Rows)
                    {
                        DataView dv1 = new DataView(ds.Tables[0]);
                        dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
                        DataSet ds2 = new DataSet();
                        ds2.Tables.Add(dv1.ToTable());

                        //row = row + 1;

                        //sht.Range("A" + row).Value = "Emp Name";
                        //sht.Range("B" + row).Value = "Quality";
                        //sht.Range("C" + row).Value = "Design";
                        //sht.Range("D" + row).Value = "Color";
                        //sht.Range("E" + row).Value = "Shape";

                        //sht.Range("F" + row).Value = "Size";
                        //sht.Range("G" + row).Value = "Issue Qty";

                        ////sht.Range("A" + row).Style.Font.SetBold();
                        //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

                        DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "Item_Finished_Id", "Rate");

                        rowfrom = row + 1;
                        foreach (DataRow dritem in dtdistinctItem.Rows)
                        {
                            row = row + 1;
                            sht.Range("A" + row).SetValue(dritem["empname"]);
                            sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
                            sht.Range("C" + row).SetValue(dritem["DesignName"]);
                            sht.Range("D" + row).SetValue(dritem["ColorName"]);
                            sht.Range("E" + row).SetValue(dritem["ShapeName"]);
                            sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

                            var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Name='" + dritem["Item_Name"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and ShapeName='" + dritem["ShapeName"] + "' and Width='" + dritem["Width"] + "'  and Length='" + dritem["Length"] + "' and Rate='" + dritem["Rate"] + "'");
                            tqty = tqty + Convert.ToDecimal(qty);
                            sht.Range("G" + row).SetValue(qty);

                            var Area = ds.Tables[0].Compute("sum(Area)", "EmpName='" + dr2["EmpName"] + "' and Item_Name='" + dritem["Item_Name"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and ShapeName='" + dritem["ShapeName"] + "' and Width='" + dritem["Width"] + "'  and Length='" + dritem["Length"] + "' and Rate='" + dritem["Rate"] + "'");
                            tArea = tArea + Convert.ToDecimal(Area);
                            sht.Range("H" + row).SetValue(Area);

                            sht.Range("I" + row).SetValue(dritem["Rate"]);

                            var Amt = ds.Tables[0].Compute("sum(Amount)", "EmpName='" + dr2["EmpName"] + "' and Item_Name='" + dritem["Item_Name"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and ShapeName='" + dritem["ShapeName"] + "' and Width='" + dritem["Width"] + "'  and Length='" + dritem["Length"] + "' and Rate='" + dritem["Rate"] + "'");
                            tAmt = tAmt + Convert.ToDecimal(Amt);
                            sht.Range("J" + row).SetValue(Amt);

                            //var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' and Rate='" + dritem["Rate"] + "'");
                            //tqty = tqty + Convert.ToDecimal(qty);
                            //sht.Range("G" + row).SetValue(qty);

                            //var Area = ds.Tables[0].Compute("sum(Area)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' and Rate='" + dritem["Rate"] + "'");
                            //tArea = tArea + Convert.ToDecimal(Area);
                            //sht.Range("H" + row).SetValue(Area);

                            //sht.Range("I" + row).SetValue(dritem["Rate"]);

                            //var Amt = ds.Tables[0].Compute("sum(Amount)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "' and Rate='" + dritem["Rate"] + "'");
                            //tAmt = tAmt + Convert.ToDecimal(Amt);
                            //sht.Range("J" + row).SetValue(Amt);

                            rowto = row;
                        }
                        row = row + 1;
                        sht.Range("F" + row).Value = "Total";
                        sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";

                        sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":$H$" + rowto + ")";
                        sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":$J$" + rowto + ")";

                        sht.Range("F" + row + ":J" + row).Style.Font.SetBold();
                        row = row + 1;

                    }
                }

                //*************GRAND TOTAL
                sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("G" + row).SetValue(tqty);
                sht.Range("H" + row).SetValue(tArea);
                sht.Range("J" + row).SetValue(tAmt);
                sht.Range("F" + row + ":J" + row).Style.Font.SetBold();


                using (var a = sht.Range("A3:J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 13).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessIssueSummaryReportVKM_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        }
    }

    private void FinishingReceiveBalanceReport()
    {
        // DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        //if (ChkForDate.Checked == true)
        //{
        //    strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        //}
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_FINISHINGRECEIVEBALANCEREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@AsOnDate", txtAsOnDate.Text);
        cmd.Parameters.AddWithValue("@Where", strCondition);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptFinishingBalanceOrderWiseReport.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingBalanceOrderWiseReport.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    private void JobWiseProcessReceiveSummary_CI()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  Summary ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }

        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
            where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[20];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Process_Rec_ID", DDChallanNo.SelectedIndex<=0?"0" : DDChallanNo.SelectedValue);
        param[6] = new SqlParameter("@Where", strCondition);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "JobWiseProcessReceiveSummary_CI", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkSummary.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptProcessDetailNewRecSummary_CI.rpt";

                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNewRecSummary_CI.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    private void FinishingIssueDetailSummary(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PM.Issueorderid=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[5] = new SqlParameter("@Issueorderid", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);        

        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "FinishingIssueDetailSummary", param);

        if (ds.Tables[0].Rows.Count > 0)
        {                
            Session["rptFileName"] = "~\\Reports\\RptFinishingIssueDetailSummary.rpt";               
          
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingIssueDetailSummary.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void RDFinishingRecSummaryWithTDS_CheckedChanged(object sender, EventArgs e)
    {
        TRAsOnDate.Visible = false;
        if (RDFinishingRecSummaryWithTDS.Checked == true)
        {
            TDJobWiseSummary.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = false;
            TRorderno.Visible = false;
            TRCheckWithTime.Visible = false;
            TRQualityWiseSummary.Visible = false;
            TRRecChallan.Visible = false;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TR1.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            ChkForPendingStockNo.Visible = false;
            TDexcelExport.Visible = false;
            ChkSummary.Visible = false;
            ChkForComplete.Visible = false;
            TRForWithoutTDS.Visible = true;
            TRQualityDesignSizeWiseSummary.Visible = false;
            TRQualitySizeWiseHissabSummary.Visible = false;
        }

    }
    protected void FinishingReceiveSummaryWithTDSReport_CI()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "";
           
            //if (ddItemName.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Item_id=" + ddItemName.SelectedValue;
            //    FilterBy = FilterBy + ", Item Name -" + ddItemName.SelectedItem.Text;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //    FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            //    FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            //    FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            //    FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            //}
            if (ChkForDate.Checked == true)
            {
                str = str + " and PH.Date>='" + TxtFromDate.Text + "' and PH.Date<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }           

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_FinishingProcessHissabSummaryWithTDSReport_CI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@ChkselectDate",  ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);           

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForWithoutTDS.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingProcessHissabSummaryWithoutTDS_CI.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingProcessHissabSummaryWithTDS_CI.rpt";
                }                

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessHissabSummaryWithTDS_CI.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
           lblMessage.Text = ex.Message;
        }

    }
    protected void RDFinishingHissabSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRAsOnDate.Visible = false;
        if (RDFinishingHissabSummary.Checked == true)
        {
            TDJobWiseSummary.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = false;
            TRorderno.Visible = false;
            TRCheckWithTime.Visible = false;
            TRQualityWiseSummary.Visible = false;
            TRRecChallan.Visible = false;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TR1.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            ChkForPendingStockNo.Visible = false;
            TDexcelExport.Visible = false;
            ChkSummary.Visible = false;
            ChkForComplete.Visible = false;
            TRForWithoutTDS.Visible = false;
            trDates.Visible = true;
            ChkForDate.Visible = true;
            TRQualityDesignSizeWiseSummary.Visible = false;
            TRQualitySizeWiseHissabSummary.Visible = false;
        }

    }
    protected void FinishingHissabSummaryReport_CI()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "";

            //if (ddItemName.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Item_id=" + ddItemName.SelectedValue;
            //    FilterBy = FilterBy + ", Item Name -" + ddItemName.SelectedItem.Text;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //    FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            //    FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            //    FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            //    FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            //}
            if (ChkForDate.Checked == true)
            {
                str = str + " and PH.Date>='" + TxtFromDate.Text + "' and PH.Date<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_FinishingProcessHissabSummaryVoucherReport_CI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@ChkselectDate", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
              
                Session["rptFileName"] = "~\\Reports\\RptFinishingProcessHissabVoucherSummary_CI.rpt";                

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessHissabVoucherSummary_CI.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

    }

    private void FinishingReceiveNillBalanceReport()
    {
        // DataSet ds = new DataSet();
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        //if (ChkForDate.Checked == true)
        //{
        //    strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        //}
        //if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        //}
        //if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        //}
        //if (DDCategory.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        //}
        //if (ddItemName.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        //}
        //if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        //}
        //if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        //}
        //if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        //}
        //if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        //}
        //if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        //}
        //if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        //{
        //    strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        //}

        if (DDProcessName.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_FINISHINGRECEIVE_NILL_BALANCEREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@AsOnDate", DateTime.Now.ToString("dd-MMM-yyyy"));
            cmd.Parameters.AddWithValue("@Where", strCondition);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingBalanceNillReportCI.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinishingBalanceNillReportCI.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please Select Process Name!');", true);
        }

        
    }
    protected void RDFinishingRegisterSummary_CheckedChanged(object sender, EventArgs e)
    {
        TRAsOnDate.Visible = false;
        if (RDFinishingRegisterSummary.Checked == true)
        {
            TDJobWiseSummary.Visible = false;
            TDsizesummary.Visible = false;
            TRBuyerItemSizeWiseSummary.Visible = false;
            TRChkBoxIssueDate.Visible = false;
            TRcustcode.Visible = false;
            TRorderno.Visible = false;
            TRCheckWithTime.Visible = false;
            TRQualityWiseSummary.Visible = false;
            TRRecChallan.Visible = false;
            TRCategoryName.Visible = false;
            TRddItemName.Visible = false;
            TRlotNo.Visible = false;
            TR1.Visible = false;
            TRGatePass.Visible = false;
            TRIssueNo.Visible = false;
            ChkForProcessIssRecSummary.Visible = false;
            ChkForPendingStockNo.Visible = false;
            TDexcelExport.Visible = false;
            ChkSummary.Visible = false;
            ChkForComplete.Visible = false;
            TRForWithoutTDS.Visible = true;
            trDates.Visible = true;
            ChkForDate.Visible = true;
            TRQualityDesignSizeWiseSummary.Visible = false;
            TRQualitySizeWiseHissabSummary.Visible = false;
        }

    }
    protected void FinishingRegisterSummaryReport_CI()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", FilterBy = "";

            //if (ddItemName.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Item_id=" + ddItemName.SelectedValue;
            //    FilterBy = FilterBy + ", Item Name -" + ddItemName.SelectedItem.Text;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //    FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            //    FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            //    FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            //    FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            //}
            if (ChkForDate.Checked == true)
            {
                str = str + " and PM.ReceiveDate>='" + TxtFromDate.Text + "' and PM.ReceiveDate<='" + TxtToDate.Text + "'";
                FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("JobWiseProcessReceiveRegisterSummary_CI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@DATEFLAG", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForWithoutTDS.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingProcessRegisterSummaryWithoutTDS_CI.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptFinishingProcessRegisterSummary_CI.rpt";
                }

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessRegisterSummary_CI.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

    }
}