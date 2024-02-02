using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_Carpet_FrmQualityWiseShipmentUpload : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }


        if (!IsPostBack)
        {
            string str = @"select Customerid,Customercode From Customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Customercode
                            SELECT CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where ICM.CATEGORY_ID=CS.CATEGORYID And CS.ID=0 And ICM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CATEGORY_NAME
                           Select varCompanyType From Mastersetting";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddBuyerCode, ds, 0, true, "---Select---");
            UtilityModule.ConditionalComboFillWithDS(ref ddlcategory, ds, 1, true, "---Select---");
            //UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER", true, "---Select---");
            ddlcategory.SelectedValue = Request.QueryString["Category"];
            if (ddlcategory.Items.Count > 0)
            {
                ddlcategory.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFill(ref DDMasterQuality, "SELECT ITEM_ID, ITEM_NAME froM ITEM_MASTER where CATEGORY_ID=" + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select --");
            DDMasterQuality.SelectedValue = Request.QueryString["Item"];
            txtid.Text = "0";
            txtDescription.Focus();          
            lablechange();
           
        }
        lbl.Visible = false;
       
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblQualityName.Text = ParameterList[0];
        lblcategory.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void BindQuality()
    {
        string str;
        DataSet ds;
        str = @"select Distinct q.QualityId,Q.QualityName from ITEM_PARAMETER_MASTER IM inner join Quality Q 
                  on IM.QUALITY_ID=Q.QualityId  and IM.ITEM_ID= " + DDMasterQuality.SelectedValue + " and Q.mastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Qualityname";

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDQuality, ds, 0, true, "--Select--");
        
    }
    protected void DDMasterQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQuality();
        Fill_Grid();
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        GDQualityShipment.DataSource = Fill_Grid_Data();
        GDQualityShipment.DataBind();
        //gdDesign.Columns[1].Visible = false;
    }
    private DataSet Fill_Grid_Data()
    {

        string where = "";
        if (ddBuyerCode.SelectedIndex > 0)
        {
            where = where + " and MQSU.CustomerCodeId=" + ddBuyerCode.SelectedValue;
        }
        if (ddlcategory.SelectedIndex > 0)
        {
            where = where + " and MQSU.Category_Id=" + ddlcategory.SelectedValue;
        }
        if (DDMasterQuality.SelectedIndex> 0)
        {
            where = where + " and MQSU.Item_Id=" + DDMasterQuality.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and MQSU.QualityId=" + DDQuality.SelectedValue;
        }

        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@UserID", Session["VarUserId"]);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@Where", where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FILLQualityShipmentUpload", param);
        return ds;

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DGWareHouseName.DataSource = ds.Tables[0];
        //    DGWareHouseName.DataBind();
        //}
        //else
        //{
        //    DGWareHouseName.DataSource = null;
        //    DGWareHouseName.DataBind();
        //}    

    }
    protected void GDQualityShipment_SelectedIndexChanged(object sender, EventArgs e)
    {

        int r = Convert.ToInt32(GDQualityShipment.SelectedIndex.ToString());

        lbl.Text = "";

        btnsave.Text = "Save";

        string id = GDQualityShipment.SelectedDataKey.Value.ToString();
       txtid.Text = id;

        ViewState["Id"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Id", id);
            param[1] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[2] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetQualityShipmentUpload", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                ddBuyerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerCodeId"].ToString();
                ddlcategory.SelectedValue = ds.Tables[0].Rows[0]["Category_Id"].ToString();
                DDMasterQuality.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
                BindQuality();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["QDescription"].ToString();
                txtShipment.Text = ds.Tables[0].Rows[0]["QShipment"].ToString();
                txtUpload.Text = ds.Tables[0].Rows[0]["QUpload"].ToString();
                txtNetWt.Text = ds.Tables[0].Rows[0]["QNetWtInKg"].ToString();
                
            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lbl.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        btnsave.Text = "Update";
       // btndelete.Visible = true;
    }
    protected void GDQualityShipment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDQualityShipment.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void GDQualityShipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDQualityShipment, "select$" + e.Row.RowIndex);
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtDescription.Text != "")
        {
            CheckDuplicateData();
            if (lbl.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[11];
                    _arrPara[0] = new SqlParameter("@Id", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@CustomerCodeId", SqlDbType.Int);
                    _arrPara[2] = new SqlParameter("@Category_id", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@Item_Id", SqlDbType.Int);
                    _arrPara[4] = new SqlParameter("@QualityId", SqlDbType.Int);
                    _arrPara[5] = new SqlParameter("@QDescription", SqlDbType.VarChar, 300);
                    _arrPara[6] = new SqlParameter("@QShipment", SqlDbType.VarChar, 300);
                    _arrPara[7] = new SqlParameter("@QUpload", SqlDbType.VarChar, 300);
                    _arrPara[8] = new SqlParameter("@VarCompanyId", SqlDbType.Int);
                    _arrPara[9] = new SqlParameter("@VarUserId", SqlDbType.Int);
                    _arrPara[10] = new SqlParameter("@QNetWtInKg", SqlDbType.Float);
                  
                  
                    if (btnsave.Text == "Save")
                        _arrPara[0].Value = 0;
                    else
                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = ddBuyerCode.SelectedValue;
                    _arrPara[2].Value = ddlcategory.SelectedValue;
                    _arrPara[3].Value = DDMasterQuality.SelectedValue;
                    _arrPara[4].Value = DDQuality.SelectedValue;
                    _arrPara[5].Value = txtDescription.Text;
                    _arrPara[6].Value = txtShipment.Text;
                    _arrPara[7].Value = txtUpload.Text;
                    _arrPara[8].Value = Session["varMasterCompanyIDForERP"].ToString();
                    _arrPara[9].Value = Session["varuserid"].ToString();
                    _arrPara[10].Value = txtNetWt.Text;

                   
                  
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_MasterQualityShipmentUpload", _arrPara);
                    
                    txtDescription.Text = "";
                    txtShipment.Text = "";
                    txtUpload.Text = "";
                    txtNetWt.Text = "";
                    //UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER", true, "---Select --");
                    lbl.Visible = true;
                    lbl.Text = "Data Successfully Saved.....";
                    //AllEnums.MasterTables.Quality.RefreshTable();
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmQuality.aspx");
                    //Lblerr.Text = ex.Message;
                    //Logs.WriteErrorLog("Masters_Campany_Design|cmdSave_Click|" + ex.Message);
                    //ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
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
                //btndelete.Visible = false;
                //////ddlcategory.SelectedValue = null;
                //////DDMasterQulaty.SelectedValue = null;
                //txtquality.Text = "";
                //txthscode.Text = "";
                //txtquality.Focus();
                //txtinstruction.Text = "";
                //txtRemark.Text = "";
            }
        }
        else
        {
            lbl.Visible = true;
            lbl.Text = "Fill Details.................";
        }
    }

    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select QDescription from MasterQualityShipmentUpload Where CustomerCodeId=" + ddBuyerCode.SelectedValue + " and Category_ID=" + ddlcategory.SelectedValue + " and  Item_Id=" + DDMasterQuality.SelectedValue + " and QualityId=" + DDQuality.SelectedValue + "  and Id !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl.Visible = true;
            lbl.Text = "Quality AlReady Exists........";
            txtDescription.Text = "";
            txtDescription.Focus();
        }
        else
        {
            lbl.Visible = false;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lbl.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            ////LinkButton lnkdel = sender as LinkButton;
            ////GridViewRow grv = lnkdel.NamingContainer as GridViewRow;
            //int BMID = Convert.ToInt32(GVBunkarMaster.DataKeys[e.RowIndex].Value);

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Id", ViewState["Id"]);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DeleteQualityShipmentUpload", param);
            lbl.Text = param[3].Value.ToString();
            Tran.Commit();

            Fill_Grid();           
            //btndelete.Visible = false;
            btnsave.Text = "Save";
            txtid.Text = "0";
            ViewState["Id"] = "0";
           

        }
        catch (Exception ex)
        {
            lbl.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
       
        //Fill_Grid();
        //btndelete.Visible = false;
        //btnsave.Text = "Save";
        //txtquality.Text = "";
        //txtid.Text = "0";
    }
    protected void ddBuyerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategorySelectedChanged();
        Fill_Grid();
    }
    protected void CategorySelectedChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDMasterQuality, "select item_id,item_name from item_master where category_id= " + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By item_name", true, "--Select--");
    }
//    protected void Button1_Click(object sender, EventArgs e)
//    {
//        Report();
//    }
//    private void Report()
//    {
    

    
//        //string qry = @"    SELECT QualityName FROM  Quality where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY QualityName";

//        string qry=@"SELECT IM.ITEM_NAME, Q.QualityName FROM  Quality Q INNER JOIN ITEM_MASTER IM ON Q.Item_Id=IM.ITEM_ID
//                    INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID=ICM.CATEGORY_ID
//                    where Q.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and Q.Item_Id="+DDMasterQulaty.SelectedValue+" and ICM.CATEGORY_ID="+ddlcategory.SelectedValue+" ORDER BY QualityName";
//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            Session["rptFileName"] = "~\\Reports\\RptfrmQualityNew.rpt";
//            //Session["rptFileName"] = Session["ReportPath"];
//            Session["GetDataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\RptfrmQualityNew.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
//        }
//    }
    protected void GDQualityShipment_RowCreated(object sender, GridViewRowEventArgs e)
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
}