using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Loom_FrmDefectedPcIssueStockNoWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId, CI.CompanyName 
            From Companyinfo CI(Nolock)
            JOIN Company_Authentication CA (Nolock) ON CA.CompanyID = CI.CompanyID And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CI.CompanyName 
            Select ID, BranchName 
            From BRANCHMASTER BM(nolock) 
            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @" 
            Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From PROCESS_NAME_MASTER PNM(nolock) 
            JOIN UserRightsProcess URP(nolock) ON URP.ProcessID = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
            WHere PNM.ProcessType = 1 And PNM.PROCESS_NAME_ID = 1 Order By PNM.PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, false, "");

            txtRecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtIssuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {

    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {

        }
    }
    protected void DGRecDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGRecDetail.EditIndex = e.NewEditIndex;
        //FillRecDetails();
        DGRecDetail.Rows[e.NewEditIndex].FindControl("txtweight").Focus();
    }
    protected void DGRecDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGRecDetail.EditIndex = -1;
        //FillRecDetails();
    }
    protected void Savedetail(Object sender = null)
    {

    }
    protected void ShowData_Click(object sender, EventArgs e)
    {
        fillGrid();
        //FillRecDetails();
    }
    protected void fillGrid()
    {
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
        param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
        param[3] = new SqlParameter("@ReceiveDate", txtRecdate.Text);
        param[4] = new SqlParameter("@UserID", Session["varuserId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillPcIssueStockNoWise", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIssueDetail.DataSource = ds.Tables[0];
            DGIssueDetail.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Receord Found');", true);
            return;
        }
    }
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DGIssueDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}