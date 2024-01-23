using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_QualityMaster : CustomPage
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
        Session["ReportPath"] = "RptQualityMaster.rpt";
        Session["CommanFormula"] = ""; 


    }
    private void Fill_Grid()
    {
        gdQuality.DataSource = Fill_Grid_Data();
        gdQuality.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "SELECT MasterQualityid,Quality,QualityCode from QualityMaster order by MasterQualityid";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/QualityMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_QualityMaster|Fill_Grid_Data|" + ex.Message);
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
    protected void gdQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdQuality.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from QualityMaster where MasterQualityid=" + id);
        try
        {

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["MasterQualityid"].ToString();
                txtQualityName.Text = ds.Tables[0].Rows[0]["Quality"].ToString();
                txtQualityCode.Text = ds.Tables[0].Rows[0]["QualityCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/QualityMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_QualityMaster|Fill_Grid_Data|" + ex.Message);
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
    protected void gdQuality_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdQuality.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdQuality_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdQuality, "select$" + e.Row.RowIndex);
        }
    }
    
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@MasterQualityid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Quality", SqlDbType.NVarChar, 50);
            _arrPara[2] = new SqlParameter("@QualityCode", SqlDbType.NVarChar,50);
            _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[0].Value = Convert.ToInt32(txtid.Text);
            _arrPara[1].Value = txtQualityName.Text;
            _arrPara[2].Value = txtQualityCode.Text;
            _arrPara[3].Value = Session["varuserid"].ToString();
            _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Quality", _arrPara);


            txtQualityName.Text = "";
            txtQualityCode.Text = "";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/QualityMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_QualityMaster|cmdSave_Click|" + ex.Message);
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
        Fill_Grid();
    }
    protected void gdQuality_RowCreated(object sender, GridViewRowEventArgs e)
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