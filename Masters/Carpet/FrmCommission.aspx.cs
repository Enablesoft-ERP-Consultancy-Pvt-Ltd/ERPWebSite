using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_FrmCommission : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Category_Name", true, "--SELECT--");
            ddCategoryName.Focus();
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedIndex = 1;
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.Focus();
            }
            lablechange();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        Quality.Visible = false;
        Design.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Quality.Visible = true;
                        break;
                    case "2":
                        Design.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By DESIGNNAME", true, "--ALL--");
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddQuality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By QUALITYNAME", true, "--SELECT--");
        FillGrid();
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[8];
            _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@CategoryID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ItemID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@QualityID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@DesignID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@Commission", SqlDbType.Float);
            _arrPara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@varCompanyId", SqlDbType.Int);

            //Select ID,CategoryID,ItemID,QualityID,DesignID,Commission,UserID,MasterCompanyID From Commission

            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddCategoryName.SelectedValue;
            _arrPara[2].Value = ddItemName.SelectedValue;
            _arrPara[3].Value = ddQuality.SelectedValue;
            _arrPara[4].Value = Design.Visible == false ? "0" : ddDesign.SelectedIndex == 0 ? "-1" : ddDesign.SelectedValue;
            _arrPara[5].Value = TxtCommission.Text;
            _arrPara[6].Value = Session["varuserid"].ToString();
            _arrPara[7].Value = Session["varMasterCompanyIDForERP"].ToString();
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Commission", _arrPara);
            LblErrorMessage.Text = "Save Details.....";
            SaveRef();
            FillGrid();
            BtnSave.Text = "Save";            
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmCommission.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("Masters_Carpet_Commission|BtnSave_Click|" + ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void SaveRef()
    {
        if (Design.Visible == true)
        {
            ddQuality.SelectedIndex = 0;
            ddDesign.SelectedIndex = 0;
            TxtCommission.Text = "";
            ddQuality.Focus();
        }
    }
    private void FillGrid()
    {
        string STR = "";
        DG.DataSource = "";
        STR = @"Select ID,CATEGORY_NAME Category,ITEM_NAME ItemName,QualityName Quality,Case When C.DesignId=-1 Then 'All' Else Case When C.DesignId=0 Then '' Else designName End End Design,Commission 
                From Commission C INNER JOIN ITEM_CATEGORY_MASTER ICM ON C.CategoryID=ICM.CATEGORY_ID INNER JOIN ITEM_MASTER IM ON C.ItemID=IM.ITEM_ID INNER JOIN 
                Quality Q ON C.QualityId=Q.QualityId Left Outer Join Design D ON C.DesignId=D.DesignId Where C.CategoryID=" + ddCategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddItemName.SelectedIndex > 0)
        {
            STR = STR + @" AND C.ItemID=" + ddItemName.SelectedValue + "";
        }
        if (ddQuality.SelectedIndex > 0)
        {
            STR = STR + @" AND C.QualityID=" + ddQuality.SelectedValue + "";
        }
        //if (Design.Visible == false)
        //{
        //    STR = STR + @" AND C.DesignID=0";
        //}
        //else if (ddDesign.SelectedItem.Text == "All" && ddDesign.Items.Count>0)
        //{
        //    STR = STR + @" AND C.DesignID=-1";
        //}
        //else
        //{
        //    STR = STR + @" AND C.DesignID=" + ddDesign.SelectedValue;
        //}
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
        DG.DataSource = Ds;
        DG.DataBind();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int VarID = Convert.ToInt32(DG.DataKeys[e.RowIndex].Value);
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete From Commission Where Id=" + VarID);
        FillGrid();
    }
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
}