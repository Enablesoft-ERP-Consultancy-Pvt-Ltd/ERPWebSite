using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_Campany_frmBrand : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"SELECT CATEGORY_ID, CATEGORY_NAME 
                    From ITEM_CATEGORY_MASTER IM(Nolock)
                    JOIN UserRights_Category UC(Nolock) ON UC.CategoryId = IM.Category_Id And UC.UserId=" + Session["varuserid"] + @" 
                    Where IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"  Order By CATEGORY_NAME
                    Select varCompanyType From Mastersetting(Nolock)";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcategory, ds, 0, true, "---Select---");

            ddlcategory.SelectedValue = Request.QueryString["Category"];
            if (ddlcategory.Items.Count > 0)
            {
                ddlcategory.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFill(ref DDMasterQulaty, @"SELECT ITEM_ID, ITEM_NAME 
                From ITEM_MASTER(Nolock) 
                Where CATEGORY_ID = " + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select --");
            DDMasterQulaty.SelectedValue = Request.QueryString["Item"];
            txtid.Text = "0";
            txtbrand.Focus();
            lablechange();
           
            lbl.Visible = false;
            Session["ReportPath"] = "Reports/RptfrmQuality.rpt";
            Session["CommanFormula"] = "";

            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
               
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
       // lblqualityname.Text = ParameterList[0];
        lblcategory.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDMasterQulaty_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        gdQuality.DataSource = Fill_Grid_Data();
        gdQuality.DataBind();
    }
    
    private DataSet Fill_Grid_Data()
    {
        string strsql = "";
        DataSet ds = null;
        try
        {
            if (Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select varCompanyType From Mastersetting(Nolock)"))==1)
            {
                strsql = "Select QualityId as Sr_No,IM.ITEM_NAME as QualityType,QualityName as " + lblbrandname.Text + @",Loss,Hscode,Instruction,Remark, MaterialRate Rate 
                from Quality Q(Nolock) 
                JOIN ITEM_MASTER IM(Nolock) ON Q.Item_Id=IM.ITEM_ID Where Q.Item_Id=" + DDMasterQulaty.SelectedValue + @" 
                And Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by QualityId";
            }
            else
            {
                strsql = "Select QualityId as Sr_No,QualityName as " + lblbrandname.Text + @" 
                From Quality(Nolock) 
                Where Item_Id=" + DDMasterQulaty.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityId";
            }

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmQuality.aspx");
        }
        return ds;
    }
    protected void gdQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdQuality.SelectedDataKey.Value.ToString();

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select distinct ql.qualityid,ql.qualityname,it.ITEM_NAME,it.ITEM_id, 
        it.category_id,Loss,ql.hscode,ql.instruction,ql.Remark,ql.QualityCode, ql.MaterialRate Rate 
        From Quality ql(Nolock) 
        join ITEM_MASTER it(Nolock) on ql.ITEM_id=it.ITEM_id 
        where qualityid=" + id + " And ql.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
        Select MT.Month_Id MonthID, MT.Month_Name [MonthName], IsNull(QL.LossPercentage, '') LossPercentage
        From MonthTable MT(Nolock) 
        LEFT JOIN QualityLoss QL(Nolock) ON QL.MonthID = MT.Month_ID And QL.QualityID = " + id + @" 
        Order By MT.Month_Id ");
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["QualityId"].ToString();
                txtbrand.Text = ds.Tables[0].Rows[0]["QualityName"].ToString();
                DDMasterQulaty.SelectedValue = ds.Tables[0].Rows[0]["ITEM_id"].ToString();
                ddlcategory.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
              
                TxtBrandCode.Text = ds.Tables[0].Rows[0]["hscode"].ToString();
               
            }
            DGMonthName.DataSource = ds.Tables[1];
            DGMonthName.DataBind();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmQuality.aspx");
        }
        btnsave.Text = "Update";
        btndelete.Visible = true;
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
        string Str = "";
       

        if (txtbrand.Text != "")
        {
            lbl.Visible = false;
            if (lbl.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[13];
                    _arrPara[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@BrandName", SqlDbType.NVarChar, 250);
                    _arrPara[2] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrPara[9] = new SqlParameter("@BrandCode", SqlDbType.NVarChar, 100);
                    _arrPara[11] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);
                    

                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtbrand.Text.ToUpper();
                    _arrPara[2].Value = DDMasterQulaty.SelectedValue;
                    _arrPara[3].Value = Session["varuserid"].ToString();
                    _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                    _arrPara[9].Value = txtbrand.Text.ToUpper();
                    _arrPara[11].Direction = ParameterDirection.Output;


                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Brand", _arrPara);
                    Tran.Commit();
                    txtbrand.Text = "";
                    TxtBrandCode.Text = "";
                  
                    txtid.Text = "0";
                  
                    lbl.Visible = true;
                    lbl.Text = _arrPara[11].Value.ToString();
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmBrand.aspx");
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
                Fill_Grid();
                btnsave.Text = "Save";
                btndelete.Visible = false;
                txtbrand.Text = "";
              
                txtbrand.Focus();
              
            }
        }
        else
        {
            lbl.Visible = true;
            lbl.Text = "Fill Details.................";
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] Parparam = new SqlParameter[5];
            //Parparam[0] = new SqlParameter("@id", Session["id"].ToString());
            Parparam[0] = new SqlParameter("@id", Convert.ToInt32(txtid.Text));
            Parparam[1] = new SqlParameter("@VarCompanyID", Session["varMasterCompanyIDForERP"].ToString());
            Parparam[2] = new SqlParameter("@VarUserID", Session["varuserid"].ToString());
            Parparam[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            Parparam[3].Direction = ParameterDirection.Output;
            
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteQuality", Parparam);
            
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + Parparam[3].Value + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmQuality.aspx");
            Tran.Rollback();
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
        txtbrand.Text = "";
        txtid.Text = "0";
    }

    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER(Nolock) where CATEGORY_ID=" + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select --");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry= @"SELECT IM.ITEM_NAME, Q.QualityName 
                FROM Quality Q(Nolock)
                JOIN ITEM_MASTER IM(Nolock) ON Q.Item_Id=IM.ITEM_ID
                JOIN ITEM_CATEGORY_MASTER ICM(Nolock) ON IM.CATEGORY_ID=ICM.CATEGORY_ID
                where Q.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and Q.Item_Id="+DDMasterQulaty.SelectedValue+" and ICM.CATEGORY_ID="+ddlcategory.SelectedValue+" ORDER BY QualityName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptfrmQualityNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptfrmQualityNew.xsd";
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
    protected void gdQuality_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        //  e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //        e.Row.RowState == DataControlRowState.Alternate)
        //e.Row.CssClass = "alternate";
    }
    protected void DGMonthName_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGMonthName, "select$" + e.Row.RowIndex);
        }
    }
}