using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_Unit : System.Web.UI.Page
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
            CommanFunction.FillCombo(ddUnit, "select UnitTypeId,UnitType from Unit_Type_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by UnitTypeId");
        }
        LblError.Visible = false;
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
            string strsql = @"SELECT dbo.Unit.UnitId as Sr_No, dbo.Unit.UnitName, dbo.UNIT_TYPE_MASTER.UnitType
                              FROM dbo.Unit INNER JOIN
                              dbo.UNIT_TYPE_MASTER ON dbo.Unit.UnitTypeID = dbo.UNIT_TYPE_MASTER.UnitTypeID And Unit.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Unit.aspx");
            Logs.WriteErrorLog("Masters_Campany_Unit|Fill_Grid_Data|" + ex.Message);
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

    protected void btnsave_Click(object sender, EventArgs e)
    {
        ChkDuplicate();
        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@UnitId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@UnitName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@UnitTypeId", SqlDbType.NVarChar, 50);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtUnit.Text.ToUpper();
                _arrPara[2].Value = ddUnit.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Unit", _arrPara);

                txtid.Text = "0";
                txtUnit.Text = "";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Unit.aspx");
                Logs.WriteErrorLog("Masters_Campany_Unit|cmdSave_Click|" + ex.Message);
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
            LblError.Visible = true;
            LblError.Text = "Save Details.....";
            Fill_Grid();
            btnsave.Text = "Save";
        }
    }

    protected void gdUnit_SelectedIndexChanged(object sender, EventArgs e)
    {

        string id = gdUnit.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Unit where UnitId=" + id + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        try
        {

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["UnitId"].ToString();
                txtUnit.Text = ds.Tables[0].Rows[0]["Unitname"].ToString();
                ddUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitTypeid"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Unit.aspx");
            Logs.WriteErrorLog("Masters_Campany_Unit|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
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


    protected void ChkDuplicate()
    {
        if (txtUnit.Text == "")
        {
            LblError.Visible = true;
            LblError.Text = "Please Fill Unit ..............";
        }
        else
        {
            LblError.Visible = true;
            LblError.Text = "";
        }

        if (LblError.Text == "")
        {
            string str;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                if (btnsave.Text == "Update")
                {
                    str = "Select isnull(UnitId,0) from Unit where UnitName='" + txtUnit.Text + "' and UnittypeId =" + ddUnit.SelectedValue + " and Unitid<>" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    str = "Select isnull(UnitId,0) from Unit where UnitName='" + txtUnit.Text + "' and UnittypeId =" + ddUnit.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblError.Visible = true;
                    LblError.Text = "Unit Already Exist....";
                    txtUnit.Text = "";
                    txtUnit.Focus();
                }
                else
                {
                    LblError.Text = "";
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Unit.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void gdUnit_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        // if (e.Row.RowType == DataControlRowType.Header)
        //   e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //        e.Row.RowState == DataControlRowState.Alternate)
        //e.Row.CssClass = "alternate";
    }
}