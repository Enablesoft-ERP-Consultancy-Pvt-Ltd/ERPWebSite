using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Process_FrmFinisherProcessCustomerLWWt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CustomerID, CI.CustomerCode From CustomerInfo CI(Nolock) Where CI.CustomerCode <> '' And MasterCompanyId = " + Session["varCompanyId"] + @" Order By CI.CustomerCode
                        Select Process_Name_ID, Process_Name From Process_Name_Master Where MasterCompanyID = " + Session["varCompanyId"] + " Order By Process_Name ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 0, false, "");

            fill_grid();
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "Select Process Name");
        }
    }

    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }

    private void fill_grid()
    {
        DGDetail.DataSource = Fill_Grid_Data();
        DGDetail.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"Select a.ID, CI.CustomerCode, PNM.PROCESS_NAME ProcessName 
                    From Finishing_Process_Customer_L_W_Wt a(Nolock) 
                    JOIN Customerinfo CI(Nolock) ON CI.CustomerId = a.CustomerID 
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID
                    Where a.MasterCompanyID = " + Session["varCompanyId"] + @" 
                    And a.CustomerID = " + DDCustomerCode.SelectedValue + " Order By CI.CustomerCode, PNM.PROCESS_NAME";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherProcessCustomerLWWt.aspx/FillGrid");
        }
        return ds;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[13];
            _arrpara[0] = new SqlParameter("@CustomerID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);

            _arrpara[0].Value = DDCustomerCode.SelectedValue;
            _arrpara[1].Value = DDProcessName.SelectedValue;

            _arrpara[2].Value = Session["varuserId"];
            _arrpara[3].Value = Session["varCompanyId"];
            _arrpara[4].Direction = ParameterDirection.InputOutput;

            SqlTransaction Tran = con.BeginTransaction();
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveFinisherProcessCustomerLWWt", _arrpara);

            Tran.Commit();

            fill_grid();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('" + _arrpara[4].Value.ToString() + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherProcessCustomerLWWt.aspx/SaveData");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }

    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int VarID = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete Finishing_Process_Customer_L_W_Wt Where ID = " + VarID);
        fill_grid();
    }
    
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "select$" + e.Row.RowIndex);
        }
    }

    protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}