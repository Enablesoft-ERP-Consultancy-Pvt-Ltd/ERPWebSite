using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_frmPenality : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            fill_ddl();
            Fill_Grid();

            if (Session["varCompanyId"].ToString() == "20")
            {
                LblQuality.Text = "Quality Type";
            }
            else
            {
                LblQuality.Text = "Quality Name";
            }
        }
    }
    private void Fill_Grid()
    {
        gdpanality.DataSource = Fill_Grid_Data();
        gdpanality.DataBind();
        //Session["ReportPath"] = "Panlty.rpt";
        //Session["CommanFormula"] = "";
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            if (Convert.ToInt32(Session["varCompanyId"]) == 20)
            {
                string strsql = @"SELECT  PM.PenalityId as SrNo,IM.ITEM_NAME,PM.Rate,PM.PenalityName, 
                             PM.PenalityType,PM.PenalityWF FROM dbo.PenalityMaster PM INNER JOIN ITEM_MASTER IM ON PM.QualityId = IM.ITEM_ID Where PM.MasterCompanyId=" + Session["varCompanyId"] + " and PM.PenalityWF='" + ddPenalityType.SelectedValue + "' and PM.PenalityType='" + ddPenality.SelectedValue + "' and PM.QualityId='" + ddQuality.SelectedValue + "' order by PenalityId ";
                ds = SqlHelper.ExecuteDataset(strsql);
            }
            else
            {
                string strsql = @"SELECT  PM.PenalityId as SrNo,dbo.Quality.QualityName,PM.Rate,PM.PenalityName, 
                             PM.PenalityType,PM.PenalityWF FROM dbo.PenalityMaster PM INNER JOIN dbo.Quality ON PM.QualityId = dbo.Quality.QualityId Where PM.MasterCompanyId=" + Session["varCompanyId"] + " and PM.PenalityWF='" + ddPenalityType.SelectedValue + "' and PM.PenalityType='" + ddPenality.SelectedValue + "' and PM.QualityId='" + ddQuality.SelectedValue + "' order by PenalityId ";
                ds = SqlHelper.ExecuteDataset(strsql);
            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmPenality|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPenality.aspx");
        }
        return ds;
    }
    private void fill_ddl()
    {
        if (Convert.ToInt32(Session["varCompanyId"]) == 20)
        {
            CommanFunction.FillCombo(ddQuality, "Select Item_Id,Item_Name from ITEM_MASTER Where MasterCompanyid=" + Session["varCompanyId"] + " and Category_Id=1 order by Item_Name");
            if (ddQuality.Items.Count > 0)
            {
                ddQuality.SelectedIndex = 0;
            }
        }
        else
        {
            CommanFunction.FillCombo(ddQuality, "Select QualityId,QualityName from Quality Where MasterCompanyid=" + Session["varCompanyId"] + " order by QualityName");
        }
    }
    protected void ddPenalityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void gdpanality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdpanality.SelectedDataKey.Value.ToString();
        Session["id"] = id;
        DataSet ds = SqlHelper.ExecuteDataset("select PenalityId,Qualityid,Rate,PenalityName,PenalityType from PenalityMaster where PenalityId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["PenalityId"].ToString();
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["Qualityid"].ToString();
                txtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                txtPenalityName.Text = ds.Tables[0].Rows[0]["PenalityName"].ToString();
               // txtPenalityType.Text = ds.Tables[0].Rows[0]["PenalityType"].ToString();
                ddPenality.SelectedValue = ds.Tables[0].Rows[0]["PenalityType"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPenality.aspx");
            //Logs.WriteErrorLog("Masters_Campany_frmpenality|Fill_Grid_Data|" + ex.Message);
        }
        btnsave.Text = "Update";
    }
    protected void gdpanality_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdpanality.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdpanality_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdpanality, "Select$" + e.Row.RowIndex);
        }
    }   
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtPenalityName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@PenalityId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrPara[3] = new SqlParameter("@PenalityName", SqlDbType.NVarChar, 50);
                _arrPara[4] = new SqlParameter("@PenalityType", SqlDbType.Char);
                _arrPara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@PenalityWF", SqlDbType.NVarChar,5);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = ddQuality.SelectedValue;
                _arrPara[2].Value = Convert.ToDouble(txtRate.Text);
                _arrPara[3].Value = txtPenalityName.Text.ToUpper();
                //_arrPara[4].Value = txtPenalityType.Text.ToUpper();
                _arrPara[4].Value = ddPenality.SelectedValue;
                _arrPara[5].Value = Session["varuserid"].ToString();
                _arrPara[6].Value = Session["varCompanyId"].ToString();
                _arrPara[7].Value = ddPenalityType.SelectedValue;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PENALITY", _arrPara);
                Tran.Commit();
                txtid.Text = "0";
                ddQuality.SelectedIndex = -1;
                txtRate.Text = "";
                txtPenalityName.Text = "";
                //txtPenalityType.Text = "";
                Label1.Visible = true;
                Label1.Text = "Save Details............";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Label1.Visible = true;
                Label1.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_frmPenality|cmdSave_Click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPenality.aspx");
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
        else
        {
            if (Label1.Text == "Penality Name already exists............")
            {
                Label1.Text = "Penality Name already exists............";
            }
            else
            {
                Label1.Text = "Please Fill Details...........";
            }
        }
        Fill_Grid();
        btnsave.Text = "Save";
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        ddQuality.SelectedIndex = -1;
        txtRate.Text = "";
        txtPenalityName.Text = "";
        //txtPenalityType.Text = "";
    }
    protected void btnclose_Click1(object sender, EventArgs e)
    {

    }
   
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select QualityId,PenalityName from PenalityMaster where PenalitId !=" + Session["id"] + " and  ( QualityId='" + ddQuality.SelectedValue + "' or PenalityName='" + txtPenalityName.Text + "') and PenalityWF='" + ddPenalityType.SelectedValue + "' and PenalityType='" + ddPenality.SelectedValue + "' And MasterCompanyId=" + Session["varCompanyid"];
            }
            else
            {
                strsql = "select QualityId,PenalityName from PenalityMaster where QualityId='" + ddQuality.SelectedValue + "' and PenalityName='" + txtPenalityName.Text + "' and PenalityWF='" + ddPenalityType.SelectedValue + "' and PenalityType='" + ddPenality.SelectedValue + "' And  MasterCompanyId=" + Session["varCompanyid"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label1.Text = "Penality Name already exists............";
                txtPenalityName.Text = "";
                txtPenalityName.Focus();
            }
            else
            {
                Label1.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPenality.aspx");
        }
    }
    protected void gdpanality_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
            //e.Row.CssClass = "alternate";
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (ddPenalityType.SelectedValue!=""  && ddPenality.SelectedValue!="" && Convert.ToInt32(ddQuality.SelectedValue) >0)
        {
            Report3();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please select all mandatory fields!');", true);
        }
    }
    private void Report3()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@PenalityWF", SqlDbType.VarChar,3);
        array[1] = new SqlParameter("@PenalityType", SqlDbType.VarChar,3);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[3] = new SqlParameter("@QualityId", SqlDbType.Int);

        array[0].Value = ddPenalityType.SelectedValue;
        array[1].Value = ddPenality.SelectedValue;
        array[2].Value = Session["varcompanyId"];
        array[3].Value = ddQuality.SelectedValue;      

        //array[1].Value = 'W';
        //array[2].Value = 'C';
        //array[3].Value = 20;  
        //array[4].Value = 1; 


        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForPenalityReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptPenalityMaster.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptPenalityMaster.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else 
        { 
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}