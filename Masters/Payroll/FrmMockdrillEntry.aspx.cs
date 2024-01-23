using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_FrmMockdrillEntry : System.Web.UI.Page
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
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
                            Select [Year], [Session] From SESSION(Nolock)  Order By [Year] Desc ";

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
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 2, false, "");
            FillGrid();
            txtfromdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVEMOCKDRILL", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", DDYear.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpCode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Date", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "')", true);
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
            TxtRemark.Text = "";
            FillGrid();
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
    protected void DDYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void FillGrid()
    {
        string str = @"Select a.ID, IsNull(EI.EmpCode, '') EmpCode, IsNull(EI.EmpName, '') EmpName, REPLACE(CONVERT(NVARCHAR(11), a.DATE, 106), ' ', '-') [Date], Remark 
            From hr_Mockdrill a(Nolock) 
            LEFT JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID 
            Where a.CompanyID = " + DDCompanyName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And YearID = " + DDYear.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        Dgdetail.DataSource = ds.Tables[0];
        Dgdetail.DataBind();
    }
    protected void Dgdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPrtID = Convert.ToInt32(Dgdetail.DataKeys[e.RowIndex].Value);
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete hr_Mockdrill Where ID = " + VarPrtID);
            lblmsg.Text = "Row Item Deleted successfully.";

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        FillGrid();
    }
}