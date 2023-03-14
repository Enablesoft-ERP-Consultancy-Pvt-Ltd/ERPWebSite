using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_FrmEmployeeMonthlyWorkingDays : System.Web.UI.Page
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
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @" 
                            Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varCompanyId"] + @" 
                            Order By D.DepartmentName 
                            select Designationid,Designation From HR_Designationmaster order by Designation
                            Select ShiftID, ShiftCode From HR_ShiftMaster HSM(Nolock) Order By ShiftId
                            Select Month_Id, Month_Name From MonthTable Order By Month_Id 
                            Select [Year], [Year] YEAR1 From YearData Order By [Year] Desc  ";

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


            UtilityModule.ConditionalComboFillWithDS(ref DDdepartment, ds, 2, true, "--Select Department--");
            if (DDdepartment.Items.Count > 0)
            {
                DDdepartment.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 3, true, "--Select Designation--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShift, ds, 4, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonthName, ds, 5, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 6, false, "");

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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETDATAFOREMPLOYEEWORKINGMONTHLYDAYS]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Departmentid", DDdepartment.SelectedIndex > 0 ? DDdepartment.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Designationid", DDDesignation.SelectedIndex > 0 ? DDDesignation.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ShiftID", DDShift.SelectedValue);
            cmd.Parameters.AddWithValue("@MonthID", DDMonthName.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", DDYear.SelectedValue);

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
        string Str = "", TotalDays, TotalHour, TotalMinutes;
        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            CheckBox chkitem = (CheckBox)Dgdetail.Rows[i].FindControl("chkitem");
            Label lblempid = (Label)Dgdetail.Rows[i].FindControl("lblempid");
            TextBox TxtDays = (TextBox)Dgdetail.Rows[i].FindControl("TxtDays");
            TextBox TxtHour = (TextBox)Dgdetail.Rows[i].FindControl("TxtHour");
            TextBox TxtMinutes = (TextBox)Dgdetail.Rows[i].FindControl("TxtMinutes");

            if (chkitem.Checked == true)
            {
                TotalDays = TxtDays.Text == "" ? "0" : TxtDays.Text;
                TotalHour = TxtHour.Text == "" ? "0" : TxtHour.Text;
                TotalMinutes = TxtMinutes.Text == "" ? "0" : TxtMinutes.Text;
                if (Str == "")
                {
                    Str = lblempid.Text + "|" + TotalDays + "|" + TotalHour + "|" + TotalMinutes + "~";
                }
                else
                {
                    Str = Str + lblempid.Text + "|" + TotalDays + "|" + TotalHour + "|" + TotalMinutes + "~";
                }
            }
        }
        if (Str == "")
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVEEMPLOYEEWORKINGMONTHLYDAYS", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", DDYear.SelectedValue);
            cmd.Parameters.AddWithValue("@MonthID", DDMonthName.SelectedValue);
            cmd.Parameters.AddWithValue("@DetailData", Str);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varuserid"]);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

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