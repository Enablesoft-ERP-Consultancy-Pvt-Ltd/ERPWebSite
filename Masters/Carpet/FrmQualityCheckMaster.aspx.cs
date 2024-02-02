using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_FrmQualityCheckMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)  
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            BtnAddNewParameter.Visible = false;
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "SELECT CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CATEGORY_NAME", true, "---Select---");
            UtilityModule.ConditionalComboFill(ref ddProcessName, "SELECT Process_Name_ID,Process_Name from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Process_Name", true, "---Select---");
            //logo();
        }
    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddItemName, "SELECT ITEM_ID, ITEM_NAME froM ITEM_MASTER where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "---Select --");
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCategoryName.SelectedIndex > 0 && ddProcessName.SelectedIndex > 0)
        {
            BtnAddNewParameter.Visible = true;
            FillGrid();
            //Fill_grid();
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + ddItemName.SelectedValue, true, "--Select--");
        Fill_grid();
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_grid();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[6];

            arr[0] = new SqlParameter("@ID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ItemID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ParaID",SqlDbType.Int);
            arr[3] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            arr[5] = new SqlParameter("@QualityID", SqlDbType.Int);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = 0;
            arr[1].Value = ddItemName.SelectedValue;
            arr[3].Value = Session["varuserid"].ToString();
            arr[4].Value = Session["varMasterCompanyIDForERP"].ToString();
            if (dquality.Items.Count == 0)
            {
                arr[5].Value = 0;
            }
            else
            {
                arr[5].Value = dquality.SelectedValue;
            }
            for (int i = 0; i < DGShowData.Rows.Count; i++)
            {
                if (((CheckBox)DGShowData.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    arr[0].Value = 0;
                    arr[2].Value = DGShowData.DataKeys[i].Value;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_QCMaster]", arr);
                }
            }
            Tran.Commit();
            Fill_grid();
            AfterSaveClear();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmQualityCheckMaster.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Fill_grid()
    {
        string Str = @"Select QCM.ID Sr_No, ITEM_NAME ItemName, QCP.SrNo, ParaName ParameterName, SName ShortName, Specification Specification, Method Method, 
            case when QCM.Enable_Disable = 1 Then 'Disable' Else 'Enable' End Status, QCM.Enable_Disable, PT.ParameterName ParameterType 
            From QCParameter QCP(Nolock) 
            JOIN QCMaster QCM(Nolock) ON QCM.ParaID = QCP.ParaID 
            JOIN Item_Master IM(Nolock) ON IM.Item_Id = QCM.ItemId 
            JOIN ParameterType PT(Nolock) ON PT.ID = QCP.ParameterTypeID 
            Where QCP.CategoryID = " + ddCategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And QCP.ProcessId = " + ddProcessName.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Str = Str + " And QCM.ItemID = " + ddItemName.SelectedValue;
        }
        if (dquality.SelectedIndex > 0)
        {
            Str = Str + " And QCM.Qualityid = " + dquality.SelectedValue;
        }
        if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16)
        {
            Str = Str + " And IsNull(QCM.QUALITYID, 0) = 0";
        }
        Str = Str + " Order By QCP.SrNo";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DG.DataSource = Ds;
        DG.DataBind();
    }
    private void AfterSaveClear()
    {
        ddItemName.SelectedIndex = 0;
        ddItemName.Focus();
        FillGrid();
    }
    protected void BtnAddParameterReferce_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {

    }
    private void FillGrid()
    {
        string Str = @"Select a.ParaID Sr_No, a.SrNo, a.ParaName ParameterName, a.SName ShortName, a.Specification Specification, 
            a.Method Method, a.ParaID, PT.ParameterName ParameterType 
            From QCParameter a(nolock) 
            JOIN ParameterType PT (nolock) ON PT.ID = a.ParameterTypeID 
            Where a.CategoryID=" + ddCategoryName.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And a.ProcessId=" + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By a.SrNo";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGShowData.DataSource = Ds;
        DGShowData.DataBind();
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarParaID = Convert.ToInt32(DG.DataKeys[e.RowIndex].Value);
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete QCMaster Where Id=" + VarParaID);
            Tran.Commit();
            Fill_grid();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Successfully Deleted...";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmQualityCheckMaster.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                DG.Columns[7].Visible = true;
                DG.Columns[8].Visible = false;
            }
            else
            {
                DG.Columns[7].Visible = false;
                DG.Columns[8].Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);

            Label lblQcParameterenable_disable = (Label)e.Row.FindControl("lblQcParameterenable_disable");
            if (lblQcParameterenable_disable.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void lnkQcParameter_ED(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            Label lblQcParameterenable_disable = (Label)gvr.FindControl("lblQcParameterenable_disable");
            string updateval = lblQcParameterenable_disable.Text == "1" ? "0" : "1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update QCMaster set Enable_Disable=" + updateval + " where ID=" + DG.DataKeys[gvr.RowIndex].Value + "");
            Fill_grid();
        }
    }
    //private void logo()
    //{
    //    imgLogo.ImageUrl.DefaultIfEmpty();
    //    imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
    //    LblCompanyName.Text = Session["varCompanyName"].ToString();
    //    LblUserName.Text = Session["varusername"].ToString();
    //}
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGShowData_RowCreated(object sender, GridViewRowEventArgs e)
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
}