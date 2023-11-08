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
public partial class Masters_Process_FrmProcessRecRateUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DataSet ds = new DataSet();

            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(Nolock)
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @" 
                    Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                    From Process_Name_Master PNM(Nolock) 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserId"] + @" 
                    Where PNM.MasterCompanyid = " + Session["varCompanyId"] + @" 
                    Order By PNM.PROCESS_NAME ";

            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");

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
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "-- Select --");
        }
    }

    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DDProcessNameSelectedIndex();
    }

    protected void DDProcessNameSelectedIndex()
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "42" || Session["VarCompanyNo"].ToString() == "46")
        {
            str = @"Select Distinct EPR.EmpID, EI.EmpName + Case When EI.EmpCode <> '' Then ' / ' + EI.EmpCode End EmployeeName 
            From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a
            JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" b on b.IssueOrderID = a.IssueOrderID 
            JOIN Employee_ProcessOrderNo EPR ON EPR.IssueOrderID = a.IssueOrderID And EPR.IssueDetailID = b.Issue_Detail_ID And EPR.ProcessId = " + DDProcessName.SelectedValue + @" 
            JOIN Empinfo EI ON EI.EMpID = EPR.Empid 
            Where isnull(a.FolioStatus,0)=0 and a.Companyid = " + DDCompanyName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" 
            Order By EmployeeName ";
        }
        else
        {
             str = @"Select Distinct EPR.EmpID, EI.EmpName + Case When EI.EmpCode <> '' Then ' / ' + EI.EmpCode End EmployeeName 
            From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a
            JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" b on b.IssueOrderID = a.IssueOrderID 
            JOIN Employee_ProcessOrderNo EPR ON EPR.IssueOrderID = a.IssueOrderID And EPR.IssueDetailID = b.Issue_Detail_ID And EPR.ProcessId = " + DDProcessName.SelectedValue + @" 
            JOIN Empinfo EI ON EI.EMpID = EPR.Empid 
            Where a.AssignDate >= GetDate()-90 And a.Companyid = " + DDCompanyName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" 
            Order By EmployeeName ";
        }

        

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "--Select--");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "42" || Session["VarCompanyNo"].ToString() == "46")
        {
            str = @"Select Distinct a.IssueOrderId, a.ChallanNo 
            From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a
            JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" b on b.IssueOrderId = a.IssueOrderId 
            JOIN Employee_ProcessOrderNo EPO ON EPO.IssueOrderId = a.IssueOrderId And EPO.IssueDetailId = b.Issue_Detail_Id And 
            EPO.ProcessId = " + DDProcessName.SelectedValue + " And EPO.EmpID = " + DDEmployeeName.SelectedValue + @" 
            Where isnull(a.FolioStatus,0)=0 And a.Companyid = " + DDCompanyName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" 
            Order By a.IssueOrderId Desc ";
        }
        else
        {
             str = @"Select Distinct a.IssueOrderId, a.ChallanNo 
            From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a
            JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" b on b.IssueOrderId = a.IssueOrderId 
            JOIN Employee_ProcessOrderNo EPO ON EPO.IssueOrderId = a.IssueOrderId And EPO.IssueDetailId = b.Issue_Detail_Id And 
            EPO.ProcessId = " + DDProcessName.SelectedValue + " And EPO.EmpID = " + DDEmployeeName.SelectedValue + @" 
            Where a.AssignDate >= GetDate()-90 And a.Companyid = " + DDCompanyName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" 
            Order By a.IssueOrderId Desc ";
        }
       

        DataSet ds = SqlHelper.ExecuteDataset(str);
        
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "--Select--");
    }

    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }

    private void Fill_Grid()
    {
        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();
    }

    private DataSet GetDetail()
    {
        DataSet DS = null;
        string str = "";

        str = @"Select b.Item_Finished_Id, a.CalType, VF.Item_Name Item,
            VF.QualityName + ' , ' + VF.DesignName + ' , ' + VF.ColorName + ' , ' + VF.ShapeName + ' , ' + 
            Case When a.UnitId = 1 Then VF.SizeMtr Else Case When a.UnitId = 6 Then VF.SizeInch Else VF.SizeFt End End [Description], 
            Round(Sum(Qty*Area), 4) Area, Sum(Qty) Qty, Rate, IsNull(Bonus, 0) BonusRate, Round(Sum(Amount), 2) Amount 
            From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a
            JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" b on b.IssueOrderId = a.IssueOrderId 
            JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id 
            Where a.IssueOrderId = " + DDIssueNo.SelectedValue + @" 
            Group By b.Item_Finished_Id, a.CalType, VF.Item_Name, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
            a.UnitId, VF.SizeMtr, VF.SizeInch, VF.SizeFt, b.Rate, Bonus 
            Order By VF.Item_Name, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, VF.SizeFt ";

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (DS.Tables[0].Rows.Count > 0)
        {
            DDCalType.SelectedValue = DS.Tables[0].Rows[0]["CalType"].ToString();
        }
        return DS;
    }

    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        string Str = "";
        for (int i = 0; i < DGOrderdetail.Rows.Count; i++)
        {
            Label lblRate = (Label)DGOrderdetail.Rows[i].FindControl("lblRate");
            TextBox TxtRate = (TextBox)DGOrderdetail.Rows[i].FindControl("TxtRate");

            Label lblBonusRate = (Label)DGOrderdetail.Rows[i].FindControl("lblBonusRate");
            TextBox TxtBonusRate = (TextBox)DGOrderdetail.Rows[i].FindControl("TxtBonusRate");

            if (Convert.ToDecimal(lblRate.Text) != Convert.ToDecimal(TxtRate.Text) || Convert.ToDecimal(lblBonusRate.Text) != Convert.ToDecimal(TxtBonusRate.Text))
            {
                Label lblItem_Finished_Id = (Label)DGOrderdetail.Rows[i].FindControl("lblItem_Finished_Id");
                Str = Str + lblItem_Finished_Id.Text + "|" + TxtRate.Text + "|" + TxtBonusRate.Text + "~";
            }
        }
        if (Str != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@IssueOrderID", DDIssueNo.SelectedValue);                
                param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
                param[2] = new SqlParameter("@DetailData", Str);
                param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                param[4] = new SqlParameter("@Userid", Session["varuserid"]);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateProcessRate", param);
                if (param[3].Value.ToString() != "")
                {
                    Tran.Rollback();
                    LblErrorMessage.Text = param[3].Value.ToString();
                }
                else
                {
                    Tran.Commit();
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Data saved successfully...!!!');", true);
                    DDIssueNo.SelectedIndex = 0;
                    DGOrderdetail.DataSource = null;
                    DGOrderdetail.DataBind();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}