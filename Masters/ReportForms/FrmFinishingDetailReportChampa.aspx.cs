﻿using System;
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

public partial class Masters_ReportForms_FrmFinishingDetailReportChampa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //TRlotNo.Visible = false;


            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
                        Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where Process_Name_Id<>1 and MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By PROCESS_NAME
                        select CI.CustomerId,CI.CustomerCode from customerinfo  CI order by CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            // CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Select--");
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            int varcompanyNo = Convert.ToInt16(Session["varMasterCompanyIDForERP"].ToString());
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            BindCategory();
        }
    }
    protected void BindCategory()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");

        if (DDCategory.Items.Count > 0)
        {
            DDCategory.SelectedIndex = 1;
            BindItem();
        }
    }
    protected void BindItem()
    {
        string Str1 = "";
        if (DDCompany.SelectedIndex > 0 || DDcustcode.SelectedIndex > 0)
        {
            Str1 = @"select Distinct VF.ITEM_ID,VF.ITEM_NAME from ordermaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                 INNER JOIN V_FinishedItemDetailNew VF ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID and VF.MasterCompanyId=20
                  where Om.Status=0 ";
            if (DDCompany.SelectedIndex > 0)
            {
                Str1 = Str1 + " And OM.companyid=" + DDCompany.SelectedValue;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                Str1 = Str1 + " And OM.CustomerId=" + DDcustcode.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddItemName, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string str = "Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname from Empinfo EI,PROCESS_Receive_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId  And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EmpName";
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " and CompanyId=" + DDCompany.SelectedValue + "";
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId<>1 order by EmpName", true, "--Select--");
        }

        DDChallanNo.Items.Clear();

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
            Str = "Select Process_Rec_Id,Process_Rec_Id from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where 1=1 ";
            if (DDCompany.SelectedIndex > 0)
            {
                Str = Str + " And Companyid=" + DDCompany.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                Str = Str + " And EmpId=" + DDEmpName.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
            Label4.Text = "Rec Challan No.";

            //ChallanNoSelectedIndexChange();           
        }
        else
        {
            Str = @"select Distinct VPRM.Process_Rec_Id,VPRM.Process_Rec_Id as Process_Rec_Id1 from VIEW_PROCESS_RECEIVE_MASTER VPRM Where 1=1";

            if (DDCompany.SelectedIndex > 0)
            {
                Str = Str + " And Companyid=" + DDCompany.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                Str = Str + " And EmpId=" + DDEmpName.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
            Label4.Text = "Rec Challan No.";
        }
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (lblMessage.Text == "")
        {
            CHECKVALIDCONTROL();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {


                if (RDFinishingDetail.Checked == true)
                {
                    FinishingReceiveDetail(tran);
                }
                else if (RDFinishingVoucherSummary.Checked == true)
                {
                    FinishingVoucherSummary(tran);
                }

                //if (RDFinishingReceiveDetail.Checked == true)
                //{
                //    FinishingReceiveDetail(tran);
                //}               
                //else if (RDFinishingIssueDetail.Checked == true)
                //{
                //    FinishingIssueDetail(tran);                   
                //}
                //else if (RDFinisherPendingPcs.Checked == true)
                //{
                //    if (ChkForFinisherNillDetail.Checked == true)
                //    {
                //        FinisherNillPendingPcs();
                //    }
                //    else
                //    {
                //        FinisherPendingPcs(tran);
                //    }
                //}

                tran.Commit();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmFinishingDetailReportChampa.aspx");
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

    private void FinishingReceiveDetail(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        //string strCondition = "And PRM.CompanyId=" + DDCompany.SelectedValue;
        string strCondition = "";
        ////Check Conditions
        //if (ChkForDate.Checked == true)
        //{
        //    strCondition = strCondition + " And PRM.ReceiveDate>='" + TxtFromDate.Text + "' And PRM.ReceiveDate<='" + TxtToDate.Text + "'";
        //}
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        //if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
        //}
        if (DDChallanNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PRM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
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
        if (ChkForVoucher.Checked == true)
        {
            strCondition = strCondition + " and PRM.VoucherNo<>0 OR PRM.VoucherNo is not null";
        }
        else
        {
            strCondition = strCondition + " and PRM.VoucherNo=0 OR PRM.VoucherNo is null";
        }



        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[2] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[4] = new SqlParameter("@Todate", TxtToDate.Text);
        param[5] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        // param[5] = new SqlParameter("@Process_Rec_Id", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);
        param[7] = new SqlParameter("@VoucherFlag", ChkForVoucher.Checked == true ? "1" : "0");

        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "FINISHINGRECEIVEDETAILWITH_VOUCHER_ForMaltiRugs", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptfinishingreceivedetailSummaryMaltiRugs.rpt";

            if (ChkForContractorWise.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingContractorWiseChampa.rpt";
            }
            else if (ChkForQualityWise.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingQualityWiseChampa.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingDetailChampa.rpt";
            }



            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingDetailChampa.xsd";

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

    private void FinishingVoucherSummary(SqlTransaction tran)
    {
        DataSet ds = new DataSet();
        //string strCondition = "And PRM.CompanyId=" + DDCompany.SelectedValue;
        string strCondition = "";
        ////Check Conditions
        //if (ChkForDate.Checked == true)
        //{
        //    strCondition = strCondition + " And PRM.ReceiveDate>='" + TxtFromDate.Text + "' And PRM.ReceiveDate<='" + TxtToDate.Text + "'";
        //}
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }

        //if (ChkForVoucher.Checked == true)
        //{
        //    strCondition = strCondition + " and PRM.VoucherNo<>0 OR PRM.VoucherNo is not null";
        //}
        //else
        //{
        //    strCondition = strCondition + " and PRM.VoucherNo=0 OR PRM.VoucherNo is null";
        //}



        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[2] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[4] = new SqlParameter("@Todate", TxtToDate.Text);
        param[5] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        // param[5] = new SqlParameter("@Process_Rec_Id", txtissueno.Text);
        param[6] = new SqlParameter("@Where", strCondition);
        param[7] = new SqlParameter("@VoucherFlag", ChkForVoucher.Checked == true ? "1" : "0");

        ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "FINISHINGRECEIVEDETAILWITH_VOUCHER_SUMMARYForMaltiRugs", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptFinisherVoucherSummJobWiseNewChampa.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinisherVoucherSummJobWiseNewChampa.xsd";

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
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtToDate) == false)
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
    protected void ShowParameters()
    {
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
                        TRDDShape.Visible = false;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = false;
                        break;
                }
            }
        }
    }
    protected void FillQuality()
    {
        string str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid where 1=1";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + "  and IM.Category_id=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + ddItemName.SelectedValue;
        }
        str = str + "  order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowParameters();
        FillQuality();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDSize.Items.Clear();
        //if (DDQuality.Items.Count > 0)
        //{
        //    DDQuality.SelectedIndex = 0;
        //    DDQuality_SelectedIndexChanged(sender, new EventArgs());
        //}
        ////************
        //UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName From Shape", true, "--Plz Select--");
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designId,vf.designName From V_finishedItemDetailnew Vf Where Vf.designId>0";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by designName";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void FillColor()
    {
        string str = @"select Distinct Vf.colorid,vf.colorname From V_finishedItemDetailnew Vf Where Vf.colorid>0";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and vf.designid=" + DDDesign.SelectedValue;
        }

        str = str + "  order by colorname";
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void FillSize()
    {
        string size = "ProdSizeFt";
        if (chkmtr.Checked == true)
        {
            size = "ProdSizemtr";
        }

        string str = @"select Distinct  Vf.Sizeid,vf." + size + " as Size  From V_finishedItemDetailnew Vf Where Vf.Sizeid>0";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
        }
        //if (DDColor.SelectedIndex > 0)
        //{
        //    str = str + "  and vf.Colorid=" + DDColor.SelectedValue;
        //}

        str = str + "  order by Size";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "---Plz Select---");
    }
    protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
        FillSize();
        FillColor();
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
    protected void RDFinishingDetail_CheckedChanged(object sender, EventArgs e)
    {
        TRcustcode.Visible = true;
        TRRecChallan.Visible = true;
        TRProcessName.Visible = true;
        ChkForDate.Checked = true;
        TRCategory.Visible = true;
        TRddItemName.Visible = true;
        TRContractorWise.Visible = true;
        TRVoucher.Visible = true;
        TRQualityWise.Visible = true;

        ChkForDate_CheckedChanged(sender, e);
        //UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName", true, "--Select--");

        BindCategory();
        DDProcessName_SelectedIndexChanged(sender, new EventArgs());

    }
    protected void RDFinishingVoucherSummary_CheckedChanged(object sender, EventArgs e)
    {
        if (RDFinishingVoucherSummary.Checked == true)
        {
            TRcustcode.Visible = true;
            TRRecChallan.Visible = false;
            TRProcessName.Visible = true;
            ChkForDate.Checked = true;
            TRCategory.Visible = false;
            TRddItemName.Visible = false;
            TRDDQuality.Visible = false;
            TRContractorWise.Visible = false;
            TRVoucher.Visible = false;
            TRQualityWise.Visible = false;


            ChkForDate_CheckedChanged(sender, e);
            //UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName", true, "--Select--");

            BindCategory();
            DDProcessName_SelectedIndexChanged(sender, new EventArgs());
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
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItem();
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItem();
    }
    protected void ChkForContractorWise_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForContractorWise.Checked == true)
        {
            ChkForQualityWise.Checked = false;
        }
    }
    protected void ChkForQualityWise_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForQualityWise.Checked == true)
        {
            ChkForContractorWise.Checked = false;
        }
    }

}