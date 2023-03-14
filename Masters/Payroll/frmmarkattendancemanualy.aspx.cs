using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmmarkattendancemanualy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock) 
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order by CI.CompanyName 
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "Select Comp Name");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
        }

    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_GETDATAFORMARKATTENDANCEMANUALLY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@COMPANYID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BRANCHID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            Dgdetail.DataSource = ds.Tables[0];
            Dgdetail.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //*************Datatable
        DataTable dt = new DataTable();
        dt.Columns.Add("empid", typeof(int));
        dt.Columns.Add("empcode", typeof(string));
        dt.Columns.Add("Manualdate", typeof(DateTime));
        dt.Columns.Add("intime", typeof(string));
        dt.Columns.Add("outtime", typeof(string));
        dt.Columns.Add("Insecond", typeof(string));
        dt.Columns.Add("Outsecond", typeof(string));
        dt.Columns.Add("shiftintime", typeof(string));
        dt.Columns.Add("shiftouttime", typeof(string));

        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            Label lblempid = (Label)Dgdetail.Rows[i].FindControl("lblempid");
            Label lblempcode = (Label)Dgdetail.Rows[i].FindControl("lblempcode");
            Label lbldate = (Label)Dgdetail.Rows[i].FindControl("lbldate");
            TextBox txtintime = (TextBox)Dgdetail.Rows[i].FindControl("txtintime");
            TextBox txtouttime = (TextBox)Dgdetail.Rows[i].FindControl("txtouttime");
            Label lblinsecond = (Label)Dgdetail.Rows[i].FindControl("lblinsecond");
            Label lbloutsecond = (Label)Dgdetail.Rows[i].FindControl("lbloutsecond");
            Label lblshiftintime = (Label)Dgdetail.Rows[i].FindControl("lblshiftintime");
            Label lblshiftouttime = (Label)Dgdetail.Rows[i].FindControl("lblshiftouttime");

            DataRow dr = dt.NewRow();
            dr["empid"] = lblempid.Text;
            dr["empcode"] = lblempcode.Text;
            dr["Manualdate"] = lbldate.Text;
            dr["intime"] = txtintime.Text;
            dr["outtime"] = txtouttime.Text;
            dr["Insecond"] = lblinsecond.Text;
            dr["Outsecond"] = lbloutsecond.Text;
            dr["shiftintime"] = lblshiftintime.Text;
            dr["shiftouttime"] = lblshiftouttime.Text;

            dt.Rows.Add(dr);

        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altval", "alert('No Data fill to Save in data grid.')", true);
            return;
        }
        //**************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVEMANUALATTENDANCE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "')", true);
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
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