using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_ReportForms_frmjobpayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select PROCESS_NAME_ID,PROCESS_NAME From Process_name_Master Where ProcessType=1 and MasterCompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME
                        select UnitsId,UnitName From Units Where MasterCompanyId=" + Session["varcompanyid"] + @" order by UnitName
                        select Month_Id,Month_Name from MonthTable order by Month_Id
                        select Year,Year as year1 From YearData order by Year1";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDJob, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 3, true, "--Plz Select--");

            DDMonth.SelectedValue = System.DateTime.Now.Month.ToString();
            DDYear.SelectedValue = System.DateTime.Now.Year.ToString();
            ds.Dispose();
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("Pro_Jobpayment", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            //SqlParameter[] param = new SqlParameter[6];
            //param[0] = new SqlParameter("@Processid", DDJob.SelectedIndex > 0 ? DDJob.SelectedValue : "0");
            //param[1] = new SqlParameter("@Unitid", DDUnit.SelectedIndex > 0 ? DDUnit.SelectedValue : "0");
            //param[2] = new SqlParameter("@Monthid", DDMonth.SelectedValue);
            //param[3] = new SqlParameter("@Year", DDYear.SelectedValue);
            //param[4] = new SqlParameter("@userid", Session["varuserid"]);
            //param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            //param[5].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@Processid", DDJob.SelectedIndex > 0 ? DDJob.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Unitid", DDUnit.SelectedIndex > 0 ? DDUnit.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Year", DDYear.SelectedValue);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            SqlParameter param = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);
            //************
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Jobpayment", param);
            cmd.ExecuteNonQuery();
            lblmsg.Text = param.Value.ToString();
            Tran.Commit();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}