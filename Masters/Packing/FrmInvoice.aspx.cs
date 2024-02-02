using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;

public partial class Masters_Packing_FrmInvoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();
            //Session["CurrentWorkingCompanyID"].ToString();
            string Qry = @"Select Year,Session From Session Order By Year Desc";

            DataSet ds1 = null;
            ds1 = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDInvoiceYear, ds1, 0, true, "--SELECT--");
            DDInvoiceYear.Focus();
           

            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "30":
                    BtnRollWeightNew.Visible = true;
                    BtnRollWt.Visible = false;
                    TRPileBase.Visible = false;
                    TDExportIGST.Visible = false;
                    BtnRateNew.Visible = true;
                    BtnRate.Visible = false;
                    TRPackingCharges.Visible = true;
                    TDFreightTerms.Visible = false;
                    TDDays.Visible = false;
                    TDAdvanceRec.Visible = false;
                    TDInsurance.Visible = false;
                    break;
                case "20":
                    BtnRollWeightNew.Visible = false;
                    BtnRollWt.Visible = true;
                    TRPileBase.Visible = true;
                    TDExportIGST.Visible = true;
                    ChkExportIGST.Checked = true;
                    BtnRateNew.Visible = false;
                    TRPackingCharges.Visible = false;
                    break;
                case "40":
                    BtnRollWeightNew.Visible = false;
                    BtnRollWt.Visible = true;
                    TRPileBase.Visible = false;
                    TDExportIGST.Visible = false;
                    BtnRateNew.Visible = false;
                    TRPackingCharges.Visible = false;
                    BtnPreview.Visible = true;
                    break;
                default:
                    BtnRollWeightNew.Visible = false;
                    BtnRollWt.Visible = true;
                    TRPileBase.Visible = false;
                    TDExportIGST.Visible = false;
                    BtnRateNew.Visible = false;
                    TRPackingCharges.Visible = false;
                    BtnPreview.Visible = false;
                    break;
            }

            UtilityModule.ConditionalComboFill(ref DDGoodsDescription, "Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GoodsName", true, "--Select--");
        }
    }
    protected void DDInvoiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDInvoiveNo.SelectedIndex > 0)
        {
            RDCustomer.Checked = false;
            RDBank.Checked = false;
            RDUnToOrder.Checked = true;
            //**************RadioButton Checked
            string sql = "select TypeOFBuyerOtherConsignee from Invoice where invoiceid=" + DDInvoiveNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (ds.Tables[0].Rows[0]["TypeOFBuyerOtherConsignee"].ToString())
                {
                    case "1":
                        RDUnToOrder.Checked = true;
                        DDBuyerOtherThanConsignee.SelectedIndex = -1;

                        break;
                    case "2":
                        RDCustomer.Checked = true;

                        break;
                    case "3":
                        RDBank.Checked = true;

                        break;
                    default:
                        RDUnToOrder.Checked = true;
                        DDBuyerOtherThanConsignee.SelectedIndex = -1;
                        break;
                }
            }

            // //**************
            //ChangeAccToRadioButton();
            RadioButtonselection();
            Fill_DropDownList();
            TxtInvoiceId.Text = DDInvoiveNo.SelectedValue;
            TDButtonShow.Visible = true;
        }
        else
        {
            TDButtonShow.Visible = false;
        }
    }
    private void Fill_DropDownList()
    {
        string Str = "";
        Str = Str + @"Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                Select Agentid,Agentname from Shipp Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Agentname
                select carriageid,carriageName from Carriage Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by carriageName
                Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
                Select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by TransModename
                Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
                Select PaymentId, PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PaymentName
                Select TermId,TermName from Term Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By TermName
                Select Bankid,BankName from Bank Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by BankName
                Select PaymentId, PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PaymentName";
        DataSet Ds = null;
        Ds = SqlHelper.ExecuteDataset(Str);
        UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, Ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShippingAgent, Ds, 1, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDPreCarriage, Ds, 2, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDReceiptAt, Ds, 3, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDByAirSea, Ds, 4, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDPortLoad, Ds, 5, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDDelivery, Ds, 6, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDTerms, Ds, 7, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDBank, Ds, 8, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDPaymentTermCustom, Ds, 9, true, "--Select--");
        DDCurrency.SelectedIndex = 0;
        DDShippingAgent.SelectedIndex = 0;
        DDPreCarriage.SelectedIndex = 0;
        DDReceiptAt.SelectedIndex = 0;
        DDByAirSea.SelectedIndex = 0;
        DDPortLoad.SelectedIndex = 0;
        DDDelivery.SelectedIndex = 0;
        DDTerms.SelectedIndex = 0;
        DDBank.SelectedIndex = 0;
        DDPaymentTermCustom.SelectedIndex = 0;

        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            Str = @"Select isnull(I.CurrencyType,P.CurrencyId) as CurrencyId,
                isnull(I.AgentId,CI.ShippingAgent) as ShippingAgent,isnull(I.PreCarrier,CI.PreCarriageBy) as PreCarriageBy,
                isnull(I.Receipt,CI.RecieptAtByPreCarrier) as RecieptAtByPreCarrier,
                isnull(I.ShipingId,CI.ByAirSea) as ByAirSea,isnull(I.portload,CI.PortOfLoading) as PortOfLoading,
                isnull(I.RollmarkHead,CI.Mark) as Mark,isnull(I.Destinationadd,CI.DestinationPlace) as DestinationPlace,
                isnull(I.DelTerms,CI.PaymentId) as PaymentId,isnull(I.CreditId,CI.TermId) as TermId
                ,isnull(I.BankId,CI.BankId) as BankId,isnull(I.PortUnload,'') as PortunLoad,I.Insurance,I.discountamt,
                I.GrossWt,I.NetWt,Replace(convert(nvarchar(11),InvoiceDate,106),' ','-') as invoicedate,isnull(I.CGST,0) as CGST,isnull(I.SGST,0) as SGST,isnull(I.IGST,0) as IGST,
                isnull(I.INRRate,0) as INRRate,isnull(I.descriptionofgoods,0) as DescriptionOfGoods,isnull(round(sum(PI.Area*PI.Price),2),0) as TotalAmount
                ,IsNull(IsNull(I.PaymentTermCustom, CI.PaymentIdCustom), 0) as PaymentIdCustom,isnull(I.CBM,0) as CBM,isnull(I.ShipToAddress,'') as ShipToAddress,isnull(I.DiscountRemark,'') as DiscountRemark,
                isnull(I.GstinType,'') as GstinType,isnull(I.EndUse,'') as EndUse,isnull(I.SUQty,'') as SUQty,isnull(I.PreferentialAgreement,'') as PreferentialAgreement,
                isnull(I.PackingCharges,'') as PackingCharges,isnull(I.LessAdvance,0) as LessAdvance,isnull(I.ExtraCharges,0) as ExtraCharges,isnull(I.Freight,0) as Freight,
                isnull(I.Ex1Rate,0) as Ex1Rate,isnull(I.Ex2Rate,0) as Ex2Rate,isnull(I.Composition,'') as Composition
                From Packing P	JOIN PackingInformation PI ON P.Packingid=PI.PackingId
				JOIN CustomerInfo CI ON P.ConsigneeId=CI.CustomerID 
				JOIN Invoice I ON P.PackingId=I.PackingId
                Where P.PackingID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                 Group by I.CurrencyType,P.CurrencyId,I.AgentId,CI.ShippingAgent,I.PreCarrier,CI.PreCarriageBy,I.Receipt,CI.RecieptAtByPreCarrier,
				I.ShipingId,CI.ByAirSea,I.portload,CI.PortOfLoading,I.RollmarkHead,CI.Mark,I.Destinationadd,CI.DestinationPlace,I.DelTerms,CI.PaymentId,
				I.CreditId,CI.TermId,I.BankId,CI.BankId,I.PortUnload,I.Insurance,I.discountamt,I.GrossWt,I.NetWt,InvoiceDate,I.CGST,I.SGST,I.IGST,I.INRRate,
				I.descriptionofgoods,I.PaymentTermCustom,CI.PaymentIdCustom,I.ShipToAddress,I.DiscountRemark,I.GstinType,I.EndUse,I.CBM,I.SUQty,I.PreferentialAgreement,I.PackingCharges,
                I.LessAdvance,I.ExtraCharges,I.Freight,I.Ex1Rate,I.Ex2Rate,I.Composition";
        }
        else if (Session["varMasterCompanyIDForERP"].ToString() == "20")
        {
            Str = @"Select isnull(I.CurrencyType,P.CurrencyId) as CurrencyId,
                isnull(I.AgentId,CI.ShippingAgent) as ShippingAgent,isnull(I.PreCarrier,CI.PreCarriageBy) as PreCarriageBy,
                isnull(I.Receipt,CI.RecieptAtByPreCarrier) as RecieptAtByPreCarrier,
                isnull(I.ShipingId,CI.ByAirSea) as ByAirSea,isnull(I.portload,WHM.WHPortOfLoading) as PortOfLoading,
                isnull(I.RollmarkHead,CI.Mark) as Mark,isnull(I.Destinationadd,CI.DestinationPlace) as DestinationPlace,
                isnull(I.DelTerms,CI.PaymentId) as PaymentId,isnull(I.CreditId,CI.TermId) as TermId
                ,isnull(I.BankId,CI.BankId) as BankId,isnull(I.PortUnload,'') as PortunLoad,I.Insurance,I.discountamt,
                isnull(GrossWt,0) as GrossWt,isnull(NetWt,0) as NetWt,Replace(convert(nvarchar(11),InvoiceDate,106),' ','-') as invoicedate,isnull(I.CGST,0) as CGST,isnull(I.SGST,0) as SGST,isnull(I.IGST,0) as IGST,
                isnull(I.INRRate,0) as INRRate,isnull(I.descriptionofgoods,0) as DescriptionOfGoods,isnull(I.ShipToAddress,'') as ShipToAddress,isnull(I.DiscountRemark,'') as DiscountRemark,
                isnull(I.GstinType,'') as GstinType,isnull(I.EndUse,'') as EndUse,isnull(I.CBM,0) as CBM,isnull(I.RawMaterialPile,'') as RawMaterialPile,isnull(I.RawMaterialBase,'') as RawMaterialBase
                ,IsNull(IsNull(I.PaymentTermCustom, CI.PaymentIdCustom), 0) as PaymentIdCustom,isnull(I.PortUnload, WHM.WHPortOfDischarge) as PortOfDischarge,
				isnull(I.CountryOfFinalDestination,WHM.WHCountryFinalDestination) as CountryFinalDestination,isnull(I.PlaceOfDelivery,WHM.WHPlaceOfDelivery) as PlaceOfDelivery,
                isnull(I.DestinationAdd,WHM.WHFinalDestination) as FinalDestination,
                isnull(I.WareHouseName,(Select distinct cast( VP.WareHouseNameByCode as varchar) +',' From V_PackingWareHouseName VP Where VP.PackingId=P.PackingId For xml path(''))) as WareHouseName
                ,isnull(I.ExportAgainstIGST,0) as ExportAgainstIGST,isnull(I.ExportAgainstLUT,0) as ExportAgainstLUT,isnull(I.Composition,'') as Composition
                From Packing P JOIN CustomerInfo CI ON P.ConsigneeId=CI.CustomerID
				JOIN Invoice I ON P.PackingId=I.PackingId
				LEFT JOIN WareHouseMaster WHM ON P.ConsigneeId=WHM.CustomerId and P.WareHouseID=WHM.WareHouseID
                Where P.PackingID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }
        else
        {
            Str = @"Select isnull(I.CurrencyType,P.CurrencyId) as CurrencyId,
                isnull(I.AgentId,CI.ShippingAgent) as ShippingAgent,isnull(I.PreCarrier,CI.PreCarriageBy) as PreCarriageBy,
                isnull(I.Receipt,CI.RecieptAtByPreCarrier) as RecieptAtByPreCarrier,
                isnull(I.ShipingId,CI.ByAirSea) as ByAirSea,isnull(I.portload,CI.PortOfLoading) as PortOfLoading,
                isnull(I.RollmarkHead,CI.Mark) as Mark,isnull(I.Destinationadd,CI.DestinationPlace) as DestinationPlace,
                isnull(I.DelTerms,CI.PaymentId) as PaymentId,isnull(I.CreditId,CI.TermId) as TermId
                ,isnull(I.BankId,CI.BankId) as BankId,isnull(I.PortUnload,'') as PortunLoad,I.Insurance,I.discountamt,
                isnull(GrossWt,0) as GrossWt,isnull(NetWt,0) as NetWt,Replace(convert(nvarchar(11),InvoiceDate,106),' ','-') as invoicedate,isnull(I.CGST,0) as CGST,isnull(I.SGST,0) as SGST,isnull(I.IGST,0) as IGST,
                isnull(I.INRRate,0) as INRRate,isnull(I.descriptionofgoods,0) as DescriptionOfGoods,isnull(I.ShipToAddress,'') as ShipToAddress,isnull(I.DiscountRemark,'') as DiscountRemark,
                isnull(I.GstinType,'') as GstinType,isnull(I.EndUse,'') as EndUse,isnull(I.CBM,0) as CBM,isnull(I.RawMaterialPile,'') as RawMaterialPile,isnull(I.RawMaterialBase,'') as RawMaterialBase
                ,IsNull(IsNull(I.PaymentTermCustom, CI.PaymentIdCustom), 0) as PaymentIdCustom,isnull(I.SUQty,'') as SUQty,isnull(I.PreferentialAgreement,'') as PreferentialAgreement,
                isnull(I.PackingCharges,'') as PackingCharges,isnull(I.LessAdvance,0) as LessAdvance,isnull(I.ExtraCharges,0) as ExtraCharges,isnull(I.Ex1Rate,0) as Ex1Rate,
                isnull(I.Ex2Rate,0) as Ex2Rate,isnull(I.Composition,'') as Composition,isnull(I.Freight,0) as Freight
                From Packing P,CustomerInfo CI ,Invoice I 
                Where P.ConsigneeId=CI.CustomerID  And P.PackingId=I.PackingId
                And P.PackingID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }

        Ds = null;
        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["CurrencyId"].ToString().Trim();
            }
            DDShippingAgent.SelectedValue = Ds.Tables[0].Rows[0]["ShippingAgent"].ToString();
            DDPreCarriage.SelectedValue = Ds.Tables[0].Rows[0]["PreCarriageBy"].ToString();
            DDReceiptAt.SelectedValue = Ds.Tables[0].Rows[0]["RecieptAtByPreCarrier"].ToString();
            DDByAirSea.SelectedValue = Ds.Tables[0].Rows[0]["ByAirSea"].ToString();
            DDPortLoad.SelectedValue = Ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
            TxtRollMarkHead.Text = Ds.Tables[0].Rows[0]["Mark"].ToString();
            TxtFinalDestination.Text = Ds.Tables[0].Rows[0]["DestinationPlace"].ToString();
            DDDelivery.SelectedValue = Ds.Tables[0].Rows[0]["PaymentId"].ToString();
            DDTerms.SelectedValue = Ds.Tables[0].Rows[0]["TermId"].ToString();
            DDBank.SelectedValue = Ds.Tables[0].Rows[0]["BankId"].ToString();
            TxtPortDisCharge.Text = Ds.Tables[0].Rows[0]["PortUnload"].ToString();
            TxtAddInsurance.Text = Ds.Tables[0].Rows[0]["insurance"].ToString();
            TxtDiscount.Text = Ds.Tables[0].Rows[0]["discountamt"].ToString();
            TxtGrossWeight.Text = Ds.Tables[0].Rows[0]["GrossWt"].ToString();
            TxtNetWeight.Text = Ds.Tables[0].Rows[0]["NetWt"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["Invoicedate"].ToString();
            txtCgst.Text = Ds.Tables[0].Rows[0]["CGST"].ToString();
            txtSgst.Text = Ds.Tables[0].Rows[0]["SGST"].ToString();
            txtIgst.Text = Ds.Tables[0].Rows[0]["IGST"].ToString();
            txtInrRate.Text = Ds.Tables[0].Rows[0]["INRRate"].ToString();
            TxtDescriptionofGood.Text = Ds.Tables[0].Rows[0]["DescriptionOfGoods"].ToString();
            txtcbm.Text = Ds.Tables[0].Rows[0]["CBM"].ToString();
            DDPaymentTermCustom.SelectedValue = Ds.Tables[0].Rows[0]["PaymentIdCustom"].ToString();
            txtComposition.Text = Ds.Tables[0].Rows[0]["Composition"].ToString();
            txtgstcess.Text = Ds.Tables[0].Rows[0]["Ex2Rate"].ToString();
            if (ChkInvoice.Checked == true)
            {
                txtShipToAddress.Text = Ds.Tables[0].Rows[0]["ShipToAddress"].ToString();
                txtDiscountRemark.Text = Ds.Tables[0].Rows[0]["DiscountRemark"].ToString();
                txtGSTINType.Text = Ds.Tables[0].Rows[0]["GstinType"].ToString();
                txtEndUse.Text = Ds.Tables[0].Rows[0]["EndUse"].ToString();
                if (Session["varMasterCompanyIDForERP"].ToString() == "20")
                {
                    txtRawMaterialPile.Text = Ds.Tables[0].Rows[0]["RawMaterialPile"].ToString();
                    txtRawMaterialBase.Text = Ds.Tables[0].Rows[0]["RawMaterialBase"].ToString();
                }

            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "20")
            {
                txtCountryOfFinalDestination.Text = Ds.Tables[0].Rows[0]["CountryFinalDestination"].ToString();
                TxtPortDisCharge.Text = Ds.Tables[0].Rows[0]["PortOfDischarge"].ToString();
                TxtFinalDestination.Text = Ds.Tables[0].Rows[0]["FinalDestination"].ToString();
                txtPlaceOfDelivery.Text = Ds.Tables[0].Rows[0]["PlaceOfDelivery"].ToString();
                txtWareHouseName.Text = Ds.Tables[0].Rows[0]["WareHouseName"].ToString();
                if (Ds.Tables[0].Rows[0]["ExportAgainstIGST"].ToString() == "1")
                {
                    ChkExportIGST.Checked = true;
                }
                else
                {
                    ChkExportIGST.Checked = false;
                }
                if (Ds.Tables[0].Rows[0]["ExportAgainstLUT"].ToString() == "1")
                {
                    ChkExportLUT.Checked = true;
                }
                else
                {
                    ChkExportLUT.Checked = false;

                }
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                TxtInvoiceAmt.Text = Ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                txtSUQty.Text = Ds.Tables[0].Rows[0]["SUQty"].ToString();
                txtDeupa.Text = Ds.Tables[0].Rows[0]["PreferentialAgreement"].ToString();
                txtPackingCharges.Text = Ds.Tables[0].Rows[0]["PackingCharges"].ToString();
                TxtPreAdvance.Text = Ds.Tables[0].Rows[0]["LessAdvance"].ToString();
                TxtExtraChargeAmt.Text = Ds.Tables[0].Rows[0]["ExtraCharges"].ToString();
                TxtAddFrieght.Text = Ds.Tables[0].Rows[0]["Freight"].ToString();
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "36")
            {
                TxtExtraChargeAmt.Text = Ds.Tables[0].Rows[0]["ExtraCharges"].ToString();
                TxtAddFrieght.Text = Ds.Tables[0].Rows[0]["Freight"].ToString();
            }
        }
        //Get Gross Wt Net wt 
        if (MySession.InvoiceReportType == "1")
        {
            if (ChkInvoice.Checked == false)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() != "20")
                {
                    //Str = @"select Sum(NetWt) as Netwt,Sum(GrossWt) as Grosswt,sum(CBM) as CBM from 
                    //PackingInformation  Where PackingId=" + DDInvoiveNo.SelectedValue;

                    Str = @"select isnull(Sum(SinglePcsnetWt*TotalRoll),0) as Netwt,isnull(Sum(SinglePcsGrossWt*TotalRoll),0) as Grosswt,isnull(sum(CBM*TotalRoll),0) as CBM from 
                  PackingInformation  Where PackingId=" + DDInvoiveNo.SelectedValue;
                    Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TxtNetWeight.Text = Ds.Tables[0].Rows[0]["Netwt"].ToString();
                        TxtGrossWeight.Text = Ds.Tables[0].Rows[0]["Grosswt"].ToString();
                        txtcbm.Text = Ds.Tables[0].Rows[0]["CBM"].ToString();
                    }
                }
            }

        }
        //
    }
    protected void RDUnToOrder_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(1);
        //ChangeAccToRadioButton();
        RadioButtonselection();
    }
    protected void RDCustomer_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(2);
        //ChangeAccToRadioButton();
        RadioButtonselection();
    }
    protected void RDBank_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(3);
        //ChangeAccToRadioButton();
        RadioButtonselection();
    }

    protected void RadioButtonselection()
    {
        string Str = "";
        string VarConsignoree = "", varConsignor = "";
        string VarBank = "";
        string VarShipToAddress = "";
        DataSet Ds;
        TxtConsignor.Text = "";
        TxtConsignee.Text = "";
        TxtBuyerOtherThanConsignee.Text = "";
        if (DDBuyerOtherThanConsignee.Items.Count > 0)
        {
            DDBuyerOtherThanConsignee.SelectedIndex = 0;
        }

        Str = @"Select CI.CompanyName CICompanyName,CI.CompAddr1 CICompAddr1,CI.CompAddr2 CICompAddr2,CI.CompAddr3 CICompAddr3,CI.CompFax CICompFax,CI.CompTel CICompTel,
                CI.Email CIEmail,CI.TinNo CITinNo,CUI.CustomerName CUICustomerName,CUI.CompanyName CUICompanyName,CUI.Address CUIAddress,CUI.BuyerOtherThanConsignee,
                CUI.PhoneNo CUIPhoneNo,CUI.Mobile CUIMobile,CUI.Email CUIEmail,B.BankName BBankName,B.Street BStreet,B.City BCity,B.State BState,B.Country BCountry,
                B.PhoneNo BPhoneNo,B.Faxno BFaxno,B.Email BEmail,I.TConsignor,I.TConsignee,I.TBuyerOConsignee,isnull(I.TypeofBuyerOtherConsignee,0) as TypeofBuyerOtherConsignee ,
                isnull(I.ShipToAddress,'') as ShipToAddress,isnull(WHConsignee,'') as WHConsignee,isnull(WHM.WHBuyerOtherConsignee,'') as WHBuyerOtherConsignee,isnull(WHM.WHShipTo,'') as WHShipTo,
                isnull(WHM.Warehousecode,'') as WarehouseCode, isnull(WHM.Warehousename,'') as WareHouseName,isnull(WHM.Address,'') as WareHouseAddress,isnull(WHM.City,'') as WareHouseCity,
                isnull(WHM.State,'') as WareHouseState,isnull(WHM.Country,'') as WareHouseCountry 
                from Invoice I JOIN CompanyInfo CI ON I.ConsignorId=CI.CompanyId 
				JOIN CustomerInfo CUI ON I.CosigneeId=CUI.CustomerId
				JOIN Bank B ON B.BankId=CUI.BankId
                LEFT JOIN CountryMaster CM ON CUI.Country=CM.countryid
				JOIN Packing P ON I.packingID=P.PackingID";

        if (Session["VarCompanyNo"].ToString() == "20")
        {
            Str = Str + " LEFT JOIN WareHouseMaster WHM ON I.CosigneeId=WHM.CustomerId and P.WareHouseID=WHM.WareHouseID Where I.InvoiceID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        }
        else
        {
            Str = Str + " LEFT JOIN WareHouseMaster WHM ON I.CosigneeId=WHM.CustomerId Where I.InvoiceID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        }

        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (ChkInvoice.Checked == true)
            {
                TxtConsignor.Text = Ds.Tables[0].Rows[0]["TConsignor"].ToString();
                VarConsignoree = Ds.Tables[0].Rows[0]["TBuyerOConsignee"].ToString();
                TxtBuyerOtherThanConsignee.Text = VarConsignoree;
                TxtConsignee.Text = Ds.Tables[0].Rows[0]["TConsignee"].ToString();
                txtShipToAddress.Text = Ds.Tables[0].Rows[0]["ShipToAddress"].ToString();
            }
            else
            {
                //TxtConsignor.Text = Ds.Tables[0].Rows[0]["CICompanyName"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr1"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr2"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr3"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompTel"].ToString() + "," + Ds.Tables[0].Rows[0]["CIEmail"].ToString() + "," + Ds.Tables[0].Rows[0]["CITinNo"].ToString();
                varConsignor = Ds.Tables[0].Rows[0]["CICompanyName"].ToString();
                if (Ds.Tables[0].Rows[0]["CICompAddr1"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CICompAddr1"].ToString(); }
                if (Ds.Tables[0].Rows[0]["CICompAddr2"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CICompAddr2"].ToString(); }
                if (Ds.Tables[0].Rows[0]["CICompAddr3"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CICompAddr3"].ToString(); }
                if (Ds.Tables[0].Rows[0]["CICompTel"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CICompTel"].ToString(); }
                if (Ds.Tables[0].Rows[0]["CIEmail"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CIEmail"].ToString(); }
                if (Ds.Tables[0].Rows[0]["CITinNo"].ToString() != "")
                { varConsignor = varConsignor + "," + Ds.Tables[0].Rows[0]["CITinNo"].ToString(); }
                TxtConsignor.Text = varConsignor.TrimStart(',');
                // VarConsignoree = Ds.Tables[0].Rows[0]["CUICompanyName"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIAddress"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIPhoneNo"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIMobile"].ToString();

                if (Ds.Tables[0].Rows[0]["WHConsignee"].ToString() != "")
                {
                    VarConsignoree = Ds.Tables[0].Rows[0]["WHConsignee"].ToString();
                }
                else
                {

                    VarConsignoree = Ds.Tables[0].Rows[0]["CUICompanyName"].ToString();
                    if (Ds.Tables[0].Rows[0]["CUIAddress"].ToString() != "")
                    { VarConsignoree = VarConsignoree + "," + Ds.Tables[0].Rows[0]["CUIAddress"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["CUIPhoneNo"].ToString() != "")
                    { VarConsignoree = VarConsignoree + "," + Ds.Tables[0].Rows[0]["CUIPhoneNo"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["CUIMobile"].ToString() != "")
                    { VarConsignoree = VarConsignoree + "," + Ds.Tables[0].Rows[0]["CUIMobile"].ToString(); }


                }
                if (Ds.Tables[0].Rows[0]["WHBuyerOtherConsignee"].ToString() != "")
                {
                    TxtBuyerOtherThanConsignee.Text = Ds.Tables[0].Rows[0]["WHBuyerOtherConsignee"].ToString();
                }
                else
                {
                    TxtBuyerOtherThanConsignee.Text = Ds.Tables[0].Rows[0]["BuyerOtherThanConsignee"].ToString();
                }
                //******VarShipToAddress
                if (Ds.Tables[0].Rows[0]["WHShipTo"].ToString() != "")
                {
                    VarShipToAddress = Ds.Tables[0].Rows[0]["WHShipTo"].ToString();
                }
                else
                {
                    if (Ds.Tables[0].Rows[0]["WarehouseCode"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WarehouseCode"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["WareHouseName"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WareHouseName"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["WareHouseAddress"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WareHouseAddress"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["WareHouseCity"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WareHouseCity"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["WareHouseState"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WareHouseState"].ToString(); }
                    if (Ds.Tables[0].Rows[0]["WareHouseCountry"].ToString() != "")
                    { VarShipToAddress = VarShipToAddress + "," + Ds.Tables[0].Rows[0]["WareHouseCountry"].ToString(); }
                    VarShipToAddress = VarShipToAddress.TrimStart(',');
                    //******

                }
                txtShipToAddress.Text = VarShipToAddress;
            }
            if (RDCustomer.Checked == true)
            {
                if (ChkInvoice.Checked == false)
                {
                    TxtConsignee.Text = VarConsignoree;
                }
            }
            else if (RDBank.Checked == true)
            {
                TxtConsignee.Text = VarBank;
                if (RDUnToOrder.Checked == true && ChkInvoice.Checked == false)
                {

                    TxtBuyerOtherThanConsignee.Text = VarConsignoree;
                    TxtConsignee.Text = "UNTO ORDER";
                }
            }
            else if (RDUnToOrder.Checked == true && ChkInvoice.Checked == false)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "20")
                {
                    TxtConsignee.Text = VarConsignoree;
                }
                else
                {
                    TxtBuyerOtherThanConsignee.Text = VarConsignoree;
                    TxtConsignee.Text = "UNTO ORDER";
                }

            }

        }
    }
    private void RadioButtonSelectedChange(int I)
    {
        switch (I)
        {
            case 1:
                RDUnToOrder.Checked = true;
                RDCustomer.Checked = false;
                RDBank.Checked = false;
                UtilityModule.ConditionalComboFill(ref DDBuyerOtherThanConsignee, "Select CustomerId,CompanyName from CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
                break;
            case 2:
                RDCustomer.Checked = true;
                RDUnToOrder.Checked = false;
                RDBank.Checked = false;
                UtilityModule.ConditionalComboFill(ref DDBuyerOtherThanConsignee, "Select CustomerId,CompanyName from CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
                break;
            case 3:
                RDBank.Checked = true;
                RDUnToOrder.Checked = false;
                RDCustomer.Checked = false;
                UtilityModule.ConditionalComboFill(ref DDBuyerOtherThanConsignee, "Select BankId,BankName from Bank Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BankName", true, "--Select--");
                break;
        }
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Validation_Check();
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrPara = new SqlParameter[55];
                _arrPara[0] = new SqlParameter("@InvoiceID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@TypeOfBuyerOtherConsignee", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@TConsignor", SqlDbType.NVarChar, 500);
                _arrPara[3] = new SqlParameter("@TConsignee", SqlDbType.NVarChar, 500);
                _arrPara[4] = new SqlParameter("@TBuyerOConsignee", SqlDbType.NVarChar, 500);
                _arrPara[5] = new SqlParameter("@CurrencyType", SqlDbType.NVarChar, 10);
                _arrPara[6] = new SqlParameter("@InvoiceDate", SqlDbType.SmallDateTime);
                _arrPara[7] = new SqlParameter("@Agentid", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@PreCarrier", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@Receipt", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@ShipingId", SqlDbType.Int);
                _arrPara[11] = new SqlParameter("@PortLoad", SqlDbType.Int);
                _arrPara[12] = new SqlParameter("@PortUnload", SqlDbType.NVarChar, 50);
                _arrPara[13] = new SqlParameter("@RollMark", SqlDbType.NVarChar, 50);
                _arrPara[14] = new SqlParameter("@RollMarkHead", SqlDbType.NVarChar, 100);
                _arrPara[15] = new SqlParameter("@DestinationAdd", SqlDbType.NVarChar, 50);
                _arrPara[16] = new SqlParameter("@GrossWt", SqlDbType.Float);
                _arrPara[17] = new SqlParameter("@NetWt", SqlDbType.Float);
                _arrPara[18] = new SqlParameter("@LessAdvance", SqlDbType.Float);
                _arrPara[19] = new SqlParameter("@Insurance", SqlDbType.Float);
                _arrPara[20] = new SqlParameter("@Freight", SqlDbType.Float);
                _arrPara[21] = new SqlParameter("@InvoiceAmount", SqlDbType.Float);
                _arrPara[22] = new SqlParameter("@ExtraCharges", SqlDbType.Float);
                _arrPara[23] = new SqlParameter("@ExtraChargeRemarks", SqlDbType.NVarChar, 100);
                _arrPara[24] = new SqlParameter("@DelTerms", SqlDbType.Int);
                _arrPara[25] = new SqlParameter("@CreditId", SqlDbType.Int);
                _arrPara[26] = new SqlParameter("@terms", SqlDbType.Int);
                _arrPara[27] = new SqlParameter("@frmbldate", SqlDbType.Int);
                _arrPara[28] = new SqlParameter("@frmbldateafter", SqlDbType.Int);
                _arrPara[29] = new SqlParameter("@FreightTerms", SqlDbType.NVarChar, 100);
                _arrPara[30] = new SqlParameter("@DiscountAmt", SqlDbType.Float);
                _arrPara[31] = new SqlParameter("@BankId", SqlDbType.Int);
                _arrPara[32] = new SqlParameter("@CBM", SqlDbType.Float);

                _arrPara[33] = new SqlParameter("@CGST", SqlDbType.Float);
                _arrPara[34] = new SqlParameter("@SGST", SqlDbType.Float);
                _arrPara[35] = new SqlParameter("@IGST", SqlDbType.Float);
                _arrPara[36] = new SqlParameter("@INRRate", SqlDbType.Float);
                _arrPara[37] = new SqlParameter("@descriptionofgoods", SqlDbType.VarChar, 700);
                _arrPara[38] = new SqlParameter("@ShipToAddress", SqlDbType.VarChar, 500);
                _arrPara[39] = new SqlParameter("@DiscountRemark", SqlDbType.VarChar, 250);
                _arrPara[40] = new SqlParameter("@GstinType", SqlDbType.VarChar, 50);
                _arrPara[41] = new SqlParameter("@EndUse", SqlDbType.VarChar, 50);
                _arrPara[42] = new SqlParameter("@RawMaterialPile", SqlDbType.VarChar, 250);
                _arrPara[43] = new SqlParameter("@RawMaterialBase", SqlDbType.VarChar, 250);
                _arrPara[44] = new SqlParameter("@PaymentTermCustom", SqlDbType.Int);
                _arrPara[45] = new SqlParameter("@CountryOfFinalDestination", SqlDbType.VarChar, 30);
                _arrPara[46] = new SqlParameter("@PlaceOfDelivery", SqlDbType.VarChar, 30);
                _arrPara[47] = new SqlParameter("@WareHouseName", SqlDbType.VarChar, 200);
                _arrPara[48] = new SqlParameter("@ExportAgainstIGST", SqlDbType.Int);
                _arrPara[49] = new SqlParameter("@ExportAgainstLUT", SqlDbType.Int);
                _arrPara[50] = new SqlParameter("@SUQty", SqlDbType.VarChar, 20);
                _arrPara[51] = new SqlParameter("@PreferentialAgreement", SqlDbType.VarChar, 20);
                _arrPara[52] = new SqlParameter("@PackingCharges", SqlDbType.Float);
                _arrPara[53] = new SqlParameter("@Composition", SqlDbType.VarChar, 500);
                _arrPara[54] = new SqlParameter("@gstcess", SqlDbType.Float);

                _arrPara[0].Value = DDInvoiveNo.SelectedValue;
                _arrPara[1].Value = RDBank.Checked == true ? 3 : RDCustomer.Checked == true ? 2 : 1;
                _arrPara[2].Value = TxtConsignor.Text.ToUpper();
                _arrPara[3].Value = TxtConsignee.Text.ToUpper();
                _arrPara[4].Value = TxtBuyerOtherThanConsignee.Text.ToUpper();
                _arrPara[5].Value = DDCurrency.Text.ToUpper();
                _arrPara[6].Value = TxtDate.Text;
                _arrPara[7].Value = DDShippingAgent.SelectedValue;
                _arrPara[8].Value = DDPreCarriage.SelectedValue;
                _arrPara[9].Value = DDReceiptAt.SelectedValue;
                _arrPara[10].Value = DDByAirSea.SelectedValue;
                _arrPara[11].Value = DDPortLoad.SelectedValue;
                _arrPara[12].Value = TxtPortDisCharge.Text;
                _arrPara[13].Value = TxtRollMarkHead.Text.ToUpper();
                _arrPara[14].Value = TxtRollMarkHead.Text.ToUpper();
                _arrPara[15].Value = TxtFinalDestination.Text;
                _arrPara[16].Value = TxtGrossWeight.Text == "" ? "0" : TxtGrossWeight.Text;
                _arrPara[17].Value = TxtNetWeight.Text == "" ? "0" : TxtNetWeight.Text;
                _arrPara[18].Value = TxtPreAdvance.Text == "" ? "0" : TxtPreAdvance.Text;
                _arrPara[19].Value = TxtAddInsurance.Text == "" ? "0" : TxtAddInsurance.Text;
                _arrPara[20].Value = TxtAddFrieght.Text == "" ? "0" : TxtAddFrieght.Text;
                _arrPara[21].Value = TxtInvoiceAmt.Text == "" ? "0" : TxtInvoiceAmt.Text;
                _arrPara[22].Value = TxtExtraChargeAmt.Text == "" ? "0" : TxtExtraChargeAmt.Text;
                _arrPara[23].Value = TxtExtraChargeRemark.Text;
                _arrPara[24].Value = DDDelivery.SelectedValue;
                _arrPara[25].Value = DDTerms.SelectedIndex == 0 ? "0" : DDTerms.SelectedValue;
                _arrPara[26].Value = DDTerms.SelectedValue;
                _arrPara[27].Value = ChkBilling.Checked == true ? 1 : 0;
                _arrPara[28].Value = ChkShipmentDays.Checked == true ? 1 : 0;
                _arrPara[29].Value = TxtFreightTerms.Text == "" ? "" : TxtFreightTerms.Text;
                _arrPara[30].Value = TxtDiscount.Text == "" ? "0" : TxtDiscount.Text;
                _arrPara[31].Value = DDBank.SelectedValue;
                _arrPara[32].Value = txtcbm.Text == "" ? "0" : txtcbm.Text;
                _arrPara[33].Value = txtCgst.Text == "" ? "0" : txtCgst.Text;
                _arrPara[34].Value = txtSgst.Text == "" ? "0" : txtSgst.Text;
                _arrPara[35].Value = txtIgst.Text == "" ? "0" : txtIgst.Text;
                _arrPara[36].Value = txtInrRate.Text == "" ? "0" : txtInrRate.Text;
                _arrPara[37].Value = TxtDescriptionofGood.Text;
                _arrPara[38].Value = txtShipToAddress.Text;
                _arrPara[39].Value = txtDiscountRemark.Text;
                _arrPara[40].Value = txtGSTINType.Text;
                _arrPara[41].Value = txtEndUse.Text;
                _arrPara[42].Value = TRPileBase.Visible == true ? txtRawMaterialPile.Text : "";
                _arrPara[43].Value = TRPileBase.Visible == true ? txtRawMaterialBase.Text : "";
                _arrPara[44].Value = TRPileBase.Visible == true ? DDPaymentTermCustom.SelectedIndex == 0 ? "0" : DDPaymentTermCustom.SelectedValue : "0";
                _arrPara[45].Value = TRPileBase.Visible == true ? txtCountryOfFinalDestination.Text : "";
                _arrPara[46].Value = TRPileBase.Visible == true ? txtPlaceOfDelivery.Text : "";
                _arrPara[47].Value = TRPileBase.Visible == true ? txtWareHouseName.Text : "";
                _arrPara[48].Value = TDExportIGST.Visible == true ? ChkExportIGST.Checked == true ? 1 : 0 : 0;
                _arrPara[49].Value = TDExportIGST.Visible == true ? ChkExportLUT.Checked == true ? 1 : 0 : 0;
                _arrPara[50].Value = TRPackingCharges.Visible == true ? txtSUQty.Text : "";
                _arrPara[51].Value = TRPackingCharges.Visible == true ? txtDeupa.Text : "";
                _arrPara[52].Value = TRPackingCharges.Visible == true ? txtPackingCharges.Text : "0";
                _arrPara[53].Value = txtComposition.Text;
                _arrPara[54].Value = txtgstcess.Text == "" ? "0" : txtgstcess.Text;

                //For Currency  Conversionrate;
                Double ConversionRate = 0;

                ConversionRate = Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(CurrentRateRefRs,0) from currencyinfo Where CurrencyId=" + DDCurrency.SelectedValue + ""));

                string Str = "Update Invoice Set TypeOfBuyerOtherConsignee=" + _arrPara[1].Value + ",TConsignor='" + _arrPara[2].Value + "',TConsignee='" + _arrPara[3].Value + @"',
                            TBuyerOConsignee=N'" + _arrPara[4].Value + "',CurrencyType='" + _arrPara[5].Value + "',InvoiceDate='" + _arrPara[6].Value + "',Agentid=" + _arrPara[7].Value + @",
                            PreCarrier=" + _arrPara[8].Value + ",Receipt=" + _arrPara[9].Value + ",ShipingId=" + _arrPara[10].Value + ",PortLoad=" + _arrPara[11].Value + @",
                            PortUnload='" + _arrPara[12].Value + "',RollMark='" + _arrPara[13].Value + "',RollMarkHead='" + _arrPara[14].Value + @"',
                            DestinationAdd='" + _arrPara[15].Value + "',GrossWt=" + _arrPara[16].Value + ",NetWt=" + _arrPara[17].Value + @",
                            LessAdvance=" + _arrPara[18].Value + ",Insurance=" + _arrPara[19].Value + ",Freight=" + _arrPara[20].Value + @",
                            InvoiceAmount=" + _arrPara[21].Value + ",ExtraCharges=" + _arrPara[22].Value + ",ExtraChargeRemarks='" + _arrPara[23].Value + @"',
                            DelTerms=" + _arrPara[24].Value + ",CreditId=" + _arrPara[25].Value + ",terms=" + _arrPara[26].Value + ",frmbldate=" + _arrPara[27].Value + @",
                            frmbldateafter=" + _arrPara[28].Value + ",FreightTerms='" + _arrPara[29].Value + "',DiscountAmt=" + _arrPara[30].Value + @",
                            BankId=" + _arrPara[31].Value + ",Status=1,CBM=" + _arrPara[32].Value + ",UnitRate=" + ConversionRate + ",CGST=" + _arrPara[33].Value + @",
                            SGST=" + _arrPara[34].Value + ",IGST=" + _arrPara[35].Value + ",INRRate=" + _arrPara[36].Value + " ,descriptionofgoods='" + _arrPara[37].Value + @"' 
                            ,ShipToAddress='" + _arrPara[38].Value + "' ,DiscountRemark='" + _arrPara[39].Value + "' ,GstinType='" + _arrPara[40].Value + "',EndUse='" + _arrPara[41].Value + @"'
                            ,RawMaterialPile='" + _arrPara[42].Value + "',RawMaterialBase='" + _arrPara[43].Value + "',PaymentTermCustom='" + _arrPara[44].Value + @"'
                            ,CountryOfFinalDestination='" + _arrPara[45].Value + "',PlaceOfDelivery='" + _arrPara[46].Value + "',WareHouseName='" + _arrPara[47].Value + @"'
                            ,ExportAgainstIGST=" + _arrPara[48].Value + @",ExportAgainstLUT=" + _arrPara[49].Value + @",SUQty='" + _arrPara[50].Value + @"',PreferentialAgreement='" + _arrPara[51].Value + @"'
                            ,PackingCharges=" + _arrPara[52].Value + @",Composition='" + _arrPara[53].Value + @"',Ex2Rate = " + _arrPara[54].Value + @"
                            Where InvoiceId=" + DDInvoiveNo.SelectedValue;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                Tran.Commit();
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Invoice'," + DDInvoiveNo.SelectedValue + ",getdate(),'Update')");
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Invoice Saved...";
                save_refresh();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoice.aspx");
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
    private void save_refresh()
    {
        if (DDShippingAgent.Items.Count > 0)
        {
            DDShippingAgent.SelectedIndex = 0;
        }
        TxtDays.Text = "";
        TxtFreightTerms.Text = "";
        TxtRollMarkHead.Text = "";
        TxtRollNo.Text = "";
        TxtFinalDestination.Text = "";
        TxtDiscount.Text = "";
        TxtGrossWeight.Text = "";
        TxtNetWeight.Text = "";
        TxtPreAdvance.Text = "";
        TxtAdvanceRec.Text = "";
        TxtAddInsurance.Text = "";
        TxtAddFrieght.Text = "";
        TxtInvoiceAmt.Text = "";
        TxtExtraChargeAmt.Text = "";
        TxtExtraChargeRemark.Text = "";
        TxtDescriptionofGood.Text = "";
        txtcbm.Text = "";
        txtCgst.Text = "";
        txtIgst.Text = "";
        txtSgst.Text = "";
        txtInrRate.Text = "";
        txtSUQty.Text = "";
        txtDeupa.Text = "";
        txtPackingCharges.Text = "";

    }
    private void Validation_Check()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDInvoiveNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtConsignor) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDShippingAgent) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDPreCarriage) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDReceiptAt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDByAirSea) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDPortLoad) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDelivery) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDBank) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPortDisCharge) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRollMarkHead) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFinalDestination) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblErrorMessage);
    B:;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDBuyerOtherThanConsignee_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet Ds;
        if (RDCustomer.Checked == true)
        {
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CU.CompanyName,CU.Address,CU.Country,CU.PinCode,CU.PhoneNo,CU.Mobile,CU.Fax,CU.Email From CustomerInfo Cu Where CustomerID=" + DDBuyerOtherThanConsignee.SelectedValue + " And CU.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtBuyerOtherThanConsignee.Text = Ds.Tables[0].Rows[0]["CompanyName"].ToString() + "," + Ds.Tables[0].Rows[0]["Address"].ToString() + "," + Ds.Tables[0].Rows[0]["Country"].ToString() + "," + Ds.Tables[0].Rows[0]["PhoneNo"].ToString() + "," + Ds.Tables[0].Rows[0]["Mobile"].ToString() + "," + Ds.Tables[0].Rows[0]["Email"].ToString();
            }
        }
        else if (RDBank.Checked == true)
        {
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select BankName,Street,City,State,Country,PhoneNo,Faxno,Email From Bank Where Bankid=" + DDBuyerOtherThanConsignee.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtBuyerOtherThanConsignee.Text = TxtConsignee.Text;
                TxtConsignee.Text = Ds.Tables[0].Rows[0]["BankName"].ToString() + "," + Ds.Tables[0].Rows[0]["Street"].ToString() + "," + Ds.Tables[0].Rows[0]["City"].ToString() + "," + Ds.Tables[0].Rows[0]["State"].ToString() + "," + Ds.Tables[0].Rows[0]["Country"].ToString() + "," + Ds.Tables[0].Rows[0]["PhoneNo"].ToString() + "," + Ds.Tables[0].Rows[0]["FaxNo"].ToString() + "," + Ds.Tables[0].Rows[0]["Email"].ToString();
            }
        }
    }
    protected void ChkInvoice_CheckedChanged(object sender, EventArgs e)
    {
        string str = @"select I.Invoiceid,I.TInvoiceNo From Invoice I,Packing P Where I.InvoiceId=P.PackingId And I.InvoiceType<>3 And 
                        I.ConsignorID = " + Session["CurrentWorkingCompanyID"] + @" And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ChkInvoice.Checked == true)
        {
            str = str + " And I.Status = 1";
        }
        else
        {
            str = str + " And I.Status = 0";
        }
        str = str + " Order By I.TinvoiceNo desc";
        UtilityModule.ConditionalComboFill(ref DDInvoiveNo, str, true, "--Select--");
    }
    protected void DDGoodsDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Descriptionofgoods = "";
        Descriptionofgoods = DDGoodsDescription.SelectedItem.Text;
        TxtDescriptionofGood.Text = TxtDescriptionofGood.Text + ',' + Descriptionofgoods;
    }
    protected void InvoiceDetailReport()
    {
        LblErrorMessage.Text = "";
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetInvoiceDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@InvoiceId", DDInvoiveNo.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
                }
                string Path = "";

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("InvoiceDetail");
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                //sht.PageSetup.AdjustTo(90);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                //sht.PageSetup.VerticalDpi = 300;
                //sht.PageSetup.HorizontalDpi = 300;
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Bottom = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Header = 0.2;
                sht.PageSetup.Margins.Footer = 0.2;
                //sht.Style.Font.FontName = "Cambria";
                sht.PageSetup.SetScaleHFWithDocument();
                sht.PageSetup.CenterHorizontally = true;

                sht.Column("A").Width = 19.67;
                sht.Column("B").Width = 20.67;
                sht.Column("C").Width = 25.33;
                sht.Column("D").Width = 16.76;
                sht.Column("E").Width = 15.89;
                sht.Column("F").Width = 15.22;
                //sht.Column("G").Width = 17.67;

                sht.Row(2).Height = 20;
                sht.Row(3).Height = 34;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ////sht.Cell("A3").Value = "INVOICE #" + ds.Tables[0].Rows[0]["TINVOICENO"] + " DATED " + ds.Tables[0].Rows[0]["INVOICEDATE"];
                    sht.Cell("A1").Value = "INVOICE REPORT";
                    sht.Range("A1:F1").Style.Font.FontName = "Calibri";
                    sht.Range("A1:F1").Style.Font.FontSize = 15;
                    sht.Range("A1:F1").Style.Font.Bold = true;
                    sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:F1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1:F1").Merge();

                    ////sht.Cell("A3").Value = "INVOICE #" + ds.Tables[0].Rows[0]["TINVOICENO"] + " DATED " + ds.Tables[0].Rows[0]["INVOICEDATE"];
                    sht.Cell("A2").Value = "GSTIN NO" + " " + ds.Tables[0].Rows[0]["GSTNo"].ToString().ToUpper();
                    sht.Range("A2:B2").Style.Font.FontName = "Calibri";
                    sht.Range("A2:B2").Style.Font.FontSize = 12;
                    sht.Range("A2:B2").Style.Font.Bold = true;
                    sht.Range("A2:B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A2:B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A2:B2").Merge();

                    sht.Cell("C2").Value = "INVOICE";
                    sht.Range("C2:D2").Style.Font.FontName = "Calibri";
                    sht.Range("C2:D2").Style.Font.FontSize = 12;
                    sht.Range("C2:D2").Style.Font.Bold = true;
                    sht.Range("C2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C2:D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C2:D2").Merge();

                    sht.Cell("E2").Value = "MOBILE NO:" + " " + ds.Tables[0].Rows[0]["CompTel"].ToString().ToUpper(); ;
                    sht.Range("E2:F2").Style.Font.FontName = "Calibri";
                    sht.Range("E2:F2").Style.Font.FontSize = 12;
                    sht.Range("E2:F2").Style.Font.Bold = true;
                    sht.Range("E2:F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("E2:F2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E2:F2").Merge();

                    sht.Cell("A3").Value = "";
                    sht.Range("A3:B3").Style.Font.FontName = "Calibri";
                    sht.Range("A3:B3").Style.Font.FontSize = 12;
                    sht.Range("A3:B3").Style.Font.Bold = true;
                    sht.Range("A3:B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A3:B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A3:B3").Merge();

                    sht.Cell("C3").Value = "";
                    sht.Range("C3:C3").Style.Font.FontName = "Calibri";
                    sht.Range("C3:C3").Style.Font.FontSize = 12;
                    sht.Range("C3:C3").Style.Font.Bold = true;
                    sht.Range("C3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C3:C3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C3:C3").Merge();

                    sht.Cell("D3").Value = ds.Tables[0].Rows[0]["COMPANYNAME"].ToString().ToUpper();
                    sht.Range("D3:F3").Style.Font.FontName = "Calibri";
                    sht.Range("D3:F3").Style.Font.FontSize = 22;
                    sht.Range("D3:F3").Style.Font.Bold = true;
                    sht.Range("D3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D3:F3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D3:F3").Merge();

                    sht.Cell("A4").Value = "";
                    sht.Range("A4:B4").Style.Font.FontName = "Calibri";
                    sht.Range("A4:B4").Style.Font.FontSize = 12;
                    sht.Range("A4:B4").Style.Font.Bold = true;
                    sht.Range("A4:B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:B4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A4:B4").Merge();

                    sht.Cell("C4").Value = "";
                    sht.Range("C4:C4").Style.Font.FontName = "Calibri";
                    sht.Range("C4:C4").Style.Font.FontSize = 12;
                    sht.Range("C4:C4").Style.Font.Bold = true;
                    sht.Range("C4:C4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C4:C4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C4:C4").Merge();

                    sht.Cell("D4").Value = "Manufacturers & Suppliers: HANDLOOM DHURRIES";
                    sht.Range("D4:F4").Style.Font.FontName = "Calibri";
                    sht.Range("D4:F4").Style.Font.FontSize = 11;
                    sht.Range("D4:F4").Style.Font.Bold = true;
                    sht.Range("D4:F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D4:F4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D4:F4").Merge();

                    sht.Cell("A5").Value = "";
                    sht.Range("A5:B5").Style.Font.FontName = "Calibri";
                    sht.Range("A5:B5").Style.Font.FontSize = 11;
                    sht.Range("A5:B5").Style.Font.Bold = true;
                    sht.Range("A5:B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A5:B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A5:B5").Merge();

                    sht.Cell("C5").Value = "";
                    sht.Range("C5:C5").Style.Font.FontName = "Calibri";
                    sht.Range("C5:C5").Style.Font.FontSize = 11;
                    sht.Range("C5:C5").Style.Font.Bold = true;
                    sht.Range("C5:C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C5:C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C5:C5").Merge();

                    sht.Cell("D5").Value = "Address:" + " " + ds.Tables[0].Rows[0]["CompAddr1"].ToString();
                    sht.Range("D5:F5").Style.Font.FontName = "Calibri";
                    sht.Range("D5:F5").Style.Font.FontSize = 11;
                    sht.Range("D5:F5").Style.Font.Bold = true;
                    sht.Range("D5:F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D5:F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D5:F5").Merge();

                    sht.Cell("A6").Value = "KIRPA RUGS";
                    sht.Range("A6:B6").Style.Font.FontName = "Calibri";
                    sht.Range("A6:B6").Style.Font.FontSize = 11;
                    sht.Range("A6:B6").Style.Font.Bold = true;
                    sht.Range("A6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A6:B6").Merge();

                    sht.Cell("C6").Value = "";
                    sht.Range("C6:C6").Style.Font.FontName = "Calibri";
                    sht.Range("C6:C6").Style.Font.FontSize = 11;
                    sht.Range("C6:C6").Style.Font.Bold = true;
                    sht.Range("C6:C6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C6:C6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C6:C6").Merge();

                    sht.Cell("D6").Value = " " + ds.Tables[0].Rows[0]["CompAddr2"].ToString() + " " + ds.Tables[0].Rows[0]["CompAddr3"].ToString();
                    sht.Range("D6:F6").Style.Font.FontName = "Calibri";
                    sht.Range("D6:F6").Style.Font.FontSize = 11;
                    sht.Range("D6:F6").Style.Font.Bold = true;
                    sht.Range("D6:F6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D6:F6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D6:F6").Merge();
                    sht.Range("D6:F6").Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A1:F1"))
                    {
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    }
                    using (var a = sht.Range("A1:A19"))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }
                    using (var a = sht.Range("F1:F19"))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }
                    using (var a = sht.Range("A7:F7"))
                    {
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    }


                    sht.Cell("A7").Value = "INVOICE NO:" + " " + ds.Tables[0].Rows[0]["TInvoiceNo"];
                    sht.Range("A7:C7").Style.Font.FontName = "Calibri";
                    sht.Range("A7:C7").Style.Font.FontSize = 12;
                    sht.Range("A7:C7").Style.Font.Bold = true;
                    sht.Range("A7:C7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A7:C7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A7:C7").Merge();

                    sht.Cell("A8").Value = "INVOICE DATE:" + " " + ds.Tables[0].Rows[0]["INVOICEDATE"];
                    sht.Range("A8:C8").Style.Font.FontName = "Calibri";
                    sht.Range("A8:C8").Style.Font.FontSize = 12;
                    sht.Range("A8:C8").Style.Font.Bold = true;
                    sht.Range("A8:C8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A8:C8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A8:C8").Merge();

                    sht.Cell("A9").Value = "REVERSE CHARGE (Y/N):";
                    sht.Range("A9:C9").Style.Font.FontName = "Calibri";
                    sht.Range("A9:C9").Style.Font.FontSize = 12;
                    sht.Range("A9:C9").Style.Font.Bold = true;
                    sht.Range("A9:C9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A9:C9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A9:C9").Merge();

                    sht.Cell("A10").Value = "STATE     UTTAR PARDESH";
                    sht.Range("A10:B10").Style.Font.FontName = "Calibri";
                    sht.Range("A10:B10").Style.Font.FontSize = 12;
                    sht.Range("A10:B10").Style.Font.Bold = true;
                    sht.Range("A10:B10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A10:B10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A10:B10").Merge();

                    sht.Cell("C10").Value = "CODE    9";
                    sht.Range("C10:C10").Style.Font.FontName = "Calibri";
                    sht.Range("C10:C10").Style.Font.FontSize = 12;
                    sht.Range("C10:C10").Style.Font.Bold = true;
                    sht.Range("C10:C10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C10:C10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C10:C10").Merge();

                    sht.Cell("A11").Value = "PO NO:" + " " + ds.Tables[0].Rows[0]["CustomerOrderNo"];
                    sht.Range("A11:C11").Style.Font.FontName = "Calibri";
                    sht.Range("A11:C11").Style.Font.FontSize = 12;
                    sht.Range("A11:C11").Style.Font.Bold = true;
                    sht.Range("A11:C11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A11:C11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A11:C11").Merge();

                    using (var a = sht.Range("C7:C11"))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Cell("D7").Value = "TRANSPORT MODE:" + " " + ds.Tables[0].Rows[0]["PreCarriageBy"];
                    sht.Range("D7:F7").Style.Font.FontName = "Calibri";
                    sht.Range("D7:F7").Style.Font.FontSize = 12;
                    sht.Range("D7:F7").Style.Font.Bold = true;
                    sht.Range("D7:F7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D7:F7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D7:F7").Merge();

                    sht.Cell("D8").Value = "VEHICLE NO:" + " " + ds.Tables[0].Rows[0]["VehicleNo"];
                    sht.Range("D8:F8").Style.Font.FontName = "Calibri";
                    sht.Range("D8:F8").Style.Font.FontSize = 12;
                    sht.Range("D8:F8").Style.Font.Bold = true;
                    sht.Range("D8:F8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D8:F8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D8:F8").Merge();

                    sht.Cell("D9").Value = "DATE OF SUPPLY:" + " " + ds.Tables[0].Rows[0]["INVOICEDATE"];
                    sht.Range("D9:F9").Style.Font.FontName = "Calibri";
                    sht.Range("D9:F9").Style.Font.FontSize = 12;
                    sht.Range("D9:F9").Style.Font.Bold = true;
                    sht.Range("D9:F9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D9:F9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D9:F9").Merge();

                    sht.Cell("D10").Value = "PLACE OF SUPPLY:" + " " + ds.Tables[0].Rows[0]["CustomerFinalDestination"];
                    sht.Range("D10:F10").Style.Font.FontName = "Calibri";
                    sht.Range("D10:F10").Style.Font.FontSize = 12;
                    sht.Range("D10:F10").Style.Font.Bold = true;
                    sht.Range("D10:F10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D10:F10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D10:F10").Merge();

                    sht.Cell("D11").Value = "E-WAY BILL NO:" + " " + ds.Tables[0].Rows[0]["EWayBillNo"];
                    sht.Range("D11:F11").Style.Font.FontName = "Calibri";
                    sht.Range("D11:F11").Style.Font.FontSize = 12;
                    sht.Range("D11:F11").Style.Font.Bold = true;
                    sht.Range("D11:F11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D11:F11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D11:F11").Merge();


                    using (var a = sht.Range("A11:F11"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }


                    sht.Cell("A12").Value = "BILLED TO";
                    sht.Range("A12:C12").Style.Font.FontName = "Calibri";
                    sht.Range("A12:C12").Style.Font.FontSize = 12;
                    sht.Range("A12:C12").Style.Font.Bold = true;
                    sht.Range("A12:C12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A12:C12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A12:C12").Merge();

                    sht.Cell("A13").Value = ds.Tables[0].Rows[0]["TConsignee"];
                    sht.Range("A13:C17").Style.Font.FontName = "Calibri";
                    sht.Range("A13:C17").Style.Font.FontSize = 11;
                    sht.Range("A13:C17").Style.Font.Bold = true;
                    sht.Range("A13:C17").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A13:C17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A13:C17").Merge();
                    sht.Range("A13:C17").Style.Alignment.SetWrapText();

                    using (var a = sht.Range("C12:C17"))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Cell("D12").Value = "SHIPPED TO";
                    sht.Range("D12:F12").Style.Font.FontName = "Calibri";
                    sht.Range("D12:F12").Style.Font.FontSize = 12;
                    sht.Range("D12:F12").Style.Font.Bold = true;
                    sht.Range("D12:F12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D12:F12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D12:F12").Merge();

                    sht.Cell("D13").Value = ds.Tables[0].Rows[0]["InvoiceShipToAddress"];
                    sht.Range("D13:F17").Style.Font.FontName = "Calibri";
                    sht.Range("D13:F17").Style.Font.FontSize = 11;
                    sht.Range("D13:F17").Style.Font.Bold = true;
                    sht.Range("D13:F17").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D13:F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("D13:F17").Merge();
                    sht.Range("D13:F17").Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A17:F17"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A18").Value = "HSN CODE";
                    sht.Range("A18:A18").Style.Font.FontName = "Calibri";
                    sht.Range("A18:A18").Style.Font.FontSize = 13;
                    sht.Range("A18:A18").Style.Font.Bold = true;
                    sht.Range("A18:A18").Merge();
                    sht.Range("A18:A18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A18:A18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A18:A18").Style.Alignment.SetWrapText();

                    sht.Range("B18").Value = "STYLE";
                    sht.Range("B18:B18").Style.Font.FontName = "Calibri";
                    sht.Range("B18:B18").Style.Font.FontSize = 13;
                    sht.Range("B18:B18").Style.Font.Bold = true;
                    sht.Range("B18:B18").Merge();
                    sht.Range("B18:B18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B18:B18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B18:B18").Style.Alignment.SetWrapText();

                    sht.Range("C18").Value = "SIZE";
                    sht.Range("C18:C18").Style.Font.FontName = "Calibri";
                    sht.Range("C18:C18").Style.Font.FontSize = 13;
                    sht.Range("C18:C18").Style.Font.Bold = true;
                    sht.Range("C18:C18").Merge();
                    sht.Range("C18:C18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C18:C18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C18:C18").Style.Alignment.SetWrapText();

                    sht.Range("D18").Value = "QUANTITY";
                    sht.Range("D18:D18").Style.Font.FontName = "Calibri";
                    sht.Range("D18:D18").Style.Font.FontSize = 13;
                    sht.Range("D18:D18").Style.Font.Bold = true;
                    sht.Range("D18:D18").Merge();
                    sht.Range("D18:D18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("D18:D18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D18:D18").Style.Alignment.SetWrapText();

                    sht.Range("E18").Value = "RATE";
                    sht.Range("E18:E18").Style.Font.FontName = "Calibri";
                    sht.Range("E18:E18").Style.Font.FontSize = 13;
                    sht.Range("E18:E18").Style.Font.Bold = true;
                    sht.Range("E18:E18").Merge();
                    sht.Range("E18:E18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E18:E18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E18:E18").Style.Alignment.SetWrapText();

                    sht.Range("F18").Value = "AMOUNT";
                    sht.Range("F18:F18").Style.Font.FontName = "Calibri";
                    sht.Range("F18:F18").Style.Font.FontSize = 13;
                    sht.Range("F18:F18").Style.Font.Bold = true;
                    sht.Range("F18:F18").Merge();
                    sht.Range("F18:F18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F18:F18").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F18:F18").Style.Alignment.SetWrapText();


                    using (var a = sht.Range("A18:F18"))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    // decimal TotalArea = 0, TotalTaxableValue = 0, TotalIGSTAmount = 0, TotalAmountInINR = 0, TotalAmountBeforeTax = 0, TotalUSDollarAmount = 0;

                    int row = 19;
                    int totalrowcount = 45;
                    int rowcount = ds.Tables[0].Rows.Count;
                    //totalrowcount = totalrowcount - rowcount;

                    if (rowcount < totalrowcount)
                    {
                        totalrowcount = totalrowcount - rowcount;
                    }
                    else
                    {
                        totalrowcount = rowcount;
                    }
                    int SrNo = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
                        sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 12;
                        //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                        sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        //sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();
                        sht.Range("A" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        SrNo = SrNo + 1;

                        ////sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["HSNCode"]);
                        sht.Range("A" + row).Style.Alignment.SetWrapText();
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Design"]);
                        sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Width"] + " x " + ds.Tables[0].Rows[i]["Length"]);
                        sht.Range("C" + row).Style.Alignment.SetWrapText();
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Price"]);
                        sht.Range("F" + row).SetValue(string.Format("{0:0.00}", ds.Tables[0].Rows[i]["Amount"].ToString()));


                        //decimal TaxableValue = 0;
                        //TaxableValue = Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                        //TotalTaxableValue = TotalTaxableValue + TaxableValue;

                        //sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["IGST"]);
                        //sht.Range("N" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("N" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        //decimal IGSTAmount = 0;
                        //IGSTAmount = ((TaxableValue * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"])) / 100);
                        //TotalIGSTAmount = TotalIGSTAmount + IGSTAmount;

                        //sht.Range("O" + row).SetValue(string.Format("{0:0.00}", IGSTAmount));
                        //sht.Range("O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("O" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        //sht.Range("P" + row).SetValue(string.Format("{0:0.00}", TaxableValue + IGSTAmount));
                        //sht.Range("P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("P" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //TotalAmountInINR = TotalAmountInINR + TaxableValue + IGSTAmount;

                        //decimal USDollarAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                        //TotalUSDollarAmount = TotalUSDollarAmount + USDollarAmount;

                        ////TotalArea = TotalArea + Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Area"]), 4);
                        ////sht.Range("J" + row).SetValue(Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Area"]), 4));
                        ////sht.Range("J" + row).Style.Alignment.SetWrapText();                    

                        //////sht.Range("L" + row).SetValue(string.Format("{0:0.00}", ds.Tables[0].Rows[i]["Amount"].ToString()));
                        //////TotalAmountBeforeTax = TotalAmountBeforeTax + Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"]), 2);


                        using (var a = sht.Range("A" + row + ":F" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }

                    if (totalrowcount > 0)
                    {
                        for (int j = row; j < totalrowcount - 1; j++)
                        {
                            sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
                            sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 12;
                            sht.Range("A" + row + ":F" + row).Style.Font.SetBold();
                            sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            //sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                            //sht.Range("A" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                            sht.Range("A" + row).SetValue("");
                            sht.Range("B" + row).SetValue("");
                            sht.Range("C" + row).SetValue("");
                            sht.Range("D" + row).SetValue("");
                            sht.Range("E" + row).SetValue("");
                            sht.Range("F" + row).SetValue("");


                            using (var a = sht.Range("A" + row + ":F" + row))
                            {
                                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            }
                            row = row + 1;
                        }
                    }

                    ////Total Amount
                    //row = row + 1;

                    sht.Range("A" + row + ":C" + row).Merge();
                    sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 12;
                    sht.Range("A" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "TOTAL PAYABLE AMOUNT (in Words)";
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "TOTAL TAXABLE AMOUNT";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("E" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    sht.Range("F" + row).Value = (string.Format("{0:0.00}", " " + Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", ""))));
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("A" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }


                    row = row + 1;

                    decimal SGSTTaxAmount = 0;
                    SGSTTaxAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["SGST"])) / 100);

                    decimal CGSTTaxAmount = 0;
                    CGSTTaxAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["CGST"])) / 100);

                    decimal IGSTTaxAmount = 0;
                    IGSTTaxAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["IGST"])) / 100);

                    Decimal Totalmt = 0;
                    Totalmt = (Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) + SGSTTaxAmount + CGSTTaxAmount + IGSTTaxAmount);
                    string amountinwords = "";

                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Totalmt));

                    string Pointamt = string.Format("{0:0.00}", Totalmt.ToString("0.00"));
                    string val = "", paise = "";
                    if (Pointamt.IndexOf('.') > 0)
                    {
                        val = Pointamt.ToString().Split('.')[1];
                        if (Convert.ToInt32(val) > 0)
                        {
                            paise = "& Paise " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                        }
                    }

                    amountinwords = amountinwords + " " + paise + "Only";

                    sht.Row(row).Height = 30;

                    ////FOB INR Amt in Words
                    sht.Range("A" + row + ":C" + row).Merge();
                    sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("A" + row).SetValue("INR:" + amountinwords.ToUpper());
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "ADD CGST @" + " " + ds.Tables[0].Rows[0]["CGST"] + " %";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("E" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    decimal CGSTAmount = 0;
                    CGSTAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["CGST"])) / 100);

                    sht.Range("F" + row).Value = (string.Format("{0:0.00}", " " + CGSTAmount));
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("A" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }


                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 12;
                    sht.Range("A" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "BANK DETAILS";
                    sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).SetValue("Certified that the particulars given");
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "ADD SGST @" + " " + ds.Tables[0].Rows[0]["SGST"] + " %";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("E" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    decimal SGSTAmount = 0;
                    SGSTAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["SGST"])) / 100);

                    sht.Range("F" + row).Value = (string.Format("{0:0.00}", " " + SGSTAmount));
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("A" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }



                    row = row + 1;

                    sht.Row(row).Height = 30;

                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "1" + " " + ds.Tables[0].Rows[0]["BankName"];
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    ////BANK Address
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankAddress"].ToString());
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).SetValue("above are TRUE & CORRECT");
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "TAX AMOUNT IGST @" + " " + ds.Tables[0].Rows[0]["IGST"] + " %";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("E" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    decimal IGSTAmount = 0;
                    IGSTAmount = ((Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) * Convert.ToDecimal(ds.Tables[0].Rows[0]["IGST"])) / 100);

                    sht.Range("F" + row).Value = (string.Format("{0:0.00}", " " + IGSTAmount));
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("D" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "ACCOUNT NO";
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankAccountNo"].ToString());
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).Value = "For" + " " + ds.Tables[0].Rows[0]["CompanyName"];
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "TOTAL AMOUNT ";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("E" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    decimal TotalAmountAfterTaxAdd = 0;
                    TotalAmountAfterTaxAdd = (Convert.ToDecimal(ds.Tables[0].Compute("sum(Amount)", "")) + CGSTAmount + SGSTAmount + IGSTAmount);

                    sht.Range("F" + row).Value = (string.Format("{0:0.00}", " " + TotalAmountAfterTaxAdd));
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("D" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }


                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "IFSC CODE";
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankIfscCode"].ToString());
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).Value = " ";
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //using (var a = sht.Range("E" + row + ":E" + row))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //}

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();

                    sht.Range("F" + row).Value = ("");
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    //using (var a = sht.Range("D" + row + ":F" + row))
                    //{
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}

                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = " ";
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).Value = " ";
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).Value = " ";
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //using (var a = sht.Range("E" + row + ":E" + row))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //}

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();

                    sht.Range("F" + row).Value = ("");
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    //using (var a = sht.Range("D" + row + ":F" + row))
                    //{
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}



                    row = row + 1;

                    sht.Row(row).Height = 30;

                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "2." + " " + ds.Tables[0].Rows[0]["BankName2"];
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankAddress2"]);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).SetValue("Authorised Signatory");
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //using (var a = sht.Range("E" + row + ":E" + row))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //}

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();

                    sht.Range("F" + row).Value = ("");
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    //using (var a = sht.Range("D" + row + ":F" + row))
                    //{
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}

                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "ACCOUNT NO";
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankAccountNo2"].ToString());
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).Value = "E. & O. E.";
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //using (var a = sht.Range("E" + row + ":E" + row))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //}

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();

                    sht.Range("F" + row).Value = ("");
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    //using (var a = sht.Range("D" + row + ":F" + row))
                    //{
                    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //}

                    row = row + 1;
                    ////BANK NAME
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Value = "IFSC CODE";
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    ////BANK Account
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[0]["BankIfscCode2"].ToString());
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("A" + row + ":A" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("B" + row + ":B" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row).SetValue("All Subjects to Sitapur Jurisdiction");
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetWrapText();

                    using (var a = sht.Range("C" + row + ":C" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":E" + row).Merge();
                    sht.Range("D" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":E" + row).Style.Font.FontSize = 12;
                    sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("D" + row).Value = "";
                    sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //using (var a = sht.Range("E" + row + ":E" + row))
                    //{
                    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //}

                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 12;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();

                    sht.Range("F" + row).Value = ("");
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    using (var a = sht.Range("F" + row + ":F" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }

                    using (var a = sht.Range("A" + row + ":F" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                }


                ////*************
                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("InvoiceDetailReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        InvoiceDetailReport();
    }

    protected void DDInvoiceYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDInvoiveNo, @"select I.Invoiceid,I.TInvoiceNo 
                From Invoice I,Packing P Where P.PackingId=I.InvoiceId And I.Status=0 And I.InvoiceType<>3 And I.ConsignorID = " + Session["CurrentWorkingCompanyID"] + @" And 
                P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and I.invoiceyear="+DDInvoiceYear.SelectedValue+" Order By I.TinvoiceNo desc", true, "--Select--");

        TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        DDInvoiveNo.Focus();
    }

}