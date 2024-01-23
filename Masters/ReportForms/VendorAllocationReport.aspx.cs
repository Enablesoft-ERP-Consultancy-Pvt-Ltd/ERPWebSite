using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class VendorAllocationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            UtilityModule.ConditionalComboFill(ref ddlCompanyname, "select Companyid, CompanyName from companyinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by CompanyName ", true, "-------SELECT---------");
            if (ddlCompanyname.Items.Count > 0)
            {
                ddlCompanyname.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddlCompanyname.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref ddvendorname, "select EmpId, EmpName from empinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by EmpName ", true, "-------SELECT---------");
            UtilityModule.ConditionalComboFill(ref DDMonth, "select Month_id, Month_Name from MonthTable ", true, "----select----");
            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from session order by Year desc", true, "----select----");

            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlParameter[] para = new SqlParameter[4];
        para[0] = new SqlParameter("@vendorid", SqlDbType.Int);
        para[1] = new SqlParameter("@Month_id", SqlDbType.Int);
        para[2] = new SqlParameter("@Year", SqlDbType.VarChar, 6);
        para[3] = new SqlParameter("@userid", SqlDbType.Int);


        para[0].Value = ddvendorname.SelectedIndex < 0 ? "0" : ddvendorname.SelectedValue;
        para[1].Value = DDMonth.SelectedValue.ToString();
        para[2].Value = DDyear.SelectedItem.Text;
        para[3].Value = Session["varuserid"].ToString();


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_vendorallocationreports", para);

        //        string str = @"select '" + ddlCompanyname.SelectedItem.Text +@"' as company,empid,empname,sum(capacity) as capacity,sum(issqty) as issueqty,
        //        sum(recqty) as recqty from v_vendorallocation ";
        //        if (ddvendorname.SelectedIndex > 0)
        //        {
        //            str = str + " where empid=" + ddvendorname.SelectedValue;
        //        }
        //        str = str + " group by empid,empname";

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptvendorallocationReport.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptvendorallocationReport.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }

        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found...');", true);
        }
    }
}