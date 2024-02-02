﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Carpet_AddShadeColor : System.Web.UI.Page
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
            lablechange();
            fill_grid();
            txtcolor.Focus();
            if (Convert.ToInt32(Session["varcompanyno"]) == 7)
            {
                btnnew.Visible = false;                
            }
        }
        lblMessage.Visible = false;
    }
    public void lablechange()
    {
        lblshadecolorname.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where PS.Company_Id=" + Session["varMasterCompanyIDForERP"] + " And  ps.parameter_id='8'").ToString();
    }
    protected void gdshadecolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdshadecolor.SelectedDataKey.Value.ToString();
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from ShadeColor WHERE shadeColorId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["shadeColorId"].ToString();
                txtcolor.Text = ds.Tables[0].Rows[0]["shadeColorName"].ToString();
                txtColorBox.Text = ds.Tables[0].Rows[0]["ShadeColorBox"].ToString();
                txtShadeColor.Text = ds.Tables[0].Rows[0]["ShadeColor"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShadeColor.aspx");
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
        btndelete.Visible = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtcolor.Text != "")
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
            txtcolor.Text = "";
            btnsave.Text = "Save";
            btndelete.Visible = false;
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Fill Details........";
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from shadecolor Where shadeColorName='" + txtcolor.Text + "' and shadeColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "shadeColor AlReady Exits........";
            txtcolor.Text = "";
            txtcolor.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    private void Store_Data()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@shadeColorId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@shadeColorName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ShadeColorBox", SqlDbType.NVarChar, 50);
                _arrPara[5] = new SqlParameter("@ShadeColor", SqlDbType.NVarChar, 50);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtcolor.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[4].Value = txtColorBox.Text.ToUpper().Trim();
                _arrPara[5].Value = txtShadeColor.Text.ToUpper().Trim();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_shadeColor", _arrPara);
                txtid.Text = "0";
                //ClearAll();
                fill_grid();
                lblMessage.Visible = true;
                lblMessage.Text = "Data have been Saved Successfully";
               // AllEnums.MasterTables.ShadeColor.RefreshTable();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShadeColor.aspx");
            Logs.WriteErrorLog("Masters_Carpet_FrmshadeColor|cmdSave_Click|" + ex.Message);
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
    private void fill_grid()
    {
        gdshadecolor.DataSource = Fill_Grid_Data();
        gdshadecolor.DataBind();
        Session["ReportPath"] = "Reports/RptShadeColor.rpt";
        Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT shadeColorId as Sr_No,shadeColorName as " + lblshadecolorname.Text + ",S.ShadeColorBox,S.ShadeColor FROM shadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By shadeColorId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns["SHADE_COLOR"].ColumnName = "ShadeColor Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShadeColor.aspx");
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
    protected void gdshadecolor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdshadecolor, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gdshadecolor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdshadecolor.PageIndex = e.NewPageIndex;
        fill_grid();
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        txtcolor.Text = "";
        txtid.Text = "0";
        txtColorBox.Text = "";
        txtShadeColor.Text = "";
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] parparam = new SqlParameter[3];
            parparam[0] = new SqlParameter("@id", ViewState["id"].ToString());
            parparam[1] = new SqlParameter("@varCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            parparam[2] = new SqlParameter("@varuserid", Session["varuserid"].ToString());

            int id = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteShadeColor", parparam);
            if (id > 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Record(s) has been deleted!";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Value in Use...";
            }

            fill_grid();
            btndelete.Visible = false;
            btnsave.Text = "Save";
            txtcolor.Text = "";
            txtid.Text = "0";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ShadeColor.aspx");
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
    protected void gdshadecolor_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void btnrpt_Click(object sender, EventArgs e)
    {
        string qry = @" SELECT ShadeColorName,ShadeColorBox,ShadeColor  FROM  ShadeColor";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptshadecolor.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptshadecolor.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
}