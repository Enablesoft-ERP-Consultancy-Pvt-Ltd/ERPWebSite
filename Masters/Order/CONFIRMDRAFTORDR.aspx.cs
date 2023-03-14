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
public partial class Masters_Order_DRAFTORDRNEW : System.Web.UI.Page
{
    public string CustomerName = "", CustomerMobile = "", CustomerEmailId = "", CustomerOrder = "0", CompanyName = "",
                  ProductName = "", TotalQty = "0", TotalAmount = "0", CurrencyName = "", DeliveryDate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        // DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["orderid"] = 0;
            ViewState["OrderDetailId"] = 0;
            ViewState["DraftOrderNo"] = 0;
            logo();
            SqlParameter[] _array = new SqlParameter[2];
            _array[0] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            _array[1] = new SqlParameter("@VarUserId", Session["varuserId"]);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillCompany_CustomerCode", _array);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--SELECT--");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["FunctionType"] = 0;
            trReferenceImage.Visible = false;
            switch (Session["varCompanyNo"].ToString())
            {
                case "1":
                    rdoUnitWise.Checked = true;
                    break;
                case "2":
                    rdoPcWise.Checked = true;
                    break;
                default:
                    #region on 27-Nov-2012
                    //                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICE1");
                    //                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"CREATE VIEW VIEW_PERFORMAINVOICE1 AS SELECT OM.COMPANYID,OM.CUSTOMERID,
                    //                    OM.LocalOrder ORDERNO,OM.ORDERDATE,OM.STATUS,OM.CustomerOrderNo CustOrderNo,OM.DispatchDate DeliveryDate,T.TermName,P.PaymentName,TM.TransModeName,
                    //                    GR.StationName,OM.SeaPort,OrderDetailId,OD.OrderId,Item_Finished_Id,QtyRequired Qty,UnitRate Rate,TotalArea Area,Amount,OD.CurrencyId,
                    //                    DiscountAmount DisAmount,Warehouseid,CancelQty,HoldQty,0 QualityCodeId,OD.Remarks,Photo,Weight,OURCODE,BUYERCODE,PKGInstruction,LBGInstruction,
                    //                    '' DESCRIPTION,LessAdvance,LessCommission,LessDiscount,OrderCalType,OrderUnitId UnitId,CI.CurrencyName,OM.Remarks DeliveryComments,
                    //                    OM.LocalOrder localorder FROM ORDERMASTER OM,ORDERDETAIL OD,Term T,Payment P,Transmode TM,GoodsReceipt GR,CurrencyInfo CI WHERE OM.ORDERID=OD.ORDERID AND 
                    //                    OM.Termid=T.TermId And OM.PaymentId=P.PaymentId And OM.ByAirSea=TM.TransmodeId And OM.PortOfLoading=GR.GoodsReceiptId and CI.CurrencyId=Od.CurrencyId");
                    #endregion
                    rdoPcWise.Checked = true;
                    trReferenceImage.Visible = true;
                    break;
            }
        }
        //show edit button
        if (Session["canedit"].ToString() == "0") //non authenticated person
        {
            ChkEditOrder.Enabled = false;
        }
    }
    protected void DDCustOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_CustomerOrderNoSelectedValue();
    }
    private void Fill_CustomerOrderNoSelectedValue()
    {
        ViewState["DraftOrderNo"] = DDCustOrderNo.SelectedValue;
        if (ChkEditOrder.Checked == true)
        {
            ViewState["orderid"] = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Orderid from OrderMaster Where DraftOrderId=" + DDCustOrderNo.SelectedValue + "");
        }
        getOrderDetail();
        Fill_Grid();
        //Report_Type();
    }
    private void getOrderDetail()
    {

        string str = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (ChkEditOrder.Checked == true)
        {
            str = @"SELECT DISTINCT DM.DRAFTORDERID Orderid,OrderUnitId UnitId,OrderCalType,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,
            CustomerOrderNo CustOrderNo,replace(convert(varchar(11),DM.DispatchDate,106), ' ','-') as DeliveryDate,LocalOrder OrderNo,TermId,PaymentId,ByAirSea,PortOfLoading,
            SeaPort From ORDERMASTER DM,ORDERDETAIL DD where DM.OrderId=DD.OrderId And DM.DRAFTORDERID=" + DDCustOrderNo.SelectedValue;
        }
        else
        {
            str = @"SELECT DISTINCT DM.Orderid,UnitId,OrderCalType,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,CustOrderNo,replace(convert(varchar(11),
            DeliveryDate,106), ' ','-') as DeliveryDate,OrderNo,TermId,PaymentId,ByAirSea,PortOfLoading,SeaPort From DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DD 
            Where DM.OrderId=DD.OrderId And DM.OrderId=" + DDCustOrderNo.SelectedValue;
        }
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderUnit, "SELECT UnitId,UnitName from Unit Where MasterCompanyId=" + Session["varCompanyId"] + " order by UnitId", true, "--SELECT--");
            DDOrderUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
            TxtOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderCalType"]) == 0)
            {
                rdoUnitWise.Checked = true;
            }
            else
            {
                rdoPcWise.Checked = true;
            }
            ddDeliveryTerms.SelectedValue = ds.Tables[0].Rows[0]["TermId"].ToString();
            ddPaymentMode.SelectedValue = ds.Tables[0].Rows[0]["PaymentId"].ToString();
            ddlByAirSea.SelectedValue = ds.Tables[0].Rows[0]["ByAirSea"].ToString();
            ddlPortOfLoading.SelectedValue = ds.Tables[0].Rows[0]["PortOfLoading"].ToString();
            txtSeaPort.Text = ds.Tables[0].Rows[0]["SeaPort"].ToString();
            if (Session["varcompanyNo"].ToString() == "21")
            {
                if (ChkEditOrder.Checked == true)
                {
                    TxtCustomerOrderNo.Text = ds.Tables[0].Rows[0]["CustOrderNo"].ToString();
                    txtlocalorderno.Text = ds.Tables[0].Rows[0]["OrderNo"].ToString();
                }
                else
                {
                    string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'L ',''),0),0)+1),1) From ORDERMASTER Where  LocalOrder Like 'L %'").ToString();
                    txtlocalorderno.Text = "L " + Str;
                    TxtCustomerOrderNo.Text = ds.Tables[0].Rows[0]["OrderNo"].ToString();
                }
            }
            else
            {
                TxtCustomerOrderNo.Text = ds.Tables[0].Rows[0]["CustOrderNo"].ToString();
                txtlocalorderno.Text = ds.Tables[0].Rows[0]["OrderNo"].ToString();
            }

        }
        else
        {
            TxtCustomerOrderNo.Text = "";
            DDOrderUnit.SelectedIndex = 0;
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ddDeliveryTerms.SelectedIndex = 0;
            ddPaymentMode.SelectedIndex = 0;
            ddlByAirSea.SelectedIndex = 0;
            ddlPortOfLoading.SelectedIndex = 0;
            txtSeaPort.Text = "";
            txtlocalorderno.Text = "";
        }


    }
    //***************************Fill the Customer Code***************************************************************************
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CompanyName", true, "--SELECT--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGOrderDetail.DataSource = "";
        Fill_Order();
    }
    private void GetDetailForSmsEmail()
    {
        if (Convert.ToString(ViewState["orderid"]) != "" || Convert.ToString(ViewState["orderid"]) != "0")
        {
            string str = @"select CI.CustomerCode,CI.CompanyName as CustomerName,CI.Mobile,CompI.CompanyName,CI.Email, OM.OrderId,OM.CustomerOrderNo,
                            sum(OD.QtyRequired) as TotalQty,Round(sum(OD.Amount),2) as TotalAmount,
                            replace(convert(varchar(11),(OM.DispatchDate),106),' ','-') as DeliveryDate,
                            replace(convert(varchar(11),(OM.OrderDate),106),' ','-') as OrderDate, 
                            dbo.F_GetOrderItem(OM.orderid) as Product,isnull(c.CurrencyName,0) as CurrencyName
                            from OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                            INNER Join customerinfo CI ON CI.CustomerId=OM.CustomerId
                            INNER JOIN CompanyInfo CompI ON OM.CompanyId=CompI.CompanyId
                            left join currencyinfo c on c.currencyid=od.CurrencyId 
                            where OM.OrderId=" + ViewState["orderid"] + @"
                            group by CI.CustomerCode,CI.CompanyName,CI.Mobile,CI.Email,CompI.CompanyName,OM.OrderId,OM.CustomerOrderNo,OM.DispatchDate,OM.OrderDate,CurrencyName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                CustomerEmailId = ds.Tables[0].Rows[0]["Email"].ToString();
                CustomerOrder = ds.Tables[0].Rows[0]["CustomerOrderNo"].ToString();
                CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                ProductName = ds.Tables[0].Rows[0]["Product"].ToString();
                TotalQty = ds.Tables[0].Rows[0]["TotalQty"].ToString();
                TotalAmount = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                CurrencyName = ds.Tables[0].Rows[0]["Currencyname"].ToString();
                DeliveryDate = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();

            }
        }
    }
    private void SendSms()
    {
        try
        {
            if (Convert.ToString(ViewState["orderid"]) != "" || Convert.ToString(ViewState["orderid"]) != "0")
            {
                try
                {
                    GetDetailForSmsEmail();
                    if (CustomerMobile != "")
                    {
                        string Message = "", From = "";
                        Message = "Dear " + CustomerName + ",\nThe Following Order entered in the Erp\nOrderNo:" + CustomerOrder + "\nCompanyName:" + CompanyName + "\nQuantity:" + TotalQty + "\nAmount:" + TotalAmount + " " + CurrencyName + "\nDeliveryDate:" + DeliveryDate + ".";
                        UtilityModule.SendMessage(CustomerMobile, Message, Convert.ToInt16(Session["varcompanyNo"]), From: From);

                        //LblErrorMessage.Visible = true;
                        //LblErrorMessage.Text = "Message Sent Successfully";                        
                    }
                    else
                    {
                        UtilityModule.MessageAlert("", "Please Fill mobile no");
                    }
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Please Fill mobile no");
                }
            }

        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
    }
    private void SendMail()
    {
        try
        {
            GetDetailForSmsEmail();
            if (CustomerEmailId != "")
            {
                string Message = "", Subject = "Order Details";
                Message = "Dear " + CustomerName + ",\nThe Following Order entered in the Erp\nOrderNo:" + CustomerOrder + "\nCompanyName:" + CompanyName + "\nQuantity:" + TotalQty + "\nAmount:" + TotalAmount + " " + CurrencyName + "\nDeliveryDate:" + DeliveryDate + ".";
                UtilityModule.SendMail("qaaleenrugs@gmail.com", "QANmkt110030", 587, "smtp.gmail.com", "qaaleenrugs@gmail.com", CustomerEmailId, Subject, Message);
                //LblErrorMessage.Visible = true;
                //LblErrorMessage.Text = "Message Sent Successfully";
            }
            else
            {
                UtilityModule.MessageAlert("", "Please Fill Customer EmailId");
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.ToString());
        }
    }
    //***********************************
    //*********************************Function to Refresh the Order Detal******************************************
    private void refreshform()
    {
        //DDCustOrderNo.SelectedIndex = 0;
        ViewState["orderid"] = "0";
        getOrderDetail();
        Fill_Order();
        Fill_Grid();
        //LblErrorMessage.Visible = false;
        TxtTotalAmount.Text = "";
        TxtTotalQtyRequired.Text = "";
        TxtOrderArea.Text = "";
    }
    //********************Function to check For Existing Finished Item Id*********************************************************
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
                SqlParameter[] _arrpara = new SqlParameter[19];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                _arrpara[2] = new SqlParameter("@DeliveryDate", SqlDbType.DateTime);
                _arrpara[3] = new SqlParameter("@CustomerOrderNo", SqlDbType.NVarChar);
                _arrpara[4] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@TermId", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@PaymentId", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@ByAirSea", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@PortOfLoading", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@SeaPort", SqlDbType.NVarChar);
                _arrpara[10] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@Qty", SqlDbType.Int);
                _arrpara[12] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@RetuenOrderid", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@Pro_Flag", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@UpdateFlag", SqlDbType.Int);
                _arrpara[16] = new SqlParameter("@localorderno", SqlDbType.NVarChar);
                _arrpara[17] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 50);


                _arrpara[1].Value = TxtOrderDate.Text;
                _arrpara[2].Value = TxtDeliveryDate.Text;
                _arrpara[3].Value = TxtCustomerOrderNo.Text != "" ? TxtCustomerOrderNo.Text : "";
                _arrpara[4].Value = Session["varuserid"].ToString();
                _arrpara[5].Value = ddDeliveryTerms.SelectedValue;
                _arrpara[6].Value = ddPaymentMode.SelectedValue;
                _arrpara[7].Value = ddlByAirSea.SelectedValue;
                _arrpara[8].Value = ddlPortOfLoading.SelectedValue;
                _arrpara[9].Value = txtSeaPort.Text;
                _arrpara[13].Direction = ParameterDirection.Output;
                _arrpara[14].Value = 0;
                _arrpara[15].Value = ChkEditOrder.Checked == true ? 1 : 0;
                _arrpara[16].Value = txtlocalorderno.Text != "" ? txtlocalorderno.Text : "";
                _arrpara[17].Direction = ParameterDirection.Output;

                for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
                {
                    _arrpara[0].Value = (ViewState["orderid"] == null ? 0 : ViewState["orderid"]);
                    string OrderDetailId = DGOrderDetail.DataKeys[i].Value.ToString();
                    TextBox QTY = (TextBox)DGOrderDetail.Rows[i].FindControl("TxtQtyGD");
                    TextBox RATE = (TextBox)DGOrderDetail.Rows[i].FindControl("TxtRateGD");
                    _arrpara[10].Value = OrderDetailId;
                    _arrpara[11].Value = QTY.Text;
                    _arrpara[12].Value = RATE.Text;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CONFIRM_DRAFT_ORDER", _arrpara);
                    ViewState["orderid"] = _arrpara[13].Value;
                    if (_arrpara[17].Value.ToString() != "")
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = _arrpara[17].Value.ToString();
                        Tran.Commit();
                        return;
                    }
                }
                Tran.Commit();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "DATA SAVED SUCCESSFULLY..";

                //Message

                string Number = "", From = "";

                switch (Session["varcompanyNo"].ToString())
                {
                    case "6":
                    case "12":
                        if (ChkEditOrder.Checked == false)
                        {
                            string str = "select Om.orderid,case When " + Session["varcompanyNo"] + "=6 Then CI.Customercode Else CI.CompanyName End as CustomerName,OM.customerorderno,replace(CONVERT(nvarchar(11),OM.dispatchdate,106),' ','-') as dispatchdate,Sum(QtyRequired) as Qty,Round(Sum(OD.Amount),2) as Amount,dbo.F_GetOrderItem(OM.orderid) as Product,isnull(c.CurrencyName,0) as CurrencyName from ordermaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId inner join Customerinfo ci on OM.customerid=CI.customerid left join currencyinfo c on c.currencyid=od.CurrencyId where OM.orderid=" + ViewState["orderid"] + " group by CI.customercode,CI.CompanyName,OM.customerorderno,OM.DispatchDate,OM.orderid,c.CurrencyName";
                            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                switch (Session["varcompanyNo"].ToString())
                                {
                                    case "6":
                                        Number = "9799998661,9829333933,9887497107";
                                        From = "AI";
                                        break;
                                    case "12":
                                        //Number = "8447281984";
                                        Number = "9717331705,9839049112,8756266111,7065029921";
                                        From = Session["UserName"].ToString();
                                        break;
                                }
                                string message = "Dear sir,\nThe Following Order entered in the Erp\nCustomer:" + ds.Tables[0].Rows[0]["CustomerName"] + "\nProduct:" + ds.Tables[0].Rows[0]["Product"] + "\nQuantity:" + ds.Tables[0].Rows[0]["Qty"] + "\nAmount:" + ds.Tables[0].Rows[0]["Amount"] + " " + ds.Tables[0].Rows[0]["Currencyname"] + "\nDeliveryDate:" + ds.Tables[0].Rows[0]["DispatchDate"] + ".";
                                UtilityModule.SendMessage(Number, message, Convert.ToInt16(Session["varcompanyNo"]), From: From);
                            }
                        }
                        break;
                    case "21":
                        if (ChkEditOrder.Checked == false)
                        {
                            SendSms();
                            //SendMail();
                        }
                        break;


                }
                // Report_Type();
                // refreshform();
                Fill_Grid();
                refreshform();
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
        if (UtilityModule.VALIDTEXTBOX(txtSeaPort) == false)
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
        if (Session["varCompanyId"].ToString() == "2")
        {
            DGOrderDetail.Columns[9].Visible = false;
        }
    }
    //***************************************************************************************************************************
    //********************************Function To Get Data to fill Gride*********************************************************
    private DataSet GetDetail()
    {
        string strsql = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            if (ChkEditOrder.Checked == true)
            {
                strsql = @"SELECT DD.OrderDetailId as Sr_No,VF.CATEGORY_NAME CATEGORY,VF.ITEM_NAME ITEMNAME,VF.QUALITYNAME+SPACE(2)+VF.DESIGNNAME+SPACE(2)+
                VF.COLORNAME+SPACE(2)+SHAPENAME+SPACE(2)+CASE WHEN DD.OrderUnitId=1 THEN VF.SIZEMTR + 'Mtr' ELSE Case When dd.OrderUnitId=2 Then VF.SIZEFT + 'ft' Else case When dd.OrderunitId=6 Then vf.Sizeinch +' Inch' Else '' End End END DESCRIPTION,DD.QtyRequired Qty,DD.UnitRate RATE,
                DD.QtyRequired*DD.TotalArea AREA,DD.AMOUNT,QM.SUBQUANTITY SUBQUALITY,'' BTNCONSUMPTION,'' BTNEXPENCE,'' BTNPACKING,DD.TotalArea as area1,OrderCalType FROM ORDERMASTER DM,V_FINISHEDITEMDETAIL VF,
                ORDERDETAIL DD LEFT OUTER JOIN QUALITYCODEMASTER QM ON DD.QUALITYCODEID=QM.QUALITYCODEID WHERE DM.ORDERID=DD.ORDERID AND DD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID AND DM.DRAFTORDERID=" + DDCustOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By DD.OrderDetailId";
            }
            else
            {
                strsql = @"SELECT DD.OrderDetailId as Sr_No,VF.CATEGORY_NAME CATEGORY,VF.ITEM_NAME ITEMNAME,VF.QUALITYNAME+SPACE(2)+VF.DESIGNNAME+SPACE(2)+VF.COLORNAME+
                SPACE(2)+SHAPENAME+SPACE(2)+CASE WHEN DD.UnitId=1 THEN VF.SIZEMTR + 'Mtr' ELSE Case When dd.UnitId=2 Then  VF.SIZEFT+ 'ft' Else Case When dd.unitId=6 Then sizeinch + 'Inch'  Else '' End END END DESCRIPTION,DD.QTY,DD.RATE,DD.QTY*DD.AREA AREA,DD.AMOUNT,
                QM.SUBQUANTITY SUBQUALITY,'' BTNCONSUMPTION,'' BTNEXPENCE,'' BTNPACKING,DD.AREA as area1,OrderCalType FROM DRAFT_ORDER_MASTER DM,V_FINISHEDITEMDETAIL VF,DRAFT_ORDER_DETAIL DD 
                LEFT OUTER JOIN QUALITYCODEMASTER QM ON DD.QUALITYCODEID=QM.QUALITYCODEID WHERE DM.ORDERID=DD.ORDERID AND DD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID AND DM.ORDERID=" + ViewState["DraftOrderNo"] + " And Vf.MasterCompanyId=" + Session["varCompanyId"] + " Order By DD.OrderDetailId";
            }
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
        catch (Exception ex)
        {
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
    private DataSet fill_gird_chk()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ITEM_MASTER.ITEM_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.Lablecustomer.Itemid, dbo.Lablecustomer.CustomerId AS customerid, 
                            dbo.ITEM_MASTER.ITEM_NAME FROM dbo.Lablecustomer INNER JOIN dbo.ITEM_MASTER ON dbo.Lablecustomer.Itemid = dbo.ITEM_MASTER.ITEM_ID INNER JOIN
                            dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
                            Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And Item_Master.MasterCompanyId=" + Session["varCompanyId"];
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
                            Where orderid=" + DDCustOrderNo.SelectedValue + " And Item_Master.MasterCompanyId=" + Session["varCompanyId"];
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
                            Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' and customerid=" + DDCustomerCode.SelectedValue + " And Item_Master.MasterCompanyId=" + Session["varCompanyId"];
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
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy"); ;
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
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
                   Where DM.Orderid=DD.Orderid And DM.ORDERID=DOD.ORDERID And DD.ORDERDETAILID=DOD.ORDERDETAILID And DD.IFinishedid=VF.Item_Finished_id And DD.OFinishedid=VF1.Item_Finished_id And U.UnitId=DD.IUnitId And U1.UnitId=DD.OUnitId
                   And DD.OrderDetailId=" + DGOrderDetail.DataKeys[0].Value + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;
    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string Str;
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGOrderDetail.Rows[index];
        BtnNewSave.Visible = true;
        if (Convert.ToInt32(Session["FunctionType"]) == 1)
        {
            Str = @"Select Str(PCMDID)+'|'+Str(DD.OrderDetailId) PCMDID,PM.PROCESS_NAME PROCESSNAME,VF.Category_Name ICategory,VF.Item_Name IItem,VF.QualityName+Space(2)+VF.DesignName+Space(2)+VF.ColorName+Space(2)+VF.ShadeColorName+Space(2)+
                  VF.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF.SizeMtr Else VF.SizeFt End IDescription,U.UnitName IUnitName,
                  IQTY,ILOSS,IRATE,VF1.Category_Name OCategory,VF1.Item_Name OItem,VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShadeColorName+Space(2)+
                  VF1.shapeName+Space(2)+Case When DOD.UnitID=1 Then VF1.SizeMtr Else VF1.SizeFt End ODescription,U1.UnitName OUnitName,OQTY,ORATE
                  FROM DRAFT_ORDER_MASTER DM,DRAFT_ORDER_DETAIL DOD,DRAFT_ORDER_CONSUMPTION_DETAIL DD,V_FinishedItemDetail VF,V_FinishedItemDetail VF1,Unit U,Unit U1,PROCESS_NAME_MASTER PM
                  Where DM.Orderid=DD.Orderid And DM.ORDERID=DOD.ORDERID And DD.ORDERDETAILID=DOD.ORDERDETAILID And DD.IFinishedid=VF.Item_Finished_id And DD.OFinishedid=VF1.Item_Finished_id And U.UnitId=DD.IUnitId And U1.UnitId=DD.OUnitId And
			      PM.PROCESS_NAME_ID=DD.PROCESSID And DD.OrderDetailId=" + row.Cells[0].Text + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGConsumption.DataSource = ds;
            DGConsumption.DataBind();
        }
        else if (Convert.ToInt32(Session["FunctionType"]) == 2)
        {
            Str = @"SELECT Str(DOC.CWOEID)+'|'+str(DOC.ORDERDETAILID) CWOEID,EN.CHARGENAME,DOC.PERCENTAGE FROM DRAFT_ORDER_CUSTWISEOTHEREXPENCE DOC,EXPENSENAME EN 
                  WHERE DOC.EXPID=EN.EXPID AND ORDERDETAILID=" + row.Cells[0].Text + " And EN.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGExpence.DataSource = ds;
            DGExpence.DataBind();
        }
        else if (Convert.ToInt32(Session["FunctionType"]) == 3)
        {
            Str = @"SELECT Str(PRMCID)+'|'+str(ORDERDETAILID) PRMCID,INNERAMT,MIDDLEAMT,MASTERAMT,OTHERAMT,CONTAINERAMT FROM DRAFT_ORDER_PACKING_AND_OTHERMATERIAL_COST 
                  WHERE ORDERDETAILID=" + row.Cells[0].Text + " And MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGPacking.DataSource = ds;
            DGPacking.DataBind();
        }
    }
    protected void BTNCONSUMPTION_Click(object sender, EventArgs e)
    {
        DGConsumption.Visible = true;
        DGExpence.Visible = false;
        DGPacking.Visible = false;
        Session["FunctionType"] = 1;
    }
    protected void BTNEXPENCE_Click(object sender, EventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = true;
        DGPacking.Visible = false;
        Session["FunctionType"] = 2;
    }
    protected void BTNPACKING_Click(object sender, EventArgs e)
    {
        DGConsumption.Visible = false;
        DGExpence.Visible = false;
        DGPacking.Visible = true;
        Session["FunctionType"] = 3;
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
                string str = @"UPDATE DRAFT_ORDER_CUSTWISEOTHEREXPENCE Set INNERAMT=" + INNERAMT.Text + ",MIDDLEAMT=" + MIDDLEAMT.Text + ",MASTERAMT=" + MASTERAMT.Text + ",OTHERAMT=" + OTHERAMT.Text + ",CONTAINERAMT=" + CONTAINERAMT.Text + " Where ORDERDETAILID=" + Convert.ToInt32(strPCMDID.Split('|')[1]) + " And CWOEID=" + Convert.ToInt32(strPCMDID.Split('|')[0]) + "";
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
    }
    protected void btnContainerpackingMatCost_Click(object sender, EventArgs e)
    {
        btnContainerPackingCost.Visible = true;
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        Fill_Order();
    }
    private void Fill_Order()
    {
        string STR = "";
        #region on 27-Nov-2012
        //RateType
        //UtilityModule.ConditionalComboFill(ref ddDeliveryTerms, "select TermId,TermName from Term order by TermName", true, "--Select--");
        ////Payment Term
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
        if (ChkEditOrder.Checked == true)
        {
            STR = "Select DISTINCT DOM.Orderid,DOM.OrderNo FROM ORDERMASTER OM,DRAFT_ORDER_MASTER DOM WHERE OM.DRAFTORDERID=DOM.ORDERID AND OM.Companyid=" + DDCompanyName.SelectedValue + "";

            if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
            {
                STR = STR + @" AND OM.Customerid=" + DDCustomerCode.SelectedValue + "";
            }
            if (Session["VarcompanyNo"].ToString() != "2")
            {
                STR = STR + @" AND OM.ORDERID NOT IN (SELECT ORDERID FROM JOBASSIGNS)";
            }
            STR = STR + "order by OrderNo";
            TDCancelDraftOrder.Visible = true;
        }
        else
        {
            STR = "Select DISTINCT Orderid,OrderNo FROM DRAFT_ORDER_MASTER Where ORDERID NOT IN (SELECT ISNULL(DRAFTORDERID,0) FROM ORDERMASTER) AND Companyid=" + DDCompanyName.SelectedValue + "";

            if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
            {
                STR = STR + @" AND Customerid=" + DDCustomerCode.SelectedValue + "";
            }
            STR = STR + "order by OrderNo";
            TDCancelDraftOrder.Visible = false;
        }
        UtilityModule.ConditionalComboFill(ref DDCustOrderNo, STR, true, "--SELECT--");
    }
    protected void DDPreviewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Report_Type();
    }
    private void Report_Type()
    {
        int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        switch (VarCompanyNo)
        {
            case 2:
                PERFORMINVOICEFORDESTINI();
                break;
            default:
                PERFORMINVOICEFORMALANI();
                break;
        }
    }
    private void PERFORMINVOICEFORMALANI()
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (Convert.ToInt32(DDPreviewType.SelectedValue) == 0)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceMainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceMainOrderNew.xsd";
            str = @"SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, 
            CompanyInfo.CompAddr3, CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName,
            VIEW_PERFORMAINVOICE.TransModeName, VIEW_PERFORMAINVOICE.StationName, VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate,
            VIEW_PERFORMAINVOICE.OrderDetailId, VIEW_PERFORMAINVOICE.Qty, VIEW_PERFORMAINVOICE.Remarks, CONTAINERCOST.Length, CONTAINERCOST.Width, CONTAINERCOST.Height, CONTAINERCOST.PCS,
            CONTAINERCOST.UnitId, V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName,
            V_FinishedItemDetail.SizeFt, VIEW_PERFORMAINVOICE.Weight, customerinfo.Address, V_FinishedItemDetail.HSCODE, UPCNO.UPCNO, VIEW_PERFORMAINVOICE.UnitId, customerinfo.Email, V_FinishedItemDetail.SizeMtr,
            V_FinishedItemDetail.SizeInch, Bank.BankName, Bank.Street, Bank.City, Bank.State, Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, Bank.FaxNo, VIEW_PERFORMAINVOICE.OURCODE, VIEW_PERFORMAINVOICE.OrderCalType,
            VIEW_PERFORMAINVOICE.Rate, VIEW_PERFORMAINVOICE.Area, VIEW_PERFORMAINVOICE.CurrencyName, V_FinishedItemDetail.ShapeId, Unit.UnitName, V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt,
            V_FinishedItemDetail.LWHInch, VIEW_PERFORMAINVOICE.DeliveryComments, CurrencyInfo.CurrencyName, CurrencyInfo.PAYINSTRUCTION, CurrencyInfo.BENEFICIARY_BANK, CurrencyInfo.REMARKS, customerinfo.PhoneNo,
            customerinfo.Fax, VIEW_PERFORMAINVOICE.localorder, Signatory.SignatoryName, CompanyInfo.IECode, CompanyInfo.CompanyId, CustomerColor.ColorNameToC, CustomerDesign.DesignNameAToC, CustomerQuality.QualityNameAToC,
            CustomerSize.SizeNameAToC, CustomerSize.MtSizeAToC, VIEW_PERFORMAINVOICE.Photo
            FROM   ((((((((((((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) LEFT OUTER JOIN dbo.CONTAINERCOST CONTAINERCOST ON VIEW_PERFORMAINVOICE.OrderDetailId=CONTAINERCOST.DraftOrderDetailId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) LEFT OUTER JOIN dbo.UPCNO UPCNO ON (VIEW_PERFORMAINVOICE.CUSTOMERID=UPCNO.CustomerId) AND (VIEW_PERFORMAINVOICE.Item_Finished_Id=UPCNO.Finishedid)) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) INNER JOIN dbo.CurrencyInfo CurrencyInfo ON VIEW_PERFORMAINVOICE.CurrencyId=CurrencyInfo.CurrencyId) LEFT OUTER JOIN dbo.CustomerSize CustomerSize ON (customerinfo.CustomerId=CustomerSize.CustomerId) AND (V_FinishedItemDetail.SizeId=CustomerSize.Sizeid)) LEFT OUTER JOIN dbo.CustomerDesign CustomerDesign ON (customerinfo.CustomerId=CustomerDesign.CustomerId) AND (V_FinishedItemDetail.designId=CustomerDesign.DesignId)) LEFT OUTER JOIN dbo.CustomerQuality CustomerQuality ON (customerinfo.CustomerId=CustomerQuality.CustomerId) AND (V_FinishedItemDetail.QualityId=CustomerQuality.QualityId)) LEFT OUTER JOIN dbo.CustomerColor CustomerColor ON (customerinfo.CustomerId=CustomerColor.CustomerId) AND (V_FinishedItemDetail.ColorId=CustomerColor.ColorId)) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId) INNER JOIN dbo.Signatory Signatory ON CompanyInfo.Sigantory=Signatory.SignatoryId
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 1)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceWithBuyerCodeMainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceWithBuyerCodeMainOrderNew.xsd";
            str = @"  SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName, VIEW_PERFORMAINVOICE.TransModeName,
            VIEW_PERFORMAINVOICE.StationName, VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate, VIEW_PERFORMAINVOICE.OrderDetailId,
            VIEW_PERFORMAINVOICE.Qty, VIEW_PERFORMAINVOICE.Remarks, CONTAINERCOST.Length, CONTAINERCOST.Width, CONTAINERCOST.Height, CONTAINERCOST.PCS, CONTAINERCOST.UnitId, V_FinishedItemDetail.ITEM_NAME,
            V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName, V_FinishedItemDetail.SizeFt, VIEW_PERFORMAINVOICE.Weight, 
            customerinfo.Address, UPCNO.UPCNO, VIEW_PERFORMAINVOICE.UnitId, customerinfo.Email, V_FinishedItemDetail.SizeMtr, V_FinishedItemDetail.SizeInch, Bank.BankName, Bank.Street, Bank.City, Bank.State,
            Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, Bank.FaxNo, VIEW_PERFORMAINVOICE.OURCODE, VIEW_PERFORMAINVOICE.OrderCalType, VIEW_PERFORMAINVOICE.Rate, VIEW_PERFORMAINVOICE.Area, VIEW_PERFORMAINVOICE.CurrencyName, 
            V_FinishedItemDetail.ShapeId, Unit.UnitName, V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt, V_FinishedItemDetail.LWHInch, VIEW_PERFORMAINVOICE.BUYERCODE, VIEW_PERFORMAINVOICE.DeliveryComments, customerinfo.PhoneNo,
            customerinfo.Fax, CurrencyInfo.CurrencyName, CurrencyInfo.PAYINSTRUCTION, CurrencyInfo.BENEFICIARY_BANK, CurrencyInfo.REMARKS, VIEW_PERFORMAINVOICE.LessAdvance, VIEW_PERFORMAINVOICE.LessCommission, VIEW_PERFORMAINVOICE.LessDiscount, 
            VIEW_PERFORMAINVOICE.localorder, CompanyInfo.CompanyId, VIEW_PERFORMAINVOICE.Photo
            FROM   (((((((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) LEFT OUTER JOIN dbo.CONTAINERCOST CONTAINERCOST ON VIEW_PERFORMAINVOICE.OrderDetailId=CONTAINERCOST.DraftOrderDetailId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) LEFT OUTER JOIN dbo.UPCNO UPCNO ON (VIEW_PERFORMAINVOICE.CUSTOMERID=UPCNO.CustomerId) AND (VIEW_PERFORMAINVOICE.Item_Finished_Id=UPCNO.Finishedid)) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) INNER JOIN dbo.CurrencyInfo CurrencyInfo ON VIEW_PERFORMAINVOICE.CurrencyId=CurrencyInfo.CurrencyId) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 2)
        {
            Session["ReportPath"] = "Reports/RptPhotoQuatiationMainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPhotoQuatiationMainOrderNew.xsd";
            str = @" SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName, VIEW_PERFORMAINVOICE.TransModeName,
            VIEW_PERFORMAINVOICE.StationName, VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate, VIEW_PERFORMAINVOICE.OrderDetailId,
            VIEW_PERFORMAINVOICE.Remarks, V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName,
            V_FinishedItemDetail.SizeFt, customerinfo.Address, VIEW_PERFORMAINVOICE.UnitId, customerinfo.Email, CONTAINERCOST.Length, CONTAINERCOST.Width, CONTAINERCOST.Height, CONTAINERCOST.PCS, 
            CONTAINERCOST.UnitId, V_FinishedItemDetail.SizeMtr, V_FinishedItemDetail.SizeInch, Bank.BankName, Bank.Street, Bank.City, Bank.State, Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, 
            Bank.FaxNo, VIEW_PERFORMAINVOICE.OURCODE, VIEW_PERFORMAINVOICE.BUYERCODE, VIEW_PERFORMAINVOICE.Rate, VIEW_PERFORMAINVOICE.CurrencyName, V_FinishedItemDetail.ShapeId, Unit.UnitName, 
            V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt, V_FinishedItemDetail.LWHInch, VIEW_PERFORMAINVOICE.DeliveryComments, customerinfo.PhoneNo, customerinfo.Fax, CurrencyInfo.CurrencyName, 
            CurrencyInfo.PAYINSTRUCTION, CurrencyInfo.BENEFICIARY_BANK, CurrencyInfo.REMARKS, VIEW_PERFORMAINVOICE.localorder, CompanyInfo.CompanyId, VIEW_PERFORMAINVOICE.Photo
            FROM   ((((((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) LEFT OUTER JOIN dbo.CONTAINERCOST CONTAINERCOST ON VIEW_PERFORMAINVOICE.OrderDetailId=CONTAINERCOST.DraftOrderDetailId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) INNER JOIN dbo.CurrencyInfo CurrencyInfo ON VIEW_PERFORMAINVOICE.CurrencyId=CurrencyInfo.CurrencyId) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 3)
        {
            Session["ReportPath"] = "Reports/RptPerformaInvoice1MainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerformaInvoice1MainOrderNew.xsd";
            str = @" SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,CompanyInfo.CompFax,
            CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName, VIEW_PERFORMAINVOICE.TransModeName, VIEW_PERFORMAINVOICE.StationName,
            VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate, VIEW_PERFORMAINVOICE.OrderDetailId, customerinfo.Address, VIEW_PERFORMAINVOICE.Qty, 
            V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.SizeInch, customerinfo.PhoneNo,
            customerinfo.Fax, VIEW_PERFORMAINVOICE.DeliveryComments, VIEW_PERFORMAINVOICE.localorder, CompanyInfo.CompanyId, VIEW_PERFORMAINVOICE.BUYERCODE, VIEW_PERFORMAINVOICE.Photo
            FROM   ((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 4)
        {
            Session["ReportPath"] = "Reports/RptPerformaInvoice2MainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerformaInvoice2MainOrderNew.xsd";
            str = @" SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERNO, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.OrderDetailId, customerinfo.Address, VIEW_PERFORMAINVOICE.Qty, V_FinishedItemDetail.ITEM_NAME, 
            V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.SizeMtr, customerinfo.PhoneNo, customerinfo.Fax, CompanyInfo.CompanyId, VIEW_PERFORMAINVOICE.Photo
            FROM   ((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 5)
        {
            Session["ReportPath"] = "Reports/RptPerFomaInvioceWithOutPKGMainOrderNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFomaInvioceWithOutPKGMainOrderNew.xsd";
            str = @" SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3, CompanyInfo.CompFax,
            CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName, VIEW_PERFORMAINVOICE.TransModeName, VIEW_PERFORMAINVOICE.StationName,
            VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate, VIEW_PERFORMAINVOICE.OrderDetailId, VIEW_PERFORMAINVOICE.Qty, VIEW_PERFORMAINVOICE.Remarks, 
            V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName, V_FinishedItemDetail.SizeFt,
            VIEW_PERFORMAINVOICE.Weight, customerinfo.Address, UPCNO.UPCNO, VIEW_PERFORMAINVOICE.UnitId, customerinfo.Email, V_FinishedItemDetail.SizeMtr, V_FinishedItemDetail.SizeInch, Bank.BankName, 
            Bank.Street, Bank.City, Bank.State, Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, Bank.FaxNo, VIEW_PERFORMAINVOICE.OURCODE, VIEW_PERFORMAINVOICE.OrderCalType, VIEW_PERFORMAINVOICE.Rate,
            VIEW_PERFORMAINVOICE.Area, VIEW_PERFORMAINVOICE.CurrencyName, V_FinishedItemDetail.ShapeId, Unit.UnitName, V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt, V_FinishedItemDetail.LWHInch,
            VIEW_PERFORMAINVOICE.DeliveryComments, CurrencyInfo.CurrencyName, CurrencyInfo.PAYINSTRUCTION, CurrencyInfo.BENEFICIARY_BANK, CurrencyInfo.REMARKS, customerinfo.PhoneNo, customerinfo.Fax, 
            VIEW_PERFORMAINVOICE.BUYERCODE, VIEW_PERFORMAINVOICE.localorder, CompanyInfo.CompanyId, VIEW_PERFORMAINVOICE.Photo
            FROM   ((((((dbo.VIEW_PERFORMAINVOICE1 VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) LEFT OUTER JOIN dbo.UPCNO UPCNO ON (VIEW_PERFORMAINVOICE.CUSTOMERID=UPCNO.CustomerId) AND (VIEW_PERFORMAINVOICE.Item_Finished_Id=UPCNO.Finishedid)) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) INNER JOIN dbo.CurrencyInfo CurrencyInfo ON VIEW_PERFORMAINVOICE.CurrencyId=CurrencyInfo.CurrencyId) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId
            Where VIEW_PERFORMAINVOICE.Orderid=" + ViewState["orderid"] + "  ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 6)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceCh1New.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceCh1New.xsd";
            str = @"  SELECT customerinfo.CompanyName, VIEW_PERFORMAINVOICE.ORDERDATE, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, 
            CompanyInfo.CompAddr3, CompanyInfo.CompFax, CompanyInfo.CompTel, VIEW_PERFORMAINVOICE.StationName, VIEW_PERFORMAINVOICE.CustOrderNo, VIEW_PERFORMAINVOICE.DeliveryDate,
            VIEW_PERFORMAINVOICE.OrderDetailId, VIEW_PERFORMAINVOICE.Qty, V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName,
            V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName, V_FinishedItemDetail.SizeFt, customerinfo.Address, VIEW_PERFORMAINVOICE.UnitId, V_FinishedItemDetail.SizeMtr,
            V_FinishedItemDetail.SizeInch, Bank.BankName, Bank.Street, Bank.City, Bank.State, Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, Bank.FaxNo, VIEW_PERFORMAINVOICE.Rate,
            VIEW_PERFORMAINVOICE.Area, VIEW_PERFORMAINVOICE.CurrencyName, V_FinishedItemDetail.ShapeId, Unit.UnitName, V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt, 
            V_FinishedItemDetail.LWHInch, customerinfo.PhoneNo, customerinfo.Fax, VIEW_PERFORMAINVOICE.localorder, Signatory.SignatoryName, CompanyInfo.CompanyId, CustomerDesign.DesignNameAToC,
            CustomerSize.SizeNameAToC, VIEW_PERFORMAINVOICE.BuyerCode, VIEW_PERFORMAINVOICE.Custorderdate, VIEW_PERFORMAINVOICE.duedate, CustomerSize.MtSizeAToC, CustomerColor.ColorNameToC, 
            CustomerQuality.QualityNameAToC, VIEW_PERFORMAINVOICE.Photo
            FROM   (((((((((dbo.VIEW_PERFORMAINVOICE VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) LEFT OUTER JOIN dbo.CustomerDesign CustomerDesign ON (customerinfo.CustomerId=CustomerDesign.CustomerId) AND (V_FinishedItemDetail.designId=CustomerDesign.DesignId)) LEFT OUTER JOIN dbo.CustomerSize CustomerSize ON (customerinfo.CustomerId=CustomerSize.CustomerId) AND (V_FinishedItemDetail.SizeId=CustomerSize.Sizeid)) LEFT OUTER JOIN dbo.CustomerQuality CustomerQuality ON (customerinfo.CustomerId=CustomerQuality.CustomerId) AND (V_FinishedItemDetail.QualityId=CustomerQuality.QualityId)) LEFT OUTER JOIN dbo.CustomerColor CustomerColor ON (customerinfo.CustomerId=CustomerColor.CustomerId) AND (V_FinishedItemDetail.ColorId=CustomerColor.ColorId)) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId) INNER JOIN dbo.Signatory Signatory ON CompanyInfo.Sigantory=Signatory.SignatoryId
            WHERE VIEW_PERFORMAINVOICE.ORDERID=" + ViewState["orderid"] + " ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";

        }
        else if (Convert.ToInt32(DDPreviewType.SelectedValue) == 7)
        {
            Session["ReportPath"] = "Reports/RptPerFormaInvoiceCh2New.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceCh2New.xsd";
            str = @"  SELECT customerinfo.CompanyName, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3, CompanyInfo.CompFax, CompanyInfo.CompTel,
            VIEW_PERFORMAINVOICE.TermName, VIEW_PERFORMAINVOICE.PaymentName, VIEW_PERFORMAINVOICE.TransModeName, VIEW_PERFORMAINVOICE.StationName, VIEW_PERFORMAINVOICE.SeaPort, VIEW_PERFORMAINVOICE.CustOrderNo,
            VIEW_PERFORMAINVOICE.DeliveryDate, VIEW_PERFORMAINVOICE.OrderDetailId, VIEW_PERFORMAINVOICE.Qty, V_FinishedItemDetail.ITEM_NAME, V_FinishedItemDetail.QualityName, V_FinishedItemDetail.designName, 
            V_FinishedItemDetail.ColorName, V_FinishedItemDetail.ShapeName, V_FinishedItemDetail.SizeFt, customerinfo.Address, VIEW_PERFORMAINVOICE.UnitId, V_FinishedItemDetail.SizeMtr, V_FinishedItemDetail.SizeInch,
            Bank.BankName, Bank.Street, Bank.City, Bank.State, Bank.Country, Bank.ACNo, Bank.SwiftCode, Bank.PhoneNo, Bank.FaxNo, VIEW_PERFORMAINVOICE.OrderCalType, VIEW_PERFORMAINVOICE.Rate, VIEW_PERFORMAINVOICE.Area,
            VIEW_PERFORMAINVOICE.CurrencyName, V_FinishedItemDetail.ShapeId, Unit.UnitName, V_FinishedItemDetail.LWHMtr, V_FinishedItemDetail.LWHFt, V_FinishedItemDetail.LWHInch, CurrencyInfo.CurrencyName, CurrencyInfo.PAYINSTRUCTION,
            CurrencyInfo.BENEFICIARY_BANK, CurrencyInfo.REMARKS, customerinfo.PhoneNo, customerinfo.Fax, Signatory.SignatoryName, CompanyInfo.CompanyId, CustomerDesign.DesignNameAToC, CustomerSize.SizeNameAToC, VIEW_PERFORMAINVOICE.BuyerCode,
            CustomerSize.MtSizeAToC, VIEW_PERFORMAINVOICE.Custorderdate, VIEW_PERFORMAINVOICE.duedate, CustomerColor.ColorNameToC, CustomerQuality.QualityNameAToC,'' as photo
            FROM   ((((((((((dbo.VIEW_PERFORMAINVOICE VIEW_PERFORMAINVOICE INNER JOIN dbo.CompanyInfo CompanyInfo ON VIEW_PERFORMAINVOICE.COMPANYID=CompanyInfo.CompanyId) INNER JOIN dbo.customerinfo customerinfo ON VIEW_PERFORMAINVOICE.CUSTOMERID=customerinfo.CustomerId) INNER JOIN dbo.V_FinishedItemDetail V_FinishedItemDetail ON VIEW_PERFORMAINVOICE.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID) INNER JOIN dbo.Unit Unit ON VIEW_PERFORMAINVOICE.UnitId=Unit.UnitId) INNER JOIN dbo.CurrencyInfo CurrencyInfo ON VIEW_PERFORMAINVOICE.CurrencyId=CurrencyInfo.CurrencyId) LEFT OUTER JOIN dbo.CustomerDesign CustomerDesign ON (customerinfo.CustomerId=CustomerDesign.CustomerId) AND (V_FinishedItemDetail.designId=CustomerDesign.DesignId)) LEFT OUTER JOIN dbo.CustomerSize CustomerSize ON (customerinfo.CustomerId=CustomerSize.CustomerId) AND (V_FinishedItemDetail.SizeId=CustomerSize.Sizeid)) LEFT OUTER JOIN dbo.CustomerQuality CustomerQuality ON (customerinfo.CustomerId=CustomerQuality.CustomerId) AND (V_FinishedItemDetail.QualityId=CustomerQuality.QualityId)) LEFT OUTER JOIN dbo.CustomerColor CustomerColor ON (customerinfo.CustomerId=CustomerColor.CustomerId) AND (V_FinishedItemDetail.ColorId=CustomerColor.ColorId)) INNER JOIN dbo.Bank Bank ON CompanyInfo.Bankid=Bank.BankId) INNER JOIN dbo.Signatory Signatory ON CompanyInfo.Sigantory=Signatory.SignatoryId
            WHERE VIEW_PERFORMAINVOICE.ORDERID=" + ViewState["orderid"] + " ORDER BY VIEW_PERFORMAINVOICE.OrderDetailId";
        }
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + ViewState["orderid"] + "";
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        //sda.Fill(ds);
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
        string STR1 = @"select OrderDetailId,Photo from Draft_Order_ReferenceImage where OrderDetailId in(select orderdetailid from draft_order_detail where orderid=" + ViewState["orderid"] + ") ";
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
    private void PERFORMINVOICEFORDESTINI()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        DataSet Ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * From SysObjects Where Name='VIEW_PERFORMAINVOICEFORDESTINI'");
        if (Ds1.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_PERFORMAINVOICEFORDESTINI");
        }
        #region
        //        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINI AS ";
        //        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        //        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        //        Str = Str + @"SELECT PM.PROCESSID,PM.FINISHEDID,PD.IFINISHEDID,PD.OFINISHEDID,OM.DRAFTORDERID ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,
        //                    FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        //        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        //        {
        //            Str = Str + ",[dbo].[GET_FINAMT](PM.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PD.IFINISHEDID) " + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "";
        //        }
        //        Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        //        Str = Str + @",OD.PHOTO IMAGE,OD.QtyRequired OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PM.FINISHEDID) CBM,OD.Remarks,OD.OurCode,BuyerCode,OD.DESCRIPTION,
        //                    OD.weight FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ORDERMASTER OM,ORDERDETAIL OD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,
        //                    FINISHED_TYPE FT WHERE PM.PCMID=PD.PCMID AND OM.OrderId=OD.OrderId And PM.FINISHEDID=OD.ITEM_FINISHED_ID AND PM.FINISHEDID=IPM.ITEM_FINISHED_ID AND 
        //                    PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND OM.DRAFTORDERID=" + ViewState["DraftOrderNo"] + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        #endregion
        string Str = "CREATE VIEW VIEW_PERFORMAINVOICEFORDESTINI AS ";
        //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT DISTINCT PROCESSID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PM,PROCESS_NAME_MASTER PNM WHERE PM.PROCESSID=PNM.PROCESS_NAME_ID AND FINISHEDID IN (SELECT ITEM_FINISHED_ID FROM DRAFT_ORDER_DETAIL WHERE ORDERID=" + ViewState["orderid"] + ")");
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT PROCESS_NAME_ID PROCESSID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID IN (1,6,7,8,9)");
        Str = Str + @"SELECT PM.PROCESSID,PM.FINISHEDID,PM.IFINISHEDID,PM.OFINISHEDID,PM.ORDERID ORDERID,IPM.PRODUCTCODE PROD_NO,IPM1.PRODUCTCODE ITEM_NO,
                        FT.FINISHED_TYPE_NAME GLASSTYPE,OQTY QTY,IRATE PP";
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Str = Str + ",[dbo].[GET_FINAMT](PM.FINISHEDID," + Ds.Tables[0].Rows[i]["PROCESSID"] + ",PM.IFINISHEDID) " + Ds.Tables[0].Rows[i]["PROCESS_NAME"] + "";
        }
        Str = Str + ",1 InnerPacking,2 MiddlePacking,3 MasterPacking";
        Str = Str + @",DOD.PHOTO IMAGE,PM.OQTY,PKGInstruction,
                    LBGInstruction,[dbo].[GET_CMB](PM.FINISHEDID) CBM,DOD.Remarks,DOD.OurCode,BuyerCode,DOD.DESCRIPTION,
                    DOD.weight FROM DRAFT_ORDER_CONSUMPTION_DETAIL PM,DRAFT_ORDER_DETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT
                        WHERE  PM.Orderdetailid=DOD.Orderdetailid AND PM.FINISHEDID=IPM.ITEM_FINISHED_ID AND PM.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND PM.O_FINISHED_TYPE_ID=FT.ID 
                    AND PM.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["DraftOrderNo"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        con.Close();
        Session["ReportPath"] = "Reports/RptPerFormaInvoiceDestini3New.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\RptPerFormaInvoiceDestini3New.xsd";
        //        string str = @" SELECT customerinfo.CompanyName, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
        //            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, customerinfo.Address, VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID, VIEW_PERFORMAINVOICEFORDESTINI.IFINISHEDID,
        //            VIEW_PERFORMAINVOICEFORDESTINI.ITEM_NO, VIEW_PERFORMAINVOICEFORDESTINI.GLASSTYPE, VIEW_PERFORMAINVOICEFORDESTINI.QTY, VIEW_PERFORMAINVOICEFORDESTINI.InnerPacking,
        //            VIEW_PERFORMAINVOICEFORDESTINI.MasterPacking, VIEW_PERFORMAINVOICEFORDESTINI.PROD_NO, customerinfo.Email, VIEW_PERFORMAINVOICEFORDESTINI.MiddlePacking, VIEW_PERFORMAINVOICEFORDESTINI.OQTY,
        //            VIEW_PERFORMAINVOICEFORDESTINI.PKGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.LBGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.CBM, VIEW_PERFORMAINVOICEFORDESTINI.Remarks, 
        //            VIEW_PERFORMAINVOICEFORDESTINI.BuyerCode, DRAFT_ORDER_MASTER.OrderNo, DRAFT_ORDER_MASTER.OrderDate, DRAFT_ORDER_MASTER.CustOrderNo, VIEW_PERFORMAINVOICEFORDESTINI.DESCRIPTION, 
        //            DRAFT_ORDER_MASTER.DeliveryDate, DRAFT_ORDER_MASTER.SeaPort, GoodsReceipt.StationName, TransMode.transmodeName, VIEW_PERFORMAINVOICEFORDESTINI.weight, customerinfo.CustomerCode, 
        //            DRAFT_ORDER_MASTER.Custorderdate, VIEW_PERFORMAINVOICEFORDESTINI.IMAGE as photo
        //            FROM   ((((dbo.CompanyInfo CompanyInfo INNER JOIN dbo.DRAFT_ORDER_MASTER DRAFT_ORDER_MASTER ON CompanyInfo.CompanyId=DRAFT_ORDER_MASTER.CompanyId) INNER JOIN dbo.VIEW_PERFORMAINVOICEFORDESTINI VIEW_PERFORMAINVOICEFORDESTINI ON DRAFT_ORDER_MASTER.OrderId=VIEW_PERFORMAINVOICEFORDESTINI.ORDERID) INNER JOIN dbo.customerinfo customerinfo ON DRAFT_ORDER_MASTER.CustomerId=customerinfo.CustomerId) LEFT OUTER JOIN dbo.GoodsReceipt GoodsReceipt ON DRAFT_ORDER_MASTER.PortOfLoading=GoodsReceipt.GoodsreceiptId) LEFT OUTER JOIN dbo.TransMode TransMode ON DRAFT_ORDER_MASTER.ByAirSea=TransMode.transmodeId  
        //            Where VIEW_PERFORMAINVOICEFORDESTINI.orderid=" + ViewState["DraftOrderNo"] + @"
        //            ORDER BY VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID";
        string str = @"SELECT customerinfo.CompanyName, CompanyInfo.CompanyName, CompanyInfo.CompAddr1, CompanyInfo.CompAddr2, CompanyInfo.CompAddr3,
            CompanyInfo.CompFax, CompanyInfo.CompTel, CompanyInfo.TinNo, CompanyInfo.Email, customerinfo.Address, VIEW_PERFORMAINVOICEFORDESTINI.FINISHEDID, VIEW_PERFORMAINVOICEFORDESTINI.IFINISHEDID,
            VIEW_PERFORMAINVOICEFORDESTINI.ITEM_NO, VIEW_PERFORMAINVOICEFORDESTINI.GLASSTYPE, VIEW_PERFORMAINVOICEFORDESTINI.QTY, VIEW_PERFORMAINVOICEFORDESTINI.InnerPacking,
            VIEW_PERFORMAINVOICEFORDESTINI.MasterPacking, VIEW_PERFORMAINVOICEFORDESTINI.PROD_NO, customerinfo.Email, VIEW_PERFORMAINVOICEFORDESTINI.MiddlePacking, VIEW_PERFORMAINVOICEFORDESTINI.OQTY,
            VIEW_PERFORMAINVOICEFORDESTINI.PKGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.LBGInstruction, VIEW_PERFORMAINVOICEFORDESTINI.CBM, VIEW_PERFORMAINVOICEFORDESTINI.Remarks, 
            VIEW_PERFORMAINVOICEFORDESTINI.BuyerCode, OrderMaster.DraftOrderID OrderNo, OrderMaster.OrderDate, OrderMaster.CustomerOrderNo CustOrderNo, VIEW_PERFORMAINVOICEFORDESTINI.DESCRIPTION, 
            OrderMaster.DispatchDate DeliveryDate, OrderMaster.SeaPort, GoodsReceipt.StationName, TransMode.transmodeName, VIEW_PERFORMAINVOICEFORDESTINI.weight, customerinfo.CustomerCode, 
            OrderMaster.Custorderdate, VIEW_PERFORMAINVOICEFORDESTINI.IMAGE as photo
            FROM   ((((dbo.CompanyInfo CompanyInfo INNER JOIN dbo.OrderMaster OrderMaster ON CompanyInfo.CompanyId=OrderMaster.CompanyId) INNER JOIN dbo.VIEW_PERFORMAINVOICEFORDESTINI VIEW_PERFORMAINVOICEFORDESTINI ON OrderMaster.DraftOrderID=VIEW_PERFORMAINVOICEFORDESTINI.ORDERID) INNER JOIN dbo.customerinfo customerinfo ON OrderMaster.CustomerId=customerinfo.CustomerId) LEFT OUTER JOIN dbo.GoodsReceipt GoodsReceipt ON OrderMaster.PortOfLoading=GoodsReceipt.GoodsreceiptId) LEFT OUTER JOIN dbo.TransMode TransMode ON OrderMaster.ByAirSea=TransMode.transmodeId  
            Where VIEW_PERFORMAINVOICEFORDESTINI.orderid=" + ViewState["DraftOrderNo"] + @"
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
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        //Session["ReportPath"] = "Reports/RptPerFormaInvoiceDestini3.rpt";
        //Session["CommanFormula"] = "{VIEW_PERFORMAINVOICEFORDESTINI.Orderid}=" + DDCustOrderNo.SelectedValue;
    }
    protected void BtnCancelDraftOrder_Click(object sender, EventArgs e)
    {
        if (DDCustOrderNo.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[1];
                _arrpara[0] = new SqlParameter("@DRAFTOrderId", SqlDbType.Int);
                _arrpara[0].Value = DDCustOrderNo.SelectedValue;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CANCEL_CONFIRM_DRAFT_ORDER", _arrpara);
                Tran.Commit();
                Fill_Order();
                //Fill_CustomerOrderNoSelectedValue();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "CANCELLED SUCCESSFULLY..";
                //refreshform();
                //Fill_Grid();
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
        }
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
    protected void DGPacking_RowCreated(object sender, GridViewRowEventArgs e)
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
        Str = Str + @",DOD.PHOTO IMAGE,DOD.QtyRequired OQTY,PKGInstruction,LBGInstruction,[dbo].[GET_CMB](PD.FINISHEDID) CBM,Remarks,dod.OurCode,BuyerCode,DOD.DESCRIPTION,dod.weight,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as pdescription
        From  ORDER_CONSUMPTION_DETAIL PD,ORDERDETAIL DOD,ITEM_PARAMETER_MASTER IPM,ITEM_PARAMETER_MASTER IPM1,FINISHED_TYPE FT,V_FinishedItemDetail vd WHERE pd.orderid=dod.orderid and  Pd.FINISHEDID=DOD.ITEM_FINISHED_ID AND Pd.FINISHEDID=IPM.ITEM_FINISHED_ID AND PD.IFINISHEDID=IPM1.ITEM_FINISHED_ID AND vd.item_finished_id=pd.IFINISHEDID AND PD.O_FINISHED_TYPE_ID=FT.ID AND PD.PROCESSINPUTID=0 AND DOD.ORDERID=" + ViewState["orderid"] + " And vd.MasterCompanyId=" + Session["varCompanyId"];
        SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        con.Close();
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICEFORDESTINI.Orderid}=" + ViewState["orderid"] + "";
        DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from VIEW_PERFORMAINVOICEFORDESTINIorder where orderid=" + ViewState["orderid"] + "");
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
    protected void TextQtyChanged_Event(object sender, EventArgs e)
    {
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        if (((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text != "" && ((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text != "") // For Area Wise
        {
            if (((Label)DGOrderDetail.Rows[RowIndex].FindControl("Ordercal")).Text == "1")
                DGOrderDetail.Rows[RowIndex].Cells[7].Text = (Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text)).ToString();
            else
                DGOrderDetail.Rows[RowIndex].Cells[7].Text = (Convert.ToDouble(((Label)DGOrderDetail.Rows[RowIndex].FindControl("area1")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text)).ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert(' Qty Or Rate Cann't Be Blank');", true);
        }
        if (((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text != "" && ((Label)DGOrderDetail.Rows[RowIndex].FindControl("area1")).Text != "")
        {
            DGOrderDetail.Rows[RowIndex].Cells[6].Text = (Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text) * Convert.ToDouble(((Label)DGOrderDetail.Rows[RowIndex].FindControl("area1")).Text)).ToString();
        }
        else
        {
            DGOrderDetail.Rows[RowIndex].Cells[6].Text = "0";
        }
    }
    protected void TextRateChanged_Event(object sender, EventArgs e)
    {
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        if (((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text != "" && ((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text != "") // For Area Wise
        {
            if (((Label)DGOrderDetail.Rows[RowIndex].FindControl("Ordercal")).Text == "1")
                DGOrderDetail.Rows[RowIndex].Cells[7].Text = (Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text)).ToString();
            else
                DGOrderDetail.Rows[RowIndex].Cells[7].Text = (Convert.ToDouble(((Label)DGOrderDetail.Rows[RowIndex].FindControl("area1")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtQtyGD")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("TxtRateGD")).Text)).ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert(' Qty Or Rate Cann't Be Blank');", true);
        }
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        }
    }
    protected void BtnReport_Click1(object sender, EventArgs e)
    {
        Report_Type();
    }
}