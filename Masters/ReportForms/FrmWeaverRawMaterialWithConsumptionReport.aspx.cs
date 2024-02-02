using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmWeaverRawMaterialWithConsumptionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            
            if (DDCompany.Items.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, "select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN EmpProcess EP ON EP.Empid=EI.EmpId and EP.ProcessId=1 order by EmpName", true, "--Select--");
            }
            BindQualityType();
            BindItemName();
            BindGodownName();
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");            
        }
    }
    private void BindQualityType()
    {
        if (RDWeaverRawMaterial.Checked == true || RDWeaverRawLedger.Checked==true)
        {
            string str1 = @"select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=0 and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name";

            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
        }
        else if(RDWeaverRawMaterialIssueReceive.Checked==true || RDFinisherRawMaterialIssueReceive.Checked==true)
        {
            UtilityModule.ConditionalComboFill(ref DDQualityType, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=0 and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Select--");
        }
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=1 and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindGodownName()
    {
        UtilityModule.ConditionalComboFill(ref DDGodownName, "select GodownId,GodownName from godownmaster GM  where GM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by GM.GodownName", true, "--Plz Select--");
                
    } 
    protected void chekboxlist_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public string QualityTypeId = "";
    protected void ReportRawMaterialWithConsumption()
    {
        lblErrmsg.Text = "";
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[9];
            _arrPara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Processid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@EmpId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@QualityTypeId", SqlDbType.VarChar, 200);  
            _arrPara[4] = new SqlParameter("@ItemId", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            _arrPara[6] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            _arrPara[7] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);            

            int n = chekboxlist.Items.Count;
            for (int i = 0; i < n; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {
                    //_arrPara[2].Value = chekboxlist.Items[i].Value;
                    QualityTypeId += chekboxlist.Items[i].Value + ",";
                }
            }

            _arrPara[0].Value = DDCompany.SelectedValue;
            _arrPara[1].Value = 1;
            _arrPara[2].Value = DDWeaverName.SelectedValue;
            _arrPara[3].Value = QualityTypeId;
            _arrPara[4].Value = DDItemName.SelectedValue;
            _arrPara[5].Value = txtfromDate.Text;
            _arrPara[6].Value = txttodate.Text;
            _arrPara[7].Value = Session["varuserid"].ToString();
            _arrPara[8].Value = Session["varMasterCompanyIDForERP"].ToString();         

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetReportWeaverRawMaterialWithConsumption", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialWithConsumptionDetailSummary.rpt";                   
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialWithConsumptionDetail.rpt";                    
                }
                //Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialWithConsumptionDetail.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverRawMaterialWithConsumptionDetail.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);               
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void ReportRawMaterialLedger()
    {
        lblErrmsg.Text = "";
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[9];
            _arrPara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Processid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@EmpId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@QualityTypeId", SqlDbType.VarChar, 300);           
            _arrPara[4] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            _arrPara[5] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            _arrPara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@varCompanyId", SqlDbType.Int);            

            int n = chekboxlist.Items.Count;
            for (int i = 0; i < n; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {
                    //_arrPara[2].Value = chekboxlist.Items[i].Value;
                    QualityTypeId += chekboxlist.Items[i].Value + ",";
                }
            }

            _arrPara[0].Value = DDCompany.SelectedValue;
            _arrPara[1].Value = 1;
            _arrPara[2].Value = DDWeaverName.SelectedValue;
            _arrPara[3].Value = QualityTypeId;            
            _arrPara[4].Value = txtfromDate.Text;
            _arrPara[5].Value = txttodate.Text;
            _arrPara[6].Value = Session["varuserid"].ToString();
            _arrPara[7].Value = Session["varMasterCompanyIDForERP"].ToString();
            

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWeaverRawMaterialLedgerReportData", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {                
                Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialLedger.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverRawMaterialLedger.xsd";                

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);                                
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void ReportWeaverRawMaterialIssueReceive()
    {
        lblErrmsg.Text = "";
        string where = "";
        try
        {
            //if (DDCompany.SelectedIndex > 0)
            //{
            //    where = where + " And WRTM.CompanyId=" + DDCompany.SelectedValue;
            //}
            if (DDWeaverName.SelectedIndex > 0)
            {
                where = where + " And WRTM.Empid=" + DDWeaverName.SelectedValue;
            }
            if (DDQualityType.SelectedIndex > 0)
            {
                where = where + " And WRTD.QualityTypeId=" + DDQualityType.SelectedValue;
            }
            if (DDItemName.SelectedIndex > 0)
            {
                where = where + " And WRTD.ItemId=" + DDItemName.SelectedValue;
            }
            if (DDGodownName.SelectedIndex > 0)
            {
                where = where + " And WRTD.GodownId=" + DDGodownName.SelectedValue;
            }           

            SqlParameter[] _arrPara = new SqlParameter[8];
            _arrPara[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
            _arrPara[1] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            _arrPara[2] = new SqlParameter("@Processid", 1);
            _arrPara[3] = new SqlParameter("@FromDate", txtfromDate.Text);
            _arrPara[4] = new SqlParameter("@ToDate", txttodate.Text);
            _arrPara[5] = new SqlParameter("@varuserid", Session["varuserid"]);
            _arrPara[6] = new SqlParameter("@varCompanyId", Session["varMasterCompanyIDForERP"]);
            _arrPara[7] = new SqlParameter("@Where", where);            


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERRAWMATERIALISSUERECEIVE_REPORTDATA", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {                
                Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialIssueReceiveReport.rpt";               
                
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverRawMaterialIssueReceiveReport.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
                
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void ReportFinisherRawMaterialIssueReceive()
    {
        lblErrmsg.Text = "";
        string where = "";
        try
        {
            //if (DDCompany.SelectedIndex > 0)
            //{
            //    where = where + " And WRTM.CompanyId=" + DDCompany.SelectedValue;
            //}
            if (DDWeaverName.SelectedIndex > 0)
            {
                where = where + " And FRM.Empid=" + DDWeaverName.SelectedValue;
            }
            if (DDQualityType.SelectedIndex > 0)
            {
                where = where + " And FRT.QualityTypeId=" + DDQualityType.SelectedValue;
            }
            if (DDItemName.SelectedIndex > 0)
            {
                where = where + " And FRT.ItemId=" + DDItemName.SelectedValue;
            }
            if (DDGodownName.SelectedIndex > 0)
            {
                where = where + " And FRT.GodownId=" + DDGodownName.SelectedValue;
            }

            SqlParameter[] _arrPara = new SqlParameter[8];
            _arrPara[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
            _arrPara[1] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            //_arrPara[2] = new SqlParameter("@Processid", 1);
            _arrPara[2] = new SqlParameter("@FromDate", txtfromDate.Text);
            _arrPara[3] = new SqlParameter("@ToDate", txttodate.Text);
            _arrPara[4] = new SqlParameter("@varuserid", Session["varuserid"]);
            _arrPara[5] = new SqlParameter("@varCompanyId", Session["varMasterCompanyIDForERP"]);
            _arrPara[6] = new SqlParameter("@Where", where);           

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFINISHERRAWMATERIALISSUERECEIVE_REPORTDATA", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinisherRawMaterialIssueReceiveReport.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinisherRawMaterialIssueReceiveReport.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);                
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); 
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDWeaverRawMaterial.Checked == true)
        {
            ReportRawMaterialWithConsumption();
        }
        if (RDWeaverRawLedger.Checked == true)
        {
            ReportRawMaterialLedger();
        }
        if (RDWeaverRawMaterialIssueReceive.Checked == true)
        {
            ReportWeaverRawMaterialIssueReceive();
        }
        if (RDFinisherRawMaterialIssueReceive.Checked == true)
        {
            ReportFinisherRawMaterialIssueReceive();
        }
                
    }
    protected void RDWeaverRawMaterial_CheckedChanged(object sender, EventArgs e)
    {
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
            BindQualityType();
        }
    }
    protected void RDWeaverRawLedger_CheckedChanged(object sender, EventArgs e)
    {
        TRReportType.Visible = false;
        TRItemName.Visible = false;
        TRChkForSummary.Visible = false;
        TRQualityType.Visible = false;
        TRQualityTypeDropDown.Visible = false;
        TRGodown.Visible = false;
        if (RDWeaverRawLedger.Checked == true)
        {
            TRItemName.Visible = false;
            TRChkForSummary.Visible = false;
            TRReportType.Visible = false;
            TRQualityType.Visible = true;
            TRQualityTypeDropDown.Visible = false;
            TRGodown.Visible = false;
            BindQualityType();
        }
    }
    protected void RDWeaverRawMaterialIssueReceive_CheckedChanged(object sender, EventArgs e)
    {
        TRReportType.Visible = false;
        TRItemName.Visible = false;
        TRChkForSummary.Visible = false;
        TRQualityType.Visible = false;
        TRQualityTypeDropDown.Visible = false;
        TRGodown.Visible = false;
        if (RDWeaverRawMaterialIssueReceive.Checked == true)
        {
            TRItemName.Visible = true;
            TRChkForSummary.Visible = false;
            TRReportType.Visible = true;
            TRQualityType.Visible = false;
            TRQualityTypeDropDown.Visible = true;
            TRGodown.Visible = true;
            BindQualityType();
        }
    }
    protected void RDFinisherRawMaterialIssueReceive_CheckedChanged(object sender, EventArgs e)
    {
        TRReportType.Visible = false;
        TRItemName.Visible = false;
        TRChkForSummary.Visible = false;
        TRQualityType.Visible = false;
        TRQualityTypeDropDown.Visible = false;
        TRGodown.Visible = false;
        if (RDFinisherRawMaterialIssueReceive.Checked == true)
        {
            TRItemName.Visible = true;
            TRChkForSummary.Visible = false;
            TRReportType.Visible = true;
            TRQualityType.Visible = false;
            TRQualityTypeDropDown.Visible = true;
            TRGodown.Visible = true;
            BindQualityType();
        }
        
    }
}