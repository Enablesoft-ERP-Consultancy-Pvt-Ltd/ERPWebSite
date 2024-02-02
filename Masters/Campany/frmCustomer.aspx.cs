using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_frmCustomer : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            fill_ddl();
            Fill_Grid();
            Fill_chelist();

            Session["ReportPath"] = "Reports/CustomerDetail.rpt";
            Session["CommanFormula"] = "";
            //UtilityModule.ConditionalComboFill(ref ddContinent, "select continentid,continentname from continentmaster Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by continentname", true, "--select--");
            // UtilityModule.ConditionalComboFill(ref DDbuyinghouse, "select buyinghouseId,Name_buying_house from buyinghouse where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by  Name_buying_house", true, "--Select--");

            string str = @"select AgencyId,AgencyName from shippingagency order by AgencyName
                           select BuyingHouseId,Name_buying_house from BuyingHouse order by Name_buying_house
                           select CUstomerTypeId,TypeName from CustomerType order by TypeName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDShippingAgency, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDBuyingHouse1, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddTypeofCustomer, ds, 2, true, "--Plz Select--");

            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "9":
                    //lblOtherThanConsigneeAir.Text = "Buyer Other Than Consignee";
                    //TDddlReceiptPreCar.Visible = false;
                    //TDlblReceiptAtPreCarrier.Visible = false;
                    //TDlblPreCarrierBy.Visible = false;
                    //TDddlPreCarr.Visible = false;
                    //TDTxtByerOtherThanConsSea.Visible = false;
                    //TDlblBuyerOtherThanConsigneeSea.Visible = false;
                    //TRBankAndAcNo.Visible = false;
                    TDBank.Visible = false;
                    TDBankLabel.Visible = false;
                    break;
            }
            hnmastercompanyid.Value = Session["varMasterCompanyIDForERP"].ToString();
        }


        lblErr.Visible = false;
    }
    private void Fill_Grid()
    {
        GvCustomer.DataSource = Fill_Grid_Data();
        GvCustomer.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT DISTINCT TOP (100) PERCENT CI.CustomerId, CI.CustomerName,CI.CustomerCode, CI.CompanyName, CI.Address, CI.BuyerOtherThanConsignee BuyerOtherThanConsigneeAir,BuerOtherThanConsigneeSea, CI.Mark, 
                      CountryMaster.CountryName, CI.PhoneNo, CI.Mobile, CI.Email, CI.Fax, CI.PinCode, Bank.BankName, CurrencyInfo.CurrencyName, 
                      CI.DestinationPlace, Carriage.CarriageName, TransMode.transmodeName, GoodsReceipt.StationName, CI.AirPort, CI.AcNo, 
                      CI.TinNo, CI.NotifyByAir, CI.SeaPort, CI.NotifyBySea, Shipp.AgentName, BuyingAgent.BuyeingAgentName,Payment.PaymentName,Term.TermName,
                      s1m.Statename,cm.continentname,s2m.Subcontinentname,s.SeaportName,a.AirPortName,isnull(CI.stateid,0) As Stateid,
                      isnull(CI.subcontinentid,0)As subcontinentid,isnull(CI.continentid,0) As continentid,bh.Name_buying_house,CustAdd1,CustAdd2,
                        isnull(Merchantname,'') as Merchantname,CI.GSTIN,CI.ConsigneeGSTIN,CI.ConsigneeCountry,CI.ConsigneeState,CI.ConsigneeStateCode
                      FROM BuyingAgent Right Outer JOIN customerinfo AS CI left outer join CountryMaster ON CI.Country=CountryMaster.CountryId Left Outer JOIN
                      Bank ON CI.BankId=Bank.BankId Left Outer JOIN CurrencyInfo ON CI.CurrencyId=CurrencyInfo.CurrencyId Left Outer JOIN
                      TransMode ON CI.ByAirSea=TransMode.transmodeId Left Outer JOIN GoodsReceipt ON CI.PortOfLoading=GoodsReceipt.GoodsreceiptId AND 
                      CI.RecieptAtByPreCarrier=GoodsReceipt.GoodsreceiptId Left Outer JOIN Shipp ON CI.ShippingAgent=Shipp.AgentId ON BuyingAgent.BuyeingAgentId=CI.BuyingAgent Left Outer JOIN
                      Carriage ON CI.PreCarriageBy=Carriage.CarriageId Left Outer JOIN Payment ON CI.PaymentId=Payment.PaymentId
				      Left Outer JOIN Term ON CI.TermId=Term.TermId left outer join continentmaster cm on cm.continentid=ci.continentid left outer join state_master s1m on s1m.stateid=ci.stateid
                      left outer join subcontinentmaster s2m on s2m.subcontinentid=ci.subcontinentid left outer join SeaPort s on s.seaportid=ci.seaportid 
                      left outer join AirPort a on a.airportid=ci.airportid left outer join buyinghouse bh on bh.buyinghouseId = BuyingAgent.buyinghouseId
                      Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (ddContinent.SelectedIndex > 0)
            {
                strsql = strsql + " And CI.continentid=" + ddContinent.SelectedValue;
            }
            if (DDSubcontinent.SelectedIndex > 0)
            {
                strsql = strsql + " And CI.subcontinentid=" + DDSubcontinent.SelectedValue;
            }
            if (ddCountry.SelectedIndex > 0)
            {
                strsql = strsql + " And CI.Country=" + ddCountry.SelectedValue;
            }
            if (DDState.SelectedIndex > 0)
            {
                strsql = strsql + " And CI.StateId=" + DDState.SelectedValue;
            }
            strsql = strsql + " ORDER BY CI.CustomerName";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
            Logs.WriteErrorLog("Masters_Campany_frmCustomer|Fill_Grid_Data|" + ex.Message);
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
    private void fill_ddl()
    {
        #region On 29-Nov-2012
        //CommanFunction.FillCombo(ddlCurrency, "Select CurrencyId,CurrencyName from currencyinfo order by CurrencyName");
        //CommanFunction.FillCombo(ddlBank, "Select Bankid,BankName from Bank order by BankName");
        //CommanFunction.FillCombo(ddlReceiptPreCar, "Select GoodsReceiptId, StationName from GoodsReceipt order by StationName");
        //CommanFunction.FillCombo(ddlPreCarr, "select carriageid,carriageName from Carriage order by carriageName");
        //CommanFunction.FillCombo(ddlByAirSea, "select TransModeid,TransModeName from Transmode order by TransModename");
        //CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt order by StationName");

        //UtilityModule.ConditionalComboFill(ref ddShipping, "Select Agentid,Agentname from Shipp order by Agentname", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddBuyingAgent, "Select BuyeingAgentid,BuyeingAgentname from BuyingAgent order by BuyeingAgentname", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddCountry, "select CountryId,CountryName from CountryMaster order by CountryName", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddPaymentMode, "Select PaymentId,PaymentName from Payment order by PaymentName", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term order by TermName", true, "--Select--");
        #endregion
        string str = @"Select CurrencyId,CurrencyName from currencyinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CurrencyName
                    Select Bankid,BankName from Bank where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by BankName
                    Select GoodsReceiptId, StationName from GoodsReceipt where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
                    select carriageid,carriageName from Carriage where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by carriageName
                    select TransModeid,TransModeName from Transmode where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by TransModename
                    Select GoodsReceiptId, StationName from GoodsReceipt where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        CommanFunction.FillComboWithDS(ddlCurrency, ds, 0);
        CommanFunction.FillComboWithDS(ddlBank, ds, 1);
        CommanFunction.FillComboWithDS(ddlReceiptPreCar, ds, 2);
        CommanFunction.FillComboWithDS(ddlPreCarr, ds, 3);
        CommanFunction.FillComboWithDS(ddlByAirSea, ds, 4);
        CommanFunction.FillComboWithDS(ddlPortOfLoading, ds, 5);
        string str1 = @"Select Agentid,Agentname from Shipp where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Agentname                                  
                      select CountryId,CountryName from CountryMaster where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CountryName
                      Select PaymentId,PaymentName from Payment where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PaymentName
                      select TermId,TermName from Term where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by TermName
                      select continentid,continentname from continentmaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by continentname";
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
        UtilityModule.ConditionalComboFillWithDS(ref ddShipping, ds1, 0, true, "--Select--");

        UtilityModule.ConditionalComboFillWithDS(ref ddCountry, ds1, 1, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref ddPaymentMode, ds1, 2, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref ddPaymentModeCustom, ds1, 2, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref ddDeliveryTerms, ds1, 3, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref ddContinent, ds1, 4, true, "--Select--");
    }
    protected void GvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GvCustomer, "Select$" + e.Row.RowIndex);
        }
    }
    protected void GvCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = @"SELECT DISTINCT 
                      TOP (100) PERCENT CI.CustomerId, CI.CustomerName, CI.CompanyName, CI.Address, CI.BuyerOtherThanConsignee BuyerOtherThanConsigneeAir,BuerOtherThanConsigneeSea, CI.Mark, 
                      CountryMaster.CountryName, CI.PhoneNo, CI.Mobile, CI.Email, CI.Fax, CI.PinCode, Bank.BankName, CurrencyInfo.CurrencyName, 
                      CI.DestinationPlace, Carriage.CarriageName, TransMode.transmodeName, GoodsReceipt.StationName, CI.AirPort, CI.AcNo, 
                      CI.CustomerCode, CI.TinNo, CI.NotifyByAir, CI.SeaPort, CI.NotifyBySea, Shipp.AgentName, BuyingAgent.BuyeingAgentName, 
                      CountryMaster.CountryId, CurrencyInfo.CurrencyId, TransMode.transmodeId, Carriage.CarriageId, 
                      Bank.BankId, CI.RecieptAtByPreCarrier, CI.PreCarriageBy, CI.PortOfLoading, CI.ByAirSea, CI.Country, CI.BankId AS Expr1, CI.CurrencyId AS Expr2, 
                     isnull(CI.ShippingAgent,0)ShippingAgent,isnull(CI.BuyingAgent,0)BuyingAgent,isnull(Payment.PaymentId,0)PaymentId,isnull(Term.TermId,0)TermId,isnull(CI.stateid,0) As Stateid,
                     cm.continentname,isnull(CI.subcontinentid,0)As subcontinentid,isnull(ci.continentid,0) As continentid,isnull(ci.seaportid,0) As seaportid,
                    isnull(ci.airportid,0) As airportid,isnull(bh.buyinghouseId,0) as BuyingHouseId,isnull(PriorityId,0) As PriorityId,SampleAddress,
                    Isnull(CustomerTypeId,0) As CustomerTypeId,isnull(CSA.ShippingAgencyid,0) As ShippingAgencyId,CSA.ShippingAgentId,CustAdd1,CustAdd2,
                    isnull(merchantname,'') as Merchantname,CI.GSTIN,CI.ConsigneeGSTIN,CI.ConsigneeCountry,CI.ConsigneeState,CI.ConsigneeStateCode,
                    isnull(PayCustom.PaymentId,0)PaymentIdCustom
                     FROM BuyingAgent Right Outer JOIN
                     customerinfo AS CI left outer join

                   
                      CountryMaster ON CI.Country=CountryMaster.CountryId Left Outer JOIN
                      Bank ON CI.BankId=Bank.BankId Left Outer JOIN
                      CurrencyInfo ON CI.CurrencyId=CurrencyInfo.CurrencyId Left Outer JOIN
                      TransMode ON CI.ByAirSea=TransMode.transmodeId Left Outer JOIN
                      GoodsReceipt ON CI.PortOfLoading=GoodsReceipt.GoodsreceiptId AND 
                      CI.RecieptAtByPreCarrier=GoodsReceipt.GoodsreceiptId Left Outer JOIN
                      Shipp ON CI.ShippingAgent=Shipp.AgentId ON BuyingAgent.BuyeingAgentId=CI.BuyingAgent Left Outer JOIN
                      Carriage ON CI.PreCarriageBy=Carriage.CarriageId Left Outer JOIN Payment ON CI.PaymentId=Payment.PaymentId
                      Left Outer JOIN Term ON CI.TermId=Term.TermId left outer join continentmaster cm on cm.continentid=ci.continentid left outer join state_master s1m on s1m.stateid=ci.stateid 
                      left outer join subcontinentmaster s2m on s2m.subcontinentid=ci.subcontinentid left outer join buyinghouse bh on bh.buyinghouseId = BuyingAgent.buyinghouseId
                      left outer join CustomerShippingAgent CSA on CSA.Customerid=CI.Customerid 
                        Left Outer JOIN Payment PayCustom ON CI.PaymentIdCustom=PayCustom.PaymentId
                        where CI.CustomerId=" + GvCustomer.SelectedValue;
        //Session["id"] = GvCustomer.SelectedValue;
        ViewState["CustomerId"] = GvCustomer.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString(); // Assigning the current customer ID.
                txtCustName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                txtCompName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                txtCustAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCustAdd2.Text = ds.Tables[0].Rows[0]["CustAdd1"].ToString();
                txtCustAddr3.Text = ds.Tables[0].Rows[0]["CustAdd2"].ToString();
                txtByerOthCons.Text = ds.Tables[0].Rows[0]["BuyerOtherThanConsigneeAir"].ToString();
                TxtByerOtherThanConsSea.Text = ds.Tables[0].Rows[0]["BuerOtherThanConsigneeSea"].ToString();
                txtMark.Text = ds.Tables[0].Rows[0]["Mark"].ToString();
                ddCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txtMob.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                txtPin.Text = ds.Tables[0].Rows[0]["PinCode"].ToString();
                if (ddlBank.Items.FindByValue(ds.Tables[0].Rows[0]["Bankid"].ToString())!=null)
                {                    
                ddlBank.SelectedValue = ds.Tables[0].Rows[0]["Bankid"].ToString();
                }
                ddlCurrency.SelectedValue = ds.Tables[0].Rows[0]["Currencyid"].ToString();
                txtFinelDest.Text = ds.Tables[0].Rows[0]["DestinationPlace"].ToString();
                ddlReceiptPreCar.SelectedValue = ds.Tables[0].Rows[0]["RecieptAtByPreCarrier"].ToString();
                ddlPreCarr.SelectedValue = ds.Tables[0].Rows[0]["PreCarriageBy"].ToString();
                ddlByAirSea.SelectedValue = ds.Tables[0].Rows[0]["ByAirSea"].ToString();
                ddlPortOfLoading.SelectedValue = ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
                ddCountry_SelectedIndexChanged(sender, e);
                DDAirport.SelectedValue = ds.Tables[0].Rows[0]["AirPortid"].ToString();
                txtAccNo.Text = ds.Tables[0].Rows[0]["AcNo"].ToString();
                txtCustCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                txtTinNo.Text = ds.Tables[0].Rows[0]["TinNo"].ToString();
                txtNotifyByAir.Text = ds.Tables[0].Rows[0]["NotifyByAir"].ToString();
                DDSeaport.SelectedValue = ds.Tables[0].Rows[0]["SeaPortid"].ToString();
                txtNotifyBySea.Text = ds.Tables[0].Rows[0]["NotifyBySea"].ToString();
                ddShipping.SelectedValue = ds.Tables[0].Rows[0]["ShippingAgent"].ToString();

                ddPaymentMode.SelectedValue = ds.Tables[0].Rows[0]["PaymentId"].ToString();
                ddPaymentModeCustom.SelectedValue = ds.Tables[0].Rows[0]["PaymentIdCustom"].ToString();
                ddDeliveryTerms.SelectedValue = ds.Tables[0].Rows[0]["TermId"].ToString();
                //ddCountry_SelectedIndexChanged(sender, e);

                ddContinent.SelectedValue = ds.Tables[0].Rows[0]["continentid"].ToString();
                ddContinent_SelectedIndexChanged(sender, e);
                DDState.SelectedValue = ds.Tables[0].Rows[0]["stateid"].ToString();

                DDSubcontinent.SelectedValue = ds.Tables[0].Rows[0]["subcontinentid"].ToString();
                DDbuyinghouse.SelectedValue = ds.Tables[0].Rows[0]["buyinghouseId"].ToString();

                txtSampleAddress.Text = ds.Tables[0].Rows[0]["SampleAddress"].ToString();
                ddPriority.SelectedValue = ds.Tables[0].Rows[0]["PriorityId"].ToString();
                ddTypeofCustomer.SelectedValue = ds.Tables[0].Rows[0]["CustomerTypeId"].ToString();
                DDBuyingHouse1.SelectedValue = ds.Tables[0].Rows[0]["BuyingHouseId"].ToString();
                txtmerchantname.Text = ds.Tables[0].Rows[0]["Merchantname"].ToString();
                txtgstin.Text = ds.Tables[0].Rows[0]["GSTIN"].ToString();
                txtConsigneeGSTIN.Text = ds.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
                txtConsigneeCountry.Text = ds.Tables[0].Rows[0]["ConsigneeCountry"].ToString();
                txtConsigneeState.Text = ds.Tables[0].Rows[0]["ConsigneeState"].ToString();
                txtConsigneeStateCode.Text = ds.Tables[0].Rows[0]["ConsigneeStateCode"].ToString();

                UtilityModule.ConditionalComboFill(ref ddBuyingAgent, "select BuyeingAgentId,BuyeingAgentName from BuyingAgent Where BuyingHouseId=" + DDBuyingHouse1.SelectedValue + "", true, "--Select--");
                ddBuyingAgent.SelectedValue = ds.Tables[0].Rows[0]["BuyingAgent"].ToString();

                DDShippingAgency.SelectedValue = ds.Tables[0].Rows[0]["ShippingAgencyId"].ToString();
                UtilityModule.ConditonalChkBoxListFill(ref ChklistShippingAgent, "select Agentid,AgentName from Shipp where AgencyId=" + DDShippingAgency.SelectedValue + " order by AgentName");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int H = 0; H < ChklistShippingAgent.Items.Count; H++)
                    {
                        if (ds.Tables[0].Rows[i]["ShippingAgentId"].ToString() == ChklistShippingAgent.Items[H].Value.ToString())
                        {
                            ChklistShippingAgent.Items[H].Selected = true;
                        }
                    }
                }


            }
            int n = Gvchklist.Rows.Count;
            int j = 0;
            DataSet DtS = null;
            DtS = SqlHelper.ExecuteDataset(con, CommandType.Text, @"SELECT Itemid FROM Lablecustomer  where CustomerId=" + GvCustomer.SelectedValue);
            j = DtS.Tables[0].Rows.Count;
            if (j > 0)
            {
                Chkvalidate.Checked = true;
                ch.Visible = true;
                int k = 0;
                for (int i = 0; i <= n - 1; i++)
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
                Chkvalidate.Checked = false;
                ch.Visible = false;
            }
            Session["ReportPath"] = "Reports/CustomerDetail.rpt";
            Session["CommanFormula"] = "{Customerinfo.CustomerId}=" + GvCustomer.SelectedValue;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
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

        cmdSave0.Text = "Update";
        btndelete.Visible = true;
    }
    protected void cmdSave_Click(object sender, EventArgs e)
    {
        Validated();
        if (lblErr.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[54];
                _arrPara[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50);
                _arrPara[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 1000);
                _arrPara[4] = new SqlParameter("@BuyerOtherThanConsignee", SqlDbType.VarChar, 400);
                _arrPara[5] = new SqlParameter("@Mark", SqlDbType.NVarChar, 50);
                _arrPara[6] = new SqlParameter("@Country", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
                _arrPara[8] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
                _arrPara[9] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
                _arrPara[10] = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
                _arrPara[11] = new SqlParameter("@PinCode", SqlDbType.NVarChar, 50);
                _arrPara[12] = new SqlParameter("@BankId", SqlDbType.Int);
                _arrPara[13] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                _arrPara[14] = new SqlParameter("@DestinationPlace", SqlDbType.NVarChar, 50);
                _arrPara[15] = new SqlParameter("@RecieptAtByPreCarrier", SqlDbType.Int);
                _arrPara[16] = new SqlParameter("@PreCarriageBy", SqlDbType.Int);
                _arrPara[17] = new SqlParameter("@ByAirSea", SqlDbType.Int);
                _arrPara[18] = new SqlParameter("@PortOfLoading", SqlDbType.Int);
                _arrPara[19] = new SqlParameter("@AirPort", SqlDbType.NVarChar, 50);
                _arrPara[20] = new SqlParameter("@AcNo", SqlDbType.NVarChar, 50);
                _arrPara[21] = new SqlParameter("@CustomerCode", SqlDbType.NVarChar, 50);
                _arrPara[22] = new SqlParameter("@TinNo", SqlDbType.NVarChar, 50);
                _arrPara[23] = new SqlParameter("@NotifybyAir", SqlDbType.NVarChar, 300);
                _arrPara[24] = new SqlParameter("@SeaPort", SqlDbType.NVarChar, 100);
                _arrPara[25] = new SqlParameter("@NotifybySea", SqlDbType.NVarChar, 250);
                _arrPara[26] = new SqlParameter("@ShippingAgent", SqlDbType.Int);
                _arrPara[27] = new SqlParameter("@BuyingAgent", SqlDbType.Int);
                _arrPara[28] = new SqlParameter("@Id", SqlDbType.Int);
                _arrPara[29] = new SqlParameter("@BuerOtherThanConsigneeSea", SqlDbType.NVarChar, 100);
                _arrPara[30] = new SqlParameter("@PaymentId", SqlDbType.Int);
                _arrPara[31] = new SqlParameter("@TermId", SqlDbType.Int);
                _arrPara[32] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[33] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[34] = new SqlParameter("@stateid", SqlDbType.Int);
                _arrPara[35] = new SqlParameter("@Continentid", SqlDbType.Int);
                _arrPara[36] = new SqlParameter("@subcontinentid", SqlDbType.Int);
                _arrPara[37] = new SqlParameter("@Seaportid", SqlDbType.Int);
                _arrPara[38] = new SqlParameter("@Airportid", SqlDbType.Int);

                _arrPara[39] = new SqlParameter("@buyinghouseId", SqlDbType.VarChar, 50);
                _arrPara[40] = new SqlParameter("@SampleAddress", SqlDbType.VarChar, 250);
                _arrPara[41] = new SqlParameter("@Priority", SqlDbType.Int);
                _arrPara[42] = new SqlParameter("@TYpeofCustomer", SqlDbType.Int);
                _arrPara[43] = new SqlParameter("@ShippingAgencyId", SqlDbType.Int);
                _arrPara[44] = new SqlParameter("@ShippingAgentId", SqlDbType.VarChar, 50);
                _arrPara[45] = new SqlParameter("@Address2", SqlDbType.VarChar, 250);
                _arrPara[46] = new SqlParameter("@Address3", SqlDbType.VarChar, 250);
                _arrPara[47] = new SqlParameter("@MerchantName", SqlDbType.VarChar, 100);
                _arrPara[48] = new SqlParameter("@GSTIN", SqlDbType.VarChar, 30);
                _arrPara[49] = new SqlParameter("@ConsigneeGSTIN", SqlDbType.VarChar, 20);
                _arrPara[50] = new SqlParameter("@ConsigneeCountry", SqlDbType.VarChar, 20);
                _arrPara[51] = new SqlParameter("@ConsigneeState", SqlDbType.VarChar, 20);
                _arrPara[52] = new SqlParameter("@ConsigneeStateCode", SqlDbType.VarChar, 5);
                _arrPara[53] = new SqlParameter("@PaymentIdCustom", SqlDbType.Int);



                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtCustName.Text.ToUpper();
                _arrPara[2].Value = txtCompName.Text.ToUpper();
                _arrPara[3].Value = txtCustAddress.Text.ToUpper();
                _arrPara[4].Value = txtByerOthCons.Text.ToUpper();
                _arrPara[5].Value = txtMark.Text.ToUpper();
                _arrPara[6].Value = ddCountry.SelectedIndex < 0 ? DBNull.Value : (object)ddCountry.SelectedValue; ;
                _arrPara[7].Value = txtPhone.Text.ToUpper();
                _arrPara[8].Value = txtMob.Text.ToUpper();
                _arrPara[9].Value = txtEmail.Text;
                _arrPara[10].Value = txtFax.Text.ToUpper();
                _arrPara[11].Value = txtPin.Text.ToUpper();
                _arrPara[12].Value = ddlBank.SelectedIndex < 0 ? "0" : ddlBank.SelectedValue;
                _arrPara[13].Value = ddlCurrency.SelectedIndex < 0 ? "0" : ddlCurrency.SelectedValue;
                _arrPara[14].Value = txtFinelDest.Text.ToUpper();
                _arrPara[15].Value = ddlReceiptPreCar.SelectedIndex < 0 ? "0" : ddlReceiptPreCar.SelectedValue;
                _arrPara[16].Value = ddlPreCarr.SelectedIndex < 0 ? "0" : ddlPreCarr.SelectedValue;
                _arrPara[17].Value = ddlByAirSea.SelectedIndex < 0 ? "0" : ddlByAirSea.SelectedValue;
                _arrPara[18].Value = ddlPortOfLoading.SelectedIndex < 0 ? "0" : ddlPortOfLoading.SelectedValue;
                _arrPara[19].Value = DDAirport.SelectedIndex > 0 ? DDAirport.SelectedItem.Text : "0";
                _arrPara[20].Value = txtAccNo.Text.ToUpper();
                _arrPara[21].Value = txtCustCode.Text.ToUpper();
                _arrPara[22].Value = txtTinNo.Text.ToUpper();
                _arrPara[23].Value = txtNotifyByAir.Text.ToUpper();
                _arrPara[24].Value = DDSeaport.SelectedIndex > 0 ? DDSeaport.SelectedItem.Text : "0";
                _arrPara[25].Value = txtNotifyBySea.Text.ToUpper();
                _arrPara[26].Value = ddShipping.SelectedIndex > 0 ? ddShipping.SelectedValue : "0";
                _arrPara[27].Value = ddBuyingAgent.SelectedIndex > 0 ? ddBuyingAgent.SelectedValue : "0";
                _arrPara[28].Direction = ParameterDirection.Output;
                _arrPara[29].Value = TxtByerOtherThanConsSea.Text.ToUpper();
                _arrPara[30].Value = ddPaymentMode.SelectedIndex > 0 ? ddPaymentMode.SelectedValue : "0";
                _arrPara[31].Value = ddDeliveryTerms.SelectedIndex > 0 ? ddDeliveryTerms.SelectedValue : "0";
                _arrPara[32].Value = Session["varuserid"].ToString();
                _arrPara[33].Value = Session["varMasterCompanyIDForERP"].ToString();
                //_arrPara[34].Value = DDState.SelectedIndex > 0 ?DDState.SelectedValue:"NULL";
                _arrPara[34].Value = DDState.SelectedIndex > 0 ? DDState.SelectedValue : "0";
                _arrPara[35].Value = ddContinent.SelectedIndex < 0 ? "0" : ddContinent.SelectedValue; ;
                _arrPara[36].Value = DDSubcontinent.SelectedIndex > 0 ? DDSubcontinent.SelectedValue : "0";
                _arrPara[37].Value = DDSeaport.SelectedIndex < 0 ? "0" : DDSeaport.SelectedValue; ;
                _arrPara[38].Value = DDAirport.SelectedIndex < 0 ? "0" : DDAirport.SelectedValue;
                _arrPara[39].Value = DDbuyinghouse.SelectedValue.ToUpper();

                _arrPara[40].Value = txtSampleAddress.Text;
                _arrPara[41].Value = ddPriority.SelectedValue;
                _arrPara[42].Value = ddTypeofCustomer.SelectedIndex <= 0 ? "0" : ddTypeofCustomer.SelectedValue;
                _arrPara[43].Value = DDShippingAgency.SelectedIndex <= 0 ? "0" : DDShippingAgency.SelectedValue;
                //ShippingAgentId
                String str = "";
                for (int i = 0; i < ChklistShippingAgent.Items.Count; i++)
                {
                    if (ChklistShippingAgent.Items[i].Selected)
                    {
                        if (str == "")
                        {
                            str = ChklistShippingAgent.Items[i].Value;
                        }
                        else
                        {
                            str = str + "," + ChklistShippingAgent.Items[i].Value;
                        }
                    }
                }
                //Buying Agent Id

                _arrPara[44].Value = str;
                _arrPara[45].Value = txtCustAdd2.Text;
                _arrPara[46].Value = txtCustAddr3.Text;
                _arrPara[47].Value = txtmerchantname.Text;
                _arrPara[48].Value = txtgstin.Text;
                _arrPara[49].Value = txtConsigneeGSTIN.Text;
                _arrPara[50].Value = txtConsigneeCountry.Text;
                _arrPara[51].Value = txtConsigneeState.Text;
                _arrPara[52].Value = txtConsigneeStateCode.Text;
                _arrPara[53].Value = ddPaymentModeCustom.SelectedIndex > 0 ? ddPaymentModeCustom.SelectedValue : "0";

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_customerinfo", _arrPara);
                if (Chkvalidate.Checked == true)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from Lablecustomer where customerid=" + _arrPara[28].Value);
                    insertvalue_Lablecustomer(Convert.ToInt32(_arrPara[28].Value), Tran);
                }
                lblErr.Visible = true;
                lblErr.Text = "Data Saved Sucessfully";
                cmdSave0.Text = "Save";
                Tran.Commit();

                Fill_chelist();
                refresh_form();
                Fill_Grid();
                btndelete.Visible = false;
            }

            catch (Exception ex)
            {
                lblErr.Visible = true;
                lblErr.Text = ex.Message;
                Tran.Rollback();
                Logs.WriteErrorLog("Masters_Campany_frmCustomer|cmdSave_Click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
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

        }
        else
        {
            if (lblErr.Text == "Customer already exists............")
            {
                lblErr.Visible = true;
                lblErr.Text = "Customer already exists............";
            }
            else
            {
                lblErr.Visible = true;
                lblErr.Text = "Please Enter the Details some * fields are missing.......";
            }
        }
        //cmdSave0.Text = "Save";
        //btndelete.Visible = false;

    }

    private void refresh_form()
    {
        txtid.Text = "0";
        txtCustCode.Text = "";
        txtCompName.Text = "";
        txtCustName.Text = "";
        txtPin.Text = "";
        txtCustAddress.Text = "";
        txtPhone.Text = "";
        txtMob.Text = "";
        txtFax.Text = "";
        ddCountry.SelectedIndex = -1;
        txtEmail.Text = "";
        txtMark.Text = "";
        txtFinelDest.Text = "";
        ddlCurrency.SelectedIndex = -1;
        DDSeaport.SelectedIndex = -1;
        ddlBank.SelectedIndex = -1;
        DDAirport.SelectedIndex = -1;
        ddlReceiptPreCar.SelectedIndex = -1;
        txtAccNo.Text = "";
        ddlPreCarr.SelectedIndex = -1;
        ddlByAirSea.SelectedIndex = -1;
        txtByerOthCons.Text = "";
        TxtByerOtherThanConsSea.Text = "";
        ddlPortOfLoading.SelectedIndex = -1;
        txtTinNo.Text = "";
        txtNotifyBySea.Text = "";
        txtNotifyByAir.Text = "";
        ddShipping.SelectedIndex = -1;
        ddBuyingAgent.SelectedIndex = -1;
        ddContinent.SelectedIndex = -1;
        DDSubcontinent.SelectedIndex = -1;
        DDState.SelectedIndex = -1;
        DDbuyinghouse.SelectedIndex = -1;
        Chkvalidate.Checked = false;
        ch.Visible = false;
        cmdSave0.Text = "Save";
        btndelete.Visible = false;
        txtCustAdd2.Text = "";
        txtCustAddr3.Text = "";
        txtmerchantname.Text = "";
        txtgstin.Text = "";
        txtConsigneeGSTIN.Text = "";
        txtConsigneeCountry.Text = "";
        txtConsigneeState.Text = "";
        txtConsigneeStateCode.Text = "";


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        fill_ddl();
        Fill_Grid();
    }
    protected void GvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomer.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlBank, "Select Bankid,BankName from Bank where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BankName");
    }
    protected void country_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCountry, "select CountryId,CountryName from CountryMaster where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CountryName", true, "--Select--");
    }
    protected void currency_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlCurrency, "Select CurrencyId,CurrencyName from currencyinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CurrencyName");
    }
    protected void cariage_Click(object sender, EventArgs e)
    {
        # region  on 29-Nov-2012
        //CommanFunction.FillCombo(ddlReceiptPreCar, "Select GoodsReceiptId, StationName from GoodsReceipt order by StationName");
        //CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt order by StationName");
        #endregion
        string str = "Select GoodsReceiptId, StationName from GoodsReceipt where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        CommanFunction.FillComboWithDS(ddlReceiptPreCar, ds, 0);
        CommanFunction.FillComboWithDS(ddlPortOfLoading, ds, 0);
    }
    protected void shipping0_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddShipping, "Select Agentid,Agentname from Shipp where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Agentname");
    }

    protected void transmode_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlByAirSea, "select TransModeid,TransModeName from Transmode where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TransModename");
    }

    protected void Button7_Click1(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");

    }
    protected void addbying_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddBuyingAgent, "Select BuyeingAgentid,BuyeingAgentname from BuyingAgent where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BuyeingAgentname");        
    }
    protected void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string str1 = "select countrycode,customercode from countrymaster inner join customerinfo on countryid=country where countryid=" + ddCountry.SelectedValue;

            con.Open();
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str1);
            int n = ds.Tables[0].Rows.Count;
            if (n == 0)
            {
                string countrycode = SqlHelper.ExecuteScalar(con, CommandType.Text, "select CountryCode from CountryMaster where Countryid=" + ddCountry.SelectedValue).ToString();
                txtCustCode.Text = countrycode + "000" + (n + 1);
            }
            else
            {
                txtCustCode.Text = ds.Tables[0].Rows[0]["countrycode"] + "000" + (n + 1);
            }

            UtilityModule.ConditionalComboFill(ref DDState, "select Stateid,Statename from state_master  where countryid=" + ddCountry.SelectedValue + " order by StateName", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDSeaport, "select SeaPortId,SeaPortName from seaport where countryid=" + ddCountry.SelectedValue + "order by SeaportName", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDAirport, "select AirPortId,AirPortName from Airport where countryid=" + ddCountry.SelectedValue + "order by AirPortName", true, "--Select--");
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }


    protected void Rpt_Click(object sender, EventArgs e)
    {
        //Session["ReportPath"] = "Reports/CustomerDetail.rpt";
        //Session["CommanFormula"] = "";
        Report();
    }
    private void Report()
    {
        string qry = @" SELECT CustomerName,CompanyName,Address,PhoneNo,CustomerCode,Email,DestinationPlace,CustAdd1,CustAdd2 FROM    customerinfo where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddCountry.SelectedIndex > 0)
        {
            qry = qry + "  And Country=" + ddCountry.SelectedValue + "";
        }
        if (ddContinent.SelectedIndex > 0)
        {
            qry = qry + "  And Continentid=" + ddContinent.SelectedValue + "";
        }
        if (DDSubcontinent.SelectedIndex > 0)
        {
            qry = qry + "  And subcontinentid=" + DDSubcontinent.SelectedValue + "";
        }
        if (DDState.SelectedIndex > 0)
        {
            qry = qry + "  And StateId=" + DDState.SelectedValue + "";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\CustomerDetailNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\CustomerDetailNew.xsd";
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
    protected void Chkvalidate_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkvalidate.Checked == true)
        {
            ch.Visible = true;
        }
        else
        {
            ch.Visible = false;
        }
    }
    private void Fill_chelist()
    {
        Gvchklist.DataSource = fill_gird_chk();
        Gvchklist.DataBind();
    }
    private DataSet fill_gird_chk()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select item_id,category_name,item_name from item_category_master
                    inner join item_master on item_category_master.category_id = item_master.category_id  where category_name ='ACCESSORIES ITEM' ANd  item_category_master.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
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
    public void insertvalue_Lablecustomer(int Customerid, SqlTransaction Tran)
    {
        int itemid = 0;
        int n = Gvchklist.Rows.Count;
        SqlParameter[] _arrPara = new SqlParameter[2];
        _arrPara[0] = new SqlParameter("@itemid", SqlDbType.Int);
        _arrPara[1] = new SqlParameter("@customerid", SqlDbType.Int);

        _arrPara[1].Value = Customerid;
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
            _arrPara[0].Value = itemid;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Lablecustomer", _arrPara);
        }

    }
    protected void PreCarriageBy_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlPreCarr, "select carriageid,carriageName from Carriage where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by carriageName");
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (cmdSave0.Text == "Update")
            {
                strsql = "select CustomerName from customerinfo where CustomerId!='" + ViewState["CustomerId"].ToString() + "' and CompanyName='" + txtCompName.Text + "' And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select CustomerName from customerinfo where CompanyName='" + txtCompName.Text + "' And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblErr.Visible = true;
                lblErr.Text = "Customer already exists............";
                txtCustName.Text = "";
                txtCustName.Focus();
            }
            else
            {
                lblErr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
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
            _array[0] = new SqlParameter("@CustomerId", ViewState["CustomerId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteCustomer", _array);
            lblErr.Visible = true;
            lblErr.Text = _array[1].Value.ToString();
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
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
        cmdSave0.Text = "Save";
        refresh_form();

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void BtnRefPaymentMode_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddPaymentMode, "Select PaymentId,PaymentName from Payment where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by PaymentName", true, "--Select--");
    }
    protected void BtnRefPaymentModeCustom_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddPaymentModeCustom, "Select PaymentId,PaymentName from Payment where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by PaymentName", true, "--Select--");
    }
    protected void BtnRefDeliveryTerms_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TermName", true, "--Select--");
    }
    protected void GvCustomer_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        // if (e.Row.RowType == DataControlRowType.Header)
        // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        // if (e.Row.RowType == DataControlRowType.DataRow &&
        //   e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
    protected void Gvchklist_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        // if (e.Row.RowType == DataControlRowType.Header)
        // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //  e.Row.RowState == DataControlRowState.Alternate)
        //  e.Row.CssClass = "alternate";
    }



    protected void ddContinent_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string str1 = "select continentcode,customercode from continentmaster cm inner join customerinfo ci on cm.continentid=ci.continentid where ci.continentid=" + ddContinent.SelectedValue;

            con.Open();
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str1);
            //int n = ds.Tables[0].Rows.Count;
            //if (n == 0)
            //{
            //    string continentcode = SqlHelper.ExecuteScalar(con, CommandType.Text, "select ContinentCode from ContinentMaster where Continentid=" + ddContinent.SelectedValue).ToString();
            //    txtCustCode.Text = continentcode + "000" + (n + 1);
            //}
            //else
            //{
            //    txtCustCode.Text = ds.Tables[0].Rows[0]["continentcode"] + "000" + (n + 1);
            //}

            UtilityModule.ConditionalComboFill(ref DDSubcontinent, "select SubcontinentID,SubcontinentName from Subcontinentmaster  where continentid=" + ddContinent.SelectedValue + " order by subcontinentName", true, "--Select--");
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCustomer.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }

    protected void Continent_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddContinent, "select continentid,continentname from continentmaster Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by continentname", true, "--select--");

    }
    protected void Subcontinent_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDSubcontinent, "select SubcontinentID,SubcontinentName from Subcontinentmaster  where continentid=" + ddContinent.SelectedValue + " order by subcontinentName", true, "--Select--");

    }
    protected void State_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDState, "select Stateid,Statename from state_master  where countryid=" + ddCountry.SelectedValue + " order by StateName", true, "--Select--");

    }

    protected void seaport2_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDSeaport, "select SeaPortId,SeaPortName from seaport where countryid=" + ddCountry.SelectedValue + "order by SeaportName", true, "--Select--");

    }
    protected void Airport2_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDAirport, "select AirPortId,AirPortName from Airport where countryid=" + ddCountry.SelectedValue + "order by AirPortName", true, "--Select--");

    }
    protected void DDShippingAgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditonalChkBoxListFill(ref ChklistShippingAgent, "select Agentid,AgentName from Shipp where AgencyId=" + DDShippingAgency.SelectedValue + " order by AgentName");
    }

    protected void btnCustomerType_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddTypeofCustomer, "select CUstomerTypeId,TypeName from CustomerType order by TypeName", true, "--Select--");
    }
    protected void DDBuyingHouse1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddBuyingAgent, "select BuyeingAgentId,BuyeingAgentName from BuyingAgent Where BuyingHouseId=" + DDBuyingHouse1.SelectedValue + "", true, "--Select--");
    }
    protected void DDSubcontinent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void DDState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnAgencyCloseFormCustomer_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDShippingAgency, "select AgencyId,AgencyName from ShippingAgency Order by AgencyName", true, "--Plz Select--");
    }
    protected void btnbuyinghouseCloseFormCustomer_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDBuyingHouse1, "select buyinghouseid,Name_buying_house from buyinghouse Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"order by Name_buying_house", true, "--select--");
    }
}







