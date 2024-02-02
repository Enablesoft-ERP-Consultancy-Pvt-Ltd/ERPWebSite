using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_MasterQualityTypeChampa : CustomPage
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
        GVMasterQualityType.DataSource = Fill_Grid_Data();
        GVMasterQualityType.DataBind();

        //Session["ReportPath"] = "Reports/Design.rpt";
        //Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select MasterQualityTypeId,MasterQualityTypeName from MasterQualityType where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            strsql = strsql + " order by MasterQualityTypeName";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/MasterQualityTypeChampa.aspx");
            Logs.WriteErrorLog("Masters_Carpet_MasterQualityTypeChampa|Fill_Grid_Data|" + ex.Message);
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
    protected void GVMasterQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = GVMasterQualityType.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        txtid.Text = "0";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select MasterQualityTypeId ,MasterQualityTypeName  from MasterQualityType where MasterQualityTypeId=" + id + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        try
        {

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["MasterQualityTypeId"].ToString();
                txtMasterQualityType.Text = ds.Tables[0].Rows[0]["MasterQualityTypeName"].ToString();
               

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/MasterQualityTypeChampa.aspx");
            Logs.WriteErrorLog("Masters_Carpet_MasterQualityTypeChampa|Fill_Grid_Data|" + ex.Message);
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
    protected void GVMasterQualityType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMasterQualityType.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void GVMasterQualityType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.GVMasterQualityType, "select$" + e.Row.RowIndex);

        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select MasterQualityTypeId,MasterQualityTypeName from MasterQualityType Where MasterQualityTypeName='" + txtMasterQualityType.Text+ "' and MasterQualityTypeId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Master Quality Type AlReady Exits........";
            txtMasterQualityType.Text= "";
            txtMasterQualityType.Focus();
        }
        else
        {
            Lblerr.Visible = false;
        }
    }
    
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtMasterQualityType.Text != "")
        {
            CheckDuplicateData();
            
            if (Lblerr.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[6];
                    _arrPara[0] = new SqlParameter("@MasterQualityTypeId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@MasterQualityTypeName", SqlDbType.VarChar, 50);
                    _arrPara[2] = new SqlParameter("@VarCompanyId", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@VarUserId", SqlDbType.Int); 


                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtMasterQualityType.Text.ToUpper();
                    _arrPara[2].Value = Session["varMasterCompanyIDForERP"].ToString();
                    _arrPara[3].Value = Session["varuserid"].ToString();                 

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveMasterQualityType", _arrPara);
                    Tran.Commit();
                    Lblerr.Visible = true;
                    Lblerr.Text = "Save Details....";
                    txtMasterQualityType.Text = "";
                    txtid.Text = "0";                   
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/MasterQualityTypeChampa.aspx");
                    Lblerr.Visible = true;
                    Lblerr.Text = ex.Message;
                    Logs.WriteErrorLog("Masters_Carpet_MasterQualityTypeChampa|cmdSave_Click|" + ex.Message);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
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
                btnsave.Text = "Save";
                btndelete.Visible = false;

            }
        }
        else
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Fill Details....";
        }
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    Report();
    //}
    //private void Report()
    //{
    //    string qry = @"  SELECT designName  FROM   Design where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY designName";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptFileName"] = "~\\Reports\\DesignNew.rpt";
    //        //Session["rptFileName"] = Session["ReportPath"];
    //        Session["GetDataset"] = ds;
    //        Session["dsFileName"] = "~\\ReportSchema\\DesignNew.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }
    //}
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@MasterQualityTypeId", ViewState["id"]);
            _array[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"]);
            _array[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            _array[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_DeleteMasterQualityType", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _array[3].Value + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/MasterQualityTypeChampa.aspx");

        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        Fill_Grid();
        btndelete.Visible = false;
        btnsave.Text = "Save";
        txtMasterQualityType.Text = "";
        txtid.Text = "0";       
    }
    protected void GVMasterQualityType_Init(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
    }
   
}