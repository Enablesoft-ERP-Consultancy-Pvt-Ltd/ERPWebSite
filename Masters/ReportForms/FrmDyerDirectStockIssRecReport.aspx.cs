using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmDyerDirectStockIssRecReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "Select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "--Select--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                UtilityModule.ConditionalComboFill(ref DDDyerName, "Select EI.EmpId,EI.EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId Where EP.ProcessId=5 Order by EI.EmpName", true, "--Plz Select--");
            }

            BindItemName();
            BindShadeColorName();
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=1 and IM.MasterCompanyid=" + Session["varCompanyId"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindQualityName()
    {
        if (DDItemName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by QualityName", true, "ALL");
        }
        else
        {
            DDQuality.Items.Clear();
            //UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varCompanyid"] + "  order by QualityName", true, "ALL");
        }
        //DDQuality_SelectedIndexChanged(sender, e);

    }
    private void BindShadeColorName()
    {
        UtilityModule.ConditionalComboFill(ref DDShadeColorName, "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY SHADECOLORNAME", true, "--Plz Select--");
                
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQualityName();
    }
    private void BindChallanNo()
    {
        if (DDReportType.SelectedValue == "0")
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select Distinct Id,ChallanNo from DyerDirectStockIssueMaster DDSIM JOIN DyerDirectStockIssueDetail DDSID ON DDSIM.ID=DDSID.masterid Where MasterCompanyId=" + Session["varCompanyid"] + @" Order by ChallanNo ", true, "--Plz Select--");
        }
        else if (DDReportType.SelectedValue == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select Distinct Id,ChallanNo from DyerDirectStockReceiveMaster DDSRM JOIN DyerDirectStockReceiveDetail DDSRD ON DDSRM.ID=DDSRD.masterid  Where MasterCompanyId=" + Session["varCompanyid"] + @" Order by ChallanNo ", true, "--Plz Select--");
        }
    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDyerName.SelectedIndex > 0)
        {
            BindChallanNo();
        }
        else
        {
            DDChallanNo.Items.Clear();
        }
    }
    protected void DDReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDChallanNo.Items.Clear();
    }
    protected void ReportDyerDirectStockIssueReceiveDetail()
    {
        lblErrmsg.Text = "";
        string where = "";
        try
        {
            //if (DDCompany.SelectedIndex > 0)
            //{
            //    where = where + " And WRTM.CompanyId=" + DDCompany.SelectedValue;
            //}
            if (DDDyerName.SelectedIndex > 0)
            {
                where = where + " And EmpId=" + DDDyerName.SelectedValue;
            }
            if (DDChallanNo.SelectedIndex > 0)
            {
                where = where + " And ChallanNo=" + DDChallanNo.SelectedValue;
            }
            if (DDItemName.SelectedIndex > 0)
            {
                where = where + " And VF.Item_Id=" + DDItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                where = where + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDShadeColorName.SelectedIndex > 0)
            {
                where = where + " And VF.ShadecolorId=" + DDShadeColorName.SelectedValue;
            }
            //if (Tdselectdate.Visible == true)
            //{
            //    where = where + " And DDSIM.Date>='" + txtfromDate.Text + "' and DDSIM.Date<='" + txttodate.Text + "'";               
            //}
            

            SqlParameter[] _arrPara = new SqlParameter[7];
            _arrPara[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
            _arrPara[1] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);           
            _arrPara[2] = new SqlParameter("@FromDate", txtfromDate.Text);
            _arrPara[3] = new SqlParameter("@ToDate", txttodate.Text);
            _arrPara[4] = new SqlParameter("@varuserid", Session["varuserid"]);
            _arrPara[5] = new SqlParameter("@varCompanyId", Session["varCompanyId"]);
            _arrPara[6] = new SqlParameter("@Where", where);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYER_DIRECT_STOCK_ISSUERECEIVE_REPORTDATA", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptDyerDirectStockSummaryReport.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptDyerDirectStockDetailReport.rpt";
                }

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptDyerDirectStockDetailReport.xsd";

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
        ReportDyerDirectStockIssueReceiveDetail();        
    }
}