using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Masters_Carpet_FrmBuyerItemWiseRecipeMaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Customerid,Customercode From Customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by Customercode
                         SELECT CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where ICM.CATEGORY_ID=CS.CATEGORYID And CS.ID=0 And ICM.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CATEGORY_NAME
                         Select ID, Name From RecipeMaster(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " Order By Name";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddBuyerCode, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCategory, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDRecipeName, ds, 2, true, "--Select--");

            if (ddCategory.Items.Count > 0)
            {
                ddCategory.SelectedValue = "1";
                ddlcategorycange();
            }
        }
    }
    protected void ddBuyerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        Fill_Grid();
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        dsn.Visible = false;

        UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= " + ddCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name", true, "--Select--");

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                        FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] 
                        Where [CATEGORY_ID]=" + ddCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"] + @" 
                        Select DesignID, DesignName From Design(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " Order By DesignName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref ddDesign, ds, 1, true, "--Select--");
                        break;
                }
            }
        }
    }
    protected void ddItemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddQuality, "SELECT QUALITYID, QUALITYNAME FROM QUALITY(Nolock) WHERE ITEM_ID = " + ddItemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME ", true, "--Select--");
        Fill_Grid();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            SqlParameter[] _arrPara = new SqlParameter[16];
            _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@CustomerID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@CategoryID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@ItemID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@QualityID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@DesignID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@RecipeID", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddBuyerCode.SelectedValue;
            _arrPara[2].Value = ddCategory.SelectedValue;
            _arrPara[3].Value = ddItemname.SelectedValue;
            _arrPara[4].Value = ql.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            _arrPara[5].Value = dsn.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            _arrPara[6].Value = DDRecipeName.SelectedValue;
            _arrPara[7].Value = Session["varuserid"].ToString();
            _arrPara[8].Value = Session["varCompanyId"].ToString();
            _arrPara[9].Direction = ParameterDirection.InputOutput;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_Save_CustomerItemRecipeDefineMaster]", _arrPara);

            Tran.Commit();
            Fill_Grid();
            ddQuality.SelectedIndex = 0;
            ddDesign.SelectedIndex = 0;
            DDRecipeName.SelectedIndex = 0;
            ddQuality.Focus();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/DefineItemCode.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void Fill_Grid()
    {
        if (ddBuyerCode.SelectedIndex > 0 && ddCategory.SelectedIndex > 0 && ddItemname.SelectedIndex > 0)
        {
            DGRecipeRateMaster.DataSource = Fill_Grid_Data();
        }
        else
        {
            DGRecipeRateMaster.DataSource = null;
        }
        DGRecipeRateMaster.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"Select a.ID, CI.CustomerCode, ICM.CATEGORY_NAME CategoryName, IM.ITEM_NAME ItemName, IsNull(Q.QualityName, '') QualityName, 
                        IsNull(D.DesignName, '') DesignName, RM.Name RecipeName 
                        From CustomerItemRecipeDefineMaster a
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = a.CustomerId 
                        JOIN ITEM_CATEGORY_MASTER ICM(Nolock) ON ICM.CATEGORY_ID = a.CategoryID 
                        JOIN ITEM_MASTER IM(Nolock) ON IM.ITEM_ID = a.ItemID 
                        JOIN RecipeMaster RM(Nolock) ON RM.ID = a.RecipeID 
                        LEFT JOIN Quality Q(Nolock) ON Q.QualityID = a.QualityID 
                        LEFT JOIN Design D(Nolock) ON D.DesignID = a.DesignID
                        Where a.MasterCompanyId = " + Session["varCompanyId"] + " And a.CustomerId = " + ddBuyerCode.SelectedValue;
            if (ddCategory.SelectedIndex > 0)
            {
                strsql = strsql + " And a.CategoryID = " + ddCategory.SelectedValue;
            }
            if (ddItemname.SelectedIndex > 0)
            {
                strsql = strsql + " And a.ItemID = " + ddItemname.SelectedValue;
            }

            strsql = strsql + " Order BY CI.CustomerCode, ICM.CATEGORY_NAME, IM.ITEM_NAME, IsNull(Q.QualityName, ''), IsNull(D.DesignName, ''), RM.Name ";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmBuyerItemWiseRecipeMaster.aspx");
            Logs.WriteErrorLog("Masters_Carpet_FrmBuyerItemWiseRecipeMaster|Fill_Grid_Data|" + ex.Message);
        }
        return ds;
    }

    protected void DGRecipeRateMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(DGRecipeRateMaster.DataKeys[e.RowIndex].Value);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            SqlParameter[] _arrPara = new SqlParameter[16];
            _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = ID;
            _arrPara[1].Value = Session["varuserid"].ToString();
            _arrPara[2].Value = Session["varCompanyId"].ToString();
            _arrPara[3].Direction = ParameterDirection.InputOutput;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_Delete_CustomerItemRecipeDefineMaster]", _arrPara);

            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();

            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmBuyerItemWiseRecipeMaster.aspx/Delete");
            Logs.WriteErrorLog("Masters_Carpet_FrmBuyerItemWiseRecipeMaster|DeleteData|" + ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete CustomerItemRecipeDefineMaster Where ID = " + ID);
        Fill_Grid();
    }
    protected void DGRecipeRateMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRecipeRateMaster, "Select$" + e.Row.RowIndex);
        }
    }
}