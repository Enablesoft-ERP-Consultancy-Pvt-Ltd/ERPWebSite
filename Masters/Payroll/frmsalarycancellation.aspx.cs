using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Masters_Payroll_frmsalaryprocessmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock) 
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order by CI.CompanyName 
                            Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varCompanyId"] + @" 
                            Order By D.DepartmentName 
                            SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE(Nolock) order by Month_Id 
                            SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA(Nolock) order by Year 
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, true, "--ALL--");
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
            DDmonth.SelectedValue = System.DateTime.Now.Month.ToString();
            DDyear.SelectedValue = System.DateTime.Now.Year.ToString();
        }
    }
    protected void btnprocess_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        lblmsg.Text = "";
    }
    protected void Salarycancellation()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_SALARYPROCESSCANCELLATION", con, Tran);
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
            cmd.Parameters.AddWithValue("@Wagescalculation", DDwagescalculation.SelectedValue);
            cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

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
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarSALCANCEL_PWD == txtpwd.Text)
        {
            Salarycancellation();
            Popup(false);
        }
        else
        {
            lblmsg.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
}