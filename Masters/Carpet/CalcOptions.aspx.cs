using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_CalcOptions : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            
            txtid.Text = "0";
            
            fill_grid();
            txtCalcOption.Focus();
        }
        lblMessage.Visible = false;
    }   

    private void fill_grid()
    {
        gdCalcOption.DataSource = Fill_Grid_Data();
        gdCalcOption.DataBind();      
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT CalcId as Sr_No,CalcName  FROM CalcOptions Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  Order By CalcId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns[1].ColumnName = "Calc Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CalcOptions.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    private void Store_Data()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@CalcId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@CalcName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value =txtCalcOption.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_CalcOption", _arrPara);
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "Save Details........";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CalcOptions.aspx");
            Logs.WriteErrorLog("Masters_Carpet_FrmCalcOptions|cmdSave_Click|" + ex.Message);
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
        fill_grid();

    }
   
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from CalcOptions Where CalcName='" + txtCalcOption.Text + "' and CalcId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "CalcName AlReady Exits........";
            txtCalcOption.Text = "";
            txtCalcOption.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    private void ClearAll()
    {
        txtid.Text = "0";
        txtCalcOption.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtCalcOption.Text != "")
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
            txtCalcOption.Text = "";
            btnSave.Text = "Save";
            btndelete.Visible = false;
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Fill Details........";
        }
    }   
   
    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
    }

    protected void gdCalcOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdCalcOption.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from CalcOptions WHERE CalcId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CalcId"].ToString();
               txtCalcOption.Text = ds.Tables[0].Rows[0]["CalcName"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CalcOptions.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnSave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void gdCalcOption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdCalcOption, "Select$" + e.Row.RowIndex);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        txtid.Text = "0";
       txtCalcOption.Text = "";
        btndelete.Visible = false;
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@CalcId", ViewState["id"]);
            _array[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"]);
            _array[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            _array[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Delete_CalcOptions", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _array[3].Value + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CalcOptions.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        fill_grid();
        btndelete.Visible = false;
        btnSave.Text = "Save";
       txtCalcOption.Text = "";
        txtid.Text = "0";

    }    
}