using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;


public partial class Masters_ReportForms_FrmWeaverRawMaterialIssueDetailReportCI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @" ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
           
            ds.Dispose();
        }
    }
    private void CarpetInternationalFormatReport()
    {
        SqlParameter[] _array = new SqlParameter[6];
        //_array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[0] = new SqlParameter("@ChallanNo", SqlDbType.VarChar,50);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);
        _array[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _array[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _array[5] = new SqlParameter("@UserId", SqlDbType.Int);

        _array[0].Value = txtChallanNo.Text;
        _array[1].Value = 1;
        _array[2].Value = 0; //For Issue
        _array[3].Value = DDCompany.SelectedValue;
        _array[4].Value = Session["varMasterCompanyIDForERP"].ToString();
        _array[5].Value = Session["VarUserId"].ToString();

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverRawMaterialIssuedDetail_ChallanNoWiseCI", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptWeaverMaterialIssueDetailChallanNoWiseCI.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeaverMaterialIssueDetailChallanNoWiseCI.xsd";

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
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        CarpetInternationalFormatReport();
    }
   
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
   
    
}