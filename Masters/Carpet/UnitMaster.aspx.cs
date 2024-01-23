using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_UnitMaster : CustomPage
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
            Fill_Grid();
        }
        Lblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        gdUnit.DataSource = Fill_Grid_Data();
        gdUnit.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select UnitTypeId as Sr_No,UnitType from Unit_Type_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/UnitMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_UnitMaster|Fill_Grid_Data|" + ex.Message);
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
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from UNIT_TYPE_MASTER Where UnitType='" + txtUnit.Text + "' and UnitTypeID !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Already Exits........";
            txtUnit.Text = "";
            txtUnit.Focus();
        }
        else
        {
            Lblerr.Visible = false;
        }
    }


    protected void btnsave_Click(object sender, EventArgs e)
    {
        CheckDuplicateData();
        if (Lblerr.Visible == false && txtUnit.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@UnitType", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtUnit.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Unit_Type_Master", _arrPara);
                txtid.Text = "0";
                txtUnit.Text = "";

            }
            catch (Exception ex)
            {
                //lblErr.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/UnitMaster.aspx");
                Logs.WriteErrorLog("Masters_Campany_Unit_Type_Master|cmdSave_Click|" + ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
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
                if (Request.QueryString["id"] != null)
                {
                    if (Request.QueryString["id"] == "1")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
                    }
                }

            }
            Lblerr.Visible = true;
            Lblerr.Text = "Save Details......";
            Fill_Grid();
        }
    }
    protected void gdUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdUnit.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Unit_Type_Master where UnitTypeId=" + id);
        try
        {

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["UnitTypeId"].ToString();
                txtUnit.Text = ds.Tables[0].Rows[0]["UnitType"].ToString();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/UnitMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_Unit_Type_Master|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void gdUnit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdUnit.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdUnit, "select$" + e.Row.RowIndex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtUnit.Text = "";
        txtUnit.Focus();
        btnsave.Text = "Save";
    }
   
}