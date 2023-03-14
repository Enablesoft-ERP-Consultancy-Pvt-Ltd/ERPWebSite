using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmsalereport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CI.CompanyName From Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "", false, "");
            UtilityModule.ConditionalComboFill(ref DDFyear, "select year,year as year1 from session order by year desc", false, "");
            UtilityModule.ConditionalComboFill(ref DDToyear, "select year,year as year1 from session order by year desc", false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        DataSet ds;
        lblErrmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();

        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[8];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@FromMonth", SqlDbType.VarChar, 5);
            array[2] = new SqlParameter("@FromYear", SqlDbType.VarChar, 6);
            array[3] = new SqlParameter("@ToMonth", SqlDbType.VarChar, 5);
            array[4] = new SqlParameter("@Toyear", SqlDbType.VarChar, 6);
            array[5] = new SqlParameter("@UserId", SqlDbType.Int);
            array[6] = new SqlParameter("@Compare", SqlDbType.Int);
            array[7] = new SqlParameter("@CompanyName", SqlDbType.VarChar, 50);

            array[0].Value = DDCompany.SelectedValue;
            array[1].Value = DDFMonth.SelectedItem.Text;
            array[2].Value = DDFyear.SelectedItem.Text;
            array[3].Value = DDToMonth.SelectedItem.Text;
            array[4].Value = DDToyear.SelectedItem.Text;
            array[5].Value = Session["varuserid"];
            array[6].Value = chkForCompare.Checked == true ? 1 : 0;
            array[7].Value = DDCompany.SelectedItem.Text;

            ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_forSaleReport", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkForCompare.Checked == false)
                {
                    Session["rptFileName"] = "~\\Reports\\RptSaleAmount.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptSaleAmount.xsd";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptSaleAmountCompare.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptSaleAmountCompare.xsd";
                }
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No records found');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}