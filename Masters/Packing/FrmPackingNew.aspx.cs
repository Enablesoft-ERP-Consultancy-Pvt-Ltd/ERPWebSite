using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmPackingNew : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
            @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    static string ViewFinishedItem = "";
    static string SizeTable = "";
    static string ChkBoxSelectedSizeId = "0";
    static int ItemFinishedId = 0;
    static string QualityName = "", DesignName = "", ColorName = "";
    static int QualityId = 0, DesignId = 0, ColorId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Packing_FrmPackingNew),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            logo();
            tdsubquality.Visible = false;
            string Qry = @" select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CustomerCode
                    Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                    Select UnitId,UnitName from Unit where UnitId in(1,2)";
            DataSet ds1 = null;
            ds1 = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds1, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds1, 1, true, "--SELECT--");
            CustomerCodeSelectedIndexChange();
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds1, 2, true, "--SELECT--");
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds1, 3, true, "--SELECT--");
            if (DDUnit.Items.Count > 0)
            {
                DDUnit.SelectedIndex = 1;
            }
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ParameteLabel();
            RDAreaWise.Checked = true;
            //int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            //if (VarProdCode == 1)
            //{
            //    TDProdCode.Visible = true;
            //}
            //else
            //{
            //    TDProdCode.Visible = false;
            //}
            CheckedMultipleRolls();
            // ViewState["PackingID"] = 0;
            //Session["PackingID"] = 0;
            ViewState["PACKINGDETAILID"] = 0;
            //DDCustomerCode.Focus();
            hnid.Value = "0";
            hnpackingid.Value = "0";
            hnfinished.Value = "";

            //if (Session["varMasterCompanyIDForERP"].ToString() == "19")
            //{
            //    Label36.Text = "Bale Nt Wt";
            //    Label37.Text = "Bale Gr Wt";
            //}
            //else
            //{
            //    Label36.Text = "One Pcs Nt Wt";
            //    Label37.Text = "One Pcs Gr Wt";
            //}

            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 19:
                    Label36.Text = "Bale Nt Wt";
                    Label37.Text = "Bale Gr Wt";
                    TDRatePerPcs.Visible = false;
                    ViewFinishedItem = "V_FinishedItemDetail";
                    SizeTable = "size";
                    TableBaleDimension.Visible = true;
                    break;
                case 30:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    ViewFinishedItem = "V_FinishedItemDetail";
                    SizeTable = "size";
                    TableBaleDimension.Visible = true;
                    break;
                case 20:
                    ViewFinishedItem = "V_FinishedItemDetailNew";
                    SizeTable = "QualitySizeNew";
                    TableBaleDimension.Visible = false;
                    break;
                default:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    ViewFinishedItem = "V_FinishedItemDetail";
                    SizeTable = "size";
                    TableBaleDimension.Visible = true;
                    break;

            }
        }
    }
    private void logo()
    {
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
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
        // TxtInvoiceNo.Focus();
    }
    protected void DDWareHouseCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillorderNo();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDConsignee, "Select CustomerId,CompanyName From  CustomerInfo where CustomerID=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Companyname", true, "--SELECT--");
        if (DDConsignee.Items.Count > 0)
        {
            DDConsignee.SelectedIndex = 1;
        }

        UtilityModule.ConditionalComboFill(ref DDWareHouseCode, "SELECT Warehouseid,WarehouseCode from WarehouseMaster Where CustomerId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by WarehouseCode", true, "--SELECT--");

        if (ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddInvoiceNo, "Select PackingID,TPackingNo+' / '+Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingNo from Packing Where ConsignorId=" + DDCompanyName.SelectedValue + " And ConsigneeId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        }
    }
    protected void FillorderNo()
    {
        if (variable.Carpetcompany == "1")
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                string str = "";
                str = @"Select distinct OM.OrderId, OM.CustomerOrderNo CustomerOrderNo from OrderMaster OM JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                        JOIN V_OrderQtyAndPackedQty VO ON OD.OrderId=VO.OrderId and OD.Item_Finished_Id=VO.Item_Finished_ID 
                        Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + @" and (VO.OrderQty-VO.PackedQty)>0  order by CustomerOrderNo";

                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select Distinct OM.OrderId,OM.CustomerOrderNo CustomerOrderNo from OrderMaster OM JOIN OrderDetail OD ON OM.OrderId=OD.OrderID Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " And WareHouseId=" + DDWareHouseCode.SelectedValue + " order by OM.CustomerOrderNo", true, "--SELECT--");
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " order by OrderId", true, "--SELECT--");
        }
    }
    protected void chksamplepack_CheckedChanged(object sender, EventArgs e)
    {
        if (chksamplepack.Checked == true)
        {
            TDSampleCustomerOrderNo.Visible = true;
            TDCustomerOrderNo.Visible = false;
            DDCustomerOrderNo.SelectedIndex = 0;

            hnsampletype.Value = "2";
            Fill_Category();
            GVPackingOrderDetail.DataSource = null;
            GVPackingOrderDetail.DataBind();
        }
        else
        {
            // TxtInvoiceNo.Text = "";
            TDSampleCustomerOrderNo.Visible = false;
            TDCustomerOrderNo.Visible = true;
            hnsampletype.Value = "1";
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
            }
        }
    }
    protected void ChkForMulipleRolls_CheckedChanged(object sender, EventArgs e)
    {
        ChkForMulipleRolls_CheckedChanged();
    }
    private void ChkForMulipleRolls_CheckedChanged()
    {
        TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "0";
        TxtTotalPcs.Text = "0";
        TxtPcsPerRoll.Text = "0";
        TxtBales.Text = "";
        CheckedMultipleRolls();
        FillorderNo();
    }
    private void CheckedMultipleRolls()
    {
        if (ChkForMulipleRolls.Checked == true)
        {
            TxtRollNoTo.Enabled = true;
            TxtPcsPerRoll.Enabled = true;
            TxtBales.Enabled = true;
        }
        else
        {
            TxtRollNoTo.Enabled = false;
            TxtBales.Enabled = false;
        }
    }
    protected void ChkForWithoutOrder_CheckedChanged(object sender, EventArgs e)
    {
        ChkForWithoutOrder_CheckedChanged();
    }
    private void ChkForWithoutOrder_CheckedChanged()
    {
        if (ChkForWithoutOrder.Checked == true)
        {
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
                DDCustomerOrderNo.Enabled = false;
            }
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct Category_ID,Category_Name from CarpetNumber CR ," + ViewFinishedItem + " VF  Where CR.Item_Finished_ID=VF.Item_Finished_Id And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-Select-");
        }
        else
        {
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
            }
            DDCustomerOrderNo.Enabled = true;
            ddCategoryName.Items.Clear();
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerOrderNo_SelectedIndexChanged();
        BindPackingOrderDetail();
    }
    private void Fill_Category()
    {
        string Str = @"Select Distinct VF.Category_ID, VF.Category_Name 
        from " + ViewFinishedItem + @" VF 
        JOIN CategorySeparate CS(Nolock) ON CS.CategoryID = VF.Category_ID And CS.ID = 0 
        Left Join OrderDetail OD ON OD.Item_Finished_Id = VF.Item_Finished_Id 
            Where VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        if (DDCustomerOrderNo.SelectedIndex > 0)
        {

            Str = Str + " And OD.OrderID = " + DDCustomerOrderNo.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref ddCategoryName, Str, true, "--SELECT--");
    }
    private void DDCustomerOrderNo_SelectedIndexChanged()
    {
        Fill_Category();
    }
    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Category_SelectedIndex_Change();
    }
    private void Category_SelectedIndex_Change()
    {
        ddlcategorycange();
        string Str = "Select Distinct VF.Item_ID,VF.Item_Name from " + ViewFinishedItem + @" VF left Outer join OrderDetail OD on OD.Item_Finished_Id=VF.Item_Finished_Id 
                Where  Category_Id=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        if (ChkForWithoutOrder.Checked != true)
        {
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
            }
        }
        Str = Str + " order by Item_Name";
        UtilityModule.ConditionalComboFill(ref ddItemName, Str, true, "--SELECT--");
    }
    private void ddlcategorycange()
    {
        ddQuality.Items.Clear();
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCategoryName.SelectedValue;
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
                    case "6":
                        TDShade.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedIndexChange();
        // ddQuality.Focus();
        BindPackingOrderDetail();
    }
    private void ItemNameSelectedIndexChange()
    {
        string Str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            Str = @"select Distinct CQ.SrNo,CQ.QualityNameAToC from CustomerQuality CQ inner join " + ViewFinishedItem + @" vf on CQ.QualityId=vf.QualityId
                     and CQ.CustomerId=" + DDCustomerCode.SelectedValue + " left join OrderDetail Od on Od.Item_Finished_Id=vf.ITEM_FINISHED_ID Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + " and Od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }

            }

            Str = Str + " order by QualityNameAToC";
        }
        else
        {
            Str = @"select Distinct vf.QualityId,vf.QualityName from " + ViewFinishedItem + @" vf left join OrderDetail OD 
                    on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                    Where VF.QualityId>0 and  VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
            if (ChkForWithoutOrder.Checked != true)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + @" And OD.Orderid=" + DDCustomerOrderNo.SelectedValue;
                }

            }
            Str = Str + " order by QualityName";
        }
        UtilityModule.ConditionalComboFill(ref ddQuality, Str, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboFill();
        Fill_Price();
        // ddDesign.Focus();
        BindPackingOrderDetail();
    }
    protected void ComboFill()
    {
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        string str = "";
        if (TDDesign.Visible == true)
        {

            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                str = @"select Distinct cd.SrNo,cd.DesignNameAToC from CustomerDesign cd inner join " + ViewFinishedItem + @" vf on cd.DesignId=vf.designId
                     and cd.CustomerId=" + DDCustomerCode.SelectedValue + " left join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        str = str + "  and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                    }

                }
                if (ddQuality.SelectedIndex > 0)
                {
                    str = str + " and cd.CQSRNO=" + ddQuality.SelectedValue;
                }
                str = str + " order by DesignNameAToC";
            }
            else
            {
                str = @"select Distinct vf.designId,vf.designName from " + ViewFinishedItem + @" vf left join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                        Where vf.Designid>0 and vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                    }

                }
                str = str + "  order by designName";
            }
            UtilityModule.ConditionalComboFill(ref ddDesign, str, true, "--Select--");
        }
        if (TDColor.Visible == true)
        {
            FillColor();
        }
        if (TDShape.Visible == true)
        {
            //string Str = "Select Distinct VF.ShapeId,VF.ShapeName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";

            string Str = "Select Distinct VF.ShapeId,VF.ShapeName from " + ViewFinishedItem + @" VF LEFT JOIN OrderDetail OD ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID 
            Where Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and VF.ShapeID > 0";
            if (ChkForWithoutOrder.Checked != true)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
                }

            }
            UtilityModule.ConditionalComboFill(ref ddShape, Str, true, "--Select--");
        }
        if (TDShade.Visible == true)
        {
            string Str = "Select Distinct VF.ShadeColorId,VF.ShadeColorName from OrderDetail OD," + ViewFinishedItem + @" VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
            if (ChkForWithoutOrder.Checked != true)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
                }

            }
            UtilityModule.ConditionalComboFill(ref ddShade, Str, true, "--Select--");
        }
    }
    protected void FillColor()
    {
        string str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            str = @"select Distinct CC.SrNo,CC.ColorNameToC from CustomerColor cc inner join " + ViewFinishedItem + @" vf on CC.ColorId=vf.ColorId
                      and cc.CustomerId=" + DDCustomerCode.SelectedValue + " left join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            if (ddDesign.SelectedIndex > 0)
            {
                str = str + " and cc.CDSRNO=" + ddDesign.SelectedValue;
            }
            str = str + " order by ColorNameToC";
        }
        else
        {
            str = @"select Distinct vf.ColorId,vf.ColorName From " + ViewFinishedItem + @" vf left join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                        and vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and vf.ColorId>0";
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }

            }
            str = str + " order by ColorName";

        }
        UtilityModule.ConditionalComboFill(ref ddColor, str, true, "--Select--");
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
        // ddSize.Focus();
        BindPackingOrderDetail();
    }
    private void ShapeSelectedChange(int sizeid = 0)
    {
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtArea.Text = "";
        //TxtPrice.Text = "";
        string Str = "", size = "SizeFt", custsize = "SizeNameAToC";


        size = "SizeFt";

        if (DDUnit.SelectedValue == "1")
        {
            size = "SizeMtr";
            custsize = "MtSizeAToC";
        }




        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                Str = @"select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName  
                from CustomerSize CS 
                inner join " + ViewFinishedItem + @" vf on CS.SizeID=vf.SizeID And vf.item_id=" + ddItemName.SelectedValue + @" 
                left join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id";

                if (ddDesign.SelectedIndex > 0)
                {
                    Str = Str + " JOIN CustomerDesign CD ON CD.DesignID = VF.DesignID And CD.Srno = " + ddDesign.SelectedValue;
                }
                Str = Str + " Where CS.CustomerId=" + DDCustomerCode.SelectedValue + @" and vf.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                    }
                }
                Str = Str + " order by SizeName";
            }
            else
            {
                Str = @"select sizeid," + size + " as SIze From " + ViewFinishedItem + @" vf where  vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + "  and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }
            }

        }
        else
        {
            if (hnsampletype.Value == "1")
            {
                Str = @"Select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName 
                  from OrderDetail OD," + ViewFinishedItem + @" VF Left outer join CustomerSize CS on CS.Sizeid=Vf.Sizeid And CustomerId=" + DDCustomerCode.SelectedValue + @" Where OD.Item_Finished_Id=VF.Item_Finished_Id
                  And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + "  And VF.ShapeId=" + ddShape.SelectedValue + "   And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";


                if (ChkForWithoutOrder.Checked != true)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        if (DDCustomerOrderNo.SelectedIndex > 0)
                        {
                            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
                        }

                    }
                }
            }
            else
            {
                Str = @"select distinct sizeid," + size + " as SIze From " + ViewFinishedItem + @" vf where vf.SizeId>0 and vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }

            }
        }




        UtilityModule.ConditionalComboFill(ref ddSize, Str, true, "--Select--");
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeSelectedIndexChange();
        //TxtPrice.Focus();
        if (ChkBoxSelectedSizeId == "0")
        {
            BindPackingOrderDetail();
        }

        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            ChangeRate();
        }


    }
    private void SizeSelectedIndexChange()
    {
        string Width = "";
        string Length = "";
        string AreaMtr = "";
        string AreaFT = "";
        if (Session["varMasterCompanyIDForERP"].ToString() == "20")
        {
            Width = "LEFT(Export_Format, CHARINDEX('x', Export_Format) - 1)";
            Length = "REPLACE(SUBSTRING(Export_Format, CHARINDEX('x', Export_Format), LEN(Export_Format)), 'x', '')";
            AreaFT = "Export_Area";
            AreaMtr = "MtrArea";

            SizeTable = "QualitySizeNew";
            if (DDUnit.SelectedValue == "1")
            {
                Width = "IsNull(LEFT(MtrSize, CHARINDEX('x', MtrSize) - 1),0)";
                Length = "IsNull(REPLACE(SUBSTRING(MtrSize, CHARINDEX('x', MtrSize), LEN(MtrSize)), 'x', ''),0)";

            }


        }
        else
        {
            Width = "WidthFt";
            Length = "LengthFt";
            AreaFT = "AreaFt";
            AreaMtr = "AreaMtr";

            SizeTable = "size";

            if (DDUnit.SelectedValue == "1")
            {
                Width = "WidthMtr";
                Length = "LengthMtr";

            }
        }

        string str = "";
        if (ChkBoxSelectedSizeId != "0")
        {
            str = "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + @" Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + AreaMtr + @" Else " + AreaFT + @" End Area from " + SizeTable + @" Where SizeId=" + ChkBoxSelectedSizeId;
        }
        else
        {
            str = "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + @" Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + AreaMtr + @" Else " + AreaFT + @" End Area from " + SizeTable + @" Where SizeId=" + ddSize.SelectedValue;
        }


        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
            TxtLength.Text = Ds.Tables[0].Rows[0]["Length"].ToString();
            TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
        }

        Fill_Price();
        GetFinishedIdRatePackQty();


        //if (ChkBoxSelectedSizeId != "0")
        //{
        //    GetFinishedIdRatePackQty();
        //}
        //else
        //{
        //    Fill_Price();
        //}
    }
    protected void TxtLength_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 1)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, 0));
            }
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                //TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, 0));
                FtAreaCalculate(TxtLength, TxtWidth, TxtArea, 1);
            }
        }
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DesignSelectedChange();

        BindPackingOrderDetail();
    }
    private void DesignSelectedChange()
    {
        FillColor();
        //        if (TDColor.Visible == true)
        //        {
        //            string Str = @"Select Distinct VF.ColorId,case When ColorNameToC is null then VF.ColorName Else ColorNameToC End As ColorName 
        //                        From OrderDetail OD,
        //                        V_FinishedItemDetail VF 
        //                        left outer join CustomerColor CC on CC.ColorId=VF.ColorId And Customerid=" + DDCustomerCode.SelectedValue + @" 
        //                        Where OD.Item_Finished_Id=VF.Item_Finished_Id And VF.Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + @" 
        //                        And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        //            if (ChkForWithoutOrder.Checked != true)
        //            {
        //                Str = Str + @" And OD.Orderid=" + DDCustomerOrderNo.SelectedValue + "";
        //            }
        //            UtilityModule.ConditionalComboFill(ref ddColor, Str, true, "--Select--");
        //        }
        Fill_Price();
        //ddColor.Focus();

    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Price();
        // ddShape.Focus();       

    }
    protected void ddShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Price();
    }
    private void Fill_Price()
    {
        TxtPackQty.Text = TxtTotalPcs.Text;

        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
        }
    }
    private void ForCheckAllRows()
    {
        if (TxtPackQty.Text != "")
        {
            for (int i = 0; i < DGStock.Rows.Count; i++)
            {
                ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
            }

            if (DGStock.Rows.Count >= Convert.ToInt32(TxtPackQty.Text))
            {
                for (int i = 0; i < Convert.ToInt32(TxtPackQty.Text); i++)
                {
                    GridViewRow row = DGStock.Rows[i];
                    ((CheckBox)row.FindControl("Chkbox")).Checked = true;
                }

            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Value";
                if (ChkForEdit.Checked == false)
                {
                    TxtPackQty.Text = "";
                    TxtPackQty.Focus();
                }
            }
        }
    }
    protected void TxtRollNoFrom_TextChanged(object sender, EventArgs e)
    {
        RollNoFromTextChanged();
    }
    private void RollNoFromTextChanged()
    {
        if (TxtRollNoFrom.Text != "")
        {
            TxtBales.Text = "1";
            if (TxtPcsPerRoll.Text == "")
            {
                TxtPcsPerRoll.Text = "1";
            }
            TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(TxtBales.Text) - 1).ToString();
            //TxtTotalPcs.Text =(Convert.ToDouble(TxtPcsPerRoll.Text) * Convert.ToDouble(TxtBales.Text)).ToString ();
        }
        TextBoxIndexChange();
        //TxtSrNo.Focus();
    }
    protected void TxtTotalPcs_TextChanged(object sender, EventArgs e)
    {
        TextBoxIndexChange();
        BtnSave.Focus();
    }
    private void TextBoxIndexChange()
    {
        LblErrorMessage.Text = "";

        if (TxtRollNoFrom.Text != "" && TxtPcsPerRoll.Text != "" && TxtPcsPerRoll.Text != "0")
        {
            if (ChkForExtraPcs.Checked == false && chksamplepack.Checked == false)
            {
                int BalanceQty = (Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text));
                if (Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) > BalanceQty)
                {
                    TxtPackQty.Text = "0";
                    TxtTotalPcs.Text = "0";
                    TxtBales.Text = "0";
                    TxtRollNoTo.Text = "0";
                    TxtTotalPcs.Focus();
                    LblErrorMessage.Text = "Pack Pcs Qty Can't be greater then balance qty " + BalanceQty + "";
                    //ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Pack Pcs Qty Can't be greater then balance qty '" + BalanceQty + ");", true);
                    return;


                }
            }
            if (Convert.ToInt32((Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) % Convert.ToInt32(TxtPcsPerRoll.Text))) > 0)
            {
                TxtPackQty.Text = "0";
                //TxtTotalPcs.Text = "0";
                TxtBales.Text = "0";
                TxtRollNoTo.Text = "0";
                int BalanceQty = Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) - Convert.ToInt32((Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) % Convert.ToInt32(TxtPcsPerRoll.Text)));
                LblErrorMessage.Text = "According to pcs per roll you can pack " + BalanceQty + " pcs";
                TxtTotalPcs.Focus();
            }
            else
            {
                TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) - 1 + (Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) / Convert.ToInt32(TxtPcsPerRoll.Text))).ToString();
                TxtBales.Text = TxtRollNoTo.Text;
            }


            // TxtTotalPcs.Text = (Convert.ToInt32(TxtPcsPerRoll.Text) * (1 + (Convert.ToInt32(TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text)))).ToString();

            //if (Session["varMasterCompanyIDForERP"].ToString() != "30")
            //{
            //    TxtBales.Text = (Convert.ToInt32(TxtRollNoFrom.Text) - 1 + (Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) / Convert.ToInt32(TxtPcsPerRoll.Text))).ToString();
            //}
            if (ChkForEdit.Checked == true)
            {
                TxtPackQty.Text = TxtTotalPcs.Text;
                // ForCheckAllRows();
                //TxtPrePackQty.Text =Convert.ToString(Convert.ToDouble(TxtPrePackQty.Text) - Convert.ToDouble(TxtPackQty.Text));
            }

            Fill_Price();
        }

        //if (TxtRollNoFrom.Text != "" && TxtRollNoTo.Text != "" && TxtPcsPerRoll.Text != "" && TxtPcsPerRoll.Text != "0")
        //{
        //    TxtTotalPcs.Text = (Convert.ToInt32(TxtPcsPerRoll.Text) * (1 + (Convert.ToInt32(TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text)))).ToString();
        //    if (Session["varMasterCompanyIDForERP"].ToString() != "30")
        //    {
        //        TxtBales.Text = (Convert.ToInt32(TxtTotalPcs.Text) / Convert.ToInt32(TxtPcsPerRoll.Text)).ToString();
        //    }
        //    if (ChkForEdit.Checked == true)
        //    {
        //        TxtPackQty.Text = TxtTotalPcs.Text;
        //        ForCheckAllRows();
        //        //TxtPrePackQty.Text =Convert.ToString(Convert.ToDouble(TxtPrePackQty.Text) - Convert.ToDouble(TxtPackQty.Text));
        //    }

        //    Fill_Price();
        //}
    }

    protected void TxtBales_TextChanged(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            //(Convert.ToInt32(TxtPcsPerRoll.Text) * (1 + (Convert.ToInt32(TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text)))).ToString();
            TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(TxtBales.Text) - 1).ToString();
            TextBoxIndexChange();
        }
    }
    protected void TxtRollNoTo_TextChanged(object sender, EventArgs e)
    {
        TextBoxIndexChange();
    }
    protected void TxtPcsPerRoll_TextChanged(object sender, EventArgs e)
    {
        //TxtTotalPcs.Text = (Convert.ToDouble(TxtPcsPerRoll.Text) * Convert.ToDouble(TxtBales.Text)).ToString();
        TextBoxIndexChange();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "You Are Successfully LoggedOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();

        //if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        //{
        //    ChangeRate();
        //}
    }
    private void Validation_Check()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;

        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
        {
            goto a;

        }
        //if (UtilityModule.VALIDTEXTBOX(TxtStockNo) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;

        }
        if (ddQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (ddDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        if (ddShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
            {
                goto a;
            }
        }
        if (ddSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (ddShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPrice) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            if (Convert.ToInt32(TxtPackQty.Text) > 0)
            {

                ViewState["PACKINGDETAILID"] = 0;
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                con.Open();
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    int num = 0;
                    string VarInvoiceYear = "";
                    if (Convert.ToInt32(ViewState["PackingID"]) == 0)
                    {
                        num = 1;
                    }
                    if (num == 1)
                    {
                        SqlParameter[] _arrpara1 = new SqlParameter[12];

                        _arrpara1[0] = new SqlParameter("@PackingId", SqlDbType.Int);
                        _arrpara1[1] = new SqlParameter("@ConsignorId", SqlDbType.Int);
                        _arrpara1[2] = new SqlParameter("@ConsigneeId", SqlDbType.Int);
                        _arrpara1[3] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
                        _arrpara1[4] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                        _arrpara1[5] = new SqlParameter("@UnitId", SqlDbType.Int);
                        _arrpara1[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                        _arrpara1[7] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
                        _arrpara1[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                        _arrpara1[9] = new SqlParameter("@InvoiceYear", SqlDbType.Int);
                        _arrpara1[10] = new SqlParameter("@Msg", SqlDbType.NVarChar, 50);
                        _arrpara1[11] = new SqlParameter("@WareHouseId", SqlDbType.Int);


                        if (ddInvoiceNo.Visible == true)
                        {
                            _arrpara1[0].Value = ddInvoiceNo.SelectedValue;
                        }
                        else
                        {
                            _arrpara1[0].Value = 0;
                        }
                        _arrpara1[0].Direction = ParameterDirection.InputOutput;
                        _arrpara1[1].Value = DDCompanyName.SelectedValue;
                        _arrpara1[2].Value = DDCustomerCode.SelectedValue;
                        _arrpara1[3].Value = TxtDate.Text;
                        _arrpara1[4].Value = DDCurrency.SelectedValue;
                        _arrpara1[5].Value = DDUnit.SelectedValue;
                        _arrpara1[6].Value = Session["varuserid"];
                        _arrpara1[7].Value = TxtInvoiceNo.Text;
                        _arrpara1[8].Value = Session["varMasterCompanyIDForERP"];
                        string VarInvoiceMonth = DateTime.Now.ToString("MM");
                        if (Convert.ToInt32(VarInvoiceMonth) >= 04)
                        {
                            VarInvoiceYear = DateTime.Now.ToString("yyyy");
                        }
                        else
                        {
                            VarInvoiceYear = DateTime.Now.ToString("yyyy");
                            VarInvoiceYear = (Convert.ToInt32(VarInvoiceYear) - 1).ToString();
                        }
                        _arrpara1[9].Value = VarInvoiceYear;
                        _arrpara1[10].Direction = ParameterDirection.Output;
                        _arrpara1[11].Value = DDWareHouseCode.SelectedValue;


                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKING_AND_INVOICE", _arrpara1);
                        ViewState["PackingID"] = _arrpara1[0].Value;
                        if (_arrpara1[10].Value.ToString() != "")  //Check For Duplicate InvoiceNo
                        {
                            LblErrorMessage.Visible = true;
                            LblErrorMessage.Text = _arrpara1[10].Value.ToString();
                            Tran.Rollback();
                            return;
                        }
                        LblErrorMessage.Text = "";
                        BtnSave.Text = "Save";
                    }
                    SqlParameter[] _arrpara = new SqlParameter[56];
                    _arrpara[0] = new SqlParameter("@PackingId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@RollNo", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@PcsFrom", SqlDbType.Int);
                    _arrpara[3] = new SqlParameter("@PcsTo", SqlDbType.Int);
                    _arrpara[4] = new SqlParameter("@ArticleNo", SqlDbType.NVarChar, 20);
                    _arrpara[5] = new SqlParameter("@Extra1", SqlDbType.NVarChar, 20);
                    _arrpara[6] = new SqlParameter("@Extra2", SqlDbType.NVarChar, 20);
                    _arrpara[7] = new SqlParameter("@FinishedID", SqlDbType.Int);
                    _arrpara[8] = new SqlParameter("@Width", SqlDbType.NVarChar, 10);
                    _arrpara[9] = new SqlParameter("@Length", SqlDbType.NVarChar, 10);
                    _arrpara[10] = new SqlParameter("@Pcs", SqlDbType.Int);
                    _arrpara[11] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[12] = new SqlParameter("@Price", SqlDbType.Float);
                    _arrpara[13] = new SqlParameter("@SrNo", SqlDbType.Int);
                    _arrpara[14] = new SqlParameter("@OrderId", SqlDbType.Int);
                    _arrpara[15] = new SqlParameter("@StockNo", SqlDbType.Int);
                    _arrpara[16] = new SqlParameter("@TStockNo", SqlDbType.NVarChar, 8000);
                    _arrpara[17] = new SqlParameter("@Id", SqlDbType.Int);
                    _arrpara[18] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
                    _arrpara[19] = new SqlParameter("@RollFrom", SqlDbType.Int);
                    _arrpara[20] = new SqlParameter("@RollTo", SqlDbType.Int);
                    _arrpara[21] = new SqlParameter("@RPcs", SqlDbType.Int);
                    _arrpara[22] = new SqlParameter("@TotalPcs", SqlDbType.Int);
                    _arrpara[23] = new SqlParameter("@TotalRoll", SqlDbType.Int);
                    _arrpara[24] = new SqlParameter("@CompStockNo", SqlDbType.Int);
                    _arrpara[25] = new SqlParameter("@CustStockNo", SqlDbType.Int);
                    _arrpara[26] = new SqlParameter("@Pack", SqlDbType.Int);
                    _arrpara[27] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 50);
                    _arrpara[28] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
                    _arrpara[29] = new SqlParameter("@Quality", SqlDbType.NVarChar, 50);
                    _arrpara[30] = new SqlParameter("@Design", SqlDbType.NVarChar, 50);
                    _arrpara[31] = new SqlParameter("@Color", SqlDbType.NVarChar, 50);
                    _arrpara[32] = new SqlParameter("@CQSRNO", SqlDbType.Int);
                    _arrpara[33] = new SqlParameter("@CDSRNO", SqlDbType.Int);
                    _arrpara[34] = new SqlParameter("@CCSRNO", SqlDbType.Int);
                    _arrpara[35] = new SqlParameter("@CalTypeAmt", SqlDbType.Int);
                    _arrpara[36] = new SqlParameter("@MulipleRollFlag", SqlDbType.Int);
                    _arrpara[37] = new SqlParameter("@PakingDetailID", SqlDbType.Int);
                    _arrpara[38] = new SqlParameter("@QualityCodeID", SqlDbType.Int);
                    _arrpara[39] = new SqlParameter("@Sub_Quality", SqlDbType.NVarChar, 450);
                    _arrpara[40] = new SqlParameter("@NetWt", SqlDbType.Float);
                    _arrpara[41] = new SqlParameter("@GrossWt", SqlDbType.Float);
                    _arrpara[42] = new SqlParameter("@PurchaseCode", SqlDbType.NVarChar, 250);
                    _arrpara[43] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar, 250);
                    _arrpara[44] = new SqlParameter("@UCCNumber", SqlDbType.VarChar, 50);
                    _arrpara[45] = new SqlParameter("@RUGID", SqlDbType.VarChar, 50);
                    _arrpara[46] = new SqlParameter("@Withbuyercode", SqlDbType.TinyInt);
                    _arrpara[47] = new SqlParameter("@BaleL", SqlDbType.VarChar, 10);
                    _arrpara[48] = new SqlParameter("@BaleW", SqlDbType.VarChar, 10);
                    _arrpara[49] = new SqlParameter("@BaleH", SqlDbType.VarChar, 10);
                    _arrpara[50] = new SqlParameter("@CBM", SqlDbType.Float);
                    _arrpara[51] = new SqlParameter("@SinglePcsNetWt", SqlDbType.Float);
                    _arrpara[52] = new SqlParameter("@SinglePcsGrossWt", SqlDbType.Float);
                    _arrpara[53] = new SqlParameter("@RatePerPcs", SqlDbType.Float);
                    _arrpara[54] = new SqlParameter("@StyleNo", SqlDbType.VarChar, 20);
                    _arrpara[55] = new SqlParameter("@CustomerOrderNo", SqlDbType.VarChar, 50);

                    //Select PackingId,RollNo,PcsFrom,PcsTo,ArticleNo,Extra1,Extra2,FinishedId,Width,Length,Pcs,Area,Price,SrNo,OrderId,StockNo,TStockNo,Id,TPackingNo,
                    //RollFrom,RollTo,RPcs,TotalPcs,TotalRoll,CompStockNo,CustStockNo,Pack,Remarks From PackingInformation
                    _arrpara[0].Value = ViewState["PackingID"];
                    _arrpara[1].Value = TxtRollNoFrom.Text;
                    _arrpara[2].Value = TxtBales.Text;
                    _arrpara[3].Value = 0;
                    _arrpara[4].Value = "";
                    _arrpara[5].Value = "";
                    _arrpara[6].Value = "";

                    _arrpara[8].Value = TxtWidth.Text;
                    _arrpara[9].Value = TxtLength.Text;
                    _arrpara[10].Value = TxtTotalPcs.Text;
                    _arrpara[11].Value = Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtTotalPcs.Text);
                    _arrpara[12].Value = TxtPrice.Text;
                    _arrpara[13].Value = TxtSrNo.Text == "" ? "0" : TxtSrNo.Text;
                    _arrpara[14].Value = ChkForWithoutOrder.Checked == true ? "0" : DDCustomerOrderNo.SelectedValue;
                    _arrpara[15].Value = 0;
                    if (hnid.Value == "0")
                        _arrpara[17].Value = ViewState["PACKINGDETAILID"];

                    else
                        _arrpara[17].Value = hnid.Value;
                    _arrpara[18].Value = TxtInvoiceNo.Text;
                    _arrpara[19].Value = TxtRollNoFrom.Text;
                    _arrpara[20].Value = TxtRollNoTo.Text;
                    _arrpara[21].Value = TxtPcsPerRoll.Text;
                    _arrpara[22].Value = TxtTotalPcs.Text;
                    _arrpara[23].Value = TxtBales.Text;
                    _arrpara[24].Value = 0;
                    _arrpara[25].Value = 0;
                    _arrpara[26].Value = 0;
                    _arrpara[27].Value = TxtRemarks.Text;
                    _arrpara[28].Value = TxtDate.Text;

                    if (chksamplepack.Checked == true)
                    {
                        _arrpara[29].Value = ddQuality.SelectedItem.Text;
                        _arrpara[30].Value = ddDesign.SelectedItem.Text;
                        _arrpara[31].Value = ddColor.SelectedItem == null ? "" : ddColor.SelectedItem.Text;
                        _arrpara[32].Value = ddQuality.SelectedValue;
                        _arrpara[33].Value = ddDesign.SelectedValue;
                        _arrpara[34].Value = ddColor.SelectedValue;
                    }
                    else
                    {
                        _arrpara[29].Value = QualityName;
                        _arrpara[30].Value = DesignName;
                        //_arrpara[31].Value = ddColor.SelectedItem.Text;
                        _arrpara[31].Value = ColorName;
                        _arrpara[32].Value = QualityId;
                        _arrpara[33].Value = DesignId;
                        _arrpara[34].Value = ColorId;
                    }

                    //_arrpara[29].Value = ddQuality.SelectedItem.Text;
                    //_arrpara[30].Value = ddDesign.SelectedItem.Text;
                    ////_arrpara[31].Value = ddColor.SelectedItem.Text;
                    //_arrpara[31].Value = ddColor.SelectedItem == null ? "" : ddColor.SelectedItem.Text;
                    //_arrpara[32].Value = ddQuality.SelectedValue;
                    //_arrpara[33].Value = ddDesign.SelectedValue;
                    //_arrpara[34].Value = ddColor.SelectedValue;


                    _arrpara[35].Value = RDAreaWise.Checked == true ? 0 : 1;
                    _arrpara[36].Value = ChkForMulipleRolls.Checked == true ? 1 : 0;
                    _arrpara[37].Direction = ParameterDirection.Output;
                    //_arrpara[38].Value = DDSubQuality.SelectedValue;
                    //_arrpara[39].Value = DDSubQuality.SelectedItem.Text;
                    _arrpara[38].Value = 0;
                    _arrpara[39].Value = "";
                    _arrpara[40].Value = 0;
                    _arrpara[41].Value = 0;
                    _arrpara[42].Value = Txtpurchasecode.Text;
                    _arrpara[43].Value = TxtBuyer.Text;
                    _arrpara[44].Value = txtuccnumber.Text;
                    _arrpara[45].Value = txtrugid.Text;
                    _arrpara[46].Value = variable.Withbuyercode;

                    _arrpara[47].Value = txtlengthBale.Text == "" ? "0" : txtlengthBale.Text;
                    _arrpara[48].Value = txtwidthBale.Text == "" ? "0" : txtwidthBale.Text;
                    _arrpara[49].Value = txtheightbale.Text == "" ? "0" : txtheightbale.Text;
                    _arrpara[50].Value = txtcbmbale.Text == "" ? "0" : txtcbmbale.Text;
                    _arrpara[51].Value = txtSinglePcsNetWt.Text == "" ? "0" : txtSinglePcsNetWt.Text;
                    _arrpara[52].Value = txtSinglePcsGrossWt.Text == "" ? "0" : txtSinglePcsGrossWt.Text;
                    _arrpara[53].Value = TxtRatePerPcs.Text == "" ? "0" : TxtRatePerPcs.Text;
                    _arrpara[54].Value = txtStyleNo.Text;
                    _arrpara[55].Value = txtSampleCustomerOrderNo.Text;

                    if (chksamplepack.Checked == true)
                    {
                        int VarQuality = 0; int VarDesign = 0; int VarColor = 0;

                        VarQuality = Convert.ToInt32(ddQuality.SelectedValue) > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
                        VarDesign = Convert.ToInt32(ddDesign.SelectedValue) > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
                        VarColor = ddColor.Visible == true && Convert.ToInt32(ddColor.SelectedValue) > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0;

                        int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
                        int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
                        int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;

                        _arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])));

                    }
                    else
                    {
                        _arrpara[7].Value = ItemFinishedId;
                    }

                    //int VarQuality = 0; int VarDesign = 0; int VarColor = 0;
                    //#region
                    ////if (TxtStockNo.Text != "" && TxtStockNo.Text != "0")
                    ////{
                    ////    if (ddQuality.Visible == true)
                    ////    {
                    ////        VarQuality = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.QualityId from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
                    ////    }

                    ////    if (ddDesign.Visible == true)
                    ////    {
                    ////        VarDesign = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.DesignID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
                    ////    }

                    ////    if (ddColor.Visible == true)
                    ////    {
                    ////        VarColor = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.colorID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
                    ////    }
                    ////}
                    ////else
                    ////{
                    //#endregion
                    //VarQuality = Convert.ToInt32(ddQuality.SelectedValue) > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
                    //VarDesign = Convert.ToInt32(ddDesign.SelectedValue) > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
                    //VarColor = ddColor.Visible == true && Convert.ToInt32(ddColor.SelectedValue) > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0;
                    ////}
                    //int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
                    //int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
                    //int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;

                    //if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                    //{
                    //    _arrpara[7].Value = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                    //}
                    //else
                    //{
                    //    //_arrpara[7].Value = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                    //    _arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])));
                    //}

                    ////_arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])));
                    string stockno11 = "";
                    //if (ChkForMulipleRolls.Checked == true)
                    //{
                    for (int i = 0; i < DGStock.Rows.Count; i++)
                    {
                        GridViewRow row = DGStock.Rows[i];
                        if (((CheckBox)row.FindControl("Chkbox")).Checked == true)
                        {
                            if (stockno11 == "")
                            {

                                stockno11 = DGStock.DataKeys[i].Value.ToString();
                            }
                            else
                            {
                                stockno11 = stockno11 + ',' + DGStock.DataKeys[i].Value.ToString();
                            }

                        }
                    }
                    _arrpara[16].Value = stockno11;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
                    ViewState["PACKINGDETAILID"] = _arrpara[37].Value;
                    //}
                    //else
                    //{
                    //    _arrpara[16].Value = TxtStockNo.Text.ToUpper();
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
                    //}
                    Tran.Commit();
                    Fill_Grid();
                    Save_Referce();
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                    //TxtStockNo.Focus();
                    BindPackingOrderDetail();
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
                    Tran.Rollback();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = ex.Message;
                    if (DGOrderDetail.Rows.Count == 0)
                    {
                        ViewState["PackingID"] = 0;
                    }
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                LblErrorMessage.Text = "Pack Qty Should be greater than zero pcs";
            }
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (ChkForWithoutOrder.Checked == false && chksamplepack.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
            {
                goto a;
            }
        }
        if (chksamplepack.Checked == true)
        {
            if (UtilityModule.VALIDTEXTBOX(txtSampleCustomerOrderNo) == false)
            {
                goto a;
            }
        }

        if (UtilityModule.VALIDDROPDOWNLIST(DDConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRollNoFrom) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtBales) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRollNoTo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPcsPerRoll) == false)
        {
            goto a;
        }

        if (UtilityModule.VALIDTEXTBOX(TxtTotalPcs) == false)
        {
            goto a;
        }

        //if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        //{
        //    goto a;
        //}
        //if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        //{
        //    goto a;

        //}
        //if (ddQuality.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (ddDesign.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (ddColor.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (ddShape.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (ddSize.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (ddShade.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
        //    {
        //        goto a;
        //    }
        //}
        if (UtilityModule.VALIDTEXTBOX(TxtWidth) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLength) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPrice) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtArea) == false)
        {
            goto a;
        }
        if (ChkForWithoutOrder.Checked == false && chksamplepack.Checked == false)
        {
            if (UtilityModule.VALIDTEXTBOX(TxtTotalQty) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPackQty) == false)
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
    private void Save_Referce()
    {
        TxtRollNoFrom.Text = Convert.ToString(Convert.ToInt32(TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text) + 1);
        //// TxtStockNo.Text = "";
        //if (Session["varcompanyno"].ToString() != "30")
        //{
        //    if (ddCategoryName.Items.Count > 0)
        //    {
        //        ddCategoryName.SelectedIndex = 0;
        //        Category_SelectedIndex_Change();
        //        ItemNameSelectedIndexChange();
        //        DDSubQuality.Items.Clear();
        //    }
        //    //if (DDCustomerOrderNo.Items.Count > 0)
        //    //{
        //    //    DDCustomerOrderNo.SelectedIndex = 0;
        //    //}
        //}
        TxtWidth.Text = "";
        TxtLength.Text = "";
        // TxtProdCode.Text = "";
        TxtArea.Text = "";
        TxtPrice.Text = "";
        //TxtTotalPcs.Text = "";
        TxtPackQty.Text = "";
        TxtRemarks.Text = "";
        hnid.Value = "0";
        hnpackingid.Value = "0";
        Txtpurchasecode.Text = "";
        TxtBuyer.Text = "";
        //TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "";
        TxtBales.Text = "";
        TxtPcsPerRoll.Text = "";
        TxtTotalPcs.Text = "";
        TxtTotalQty.Text = "0";
        txtuccnumber.Text = "";
        txtrugid.Text = "";
        txtlengthBale.Text = "";
        txtwidthBale.Text = "";
        txtheightbale.Text = "";
        txtcbmbale.Text = "";
        txtSinglePcsNetWt.Text = "";
        txtSinglePcsGrossWt.Text = "";
        TxtRatePerPcs.Text = "";
        txtStyleNo.Text = "";

        if (Session["varcompanyno"].ToString() == "30")
        {
            TxtRollNoFrom.Text = (Convert.ToInt32(TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text) + 1).ToString();
            TxtBales.Text = "";
            TxtRollNoTo.Text = "0";
            TxtPcsPerRoll.Text = "";
        }
        if (chksamplepack.Checked == true)
        {
            hnsampletype.Value = "2";
        }
        else
        {
            hnsampletype.Value = "1";
        }
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
        BindTabIndex();
        if (DGOrderDetail.Rows.Count > 0)
        {
            DGOrderDetail.Visible = true;
            DDWareHouseCode.Enabled = false;
        }
        else
        {
            DGOrderDetail.Visible = true;
            DDWareHouseCode.Enabled = true;
        }


    }
    //********************************Function To Get Data to fill Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int VarCalType = 0;
            if (RDAreaWise.Checked == true)
            {
                VarCalType = 0;
            }
            else
            {
                VarCalType = 1;
            }
            string strsql = @"Select ID Sr_No,PackingId,Category_Name Category,Item_Name ItemName,PI.Quality,PI.Design,PI.Color,ShapeName, 
                              PI.Width+'x'+PI.Length as Size,Pcs Qty,Price Rate,Area,
                              Case When " + VarCalType + @"=0 Then Area*Price Else Pcs*Price End Amount ,dbo.F_GetstockNo(ID)  As StockNo,RollFrom,RollTo,UCCNumber,RUGID,
                              case when PI.OrderId=0 then isnull(PI.CustomerOrderNo,'') Else isnull(OM.customerorderNo,'') End as CustomerorderNo,isnull(PI.RatePerPcs,0) as RatePerPcs,isnull(PI.SinglePcsNetWt,0) as SinglePcsNetWt,
                              PI.Width,PI.Length
                              From PackingInformation PI inner join " + ViewFinishedItem + @" VF on PI.Finishedid=VF.Item_Finished_id left join ordermaster OM on PI.Orderid=OM.orderid Where PI.PackingId=" + ViewState["PackingID"] + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by RollFrom asc";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            txttotalareagrid.Text = "";
            txttotalpcsgrid.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalpcsgrid.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
                txttotalareagrid.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(Area)", "")), 4).ToString();
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPackingNew.aspx");
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
        if (TxtTotalPcs.Text != "" && TxtPrePackQty.Text != "" && TxtPackQty.Text != "")
        {
            if (Convert.ToInt32(TxtPackQty.Text) > (Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text)))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Pending Qty";
                TxtPackQty.Text = "";
                TxtPackQty.Focus();
            }
            else
            {
                ForCheckAllRows();
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        lnkchnginvoice.Visible = false;
        TRSearchInvoiceNo.Visible = false;
        if (ChkForEdit.Checked == true)
        {
            TRSearchInvoiceNo.Visible = true;
            TDDDInvoiceNo.Visible = true;
            ddInvoiceNo.Items.Clear();
            TDChkForChangeQDC.Visible = false;
            CustomerCodeSelectedIndexChange();
            DGStock.Visible = false;
            Save_Referce();
            ViewState["PackingID"] = 0;
            lnkchnginvoice.Visible = true;

        }
        else
        {
            TRSearchInvoiceNo.Visible = false;
            TDChkForChangeQDC.Visible = false;
            TDDDInvoiceNo.Visible = false;
            ddInvoiceNo.Items.Clear();
            DGStock.Visible = false;
            Save_Referce();
            TxtInvoiceNo.Text = "";
            TxtDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            DGOrderDetail.Visible = false;
            hnid.Value = "0";
            hnpackingid.Value = "0";
            ViewState["PackingID"] = 0;
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 0;
            }
            if (DDConsignee.Items.Count > 0)
            {
                DDConsignee.SelectedIndex = 0;
            }
        }
    }
    protected void ddInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        string str = @"Select PackingId,InvoiceNo,Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingDate,CurrencyId,UnitId,TinvoiceNo//                   
        //                    From Packing (Nolock) 
        //                    Where PackingId = " + ddInvoiceNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
        //                    Select Distinct RollFrom, RollFrom Rollfrom1 
        //                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @" order by Rollfrom1
        //                    Select IsNull(Max(RollTo), 0) + 1 MaxRollNo, MulipleRollFlag 
        //                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @"
        //                    Group by MulipleRollFlag";

        string str = @"Select distinct P.PackingId,P.InvoiceNo,Replace(Convert(VarChar(11),P.PackingDate,106), ' ','-') PackingDate,P.CurrencyId,P.UnitId,P.TinvoiceNo,PI.OrderId,
                    isnull(PI.CustomerOrderNo,'') as CustomerOrderNo ,isnull(P.WareHouseId,0) WareHouseId
                    From Packing(Nolock) P JOIN Packinginformation(Nolock) PI ON P.PackingId=PI.PackingId 
                    Where P.PackingId = " + ddInvoiceNo.SelectedValue + " And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                    Select Distinct RollFrom, RollFrom Rollfrom1 
                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @" order by Rollfrom1
                    Select IsNull(Max(RollTo), 0) + 1 MaxRollNo, MulipleRollFlag 
                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @"
                    Group by MulipleRollFlag";



        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            //if (Ds.Tables[0].Rows[0]["OrderId"].ToString() == "0")
            //{
            //    chksamplepack.Checked = true;
            //    chksamplepack.Enabled = false;
            //    TDCustomerOrderNo.Visible = false;
            //    TDSampleCustomerOrderNo.Visible = true;
            //    //txtSampleCustomerOrderNo.Enabled = false;
            //    //txtSampleCustomerOrderNo.Text = Ds.Tables[0].Rows[0]["CustomerOrderNo"].ToString();
            //    Fill_Category();
            //}
            DDWareHouseCode.SelectedValue = Ds.Tables[0].Rows[0]["WareHouseId"].ToString();
            DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
            TxtInvoiceNo.Text = Ds.Tables[0].Rows[0]["TinvoiceNo"].ToString();
            ViewState["PackingID"] = Ds.Tables[0].Rows[0]["PackingId"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["PackingDate"].ToString();
            Fill_Grid();
            ChkForChangeQDCCheckedChange();


            //TRcarpetSet.Visible = true;
            ////From Roll

            UtilityModule.ConditionalComboFillWithDS(ref DDfromroll, Ds, 1, true, "--SELECT--");
        }
        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            if (Ds.Tables[2].Rows.Count > 0)
            {
                if (Ds.Tables[2].Rows[0]["MulipleRollFlag"].ToString() == "1")
                {
                    ChkForMulipleRolls.Checked = true;
                    ChkForMulipleRolls_CheckedChanged();
                }
                TxtRollNoFrom.Text = Ds.Tables[2].Rows[0]["MaxRollNo"].ToString();
            }
        }
    }
    protected void ChkForChangeQDC_CheckedChanged(object sender, EventArgs e)
    {
        ChkForChangeQDCCheckedChange();
    }
    private void ChkForChangeQDCCheckedChange()
    {
        if (ChkForChangeQDC.Checked == true && ddInvoiceNo.SelectedIndex > 0)
        {
            TrDownGrid.Visible = true;
            string Str = @"Select Replace(Str(PackingId)+'|'+Str(Finishedid)+'|'+Str(CQSRNO)+'|'+Str(CDSRNO)+'|'+Str(CCSRNO),' ','') SrNo,
                       Category_Name Category,Item_Name Item,Quality,Design,Color
                       From PackingInFormation PI," + ViewFinishedItem + @" VF Where PI.Finishedid=VF.Item_Finished_id And PI.PackingId=" + ddInvoiceNo.SelectedValue + "  And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                       Group By PackingId,Finishedid,CQSRNO,CDSRNO,CCSRNO,Category_Name,Item_Name,Quality,Design,Color,Price";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGChangeQDC.DataSource = Ds;
            DGChangeQDC.DataBind();
        }
        else
        {
            TrDownGrid.Visible = false;
        }
    }
    protected void BtnSaveChangeQDC_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara1 = new SqlParameter[20];
            for (int i = 0; i < DGChangeQDC.Rows.Count; i++)
            {
                string StrID = DGChangeQDC.DataKeys[i].Value.ToString();
                //TextBox TxtCATEGORY = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtCATEGORY");
                //TextBox TxtITEM = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtITEM");
                TextBox TxtQUALITY = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtQUALITY");
                TextBox TxtDESIGN = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtDESIGN");
                TextBox TxtCOLOR = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtCOLOR");

                _arrpara1[0] = new SqlParameter("@VarPackingId", SqlDbType.Int);
                _arrpara1[1] = new SqlParameter("@VarFinishedid", SqlDbType.Int);
                _arrpara1[2] = new SqlParameter("@VarCQSRNO", SqlDbType.Int);
                _arrpara1[3] = new SqlParameter("@VarCDSRNO", SqlDbType.Int);
                _arrpara1[4] = new SqlParameter("@VarCCSRNO", SqlDbType.Int);
                _arrpara1[5] = new SqlParameter("@Quality", SqlDbType.NVarChar, 250);
                _arrpara1[6] = new SqlParameter("@Design", SqlDbType.NVarChar, 250);
                _arrpara1[7] = new SqlParameter("@Color", SqlDbType.NVarChar, 250);

                _arrpara1[0].Value = Convert.ToInt32(StrID.Split('|')[0]);
                _arrpara1[1].Value = Convert.ToInt32(StrID.Split('|')[1]);
                _arrpara1[2].Value = Convert.ToInt32(StrID.Split('|')[2]);
                _arrpara1[3].Value = Convert.ToInt32(StrID.Split('|')[3]);
                _arrpara1[4].Value = Convert.ToInt32(StrID.Split('|')[4]);
                _arrpara1[5].Value = TxtQUALITY.Text;
                _arrpara1[6].Value = TxtDESIGN.Text;
                _arrpara1[7].Value = TxtCOLOR.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION_QDC_Change", _arrpara1);
            }
            Tran.Commit();
            ChkForChangeQDC.Checked = false;
            ChkForChangeQDCCheckedChange();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BindTabIndex()
    {
        int TabIndexNo = DGOrderDetail.Rows.Count;

        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            TextBox txtRollFrom = (TextBox)DGOrderDetail.Rows[i].FindControl("txtRollFrom");
            TextBox txtQualityName = (TextBox)DGOrderDetail.Rows[i].FindControl("txtQualityName");
            TextBox txtDesignName = (TextBox)DGOrderDetail.Rows[i].FindControl("txtDesignName");
            TextBox txtColorName = (TextBox)DGOrderDetail.Rows[i].FindControl("txtColorName");
            TextBox txtWidth = (TextBox)DGOrderDetail.Rows[i].FindControl("txtWidth");
            TextBox txtLength = (TextBox)DGOrderDetail.Rows[i].FindControl("txtLength");
            TextBox txtRate = (TextBox)DGOrderDetail.Rows[i].FindControl("txtRate");

            txtRollFrom.TabIndex = Convert.ToInt16(i + 1);
            txtQualityName.TabIndex = Convert.ToInt16(TabIndexNo + 1);
            txtDesignName.TabIndex = Convert.ToInt16(TabIndexNo + TabIndexNo + 1);
            txtColorName.TabIndex = Convert.ToInt16(TabIndexNo + TabIndexNo + TabIndexNo + 1);
            txtWidth.TabIndex = Convert.ToInt16(TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + 1);
            txtLength.TabIndex = Convert.ToInt16(TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + 1);
            txtRate.TabIndex = Convert.ToInt16(TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + TabIndexNo + 1);
        }
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);

            //if (e.Row.RowIndex == 0)
            //    e.Row.Style.Add("width", "20px");


            Label lblQty = (Label)e.Row.FindControl("lblQty");

            Label lblArea = (Label)e.Row.FindControl("lblArea");
            Label lblOnePcsArea = (Label)e.Row.FindControl("lblOnePcsArea");

            lblOnePcsArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) / Convert.ToDecimal(lblQty.Text));


            for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "30")
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
                    {
                        DGOrderDetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    //protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    //{
    //    LblErrorMessage.Text = "";
    //    LblErrorMessage.Visible = false;
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId from " + ViewFinishedItem + @" v ,orderdetail od Where OD.Item_Finished_Id=V.Item_Finished_Id and od.orderid=" + DDCustomerOrderNo.SelectedValue + " and ProductCode='" + TxtProdCode.Text + "'");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        if (ddCategoryName.Items.Count > 0)
    //        {
    //            ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    //            Category_SelectedIndex_Change();
    //        }
    //        if (ddItemName.Items.Count > 0)
    //        {
    //            ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
    //            ItemNameSelectedIndexChange();
    //        }
    //        if (ddQuality.Items.Count > 0 && ddQuality.Visible == true)
    //        {
    //            ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
    //            ComboFill();
    //            Fill_Price();
    //        }
    //        if (ddDesign.Items.Count > 0 && ddDesign.Visible == true)
    //        {
    //            ddDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
    //            DesignSelectedChange();
    //        }
    //        if (ddColor.Items.Count > 0 && ddColor.Visible == true)
    //        {
    //            ddColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
    //            Fill_Price();
    //        }
    //        if (ddShape.Items.Count > 0 && ddShape.Visible == true)
    //        {
    //            ddShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
    //            ShapeSelectedChange();
    //        }
    //        if (ddSize.Items.Count > 0 && ddSize.Visible == true)
    //        {
    //            ddSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
    //            SizeSelectedIndexChange();
    //        }
    //        if (ddShade.Items.Count > 0 && ddShade.Visible == true)
    //        {
    //            ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
    //            Fill_Price();
    //        }
    //    }
    //    else
    //    {
    //        LblErrorMessage.Text = "Product Code Does Not Exist";
    //        LblErrorMessage.Visible = true;
    //    }
    //}
    protected void TxtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkForEdit.Checked == true)
        {
            str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And PackingId<>" + ddInvoiceNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
        }
        else
        {
            str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
        }
        if (str != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Opn1", "alert('Invoice Number " + str + " Already Exists...');", true);
            TxtInvoiceNo.Text = "";
        }
    }


    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            array[1] = new SqlParameter("@PackingDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("Lblpacking")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletePacking", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Delete", "alert('" + array[2].Value + "');", true);
            Fill_Grid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    public string getStockNo(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select strcarpets From [dbo].[GetTStockNosPack](" + strVal + ",0)");
        val = ds.Tables[0].Rows[0]["strcarpets"].ToString();
        return val;
    }

    protected void DGOrderDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderDetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGOrderDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderDetail.EditIndex = -1;
        Fill_Grid();
    }
    protected void DGOrderDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[7];
            array[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            array[1] = new SqlParameter("@PackingDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[3] = new SqlParameter("@UCCNumber", SqlDbType.VarChar, 100);
            array[4] = new SqlParameter("@RUGID", SqlDbType.VarChar, 100);
            array[5] = new SqlParameter("@Rate", SqlDbType.Float);
            array[6] = new SqlParameter("@SinglePcsNetWt", SqlDbType.Float);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("Lblpacking")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;
            array[3].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtUccNumber")).Text;
            array[4].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtrugid")).Text;
            array[5].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtRate")).Text;
            array[6].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtSinglePcsNetWt")).Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UPdatePacking]", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "update", "alert('" + array[2].Value + "');", true);
            DGOrderDetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }


    }
    private void FtAreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarAreaNew, int VarFactor)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootHeight = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int FootHeightInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        int InchHeight = 0;
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";

        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text == "" ? "0" : VarLengthNew.Text));
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
            case "12":
                InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
                InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
                break;
            default:
                if (VarLengthNew.Text != "")
                {
                    if (VarLengthNew.Text != "")
                    {
                        FootLength = Convert.ToInt32(Str.Split('.')[0]);
                        FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootLengthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12";
                        VarLengthNew.Text = "";
                        VarLengthNew.Focus();
                    }
                }
                if (VarWidthNew.Text != "")
                {
                    Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
                    if (VarWidthNew.Text != "")
                    {
                        FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                        FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootWidthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12";
                        VarWidthNew.Text = "";
                        VarWidthNew.Focus();
                    }
                }
                InchLength = (FootLength * 12) + FootLengthInch;
                InchWidth = (FootWidth * 12) + FootWidthInch;
                break;
        }
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);
        VarAreaNew.Text = Convert.ToString(VarArea);


    }
    protected void DDfromroll_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDtoroll, "select Distinct RollTo,RollTo as RollTo1 From Packinginformation Where PackingId=" + ddInvoiceNo.SelectedValue + " and RollFrom>=" + DDfromroll.SelectedValue + " order by RollTo1", true, "--Plz Select--");
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        string reporttitle = "";
        if (DDfromroll.SelectedIndex > 0)
        {
            reporttitle = reporttitle + " From Roll : " + DDfromroll.SelectedValue;
        }
        if (DDtoroll.SelectedIndex > 0)
        {
            reporttitle = reporttitle + " To Roll : " + DDtoroll.SelectedValue;
        }
        string str = @"select Vf.QualityName as Quality,vf.designName as Design,vf.ColorName as Color,Width+'x'+Length as Size,Sum(Pd.pcs) as Qty,
                    P.TInvoiceNo,'" + reporttitle + @"' as Reporttitle";

        if (Session["varMasterCompanyIDForERP"].ToString() == "16")
        {
            str = str + " ,'' RollFrom, '' RollTo";
        }
        else
        {
            str = str + " ,PD.RollFrom,PD.RollTo";
        }
        str = str + @" From Packing P inner Join PackingInformation Pd on P.PackingId=pd.PackingId
                     inner join " + ViewFinishedItem + @" vf on PD.FinishedId=vf.ITEM_FINISHED_ID
                     Where P.Packingid=" + ddInvoiceNo.SelectedValue;

        if (DDfromroll.SelectedIndex > 0)
        {
            str = str + " and RollFrom>=" + DDfromroll.SelectedValue + "";
        }
        if (DDtoroll.SelectedIndex > 0)
        {
            str = str + " and RollTO<=" + DDtoroll.SelectedValue + "";
        }
        str = str + " group by Vf.QualityName,vf.designName,vf.ColorName,Width,Length,P.TInvoiceNo";
        if (Session["varMasterCompanyIDForERP"].ToString() != "16")
        {
            str = str + " ,PD.RollFrom,PD.RollTo";
            str = str + " Order By PD.RollFrom";
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "reports/rptcarpetset.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptcarpetset.xsd";
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
    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void lnkchnginvoice_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Invoiceid", ddInvoiceNo.SelectedValue);
            param[1] = new SqlParameter("@InvoiceNo", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Value = TxtInvoiceNo.Text;
            param[2] = new SqlParameter("@Invoicedate", SqlDbType.VarChar, 50);
            param[2].Direction = ParameterDirection.InputOutput;
            param[2].Value = TxtDate.Text;
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEINVOICENODATE", param);
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = param[3].Value.ToString();

        }
        catch (Exception ex)
        {
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGOrderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGOrderDetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void TxtSearchInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtSearchInvoiceNo.Text != "")
        {
            try
            {
                string str = @"select consignorId,cosigneeId,InvoiceYear,InvoiceId,TInvoiceNo 
                From INVOICE With (Nolock) 
                Where Consignorid = " + DDCompanyName.SelectedValue + " And Tinvoiceno='" + TxtSearchInvoiceNo.Text + "'";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DDCustomerCode.Items.FindByValue(ds.Tables[0].Rows[0]["cosigneeId"].ToString()) != null)
                    {
                        DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["cosigneeId"].ToString();
                        CustomerCodeSelectedIndexChange();
                    }
                    if (ddInvoiceNo.Items.FindByValue(ds.Tables[0].Rows[0]["InvoiceId"].ToString()) != null)
                    {
                        ddInvoiceNo.SelectedValue = ds.Tables[0].Rows[0]["InvoiceId"].ToString();
                        ddInvoiceNo_SelectedIndexChanged(sender, new EventArgs());
                    }

                    DDWareHouseCode_SelectedIndexChanged(sender, new EventArgs());
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altinv", "alert('Invalid Invoice No. !!!')", true);
                }
                TxtSearchInvoiceNo.Text = "";
                TxtSearchInvoiceNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
    protected void ChangeRate()
    {
        string OrderUnitId = "", PackingUnitid = "";
        string str = @"Select Distinct OM.OrderId,OD.OrderUnitid,OD.ordercaltype From OrderMaster(NoLock) OM JOIN OrderDetail (NoLock) OD ON OM.OrderId=OD.OrderId Where OM.OrderId=" + DDCustomerOrderNo.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            OrderUnitId = ds.Tables[0].Rows[0]["OrderUnitid"].ToString();
        }
        if (DDUnit.SelectedIndex > 0)
        {
            PackingUnitid = DDUnit.SelectedValue;
        }
        if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "1")
        {
            if ((RDAreaWise.Checked))
            {
                TxtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) / Convert.ToDecimal(TxtArea.Text), 2));

            }
        }
        if (OrderUnitId != "" && PackingUnitid != "")
        {
            if (OrderUnitId == "2" && PackingUnitid == "1")
            {
                TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) / 10.764), 2));

                TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
            }
            if (OrderUnitId == "1" && PackingUnitid == "2")
            {
                TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) * 10.764), 2));

                TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
            }
        }
    }
    protected void GVPackingOrderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVPackingOrderDetail.PageIndex = e.NewPageIndex;
        BindPackingOrderDetail();
    }
    private DataSet BindPackingOrderDetail()
    {
        DataSet DS = new DataSet();

        string str = "";

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.item_id=" + ddItemName.SelectedValue;
        }
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            if (ddQuality.SelectedIndex > 0)
            {
                str = str + " and CQ.SrNo=" + ddQuality.SelectedValue;
            }
        }
        else
        {
            if (ddQuality.SelectedIndex > 0)
            {
                str = str + " and VF.QualityId=" + ddQuality.SelectedValue;
            }
        }

        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            if (ddDesign.SelectedIndex > 0)
            {
                str = str + " and CD.SrNo=" + ddDesign.SelectedValue;
            }
        }
        else
        {
            if (ddDesign.SelectedIndex > 0)
            {
                str = str + " and VF.DesignId=" + ddDesign.SelectedValue;
            }
        }

        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            if (ddColor.SelectedIndex > 0)
            {
                str = str + " and CC.SrNo=" + ddColor.SelectedValue;
            }
        }
        else
        {
            if (ddColor.SelectedIndex > 0)
            {
                str = str + " and VF.ColorId=" + ddColor.SelectedValue;
            }
        }

        if (ddShape.SelectedIndex > 0)
        {
            str = str + " and VF.ShapeId=" + ddShape.SelectedValue;
        }
        if (ddSize.SelectedIndex > 0)
        {
            str = str + " and VF.SizeId=" + ddSize.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GetPackingOrderDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerCodeId", DDCustomerCode.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerOrderId", DDCustomerOrderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@hnsampletype", hnsampletype.Value);
        cmd.Parameters.AddWithValue("@ChkWithoutOrder", ChkForWithoutOrder.Checked == false ? "0" : "1");
        cmd.Parameters.AddWithValue("@UnitId", DDUnit.SelectedValue);
        cmd.Parameters.AddWithValue("@WareHouseID", DDWareHouseCode.SelectedValue);


        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();
        //***********
        if (DS.Tables[0].Rows.Count > 0)
        {
            GVPackingOrderDetail.DataSource = DS.Tables[0];
            GVPackingOrderDetail.DataBind();

            // DS.Dispose();

        }
        else
        {
            GVPackingOrderDetail.DataSource = null;
            GVPackingOrderDetail.DataBind();
        }
        return DS;
    }
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }

    }
    protected void GVPackingOrderDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet ddd = new DataSet();
        ddd = BindPackingOrderDetail();
        DataTable dt = ddd.Tables[0];
        string sortingDirection = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            sortingDirection = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            sortingDirection = "Asc";
        }
        DataView sortedView = new DataView(dt);
        sortedView.Sort = e.SortExpression + " " + sortingDirection;
        //Session["objects"] = sortedView;
        GVPackingOrderDetail.DataSource = sortedView;
        GVPackingOrderDetail.DataBind();
    }
    protected void Chkboxitem_CheckedChanged(object sender, EventArgs e)
    {
        //ddSize_SelectedIndexChanged(sender, e);

        CheckBox chkboxitem = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkboxitem.Parent.Parent;
        Label lblSizeId = (Label)row.FindControl("lblSizeId");
        if (chkboxitem != null)
        {
            if (chkboxitem.Checked == true)
            {
                ChkBoxSelectedSizeId = lblSizeId.Text;
                ddSize_SelectedIndexChanged(sender, e);
                if (TxtRollNoFrom.Text == "")
                {
                    TxtRollNoFrom.Text = "1";
                    RollNoFromTextChanged();
                }
                RollNoFromTextChanged();
            }
            else
            {
                TxtWidth.Text = "";
                TxtLength.Text = "";
                TxtPrice.Text = "";
                TxtArea.Text = "";
                TxtTotalQty.Text = "";
                ChkBoxSelectedSizeId = "0";
                TxtTotalPcs.Text = "0";
                TxtBales.Text = "0";
                TxtRollNoTo.Text = "0";

            }
        }
        BtnSave.Focus();

    }
    protected void GetFinishedIdRatePackQty()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            for (int i = 0; i < GVPackingOrderDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)GVPackingOrderDetail.Rows[i].FindControl("Chkboxitem"));

                if (Chkboxitem.Checked == true)
                {
                    Label lblItemId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblItemId"));
                    Label lblQualityId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblQualityId"));
                    Label lblDesignId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblDesignId"));
                    Label lblColorId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblColorId"));
                    Label lblShapeId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblShapeId"));
                    Label lblSizeId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblSizeId"));
                    Label lblShadeColorId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblShadeColorId"));
                    Label lblOrderId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblOrderId"));
                    Label lblQualityName = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblQualityName"));
                    Label lblDesignName = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblDesignName"));
                    Label lblcolorname = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblcolorname"));
                    Label lblBalanceQty = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblBalanceQty"));

                    QualityName = lblQualityName.Text;
                    DesignName = lblDesignName.Text;
                    ColorName = lblcolorname.Text;

                    QualityId = Convert.ToInt32(lblQualityId.Text);
                    DesignId = Convert.ToInt32(lblDesignId.Text);
                    ColorId = Convert.ToInt32(lblColorId.Text);
                    TxtTotalPcs.Text = lblBalanceQty.Text;

                    //*********************
                    //***********       

                    SqlParameter[] param = new SqlParameter[16];
                    param[0] = new SqlParameter("@OrderId", lblOrderId.Text);
                    param[1] = new SqlParameter("@ITEM_ID", lblItemId.Text);
                    param[2] = new SqlParameter("@QualityId", lblQualityId.Text);
                    param[3] = new SqlParameter("@DesignId", lblDesignId.Text);
                    param[4] = new SqlParameter("@ColorId", lblColorId.Text);
                    param[5] = new SqlParameter("@SHAPE_ID", lblShapeId.Text);
                    param[6] = new SqlParameter("@SIZE_ID", lblSizeId.Text);
                    // param[7] = new SqlParameter("@ProCode", TxtProdCode.Text);
                    param[7] = new SqlParameter("@ProCode", "0");
                    param[8] = new SqlParameter("@SHADECOLOR_ID", lblShadeColorId.Text);
                    param[9] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                    param[10] = new SqlParameter("@Rate", SqlDbType.Float);
                    param[10].Direction = ParameterDirection.Output;
                    param[11] = new SqlParameter("@OrderQty", SqlDbType.Int);
                    param[11].Direction = ParameterDirection.Output;
                    param[12] = new SqlParameter("@PrePackQty", SqlDbType.Int);
                    param[12].Direction = ParameterDirection.Output;
                    param[13] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                    param[13].Direction = ParameterDirection.Output;
                    param[14] = new SqlParameter("@ChkWithoutOrder", ChkForWithoutOrder.Checked == true ? "1" : "0");
                    param[15] = new SqlParameter("@hnsampletype", hnsampletype.Value);

                    /// param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                    ///param[2].Direction = ParameterDirection.Output;
                    //**************
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_FinishedId_OrderRate_OrderQty_PrePackQty", param);
                    TxtTotalQty.Text = param[11].Value.ToString();
                    TxtPrice.Text = param[10].Value.ToString();
                    TxtPrePackQty.Text = param[12].Value.ToString();
                    ItemFinishedId = Convert.ToInt32(param[13].Value.ToString());
                    //LblErrorMessage.Text = param[2].Value.ToString();
                    TxtPackQty.Text = TxtTotalPcs.Text;
                    Tran.Commit();
                    //FillBeamDetail();
                    //FillGrid();
                }
            }




        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkForExtraPcs_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //Get the value of column from the DataKeys using the RowIndex.
        int id = Convert.ToInt32(DGOrderDetail.DataKeys[rowIndex].Values[0]);

        //ViewState["InvoiceId"] = id;

        string Strdetail = "";

        Label Lblpacking = ((Label)DGOrderDetail.Rows[rowIndex].FindControl("Lblpacking"));
        Label lblID = ((Label)DGOrderDetail.Rows[rowIndex].FindControl("lblID"));
        TextBox txtQualityName = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtQualityName"));
        TextBox txtDesignName = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtDesignName"));
        TextBox txtColorName = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtColorName"));
        TextBox txtRollFrom = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtRollFrom"));
        TextBox txtRollTo = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtRollTo"));
        TextBox txtWidth = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtWidth"));
        TextBox txtLength = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtLength"));
        TextBox txtRate = ((TextBox)DGOrderDetail.Rows[rowIndex].FindControl("txtRate"));


        if ((Convert.ToInt32(txtRollFrom.Text) <= 0) && (Convert.ToInt32(txtRollTo.Text) <= 0) && (Convert.ToDecimal(txtRate.Text) <= 0) && (Convert.ToDecimal(txtWidth.Text) <= 0) && (Convert.ToDecimal(txtLength.Text) <= 0))   // Change when Updated Completed
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill all textboxes values');", true);
            //txtWidth.Focus();
            return;
        }
        else if (txtQualityName.Text != "" && txtDesignName.Text != "" && txtColorName.Text != "" && (Convert.ToInt32(txtRollFrom.Text) >= 0) && (Convert.ToInt32(txtRollTo.Text) >= 0) && (Convert.ToDecimal(txtRate.Text) >= 0) && (Convert.ToDecimal(txtWidth.Text) >= 0) && (Convert.ToDecimal(txtLength.Text) >= 0))   // Change when Updated Completed
        {
            Strdetail = Strdetail + Lblpacking.Text + '|' + lblID.Text + '|' + txtQualityName.Text + '|' + txtDesignName.Text + '|' + txtColorName.Text + '|' + txtRollFrom.Text + '|' + txtRollTo.Text + '|' + txtWidth.Text + '|' + txtLength.Text + '|' + txtRate.Text + '~';
        }

        if (Strdetail != "")
        {
            LblErrorMessage.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] array = new SqlParameter[4];
                array[0] = new SqlParameter("@PackingId", Lblpacking.Text);
                array[1] = new SqlParameter("@StringDetail", Strdetail);
                array[2] = new SqlParameter("@UnitID", DDUnit.SelectedValue);
                array[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                array[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UPdatePackingDetail]", array);
                Tran.Commit();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "update", "alert('" + array[2].Value + "');", true);
                DGOrderDetail.EditIndex = -1;
                Fill_Grid();
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }


    }
    protected void BtnUpdateAll_Click(object sender, EventArgs e)
    {
        string packingid = "0";
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            ////CheckBox Chkboxitem = ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkboxitem"));            

            Label Lblpacking = ((Label)DGOrderDetail.Rows[i].FindControl("Lblpacking"));
            Label lblID = ((Label)DGOrderDetail.Rows[i].FindControl("lblID"));
            TextBox txtQualityName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtQualityName"));
            TextBox txtDesignName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtDesignName"));
            TextBox txtColorName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtColorName"));
            TextBox txtRollFrom = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollFrom"));
            //TextBox txtRollTo = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollTo"));
            TextBox txtWidth = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtWidth"));
            TextBox txtLength = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtLength"));
            TextBox txtRate = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRate"));
            Label lblRollTo = ((Label)DGOrderDetail.Rows[i].FindControl("lblRollTo"));

            packingid = Lblpacking.Text;

            if ((Convert.ToInt32(txtRollFrom.Text) <= 0) && (Convert.ToInt32(lblRollTo.Text) <= 0) && (Convert.ToDecimal(txtRate.Text) <= 0) && (Convert.ToDecimal(txtWidth.Text) <= 0) && (Convert.ToDecimal(txtLength.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill all textboxes values');", true);
                //txtWidth.Focus();
                return;
            }
        }

        string Strdetail = "";
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            ////CheckBox Chkboxitem = ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkboxitem"));


            Label Lblpacking = ((Label)DGOrderDetail.Rows[i].FindControl("Lblpacking"));
            Label lblID = ((Label)DGOrderDetail.Rows[i].FindControl("lblID"));
            TextBox txtQualityName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtQualityName"));
            TextBox txtDesignName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtDesignName"));
            TextBox txtColorName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtColorName"));
            TextBox txtRollFrom = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollFrom"));
            //TextBox txtRollTo = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollTo"));
            TextBox txtWidth = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtWidth"));
            TextBox txtLength = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtLength"));
            TextBox txtRate = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRate"));
            Label lblRollTo = ((Label)DGOrderDetail.Rows[i].FindControl("lblRollTo"));

            if (txtQualityName.Text != "" && txtDesignName.Text != "" && txtColorName.Text != "" && (Convert.ToInt32(txtRollFrom.Text) >= 0) && (Convert.ToInt32(lblRollTo.Text) >= 0) && (Convert.ToDecimal(txtRate.Text) >= 0) && (Convert.ToDecimal(txtWidth.Text) >= 0) && (Convert.ToDecimal(txtLength.Text) >= 0))   // Change when Updated Completed
            {
                Strdetail = Strdetail + Lblpacking.Text + '|' + lblID.Text + '|' + txtQualityName.Text + '|' + txtDesignName.Text + '|' + txtColorName.Text + '|' + txtRollFrom.Text + '|' + lblRollTo.Text + '|' + txtWidth.Text + '|' + txtLength.Text + '|' + txtRate.Text + '~';
            }

        }

        if (Strdetail != "")
        {
            LblErrorMessage.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] array = new SqlParameter[4];
                array[0] = new SqlParameter("@PackingId", packingid);
                array[1] = new SqlParameter("@StringDetail", Strdetail);
                array[2] = new SqlParameter("@UnitID", DDUnit.SelectedValue);
                array[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                array[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UPdatePackingDetail]", array);
                Tran.Commit();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "update", "alert('" + array[3].Value + "');", true);

                Fill_Grid();
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
    }
    protected void BtnDeleteAll_Click(object sender, EventArgs e)
    {
        string packingid = "0";
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            ////CheckBox Chkboxitem = ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkboxitem"));            

            Label Lblpacking = ((Label)DGOrderDetail.Rows[i].FindControl("Lblpacking"));
            Label lblID = ((Label)DGOrderDetail.Rows[i].FindControl("lblID"));
            TextBox txtQualityName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtQualityName"));
            TextBox txtDesignName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtDesignName"));
            TextBox txtColorName = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtColorName"));
            TextBox txtRollFrom = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollFrom"));
            TextBox txtRollTo = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRollTo"));
            TextBox txtWidth = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtWidth"));
            TextBox txtLength = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtLength"));
            TextBox txtRate = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRate"));

            packingid = Lblpacking.Text;
            break;

        }

        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            array[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@UserId", SqlDbType.Int);

            array[0].Value = packingid;
            array[1].Direction = ParameterDirection.Output;
            array[2].Value = Session["varMasterCompanyIDForERP"];
            array[3].Value = Session["VarUserId"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletePackingByPackingId", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Delete", "alert('" + array[1].Value + "');", true);
            Fill_Grid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}