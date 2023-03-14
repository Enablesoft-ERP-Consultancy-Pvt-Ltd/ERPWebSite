using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Campany_FrmTodoManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "Select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " Order by CompanyName", true, "----Select Company----");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                //DDCompanyName.SelectedIndex = 1;
                CompanySelectedChange();
            }
            ViewState["SrNo"] = 0;
            TxtDueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            DDUserName.Focus();
        }
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDUserName, "Select UserId,UserName+' -- '+LoginName from NewUserDetail Where Companyid=" + Session["varCompanyId"], true, "----Select ----");
    }
    protected void DDUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[10];
            _arrpara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@PriorityLevel", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@DueDate", SqlDbType.SmallDateTime);
            _arrpara[5] = new SqlParameter("@JobStatus", SqlDbType.NVarChar, 150);
            _arrpara[6] = new SqlParameter("@WorkToDo", SqlDbType.NVarChar);
            _arrpara[7] = new SqlParameter("@Remark", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@VarUserId", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrpara[0].Value = ViewState["SrNo"];
            _arrpara[1].Value = DDCompanyName.SelectedValue;
            _arrpara[2].Value = DDUserName.SelectedValue;
            _arrpara[3].Value = DDPriorityLevel.SelectedValue;
            _arrpara[4].Value = TxtDueDate.Text;
            _arrpara[5].Value = TxtJobStatus.Text.ToUpper();
            _arrpara[6].Value = TxtWorkToDo.Text.ToUpper();
            _arrpara[7].Value = TxtRemark.Text.ToUpper();
            _arrpara[8].Value = Session["varuserid"];
            _arrpara[9].Value = Session["VarcompanyNo"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ManagmentToDo", _arrpara);
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + ex.Message + "...');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        ReferenceAfterSave();
    }
    protected void DGToDoManagment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select SrNo,CompanyId,UserID,PriorityLevel,replace(convert(varchar(11),DueDate,106), ' ','-') DueDate,JobStatus,WorkToDo,Remark From ManagmentToDo Where SrNo=" + DGToDoManagment.SelectedDataKey.Value);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDPriorityLevel.SelectedValue = Ds.Tables[0].Rows[0]["PriorityLevel"].ToString();
                TxtDueDate.Text = Ds.Tables[0].Rows[0]["DueDate"].ToString();
                TxtJobStatus.Text = Ds.Tables[0].Rows[0]["JobStatus"].ToString();
                TxtWorkToDo.Text = Ds.Tables[0].Rows[0]["WorkToDo"].ToString();
                TxtRemark.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
                ViewState["SrNo"] = Ds.Tables[0].Rows[0]["SrNo"].ToString();
                BtnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            BtnSave.Text = "Save";
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + ex.Message + "...');", true);
        }
    }
    protected void DGToDoManagment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete ManagmentToDo Where SrNo=" + DGToDoManagment.DataKeys[e.RowIndex].Value);
            Fill_Grid();
            ReferenceAfterSave();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + ex.Message + "...');", true);
        }
    }
    private void ReferenceAfterSave()
    {
        BtnSave.Text = "Save";
        TxtWorkToDo.Text = "";
        TxtRemark.Text = "";
        TxtWorkToDo.Focus();
    }
    private void Fill_Grid()
    {
        ViewState["SrNo"] = 0;
        string Str = "Select SrNo,WorkToDo,Remark,Case When PriorityLevel=0 Then 'Normal' Else Case When PriorityLevel=1 Then 'Urgent' Else 'Top Urgent' End End PriorityLevel,replace(convert(varchar(11),DueDate,106), ' ','-') DueDate,JobStatus From ManagmentToDo Where MasterCompanyId=" + Session["varCompanyId"] + " And CompanyId=" + DDCompanyName.SelectedValue + " And UserID=" + DDUserName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGToDoManagment.DataSource = Ds.Tables[0];
        DGToDoManagment.DataBind();
    }
    //protected void gvcarpetdetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";
    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";
    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    protected void DGToDoManagment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGToDoManagment, "Select$" + e.Row.RowIndex);
        }
    }
}