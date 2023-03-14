using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Packing_FrmReadyToPack : System.Web.UI.Page
{
    static int MasterCompanyId;
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (IsPostBack == false)
        {
            ViewState["RTPID"] = 0;
           if (Session["varCompanyId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            logo();
            lablechange();
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname", true, "--SELECT--");
            
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CustomerCode", true, "--SELECT--");
            ParameteLabel();
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"))  ;
            if (VarProdCode == 1)
            {
                TDProdCode.Visible = true;
                TDFinishType.Visible = true;
            }
            else
            {
                TDProdCode.Visible = false;
                TDFinishType.Visible = false;
            }
            DDCustomerCode.Focus();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        lblShade.Text = ParameterList[7];
    }
    private void logo()
    {
        imgLogo.ImageUrl.DefaultIfEmpty();
        imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblShade.Text = ParameterList[7];
    }

    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();
        DDOrderNo.Focus();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "Select OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue+" order by LocalOrder+' / '+CustomerOrderNo" , true, "--SELECT--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtProdCode.Text = "";
        UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct CATEGORY_ID,CATEGORY_NAME from V_FinishedItemDetail VF,CategorySeparate CS,OrderDetail OD Where VF.Category_Id=CS.CategoryId And CS.Id=0 And OD.Item_Finished_Id=VF.Item_Finished_Id And OD.Orderid=" + DDOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID", true, "--SELECT--");
        CustomerOrderNoSelectedIndexChange();
        Fill_Grid_Show();
        Fill_Grid();
    }
    private void Fill_Grid_Show()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT Distinct PRODUCTCODE FROM ORDERDETAIL OD,ITEM_PARAMETER_MASTER IPM,STOCK S WHERE OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID AND S.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID AND OD.ORDERID=" + DDOrderNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY PRODUCTCODE");
        DGSHOWDATA.DataSource = Ds;
        DGSHOWDATA.DataBind();
    }
    private void CustomerOrderNoSelectedIndexChange()
    {
        CommanFunction.FillCombo(DDOrderUnit, "Select Distinct UnitId,UnitName from Unit U,OrderMaster OM,orderdetail od Where om.orderid=od.orderid and Od.OrderUnitId=U.UnitId and OM.Orderid=" + DDOrderNo.SelectedValue);
        DDOrderUnit.Focus();
    }
    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        CATEGORY_DEPENDS_CONTROLS();
        ddItemName.Focus();
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        ItemNameSelectedIndexChange();
        DDFinishType.Focus();
    }
    private void ItemNameSelectedIndexChange()
    {
        string Str = "Select Distinct VF.QualityID,VF.QualityName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        UtilityModule.ConditionalComboFill(ref ddQuality, Str, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        ComboFill();
        Fill_Finish_Type();
    }
    protected void ComboFill()
    {
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        if (TDDesign.Visible == true)
        {
            string Str = "Select Distinct VF.DesignID,VF.DesignName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
            UtilityModule.ConditionalComboFill(ref ddDesign, Str, true, "--Select--");
        }
        if (TDColor.Visible == true)
        {
            string Str = "Select Distinct VF.ColorId,VF.ColorName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
            UtilityModule.ConditionalComboFill(ref ddColor, Str, true, "--Select--");
        }
        if (TDShape.Visible == true)
        {
            string Str = "Select Distinct VF.ShapeId,VF.ShapeName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
            UtilityModule.ConditionalComboFill(ref ddShape, Str, true, "--Select--");
        }
        if (TDShade.Visible == true)
        {
            string Str = "Select Distinct VF.ShadeColorId,VF.ShadeColorName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
            UtilityModule.ConditionalComboFill(ref ddShade, Str, true, "--Select--");
        }
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        FILL_LOTNO();
        Fill_StockQty();
        
        TxtPackQty.Focus();
    }
    private void FILL_LOTNO()
    {
        int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
        UtilityModule.ConditionalComboFill(ref ddlotno, "Select DISTINCT stockid,lotno from stock where item_finished_id=" + finishedid + " and companyid=" + DDCompanyName.SelectedValue + " AND FINISHED_TYPE_ID=" + DDFinishType.SelectedValue + " ", true, " Select ");
        if (ddlotno.Items.Count > 0)
        {
            ddlotno.SelectedIndex = 1;
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        ProdCode_TextChanges();
    }
    private void ProdCode_TextChanges()
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        if (TxtProdCode.Text != "")
        {
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID", true, "--SELECT--");
            Str = "SELECT IPM.*,IM.CATEGORY_ID From ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM WHERE IPM.ITEM_ID=IM.ITEM_ID And ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"] + "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ItemNameSelectedIndexChange();
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ComboFill();
                if (TDDesign.Visible == true)
                {
                    ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                }
                if (TDColor.Visible == true)
                {
                    ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                }
                if (TDShape.Visible == true)
                {
                    ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                }
                Fill_Size();
                if (TDSize.Visible == true)
                {
                    ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                if (TDShade.Visible == true)
                {
                    ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeCOLOR_ID"].ToString();
                }
                Fill_Finish_Type();
                DDGodown.Focus();
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                LblErrorMessage.Visible = true;
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }
    private void Fill_Size()
    {
       // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        switch (Convert.ToInt16(Session["varCompanyId"]))
        {
            case 3:
                if (ddShape.SelectedValue == "3")
                {
                    if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 1)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr+'x'+str(HeightMtr) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt+'x'+str(HeightFt) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch+'x'+str(Heightinch) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                }
                else
                {
                    if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 1)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                }
                break;
            default:
                if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 1)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                break;
        }
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select DISTINCT IM.ITEM_ID,IM.ITEM_NAME from  V_FinishedItemDetail VF,ITEM_MASTER IM,OrderDetail OD Where VF.Item_ID=IM.Item_Id And OD.Item_Finished_Id=VF.Item_Finished_Id And OD.Orderid=" + DDOrderNo.SelectedValue + " And VF.CATEGORY_ID=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
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
                }
            }
        }
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
       string Qry = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @"
        SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + @"
        SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + @"
        SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + @"
        SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds,0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds,2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds,4, true, "--SELECT--");      
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    private void Fill_Finish_Type()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TDQuality.Visible == true && ddQuality.SelectedIndex > 0) || TDQuality.Visible != true)
        {
            quality = 1;
        }
        if (TDDesign.Visible == true && ddDesign.SelectedIndex > 0 || TDDesign.Visible != true)
        {
            design = 1;
        }
        if (TDColor.Visible == true && ddColor.SelectedIndex > 0 || TDColor.Visible != true)
        {
            color = 1;
        }
        if (TDShape.Visible == true && ddShape.SelectedIndex > 0 || TDShape.Visible != true)
        {
            shape = 1;
        }
        if (TDSize.Visible == true && ddSize.SelectedIndex > 0 || TDSize.Visible != true)
        {
            size = 1;
        }
        if (TDShade.Visible == true && ddShade.SelectedIndex > 0 || TDShade.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, "", Convert.ToInt32(Session["varCompanyId"])));
                if (finishedid > 0)
                {
                    TxtTotalQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(SUM(QtyRequired),0) from OrderDetail Where ORDERID=" + DDOrderNo.SelectedValue + " And ITEM_FINISHED_ID=" + finishedid)).ToString();
                    TxtPrePackQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select ISNULL(SUM(Qty),0) from ReadyToPack Where CompanyId=" + DDCompanyName.SelectedValue + " And Orderid=" + DDOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                    UtilityModule.ConditionalComboFill(ref DDFinishType, "SELECT DISTINCT FT.ID,FT.FINISHED_TYPE_NAME FROM STOCK S,FINISHED_TYPE FT Where S.FINISHED_TYPE_ID=FT.ID AND ITEM_FINISHED_ID=" + finishedid + " And FT.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                    string Str = "SELECT DISTINCT GM.GODOWNID,GM.GODOWNNAME FROM STOCK S,GODOWNMASTER GM WHERE GM.GODOWNID=S.GODOWNID AND ITEM_FINISHED_ID=" + finishedid + " And GM.MasterCompanyId=" + Session["varCompanyId"];
                    if (DDFinishType.Items.Count > 0)
                    {
                        DDFinishType.SelectedIndex = 1;
                        Str = Str + " AND S.FINISHED_TYPE_ID=" + DDFinishType.SelectedValue;
                    }
                    UtilityModule.ConditionalComboFill(ref DDGodown, Str , true, "--SELECT--");
                    if (DDGodown.SelectedIndex > 0)
                    {
                       TxtStockQty.Text = (SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT ISNULL(SUM(QTYINHAND),0) FROM STOCK Where COMPANYID=" + DDCompanyName.SelectedValue + " And ITEM_FINISHED_ID=" + finishedid + " And FINISHED_TYPE_ID=" + DDFinishType.SelectedValue + " And GODOWNID=" + DDGodown.SelectedValue)).ToString();
                    }                    
                    Tran.Commit();
                }
           }
            catch(Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmReadyToPack.aspx");
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "You Are SuccessFully LogOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    protected void DDFinishType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, "", Convert.ToInt32(Session["varCompanyId"])));
            if (finishedid > 0)
            {
                string Str = "SELECT DISTINCT GM.GODOWNID,GM.GODOWNNAME FROM STOCK S,GODOWNMASTER GM WHERE GM.GODOWNID=S.GODOWNID AND ITEM_FINISHED_ID=" + finishedid + " And GM.MasterCompanyId=" + Session["varCompanyId"];
                if (DDFinishType.Items.Count > 0)
                {
                    Str = Str + " AND S.FINISHED_TYPE_ID=" + DDFinishType.SelectedValue;
                }
                UtilityModule.ConditionalComboFill(ref DDGodown, Str, true, "--SELECT--");
                Tran.Commit();
                TxtPackQty.Focus();
            }
        }
        catch(Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmReadyToPack.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Fill_StockQty()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, "", Convert.ToInt32(Session["varCompanyId"])));
            if (finishedid > 0)
            {
                TxtStockQty.Text = (SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT ISNULL(SUM(QTYINHAND),0) FROM STOCK Where COMPANYID=" + DDCompanyName.SelectedValue + " And ITEM_FINISHED_ID=" + finishedid + " And FINISHED_TYPE_ID=" + DDFinishType.SelectedValue + " and LotNo='"+ddlotno.SelectedItem.Text+"' And GODOWNID=" + DDGodown.SelectedValue)).ToString();
                Tran.Commit();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmReadyToPack.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=0 And ProductCode Like '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Str = "";
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (LblErrorMessage.Text == "")
            {
                    SqlParameter[] _arrpara1 = new SqlParameter[12];
                    _arrpara1[0] = new SqlParameter("@ID", SqlDbType.Int);
                    _arrpara1[0].Direction = ParameterDirection.InputOutput;
                    _arrpara1[1] = new SqlParameter("@COMPANYID", SqlDbType.Int);
                    _arrpara1[2] = new SqlParameter("@ORDERID", SqlDbType.Int);
                    _arrpara1[3] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                    _arrpara1[4] = new SqlParameter("@FINISH_TYPE_ID", SqlDbType.Int);
                    _arrpara1[5] = new SqlParameter("@QTY", SqlDbType.Int);
                    _arrpara1[6] = new SqlParameter("@USERID", SqlDbType.Int);
                    _arrpara1[7] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
                    _arrpara1[8] = new SqlParameter("@DATE", SqlDbType.SmallDateTime);
                    _arrpara1[9] = new SqlParameter("@GODOWNID", SqlDbType.Int);
                    _arrpara1[10] = new SqlParameter("@Message", SqlDbType.NVarChar,150);
                    _arrpara1[11] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 250);
                    _arrpara1[10].Direction = ParameterDirection.Output;
                    if (BtnSave.Text == "Save")
                    {
                        _arrpara1[0].Value = 0;
                    }
                    else
                    {
                        _arrpara1[0].Value = ViewState["RTPID"];
                    }
                  
                    //_arrpara1[0].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT ISNULL(MAX(ID),0)+1 FROM READYTOPACK");

                    _arrpara1[1].Value = DDCompanyName.SelectedValue;
                    _arrpara1[2].Value = DDOrderNo.SelectedValue;
                    _arrpara1[3].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, "", Convert.ToInt32(Session["varCompanyId"])));
                    _arrpara1[4].Value = TDFinishType.Visible == true ? DDFinishType.SelectedValue : "0";
                    _arrpara1[5].Value = TxtPackQty.Text;
                    _arrpara1[6].Value = Session["varuserid"];
                    _arrpara1[7].Value = Session["varCompanyId"];
                    _arrpara1[8].Value = TxtDate.Text;
                    _arrpara1[9].Value = DDGodown.SelectedValue;
                    _arrpara1[11].Value = ddlotno.SelectedItem.Text;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ReadyToPack", _arrpara1);
                    txtpackno.Text = Convert.ToString(_arrpara1[0].Value);
                    UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(_arrpara1[3].Value), Convert.ToInt32(DDGodown.SelectedValue), Convert.ToInt32(DDCompanyName.SelectedValue), ddlotno.SelectedItem.Text, Convert.ToDouble(TxtPackQty.Text), Convert.ToDateTime(TxtDate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Ready To Pack", Convert.ToInt32(_arrpara1[0].Value), Tran, 0, false, 0, Convert.ToInt32(DDFinishType.SelectedValue));
                    Msg = _arrpara1[10].Value.ToString();
                    MessageSave(Msg);

                    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'ReadyToPack'," + _arrpara1[0].Value + ",getdate(),'Insert')");
                    LblErrorMessage.Text = "";
                    BtnSave.Text = "Save";
                    Tran.Commit();
                    Fill_Grid();
                    Save_Referce();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmReadyToPack.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            Msg = ex.Message;
            MessageSave(Msg);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Save_Referce()
    {
        TxtProdCode.Text = "";
        ProdCode_TextChanges();
        DDFinishType.SelectedIndex = 0;
        DDGodown.SelectedIndex = 0;
        TxtPackQty.Text = "";
        TxtStockQty.Text = "";
        TxtTotalQty.Text ="";
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
    }
    //********************************Function To Get Data to fill Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"Select ID Sr_No,Category_Name Category,Item_Name ItemName,QualityName+Space(2)+DesignName+Space(2)+ColorName+Space(2)+ShapeName+Space(2)+ 
                             Case When " + DDOrderUnit.SelectedValue + @"=1 Then SizeMtr Else SizeFt End+Space(2)+ShadeColorName Description,Qty,GodownId
                             from ReadyToPack PP,V_FinishedItemDetail VF Where PP.Finishedid=VF.Item_Finished_id And PP.OrderId=" + DDOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmReadyToPack.aspx");
            LblErrorMessage.Visible = true;
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
    protected void TxtPackQty_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        double VarTotalQty = 0.0;
        double VarStockQty = 0.0;
        double VarPackQty = 0.0;
        double VarPrePackQty = 0.0;
        if (TxtTotalQty.Text != "")
        {
            VarTotalQty = Convert.ToDouble(TxtTotalQty.Text);
        }
        if (TxtStockQty.Text != "")
        {
            VarStockQty = Convert.ToDouble(TxtStockQty.Text);
        }
        if (TxtPackQty.Text != "")
        {
            VarPackQty = Convert.ToDouble(TxtPackQty.Text);
        }
        if (TxtPrePackQty.Text != "")
        {
            VarPrePackQty = Convert.ToDouble(TxtPrePackQty.Text);
        }
        if (BtnSave.Text == "Update")
        {
            if (VarPackQty  > VarTotalQty)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Value";
                TxtPackQty.Text = "";
                TxtPackQty.Focus();
               Msg= "Pls Enter Correct Value";
               MessageSave(Msg);
            }
            else
            {
                BtnSave.Focus();
            }         
           
        }
        else
        {
            if (VarPackQty > (VarTotalQty - VarPrePackQty) || VarPackQty > VarStockQty)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Value";
                Msg = "Pls Enter Correct Value";
                TxtPackQty.Text = "";
                TxtPackQty.Focus();
                MessageSave(Msg);
            }
            else
            {
                BtnSave.Focus();
            }
        }
    }
    protected void BTNCLOSE_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            // TxtProdCode.Text = DGOrderDetail.Rows[DGOrderDetail.SelectedIndex].Cells[2].Text.ToString();
            ViewState["RTPID"] = DGOrderDetail.SelectedDataKey.Value;
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID", true, "--SELECT--");
            string str = @"  SELECT IPM.*,IM.CATEGORY_ID From ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM WHERE IPM.ITEM_ID=IM.ITEM_ID and IPM.ITEM_FINISHED_ID =  (select distinct FinishedId from readytopack where id= " + DGOrderDetail.SelectedDataKey.Value + ") And IM.MasterCompanyId=" + Session["varCompanyId"] + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ItemNameSelectedIndexChange();
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ComboFill();
                if (TDDesign.Visible == true)
                {
                    ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                }
                if (TDColor.Visible == true)
                {
                    ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                }
                if (TDShape.Visible == true)
                {
                    ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                }
                Fill_Size();
                if (TDSize.Visible == true)
                {
                    ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                if (TDShade.Visible == true)
                {
                    ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeCOLOR_ID"].ToString();
                }
                Fill_Finish_Type();
                if (DDGodown.Items.Count > 0)
                {
                    DDGodown.SelectedValue = ((Label)DGOrderDetail.Rows[DGOrderDetail.SelectedIndex].FindControl("LblGodownID")).Text;

                }
                DDGodown.Focus();
                FILL_LOTNO();
                Fill_StockQty();
                TxtPackQty.Text = DGOrderDetail.Rows[DGOrderDetail.SelectedIndex].Cells[3].Text.ToString();
                TxtStockQty.Text = Convert.ToString(Convert.ToInt32(TxtStockQty.Text) + Convert.ToInt32(TxtPackQty.Text));
                TxtPrePackQty.Text = Convert.ToString(Convert.ToInt32(TxtPrePackQty.Text) - Convert.ToInt32(TxtPackQty.Text));
                string lot = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select lotno from ReadyToPack where ID=" + ViewState["RTPID"] + ""));
                if (ddlotno.Items.Count > 0)
                {
                    ddlotno.SelectedItem.Text = lot;
                }
                txtpackno.Text = ViewState["RTPID"].ToString();
                //TxtStockQty.Text = (Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text)).ToString();
                BtnSave.Text = "Update";
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                LblErrorMessage.Visible = true;
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int i = Convert.ToInt32(DGOrderDetail.DataKeys[e.RowIndex].Value);
            int Qty = Convert.ToInt32(DGOrderDetail.Rows[e.RowIndex].Cells[3].Text);
            SqlParameter[] _Param = new SqlParameter[5];
            _Param[0] = new SqlParameter("@RTPId", i);
            _Param[1] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"]);
            _Param[2] = new SqlParameter("@Qty", Qty);
            _Param[3] = new SqlParameter("@OrderID", DDOrderNo.SelectedValue);
            _Param[4] = new SqlParameter("@Message", SqlDbType.NVarChar,100);
            _Param[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ReadyToPack_Delete", _Param);
            Msg = _Param[4].Value.ToString();
            MessageSave(Msg);
            Fill_Grid();
            }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Packing/ReadyToPack");
            Msg = ex.Message;
            MessageSave(Msg);
        }
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_StockQty();
        TxtPackQty.Focus();
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == false)
        {
            Response.Redirect("FrmReadyToPack.aspx");
        }

    }
    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n=DGSHOWDATA.SelectedIndex;
        TxtProdCode.Text=DGSHOWDATA.Rows[n].Cells[0].Text;
        ProdCode_TextChanges();
    }
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "Select$" + e.Row.RowIndex);
        }
    }
   
}