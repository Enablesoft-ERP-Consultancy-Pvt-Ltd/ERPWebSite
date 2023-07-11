using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Master_Recipe_AddRecipeName : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["ID"] = "0";
            fill_grid();
            TxtRecipeName.Focus();
        }
        Lblerrer.Visible = false;
    }
    private void fill_grid()
    {
        DGRecipeName.DataSource = Fill_Grid_Data();
        DGRecipeName.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"Select ID, Name, EnableDisbleFlag, Case When EnableDisbleFlag = 1 Then 'Enable' Else 'Disble' End Flag 
            From RecipeMaster(Nolock) 
            Where ProcessID = " + Request.QueryString["ProcessID"] + " And MasterCompanyId = " + Session["varCompanyId"] + " Order By Name";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Recipe/AddReceipeName.aspx");
            Lblerrer.Visible = true;
            Lblerrer.Text = ex.Message;
        }
        return ds;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (TxtRecipeName.Text != "")
            {
                SqlParameter[] _arrPara1 = new SqlParameter[7];
                _arrPara1[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara1[1] = new SqlParameter("@Name", SqlDbType.VarChar, 200);
                _arrPara1[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
                _arrPara1[3] = new SqlParameter("@UserID", SqlDbType.Int);
                _arrPara1[4] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
                _arrPara1[5] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);
                _arrPara1[6] = new SqlParameter("@EnableDisbleFlag", SqlDbType.Int);

                _arrPara1[0].Direction = ParameterDirection.InputOutput;
                _arrPara1[0].Value = ViewState["ID"];
                _arrPara1[1].Value = TxtRecipeName.Text.ToUpper();
                _arrPara1[2].Value = Request.QueryString["ProcessID"];
                _arrPara1[3].Value = Session["varuserid"].ToString();
                _arrPara1[4].Value = Session["varCompanyId"].ToString();
                _arrPara1[5].Direction = ParameterDirection.InputOutput;
                _arrPara1[6].Value = ChkForEnableDisbleFlag.Checked == true ? 1 : 0;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_RECIPENAME", _arrPara1);
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", " alert('" + _arrPara1[5].Value + "');", true);
                Tran.Commit();

                ViewState["ID"] = "0";
                TxtRecipeName.Text = "";
                ChkForEnableDisbleFlag.Checked = false;
                TxtRecipeName.Focus();
                fill_grid();
                btnSave.Text = "Save";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Recipe/AddRecipeNameBtnSaveClick.aspx");
            Lblerrer.Visible = true;
            Lblerrer.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }
    }

    protected void DGRecipeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ID"] = DGRecipeName.SelectedDataKey.Value.ToString();
        TxtRecipeName.Text = DGRecipeName.Rows[DGRecipeName.SelectedIndex].Cells[0].Text;

        if (DGRecipeName.Rows[DGRecipeName.SelectedIndex].Cells[2].Text == "1")
        {
            ChkForEnableDisbleFlag.Checked = true;
        }
        else
        {
            ChkForEnableDisbleFlag.Checked = false;
        }
        btnSave.Text = "Update";
    }
    protected void DGRecipeName_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRecipeName, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Lblerrer.Text = "";
        btnSave.Text = "Save";
        TxtRecipeName.Text = "";
        TxtRecipeName.Focus();
    }
}