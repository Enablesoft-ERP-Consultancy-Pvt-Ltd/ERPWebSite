using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;

public partial class Masters_Carpet_FrmItemStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS,ITEM_CATEGORY_MASTER IM ,UserRights_Category UC  Where IM.Category_Id=UC.Categoryid And UC.UserId=" + Session["varuserid"] + " And IM.Category_Id=CS.CategoryId And IM.MasterCompanyId= " + Session["varCompanyId"] + "  Order by CATEGORY_NAME", true, "--SELECT--");
        if (DDItemCategory.Items.Count > 0)
            {
                lablechange();
                DDItemCategory.SelectedIndex = 1;
                ddlcategorycange();
                fillCombo();
                fill_grid();
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
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
        string str = "select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+SizeMtr Description,ITEM_FINISHED_ID as finishedid from V_FinishedItemDetail Where CATEGORY_ID=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
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
            str = str + "  and SizeId=" + DDSize.SelectedValue + "";
        }
        str=str +" and status="+DDStatus.SelectedValue+"";
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
                        IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDItemCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
    }
    private void fillCombo()
    {
        if (TdDESIGN.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,designName from Design Where MasterCompanyId=" + Session["varCompanyid"] + " order by designname", true, "--SELECT--");
        }
        if (TDColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyid"] + "  order by ColorName", true, "--SELECT--");
        }
        if (TDShape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from shape Where MasterCompanyid=" + Session["varCompanyId"] + " order by shapename", true, "--SELECT--");
        }
        if (TDSize.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size Where MasterCompanyid=" + Session["varCompanyId"] + "  order by Sizeft", true, "--SELECT--");
        }
        if (TDShadeColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadecolorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorName", true, "--SELECT--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemSelectedChange();
        fill_grid();
    }
    private void ItemSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName", true, "--SELECT--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        shapeselectedindexchange();
        fill_grid();
    }
    private void shapeselectedindexchange()
    {
        UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Sizeft", true, "--SELECT--");
        fill_grid();
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        save_data();
    }
    private void save_data()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlParameter[] _arrpara = new SqlParameter[3];
            _arrpara[0] = new SqlParameter("@ItemFinishedId", SqlDbType.Int);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrpara[0].Value = DGOrderDetail.DataKeys[i].Value;
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Update ITEM_PARAMETER_MASTER set Status="+ddststatuschange.SelectedValue+" where ITEM_FINISHED_ID="+DGOrderDetail.DataKeys[i].Value+" ");
                }
            }
            tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saves Successfully!');", true);
            fill_grid();
        }
        catch (Exception ex)
        {
       
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
    protected void DDStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
}