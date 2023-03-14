using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Packing_FrmPackingDestini : System.Web.UI.Page
{
    static int MasterCompanyId;
    string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (IsPostBack == false)
        {
            
            ViewState["InvoiceId"] = 0;
            ViewState["Roll"] = 0;
            ViewState["UpdateRoll"] = 0;
            ViewState["PkgInfoID"] = 0;
            ViewState["IsEdit"] = 0;
            ViewState["FinishedID"] = 0;
            logo();
            lablechange();
//            string qry = @"select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
//                    SELECT DISTINCT CI.CustomerId,CI.CustomerCode FROM CustomerInfo CI,READYTOPACK RP,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID AND RP.ORDERID=OM.ORDERID And CI.MasterCompanyId=" + Session["varCompanyId"] + @" ORDER BY CustomerCode
//                    Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + @"
//                    Select UnitId,UnitName from Unit Where MasterCompanyId=" + Session["varCompanyId"];
            string qry = @"select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                    SELECT DISTINCT CI.CustomerId,CI.CustomerCode FROM CustomerInfo CI,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID And CI.MasterCompanyId=" + Session["varCompanyId"] + @" ORDER BY CustomerCode
                    Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + @"
                    Select UnitId,UnitName from Unit Where MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--SELECT--");

            CustomerCodeSelectedIndexChange();
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 2, true, "--SELECT--");
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 3, true, "--SELECT--");

            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ParameteLabel();
            TxtCompanyID.Text = Session["varCompanyNo"].ToString();
            if (TxtCompanyID.Text == "2")
            {
                tdrollnobales.Visible = false;
                tdpcsperroll.Visible = true;
                tdtotalpcs.Visible = true;
                tdsrno.Visible = false;
            }
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            if (VarProdCode == 1)
            {
                TDProdCode.Visible = true;
                TDStockNo.Visible = false;
                ChkForQtyWise.Checked = true;
                TDFinishType.Visible = true;
                RDAreaWise.Checked = false;
                RDPcsWise.Checked = true;
                DDUnit.SelectedValue = "4";
            }
            else
            {
                RDPcsWise.Checked = false;
                RDAreaWise.Checked = true;
                TDProdCode.Visible = false;
                TDStockNo.Visible = true;
                ChkForQtyWise.Checked = false;
                TDFinishType.Visible = false;
                DDUnit.SelectedIndex = 1;
            }
            CheckedMultipleRolls();
            //ViewState["InvoiceId"] = 0;
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
        DDConsignee.Focus();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDConsignee, "Select CustomerId,CompanyName From  CustomerInfo where CustomerID=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Companyname", true, "--SELECT--");
        if (DDConsignee.Items.Count > 0)
        {
            DDConsignee.SelectedIndex = 1;
        }
        //if (ChkEditOrder.Checked == true)
        //    UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select DISTINCT OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo CustomerOrderNo from PackingInformation RP,ORDERMASTER OM Where RP.ORDERID=OM.ORDERID AND OM.CompanyId=" + DDCompanyName.SelectedValue + " And OM.Customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
        //else
        //    UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select DISTINCT OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo CustomerOrderNo from READYTOPACK RP,ORDERMASTER OM Where RP.ORDERID=OM.ORDERID AND OM.CompanyId=" + DDCompanyName.SelectedValue + " And OM.Customerid=" + DDCustomerCode.SelectedValue + " And RP.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select DISTINCT OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo CustomerOrderNo from PackingInformation RP,ORDERMASTER OM Where RP.ORDERID=OM.ORDERID AND OM.CompanyId=" + DDCompanyName.SelectedValue + " And OM.Customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
        else
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select DISTINCT OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo CustomerOrderNo from ORDERMASTER OM Where OM.CompanyId=" + DDCompanyName.SelectedValue + " And OM.Customerid=" + DDCustomerCode.SelectedValue + " And OM.CompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void ChkForMulipleRolls_CheckedChanged(object sender, EventArgs e)
    {
        TxtRollNoFrom.Text = "0";
        TxtRollNoTo.Text = "0";
        TxtTotalPcs.Text = "0";
        TxtPcsPerRoll.Text = "0";
        TxtBales.Text = "";
        CheckedMultipleRolls();
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
            TxtRollNoTo.Enabled = true;
            TxtTotalPcs.Enabled = true;
            TxtBales.Enabled = false;
        }
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        string Str = "";
        int VarNumber = 0;
        DataSet Ds;
        LblErrorMessage.Text = "";
        if (TxtStockNo.Text != "")
        {
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from CarpetNumber CN,V_FinishedItemDetail VF Where CN.Item_Finished_Id=VF.Item_Finished_Id And TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (Ds.Tables[0].Rows[0]["Pack"].ToString() == "1")
                {
                    VarNumber = 1;
                    LblErrorMessage.Text = "AllReady Packed";
                    TxtStockNo.Text = "";
                    TxtStockNo.Focus();
                }
                if (Ds.Tables[0].Rows[0]["IssRecStatus"].ToString() == "1")
                {
                    VarNumber = 1;
                    LblErrorMessage.Text = "This Stock No Issue To Any Process Pls Receive It";
                    TxtStockNo.Text = "";
                    TxtStockNo.Focus();
                }
                if (VarNumber == 0)
                {
                    Str = "Select OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Orderid=" + Ds.Tables[0].Rows[0]["Orderid"] + "";
                    if (ChkForWithoutOrder.Checked != true)
                    {
                        Str = Str + @" And Customerid=" + DDCustomerCode.SelectedValue;
                    }
                    UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, Str, true, "--SELECT--");
                    if (DDCustomerCode.Items.Count > 0)
                    {
                        DDCustomerOrderNo.SelectedIndex = 1;
                    }
                    Str = "Select Distinct Category_ID,Category_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And VF.MasterCompanyId=" + Session["varCompanyId"];
                    if (ChkForWithoutOrder.Checked != true)
                    {
                        Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
                    }
                    UtilityModule.ConditionalComboFill(ref ddCategoryName, Str, true, "--SELECT--");
                    if (ddCategoryName.Items.Count > 0)
                    {
                        ddCategoryName.SelectedIndex = 1;
                    }
                    ddCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["Category_ID"].ToString();
                    Category_SelectedIndex_Change();
                    ddItemName.SelectedValue = Ds.Tables[0].Rows[0]["Item_ID"].ToString();
                    ItemNameSelectedIndexChange();
                    ddQuality.SelectedValue = Ds.Tables[0].Rows[0]["QualityID"].ToString();
                    ComboFill();
                    if (TDDesign.Visible == true)
                    {
                        ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["DesignID"].ToString();
                    }
                    if (TDColor.Visible == true)
                    {
                        ddColor.SelectedValue = Ds.Tables[0].Rows[0]["ColorID"].ToString();
                    }
                    if (TDShape.Visible == true)
                    {
                        ddShape.SelectedValue = Ds.Tables[0].Rows[0]["ShapeID"].ToString();
                        ShapeSelectedChange();
                        ddSize.SelectedValue = Ds.Tables[0].Rows[0]["SizeID"].ToString();
                    }
                    if (TDShade.Visible == true)
                    {
                        ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["shadeColorID"].ToString();
                    }
                    SizeSelectedIndexChange();
                }
            }
            else
            {
                LblErrorMessage.Text = "Stock No Does Not Exits Pls Enter Correct Stock No";
                TxtStockNo.Text = "";
                TxtStockNo.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            Category_SelectedIndex_Change();
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct Category_ID,Category_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        TxtProdCode.Focus();
        Fill_Grid_Show();
        if (ChkEditOrder.Checked == true && DDCustomerOrderNo.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddinvoiceno, "select distinct pi.packingid,pi.tpackingno from ordermaster p,PackingInformation pi,Customerinfo CI where p.orderid=pi.orderid And CI.CustomerId=P.CustomerId And CI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
            TxtInvoiceNo.Focus();
        }
    }
    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Category_SelectedIndex_Change();
        ddItemName.Focus();
    }
    private void Category_SelectedIndex_Change()
    {
        ddlcategorycange();
        string Str = "Select Distinct VF.Item_ID,VF.Item_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (ChkForWithoutOrder.Checked != true)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
        }
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
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCategoryName.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                        TDWidth.Visible = true;
                        TDLength.Visible = true;
                        TDArea.Visible = true;
                        TDSize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedIndexChange();
        ddQuality.Focus();
    }
    private void ItemNameSelectedIndexChange()
    {
        string Str = ""; ;
        if (chkfinal.Checked == true)
        {
            Str = "Select Distinct VF.QualityID,VF.QualityName from READYTOPACK RP,V_FinishedItemDetail VF Where RP.FinishedId=VF.Item_Finished_Id And RP.ORDERID=" + DDCustomerOrderNo.SelectedValue + " AND Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            Str = "Select Distinct VF.QualityID,VF.QualityName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And OD.ORDERID=" + DDCustomerOrderNo.SelectedValue + " AND Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"]; ;
        }
        if (ChkForWithoutOrder.Checked != true)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref ddQuality, Str, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboFill();
        Fill_Finish_Type();
        DDFinishType.Focus();
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
            string Str = "Select Distinct VF.DesignID,VF.DesignName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (ChkForWithoutOrder.Checked != true)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref ddDesign, Str, true, "--Select--");
        }
        if (TDColor.Visible == true)
        {
            string Str = "Select Distinct VF.ColorId,VF.ColorName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (ChkForWithoutOrder.Checked != true)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
            }
            UtilityModule.ConditionalComboFill(ref ddColor, Str, true, "--Select--");
        }
        if (TDShape.Visible == true)
        {
            string Str = "Select Distinct VF.ShapeId,VF.ShapeName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (ChkForWithoutOrder.Checked != true)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
            }
            UtilityModule.ConditionalComboFill(ref ddShape, Str, true, "--Select--");
        }
        if (TDShade.Visible == true)
        {
            string Str = "Select Distinct VF.ShadeColorId,VF.ShadeColorName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (ChkForWithoutOrder.Checked != true)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
            }
            UtilityModule.ConditionalComboFill(ref ddShade, Str, true, "--Select--");
        }
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
    }
    private void ShapeSelectedChange()
    {
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtArea.Text = "";
        TxtPrice.Text = "";
        string Str = "Select Distinct VF.SizeId,Case When 1=" + DDUnit.SelectedValue + " Then SizeMtr Else SizeFt End SizeName from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.ShapeId=" + ddShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (ChkForWithoutOrder.Checked != true)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
        }
        UtilityModule.ConditionalComboFill(ref ddSize, Str, true, "--Select--");
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeSelectedIndexChange();
    }
    private void SizeSelectedIndexChange()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Case When 1=" + DDUnit.SelectedValue + @" Then WidthMtr Else WidthFt End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then LengthMtr Else LengthFt End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then AreaMtr Else AreaFt End Area from Size Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
            TxtLength.Text = Ds.Tables[0].Rows[0]["Length"].ToString();
            TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
        }
        Fill_Finish_Type();
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
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, 0));
            }
        }
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    protected void ddShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Finish_Type();
    }
    private void Fill_Price()
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
            int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
            if (finishedid > 0)
            {
                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from OrderDetail Where Orderid=" + DDCustomerOrderNo.SelectedValue + " And Item_Finished_Id=" + finishedid);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    TxtPrice.Text = Ds.Tables[0].Rows[0]["UnitRate"].ToString();
                    if (ChkForQtyWise.Checked == false)
                    {
                        TxtPackQty.Text = "1";
                    }
                }
            }
        }
    }
    protected void TxtRollNoFrom_TextChanged(object sender, EventArgs e)
    {
        if (TxtRollNoFrom.Text != "")
        {
            TxtBales.Text = "1";
            if (TxtPcsPerRoll.Text == "")
            {
                TxtPcsPerRoll.Text = "1";
            }
            TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(TxtBales.Text) - 1).ToString();
            TxtTotalPcs.Text = (Convert.ToDouble(TxtPcsPerRoll.Text) * Convert.ToDouble(TxtBales.Text)).ToString();
            TxtRollNoTo.Focus();
        }
    }

    protected void TxtRollNoTo_TextChanged(object sender, EventArgs e)
    {
        TxtPcsPerRoll.Focus();
    }

    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
        TxtDate.Focus();
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
        if (UtilityModule.VALIDTEXTBOX(TxtStockNo) == false)
        {
            goto a;
        }
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
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            if (LblErrorMessage.Text == "")
            {
                int num = 0;
                string VarInvoiceYear = "";
                if (Convert.ToInt32(ViewState["InvoiceId"]) == 0)
                {
                    num = 1;
                }
                if (num == 1)
                {
                    SqlParameter[] _arrpara1 = new SqlParameter[10];
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
                    _arrpara1[0].Value = 0;
                    _arrpara1[0].Direction = ParameterDirection.InputOutput;
                    _arrpara1[1].Value = DDCompanyName.SelectedValue;
                    _arrpara1[2].Value = DDCustomerCode.SelectedValue;
                    _arrpara1[3].Value = TxtDate.Text;
                    _arrpara1[4].Value = DDCurrency.SelectedValue;
                    _arrpara1[5].Value = DDUnit.SelectedValue;
                    _arrpara1[6].Value = Session["varuserid"];
                    _arrpara1[7].Value = TxtInvoiceNo.Text;
                    _arrpara1[8].Value = Session["varCompanyId"];
                    string VarInvoiceMonth = DateTime.Now.ToString("MM");
                    if (Convert.ToInt32(VarInvoiceMonth) > 04)
                    {
                        VarInvoiceYear = DateTime.Now.ToString("yyyy");
                    }
                    else
                    {
                        VarInvoiceYear = DateTime.Now.ToString("yyyy");
                        VarInvoiceYear = (Convert.ToInt32(VarInvoiceYear) - 1).ToString();
                    }
                    _arrpara1[9].Value = VarInvoiceYear;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKING_AND_INVOICE", _arrpara1);
                    ViewState["InvoiceId"] = _arrpara1[0].Value;
                    LblErrorMessage.Text = "";
                    BtnSave.Text = "Save";
                }
                //if (ViewState["UpdateRoll"].ToString() == "1")
                //{
                    if (ViewState["Roll"].ToString() == "0")
                    {
                        //if (ChkEditOrder.Checked == true)
                        //{
                            int TotalCurRoll = Convert.ToInt32(TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text == "" ? "0" : TxtRollNoFrom.Text) + 1;
                            SqlParameter[] _param = new SqlParameter[3];
                            _param[0] = new SqlParameter("@PackingId", ViewState["InvoiceId"]);
                            _param[1] = new SqlParameter("@TotalCurRoll", TotalCurRoll);
                            _param[2] = new SqlParameter("@OrderID", DDCustomerOrderNo.SelectedValue);
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Packing_UpdateRollOnEdit", _param);
                       // }
                    }
               // }
                if (ViewState["Roll"].ToString() == "1")
                {
                    //if (ChkEditOrder.Checked == true)
                    //{
                        int TotalCurRoll = Convert.ToInt32(TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text == "" ? "0" : TxtRollNoFrom.Text) + 1;
                        SqlParameter[] _param = new SqlParameter[4];
                        _param[0] = new SqlParameter("@PackingId", ViewState["InvoiceId"]);
                        _param[1] = new SqlParameter("@TotalCurRoll", TotalCurRoll);
                        _param[2] = new SqlParameter("@OrderID", DDCustomerOrderNo.SelectedValue);
                        _param[3] = new SqlParameter("@PkgInfoID", ViewState["PkgInfoID"]);
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Packing_UpdateRollOnEdit_1", _param);
                   // }
                }
                SqlParameter[] _arrpara = new SqlParameter[33];
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
                _arrpara[16] = new SqlParameter("@TStockNo", SqlDbType.NVarChar, 50);
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
                _arrpara[29] = new SqlParameter("@CalType", SqlDbType.Int);
                _arrpara[30] = new SqlParameter("@Finished_Type_ID", SqlDbType.Int);
                _arrpara[31] = new SqlParameter("@stock", SqlDbType.Int);
                _arrpara[32] = new SqlParameter("@IsEdit", SqlDbType.Int);
                

                _arrpara[0].Value = ViewState["InvoiceId"];
                _arrpara[1].Value = TxtRollNoFrom.Text == "" ? "0" : TxtRollNoFrom.Text;
                _arrpara[2].Value = TxtBales.Text == "" ? "0" : TxtBales.Text;
                _arrpara[3].Value = 0;
                _arrpara[4].Value = "";
                _arrpara[5].Value = "";
                _arrpara[6].Value = "";
                _arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, "", Convert.ToInt32(Session["varCompanyId"])));
                _arrpara[8].Value = TxtWidth.Text == "" ? "0" : TxtWidth.Text;
                _arrpara[9].Value = TxtLength.Text == "" ? "0" : TxtLength.Text;
                _arrpara[10].Value = TxtPackQty.Text == "" ? "0" : TxtPackQty.Text;
                _arrpara[11].Value = TxtArea.Text == "" ? "0" : (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtPackQty.Text)).ToString();
                _arrpara[12].Value = TxtPrice.Text;
                _arrpara[13].Value = TxtSrNo.Text == "" ? "0" : TxtSrNo.Text;
                _arrpara[14].Value = ChkForWithoutOrder.Checked == true ? "0" : DDCustomerOrderNo.SelectedValue;
                _arrpara[15].Value = 0;
                _arrpara[16].Value = TxtStockNo.Text == "" ? " " : TxtStockNo.Text;
                _arrpara[17].Value = 0;
                _arrpara[18].Value = TxtInvoiceNo.Text;
                _arrpara[19].Value = TxtRollNoFrom.Text == "" ? "0" : TxtRollNoFrom.Text;
                _arrpara[20].Value = TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text;
                _arrpara[21].Value = TxtPcsPerRoll.Text == "" ? "0" : TxtPcsPerRoll.Text;
                _arrpara[22].Value = TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text;
                _arrpara[23].Value = TxtBales.Text == "" ? "0" : TxtTotalPcs.Text;
                _arrpara[24].Value = 0;
                _arrpara[25].Value = 0;
                _arrpara[26].Value = 0;
                _arrpara[27].Value = TxtRemarks.Text;
                _arrpara[28].Value = TxtDate.Text;
                _arrpara[29].Value = RDAreaWise.Checked == true ? 0 : 1;
                _arrpara[30].Value = TDFinishType.Visible == true ? DDFinishType.SelectedValue : "0";
                _arrpara[31].Value = chkfinal.Checked == true ? 1 : 0;
                _arrpara[32].Value = ViewState["IsEdit"];
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION_Destini", _arrpara);

               
                Tran.Commit();
                Fill_Grid();
                Save_Referce();
                                              
            }
        }
        catch (Exception ex)
        {
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
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
        if (UtilityModule.VALIDTEXTBOX(TxtPackQty) == false)
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
    private void Save_Referce()
    {
        TxtStockNo.Text = "";
        TxtProdCode.Text = "";
        ProdCode_TextChanges();
        ddCategoryName.SelectedIndex = 0;
        Category_SelectedIndex_Change();
        ItemNameSelectedIndexChange();
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtArea.Text = "";
        TxtPrice.Text = "";
        TxtPackQty.Text = "";
        TxtRemarks.Text = "";
        TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "";
        TxtPcsPerRoll.Text = "";
        TxtTotalPcs.Text = "";
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
        if (DGOrderDetail.Rows.Count > 0)
        {
            DGOrderDetail.Visible = true;
            if (ChkEditOrder.Checked==true)
            {
                DGOrderDetail.Columns[8].Visible = true;
            }
        }
        else
            DGOrderDetail.Visible = false;
    }
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
            string strsql = @"Select ID Sr_No,Category_Name Category,Item_Name ItemName,QualityName+Space(2)+DesignName+Space(2)+ColorName+Space(2)+ShapeName+Space(2)+ 
                              Case When " + DDUnit.SelectedValue + @"=1 Then SizeMtr Else SizeFt End+Space(2)+ShadeColorName Description,Pcs Qty,Price Rate,Area,
                              Case When " + VarCalType + @"=0 Then Area*Price Else Pcs*Price End Amount ,TStockNo StockNo  from PackingInformation PI,V_FinishedItemDetail VF 
                              Where PI.Finishedid=VF.Item_Finished_id And PI.PackingId=" + ViewState["InvoiceId"] + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch
        {
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
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
       // ViewState["Roll"] = 1;
        ProdCode_TextChanges();
        TxtPackQty.Focus();
    }
    private void ProdCode_TextChanges()
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        if (TxtProdCode.Text != "")
        {
            if (chkfinal.Checked == true)
            {
                Str = "SELECT DISTINCT IM.CATEGORY_ID,IM.CATEGORY_NAME FROM V_FinishedItemDetail VF,READYTOPACK RP,ITEM_CATEGORY_MASTER IM,CategorySeparate CS WHERE VF.Item_Finished_Id=RP.FinishedId AND VF.CATEGORY_ID=IM.CATEGORY_ID AND IM.Category_Id=CS.CategoryId And CS.Id=0 AND RP.Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                Str = "SELECT DISTINCT IM.CATEGORY_ID,IM.CATEGORY_NAME FROM V_FinishedItemDetail VF,OrderDetail RP,ITEM_CATEGORY_MASTER IM,CategorySeparate CS WHERE VF.Item_Finished_Id=RP.Item_Finished_Id AND VF.CATEGORY_ID=IM.CATEGORY_ID AND IM.Category_Id=CS.CategoryId And CS.Id=0 AND RP.Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            UtilityModule.ConditionalComboFill(ref ddCategoryName, Str, true, "--SELECT--");
            Str = "SELECT IPM.*,IM.CATEGORY_ID From ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM WHERE IPM.ITEM_ID=IM.ITEM_ID And ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
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
                    Fill_Size();
                }
                if (TDSize.Visible == true)
                {
                    ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                if (TDShade.Visible == true)
                {
                    ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeCOLOR_ID"].ToString();
                }
                Fill_Finish_Type();
                if (ChkEditOrder.Checked == false)
                {

                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT unitrate FROM ORDERDETAIL WHERE ORDERID=" + DDCustomerOrderNo.SelectedValue + " AND item_finished_id=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + "");
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        TxtPrice.Text = ds2.Tables[0].Rows[0][0].ToString();
                    }
                    else
                        TxtPrice.Text = "0";
                }
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
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;
        string str = "";
        if (chkfinal.Checked == true)
        {
            str = "SELECT DISTINCT IM.ITEM_ID,IM.ITEM_NAME FROM V_FinishedItemDetail VF,ITEM_MASTER IM,READYTOPACK RP Where VF.Item_ID=IM.Item_Id And RP.FinishedId=VF.Item_Finished_Id And RP.Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.CATEGORY_ID=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            str = "SELECT DISTINCT IM.ITEM_ID,IM.ITEM_NAME FROM V_FinishedItemDetail VF,ITEM_MASTER IM,OrderDetail OD Where VF.Item_ID=IM.Item_Id And OD.Item_Finished_Id=VF.Item_Finished_Id And OD.Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.CATEGORY_ID=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "SELECT--");
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
    private void Fill_Size()
    {
        int VarCompanyNo = Convert.ToInt32(Session["varCompanyNo"].ToString());
        switch (VarCompanyNo)
        {
            case 3:
                if (ddShape.SelectedValue == "3")
                {
                    if (Convert.ToInt32(DDUnit.SelectedValue) == 1)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr+'x'+str(HeightMtr) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt+'x'+str(HeightFt) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDUnit.SelectedValue) == 6)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch+'x'+str(Heightinch) Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                }
                else
                {
                    if (Convert.ToInt32(DDUnit.SelectedValue) == 1)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                    else if (Convert.ToInt32(DDUnit.SelectedValue) == 6)
                    {
                        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                    }
                }
                break;
            default:
                if (Convert.ToInt32(DDUnit.SelectedValue) == 1)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                else if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                else if (Convert.ToInt32(DDUnit.SelectedValue) == 6)
                {
                    UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid", true, "--SELECT--");
                }
                break;
        }
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
                ViewState["FinishedID"] = finishedid;
                if (finishedid > 0)
                {
                    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from OrderDetail Where Orderid=" + DDCustomerOrderNo.SelectedValue + " And Item_Finished_Id=" + finishedid);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        if (ChkEditOrder.Checked == false)
                        {
                            TxtPrice.Text = Ds.Tables[0].Rows[0]["UnitRate"].ToString();
                        }
                        if (ChkForQtyWise.Checked == false)
                        {
                            TxtPackQty.Text = "1";
                        }
                    }
                    //TxtTotalQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select ISNULL(SUM(Qty),0) from ReadyToPack Where CompanyId=" + DDCompanyName.SelectedValue + " And Orderid=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                    //TxtPrePackQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Sum(Pcs),0) from Packing P,PackingInformation PI Where P.PackingId=PI.PackingId And P.ConsignorID=" + DDCompanyName.SelectedValue + " And PI.Orderid=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And P.MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                    //UtilityModule.ConditionalComboFill(ref DDFinishType, "SELECT DISTINCT FT.ID,FT.FINISHED_TYPE_NAME FROM READYTOPACK RP,FINISHED_TYPE FT Where RP.FINISH_TYPE_ID=FT.ID AND FINISHEDID=" + finishedid + " And RP.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                    if (chkfinal.Checked == true)
                    {
                        TxtTotalQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select ISNULL(SUM(Qty),0) from ReadyToPack Where CompanyId=" + DDCompanyName.SelectedValue + " And Orderid=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                        TxtPrePackQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Sum(Pcs),0) from Packing P,PackingInformation PI Where P.PackingId=PI.PackingId And P.ConsignorID=" + DDCompanyName.SelectedValue + " And PI.Orderid=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And P.MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                        UtilityModule.ConditionalComboFill(ref DDFinishType, "SELECT DISTINCT FT.ID,FT.FINISHED_TYPE_NAME FROM READYTOPACK RP,FINISHED_TYPE FT Where RP.FINISH_TYPE_ID=FT.ID AND FINISHEDID=" + finishedid + " And RP.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                    }
                    else
                    {
                        TxtTotalQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(QtyRequired,0) from ordermaster OM, orderdetail OD where OM.Orderid=OD.Orderid AND OM.orderid =" + DDCustomerOrderNo.SelectedValue + " and Item_Finished_Id=" + finishedid + " AND CompanyID=" + Session["varCompanyId"] + "")).ToString();
                        TxtPrePackQty.Text = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Sum(Pcs),0) from Packing P,PackingInformation PI Where P.PackingId=PI.PackingId And P.ConsignorID=" + DDCompanyName.SelectedValue + " And PI.Orderid=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " And P.MasterCompanyId=" + Session["varCompanyId"] + "")).ToString();
                        UtilityModule.ConditionalComboFill(ref DDFinishType, "SELECT DISTINCT FT.ID,FT.FINISHED_TYPE_NAME FROM order_consumption_detail OCD,FINISHED_TYPE FT Where OCD.O_FINISHED_TYPE_ID=FT.ID AND OFINISHEDID=" + finishedid + " ", true, "--SELECT--");
                    }

                    if (DDFinishType.Items.Count > 0)
                    {
                        DDFinishType.SelectedIndex = 1;
                    }
                    Tran.Commit();
                }
            }
            catch (Exception ex)
            {
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
    protected void RDPcsWise_CheckedChanged(object sender, EventArgs e)
    {
        RadioCheckChange();
    }
    protected void RDAreaWise_CheckedChanged(object sender, EventArgs e)
    {
        RadioCheckChange();
    }
    private void RadioCheckChange()
    {
        if (RDPcsWise.Checked == true)
        {
            RDAreaWise.Checked = false;
            RDAreaWise.Checked = true;
        }
        else if (RDAreaWise.Checked == true)
        {
            RDAreaWise.Checked = true;
            RDAreaWise.Checked = false;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT DISTINCT vf.ProductCode FROM ITEM_PARAMETER_MASTER IPM,OrderDetail RP,CategorySeparate CS,V_FinishedItemDetail VF WHERE RP.Item_Finished_Id=IPM.Item_Finished_Id And RP.Item_Finished_Id=VF.Item_Finished_Id And VF.Category_Id=CS.CategoryId And CS.Id=0 And vf.ProductCode Like '" + prefixText + "%' And VF.MasterCompanyId=" + MasterCompanyId + "";
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
  
    private void Fill_Grid_Show()
    {
        DGSHOWDATA.DataSource = "";
        if (DDCustomerCode.SelectedIndex > 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Distinct ProductCode from orderdetail ID,ITEM_PARAMETER_MASTER IPM WHERE ID.item_FINISHED_ID=IPM.ITEM_FINISHED_ID And ID.orderid=" + DDCustomerOrderNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "");
            DGSHOWDATA.DataSource = Ds;
            DGSHOWDATA.DataBind();
            if (DGSHOWDATA.Rows.Count > 0)
                DGSHOWDATA.Visible = true;
            else
                DGSHOWDATA.Visible = false;
        }
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            refreshchk();
            TxtInvoiceNo.Enabled = false;
            tdinvoice.Visible = true;
        }
        else
        {
            refreshchk();
            TxtInvoiceNo.Enabled = true;
            tdinvoice.Visible = false;
        }
    }
    private void refreshchk()
    {
        DDCustomerCode.SelectedIndex = 0;
        DDConsignee.SelectedIndex = -1;
        TxtInvoiceNo.Text = "";
       // DDCurrency.SelectedIndex = 0;
        DGOrderDetail.Visible = false;
        DGSHOWDATA.Visible = false;
        //DDUnit.SelectedIndex = 0;
        //TxtDate.Text = "";
        TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "";
        TxtPcsPerRoll.Text = "";
        DDCustomerOrderNo.Items.Clear();
        TxtProdCode.Text = "";
        ddCategoryName.Items.Clear();
        ddItemName.Items.Clear();
        ddQuality.Items.Clear();
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShape.Items.Clear();
        DDFinishType.Items.Clear();
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtArea.Text = "";
        TxtTotalQty.Text = "";
        TxtPrePackQty.Text = "";
        TxtPackQty.Text = "";
        TxtRemarks.Text = "";
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT DISTINCT CI.CustomerId,CI.CustomerCode FROM CustomerInfo CI,packingInformation RP,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID AND RP.ORDERID=OM.ORDERID And CI.MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY CustomerCode", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT DISTINCT CI.CustomerId,CI.CustomerCode FROM CustomerInfo CI,OrderDetail RP,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID AND RP.ORDERID=OM.ORDERID And CI.MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY CustomerCode", true, "--SELECT--");
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
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            ViewState["IsEdit"] = 1;
        }
        ViewState["Roll"] = 1;
        int n = DGOrderDetail.SelectedIndex;
        string id = ((Label)DGOrderDetail.Rows[n].FindControl("lblid")).Text;
        ViewState["PkgInfoID"] = id;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select p.invoiceno,p.consigneeid,replace(convert(varchar(11),p.packingdate,106) ,' ','-') as pdate,p.currencyid,p.unitid,pi.* from PackingInformation pi,packing p where  p.packingid=pi.packingid and id=" + id + " And P.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["InvoiceId"] = ds.Tables[0].Rows[0]["packingid"].ToString();
            TxtInvoiceNo.Text = ds.Tables[0].Rows[0]["invoiceno"].ToString();
            DDConsignee.SelectedValue = ds.Tables[0].Rows[0]["consigneeid"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["currencyid"].ToString();
            DDUnit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["pdate"].ToString();
            TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            TxtRollNoFrom.Text = ds.Tables[0].Rows[0]["rollfrom"].ToString();
            TxtRollNoTo.Text = ds.Tables[0].Rows[0]["rollto"].ToString();
            TxtPcsPerRoll.Text = ds.Tables[0].Rows[0]["rpcs"].ToString();
            TxtPrice.Text = ds.Tables[0].Rows[0]["Price"].ToString();
            TxtTotalPcs.Text = ds.Tables[0].Rows[0]["TotalPCS"].ToString();

            if (ds.Tables[0].Rows[0]["IsFinal"].ToString() == "1")
                chkfinal.Checked = true;
            else
                chkfinal.Checked = false;
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select productcode from item_parameter_master where item_finished_id=" + ds.Tables[0].Rows[0]["finishedid"].ToString() + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            TxtProdCode.Text = ds1.Tables[0].Rows[0][0].ToString();
            ProdCode_TextChanges();
            TxtPackQty.Text = DGOrderDetail.Rows[n].Cells[3].Text;
        }
    }
    protected void ddinvoiceno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["InvoiceId"] = ddinvoiceno.SelectedValue.ToString();
        string str = @"Select  replace(convert(varchar(11),PackingDate,106) ,' ','-') as pdate,CurrencyId,UnitId,InvoiceNo from packing where packingid= " + ddinvoiceno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if(ds.Tables[0].Rows.Count > 0)
        {
            TxtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["pdate"].ToString();
        }
        TxtInvoiceNo.Text = ddinvoiceno.SelectedItem.Text.Trim();
        Fill_Grid();
        TxtProdCode.Focus();
    }
    protected void TxtTotalPcs_TextChanged(object sender, EventArgs e)
    {
        TotalPcsChanged();
    }

    protected void TotalPcsChanged()
    {
        string str="";
        if (ViewState["Roll"].ToString() == "0")
        {
            //if (ChkEditOrder.Checked == false)
            //{
            //    str = "select isnull(max(rollto),0)+1 from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "' ";
            //}
            //else
            //{
            //    str = "select isnull(MAX(rollto),0)+1 from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "'  AND FinishedId=(select DISTINCT ITEM_FINISHED_ID from item_parameter_MASTER WHERE PRODUCTCODE='" + TxtProdCode.Text + "' )";
            //}
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            str = "select distinct packingid from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "' and orderid=" + DDCustomerOrderNo.SelectedValue;
            DataSet Qds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (Qds.Tables[0].Rows.Count > 0)
            {
                str = "select isnull(max(rollto),0)+1 from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "'  AND orderid= " + DDCustomerOrderNo.SelectedValue;
                ViewState["UpdateRoll"] = 1;
            }
            else
            {
                str = "select isnull(max(rollto),0)+1 from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "' ";
                ViewState["UpdateRoll"] = 0;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(rollto),0)+1 from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "' ");
            TxtRollNoFrom.Text = ds.Tables[0].Rows[0][0].ToString();
           
        }
        else
        {
             str = @"select isnull(MAX(rollfrom),0) from PackingInformation where tpackingno='" + TxtInvoiceNo.Text + "'  AND FinishedId=(select DISTINCT ITEM_FINISHED_ID from item_parameter_MASTER WHERE PRODUCTCODE='" + TxtProdCode.Text + "' )";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            TxtRollNoFrom.Text = ds.Tables[0].Rows[0][0].ToString();
        }
        int box = 0, rem = 0;
        TxtTotalPcs.Text = TxtTotalPcs.Text == "" ? "1" : TxtTotalPcs.Text;
        box = Convert.ToInt32(TxtTotalPcs.Text) / Convert.ToInt32(TxtPcsPerRoll.Text);
        rem = Convert.ToInt32(TxtTotalPcs.Text) % Convert.ToInt32(TxtPcsPerRoll.Text);
        if (rem > 0)
            box = box + 1;
        TxtRollNoTo.Text = Convert.ToString(Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(box) - 1);

        DDCustomerOrderNo.Focus();
        
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
    private void Invoice()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete From TempPCs");

            string Str = @"Delete From TempPCs                            
                           Insert into TempPCs Select PackingId,VF.QualityName,Sum(Pcs) as Pcs,sum(Area) as Area,Price,OM.CustomerOrderNo OrderNo,Min(RollFrom) As RollFrom,Max(RollTo) as RollTo,Max(RollTo)-Min(RollFrom)+1 as TotalRoll,VF.DesignName,
                           Isnull(PI.ID,0) As SerializeOrder,vf.item_finished_id as finished_id ,pi.remarks as remark,om.orderdate as orderdate,(select * from [dbo].[Get_packingOrderNo](pi.PackingID)) totorder,
                           PI.ItemDescription  as description,pi.rpcs as boxqty,[dbo].[GET_CMB](Vf.Item_Finished_Id),PI.HTSCode
                           From V_FinishedItemDetail VF inner join PackingInformation PI on PI.Finishedid=VF.Item_Finished_Id  Left Outer Join OrderMaster OM
                           ON PI.Orderid=OM.Orderid Left Outer Join CustomerDesign CD on VF.DesignId=CD.DesignId And CD.CustomerId=OM.CustomerId 
                           left Outer Join CustomerColor CC on CC.ColorId=VF.ColorId And CC.CustomerId=OM.CustomerId Where PackingID=" + ViewState["InvoiceId"] + "  And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                           Group by Qualityname,Price,OM.CustomerOrderNo,PackingId,PI.ID,VF.DesignName,vf.item_finished_id,pi.remarks,om.orderdate,
                           vf.CATEGORY_NAME,vf.ITEM_NAME,vf.ColorName,vf.ShapeName,pi.rpcs,DesignNameAToC,ColorNameToC ,PI.HTSCode,PI.ItemDescription  Order by PackingID

                            Delete from temp_Roll";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
            // SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete from temp_Roll");
            //varRoll = StrRollNr()
            Str = "Insert into Temp_Roll Select Distinct PackingId,RollNo,'' SpellRoll,PurchaseCode,BuyerCode  From PackingInformation Where PackingId=" + ViewState["InvoiceId"] + @"
            Delete from Temp_Normal ";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
            // SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete from Temp_Normal");
            int i = 1;
            Str = @"Select Distinct P.PackingId,Q.QualityName,Price,Sum(Area*Pcs) as TArea,Sum(Pcs) as Pc,Sum(Area*Price) TA,Q.QualityId From PackingInformation P,
                   V_FinishedItemDetail VF,Quality Q where P.FinishedId=VF.Item_Finished_Id And VF.QualityId=Q.QualityId And P.PackingId=" + ViewState["InvoiceId"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                   Group By P.PackingId,Q.QualityName,Price,Q.QualityId";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
            FillTemp_Detail();
            if (ds.Tables[0].Rows.Count > 0)
            {
                int Count = ds.Tables[0].Rows.Count;
                for (int j = 0; j < Count; j++)
                {
                    Str = "Insert into Temp_Normal Values(" + ds.Tables[0].Rows[j]["PackingId"] + " ,'" + ds.Tables[0].Rows[j]["QualityName"] + "'," + ds.Tables[0].Rows[j]["Price"] + ", " + ds.Tables[0].Rows[j]["TArea"] + "," + ds.Tables[0].Rows[j]["TA"] + "," + ds.Tables[0].Rows[j]["Pc"] + "," + i + ")";
                    i = i + 1;
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
                }
            }

            Session["CommanFormula"] = "{Invoice.InvoiceID}=" + ViewState["InvoiceId"] + " ";
            int VarCompanyNo = Convert.ToInt32(Session["varcompanyno"]);
            Session["ReportPath"] = "Reports/RptPACKINGType_destiniNEW.rpt";

            string qry = "";
            //DataSet ds = new DataSet();
            qry = @"SELECT Invoice.InvoiceDate,Invoice.TInvoiceNo,Invoice.TBuyerOConsignee,TempPcs.Quality,TempPcs.Pcs,TempPcs.Price,Temp_Detail.CompanyName,Temp_Detail.CompAddr1,Temp_Detail.CompAddr2,Temp_Detail.CompAddr3,Temp_Detail.country,Temp_Detail.PortUnLoad,Temp_Detail.DestinationAdd,Temp_Detail.CarriageName,Temp_Detail.StationName,Temp_Detail.TransModename,Temp_Detail.StationName1,
                   Temp_Detail.PaymentName,Temp_Detail.NoOfRolls,Temp_Detail.CurrencyName,Temp_Detail.GrossWT,Temp_Detail.NetWt,Temp_Detail.Terms,Invoice.GriNo,Invoice.BLNo,Invoice.BlDt,Temp_Detail.IECode,Invoice.NOTIFY,Temp_Detail.CommisionAmount,Temp_Detail.LessAdvance,Temp_Detail.Freight,Temp_Detail.Insurance,Temp_Detail.ComPer,Temp_Detail.ExtraChargeRemarks,Temp_Detail.ExtraCharges,
                   CustomerInfo.CompanyName,CustomerInfo.Address,Invoice.Terms,Invoice.CreditId,Invoice.LcNo,Invoice.Lcdate,Temp_Detail.DilveryName,TempPcs.Design,Invoice.InvoiceId,Sku_No.Sku_no,CustomerInfo.CustomerCode,TempPcs.OrderNo,TempPcs.remark,TempPcs.RollFrom,TempPcs.RollTo,TempPcs.totorder,TempPcs.description,TempPcs.boxqty,TempPcs.TotalRoll,Temp_Detail.UnitName,TempPcs.CBM,TempPcs.HTSCode
                   FROM   TempPcs INNER JOIN Temp_Detail INNER JOIN Invoice ON Temp_Detail.PackingId=Invoice.InvoiceId ON TempPcs.PackignID=Invoice.InvoiceId LEFT OUTER JOIN Sku_No ON TempPcs.finished_id=Sku_No.finished_id INNER JOIN CustomerInfo ON Invoice.cosigneeId=CustomerInfo.CustomerId
                   Where Invoice.Invoiceid=" + ViewState["InvoiceId"] + " ORDER BY TempPcs.OrderNo,TempPcs.Quality,TempPcs.Design,TempPcs.Price ";
                   ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
                   Session["dsFileName"] = "~\\ReportSchema\\RptPACKINGType_destiniNEW.xsd";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            //Session["dsFileName"] = "~\\ReportSchema\\RptInvoiceType_destiniNEW.xsd";
            Session["rptFileName"] = Session["ReportPath"].ToString();
            Session["GetDataset"] = ds;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "OPEN();", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPackingList.aspx");
            Logs.WriteErrorLog("Masters_Packing_FrmInvoiceRate|Fill_Grid_Data|" + ex.Message);
        }
    }
       
    

    private void FillTemp_Detail()
    {
        SqlConnection Con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DataSet ds;
            string Strsql = "";
            SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "Delete from Temp_Detail");
            Strsql = @"Select Isnull(Si.SignatoryName,'') as SignatoryName,CO.CompanyName,CompAddr1,CompAddr2,CompAddr3,RBICode,IECode, CI.CompanyName as CN,isnull(CI.Address,'') as CA,'' CC,'' CS,isnull(CI.country,'') as CCO,CI.PinCode PC,CI.AcNo CAO,CI.MARK, B.BankName ,B.Street,B.City,B.State,B.Country ,
                     ino.InvoiceId,InvoiceDate,INO.PortUnLoad,INO.DestinationAdd,
                     CA.CarriageName,GR.StationName, TM.TransModename,GR1.StationName SN,PA.PaymentName,NoOfRolls,GD.GoodsName,UN.UnitName,CU.CurrencyName,INO.Knots,INO.Contents, INO.GrossWT,INO.NetWt,INO.CommisionPercent,ISNULL(INO.CommisionAmount,0),INO.Freight,INO.Insurance,INO.TypeOFBuyerOtherConsignee, INO.BillToCustomer,PM.CurrencyID ,INO.LessAdvance,'' Pc_Normal,INO.invSubTotal,'' TotalPcs,INO.Terms,INO.FrmBlDate, PA.PaymentId,PM.unitid,INO.BLNo,INO.BlDt,INO.LCNO,INO.VesselName,INO.ShipingId,INO.cosigneeId,INO.ComPer,ino.eref, B1.Street CompBankStreet,B1.City CompBankCity,B1.State CompBankState,B1.Country CompBankCountry, B.SwiftCode CustmBankSwiftCode, B.Email CustmBankEmail,B.Phoneno CustmBankPhone,B.FaxNo CustmBankFax,CI.AcNo As CustmAcNo,INO.RollMark,INO.RollMarkHead, CO.CompTel,CO.UPTTNO as TinNo,CO.EMail,INO.ExtraChargeRemarks,INO.ExtraCharges,CU.CurrencyTypePs 
                     From CustomerInfo CI,Bank as B1 ,Bank B,Carriage CA,GoodsReceipt GR,TransMode TM,GoodsReceipt GR1,Packing PM,Unit UN,CurrencyInfo CU, 
                     CompanyInfo CO Left outer Join Signatory Si On Co.Sigantory=Si.SignatoryID ,Invoice INO left outer join Payment PA on PA.PaymentId=INO.DElTerms left outer join GoodsDesc GD on GD.GoodsID=INO.GoodsId
                     Where CO.CompanyID=INO.Consignorid and CI.CustomerID=INO.Cosigneeid And CI.BankId=B.BankId and
                     CA.CarriageId=INO.PreCarrier and GR.GoodsReceiptId=INO.Receipt And TM.TransModeId=INO.ShipingId and GR1.GoodsReceiptId=INO.PortLoad and PM.packingid=INO.InvoiceId and PM.UnitId=UN.UnitId and PM.CurrencyID=CU.CurrencyID and B1.BankID=Co.BankID and InvoiceId=" + ViewState["InvoiceId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(Con, CommandType.Text, Strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {

                string Str = @"Insert into Temp_Detail Select INO.InvoiceId,CO.CompanyName,CompAddr1,CompAddr2,CompAddr3,RBICode,IECode,INO.RollMarkHead,
                        CI.CompanyName as a1,IsNull(CI.Address,'') as a2,'' a3,'' a4,IsNull(CI.country,'') as  a5,CI.AcNo a6,INO.RollMark,INO.InvoiceId,InvoiceDate,
                        '' OrderNo,'' OrderDate,INO.PortUnLoad,INO.DestinationAdd,CA.CarriageName,GR.StationName,TM.TransModename,GR1.StationName SN,'' DilveryName,
                        PA.PaymentName,NoOfRolls,GD.GoodsName,UN.UnitName,CU.CurrencyName,INO.Knots,INO.Contents,INO.GrossWT,INO.NetWt,INO.CommisionPercent,
                        ISNULL(INO.CommisionAmount,0),INO.LessAdvance,INO.Freight,INO.Insurance,CI.CompanyName As b1,IsNull(CI.Address,'') As b2,'' b3,'' b4,
                        IsNull(CI.country,'') As  b5,CI.AcNo b6,'Pc' varNormal_Pc1,'' varAmountWord,'' varTAmount,'' varTotalPcs,'' varTerms,'' varDueDate,
                        '' varDeclaration,'' SetId,INO.ComPer,ino.eref,Isnull(Si.SignatoryName,'') as SignatoryName,CO.CompTel,CO.UPTTNO AS TinNo,CO.EMail,INO.TInvoiceNo,
                        INO.ExtraChargeRemarks,INO.ExtraCharges,'' VarRollNo,CU.CurrencyTypePs 
                        From CustomerInfo CI,Bank as B1,Bank B,Carriage CA,
                        GoodsReceipt GR,TransMode TM,GoodsReceipt GR1,Packing PM,Unit UN,CurrencyInfo CU,CompanyInfo CO Left outer Join 
                        Signatory Si  On Co.Sigantory=Si.SignatoryID,Invoice INO left outer join Payment PA on PA.PaymentId=INO.DElTerms left outer join GoodsDesc GD on GD.GoodsID=INO.GoodsId
                        Where CO.CompanyID=INO.Consignorid and CI.CustomerID=INO.Cosigneeid and CI.BankId=B.BankId and 
                        CA.CarriageId=INO.PreCarrier and GR.GoodsReceiptId=INO.Receipt 
                        and TM.TransModeId=INO.ShipingId and   
                        GR1.GoodsReceiptId=INO.PortLoad and PM.PackingId=INO.InvoiceId and PM.UnitId=UN.UnitId and PM.CurrencyID=CU.CurrencyID and 
                        B1.BankID=Co.BankID and InvoiceId=" + ViewState["InvoiceId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"];
                SqlHelper.ExecuteNonQuery(Con, CommandType.Text, Str);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPrintInvoicePackingList.aspx");
            Logs.WriteErrorLog("Masters_Packing_FrmInvoiceRate|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();
                Con.Dispose();
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Invoice();
    }
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int i = Convert.ToInt32(DGOrderDetail.DataKeys[e.RowIndex].Value);
        SqlParameter[] _param = new SqlParameter[3];
        _param[0] = new SqlParameter("@PInfoID", i);
        _param[1] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"].ToString());
        _param[2] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
        _param[2].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Packing_Delete", _param);
        ViewState["Roll"]= 0;
        msg = _param[2].Value.ToString();
        MessageSave(msg);
        Fill_Grid();
        //Fill_DataGridShow();
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

    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IsEdit"] = 0;
        TxtPackQty.Text = "";
        TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "";
        TxtTotalPcs.Text = "";

        ViewState["Roll"] = 0;
        int n = DGSHOWDATA.SelectedIndex;
        TxtProdCode.Text = DGSHOWDATA.Rows[n].Cells[0].Text;
        ProdCode_TextChanges();
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT unitrate FROM ORDERDETAIL WHERE ORDERID=" + DDCustomerOrderNo.SelectedValue + " AND item_finished_id= (select DISTINCT ITEM_FINISHED_ID from item_parameter_MASTER WHERE PRODUCTCODE='" + TxtProdCode.Text + "' )" + "");
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        TxtPrice.Text = ds2.Tables[0].Rows[0][0].ToString();
                    }
                    else
                        TxtPrice.Text = "0";
              
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

    protected void TxtPcsPerRoll_TextChanged(object sender, EventArgs e)
    {
        TxtTotalPcs.Text = "";
        TxtTotalPcs.Focus();
    }

    protected void TxtPackQty_TextChanged(object sender, EventArgs e)
    {
        TxtTotalPcs.Text = TxtPackQty.Text;
        try
        {

            if (ViewState["Roll"].ToString() == "0")
            {
                TxtPcsPerRoll.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, " select isnull(PCS,0) from packingcost WHERE FINISHEDID=" + ViewState["FinishedID"] + " AND PackingType=3").ToString();
            }

            TotalPcsChanged();
            if (TxtInvoiceNo.Text == "")
            {
                TxtInvoiceNo.Focus();
            }
            else
            {
                TxtPcsPerRoll.Focus();
            }
        }
        catch (Exception ex)
        {
            msg = "Please Check the packing cost against this item first!";
            MessageSave(msg);
        }
    }
}