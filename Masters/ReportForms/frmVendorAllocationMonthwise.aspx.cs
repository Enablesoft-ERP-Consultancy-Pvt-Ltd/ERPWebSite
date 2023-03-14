using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class frmVendorAllocationMonthwise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["vacompanyid"] == null)
        //{
        //    Response.Redirect("~/Login.aspx");
        //}
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDVendor, "select empid, empname from empinfo where MasterCompanyId=" + Session["varCompanyId"] + "", true, "---select---");
            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from session order by Year desc", true, "----select----");
            UtilityModule.ConditonalChkBoxListFill(ref checkmonth, "select month_id,Month_Name from Monthtable");
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
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@vendorid", SqlDbType.Int);
        para[1] = new SqlParameter("@Month_id", SqlDbType.VarChar, 100);
        para[2] = new SqlParameter("@Year", SqlDbType.VarChar, 6);

        para[0].Value = DDVendor.SelectedIndex < 0 ? "0" : DDVendor.SelectedValue;
        string str = null;
        for (int i = 0; i < checkmonth.Items.Count; i++)
        {
            if (checkmonth.Items[i].Selected)
            {
                str = str == null ? checkmonth.Items[i].Value : str + "," + checkmonth.Items[i].Value;
            }
        }
        para[1].Value = str;
        para[2].Value = DDyear.SelectedItem.Text;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_monthwisecapacity", para);

        //        string str = @"select '" + ddlCompanyname.SelectedItem.Text +@"' as company,empid,empname,sum(capacity) as capacity,sum(issqty) as issueqty,
        //        sum(recqty) as recqty from v_vendorallocation ";
        //        if (ddvendorname.SelectedIndex > 0)
        //        {
        //            str = str + " where empid=" + ddvendorname.SelectedValue;
        //        }
        //        str = str + " group by empid,empname";

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptvendorallocationMonthWise.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptvendorallocationMonthWise.rpt.xsd";
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