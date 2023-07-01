using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

public partial class Masters_Campany_ItemName : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            lablechange();
            Fill_Combo();
            if (variable.Carpetcompany == "1")
            {
                tr2.Visible = true;
                TDkatiwithexportsize.Visible = true;
                BindItemType();
            }
            else
            {
                tr2.Visible = false;
            }
            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 20:
                    TDMasterQualityType.Visible = true;
                    TDMasterQualityType2.Visible = true;
                    break;
                case 16:
                    chkpretreat.Visible = true;
                    //  chkdyesstuff.Visible = true;
                    chkchem.Visible = true;
                    break;
                case 28:
                    chkpretreat.Visible = true;
                    //  chkdyesstuff.Visible = true;
                    chkchem.Visible = true;
                    break;
                default:
                    TDMasterQualityType.Visible = false;
                    TDMasterQualityType2.Visible = false;
                    break;
            }
        }
        Lblerr.Visible = false;
    }

    protected void BindItemType()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();

            DataTable dt = DataAccess.fetch("Pro_ItemType", param);

            ddlItemType.DataValueField = "ID";
            ddlItemType.DataTextField = "ItemType";
            ddlItemType.DataSource = dt;
            ddlItemType.DataBind();
            ddlItemType.Items.Insert(0, "-");
            ddlItemType.Items[0].Value = "-1";
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            //lblmsg.Text = ex.Message;
            con.Close();
        }
    }
    public void lablechange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            String[] ParameterList = new String[8];
            ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
            lblcategoryname.Text = ParameterList[5];
            lblitemname.Text = ParameterList[6];
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddItemName.aspx");
            Lblerr.Visible = true;
            Lblerr.Text = "Data base error..................";
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

    private void Fill_Combo()
    {
        #region on 03-Dec-2012
        //UtilityModule.ConditionalComboFill(ref ddCategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,UserRights_Category UC where IM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + " Order by CATEGORY_Id", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref ddUnit, "Select UnitTypeID,UnitType from UNIT_TYPE_MASTER Order by UnitTypeID", true, "--Select--");
        #endregion
        string str = @"Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,UserRights_Category UC 
                    where IM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + @" 
                    Order by Category_Name
                    Select UnitTypeID,UnitType from UNIT_TYPE_MASTER  Order by UnitTypeID 
                    select ID,ItemType from ItemType";
        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref ddCategory, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddUnit, ds, 1, true, "--Select--");
        if (ddUnit.Items.Count > 0)
        {
            ddUnit.SelectedValue = "1";
        }
        if (ddCategory.Items.Count > 0)
        {
            ddCategory.SelectedValue = "1";
            Fill_Grid();
        }
        UtilityModule.ConditionalComboFillWithDS(ref ddlItemType, ds, 2, true, "--Select--");

        if (Session["varCompanyId"].ToString() == "20")
        {
            string str2 = @"Select MasterQualityTypeId,MasterQualityTypeName from MASTERQUALITYTYPE where MasterCompanyId=" + Session["varCompanyId"] + @" Order by MasterQualityTypeName";
            DataSet ds2 = SqlHelper.ExecuteDataset(str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDMasterQualityType, ds2, 0, true, "--SELECT--");
        }
    }

    protected void gdItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdItem.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }

    protected void gdItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_detail_back();
    }

    private void fill_detail_back()
    {
        try
        {
            DataSet ds;
            string sqlstr = @" Select CATEGORY_ID, UnitTypeID, ITEM_NAME, ITEM_CODE, FlagFixWeight, ItemType, isnull(KATIWITHEXPORTSIZE,0) KATIWITHEXPORTSIZE,
            IsNull(MasterQualityTypeId, 0) MasterQualityTypeId, CUSHIONTYPEITEM, pretreament, dyechem, PassSize 
            From ITEM_MASTER(Nolock) 
            Where Item_Id = " + gdItem.SelectedValue + " And MasterCompanyid = " + Session["varCompanyId"];

            ViewState["ItemId"] = gdItem.SelectedValue;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
            ddCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();

            if (variable.Carpetcompany == "1")
            {
                if (ddlItemType.Items.FindByValue(ds.Tables[0].Rows[0]["ItemType"].ToString()) != null)
                {
                    ddlItemType.SelectedValue = ds.Tables[0].Rows[0]["ItemType"].ToString();
                }
            }
            ddUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitTypeID"].ToString();
            txtItemName.Text = ds.Tables[0].Rows[0]["ITEM_NAME"].ToString();
            txtItemCode.Text = ds.Tables[0].Rows[0]["ITEM_CODE"].ToString();
            btnsave.Text = "Update";
            txtid.Text = gdItem.SelectedValue.ToString();
            btndelete.Visible = true;
            ChkFlagFixWeight.Checked = false;

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["FlagFixWeight"]) == 1)
            {
                ChkFlagFixWeight.Checked = true;
            }
            if (TDkatiwithexportsize.Visible == true)
            {
                chkkatiwithexportsize.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["KATIWITHEXPORTSIZE"]);
            }

            if (TDMasterQualityType.Visible == true && TDMasterQualityType2.Visible == true)
            {
                DDMasterQualityType.SelectedValue = ds.Tables[0].Rows[0]["MasterQualityTypeId"].ToString();
            }
            ChkForCushionTypeItem.Checked = false;
            if (ds.Tables[0].Rows[0]["CUSHIONTYPEITEM"].ToString() == "1")
            {
                ChkForCushionTypeItem.Checked = true;
            }
            chkpretreat.Checked = false;
            if (ds.Tables[0].Rows[0]["pretreament"].ToString() == "1")
            {
                chkpretreat.Checked = true;
            }
            chkchem.Checked = false;
            if (ds.Tables[0].Rows[0]["dyechem"].ToString() == "1")
            {
                chkchem.Checked = true;
            }
            ChkForSizeWiseConsumption.Checked = false;
            ChkForAllDesignColorSizeWiseConsumption.Checked = false;

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["PassSize"]) == 1)
            {
                ChkForSizeWiseConsumption.Checked = true;
            }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["PassSize"]) == 3)
            {
                ChkForAllDesignColorSizeWiseConsumption.Checked = true;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ItemName.aspx");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        validate();
        if (Lblerr.Text != "Importent Field Missing.........")
        {
            CheckDuplicateDate();
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
                    SqlParameter[] _arrPara = new SqlParameter[15];
                    _arrPara[0] = new SqlParameter("@ITEM_Id", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@CATEGORY_ID", SqlDbType.Int);
                    // _arrPara[2] = new SqlParameter("@ITEM_PARAMETER_ID", SqlDbType.Int);
                    _arrPara[2] = new SqlParameter("@UnitTypeID", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@ITEM_Name", SqlDbType.VarChar, 150);
                    _arrPara[4] = new SqlParameter("@ITEM_Code", SqlDbType.VarChar, 150);
                    _arrPara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[6] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrPara[7] = new SqlParameter("@FlagType", SqlDbType.Int);
                    _arrPara[8] = new SqlParameter("@ItemType", SqlDbType.Int);
                    _arrPara[9] = new SqlParameter("@Katiwithexportsize", SqlDbType.Int);
                    _arrPara[10] = new SqlParameter("@MasterQualityTypeId", SqlDbType.Int);
                    _arrPara[11] = new SqlParameter("@CushionTypeItem", SqlDbType.Int);
                    _arrPara[12] = new SqlParameter("@Pretreatment", SqlDbType.Int);
                    _arrPara[13] = new SqlParameter("@dyechem", SqlDbType.Int);
                    _arrPara[14] = new SqlParameter("@PassSize", SqlDbType.Int);

                    _arrPara[1].Value = ddCategory.SelectedValue;
                    //_arrPara[2].Value = ddItemParameter.SelectedValue;
                    _arrPara[2].Value = ddUnit.SelectedValue;
                    _arrPara[3].Value = txtItemName.Text.ToUpper();
                    _arrPara[4].Value = txtItemCode.Text.ToUpper();
                    _arrPara[5].Value = Session["varuserid"].ToString();
                    _arrPara[6].Value = Session["varCompanyId"].ToString();
                    _arrPara[7].Value = ChkFlagFixWeight.Checked == true ? 1 : 0;
                    _arrPara[8].Value = variable.Carpetcompany == "1" ? ddlItemType.SelectedValue : "-1";
                    _arrPara[9].Value = TDkatiwithexportsize.Visible == false ? "0" : (chkkatiwithexportsize.Checked == true ? "1" : "0");
                    _arrPara[10].Value = TDMasterQualityType.Visible == false ? "0" : (DDMasterQualityType.SelectedIndex > 0 ? DDMasterQualityType.SelectedValue : "0");
                    _arrPara[11].Value = ChkForCushionTypeItem.Checked == true ? "1" : "0";
                    _arrPara[12].Value = chkpretreat.Checked == true ? "1" : "0";
                    _arrPara[13].Value = chkchem.Checked == true ? "1" : "0";
                    _arrPara[14].Value = ChkForSizeWiseConsumption.Checked == true ? "1" : ChkForAllDesignColorSizeWiseConsumption.Checked == true ? "3" : "0";

                    if (btnsave.Text == "Update")
                    {
                        _arrPara[0].Value = txtid.Text;
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATE_ITEM", _arrPara);
                        btnsave.Text = "Save";
                        txtid.Text = "0";
                    }
                    else
                    {
                        _arrPara[0].Direction = ParameterDirection.Output;

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_ITEM1", _arrPara);
                        txtid.Text = _arrPara[0].Value.ToString();
                    }
                    //ddCategory.SelectedIndex = -1;
                    //ddItemParameter.SelectedIndex = -1;
                    //ddUnit.SelectedIndex = -1;
                    Tran.Commit();
                    txtItemName.Text = "";
                    txtItemCode.Text = "";
                    chkkatiwithexportsize.Checked = false;
                    btnsave.Text = "Save";
                    btndelete.Visible = false;
                    Lblerr.Visible = true;
                    Lblerr.Text = "Save Details....";
                    ChkForCushionTypeItem.Checked = false;
                    ChkForSizeWiseConsumption.Checked = false;
                    ChkForAllDesignColorSizeWiseConsumption.Checked = false;
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    Lblerr.Visible = true;
                    Lblerr.Text = ex.Message;
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ItemName.aspx");
                    Logs.WriteErrorLog("Masters_Campany_ItemName|cmdSave_Click|" + ex.Message);
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
        }
        else
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Important Fields are Missing.........";
        }
    }


    //***********************************************************************************************//
    private void validate()
    {
        if (txtItemName.Text == "" || ddCategory.SelectedIndex < 1 || ddUnit.SelectedIndex < 1)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Important Fields are  Missing.........";
        }
        else
        {
            Lblerr.Text = "";
            Lblerr.Visible = false;
        }

    }

    private void CheckDuplicateDate()
    {
        //int id = 0;
        DataSet ds = null;
        string strsql = @"Select * from ITEM_MASTER Where Category_Id=" + ddCategory.SelectedValue + " And Item_Name='" + txtItemName.Text + "' and ITEM_ID !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        ds = SqlHelper.ExecuteDataset(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Item Name AlReady Exits........";
            txtItemName.Text = "";
            txtItemName.Focus();
        }
        else
        {
            Lblerr.Visible = false;
        }
    }

    //************************************************************************************************//
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtItemName_AutoCompleteExtender.ContextKey = ddCategory.SelectedValue;
        if (Session["varcompanyno"].ToString() == "7" && ddCategory.SelectedValue == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddUnit, " Select UnitTypeID,UnitType from UNIT_TYPE_MASTER where UnitTypeID in(1,2) Order by UnitTypeID", true, "-Select Unit-");
        }
        Fill_Grid();

        btndelete.Visible = false;
        btnsave.Text = "Save";
        txtItemName.Text = "";
        txtItemCode.Text = "";
        txtid.Text = "0";
    }

    private void Fill_Grid()
    {
        gdItem.DataSource = Fill_Grid_Data();
        gdItem.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = "SELECT I.ITEM_ID as Sr_No,IM.CATEGORY_NAME as '" + lblcategoryname.Text.ToString() + "' ,I.ITEM_NAME as '" + lblitemname.Text.ToString() + @"' ,
            U.unittype as UNIT_TYPE,I.ITEM_CODE as Item_Code, IT.ItemType as Item_Type,isnull(MQT.MasterQualityTypeName,'') as MasterQualityTypeName 
            FROM ITEM_CATEGORY_MASTER IM INNER JOIN ITEM_MASTER I ON IM.CATEGORY_ID=I.CATEGORY_ID 
            INNER JOIN UNIT_TYPE_MASTER U ON I.UnitTypeID=U.UnittypeID LEFT JOIN ItemType IT ON I.ItemType=IT.ID 
            LEFT JOIN MasterQualityType MQT ON I.MasterQualityTypeId=MQT.MasterQualityTypeId
            Where IM.Category_Id=" + ddCategory.SelectedValue + "  Order by I.ITEM_NAME ";
            ds = SqlHelper.ExecuteDataset(strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ItemName.aspx");
            Logs.WriteErrorLog("Masters_Campany_ItemName|Fill_Grid_Data|" + ex.Message);
        }
        return ds;
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
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@ITEM_ID", ViewState["ItemId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteITEM_MASTER", _array);
            Tran.Commit();
            Lblerr.Visible = true;
            Lblerr.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblerr.Visible = true;
            Lblerr.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ItemName.aspx");
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
        txtItemName.Text = "";
        txtItemCode.Text = "";
        txtid.Text = "0";
    }
    protected void gdItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdItem, "select$" + e.Row.RowIndex);

            if (variable.Carpetcompany == "1")
            {
                // e.Row.Cells[5].Visible = true;
                e.Row.Cells[5].Style.Add("display", "block");
                gdItem.HeaderRow.Cells[5].Style.Add("display", "block");

                if (Session["VarCompanyId"].ToString() == "20")
                {
                    e.Row.Cells[6].Visible = true;
                }
                else
                {
                    e.Row.Cells[6].Visible = false;
                }
            }
            else
            {

                e.Row.Cells[5].Style.Add("display", "none");
                gdItem.HeaderRow.Cells[5].Style.Add("display", "none");
                e.Row.Cells[6].Visible = false;
            }
        }
    }
    protected void gdItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        //e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //e.Row.RowState == DataControlRowState.Alternate)
        //e.Row.CssClass = "alternate";
    }
}