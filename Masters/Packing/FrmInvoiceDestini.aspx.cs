using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Packing_FrmInvoiceDestini : System.Web.UI.Page
{
    string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();
            UtilityModule.ConditionalComboFill(ref DDInvoiveNo, @"select I.Invoiceid,I.TInvoiceNo 
                From Invoice I,Packing P 
                Where  P.PackingId=I.PackingId And I.Status=0 And I.InvoiceType<>3 And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                And I.consignorId = " + Session["CurrentWorkingCompanyID"] + @" Order By I.TinvoiceNo desc", true, "--Select--");
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtShippingBillDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            DDInvoiveNo.Focus();
            LblErrorMessage.Text = "";
        }
    }
    protected void DDInvoiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDInvoiveNo.SelectedIndex > 0)
        {
            RDCustomer.Checked = false;
            RDBank.Checked = false;
            RDUnToOrder.Checked = true;
            ChangeAccToRadioButton();
            Fill_DropDownList();
            TxtInvoiceId.Text = DDInvoiveNo.SelectedValue;
            string str = "select isnull(sum(NetWeight),0) NetWeight,isnull(sum(GROSSWEIGHT),0) GROSSWEIGHT from V_Packing_Net_GrossWeight where packingid= " + DDInvoiveNo.SelectedValue;
            //            string str = @" select isnull(QtyRequired*Weight,0) NetWeight,isnull((Max(PCM.GROSSWEIGHT))*OD.QtyRequired ,0) GROSSWEIGHT from ordermaster OM, orderdetail  OD, PackingInformation PInfo, processconsumptionmaster PCM
            //                            where OM.OrderId= OD.OrderId AND OM.OrderId= PInfo.OrderId AND OD.Item_Finished_Id=PInfo.FinishedId AND PCM.finishedid=PInfo.FinishedId AND PInfo.PackingId=" + DDInvoiveNo.SelectedValue +@"
            //                            GROUP BY OD.OrderId,PInfo.FinishedId,QtyRequired,Weight";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ChkInvoice.Checked == false)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TxtNetWeight.Text = ds.Tables[0].Rows[0]["NetWeight"].ToString();
                    TxtGrossWeight.Text = ds.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();
                }
            }
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
        Str = @"Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            Select Agentid,Agentname from Shipp Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Agentname
            select carriageid,carriageName from Carriage Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by carriageName
            Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
            Select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by TransModename";
        //Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName";
        DataSet ds = SqlHelper.ExecuteDataset(Str);
        UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShippingAgent, ds, 1, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDPreCarriage, ds, 2, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDReceiptAt, ds, 3, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDByAirSea, ds, 4, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDPortLoad, ds, 3, true, "--Select--");

        DDCurrency.SelectedIndex = 0;
        DDShippingAgent.SelectedIndex = 0;
        DDPreCarriage.SelectedIndex = 0;
        DDReceiptAt.SelectedIndex = 0;
        DDByAirSea.SelectedIndex = 0;
        DDPortLoad.SelectedIndex = 0;

        if (ChkInvoice.Checked == false)
        {
            Str = @"Select P.CurrencyId,CI.ShippingAgent,CI.PreCarriageBy,CI.RecieptAtByPreCarrier,CI.ByAirSea,CI.PortOfLoading,CI.Mark,CI.DestinationPlace,CI.PaymentId,
                    CI.TermId,CI.BankId,'' PortUnload From Packing P,CustomerInfo CI Where P.ConsigneeId=CI.CustomerID And P.PackingID=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            BtnDelete.Visible = false;
        }
        else
        {
            //            Str = @"Select CurrencyType CurrencyId,Agentid ShippingAgent,PreCarrier PreCarriageBy,Receipt RecieptAtByPreCarrier,ShipingId ByAirSea,PortLoad PortOfLoading,
            //                    RollMarkHead Mark,DestinationAdd DestinationPlace,DelTerms PaymentId,CreditId TermId,Isnull(Bankrefno,0) BankId,PortUnload
            //                    From Invoice Where PackingID=" + DDInvoiveNo.SelectedValue;
            Str = @"Select TypeOfBuyerOtherConsignee,TConsignor,TConsignee,SBillNO,RollMark,RollMarkHead,SBILLDATE,VesselName,GrossWt,
                    NetWt,LessAdvance,Insurance,Freight,InvoiceAmount,ExtraCharges,ExtraChargeRemarks,TBuyerOConsignee,InvoiceDate,CurrencyType CurrencyId,
                    Agentid ShippingAgent,PreCarrier PreCarriageBy,Receipt RecieptAtByPreCarrier,ShipingId ByAirSea,PortLoad PortOfLoading,
                    RollMarkHead Mark,DestinationAdd DestinationPlace,DelTerms PaymentId,CreditId TermId,Isnull(Bankrefno,0) BankId,PortUnload
                    From Invoice Where PackingID=" + DDInvoiveNo.SelectedValue;
            BtnDelete.Visible = true;
        }

        //if (ChkInvoice.Checked == true)
        //{
        //    Str = "Select P.CurrencyId,CI.* From Packing P,CustomerInfo CI Where P.ConsigneeId=CI.CustomerID And P.packingid=" + DDInvoiveNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        DataSet Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds1.Tables[0].Rows.Count > 0)
        {
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedValue = Ds1.Tables[0].Rows[0]["CurrencyId"].ToString();
            }

            DDShippingAgent.SelectedValue = Ds1.Tables[0].Rows[0]["ShippingAgent"].ToString();
            DDPreCarriage.SelectedValue = Ds1.Tables[0].Rows[0]["PreCarriageBy"].ToString();
            DDReceiptAt.SelectedValue = Ds1.Tables[0].Rows[0]["RecieptAtByPreCarrier"].ToString();
            DDByAirSea.SelectedValue = Ds1.Tables[0].Rows[0]["ByAirSea"].ToString();
            DDPortLoad.SelectedValue = Ds1.Tables[0].Rows[0]["PortOfLoading"].ToString();
            TxtRollMarkHead.Text = Ds1.Tables[0].Rows[0]["Mark"].ToString().ToUpper();
            TxtFinalDestination.Text = Ds1.Tables[0].Rows[0]["DestinationPlace"].ToString().ToUpper();
            TxtPortDisCharge.Text = Ds1.Tables[0].Rows[0]["PortUnload"].ToString().ToUpper();

            if (ChkInvoice.Checked == true)
            {
                // TxtRollNo.Text = Ds.Tables[0].Rows[0]["PortUnload"].ToString();
                TxtVessalName.Text = Ds1.Tables[0].Rows[0]["VesselName"].ToString();
                TxtGrossWeight.Text = Ds1.Tables[0].Rows[0]["GrossWt"].ToString();
                TxtNetWeight.Text = Ds1.Tables[0].Rows[0]["NetWt"].ToString();
                TxtPreAdvance.Text = Ds1.Tables[0].Rows[0]["LessAdvance"].ToString();
                // TxtAdvanceRec.Text = Ds.Tables[0].Rows[0]["Insurance"].ToString();
                TxtAddFrieght.Text = Ds1.Tables[0].Rows[0]["Freight"].ToString();
                TxtAddInsurance.Text = Ds1.Tables[0].Rows[0]["Insurance"].ToString();
                TxtInvoiceAmt.Text = Ds1.Tables[0].Rows[0]["InvoiceAmount"].ToString();
                TxtExtraChargeAmt.Text = Ds1.Tables[0].Rows[0]["ExtraCharges"].ToString();
                TxtExtraChargeRemark.Text = Ds1.Tables[0].Rows[0]["ExtraChargeRemarks"].ToString().ToUpper();
                TxtShippingBillNo.Text = Ds1.Tables[0].Rows[0]["SBillNO"].ToString().ToUpper();
                TxtShippingBillDate.Text = Ds1.Tables[0].Rows[0]["SBILLDATE"].ToString();
            }
        }
        //}
    }
    protected void RDUnToOrder_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(1);
        ChangeAccToRadioButton();
    }
    protected void RDCustomer_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(2);
        ChangeAccToRadioButton();
    }
    protected void RDBank_CheckedChanged(object sender, EventArgs e)
    {
        RadioButtonSelectedChange(3);
        ChangeAccToRadioButton();
    }
    private void ChangeAccToRadioButton()
    {
        string Str = "";
        string VarConsignoree = "";
        string VarBank = "";
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
                B.PhoneNo BPhoneNo,B.Faxno BFaxno,B.Email BEmail from Invoice I,CompanyInfo CI,CustomerInfo CUI,Bank B Where I.ConsignorId=CI.CompanyId And 
                I.CosigneeId=CUI.CustomerId And B.BankId=CUI.BankId And I.InvoiceID=" + DDInvoiveNo.SelectedValue + " And CUI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtConsignor.Text = Ds.Tables[0].Rows[0]["CICompanyName"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr1"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr2"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompAddr3"].ToString() + "," + Ds.Tables[0].Rows[0]["CICompTel"].ToString() + "," + Ds.Tables[0].Rows[0]["CIEmail"].ToString() + "," + Ds.Tables[0].Rows[0]["CITinNo"].ToString();
            VarConsignoree = Ds.Tables[0].Rows[0]["CUICompanyName"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIAddress"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIPhoneNo"].ToString() + "," + Ds.Tables[0].Rows[0]["CUIMobile"].ToString();
            TxtBuyerOtherThanConsignee.Text = Ds.Tables[0].Rows[0]["BuyerOtherThanConsignee"].ToString();
            VarBank = Ds.Tables[0].Rows[0]["BBankName"].ToString() + "," + Ds.Tables[0].Rows[0]["BStreet"].ToString() + "," + Ds.Tables[0].Rows[0]["BCity"].ToString() + "," + Ds.Tables[0].Rows[0]["BState"].ToString() + "," + Ds.Tables[0].Rows[0]["BCountry"].ToString() + "," + Ds.Tables[0].Rows[0]["BPhoneNo"].ToString() + "," + Ds.Tables[0].Rows[0]["BFaxno"].ToString() + "," + Ds.Tables[0].Rows[0]["BEmail"].ToString();
        }
        if (RDCustomer.Checked == true)
        {
            TxtConsignee.Text = VarConsignoree;
        }
        else if (RDBank.Checked == true)
        {
            TxtConsignee.Text = VarBank;
            if (RDUnToOrder.Checked == true)
            {
                TxtBuyerOtherThanConsignee.Text = VarConsignoree;
                TxtConsignee.Text = "UNTO ORDER";
            }
        }
        else if (RDUnToOrder.Checked == true)
        {
            TxtBuyerOtherThanConsignee.Text = VarConsignoree;
            TxtConsignee.Text = "UNTO ORDER";
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
                SqlParameter[] _arrPara = new SqlParameter[27];
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
                _arrPara[13] = new SqlParameter("@SBillNO", SqlDbType.NVarChar, 50);
                _arrPara[14] = new SqlParameter("@RollMark", SqlDbType.NVarChar, 50);
                _arrPara[15] = new SqlParameter("@RollMarkHead", SqlDbType.NVarChar, 100);
                _arrPara[16] = new SqlParameter("@SBILLDATE", SqlDbType.SmallDateTime);
                _arrPara[17] = new SqlParameter("@DestinationAdd", SqlDbType.NVarChar, 50);
                _arrPara[18] = new SqlParameter("@VesselName", SqlDbType.NVarChar, 50);
                _arrPara[19] = new SqlParameter("@GrossWt", SqlDbType.Float);
                _arrPara[20] = new SqlParameter("@NetWt", SqlDbType.Float);
                _arrPara[21] = new SqlParameter("@LessAdvance", SqlDbType.Float);
                _arrPara[22] = new SqlParameter("@Insurance", SqlDbType.Float);
                _arrPara[23] = new SqlParameter("@Freight", SqlDbType.Float);
                _arrPara[24] = new SqlParameter("@InvoiceAmount", SqlDbType.Float);
                _arrPara[25] = new SqlParameter("@ExtraCharges", SqlDbType.Float);
                _arrPara[26] = new SqlParameter("@ExtraChargeRemarks", SqlDbType.NVarChar, 100);

                _arrPara[0].Value = DDInvoiveNo.SelectedValue;
                _arrPara[1].Value = RDBank.Checked == true ? 3 : RDCustomer.Checked == true ? 2 : 1;
                _arrPara[2].Value = TxtConsignor.Text.ToUpper();
                _arrPara[3].Value = TxtConsignee.Text.ToUpper();
                _arrPara[4].Value = TxtBuyerOtherThanConsignee.Text.ToUpper();
                _arrPara[5].Value = DDCurrency.SelectedValue;
                _arrPara[6].Value = TxtDate.Text;
                _arrPara[7].Value = DDShippingAgent.SelectedValue;
                _arrPara[8].Value = DDPreCarriage.SelectedValue;
                _arrPara[9].Value = DDReceiptAt.SelectedValue;
                _arrPara[10].Value = DDByAirSea.SelectedValue;
                _arrPara[11].Value = DDPortLoad.SelectedValue;
                _arrPara[12].Value = TxtPortDisCharge.Text;
                _arrPara[13].Value = TxtShippingBillNo.Text;
                _arrPara[14].Value = TxtRollMarkHead.Text.ToUpper();
                _arrPara[15].Value = TxtRollMarkHead.Text.ToUpper();
                _arrPara[16].Value = TxtShippingBillDate.Text;
                _arrPara[17].Value = TxtFinalDestination.Text;
                _arrPara[18].Value = TxtVessalName.Text.ToUpper();
                _arrPara[19].Value = TxtGrossWeight.Text == "" ? "0" : TxtGrossWeight.Text;
                _arrPara[20].Value = TxtNetWeight.Text == "" ? "0" : TxtNetWeight.Text;
                _arrPara[21].Value = TxtPreAdvance.Text == "" ? "0" : TxtPreAdvance.Text;
                _arrPara[22].Value = TxtAddInsurance.Text == "" ? "0" : TxtAddInsurance.Text;
                _arrPara[23].Value = TxtAddFrieght.Text == "" ? "0" : TxtAddFrieght.Text;
                _arrPara[24].Value = TxtInvoiceAmt.Text == "" ? "0" : TxtInvoiceAmt.Text;
                _arrPara[25].Value = TxtExtraChargeAmt.Text == "" ? "0" : TxtExtraChargeAmt.Text;
                _arrPara[26].Value = TxtExtraChargeRemark.Text;

                string Str = "Update Invoice Set TypeOfBuyerOtherConsignee=" + _arrPara[1].Value + ",TConsignor='" + _arrPara[2].Value + "',TConsignee='" + _arrPara[3].Value + @"',
                            TBuyerOConsignee='" + _arrPara[4].Value + "',CurrencyType='" + _arrPara[5].Value + "',InvoiceDate='" + _arrPara[6].Value + "',Agentid=" + _arrPara[7].Value + @",
                            PreCarrier=" + _arrPara[8].Value + ",Receipt=" + _arrPara[9].Value + ",ShipingId=" + _arrPara[10].Value + ",PortLoad=" + _arrPara[11].Value + @",
                            PortUnload='" + _arrPara[12].Value + "',SBillNO='" + _arrPara[13].Value + "',RollMark='" + _arrPara[14].Value + "',RollMarkHead='" + _arrPara[15].Value + @"',
                            SBILLDATE='" + _arrPara[16].Value + "',DestinationAdd='" + _arrPara[17].Value + "',VesselName='" + _arrPara[18].Value + "',GrossWt=" + _arrPara[19].Value + @",
                            NetWt=" + _arrPara[20].Value + ",LessAdvance=" + _arrPara[21].Value + ",Insurance=" + _arrPara[22].Value + ",Freight=" + _arrPara[23].Value + @",
                            InvoiceAmount=" + _arrPara[24].Value + ",ExtraCharges=" + _arrPara[25].Value + ",ExtraChargeRemarks='" + _arrPara[26].Value + @"', 
                            Status=1 Where InvoiceId=" + DDInvoiveNo.SelectedValue;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                Tran.Commit();
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Invoice'," + DDInvoiveNo.SelectedValue + ",getdate(),'Update')");
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Saved Successfully";
                msg = "Record(s) has been saved successfully !";
                MessageSave(msg);
                Refresh();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceDestini.aspx");
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
    B: ;
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
        string Str = @"Select I.Invoiceid, I.TInvoiceNo 
            From Invoice I,Packing P 
            Where I.Packingid=P.Packingid And I.InvoiceType<>3 And I.consignorId = " + Session["CurrentWorkingCompanyID"] + " And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        if (ChkInvoice.Checked == true)
        {
            Str = Str + " And I.Status=1 ";
        }
        else
        {
            BtnDelete.Visible = false;
            Str = Str + " And I.Status=0";
        }
        UtilityModule.ConditionalComboFill(ref DDInvoiveNo, Str, true, "--Select--");
        Str = Str + " Order By I.TinvoiceNo desc";
        Refresh();
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string Str = "Update Invoice SET Status=0 Where Invoiceid=" + DDInvoiveNo.SelectedValue;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            msg = "Record(s) has been successfully deleted !";
            MessageSave(msg);
            //ChkInvoice.Checked = false;
            UtilityModule.ConditionalComboFill(ref DDInvoiveNo, @"select I.Invoiceid,I.TInvoiceNo 
                From Invoice I,Packing P 
                Where I.Packingid=P.Packingid And I.Status=1 And 
                I.consignorId = " + Session["CurrentWorkingCompanyID"] + " And I.InvoiceType<>3 And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                Order By I.TinvoiceNo desc", true, "--Select--");

            Refresh();
        }
        catch (Exception ex)
        {
            msg = "Unsuccessful !" + ex.Message.ToString();
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
    private void Refresh()
    {
        if (DDShippingAgent.Items.Count > 0)
        {
            DDShippingAgent.SelectedIndex = 0;
        }

        TxtRollMarkHead.Text = "";
        //TxtRollNo.Text = "";
        TxtFinalDestination.Text = "";
        TxtGrossWeight.Text = "";
        TxtNetWeight.Text = "";
        TxtPreAdvance.Text = "";
        //TxtAdvanceRec.Text = "";
        TxtAddInsurance.Text = "";
        TxtAddFrieght.Text = "";
        TxtInvoiceAmt.Text = "";
        TxtExtraChargeAmt.Text = "";
        TxtExtraChargeRemark.Text = "";
        TxtPortDisCharge.Text = "";
        TxtShippingBillNo.Text = "";
        TxtVessalName.Text = "";
        DDInvoiveNo.SelectedIndex = 0;
        DDCurrency.SelectedIndex = 0;
        DDShippingAgent.SelectedIndex = 0;
        DDPreCarriage.SelectedIndex = 0;
        DDReceiptAt.SelectedIndex = 0;
        DDByAirSea.SelectedIndex = 0;
        DDPortLoad.SelectedIndex = 0;
    }
}