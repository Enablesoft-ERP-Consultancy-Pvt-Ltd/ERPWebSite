using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmPNMChampoIndentDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@CompanyID", 1);
        param[1] = new SqlParameter("@ProcessID", 5);
        param[2] = new SqlParameter("@EmpID", 2440);
        param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[4] = new SqlParameter("@ToDate", txttodate.Text);
        param[5] = new SqlParameter("@Type", 1);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetChampoIndentDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIndentDetail.DataSource = ds.Tables[0];
            DGIndentDetail.DataBind();
        }
    }

    protected void lnkbtnToOpenIndentDetail_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        GridViewRow grv = lnk.NamingContainer as GridViewRow;
        int lblIndentID = Convert.ToInt16(((Label)DGIndentDetail.Rows[grv.RowIndex].FindControl("lblIndentID")).Text);

        string qry = @"select * From ExportERP.DBO.V_indentreportForCarpetCompany where indentid=" + lblIndentID;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptindentreportnew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptindentreportnew.xsd";
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
}