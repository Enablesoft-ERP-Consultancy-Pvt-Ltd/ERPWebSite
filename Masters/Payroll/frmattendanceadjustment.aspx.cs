using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmattendanceadjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                            Order By D.DepartmentName 
                            select Designationid,Designation From HR_Designationmaster order by Designation
                            Select ShiftID, ShiftCode From HR_ShiftMaster HSM(Nolock) Order By ShiftId
                            Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock) 
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order by CI.CompanyName 
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDdepartment, ds, 0, true, "--Select Department--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 1, true, "--Select Designation--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShift, ds, 2, false, "");

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 3, true, "Select Comp Name");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");

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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETDATAFORATTENDANCEADJUSTMENT]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@adjustType", Rdintime.Checked == true ? "1" : (Rdouttime.Checked == true ? "2" : "0"));
            cmd.Parameters.AddWithValue("@timebefore_after", txttimebefore_after.Text);
            cmd.Parameters.AddWithValue("@Departmentid", DDdepartment.SelectedIndex > 0 ? DDdepartment.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Designationid", DDDesignation.SelectedIndex > 0 ? DDDesignation.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ShiftID", DDShift.SelectedValue);
            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

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
        dt.Columns.Add("updatetime", typeof(string));
        dt.Columns.Add("In_Outsecond", typeof(string));

        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            CheckBox chkitem = (CheckBox)Dgdetail.Rows[i].FindControl("chkitem");
            Label lblempid = (Label)Dgdetail.Rows[i].FindControl("lblempid");
            Label lblempcode = (Label)Dgdetail.Rows[i].FindControl("lblempcode");
            Label lbldate = (Label)Dgdetail.Rows[i].FindControl("lbldate");
            Label lblin_outsecond = (Label)Dgdetail.Rows[i].FindControl("lblin_outsecond");

            if (chkitem.Checked == true)
            {
                DataRow dr = dt.NewRow();
                dr["empid"] = lblempid.Text;
                dr["empcode"] = lblempcode.Text;
                dr["Manualdate"] = lbldate.Text;
                dr["updatetime"] = txttimeset.Text;
                dr["in_outsecond"] = lblin_outsecond.Text;
                dt.Rows.Add(dr);
            }
        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altval", "alert('Please select atleast one check box to save data.')", true);
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVEADJUSTMENTATTENDANCE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@adjusttype", Rdintime.Checked == true ? "1" : (Rdouttime.Checked == true ? "2" : "0"));
            //cmd.Parameters.AddWithValue("@timedifference",txtminutes.Text);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@ShiftID", DDShift.SelectedValue);

            cmd.ExecuteNonQuery();
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "')", true);
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
            btngetdata_Click(sender, new EventArgs());
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