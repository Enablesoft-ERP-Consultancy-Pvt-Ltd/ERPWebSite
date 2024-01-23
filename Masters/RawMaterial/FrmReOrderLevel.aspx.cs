using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;

public partial class Masters_RawMaterial_FrmReOrderLevel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "select UnitId,UnitName from Unit Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by UnitId", true, "--SELECT--");
            if (Session["varcompanyno"].ToString() == "7")
            {
                UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS,ITEM_CATEGORY_MASTER IM ,UserRights_Category UC  Where IM.Category_Id=UC.Categoryid And cs.id=1 And UC.UserId=" + Session["varuserid"] + " And IM.Category_Id=CS.CategoryId  And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_NAME", true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS,ITEM_CATEGORY_MASTER IM ,UserRights_Category UC  Where IM.Category_Id=UC.Categoryid And UC.UserId=" + Session["varuserid"] + " And IM.Category_Id=CS.CategoryId And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order by CATEGORY_NAME", true, "--SELECT--");
            }
            if (DDItemCategory.Items.Count > 0)
            {
                lablechange();
                HDF1.Value = Session["varcompanyno"].ToString();
                DDItemCategory.SelectedIndex = 1;
                TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
                ddlcategorycange();
                fillCombo();
                fill_grid();
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        LblShadeColor.Text = ParameterList[7];
    }
    private void fill_grid()
    {
        string str = "select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+SizeMtr Description,v.ITEM_FINISHED_ID as finishedid,isnull(r.MinStockQty,0) as qty from V_FinishedItemDetail v left outer join Reorderqty r On v.ITEM_FINISHED_ID=r.Item_Finished_id Where CATEGORY_ID=" + DDItemCategory.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (DDItemName.Visible == true && DDItemName.SelectedIndex > 0)
        {
            str = str + " and ITEM_ID=" + DDItemName.SelectedValue + "";
        }
        if (DDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " and QualityId=" + DDQuality.SelectedValue + "";
        }
        if (DDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            str = str + " and designId=" + DDDesign.SelectedValue + "";
        }
        if (DDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            str = str + " and ColorId=" + DDColor.SelectedValue + "";
        }
        if (DDShape.Visible == true && DDShape.SelectedIndex > 0)
        {
            str = str + " and ShapeId=" + DDShape.SelectedValue + "";
        }
        if (ddshadecolor.Visible == true && ddshadecolor.SelectedIndex > 0)
        {
            str = str + " and ShadecolorId=" + ddshadecolor.SelectedValue + "";
        }
        if (DDSize.Visible == true && DDSize.SelectedIndex > 0)
        {
            str = str + " and SizeId=" + " and SizeId=" + DDSize.SelectedValue + "";
        }
        str = str + " and v.status=1";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderDetail.DataSource = ds;
        DGOrderDetail.DataBind();
    }
    protected void DDItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        fillCombo();
        fill_grid();
    }
    private void ddlcategorycange()
    {
        TDQuality.Visible = false;
        TdDESIGN.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDShadeColor.Visible = false;
        TDSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                        FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                        IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDItemCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TdDESIGN.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                    case "6":
                        TDShadeColor.Visible = true;
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "---SELECT----");
        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
    }
    private void fillCombo()
    {
        if (TdDESIGN.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");
        }
        if (TDColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--SELECT--");
        }
        if (TDShape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--SELECT--");
        }
        if (TDShadeColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorname from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShadeColorName", true, "--SELECT--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemSelectedChange();
        fill_grid();
    }
     private void ItemSelectedChange()
     {
         UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "--SELECT--");
         TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue;
     }
     protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
     {
         shapeselectedindexchange();
         fill_grid();
     }
     private void shapeselectedindexchange()
     {
         if (DDShape.SelectedIndex > 0)
         {
             if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
             {
                 UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
             }
             else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
             {
                 UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
             }
             else
             {
                 UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
             }
             btnAddSize.Enabled = true;
         }
         else
         {
             btnAddSize.Enabled = false;
         }
     }
     protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
     {
         fill_grid(); 
     }
     protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
     {
         fill_grid();
     }
     protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
     {
         fill_grid();
     }
     protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
     {
         fill_grid();
     }
     protected void ddshadecolor_SelectedIndexChanged(object sender, EventArgs e)
     {
         fill_grid();
     }
     protected void refreshcategory_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order by CATEGORY_NAME", true, "--SELECT--");
     }
     protected void refreshquality_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "--SELECT--");
     }
     protected void refreshdesign_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");
     }
     protected void refreshcolor_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--SELECT--");
     }
     protected void refreshshade_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorname from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShadeColorname", true, "--SELECT--");
     }
     protected void refreshshape_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--SELECT--");
     }
     protected void refreshsize_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
     }
     protected void BtnRefreshItem_Click(object sender, EventArgs e)
     {
         UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name ", true, "---SELECT----");
     }
     protected void BtnSave_Click(object sender, EventArgs e)
     {
         save_data();
     }
     private void save_data()
     {
         LblErrorMessage.Text = "";
         LblErrorMessage.Visible = false;
         try
         {
             int ItemFinishedId = 0;
             SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
             SqlParameter[] _arrpara = new SqlParameter[3];
             _arrpara[0] = new SqlParameter("@ItemFinishedId", SqlDbType.Int);
             _arrpara[1] = new SqlParameter("@Qty", SqlDbType.Float);
             con.Open();
             SqlTransaction tran = con.BeginTransaction();
             if (TxtQuantity.Text != "" && TxtQuantity.Text != "0")
             {
                 ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                 _arrpara[0].Value = ItemFinishedId;
                 _arrpara[1].Value = TxtQuantity.Text;
                 SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_ReOrderLevel", _arrpara);
             }
             for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
             {
                 double qty = Convert.ToDouble(((TextBox)DGOrderDetail.Rows[i].FindControl("Txtqty")).Text);
                 if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true && qty > 0)
                 {
                     ItemFinishedId = Convert.ToInt32(((Label)DGOrderDetail.Rows[i].FindControl("lblfinished")).Text);
                     _arrpara[0].Value = ItemFinishedId;
                     _arrpara[1].Value = qty;
                     SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_ReOrderLevel", _arrpara);
                 }
             }
             tran.Commit();
             TxtQuantity.Text = "";
             LblErrorMessage.Text = "Data Saved Successfully";
             LblErrorMessage.Visible = true;
             if(ddshadecolor.Items.Count>0)
             {
                 ddshadecolor.SelectedIndex = 0;
             }
             fill_grid();
         }
         catch (Exception ex)
         {
             LblErrorMessage.Text = ex.Message;
             LblErrorMessage.Visible = true;
         }
     }
     protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
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