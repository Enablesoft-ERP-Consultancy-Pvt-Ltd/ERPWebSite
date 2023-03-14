using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;


public partial class Masters_Order_DRAFTORDRNEW : System.Web.UI.Page
{
    int OrderDetailId = 0;
    int ItemFinishedId = 0;
    static int MasterCompanyId;
    static string PICode = "";
    string CustOrderNo;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        //PICode = dr["PiCode"].ToString();
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PICode from mastersetting");
            ViewState["PICode"] = ds.Tables[0].Rows[0]["PiCode"].ToString();
            HfCompanyId.Value = Session["varcompanyId"].ToString();
            ViewState["orderid"] = 0;
            ViewState["OrderDetailId"] = 0;
            ViewState["FinishedID"] = 0;
            ViewState["UpdateStatus"] = 0;
            ViewState["PISeries"] = 0;
            logo();
            SqlParameter[] _array = new SqlParameter[2];
            _array[0] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            _array[1] = new SqlParameter("@VarUserId ", Session["varuserId"]);
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillCombo", _array);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--SELECT--");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtexfactorydate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFillWithDS(ref DDItemCategory, ds, 2, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 3, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 4, false, "");
            DDCurrency.SelectedIndex = 1;
            TxtArea.Enabled = false;
            Session["FunctionType"] = 0;
            CHKFORCURRENTCONSUMPTION.Visible = false;
            TDTxtLocalOrderNo.Visible = true;
            TxtLocalOrderNo.Visible = true;
            trReferenceImage.Visible = false;
            tdUPCNO.Visible = false;
            tdOurCode.Visible = false;
            tdBuyerCode.Visible = false;
            lablechange();
            ChkGeneratePINo.Visible = false;
            TDPINo.Visible = false;
            switch (Session["varCompanyNo"].ToString())
            {
                case "1":
                    procode.Visible = false;
                    ItemDescription.Visible = true;
                    rdoUnitWise.Checked = true;
                    trless.Visible = false;
                    TRStockNo.Visible = false;
                    break;
                case "2":
                    procode.Visible = true;
                    ItemDescription.Visible = false;
                    rdoPcWise.Checked = true;
                    tdOurCode.Visible = true;
                    tdBuyerCode.Visible = true;
                    trless.Visible = true;
                    BtnShowConsumption.Visible = true;
                    tdDeliveryComments.Visible = false;
                    //tdtxtDeliveryComments.Visible = false;
                    lblourcode.Text = "BAR CODE";
                    TxtOurCode.Visible = false;
                    TxtCRBCode.Visible = true;
                    trtot.Visible = false;
                    TRPrice.Visible = true;
                    tdHTS.Visible = true;
                    TRStockNo.Visible = false;
                    break;
                case "3":
                    procode.Visible = false;
                    ItemDescription.Visible = false;
                    rdoPcWise.Checked = true;
                    tdUPCNO.Visible = true;
                    tdOurCode.Visible = true;
                    tdBuyerCode.Visible = true;
                    trReferenceImage.Visible = true;
                    trless.Visible = true;
                    TRStockNo.Visible = false;
                    break;
                case "6":
                    procode.Visible = true;
                    ItemDescription.Visible = false;
                    rdoPcWise.Checked = true;
                    tdOurCode.Visible = true;
                    tdBuyerCode.Visible = true;
                    trless.Visible = false;
                    TRStockNo.Visible = false;
                    break;
                case "8":
                    procode.Visible = true;
                    ItemDescription.Visible = false;
                    TRStockNo.Visible = false;
                    break;
                case "12":
                    tdBuyerCode.Visible = true;
                    rdoPcWise.Checked = true;
                    TRStockNo.Visible = false;
                    break;
                case "10":
                    DDPreviewType.Items.Clear();
                    tdBuyerCode.Visible = true;
                    lblprodcode.Text = "Item Code";
                    rdoPcWise.Checked = true;
                    DDPreviewType.Items.Add(new ListItem("PerForma Invoice", "0"));
                    TRStockNo.Visible = false;
                    break;
                case "33":
                    TRStockNo.Visible = true;
                    break;
                default:
                    procode.Visible = false;
                    ItemDescription.Visible = false;
                    TRStockNo.Visible = false;
                    break;
            }
        }
        //show edit button
        if (Session["canedit"].ToString() == "0") //non authenticated person
        {
            ChkEditOrder.Enabled = false;
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
    //************************************************************
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        string STR = "";
        if (ChkEditOrder.Checked == true)
        {
            ChkGeneratePINo.Visible = true;
            chkotherinformation.Checked = true;
            DivOtherInformation.Visible = true;
            ViewState["UpdateStatus"] = 1;
            TDDDCustomerOrderNo.Visible = true;
            DDCustOrderNo.Visible = true;
            TxtLocalOrderNo.Visible = false;
            TDTxtLocalOrderNo.Visible = false;
            
            STR = "SELECT Orderid,OrderNo FROM DRAFT_ORDER_MASTER Where Companyid=" + DDCompanyName.SelectedValue;

            if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
            {
                STR = STR + @" AND Customerid=" + DDCustomerCode.SelectedValue + "";
            }
            if (Session["VarcompanyNo"].ToString() != "2")
            {
                STR = STR + @" AND ORDERID NOT IN (SELECT IsNull(draftorderid,0) FROM ORDERMASTER)";
            }
            STR = STR + "order by OrderNo";
            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, STR, true, "--SELECT--");
            CHKFORCURRENTCONSUMPTION.Visible = true;
            TxtLocalOrderNo.Text = "";
            TxtOrderDate.Text = "";
            Txtcustorderdt.Text = "";
            txtduedate.Text = "";
            txtexfactorydate.Text = "";
        }
        else
        {
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtexfactorydate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CHKFORCURRENTCONSUMPTION.Visible = false;
            TxtLocalOrderNo.Text = "";
            DDCustOrderNo.Visible = false;
            TDDDCustomerOrderNo.Visible = false;
            TxtLocalOrderNo.Visible = true;
            TDTxtLocalOrderNo.Visible = true;
            ViewState["orderid"] = 0;
            chkotherinformation.Checked = false;
            DivOtherInformation.Visible = false;
            ChkGeneratePINo.Visible = false;
            ViewState["PISeries"] = 0;
            txtPINo.Text = "";
            TDPINo.Visible = false;
            ChkGeneratePINo.Checked = false;
            ChkGeneratePINo.Enabled = true;
        }
    }
    protected void DDCustOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["orderid"] = Convert.ToInt32(DDCustOrderNo.SelectedValue);
        getOrderDetail();
        Fill_Grid();
        TxtCustomerOrderNo.Focus();
        ViewState["UpdateStatus"] = 1;

    }
    private void getOrderDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = @"SELECT Distinct DM.Orderid,UnitId,OrderCalType,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,CustOrderNo,Replace(convert(varchar(11),Dm.duedate,106), ' ','-') as duedate,
                       Replace(convert(varchar(11),DeliveryDate,106), ' ','-') as DeliveryDate,replace(convert(varchar(11),ExfactoryDate,106), ' ','-') as ExfactoryDate,
                    OrderNo,TermId,PaymentId,ByAirSea,PortOfLoading,SeaPort,DeliveryComments ,Replace(convert(varchar(11),Custorderdate,106), ' ','-') as Custorderdate,
                    DM.ReportRef,DM.ReportKindAtten,DM.PINo,DM.PISeries,DM.ReverseChargesApplicable
                       From DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DD Where DM.OrderId=DD.OrderId And DM.OrderId=" + DDCustOrderNo.SelectedValue;
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["orderid"] = Convert.ToInt32(ds.Tables[0].Rows[0]["orderid"]);
            TxtLocalOrderNo.Text = ds.Tables[0].Rows[0]["OrderNo"].ToString();
            TxtCustomerOrderNo.Text = ds.Tables[0].Rows[0]["CustOrderNo"].ToString();
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "SELECT UnitId,UnitName from Unit order by UnitName", true, "--SELECT--");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDOrderUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
            }
            TxtOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            Txtcustorderdt.Text = ds.Tables[0].Rows[0]["Custorderdate"].ToString();
            txtduedate.Text = ds.Tables[0].Rows[0]["duedate"].ToString();
            TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();
            txtexfactorydate.Text = ds.Tables[0].Rows[0]["ExfactoryDate"].ToString();
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderCalType"]) == 0)
            {
                rdoUnitWise.Checked = true;
                rdoPcWise.Checked = false;
            }
            else
            {
                rdoPcWise.Checked = true;
                rdoUnitWise.Checked = false;
            }
            ddDeliveryTerms.SelectedValue = ds.Tables[0].Rows[0]["TermId"].ToString();
            ddPaymentMode.SelectedValue = ds.Tables[0].Rows[0]["PaymentId"].ToString();
            ddlByAirSea.SelectedValue = ds.Tables[0].Rows[0]["ByAirSea"].ToString();
            ddlPortOfLoading.SelectedValue = ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
            txtSeaPort.Text = ds.Tables[0].Rows[0]["SeaPort"].ToString();
            TxtDeliveryComments.Text = ds.Tables[0].Rows[0]["DeliveryComments"].ToString();
            txtReportRef.Text = ds.Tables[0].Rows[0]["ReportRef"].ToString();
            txtReportKindAtten.Text = ds.Tables[0].Rows[0]["ReportKindAtten"].ToString();
            ViewState["PISeries"] = Convert.ToInt32(ds.Tables[0].Rows[0]["PISeries"]);
            txtPINo.Text = ds.Tables[0].Rows[0]["PINo"].ToString();
            if (txtPINo.Text == "" || txtPINo.Text == "null")
            {
                ChkGeneratePINo.Enabled = true;
                ChkGeneratePINo.Checked = false;
            }
            else if (txtPINo.Text.Trim() != "")
            {
                TDPINo.Visible = true;
                ChkGeneratePINo.Enabled = false;
                ChkGeneratePINo.Checked = true;
            }
            ddlReverseChargesApplicable.SelectedValue = ds.Tables[0].Rows[0]["ReverseChargesApplicable"].ToString();
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CompanyName", true, "--SELECT--");
    }
    protected void TxtCustOrderNo_Validate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            CustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(CustomerOrderNo,0) asd from OrderMaster where CustomerOrderNo='" + TxtLocalOrderNo.Text + "'"));
            if (CustOrderNo != "")
            {

            }
            else
            {
                string Str = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(LocalOrder),'L 0') from OrderMaster").ToString();
                int n = Convert.ToInt32(Str.Split(' ')[1]);
                TxtLocalOrderNo.Text = "L " + (n + 1).ToString();
            }
            Tran.Commit();
        }
        catch (Exception)
        {
            LblErrorMessage.Visible = true;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //**************************TO Fill the OrderUnit*****************************************************************************
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string STR = "";
        #region by MK On 27-NOV-2012
        //UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term order by TermName", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddPaymentMode, "Select PaymentId,PaymentName from Payment order by PaymentName", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddlByAirSea, "select TransModeid,TransModeName from Transmode order by TransModename", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt order by StationName", true, "--Select--");
        #endregion
        STR = @"select TermId,TermName from Term Where MasterCompanyId=" + Session["varCompanyId"] + @" order by TermName 
             Select PaymentId,PaymentName from Payment Where MasterCompanyId=" + Session["varCompanyId"] + @" order by PaymentName
             select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varCompanyId"] + @" order by TransModename
             Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varCompanyId"] + " order by StationName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
        UtilityModule.ConditionalComboFillWithDS(ref ddDeliveryTerms, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddPaymentMode, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddlByAirSea, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddlPortOfLoading, ds, 3, true, "--SELECT--");
        TxtLocalOrderNo.Text = "";
        if (ChkEditOrder.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "select UnitId,UnitName from Unit order by UnitName", true, "--SELECT--");
            if (Session["varCompanyId"].ToString() == "2")
            {
                DDOrderUnit.SelectedIndex = 3;
            }
            else
            {
                DDOrderUnit.SelectedIndex = 2;
            }
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select TermId,PaymentId,ByAirSea,PortOfLoading,SeaPort from CustomerInfo Where Customerid=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ddDeliveryTerms.SelectedValue = Ds.Tables[0].Rows[0]["TermId"].ToString();
                ddPaymentMode.SelectedValue = Ds.Tables[0].Rows[0]["PaymentId"].ToString();
                ddlByAirSea.SelectedValue = Ds.Tables[0].Rows[0]["ByAirSea"].ToString();
                ddlPortOfLoading.SelectedValue = Ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
                txtSeaPort.Text = Ds.Tables[0].Rows[0]["SeaPort"].ToString();
            }
            if (Session["VarcompanyNo"].ToString() != "2")
            {
                // TxtLocalOrderNo.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT ISNULL(MAX(ORDERID),0)+1 FROM DRAFT_ORDER_MASTER").ToString();
                GetdraftOrderno();
            }
        }
        else if (ChkEditOrder.Checked == true)
        {
            STR = "SELECT Orderid,OrderNo FROM DRAFT_ORDER_MASTER Where Companyid=" + DDCompanyName.SelectedValue;

            if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
            {
                STR = STR + @" AND Customerid=" + DDCustomerCode.SelectedValue + "";
            }
            if (Session["VarcompanyNo"].ToString() != "2")
            {
                STR = STR + @" AND ORDERID NOT IN (SELECT IsNull(draftorderid,0) From OrderMaster)";
            }
            STR = STR + "order by OrderNo";
            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, STR, true, "--SELECT--");
            DDCustOrderNo.Focus();
        }
        DDCustomerCode.Focus();
    }
    //***********************************
    protected void DDItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        fillCombo();
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
        DDItemName.Focus();
    }
    //*********************************Function to Refresh the Order Detail******************************************
    private void refreshform()
    {
        LblErrorMessage.Visible = false;
        TxtQuantity.Text = "";
        TxtPrice.Text = "";
        TxtArea.Text = "";
        TxtTotalAmount.Text = "";
        TxtTotalQtyRequired.Text = "";
        TxtOrderArea.Text = "";
        TXTRemarks.Text = "";
        TxtUPCNO.Text = "";
        TxtWeight.Text = "";
        TxtOurCode.Text = "";
        TxtBuyerCode.Text = "";
        TxtPKGInstruction.Text = "";
        TxtLBGInstruction.Text = "";
        TxtDescription.Text = "";
        txtperformainvoiceno.Text = "";
        txtShipto.Text = "";
        txtLocationType.Text = "";
        txtMaterial.Text = "";
        txtTexture.Text = "";
        txtWidth.Text = "";
        txtLength.Text = "";
        // txtReportKindAtten.Text = "";
        txtMaterialFormDescription.Text = "";
        txtMaterialRate.Text = "";
        txtJobRate.Text = "";
        txtJobWork.Text = "";
        txtStockNo.Text = "";


    }

    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemSelectedChange();
    }
    private void ItemSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by Q.QualityName", true, "--SELECT--");

        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue;
        DDQuality.Focus();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str;
        double Area = 0;

        if (DDSize.SelectedIndex > 0)
        {
            Str = (DDSize.SelectedItem.Text);

            txtWidth.Text = (Str.Split('x')[0]);
            txtLength.Text = (Str.Split('x')[1]);

            Area = UtilityModule.DraftOrderCalculate_Area(Convert.ToDouble(txtLength.Text), Convert.ToDouble(txtWidth.Text), Convert.ToInt32(DDsizetype.SelectedValue), DDShape.SelectedItem.Text, Convert.ToInt32(Session["varCompanyId"].ToString()), 1);
            TxtArea.Text = Convert.ToString(Area);
        }
        // area();
        TxtQuantity.Focus();


    }
    protected void txtWidth_TextChanged(object sender, EventArgs e)
    {
        double Area = 0;
        Area = UtilityModule.DraftOrderCalculate_Area(Convert.ToDouble(txtLength.Text), Convert.ToDouble(txtWidth.Text), Convert.ToInt32(DDsizetype.SelectedValue), DDShape.SelectedItem.Text, Convert.ToInt32(Session["varCompanyId"].ToString()), 1);
        TxtArea.Text = Convert.ToString(Area);
        // area();
    }
    protected void txtLength_TextChanged(object sender, EventArgs e)
    {
        double Area = 0;
        Area = UtilityModule.DraftOrderCalculate_Area(Convert.ToDouble(txtLength.Text), Convert.ToDouble(txtWidth.Text), Convert.ToInt32(DDsizetype.SelectedValue), DDShape.SelectedItem.Text, Convert.ToInt32(Session["varCompanyId"].ToString()), 1);
        TxtArea.Text = Convert.ToString(Area);
        // area();
    }
    //***********************************************************************************************************************
    //***********************Function to calculate the total area Of the Ordered Item**************************************** 


    private void area()
    {
        if (DDSize.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 1)
            {
                TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaMtr from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
            }
            else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
            {
                TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaFt from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
            }
            else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
            {
                if (Session["VarcompanyId"].ToString() == "9")
                {
                    TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT Round(AreaInch/144,4) from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
                else
                {
                    TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaInch from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
            }
            else
            {
                TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaFt from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
            }
            con.Close();
            //else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
        }
        else
        {
            TxtArea.Text = null;
        }
    }
    //************************************************************************************************************************
    //***********************To Fill The Drop Down lists for Design, Color, Shape*********************************************
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue + "&Quality=" + DDQuality.SelectedValue;
        if (Session["varCompanyId"].ToString() == "2")
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TxtOurCode.Text, Convert.ToInt32(Session["varCompanyId"]));
            ViewState["FinishedID"] = ItemFinishedId;
            FillNet_GrossWeight(ItemFinishedId);
        }
        btnaddquality.Focus();
    }
    //*************************************************************************************************************************
    //**********************Save Button Funcationality**************************************************************************
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //Save_Image(15);
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int VARUPDATE_FLAG = 0;
                SqlParameter[] _arrpara = new SqlParameter[69];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@Companyid", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Customerid", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@OrderNo", SqlDbType.NVarChar);
                _arrpara[4] = new SqlParameter("@OrderCalType", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@UnitId", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                _arrpara[7] = new SqlParameter("@Status", SqlDbType.NVarChar);
                _arrpara[8] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@Qty", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrpara[12] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrpara[14] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@DisAmount", SqlDbType.Float);
                _arrpara[16] = new SqlParameter("@WhereHouseid", SqlDbType.Int);
                _arrpara[17] = new SqlParameter("@CancelQty", SqlDbType.Int);
                _arrpara[18] = new SqlParameter("@HoldQty", SqlDbType.Int);
                _arrpara[19] = new SqlParameter("@QualityCodeId", SqlDbType.Int);
                _arrpara[20] = new SqlParameter("@RetuenOrderid", SqlDbType.Int);
                _arrpara[21] = new SqlParameter("@ReturnOrderDetailId", SqlDbType.Int);
                _arrpara[22] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
                _arrpara[23] = new SqlParameter("@CustomerOrderNo", SqlDbType.NVarChar);
                _arrpara[24] = new SqlParameter("@DeliveryDate", SqlDbType.DateTime);
                _arrpara[25] = new SqlParameter("@TermId", SqlDbType.Int);
                _arrpara[26] = new SqlParameter("@PaymentId", SqlDbType.Int);
                _arrpara[27] = new SqlParameter("@ByAirSea", SqlDbType.Int);
                _arrpara[28] = new SqlParameter("@PortOfLoading", SqlDbType.Int);
                _arrpara[29] = new SqlParameter("@SeaPort", SqlDbType.NVarChar);
                _arrpara[30] = new SqlParameter("@Weight", SqlDbType.Float);
                _arrpara[31] = new SqlParameter("@UPCNO", SqlDbType.NVarChar);
                _arrpara[32] = new SqlParameter("@OurCode", SqlDbType.NVarChar);
                _arrpara[33] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar);
                _arrpara[34] = new SqlParameter("@PKGInstruction", SqlDbType.NVarChar);
                _arrpara[35] = new SqlParameter("@LBGInstruction", SqlDbType.NVarChar);
                _arrpara[36] = new SqlParameter("@DeliveryComments", SqlDbType.NVarChar);
                _arrpara[37] = new SqlParameter("@Description", SqlDbType.NVarChar);
                _arrpara[38] = new SqlParameter("@LessAdvance", SqlDbType.Float);
                _arrpara[39] = new SqlParameter("@LessCommission", SqlDbType.Float);
                _arrpara[40] = new SqlParameter("@LessDiscount", SqlDbType.Float);
                _arrpara[41] = new SqlParameter("@custorderdate", SqlDbType.DateTime);
                _arrpara[42] = new SqlParameter("@CRBCODE", SqlDbType.NVarChar);
                _arrpara[43] = new SqlParameter("@DueDate", SqlDbType.DateTime);
                _arrpara[44] = new SqlParameter("@HTSCODE", SqlDbType.NVarChar);
                _arrpara[45] = new SqlParameter("@ExfactoryDate", SqlDbType.SmallDateTime);
                _arrpara[46] = new SqlParameter("@PerformaInvoiceNo", SqlDbType.VarChar, 50);
                _arrpara[47] = new SqlParameter("@ShipTo", SqlDbType.VarChar, 200);
                _arrpara[48] = new SqlParameter("@flagSize", SqlDbType.TinyInt);
                _arrpara[49] = new SqlParameter("@GST", SqlDbType.Float);
                _arrpara[50] = new SqlParameter("@IGST", SqlDbType.Float);
                _arrpara[51] = new SqlParameter("@LocationType", SqlDbType.NVarChar, 100);
                _arrpara[52] = new SqlParameter("@Material", SqlDbType.NVarChar, 100);
                _arrpara[53] = new SqlParameter("@Texture", SqlDbType.NVarChar, 100);
                _arrpara[54] = new SqlParameter("@ReportRef", SqlDbType.NVarChar, 400);
                _arrpara[55] = new SqlParameter("@ReportKindAtten", SqlDbType.NVarChar, 400);
                _arrpara[56] = new SqlParameter("@UpdateStatus", SqlDbType.Int);
                _arrpara[57] = new SqlParameter("@SizeWidth", SqlDbType.VarChar, 20);
                _arrpara[58] = new SqlParameter("@SizeLength", SqlDbType.VarChar, 20);
                _arrpara[59] = new SqlParameter("@MaterialFormDescription", SqlDbType.VarChar, 250);
                _arrpara[60] = new SqlParameter("@MaterialRate", SqlDbType.Float);
                _arrpara[61] = new SqlParameter("@JobWork", SqlDbType.VarChar, 200);
                _arrpara[62] = new SqlParameter("@JobRate", SqlDbType.Float);
                _arrpara[63] = new SqlParameter("@MaterialJobWorkGST", SqlDbType.Float);
                _arrpara[64] = new SqlParameter("@MaterialJobWorkIGST", SqlDbType.Float);
                _arrpara[65] = new SqlParameter("@PISeries", SqlDbType.Int);
                _arrpara[66] = new SqlParameter("@PINo", SqlDbType.VarChar, 100);
                _arrpara[67] = new SqlParameter("@ReverseChargesApplicable", SqlDbType.VarChar, 5);
                _arrpara[68] = new SqlParameter("@PackingForwardingCharges", SqlDbType.Float);




                ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TxtOurCode.Text, Convert.ToInt32(Session["varCompanyId"]));

                int VarOrderDetailId = 0;
                VarOrderDetailId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT IsNull(ORDERDETAILID,0) from DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + " And Item_Finished_id=" + ItemFinishedId + " And OrderDetailId!=" + ViewState["OrderDetailId"] + " And Rate=" + TxtPrice.Text + " And OurCode='" + TxtOurCode.Text + "'"));
                if (VarOrderDetailId == 0)
                {
                    _arrpara[0].Value = (ViewState["orderid"] == null ? 0 : ViewState["orderid"]);
                    _arrpara[1].Value = DDCompanyName.SelectedValue;
                    _arrpara[2].Value = DDCustomerCode.SelectedValue;
                    _arrpara[3].Value = TxtLocalOrderNo.Text.ToUpper();
                    _arrpara[4].Value = (rdoPcWise.Checked == true ? 1 : 0);
                    _arrpara[5].Value = DDOrderUnit.SelectedValue;
                    _arrpara[6].Value = TxtOrderDate.Text;
                    _arrpara[7].Value = "";
                    _arrpara[8].Value = ViewState["OrderDetailId"];
                    _arrpara[9].Value = ItemFinishedId;
                    _arrpara[10].Value = TxtQuantity.Text;
                    _arrpara[11].Value = TxtPrice.Text;
                    _arrpara[12].Value = (DDSize.Visible == true ? TxtArea.Text : "0");
                    if (rdoUnitWise.Checked == true)
                    {
                        _arrpara[13].Value = Convert.ToDouble(_arrpara[10].Value) * Convert.ToDouble(_arrpara[11].Value) * (Convert.ToDouble(_arrpara[12].Value));
                    }
                    else
                    {
                        _arrpara[13].Value = Convert.ToDouble(_arrpara[10].Value) * Convert.ToDouble(_arrpara[11].Value);
                    }
                    _arrpara[14].Value = DDCurrency.SelectedValue;
                    _arrpara[15].Value = 0;
                    _arrpara[16].Value = 0;
                    _arrpara[17].Value = 0;
                    _arrpara[18].Value = 0;
                    _arrpara[19].Value = Convert.ToInt32(DDItemCode.Visible == true ? Convert.ToInt32(DDItemCode.SelectedValue) : 0);
                    _arrpara[20].Direction = ParameterDirection.Output;
                    _arrpara[21].Direction = ParameterDirection.Output;
                    _arrpara[22].Value = TXTRemarks.Text;
                    _arrpara[23].Value = TxtCustomerOrderNo.Text.ToUpper();
                    _arrpara[24].Value = TxtDeliveryDate.Text;
                    _arrpara[25].Value = ddDeliveryTerms.SelectedValue;
                    _arrpara[26].Value = ddPaymentMode.SelectedValue;
                    _arrpara[27].Value = ddlByAirSea.SelectedValue;
                    _arrpara[28].Value = ddlPortOfLoading.SelectedValue;
                    _arrpara[29].Value = txtSeaPort.Text;
                    _arrpara[30].Value = TxtWeight.Text == "" ? "0" : TxtWeight.Text;
                    _arrpara[31].Value = TxtUPCNO.Text;
                    _arrpara[32].Value = TxtOurCode.Text;
                    _arrpara[33].Value = TxtBuyerCode.Text;
                    _arrpara[34].Value = TxtPKGInstruction.Text;
                    _arrpara[35].Value = TxtLBGInstruction.Text;
                    _arrpara[36].Value = TxtDeliveryComments.Text;
                    _arrpara[37].Value = TxtDescription.Text;
                    _arrpara[38].Value = txtlessadv.Text != "" ? Convert.ToDouble(txtlessadv.Text) : 0;
                    _arrpara[39].Value = txtcomm.Text != "" ? Convert.ToDouble(txtcomm.Text) : 0;
                    _arrpara[40].Value = Txtdiscount.Text != "" ? Convert.ToDouble(Txtdiscount.Text) : 0;
                    _arrpara[41].Value = Txtcustorderdt.Text;
                    _arrpara[42].Value = TxtCRBCode.Text;
                    _arrpara[43].Value = txtduedate.Text;
                    _arrpara[44].Value = TxtHtsCode.Text;
                    _arrpara[45].Value = txtexfactorydate.Text;
                    _arrpara[46].Value = txtperformainvoiceno.Text;
                    _arrpara[47].Value = txtShipto.Text;
                    _arrpara[48].Value = DDsizetype.SelectedValue;
                    _arrpara[49].Value = TxtGST.Text == "" ? "0" : TxtGST.Text;
                    _arrpara[50].Value = TxtIGST.Text == "" ? "0" : TxtIGST.Text;
                    _arrpara[51].Value = txtLocationType.Text;
                    _arrpara[52].Value = txtMaterial.Text;
                    _arrpara[53].Value = txtTexture.Text;
                    _arrpara[54].Value = txtReportRef.Text;
                    _arrpara[55].Value = txtReportKindAtten.Text;
                    _arrpara[56].Value = ViewState["UpdateStatus"];
                    _arrpara[57].Value = txtWidth.Text == "" ? "0" : txtWidth.Text;
                    _arrpara[58].Value = txtLength.Text == "" ? "0" : txtLength.Text;
                    _arrpara[59].Value = txtMaterialFormDescription.Text;
                    _arrpara[60].Value = txtMaterialRate.Text == "" ? "0" : txtMaterialRate.Text;
                    _arrpara[61].Value = txtJobWork.Text;
                    _arrpara[62].Value = txtJobRate.Text == "" ? "0" : txtJobRate.Text;
                    _arrpara[63].Value = txtMaterialJobWorkGST.Text == "" ? "0" : txtMaterialJobWorkGST.Text;
                    _arrpara[64].Value = txtMaterialJobWorkIGST.Text == "" ? "0" : txtMaterialJobWorkIGST.Text;
                    _arrpara[65].Value = ViewState["PISeries"];
                    _arrpara[66].Value = txtPINo.Text;
                    _arrpara[67].Value = ddlReverseChargesApplicable.SelectedValue;
                    _arrpara[68].Value = txtPackingForwardingCharges.Text == "" ? "0" : txtPackingForwardingCharges.Text;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DRAFT_ORDER_ENTER", _arrpara);

                    ViewState["orderid"] = _arrpara[20].Value;
                    VARUPDATE_FLAG = Convert.ToInt32(ViewState["OrderDetailId"]) == 0 ? 0 : 1;
                    ViewState["OrderDetailId"] = "0";
                    UtilityModule.DRAFT_ORDER_CONSUMPTION_DEFINE(ItemFinishedId, Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[21].Value), VARUPDATE_FLAG, CHKFORCURRENTCONSUMPTION.Checked == true ? 1 : 0, Tran);

                    Tran.Commit();
                    Save_Image(Convert.ToInt32(_arrpara[21].Value));

                    LblErrorMessage.Text = "DATA SAVED SUCCESSFULLY..";
                    refreshform();
                    Fill_Grid();
                    newPreview1.ImageUrl = "";

                    if (txtPINo.Text != "")
                    {
                        ChkGeneratePINo.Enabled = false;
                        ChkGeneratePINo.Checked = true;
                    }

                    if (Session["VarcompanyNo"].ToString() == "33")
                    {
                        string STR = "";
                        //if (ChkEditOrder.Checked == true)
                        // {
                        if (BtnSave.Text == "Save")
                        {
                            ViewState["UpdateStatus"] = 2;
                        }

                        DDCustOrderNo.Visible = true;
                        if (DDCompanyName.SelectedIndex > 0)
                        {
                            STR = "SELECT Orderid,OrderNo FROM DRAFT_ORDER_MASTER Where Companyid=" + DDCompanyName.SelectedValue + "";
                        }
                        if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
                        {
                            STR = STR + @" AND Customerid=" + DDCustomerCode.SelectedValue + "";
                        }
                        if (Session["VarcompanyNo"].ToString() != "2")
                        {
                            STR = STR + @" AND ORDERID NOT IN (SELECT IsNull(draftorderid,0) FROM ORDERMASTER)";
                        }
                        STR = STR + "order by OrderNo";
                        UtilityModule.ConditionalComboFill(ref DDCustOrderNo, STR, true, "--SELECT--");
                        if (DDCompanyName.SelectedIndex > 0)
                        {
                            DDCustOrderNo.SelectedValue = ViewState["orderid"].ToString();
                            TxtLocalOrderNo.Text = DDCustOrderNo.SelectedItem.Text;
                        }

                        //}
                    }

                }
                else
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = " DUPLICATE DATA EXISTS ..";
                    Tran.Commit();
                    LblErrorMessage.Focus();
                }
                BtnSave.Text = "Save";
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
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath, double HScaleFactor = 0.3)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * HScaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    public void Save_Image(int OrderDetailId)
    {
        SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        myConnection.Open();
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string targetPath = Server.MapPath("../../ImageDraftorder/d" + OrderDetailId + "draftdetail.gif");
            string img = "~\\ImageDraftorder\\d" + OrderDetailId + "draftdetail.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;

            FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
            if (TheFile.Exists)
            {
                File.Delete(MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
            }
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile, 0.6);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update DRAFT_ORDER_DETAIL Set Photo='" + img + "' Where OrderDetailId=" + OrderDetailId + "");
        }
        else
        {
            if (Session["ProdCodeValidation"].ToString() == "1" && TxtProdCode.Text != "")
            {
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Top(1)mi.Photo From Item_Parameter_Master IM,Main_Item_Image  MI,DRAFT_ORDER_DETAIL od
                             where Productcode='" + TxtProdCode.Text + "' And IM.Item_Finished_id=MI.Finishedid And IM.MasterCompanyId=" + Session["varCompanyId"] + " and od.Item_Finished_Id=im.Item_Finished_id and ORDERDETAILID=" + OrderDetailId + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string targetPath = Server.MapPath("../../ImageDraftorder/d" + OrderDetailId + "draftdetail.gif");
                    string img = "~\\ImageDraftorder\\d" + OrderDetailId + "draftdetail.gif";

                    // FileInfo TheFile = new FileInfo(Server.MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif");
                    FileInfo TheFile = new FileInfo(Server.MapPath(ds.Tables[0].Rows[0]["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        //  File.Copy(MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif", MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
                        File.Copy(MapPath(ds.Tables[0].Rows[0]["photo"].ToString()), MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
                    }
                    //else
                    //{
                    //    Stream strm = PhotoImage.PostedFile.InputStream;
                    //    var targetFile = targetPath;
                    //    GenerateThumbnails(0.3, strm, targetFile);
                    //}
                    SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE DRAFT_ORDER_DETAIL SET PHOTO='" + img + "' where  ORDERDETAILID=" + OrderDetailId + " ");
                }
            }
            else
            {
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select MII.PHOTO from MAIN_ITEM_IMAGE mii,DRAFT_ORDER_DETAIL d where d.ITEM_FINISHED_ID=MII.FINISHEDID AND d.ORDERDETAILID=" + OrderDetailId + " And MII.MasterCompanyId=" + Session["varCompanyId"] + "");
                //SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "UPDATE DRAFT_ORDER_DETAIL SET PHOTO=MII.PHOTO FROM MAIN_ITEM_IMAGE MII WHERE DRAFT_ORDER_DETAIL.ITEM_FINISHED_ID=MII.FINISHEDID AND ORDERDETAILID=" + OrderDetailId + " And MII.MasterCompanyId=" + Session["varCompanyId"] + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string targetPath = Server.MapPath("../../ImageDraftorder/d" + OrderDetailId + "draftdetail.gif");
                    string img = "~\\ImageDraftorder\\d" + OrderDetailId + "draftdetail.gif";
                    FileInfo TheFile = new FileInfo(Server.MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif");
                    if (TheFile.Exists)
                    {
                        File.Delete(MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
                        File.Copy(MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif", MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftdetail.gif");
                    }
                    SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE DRAFT_ORDER_DETAIL SET PHOTO='" + img + "' where  ORDERDETAILID=" + OrderDetailId + " ");
                }

            }
        }
        if (FileReferenceImage.FileName != "")
        {
            string filename = Path.GetFileName(FileReferenceImage.PostedFile.FileName);
            string targetPath = Server.MapPath("../../ImageDraftorder/d" + OrderDetailId + "draftref.gif");
            string img = "~\\ImageDraftorder\\d" + OrderDetailId + "draftref.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = FileReferenceImage.PostedFile.InputStream;
            var targetFile = targetPath;
            FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftref.gif");
            if (TheFile.Exists)
            {
                File.Delete(MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftref.gif");
            }
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            int IDNEW = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Max(ID),0)+1 From Draft_Order_ReferenceImage"));
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into Draft_Order_ReferenceImage(OrderDetailId,Photo,ID) values(" + OrderDetailId + ",'" + img + "'," + IDNEW + ")");
            //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FileReferenceImage.PostedFile.InputStream);
            //storeimage.ExecuteNonQuery();
        }
        myConnection.Close();
        myConnection.Dispose();
    }
    protected void BtnReferenceImageSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["OrderDetailId"]) != 0)
        {
            SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            myConnection.Open();
            if (FileReferenceImage.FileName != "")
            {
                string Qry = "";
                string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
                string targetPath = Server.MapPath("../../ImageDraftorder/d" + OrderDetailId + "draftref.gif");
                string img = "~\\ImageDraftorder\\d" + OrderDetailId + "draftref.gif";
                //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
                Stream strm = PhotoImage.PostedFile.InputStream;
                var targetFile = targetPath;
                FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftref.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~\\ImageDraftorder\\d") + OrderDetailId + "draftref.gif");
                }
                if (PhotoImage.FileName != null && PhotoImage.FileName != "")
                {
                    GenerateThumbnails(0.3, strm, targetFile);
                }
                Qry = @"select Distinct orderdetailid from Draft_Order_ReferenceImage  where orderdetailid= " + ViewState["OrderDetailId"];
                int Order_DetailID = Convert.ToInt32(SqlHelper.ExecuteScalar(myConnection, CommandType.Text, Qry));
                if (Order_DetailID > 0)
                {
                    //GenerateThumbnails(0.3, strm, targetFile);
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE Draft_Order_ReferenceImage SET Photo ='" + img + "'  WHERE orderdetailid= " + ViewState["OrderDetailId"]);
                }
                else
                {
                    //GenerateThumbnails(0.3, strm, targetFile);
                    int IDNEW = Convert.ToInt32(SqlHelper.ExecuteScalar(myConnection, CommandType.Text, "Select IsNull(Max(ID),0)+1 From Draft_Order_ReferenceImage"));
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into Draft_Order_ReferenceImage(OrderDetailId,Photo,ID) values(" + ViewState["OrderDetailId"] + ",'" + targetPath + "'," + IDNEW + ")");
                }
                //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
                //System.Drawing.Image img = System.Drawing.Image.FromStream(FileReferenceImage.PostedFile.InputStream);
                //storeimage.ExecuteNonQuery();
            }
            myConnection.Close();
            myConnection.Dispose();
        }
        else
        {
            LblErrorMessage.Focus();
            LblErrorMessage.Text = "Pls Select Any Row Of Data Grid";
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDOrderUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLocalOrderNo) == false)
        {
            goto a;

        }
        //if (UtilityModule.VALIDTEXTBOX(TxtCustomerOrderNo) == false)
        //{
        //    goto a;

        //}
        if (UtilityModule.VALIDTEXTBOX(TxtOrderDate) == false)
        {
            goto a;

        }
        if (UtilityModule.VALIDTEXTBOX(TxtDeliveryDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddDeliveryTerms) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddPaymentMode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlByAirSea) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlPortOfLoading) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(txtSeaPort) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemCategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;

        }
        if (DDItemCode.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDItemCode) == false)
            {
                goto a;
            }
        }
        if (DDQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
        }
        if (DDItemCode.Visible == true)
        {

            if (UtilityModule.VALIDDROPDOWNLIST(DDItemCode) == false)
            {
                goto a;
            }
        }
        if (DDDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
            {
                goto a;
            }
        }
        if (DDColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
            {
                goto a;
            }
        }
        if (DDShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDShape) == false)
            {
                goto a;
            }
        }
        if (DDSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
            {
                goto a;
            }
        }
        if (ddshadecolor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshadecolor) == false)
            {
                goto a;
            }
        }
        if (TxtArea.Visible == true)
        {

            if (UtilityModule.VALIDTEXTBOX(TxtArea) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtQuantity) == false)
        {
            goto a;
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
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    //**************************************************************************************************************************
    //*******************************Function To Fill the Data In to Data Gride*************************************************
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
        //Report_Type();
    }
    //***************************************************************************************************************************
    //********************************Function To Get Data to fill Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"SELECT DD.OrderDetailId as Sr_No,Case when VF.MasterCompanyid=10 then VF.ProductCode else VF.CATEGORY_NAME End As CATEGORY ,VF.ITEM_NAME  ItemName,IsNull(VF.QUALITYNAME,'')+SPACE(2)+IsNull(VF.DESIGNNAME,'')+SPACE(2)+IsNull(VF.COLORNAME,'')+SPACE(2)+IsNull(SHAPENAME,'')+SPACE(2)+
                              case When DD.Flagsize=0 then vf.SizeFt+' '+sz.Type when DD.Flagsize=1 Then vf.SizeMtr+' '+sz.Type Else vf.SizeInch+' '+sz.type End DESCRIPTION,DD.QTY,DD.RATE,DD.QTY*DD.AREA AREA,DD.AMOUNT,IsNull(QM.SUBQUANTITY,'') SUBQUALITY,PHOTO,'' BTNCONSUMPTION,'' BTNEXPENCE,'' BTNPACKING,dbo.[GET_DreftORDER_CONSUMPTION_DEFINE_OR_NOT](DD.OrderDetailId,DD.ITEM_FINISHED_ID) as Consumpflag
                              FROM DRAFT_ORDER_MASTER DM,V_FINISHEDITEMDETAIL VF,DRAFT_ORDER_DETAIL DD LEFT OUTER JOIN QUALITYCODEMASTER QM ON DD.QUALITYCODEID=QM.QUALITYCODEID
                              inner join sizetype Sz on Sz.val=DD.Flagsize
                              WHERE DM.ORDERID=DD.ORDERID AND DD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID AND DM.ORDERID=" + ViewState["orderid"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By DD.OrderDetailId";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;
            int TotalQtyRequired = 0;
            double TotalAmount = 0, TotalArea = 0;
            for (int i = 0; i < n; i++)
            {
                TotalQtyRequired = TotalQtyRequired + Convert.ToInt32(ds.Tables[0].Rows[i]["QTY"]);
                TotalAmount = TotalAmount + Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"]);
                TotalArea = TotalArea + Convert.ToDouble(ds.Tables[0].Rows[i]["AREA"]);
            }
            TxtOrderArea.Text = TotalArea.ToString();
            TxtTotalAmount.Text = TotalAmount.ToString();
            TxtTotalQtyRequired.Text = TotalQtyRequired.ToString();
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

    //**************************************************************************************************************************
    //************************************To Fill Shape In dropDown**************************************************************
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Size();
        DDSize.Focus();
    }
    private void Fill_Size()
    {
        string Sizename = "", str = "";
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                Sizename = "SIZEFT";
                break;
            case "1":
                Sizename = "SizeMtr";
                break;
            case "2":
                Sizename = "Sizeinch";
                break;
            default:
                Sizename = "SIZEFT";
                break;
        }
        str = "SELECT SizeId," + Sizename + " Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by " + Sizename + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");

    }
    //****************************************************************************************************************************
    //******************************** Function To fill the detail of the Order Back in the Form*********************************************
    private void fillOrderBack()
    {

        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,Od.UnitRate,round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,Od.DyeingInstructions,Od.DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,IPM.SHAPE_ID,IPM.SIZE_ID from OrderDetail Od,ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM where IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID and Od.OrderDetailId=" + DGOrderDetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

            OrderDetailId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrderDetailId"]);
            ViewState["OrderDetailId"] = OrderDetailId;

            DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Q.MasterCompanyId=" + Session["varCompanyId"] + " order by Q.QualityName", true, "--SELECT--");
            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDItemCode, "select  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
            DDItemCode.SelectedValue = ds.Tables[0].Rows[0]["QualityCodeId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDesign, "select V.DesignId,Case when DesignNameAToC is null then DesignName Else DesignNameAToC End DesignName from Design V left outer join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  C.ColorId,Case When ColorNameToC is null then ColorName Else ColorNameToC End ColorName from Color C left outer join CustomerColor CC on CC.ColorId=C.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename ", true, "--SELECT--");
            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
            DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
            DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            Fill_Size();
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
            TxtArea.Text = ds.Tables[0].Rows[0]["TotalArea"].ToString();
            TxtQuantity.Text = ds.Tables[0].Rows[0]["QtyRequired"].ToString();
            TxtPrice.Text = ds.Tables[0].Rows[0]["UnitRate"].ToString();
            if (Convert.ToInt32(DDQuality.SelectedValue) > 0)
            {
                DivQuality.Visible = true;
            }
            else
            {
                DivQuality.Visible = false;
            }
            if (Convert.ToInt32(DDDesign.SelectedValue) > 0)
            {
                trDesign.Visible = true;
            }
            else
            {
                trDesign.Visible = false;
            }
            if (Convert.ToInt32(DDColor.SelectedValue) > 0)
            {
                trColor.Visible = true;
            }
            else
            {
                trColor.Visible = false;
            }
            if (Convert.ToInt32(DDShape.SelectedValue) > 0)
            {
                trShape.Visible = true;
            }
            else
            {
                trShape.Visible = false;
            }


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

    }
    protected void TxtCustOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtCustOrderNo_Validate();
    }
    private void ddlcategorycange()
    {
        DivQuality.Visible = false;
        trDesign.Visible = false;
        trColor.Visible = false;
        trShape.Visible = false;
        trSize.Visible = false;
        TrtxtArea.Visible = false;
        trShadeColor.Visible = false;
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
                        DivQuality.Visible = true;
                        break;
                    case "2":
                        trDesign.Visible = true;
                        break;
                    case "3":
                        trColor.Visible = true;
                        break;
                    case "4":
                        trShape.Visible = true;
                        break;
                    case "5":
                        trSize.Visible = true;
                        TrtxtArea.Visible = true;
                        break;
                    case "6":
                        trShadeColor.Visible = true;
                        break;
                }
            }
        }
    }
    /******************************************************To Fill Combo***********************************************************/
    private void fillCombo()
    {
        #region On 27-Nov-2012
        //UtilityModule.ConditionalComboFill(ref DDDesign, "select * from v_Design_attach order by designname", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color order by ColorName", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref DDShape, "SELECT * from v_Shape_attach order by shapename", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT * from ShadeColor order by ShadeColorname", true, "--SELECT--");
        #endregion
        string str = "select V.DesignId,Case when DesignNameAToC is null then DesignName Else DesignNameAToC End DesignName from Design V left outer join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + @" order by designname
                     SELECT  C.ColorId,Case When ColorNameToC is null then ColorName Else ColorNameToC End ColorName from Color C left outer join CustomerColor CC on CC.ColorId=C.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + "  Where C.MasterCompanyId=" + Session["varCompanyId"] + @" order by ColorName
                     SELECT ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + @" order by shapename
                     SELECT ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddshadecolor, ds, 3, true, "--SELECT--");
    }
    protected void fillitemcode_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--Select--");
    }
    protected void BtnRefreshItem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
    }
    private DataSet fill_gird_chk()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ITEM_MASTER.ITEM_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid, dbo.Lablecustomer.CustomerId AS customerid, 
                            dbo.ITEM_MASTER.ITEM_NAME FROM dbo.Lablecustomer INNER JOIN dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid = dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                            dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                            where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmCustomer|fill_gird_chk|" + ex.Message);
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
    private DataSet fill_gird_chk2()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT dbo.ITEM_MASTER.ITEM_ID, dbo.LabelOrder.Orderid, dbo.ITEM_MASTER.ITEM_NAME, dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, 
                            dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME FROM dbo.ITEM_CATEGORY_MASTER INNER JOIN dbo.ITEM_MASTER ON dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID=dbo.ITEM_MASTER.CATEGORY_ID INNER JOIN
                            dbo.LabelOrder ON dbo.ITEM_MASTER.ITEM_ID = dbo.LabelOrder.ItemId
                            where orderid=" + DDCustOrderNo.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            //  ds.Tables[0].Columns["customerid"].ColumnName = "SerialNo.";
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmCustomer|fill_gird_chk|" + ex.Message);
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
    private DataSet fill_gird_chk_order()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            string strsql = @"SELECT dbo.ITEM_MASTER.ITEM_ID,dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid,dbo.Lablecustomer.CustomerId AS customerid, 
                            dbo.ITEM_MASTER.ITEM_NAME FROM dbo.Lablecustomer INNER JOIN dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid=dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                            dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                            where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmCustomer|fill_gird_chk|" + ex.Message);
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        #region on 27-Nov-2012
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //try
        //{
        //    con.Open();
        //    int catgid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select max(CATEGORY_ID) from ITEM_CATEGORY_MASTER"));
        //    int srno = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, " select  ISnull (max(Sr_No),0)+1 from CategorySeparate"));

        //    string str2 = "insert into CategorySeparate values(" + srno + ",0," + catgid + "," + Session["varuserid"] + "," + Session["varCompanyId"] + ")";
        //    SqlHelper.ExecuteNonQuery(con, CommandType.Text, str2);
        //}

        //catch (Exception ex)
        //{
        //    Logs.WriteErrorLog("error");
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
        #endregion
        UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME", true, "--SELECT--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by Q.QualityName", true, "--SELECT--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, "select V.DesignId,Case when DesignNameAToC is null then DesignName Else DesignNameAToC End DesignName from Design V left outer join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  C.ColorId,Case When ColorNameToC is null then ColorName Else ColorNameToC End ColorName from Color C left outer join CustomerColor CC on CC.ColorId=C.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorname", true, "--SELECT--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename", true, "--SELECT--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        Fill_Size();
    }
    protected void DDOrderUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDShape.SelectedIndex > 0)
        {
            Fill_Size();

        }
        TxtOrderDate.Focus();
    }
    protected void TxtLocalOrderNo_TextChanged(object sender, EventArgs e)
    {
        int VarOrderId = 0;
        LblErrorMessage.Text = "";
        VarOrderId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT Orderid from DRAFT_ORDER_MASTER WHERE ORDERNO='" + TxtLocalOrderNo.Text + "'"));
        if (VarOrderId != 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = " DUPLICATE ORDER NO EXITS ..";
            TxtLocalOrderNo.Text = "";
            TxtLocalOrderNo.Focus();
        }
        else
        {
            TxtCustomerOrderNo.Focus();
        }
    }

    protected void fill_grid1()
    {
        DGConsumption.DataSource = getdetail();
        DGConsumption.DataBind();
        for (int i = 0; i < DGConsumption.Rows.Count; i++)
        {
            ((TextBox)DGConsumption.Rows[i].FindControl("txtp_tage")).Text = DGConsumption.Rows[i].Cells[2].Text;
        }
    }

    protected DataSet getdetail()
    {
        DataSet ds = null;
        string Str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();

            Str = @"Select PCMDID SrNo,VF.Category_Name,VF.Item_Name,VF.QualityName+Space(2)+VF.DesignName+Space(2)+VF.ColorName+Space(2)+VF.ShadeColorName+Space(2)+
                   VF.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF.SizeMtr Else VF.SizeFt End Description,U.UnitName,
                   IQTY,ILOSS,IRATE,VF1.Category_Name,VF1.Item_Name,VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShadeColorName+Space(2)+
                   VF1.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF1.SizeMtr Else VF1.SizeFt End Description,U1.UnitName,OQTY,ORATE,DD.Orderid,DD.OrderDetailId
                   FROM DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DOD,DRAFT_ORDER_CONSUMPTION_DETAIL DD,V_FinishedItemDetail VF,V_FinishedItemDetail VF1,Unit U,Unit U1
                   Where DM.Orderid=DD.Orderid And DM.ORDERID=DOD.ORDERID AND DOD.OrderDetailID=DD.OrderDetailID AND DD.IFinishedid=VF.Item_Finished_id And DD.OFinishedid=VF1.Item_Finished_id And U.UnitId=DD.IUnitId And U1.UnitId=DD.OUnitId
                   And DD.OrderDetailId=" + DGOrderDetail.DataKeys[0].Value + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        }
        catch (Exception)
        {
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return ds;
    }
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DGOrderDetail.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string Str = @"Select IM.Category_Id,IM.Item_Id,IPM.Quality_Id,IPM.Design_Id,IPM.Color_Id,IPM.Shape_Id,IPM.Size_id,IPM.Shadecolor_id ,OD.*
                           from Draft_Order_Detail OD,Item_Parameter_Master IPM,Item_Master IM Where OD.Item_Finished_id=IPM.Item_Finished_id And 
                           IM.Item_Id=IPM.Item_Id And OD.OrderDetailid=" + DGOrderDetail.SelectedDataKey.Value + " And IM.MasterCompanyId=" + Session["varCompanyId"] + @"
                           select top(1) Photo from Draft_Order_ReferenceImage where OrderDetailId=" + DGOrderDetail.SelectedDataKey.Value + "";
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDItemCategory.SelectedValue = Ds.Tables[0].Rows[0]["Category_Id"].ToString();
                ddlcategorycange();
                fillCombo();
                UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
                DDItemName.SelectedValue = Ds.Tables[0].Rows[0]["Item_Id"].ToString();
                UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by Q.QualityName", true, "--SELECT--");
                DDQuality.SelectedValue = Ds.Tables[0].Rows[0]["Quality_Id"].ToString();
                TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue;
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["QualityCodeId"]) > 0)
                {
                    UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
                    DDItemCode.SelectedValue = Ds.Tables[0].Rows[0]["QualityCodeId"].ToString();
                    TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue + "&Quality=" + DDQuality.SelectedValue;
                }
                if (trDesign.Visible == true)
                {
                    DDDesign.SelectedValue = Ds.Tables[0].Rows[0]["Design_Id"].ToString();
                }
                if (trColor.Visible == true)
                {
                    DDColor.SelectedValue = Ds.Tables[0].Rows[0]["Color_Id"].ToString();
                }
                if (trShadeColor.Visible == true)
                {
                    ddshadecolor.SelectedValue = Ds.Tables[0].Rows[0]["Shadecolor_id"].ToString();
                }
                if (trShape.Visible == true)
                {
                    DDShape.SelectedValue = Ds.Tables[0].Rows[0]["Shape_Id"].ToString();

                    DDsizetype.SelectedValue = Ds.Tables[0].Rows[0]["Flagsize"].ToString();
                    Fill_Size();
                }
                if (trSize.Visible == true)
                {
                    DDSize.SelectedValue = Ds.Tables[0].Rows[0]["Size_id"].ToString();
                    TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
                }
                TxtQuantity.Text = Ds.Tables[0].Rows[0]["Qty"].ToString();
                TxtPrice.Text = Ds.Tables[0].Rows[0]["Rate"].ToString();
                TXTRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
                ViewState["OrderDetailId"] = DGOrderDetail.SelectedDataKey.Value;
                BtnReferenceImageSave.Visible = true;
                btnContainerpackingMatCost.Visible = true;
                TxtOrderDetailId.Text = DGOrderDetail.SelectedDataKey.Value.ToString();
                TxtWeight.Text = Ds.Tables[0].Rows[0]["Weight"].ToString();
                newPreview1.ImageUrl = Ds.Tables[0].Rows[0]["photo"].ToString();
                if (Ds.Tables[1].Rows.Count > 0)
                {
                    ImageReferenceImage.ImageUrl = Ds.Tables[1].Rows[0][0].ToString();
                }
                TxtOurCode.Text = Ds.Tables[0].Rows[0]["OurCode"].ToString();
                TxtBuyerCode.Text = Ds.Tables[0].Rows[0]["BuyerCode"].ToString();
                TxtPKGInstruction.Text = Ds.Tables[0].Rows[0]["PKGInstruction"].ToString();
                TxtLBGInstruction.Text = Ds.Tables[0].Rows[0]["LBGInstruction"].ToString();
                TxtDescription.Text = Ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["currencyid"].ToString();
                TxtCRBCode.Text = Ds.Tables[0].Rows[0]["CRBCODE"].ToString();
                TxtHtsCode.Text = Ds.Tables[0].Rows[0]["HTSCODE"].ToString();
                txtperformainvoiceno.Text = Ds.Tables[0].Rows[0]["PerformaInvoiceNo"].ToString();
                txtShipto.Text = Ds.Tables[0].Rows[0]["ShipTo"].ToString();
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["OrderCalType"]) == 0)
                {
                    rdoUnitWise.Checked = true;
                    rdoPcWise.Checked = false;
                }
                else
                {
                    rdoPcWise.Checked = true;
                    rdoUnitWise.Checked = false;
                }

                TxtGST.Text = Ds.Tables[0].Rows[0]["GST"].ToString();
                TxtIGST.Text = Ds.Tables[0].Rows[0]["IGST"].ToString();
                txtLocationType.Text = Ds.Tables[0].Rows[0]["LocationType"].ToString();
                txtMaterial.Text = Ds.Tables[0].Rows[0]["Material"].ToString();
                txtTexture.Text = Ds.Tables[0].Rows[0]["Texture"].ToString();
                ViewState["UpdateStatus"] = 1;
                txtWidth.Text = Ds.Tables[0].Rows[0]["SizeWidth"].ToString();
                txtLength.Text = Ds.Tables[0].Rows[0]["SizeLength"].ToString();
                txtMaterialFormDescription.Text = Ds.Tables[0].Rows[0]["MaterialFormDescription"].ToString();
                txtMaterialRate.Text = Ds.Tables[0].Rows[0]["MaterialRate"].ToString();
                txtJobWork.Text = Ds.Tables[0].Rows[0]["JobWork"].ToString();
                txtJobRate.Text = Ds.Tables[0].Rows[0]["JobRate"].ToString();
                txtMaterialJobWorkGST.Text = Ds.Tables[0].Rows[0]["MaterialJobWorkGST"].ToString();
                txtMaterialJobWorkIGST.Text = Ds.Tables[0].Rows[0]["MaterialJobWorkIGST"].ToString();
                Txtdiscount.Text = Ds.Tables[0].Rows[0]["LessDiscount"].ToString();
                txtPackingForwardingCharges.Text = Ds.Tables[0].Rows[0]["PackingForwardingCharges"].ToString();
                txtlessadv.Text = Ds.Tables[0].Rows[0]["LessAdvance"].ToString();

            }
            BtnSave.Text = "Update";
            con.Close();
            con.Dispose();
        }
    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string Str;
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGOrderDetail.Rows[index];
        BtnNewSave.Visible = true;
        if (e.CommandName.Equals("New"))
        {
            DGConsumption.Visible = true;
            DGExpence.Visible = false;
            DGPacking.Visible = false;
            Session["FunctionType"] = 1;
            int id = Convert.ToInt32(DGOrderDetail.DataKeys[index].Value.ToString());
            REFIMAGEDG.Visible = false;
            BtnNewSave.Visible = true;
            Str = @"Select Distinct Str(PCMDID)+'|'+Str(DD.OrderDetailId) PCMDID,PM.PROCESS_NAME PROCESSNAME,VF.Category_Name ICategory,VF.Item_Name IItem,VF.QualityName+Space(2)+VF.DesignName+Space(2)+VF.ColorName+Space(2)+VF.ShadeColorName+Space(2)+
                   VF.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF.SizeMtr Else VF.SizeFt End IDescription,U.UnitName IUnitName,
                   IQTY,ILOSS,IRATE,VF1.Category_Name OCategory,VF1.Item_Name OItem,VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShadeColorName+Space(2)+
                   VF1.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF1.SizeMtr Else VF1.SizeFt End ODescription,U1.UnitName OUnitName,OQTY,ORATE
                   FROM DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DOD,DRAFT_ORDER_CONSUMPTION_DETAIL DD,V_FinishedItemDetail VF,V_FinishedItemDetail VF1,Unit U,Unit U1,PROCESS_NAME_MASTER PM
                   Where DM.Orderid=DD.Orderid And DM.ORDERID=DOD.ORDERID AND DOD.OrderDetailID=DD.OrderDetailID And DD.IFinishedid=VF.Item_Finished_id And DD.OFinishedid=VF1.Item_Finished_id And U.UnitId=DD.IUnitId And U1.UnitId=DD.OUnitId And
				   PM.PROCESS_NAME_ID=DD.PROCESSID And DD.OrderDetailId=" + id + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGConsumption.DataSource = ds;
            DGConsumption.DataBind();
        }
        if (e.CommandName.Equals("Exp"))
        {
            DGConsumption.Visible = false;
            DGExpence.Visible = true;
            DGPacking.Visible = false;
            Session["FunctionType"] = 2;
            REFIMAGEDG.Visible = false;
            Str = @"SELECT Distinct Str(DOC.CWOEID)+'|'+str(DOC.ORDERDETAILID) CWOEID,EN.CHARGENAME,DOC.PERCENTAGE FROM DRAFT_ORDER_CUSTWISEOTHEREXPENCE DOC,EXPENSENAME EN 
                    WHERE DOC.EXPID=EN.EXPID AND ORDERDETAILID=" + row.Cells[0].Text + " And EN.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGExpence.DataSource = ds;
            DGExpence.DataBind();
        }
        else if (e.CommandName.Equals("RefImage"))
        {
            Str = @"SELECT ID,PHOTO FROM Draft_Order_ReferenceImage Where ORDERDETAILID=" + row.Cells[0].Text;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            REFIMAGEDG.DataSource = ds;
            REFIMAGEDG.DataBind();
        }

    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Session["varCompanyId"].ToString() == "10")
            {
                DGOrderDetail.Columns[1].HeaderText = "ITEMCODE";

            }
            Label lbl = ((Label)e.Row.FindControl("lblconsumpflag"));
            if (Convert.ToInt32(lbl.Text) > 0)
            {
                e.Row.BackColor = Color.Green;
            }

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);

        }

    }
    protected void BtnNewSave_Click(object sender, EventArgs e)
    {
        int N = 0;
        string strPCMDID = "";
        if (Convert.ToInt32(Session["FunctionType"]) == 1)
        {
            N = DGConsumption.Rows.Count;
            for (int i = 0; i < N; i++)
            {
                strPCMDID = DGConsumption.DataKeys[i].Value.ToString();
                TextBox IQTY = (TextBox)DGConsumption.Rows[i].FindControl("TXTIQTY");
                TextBox IRATE = (TextBox)DGConsumption.Rows[i].FindControl("TXTIRate");
                TextBox ILOSS = (TextBox)DGConsumption.Rows[i].FindControl("TXTILOSS");
                TextBox OQTY = (TextBox)DGConsumption.Rows[i].FindControl("TXTOQTY");
                TextBox ORATE = (TextBox)DGConsumption.Rows[i].FindControl("TXTORAte");
                string str = @"UPDATE DRAFT_ORDER_CONSUMPTION_DETAIL Set IQTY=" + IQTY.Text + ",ILOSS=" + ILOSS.Text + ",IRate=" + IRATE.Text + ",OQTY=" + OQTY.Text + ",ORate=" + ORATE.Text + @" 
                            Where ORDERDETAILID=" + Convert.ToInt32(strPCMDID.Split('|')[1]) + " And PCMDID=" + Convert.ToInt32(strPCMDID.Split('|')[0]) + "";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            }
        }
        else if (Convert.ToInt32(Session["FunctionType"]) == 2)
        {
            N = DGExpence.Rows.Count;
            for (int i = 0; i < N; i++)
            {
                strPCMDID = DGExpence.DataKeys[i].Value.ToString();
                TextBox Percentage = (TextBox)DGExpence.Rows[i].FindControl("TXTPercentage");
                string str = @"UPDATE DRAFT_ORDER_CUSTWISEOTHEREXPENCE Set PERCENTAGE=" + Percentage.Text + " Where ORDERDETAILID=" + Convert.ToInt32(strPCMDID.Split('|')[1]) + " And CWOEID=" + Convert.ToInt32(strPCMDID.Split('|')[0]) + "";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            }
        }
        else if (Convert.ToInt32(Session["FunctionType"]) == 3)
        {
            N = DGPacking.Rows.Count;
            for (int i = 0; i < N; i++)
            {
                strPCMDID = DGPacking.DataKeys[i].Value.ToString();
                TextBox INNERAMT = (TextBox)DGPacking.Rows[i].FindControl("TXTINNERAMT");
                TextBox MIDDLEAMT = (TextBox)DGPacking.Rows[i].FindControl("TXTMIDDLEAMT");
                TextBox MASTERAMT = (TextBox)DGPacking.Rows[i].FindControl("TXTMASTERAMT");
                TextBox OTHERAMT = (TextBox)DGPacking.Rows[i].FindControl("TXTOTHERAMT");
                TextBox CONTAINERAMT = (TextBox)DGPacking.Rows[i].FindControl("TXTCONTAINERAMT");
                string str = @"UPDATE DRAFT_ORDER_PACKING_AND_OTHERMATERIAL_COST Set INNERAMT=" + INNERAMT.Text + ",MIDDLEAMT=" + MIDDLEAMT.Text + ",MASTERAMT=" + MASTERAMT.Text + ",OTHERAMT=" + OTHERAMT.Text + ",CONTAINERAMT=" + CONTAINERAMT.Text + " Where ORDERDETAILID=" + Convert.ToInt32(strPCMDID.Split('|')[1]) + " And PRMCID=" + Convert.ToInt32(strPCMDID.Split('|')[0]) + "";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            }
        }
        DGConsumption.DataSource = "";
        DGExpence.DataSource = "";
        DGPacking.DataSource = "";
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = false;
        BtnNewSave.Visible = false;
        Session["FunctionType"] = "0";
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        // ProdCode_TextChanges();
        if (TxtProdCode.Text != "")
        {
            //BtnNew_Click(sender, e);
        }
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        if (TxtProdCode.Text != "")
        {
            UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME", true, "--SELECT--");
            Str = "SELECT IPM.*,IM.CATEGORY_ID,IsNull(PHOTO,'') PHOTO,IsNull(MII.REMARKS,'') REMARKS from ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM LEFT OUTER JOIN MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID WHERE IPM.ITEM_ID=IM.ITEM_ID And ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                IN_CATEGORY_DEPENDS_CONTROLS();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, ddshadecolor, Convert.ToInt32(DDItemName.SelectedValue));
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();

                //if (Session["ProdCodeValidation"].ToString() != "1")
                //{
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                Fill_Size();
                //UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID,SIZEFT FROM SIZE WHERE SHAPEID=" + DDShape.SelectedValue + "", true, "--SELECT--");
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                //}
                // int ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TxtOurCode.Text, Convert.ToInt32(Session["varCompanyId"]));
                int ItemFinishedId = Convert.ToInt32(ds.Tables[0].Rows[0]["Item_Finished_id"]);
                ViewState["FinishedID"] = ItemFinishedId;
                FillNet_GrossWeight(ItemFinishedId);
                area();
                TxtDescription.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                TxtQuantity.Focus();
                newPreview1.ImageUrl = ds.Tables[0].Rows[0]["PHOTO"].ToString();
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                LblErrorMessage.Visible = true;
                DDItemCategory.SelectedIndex = 0;
                IN_CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDItemCategory.SelectedIndex = 0;
            IN_CATEGORY_DEPENDS_CONTROLS();
        }
    }
    private void IN_CATEGORY_DEPENDS_CONTROLS()
    {
        DivQuality.Visible = false;
        trDesign.Visible = false;
        trColor.Visible = false;
        trShape.Visible = false;
        trSize.Visible = false;
        trShadeColor.Visible = false;
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select DISTINCT ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by ITEM_NAME", true, "SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDItemCategory.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        DivQuality.Visible = true;
                        break;
                    case "2":
                        trDesign.Visible = true;
                        break;
                    case "3":
                        trColor.Visible = true;
                        break;
                    case "4":
                        trShape.Visible = true;
                        break;
                    case "5":
                        trSize.Visible = true;
                        break;
                    case "6":
                        trShadeColor.Visible = true;
                        break;
                }
            }
        }
    }
    private void ProdCode_TextChanges()
    {

    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        #region On 27-Nov-2012
        //string Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " order by QUALITYNAME";
        //UtilityModule.ConditionalComboFill(ref Quality, Str, true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN order by DESIGNNAME", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR order by COLORNAME", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE order by SHAPENAME", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Shade, "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR order by SHADECOLORNAME", true, "--SELECT--");
        #endregion
        string strsql = "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + Itemid + " And Q.MasterCompanyId=" + Session["varCompanyId"] + @" order by Q.QualityName
                           select V.DesignId,Case when DesignNameAToC is null then DesignName Else DesignNameAToC End DesignName from Design V left outer join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + @" order by designname
                            SELECT  C.ColorId,Case When ColorNameToC is null then ColorName Else ColorNameToC End ColorName from Color C left outer join CustomerColor CC on CC.ColorId=C.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + @" order by ColorName
                            SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + @"
                            SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varCompanyId"];
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        // int i = 0;
        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");

    }
    protected void btnContainerpackingMatCost_Click(object sender, EventArgs e)
    {
        btnContainerPackingCost.Visible = true;
    }
    protected void TxtQuantity_TextChanged(object sender, EventArgs e)
    {
        TxtUPCNO.Text = "";
        int ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TxtOurCode.Text, Convert.ToInt32(Session["varCompanyId"]));
        string Str = "SELECT Isnull(UPCNO,0) UPCNO FROM UPCNO WHERE CUSTOMERID=" + DDCustomerCode.SelectedValue + " AND FINISHEDID=" + ItemFinishedId;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtUPCNO.Text = Ds.Tables[0].Rows[0]["UPCNO"].ToString();
        }
        TxtPrice.Focus();
    }
    protected void DDPreviewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Report_Type();
    }
    private void report()
    {
        if (Convert.ToInt32(DDPreviewType.SelectedValue) == 0)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "10":
                    Session["ReportPath"] = "Reports/RptPerFormaInvoiceorderforraasindia.rpt";
                    break;
                default:
                    Session["ReportPath"] = "Reports/RptPerFormaInvoiceNEW.rpt";
                    break;
            }
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 1)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceWithBuyerCodeNew.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 2)
        {
            Session["ReportPath"] = "Reports/RptPhotoQuatiationNew.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 3)
        {
            Session["ReportPath"] = "Reports/RptPerformaInvoice1New.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 4)
        {
            Session["ReportPath"] = "Reports/RptPerformaInvoice2New.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 5)
        {
            Session["ReportPath"] = "Reports/RptPerFomaInvioceWithOutPKGNew.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 6)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceCh1New.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 7)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceCh2New.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 8)
        {
            Session["ReportPath"] = "Reports/RptPhotoQuatiationNew2.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 9)
        {
            Session["ReportPath"] = "Reports/RptAreaRugRetail.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 10)
        {
            Session["ReportPath"] = "Reports/RptCustomizationQuotation.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 11)
        {
            Session["ReportPath"] = "Reports/RptUnderlaySupplyQuotation.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 12)
        {
            Session["ReportPath"] = "Reports/RptInstallationWallQuotation.rpt";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 13)
        {
            Session["ReportPath"] = "Reports/RptProformaInvoiceNew.rpt";
        }

    }
    private void Report_Type()
    {
        // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        switch (Convert.ToInt16(Session["varCompanyId"]))
        {
            case 2:
                PERFORMINVOICEFORDESTINI();
                break;
            case 3:
                PERFORMINVOICEFORMALANI();
                break;
            default:
                PERFORMINVOICEFORMALANI();
                break;
        }
    }

    private void PERFORMINVOICEFORMALANI()
    {
        report();
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@PreviewType", DDPreviewType.SelectedValue);
        array[1] = new SqlParameter("@OrderId", ViewState["orderid"]);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        //array[2] = new SqlParameter("@MasterCompanyId", 10);
        array[3] = new SqlParameter("@UserId", Session["varuserId"]);
        array[4] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[4].Direction = ParameterDirection.Output;
        array[5] = new SqlParameter("@ReportPath", Session["ReportPath"]);
        array[6] = new SqlParameter("@dsFileName", SqlDbType.VarChar, 150);
        array[6].Direction = ParameterDirection.Output;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDraftOrderNewReportData", array);

        Session["dsFileName"] = array[6].Value;

        //if (ds.Tables[2].Rows.Count > 0)
        //{
        ds.Tables[2].TableName = "Table3";
        //}
        ds.Tables[1].TableName = "Table2";
        ds.Tables[0].TableName = "Table1";

        ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (Convert.ToString(dr["Photo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                if (TheFile.Exists)
                {
                    string img = dr["Photo"].ToString();
                    img = Server.MapPath(img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["ImageName"] = img_Byte;
                }
            }
        }


        ds.Tables[0].Columns.Add("CompanyLogo2", typeof(System.Byte[]));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (Convert.ToString(dr["CompanyLogo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                if (TheFile.Exists)
                {
                    string img = dr["CompanyLogo"].ToString();
                    img = Server.MapPath(img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["CompanyLogo2"] = img_Byte;
                }
            }
        }

        //string STR1 = @"select OrderDetailId,Photo from Draft_Order_ReferenceImage where OrderDetailId in(select orderdetailid from draft_order_detail where orderid=" + ViewState["orderid"] + ") ";
        //SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
        //DataTable dt1 = new DataTable();
        //sda1.Fill(dt1);
        //ds.Tables.Add(dt1);
        ds.Tables[1].Columns.Add("RefImageName", typeof(System.Byte[]));
        if (ds.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["RefImageName"] = img_Byte;
                    }
                }
            }
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            //ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=10px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["orderid"] + "";
    }
    protected void BTNREFIMAGE_Click(object sender, EventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = false;
        Session["FunctionType"] = 4;
        REFIMAGEDG.Visible = true;
    }
    private void PERFORMINVOICEFORDESTINI()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        DataSet Ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='VIEW_PERFORMAINVOICEFORDESTINI'");
        if (Ds1.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICEFORDESTINI");
        }
        #region
        //string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINI AS ";
        ////DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        //Str = Str + "SELECT PM.PROCESSID,PM.FINISHEDID,PD.IFINISHEDID,PD.OFINISHEDID,DOD.ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        //for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        //{
        //    Str = Str + ",[dbo].[GET_FINAMT](PM.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PD.IFINISHEDID) " + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "";
        //}
        //Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        //Str = Str + ",DOD.PHOTO IMAGE,DOD.QTY OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PM.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,DOD.DESCRIPTION,dod.weight  FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,DRAFT_ORDER_DETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT WHERE PM.PCMID=PD.PCMID AND PM.FINISHEDID=DOD.ITEM_FINISHED_ID AND PM.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["orderid"] + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        #endregion
        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINI AS ";
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        Str = Str + "SELECT PM.PROCESSID,PM.FINISHEDID,PM.IFINISHEDID,PM.OFINISHEDID,DOD.ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Str = Str + ",[dbo].[GET_FINAMT](PM.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PM.IFINISHEDID) " + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "";
        }
        Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        Str = Str + ",DOD.PHOTO IMAGE,DOD.QTY OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PM.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,CRBCode,DOD.DESCRIPTION,dod.weight FROM DRAFT_ORDER_CONSUMPTION_DETAIL PM,DRAFT_ORDER_DETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT WHERE  PM.Orderdetailid=DOD.Orderdetailid AND PM.FINISHEDID=IPM.ITEM_FINISHED_ID AND PM.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND PM.O_FINISHED_TYPE_ID=FT.ID AND PM.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["orderid"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        con.Close();
        // if (Ds1.Tables[0].Rows.Count > 0)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceDestini2New.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceDestini2New.xsd";
            string str = @" SELECT customerinfo.CompanyName, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, customerinfo.Address, VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID, VIEW_PERFORMAINVOICEFORDESTINI.IFINISHEDID,
            VIEW_PERFORMAINVOICEFORDESTINI.ITEM_NO, VIEW_PERFORMAINVOICEFORDESTINI.GLASSTYPE, VIEW_PERFORMAINVOICEFORDESTINI.QTY, VIEW_PERFORMAINVOICEFORDESTINI.InnerPacking,
            VIEW_PERFORMAINVOICEFORDESTINI.MasterPacking, VIEW_PERFORMAINVOICEFORDESTINI.PROD_NO, customerinfo.Email, VIEW_PERFORMAINVOICEFORDESTINI.MiddlePacking, VIEW_PERFORMAINVOICEFORDESTINI.OQTY,
            VIEW_PERFORMAINVOICEFORDESTINI.PKGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.LBGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.CBM, VIEW_PERFORMAINVOICEFORDESTINI.Remarks, 
            VIEW_PERFORMAINVOICEFORDESTINI.BuyerCode, DRAFT_ORDER_MASTER.OrderNo, DRAFT_ORDER_MASTER.OrderDate, DRAFT_ORDER_MASTER.CustOrderNo, VIEW_PERFORMAINVOICEFORDESTINI.DESCRIPTION, 
            DRAFT_ORDER_MASTER.DeliveryDate, DRAFT_ORDER_MASTER.SeaPort, GoodsReceipt.StationName, TransMode.transmodeName, VIEW_PERFORMAINVOICEFORDESTINI.weight, customerinfo.CustomerCode, 
            DRAFT_ORDER_MASTER.Custorderdate, VIEW_PERFORMAINVOICEFORDESTINI.IMAGE as photo,VIEW_PERFORMAINVOICEFORDESTINI.CRBCode BarCode
            FROM   ((((dbo.CompanyInfo CompanyInfo INNER JOIN dbo.DRAFT_ORDER_MASTER DRAFT_ORDER_MASTER ON CompanyInfo.CompanyId=DRAFT_ORDER_MASTER.CompanyId) INNER JOIN dbo.VIEW_PERFORMAINVOICEFORDESTINI VIEW_PERFORMAINVOICEFORDESTINI ON DRAFT_ORDER_MASTER.OrderId=VIEW_PERFORMAINVOICEFORDESTINI.ORDERID) INNER JOIN dbo.customerinfo customerinfo ON DRAFT_ORDER_MASTER.CustomerId=customerinfo.CustomerId) LEFT OUTER JOIN dbo.GoodsReceipt GoodsReceipt ON DRAFT_ORDER_MASTER.PortOfLoading=GoodsReceipt.GoodsreceiptId) LEFT OUTER JOIN dbo.TransMode TransMode ON DRAFT_ORDER_MASTER.ByAirSea=TransMode.transmodeId  
            Where VIEW_PERFORMAINVOICEFORDESTINI.Orderid=" + ViewState["orderid"] + @"
            ORDER BY VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID";
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            //            DataSet Ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='V_PackingCostFinishedID'");
            //            if (Ds2.Tables[0].Rows.Count > 0)
            //            {
            //                SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW V_PackingCostFinishedID");
            //            }
            //            string str1 = @" create view V_PackingCostFinishedID as
            //                 Select Distinct PackingType,Finishedid from PACKINGCOST where finishedid in (select Distinct Finishedid from VIEW_PERFORMAINVOICEFORDESTINI)";
            //            SqlHelper.ExecuteDataset(con, CommandType.Text, str1);
            //            Ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='V_PackingCost1'");
            //            if (Ds2.Tables[0].Rows.Count > 0)
            //            {
            //                SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW V_PackingCost1");
            //            }
            //            str1 = @" Create view V_PackingCost1 as 
            //                SELECT PACKINGCOST.Length, PACKINGCOST.Width, PACKINGCOST.Height, PACKINGCOST.NetCost, PACKINGCOST.PCS, PACKINGCOST.PackingType, PACKINGCOST.Finishedid
            //                FROM   dbo.PACKINGCOST PACKINGCOST where finishedid not in (select Distinct Finishedid from V_PackingCostFinishedID where  PackingType =1)
            //                Union
            //                SELECT PACKINGCOST.Length, PACKINGCOST.Width, PACKINGCOST.Height, PACKINGCOST.NetCost, PACKINGCOST.PCS, PACKINGCOST.PackingType, PACKINGCOST.Finishedid
            //                FROM   dbo.PACKINGCOST PACKINGCOST where finishedid not in (select Distinct Finishedid from V_PackingCostFinishedID where PackingType =2)
            //                Union
            //                SELECT PACKINGCOST.Length, PACKINGCOST.Width, PACKINGCOST.Height, PACKINGCOST.NetCost, PACKINGCOST.PCS, PACKINGCOST.PackingType, PACKINGCOST.Finishedid
            //                FROM   dbo.PACKINGCOST PACKINGCOST where finishedid not in (select Distinct Finishedid from V_PackingCostFinishedID where PackingType =3)";
            //            SqlHelper.ExecuteDataset(con, CommandType.Text, str1);
            //            str1 = @"select Length,Width,Height,NetCost,PCS,PackingType,Finishedid from V_PackingCost1
            //                    Union 
            //                    SELECT Length, Width, Height, NetCost, PCS, PackingType, Finishedid
            //                    FROM  PACKINGCOST where finishedid  in (select Distinct Finishedid from V_PackingCostFinishedID)";

            string str1 = @" SELECT PACKINGCOST.Length, PACKINGCOST.Width, PACKINGCOST.Height, PACKINGCOST.NetCost, PACKINGCOST.PCS, PACKINGCOST.PackingType, PACKINGCOST.Finishedid
                            FROM   dbo.PACKINGCOST PACKINGCOST";

            SqlDataAdapter sda1 = new SqlDataAdapter(str1, con);
            DataTable dt = new DataTable();
            sda1.Fill(dt);
            ds.Tables.Add(dt);
            string str2 = @"  SELECT VIEW_FINISHED_TYPEFORDESTINI.FINISHED_TYPE_NAME, VIEW_FINISHED_TYPEFORDESTINI.FINISHEDID, VIEW_FINISHED_TYPEFORDESTINI.PCMID, VIEW_FINISHED_TYPEFORDESTINI.IFINISHEDID
            FROM   dbo.VIEW_FINISHED_TYPEFORDESTINI VIEW_FINISHED_TYPEFORDESTINI
            ORDER BY VIEW_FINISHED_TYPEFORDESTINI.PCMID";
            SqlDataAdapter sda2 = new SqlDataAdapter(str2, con);
            DataTable dt1 = new DataTable();
            sda2.Fill(dt1);
            ds.Tables.Add(dt1);
            //sda.Fill(ds);
            ds.Tables[0].Columns.Add("ImageThumbNail", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["ImageThumbNail"] = img_Byte;
                    }
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }

        }
        //Session["ReportPath"] = "Reports/RptPerFormaInvoiceDestini2.rpt";
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICEFORDESTINI.Orderid}=" + ViewState["orderid"] + "";
    }
    protected void BTNEXCEL_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICEFORDESTINI");
        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINI AS ";
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        Str = Str + "SELECT PM.PROCESSID,PM.FINISHEDID,PD.IFINISHEDID,PD.OFINISHEDID,DOD.ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Str = Str + ",[dbo].[GET_FINAMT](PM.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PD.IFINISHEDID) " + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "";
        }
        Str = Str + ",DOD.PHOTO IMAGE FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,DRAFT_ORDER_DETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT WHERE PM.PCMID=PD.PCMID AND PM.FINISHEDID=DOD.ITEM_FINISHED_ID AND PM.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["orderid"] + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        con.Close();
    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = false;
        Session["FunctionType"] = 5;
        REFIMAGEDG.Visible = false;
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT IPM.*,IM.CATEGORY_ID from ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM LEFT OUTER JOIN MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID WHERE IPM.ITEM_ID=IM.ITEM_ID And ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            newPreview1.ImageUrl = "~/ImageHandler.ashx?Id=" + Ds.Tables[0].Rows[0]["ITEM_FINISHED_ID"] + "&img=3";
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT Distinct ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=0 And ProductCode Like '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId + "";
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
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDColor.Focus();
        if (Session["varCompanyId"].ToString() == "2")
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TxtOurCode.Text, Convert.ToInt32(Session["varCompanyId"]));
            ViewState["FinishedID"] = ItemFinishedId;
            FillNet_GrossWeight(ItemFinishedId);

        }
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddshadecolor.Focus();
        btnaddcolor.Focus();
    }
    //protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void REFIMAGEDG_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGExpence_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGPacking_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = false;
        Session["FunctionType"] = 5;
        REFIMAGEDG.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrpara = new SqlParameter[2];
            _arrpara[0] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrpara[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("lblorderdetailid")).Text;
            _arrpara[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DRAFT_ORDER_DELETE_ROW", _arrpara);
            Tran.Commit();
            LblErrorMessage.Text = "DATA DELETED SUCCESSFULLY..";
            ViewState["orderid"] = _arrpara[1].Value;
            Fill_Grid();
        }
        catch (Exception)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Session["FunctionType"] = "0";
    }
    protected void DGOrderDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //        DGConsumption.Visible = true;
        //        DGExpence.Visible = false;
        //        DGPacking.Visible = false;
        //        Session["FunctionType"] = 1;
        //        int id = Convert.ToInt32(DGOrderDetail.DataKeys[e.NewEditIndex].Value.ToString());
        //        REFIMAGEDG.Visible = false;
        //        string Str;
        //        BtnNewSave.Visible = true;
        //        Str = @"Select Distinct Str(PCMDID)+'|'+Str(DD.OrderDetailId) PCMDID,PM.PROCESS_NAME PROCESSNAME,VF.Category_Name ICategory,VF.Item_Name IItem,VF.QualityName+Space(2)+VF.DesignName+Space(2)+VF.ColorName+Space(2)+VF.ShadeColorName+Space(2)+
        //                   VF.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF.SizeMtr Else VF.SizeFt End IDescription,U.UnitName IUnitName,
        //                   IQTY,ILOSS,IRATE,VF1.Category_Name OCategory,VF1.Item_Name OItem,VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShadeColorName+Space(2)+
        //                   VF1.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF1.SizeMtr Else VF1.SizeFt End ODescription,U1.UnitName OUnitName,OQTY,ORATE
        //                   FROM DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DOD,DRAFT_ORDER_CONSUMPTION_DETAIL DD,V_FinishedItemDetail VF,V_FinishedItemDetail VF1,Unit U,Unit U1,PROCESS_NAME_MASTER PM
        //                   Where DM.Orderid=DD.Orderid And DM.ORDERID=DOD.ORDERID AND DOD.OrderDetailID=DD.OrderDetailID And DD.IFinishedid=VF.Item_Finished_id And DD.OFinishedid=VF1.Item_Finished_id And U.UnitId=DD.IUnitId And U1.UnitId=DD.OUnitId And
        //				   PM.PROCESS_NAME_ID=DD.PROCESSID And DD.OrderDetailId=" + id + "";
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        DGConsumption.DataSource = ds;
        //        DGConsumption.DataBind();
    }
    protected void DGOrderDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = true;
        Session["FunctionType"] = 3;
        REFIMAGEDG.Visible = false;
        String Str = @"SELECT Distinct Str(PRMCID)+'|'+str(ORDERDETAILID) PRMCID,INNERAMT,MIDDLEAMT,MASTERAMT,OTHERAMT,CONTAINERAMT FROM DRAFT_ORDER_PACKING_AND_OTHERMATERIAL_COST 
                    WHERE ORDERDETAILID=" + DGOrderDetail.DataKeys[e.RowIndex].Value.ToString() + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGPacking.DataSource = ds;
        DGPacking.DataBind();
    }
    protected void RefereshBuyerMasterCode_Click(object sender, EventArgs e)
    {
        ItemSelectedChange();
        fillCombo();
    }
    protected void BtnReport_Click(object sender, EventArgs e)
    {

        Report_Type();
    }
    private void FillNet_GrossWeight(int Varfinishedid)
    {
        string Qry = @"select isnull(max(NETWEIGHT),0) NETWEIGHT,isnull(max(GROSSWEIGHT),0) GROSSWEIGHT from processconsumptionmaster where FINISHEDID=" + Varfinishedid + @"
                       select isnull(ApprovedAmount,0) ApprovedAmount,isnull(CurrencyID,0) CurrencyID from Item_Approval_Detail  where FINISHEDID=" + Varfinishedid;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtWeight.Text = Ds.Tables[0].Rows[0]["NetWeight"].ToString();
            TxtGrsweight.Text = Ds.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();

            if (TxtGrsweight.Text == "" || TxtGrsweight.Text == "0")
            {
                TxtGrsweight.Text = TxtWeight.Text;
            }

        }
        if (Ds.Tables[1].Rows.Count > 0)
        {
            //TxtNetPrice.Text = Ds.Tables[1].Rows[0]["ApprovedAmount"].ToString();
            TxtPrice.Text = Ds.Tables[1].Rows[0]["ApprovedAmount"].ToString();
            DDCurrency.SelectedValue = Ds.Tables[1].Rows[0]["CurrencyID"].ToString();
        }

    }
    protected void DDCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlParameter[] _param = new SqlParameter[5];
        _param[0] = new SqlParameter("@FinishedID", ViewState["FinishedID"].ToString());
        _param[1] = new SqlParameter("@CurrencyID", DDCurrency.SelectedValue);
        _param[2] = new SqlParameter("@NetAmount", SqlDbType.Float);
        _param[2].Direction = ParameterDirection.InputOutput;
        _param[2].Value = TxtPrice.Text;
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Draft_CurrencyConversion", _param);
        TxtPrice.Text = (Math.Round(float.Parse(_param[2].Value.ToString()), 2)).ToString();
    }
    protected void Btn_Click(object sender, EventArgs e)
    {
        ViewState["OrderDetailId"] = 0;
        BtnSave.Text = "Save";

    }
    protected void byn12_Click(object sender, EventArgs e)
    {
        ViewState["orderid"] = 460;
        Fill_Grid();
    }
    protected void GetdraftOrderno()
    {
        string str = "";
        DataSet ds;
        if (Session["varcompanyNo"].ToString() == "6")
        {
            switch (Convert.ToInt16(DDCustomerCode.SelectedValue))
            {
                case 48: //Sample
                    str = @"select isnull(Count(orderid), 0)+1 as orderid from draft_order_master Where customerid=48
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
                case 49: //Designing
                    str = @"select isnull(Count(orderid), 0)+1 as orderid from draft_order_master Where customerid=49
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
                default:
                    str = @"select isnull(Count(orderid), 0)+1 as orderid from draft_order_master Where customerid not in(48,49)
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
            }
        }
        else
        {
            str = @"select isnull(max(orderid), 0)+1 as orderid from draft_order_master
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;

        }

        if (Session["varcompanyNo"].ToString() == "33")
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            string code = "QAN/Q-" + ds.Tables[0].Rows[0]["orderid"].ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(ds.Tables[1].Rows[0]["date"]));
            TxtLocalOrderNo.Text = code;
            ds.Dispose();
        }
        else
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            string code = ViewState["PICode"].ToString() + (ds.Tables[1].Rows[0]["code"] == "" ? "" : ds.Tables[1].Rows[0]["code"] + "_") + ds.Tables[0].Rows[0]["orderid"].ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(ds.Tables[1].Rows[0]["date"]));
            TxtLocalOrderNo.Text = code;
            ds.Dispose();
        }

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Size();
    }
    protected void chkotherinformation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkotherinformation.Checked == true)
        {
            DivOtherInformation.Visible = true;
        }
        else
        {
            DivOtherInformation.Visible = false;
        }
    }
    protected void GetDraftPINo()
    {
        string str = "";
        DataSet ds;
        if (Session["varcompanyNo"].ToString() == "6")
        {
            switch (Convert.ToInt16(DDCustomerCode.SelectedValue))
            {
                case 48: //Sample
                    str = @"select isnull(Count(PISeries), 0)+1 as PISeries from draft_order_master Where customerid=48
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
                case 49: //Designing
                    str = @"select isnull(Count(PISeries), 0)+1 as PISeries from draft_order_master Where customerid=49
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
                default:
                    str = @"select isnull(Count(PISeries), 0)+1 as PISeries from draft_order_master Where customerid not in(48,49)
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;
                    break;
            }
        }
        else
        {
            str = @"select isnull(max(PISeries), 0)+1 as PISeries from draft_order_master
                     select isnull(CustomerCode,'') as code,getdate() as date from customerinfo where customerid=" + DDCustomerCode.SelectedValue;

        }

        if (Session["varcompanyNo"].ToString() == "33")
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            string code = "QAN/PI/" + ds.Tables[0].Rows[0]["PISeries"].ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(ds.Tables[1].Rows[0]["date"]));
            txtPINo.Text = code;
            ViewState["PISeries"] = ds.Tables[0].Rows[0]["PISeries"];
            ds.Dispose();
        }
        else
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            string code = ViewState["PICode"].ToString() + (ds.Tables[1].Rows[0]["code"] == "" ? "" : ds.Tables[1].Rows[0]["code"] + "_") + ds.Tables[0].Rows[0]["PISeries"].ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(ds.Tables[1].Rows[0]["date"]));
            txtPINo.Text = code;
            ViewState["PISeries"] = ds.Tables[0].Rows[0]["PISeries"];
            ds.Dispose();
        }

    }
    protected void ChkGeneratePINo_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkGeneratePINo.Checked == true)
        {
            TDPINo.Visible = true;
            GetDraftPINo();
        }
        else
        {
            TDPINo.Visible = false;
        }
    }
    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (txtStockNo.Text != "")
        {
            DataSet ds2 = null;
            DataSet ds = null;
            int ItemFinishedId2 = 0;
            decimal price = 0;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                ds2 = null;
                string sql2 = @"select CN.item_Finished_Id,S.Price from CarpetNumber CN INNER JOIN Stock S ON CN.item_Finished_Id=S.Item_Finished_Id where CN.TStockNo='" + txtStockNo.Text.Trim() + "'";
                ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, sql2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    ItemFinishedId2 = Convert.ToInt32(ds2.Tables[0].Rows[0]["item_Finished_Id"]);
                    price = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Price"]);

                    if (ItemFinishedId2 > 0)
                    {
                        ds = null;
                        string sql = @"select CATEGORY_ID,QualityId,designId,ColorId,ShapeId,SizeId,ITEM_ID,ShadecolorId,MasterCompanyId from V_FinishedItemDetail where ITEM_FINISHED_ID=" + ItemFinishedId2 + " And MasterCompanyId=" + Session["varCompanyId"];
                        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
                            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
                            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT Q.QualityId,Case when QualityNameAToC is null then Q.QualityName Else QualityNameAToC End QualityName from Quality Q left outer join CustomerQuality CQ on Q.QualityId=CQ.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by Q.QualityName", true, "--SELECT--");
                            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                            UtilityModule.ConditionalComboFill(ref DDItemCode, "select  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
                            DDItemCode.SelectedValue = "0";

                            UtilityModule.ConditionalComboFill(ref DDDesign, "select V.DesignId,Case when DesignNameAToC is null then DesignName Else DesignNameAToC End DesignName from Design V left outer join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
                            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  C.ColorId,Case When ColorNameToC is null then ColorName Else ColorNameToC End ColorName from Color C left outer join CustomerColor CC on CC.ColorId=C.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
                            UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename ", true, "--SELECT--");
                            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
                            DDColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                            DDShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                            Fill_Size();
                            DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                            DDSize_SelectedIndexChanged(sender, e);
                            TxtQuantity.Text = "1";
                            TxtPrice.Text = Convert.ToString(price);
                            if (Convert.ToInt32(DDQuality.SelectedValue) > 0)
                            {
                                DivQuality.Visible = true;
                            }
                            else
                            {
                                DivQuality.Visible = false;
                            }
                            if (Convert.ToInt32(DDDesign.SelectedValue) > 0)
                            {
                                trDesign.Visible = true;
                            }
                            else
                            {
                                trDesign.Visible = false;
                            }
                            if (Convert.ToInt32(DDColor.SelectedValue) > 0)
                            {
                                trColor.Visible = true;
                            }
                            else
                            {
                                trColor.Visible = false;
                            }
                            if (Convert.ToInt32(DDShape.SelectedValue) > 0)
                            {
                                trShape.Visible = true;
                            }
                            else
                            {
                                trShape.Visible = false;
                            }
                        }
                    }
                }
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
            DDItemCategory_SelectedIndexChanged(sender, e);
            DDItemName_SelectedIndexChanged(sender, e);
            DDQuality_SelectedIndexChanged(sender, e);
        }
    }
}