using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Loom_FrmIssueOrderNoConsumptionExtraPercentage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA 
                        Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName 
                        Select * From PROCESS_NAME_MASTER Where PROCESS_NAME_ID Not in (5, 9) Order By PROCESS_NAME_ID ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, false, "");
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedValue = "1";
                FillEmployeeName();
            }
            TxtFolioNo.Focus();
        }        
    }

    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        DDEmployeeName.SelectedIndex = 0;
        FillFolioNo();
        TxtExtraPercentageQty.Text = "";
    }
    protected void TxtFolioNo_TextChanged(object sender, EventArgs e)
    {
        string Status = "Pending";
        if (chkcomplete.Checked == true)
        {
            Status = "Complete";
        }
        string str = @"Select a.CompanyID, " + DDProcessName.SelectedValue + @" ProcessID, a.EmpID, a.IssueOrderID 
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                Where a.Status = '" + Status + "' And a.CompanyID = " + DDCompanyName.SelectedValue + " And a.IssueOrderID = '" + TxtFolioNo.Text + @"' 
                UNION
                Select a.CompanyID, " + DDProcessName.SelectedValue + @" ProcessID, EPO.EmpID, a.IssueOrderID 
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = " + DDProcessName.SelectedValue + @" 
                Where a.Status = '" + Status + "' And a.CompanyID = " + DDCompanyName.SelectedValue + " And a.IssueOrderID = '" + TxtFolioNo.Text + @"' ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDEmployeeName.SelectedValue = ds.Tables[0].Rows[0]["EmpID"].ToString();
            FillFolioNo();
            DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderID"].ToString();
            FolioNoSelectedChanged();
            TxtExtraPercentageQty.Focus();
        }
        else
        {
            TxtFolioNo.Text = "";
            TxtFolioNo.Focus();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployeeName();
    }
    protected void FillEmployeeName()
    {
        string str = @"Select Distinct a.EmpID, EI.EmpName + ' / ' + EI.EmpCode EmpCode
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID 
                Where CompanyID = " + DDCompanyName.SelectedValue + @" 
                UNION
                Select Distinct a.EmpID, EI.EmpName + ' / ' + EI.EmpCode EmpCode
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = " + DDProcessName.SelectedValue + @" 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = EPO.EmpID 
                Where CompanyID = " + DDCompanyName.SelectedValue + @" 
                Order BY EmpCode";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "Select Employee Name");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
    protected void FillFolioNo()
    {
        string Status = "Pending";
        if(chkcomplete.Checked ==true)
        {
            Status = "Complete";
        }
        string str = @"Select Distinct a.IssueOrderID, a.IssueOrderID IssueOrderID1
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID 
                Where a.Status = '" + Status + "' And a.CompanyID = " + DDCompanyName.SelectedValue + " And a.EmpID = " + DDEmployeeName.SelectedValue + @" 
                UNION
                Select Distinct a.IssueOrderID, a.IssueOrderID IssueOrderID1
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock)
                JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = " + DDProcessName.SelectedValue + @" 
                        And EPO.EmpID = " + DDEmployeeName.SelectedValue + @" 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = EPO.EmpID 
                Where a.Status = '" + Status + "' And a.CompanyID = " + DDCompanyName.SelectedValue + @" 
                Order BY IssueOrderID1"; 

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDFolioNo, ds, 0, true, "Select FolioNo");
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FolioNoSelectedChanged();
    }
    protected void FolioNoSelectedChanged()
    {
        string str = @"Select CompanyID, ProcessID, IssueOrderID, ExtraPercentage 
                From ProcessConsumptionExtraPercentage (Nolock) 
                Where CompanyID = " + DDCompanyName.SelectedValue + @" 
                And ProcessID = " + DDProcessName.SelectedValue + @" And IssueOrderID = " + DDFolioNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtExtraPercentageQty.Text = ds.Tables[0].Rows[0]["ExtraPercentage"].ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
            param[3] = new SqlParameter("@ExtraPercentageQty", TxtExtraPercentageQty.Text == "" ? "0" : TxtExtraPercentageQty.Text);
            param[4] = new SqlParameter("@UserID", Session["varuserid"]);
            param[5] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UpdateProcessConsumptionExtraPercentage", param);
            Tran.Commit();
            TxtExtraPercentageQty.Text = "";
            DDFolioNo.SelectedIndex = 0;
            TxtFolioNo.Text = "";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Data successfully saved');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + ex.Message + "');", true);
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}