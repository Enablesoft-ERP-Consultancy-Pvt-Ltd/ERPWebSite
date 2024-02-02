using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Net.Mail;
public partial class Masters_Order_FrmLocalConsumptionForOrder : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    int sizeUnit;

    protected void Page_Load(object sender, EventArgs e)
    {
        MailMessage m = new MailMessage();

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            if (Request.QueryString["finishedid"] != "")
            {
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select   CATEGORY_NAME+'   '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShapeName+' '+ShadeColorName from V_FinishedItemDetail where item_finished_id=" + Request.QueryString["finishedid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
                Itemdescrip.Text = ds.Tables[0].Rows[0][0].ToString() + "   Qty=" + Request.QueryString["Qty"];
            }
            UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS,UserRights_Category UC Where IM.Category_Id=CS.CategoryId And CS.Id<>0 And UC.CategoryId=IM.Category_Id And UC.UserId=" + Session["varuserid"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_NAME", true, "--SELECT--");
            if (DDItemCategory.Items.Count > 0)
            {
                DDItemCategory.SelectedValue = "1";
                dditemcategorychange();
            }
            lablechange();

            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    tdsidegrid.Visible = true;
                    Fill_Grid();
                    filldata();
                    fillsidegrid();
                    break;
                case "3":
                    tdsidegrid.Visible = true;
                    Fill_Grid();
                    filldata();
                    //fillsidegrid();
                    break;
                case "10":
                    tdsidegrid.Visible = true;
                    Fill_Grid();
                    filldata();
                    //fillsidegrid();
                    break;
                default:
                    tdsidegrid.Visible = true;
                    Fill_Grid();
                    filldata();
                    //fillsidegrid();
                    break;
            }

        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        LblDesignName.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        LblShadeColor.Text = ParameterList[7];
    }
    protected void DDItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        dditemcategorychange();
        switch (Session["varcompanyNo"].ToString())
        {
            case "7":
                fillsidegrid();
                break;
            case "3":
                fillsidegrid();
                break;
            case "10":
                fillsidegrid();
                break;


        }

    }
    private void dditemcategorychange()
    {
        ddlcategorycange();
        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
        fillCombo();
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "---SELECT----");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDItemNamechange();
        if (Session["Varcompanyno"].ToString() == "7")
        {
            fillsidegrid();
        }
    }
    private void DDItemNamechange()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref DDUnitName, "SELECT UnitID,UNitName from Unit U,Item_Master IM  Where U.UnitTypeId=IM.UnitTypeId And  Item_Id=" + DDItemName.SelectedValue + " And U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        if (DDUnitName.Items.Count > 0)
        {
            DDUnitName.SelectedIndex = 1;
        }
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDshapechange();
    }
    private void DDshapechange()
    {
        UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            SaveDetail();
        }
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "7":
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "closeform();", true);
                break;

        }

        //BtnSave_Click(sender, e);
    }
    protected void DGConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGConsumption, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGConsumption_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete From OrderLocalConsumption where ID=" + DGConsumption.DataKeys[e.RowIndex].Value);
        Fill_Grid();
    }
    protected void DGConsumption_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillOrderBack();
    }
    private void ddlcategorycange()
    {
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
                        TDDesign.Visible = true;
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
                        TDShade.Visible = true;
                        break;
                    case "10":
                        TDColor.Visible = true;
                        break;
                }
            }
        }
    }
    private void fillCombo()
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShadeColorName", true, "--SELECT--");
    }
    protected void ChMeteerSize_CheckedChanged(object sender, EventArgs e)
    {
        if (ChFtSize.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemCategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        if (TDQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
        }
        if (TDDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
            {
                goto a;
            }
        }
        if (TDColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
            {
                goto a;
            }
        }
        if (TDShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshadecolor) == false)
            {
                goto a;
            }
        }
        if (TDShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDShape) == false)
            {
                goto a;
            }
        }
        if (TDSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnitName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtQty) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    protected void SaveDetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[11];
            _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Finishedid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@UnitId", SqlDbType.Float);
            _arrpara[3] = new SqlParameter("@Qty", SqlDbType.Float);
            _arrpara[4] = new SqlParameter("@SizeUnit", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@DetailID", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@orderdetailid", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@Remark", SqlDbType.VarChar, 250);
            _arrpara[10] = new SqlParameter("@thanlength", SqlDbType.Float);
            ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtQty, ddshadecolor, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            _arrpara[0].Value = Request.QueryString["Orderid"];
            _arrpara[1].Value = ItemFinishedId;
            _arrpara[2].Value = DDUnitName.SelectedValue;
            _arrpara[3].Value = TxtQty.Text;
            _arrpara[6].Value = Session["varuserid"].ToString();
            _arrpara[7].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrpara[8].Value = Request.QueryString["orderdetailid"];
            _arrpara[9].Value = txtremark.Text;
            _arrpara[10].Value = txtthanlength.Text != "" ? Convert.ToDouble(txtthanlength.Text) : 0;
            if (ChFtSize.Checked == true)
            {
                sizeUnit = 2; //2 For Ft size
            }
            else
            {
                sizeUnit = 1;
            }
            if (BtnSave.Text == "Update")
            {
                _arrpara[5].Value = Session["DetailId"];
            }
            else
            {
                _arrpara[5].Value = 0;
            }
            _arrpara[4].Value = sizeUnit;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_OrderLocalConsumption", _arrpara);
            Tran.Commit();
            LblErrorMessage.Text = "Data Saved Successfully";
            LblErrorMessage.Visible = true;
            BtnSave.Text = "Save";
            refreshform();
            Fill_Grid();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmLocalConsumptionForOrder.aspx");
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Fill_Grid()
    {
        DGConsumption.DataSource = GetDetail();
        DGConsumption.DataBind();
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"SELECT ID,Category_Name +'  '+VF.ITEM_NAME+'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
                            CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,U1.Unitname As Unit,Qty,Od.thanlength,od.remark FROM OrderLocalConsumption OD Inner JOIN V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id 
                            INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id INNER JOIN Unit U On OD.SizeUnit=U.UnitId inner Join Unit U1 on OD.UnitId=U1.UnitId
                            Where OrderId=" + Request.QueryString["Orderid"] + " And OrderDetailid=" + Request.QueryString["OrderDetailid"] + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmLocalConsumptionForOrder.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds;
    }
    private void refreshform()
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        ChFtSize.Checked = false;
        if (TDDesign.Visible == true)
        {
            DDDesign.SelectedIndex = 0;
        }
        if (TDColor.Visible == true)
        {
            DDColor.SelectedIndex = 0;
        }
        if (TDShape.Visible == true)
        {
            DDShape.SelectedIndex = 0;
        }
        if (TDShade.Visible == true)
        {
            ddshadecolor.SelectedIndex = 0;
        }
        if (TDSize.Visible == true)
        {
            DDSize.SelectedIndex = 0;
        }
        //TxtQty.Text = "";
    }
    private void fillOrderBack()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            BtnSave.Text = "Update";
            string sql = @"SELECT CATEGORY_ID,Item_ID,QUALITYID,DESIGNID,COLORID,SHAPEID,SIZEID,ShadeColorId,U.UnitId,SizeUnit,Qty,od.thanlength,od.remark from OrderLocalConsumption Od,
                           V_FinishedItemDetail VF,Unit U where VF.Item_Finished_Id=Od.FinishedId and OD.UnitId=U.UnitId ANd 
                           Od.ID=" + DGConsumption.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtQty.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "---SELECT----");
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["Item_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDUnitName, "SELECT UnitID,UNitName from Unit U,Item_Master IM  Where U.UnitTypeId=IM.UnitTypeId And  Item_Id=" + DDItemName.SelectedValue + " And U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by UnitName", true, "--SELECT--");
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITYID"].ToString();
                DDUnitName.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDColor, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorname", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShadeColorName", true, "--SELECT--");
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGNID"].ToString();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLORID"].ToString();
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPEID"].ToString();
                ddshadecolor.SelectedValue = ds.Tables[0].Rows[0]["ShadeColorId"].ToString();
                txtthanlength.Text = ds.Tables[0].Rows[0]["thanlength"].ToString();
                txtremark.Text = ds.Tables[0].Rows[0]["remark"].ToString();
                Session["DetailId"] = DGConsumption.SelectedValue;
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["SizeUnit"]) == 1)
                {
                    UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
                }
                else
                {
                    ChFtSize.Checked = true;
                    UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
                }
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZEID"].ToString();
                if (Convert.ToInt32(DDQuality.SelectedIndex) > 0)
                {
                    TDQuality.Visible = true;
                }
                else
                {
                    TDQuality.Visible = false;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    TDDesign.Visible = true;
                }
                else
                {
                    TDDesign.Visible = false;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    TDColor.Visible = true;
                }
                else
                {
                    TDColor.Visible = false;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    TDShape.Visible = true;
                }
                else
                {
                    TDShape.Visible = false;
                }
                if (ddshadecolor.SelectedIndex > 0)
                {
                    TDShade.Visible = true;
                }
                else
                {
                    TDShade.Visible = false;
                }
                LblErrorMessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmLocalConsumptionForOrder.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
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
    protected void DGConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int catgid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select max(CATEGORY_ID) from ITEM_CATEGORY_MASTER"));
            int srno = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, " select  ISnull (max(Sr_No),0)+1 from CategorySeparate"));
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "delete CategorySeparate where id=0 and categoryid=" + catgid + "");
            string str2 = "insert into CategorySeparate values(" + srno + ",0," + catgid + "," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ")";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str2);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmLocalConsumptionForOrder.aspx");
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_NAME", true, "--SELECT--");
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
        UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--SELECT--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
    }
    protected void fillitemcode_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by SubQuantity", true, "--Select--");
    }
    protected void BtnRefreshItem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name ", true, "---SELECT----");
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--SELECT--");
    }
    private void filldata()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet dt = SqlHelper.ExecuteDataset(con, CommandType.Text, "select finishedid,Remark from OrderLocalConsumption where orderid=" + Request.QueryString["orderid"] + " order by id desc");
        if (dt.Tables[0].Rows.Count > 0 && dt.Tables[0].Rows[0][0].ToString() != "")
        {
            DataSet dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId  from V_FinishedItemDetail where item_finished_id=" + dt.Tables[0].Rows[0][0].ToString() + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (dt1.Tables[0].Rows.Count > 0)
            {
                if (DDItemCategory.Visible == true)
                {
                    DDItemCategory.SelectedValue = dt1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    dditemcategorychange();
                }
                if (DDItemName.Visible == true)
                {
                    DDItemName.SelectedValue = dt1.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    DDItemNamechange();
                }
                if (DDQuality.Visible == true)
                {
                    DDQuality.SelectedValue = dt1.Tables[0].Rows[0]["QualityId"].ToString();
                }
                if (DDDesign.Visible == true)
                {
                    DDDesign.SelectedValue = dt1.Tables[0].Rows[0]["designId"].ToString();
                }
                if (DDColor.Visible == true)
                {
                    DDColor.SelectedValue = dt1.Tables[0].Rows[0]["ColorId"].ToString();
                }
                if (DDShape.Visible == true)
                {
                    DDShape.SelectedValue = dt1.Tables[0].Rows[0]["ShapeId"].ToString();
                    DDshapechange();
                }
                if (DDSize.Visible == true)
                {
                    DDSize.SelectedValue = dt1.Tables[0].Rows[0]["SizeId"].ToString();
                }
                if (ddshadecolor.Visible == true)
                {
                    ddshadecolor.SelectedValue = dt1.Tables[0].Rows[0]["ShadecolorId"].ToString();
                }
            }
            txtremark.Text = dt.Tables[0].Rows[0]["Remark"].ToString();
        }
    }
    private void fillsidegrid()
    {
        string str = @"select CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,ITEM_FINISHED_ID as finishedid  from V_FinishedItemDetail Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        if (DDItemCategory.SelectedIndex > 0)
        {
            if (DDItemName.SelectedIndex > 0)
                str = str + " and ITEM_ID=" + DDItemName.SelectedValue + "  and CATEGORY_ID=" + DDItemCategory.SelectedValue + "";
            else
                str = str + " and CATEGORY_ID=" + DDItemCategory.SelectedValue + "";
        }
        else
        {
            if (DDItemName.SelectedIndex > 0)
            {
                str = str + " Where ITEM_ID=" + DDItemName.SelectedValue + "";
            }
        }
        str = str + "and status=1";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGShowConsumption.DataSource = ds;
        DGShowConsumption.DataBind();
    }
    protected void DGShowConsumption_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dt1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId  from V_FinishedItemDetail where item_finished_id=" + DGShowConsumption.SelectedDataKey.Value + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");

        if (DDItemCategory.Visible == true && DDItemCategory.Items.Count > 0)
        {
            DDItemCategory.SelectedValue = dt1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            dditemcategorychange();
        }
        if (DDItemName.Visible == true && DDItemName.Items.Count > 0)
        {
            DDItemName.SelectedValue = dt1.Tables[0].Rows[0]["ITEM_ID"].ToString();
            DDItemNamechange();
        }
        if (DDQuality.Visible == true && DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedValue = dt1.Tables[0].Rows[0]["QualityId"].ToString();
        }
        if (DDDesign.Visible == true && DDDesign.Items.Count > 0)
        {
            DDDesign.SelectedValue = dt1.Tables[0].Rows[0]["designId"].ToString();
        }
        if (DDColor.Visible == true && DDColor.Items.Count > 0)
        {
            DDColor.SelectedValue = dt1.Tables[0].Rows[0]["ColorId"].ToString();
        }
        if (DDShape.Visible == true && DDShape.Items.Count > 0)
        {
            DDShape.SelectedValue = dt1.Tables[0].Rows[0]["ShapeId"].ToString();
            DDshapechange();
        }
        if (DDSize.Visible == true && DDSize.Items.Count > 0)
        {
            DDSize.SelectedValue = dt1.Tables[0].Rows[0]["SizeId"].ToString();
        }
        if (ddshadecolor.Visible == true && ddshadecolor.Items.Count > 0)
        {
            ddshadecolor.SelectedValue = dt1.Tables[0].Rows[0]["ShadecolorId"].ToString();
        }
    }
    protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGShowConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowConsumption, "Select$" + e.Row.RowIndex);
        }
    }
    protected void txtperqty_TextChanged(object sender, EventArgs e)
    {
        if (txtperqty.Text != "")
        {
            getconsmp();
        }
    }

    public void getconsmp()
    {
        Double Qty = Convert.ToDouble(Request.QueryString["Qty"]);
        TxtQty.Text = (Qty * Convert.ToDouble(txtperqty.Text)).ToString();

    }

}