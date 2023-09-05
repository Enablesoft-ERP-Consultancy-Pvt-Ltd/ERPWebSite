using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmPNMChampoProcessProgramDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            String Str = @"select CI.CompanyId,CompanyName 
            From CompanyInfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" 
            order by CompanyName
            Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" And Process_Name_ID = 5
            Select CustomerID, CustomerCode From CustomerInfo CI(Nolock) Order By CustomerCode ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddcompany, ds, 0, true, "--Plz Select--");

            if (ddcompany.Items.FindByValue(Session["CurrentWorkingCompanyID"].ToString()) != null)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, ds, 2, true, "--Select--");

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@CompanyID", ddcompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", ddprocess.SelectedValue);
        if (ddcustomer.SelectedIndex > 0)
        {
            param[2] = new SqlParameter("@CustomerID", ddcustomer.SelectedValue);
        }
        else
        {
            param[2] = new SqlParameter("@CustomerID", 0);
        }
        param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[4] = new SqlParameter("@ToDate", txttodate.Text);
        param[5] = new SqlParameter("@Type", 1);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetChampoProcessProgramDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGPPDetail.DataSource = ds.Tables[0];
            DGPPDetail.DataBind();
        }
    }

    protected void lnkbtnToOpenIndentDetail_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        GridViewRow grv = lnk.NamingContainer as GridViewRow;
        int lblPPID = Convert.ToInt16(((Label)DGPPDetail.Rows[grv.RowIndex].FindControl("lblPPID")).Text);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_Processprogramreport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@PPID", lblPPID);
        cmd.Parameters.AddWithValue("@Processid", ddprocess.SelectedValue);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {   
            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptdyeingprogramnew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else 
        { 
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Record Found!');", true); 
        }
    }
}