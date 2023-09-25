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
using System.Text;
using System.Drawing;
using ClosedXML.Excel;

public partial class Masters_Order_Order : System.Web.UI.Page
{
    int OrderDetailId = 0;
    int ItemFinishedId = 0;
    string CustOrderNo;
    static string OldCustOrderNo;
    static int MasterCompanyId;
    static string WithBuyerCode;
    static int Flagforsampleorder = 0;

    DataSet dssort = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        // DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        // WithBuyerCode = dr["WithBuyerCode"].ToString();
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        Flagforsampleorder = variable.Varflagforsampleorder;
        if (!IsPostBack)
        {


            tbsample.ActiveTabIndex = 0;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select WithBuyerCode,VarProdCode from Mastersetting");
            //ViewState["WithBuyercode"] = ds.Tables[0].Rows[0]["WithBuyerCode"].ToString();

            int VarProdCode = Convert.ToInt16(ds.Tables[0].Rows[0]["VarProdCode"]);

            switch (VarProdCode)
            {
                case 0:
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    break;
                case 1:
                    TDtxtprocode.Visible = true;
                    TDLabelprocode.Visible = true;
                    break;
            }
            ViewState["OrderDetailId"] = 0;
            TxtPrice.Text = "0";
            logo();

            tdchklabel.Visible = false;

            if (variable.VarNewQualitySize == "1")
            {
                btnAddSize.Visible = false;
                btnAddQualitySize.Visible = true;
            }
            else
            {
                btnAddSize.Visible = true;
                btnAddQualitySize.Visible = false;
            }

            if (Session["varcompanyId"].ToString() == "20")
            {
                TRArticle.Visible = false;
                CHKFORCURRENTCONSUMPTIONNew.Visible = false;
                CHKFORCURRENTCONSUMPTION.Visible = false;
                btnupdateallconsmp.Visible = false;
                trHeader.Visible = false;
                btnadditemcategory.Visible = false;
                BtnAdd0.Visible = false;
                btnaddquality.Visible = false;
                btnadddesign.Visible = false;
                btnaddcolor.Visible = false;
                btnaddshape.Visible = false;
                btnAddQualitySize.Visible = false;
                ChkForPerformaInvoiceType2.Visible = false;
                TDWareHouseNameByCodeLabel.Visible = true;
                TDWareHouseNameByCodeDDL.Visible = true;
                Label20.Text = "WareHouse Code";
            }
            if (Session["varcompanyId"].ToString() == "16")
            {
                ChkForEditGrid.Visible = true;
                TrInspectionDate.Visible = true;
            }
            if (Session["varcompanyId"].ToString() == "28")
            {
                ChkForEditGrid.Visible = true;
            }
            if (Session["varcompanyId"].ToString() == "44")
            {
                tdfiller.Visible = true;
                tdmer.Visible = true;
                tdincharge.Visible = true;
                TDextraqty.Visible = true;
                tdextraqtylable.Visible = true;


                if (ddfiller.Items.Count > 0)
                {
                    ddfiller.SelectedValue = "2";
                }

                //  tdbuyerrefno.Visible = true;
                //  tdupcno.Visible = true;
            }
            string str = @"select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory
                           select CI.CompanyId,CompanyName From CompanyInfo CI inner Join Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                           SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CompanyName
                           select TransModeId,TransModeName from Transmode order by TransModeName
                           select val,type from sizetype
                           Select PaymentId, PaymentName from Payment Where MasterCompanyId=" + Session["varCompanyId"] + @" order by PaymentName
                           Select TermId,TermName from Term Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By TermName
                           Select RecipeType From MasterSetting(Nolock)
                           Select ID, Name From RecipeMaster (Nolock) Where EnableDisbleFlag = 1 And MasterCompanyID = " + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref ddordertype, ds, 0, true, "Select OrderCategory");
            rdoUnitWise.Checked = true;
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 1, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            BindCustomerCode();
            if (Session["varcompanyId"].ToString() == "20")
            {
                if (DDCustomerCode.Items.Count > 0)
                {
                    DDCustomerCode.SelectedIndex = 0;
                    ddcustomer_selectedchange();
                }
            }
            else
            {
                if (DDCustomerCode.Items.Count > 0)
                {
                    DDCustomerCode.SelectedIndex = 1;
                    ddcustomer_selectedchange();
                }
            }

            TDDDRecipeName.Visible = false;

            if (ds.Tables[7].Rows[0]["RecipeType"].ToString() == "1")
            {
                TDDDRecipeName.Visible = true;
                UtilityModule.ConditionalComboFillWithDS(ref DDRecipeName, ds, 8, true, "--SELECT--");
                if (DDRecipeName.Items.Count > 0)
                {
                    DDRecipeName.SelectedValue = "1";
                }
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDShipment, ds, 3, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 4, false, "");
            if (DDsizetype.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizetype.SelectedValue = variable.VarDefaultSizeId;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddlModeOfPayment, ds, 5, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddlDeliveryTerms, ds, 6, true, "--Select--");

            HNLBL.Value = "1";
            HNLBL.Visible = true;
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtInspectionDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDispatchDate.Text = TxtDeliveryDate.Text;
            DDCustomerCode.Focus();
            ddordertype.SelectedValue = "1";
            if (Session["varCompanyId"].ToString() == "44")
            {
                TDSampleCode.Visible = true;
            }
            lablechange();
            Chkbx.Checked = false;
            ch.Visible = false;
            DDCustomerCode.Focus();

            BindCategoryQuality();

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_ORDER_PHOTO" + "  " + " DELETE TEMP_ORDER_REFERENCE");
            // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            HDF1.Value = Session["varCompanyId"].ToString();
            switch (Convert.ToInt16(Session["varCompanyId"]))
            {
                case 1:

                    ChkForPerformaInvoiceType2.Visible = false;
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    ItemDescription.Visible = true;
                    trlessadv.Visible = false;
                    trlesscomm.Visible = false;
                    TDtxtdiscount.Visible = false;
                    break;
                case 2:

                    ChkForPerformaInvoiceType2.Visible = false;
                    TDtxtprocode.Visible = true;
                    TDLabelprocode.Visible = true;
                    ItemDescription.Visible = false;
                    trlessadv.Visible = false;
                    trlesscomm.Visible = false;
                    TDtxtdiscount.Visible = false;
                    break;
                case 3:
                    //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICE");
                    //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "CREATE VIEW VIEW_PERFORMAINVOICE AS SELECT OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID, OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, T.TermName, P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id, OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight, OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,OM.LocalOrder,Custorderdate,duedate FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId");
                    //ddPreview.Items.Add(new ListItem("PerForma Invoice BUYERCODE", "2"));
                    //ddPreview.Items.Add(new ListItem("Photo Quotation","3"));
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    ItemDescription.Visible = false;
                    trdye.Visible = false;
                    trfin.Visible = false;
                    trprod.Visible = false;
                    trlessadv.Visible = true;
                    trlesscomm.Visible = true;
                    TDtxtdiscount.Visible = true;
                    ChkForPerformaInvoiceType2.Visible = false;
                    break;

                case 4:
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    trlessadv.Visible = false;
                    trlesscomm.Visible = false;
                    TDtxtdiscount.Visible = false;
                    ChkForPerformaInvoiceType2.Visible = false;
                    TDRugIdNo.Visible = true;
                    break;
                case 7:
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICE");
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"CREATE VIEW VIEW_PERFORMAINVOICE AS SELECT OM.CompanyId, OM.CustomerId, 
                    OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID, OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, 
                    T.TermName, P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id, OD.QtyRequired AS QTY, 
                    OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, 
                    OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight, OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, 
                    CI.CurrencyName, '' AS DeliveryComments,OM.LocalOrder FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS OD ON OM.OrderId = OD.OrderId 
                    INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId 
                    LEFT OUTER JOIN dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                    LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId");
                    lblCurrency.Visible = false;
                    Tdcurrency.Visible = false;
                    lblShippment.Visible = false;
                    DDShipment.Visible = false;
                    TDAREA.Visible = false;
                    TDTxtArea.Visible = false;
                    TDTxtArticleNo.Visible = false;
                    TDARTICLENo.Visible = false;
                    TDTXTBUYERCODE.Visible = false;
                    TDlblBUYERCODE.Visible = false;
                    TDWareHouse.Visible = false;
                    TdDDWareHouse.Visible = false;
                    TDlblUPCNO.Visible = false;
                    TDTXTUPCNO.Visible = false;
                    TDLblLBGInstruction.Visible = false;
                    //TDTxtLBGInstruction.Visible = false;
                    TDOURCODE.Visible = false;
                    TDTXTOURCODE.Visible = false;
                    TDPrice.Visible = false;
                    TDtxtPrice.Visible = false;
                    TDDueDate.Visible = true;
                    TxtDueDate.Visible = true;
                    lblduedate.Visible = true;
                    CHKFORCURRENTCONSUMPTION.Visible = false;
                    btnupdateallconsmp.Visible = false;
                    TDTxtcustorderdt.Visible = true;
                    td1.Visible = true;
                    lblcustname.Text = "Stock At PH";
                    lblcustcode.Text = "CUSTOMER NAME";
                    Txtcustorderdt.Visible = true;
                    ddPreview.Visible = false;
                    TDtxtdiscount.Visible = false;
                    LblDelvDate.Text = "EXPIRY DATE*";
                    lblduedate.Text = "Store DEL. DATE *";
                    LblDispatchDate.Text = " Stock at PH Date.";
                    // LblDisplayDelVDate.Text = "EXPIRY DATE";
                    tdrunit.Visible = false;
                    tdunitrate.Visible = false;
                    rdoUnitWise.Checked = false;
                    rdoPcWise.Checked = true;
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    tdordarea.Visible = false;
                    tdtotamt.Visible = false;
                    trlessadv.Visible = false;

                    TxtOrderArea.Visible = false;
                    TxtTotalAmount.Visible = false;

                    trlesscomm.Visible = false;
                    TxtDispatchDate.Enabled = false;
                    lblcustcode.Text = "CUSTOMER NAME*";
                    // labelCustCOde.Text = "CUSTOMERNAME";
                    TDNewOrder.Visible = true;
                    TDNewOrderNo.Visible = true;
                    TxtOrderDate.Enabled = false;
                    Chksupply.Visible = true;
                    BtnReport.Visible = false;
                    BtnNew.Visible = false;
                    td.Visible = true;
                    tdinspdate.Visible = false;
                    TDInspection.Visible = false;
                    TxtLocalOrderNo.Enabled = false;
                    TRInstructions.Visible = false;
                    tdpkins.Visible = false;
                    tdremark.Visible = false;
                    tdmastremark.Visible = true;
                    BtnAddBuyerMasterCode.Visible = false;
                    tdchklabel.Visible = false;
                    BtncostReport.Visible = false;
                    ChkForPerformaInvoiceType2.Visible = false;
                    break;
                case 9:
                    BtncostReport.Visible = false;
                    ChkForPerformaInvoiceType2.Visible = false;
                    //   ddPreview.Items.Add(new ListItem("Costing Detail", "3"));
                    break;
                case 10:
                    //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICE");
                    //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "CREATE VIEW VIEW_PERFORMAINVOICE AS SELECT OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID, OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, T.TermName, P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id, OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight, OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,OM.LocalOrder,Custorderdate,duedate FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId");

                    //ddPreview.Items.Add(new ListItem("PerForma Invoice BUYERCODE", "2"));
                    //ddPreview.Items.Add(new ListItem("Photo Quotation","3"));
                    TDtxtprocode.Visible = false;
                    TDLabelprocode.Visible = false;
                    ItemDescription.Visible = false;
                    trdye.Visible = false;
                    trfin.Visible = false;
                    trprod.Visible = false;
                    trlessadv.Visible = true;
                    trlesscomm.Visible = true;
                    TDtxtdiscount.Visible = true;
                    rdoPcWise.Checked = true;
                    ChkForPerformaInvoiceType2.Visible = false;
                    break;
                case 12:
                    rdoPcWise.Checked = true;
                    ChkForPerformaInvoiceType2.Visible = false;
                    break;
                case 14:
                    TDEditOrder.Visible = false;
                    ChkForPerformaInvoiceType2.Visible = false;
                    if (Session["usertype"].ToString() == "1") //Admin Only
                    {
                        TDEditOrder.Visible = true;
                    }
                    break;
                case 30:
                    // TRArticle.Visible = false;
                    LblDispatchDate.Text = "Ex-Factory Date";
                    ChkForPerformaInvoiceType2.Visible = true;
                    break;
                case 43:
                    TDUpdateConsumptionFolioAndReceive.Visible = true;
                    break;
                default:

                    lblCurrency.Visible = true;
                    Tdcurrency.Visible = true;
                    lblShippment.Visible = true;
                    DDShipment.Visible = true;
                    ChkForPerformaInvoiceType2.Visible = false;
                    break;
            }

            BtnAddBuyerMasterCode.Visible = false;

            //show edit button
            if (Session["canedit"].ToString() == "0") //non authenticated person
            {
                ChkEditOrder.Enabled = false;
            }
            //*************************ReportType
            string Type1;
            switch (Session["varcompanyid"].ToString())
            {
                case "3":
                    Type1 = "PERFORMA INVOICE HSCODE";
                    break;
                case "7":
                    Type1 = "PERFORMA INVOICE HSCODE";
                    break;
                case "10":
                    Type1 = "PERFORMA INVOICE HSCODE";
                    break;
                default:
                    Type1 = "PREVIEW";
                    break;
            }
            ddPreview.Items.Add(new ListItem(Type1, "1"));
            ddPreview.Items.Add(new ListItem("INTERNAL OC", "2"));
            if (Session["varcompanyid"].ToString() == "44")
            {
                ddPreview.Items.Add(new ListItem("PO CONSUMPTION", "3"));
            }
            else
            {
                ddPreview.Items.Add(new ListItem("COSTING DETAIL", "3"));
            }
            ddPreview.Items.Add(new ListItem("PROFORMA INVOICE", "4"));
            if (variable.Carpetcompany == "1")
            {
                ddPreview.Items.Add(new ListItem("WEAVER WISE DETAIL", "5"));
            }
            //***********************Extra Pcs 
            if (variable.VarExtraPcsProduction == "1")
            {
                TDExtraflag.Visible = true;
                chkextraflag.Text = variable.VarExtraPcsPercentage + "% Extra Quantity";
            }
            //****
            if (Session["varcompanyid"].ToString() == "21")
            {
                DDOrderUnit.SelectedValue = "1";
                rdoPcWise.Checked = true;
                DDOrderUnit.Enabled = false;
            }
            if (Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "38" || Session["varcompanyid"].ToString() == "247")
            {
                ddPreview.Items.Add(new ListItem("BOM DETAIL", "6"));
            }

        }

    }
    protected void SelectedPaymentTerms()
    {
        string str = @"SELECT ByAirSea from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + @"
                        SELECT PaymentId from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + @"
                        SELECT TermId from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + @"
                        SELECT CurrencyId from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + @" ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDShipment.SelectedValue = ds.Tables[0].Rows[0]["ByAirSea"].ToString();
        }
        else
        {
            DDShipment.SelectedValue = "0";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlModeOfPayment.SelectedValue = ds.Tables[1].Rows[0]["PaymentId"].ToString();
        }
        else
        {
            ddlModeOfPayment.SelectedValue = "0";
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            ddlDeliveryTerms.SelectedValue = ds.Tables[2].Rows[0]["TermId"].ToString();
        }
        else
        {
            ddlDeliveryTerms.SelectedValue = "0";
        }
        if (ds.Tables[3].Rows.Count > 0)
        {
            DDCurrency.SelectedValue = ds.Tables[3].Rows[0]["CurrencyId"].ToString();
        }
        else
        {
            DDCurrency.SelectedValue = "0";
        }
    }
    protected void BindCustomerCode()
    {
        string CustomerCode = "CompanyName + SPACE(5)+Customercode";

        if (Session["varcompanyId"].ToString() == "20")
        {
            CustomerCode = "Customercode + SPACE(5)+ CompanyName";
        }
        string Str = @"SELECT CI.customerid," + CustomerCode + @" CompanyName 
            From customerinfo CI(nolock)
            JOIN CustomerUser CU ON CU.CustomerID = CI.CustomerID And CU.UserID = " + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + " order by CompanyName";

        if (Session["varcompanyId"].ToString() == "42")
        {
            Str = @"Select CI.CustomerID, " + CustomerCode + @" CompanyName 
            From Customerinfo CI(nolock)
            JOIN CompanyWiseCustomerDetail CII(nolock) ON CII.CustomerID = CI.CustomerID And CII.CompanyID = " + DDCompanyName.SelectedValue + @" 
            JOIN CustomerUser CU ON CU.CustomerID = CI.CustomerID And CU.UserID = " + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + " order by CompanyName";
        }

        UtilityModule.ConditionalComboFill(ref DDCustomerCode, Str, true, "--SELECT--");
    }
    protected void BindCategoryQuality()
    {
        if (Session["varcompanyId"].ToString() == "20")
        {
            DateTime dt = Convert.ToDateTime(TxtDeliveryDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtDeliveryDate.Text);
            DateTime dt2 = dt.AddDays(-14);
            TxtDispatchDate.Text = dt2.ToString("dd-MMM-yyyy");
        }
        else
        {
            TxtDispatchDate.Text = TxtDeliveryDate.Text;
        }

        if (CHKFORCURRENTCONSUMPTIONNew.Checked == true)
        {
            CHKFORCURRENTCONSUMPTION.Checked = true;
        }
        UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS Inner join ITEM_CATEGORY_MASTER IM on IM.Category_Id=CS.CategoryId And CS.Id=0 Inner join UserRights_Category UC on  IM.Category_Id=UC.Categoryid And UC.UserId=" + Session["varuserid"] + " And  IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME", true, "--SELECT--");
        if (DDItemCategory.Items.Count > 0)
        {
            DDItemCategory.SelectedIndex = 1;

            ddlcategorycange();
            fillCombo();
            // UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
            TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
        }

        UtilityModule.ConditionalComboFill(ref DDCurrency, "SELECT CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CurrencyName", true, "-SELECT-");

        if (Session["varcompanyId"].ToString() == "20")
        {
            UtilityModule.ConditionalComboFill(ref DDWareHouseName, "SELECT Warehouseid,WarehouseCode from WarehouseMaster Where CustomerId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Warehousename", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDWareHouseName, "SELECT Warehouseid,Warehousename from WarehouseMaster Where MasterCompanyId=" + Session["varCompanyId"] + " order by Warehousename", true, "--SELECT--");
        }

        if (Session["varcompanyId"].ToString() != "20")
        {
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCurrency.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT CurrencyId from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
            }
        }

        ////comment by sp
        if (ViewState["order_id"] == null || ViewState["order_id"].ToString() == "")
        {
            ViewState["order_id"] = "0";
        }
        TxtOrderID.Text = ViewState["order_id"].ToString();
        LblErrorMessage.Visible = false;
        DDItemName.Focus();
        if (Session["varcompanyno"].ToString() == "7" && DDOrderUnit.Items.Count > 0)
        {
            DDOrderUnit.SelectedValue = "4";
            if (DDItemCategory.Items.Count > 0)
            {
                DDItemCategory.SelectedIndex = 1;
                ddlcategorycange();
                if (DDItemName.Items.Count > 0)
                {
                    DDItemName.SelectedIndex = 1;
                }
                fillCombo();
                ItemSelectedChange();
                if (DDQuality.Items.Count > 0)
                {
                    DDQuality.SelectedIndex = 1;
                    QualitySelectedChange();
                    if (DDDesign.Items.Count > 0)
                    {
                        DDDesign.SelectedIndex = 1;
                    }
                    if (DDColor.Items.Count > 0)
                    {
                        DDColor.SelectedIndex = 1;
                    }
                    if (DDShape.Items.Count > 0)
                    {
                        DDShape.SelectedIndex = 1;
                    }
                    if (ddshadecolor.Items.Count > 0)
                    {
                        ddshadecolor.SelectedIndex = 1;
                    }
                }
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
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        CHKFORCURRENTCONSUMPTIONNew.Visible = false;
        btnupdateallconsmp.Visible = false;
        btnchngorderno.Visible = false;
        Tdcustordersearch.Visible = false;
        if (ChkEditOrder.Checked == true)
        {
            if (Session["usertype"].ToString() == "1")
            {
                btnchngorderno.Visible = true;
            }
            Chkbx.Checked = false;
            Chkbx.Enabled = false;
            Tdcustordersearch.Visible = true;

            ch.Visible = false;
            DDCustOrderNo.Visible = true;
            TxtCustOrderNo.Visible = false;
            TDDueDate.Visible = false;
            td.Visible = false;
            TDNewOrder.Visible = false;
            TDNewOrderNo.Visible = false;
            TxtNewOrderNo.Text = "";
            if (Session["varCompanyId"].ToString() == "20")
            {
                CHKFORCURRENTCONSUMPTIONNew.Visible = false;
                CHKFORCURRENTCONSUMPTION.Visible = false;
                btnupdateallconsmp.Visible = false;
            }
            else
            {
                CHKFORCURRENTCONSUMPTIONNew.Visible = true;
                CHKFORCURRENTCONSUMPTION.Visible = true;
                btnupdateallconsmp.Visible = true;
            }

            if (HDF1.Value == "7")
            {
                td.Visible = true;
                TDDueDate.Visible = true;
                TDNewOrder.Visible = true;
                TDNewOrderNo.Visible = true;
                CHKFORCURRENTCONSUMPTIONNew.Visible = false;
                CHKFORCURRENTCONSUMPTION.Visible = false;
                btnupdateallconsmp.Visible = false;
            }

            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "", true, "");

            BindCustomerCode();
            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "", true, "");

            UtilityModule.ConditionalComboFill(ref ddlModeOfPayment, "Select PaymentId, PaymentName from Payment P inner join OrderMaster OM on P.PaymentId=OM.PaymentMode And OM.customerid=" + DDCustomerCode.SelectedValue + "  order by P.PaymentName", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddlDeliveryTerms, "Select TermId,TermName from Term T inner join OrderMaster OM on T.TermId=OM.TermId And OM.customerid=" + DDCustomerCode.SelectedValue + "  order by T.TermName", true, "--SELECT--");


            //CHKFORCURRENTCONSUMPTION.Visible = true;

            //if (Session["varCompanyId"].ToString() == "21")
            //{
            //    if (DDCompanyName.Items.Count > 0)
            //    {
            //        DDCompanyName.SelectedIndex = 1;
            //    }
            //}
            //else
            //{
            //    DDCompanyName.SelectedIndex = 1;
            //}
            TxtCustOrderNo.Text = "";
            TxtLocalOrderNo.Text = "";
            TxtOrderDate.Text = "";
            TxtDeliveryDate.Text = "";
            TxtDueDate.Text = "";
            Txtcustorderdt.Text = "";
            TxtInspectionDate.Text = "";
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                ddcustomer_selectedchange();
            }
        }
        else
        {
            TDNewOrder.Visible = false;
            TDNewOrderNo.Visible = false;
            TDDueDate.Visible = false;
            td.Visible = false;
            if (HDF1.Value == "7")
            {
                TDDueDate.Visible = true;
                td.Visible = true;
                TDNewOrder.Visible = true;
                TDNewOrderNo.Visible = true;
                CHKFORCURRENTCONSUMPTION.Visible = false;
            }
            CHKFORCURRENTCONSUMPTION.Visible = false;
            btnupdateallconsmp.Visible = false;

            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "SELECT OrderId,CustomerOrderNo from OrderMaster Where status=0 and customerid=" + DDCustomerCode.SelectedValue + " order by CustomerOrderNo", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "", true, "");

            BindCustomerCode();
            UtilityModule.ConditionalComboFill(ref ddlModeOfPayment, "Select PaymentId, PaymentName from Payment P inner join OrderMaster OM on P.PaymentId=OM.PaymentMode And OM.customerid=" + DDCustomerCode.SelectedValue + "  order by P.PaymentName", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddlDeliveryTerms, "Select TermId,TermName from Term T inner join OrderMaster OM on T.TermId=OM.TermId And OM.customerid=" + DDCustomerCode.SelectedValue + "  order by T.TermName", true, "--SELECT--");
            Chkbx.Checked = false;
            Chkbx.Enabled = false;
            ch.Visible = false;
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtCustOrderNo.Text = "";
            TxtLocalOrderNo.Text = "";
            DDCustOrderNo.Visible = false;
            TxtCustOrderNo.Visible = true;
            TxtNewOrderNo.Text = "";
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                ddcustomer_selectedchange();
            }

            DGOrderDetail.DataSource = null;
            DGOrderDetail.DataBind();

            DGOrderDetail2.DataSource = null;
            DGOrderDetail2.DataBind();

            TxtOrderArea.Text = "";
            TxtTotalAmount.Text = "";
            TxtTotalQtyRequired.Text = "";
            ViewState["order_id"] = "0";
            if (ViewState["order_id"] == null || ViewState["order_id"].ToString() == "")
            {
                ViewState["order_id"] = "0";
            }
            LblErrorMessage.Text = "";
        }
    }
    //****************************************************************************************************************************
    private void Save_label(int Orderid, SqlTransaction Tran)
    {
        int itemid = 0;
        int n = Gvchklist.Rows.Count;
        SqlParameter[] _arrPara = new SqlParameter[2];
        _arrPara[0] = new SqlParameter("@Itemid", SqlDbType.Int);
        _arrPara[1] = new SqlParameter("@Orderid", SqlDbType.Int);
        _arrPara[1].Value = Orderid;
        for (int i = 0; i < n; i++)
        {
            GridViewRow row = Gvchklist.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
            if (isChecked == true)
            {
                itemid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
                _arrPara[0].Value = itemid;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_labelorderinsert", _arrPara);
            }
        }
    }
    //*************************
    protected void DDCustOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        int orderNo;
        ViewState["order_id"] = Convert.ToInt32(DDCustOrderNo.SelectedValue);
        TxtCustomerID.Text = DDCustomerCode.SelectedValue;
        OldCustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CustomerOrderNo From OrderMaster where  OrderId=" + DDCustOrderNo.SelectedValue + ""));
        hncustomerorderNo.Value = OldCustOrderNo;
        getOrderDetail();
        if (ChkEditOrder.Checked == true)
        {
            Fill_Grid();
            Fill_chelist();
            // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyId"].ToString() == "7")
            {
                str = @"SELECT Isnull(SUBSTRING(LocalOrder,CHARINDEX('.',LocalOrder)+1,CHARINDEX('.',LocalOrder)),'') As OrderNo from OrderMaster
                      WHERE  ORDERID=" + DDCustOrderNo.SelectedValue;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["OrderNo"] != "")
                    {
                        orderNo = Convert.ToInt32(ds.Tables[0].Rows[0]["OrderNo"].ToString()) + 1;
                        TxtNewOrderNo.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select LEFT(LocalOrder,CHARINDEX('.',LocalOrder)) from ordermaster where  orderid=" + DDCustOrderNo.SelectedValue + "").ToString();
                        TxtNewOrderNo.Text = TxtNewOrderNo.Text + orderNo;
                    }
                    else
                    {
                        TxtNewOrderNo.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select LocalOrder+'.1' from ordermaster where  orderid=" + DDCustOrderNo.SelectedValue + "").ToString();
                    }
                }
            }
            else
            {
                TxtNewOrderNo.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select LocalOrder from ordermaster where  orderid=" + DDCustOrderNo.SelectedValue + "").ToString();
            }
            if (Gvchklist.Rows.Count > 0)
            {
                Check_mark();
            }


        }
    }
    //****************************************************************************************************************************
    //**********************
    private void getOrderDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = @"SELECT OM.Orderid,LocalOrder,IsNull(OrderUnitId,0) OrderUnitId,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,
                    replace(convert(varchar(11),OM.DispatchDate,106), ' ','-') as DispatchDate,OrderCategoryId,replace(convert(varchar(11),Custorderdate,106), ' ','-') as Custorderdate,
                    replace(convert(varchar(11),duedate,106), ' ','-') as duedate,replace(convert(varchar(11),InspectionDate,106), ' ','-') as InspectionDate,om.Remarks,
                    OD.LessDiscount,OM.PaymentMode,OM.TermId,OD.TransmodeId,OD.CurrencyId,isnull(Extrapcs_percentage,0) as Extrapcs_percentage,ordercaltype, 
                    replace(convert(varchar(11),OM.InspectionDateNew,106), ' ','-') InspectionDateNew, InspectionQtyNew, 
                    replace(convert(varchar(11),OM.FinalInspectionDateNew,106), ' ','-') FinalInspectionDateNew, FinalInspectionQtyNew 
                    From OrderMaster OM 
                    Left Outer Join OrderDetail OD ON OM.OrderId=OD.OrderId Where  OM.OrderId=" + DDCustOrderNo.SelectedValue;
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["order_id"] = Convert.ToInt32(ds.Tables[0].Rows[0]["orderid"]);
            TxtLocalOrderNo.Text = ds.Tables[0].Rows[0]["LocalOrder"].ToString();
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "SELECT UnitId,UnitName from Unit order by UnitName", true, "--SELECT--");
            DDOrderUnit.SelectedValue = ds.Tables[0].Rows[0]["OrderUnitId"].ToString();
            enabledisbalesizetype();
            TxtOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
            TxtDispatchDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
            Txtcustorderdt.Text = ds.Tables[0].Rows[0]["Custorderdate"].ToString();
            TxtDueDate.Text = ds.Tables[0].Rows[0]["duedate"].ToString();
            if (ds.Tables[0].Rows[0]["InspectionDate"].ToString() == "")
            {
                TxtInspectionDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                TxtInspectionDate.Text = ds.Tables[0].Rows[0]["InspectionDate"].ToString();
            }
            rdoUnitWise.Checked = true;

            if (TrInspectionDate.Visible == true)
            {
                TxtInspectionDateNew.Text = ds.Tables[0].Rows[0]["InspectionDateNew"].ToString();
                TxtInspectionQtyNew.Text = ds.Tables[0].Rows[0]["InspectionQtyNew"].ToString();
                TxtFinalInspectionDateNew.Text = ds.Tables[0].Rows[0]["FinalInspectionDateNew"].ToString();
                TxtFinalInspectionQtyNew.Text = ds.Tables[0].Rows[0]["FinalInspectionQtyNew"].ToString();
            }

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderCalType"]) == 1)
            {
                rdoUnitWise.Checked = false;
                rdoPcWise.Checked = true;
            }
            else
            {
                rdoUnitWise.Checked = true;
                rdoPcWise.Checked = false;
            }
            txtmastremark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            Txtdiscount.Text = ds.Tables[0].Rows[0]["LessDiscount"].ToString();
            DDShipment.SelectedValue = ds.Tables[0].Rows[0]["TransmodeId"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            UtilityModule.ConditionalComboFill(ref ddordertype, "select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory", true, "Select OrderCategory");
            ddordertype.SelectedValue = ds.Tables[0].Rows[0]["OrderCategoryId"].ToString();
            UtilityModule.ConditionalComboFill(ref ddlModeOfPayment, "Select PaymentId, PaymentName from Payment P  order by P.PaymentName", true, "--SELECT--");

            ddlModeOfPayment.SelectedValue = ds.Tables[0].Rows[0]["PaymentMode"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["PaymentMode"].ToString();
            UtilityModule.ConditionalComboFill(ref ddlDeliveryTerms, "Select TermId,TermName from Term T  order by T.TermName", true, "--SELECT--");
            ddlDeliveryTerms.SelectedValue = ds.Tables[0].Rows[0]["TermId"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["TermId"].ToString();

            if (Convert.ToInt16(ds.Tables[0].Rows[0]["Extrapcs_percentage"]) > 0)
            {
                chkextraflag.Checked = true;
                chkextraflag.Text = ds.Tables[0].Rows[0]["Extrapcs_Percentage"].ToString() + "% Extra Quantity";
            }
            else
            {
                chkextraflag.Checked = false;
                if (TDExtraflag.Visible == true)
                {
                    chkextraflag.Text = variable.VarExtraPcsPercentage + "% Extra Quantity";
                }
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCustomerCode();
    }
    protected void TxtCustOrderNo_Validate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            Lblmessage.Visible = false;
            if (Session["varcompanyno"].ToString() == "7")
            {
                CustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(CustomerOrderNo,0) asd from OrderMaster om inner join orderdetail od on om.orderid=od.orderid and CustomerOrderNo='" + TxtCustOrderNo.Text + "'"));
            }
            else
            {
                CustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(CustomerOrderNo,0) asd from OrderMaster where  customerid=" + DDCustomerCode.SelectedValue + " and CustomerOrderNo='" + TxtCustOrderNo.Text + "'"));
            }
            if (CustOrderNo != "" && HDF1.Value != "7")
            {
                TxtCustOrderNo.Text = "";
                TxtLocalOrderNo.Text = "";
                Lblmessage.Text = "1";
                TxtCustOrderNo.Focus();
                Lblmessage.Visible = true;
                Lblmessage.Text = "Customer Order Number Already Exist......";
            }
            else if (HDF1.Value == "7")
            {
                if (CustOrderNo != "")
                {
                    TxtCustOrderNo.Focus();
                    Lblmessage.Visible = true;
                    Lblmessage.Text = "Customer Order Number Already Exist......";
                    HNLBL.Value = "0";
                }
                else
                {
                    Lblmessage.Visible = true;
                    Lblmessage.Text = "1";
                    HNLBL.Value = "1";
                    HNLBL.Visible = true;
                }
            }
            else
            {
                Lblmessage.Visible = false;
                Lblmessage.Text = "1";
                HNLBL.Value = "1";
                HNLBL.Visible = true;
            }
            if (Session["varcompanyId"].ToString() == "20")
            {
                TxtLocalOrderNo.Text = TxtCustOrderNo.Text;
            }
            else if (Session["varcompanyId"].ToString() == "30")
            {
                if (DDCompanyName.SelectedValue == "1")
                {
                    string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'L %'").ToString();
                    TxtLocalOrderNo.Text = "L " + Str;
                }
                else if (DDCompanyName.SelectedValue == "2")
                {
                    string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'LF ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'LF %'").ToString();
                    TxtLocalOrderNo.Text = "LF " + Str;
                }
            }
            else if (Session["varcompanyId"].ToString() == "37")
            {
                if (DDCompanyName.SelectedValue == "1")
                {
                    string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'SUN/L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'SUN/L %'").ToString();
                    TxtLocalOrderNo.Text = "SUN/L " + Str;
                }
                else if (DDCompanyName.SelectedValue == "2")
                {
                    string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'VI/L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'VI/L %'").ToString();
                    TxtLocalOrderNo.Text = "VI/L " + Str;
                }
            }
            else
            {
                string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'L %'").ToString();
                TxtLocalOrderNo.Text = "L " + Str;
            }
        }
        catch (Exception)
        {
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //**********************************Validate Date**********************************************************************
    protected void validdate_DeliveryDate()
    {
        if (Convert.ToDateTime(TxtOrderDate.Text) > Convert.ToDateTime(TxtDeliveryDate.Text))
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Order Date must be less then " + LblDelvDate.Text + "....";
            TxtDeliveryDate.Text = null;
            TxtDeliveryDate.Focus();
        }
        else
        {
            Lblmessage.Visible = false;
        }
    }
    protected void validate_DueDate()
    {
        if (Convert.ToDateTime(TxtDeliveryDate.Text) <= Convert.ToDateTime(TxtDueDate.Text))
        {
            Lblmessage.Visible = false;
        }
        //else if (Convert.ToDateTime(tbDeliveryDate.Text) <= Convert.ToDateTime(TxtDueDate.Text))
        //{
        //    Lblmessage.Visible = false;
        //}

        else
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Delivery Date must be less then Due date....";
            TxtDueDate.Text = null;
            TxtDueDate.Focus();
        }
    }
    protected void validate_DispatchDate()
    {
        if (Convert.ToDateTime(TxtDispatchDate.Text) > Convert.ToDateTime(TxtOrderDate.Text))
        {
            //if (Convert.ToDateTime(lblDeliveryDate.Text) < Convert.ToDateTime(TxtDispatchDate.Text))
            if (Convert.ToDateTime(TxtDeliveryDate.Text) < Convert.ToDateTime(TxtDispatchDate.Text))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Dispatch Date must be greater or equals to Order Date and less or equals to " + LblDelvDate.Text + "....";
                TxtDispatchDate.Text = null;
                TxtDispatchDate.Focus();
            }
            else
            {
                LblErrorMessage.Visible = false;
            }
        }
        else if (Convert.ToDateTime(TxtOrderDate.Text) > Convert.ToDateTime(TxtDispatchDate.Text))
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Dispatch Date must be greater or equals to Order Date and less or equals to " + LblDelvDate.Text + "....";
            TxtDispatchDate.Text = null;
            TxtDispatchDate.Focus();
        }
        else if (Convert.ToDateTime(TxtInspectionDate.Text) > Convert.ToDateTime(TxtDispatchDate.Text))
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Dispatch Date must be greater or equals to Inspection Date and less or equals to " + LblDelvDate.Text + "....";
            TxtDispatchDate.Text = null;
            TxtDispatchDate.Focus();
        }
        else
        {
            LblErrorMessage.Visible = false;
        }
    }
    //****************************************************************************************************************************
    //**************************TO Fill the OrderUnit*****************************************************************************
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategoryQuality();
        ddcustomer_selectedchange();
        TxtCustOrderNo.Focus();

        if (Session["varcompanyno"].ToString() == "20")
        {
            SelectedPaymentTerms();
        }
    }
    private void ddcustomer_selectedchange()
    {
        if (ChkEditOrder.Checked == true)
        {
            Chkbx.Enabled = false;
            if (Session["varcompanyno"].ToString() == "7")
            {
                if (variable.VarNewQualitySize == "1")
                {
                    UtilityModule.ConditionalComboFill(ref DDCustOrderNo, @"select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo from OrderMaster om left outer join orderdetail od On om.orderid=od.orderid left outer join V_FinishedItemDetailNew v On od.Item_Finished_Id= v.Item_Finished_Id left outer join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId
                    left outer join OrderProcessPlanning pm On om.orderid=pm.orderid Where om.status=0 and isnull(FinalStatus,0)<>1  and uc.userid=" + Session["varuserid"] + " and customerid=" + DDCustomerCode.SelectedValue + "  and om.companyId= " + DDCompanyName.SelectedValue + "", true, "--SELECT--");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDCustOrderNo, @"select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo from OrderMaster om left outer join orderdetail od On om.orderid=od.orderid left outer join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id left outer join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId
                    left outer join OrderProcessPlanning pm On om.orderid=pm.orderid Where om.status=0 and isnull(FinalStatus,0)<>1  and uc.userid=" + Session["varuserid"] + " and customerid=" + DDCustomerCode.SelectedValue + "  and om.companyId= " + DDCompanyName.SelectedValue + "", true, "--SELECT--");
                }
            }
            else
            {
                if (variable.Carpetcompany == "1")
                {
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "9":
                            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where status=0 and customerid=" + DDCustomerCode.SelectedValue + "  and companyId=" + DDCompanyName.SelectedValue + " and ORDERFROMSAMPLE=0", true, "--SELECT--");
                            break;
                        default:
                            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "select OrderId,CustomerOrderNo from OrderMaster Where status=0 and customerid=" + DDCustomerCode.SelectedValue + "  and companyId=" + DDCompanyName.SelectedValue + " and ORDERFROMSAMPLE=0 order by customerorderNo", true, "--SELECT--");
                            break;
                    }

                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where status=0 and customerid=" + DDCustomerCode.SelectedValue + "  and companyId=" + DDCompanyName.SelectedValue + " and ORDERFROMSAMPLE=0", true, "--SELECT--");
                }

            }
        }
        else
        {
            Chkbx.Enabled = true;
            Fill_chelist();
            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "", true, "");
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "select UnitId,UnitName from Unit Where MasterCompanyId=" + Session["varCompanyId"] + " order by UnitId", true, "--SELECT--");
            switch (Session["varcompanyId"].ToString())
            {
                case "7":
                    DDOrderUnit.SelectedIndex = 4;  //Dilli Karigari
                    break;
                case "14":
                    DDOrderUnit.SelectedIndex = 1;  //Dilli Karigari
                    break;
                case "27":
                    DDOrderUnit.SelectedIndex = 1;  //Antique Panipat
                    break;
                case "21":
                    DDOrderUnit.SelectedIndex = 1;  //Kaysons Agra
                    break;
                default:
                    DDOrderUnit.SelectedIndex = 2;
                    break;

            }
            enabledisbalesizetype();

        }
    }
    // ****************************************************************************************************************************
    //***********************************
    protected void DDOrderUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        enabledisbalesizetype();
        shapeselectedindexchange();
    }
    protected void enabledisbalesizetype()
    {
        DDsizetype.Enabled = true;
        if (Session["varcompanyId"].ToString() != "45")
        {
            switch (DDOrderUnit.SelectedValue.ToString())
            {
                case "1":
                    DDsizetype.Enabled = false;
                    DDsizetype.SelectedValue = "1";
                    break;
                case "2":
                    DDsizetype.Enabled = false;
                    DDsizetype.SelectedValue = "0";
                    break;
                default:
                    break;
            }
        }
    }
    //*************************TO Fill Item Name in In DropDown*******************************************************************
    protected void DDItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        fillCombo();
    }
    //******************************This Function Enter the Detail of Customer in data base***************************************
    //*******************************and Enable the customer to Fill the Order Detail*********************************************
    protected void BtnPlaceOrder_Click(object sender, EventArgs e)
    {
        {
            LblErrorMessage.Text = "";
            //PlaceOrder();
            if (ViewState["order_id"] == null || ViewState["order_id"].ToString() == "")
            {
                ViewState["order_id"] = "0";
            }
            // TxtCustomerID.Text = DDCustomerCode.SelectedValue;
            if (ChkEditOrder.Checked == true)
            {
                Fill_Grid();
            }
            //Fill_Grid();
            // FillTotal();

            if (Lblmessage.Text == "1" || Lblmessage.Visible == false)
            {
                DivCustomerDetail.Visible = false;
                ////comment by sp

                //DivOrderInfo.Visible = true;
                //LblMasterCompanyName.Text = DDCompanyName.SelectedItem.Text;
                //lblCustomerCode.Text = DDCustomerCode.SelectedItem.Text;
                if (ChkEditOrder.Checked == true)
                {
                    var index = DDCustOrderNo.SelectedItem.Text.Split('/');
                    if (index != null)
                    {
                        ////comment by sp
                        //lblCustOrderNo.Text = DDCustOrderNo.SelectedItem.Text.Split('/')[1].Trim();
                    }
                    else
                    {
                        ////comment by sp
                        // lblCustOrderNo.Text = DDCustOrderNo.SelectedItem.Text.Trim();
                    }
                }
                else
                {
                    ////comment by sp
                    //lblCustOrderNo.Text = TxtCustOrderNo.Text;
                }

                ////comment by sp
                //lblLocalOrderNo.Text = TxtLocalOrderNo.Text.ToUpper();

                //// lblOrderDate.Text = TxtOrderDate.Text;
                ////lblDeliveryDate.Text = TxtDeliveryDate.Text;

                ////comment by sp
                //tbOrderDate.Text = TxtOrderDate.Text; ;
                //tbDeliveryDate.Text = TxtDeliveryDate.Text;

                TxtDispatchDate.Text = TxtDeliveryDate.Text;
                ////comment by sp
                //LBLORDERTYPE.Text = ddordertype.SelectedItem.Text;
                //if (CHKFORCURRENTCONSUMPTIONNew.Checked == true)
                //{
                //    CHKFORCURRENTCONSUMPTION.Checked = true;
                //}
                //UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS Inner join ITEM_CATEGORY_MASTER IM on IM.Category_Id=CS.CategoryId And CS.Id=0 Inner join UserRights_Category UC on  IM.Category_Id=UC.Categoryid And UC.UserId=" + Session["varuserid"] + " And  IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME", true, "--SELECT--");
                //if (DDItemCategory.Items.Count > 0)
                //{
                //    DDItemCategory.SelectedIndex = 1;
                //    ddlcategorycange();
                //    fillCombo();
                //    // UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
                //    TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
                //}
                //UtilityModule.ConditionalComboFill(ref DDWareHouseName, "SELECT Warehouseid,Warehousename from WarehouseMaster Where MasterCompanyId=" + Session["varCompanyId"] + " order by Warehousename", true, "--SELECT--");
                //UtilityModule.ConditionalComboFill(ref DDCurrency, "SELECT CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CurrencyName", true, "-SELECT-");
                //DDCurrency.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT CurrencyId from customerinfo where CustomerId=" + DDCustomerCode.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
                //////comment by sp
                //// TxtOrderID.Text = ViewState["order_id"].ToString();
                //LblErrorMessage.Visible = false;
                //DDItemName.Focus();
                //if (Session["varcompanyno"].ToString() == "7" && DDOrderUnit.Items.Count > 0)
                //{
                //    DDOrderUnit.SelectedValue = "4";
                //    if (DDItemCategory.Items.Count > 0)
                //    {
                //        DDItemCategory.SelectedIndex = 1;
                //        ddlcategorycange();
                //        if (DDItemName.Items.Count > 0)
                //        {
                //            DDItemName.SelectedIndex = 1;
                //        }
                //        fillCombo();
                //        ItemSelectedChange();
                //        if (DDQuality.Items.Count > 0)
                //        {
                //            DDQuality.SelectedIndex = 1;
                //            QualitySelectedChange();
                //            if (DDDesign.Items.Count > 0)
                //            {
                //                DDDesign.SelectedIndex = 1;
                //            }
                //            if (DDColor.Items.Count > 0)
                //            {
                //                DDColor.SelectedIndex = 1;
                //            }
                //            if (DDShape.Items.Count > 0)
                //            {
                //                DDShape.SelectedIndex = 1;
                //            }
                //            if (ddshadecolor.Items.Count > 0)
                //            {
                //                ddshadecolor.SelectedIndex = 1;
                //            }
                //        }
                //    }
                //}

                ////tdbtn11.Style.Add("display", "");
                ////trmaster.Style.Add("DISPLAY", "none");
            }
        }
    }
    //*********************************************************************************************************
    //*****************Function to Enter theCustomer information in the Data Base******************************
    protected void PlaceOrder()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (ChkEditOrder.Checked == false)
            {
                ValidateCustOrderNo();

            }
            if (Lblmessage.Visible == false)
            {
                SqlParameter[] _arrpara = new SqlParameter[14];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@CustomerOrderNo", SqlDbType.NVarChar);
                _arrpara[4] = new SqlParameter("@LocalOrder", SqlDbType.NVarChar);
                //_arrpara[5] = new SqlParameter("@OrderUnitId", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                _arrpara[6] = new SqlParameter("@DispatchDate", SqlDbType.DateTime);
                _arrpara[7] = new SqlParameter("@DueDate", SqlDbType.DateTime);
                //_arrpara[9] = new SqlParameter("@OrderType", SqlDbType.Text);
                _arrpara[8] = new SqlParameter("@Id", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@ordercategory", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@Custorderdate", SqlDbType.DateTime);
                _arrpara[11] = new SqlParameter("@InspectionDate", SqlDbType.SmallDateTime);
                _arrpara[12] = new SqlParameter("Remark", SqlDbType.NVarChar, 1000);
                _arrpara[13] = new SqlParameter("VarCompanyNo", SqlDbType.Int);
                if (ChkEditOrder.Checked == false)
                {
                    ViewState["order_id"] = 0;
                    CustOrderNo = Convert.ToString(TxtCustOrderNo.Text);
                }
                else
                {
                    CustOrderNo = Convert.ToString(DDCustOrderNo.SelectedItem.Text);
                }
                _arrpara[0].Value = ViewState["order_id"];
                _arrpara[1].Value = DDCustomerCode.SelectedValue;
                _arrpara[2].Value = DDCompanyName.SelectedValue;
                _arrpara[3].Value = CustOrderNo.ToUpper();
                _arrpara[4].Value = TxtLocalOrderNo.Text.ToUpper();
                // //_arrpara[5].Value = DDOrderUnit.SelectedValue;

                _arrpara[5].Value = TxtOrderDate.Text;
                _arrpara[6].Value = TxtDeliveryDate.Text;

                ////comment by sp
                //_arrpara[5].Value = tbOrderDate.Text;
                //_arrpara[6].Value = tbDeliveryDate.Text;
                if (HDF1.Value == "7")
                {
                    _arrpara[7].Value = TxtDueDate.Text;
                }
                else
                {
                    _arrpara[7].Value = DateTime.Now;
                }
                _arrpara[8].Direction = ParameterDirection.Output;
                _arrpara[9].Value = ddordertype.SelectedValue;
                _arrpara[10].Value = Txtcustorderdt.Text;
                if (TxtInspectionDate.Visible == true)
                    _arrpara[11].Value = TxtInspectionDate.Text;
                else
                    _arrpara[11].Value = DateTime.Now.Date;

                _arrpara[12].Value = txtmastremark.Text;
                _arrpara[13].Value = Session["varcompanyno"];
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PlaceOrder", _arrpara);
                ViewState["order_id"] = _arrpara[8].Value;
                HttpCookie AA = new HttpCookie("ck");
                AA.Value = ViewState["order_id"].ToString();
                if (Chkbx.Checked == true)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from Labelorder where Orderid=" + ViewState["order_id"]);
                    Save_label(Convert.ToInt32(ViewState["order_id"]), Tran);
                }
                if (CHKFORCURRENTCONSUMPTIONNew.Checked == true)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete ORDER_CONSUMPTION_DETAIL Where OrderId=" + ViewState["order_id"]);
                    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select OrderId,OrderDetailId,Item_Finished_Id From OrderDetail Where OrderId=" + ViewState["order_id"]);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(Ds.Tables[0].Rows[i]["Item_Finished_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["OrderId"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["OrderDetailId"]), 1, CHKFORCURRENTCONSUMPTIONNew.Checked == true ? 1 : 0, Tran, effectivedate: TxtOrderDate.Text);
                        }
                    }
                }
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //****************************  Check Duplicate OrderNo And Local Order No ******************************
    private void ValidateCustOrderNo()
    {
        DataSet ds = null;
        string strsql = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (Session["varcompanyno"].ToString() == "7")
        {
            strsql = @"select isnull(CustomerOrderNo,0) asd from OrderMaster om inner join orderdetail od on om.orderid=od.orderid and CustomerOrderNo='" + TxtCustOrderNo.Text + "'";
        }
        else
        {
            strsql = @"SELECT * from OrderMaster Where  CustomerOrderNo='" + TxtCustOrderNo.Text + "'";
        }
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Customer Order No AllReady Exists.......";
            TxtCustOrderNo.Text = "";
            TxtCustOrderNo.Focus();
        }
        else
        {
            Lblmessage.Visible = false;
        }
        strsql = @"SELECT * from OrderMaster Where  LocalOrder='" + TxtLocalOrderNo.Text + "'";
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Local Order No AllReady Exits.......";
            //TxtCustOrderNo.Text = "";
            TxtLocalOrderNo.Focus();
        }
        else
        {
            Lblmessage.Visible = false;
        }
    }
    //*************************Function to Enter the Order Detail In the Database************************************
    protected void EnterOrder()
    {
        string sp = string.Empty;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            //Lblmessage.Text = "";
            LblErrorMessage.Text = "";
            LblErrorMessage.Visible = false;
            int VARUPDATE_FLAG = 0;
            SqlParameter[] _arrpara = new SqlParameter[67];
            _arrpara[0] = new SqlParameter("@ItemFinishedId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@OrderId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ItemId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[4] = new SqlParameter("@Quantity", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@Price", SqlDbType.Float);
            _arrpara[6] = new SqlParameter("@WhereHouseid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@ArticalNo", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@DispatchDate", SqlDbType.DateTime);
            _arrpara[9] = new SqlParameter("@WeavingInstruction", SqlDbType.NVarChar);
            _arrpara[10] = new SqlParameter("@FinishingInstruction", SqlDbType.NVarChar);
            _arrpara[11] = new SqlParameter("@DyeingInstruction", SqlDbType.NVarChar);
            _arrpara[12] = new SqlParameter("@TotalAmount", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@QualityCodeId", SqlDbType.Int);
            _arrpara[15] = new SqlParameter("@CurrencyId", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@OrderDetaiID_New", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@UPCNO", SqlDbType.NVarChar);
            _arrpara[18] = new SqlParameter("@OurCode", SqlDbType.NVarChar);
            _arrpara[19] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar);
            _arrpara[20] = new SqlParameter("@Pro_Flag", SqlDbType.Int);
            _arrpara[21] = new SqlParameter("@remark", SqlDbType.NVarChar);
            _arrpara[22] = new SqlParameter("@PKGInstruction", SqlDbType.NVarChar);
            _arrpara[23] = new SqlParameter("@LBGInstruction", SqlDbType.NVarChar);
            _arrpara[24] = new SqlParameter("@LessAdvance", SqlDbType.Float);
            _arrpara[25] = new SqlParameter("@LessCommission", SqlDbType.Float);
            _arrpara[26] = new SqlParameter("@LessDiscount", SqlDbType.Float);
            _arrpara[27] = new SqlParameter("@OrderUnitId", SqlDbType.Int);
            _arrpara[28] = new SqlParameter("@OrderType", SqlDbType.Int);
            _arrpara[29] = new SqlParameter("@NewOrderNo", SqlDbType.NVarChar);
            _arrpara[30] = new SqlParameter("@OldOrderNo", SqlDbType.NVarChar);
            _arrpara[31] = new SqlParameter("@Mastercompany", SqlDbType.Int);
            _arrpara[32] = new SqlParameter("@TransModeid", SqlDbType.Int);

            //For Order Entry
            _arrpara[33] = new SqlParameter("@CustomerId", SqlDbType.Int);
            _arrpara[34] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[35] = new SqlParameter("@CustomerOrderNo", SqlDbType.VarChar, 100);
            _arrpara[36] = new SqlParameter("@LocalOrder", SqlDbType.VarChar, 100);
            _arrpara[37] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
            _arrpara[38] = new SqlParameter("@OrderDispatchDate", SqlDbType.DateTime);
            _arrpara[39] = new SqlParameter("@DueDate", SqlDbType.DateTime);

            _arrpara[40] = new SqlParameter("@ordercategory", SqlDbType.Int);
            _arrpara[41] = new SqlParameter("@Custorderdate", SqlDbType.DateTime);
            _arrpara[42] = new SqlParameter("@InspectionDate", SqlDbType.SmallDateTime);
            _arrpara[43] = new SqlParameter("@orderremark", SqlDbType.VarChar, 1000);
            _arrpara[44] = new SqlParameter("@ChkOrderNo", SqlDbType.TinyInt);
            _arrpara[45] = new SqlParameter("@flagsize", SqlDbType.TinyInt);
            _arrpara[46] = new SqlParameter("@CQID", SqlDbType.Int);
            _arrpara[47] = new SqlParameter("@DSRNO", SqlDbType.Int);
            _arrpara[48] = new SqlParameter("@CSRNO", SqlDbType.Int);
            _arrpara[49] = new SqlParameter("@WithBuyercode", SqlDbType.Int);
            _arrpara[50] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _arrpara[50].Direction = ParameterDirection.Output;

            _arrpara[51] = new SqlParameter("@PaymentMode", SqlDbType.Int);
            _arrpara[52] = new SqlParameter("@TermId", SqlDbType.Int);
            _arrpara[53] = new SqlParameter("@Extrapcs_Percentage", SqlDbType.Int);
            _arrpara[54] = new SqlParameter("@DEMOORDERWITHLOCALQDCS", SqlDbType.TinyInt);//DEMOORDERWITHLOCALQDCS
            _arrpara[55] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[56] = new SqlParameter("@SampleCode", SqlDbType.VarChar, 100);
            _arrpara[57] = new SqlParameter("@RecipeNameID", SqlDbType.Int);
            _arrpara[58] = new SqlParameter("@WareHouseNameId", SqlDbType.Int);

            _arrpara[59] = new SqlParameter("@InspectionDateNew", SqlDbType.DateTime);
            _arrpara[60] = new SqlParameter("@InspectionQtyNew", SqlDbType.Int);
            _arrpara[61] = new SqlParameter("@FinalInspectionDateNew", SqlDbType.DateTime);
            _arrpara[62] = new SqlParameter("@FinalInspectionQtyNew", SqlDbType.Int);
            _arrpara[63] = new SqlParameter("@Filler", SqlDbType.VarChar, 10);
            _arrpara[64] = new SqlParameter("@Merchandise", SqlDbType.VarChar, 100);
            _arrpara[65] = new SqlParameter("@Incharge", SqlDbType.VarChar, 100);
            _arrpara[66] = new SqlParameter("@extraqty", SqlDbType.Int);

            //************Buyercode
            if (variable.Withbuyercode == "1")
            {
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
                    }
                    else
                    {
                        ItemFinishedId = UtilityModule.getItemFinishedIdWithBuyercode(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
                    }
                }
                else
                {
                    ItemFinishedId = UtilityModule.getItemFinishedIdWithBuyercode(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
                }
            }
            else
            {
                ItemFinishedId = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
            }

            con.Open();
            _arrpara[0].Value = ItemFinishedId;
            _arrpara[1].Direction = ParameterDirection.InputOutput;
            _arrpara[1].Value = ViewState["order_id"];
            _arrpara[2].Value = DDItemName.SelectedValue;
            _arrpara[3].Value = (TxtArea.Text == "" ? 0 : Convert.ToDouble(TxtArea.Text));
            _arrpara[4].Value = TxtQuantity.Text;
            _arrpara[5].Value = TxtPrice.Text != "" ? Convert.ToDouble(TxtPrice.Text) : 0;
            if (DDWareHouseName.SelectedValue != "")
            {
                _arrpara[6].Value = Convert.ToInt32(DDWareHouseName.SelectedValue) > 0 ? Convert.ToInt32(DDWareHouseName.SelectedValue) : 0;
            }
            else
            {
                _arrpara[6].Value = 0;
            }
            _arrpara[7].Value = TxtArticleNo.Text.ToUpper();
            _arrpara[8].Value = (TxtDispatchDate.Text).ToString();
            _arrpara[9].Value = TxtWeavingInstructions.Text.ToUpper();
            _arrpara[10].Value = TxtFinishingInstructions.Text.ToUpper();
            _arrpara[11].Value = TxtDyeingInstructions.Text.ToUpper();
            _arrpara[15].Value = DDCurrency.SelectedValue != "" ? DDCurrency.SelectedValue : "0";
            _arrpara[16].Direction = ParameterDirection.Output;
            if (rdoUnitWise.Checked == true)
            {
                _arrpara[12].Value = Convert.ToDouble(_arrpara[3].Value) * Convert.ToDouble(_arrpara[4].Value) * (Convert.ToDouble(_arrpara[5].Value));
                _arrpara[28].Value = 0;
            }
            else
            {
                _arrpara[12].Value = Convert.ToDouble(_arrpara[4].Value) * (Convert.ToDouble(_arrpara[5].Value));
                _arrpara[28].Value = 1;
            }
            _arrpara[14].Value = DDItemCode.Visible == false ? "0" : DDItemCode.SelectedValue;
            if (Convert.ToInt32(ViewState["OrderDetailId"]) > 0 && ChkEditOrder.Checked == true)
            {
                _arrpara[13].Value = ViewState["OrderDetailId"];
                VARUPDATE_FLAG = 1;
                _arrpara[30].Value = OldCustOrderNo;
                _arrpara[29].Value = TxtNewOrderNo.Text;
            }
            else if (Convert.ToInt32(ViewState["OrderDetailId"]) > 0 && hnUpdateStatus.Value == "1" && ChkEditOrder.Checked == false)
            {
                _arrpara[13].Value = ViewState["OrderDetailId"];
                VARUPDATE_FLAG = 1;
                _arrpara[30].Value = OldCustOrderNo;
                _arrpara[29].Value = TxtLocalOrderNo.Text;
            }
            else
            {
                _arrpara[13].Value = OrderDetailId;
                _arrpara[29].Value = TxtNewOrderNo.Text;
                _arrpara[30].Value = OldCustOrderNo;
            }
            _arrpara[17].Value = TXTUPCNO.Text;
            _arrpara[18].Value = TXTOURCODE.Text;
            _arrpara[19].Value = TXTBUYERCODE.Text;
            _arrpara[20].Value = 0;
            _arrpara[21].Value = txtremark.Text;
            _arrpara[22].Value = TxtPKGInstruction.Text;
            _arrpara[23].Value = TxtLBGInstruction.Text;
            _arrpara[24].Value = txtlessadv.Text != "" ? Convert.ToDouble(txtlessadv.Text) : 0;
            _arrpara[25].Value = txtcomm.Text != "" ? Convert.ToDouble(txtcomm.Text) : 0;
            _arrpara[26].Value = Txtdiscount.Text != "" ? Convert.ToDouble(Txtdiscount.Text) : 0;
            _arrpara[27].Value = DDOrderUnit.SelectedValue;
            _arrpara[31].Value = Session["VarcompanyNo"];
            _arrpara[32].Value = DDShipment.SelectedIndex < 0 ? "0" : DDShipment.SelectedValue;

            //For Order Entry
            _arrpara[33].Value = DDCustomerCode.SelectedValue;
            _arrpara[34].Value = DDCompanyName.SelectedValue;
            if (hnUpdateStatus.Value == "" || hnUpdateStatus.Value == null)
            {
                hnUpdateStatus.Value = "0";
            }
            if (ChkEditOrder.Checked == false && (hnUpdateStatus.Value == "1" || hnUpdateStatus.Value == "0"))
            {
                CustOrderNo = Convert.ToString(TxtCustOrderNo.Text);
            }
            else
            {
                //var index = DDCustOrderNo.SelectedItem.Text.Split('/');
                //if (index != null)
                //{
                //    CustOrderNo = DDCustOrderNo.SelectedItem.Text.Split('/')[1].Trim();
                //}
                //else
                //{
                //    CustOrderNo = DDCustOrderNo.SelectedItem.Text;
                //}
                CustOrderNo = hncustomerorderNo.Value;
            }
            _arrpara[35].Value = CustOrderNo;
            _arrpara[36].Value = TxtLocalOrderNo.Text;
            _arrpara[37].Value = TxtOrderDate.Text;
            _arrpara[38].Value = TxtDeliveryDate.Text;

            ////comment by sp
            //_arrpara[37].Value = tbOrderDate.Text;
            //_arrpara[38].Value = tbDeliveryDate.Text;
            if (HDF1.Value == "7")
            {
                _arrpara[39].Value = TxtDueDate.Text;
            }
            else
            {
                _arrpara[39].Value = DateTime.Now;
            }
            _arrpara[40].Value = ddordertype.SelectedValue;
            _arrpara[41].Value = Txtcustorderdt.Text;
            if (TxtInspectionDate.Visible == true)
            {
                _arrpara[42].Value = TxtInspectionDate.Text;
            }
            else
            {
                _arrpara[42].Value = DateTime.Now.Date;
            }

            _arrpara[43].Value = txtremark.Text;

            _arrpara[44].Direction = ParameterDirection.Output;
            _arrpara[45].Value = DDsizetype.SelectedValue;
            if (variable.Withbuyercode == "1")
            {
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        _arrpara[46].Value = 0;
                        _arrpara[47].Value = 0;
                        _arrpara[48].Value = 0;
                    }
                    else
                    {
                        _arrpara[46].Value = TDQuality.Visible == true ? DDQuality.SelectedValue : "0";
                        _arrpara[47].Value = TdDESIGN.Visible == true ? DDDesign.SelectedValue : "0";
                        _arrpara[48].Value = TDColor.Visible == true ? DDColor.SelectedValue : "0";
                    }
                }
                else
                {
                    _arrpara[46].Value = TDQuality.Visible == true ? DDQuality.SelectedValue : "0";
                    _arrpara[47].Value = TdDESIGN.Visible == true ? DDDesign.SelectedValue : "0";
                    _arrpara[48].Value = TDColor.Visible == true ? DDColor.SelectedValue : "0";
                }
            }
            else
            {
                _arrpara[46].Value = 0;
                _arrpara[47].Value = 0;
                _arrpara[48].Value = 0;
            }
            _arrpara[49].Value = variable.Withbuyercode;

            _arrpara[51].Value = ddlModeOfPayment.SelectedIndex < 0 ? "0" : ddlModeOfPayment.SelectedValue;
            _arrpara[52].Value = ddlDeliveryTerms.SelectedIndex < 0 ? "0" : ddlDeliveryTerms.SelectedValue;
            _arrpara[53].Value = chkextraflag.Checked == true ? variable.VarExtraPcsPercentage : "0";
            _arrpara[54].Value = ddordertype.SelectedValue == "2" ? (variable.VarDEMORDERWITHLOCALQDCS == "1" ? "1" : "0") : "0";
            _arrpara[55].Value = Session["varuserid"];
            _arrpara[56].Value = TDSampleCode.Visible == true ? TxtSampleCode.Text : "0";
            _arrpara[57].Value = TDDDRecipeName.Visible == true ? DDRecipeName.SelectedValue : "0";
            _arrpara[58].Value = TDWareHouseNameByCodeDDL.Visible == true ? DDWareHouseNameByCode.SelectedValue : "0";

            _arrpara[59].Value = TrInspectionDate.Visible == true ? (TxtInspectionDateNew.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtInspectionDateNew.Text) : DateTime.Now.ToString("dd-MMM-yyyy");
            _arrpara[60].Value = TrInspectionDate.Visible == true ? (TxtInspectionQtyNew.Text == "" ? "0" : TxtInspectionQtyNew.Text) : "0";
            _arrpara[61].Value = TrInspectionDate.Visible == true ? (TxtFinalInspectionDateNew.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtFinalInspectionDateNew.Text) : DateTime.Now.ToString("dd-MMM-yyyy");
            _arrpara[62].Value = TrInspectionDate.Visible == true ? (TxtFinalInspectionQtyNew.Text == "" ? "0" : TxtFinalInspectionQtyNew.Text) : "0";
            _arrpara[63].Value = tdfiller.Visible == true ? (ddfiller.SelectedItem.Text != "" ? ddfiller.SelectedItem.Text : "") : "";
            _arrpara[64].Value = tdmer.Visible == true ? (txtmer.Text == "" ? "" : txtmer.Text) : "";
            _arrpara[65].Value = tdincharge.Visible == true ? (txtincharge.Text == "" ? "0" : txtincharge.Text) : "0";
            _arrpara[66].Value = TDextraqty.Visible == true ? (txtextraqty.Text == "" ? "0" : txtextraqty.Text) : "0";
            //
            SqlTransaction tran = con.BeginTransaction();
            if (Session["varcompanyno"].ToString() == "44")
            {
                sp = "Pro_EnterOrderAGNI1";

            }
            else
            {

                sp = "Pro_EnterOrder1";

            }
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, sp, _arrpara);

            ViewState["OrderDetailId"] = _arrpara[16].Value.ToString() == "" ? "0" : _arrpara[16].Value;
            ViewState["order_id"] = _arrpara[1].Value;

            //TxtPrice.Text = "0";
            tran.Commit();
            //If localOrderNo or CustomerOrderNo allready exist
            //Lblmessage.Visible = true;
            if (_arrpara[50].Value.ToString() != "")
            {
                LblErrorMessage.Text = _arrpara[50].Value.ToString();
                LblErrorMessage.Visible = true;
                //ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('" + _arrpara[50].Value.ToString() + "');", true);
                return;
            }
            //else if (_arrpara[44].Value.ToString() == "2") //Item Already exists
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "alert1", "alert('Item already exists !!!');", true);
            //    return;
            //}
            //
            //******************************Start ForConsumptionUpdation*******************************//
            int UpdateCurrentConsumption = 0;
            if (ChkEditOrder.Checked == true && Convert.ToInt32(hnOldFinishedId.Value) > 0)
            {
                if (Convert.ToInt32(hnOldFinishedId.Value) != ItemFinishedId)
                {
                    UpdateCurrentConsumption = 1;
                }
            }

            if (CHKFORCURRENTCONSUMPTION.Checked == true)
            {
                UpdateCurrentConsumption = 1;
            }

            //******************************End ForConsumptionUpdation*******************************//

            UtilityModule.ORDER_CONSUMPTION_DEFINE(ItemFinishedId, Convert.ToInt32(ViewState["order_id"]), Convert.ToInt32(ViewState["OrderDetailId"]), VARUPDATE_FLAG, UpdateCurrentConsumption, effectivedate: TxtOrderDate.Text);
            //Update status
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Exec Pro_Updatestatus " + Session["varcompanyId"] + "," + Session["Varuserid"] + ",'Order_Consumption_Detail'," + ViewState["order_id"] + ",'Consumption Updated'");
            //
            //*****Auto Image save
            if (variable.VargetorderimagefromItemcode == "1" && hnorderphoto.Value == "")
            {
                string str = "select photo From MAIN_ITEM_IMAGE  Where FINISHEDID=" + ItemFinishedId + " And photo<>''";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string sourceimage = Server.MapPath(ds.Tables[0].Rows[0]["photo"].ToString());
                    string Destimage = Server.MapPath("~/ImageDraftorder/" + ViewState["OrderDetailId"] + "orderdetail.gif");

                    if (File.Exists(sourceimage))
                    {
                        System.IO.File.Copy(sourceimage, Destimage);
                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update orderdetail set photo='~/ImageDraftorder/" + ViewState["OrderDetailId"] + "orderdetail.gif' where orderdetailid=" + ViewState["OrderDetailId"] + "");
                    }
                }
            }
            hnorderphoto.Value = "";
            //***************
            refreshform();
            LblErrorMessage.Text = "Data saved successfully...";
            LblErrorMessage.Visible = true;
            hnOldFinishedId.Value = "0";
            // ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('Data saved successfully...');", true);

            //  FillTotal();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //*************************************************************************************************************
    //*********************************Function to Refresh the Order Detail******************************************
    private void FillTotal()
    {

        int varTotalPcs = 0;
        double varTotalArea = 0;
        double varTotalAmount = 0;
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            varTotalPcs = varTotalPcs + Convert.ToInt32(DGOrderDetail.Rows[i].Cells[2].Text);
            varTotalArea = varTotalArea + Convert.ToDouble(DGOrderDetail.Rows[i].Cells[3].Text);
            varTotalAmount = varTotalAmount + Convert.ToDouble(DGOrderDetail.Rows[i].Cells[5].Text);
        }
        TxtTotalQtyRequired.Text = varTotalPcs.ToString();

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            TxtOrderArea.Text = Math.Round(varTotalArea, 2).ToString();
        }
        else
        {
            TxtOrderArea.Text = varTotalArea.ToString();
        }

        TxtTotalAmount.Text = varTotalAmount.ToString();
    }
    private void refreshform()
    {
        LblErrorMessage.Visible = false;
        if (DDSize.Items.Count > 0)
        {
            DDSize.SelectedIndex = 0;
        }
        btnAddSize.Enabled = false;
        //DDWareHouseName.SelectedIndex = -1;
        TxtArea.Text = "";
        TxtProdCode.Text = "";
        TxtQuantity.Text = "";
        //TxtPrice.Text = "";
        TxtArticleNo.Text = "";
        // TxtDispatchDate.Text = "";
        TxtWeavingInstructions.Text = "";
        TxtFinishingInstructions.Text = "";
        TxtDyeingInstructions.Text = "";
        TxtTotalAmount.Text = "";
        TxtTotalQtyRequired.Text = "";
        TxtOrderArea.Text = "";
        txtremark.Text = "";

        TxtLBGInstruction.Text = "";
        TxtPKGInstruction.Text = "";
        ViewState["OrderDetailId"] = 0;
        hnUpdateStatus.Value = "0";
    }
    //****************************************************************************************************************************  
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemSelectedChange();
        if (DDItemCategory.SelectedIndex > 0 && DDItemName.SelectedIndex > 0 && Session["varcompanyno"].ToString() != "7")
        {
            BtnAddBuyerMasterCode.Visible = true;
        }
    }
    private void ItemSelectedChange()
    {
        string str = "";
        if (variable.Withbuyercode == "1")
        {
            //            UtilityModule.ConditionalComboFill(ref DDQuality, @"SELECT Distinct Q.QualityId,isnull(Cq.QualityNameAtoC,'') As QualityName
            //            from Quality Q Inner join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName", true, "--SELECT--");
            if (ddordertype.SelectedValue == "2")
            {
                if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                {
                    str = @"SELECT Distinct Q.QualityId,Q.QualityName As QualityName
                             from Quality Q Left Outer join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName";
                }
                else
                {
                    str = @"select CQ.SrNo,CQ.QualityNameAToC+' ['+Q.Qualityname+' ]'  as QualityNameAToC from CustomerQuality CQ  inner join Quality Q on CQ.QualityId=Q.QualityId 
                      and CQ.CustomerId=" + DDCustomerCode.SelectedValue + " and Q.Item_Id=" + DDItemName.SelectedValue + " and Q.MasterCompanyid=" + Session["varcompanyid"] + " and CQ.Enable_Disable=1 order by CQ.QualityNameAToC";
                }
            }
            else
            {
                str = @"select CQ.SrNo,CQ.QualityNameAToC+' ['+Q.Qualityname+' ]'  as QualityNameAToC from CustomerQuality CQ  inner join Quality Q on CQ.QualityId=Q.QualityId 
                      and CQ.CustomerId=" + DDCustomerCode.SelectedValue + " and Q.Item_Id=" + DDItemName.SelectedValue + " and Q.MasterCompanyid=" + Session["varcompanyid"] + " and CQ.Enable_Disable=1 order by CQ.QualityNameAToC";

            }
            UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, @"SELECT Distinct Q.QualityId,Q.QualityName As QualityName
                                                            from Quality Q Left Outer join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName", true, "--SELECT--");
        }
        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue;

        BtnAdd0.Focus();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        area();
        btnAddSize.Focus();
        if (Session["VarCompanyId"].ToString() == "30")
        {
            GetRate();
        }
    }
    //***********************************************************************************************************************
    //***********************Function to calculate the total area Of the Ordered Item**************************************** 
    private void area()
    {
        if (DDSize.SelectedIndex > 0)
        {
            if (Session["varcompanyId"].ToString() == "30")
            {
                string DDSizeValue = DDSize.SelectedItem.Text;
                string stringBeforeChar = DDSizeValue.Substring(0, DDSizeValue.IndexOf("["));

                string[] splitString = stringBeforeChar.Split('x');

                string Width = splitString[0].Trim();
                string Length = splitString[1].Trim();

                Check_Length_Width_Format(Length, Width);
            }
            else
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
                {
                    if (Session["varcompanyId"].ToString() == "9")
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT MtrArea from QualitySizeNew where sizeid=" + DDSize.SelectedValue + " "));
                        }
                        else
                        {
                            TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaINCH from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                        }


                        TxtArea.Text = Math.Round((Convert.ToDouble(TxtArea.Text) / 144), 3).ToString();
                    }
                    else
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT MtrArea from QualitySizeNew where sizeid=" + DDSize.SelectedValue + " "));

                            if (Session["VarCompanyNo"].ToString() == "43")
                            {
                                TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                            }
                        }
                        else
                        {
                            TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaINCH from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));

                            if (Session["VarCompanyNo"].ToString() == "43")
                            {
                                TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                            }
                        }
                    }
                }
                else if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT Export_Area from QualitySizeNew where sizeid=" + DDSize.SelectedValue + " "));

                        if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                        }
                    }
                    else
                    {
                        TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaFt from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));

                        if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                        }
                    }
                }
                else
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT MtrArea from QualitySizeNew where sizeid=" + DDSize.SelectedValue + " "));

                        if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                        }
                    }
                    else
                    {
                        TxtArea.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT AreaMtr from Size where sizeid=" + DDSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));

                        if (Session["VarCompanyNo"].ToString() == "43")
                        {
                            TxtArea.Text = Math.Round(Convert.ToDouble(TxtArea.Text), 2).ToString();
                        }
                    }
                }
                con.Close();
            }

        }
        else
        {
            TxtArea.Text = null;
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        QualitySelectedChange();
        FillDesign();
    }
    private void QualitySelectedChange()
    {
        if (ItemDescription.Visible == true)
        {

            UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
            TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue + "&Quality=" + DDQuality.SelectedValue;
        }
    }
    protected void FillDesign()
    {
        hnqualityid.Value = "0";
        hnCQsrno.Value = "0";
        string str = "";
        if (TdDESIGN.Visible == true)
        {
            if (variable.Withbuyercode == "1")
            {
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        //str = @"SELECT DISTINCT  DESIGNID,DESIGNNAME FROM V_FINISHEDITEMDETAIL WHERE ITEM_ID=" + DDItemName.SelectedValue + " AND DESIGNID>0 ";
                        //if (DDQuality.SelectedIndex > 0)
                        //{
                        //    str = str + " and QualityId=" + DDQuality.SelectedValue;
                        //}
                        //str = str + " order by designName";

                        str = @"SELECT DISTINCT  DESIGNID,DESIGNNAME FROM DESIGN Order By designName";

                    }
                    else
                    {
                        str = @"select Distinct CD.SrNo,Cd.DesignNameAToC+ ' [' + D.Designname +']' as DesignNameAToC From CustomerDesign CD inner join Design D on CD.DesignId=D.designId inner join ITEM_PARAMETER_MASTER IPM on D.designId=IPM.DESIGN_ID
                              Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and CD.Customerid=" + DDCustomerCode.SelectedValue + "  and CD.Enable_Disable=1";
                        if (TDQuality.Visible == true)
                        {
                            if (DDQuality.SelectedIndex > 0)
                            {
                                hnqualityid.Value = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select qualityid from customerquality Where Srno=" + DDQuality.SelectedValue + "").ToString();
                                if (variable.VarMasterBuyercodeSeqWise == "1")
                                {
                                    hnCQsrno.Value = DDQuality.SelectedValue;
                                    str = str + " and CD.CQSRNO=" + DDQuality.SelectedValue + "";
                                }
                                else
                                {
                                    str = str + " and IPM.Quality_ID=" + (hnqualityid.Value == "" ? "0" : hnqualityid.Value) + "";
                                }
                            }
                        }
                        str = str + " order by DesignNameAToC";
                    }
                }
                else
                {

                    str = @"select Distinct CD.SrNo,Cd.DesignNameAToC+ ' [' + D.Designname +']' as DesignNameAToC From CustomerDesign CD inner join Design D on CD.DesignId=D.designId inner join ITEM_PARAMETER_MASTER IPM on D.designId=IPM.DESIGN_ID
                              Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and CD.Customerid=" + DDCustomerCode.SelectedValue + "  and CD.Enable_Disable=1";
                    if (TDQuality.Visible == true)
                    {
                        if (DDQuality.SelectedIndex > 0)
                        {
                            hnqualityid.Value = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select qualityid from customerquality Where Srno=" + DDQuality.SelectedValue + "").ToString();
                            if (variable.VarMasterBuyercodeSeqWise == "1")
                            {
                                hnCQsrno.Value = DDQuality.SelectedValue;
                                str = str + " and CD.CQSRNO=" + DDQuality.SelectedValue + "";
                            }
                            else
                            {
                                str = str + " and IPM.Quality_ID=" + (hnqualityid.Value == "" ? "0" : hnqualityid.Value) + "";
                            }
                        }
                    }
                    str = str + " order by DesignNameAToC";
                }

                UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--SELECT");

            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName As Designname
                                                             from Design V Left Outer Join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
            }
        }
    }

    protected void fillColor()
    {
        hndesignid.Value = "0";
        string str = "";
        if (TDColor.Visible == true)
        {
            if (variable.Withbuyercode == "1")
            {
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        //str = @"SELECT DISTINCT  COLORID,COLORNAME FROM V_FINISHEDITEMDETAIL WHERE ITEM_ID=" + DDItemName.SelectedValue + " AND Colorid>0 ";
                        //if (DDQuality.SelectedIndex > 0)
                        //{
                        //    str = str + " and QualityId=" + DDQuality.SelectedValue;
                        //}
                        //if (DDDesign.SelectedIndex > 0)
                        //{
                        //    str = str + " and DesignId=" + DDDesign.SelectedValue;
                        //}
                        //str = str + " order by COLORNAME";

                        str = @"SELECT DISTINCT  COLORID,COLORNAME FROM COLOR ORDER BY COLORNAME";
                    }
                    else
                    {
                        str = @"select Distinct CC.SrNo,CC.ColorNameToC+ ' [' + C.Colorname +']' as ColorNameToC From CustomerColor CC inner join Color C on CC.Colorid=C.Colorid inner join ITEM_PARAMETER_MASTER IPM on C.Colorid=IPM.Color_ID
                              Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.Quality_ID=" + (hnqualityid.Value == "" ? "0" : hnqualityid.Value) + " and CC.Customerid=" + DDCustomerCode.SelectedValue + " and CC.Enable_disable=1";
                        if (TdDESIGN.Visible == true)
                        {
                            if (DDDesign.SelectedIndex > 0)
                            {
                                hndesignid.Value = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select designid from customerdesign where srno=" + DDDesign.SelectedValue + "").ToString();
                                if (variable.VarMasterBuyercodeSeqWise == "1")
                                {
                                    str = str + " and CC.CDSRNO=" + DDDesign.SelectedValue + "";
                                }
                                else
                                {
                                    str = str + " and IPM.Design_ID=" + (hndesignid.Value == "" ? "0" : hndesignid.Value) + "";
                                }
                            }
                        }
                        str = str + " order by ColorNameToC";
                    }
                }
                else
                {
                    str = @"select Distinct CC.SrNo,CC.ColorNameToC+ ' [' + C.Colorname +']' as ColorNameToC From CustomerColor CC inner join Color C on CC.Colorid=C.Colorid inner join ITEM_PARAMETER_MASTER IPM on C.Colorid=IPM.Color_ID
                              Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.Quality_ID=" + (hnqualityid.Value == "" ? "0" : hnqualityid.Value) + " and CC.Customerid=" + DDCustomerCode.SelectedValue + " and CC.Enable_disable=1";
                    if (TdDESIGN.Visible == true)
                    {
                        if (DDDesign.SelectedIndex > 0)
                        {
                            hndesignid.Value = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select designid from customerdesign where srno=" + DDDesign.SelectedValue + "").ToString();
                            if (variable.VarMasterBuyercodeSeqWise == "1")
                            {
                                str = str + " and CC.CDSRNO=" + DDDesign.SelectedValue + "";
                            }
                            else
                            {
                                str = str + " and IPM.Design_ID=" + (hndesignid.Value == "" ? "0" : hndesignid.Value) + "";
                            }
                        }
                    }
                    str = str + " order by ColorNameToC";
                }
                UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--SELECT");

            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName  As ColorName
                                                             from Color C Left Outer Join CustomerColor CC on C.ColorId=CC.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
            }
        }
    }

    //**********************Save Button Funcationality**************************************************************************
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (Session["varCompanyId"].ToString() == "44")
        {

            if (string.IsNullOrEmpty(txtmer.Text) || string.IsNullOrEmpty(txtincharge.Text))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Merchandiser & Production Incharge fields are mandatory!!!')", true);
                return;
            }

        }
        valdate_ArticalNo();
        if (LblErrorMessage.Text == "")
        {
            CHECKVALIDCONTROL();
        }
        if (LblErrorMessage.Text == "")
        {
            int ItemId = Convert.ToInt32(DDItemName.SelectedValue);
            // ItemFinishedId = Get_Finished_Item_Id(ItemId);//************Function Will Return the Finished Item Id Detail**********
            EnterOrder();
            Fill_Grid();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDOrderUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemCategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
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
        //if (UtilityModule.VALIDTEXTBOX(TxtPrice) == false)
        //{
        //    goto a;
        //}
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    private void Fill_Grid()
    {
        switch (HDF1.Value)
        {
            case "7":
                DGOrderDetail2.DataSource = GetDetail();
                DGOrderDetail2.DataBind();
                trgrid1.Visible = false;
                trgrid2.Visible = true;
                colour();
                break;
            case "3":
                DGOrderDetail2.DataSource = GetDetail();
                DGOrderDetail2.DataBind();
                trgrid1.Visible = false;
                trgrid2.Visible = true;
                colour();
                break;
            case "10":
                DGOrderDetail2.DataSource = GetDetail();
                DGOrderDetail2.DataBind();
                trgrid1.Visible = false;
                trgrid2.Visible = true;
                colour();
                break;
            case "12":
                DGOrderDetail.DataSource = GetDetail();
                DGOrderDetail.DataBind();

                trgrid2.Visible = false;
                trgrid1.Visible = true;
                break;
            default:
                // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select flagforsampleorder from Mastersetting");
                //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
                if (Flagforsampleorder == 1)
                {

                    if (ddordertype.SelectedValue == "2") // Sample
                    {
                        DGOrderDetail2.DataSource = GetDetail();
                        DGOrderDetail2.DataBind();
                        trgrid1.Visible = false;
                        trgrid2.Visible = true;
                        colour();
                    }
                    else
                    {
                        DGOrderDetail.DataSource = GetDetail();
                        DGOrderDetail.DataBind();

                        trgrid2.Visible = false;
                        trgrid1.Visible = true;
                    }
                }
                else
                {
                    DGOrderDetail.DataSource = GetDetail();
                    DGOrderDetail.DataBind();

                    trgrid2.Visible = false;
                    trgrid1.Visible = true;
                }
                break;
        }
        //in case of WithoutBOM
        if (Session["WithoutBOM"].ToString() == "1")
        {
            DGOrderDetail2.DataSource = GetDetail();
            DGOrderDetail2.DataBind();
            trgrid1.Visible = false;
            trgrid2.Visible = true;
            colour();
        }
        //

    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        string strsql = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            if (variable.Withbuyercode == "1")
            {
                if (variable.VarNewQualitySize == "1")
                {
                    strsql = @"SELECT Distinct ROW_NUMBER() over (order by OD.OrderDetailId ) as srno, OD.OrderDetailId as Sr_No,od.orderid,VF.ITEM_NAME AS ITEM_NAME,isnull(CQ.QualityNameAToC,'') as Quality,
                                isnull(CD.DesignNameAToC,'') as Design,isnull(CC.ColorNameToC,'') as Color,isnull(ShapeName,'') as Shape,
                                case when VF.MasterCompanyId=20  then case When OD.flagSize=0 Then vf.sizeft when Od.flagsize=1 then vf.sizemtr else vf.sizeinch end  else 
                                
                                case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end end as Size,
                                isnull(CQ.QualityNameAToC,'')+'  '+isnull(CD.DesignNameAToC,'')+'  '+isnull(CC.ColorNameToC,'')+'  '+isnull(ShapeName,'')+'  '+
                                case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Description,OD.QtyRequired*OD.TotalArea Area,OD.QtyRequired Qty,OD.UnitRate Rate,
                                Round(Od.Amount,2) Amount,Ci.CurrencyName,OD.ArticalNo,OD.WeavingInstruction,OD.FinishingInstructions,OD.DyeingInstructions,
                                od.Remark,od.PKGInstruction,od.LBGInstruction,Vf.Category_Name+' '+VF.ITEM_NAME +' '+QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'   '+isnull(ShapeName,'')+'  '+
                                isnull(CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else Case When OD.OrderUnitId=2 Then  
                                SizeFt Else case When OD.OrderUnitId=6 Then SizeInch End ENd End,'') Description1 ,
                                od.Item_Finished_Id as finished,PHOTO,IPM.productCode,
                                dbo.[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT](OD.orderid,OD.Item_Finished_Id) as Consmpflag,OD.flagSize
                                FROM OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                                 Inner JOIN V_FinishedItemDetailNew VF ON OD.Item_Finished_Id=VF.Item_Finished_Id 
                                 left join CustomerQuality CQ on OD.CQID=CQ.SrNo and CQ.CustomerId=OM.CustomerId
                                 left join CustomerDesign CD on OD.DSRNO=CD.SrNo and CD.CustomerId=OM.CustomerId
                                 left join CustomerColor CC on OD.CSRNO=CC.SrNo and CC.CustomerId=OM.CustomerId
                                 INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.Item_Finished_Id=IPM.Item_Finished_Id inner join sizetype sz on Od.flagsize=sz.val
                                 left outer join CurrencyInfo Ci on Ci.CurrencyId=Od.CurrencyId  
                            Where OD.OrderId=" + ViewState["order_id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " order by OD.OrderDetailId asc";
                }
                else
                {

                    strsql = @"SELECT Distinct ROW_NUMBER() over (order by OD.OrderDetailId ) as srno, OD.OrderDetailId as Sr_No,od.orderid,VF.ITEM_NAME AS ITEM_NAME,isnull(CQ.QualityNameAToC,'') as Quality,isnull(CD.DesignNameAToC,'') as Design,isnull(CC.ColorNameToC,'') as Color,isnull(ShapeName,'') as Shape,
                                case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Size,
                                isnull(CQ.QualityNameAToC,vf.QualityName)+'  '+isnull(CD.DesignNameAToC,vf.designName)+'  '+isnull(CC.ColorNameToC,vf.ColorName)+'  '+isnull(ShapeName,'')+'  '+
                            case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Description,OD.QtyRequired*OD.TotalArea Area,OD.QtyRequired Qty,OD.UnitRate Rate,
                            Round(Od.Amount,2) Amount,Ci.CurrencyName,OD.ArticalNo,OD.WeavingInstruction,OD.FinishingInstructions,OD.DyeingInstructions,
                            od.Remark,od.PKGInstruction,od.LBGInstruction,Vf.Category_Name+' '+VF.ITEM_NAME +' '+QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'   '+isnull(ShapeName,'')+'  '+
                            isnull(CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else Case When OD.OrderUnitId=2 Then  
                            SizeFt Else case When OD.OrderUnitId=6 Then SizeInch End ENd End,'') Description1 ,
                            od.Item_Finished_Id as finished,PHOTO,IPM.productCode,
                            dbo.[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT](OD.orderid,OD.Item_Finished_Id) as Consmpflag,OD.flagSize
                            FROM OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            Inner JOIN V_FinishedItemDetail VF ON OD.Item_Finished_Id=VF.Item_Finished_Id 
                            left join CustomerQuality CQ on OD.CQID=CQ.SrNo and CQ.CustomerId=OM.CustomerId
                            left join CustomerDesign CD on OD.DSRNO=CD.SrNo and CD.CustomerId=OM.CustomerId
                            left join CustomerColor CC on OD.CSRNO=CC.SrNo and CC.CustomerId=OM.CustomerId
                            INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.Item_Finished_Id=IPM.Item_Finished_Id inner join sizetype sz on Od.flagsize=sz.val
                            left outer join CurrencyInfo Ci on Ci.CurrencyId=Od.CurrencyId 
                            Where OD.OrderId=" + ViewState["order_id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " order by OD.OrderDetailId asc";

                }
            }
            else
            {
                if (variable.VarNewQualitySize == "1")
                {
                    strsql = @" SELECT Distinct ROW_NUMBER() over (order by OD.OrderDetailId ) as srno, OD.OrderDetailId as Sr_No,od.orderid,VF.ITEM_NAME AS ITEM_NAME,QualityName as Quality,
                                DesignName as Design,ColorName as Color,ShapeName as Shape,
                                case when VF.MasterCompanyId=20  then case When OD.flagSize=0 Then vf.sizeft when Od.flagsize=1 then vf.sizemtr else vf.sizeinch end  else 
                                
                                case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end end as Size,                         
                                QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShapeName,'')+'  '+
                                case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Description,OD.QtyRequired*OD.TotalArea Area,OD.QtyRequired Qty,OD.UnitRate Rate,
                              Round(Od.Amount,2) Amount,Ci.CurrencyName,OD.ArticalNo,OD.WeavingInstruction,OD.FinishingInstructions,OD.DyeingInstructions,
                                od.Remark,od.PKGInstruction,od.LBGInstruction,Vf.Category_Name+' '+VF.ITEM_NAME +' '+QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'   '+isnull(ShapeName,'')+'  '+
                             isnull(CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else Case When OD.OrderUnitId=2 Then  SizeFt Else case When OD.OrderUnitId=6 Then SizeInch End ENd End,'') Description1 ,od.Item_Finished_Id as finished,PHOTO,IPM.productCode,dbo.[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT](OD.orderid,OD.Item_Finished_Id) as Consmpflag,OD.flagSize
                             FROM OrderDetail OD Inner JOIN V_FinishedItemDetailNew VF ON OD.Item_Finished_Id=VF.Item_Finished_Id 
                            INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.Item_Finished_Id=IPM.Item_Finished_Id inner join sizetype sz on Od.flagsize=sz.val
                            left outer join CurrencyInfo Ci on Ci.CurrencyId=Od.CurrencyId  
                            Where OD.OrderId=" + ViewState["order_id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " order by OD.OrderDetailId asc";
                }
                else
                {
                    strsql = @"SELECT Distinct ROW_NUMBER() over (order by OD.OrderDetailId ) as srno, OD.OrderDetailId as Sr_No,od.orderid,VF.ITEM_NAME AS ITEM_NAME,QualityName as Quality,DesignName as Design,ColorName as Color,ShapeName as Shape,
                            case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Size,
                            QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShapeName,'')+'  '+
                             case When OD.flagSize=0 Then vf.sizeft+' '+sz.type when Od.flagsize=1 then vf.sizemtr+' '+sz.type else vf.sizeinch+' '+sz.type end as Description,OD.QtyRequired*OD.TotalArea Area,OD.QtyRequired Qty,OD.UnitRate Rate,
                              Round(Od.Amount,2) Amount,Ci.CurrencyName,OD.ArticalNo,OD.WeavingInstruction,OD.FinishingInstructions,OD.DyeingInstructions,
                            od.Remark,od.PKGInstruction,od.LBGInstruction,Vf.Category_Name+' '+VF.ITEM_NAME +' '+QualityName+'  '+isnull(DesignName,'')+'  '+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'   '+isnull(ShapeName,'')+'  '+
                             isnull(CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else Case When OD.OrderUnitId=2 Then  SizeFt Else case When OD.OrderUnitId=6 Then SizeInch End ENd End,'') Description1 ,od.Item_Finished_Id as finished,PHOTO,IPM.productCode,dbo.[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT](OD.orderid,OD.Item_Finished_Id) as Consmpflag,OD.flagSize
                             FROM OrderDetail OD Inner JOIN V_FinishedItemDetail VF ON OD.Item_Finished_Id=VF.Item_Finished_Id 
                            INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.Item_Finished_Id=IPM.Item_Finished_Id inner join sizetype sz on Od.flagsize=sz.val
                            left outer join CurrencyInfo Ci on Ci.CurrencyId=Od.CurrencyId 
                            Where OD.OrderId=" + ViewState["order_id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " order by OD.OrderDetailId asc";
                }
            }
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;

            if (Session["VarCompanyNo"].ToString() == "43")
            {
                TxtOrderArea.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("sum(Area)", "")), 2).ToString();
            }
            else
            {
                TxtOrderArea.Text = ds.Tables[0].Compute("sum(Area)", "").ToString();
            }


            TxtTotalAmount.Text = ds.Tables[0].Compute("sum(Amount)", "").ToString();
            TxtTotalQtyRequired.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
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

    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        shapeselectedindexchange();
        btnaddshape.Focus();
    }

    private void shapeselectedindexchange()
    {
        string size = "", ProdSize = "", BuyerSize = "";

        string str = "";

        if (variable.VarNewQualitySize == "1")
        {
            switch (Convert.ToInt16(DDsizetype.SelectedValue))
            {
                case 0:
                    size = "Export_Format";
                    ProdSize = "Production_Ft_Format";
                    BuyerSize = "SizeNameAtoc";
                    break;
                case 1:
                    size = "MtrSize";
                    ProdSize = "Production_Mt_Format";
                    BuyerSize = "MtSizeAtoC";
                    break;
                case 2:
                    size = "MtrSize";
                    ProdSize = "Production_Ft_Format";
                    BuyerSize = "inchSize";
                    break;
                default:
                    size = "Export_Format";
                    ProdSize = "Production_Ft_Format";
                    BuyerSize = "SizeNameAtoc";
                    break;
            }
        }
        else
        {

            switch (Convert.ToInt16(DDsizetype.SelectedValue))
            {
                case 0:
                    size = "Sizeft";
                    ProdSize = "ProdSizeft";
                    BuyerSize = "SizeNameAtoc";
                    break;
                case 1:
                    size = "Sizemtr";
                    ProdSize = "ProdSizemtr";
                    BuyerSize = "MtSizeAtoC";
                    break;
                case 2:
                    size = "Sizeinch";
                    ProdSize = "ProdSizeft";
                    BuyerSize = "inchSize";
                    break;
                default:
                    size = "Sizeft";
                    ProdSize = "ProdSizeft";
                    BuyerSize = "SizeNameAtoc";
                    break;
            }
        }
        //size Query
        if (variable.Withbuyercode == "1")
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "4":
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = "Select Distinct QSN.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" From QualitySizeNew QSN Inner Join CustomerSize CS on QSN.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @"
                      Where shapeid=" + DDShape.SelectedValue + " order by " + size + "";
                    }
                    else
                    {
                        str = "Select Distinct S.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" From Size S Inner Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @"
                      Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

                    }
                    break;
                default:
                    //case "4":
                    if (variable.VarNewQualitySize == "1")
                    {
                        if (Session["varcompanyId"].ToString() == "20")
                        {
                            str = @"select Distinct QSN.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join QualitySizeNew  QSN on
                          Im.QUALITY_ID=QSN.QualityId inner join CustomerSize CS on cs.Sizeid=QSN.SizeId
                          and Cs.CustomerId=" + DDCustomerCode.SelectedValue + " and QSN.shapeId=" + DDShape.SelectedValue + "  and Im.Item_Id=" + DDItemName.SelectedValue + " and IM.QUALITY_ID=" + hnqualityid.Value + " ";
                        }
                        else
                        {
                            str = @"select Distinct QSN.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join QualitySizeNew  QSN on
                          Im.SIZE_ID=QSN.SizeId inner join CustomerSize CS on cs.Sizeid=QSN.SizeId
                          and Cs.CustomerId=" + DDCustomerCode.SelectedValue + " and QSN.shapeId=" + DDShape.SelectedValue + "  and Im.Item_Id=" + DDItemName.SelectedValue + " and IM.QUALITY_ID=" + hnqualityid.Value + " ";
                        }
                    }
                    else
                    {
                        if (ddordertype.SelectedValue == "2")
                        {
                            if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                            {
                                str = @"select Distinct S.Sizeid,isnull(" + size + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                                      Im.SIZE_ID=S.SizeId and S.shapeId=" + DDShape.SelectedValue + "  and Im.Item_Id=" + DDItemName.SelectedValue + " and IM.QUALITY_ID=" + DDQuality.SelectedValue + "  and S.MasterCompanyId=" + Session["varCompanyId"];
                                str = str + " order by " + size;
                            }
                            else
                            {
                                str = @"select Distinct S.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                              Im.SIZE_ID=S.SizeId inner join CustomerSize CS on cs.Sizeid=S.SizeId
                              and Cs.CustomerId=" + DDCustomerCode.SelectedValue + " and S.shapeId=" + DDShape.SelectedValue + "  and Im.Item_Id=" + DDItemName.SelectedValue + " and IM.QUALITY_ID=" + hnqualityid.Value + "  and S.MasterCompanyId=" + Session["varCompanyId"];
                                if (TdDESIGN.Visible == true && DDDesign.SelectedIndex > 0)
                                {
                                    str = str + " And IM.Design_Id=" + hndesignid.Value;
                                }
                                if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
                                {
                                    string colorid = "0";
                                    colorid = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select colorid from customercolor where srno=" + DDColor.SelectedValue + " and customerid=" + DDCustomerCode.SelectedValue + "").ToString();
                                    str = str + " And IM.Color_Id=" + colorid;
                                }
                                str = str + " order by " + size;
                            }
                        }
                        else
                        {

                            str = @"select Distinct S.Sizeid,isnull(" + BuyerSize + ",'')+'  '+'['+" + ProdSize + "+']' As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                          Im.SIZE_ID=S.SizeId inner join CustomerSize CS on cs.Sizeid=S.SizeId
                          and Cs.CustomerId=" + DDCustomerCode.SelectedValue + " and S.shapeId=" + DDShape.SelectedValue + "  and Im.Item_Id=" + DDItemName.SelectedValue + " and IM.QUALITY_ID=" + hnqualityid.Value + "  and S.MasterCompanyId=" + Session["varCompanyId"];

                            if (TdDESIGN.Visible == true && DDDesign.SelectedIndex > 0)
                            {
                                str = str + " And IM.Design_Id=" + hndesignid.Value;
                            }
                            if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
                            {
                                string colorid = "0";
                                colorid = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select colorid from customercolor where srno=" + DDColor.SelectedValue + " and customerid=" + DDCustomerCode.SelectedValue + "").ToString();
                                str = str + " And IM.Color_Id=" + colorid;
                            }
                            str = str + " order by " + size;
                        }
                    }

                    break;
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                str = "Select Distinct QSN.Sizeid,QSN." + size + "+Space(2)+isnull(" + BuyerSize + ",'') As  " + size + " From QualitySizeNew QSN Left Outer Join CustomerSize CS on QSN.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @"
                 Where shapeid=" + DDShape.SelectedValue + " order by " + size + "";
            }
            else
            {

                if (Session["varcompanyId"].ToString() == "44")
                {
                    switch (DDsizetype.SelectedValue.ToString())
                    {
                        case "0":
                            str = "Select Distinct S.Sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @" Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by 1";
                            //str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + DDShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        case "1":
                            str = "Select Distinct S.Sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end as  Sizemtr From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @" Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by 1";
                            //str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end as  Sizemtr from Size S Where S.shapeid=" + DDShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                            break;
                        case "2":
                            str = "Select Distinct S.Sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else ''  end as  Sizeinch From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @" Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by 1";
                            //str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else ''  end as  Sizeinch from Size S  Where S.shapeid=" + DDShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        default:
                            str = "Select Distinct S.Sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @" Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by 1";
                            //str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + DDShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                    }


                }
                else
                {


                    str = "Select Distinct S.Sizeid,S." + size + " As  " + size + " From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId And CustomerId=" + DDCustomerCode.SelectedValue + @"
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";
                }
            }
        }
        //

        if (DDShape.SelectedIndex > 0)
        {
            //if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 6)
            //{
            //    UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Sizeinch", true, "--SELECT--");
            //}
            //else
            //{
            UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
            //  }
            btnAddSize.Enabled = true;
        }
        else
        {
            btnAddSize.Enabled = false;
        }
    }
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForEditGrid.Checked == true && (MasterCompanyId == 16 || MasterCompanyId == 28))
        {
            fillOrderBack();
        }
        if (MasterCompanyId != 16 && MasterCompanyId != 28)
        {
            fillOrderBack();
        }
    }
    //****************************************************************************************************************************
    //******************************** Function To fill the detail of the Order Back in the Form *********************************************
    private void fillOrderBack()
    {
        DataSet ds = null;
        try
        {
            ds = null;
            string sql;
            switch (HDF1.Value)
            {
                case "7":
                    sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,
                            Od.CurrencyId,Od.UnitRate,round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,
                            Od.FinishingInstructions,Od.DyeingInstructions,replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,
                            IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,
                            Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark ,
                            OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId, OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                            ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty
                            From OrderDetail Od,ordermaster om,ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID and 
                            Od.OrderDetailId=" + DGOrderDetail2.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
                case "3":
                    sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,Od.UnitRate,round(Od.Amount,2)Amount,
                            Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,Od.DyeingInstructions,
                            replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,IPM.SHAPE_ID,IPM.SIZE_ID,
                            IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,
                            isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId, OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                            ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty                            
                            From OrderDetail Od,ordermaster om,ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM Where  om.orderid=od.orderid and 
                            IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID and Od.OrderDetailId=" + DGOrderDetail2.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
                case "10":
                    sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,Od.UnitRate,
                            round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,Od.DyeingInstructions,
                            replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,
                            IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,
                            od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark , OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                            OD.RecipeNameID,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                            ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty
                            From OrderDetail Od,ordermaster om,ITEM_MASTER IM,
                            ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID 
                            and Od.OrderDetailId=" + DGOrderDetail2.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
                case "12":
                    sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,Od.UnitRate,
                            round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,Od.DyeingInstructions,
                            replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,
                            IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,
                            od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                            OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                            ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty                            
                            From OrderDetail Od,ordermaster om,ITEM_MASTER IM,
                            ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and  IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID 
                            and Od.OrderDetailId=" + DGOrderDetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
                case "44":
                    sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,
                                Od.UnitRate,round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,
                                Od.DyeingInstructions,replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,
                                IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,
                                od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                                OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId,ISNULL(OD.filler,2) AS filler,
                                ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty 
                                From OrderDetail Od,ordermaster om,ITEM_MASTER IM,
                                ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID and Od.OrderDetailId=" + DGOrderDetail.SelectedValue + @" 
                                And IM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
                default:
                    if (Flagforsampleorder == 1)
                    {
                        if (Convert.ToInt16(ddordertype.SelectedValue) == 2) //For sample
                        {
                            sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,Od.UnitRate,
                                     round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,
                                    Od.DyeingInstructions,replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,
                                    IPM.COLOR_ID,IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,
                                    od.BUYERCODE,od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                                    OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                                    ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty 
                                    From OrderDetail Od,ordermaster om,ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID 
                                    and Od.OrderDetailId=" + DGOrderDetail2.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                        }
                        else
                        {
                            sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,
                                    Od.UnitRate,round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,
                                    Od.DyeingInstructions,replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,
                                    IPM.COLOR_ID,IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE, 
                                    od.BUYERCODE,od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                                    OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                                    ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty 
                                    From OrderDetail Od,ordermaster om,
                                    ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID 
                                    and Od.OrderDetailId=" + DGOrderDetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                        }
                    }
                    else
                    {
                        sql = @"SELECT Od.OrderDetailId,Od.Item_Finished_id,Od.QualityCodeId,Od.Item_Id,IPM.ProductCode,Od.QtyRequired,Od.CurrencyId,
                                Od.UnitRate,round(Od.Amount,2)Amount,Od.TotalArea,Od.Warehouseid,Od.ArticalNo,Od.WeavingInstruction,Od.FinishingInstructions,
                                Od.DyeingInstructions,replace(convert(varchar(11),Od.DispatchDate,106), ' ','-') as DispatchDate,IM.CATEGORY_ID,IPM.QUALITY_ID,IPM.DESIGN_ID,IPM.COLOR_ID,
                                IPM.SHAPE_ID,IPM.SIZE_ID,IPM.SHADECOLOR_ID,Tag_Flag,OrderCalType,OrderUnitId,Isnull(Transmodeid,0) As Transmodeid,od.OURCODE,od.BUYERCODE,
                                od.flagsize,OD.CQID,OD.DSRNO,OD.CSRNO,isnull(od.photo,'') as Photo,ISNULL(od.remark,'') as remark, OM.DEMOORDERWITHLOCALQDCS,OM.OrderCategoryId,
                                OD.RecipeNameID ,isnull(OD.WareHouseNameId,0) as WareHouseNameId, 
                                ISNULL(OD.Merchandise,'') Merchandise,ISNULL(OD.Incharge,'') Incharge,isnull(od.extraqty,0) extraqty 
                                From OrderDetail Od,ordermaster om,ITEM_MASTER IM,
                                ITEM_PARAMETER_MASTER IPM Where om.orderid=od.orderid and IPM.Item_Finished_Id=Od.Item_Finished_Id and IM.ITEM_ID=Od.ITEM_ID and Od.OrderDetailId=" + DGOrderDetail.SelectedValue + @" 
                                And IM.MasterCompanyId=" + Session["varCompanyId"];
                    }
                    break;
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnUpdateStatus.Value = "1";
                OrderDetailId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrderDetailId"]);
                ViewState["OrderDetailId"] = OrderDetailId;
                hnOldFinishedId.Value = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                rdoUnitWise.Checked = true;
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderCalType"]) == 1)
                {
                    rdoUnitWise.Checked = false;
                    rdoPcWise.Checked = true;
                }
                else
                {
                    rdoUnitWise.Checked = true;
                    rdoPcWise.Checked = false;
                }
                DDOrderUnit.SelectedValue = ds.Tables[0].Rows[0]["OrderUnitId"].ToString();
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                fillCombo();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
                ItemSelectedChange();
                if (variable.Withbuyercode == "1")
                {
                    if (ds.Tables[0].Rows[0]["OrderCategoryId"].ToString() == "2")
                    {
                        if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                        {
                            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                        }
                        else
                        {
                            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["CQID"].ToString();
                            hnqualityid.Value = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                        }
                    }
                    else
                    {
                        DDQuality.SelectedValue = ds.Tables[0].Rows[0]["CQID"].ToString();
                        hnqualityid.Value = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                    }
                }
                else
                {
                    DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                }
                if (ItemDescription.Visible == true)
                {
                    QualitySelectedChange();
                    DDItemCode.SelectedValue = ds.Tables[0].Rows[0]["QualityCodeId"].ToString();
                }
                if (TdDESIGN.Visible == true)
                {
                    if (variable.Withbuyercode == "1")
                    {
                        if (ds.Tables[0].Rows[0]["OrderCategoryId"].ToString() == "2")
                        {
                            if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                            {
                                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                            }
                            else
                            {
                                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Dsrno"].ToString();
                                hndesignid.Value = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                            }
                        }
                        else
                        {
                            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Dsrno"].ToString();
                            hndesignid.Value = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                        }
                    }
                    else
                    {
                        DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                    }
                }
                if (TDColor.Visible == true)
                {
                    if (variable.Withbuyercode == "1")
                    {
                        if (ds.Tables[0].Rows[0]["OrderCategoryId"].ToString() == "2")
                        {
                            if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                            {
                                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                            }
                            else
                            {
                                DDColor.SelectedValue = ds.Tables[0].Rows[0]["Csrno"].ToString();
                            }
                        }
                        else
                        {
                            DDColor.SelectedValue = ds.Tables[0].Rows[0]["Csrno"].ToString();
                        }
                    }
                    else
                    {
                        DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                    }
                }
                if (TDShape.Visible == true)
                {
                    DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                }
                if (TDSize.Visible == true)
                {
                    DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["flagsize"].ToString();
                    shapeselectedindexchange();
                    DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                if (TDShadeColor.Visible == true)
                {
                    ddshadecolor.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
                }
                if (HDF1.Value == "44")
                {
                    ddfiller.SelectedValue = ds.Tables[0].Rows[0]["filler"].ToString();
                }

                DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
                TxtArea.Text = ds.Tables[0].Rows[0]["TotalArea"].ToString();
                TxtQuantity.Text = ds.Tables[0].Rows[0]["QtyRequired"].ToString();

                TxtPrice.Text = ds.Tables[0].Rows[0]["UnitRate"].ToString();
                DDWareHouseName.SelectedValue = ds.Tables[0].Rows[0]["Warehouseid"].ToString();
                TxtArticleNo.Text = ds.Tables[0].Rows[0]["ArticalNo"].ToString();
                TxtDispatchDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
                TxtWeavingInstructions.Text = ds.Tables[0].Rows[0]["WeavingInstruction"].ToString();
                TxtFinishingInstructions.Text = ds.Tables[0].Rows[0]["FinishingInstructions"].ToString();
                TxtDyeingInstructions.Text = ds.Tables[0].Rows[0]["DyeingInstructions"].ToString();
                TXTOURCODE.Text = ds.Tables[0].Rows[0]["OURCODE"].ToString();
                TXTBUYERCODE.Text = ds.Tables[0].Rows[0]["BUYERCODE"].ToString();
                DDShipment.SelectedValue = ds.Tables[0].Rows[0]["TransmodeId"].ToString();
                hnorderphoto.Value = ds.Tables[0].Rows[0]["photo"].ToString();
                LblErrorMessage.Visible = false;
                txtremark.Text = ds.Tables[0].Rows[0]["remark"].ToString();
                DDRecipeName.SelectedValue = ds.Tables[0].Rows[0]["RecipeNameID"].ToString();
                txtmer.Text = ds.Tables[0].Rows[0]["Merchandise"].ToString();
                txtincharge.Text = ds.Tables[0].Rows[0]["Incharge"].ToString();
                txtextraqty.Text = ds.Tables[0].Rows[0]["extraqty"].ToString();
                if (TDWareHouseNameByCodeDDL.Visible == true)
                {
                    UtilityModule.ConditionalComboFill(ref DDWareHouseNameByCode, "SELECT WareHouseNameId,WareHouseNameByCode from WareHouseNameByWareHouseCode Where CustomerId=" + DDCustomerCode.SelectedValue + " and WareHouseId=" + DDWareHouseName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by WareHouseNameByCode", true, "--SELECT--");
                    DDWareHouseNameByCode.SelectedValue = ds.Tables[0].Rows[0]["WareHouseNameId"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            Label lbl = ((Label)e.Row.FindControl("lblconsumpflag"));
            if (Convert.ToInt32(lbl.Text) > 0)
            {
                e.Row.BackColor = Color.Green;
            }

            for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
            {
                //if (DGOrderDetail.Columns[i].HeaderText == "Finished_ID")
                //{
                //    DGOrderDetail.Columns[i].Visible = false;
                //}
                //if (DGOrderDetail.Columns[i].HeaderText == "FlagSize")
                //{
                //    DGOrderDetail.Columns[i].Visible = false;
                //}

                if (Session["varcompanyId"].ToString() == "20")
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "Description" || DGOrderDetail.Columns[i].HeaderText == "ArticalNo" || DGOrderDetail.Columns[i].HeaderText == "W Inst" || DGOrderDetail.Columns[i].HeaderText == "F Inst" || DGOrderDetail.Columns[i].HeaderText == "D Inst" || DGOrderDetail.Columns[i].HeaderText == "Remark" || DGOrderDetail.Columns[i].HeaderText == "PKGInst" || DGOrderDetail.Columns[i].HeaderText == "LBGInst" || DGOrderDetail.Columns[i].HeaderText == "Add Consumption" || DGOrderDetail.Columns[i].HeaderText == "Finished_ID" || DGOrderDetail.Columns[i].HeaderText == "FlagSize" || DGOrderDetail.Columns[i].HeaderText == "AddImage")
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }

                }
                else
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "Quality" || DGOrderDetail.Columns[i].HeaderText == "Design" || DGOrderDetail.Columns[i].HeaderText == "Color" || DGOrderDetail.Columns[i].HeaderText == "Shape" || DGOrderDetail.Columns[i].HeaderText == "Size")
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }

                }
            }

            switch (HDF1.Value)
            {
                case "7":
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail2, "Select$" + e.Row.RowIndex);
                    break;
                case "3":
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail2, "Select$" + e.Row.RowIndex);
                    break;
                case "10":
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail2, "Select$" + e.Row.RowIndex);
                    break;
                case "12":
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
                    break;
                default:
                    if (Flagforsampleorder == 1)
                    {
                        if (Convert.ToInt16(ddordertype.SelectedValue) == 2) //For Sample
                        {
                            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail2, "Select$" + e.Row.RowIndex);
                        }
                        else
                        {
                            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
                        }
                    }
                    else { e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex); }
                    break;
            }
        }
    }
    protected void TxtCustOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtCustOrderNo_Validate();
        hncustomerorderNo.Value = TxtCustOrderNo.Text;
        ddordertype.Focus();
    }
    protected void DDWareHouseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "20")
        {
            UtilityModule.ConditionalComboFill(ref DDWareHouseNameByCode, "SELECT WareHouseNameId,WareHouseNameByCode from WareHouseNameByWareHouseCode Where CustomerId=" + DDCustomerCode.SelectedValue + " and WareHouseId=" + DDWareHouseName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by WareHouseNameByCode", true, "--SELECT--");
        }
        TxtPrice.Focus();
    }
    protected void TxtDispatchDate_TextChanged(object sender, EventArgs e)
    {
        validate_DispatchDate();
        TxtArticleNo.Focus();
    }
    /*-----------------------------------------------------------------------------------------------------------------------
                                    Function to show Parameters according to category for process
    ------------------------------------------------------------------------------------------------------------------------*/

    private void ddlcategorycange()
    {
        TDQuality.Visible = false;
        TdDESIGN.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDShadeColor.Visible = false;
        TDSize.Visible = false;
        TDAREA.Visible = false;
        TDTxtArea.Visible = false;
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
                        if (Session["VarcompanyNo"].ToString() == "7")
                        {
                            TDAREA.Visible = false;
                            TDTxtArea.Visible = false;
                        }
                        else
                        {
                            TDAREA.Visible = true;
                            TDTxtArea.Visible = true;
                        }
                        break;
                    case "6":
                        TDShadeColor.Visible = true;
                        break;
                    case "10":
                        TDColor.Visible = true;
                        break;
                }
            }
        }
        if (variable.Withbuyercode == "1")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDItemName, @"select Distinct Vf.ITEM_ID,vf.ITEM_NAME from V_FinishedItemDetailNew Vf inner join CustomerQuality CQ on vf.QualityId=CQ.QualityId
                                                               Where vf.CATEGORY_ID= " + DDItemCategory.SelectedValue + " and cQ.CustomerId=" + DDCustomerCode.SelectedValue + "  order by Item_NAme ", true, "---SELECT----");
            }
            else
            {
                string str = "";
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        str = @"select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name";
                    }
                    else
                    {
                        str = @"select Distinct Vf.ITEM_ID,vf.ITEM_NAME from V_FinishedItemDetail Vf inner join CustomerQuality CQ on vf.QualityId=CQ.QualityId
                       Where vf.CATEGORY_ID= " + DDItemCategory.SelectedValue + " and cQ.CustomerId=" + DDCustomerCode.SelectedValue + "  order by Item_NAme ";
                    }
                }
                else
                {
                    str = @"select Distinct Vf.ITEM_ID,vf.ITEM_NAME from V_FinishedItemDetail Vf inner join CustomerQuality CQ on vf.QualityId=CQ.QualityId
                       Where vf.CATEGORY_ID= " + DDItemCategory.SelectedValue + " and cQ.CustomerId=" + DDCustomerCode.SelectedValue + "  order by Item_NAme ";
                }

                UtilityModule.ConditionalComboFill(ref DDItemName, str, true, "---SELECT----");
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
        }

        TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
    }
    /******************************************************To Fill Combo***********************************************************/
    private void fillCombo()
    {
        string str = "";
        if (TdDESIGN.Visible == true)
        {
            if (variable.Withbuyercode == "1")
            {

                if (ddordertype.SelectedValue == "2") //Sample order
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        str = @"select Distinct V.DesignId,DesignName As Designname
                              from Design V Left Outer Join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname";
                    }
                    else
                    {
                        str = @"select CD.SrNo,CD.DesignNameAToC +' ['+D.DesignName+']' as DesignNameAToC from CustomerDesign CD inner join Design D on Cd.DesignId=D.designId and CD.CustomerId=" + DDCustomerCode.SelectedValue + " and D.MasterCompanyid=" + Session["varcompanyid"] + @"
                         order by DesignNameAToC";
                    }
                }
                else
                {
                    str = @"select CD.SrNo,CD.DesignNameAToC +' ['+D.DesignName+']' as DesignNameAToC from CustomerDesign CD inner join Design D on Cd.DesignId=D.designId and CD.CustomerId=" + DDCustomerCode.SelectedValue + " and D.MasterCompanyid=" + Session["varcompanyid"] + @"
                         order by DesignNameAToC";
                }
                UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--SELECT");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName As Designname
                                                             from Design V Left Outer Join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
            }
        }
        if (TDColor.Visible == true)
        {
            if (variable.Withbuyercode == "1")
            {
                if (ddordertype.SelectedValue == "2")
                {
                    if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                    {
                        str = @"select Distinct C.ColorId,C.ColorName As ColorName
                                from Color C Left Outer Join CustomerColor CC on C.ColorId=CC.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName";
                    }
                    else
                    {
                        str = @"select CC.SrNo,CC.ColorNameToC+' ['+C.colorname+']' as ColorNameToC from CustomerColor CC inner join color C on CC.ColorId=C.ColorId
                         and CC.CustomerId=" + DDCustomerCode.SelectedValue + " and C.MasterCompanyId=" + Session["varcompanyid"] + " order by ColorNameToC";
                    }
                }
                else
                {
                    str = @"select CC.SrNo,CC.ColorNameToC+' ['+C.colorname+']' as ColorNameToC from CustomerColor CC inner join color C on CC.ColorId=C.ColorId
                         and CC.CustomerId=" + DDCustomerCode.SelectedValue + " and C.MasterCompanyId=" + Session["varcompanyid"] + " order by ColorNameToC";

                }
                UtilityModule.ConditionalComboFill(ref DDColor, str, true, "-SELECT-");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName As ColorName
                                                             from Color C Left Outer Join CustomerColor CC on C.ColorId=CC.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
            }
        }
        if (TDShape.Visible == true)
        {
            if (variable.Withbuyercode == "1")
            {
                if (variable.VarNewQualitySize == "1")
                {

                    UtilityModule.ConditionalComboFill(ref DDShape, @"select distinct S.ShapeId,S.ShapeName from QualitySizeNew QSN inner join CustomerSize CS on QSN.SizeId=CS.Sizeid
                                                                        INNER JOIN Shape S ON QSN.ShapeId=S.ShapeId INNER JOIN ITEM_MASTER IM ON QSN.QualityTypeId=IM.ITEM_ID
                                                                        INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID=ICM.CATEGORY_ID
                                                                        Where ICM.CATEGORY_ID=" + DDItemCategory.SelectedValue + " and Cs.CustomerId=" + DDCustomerCode.SelectedValue + "  order by ShapeName", true, "--SELECT--");


                }
                else
                {
                    if (ddordertype.SelectedValue == "2")
                    {
                        if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                        {
                            str = "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename";
                        }
                        else
                        {
                            str = @"select Distinct Vf.ShapeId,vf.ShapeName from V_FinishedItemDetail Vf inner join CustomerSize CS on vf.SizeId=CS.Sizeid
                             Where vf.CATEGORY_ID=" + DDItemCategory.SelectedValue + " and Cs.CustomerId=" + DDCustomerCode.SelectedValue + "  order by ShapeName";
                        }

                    }
                    else
                    {
                        str = @"select Distinct Vf.ShapeId,vf.ShapeName from V_FinishedItemDetail Vf inner join CustomerSize CS on vf.SizeId=CS.Sizeid
                             Where vf.CATEGORY_ID=" + DDItemCategory.SelectedValue + " and Cs.CustomerId=" + DDCustomerCode.SelectedValue + "  order by ShapeName";
                    }

                    UtilityModule.ConditionalComboFill(ref DDShape, str, true, "--SELECT--");
                }
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename", true, "--SELECT--");
            }
        }
        if (TDShadeColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT * from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorName", true, "--SELECT--");
        }
    }
    protected void fillitemcode_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--Select--");
    }
    protected void BtnRefreshItem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name ", true, "---SELECT----");
    }
    //********************************  chek list  ************************************
    public void fill_cklist()
    {
        int n = Gvchklist.Rows.Count;
        string str = @"SELECT Itemid FROM Labelorder lbl inner Join OrderMaster  OM on lbl.OrderId=OM.OrderId where  lbl.OrderId=" + DDCustOrderNo.SelectedValue;
        DataSet DtS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        int k = 0;
        int l = DtS.Tables[0].Rows.Count;
        if (l > 0)
        {
            for (int i = 0; i <= n - 1 && i <= l; i++)
            {
                GridViewRow row = Gvchklist.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
                int itmid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
                if (itmid == Convert.ToInt32(DtS.Tables[0].Rows[k][0]))
                {
                    ((CheckBox)row.FindControl("Chkbox")).Checked = true;
                    k++;
                    l++;
                }
                else
                {
                    ((CheckBox)row.FindControl("Chkbox")).Checked = false;
                }
            }
        }
    }
    //**********************   fuction for chk grid ********************************
    private void Fill_chelist()
    {
        Gvchklist.DataSource = fill_gird_chk();
        Gvchklist.DataBind();
    }
    //*****************************  fill chek gird  ****************************
    private DataSet fill_gird_chk()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ITEM_MASTER.ITEM_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid, dbo.Lablecustomer.CustomerId AS customerid, 
                            dbo.ITEM_MASTER.ITEM_NAME FROM dbo.Lablecustomer INNER JOIN dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid = dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                            dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                            Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
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
        if (ds.Tables[0].Rows.Count > 0)
        {
            Chkbx.Enabled = true;
        }
        return ds;
    }
    //------------------------------------------
    private void Check_mark()
    {
        int n = Gvchklist.Rows.Count;
        int j = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet DtS = null;
        DtS = SqlHelper.ExecuteDataset(con, CommandType.Text, @"SELECT Itemid FROM Labelorder  where OrderId=" + DDCustOrderNo.SelectedValue);
        j = DtS.Tables[0].Rows.Count;
        if (j > 0)
        {
            Chkbx.Checked = true;
            ch.Visible = true;
            int k = 0;
            for (int i = 0; i <= n - 1 && k < j; i++)
            {
                GridViewRow row = Gvchklist.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
                int itmid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
                if (itmid == Convert.ToInt32(DtS.Tables[0].Rows[k][0]))
                {
                    ((CheckBox)row.FindControl("Chkbox")).Checked = true;
                    k++;
                }
                else
                {
                    ((CheckBox)row.FindControl("Chkbox")).Checked = false;
                }
            }
        }
        else
        {
            Chkbx.Checked = false;
            ch.Visible = false;
        }
    }
    // ************************ btn chek event **********************
    protected void Chkbx_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkbx.Checked == true)
        {
            ch.Visible = true;
        }
        else
        {
            ch.Visible = false;
        }
    }
    //****************************  insert into label order tabel *******************************
    public void insertvalue_LableOrder()
    {
        //SqlCommand cmd = new SqlCommand();
        int itemid = 0;
        int n = Gvchklist.Rows.Count;
        for (int i = 0; i < n; i++)
        {
            GridViewRow row = Gvchklist.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
            if (isChecked == true)
            {
                itemid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
            }
            else
            {
                itemid = 0;
            }
            //string strpercentage = ((TextBox)Gvitemdetail.Rows[i].FindControl("txtp_tage")).Text;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[2];
                _arrPara[0] = new SqlParameter("@itemid", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@orderid", SqlDbType.Int);
                _arrPara[0].Value = itemid;
                _arrPara[1].Value = Convert.ToInt32(ViewState["order_id"].ToString());
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_labelorderinsert", _arrPara);
                //  ds.Tables[0].Columns["customerid"].ColumnName = "SerialNo.";
            }
            catch
            {
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
    }
    //**********************   fuction for chk grid ********************************
    private void Fill_chelist2()
    {
        Gvchklist.DataSource = fill_gird_chk2();
        Gvchklist.DataBind();
    }
    //*******************************************  fill chk grid on order id condition *********************************
    private DataSet fill_gird_chk2()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT     dbo.ITEM_MASTER.ITEM_ID, dbo.LabelOrder.Orderid, dbo.ITEM_MASTER.ITEM_NAME, dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, 
                            dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME
                            FROM dbo.ITEM_CATEGORY_MASTER INNER JOIN
                            dbo.ITEM_MASTER ON dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID = dbo.ITEM_MASTER.CATEGORY_ID INNER JOIN
                            dbo.LabelOrder ON dbo.ITEM_MASTER.ITEM_ID = dbo.LabelOrder.ItemId
                            Where orderid=" + DDCustOrderNo.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
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
    //********************************  chek list  ************************************
    public void fill_cklist2()
    {
        int n = Gvchklist.Rows.Count;
        for (int i = 0; i <= n - 1; i++)
        {
            GridViewRow row = Gvchklist.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
            int itmid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
            string str = @"SELECT     dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid, dbo.Lablecustomer.CustomerId AS customerid, 
                         dbo.ITEM_MASTER.ITEM_NAME
                         FROM dbo.Lablecustomer INNER JOIN
                         dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid = dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                         dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                         Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            DataSet DtS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (DtS.Tables[0].Rows.Count > 0)
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
            else
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = false;
            }
        }
    }
    //**********************   fuction for chk grid ********************************
    private void Fill_chelist_order()
    {
        Gvchklist.DataSource = fill_gird_chk_order();
        Gvchklist.DataBind();
    }
    //*****************************  fill chek gird  ****************************
    private DataSet fill_gird_chk_order()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid, dbo.Lablecustomer.CustomerId AS customerid, 
                            dbo.ITEM_MASTER.ITEM_NAME
                            FROM dbo.Lablecustomer INNER JOIN
                            dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid = dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                            dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                            Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
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
    public void fill_gird_chk_order_chk()
    {
        int n = Gvchklist.Rows.Count;
        for (int i = 0; i <= n - 1; i++)
        {
            GridViewRow row = Gvchklist.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
            int itmid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
            string str = @"SELECT     dbo.ITEM_MASTER.ITEM_ID, dbo.LabelOrder.Orderid, dbo.ITEM_MASTER.ITEM_NAME, dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, 
                         dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME
                         FROM dbo.ITEM_CATEGORY_MASTER INNER JOIN
                         dbo.ITEM_MASTER ON dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID = dbo.ITEM_MASTER.CATEGORY_ID INNER JOIN
                         dbo.LabelOrder ON dbo.ITEM_MASTER.ITEM_ID = dbo.LabelOrder.ItemId
                         Where orderid=" + DDCustOrderNo.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            DataSet DtS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            int value = DtS.Tables[0].Rows.Count;
            //  int coln =Convert.ToInt16(DtS.Tables[0].Rows[i]["ITEM_ID"].ToString());
            if (DtS.Tables[0].Rows.Count > 0)
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
            else
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = false;
            }
        }
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
        UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME", true, "--SELECT--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        #region on 18-Dec-2012
        //UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " order by QualityName", true, "--SELECT--");
        #endregion
        if (variable.Withbuyercode == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, @"SELECT Distinct Q.QualityId,isnull(Cq.QualityNameAtoC,'') As QualityName
                                                            from Quality Q Inner join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, @"SELECT Distinct Q.QualityId,Q.QualityName + space(2) +isnull(Cq.QualityNameAtoC,'') As QualityName
                                                            from Quality Q Left Outer join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName", true, "--SELECT--");
        }
        //        UtilityModule.ConditionalComboFill(ref DDQuality, @"SELECT  Q.QualityId,Q.QualityName+Space(2)+isnull(Cq.QualityNameAtoC,'') As QualityName
        //                                                          from Quality Q Left Outer join CustomerQuality CQ on Q.QualityId=Cq.QualityId And CustomerId=" + DDCustomerCode.SelectedValue + " Where Item_Id=" + DDItemName.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + "  order by QualityName", true, "--SELECT--");

    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        FillDesign();
        //UtilityModule.ConditionalComboFill(ref DDDesign, "select Designid,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        fillColor();
        //UtilityModule.ConditionalComboFill(ref DDColor, "SELECT  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddshadecolor, "SELECT ShadeColorId,ShadeColorname from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + "  order by ShadeColorname", true, "--SELECT--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDShape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename", true, "--SELECT--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        shapeselectedindexchange();
    }
    protected void TxtArticleNo_TextChanged(object sender, EventArgs e)
    {
        valdate_ArticalNo();
        TXTUPCNO.Focus();
    }
    private void valdate_ArticalNo()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string articalno = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select ArticalNo from OrderDetail  where OrderDetailId !=" + ViewState["OrderDetailId"] + " and ArticalNo='" + TxtArticleNo.Text + "'"));
            if (articalno != "")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Error: Artical no already exist.......";
            }
            else
            {
                LblErrorMessage.Text = "";
            }
            if (TxtQuantity.Text == "" || TxtQuantity.Text == "0")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Error: Quantity Cann't be Blank Or Zero.......";
            }
            else
            {
                LblErrorMessage.Text = "";
            }
        }
        catch
        {
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkForInternal_OC_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForInternal_OC.Checked == true)
        {
            Session["ReportPath"] = "Reports/LocalOC.rpt";
            Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
        }
        else
        {
            Session["ReportPath"] = "Reports/OrderReport.rpt";
            Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
        }
    }
    //---------------------------------------Product code autocomplete--------------
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where id=0 and ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId + "";
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
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
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        if (TxtProdCode.Text != "")
        {
            //ddCategoryName.SelectedIndex = 0;
            UtilityModule.ConditionalComboFill(ref DDItemCategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            Str = "Select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  And Id=0 and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"] + "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDItemCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---SELECT----");
                TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue;
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by QualityName ", true, "--SELECT--");
                TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue;
                // QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape,ddShade, Convert.ToInt32(ddItemName.SelectedValue),0);
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref DDItemCode, "SELECT  QualityCodeId,SubQuantity from QualityCodeMaster where QualityId=" + DDQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SubQuantity", true, "--SELECT--");
                TxtFinishedid.Text = "Category=" + DDItemCategory.SelectedValue + "&Item=" + DDItemName.SelectedValue + "&Quality=" + DDQuality.SelectedValue;
                UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
                // Fill_Sub_Quality();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                shapeselectedindexchange();
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                DDSize_SelectedIndexChanged(sender, e);
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDItemCategory.SelectedIndex = 0;
                ddlcategorycange();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDItemCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        shapeselectedindexchange();
    }
    protected void ddPreview_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt16(ddPreview.SelectedValue) == 3)
        //{
        //    costingReport();
        //}
        //else
        //{
        Report_Type();
        // }
    }
    //private void Report_Type()
    //{
    //    // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));

    //    switch (Convert.ToInt16(Session["varCompanyId"]))
    //    {
    //        case 1:
    //            ReportForSamara();
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //            break;
    //        case 2:
    //            ReportForDestini();
    //            StringBuilder stb = new StringBuilder();
    //            stb.Append("<script>");
    //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //            break;
    //        case 3:
    //            ReportForMalani();
    //            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //            break;
    //        case 4:
    //            ReportForSamara();
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //            break;
    //        case 7:
    //            //ReportForMalani();
    //            Session["ReportPath"] = "Reports/RptPerFormaInvoiceDelhi.rpt";
    //            Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"] + "";
    //            break;
    //        case 10:
    //            ReportForrassindia();
    //            break;
    //        default:
    //            ReportForSamara();
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //            break;
    //    }
    //}

    private void Report_Type()
    {
        // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        switch (ddPreview.SelectedValue.ToString())
        {
            case "1":
                switch (Session["varcompanyid"].ToString())
                {
                    case "1":
                        Session["ReportPath"] = "Reports/OrderReport.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "2":
                        ReportForDestini();
                        break;
                    case "3":
                        MalaniReporttype1();
                        break;
                    case "4":
                        Session["ReportPath"] = "Reports/OrderReport.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "7":
                        Session["ReportPath"] = "Reports/RptPerFormaInvoiceDelhi.rpt";
                        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"] + "";
                        break;
                    case "10":
                        RptForRasindiaType1();
                        break;
                    case "20":
                        MaltiRugOrderPreviewReport();
                        //Session["ReportPath"] = "Reports/OrderReportForMaltiRug.rpt";
                        //Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "30":
                        Session["ReportPath"] = "Reports/OrderReportNewSamara.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "42":
                        Session["ReportPath"] = "Reports/OrderReportNewVikramMirzapur.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "44":
                        if (variable.Carpetcompany == "1")
                        {
                            Session["ReportPath"] = "Reports/OrderReportagni.rpt";

                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/OrderReport.rpt";
                        }
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    default:
                        if (variable.Carpetcompany == "1")
                        {
                            Session["ReportPath"] = "Reports/OrderReportNew.rpt";

                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/OrderReport.rpt";
                        }

                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                }
                break;
            case "2":
                switch (Session["varcompanyid"].ToString())
                {
                    case "1":
                        Session["ReportPath"] = "Reports/LocalOC.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "3":
                        Session["ReportPath"] = "Reports/LocalOC.rpt";
                        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"];
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                    case "30":
                        SqlParameter[] param = new SqlParameter[1];
                        param[0] = new SqlParameter("@orderid", ViewState["order_id"]);
                        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetCustomerOrderInternalOCReport", param);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["GetDataset"] = ds;
                            Session["rptFileName"] = "~\\Reports\\RptLocalOC.rpt";
                            Session["dsFileName"] = "~\\ReportSchema\\RptLocalOC.xsd";
                            StringBuilder stb = new StringBuilder();
                            stb.Append("<script>");
                            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                        }
                        ds.Dispose();
                        break;
                    default:
                        Session["ReportPath"] = "Reports/LocalOC.rpt";
                        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        break;
                }
                break;
            case "3":
                costingReport();
                break;
            case "4":
                switch (Session["varcompanyid"].ToString())
                {
                    case "30":
                        if (ChkForPerformaInvoiceType2.Checked == true)
                        {
                            GeneratePerformainvoiceNo();
                            PerformainvoiceSamaraType2();
                        }
                        else
                        {
                            GeneratePerformainvoiceNo();
                            PerformainvoiceSamara();
                        }
                        break;
                    case "19":
                        if (DDCustomerCode.SelectedValue == "17")
                        {
                            GeneratePerformainvoiceNo();
                            PerformainvoiceJRExportForCOIN();
                        }
                        else
                        {
                            GeneratePerformainvoiceNo();
                            Performainvoice();
                        }
                        break;
                    case "16":
                        GeneratePerformainvoiceNo();
                        PerformainvoiceChampoType2();
                        break;
                    case "37":
                        GeneratePerformainvoiceNo();
                        PerformainvoiceSundeepExport();
                        break;
                    default:
                        GeneratePerformainvoiceNo();
                        Performainvoice();
                        break;
                }
                break;
            case "5":
                Vendorwisedetail();
                break;
            case "6":
                BomDetail();
                break;
            default:
                #region
                //switch (Convert.ToInt16(Session["varCompanyId"]))
                //{
                //    case 1:
                //        ReportForSamara();
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                //        break;
                //    case 2:
                //        ReportForDestini();
                //        StringBuilder stb = new StringBuilder();
                //        stb.Append("<script>");
                //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                //        break;
                //    case 3:
                //        ReportForMalani();
                //        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                //        break;
                //    case 4:
                //        ReportForSamara();
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                //        break;
                //    case 7:
                //        //ReportForMalani();
                //        Session["ReportPath"] = "Reports/RptPerFormaInvoiceDelhi.rpt";
                //        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"] + "";
                //        break;
                //    case 10:
                //        ReportForrassindia();
                //        break;
                //    default:
                //        ReportForSamara();
                //        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                //        break;
                //}
                #endregion
                break;
        }

    }
    protected void MalaniReporttype1()
    {
        String STR = "";
        DataSet ds;
        Session["ReportPath"] = "Reports/RptPerFormaInvoiceorderNew.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceorderNew.xsd";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        STR = @" SELECT CSI.CompanyName,CSI.Address,CMI.CompanyName,CMI.CompAddr1,CMI.CompAddr2,CMI.CompAddr3,CMI.CompFax,CMI.CompTel,CMI.TinNo,CMI.Email,OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID,
                     OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, T.TermName, 
                     P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id,
                     OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount
                     AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight,
                    OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,
                    OM.LocalOrder,Custorderdate,duedate,vf.ITEM_NAME  ,vf.QualityName ,vf.designName,vf.ColorName,vf.ShapeName,vf.SizeFt,
                     B.BankName, B.Street, B.City,B.State, B.Country, B.ACNo, B.SwiftCode, B.PhoneNo, 
                     B.FaxNo,U.UnitName,CS.Length,CS.Width,CS.Height,CS.PCS,CS.UnitId,UP.UPCNO,vf.ShapeId,CustomerColor.ColorNameToC,CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
                      CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS 
                     OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId 
                     LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN 
                     dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                     LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId inner join Companyinfo CMI on CMI.COmpanyId=OM.CompanyId
                     Inner join Customerinfo CSI on CSI.Customerid=OM.Customerid inner join V_FInishedItemDetail Vf on Vf.Item_Finished_Id=OD.Item_Finished_Id
                      inner join Unit U on U.UNitid=Od.OrderUnitId left outer join CONTAINERCOST CS on CS.DraftOrderDetailId=OD.OrderDetailId
                      left outer join UPCNO UP on UP.Customerid=CSI.Customerid And UP.Finishedid=Vf.Item_Finished_id
                      inner join Bank B on CMI.BankId=B.BankId LEFT OUTER JOIN CustomerDesign CustomerDesign ON 
                       CSI.CustomerId=CustomerDesign.CustomerId AND vf.designId=CustomerDesign.DesignId LEFT OUTER JOIN 
                       CustomerSize CustomerSize ON CSI.CustomerId=CustomerSize.CustomerId AND Vf.SizeId=CustomerSize.Sizeid 
                     LEFT OUTER JOIN CustomerQuality CustomerQuality ON CSI.CustomerId=CustomerQuality.CustomerId AND 
                     Vf.QualityId=CustomerQuality.QualityId LEFT OUTER JOIN CustomerColor CustomerColor ON 
                      CSI.CustomerId=CustomerColor.CustomerId AND Vf.ColorId=CustomerColor.ColorId
                         where OM.OrderId=" + ViewState["order_id"];



        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
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
        string STR1 = @"select distinct  oi.OrderDetailId,oi.Photo from order_referenceimage as oi  inner join orderdetail od on od.OrderDetailid=oi.OrderDetailid 
                            where orderid=" + ViewState["order_id"] + "";
        SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
        DataTable dt1 = new DataTable();
        sda1.Fill(dt1);
        ds.Tables.Add(dt1);
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
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }

    }
    protected void Performainvoice()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");
            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 16.43;
            sht.Columns("B").Width = 15.29;
            sht.Columns("C").Width = 13.43;
            sht.Columns("D").Width = 7.71;
            sht.Columns("E").Width = 3.14;
            sht.Columns("F").Width = 1.71;
            sht.Columns("G").Width = 4.29;
            sht.Columns("H").Width = 2.29;
            sht.Columns("I").Width = 3.86;
            sht.Columns("J").Width = 6.57;
            sht.Columns("K").Width = 7.57;
            sht.Columns("L").Width = 10.14;
            sht.Columns("M").Width = 6.71;
            sht.Row(1).Height = 21;
            //**************
            sht.Range("A1:M1").Merge();
            sht.Range("A1").Value = "P R O F O R M A   I N V O I C E";
            sht.Range("A1:M1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:M1").Style.Font.FontSize = 16;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************
            sht.Range("A2:D2").Merge();
            sht.Range("A2:D2").Style.Font.FontSize = 11;
            sht.Range("A2:D2").Style.Font.Bold = true;


            sht.Range("A2").Value = "Exporter";
            sht.Range("A3:D3").Style.Font.Bold = true;
            sht.Range("A3:D3").Style.Font.FontSize = 13;


            sht.Range("A3:D3").Merge();
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A4:D4").Merge();
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A5:D5").Merge();
            sht.Range("A5").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"] + " FAX : " + ds.Tables[0].Rows[0]["Compfax"];
            sht.Range("A6:D6").Merge();
            sht.Range("A6").Value = "E-MAIL : " + ds.Tables[0].Rows[0]["email"];
            //'Invoice NO Date
            sht.Range("E2:H2").Merge();
            sht.Range("E2:H3").Style.Font.FontSize = 11;
            sht.Range("E2:H3").Style.Font.Bold = true;
            sht.Range("E2").Value = "Invoice No.";
            //'value
            sht.Range("I2:K2").Merge();
            sht.Range("I2").SetValue(ds.Tables[0].Rows[0]["invoiceno"]);

            sht.Range("E3:H3").Merge();
            sht.Range("E3").Value = "Invoice Date";
            sht.Range("I3:K3").Merge();
            sht.Range("I3").SetValue(ds.Tables[0].Rows[0]["invoicedate"]);
            //********Exporterref
            sht.Range("L2:M2").Merge();
            sht.Range("L2:M2").Style.Font.FontSize = 11;
            sht.Range("L2:M2").Style.Font.Bold = true;
            sht.Range("L2").Value = "Exporter's Ref.";
            //'value
            sht.Range("L3:M3").Merge();
            sht.Range("L3").Value = "";
            //*******
            sht.Range("E4:M4").Merge();
            sht.Range("E4").Value = "Buyer's Order No. & date";
            sht.Range("E4:M4").Style.Font.FontSize = 11;
            sht.Range("E4:M4").Style.Font.Bold = true;

            sht.Range("E5:M5").Merge();
            sht.Range("E5").Value = "PO# " + ds.Tables[0].Rows[0]["customerorderNo"] + "/" + ds.Tables[0].Rows[0]["orderdate"];

            //'Other ref
            sht.Range("E6:M6").Merge();
            sht.Range("E6").Value = "Other Reference(s) :";
            sht.Range("E6:M6").Style.Font.FontSize = 11;
            sht.Range("E6:M6").Style.Font.Bold = true;
            sht.Range("E7:M7").Merge();
            sht.Range("E7").Value = "";

            //'consignee

            sht.Range("A8:D8").Merge();
            sht.Range("A8:D8").Style.Font.FontSize = 11;
            sht.Range("A8:D8").Style.Font.Bold = true;
            sht.Range("A8").Value = "Consignee";
            //'value
            sht.Range("A9:D9").Merge();
            sht.Range("A9").Value = ds.Tables[0].Rows[0]["customercompany"];
            sht.Range("A10:D11").Merge();
            sht.Range("A10").Value = ds.Tables[0].Rows[0]["customeraddress"];
            sht.Range("A10:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A10:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A10").Style.Alignment.WrapText = true;

            sht.Range("A12:D12").Merge();
            sht.Range("A12").Value = "TEL :" + ds.Tables[0].Rows[0]["customerphoneno"] + " FAX : " + ds.Tables[0].Rows[0]["customerfax"];
            sht.Range("A13:D13").Merge();
            sht.Range("A13").Value = "E-MAIL :" + ds.Tables[0].Rows[0]["customermail"];
            sht.Range("A14:D14").Merge();
            sht.Range("A15:D15").Merge();
            //'*******Buyerotherthanconsignee

            sht.Range("E8:M8").Merge();
            sht.Range("E8").Value = "Buyer (if other than consignee)";
            sht.Range("E8:M8").Style.Font.FontSize = 11;
            sht.Range("E8:M8").Style.Font.Bold = true;
            //'
            //sht.Range("E10:M10").Merge();
            //sht.Range("E10:M10").Style.Font.FontSize = 11;
            //sht.Range("E10:M10").Style.Font.Bold = true;
            //sht.Range("E10").SetValue("SHIP DATE :- " + ds.Tables[0].Rows[0]["Dispatchdate"]);
            //'******Country of origin of goods
            sht.Range("E14:J14").Merge();
            sht.Range("E14:J14").Style.Font.FontSize = 11;
            sht.Range("E14:J14").Style.Font.Bold = true;
            sht.Range("E14").Value = "Country of Origin of goods";
            //'value
            sht.Range("E15:H15").Merge();
            sht.Range("E15:H15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E15").Value = "INDIA";
            //'Country of final destination
            sht.Range("K14:M14").Merge();
            sht.Range("K14:M14").Style.Font.FontSize = 11;
            sht.Range("K14:M14").Style.Font.Bold = true;
            sht.Range("K14").Value = "Country of Final Destination";
            //'value
            sht.Range("K15:M15").Merge();
            sht.Range("K15:M15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("K15").Value = ds.Tables[0].Rows[0]["countryname"];
            //'****Precarrigeby

            sht.Range("A16").Value = "Pre-Carriage by";
            sht.Range("A16").Style.Font.FontSize = 11;
            sht.Range("A16").Style.Font.Bold = true;
            //'
            sht.Range("A17").Value = ds.Tables[0].Rows[0]["Precarriageby"];
            //'*****Place of Receipt
            sht.Range("B16:D16").Merge();
            sht.Range("B16:D16").Style.Font.FontSize = 11;
            sht.Range("B16:D16").Style.Font.Bold = true;
            sht.Range("B16").Value = "Place of Receipt";
            //'
            sht.Range("B17:D17").Merge();
            sht.Range("B17").Value = ds.Tables[0].Rows[0]["Placeofreceipt"];
            //'*****vessel_flight no
            sht.Range("A18").Value = "Mode of Shipment";
            sht.Range("A18").Style.Font.FontSize = 11;
            sht.Range("A18").Style.Font.Bold = true;

            //'
            sht.Range("A19").Value = ds.Tables[0].Rows[0]["modeofshipment"];
            //'**********port of loading
            sht.Range("B18:D18").Merge();
            sht.Range("B18:D18").Style.Font.FontSize = 11;
            sht.Range("B18:D18").Style.Font.Bold = true;
            sht.Range("B18").Value = "Port of Loading";
            //'
            sht.Range("B19:D19").Merge();
            sht.Range("B19").Value = ds.Tables[0].Rows[0]["portofloading"];
            //'********Port of discharge
            sht.Range("A20").Value = "Port of Discharge";
            sht.Range("A20").Style.Font.FontSize = 11;
            sht.Range("A20").Style.Font.Bold = true;
            //'
            sht.Range("A21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //'***********Final destination
            sht.Range("B20:D20").Merge();
            sht.Range("B20:D20").Style.Font.FontSize = 11;
            sht.Range("B20:D20").Style.Font.Bold = true;

            sht.Range("B20").Value = "Final Destination";
            //'
            sht.Range("B21:D21").Merge();
            sht.Range("B21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //SHIPDATE
            sht.Range("E20:M20").Merge();
            sht.Range("E20").Value = "SHIP DATE :-" + ds.Tables[0].Rows[0]["Dispatchdate"];
            sht.Range("E20:M20").Style.Font.FontSize = 11;
            sht.Range("E20:M20").Style.Font.Bold = true;
            sht.Range("E20:M20").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //term of sale
            sht.Range("E16:M16").Merge();
            sht.Range("E16").Value = "Terms of Delivery and Payment";
            sht.Range("E16:M16").Style.Font.FontSize = 11;
            sht.Range("E16:M16").Style.Font.Bold = true;
            sht.Range("E18:M18").Merge();
            sht.Range("E19:M19").Merge();
            sht.Range("E18").Value = ds.Tables[0].Rows[0]["Modofpayment"];
            sht.Range("E19").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
            //'*****Mark & Nos.
            sht.Range("A22:B23").Style.Font.FontSize = 11;
            sht.Range("A22:B23").Style.Font.Bold = true;
            sht.Range("A22").Value = "Marks & Nos.";
            sht.Range("B22").Value = "No. & Kind";
            sht.Range("B23").Value = "of Packages";
            //'******Description of goods
            sht.Range("C22:D22").Merge();
            sht.Range("C22").Value = "Description of Goods";
            sht.Range("C22:D22").Style.Font.FontSize = 11;
            sht.Range("C22:D22").Style.Font.Bold = true;
            //'******Detail Headers
            sht.Range("A24:M26").Style.Font.FontSize = 10;
            sht.Range("A24:M26").Style.Font.Bold = true;
            sht.Range("A24:M26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A24:A26").Merge();
            sht.Range("A24:A26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A24:A26").Style.Font.FontSize = 12;
            sht.Range("A24").Value = "QUALITY";

            sht.Range("B24:B26").Merge();
            sht.Range("B24:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B24:B26").Style.Font.FontSize = 12;
            sht.Range("B24").Value = "DESIGN";

            sht.Range("C24:C26").Merge();
            sht.Range("C24:C26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C24:C26").Style.Font.FontSize = 12;
            sht.Range("C24").Value = "COLOR";


            sht.Range("D24:F26").Merge();
            sht.Range("D24:F26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D24:F26").Style.Font.FontSize = 12;
            sht.Range("D24").Value = "SIZE";

            sht.Range("G24:I26").Merge();
            sht.Range("G24:I26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G24").Value = "QUANTITY";
            sht.Range("J24:K26").Merge();
            sht.Range("J24:K26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J24").Value = "RATE";

            sht.Range("J25:K25").Merge();
            sht.Range("J25:K25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J25").Value = ds.Tables[0].Rows[0]["Modofpayment"] + " " + ds.Tables[0].Rows[0]["Currencyname"];

            string unitname;
            if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "0")
            {
                switch (ds.Tables[0].Rows[0]["unitid"].ToString())
                {
                    case "1":
                        unitname = "SQ. Mtr";
                        break;
                    case "2":
                        unitname = "SQ. Ft";
                        break;
                    default:
                        unitname = ds.Tables[0].Rows[0]["unitname"].ToString();
                        break;
                }
            }
            else
            {
                unitname = "PC";
            }
            sht.Range("J26:K26").Merge();
            sht.Range("J26:K26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J26").Value = "Per " + unitname;

            sht.Range("L25:M25").Merge();
            sht.Range("L25:M25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L25:M25").Value = "AMOUNT";

            sht.Range("L26:M26").Merge();
            sht.Range("L26:M26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L26:M26").Value = ds.Tables[0].Rows[0]["currencyname"];
            //*****************
            int i;
            i = 27;
            double TotalArea = 0;
            double GrandTotalArea = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":M" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":M" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":M" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["Qualityname"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["Designname"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["colorname"]);
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);
                sht.Range("G" + i + ":I" + i).Merge();
                sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                sht.Range("J" + i + ":K" + i).Merge();
                sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                sht.Range("L" + i + ":M" + i).Merge();
                sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                TotalArea = Convert.ToDouble(ds.Tables[0].Rows[j]["qtyrequired"]) * Convert.ToDouble(ds.Tables[0].Rows[j]["TotalArea"]);
                GrandTotalArea = GrandTotalArea + TotalArea;

                using (var a = sht.Range("A" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("B" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("C" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("D" + i + ":F" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("G" + i + ":I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i + ":K" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("L" + i + ":M" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                i = i + 1;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            //******
            i = i + 1;
            sht.Range("A" + i + ":M" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":M" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":M" + i).Style.Font.FontSize = 10;

            sht.Range("G" + i + ":I" + i).Merge();
            sht.Range("G" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));

            sht.Range("L" + i + ":M" + i).Merge();
            sht.Range("L" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            using (var a = sht.Range("A" + i + ":M" + i))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL PCS :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", "") + " PCS");

            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL AMOUNT :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(amount)", "") + " " + ds.Tables[0].Rows[0]["currencyname"]);
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            string unit = "";

            switch (ds.Tables[0].Rows[0]["flagsize"].ToString())
            {
                case "1":
                    unit = "SQ.MTR";
                    break;
                case "0":
                    unit = "SQ.FT";
                    break;
            }
            sht.Range("A" + i).Value = "TOTAL " + unit + " :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(GrandTotalArea + " " + unit);
            //sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(totalarea)", "") + " " + unit);

            //*************amount in words
            //string amountinwords = "";
            //Decimal Amount = Convert.ToDecimal(ds.Tables[0].Compute("sum(amount)", ""));
            //amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
            //string val = "", paise = "";
            //if (Amount.ToString().IndexOf('.') > 0)
            //{
            //    val = Amount.ToString().Split('.')[1];
            //    if (Convert.ToInt32(val) > 0)
            //    {
            //        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
            //    }
            //}
            //amountinwords = ds.Tables[0].Rows[0]["currencyname"] + " " + amountinwords + " " + paise + "Only";
            //i = i + 1;
            //sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //sht.Range("M" + i + ":M" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //sht.Range("A" + i + ":I" + (i + 1)).Merge();
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.WrapText = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Font.Bold = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            //sht.Range("A" + i).Value = amountinwords;
            //Bank Details  
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Beneficiary:";
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["bankname"] + "," + ds.Tables[0].Rows[0]["City"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Value = "SWIFT CODE :";
            sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["swiftcode"];
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account:";
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["Accountname"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account no." + ds.Tables[0].Rows[0]["acno"];
            //signature and date
            i = i + 1;
            sht.Range("J" + i + ":M" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i + ":M" + i).Style.Font.Bold = true;
            sht.Range("J" + i + ":M" + i).Merge();
            sht.Range("J" + i).Value = "For " + ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //Declaration
            i = i + 1;
            sht.Range("A" + i).Value = "Declaration:";
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //*************
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "We declare that this invoice show the actual price of the goods";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "described and that all particulars are true and correct";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":M" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //***********Borders
            sht.Range("A1:M1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:A29").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("D2:D26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A7:M7").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E3:M3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("K2:K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("H2:H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("M2:M29").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("E5:M5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A15:M15").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E13:M13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("J14:J15").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A17:D17").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A19:D19").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A21:M21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A16:A26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("B22:B26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A26:M26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            using (var a = sht.Range("A24:A26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("C24:C26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("G24:I26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("K24:K26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A23:M23").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //***********

            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoice-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

            //Session["rptFileName"] = "~\\Reports\\Rptorderperformainvoice.rpt";                   
            //Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\Rptorderperformainvoice.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    //private void ReportForSamara()
    //{
    //    if (Convert.ToInt32(ddPreview.SelectedValue) == 1)
    //    {
    //        if (Session["varcompanyId"].ToString() == "20")
    //        {
    //            MaltiRugOrderPreviewReport();
    //            //Session["ReportPath"] = "Reports/OrderReportForMaltiRug.rpt";
    //            //Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
    //           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //        }
    //        else
    //        {
    //            Session["ReportPath"] = "Reports/OrderReport.rpt";
    //            Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //        }
    //    }
    //    else if (Convert.ToInt32(ddPreview.SelectedValue) == 2)
    //    {
    //        Session["ReportPath"] = "Reports/LocalOC.rpt";
    //        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ViewState["order_id"] + "";
    //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    //    }
    //    else if (Convert.ToInt32(ddPreview.SelectedValue) == 5)
    //    {
    //        Vendorwisedetail();
    //    }
    //}
    string Export = "";
    private void MaltiRugOrderPreviewReport()
    {
        if (cbExport.Checked == true)
        {
            Export = "Y";
        }
        else
        {
            switch (variable.ReportWithpdf)
            {
                case "1":
                    Export = "N";
                    break;
                default:
                    Export = "Y";
                    break;
            }
        }

        Session["ReportPath"] = "Reports/OrderReportForMaltiRug.rpt";
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Orderid", ViewState["order_id"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_GetNewOrderReportDataForMalti", param);

        Session["dsFileName"] = "~\\ReportSchema\\OrderReportForMaltiRug.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void Vendorwisedetail()
    {
        if (cbExport.Checked == true)
        {
            Export = "Y";
        }
        else
        {
            switch (variable.ReportWithpdf)
            {
                case "1":
                    Export = "N";
                    break;
                default:
                    Export = "Y";
                    break;
            }
        }

        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@localOrderNo", TxtLocalOrderNo.Text);
        param[1] = new SqlParameter("@userid", Session["varuserid"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getVendorWiseDetail", param);

        Session["dsFileName"] = "~\\ReportSchema\\RptvendorwisedetailForMaltiRug.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["VarCompanyId"].ToString())
            {
                case "20":
                    Session["ReportPath"] = "Reports/RptvendorwisedetailForMaltiRug.rpt";
                    break;
                case "30":
                    Session["ReportPath"] = "Reports/RptvendorwisedetailForSamara.rpt";
                    break;
                case "31":
                    Session["ReportPath"] = "Reports/RptvendorwisedetailForOPCarpet.rpt";
                    break;
                default:
                    Session["ReportPath"] = "Reports/RptvendorwisedetailForALL.rpt";
                    break;
            }

            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void ReportForDestini()
    {
        if (Convert.ToInt32(ddPreview.SelectedValue) == 1)
        {
            //Session["ReportPath"] = "Reports/RptPerFormaInvoiceWithBuyerCode.rpt";
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceDestiniorderNew.rpt";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        DataSet Ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='VIEW_PERFORMAINVOICEFORDESTINIorder'");
        if (Ds1.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICEFORDESTINIorder");
        }
        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINIorder AS ";
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + Session["OrderId"] + ")");
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        Str = Str + "SELECT PD.PROCESSID,PD.FINISHEDID,PD.IFINISHEDID,PD.OFINISHEDID,DOD.ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Str = Str + ",[dbo].[GET_FINAMT](PD.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PD.IFINISHEDID) '" + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "'";
        }
        Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        Str = Str + @",DOD.PHOTO IMAGE,DOD.QtyRequired OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PD.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,
        DOD.DESCRIPTION,dod.weight From  ORDER_CONSUMPTION_DETAIL PD,ORDERDETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT 
        WHERE pd.orderid=dod.orderid and  Pd.FINISHEDID=DOD.ITEM_FINISHED_ID AND Pd.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND 
        PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["order_id"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "";
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        //con.Close();
        string str = @" SELECT customerinfo.CompanyName, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3, CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, customerinfo.Address, VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID, VIEW_PERFORMAINVOICEFORDESTINI.IFINISHEDID, VIEW_PERFORMAINVOICEFORDESTINI.ITEM_NO, VIEW_PERFORMAINVOICEFORDESTINI.GLASSTYPE, VIEW_PERFORMAINVOICEFORDESTINI.QTY, VIEW_PERFORMAINVOICEFORDESTINI.InnerPacking, VIEW_PERFORMAINVOICEFORDESTINI.MasterPacking, VIEW_PERFORMAINVOICEFORDESTINI.PROD_NO, customerinfo.Email, VIEW_PERFORMAINVOICEFORDESTINI.MiddlePacking, VIEW_PERFORMAINVOICEFORDESTINI.OQTY, VIEW_PERFORMAINVOICEFORDESTINI.PKGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.LBGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.CBM, VIEW_PERFORMAINVOICEFORDESTINI.Remarks, VIEW_PERFORMAINVOICEFORDESTINI.BuyerCode, DRAFT_ORDER_MASTER.OrderNo, DRAFT_ORDER_MASTER.OrderDate, DRAFT_ORDER_MASTER.CustOrderNo, VIEW_PERFORMAINVOICEFORDESTINI.DESCRIPTION, DRAFT_ORDER_MASTER.DeliveryDate, DRAFT_ORDER_MASTER.SeaPort, GoodsReceipt.StationName, TransMode.transmodeName, VIEW_PERFORMAINVOICEFORDESTINI.weight, customerinfo.CustomerCode, DRAFT_ORDER_MASTER.Custorderdate, VIEW_PERFORMAINVOICEFORDESTINI.IMAGE as photo,customerinfo.PhoneNo as custtel,customerinfo.Email as custemail,customerinfo.Fax as custfax
        FROM   ((((dbo.CompanyInfo CompanyInfo INNER JOIN dbo.DRAFT_ORDER_MASTER DRAFT_ORDER_MASTER ON CompanyInfo.CompanyId=DRAFT_ORDER_MASTER.CompanyId) INNER JOIN 
        dbo.VIEW_PERFORMAINVOICEFORDESTINIorder VIEW_PERFORMAINVOICEFORDESTINI ON DRAFT_ORDER_MASTER.OrderId=VIEW_PERFORMAINVOICEFORDESTINI.ORDERID) INNER JOIN dbo.customerinfo customerinfo ON DRAFT_ORDER_MASTER.CustomerId=customerinfo.CustomerId) LEFT OUTER JOIN 
        dbo.GoodsReceipt GoodsReceipt ON DRAFT_ORDER_MASTER.PortOfLoading=GoodsReceipt.GoodsreceiptId) LEFT OUTER JOIN  dbo.TransMode TransMode ON DRAFT_ORDER_MASTER.ByAirSea=TransMode.transmodeId
        where VIEW_PERFORMAINVOICEFORDESTINI.orderid=" + DDCustOrderNo.SelectedValue + @"
        ORDER BY VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID";
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
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
        Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceDestiniorderNew.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        //Session["CommanFormula"] = "{VIEW_PERFORMAINVOICEFORDESTINI.Orderid}=" + ViewState["order_id"] + "";
    }
    private void ReportForMalani()
    {
        string STR = "";
        DataSet ds = new DataSet();

        //sda.Fill(dt);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        if (Convert.ToInt32(ddPreview.SelectedValue) == 1)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceorderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceorderNew.xsd";
            if (variable.VarNewQualitySize == "1")
            {
                STR = @" SELECT CSI.CompanyName,CSI.Address,CMI.CompanyName,CMI.CompAddr1,CMI.CompAddr2,CMI.CompAddr3,CMI.CompFax,CMI.CompTel,CMI.TinNo,CMI.Email,OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID,
                     OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, T.TermName, 
                     P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id,
                     OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount
                     AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight,
                    OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,
                    OM.LocalOrder,Custorderdate,duedate,vf.ITEM_NAME  ,vf.QualityName ,vf.designName,vf.ColorName,vf.ShapeName,vf.SizeFt,
                     B.BankName, B.Street, B.City,B.State, B.Country, B.ACNo, B.SwiftCode, B.PhoneNo, 
                     B.FaxNo,U.UnitName,CS.Length,CS.Width,CS.Height,CS.PCS,CS.UnitId,UP.UPCNO,vf.ShapeId,CustomerColor.ColorNameToC,CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
                      CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS 
                     OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId 
                     LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN 
                     dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                     LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId inner join Companyinfo CMI on CMI.COmpanyId=OM.CompanyId
                     Inner join Customerinfo CSI on CSI.Customerid=OM.Customerid inner join V_FInishedItemDetailNew Vf on Vf.Item_Finished_Id=OD.Item_Finished_Id
                      inner join Unit U on U.UNitid=Od.OrderUnitId left outer join CONTAINERCOST CS on CS.DraftOrderDetailId=OD.OrderDetailId
                      left outer join UPCNO UP on UP.Customerid=CSI.Customerid And UP.Finishedid=Vf.Item_Finished_id
                      inner join Bank B on CMI.BankId=B.BankId LEFT OUTER JOIN CustomerDesign CustomerDesign ON 
                       CSI.CustomerId=CustomerDesign.CustomerId AND vf.designId=CustomerDesign.DesignId LEFT OUTER JOIN 
                       CustomerSize CustomerSize ON CSI.CustomerId=CustomerSize.CustomerId AND Vf.SizeId=CustomerSize.Sizeid 
                     LEFT OUTER JOIN CustomerQuality CustomerQuality ON CSI.CustomerId=CustomerQuality.CustomerId AND 
                     Vf.QualityId=CustomerQuality.QualityId LEFT OUTER JOIN CustomerColor CustomerColor ON 
                      CSI.CustomerId=CustomerColor.CustomerId AND Vf.ColorId=CustomerColor.ColorId
                         where OM.OrderId=" + ViewState["order_id"];
            }
            else
            {
                STR = @" SELECT CSI.CompanyName,CSI.Address,CMI.CompanyName,CMI.CompAddr1,CMI.CompAddr2,CMI.CompAddr3,CMI.CompFax,CMI.CompTel,CMI.TinNo,CMI.Email,OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID,
                     OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DueDate AS DeliveryDate, T.TermName, 
                     P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id,
                     OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount
                     AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight,
                    OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,
                    OM.LocalOrder,Custorderdate,duedate,vf.ITEM_NAME  ,vf.QualityName ,vf.designName,vf.ColorName,vf.ShapeName,vf.SizeFt,
                     B.BankName, B.Street, B.City,B.State, B.Country, B.ACNo, B.SwiftCode, B.PhoneNo, 
                     B.FaxNo,U.UnitName,CS.Length,CS.Width,CS.Height,CS.PCS,CS.UnitId,UP.UPCNO,vf.ShapeId,CustomerColor.ColorNameToC,CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
                      CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS 
                     OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId 
                     LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN 
                     dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                     LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId inner join Companyinfo CMI on CMI.COmpanyId=OM.CompanyId
                     Inner join Customerinfo CSI on CSI.Customerid=OM.Customerid inner join V_FInishedItemDetail Vf on Vf.Item_Finished_Id=OD.Item_Finished_Id
                      inner join Unit U on U.UNitid=Od.OrderUnitId left outer join CONTAINERCOST CS on CS.DraftOrderDetailId=OD.OrderDetailId
                      left outer join UPCNO UP on UP.Customerid=CSI.Customerid And UP.Finishedid=Vf.Item_Finished_id
                      inner join Bank B on CMI.BankId=B.BankId LEFT OUTER JOIN CustomerDesign CustomerDesign ON 
                       CSI.CustomerId=CustomerDesign.CustomerId AND vf.designId=CustomerDesign.DesignId LEFT OUTER JOIN 
                       CustomerSize CustomerSize ON CSI.CustomerId=CustomerSize.CustomerId AND Vf.SizeId=CustomerSize.Sizeid 
                     LEFT OUTER JOIN CustomerQuality CustomerQuality ON CSI.CustomerId=CustomerQuality.CustomerId AND 
                     Vf.QualityId=CustomerQuality.QualityId LEFT OUTER JOIN CustomerColor CustomerColor ON 
                      CSI.CustomerId=CustomerColor.CustomerId AND Vf.ColorId=CustomerColor.ColorId
                         where OM.OrderId=" + ViewState["order_id"];
            }


            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
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
            string STR1 = @"select distinct  oi.OrderDetailId,oi.Photo from order_referenceimage as oi  inner join orderdetail od on od.OrderDetailid=oi.OrderDetailid 
                            where orderid=" + ViewState["order_id"] + "";
            SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);
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
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }

        }

        else if (Convert.ToInt32(ddPreview.SelectedValue) == 2)
        {
            Session["ReportPath"] = "Reports/LocalOC.rpt";
            Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"];
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);

        }

    }
    protected void RptForRasindiaType1()
    {
        string STR = "";
        DataSet ds;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        Session["ReportPath"] = "Reports/RptPerFormaInvoiceordenewrforraasindia.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceordenewrforraasindia.xsd";
        STR = @" SELECT CSI.CompanyName,CSI.Address,CMI.CompanyName,CMI.CompAddr1,CMI.CompAddr2,CMI.CompAddr3,CMI.CompFax,CMI.CompTel,CMI.TinNo,CMI.Email,OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID,
                      OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DispatchDate AS DeliveryDate, T.TermName, 
                      P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id,
                      OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount
                      AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight,
                      OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,
                      OM.LocalOrder,OM.Custorderdate,OM.duedate,vf.ITEM_NAME  ,vf.QualityName ,vf.designName,vf.ColorName,vf.ShapeName,vf.SizeFt,
                      B.BankName, B.Street, B.City,B.State, B.Country, B.ACNo, B.SwiftCode, B.PhoneNo, 
                       B.FaxNo,U.UnitName,CS.Length,CS.Width,CS.Height,CS.PCS,CS.UnitId,UP.UPCNO,vf.ShapeId,CustomerColor.ColorNameToC,CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
                      CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC,DOM.exfactorydate,Shipto,PerformaInvoiceNo,CSI.EMail,DOD.HTScode," + Session["varcompanyId"] + @" as MastercompanyId,Vf.ProductCode FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS 
                     OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId 
                     LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN 
                     dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                      LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId inner join Companyinfo CMI on CMI.COmpanyId=OM.CompanyId
                     Inner join Customerinfo CSI on CSI.Customerid=OM.Customerid inner join V_FInishedItemDetail Vf on Vf.Item_Finished_Id=OD.Item_Finished_Id
                      inner join Unit U on U.UNitid=Od.OrderUnitId left outer join CONTAINERCOST CS on CS.DraftOrderDetailId=OD.OrderDetailId
                     left outer join UPCNO UP on UP.Customerid=CSI.Customerid And UP.Finishedid=Vf.Item_Finished_id
                      inner join Bank B on CMI.BankId=B.BankId LEFT OUTER JOIN CustomerDesign CustomerDesign ON 
                     CSI.CustomerId=CustomerDesign.CustomerId AND vf.designId=CustomerDesign.DesignId LEFT OUTER JOIN 
                     CustomerSize CustomerSize ON CSI.CustomerId=CustomerSize.CustomerId AND Vf.SizeId=CustomerSize.Sizeid 
                     LEFT OUTER JOIN CustomerQuality CustomerQuality ON CSI.CustomerId=CustomerQuality.CustomerId AND 
                     Vf.QualityId=CustomerQuality.QualityId LEFT OUTER JOIN CustomerColor CustomerColor ON 
                       CSI.CustomerId=CustomerColor.CustomerId AND Vf.ColorId=CustomerColor.ColorId left outer join Draft_order_master as DOM on DOM.orderid=OM.orderid 
                      left outer join Draft_order_detail as DOD on DOD.Orderdetailid=OD.Orderdetailid
                       where OM.OrderId=" + ViewState["order_id"];


        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
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
        string STR1 = @"select distinct  oi.OrderDetailId,oi.Photo from order_referenceimage as oi  inner join orderdetail od on od.OrderDetailid=oi.OrderDetailid 
                            where orderid=" + ViewState["order_id"] + "";
        SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
        DataTable dt1 = new DataTable();
        sda1.Fill(dt1);
        ds.Tables.Add(dt1);
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
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    private void ReportForrassindia()
    {
        string STR = "";
        DataSet ds = new DataSet();

        //sda.Fill(dt);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        if (Convert.ToInt32(ddPreview.SelectedValue) == 1)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "10":
                    Session["ReportPath"] = "Reports/RptPerFormaInvoiceordenewrforraasindia.rpt";
                    Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceordenewrforraasindia.xsd";
                    break;
                default:
                    Session["ReportPath"] = "Reports/RptPerFormaInvoiceorderNew.rpt";
                    Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceorderNew.xsd";
                    break;
            }

            STR = @" SELECT CSI.CompanyName,CSI.Address,CMI.CompanyName,CMI.CompAddr1,CMI.CompAddr2,CMI.CompAddr3,CMI.CompFax,CMI.CompTel,CMI.TinNo,CMI.Email,OM.CompanyId, OM.CustomerId, OM.LocalOrder AS ORDERNO,OD.ORDERCALTYPE,OD.OrderUnitId AS UNITID,
                      OM.OrderDate, OM.Status, OM.CustomerOrderNo AS CustOrderNo, OM.DispatchDate AS DeliveryDate, T.TermName, 
                      P.PaymentName,TM.transmodeName, GR.StationName, OM.SeaPort, OD.OrderDetailId, OD.OrderId, OD.Item_Finished_Id,
                      OD.QtyRequired AS QTY, OD.UnitRate AS RATE, OD.TotalArea AS AREA, OD.Amount, OD.CurrencyId, OD.DiscountAmount
                      AS DISAMOUNT, OD.Warehouseid, OD.CancelQty, OD.HoldQty, OD.QualityCodeId, OD.Remarks, OD.Photo, OD.Weight,
                      OD.OURCODE, OD.BUYERCODE, '' AS PKGINSTRUCTION, '' AS LBGINSTRUCTION, CI.CurrencyName, '' AS DeliveryComments,
                      OM.LocalOrder,OM.Custorderdate,OM.duedate,vf.ITEM_NAME  ,vf.QualityName ,vf.designName,vf.ColorName,vf.ShapeName,vf.SizeFt,
                      B.BankName, B.Street, B.City,B.State, B.Country, B.ACNo, B.SwiftCode, B.PhoneNo, 
                       B.FaxNo,U.UnitName,CS.Length,CS.Width,CS.Height,CS.PCS,CS.UnitId,UP.UPCNO,vf.ShapeId,CustomerColor.ColorNameToC,CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
                      CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC,DOM.exfactorydate,Shipto,PerformaInvoiceNo,CSI.EMail,DOD.HTScode," + Session["varcompanyId"] + @" as MastercompanyId,Vf.ProductCode FROM dbo.OrderMaster AS OM INNER JOIN dbo.OrderDetail AS 
                     OD ON OM.OrderId = OD.OrderId INNER JOIN dbo.CurrencyInfo AS CI ON OD.CurrencyId = CI.CurrencyId 
                     LEFT OUTER JOIN dbo.GoodsReceipt AS GR ON OM.PortOfLoading = GR.GoodsreceiptId LEFT OUTER JOIN 
                     dbo.TransMode AS TM ON OM.ByAirSea = TM.transmodeId LEFT OUTER JOIN dbo.Payment AS P ON OM.PaymentId = P.PaymentId 
                      LEFT OUTER JOIN dbo.Term AS T ON OM.TermId = T.TermId inner join Companyinfo CMI on CMI.COmpanyId=OM.CompanyId
                     Inner join Customerinfo CSI on CSI.Customerid=OM.Customerid inner join V_FInishedItemDetail Vf on Vf.Item_Finished_Id=OD.Item_Finished_Id
                      inner join Unit U on U.UNitid=Od.OrderUnitId left outer join CONTAINERCOST CS on CS.DraftOrderDetailId=OD.OrderDetailId
                     left outer join UPCNO UP on UP.Customerid=CSI.Customerid And UP.Finishedid=Vf.Item_Finished_id
                      inner join Bank B on CMI.BankId=B.BankId LEFT OUTER JOIN CustomerDesign CustomerDesign ON 
                     CSI.CustomerId=CustomerDesign.CustomerId AND vf.designId=CustomerDesign.DesignId LEFT OUTER JOIN 
                     CustomerSize CustomerSize ON CSI.CustomerId=CustomerSize.CustomerId AND Vf.SizeId=CustomerSize.Sizeid 
                     LEFT OUTER JOIN CustomerQuality CustomerQuality ON CSI.CustomerId=CustomerQuality.CustomerId AND 
                     Vf.QualityId=CustomerQuality.QualityId LEFT OUTER JOIN CustomerColor CustomerColor ON 
                       CSI.CustomerId=CustomerColor.CustomerId AND Vf.ColorId=CustomerColor.ColorId left outer join Draft_order_master as DOM on DOM.orderid=OM.orderid 
                      left outer join Draft_order_detail as DOD on DOD.Orderdetailid=OD.Orderdetailid
                       where OM.OrderId=" + ViewState["order_id"];


            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
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
            string STR1 = @"select distinct  oi.OrderDetailId,oi.Photo from order_referenceimage as oi  inner join orderdetail od on od.OrderDetailid=oi.OrderDetailid 
                            where orderid=" + ViewState["order_id"] + "";
            SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);
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
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }

        }

        else if (Convert.ToInt32(ddPreview.SelectedValue) == 2)
        {
            Session["ReportPath"] = "Reports/LocalOC.rpt";
            Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["order_id"];
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);

        }

    }
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //a.Delete(name);
        if (hnsst.Value == "true")
        {
            DataSet Ds = null;
            int VarDetailId;
            DataSet Ds1, Ds2, Ds3;
            Lblmessage.Text = "";
            LblErrorMessage.Text = "";

            if (HDF1.Value == "7")
                VarDetailId = Convert.ToInt32(DGOrderDetail2.DataKeys[e.RowIndex].Value);
            else
                VarDetailId = Convert.ToInt32(DGOrderDetail.DataKeys[e.RowIndex].Value);
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //string Str = "Select orderid,Item_Finished_ID,OrderDetailId From OrderDetail Where OrderDetailId=" + VarDetailId + "";
                //Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select orderid,orderdetailid,Item_finished_id From OrderDetail Where OrderDetailId=" + VarDetailId + "");
                //if (Ds.Tables[0].Rows.Count > 0)
                //{
                //    Ds1 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select orderid From JobAssigns Where OrderId=" + Ds.Tables[0].Rows[0]["OrderId"] + " And Item_Finished_ID=" + Ds.Tables[0].Rows[0]["Item_Finished_ID"] + "");
                //    if (Ds1.Tables[0].Rows.Count > 0)
                //    {
                //        Ds2 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select Process_Name_Id,Process_Name From Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + " and Process_name_id in(1,16) Order By Process_Name_Id");
                //        if (Ds2.Tables[0].Rows.Count > 0)
                //        {
                //            for (int i = 0; i < Ds2.Tables[0].Rows.Count; i++)
                //            {
                //                Ds3 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select Item_Finished_ID from Process_issue_Master_" + Ds2.Tables[0].Rows[i]["Process_Name_Id"] + " PIM inner join PROCESS_ISSUE_DETAIL_" + Ds2.Tables[0].Rows[i]["Process_Name_Id"] + " PID on PIM.issueorderid=PID.issueorderid and PIM.status<>'canceled' Where PID.OrderId=" + Ds.Tables[0].Rows[0]["OrderId"] + " And PID.Item_Finished_ID=" + Ds.Tables[0].Rows[0]["Item_Finished_ID"] + "");
                //                if (Ds3.Tables[0].Rows.Count > 0)
                //                {
                //                    LblErrorMessage.Visible = true;
                //                    LblErrorMessage.Text = "AlReady Issue To " + Ds2.Tables[0].Rows[i]["Process_Name"] + " Process";
                //                    //i = Ds2.Tables[0].Rows.Count;
                //                    return;
                //                }
                //            }
                //        }
                //    }
                if (LblErrorMessage.Text == "")
                {
                    SqlParameter[] _arrpara = new SqlParameter[6];
                    //_arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                    _arrpara[0] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@VarRow", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 300);
                    _arrpara[3] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                    _arrpara[4] = new SqlParameter("@userid", SqlDbType.Int);

                    // _arrpara[0].Value = Ds.Tables[0].Rows[0]["OrderId"];
                    _arrpara[0].Value = VarDetailId;
                    _arrpara[1].Value = 1;
                    _arrpara[2].Direction = ParameterDirection.Output;
                    _arrpara[3].Value = Session["varcompanyid"];
                    _arrpara[4].Value = Session["varuserid"];
                    if (DGOrderDetail.Rows.Count == 1)
                    {
                        _arrpara[1].Value = 0;
                    }
                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteOrder_Row", _arrpara);
                    //if (HDF1.Value == "7")
                    //{
                    //    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "delete from OrderLocalConsumption where orderdetailid=" + _arrpara[1].Value + " and orderid=" + _arrpara[0].Value + "");
                    //}
                    tran.Commit();
                    Fill_Grid();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[2].Value.ToString();
                    //LblErrorMessage.Text = "Successfully Deleted..";
                }
                //}
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    tdtab2.Visible = true;
    //    tdtab1.Visible = false;
    //    trsave.Visible = true;
    //    //tdbtn11.Visible = true;
    //}
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    tdtab2.Visible = false;
    //    tdtab1.Visible = true;
    //    trsave.Visible = false;
    //    //tdbtn11.Visible = true;
    //}
    //protected void Gvchklist_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void Txtcustorderdt_TextChanged(object sender, EventArgs e)
    {
        TxtDispatchDate.Text = Txtcustorderdt.Text;
    }
    protected void Refreshgrid_Click(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void refreshPhotoRefImage_Click(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    public string getgiven(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(qty),0) from OrderLocalConsumption where orderdetailid=" + strVal + " and orderid=" + strval1 + "");
        val = ds.Tables[0].Rows[0][0].ToString();
        return val;
    }
    public void colour()
    {
        for (int i = 0; i < DGOrderDetail2.Rows.Count; i++)
        {
            string consump = ((Label)DGOrderDetail2.Rows[i].FindControl("lblconsumption")).Text;
            if (consump != "" && consump != "0")
            {
                DGOrderDetail2.Rows[i].BackColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void Chksupply_CheckedChanged(object sender, EventArgs e)
    {
        BtnShowConsumption.Visible = true;
        BtnShowConsumption.Enabled = true;
    }
    protected void RefereshBuyerMasterCode_Click(object sender, EventArgs e)
    {
        ItemSelectedChange();
        fillCombo();
    }

    protected void BtncostReport_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/RptPerFormaInvoiceCostorder.rpt";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        DataSet Ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='VIEW_PERFORMAINVOICEFORDESTINIorder'");
        if (Ds1.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICEFORDESTINIorder");
        }
        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINIorder AS ";
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        Str = Str + "SELECT PD.PROCESSID,PD.FINISHEDID,PD.IFINISHEDID,PD.OFINISHEDID,DOD.ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Str = Str + ",[dbo].[GET_FINAMT](PD.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PD.IFINISHEDID) '" + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "'";
        }
        Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        if (variable.VarNewQualitySize == "1")
        {
            Str = Str + @",DOD.PHOTO IMAGE,DOD.QtyRequired OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PD.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,DOD.DESCRIPTION,dod.weight,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as pdescription
            From  ORDER_CONSUMPTION_DETAIL PD,ORDERDETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT,V_FinishedItemDetailNew vd WHERE pd.orderid=dod.orderid and  Pd.FINISHEDID=DOD.ITEM_FINISHED_ID AND Pd.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND vd.item_finished_id=pd.IFINISHEDID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["order_id"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "";
        }
        else
        {
            Str = Str + @",DOD.PHOTO IMAGE,DOD.QtyRequired OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PD.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,DOD.DESCRIPTION,dod.weight,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as pdescription
            From  ORDER_CONSUMPTION_DETAIL PD,ORDERDETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT,V_FinishedItemDetail vd WHERE pd.orderid=dod.orderid and  Pd.FINISHEDID=DOD.ITEM_FINISHED_ID AND Pd.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND vd.item_finished_id=pd.IFINISHEDID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["order_id"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "";
        }
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        con.Close();
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICEFORDESTINI.Orderid}=" + ViewState["order_id"] + "";
        DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from VIEW_PERFORMAINVOICEFORDESTINIorder where orderid=" + ViewState["order_id"] + "");
        if (ds4.Tables[0].Rows.Count > 0)
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ReportViewer.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, fullscreen=no, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void TxtLocalOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtCustOrderNo_Validate();
    }
    protected void BtnReport_Click(object sender, EventArgs e)
    {
        //if (ddPreview.SelectedValue == "3")
        //{
        //    costingReport();
        //}
        //else
        //{
        Report_Type();
        //}
    }
    protected void costingReport()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        if (Session["varcompanyNo"].ToString() == "12")
        {
            string str;
            if (variable.VarNewQualitySize == "1")
            {
                str = @"select CM.CompanyName,Customercode,isnull(OM.CustomerOrderNo,0) as CustomerOrderNo,VF.ITEM_NAME,Category_Name+' '+VF.QualityName+' '+VF.designName+' '+VF.ShadeColorName+' '+VF.ShapeName+' '+CASE 
                WHEN OLC.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,Qty from OrderLocalConsumption OLC inner join v_finisheditemdetailNew VF on OLC.FinishedId=VF.Item_Finished_Id inner join Ordermaster OM on
                OLC.Orderid=OM.orderid  inner join customerinfo CI on OM.Customerid=CI.Customerid inner join Companyinfo CM on OM.Companyid=CM.Companyid where VF.MasterCompanyId=" + Session["varCompanyId"] + " and OM.Companyid=" + DDCompanyName.SelectedValue + " and om.orderid=" + ViewState["order_id"] + "";
            }
            else
            {
                str = @"select CM.CompanyName,Customercode,isnull(OM.CustomerOrderNo,0) as CustomerOrderNo,VF.ITEM_NAME,Category_Name+' '+VF.QualityName+' '+VF.designName+' '+VF.ShadeColorName+' '+VF.ShapeName+' '+CASE 
                WHEN OLC.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,Qty from OrderLocalConsumption OLC inner join v_finisheditemdetail VF on OLC.FinishedId=VF.Item_Finished_Id inner join Ordermaster OM on
                OLC.Orderid=OM.orderid  inner join customerinfo CI on OM.Customerid=CI.Customerid inner join Companyinfo CM on OM.Companyid=CM.Companyid where VF.MasterCompanyId=" + Session["varCompanyId"] + " and OM.Companyid=" + DDCompanyName.SelectedValue + " and om.orderid=" + ViewState["order_id"] + "";
            }

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptOrderCostingConsumptionREP.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptOrderCostingConsumptionREP.xsd";
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
        else
        {
            SqlParameter[] _array = new SqlParameter[3];
            _array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[1] = new SqlParameter("@OrderId", SqlDbType.Int);

            _array[0].Value = DDCompanyName.SelectedValue;
            _array[1].Value = ViewState["order_id"];

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_OrderCosting", _array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["varcompanyNo"].ToString() == "44")
                {
                    Session["rptFileName"] = "~\\Reports\\RptOrderPOConsumption.rpt";
                }
                else if (Session["varcompanyNo"].ToString() == "45")
                {
                    Session["rptFileName"] = "~\\Reports\\RptOrderCostingmws.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptOrderCosting.rpt";

                }
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptOrderCosting.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
    }

    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillColor();
        RecipeNameFill();
    }
    protected void RecipeNameFill()
    {
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[9];

            _arrpara[0] = new SqlParameter("@CUSTOMERID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@CategoryID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ItemID", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@QualityID", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@DesignID", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@MSG", SqlDbType.VarChar, 300);
            _arrpara[6] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@RecipeNameID", SqlDbType.Int);

            _arrpara[0].Value = DDCustomerCode.SelectedValue;
            _arrpara[1].Value = DDItemCategory.SelectedValue;
            _arrpara[2].Value = DDItemName.SelectedValue;
            _arrpara[3].Value = DDQuality.SelectedValue;
            _arrpara[4].Value = DDDesign.SelectedValue;
            _arrpara[5].Direction = ParameterDirection.Output;
            _arrpara[6].Value = Session["varcompanyid"];
            _arrpara[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_RECIPENAMEID", _arrpara);
            if (_arrpara[5].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please define recipe for this item');", true);
            }
            DDRecipeName.SelectedValue = _arrpara[7].Value.ToString();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        shapeselectedindexchange();
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
    protected void DGOrderDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet ddd = new DataSet();
        ddd = GetDetail();
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
        DGOrderDetail.DataSource = sortedView;
        DGOrderDetail.DataBind();
    }
    protected void GeneratePerformainvoiceNo()
    {
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@orderid", ViewState["order_id"]);
        param[1] = new SqlParameter("@invoiceNo", "");
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //'***********
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_generatePerformainvoiceNo", param);
    }

    protected void btnupdateallconsmp_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        try
        {
            int rowcount = DGOrderDetail.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                Label lblFinishedid = (Label)DGOrderDetail.Rows[i].FindControl("lblFinishedid");
                int orderdetailid = Convert.ToInt32(DGOrderDetail.DataKeys[i].Value);
                UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(lblFinishedid.Text), Convert.ToInt32(ViewState["order_id"]), orderdetailid, VARUPDATE_FLAG: 1, UPDATECURRENTCONSUMPTION: 1, effectivedate: TxtOrderDate.Text);
            }

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Exec Pro_UpdateRecipeNameID " + Session["varcompanyId"] + ", " + Session["Varuserid"] + ", " + ViewState["order_id"]);

            //Update status
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Exec Pro_Updatestatus " + Session["varcompanyId"] + "," + Session["Varuserid"] + ",'Order_Consumption_Detail'," + ViewState["order_id"] + ",'Consumption Updated'");
            //
            ScriptManager.RegisterStartupScript(Page, GetType(), "a", "alert('Consumption Updated successfully...')", true);
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
    }

    protected void PerformainvoiceSamara()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");
            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 16.43;
            sht.Columns("B").Width = 15.29;
            sht.Columns("C").Width = 13.43;
            sht.Columns("D").Width = 7.71;
            sht.Columns("E").Width = 3.14;
            sht.Columns("F").Width = 1.71;

            sht.Columns("G").Width = 4.29;
            sht.Columns("H").Width = 2.29;
            sht.Columns("I").Width = 3.86;

            sht.Columns("J").Width = 4.29;
            sht.Columns("K").Width = 2.29;
            sht.Columns("L").Width = 3.86;

            sht.Columns("M").Width = 4.29;
            sht.Columns("N").Width = 2.29;
            sht.Columns("O").Width = 3.86;

            sht.Columns("P").Width = 6.57;
            sht.Columns("Q").Width = 7.57;

            sht.Columns("R").Width = 10.14;
            sht.Columns("S").Width = 6.71;

            sht.Row(1).Height = 21;
            //**************
            sht.Range("A1:S1").Merge();
            sht.Range("A1").Value = "P R O F O R M A   I N V O I C E";
            sht.Range("A1:S1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:S1").Style.Font.FontSize = 16;
            sht.Range("A1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************
            sht.Range("A2:D2").Merge();
            sht.Range("A2:D2").Style.Font.FontSize = 11;
            sht.Range("A2:D2").Style.Font.Bold = true;


            sht.Range("A2").Value = "Exporter";
            sht.Range("A3:D3").Style.Font.Bold = true;
            sht.Range("A3:D3").Style.Font.FontSize = 13;


            sht.Range("A3:D3").Merge();
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A4:D4").Merge();
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A5:D5").Merge();
            sht.Range("A5").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"] + " FAX : " + ds.Tables[0].Rows[0]["Compfax"];
            sht.Range("A6:D6").Merge();
            sht.Range("A6").Value = "E-MAIL : " + ds.Tables[0].Rows[0]["email"];
            //'Invoice NO Date
            sht.Range("E2:H2").Merge();
            sht.Range("E2:H3").Style.Font.FontSize = 11;
            sht.Range("E2:H3").Style.Font.Bold = true;
            sht.Range("E2").Value = "Invoice No.";
            //'value
            sht.Range("I2:K2").Merge();
            sht.Range("I2").SetValue(ds.Tables[0].Rows[0]["invoiceno"]);

            sht.Range("E3:H3").Merge();
            sht.Range("E3").Value = "Invoice Date";
            sht.Range("I3:K3").Merge();
            sht.Range("I3").SetValue(ds.Tables[0].Rows[0]["invoicedate"]);
            //********Exporterref
            sht.Range("L2:S2").Merge();
            sht.Range("L2:S2").Style.Font.FontSize = 11;
            sht.Range("L2:S2").Style.Font.Bold = true;
            sht.Range("L2").Value = "Exporter's Ref.";
            //'value
            sht.Range("L3:S3").Merge();
            sht.Range("L3").Value = "";
            //*******
            sht.Range("E4:S4").Merge();
            sht.Range("E4").Value = "Buyer's Order No. & date";
            sht.Range("E4:S4").Style.Font.FontSize = 11;
            sht.Range("E4:S4").Style.Font.Bold = true;

            sht.Range("E5:S5").Merge();
            sht.Range("E5").Value = "PO# " + ds.Tables[0].Rows[0]["customerorderNo"] + "/" + ds.Tables[0].Rows[0]["orderdate"];

            //'Other ref
            sht.Range("E6:S6").Merge();
            sht.Range("E6").Value = "Other Reference(s) :";
            sht.Range("E6:S6").Style.Font.FontSize = 11;
            sht.Range("E6:S6").Style.Font.Bold = true;
            sht.Range("E7:S7").Merge();
            sht.Range("E7").Value = "";

            //'consignee

            sht.Range("A8:D8").Merge();
            sht.Range("A8:D8").Style.Font.FontSize = 11;
            sht.Range("A8:D8").Style.Font.Bold = true;
            sht.Range("A8").Value = "Consignee";
            //'value
            sht.Range("A9:D9").Merge();
            sht.Range("A9").Value = ds.Tables[0].Rows[0]["customercompany"];
            sht.Range("A10:D11").Merge();
            sht.Range("A10").Value = ds.Tables[0].Rows[0]["customeraddress"];
            sht.Range("A10:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A10:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A10").Style.Alignment.WrapText = true;

            sht.Range("A12:D12").Merge();
            sht.Range("A12").Value = "TEL :" + ds.Tables[0].Rows[0]["customerphoneno"] + " FAX : " + ds.Tables[0].Rows[0]["customerfax"];
            sht.Range("A13:D13").Merge();
            sht.Range("A13").Value = "E-MAIL :" + ds.Tables[0].Rows[0]["customermail"];
            sht.Range("A14:D14").Merge();
            sht.Range("A15:D15").Merge();
            //'*******Buyerotherthanconsignee

            sht.Range("E8:S8").Merge();
            sht.Range("E8").Value = "Buyer (if other than consignee)";
            sht.Range("E8:S8").Style.Font.FontSize = 11;
            sht.Range("E8:S8").Style.Font.Bold = true;

            //'value
            sht.Range("E9:S13").Merge();
            sht.Range("E9").Value = ds.Tables[0].Rows[0]["BuerOtherThanConsigneeSea"];
            sht.Range("E9:S13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("E9:S13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("E9").Style.Alignment.WrapText = true;

            //'
            //sht.Range("E10:M10").Merge();
            //sht.Range("E10:M10").Style.Font.FontSize = 11;
            //sht.Range("E10:M10").Style.Font.Bold = true;
            //sht.Range("E10").SetValue("SHIP DATE :- " + ds.Tables[0].Rows[0]["Dispatchdate"]);
            //'******Country of origin of goods
            sht.Range("E14:J14").Merge();
            sht.Range("E14:J14").Style.Font.FontSize = 11;
            sht.Range("E14:J14").Style.Font.Bold = true;
            sht.Range("E14").Value = "Country of Origin of goods";
            //'value
            sht.Range("E15:H15").Merge();
            sht.Range("E15:H15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E15").Value = "INDIA";
            //'Country of final destination
            sht.Range("K14:S14").Merge();
            sht.Range("K14:S14").Style.Font.FontSize = 11;
            sht.Range("K14:S14").Style.Font.Bold = true;
            sht.Range("K14").Value = "Country of Final Destination";
            //'value
            sht.Range("K15:S15").Merge();
            sht.Range("K15:S15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("K15").Value = ds.Tables[0].Rows[0]["countryname"];
            //'****Precarrigeby

            sht.Range("A16").Value = "Pre-Carriage by";
            sht.Range("A16").Style.Font.FontSize = 11;
            sht.Range("A16").Style.Font.Bold = true;
            //'
            sht.Range("A17").Value = ds.Tables[0].Rows[0]["Precarriageby"];
            //'*****Place of Receipt
            sht.Range("B16:D16").Merge();
            sht.Range("B16:D16").Style.Font.FontSize = 11;
            sht.Range("B16:D16").Style.Font.Bold = true;
            sht.Range("B16").Value = "Place of Receipt";
            //'
            sht.Range("B17:D17").Merge();
            sht.Range("B17").Value = ds.Tables[0].Rows[0]["Placeofreceipt"];
            //'*****vessel_flight no
            sht.Range("A18").Value = "Mode of Shipment";
            sht.Range("A18").Style.Font.FontSize = 11;
            sht.Range("A18").Style.Font.Bold = true;

            //'
            sht.Range("A19").Value = ds.Tables[0].Rows[0]["modeofshipment"];
            //'**********port of loading
            sht.Range("B18:D18").Merge();
            sht.Range("B18:D18").Style.Font.FontSize = 11;
            sht.Range("B18:D18").Style.Font.Bold = true;
            sht.Range("B18").Value = "Port of Loading";
            //'
            sht.Range("B19:D19").Merge();
            sht.Range("B19").Value = ds.Tables[0].Rows[0]["portofloading"];
            //'********Port of discharge
            sht.Range("A20").Value = "Port of Discharge";
            sht.Range("A20").Style.Font.FontSize = 11;
            sht.Range("A20").Style.Font.Bold = true;
            //'
            sht.Range("A21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //'***********Final destination
            sht.Range("B20:D20").Merge();
            sht.Range("B20:D20").Style.Font.FontSize = 11;
            sht.Range("B20:D20").Style.Font.Bold = true;

            sht.Range("B20").Value = "Final Destination";
            //'
            sht.Range("B21:D21").Merge();
            sht.Range("B21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //SHIPDATE
            sht.Range("E20:S20").Merge();
            sht.Range("E20").Value = "SHIP DATE :-" + ds.Tables[0].Rows[0]["Dispatchdate"];
            sht.Range("E20:S20").Style.Font.FontSize = 11;
            sht.Range("E20:S20").Style.Font.Bold = true;
            sht.Range("E20:S20").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //term of sale
            sht.Range("E16:S16").Merge();
            sht.Range("E16").Value = "Terms of Delivery and Payment";
            sht.Range("E16:S16").Style.Font.FontSize = 11;
            sht.Range("E16:S16").Style.Font.Bold = true;
            sht.Range("E18:S18").Merge();
            sht.Range("E19:S19").Merge();
            sht.Range("E18").Value = ds.Tables[0].Rows[0]["Modofpayment"];
            sht.Range("E19").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
            //'*****Mark & Nos.
            sht.Range("A22:B23").Style.Font.FontSize = 11;
            sht.Range("A22:B23").Style.Font.Bold = true;
            sht.Range("A22").Value = "Marks & Nos.";
            sht.Range("B22").Value = "No. & Kind";
            sht.Range("B23").Value = "of Packages";
            //'******Description of goods
            sht.Range("C22:D22").Merge();
            sht.Range("C22").Value = "Description of Goods";
            sht.Range("C22:D22").Style.Font.FontSize = 11;
            sht.Range("C22:D22").Style.Font.Bold = true;
            //'******Detail Headers
            sht.Range("A24:S26").Style.Font.FontSize = 10;
            sht.Range("A24:S26").Style.Font.Bold = true;
            sht.Range("A24:S26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A24:A26").Merge();
            sht.Range("A24:A26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A24:A26").Style.Font.FontSize = 12;
            sht.Range("A24").Value = "QUALITY";

            sht.Range("B24:B26").Merge();
            sht.Range("B24:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B24:B26").Style.Font.FontSize = 12;
            sht.Range("B24").Value = "DESIGN";

            sht.Range("C24:C26").Merge();
            sht.Range("C24:C26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C24:C26").Style.Font.FontSize = 12;
            sht.Range("C24").Value = "COLOR";


            sht.Range("D24:F26").Merge();
            sht.Range("D24:F26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D24:F26").Style.Font.FontSize = 12;
            sht.Range("D24").Value = "SIZE";

            sht.Range("G24:I26").Merge();
            sht.Range("G24:I26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G24:I26").Style.Font.FontSize = 12;
            sht.Range("G24").Value = "AREA";

            sht.Range("J24:L26").Merge();
            sht.Range("J24:L26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J24:L26").Style.Font.FontSize = 12;
            sht.Range("J24").Value = "QUANTITY";

            sht.Range("M24:O26").Merge();
            sht.Range("M24:O26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("M24").Value = "TOTAL AREA";

            sht.Range("P24:Q26").Merge();
            sht.Range("P24:Q26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P24").Value = "RATE";

            sht.Range("P25:Q25").Merge();
            sht.Range("P25:Q25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P25").Value = ds.Tables[0].Rows[0]["Modofpayment"] + " " + ds.Tables[0].Rows[0]["Currencyname"];

            string unitname;
            if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "0")
            {
                switch (ds.Tables[0].Rows[0]["unitid"].ToString())
                {
                    case "1":
                        unitname = "SQ. Mtr";
                        break;
                    case "2":
                        unitname = "SQ. Ft";
                        break;
                    default:
                        unitname = ds.Tables[0].Rows[0]["unitname"].ToString();
                        break;
                }
            }
            else
            {
                unitname = "PC";
            }
            sht.Range("P26:Q26").Merge();
            sht.Range("P26:Q26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P26").Value = "Per " + unitname;

            sht.Range("R25:S25").Merge();
            sht.Range("R25:S25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("R25:S25").Value = "AMOUNT";

            sht.Range("R26:S26").Merge();
            sht.Range("R26:S26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("R26:S26").Value = ds.Tables[0].Rows[0]["currencyname"];

            //*****************
            int i;
            i = 27;
            int LastRowId = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":S" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":S" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":S" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["Qualityname"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["Designname"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["colorname"]);
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);

                sht.Range("G" + i + ":I" + i).Merge();
                sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["totalarea"]);

                sht.Range("J" + i + ":L" + i).Merge();
                sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);

                sht.Range("M" + i + ":O" + i).Merge();
                Decimal TotalArea = 0;
                TotalArea = Convert.ToDecimal(ds.Tables[0].Rows[j]["totalarea"]) * Convert.ToDecimal(ds.Tables[0].Rows[j]["qtyrequired"]);
                sht.Range("M" + i).SetValue(TotalArea);

                sht.Range("P" + i + ":Q" + i).Merge();
                sht.Range("P" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);

                sht.Range("R" + i + ":S" + i).Merge();
                sht.Range("R" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                //sht.Range("G" + i + ":I" + i).Merge();
                //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                //sht.Range("J" + i + ":K" + i).Merge();
                //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                //sht.Range("L" + i + ":M" + i).Merge();
                //sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                using (var a = sht.Range("A" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("B" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("C" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("D" + i + ":F" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("G" + i + ":I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i + ":L" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("M" + i + ":O" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("P" + i + ":Q" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("R" + i + ":S" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                i = i + 1;
                LastRowId = i;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("J" + i + ":L" + i).Merge();
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("M" + i + ":O" + i).Merge();
            using (var a = sht.Range("M" + i + ":O" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("R" + i + ":S" + i).Merge();
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("J" + i + ":L" + i).Merge();
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }

            sht.Range("M" + i + ":O" + i).Merge();
            using (var a = sht.Range("M" + i + ":O" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }

            sht.Range("R" + i + ":S" + i).Merge();
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("J" + i + ":L" + i).Merge();
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("M" + i + ":O" + i).Merge();
            using (var a = sht.Range("M" + i + ":O" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("R" + i + ":S" + i).Merge();
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            //******
            i = i + 1;
            sht.Range("A" + i + ":S" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":S" + i).Style.Font.FontSize = 10;

            sht.Range("J" + i + ":L" + i).Merge();
            sht.Range("J" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));

            sht.Range("M" + i + ":O" + i).Merge();
            //sht.Range("M" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));
            sht.Range("M" + i).FormulaA1 = "=sum(M" + 27 + ':' + "$M$" + LastRowId + ")";

            sht.Range("R" + i + ":S" + i).Merge();
            sht.Range("R" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            using (var a = sht.Range("A" + i + ":S" + i))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL PCS :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", "") + " PCS");

            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL AMOUNT :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(amount)", "") + " " + ds.Tables[0].Rows[0]["currencyname"]);
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            string unit = "";

            switch (ds.Tables[0].Rows[0]["flagsize"].ToString())
            {
                case "1":
                    unit = "SQ.MTR";
                    break;
                case "0":
                    unit = "SQ.FT";
                    break;
            }
            sht.Range("A" + i).Value = "TOTAL " + unit + " :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(totalarea)", "") + " " + unit);
            //sht.Range("B" + i).FormulaA1 = "=sum(M" + 27 + ':' + "$M$" + LastRowId + ")";
            sht.Range("B" + i).SetValue(sht.Evaluate("=sum(M" + 27 + ':' + "$M$" + LastRowId + ")") + " " + unit);

            //*************amount in words
            //string amountinwords = "";
            //Decimal Amount = Convert.ToDecimal(ds.Tables[0].Compute("sum(amount)", ""));
            //amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
            //string val = "", paise = "";
            //if (Amount.ToString().IndexOf('.') > 0)
            //{
            //    val = Amount.ToString().Split('.')[1];
            //    if (Convert.ToInt32(val) > 0)
            //    {
            //        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
            //    }
            //}
            //amountinwords = ds.Tables[0].Rows[0]["currencyname"] + " " + amountinwords + " " + paise + "Only";
            //i = i + 1;
            //sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //sht.Range("M" + i + ":M" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //sht.Range("A" + i + ":I" + (i + 1)).Merge();
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.WrapText = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Font.Bold = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            //sht.Range("A" + i).Value = amountinwords;
            //Bank Details  
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Beneficiary:";
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["bankname"] + "," + ds.Tables[0].Rows[0]["City"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Value = "SWIFT CODE :";
            sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["swiftcode"];
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account:";
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["Accountname"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account no." + ds.Tables[0].Rows[0]["acno"];
            //signature and date
            i = i + 1;
            sht.Range("J" + i + ":S" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("J" + i + ":S" + i).Merge();
            sht.Range("J" + i).Value = "For " + ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //Declaration
            i = i + 1;
            sht.Range("A" + i).Value = "Declaration:";
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //*************
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "We declare that this invoice show the actual price of the goods";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "described and that all particulars are true and correct";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":S" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //***********Borders
            sht.Range("A1:S1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:A29").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("D2:D26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A7:S7").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E3:S3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("K2:K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("H2:H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("S2:S29").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("E5:S5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A15:S15").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E13:S13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("J14:J15").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A17:D17").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A19:D19").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A21:S21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A16:A26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("B22:B26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A26:S26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            using (var a = sht.Range("A24:A26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("C24:C26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("G24:I26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("J24:L26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("M24:O26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("P24:Q26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("R24:S26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            //using (var a = sht.Range("K24:K26"))
            //{
            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //}
            sht.Range("A23:S23").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //***********

            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoiceSamara-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

            //Session["rptFileName"] = "~\\Reports\\Rptorderperformainvoice.rpt";                   
            //Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\Rptorderperformainvoice.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }

    protected void btnchngorderno_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        txtoldcustorderno.Text = OldCustOrderNo;
        txtnewcustorderno.Text = "";
    }
    protected void btnchangeorderno_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //ViewState["order_id"]
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@oldorderno", txtoldcustorderno.Text);
            param[1] = new SqlParameter("@neworderno", txtnewcustorderno.Text);
            param[2] = new SqlParameter("@orderid", ViewState["order_id"]);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGECUSTORDERNO", param);
            LblErrorMessage.Text = param[5].Value.ToString();
            LblErrorMessage.Visible = true;
            Tran.Commit();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void txtcustordersearch_TextChanged(object sender, EventArgs e)
    {
        if (txtcustordersearch.Text != "")
        {
            string str = @"Select CompanyId, CustomerId, OrderId 
                    From OrderMaster Where CompanyID = " + DDCompanyName.SelectedValue + @" And (CustomerOrderNo = '" + txtcustordersearch.Text + "' OR LocalOrder='" + txtcustordersearch.Text + "')";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDCustomerCode.Items.FindByValue(ds.Tables[0].Rows[0]["customerid"].ToString()) != null)
                {
                    DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
                    DDCustomerCode_SelectedIndexChanged(sender, new EventArgs());
                }
                if (DDCustOrderNo.Items.FindByValue(ds.Tables[0].Rows[0]["orderid"].ToString()) != null)
                {
                    DDCustOrderNo.SelectedValue = ds.Tables[0].Rows[0]["orderid"].ToString();
                    DDCustOrderNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altor", "alert('Invalid customer order No.!!!')", true);
            }
        }
    }
    protected void PerformainvoiceJRExportForCOIN()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");
            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 12.18;
            sht.Columns("B").Width = 37.18;
            sht.Columns("C").Width = 15.36;
            sht.Columns("D").Width = 8.73;
            sht.Columns("E").Width = 9.55;
            sht.Columns("F").Width = 7.18;
            sht.Columns("G").Width = 8.18;
            sht.Columns("H").Width = 8.73;
            sht.Columns("I").Width = 13.82;

            //sht.Row(1).Height = 21;
            //**************

            sht.Range("A1:I1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A1:I1").Style.Font.FontName = "Arial";
            sht.Range("A1:I1").Style.Font.FontSize = 20;
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Font.SetFontColor(XLColor.Red);
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A2:I2").Merge();
            sht.Range("A2").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A2:I2").Style.Font.FontName = "Arial";
            sht.Range("A2:I2").Style.Font.FontSize = 20;
            sht.Range("A2:I2").Style.Font.Bold = true;
            sht.Range("A2:I2").Style.Font.SetFontColor(XLColor.Red);
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A3:I3").Merge();
            sht.Range("A3").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"] + " FAX : " + ds.Tables[0].Rows[0]["Compfax"] + " " + "E-MAIL : " + ds.Tables[0].Rows[0]["email"];
            sht.Range("A3:I3").Style.Font.FontName = "Arial";
            sht.Range("A3:I3").Style.Font.FontSize = 16;
            sht.Range("A3:I3").Style.Font.Bold = true;
            sht.Range("A3:I3").Style.Font.SetFontColor(XLColor.Red);
            sht.Range("A3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A4:I4").Merge();
            sht.Range("A4").Value = "PROFORMA INVOICES / SALES CONFIRMATION";
            sht.Range("A4:I4").Style.Font.FontName = "Arial";
            sht.Range("A4:I4").Style.Font.FontSize = 16;
            sht.Range("A4:I4").Style.Font.Bold = true;
            sht.Range("A4:I4").Style.Font.SetUnderline();
            //sht.Range("A4:I4").Style.Font.SetFontColor(XLColor.Red);
            sht.Range("A4:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A5:A8").Merge();
            sht.Range("A5").Value = "Messrs:";
            sht.Range("A5:A8").Style.Font.FontName = "Arial";
            sht.Range("A5:A8").Style.Font.FontSize = 10;
            sht.Range("A5:A8").Style.Font.Bold = true;
            sht.Range("A5:A8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("B5:B8").Merge();
            sht.Range("B5").Value = ds.Tables[0].Rows[0]["CustomerAddress"];
            sht.Range("B5:B8").Style.Font.FontName = "Arial";
            sht.Range("B5:B8").Style.Font.FontSize = 9;
            sht.Range("B5:B8").Style.Font.Bold = true;
            sht.Range("B5:B8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //sht.Range("B5:B8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B5:B8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B5").Style.Alignment.WrapText = true;


            sht.Range("H6").Value = "DATE :";
            sht.Range("H6").Style.Font.FontName = "Arial";
            sht.Range("H6").Style.Font.FontSize = 10;
            sht.Range("H6").Style.Font.Bold = true;
            sht.Range("H6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("I6").Value = ds.Tables[0].Rows[0]["OrderDate"];
            sht.Range("I6").Style.Font.FontName = "Arial";
            sht.Range("I6").Style.Font.FontSize = 10;
            sht.Range("I6").Style.Font.Bold = true;
            sht.Range("I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("H7").Value = "PI NO :";
            sht.Range("H7").Style.Font.FontName = "Arial";
            sht.Range("H7").Style.Font.FontSize = 10;
            sht.Range("H7").Style.Font.Bold = true;
            sht.Range("H7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("I7").Value = ds.Tables[0].Rows[0]["InvoiceNo"];
            sht.Range("I7").Style.Font.FontName = "Arial";
            sht.Range("I7").Style.Font.FontSize = 10;
            sht.Range("I7").Style.Font.Bold = true;
            sht.Range("I7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //'******Detail Headers
            sht.Range("A9:I9").Style.Font.FontSize = 10;
            sht.Range("A9:I9").Style.Font.FontName = "Arial";
            sht.Range("A9:I9").Style.Font.Bold = true;
            sht.Range("A9:I9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A9").Style.Font.FontSize = 10;
            sht.Range("A9").Value = "STYLE NO.";
            sht.Row(9).Height = 50;

            sht.Range("B9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B9").Style.Font.FontSize = 10;
            sht.Range("B9").Value = "DESCRIPTION";

            sht.Range("C9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C9").Style.Font.FontSize = 10;
            sht.Range("C9").Value = "COMPOSITION";

            sht.Range("D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D9").Style.Font.FontSize = 10;
            sht.Range("D9").Value = "DELIVERY";

            sht.Range("E9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E9").Style.Font.FontSize = 10;
            sht.Range("E9").Value = "Net Wt. in grms.";
            sht.Range("E9").Style.Alignment.WrapText = true;

            sht.Range("F9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("F9").Style.Font.FontSize = 10;
            sht.Range("F9").Value = "QTY";

            sht.Range("G9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G9").Style.Font.FontSize = 10;
            sht.Range("G9").Value = "UNIT PRICE (" + ds.Tables[0].Rows[0]["Currencyname"] + ")";
            sht.Range("G9").Style.Alignment.WrapText = true;

            sht.Range("H9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("H9").Style.Font.FontSize = 10;
            sht.Range("H9").Value = "AMOUNT (" + ds.Tables[0].Rows[0]["Currencyname"] + ")";
            sht.Range("H9").Style.Alignment.WrapText = true;

            sht.Range("I9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("I9").Style.Font.FontSize = 10;
            sht.Range("I9").Value = "SHIPMENT TERM   (" + ds.Tables[0].Rows[0]["modeofshipment"] + ")";
            sht.Range("I9").Style.Alignment.WrapText = true;

            //*****************         
            int i;
            i = 10;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":I" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":I" + i).Style.Font.FontName = "Arial";
                sht.Range("A" + i + ":I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":I" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":I" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["Item_Name"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["Designname"] + " " + ds.Tables[0].Rows[j]["Size"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["Qualityname"]);
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Dispatchdate"]);
                sht.Range("E" + i).SetValue("");
                sht.Range("F" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                sht.Range("H" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);
                sht.Range("I" + i).SetValue(ds.Tables[0].Rows[j]["modeofshipment"]);

                using (var a = sht.Range("A" + i + ":I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                i = i + 1;
            }

            //******

            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":I" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":I" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":I" + i).Style.Font.FontName = "Arial";

            sht.Range("E" + i + ":E" + i).Merge();
            sht.Range("E" + i).SetValue("Total :");

            sht.Range("F" + i + ":F" + i).Merge();
            sht.Range("F" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));

            sht.Range("H" + i + ":H" + i).Merge();
            sht.Range("H" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            i = i + 1;
            sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;


            sht.Range("E" + i + ":G" + i).Merge();
            sht.Range("E" + i).SetValue("Trade discount @" + ds.Tables[0].Rows[0]["LessDiscount"] + " %");
            sht.Range("E" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
            sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Arial";

            sht.Range("H" + i + ":H" + i).Merge();
            sht.Range("H" + i).SetValue((Convert.ToDouble(ds.Tables[0].Compute("sum(amount)", "")) * Convert.ToDouble(ds.Tables[0].Rows[0]["LessDiscount"])) / 100);
            sht.Range("H" + i + ":H" + i).Style.Font.Bold = true;
            sht.Range("H" + i + ":H" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("E" + i + ":G" + i).Merge();
            sht.Range("E" + i).SetValue("Grand Total:");
            sht.Range("E" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
            sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Arial";

            sht.Range("H" + i + ":H" + i).Merge();
            sht.Range("H" + i).FormulaA1 = "=($H$" + (i - 2) + '+' + "$H" + (i - 1) + ")";
            sht.Range("H" + i + ":H" + i).Style.Font.Bold = true;

            i = i + 1;
            sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;


            sht.Range("B" + i).SetValue("SHIPPING MARK:" + ds.Tables[0].Rows[0]["modeofshipment"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("Coin srl con socio unico");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 9;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("Coin srl con socio unico  ORDER NUMBER");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("Coin srl con socio unico  STYLE NUMBER");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("TOTAL PIECES PER MASTER");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("GROSS WEIGHT");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("CARTON WEIGHT ON TOTAL CARTON");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("TERMS AND CONDITION:");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 9;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i).SetValue("1. Loading Port and Destination:" + ds.Tables[0].Rows[0]["modeofshipment"] + "," + ds.Tables[0].Rows[0]["destinationplace"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i).SetValue("2. Terms of Payment:" + ds.Tables[0].Rows[0]["modofpayment"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i).SetValue("3. Country of Origin:" + ds.Tables[0].Rows[0]["countryname"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i).SetValue("4. BANK DETAILS  :" + ds.Tables[0].Rows[0]["BankName"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":I" + i).Merge();
            sht.Range("B" + i).SetValue("ADDRESS :" + ds.Tables[0].Rows[0]["street"] + " " + ds.Tables[0].Rows[0]["city"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("Branch Code - :" + ds.Tables[0].Rows[0]["Branchcode"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("Ph.:" + ds.Tables[0].Rows[0]["BankPhoneNo"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("A/C No." + ds.Tables[0].Rows[0]["AcNo"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("SWIFT CODE OF OUR BRANCH :" + ds.Tables[0].Rows[0]["SwiftCode"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("IFSC CODE  :" + ds.Tables[0].Rows[0]["ifsccode"]);
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i).SetValue("6. Trade Discount ");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;


            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("F" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;


            sht.Range("F" + i + ":I" + i).Merge();
            sht.Range("F" + i).SetValue("For:" + ds.Tables[0].Rows[0]["CompanyName"]);
            sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("F" + i).Style.Font.Bold = true;
            sht.Range("F" + i).Style.Font.FontSize = 10;
            sht.Range("F" + i).Style.Font.FontName = "Arial";


            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            sht.Range("F" + i + ":I" + i).Merge();
            sht.Range("F" + i).SetValue("Authorised Signatory");
            sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("F" + i).Style.Font.Bold = true;
            sht.Range("F" + i).Style.Font.FontSize = 10;
            sht.Range("F" + i).Style.Font.FontName = "Arial";

            i = i + 1;
            //sht.Range("A" + i + ":I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i).SetValue("CONFIRMED BUYER / (COMPANY CHOP)");
            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //sht.Range("B" + i).Style.Font.Bold = true;
            sht.Range("B" + i).Style.Font.FontSize = 11;
            sht.Range("B" + i).Style.Font.FontName = "Arial";

            sht.Range("F" + i + ":I" + i).Merge();
            sht.Range("F" + i).SetValue("SELLER / (COMPANY CHOP)");
            sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //sht.Range("F" + i).Style.Font.Bold = true;
            sht.Range("F" + i).Style.Font.FontSize = 11;
            sht.Range("F" + i).Style.Font.FontName = "Arial";


            //***********Borders
            sht.Range("A1:I1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A1:I1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:I2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A3:I3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A4:I4").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            sht.Range("A8:I8").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            sht.Range("A1:A9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            sht.Range("A1:A9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I1:I9").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B1:B9").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("H1:H9").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A9:I9").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A9:I9").Style.Border.BottomBorder = XLBorderStyleValues.Thin;


            //***********
            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoiceCoin-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();


        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void PerformainvoiceSamaraType2()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");



            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 5.56;
            sht.Columns("B").Width = 11.89;
            sht.Columns("C").Width = 11.67;
            sht.Columns("D").Width = 22.44;
            sht.Columns("E").Width = 9.89;
            sht.Columns("F").Width = 8.44;

            sht.Columns("G").Width = 6.67;
            sht.Columns("H").Width = 10.56;
            sht.Columns("I").Width = 7.33;

            sht.Columns("J").Width = 10.22;
            sht.Columns("K").Width = 11.00;
            sht.Columns("L").Width = 9.00;

            sht.Columns("M").Width = 8.22;
            sht.Columns("N").Width = 7.78;
            sht.Columns("O").Width = 14.11;

            sht.Columns("P").Width = 9.00;
            sht.Columns("Q").Width = 8.89;

            //sht.Row(1).Height = 21;
            //**************
            sht.Range("A1:Q1").Merge();
            sht.Range("A1").Value = "P E R F O R M A   I N V O I C E";
            sht.Range("A1:Q1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:Q1").Style.Font.FontSize = 16;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************
            sht.Range("A2:C2").Merge();
            sht.Range("A2:C2").Style.Font.FontSize = 12;
            sht.Range("A2:C2").Style.Font.Bold = true;
            sht.Range("A2").Value = "Exporter";

            //sht.Range("A3:C3").Style.Font.Bold = true;
            sht.Range("A3:C3").Style.Font.FontSize = 11;
            sht.Range("A3:C3").Merge();
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A4:C4").Merge();
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A5:C5").Merge();
            sht.Range("A5").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"];
            sht.Range("A6:C6").Merge();
            sht.Range("A6").Value = "FAX : " + ds.Tables[0].Rows[0]["Compfax"];
            sht.Range("A7:C7").Merge();
            sht.Range("A7").Value = "E-MAIL : " + ds.Tables[0].Rows[0]["email"];

            ////Consignee            
            sht.Range("D2").Style.Font.FontSize = 12;
            sht.Range("D2").Style.Font.Bold = true;
            sht.Range("D2").Value = "Consignee";

            ////Values           
            sht.Range("D3:D6").Style.Font.FontSize = 11;
            //sht.Range("D3:D6").Style.Font.Bold = true;
            sht.Range("D3").Value = ds.Tables[0].Rows[0]["customercompany"];

            ////Delivery Address 
            sht.Range("E2:G2").Merge();
            sht.Range("E2:G2").Style.Font.FontSize = 12;
            sht.Range("E2:G2").Style.Font.Bold = true;
            sht.Range("E2:G2").Value = "Delivery Address";

            ////Values   
            sht.Range("E3:G4").Merge();
            sht.Range("E3:G6").Style.Font.FontSize = 11;
            //sht.Range("E3:G6").Style.Font.Bold = true;
            sht.Range("E3").Value = ds.Tables[0].Rows[0]["customeraddress"];
            sht.Range("E3:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("E3:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("E3").Style.Alignment.WrapText = true;

            sht.Range("E5:G5").Merge();
            sht.Range("E5").Value = "TEL :" + ds.Tables[0].Rows[0]["customerphoneno"];
            sht.Range("E6:G6").Merge();
            sht.Range("E6").Value = "FAX : " + ds.Tables[0].Rows[0]["customerfax"];
            sht.Range("E7:G7").Merge();
            sht.Range("E7").Value = "E-MAIL :" + ds.Tables[0].Rows[0]["customermail"];

            ////Notify Party 
            sht.Range("H2:I2").Merge();
            sht.Range("H2:I2").Style.Font.FontSize = 12;
            sht.Range("H2:I2").Style.Font.Bold = true;
            sht.Range("H2").Value = "Notify Party";

            ////Values   
            sht.Range("H3:I4").Merge();
            sht.Range("H3:I6").Style.Font.FontSize = 11;
            //sht.Range("H3:I6").Style.Font.Bold = true;
            sht.Range("H3").Value = ds.Tables[0].Rows[0]["NotifyParty"];
            sht.Range("H3:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("H3:I4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("H3").Style.Alignment.WrapText = true;

            ////Pre CarriageBy 
            sht.Range("J2:K2").Merge();
            sht.Range("J2:K2").Style.Font.FontSize = 12;
            sht.Range("J2:K2").Style.Font.Bold = true;
            sht.Range("J2").Value = "Pre-Carriage By";

            ////Values   
            sht.Range("J3:K3").Merge();
            sht.Range("J3:K3").Style.Font.FontSize = 11;
            //sht.Range("J3:K3").Style.Font.Bold = true;
            sht.Range("J3").Value = ds.Tables[0].Rows[0]["Precarriageby"];
            sht.Range("J3").Style.Alignment.WrapText = true;

            ////Port Of Discharge 
            sht.Range("J4:K4").Merge();
            sht.Range("J4:K4").Style.Font.FontSize = 12;
            sht.Range("J4:K4").Style.Font.Bold = true;
            sht.Range("J4").Value = "Port Of Discharge";

            ////Values   
            sht.Range("J5:K5").Merge();
            sht.Range("J5:K5").Style.Font.FontSize = 11;
            //sht.Range("J5:K5").Style.Font.Bold = true;
            sht.Range("J5").Value = "";
            sht.Range("J5").Style.Alignment.WrapText = true;

            ////Port Of Loading 
            sht.Range("J6:K6").Merge();
            sht.Range("J6:K6").Style.Font.FontSize = 12;
            sht.Range("J6:K6").Style.Font.Bold = true;
            sht.Range("J6").Value = "Port Of Loading";

            ////Values   
            sht.Range("J7:K7").Merge();
            sht.Range("J7:K7").Style.Font.FontSize = 11;
            //sht.Range("J7:K7").Style.Font.Bold = true;
            sht.Range("J7").Value = ds.Tables[0].Rows[0]["portofloading"];
            sht.Range("J7").Style.Alignment.WrapText = true;


            ////Country Of Origin
            sht.Range("L2:N2").Merge();
            sht.Range("L2:N2").Style.Font.FontSize = 12;
            sht.Range("L2:N2").Style.Font.Bold = true;
            sht.Range("L2").Value = "Country Of Origin";

            ////Values   
            sht.Range("L3:N3").Merge();
            sht.Range("L3:N3").Style.Font.FontSize = 11;
            // sht.Range("L3:N3").Style.Font.Bold = true;
            sht.Range("L3").Value = "INDIA";
            sht.Range("L3").Style.Alignment.WrapText = true;

            ////Final Destination 
            sht.Range("L4:N4").Merge();
            sht.Range("L4:N4").Style.Font.FontSize = 12;
            sht.Range("L4:N4").Style.Font.Bold = true;
            sht.Range("L4").Value = "Final Destination";

            ////Values   
            sht.Range("L5:N5").Merge();
            //sht.Range("L5:N5").Style.Font.FontSize = 11;
            sht.Range("L5:N5").Style.Font.Bold = true;
            sht.Range("L5").Value = ds.Tables[0].Rows[0]["destinationplace"];
            sht.Range("L5").Style.Alignment.WrapText = true;

            ////Final Destination Country 
            sht.Range("L6:N6").Merge();
            sht.Range("L6:N6").Style.Font.FontSize = 12;
            sht.Range("L6:N6").Style.Font.Bold = true;
            sht.Range("L6").Value = "Final Destination Country";

            ////Values   
            sht.Range("L7:N7").Merge();
            sht.Range("L7:N7").Style.Font.FontSize = 11;
            //sht.Range("L7:N7").Style.Font.Bold = true;
            sht.Range("L7").Value = ds.Tables[0].Rows[0]["countryname"];
            sht.Range("L7").Style.Alignment.WrapText = true;
            sht.Range("L7:N7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ////Invoice No           
            sht.Range("O2").Style.Font.FontSize = 12;
            sht.Range("O2").Style.Font.Bold = true;
            sht.Range("O2").Value = "Invoice No";

            ////Values   
            sht.Range("P2:Q2").Merge();
            sht.Range("P2:Q2").Style.Font.FontSize = 11;
            //sht.Range("P2:Q2").Style.Font.Bold = true;
            sht.Range("P2").Value = ds.Tables[0].Rows[0]["invoiceno"];
            sht.Range("P2").Style.Alignment.WrapText = true;
            sht.Range("P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            ////Order Date           
            sht.Range("O3").Style.Font.FontSize = 12;
            sht.Range("O3").Style.Font.Bold = true;
            sht.Range("O3").Value = "Order Date";

            ////Values   
            sht.Range("P3:Q3").Merge();
            sht.Range("P3:Q3").Style.Font.FontSize = 11;
            //sht.Range("P3:Q3").Style.Font.Bold = true;
            sht.Range("P3").Value = ds.Tables[0].Rows[0]["orderdate"];
            sht.Range("P3").Style.Alignment.WrapText = true;
            sht.Range("P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            ////PO #           
            sht.Range("O4").Style.Font.FontSize = 12;
            sht.Range("O4").Style.Font.Bold = true;
            sht.Range("O4").Value = "PO #";

            ////Values   
            sht.Range("P4:Q4").Merge();
            sht.Range("P4:Q4").Style.Font.FontSize = 11;
            //sht.Range("P4:Q4").Style.Font.Bold = true;
            sht.Range("P4").Value = ds.Tables[0].Rows[0]["customerorderNo"];
            sht.Range("P4").Style.Alignment.WrapText = true;
            sht.Range("P4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            ////Delivery Date          
            sht.Range("O5").Style.Font.FontSize = 12;
            sht.Range("O5").Style.Font.Bold = true;
            sht.Range("O5").Value = "Delivery Date";

            ////Values   
            sht.Range("P5:Q5").Merge();
            sht.Range("P5:Q5").Style.Font.FontSize = 11;
            //sht.Range("P5:Q5").Style.Font.Bold = true;
            sht.Range("P5").Value = ds.Tables[0].Rows[0]["dispatchdate"];
            sht.Range("P5").Style.Alignment.WrapText = true;
            sht.Range("P5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            using (var a = sht.Range("A2:Q7"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            //'******Detail Headers
            sht.Range("A8:Q8").Style.Font.FontSize = 12;
            sht.Range("A8:Q8").Style.Font.Bold = true;
            sht.Range("A8:Q8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A8:A9").Merge();
            sht.Range("A8:A9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A8:A9").Style.Font.FontSize = 12;
            sht.Range("A8").Value = "S.No";
            sht.Range("A8").Style.Alignment.WrapText = true;

            sht.Range("B8:B9").Merge();
            sht.Range("B8:B9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B8:B9").Style.Font.FontSize = 12;
            sht.Range("B8").Value = "PO #";
            sht.Range("B8").Style.Alignment.WrapText = true;

            sht.Range("C8:C9").Merge();
            sht.Range("C8:C9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C8:C9").Style.Font.FontSize = 12;
            sht.Range("C8").Value = "Buyer ItemCode";
            sht.Range("C8").Style.Alignment.WrapText = true;

            sht.Range("D8:D9").Merge();
            sht.Range("D8:D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D8:D9").Style.Font.FontSize = 12;
            sht.Range("D8").Value = "Description Of Goods";
            sht.Range("D8").Style.Alignment.WrapText = true;

            sht.Range("E8:G9").Merge();
            sht.Range("E8:G9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E8:G9").Style.Font.FontSize = 12;
            sht.Range("E8").Value = "Item Picture";
            sht.Range("E8").Style.Alignment.WrapText = true;

            sht.Range("H8:I9").Merge();
            sht.Range("H8:I9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("H8:I9").Style.Font.FontSize = 12;
            sht.Range("H8").Value = "Composition";
            sht.Range("H8").Style.Alignment.WrapText = true;

            //sht.Range("J8:J9").Merge();
            sht.Range("J8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J8").Style.Font.FontSize = 12;
            sht.Range("J8").Value = "Length";
            //sht.Range("J8").Style.Alignment.WrapText = true;

            //sht.Range("K8:K9").Merge();
            sht.Range("K8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("K8").Style.Font.FontSize = 12;
            sht.Range("K8").Value = "Width";
            //sht.Range("K8").Style.Alignment.WrapText = true;

            string unitname;
            if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "0")
            {
                switch (ds.Tables[0].Rows[0]["unitid"].ToString())
                {
                    case "1":
                        unitname = "SQ. Mtr";
                        break;
                    case "2":
                        unitname = "SQ. Ft";
                        break;
                    default:
                        unitname = ds.Tables[0].Rows[0]["unitname"].ToString();
                        break;
                }
            }
            else
            {
                unitname = "PC";
            }

            sht.Range("J9:K9").Merge();
            sht.Range("J9:K9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("J9:K9").Style.Font.FontSize = 10;
            sht.Range("J9:K9").Style.Font.Bold = true;
            sht.Range("J9").Value = "Rug Size " + unitname;
            sht.Range("J9").Style.Alignment.WrapText = true;

            sht.Range("L8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L8").Style.Font.FontSize = 12;
            sht.Range("L8").Value = "Area";
            //sht.Range("L8").Style.Alignment.WrapText = true;

            sht.Range("M8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("M8").Style.Font.FontSize = 12;
            sht.Range("M8").Value = "Net Wt";
            //sht.Range("L8").Style.Alignment.WrapText = true;

            sht.Range("N8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N8").Style.Font.FontSize = 12;
            sht.Range("N8").Value = "Rate";
            //sht.Range("L8").Style.Alignment.WrapText = true;

            sht.Range("L9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L9").Style.Font.FontSize = 10;
            sht.Range("L9").Value = unitname;
            sht.Range("L9").Style.Font.Bold = true;

            sht.Range("M9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("M9").Style.Font.FontSize = 10;
            sht.Range("M9").Value = "LBS";
            sht.Range("M9").Style.Font.Bold = true;

            sht.Range("N9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N9").Style.Font.FontSize = 10;
            sht.Range("N9").Value = ds.Tables[0].Rows[0]["currencyname"];
            sht.Range("N9").Style.Font.Bold = true;

            sht.Range("O8:O9").Merge();
            sht.Range("O8:O9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("O8:O9").Style.Font.FontSize = 12;
            sht.Range("O8").Value = "Order Qty";
            sht.Range("O8").Style.Alignment.WrapText = true;

            sht.Range("P8:P9").Merge();
            sht.Range("P8:P9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P8:P9").Style.Font.FontSize = 12;
            sht.Range("P8").Value = "Total Area " + unitname;
            sht.Range("P8").Style.Alignment.WrapText = true;

            sht.Range("Q8:Q9").Merge();
            sht.Range("Q8:Q9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q8:Q9").Style.Font.FontSize = 12;
            sht.Range("Q8").Value = "Total Values";
            sht.Range("Q8").Style.Alignment.WrapText = true;

            //*****************

            int i;
            i = 10;
            int LastRowId = 0;
            int SrNo = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":Q" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":Q" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":Q" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                SrNo = SrNo + 1;
                sht.Range("A" + i).SetValue(SrNo);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["customerorderno"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["ItemCode"]);
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["QualityName"]);

                var img = "";
                if (ds.Tables[0].Rows[j]["ImagePath"].ToString() != "")
                {
                    img = Server.MapPath(ds.Tables[0].Rows[j]["ImagePath"].ToString());
                }


                sht.Range("E" + i + ":G" + i).Merge();
                //sht.Range("E" + i).SetValue("Item Picture");                
                sht.Range("E" + i).SetValue(img);

                sht.Range("H" + i + ":I" + i).Merge();
                sht.Range("H" + i).SetValue("");
                string s = ds.Tables[0].Rows[j]["size"].ToString();
                string[] parts = s.Split('x');
                //int i1 = int.Parse(parts[0]);
                //int i2 = int.Parse(parts[1]);
                sht.Range("J" + i).SetValue(parts[0]);
                sht.Range("K" + i).SetValue(parts[1]);
                sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["totalarea"]);
                sht.Range("M" + i).SetValue("");
                sht.Range("N" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                sht.Range("O" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                Decimal TotalArea = 0;
                TotalArea = Convert.ToDecimal(ds.Tables[0].Rows[j]["totalarea"]) * Convert.ToDecimal(ds.Tables[0].Rows[j]["qtyrequired"]);
                sht.Range("P" + i).SetValue(TotalArea);
                sht.Range("Q" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                using (var a = sht.Range("A" + i + ":Q" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                i = i + 1;
                LastRowId = i;
            }
            //i = i + 1;
            sht.Range("A" + i + ":Q" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":Q" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":Q" + i).Style.Font.FontSize = 11;
            sht.Range("L" + i + ":N" + i).Merge();
            sht.Range("L" + i).SetValue("Total");

            sht.Range("O" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));
            sht.Range("P" + i).FormulaA1 = "=sum(P" + 10 + ':' + "$P$" + (LastRowId - 1) + ")";
            sht.Range("Q" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            using (var a = sht.Range("A" + i + ":Q" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("A8:Q9"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //***********

            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoiceSamaraType2-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

            //Session["rptFileName"] = "~\\Reports\\Rptorderperformainvoice.rpt";                   
            //Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\Rptorderperformainvoice.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void ddordertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "44")
        {
            TDSampleCode.Visible = true;
        }
        else
        {
            TDSampleCode.Visible = false;
            TDRugIdNo.Visible = false;

        }
        TDRugIdNo.Visible = false;

        if (Session["VarCompanyNo"].ToString() == "4")
        {
            if (ddordertype.SelectedValue == "1" || ddordertype.SelectedValue == "3")
            {
                TDRugIdNo.Visible = true;
                TDSampleCode.Visible = false;
            }
            else if (ddordertype.SelectedValue == "2")
            {
                TDSampleCode.Visible = true;
                TDRugIdNo.Visible = false;
            }
        }
        else
        {
            if (Session["VarCompanyNo"].ToString() == "44")
            {
                TDSampleCode.Visible = true;
            }
            if (ddordertype.SelectedValue == "2")
            {
                TDSampleCode.Visible = true;

                if (Session["varcompanyId"].ToString() == "16" || (Session["varcompanyId"].ToString() == "28" && (Session["varSubCompanyId"].ToString() == "282" || Session["varSubCompanyId"].ToString() == "285")))
                {
                    string Str = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'S ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'S %'").ToString();
                    TxtCustOrderNo.Text = "S " + Str;
                    TxtLocalOrderNo.Text = "S " + Str;
                }
            }
            else
            {
                TxtCustOrderNo.Text = "";
                TxtLocalOrderNo.Text = "";
            }
        }
    }
    protected void TxtSampleCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        if (TxtSampleCode.Text != "")
        {
            Str = @"Select a.Categoryid, a.ItemID, a.QualityID, IsNull((Select Min(D.DesignID) From Design D(Nolock) Where D.DesignName = a.DesignName), 0) DesignID, 
                    IsNull((Select Min(C.ColorID) From Color C(Nolock) Where C.ColorName = a.ColorName), 0) ColorID, a.ShapeID, a.SizeID 
                    FROM SampleDevelopmentMaster a(Nolock) 
                    WHERE a.SAMPLECODE = '" + TxtSampleCode.Text + "' And a.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();
                ddlcategorycange();
                fillCombo();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemID"].ToString();
                ItemSelectedChange();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
                QualitySelectedChange();
                FillDesign();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                fillColor();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLORID"].ToString();
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPEID"].ToString();
                DDShape_SelectedIndexChanged(sender, new EventArgs());
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZEID"].ToString();
                DDSize_SelectedIndexChanged(sender, e);
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDItemCategory.SelectedIndex = 0;
                ddlcategorycange();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDItemCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    protected void txtRugIdNo_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        if (txtRugIdNo.Text != "")
        {

            Str = @"Select distinct IPM.ITEM_FINISHED_ID,IM.CATEGORY_ID as CategoryId,IPM.ITEM_ID as ItemId,CQ.SrNo as QualityId,CD.SrNo as DesignId,CC.SrNo as ColorId,
                    IPM.SHADECOLOR_ID as ShadeColorId,IPM.SHAPE_ID as ShapeId,IPM.SIZE_ID as SizeId,IPM.DESCRIPTION,IPM.ProductCode,IPM.OurCode,IPM.Status,IPM.MasterCompanyId
                    FROM ITEM_PARAMETER_MASTER IPM(NoLock) 
					JOIN CustomerQuality CQ ON CQ.CustomerID = " + DDCustomerCode.SelectedValue + @" And CQ.QualityId = IPM.QUALITY_ID
					JOIN CustomerDesign CD ON CD.CustomerId=" + DDCustomerCode.SelectedValue + @" and CD.DesignId=IPM.DESIGN_ID and CD.CQSRNO=CQ.SrNo
					JOIN CustomerColor CC ON CC.CustomerId=" + DDCustomerCode.SelectedValue + @" and CC.ColorId=IPM.COLOR_ID --and CC.CDSRNO=CD.SrNo
					JOIN CustomerSize CCS ON CCS.CustomerId=" + DDCustomerCode.SelectedValue + @" and CCS.Sizeid=IPM.SIZE_ID
                    JOIN ITEM_MASTER IM(NoLock) ON IPM.ITEM_ID=IM.ITEM_ID
                    JOIN CategorySeparate CS(NoLock) ON IM.Category_Id=CS.CategoryId and Id=0
                    WHERE IPM.ProductCode = '" + txtRugIdNo.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDItemCategory.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();
                ddlcategorycange();
                fillCombo();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemID"].ToString();
                ItemSelectedChange();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
                QualitySelectedChange();
                FillDesign();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                fillColor();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLORID"].ToString();
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPEID"].ToString();
                DDShape_SelectedIndexChanged(sender, new EventArgs());
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZEID"].ToString();
                DDSize_SelectedIndexChanged(sender, e);
            }
            else
            {
                LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDItemCategory.SelectedIndex = 0;
                ddlcategorycange();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDItemCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    protected void TxtDeliveryDate_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "20")
        {
            DateTime dt = Convert.ToDateTime(TxtDeliveryDate.Text);
            DateTime dt2 = dt.AddDays(-14);
            TxtDispatchDate.Text = dt2.ToString("dd-MMM-yyyy");
            validate_DispatchDate();
        }
    }
    protected void GetRate()
    {
        int Item_Finished_Id = 0;
        int OrderCalType;
        //************Buyercode
        if (variable.Withbuyercode == "1")
        {
            if (ddordertype.SelectedValue == "2")
            {
                if (variable.VarDEMORDERWITHLOCALQDCS == "1")
                {
                    Item_Finished_Id = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
                }
                else
                {
                    Item_Finished_Id = UtilityModule.getItemFinishedIdWithBuyercode(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
                }
            }
            else
            {
                Item_Finished_Id = UtilityModule.getItemFinishedIdWithBuyercode(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
            }
        }
        else
        {
            Item_Finished_Id = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, ddshadecolor, 0, TXTOURCODE.Text, Convert.ToInt32(Session["varCompanyId"]));
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);
            param[1] = new SqlParameter("@OrderUnitId", SqlDbType.Int);
            param[2] = new SqlParameter("@OrderCalType", SqlDbType.Int);
            param[3] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            param[4] = new SqlParameter("@Rate", SqlDbType.Float);

            if (rdoUnitWise.Checked == true)
            {
                OrderCalType = 0;
            }
            else
            {
                OrderCalType = 1;
            }

            param[0].Value = Item_Finished_Id;
            param[1].Value = DDOrderUnit.SelectedValue;
            param[2].Value = OrderCalType;
            param[3].Value = Session["VarCompanyId"];
            param[4].Direction = ParameterDirection.Output;

            //**********
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetFinishedIdPrice", param);
            int rowscount;
            rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetFinishedIdPrice", param);
            Tran.Commit();

            if (param[4].Value.ToString() != "0")
            {
                TxtPrice.Text = param[4].Value.ToString();
            }
            else
            {
                TxtPrice.Text = "0";
            }
            ////Lblmessage.Text = param[4].Value.ToString();
            ////Lblmessage.Visible = true;
            ////ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + Lblmessage.Text + "');", true);


        }
        catch (Exception ex)
        {
            Lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    private void Check_Length_Width_Format(string Length, string Width)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        string Str = "";


        if (Length != "")
        {
            if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(Length));
                Length = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                InchLength = (FootLength * 12) + FootLengthInch;
                if (FootLengthInch > 11)
                {
                    Lblmessage.Text = "Inch value must be less than 12";
                    Length = "";
                    //txtlength.Focus();
                }
            }
        }
        if (Width != "")
        {
            if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(Width));
                Width = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                InchWidth = (FootWidth * 12) + FootWidthInch;
                if (FootWidthInch > 11)
                {
                    Lblmessage.Text = "Inch value must be less than 12";
                    Width = "";
                    //txtwidth.Focus();
                }
            }
        }
        if (Length != "" && Width != "")
        {
            int Shape = Convert.ToInt16(DDShape.SelectedValue);

            if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 1)
            {
                // TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length), Convert.ToDouble(Width), 0, Shape));

                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    TxtArea.Text = Convert.ToString(Math.Round((Convert.ToDouble(Length) * Convert.ToDouble(Width)) / 10000, 2));
                }
                else
                {
                    TxtArea.Text = Convert.ToString(Math.Round((Convert.ToDouble(Length) * Convert.ToDouble(Width)) / 10000, 4));
                }


            }
            if (Convert.ToInt32(DDOrderUnit.SelectedValue) == 2 || Convert.ToInt16(DDOrderUnit.SelectedValue) == 6)
            {
                int VarFactor = 1;
                //if (DDShape.SelectedValue == "2")
                //{
                //    string str = "Select VarCompanyNo,RoundFtFlag,ProductionArea,vargirh,Varfactor From MasterSetting";
                //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        VarFactor = Convert.ToInt32(ds.Tables[0].Rows[0]["Varfactor"].ToString());
                //    }
                //    TxtArea.Text = Convert.ToString(Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4));

                //}
                //else
                //{
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    TxtArea.Text = Convert.ToString(Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 2));
                }
                else
                {
                    TxtArea.Text = Convert.ToString(Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4));
                }


                //}
                ////TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), 0, Shape, UnitId: Convert.ToInt16(DDOrderUnit.SelectedValue)));
            }
        }
    }

    protected void PerformainvoiceChampoType2()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");
            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 16.43;
            sht.Columns("B").Width = 15.29;
            sht.Columns("C").Width = 13.43;
            sht.Columns("D").Width = 7.71;
            sht.Columns("E").Width = 3.14;
            sht.Columns("F").Width = 1.71;

            sht.Columns("G").Width = 4.29;
            sht.Columns("H").Width = 2.29;
            sht.Columns("I").Width = 3.86;

            sht.Columns("J").Width = 4.29;
            sht.Columns("K").Width = 2.29;
            sht.Columns("L").Width = 3.86;

            sht.Columns("M").Width = 4.29;
            sht.Columns("N").Width = 2.29;
            sht.Columns("O").Width = 3.86;

            sht.Columns("P").Width = 6.57;
            sht.Columns("Q").Width = 7.57;

            sht.Columns("R").Width = 10.14;
            sht.Columns("S").Width = 6.71;

            sht.Row(1).Height = 21;
            //**************
            sht.Range("A1:S1").Merge();
            sht.Range("A1").Value = "P R O F O R M A   I N V O I C E";
            sht.Range("A1:S1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:S1").Style.Font.FontSize = 16;
            sht.Range("A1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************
            sht.Range("A2:D2").Merge();
            sht.Range("A2:D2").Style.Font.FontSize = 11;
            sht.Range("A2:D2").Style.Font.Bold = true;


            sht.Range("A2").Value = "Exporter";
            sht.Range("A3:D3").Style.Font.Bold = true;
            sht.Range("A3:D3").Style.Font.FontSize = 13;


            sht.Range("A3:D3").Merge();
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A4:D4").Merge();
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A5:D5").Merge();
            sht.Range("A5").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"] + " FAX : " + ds.Tables[0].Rows[0]["Compfax"];
            sht.Range("A6:D6").Merge();
            sht.Range("A6").Value = "E-MAIL : " + ds.Tables[0].Rows[0]["email"];
            sht.Range("A7:B7").Merge();
            sht.Range("A7").Value = "PAN NO : " + ds.Tables[0].Rows[0]["PANNr"];
            sht.Range("C7:D7").Merge();
            sht.Range("C7").Value = "GSTIN NO : " + ds.Tables[0].Rows[0]["GSTNo"];

            //'Invoice NO Date
            sht.Range("E2:S2").Merge();
            sht.Range("E2:S2").Style.Font.FontSize = 11;
            sht.Range("E2:S2").Style.Font.Bold = true;
            sht.Range("E2").Value = "Invoice No. & Date";
            ////'value
            //sht.Range("I2:K2").Merge();
            //sht.Range("I2").SetValue(ds.Tables[0].Rows[0]["invoiceno"]);

            sht.Range("E3:S3").Merge();
            sht.Range("E3").Value = ds.Tables[0].Rows[0]["invoiceno"].ToString() + " & " + ds.Tables[0].Rows[0]["invoicedate"].ToString();
            sht.Range("E3:S3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //sht.Range("I3:K3").Merge();
            //sht.Range("I3").SetValue(ds.Tables[0].Rows[0]["invoicedate"]);

            sht.Range("E4:S4").Merge();
            sht.Range("E4").Value = "Buyer's Order No. & date";
            sht.Range("E4:S4").Style.Font.FontSize = 11;
            sht.Range("E4:S4").Style.Font.Bold = true;

            sht.Range("E5:S5").Merge();
            sht.Range("E5").Value = "PO# " + ds.Tables[0].Rows[0]["customerorderNo"] + "/" + ds.Tables[0].Rows[0]["orderdate"];

            //'Other ref
            sht.Range("E6:S6").Merge();
            sht.Range("E6").Value = "Other Reference(s) :";
            sht.Range("E6:S6").Style.Font.FontSize = 11;
            sht.Range("E6:S6").Style.Font.Bold = true;
            sht.Range("E7:S7").Merge();
            sht.Range("E7").Value = "";

            //'consignee

            sht.Range("A8:D8").Merge();
            sht.Range("A8:D8").Style.Font.FontSize = 11;
            sht.Range("A8:D8").Style.Font.Bold = true;
            sht.Range("A8").Value = "Consignee";
            //'value
            sht.Range("A9:D9").Merge();
            sht.Range("A9").Value = ds.Tables[0].Rows[0]["customercompany"];
            sht.Range("A10:D11").Merge();
            sht.Range("A10").Value = ds.Tables[0].Rows[0]["customeraddress"];
            sht.Range("A10:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A10:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A10").Style.Alignment.WrapText = true;

            sht.Range("A12:D12").Merge();
            sht.Range("A12").Value = "TEL :" + ds.Tables[0].Rows[0]["customerphoneno"] + " FAX : " + ds.Tables[0].Rows[0]["customerfax"];
            sht.Range("A13:D13").Merge();
            sht.Range("A13").Value = "E-MAIL :" + ds.Tables[0].Rows[0]["customermail"];
            sht.Range("A14:D14").Merge();
            sht.Range("A15:D15").Merge();
            //'*******Buyerotherthanconsignee

            ////sht.Range("E8:S8").Merge();
            ////sht.Range("E8").Value = "Buyer (if other than consignee)";
            ////sht.Range("E8:S8").Style.Font.FontSize = 11;
            ////sht.Range("E8:S8").Style.Font.Bold = true;

            //////'value
            ////sht.Range("E9:S13").Merge();
            ////sht.Range("E9").Value = ds.Tables[0].Rows[0]["BuerOtherThanConsigneeSea"];
            ////sht.Range("E9:S13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            ////sht.Range("E9:S13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ////sht.Range("E9").Style.Alignment.WrapText = true;

            sht.Range("E8:S8").Merge();
            sht.Range("E8").Value = "IEC #" + ds.Tables[0].Rows[0]["IECode"];
            sht.Range("E8:S8").Style.Font.FontSize = 11;
            sht.Range("E8:S8").Style.Font.Bold = true;

            //'value
            sht.Range("E9:S13").Merge();
            sht.Range("E9").Value = "";
            sht.Range("E9:S13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("E9:S13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("E9").Style.Alignment.WrapText = true;

            //'
            //sht.Range("E10:M10").Merge();
            //sht.Range("E10:M10").Style.Font.FontSize = 11;
            //sht.Range("E10:M10").Style.Font.Bold = true;
            //sht.Range("E10").SetValue("SHIP DATE :- " + ds.Tables[0].Rows[0]["Dispatchdate"]);
            //'******Country of origin of goods
            sht.Range("E14:J14").Merge();
            sht.Range("E14:J14").Style.Font.FontSize = 11;
            sht.Range("E14:J14").Style.Font.Bold = true;
            sht.Range("E14").Value = "Country of Origin of goods";
            //'value
            sht.Range("E15:H15").Merge();
            sht.Range("E15:H15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E15").Value = "INDIA";
            //'Country of final destination
            sht.Range("K14:S14").Merge();
            sht.Range("K14:S14").Style.Font.FontSize = 11;
            sht.Range("K14:S14").Style.Font.Bold = true;
            sht.Range("K14").Value = "Country of Final Destination";
            //'value
            sht.Range("K15:S15").Merge();
            sht.Range("K15:S15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("K15").Value = ds.Tables[0].Rows[0]["countryname"];
            //'****Precarrigeby

            sht.Range("A16").Value = "Pre-Carriage by";
            sht.Range("A16").Style.Font.FontSize = 11;
            sht.Range("A16").Style.Font.Bold = true;
            //'
            sht.Range("A17").Value = ds.Tables[0].Rows[0]["Precarriageby"];

            ////'*****Place of Receipt
            //sht.Range("B16:D16").Merge();
            //sht.Range("B16:D16").Style.Font.FontSize = 11;
            //sht.Range("B16:D16").Style.Font.Bold = true;
            //sht.Range("B16").Value = "Place of Receipt";
            ////'
            //sht.Range("B17:D17").Merge();
            //sht.Range("B17").Value = ds.Tables[0].Rows[0]["Placeofreceipt"];

            //'*****Place of Receipt
            sht.Range("B16:D16").Merge();
            sht.Range("B16:D16").Style.Font.FontSize = 11;
            sht.Range("B16:D16").Style.Font.Bold = true;
            sht.Range("B16").Value = "";
            //'
            sht.Range("B17:D17").Merge();
            sht.Range("B17").Value = "";


            //'*****vessel_flight no
            sht.Range("A18").Value = "Vessel Flight No";
            sht.Range("A18").Style.Font.FontSize = 11;
            sht.Range("A18").Style.Font.Bold = true;

            //'
            sht.Range("A19").Value = ds.Tables[0].Rows[0]["modeofshipment"];
            //'**********port of loading
            sht.Range("B18:D18").Merge();
            sht.Range("B18:D18").Style.Font.FontSize = 11;
            sht.Range("B18:D18").Style.Font.Bold = true;
            sht.Range("B18").Value = "Port of Loading";
            //'
            sht.Range("B19:D19").Merge();
            sht.Range("B19").Value = ds.Tables[0].Rows[0]["portofloading"];
            //'********Port of discharge
            sht.Range("A20").Value = "Port of Discharge";
            sht.Range("A20").Style.Font.FontSize = 11;
            sht.Range("A20").Style.Font.Bold = true;
            //'
            sht.Range("A21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //'***********Final destination
            sht.Range("B20:D20").Merge();
            sht.Range("B20:D20").Style.Font.FontSize = 11;
            sht.Range("B20:D20").Style.Font.Bold = true;

            sht.Range("B20").Value = "Final Destination";
            //'
            sht.Range("B21:D21").Merge();
            sht.Range("B21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //////SHIPDATE
            ////sht.Range("E20:S20").Merge();
            ////sht.Range("E20").Value = "SHIP DATE :-" + ds.Tables[0].Rows[0]["Dispatchdate"];
            ////sht.Range("E20:S20").Style.Font.FontSize = 11;
            ////sht.Range("E20:S20").Style.Font.Bold = true;
            ////sht.Range("E20:S20").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //////term of sale
            sht.Range("E16:S16").Merge();
            sht.Range("E16").Value = "Terms of Delivery and Payment";
            sht.Range("E16:S16").Style.Font.FontSize = 11;
            sht.Range("E16:S16").Style.Font.Bold = true;
            sht.Range("E18:S18").Merge();
            sht.Range("E19:S19").Merge();
            sht.Range("E18").Value = ds.Tables[0].Rows[0]["Modofpayment"];
            sht.Range("E19").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
            sht.Range("E18:E19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //'*****Mark & Nos.
            sht.Range("A22:B23").Style.Font.FontSize = 11;
            sht.Range("A22:B23").Style.Font.Bold = true;
            sht.Range("A22:B23").Merge();
            sht.Range("A22").Value = "Marks & Nos. No.& Kind of Packages";
            sht.Range("A22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            //'******Description of goods
            sht.Range("C22:D22").Merge();
            sht.Range("C22").Value = "";
            sht.Range("C22:D22").Style.Font.FontSize = 11;
            sht.Range("C22:D22").Style.Font.Bold = true;
            //'******Detail Headers

            sht.Range("A24:S26").Style.Font.FontSize = 10;
            sht.Range("A24:S26").Style.Font.Bold = true;
            sht.Range("A24:S26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A24:A26").Merge();
            sht.Range("A24:A26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A24:A26").Style.Font.FontSize = 12;
            sht.Range("A24").Value = "ROLL #";

            sht.Range("B24:B26").Merge();
            sht.Range("B24:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B24:B26").Style.Font.FontSize = 12;
            sht.Range("B24").Value = "QUALITY";

            sht.Range("C24:C26").Merge();
            sht.Range("C24:C26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C24:C26").Style.Font.FontSize = 12;
            sht.Range("C24").Value = "DESIGN ";


            sht.Range("D24:F26").Merge();
            sht.Range("D24:F26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D24:F26").Style.Font.FontSize = 12;
            sht.Range("D24").Value = "COLOR ";

            sht.Range("G24:I26").Merge();
            sht.Range("G24:I26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G24:I26").Style.Font.FontSize = 12;
            sht.Range("G24").Value = "SIZE ";

            sht.Range("J24:L26").Merge();
            sht.Range("J24:L26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J24:L26").Style.Font.FontSize = 12;
            sht.Range("J24").Value = "QUANTITY";

            sht.Range("M24:O25").Merge();
            sht.Range("M24:O25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("M24").Value = "AREA";

            sht.Range("P24:Q26").Merge();
            sht.Range("P24:Q26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P24").Value = "FOB/PRICE";

            sht.Range("P25:Q25").Merge();
            sht.Range("P25:Q25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P25").Value = ds.Tables[0].Rows[0]["Modofpayment"] + " " + ds.Tables[0].Rows[0]["Currencyname"];

            string unitname;
            if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "0")
            {
                switch (ds.Tables[0].Rows[0]["unitid"].ToString())
                {
                    case "1":
                        unitname = "SQ. Mtr";
                        break;
                    case "2":
                        unitname = "SQ. Ft";
                        break;
                    default:
                        unitname = ds.Tables[0].Rows[0]["unitname"].ToString();
                        break;
                }
            }
            else
            {
                unitname = "PC";
            }
            sht.Range("P26:Q26").Merge();
            sht.Range("P26:Q26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P26").Value = "Per " + unitname;

            sht.Range("M26:O26").Merge();
            sht.Range("M26:O26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("M26").Value = "Per " + unitname;

            sht.Range("R25:S25").Merge();
            sht.Range("R25:S25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("R25:S25").Value = "AMOUNT";

            sht.Range("R26:S26").Merge();
            sht.Range("R26:S26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("R26:S26").Value = ds.Tables[0].Rows[0]["currencyname"];

            //*****************
            int i;
            i = 27;
            int LastRowId = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":S" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":S" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":S" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["Qualityname"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["Designname"]);
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["colorname"]);
                sht.Range("G" + i + ":I" + i).Merge();
                sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);

                sht.Range("J" + i + ":L" + i).Merge();
                sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);

                sht.Range("M" + i + ":O" + i).Merge();
                sht.Range("M" + i).SetValue(ds.Tables[0].Rows[j]["totalarea"]);

                //sht.Range("M" + i + ":O" + i).Merge();
                //Decimal TotalArea = 0;
                //TotalArea = Convert.ToDecimal(ds.Tables[0].Rows[j]["totalarea"]) * Convert.ToDecimal(ds.Tables[0].Rows[j]["qtyrequired"]);
                //sht.Range("M" + i).SetValue(TotalArea);

                sht.Range("P" + i + ":Q" + i).Merge();
                sht.Range("P" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);

                sht.Range("R" + i + ":S" + i).Merge();
                sht.Range("R" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                //sht.Range("G" + i + ":I" + i).Merge();
                //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                //sht.Range("J" + i + ":K" + i).Merge();
                //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                //sht.Range("L" + i + ":M" + i).Merge();
                //sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                using (var a = sht.Range("A" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("B" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("C" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("D" + i + ":F" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("G" + i + ":I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i + ":L" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("M" + i + ":O" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("P" + i + ":Q" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("R" + i + ":S" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                i = i + 1;
                LastRowId = i;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("J" + i + ":L" + i).Merge();
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("M" + i + ":O" + i).Merge();
            using (var a = sht.Range("M" + i + ":O" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("R" + i + ":S" + i).Merge();
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            ////i = i + 1;
            ////sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////sht.Range("J" + i + ":L" + i).Merge();
            ////using (var a = sht.Range("J" + i + ":L" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////}

            ////sht.Range("M" + i + ":O" + i).Merge();
            ////using (var a = sht.Range("M" + i + ":O" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////}

            ////sht.Range("R" + i + ":S" + i).Merge();
            ////using (var a = sht.Range("R" + i + ":S" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////}
            ////i = i + 1;
            ////sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////sht.Range("J" + i + ":L" + i).Merge();
            ////using (var a = sht.Range("J" + i + ":L" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////}
            ////sht.Range("M" + i + ":O" + i).Merge();
            ////using (var a = sht.Range("M" + i + ":O" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ////}
            ////sht.Range("R" + i + ":S" + i).Merge();
            ////using (var a = sht.Range("R" + i + ":S" + i))
            ////{
            ////    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ////    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ////}
            //////******
            i = i + 1;
            sht.Range("A" + i + ":S" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":S" + i).Style.Font.FontSize = 10;

            sht.Range("J" + i + ":L" + i).Merge();
            sht.Range("J" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));

            sht.Range("M" + i + ":O" + i).Merge();
            //sht.Range("M" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));
            sht.Range("M" + i).FormulaA1 = "=sum(M" + 27 + ':' + "$M$" + LastRowId + ")";

            sht.Range("R" + i + ":S" + i).Merge();
            sht.Range("R" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            using (var a = sht.Range("A" + i + ":S" + i))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("J" + i + ":L" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("R" + i + ":S" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":S" + i).Merge();
            sht.Range("A" + i + ":S" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "Amount Chargeable in Word :-";

            i = i + 1;

            Decimal Totalmt = 0;
            Totalmt = Convert.ToDecimal(ds.Tables[0].Compute("sum(amount)", ""));
            string amountinwords = "";

            Decimal IntegerPartofTotalAmt = 0;
            IntegerPartofTotalAmt = Math.Floor(Math.Abs(Totalmt));

            amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(IntegerPartofTotalAmt));

            string Pointamt = string.Format("{0:0.00}", Totalmt.ToString("0.00"));
            string val = "", paise = "";
            if (Pointamt.IndexOf('.') > 0)
            {
                val = Pointamt.ToString().Split('.')[1];
                if (Convert.ToInt32(val) > 0)
                {
                    paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                }
            }

            amountinwords = ds.Tables[0].Rows[0]["currencyName"] + " " + amountinwords + " " + paise + "Only";

            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":S" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = amountinwords.ToUpper();

            i = i + 1;



            //signature and date
            i = i + 1;
            sht.Range("J" + i + ":S" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i + ":S" + i).Style.Font.Bold = true;
            sht.Range("J" + i + ":S" + i).Merge();
            sht.Range("J" + i).Value = "For " + ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //Declaration
            i = i + 1;
            sht.Range("A" + i).Value = "Declaration:";
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //*************
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "We declare that this invoice show the actual price of the goods";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "described and that all particulars are true and correct";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("S" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":S" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //***********Borders
            sht.Range("A1:S1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:A29").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("D2:D26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A7:S7").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E3:S3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("K2:K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("H2:H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("S2:S29").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("E5:S5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A15:S15").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E13:S13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("J14:J15").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A17:D17").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A19:D19").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A21:S21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A16:A26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("B22:B26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A26:S26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            using (var a = sht.Range("A24:A26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("C24:C26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("G24:I26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("J24:L26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("M24:O26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("P24:Q26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("R24:S26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            //using (var a = sht.Range("K24:K26"))
            //{
            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //}
            sht.Range("A23:S23").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //***********

            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoiceChampo-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

            //Session["rptFileName"] = "~\\Reports\\Rptorderperformainvoice.rpt";                   
            //Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\Rptorderperformainvoice.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }

    private void BomDetail()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@OrderID", ViewState["order_id"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetBomDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;

            sht.Range("A1").Value = "BOM Sheet Details";
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J1").Merge();

            //Headers
            sht.Range("A2").Value = "Buyer Code";
            sht.Range("B2").SetValue(ds.Tables[0].Rows[0]["CustomerCode"]);
            sht.Range("C2").Value = "Inspection date";
            sht.Range("D2").Value = "Inspection qty";
            sht.Range("E2").Value = "Final Inspection";
            sht.Range("F2").Value = "Final ins.qty";
            sht.Range("G2").Value = "Ex-factory";
            sht.Range("H2").Value = "Ex-fact.qty";
            sht.Range("I2").Value = "DOC Title";
            sht.Range("A3").Value = "BPO";
            sht.Range("B3").SetValue(ds.Tables[0].Rows[0]["CustomerOrderNo"]);

            sht.Range("C3").SetValue(ds.Tables[0].Rows[0]["InspectionDateNew"]);
            sht.Range("D3").SetValue(ds.Tables[0].Rows[0]["InspectionQtyNew"]);
            sht.Range("E3").SetValue(ds.Tables[0].Rows[0]["FinalInspectionDateNew"]);
            sht.Range("F3").SetValue(ds.Tables[0].Rows[0]["FinalInspectionQtyNew"]);
            sht.Range("G3").SetValue(ds.Tables[0].Rows[0]["ExfactoryDate"]);
            sht.Range("I3").Value = "Doc No";
            sht.Range("A4").Value = "Order Date";
            sht.Range("B4").SetValue(ds.Tables[0].Rows[0]["OrderDate"]);
            sht.Range("I4").Value = "Date";
            sht.Range("I5").Value = "Version";

            sht.Range("A6").Value = "Design & Quality Wise RM Req.";
            sht.Range("A6:I6").Style.Font.Bold = true;
            sht.Range("A6:I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A6:I6").Merge();
            sht.Range("A7").Value = "Design Code";
            sht.Range("B7").Value = "Count";
            sht.Range("C7").Value = "Consmp Qty";
            sht.Range("D7").Value = "Supplier Name(to be fild manually)";
            sht.Range("E7").Value = "Remark";
            sht.Range("A7:J7").Style.Font.Bold = true;
            row = 8;
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[1].Rows[i]["DesignName"]);
                sht.Range("B" + row).SetValue(ds.Tables[1].Rows[i]["QualityName"]);
                sht.Range("C" + row).SetValue(ds.Tables[1].Rows[i]["OrderConsmpQty"]);
                row = row + 1;
            }
            row = row + 1;
            sht.Range("A" + row).Value = "Summary Of RM Req.";
            sht.Range("B" + row).Value = "Count";
            sht.Range("C" + row).Value = "Consmp Qty";
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            row = row + 1;

            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                sht.Range("B" + row).SetValue(ds.Tables[2].Rows[i]["QualityName"]);
                sht.Range("C" + row).SetValue(ds.Tables[2].Rows[i]["OrderConsmpQty"]);
                row = row + 1;
            }
            row = row + 1;
            sht.Range("A" + row).Value = "Chemicals For Latex";
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":J" + row).Merge();

            row = row + 1;

            sht.Range("A" + row).Value = "Recipe of latex Gm/sqyd";
            sht.Range("B" + row).Value = "Unit Name";
            sht.Range("C" + row).Value = "Consmp Qty";
            sht.Range("D" + row).Value = "Total Consmp Qty";
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;

            row = row + 1;
            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[3].Rows[i]["Description"]);
                sht.Range("B" + row).SetValue(ds.Tables[3].Rows[i]["UnitName"]);
                sht.Range("C" + row).SetValue(ds.Tables[3].Rows[i]["ConsmpQty"]);
                sht.Range("D" + row).SetValue(ds.Tables[3].Rows[i]["TotalConsmpQty"]);
                row = row + 1;
            }

            row = row + 1;
            sht.Range("A" + row).Value = "Other Requirment For Final Finishing";
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":J" + row).Merge();
            row = row + 1;

            sht.Range("A" + row).Value = "Process Name";
            sht.Range("B" + row).Value = "Description";
            sht.Range("C" + row).Value = "Unit Name";
            sht.Range("D" + row).Value = "Consmp Qty";
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            row = row + 1;
            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[4].Rows[i]["ProcessName"]);
                sht.Range("B" + row).SetValue(ds.Tables[4].Rows[i]["Description"]);
                sht.Range("C" + row).SetValue(ds.Tables[4].Rows[i]["UnitName"]);
                sht.Range("D" + row).SetValue(ds.Tables[4].Rows[i]["OrderConsmpQty"]);
                row = row + 1;
            }

            sht.Columns(1, 20).AdjustToContents();
            using (var a = sht.Range("A1:J" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BomSheetFormat:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void PerformainvoiceSundeepExport()
    {
        string str = "select * From V_PerformaInvoice Where orderid=" + ViewState["order_id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PROFORMA INVOICE");
            //Page
            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            //sht.PageSetup.FitToPages(1, 5);           
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            //
            sht.PageSetup.Margins.Top = 0.25;
            sht.PageSetup.Margins.Left = 0.236220472440945;
            sht.PageSetup.Margins.Right = 0.236220472440945;
            sht.PageSetup.Margins.Bottom = 0.236220472440945;
            sht.PageSetup.Margins.Header = 0.669291338582677;
            sht.PageSetup.Margins.Footer = 0.511811023622047;
            sht.PageSetup.CenterHorizontally = true;
            sht.PageSetup.CenterVertically = true;
            //sht.PageSetup.SetScaleHFWithDocument();
            //************
            //set columnwidth
            sht.Columns("A").Width = 16.43;
            sht.Columns("B").Width = 15.29;
            sht.Columns("C").Width = 13.43;
            sht.Columns("D").Width = 7.71;
            sht.Columns("E").Width = 3.14;
            sht.Columns("F").Width = 1.71;
            sht.Columns("G").Width = 4.29;
            sht.Columns("H").Width = 2.29;
            sht.Columns("I").Width = 3.86;
            sht.Columns("J").Width = 6.57;
            sht.Columns("K").Width = 7.57;
            sht.Columns("L").Width = 10.14;
            sht.Columns("M").Width = 6.71;
            sht.Columns("N").Width = 15.29;
            sht.Columns("O").Width = 15.29;
            sht.Row(1).Height = 21;
            //**************
            sht.Range("A1:O1").Merge();
            sht.Range("A1").Value = "P R O F O R M A   I N V O I C E";
            sht.Range("A1:O1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:O1").Style.Font.FontSize = 16;
            sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************
            sht.Range("A2:D2").Merge();
            sht.Range("A2:D2").Style.Font.FontSize = 11;
            sht.Range("A2:D2").Style.Font.Bold = true;


            sht.Range("A2").Value = "Exporter";
            sht.Range("A3:D3").Style.Font.Bold = true;
            sht.Range("A3:D3").Style.Font.FontSize = 13;


            sht.Range("A3:D3").Merge();
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A4:D4").Merge();
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A5:D5").Merge();
            sht.Range("A5").Value = "TEL :" + ds.Tables[0].Rows[0]["Comptel"] + " FAX : " + ds.Tables[0].Rows[0]["Compfax"];
            sht.Range("A6:D6").Merge();
            sht.Range("A6").Value = "E-MAIL : " + ds.Tables[0].Rows[0]["email"];
            //'Invoice NO Date
            sht.Range("E2:H2").Merge();
            sht.Range("E2:H3").Style.Font.FontSize = 11;
            sht.Range("E2:H3").Style.Font.Bold = true;
            sht.Range("E2").Value = "Invoice No.";
            //'value
            sht.Range("I2:K2").Merge();
            sht.Range("I2").SetValue(ds.Tables[0].Rows[0]["invoiceno"]);

            sht.Range("E3:H3").Merge();
            sht.Range("E3").Value = "Invoice Date";
            sht.Range("I3:K3").Merge();
            sht.Range("I3").SetValue(ds.Tables[0].Rows[0]["invoicedate"]);
            //********Exporterref
            sht.Range("L2:O2").Merge();
            sht.Range("L2:O2").Style.Font.FontSize = 11;
            sht.Range("L2:O2").Style.Font.Bold = true;
            sht.Range("L2").Value = "Exporter's Ref.";
            //'value
            sht.Range("L3:O3").Merge();
            sht.Range("L3").Value = "";
            //*******
            sht.Range("E4:O4").Merge();
            sht.Range("E4").Value = "Buyer's Order No. & date";
            sht.Range("E4:O4").Style.Font.FontSize = 11;
            sht.Range("E4:O4").Style.Font.Bold = true;

            sht.Range("E5:O5").Merge();
            sht.Range("E5").Value = "PO# " + ds.Tables[0].Rows[0]["customerorderNo"] + "/" + ds.Tables[0].Rows[0]["orderdate"];

            //'Other ref
            sht.Range("E6:O6").Merge();
            sht.Range("E6").Value = "Other Reference(s) :";
            sht.Range("E6:O6").Style.Font.FontSize = 11;
            sht.Range("E6:O6").Style.Font.Bold = true;
            sht.Range("E7:O7").Merge();
            sht.Range("E7").Value = "";

            //'consignee

            sht.Range("A8:D8").Merge();
            sht.Range("A8:D8").Style.Font.FontSize = 11;
            sht.Range("A8:D8").Style.Font.Bold = true;
            sht.Range("A8").Value = "Consignee";
            //'value
            sht.Range("A9:D9").Merge();
            sht.Range("A9").Value = ds.Tables[0].Rows[0]["customercompany"];
            sht.Range("A10:D11").Merge();
            sht.Range("A10").Value = ds.Tables[0].Rows[0]["customeraddress"];
            sht.Range("A10:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A10:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A10").Style.Alignment.WrapText = true;

            sht.Range("A12:D12").Merge();
            sht.Range("A12").Value = "TEL :" + ds.Tables[0].Rows[0]["customerphoneno"] + " FAX : " + ds.Tables[0].Rows[0]["customerfax"];
            sht.Range("A13:D13").Merge();
            sht.Range("A13").Value = "E-MAIL :" + ds.Tables[0].Rows[0]["customermail"];
            sht.Range("A14:D14").Merge();
            sht.Range("A15:D15").Merge();
            //'*******Buyerotherthanconsignee

            sht.Range("E8:O8").Merge();
            sht.Range("E8").Value = "Buyer (if other than consignee)";
            sht.Range("E8:O8").Style.Font.FontSize = 11;
            sht.Range("E8:O8").Style.Font.Bold = true;
            //'
            //sht.Range("E10:M10").Merge();
            //sht.Range("E10:M10").Style.Font.FontSize = 11;
            //sht.Range("E10:M10").Style.Font.Bold = true;
            //sht.Range("E10").SetValue("SHIP DATE :- " + ds.Tables[0].Rows[0]["Dispatchdate"]);
            //'******Country of origin of goods
            sht.Range("E14:J14").Merge();
            sht.Range("E14:J14").Style.Font.FontSize = 11;
            sht.Range("E14:J14").Style.Font.Bold = true;
            sht.Range("E14").Value = "Country of Origin of goods";
            //'value
            sht.Range("E15:H15").Merge();
            sht.Range("E15:H15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E15").Value = "INDIA";
            //'Country of final destination
            sht.Range("K14:O14").Merge();
            sht.Range("K14:O14").Style.Font.FontSize = 11;
            sht.Range("K14:O14").Style.Font.Bold = true;
            sht.Range("K14").Value = "Country of Final Destination";
            //'value
            sht.Range("K15:O15").Merge();
            sht.Range("K15:O15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("K15").Value = ds.Tables[0].Rows[0]["countryname"];
            //'****Precarrigeby

            sht.Range("A16").Value = "Pre-Carriage by";
            sht.Range("A16").Style.Font.FontSize = 11;
            sht.Range("A16").Style.Font.Bold = true;
            //'
            sht.Range("A17").Value = ds.Tables[0].Rows[0]["Precarriageby"];
            //'*****Place of Receipt
            sht.Range("B16:D16").Merge();
            sht.Range("B16:D16").Style.Font.FontSize = 11;
            sht.Range("B16:D16").Style.Font.Bold = true;
            sht.Range("B16").Value = "Place of Receipt";
            //'
            sht.Range("B17:D17").Merge();
            sht.Range("B17").Value = ds.Tables[0].Rows[0]["Placeofreceipt"];
            //'*****vessel_flight no
            sht.Range("A18").Value = "Mode of Shipment";
            sht.Range("A18").Style.Font.FontSize = 11;
            sht.Range("A18").Style.Font.Bold = true;

            //'
            sht.Range("A19").Value = ds.Tables[0].Rows[0]["modeofshipment"];
            //'**********port of loading
            sht.Range("B18:D18").Merge();
            sht.Range("B18:D18").Style.Font.FontSize = 11;
            sht.Range("B18:D18").Style.Font.Bold = true;
            sht.Range("B18").Value = "Port of Loading";
            //'
            sht.Range("B19:D19").Merge();
            sht.Range("B19").Value = ds.Tables[0].Rows[0]["portofloading"];
            //'********Port of discharge
            sht.Range("A20").Value = "Port of Discharge";
            sht.Range("A20").Style.Font.FontSize = 11;
            sht.Range("A20").Style.Font.Bold = true;
            //'
            sht.Range("A21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //'***********Final destination
            sht.Range("B20:D20").Merge();
            sht.Range("B20:D20").Style.Font.FontSize = 11;
            sht.Range("B20:D20").Style.Font.Bold = true;

            sht.Range("B20").Value = "Final Destination";
            //'
            sht.Range("B21:D21").Merge();
            sht.Range("B21").Value = ds.Tables[0].Rows[0]["destinationplace"];
            //SHIPDATE
            sht.Range("E20:O20").Merge();
            sht.Range("E20").Value = "SHIP DATE :-" + ds.Tables[0].Rows[0]["Dispatchdate"];
            sht.Range("E20:O20").Style.Font.FontSize = 11;
            sht.Range("E20:O20").Style.Font.Bold = true;
            sht.Range("E20:O20").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //term of sale
            sht.Range("E16:O16").Merge();
            sht.Range("E16").Value = "Terms of Delivery and Payment";
            sht.Range("E16:O16").Style.Font.FontSize = 11;
            sht.Range("E16:O16").Style.Font.Bold = true;
            sht.Range("E18:O18").Merge();
            sht.Range("E19:O19").Merge();
            sht.Range("E18").Value = ds.Tables[0].Rows[0]["Modofpayment"];
            sht.Range("E19").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
            //'*****Mark & Nos.
            sht.Range("A22:B23").Style.Font.FontSize = 11;
            sht.Range("A22:B23").Style.Font.Bold = true;
            sht.Range("A22").Value = "Marks & Nos.";
            sht.Range("B22").Value = "No. & Kind";
            sht.Range("B23").Value = "of Packages";
            //'******Description of goods
            sht.Range("C22:D22").Merge();
            sht.Range("C22").Value = "Description of Goods";
            sht.Range("C22:D22").Style.Font.FontSize = 11;
            sht.Range("C22:D22").Style.Font.Bold = true;
            //'******Detail Headers
            sht.Range("A24:O26").Style.Font.FontSize = 10;
            sht.Range("A24:O26").Style.Font.Bold = true;
            sht.Range("A24:O26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A24:A26").Merge();
            sht.Range("A24:A26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A24:A26").Style.Font.FontSize = 12;
            sht.Range("A24").Value = "SKU NO";

            sht.Range("B24:B26").Merge();
            sht.Range("B24:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B24:B26").Style.Font.FontSize = 12;
            sht.Range("B24").Value = "COMPOSITION";

            sht.Range("C24:C26").Merge();
            sht.Range("C24:C26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C24:C26").Style.Font.FontSize = 12;
            sht.Range("C24").Value = "QUALITY";


            sht.Range("D24:F26").Merge();
            sht.Range("D24:F26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("D24:F26").Style.Font.FontSize = 12;
            sht.Range("D24").Value = "DESIGN";

            sht.Range("G24:I26").Merge();
            sht.Range("G24:I26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G24").Value = "COLOR";

            sht.Range("J24:K26").Merge();
            sht.Range("J24:K26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J24").Value = "SIZE";

            sht.Range("L24:M26").Merge();
            sht.Range("L24:M26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L24:M26").Style.Font.FontSize = 12;
            sht.Range("L24").Value = "QUANTITY";

            sht.Range("N24:N24").Merge();
            sht.Range("N24:N26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N24").Value = "RATE";

            sht.Range("N25:N25").Merge();
            sht.Range("N25:N25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N25").Value = ds.Tables[0].Rows[0]["Modofpayment"] + " " + ds.Tables[0].Rows[0]["Currencyname"];

            string unitname;
            if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "0")
            {
                switch (ds.Tables[0].Rows[0]["unitid"].ToString())
                {
                    case "1":
                        unitname = "SQ. Mtr";
                        break;
                    case "2":
                        unitname = "SQ. Ft";
                        break;
                    default:
                        unitname = ds.Tables[0].Rows[0]["unitname"].ToString();
                        break;
                }
            }
            else
            {
                unitname = "PC";
            }
            sht.Range("N26:N26").Merge();
            sht.Range("N26:N26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N26").Value = "Per " + unitname;

            sht.Range("O25:O25").Merge();
            sht.Range("O25:O25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("O25:O25").Value = "AMOUNT";

            sht.Range("O26:O26").Merge();
            sht.Range("O26:O26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("O26:O26").Value = ds.Tables[0].Rows[0]["currencyname"];
            //*****************
            int i;
            i = 27;
            double TotalArea = 0;
            double GrandTotalArea = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":O" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i + ":O" + i).Style.Alignment.WrapText = true;
                sht.Range("A" + i + ":O" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["SKUNo"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["Composition"]);

                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["Qualityname"]);
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Designname"]);
                sht.Range("G" + i + ":I" + i).Merge();
                sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["colorname"]);
                sht.Range("J" + i + ":K" + i).Merge();
                sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);
                sht.Range("L" + i + ":M" + i).Merge();
                sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                sht.Range("N" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                sht.Range("O" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);


                //sht.Range("D" + i + ":F" + i).Merge();
                //sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);
                //sht.Range("G" + i + ":I" + i).Merge();
                //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["qtyrequired"]);
                //sht.Range("J" + i + ":K" + i).Merge();
                //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[j]["unitrate"]);
                //sht.Range("L" + i + ":M" + i).Merge();
                //sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["amount"]);

                TotalArea = Convert.ToDouble(ds.Tables[0].Rows[j]["qtyrequired"]) * Convert.ToDouble(ds.Tables[0].Rows[j]["TotalArea"]);
                GrandTotalArea = GrandTotalArea + TotalArea;

                using (var a = sht.Range("A" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("B" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("C" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("D" + i + ":F" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("G" + i + ":I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i + ":K" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("L" + i + ":M" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("N" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("O" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                i = i + 1;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G" + i + ":I" + i).Merge();
            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            }
            sht.Range("L" + i + ":M" + i).Merge();
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            //******
            i = i + 1;
            sht.Range("A" + i + ":O" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + i + ":O" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":O" + i).Style.Font.FontSize = 10;

            sht.Range("L" + i + ":M" + i).Merge();
            sht.Range("L" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", ""));

            sht.Range("O" + i + ":O" + i).Merge();
            sht.Range("O" + i).SetValue(ds.Tables[0].Compute("sum(amount)", ""));

            using (var a = sht.Range("A" + i + ":O" + i))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("M" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            using (var a = sht.Range("G" + i + ":I" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("L" + i + ":M" + i))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL PCS :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(qtyrequired)", "") + " PCS");

            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Value = "TOTAL AMOUNT :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(amount)", "") + " " + ds.Tables[0].Rows[0]["currencyname"]);
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            string unit = "";

            switch (ds.Tables[0].Rows[0]["flagsize"].ToString())
            {
                case "1":
                    unit = "SQ.MTR";
                    break;
                case "0":
                    unit = "SQ.FT";
                    break;
            }
            sht.Range("A" + i).Value = "TOTAL " + unit + " :-";

            sht.Range("B" + i + ":C" + i).Merge();
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("B" + i).SetValue(GrandTotalArea + " " + unit);
            //sht.Range("B" + i).SetValue(ds.Tables[0].Compute("sum(totalarea)", "") + " " + unit);

            //*************amount in words
            //string amountinwords = "";
            //Decimal Amount = Convert.ToDecimal(ds.Tables[0].Compute("sum(amount)", ""));
            //amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
            //string val = "", paise = "";
            //if (Amount.ToString().IndexOf('.') > 0)
            //{
            //    val = Amount.ToString().Split('.')[1];
            //    if (Convert.ToInt32(val) > 0)
            //    {
            //        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
            //    }
            //}
            //amountinwords = ds.Tables[0].Rows[0]["currencyname"] + " " + amountinwords + " " + paise + "Only";
            //i = i + 1;
            //sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //sht.Range("M" + i + ":M" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //sht.Range("A" + i + ":I" + (i + 1)).Merge();
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.WrapText = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Font.Bold = true;
            //sht.Range("A" + i + ":I" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            //sht.Range("A" + i).Value = amountinwords;
            //Bank Details  
            sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 2;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Beneficiary:";
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["bankname"] + "," + ds.Tables[0].Rows[0]["City"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Value = "SWIFT CODE :";
            sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["swiftcode"];
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account:";
            sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":C" + i).Style.Font.SetUnderline();
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = ds.Tables[0].Rows[0]["Accountname"];
            i = i + 1;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":C" + i).Merge();
            sht.Range("A" + i).Value = "Account no." + ds.Tables[0].Rows[0]["acno"];
            //signature and date
            i = i + 1;
            sht.Range("J" + i + ":O" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("J" + i + ":O" + i).Style.Font.Bold = true;
            sht.Range("J" + i + ":O" + i).Merge();
            sht.Range("J" + i).Value = "For " + ds.Tables[0].Rows[0]["Companyname"];
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //Declaration
            i = i + 1;
            sht.Range("A" + i).Value = "Declaration:";
            sht.Range("A" + i).Style.Font.FontSize = 10;
            sht.Range("A" + i).Style.Font.Bold = true;
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //*************
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "We declare that this invoice show the actual price of the goods";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            i = i + 1;
            sht.Range("A" + i + ":G" + i).Merge();
            sht.Range("A" + i).Value = "described and that all particulars are true and correct";
            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("O" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":O" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //***********Borders
            sht.Range("A1:O1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:A29").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("D2:D26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A7:O7").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E3:O3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("K2:K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("H2:H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("O2:O29").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("E5:O5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A15:O15").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("E13:O13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("J14:J15").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A17:D17").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A19:D19").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A21:O21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A16:A26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("B22:B26").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A26:O26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            using (var a = sht.Range("A24:A26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("C24:C26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("G24:I26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("N24:N26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("O24:O26"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("K24:K26"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A23:M23").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //***********

            string Path;
            string Fileextension = "xlsx";
            string filename = "ProformaInvoiceSundeepExport-" + UtilityModule.validateFilename("PO#" + ds.Tables[0].Rows[0]["customerorderNo"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void btnUpdateFolioReceiveCons_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@OrderID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = ViewState["order_id"];
            _arrPara[1].Value = Session["varuserid"].ToString();
            _arrPara[2].Value = Session["varCompanyId"].ToString();
            _arrPara[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "UpdateOrderWiseProcessReceiveConsumption", _arrPara);

            if (_arrPara[3].Value.ToString() == "consumption updated successfully.")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Updated');", true);
                tran.Commit();
            }
            else
            {
                LblErrorMessage.Text = _arrPara[3].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("Master|Order||" + ex.Message);
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
}