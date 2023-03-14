using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_frmProductionInstruction : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddlcategory, "SELECT CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,UserRights_Category UC where IM.Category_Id=UC.CategoryId  And UC.UserId=" + Session["varuserid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "  Order By CATEGORY_NAME", true, "---Select---");
            //UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER", true, "---Select---");
            ddlcategory.SelectedValue = Request.QueryString["Category"];
            if (ddlcategory.Items.Count > 0)
            {
                ddlcategory.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "SELECT ITEM_ID, ITEM_NAME froM ITEM_MASTER where CATEGORY_ID=" + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
            DDMasterQulaty.SelectedValue = Request.QueryString["Item"];

            UtilityModule.ConditionalComboFill(ref DDQualityName, "SELECT QualityId,QualityName,Item_id froM Quality where Item_id=" + DDMasterQulaty.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
            DDQualityName.SelectedValue = Request.QueryString["QualityName"];

            UtilityModule.ConditionalComboFill(ref DDDesignName, @"select distinct IPM.QUALITY_ID,IPM.ITEM_ID, D.designId,D.designName from ITEM_PARAMETER_MASTER IPM INNER JOIN Design D ON IPM.DESIGN_ID=D.designId 
                                                                    where QUALITY_ID=" + DDQualityName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
            DDDesignName.SelectedValue = Request.QueryString["DesignName"];
            txtid.Text = "0";
           // txtquality.Focus();
            //Fill_Grid();
            lablechange();
            //if (1 == Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select varCompanyType From Mastersetting")))
            //{
            //    TDLossText.Visible = true;
            //    TDLossLabel.Visible = true;
            //}
        }
        lbl.Visible = false;
        Session["ReportPath"] = "Reports/RptfrmQuality.rpt";
        Session["CommanFormula"] = "";
        //
        switch (Convert.ToInt16(Session["varcompanyId"]))
        {                     
            default:
                trLoss.Visible = false;
                trHSCode.Visible = true;
                Trinstruction.Visible = true;
                TrQualityRemark.Visible = true;
                break;
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lblcategory.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDMasterQulaty_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQualityName, "select QualityId,QualityName,Item_id froM Quality where Item_id=" + DDMasterQulaty.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
        //Fill_Grid();
    }
    private void Fill_Grid()
    {
        gdDesign.DataSource = Fill_Grid_Data();
        gdDesign.DataBind();
        //gdDesign.Columns[1].Visible = false;
    }
    private DataSet Fill_Grid_Data()
    {
        string strsql = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            if (1 == Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select varCompanyType From Mastersetting")))
            {
                strsql = @"Select ProdInstId as Sr_No,IM.ITEM_NAME as QualityType,Q.QualityName,D.designName,PI.Loss,PI.HSCode,PI.Instruction,PI.Remark from ProductionInstruction PI 
                            INNER JOIN Quality Q ON PI.QualityId=Q.QualityId INNER JOIN Design D ON PI.DesignId=D.designId
                            INNER JOIN ITEM_MASTER IM ON Q.Item_Id=IM.ITEM_ID Where PI.QualityID="+DDQualityName.SelectedValue+ " and PI.DesignId=" + DDDesignName.SelectedValue + " And PI.MasterCompanyId=" + Session["varCompanyId"] + "  order by Q.QualityId";
            }
            //else
            //{
            //    strsql = "Select QualityId as Sr_No,QualityName as " + lblqualityname.Text + " from Quality Where Item_Id=" + DDMasterQulaty.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by QualityId";
            //}
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmProductionInstruction.aspx");
            // Logs.WriteErrorLog("Masters_Campany_Design|Fill_Grid_Data|" + ex.Message);
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
    protected void gdDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["QualityId"] = "";
        ViewState["DesignId"] = "";
        string id = gdDesign.SelectedDataKey.Value.ToString();
        Session["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"Select ProdInstId as Sr_No,IM.category_id,IM.ITEM_ID,Q.QualityId,D.designId,PI.Loss,PI.HSCode,PI.Instruction,PI.Remark from ProductionInstruction PI 
                                                INNER JOIN Quality Q ON PI.QualityId=Q.QualityId
                                                INNER JOIN Design D ON PI.DesignId=D.designId
                                                INNER JOIN ITEM_MASTER IM ON Q.Item_Id=IM.ITEM_ID where PI.ProdInstId=" + id + " And PI.MasterCompanyid=" + Session["varCompanyId"] + "");
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["Sr_No"].ToString();                
                ddlcategory.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
                DDMasterQulaty.SelectedValue = ds.Tables[0].Rows[0]["ITEM_id"].ToString();
                DDQualityName.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                ViewState["QualityId"] = Convert.ToInt32(DDQualityName.SelectedValue);
                DDDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();
                ViewState["DesignId"] = Convert.ToInt32(DDDesignName.SelectedValue);
                TxtLoss.Text = ds.Tables[0].Rows[0]["Loss"].ToString();
                txthscode.Text = ds.Tables[0].Rows[0]["hscode"].ToString();
                txtinstruction.Text = ds.Tables[0].Rows[0]["instruction"].ToString();
                txtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmProductionInstruction.aspx");
            // Logs.WriteErrorLog("Masters_Campany_Design|Fill_Grid_Data|" + Ex.Message);
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
    protected void gdDesign_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdDesign.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdDesign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdDesign, "select$" + e.Row.RowIndex);
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (DDDesignName.SelectedIndex > 0)
        {
            CheckDuplicateData();
            if (lbl.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[9];
                    _arrPara[0] = new SqlParameter("@ProdInstId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                    _arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@Loss", SqlDbType.Float);
                    _arrPara[4] = new SqlParameter("@HSCode", SqlDbType.VarChar,50);
                    _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);                   
                    _arrPara[6] = new SqlParameter("@Instruction", SqlDbType.NVarChar, 8000);
                    _arrPara[7] = new SqlParameter("@Remark", SqlDbType.NVarChar, 2000);
                    _arrPara[8] = new SqlParameter("@varuserid", SqlDbType.Int);
                    if (btnsave.Text == "Save")
                        _arrPara[0].Value = 0;
                    else
                        _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value =Convert.ToInt32( DDQualityName.SelectedValue);
                    _arrPara[2].Value = Convert.ToInt32(DDDesignName.SelectedValue);                    
                    _arrPara[3].Value = TxtLoss.Text == "" ? "0" : TxtLoss.Text;
                    _arrPara[4].Value = txthscode.Text.ToUpper();
                    _arrPara[5].Value =Convert.ToInt32(Session["varCompanyId"].ToString());                  
                   
                    _arrPara[6].Value = txtinstruction.Text == "" ? "NULL" : txtinstruction.Text.Trim();
                    _arrPara[7].Value = txtRemark.Text == "" ? "NULL" : txtRemark.Text.Trim();
                    _arrPara[8].Value = Convert.ToInt32(Session["varuserid"].ToString());
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_ProductionInstruction", _arrPara);                   
                    TxtLoss.Text = "";
                    //UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER", true, "---Select --");
                    lbl.Visible = true;
                    lbl.Text = "Data Successfully Saved.....";
                    //AllEnums.MasterTables.Quality.RefreshTable();
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmProductionInstruction.aspx");
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
                btndelete.Visible = false;
                //ddlcategory.SelectedValue = null;
                //DDMasterQulaty.SelectedValue = null;
                DDQualityName.SelectedValue = null;
                DDDesignName.SelectedValue = null;
                txthscode.Text = "";               
                txtinstruction.Text = "";
                txtRemark.Text = "";
                txtid.Text = "0";
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
                     
        string strsql = @"Select QualityId,DesignId from ProductionInstruction Where QualityId=" + DDQualityName.SelectedValue + " And DesignId='" + DDDesignName.SelectedValue + "' and ProdInstId!=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl.Visible = true;
            lbl.Text = "Quality and Design AlReady Exists........";
            DDQualityName.SelectedValue = null;
            DDDesignName.SelectedValue = null;            
        }
        else
        {
            lbl.Visible = false;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] Parparam = new SqlParameter[5];
            Parparam[0] = new SqlParameter("@id", Session["id"].ToString());
            Parparam[1] = new SqlParameter("@VarCompanyID", Session["varCompanyId"].ToString());
            Parparam[2] = new SqlParameter("@VarUserID", Session["varuserid"].ToString());
            Parparam[3] = new SqlParameter("@QualityId",Convert.ToInt32(ViewState["QualityId"]));
            Parparam[4] = new SqlParameter("@DesignID",Convert.ToInt32(ViewState["QualityId"]));
            int id = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteProductionInstruction", Parparam);

            if (id > 0)
            {
                lbl.Visible = true;
                lbl.Text = "Record(s) has been deleted!";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Value in Use...";

            }

            # region //Author: Rajeev, Converted into procedure, Date:23 Nov 12

            //int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select QUALITY_ID from ITEM_PARAMETER_MASTER where QUALITY_ID=" + Session["id"].ToString()));
            //if (id <= 0)
            //{
            //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Quality where QUALITYID=" + Session["id"].ToString());
            //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Quality'," + Session["id"].ToString() + ",getdate(),'Delete')");
            //    lbl.Visible = true;
            //    lbl.Text = "Value Deleted.....";
            //}
            //else
            //{
            //    lbl.Visible = true;
            //    lbl.Text = "Value in Use...";

            //}
            #endregion
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmQuality.aspx");
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
       // txtquality.Text = "";
        txtid.Text = "0";
    }

    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDMasterQulaty, "select ITEM_ID, ITEM_NAME froM ITEM_MASTER where CATEGORY_ID=" + ddlcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
    }
    protected void DDQualityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesignName, @"select distinct D.designId,D.designName from ITEM_PARAMETER_MASTER IPM
                                                                INNER JOIN Design D ON IPM.DESIGN_ID=D.designId 
                                                                where Quality_ID=" + DDQualityName.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
    }
    protected void DDDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = @"    SELECT QualityName FROM  Quality where MasterCompanyid=" + Session["varCompanyId"] + "  ORDER BY QualityName";
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
    protected void gdDesign_RowCreated(object sender, GridViewRowEventArgs e)
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