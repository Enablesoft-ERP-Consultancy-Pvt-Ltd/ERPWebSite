using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Payroll_frmsalaryprocessmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock) 
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order by CI.CompanyName 
                            Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                            Order By D.DepartmentName 
                            SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE(nolock) order by Month_Id
                            SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA(nolock) order by Year 
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDmonth, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDyear, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            TxtSalaryDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DDmonth.SelectedValue = System.DateTime.Now.Month.ToString();
            DDyear.SelectedValue = System.DateTime.Now.Year.ToString();
        }
    }
    protected void btnprocess_Click(object sender, EventArgs e)
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_SALARYPROCESS]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@companyid", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Deptid", DDDepartment.SelectedIndex > 0 ? DDDepartment.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Monthid", DDmonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDmonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@WAGESCALCULATION", DDwagescalculation.SelectedValue);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            if (chkResignStatus.Checked == true)
            {
                cmd.Parameters.AddWithValue("@RESIGNSTATUS", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RESIGNSTATUS", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@SalaryDate", TxtSalaryDate.Text);

            cmd.ExecuteNonQuery();
            Tran.Commit();
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + lblmsg.Text + "')", true);
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