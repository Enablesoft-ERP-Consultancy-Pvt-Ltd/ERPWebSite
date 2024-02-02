using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddCustomer : CustomPage
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
                            CI.TinNo, CI.NotifyByAir, CI.SeaPort, CI.NotifyBySea, Shipp.AgentName, BuyingAgent.BuyeingAgentName,Payment.PaymentName,Term.TermName
                            FROM BuyingAgent Right Outer JOIN
                            customerinfo AS CI INNER JOIN
                            CountryMaster ON CI.Country=CountryMaster.CountryId Left Outer JOIN
                            Bank ON CI.BankId=Bank.BankId Left Outer JOIN
                            CurrencyInfo ON CI.CurrencyId=CurrencyInfo.CurrencyId Left Outer JOIN
                            TransMode ON CI.ByAirSea=TransMode.transmodeId Left Outer JOIN
                            GoodsReceipt ON CI.PortOfLoading=GoodsReceipt.GoodsreceiptId AND 
                            CI.RecieptAtByPreCarrier=GoodsReceipt.GoodsreceiptId Left Outer JOIN
                            Shipp ON CI.ShippingAgent=Shipp.AgentId ON BuyingAgent.BuyeingAgentId=CI.BuyingAgent Left Outer JOIN
                            Carriage ON CI.PreCarriageBy=Carriage.CarriageId Left Outer JOIN Payment ON CI.PaymentId=Payment.PaymentId
					        Left Outer JOIN Term ON CI.TermId=Term.TermId Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                            ORDER BY CI.CustomerId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmCustomer|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
        CommanFunction.FillCombo(ddlCurrency, "Select CurrencyId,CurrencyName from currencyinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CurrencyName");
        CommanFunction.FillCombo(ddlBank, "Select Bankid,BankName from Bank Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BankName");
        CommanFunction.FillCombo(ddlReceiptPreCar, "Select GoodsReceiptId, StationName from GoodsReceipt where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");
        CommanFunction.FillCombo(ddlPreCarr, "select carriageid,carriageName from Carriage Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by carriageName");
        CommanFunction.FillCombo(ddlByAirSea, "select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TransModename");
        CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");
        UtilityModule.ConditionalComboFill(ref ddShipping, "Select Agentid,Agentname from Shipp Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Agentname", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref ddBuyingAgent, "Select BuyeingAgentid,BuyeingAgentname from BuyingAgent Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BuyeingAgentname", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref ddCountry, "select CountryId,CountryName from CountryMaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CountryName", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref ddPaymentMode, "Select PaymentId,PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by PaymentName", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TermName", true, "--Select--");
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
                      CountryMaster.CountryId, CurrencyInfo.CurrencyId, TransMode.transmodeId, Carriage.CarriageId, BuyingAgent.BuyeingAgentId, 
                      Bank.BankId, CI.RecieptAtByPreCarrier, CI.PreCarriageBy, CI.PortOfLoading, CI.ByAirSea, CI.Country, CI.BankId AS Expr1, CI.CurrencyId AS Expr2, 
                      CI.ShippingAgent, CI.BuyingAgent,Payment.PaymentId,Term.TermId
                      FROM BuyingAgent Right Outer JOIN
                      customerinfo AS CI INNER JOIN
                      CountryMaster ON CI.Country=CountryMaster.CountryId Left Outer JOIN
                      Bank ON CI.BankId=Bank.BankId Left Outer JOIN
                      CurrencyInfo ON CI.CurrencyId=CurrencyInfo.CurrencyId Left Outer JOIN
                      TransMode ON CI.ByAirSea=TransMode.transmodeId Left Outer JOIN
                      GoodsReceipt ON CI.PortOfLoading=GoodsReceipt.GoodsreceiptId AND 
                      CI.RecieptAtByPreCarrier=GoodsReceipt.GoodsreceiptId Left Outer JOIN
                      Shipp ON CI.ShippingAgent=Shipp.AgentId ON BuyingAgent.BuyeingAgentId=CI.BuyingAgent Left Outer JOIN
                      Carriage ON CI.PreCarriageBy=Carriage.CarriageId Left Outer JOIN Payment ON CI.PaymentId=Payment.PaymentId
                      Left Outer JOIN Term ON CI.TermId=Term.TermId
                      where CI.CustomerId=" + GvCustomer.SelectedValue;
        //Session["id"] = GvCustomer.SelectedValue;
        ViewState["id"] = GvCustomer.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                 txtid.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString(); // Assigning the current customer ID.
                txtCustName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                txtCompName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                txtCustAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtByerOthCons.Text = ds.Tables[0].Rows[0]["BuyerOtherThanConsigneeAir"].ToString();
                TxtByerOtherThanConsSea.Text = ds.Tables[0].Rows[0]["BuerOtherThanConsigneeSea"].ToString();
                txtMark.Text = ds.Tables[0].Rows[0]["Mark"].ToString();
                ddCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txtMob.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                txtPin.Text = ds.Tables[0].Rows[0]["PinCode"].ToString();
                ddlBank.SelectedValue = ds.Tables[0].Rows[0]["Bankid"].ToString();
                ddlCurrency.SelectedValue = ds.Tables[0].Rows[0]["Currencyid"].ToString();
                txtFinelDest.Text = ds.Tables[0].Rows[0]["DestinationPlace"].ToString();
                ddlReceiptPreCar.SelectedValue = ds.Tables[0].Rows[0]["RecieptAtByPreCarrier"].ToString();
                ddlPreCarr.SelectedValue = ds.Tables[0].Rows[0]["PreCarriageBy"].ToString();
                ddlByAirSea.SelectedValue = ds.Tables[0].Rows[0]["ByAirSea"].ToString();
                ddlPortOfLoading.SelectedValue = ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
                txtAirPort.Text = ds.Tables[0].Rows[0]["AirPort"].ToString();
                txtAccNo.Text = ds.Tables[0].Rows[0]["AcNo"].ToString();
                txtCustCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                txtTinNo.Text = ds.Tables[0].Rows[0]["TinNo"].ToString();
                txtNotifyByAir.Text = ds.Tables[0].Rows[0]["NotifyByAir"].ToString();
                txtSeaPort.Text = ds.Tables[0].Rows[0]["SeaPort"].ToString();
                txtNotifyBySea.Text = ds.Tables[0].Rows[0]["NotifyBySea"].ToString();
                ddShipping.SelectedValue = ds.Tables[0].Rows[0]["ShippingAgent"].ToString();
                ddBuyingAgent.SelectedValue = ds.Tables[0].Rows[0]["BuyingAgent"].ToString();
                ddPaymentMode.SelectedValue = ds.Tables[0].Rows[0]["PaymentId"].ToString();
                ddDeliveryTerms.SelectedValue = ds.Tables[0].Rows[0]["TermId"].ToString();
            }
            int n = Gvchklist.Rows.Count;
            int j=0;
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
            Session["CommanFormula"] = "{Customerinfo.CustomerId}="+GvCustomer.SelectedValue;
            }
       catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
       
        cmdSave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void cmdSave_Click(object sender, EventArgs e)
    {
            Validated();
            if (lblErr.Text == "")
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[34];
                    _arrPara[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50);
                    _arrPara[2] = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50);
                    _arrPara[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 250);
                    _arrPara[4] = new SqlParameter("@BuyerOtherThanConsignee", SqlDbType.NVarChar, 250);
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

                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtCustName.Text.ToUpper();
                    _arrPara[2].Value = txtCompName.Text.ToUpper();
                    _arrPara[3].Value = txtCustAddress.Text.ToUpper();
                    _arrPara[4].Value = txtByerOthCons.Text.ToUpper();
                    _arrPara[5].Value = txtMark.Text.ToUpper();
                    _arrPara[6].Value = ddCountry.SelectedValue;
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
                    _arrPara[19].Value = txtAirPort.Text.ToUpper();
                    _arrPara[20].Value = txtAccNo.Text.ToUpper();
                    _arrPara[21].Value = txtCustCode.Text.ToUpper();
                    _arrPara[22].Value = txtTinNo.Text.ToUpper();
                    _arrPara[23].Value = txtNotifyByAir.Text.ToUpper();
                    _arrPara[24].Value = txtSeaPort.Text.ToUpper();
                    _arrPara[25].Value = txtNotifyBySea.Text.ToUpper();
                    _arrPara[26].Value = ddShipping.SelectedIndex > 0 ? ddShipping.SelectedValue : "0";
                    _arrPara[27].Value = ddBuyingAgent.SelectedIndex > 0 ? ddBuyingAgent.SelectedValue : "0";
                    _arrPara[28].Direction = ParameterDirection.Output;
                    _arrPara[29].Value = TxtByerOtherThanConsSea.Text.ToUpper();
                    _arrPara[30].Value = ddPaymentMode.SelectedIndex > 0 ? ddPaymentMode.SelectedValue : "0";
                    _arrPara[31].Value = ddDeliveryTerms.SelectedIndex > 0 ? ddDeliveryTerms.SelectedValue : "0";
                    _arrPara[32].Value = Session["varuserid"].ToString();
                        _arrPara[33].Value= Session["varMasterCompanyIDForERP"].ToString();
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_customerinfo", _arrPara);
                    if (Chkvalidate.Checked == true)
                    {
                        SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete from Lablecustomer where customerid=" + _arrPara[28].Value);
                        insertvalue_Lablecustomer(Convert.ToInt32(_arrPara[28].Value));
                    }
                    lblErr.Visible = true;
                    Fill_Grid();
                    Fill_chelist();
                    refresh_form();
                    lblErr.Text = "Data Saved Sucessfully";
                    cmdSave.Text = "Save";
                    btndelete.Visible = false;
                }

                catch (Exception ex)
                {
                    lblErr.Text = ex.Message;
                    Logs.WriteErrorLog("Masters_Campany_frmCustomer|cmdSave_Click|" + ex.Message);
                    UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
                lblErr.Visible = true;
                lblErr.Text = "Save Details............";
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
    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        refresh_form();
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
        txtSeaPort.Text = "";
        ddlBank.SelectedIndex = -1;
        txtAirPort.Text = "";
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
        Chkvalidate.Checked = false;
        ch.Visible = false;
        cmdSave.Text = "Save";
        btndelete.Visible = false;
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
        CommanFunction.FillCombo(ddlBank, "Select Bankid,BankName from Bank Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BankName");
    }
    protected void country_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCountry, "select CountryId,CountryName from CountryMaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CountryName", true, "--Select--");
    }
    protected void currency_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlCurrency, "Select CurrencyId,CurrencyName from currencyinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CurrencyName");
    }
    protected void cariage_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlReceiptPreCar, "Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");
        CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");
    }
    protected void shipping0_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddShipping, "Select Agentid,Agentname from Shipp Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Agentname");
    }
  
    protected void transmode_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlByAirSea, "select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TransModename");
    }
  
    protected void Button7_Click1(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddlPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by StationName");
       
    }
    protected void addbying_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(ddBuyingAgent, "Select BuyeingAgentid,BuyeingAgentname from BuyingAgent Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by BuyeingAgentname");
    }
    protected void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string str1 = "select countrycode,customercode from countrymaster inner join customerinfo on countryid=country where customerinfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And countryid=" + ddCountry.SelectedValue;
            con.Open();
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str1);
            int n = ds.Tables[0].Rows.Count;
            if (n == 0)
            {
                string countrycode = SqlHelper.ExecuteScalar(con, CommandType.Text, "select CountryCode from CountryMaster where Countryid=" + ddCountry.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "").ToString();
                txtCustCode.Text = countrycode + "000" + (n + 1);
            }
            else
            {
                txtCustCode.Text = ds.Tables[0].Rows[0]["countrycode"] + "000" + (n + 1);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Rpt_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/CustomerDetail.rpt";
        Session["CommanFormula"] = "";
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
                    inner join item_master on item_category_master.category_id = item_master.category_id  where category_name ='ACCESSORIES ITEM' And itemmaster.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
    public void insertvalue_Lablecustomer(int Customerid)
    {
        int itemid=0;
        int n = Gvchklist.Rows.Count;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         con.Open();
         try
         {
           SqlParameter[] _arrPara = new SqlParameter[2];
           _arrPara[0] = new SqlParameter("@itemid", SqlDbType.Int);
           _arrPara[1] = new SqlParameter("@customerid", SqlDbType.Int);

            _arrPara[1].Value =Customerid;
        for (int i = 0; i < n; i++)
        {
            GridViewRow row = Gvchklist.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;
            if (isChecked==true)
            {
                 itemid = Convert.ToInt32(Gvchklist.Rows[i].Cells[3].Text);
            }
            else{
                itemid = 0;
            }
            _arrPara[0].Value = itemid;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Lablecustomer", _arrPara);
            }
         }
     catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
            if (cmdSave.Text == "Update")
            {
                strsql = "select CustomerName from customerinfo where CustomerId!='" + ViewState["id"].ToString() + "' and CompanyName='" + txtCompName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select CustomerName from customerinfo where CompanyName='" + txtCompName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
           con.Open();
           try
           {
               int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CustomerId from OrderMaster where CustomerId=" + ViewState["id"].ToString()));
               if (id <= 0)
               {
                   SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from customerinfo where CustomerId=" + ViewState["id"].ToString());
                   DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                   SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'customerinfo'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                   lblErr.Visible = true;
                   lblErr.Text = "Value Deleted...";
               }
               else
               {
                   lblErr.Visible = true;
                   lblErr.Text = "Value in Use...";
               }
           }
           catch (Exception ex)
           {
               UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
           cmdSave.Text = "Save";
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
    protected void BtnRefDeliveryTerms_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by TermName", true, "--Select--");
    }
    protected void GvCustomer_RowCreated(object sender, GridViewRowEventArgs e)
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
}
