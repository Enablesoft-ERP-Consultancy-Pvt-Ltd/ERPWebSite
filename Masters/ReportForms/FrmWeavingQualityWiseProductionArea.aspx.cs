using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmWeavingQualityWiseProductionArea : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId, CompanyName From CompanyInfo CI, Company_Authentication CA Where CI.CompanyId = CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "--Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            if (DDCompany.Items.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, "Select EI.EmpId, EI.EmpName + case When isnull(Ei.empcode, '') = '' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varcompanyId"] + @" INNER JOIN EmpProcess EP ON EP.Empid=EI.EmpId and EP.ProcessId=1 order by EmpName", true, "--Select--");
            }

            BindQualityType();
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    private void BindQualityType()
    {
        string str1 = @"Select Distinct MQT.MasterQualityTypeId,MasterQualityTypeName From MasterQualityType MQT LEFT JOIN ITEM_MASTER IM ON MQT.MasterQualityTypeId=IM.MasterQualityTypeId
                        Where MQT.MasterCompanyId=" + Session["varCompanyId"] + @" Order by MQT.MasterQualityTypeName";

        UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
    }
    protected void chekboxlist_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public string MasterQualityTypeId = "";
    protected void ReportQualityTypeWiseProductionArea()
    {
        lblErrmsg.Text = "";
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[9];
            _arrPara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Processid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@EmpId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@MasterQualityTypeId", SqlDbType.VarChar, 200);
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
                    MasterQualityTypeId += chekboxlist.Items[i].Value + ",";
                }
            }

            _arrPara[0].Value = DDCompany.SelectedValue;
            _arrPara[1].Value = 1;
            _arrPara[2].Value = DDWeaverName.SelectedValue;
            _arrPara[3].Value = MasterQualityTypeId;
            _arrPara[4].Value = txtfromDate.Text;
            _arrPara[5].Value = txttodate.Text;
            _arrPara[6].Value = Session["varuserid"].ToString();
            _arrPara[7].Value = Session["varCompanyId"].ToString();

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetReportWeaverQualityWiseProductionArea", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptWeaverQualityWiseProductionAverage.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverQualityWiseProductionAverage.xsd";

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
        ReportQualityTypeWiseProductionArea();
    }
}