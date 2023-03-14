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
using ClosedXML.Excel;

public partial class Masters_Packing_FrmInvoiceNewByGrid : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
 @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    static string ChkBoxSelectedSizeId = "0";
    static int ItemFinishedId = 0;
    static string QualityName = "", DesignName = "", ColorName = "";
    static int QualityId = 0, DesignId = 0, ColorId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
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
                typeof(Masters_Packing_FrmInvoiceNewByGrid),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            logo();
            
            string Qry = @" select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CustomerCode
                    Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + @"
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
            //ParameteLabel();
            RDAreaWise.Checked = true;
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            if (VarProdCode == 1)
            {
                TDProdCode.Visible = true;
            }
            else
            {
                TDProdCode.Visible = false;
            }          
            // ViewState["PackingID"] = 0;
            //Session["PackingID"] = 0;
            ViewState["INVOICEDETAILID"] = 0;
            //DDCustomerCode.Focus();
            hnid.Value = "0";
            hnInvoiceID.Value = "0";
            hnfinished.Value = "";

            //if (Session["varcompanyId"].ToString() == "19")
            //{
            //    Label36.Text = "Bale Nt Wt";
            //    Label37.Text = "Bale Gr Wt";
            //}
            //else
            //{
            //    Label36.Text = "One Pcs Nt Wt";
            //    Label37.Text = "One Pcs Gr Wt";
            //}

            //switch (Convert.ToInt16(Session["varcompanyId"]))
            //{
            //    case 19:                    
            //        break;               
            //    default:                                     
            //        break;

            //}
        }
    }
    private void logo()
    {
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void FillBillToShipTO()
    {
        string str = @"select  WHConsignee,WHShipTo from WareHouseMaster 
                    Where CustomerId = " + DDConsignee.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @" ";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            txtBilledTo.Text = Ds.Tables[0].Rows[0]["WHConsignee"].ToString();
            txtShippedTo.Text = Ds.Tables[0].Rows[0]["WHShipTo"].ToString();
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();
        // TxtInvoiceNo.Focus();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        TxtInvoiceNo.Text = "";
        ViewState["InvoiceID"] = 0;

        DGOrderDetail.DataSource = null;
        DGOrderDetail.DataBind();

        UtilityModule.ConditionalComboFill(ref DDConsignee, "Select CustomerId,CompanyName From  CustomerInfo where CustomerID=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Companyname", true, "--SELECT--");
        if (DDConsignee.Items.Count > 0)
        {
            DDConsignee.SelectedIndex = 1;
        }
        FillorderNo();
        if (DDConsignee.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == false)
            {
                FillBillToShipTO();
            }
            else
            {
                string str = @"select  BilledTo,ShippedTo from InvoiceMaster Where Consigneeid = " + DDConsignee.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @" ";

                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    txtBilledTo.Text = Ds.Tables[0].Rows[0]["BilledTo"].ToString();
                    txtShippedTo.Text = Ds.Tables[0].Rows[0]["ShippedTo"].ToString();
                }
            }
        }
        if (ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddInvoiceNo, "Select InvoiceId,InvoiceNo+' / '+Replace(Convert(VarChar(11),InvoiceDate,106), ' ','-') InvoiceNo from InvoiceMaster  Where ConsignorId=" + DDCompanyName.SelectedValue + " And ConsigneeId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        }
    }
    protected void FillorderNo()
    {
        if (variable.Carpetcompany == "1")
        {
            if (Session["varCompanyId"].ToString() == "30")
            {
                string str = "";
                str = @"Select distinct OM.OrderId, OM.CustomerOrderNo CustomerOrderNo from OrderMaster OM JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                        JOIN V_OrderQtyAndPackedQty VO ON OD.OrderId=VO.OrderId and OD.Item_Finished_Id=VO.Item_Finished_ID 
                        Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + @" and (VO.OrderQty-VO.PackedQty)>0  order by CustomerOrderNo";

                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select OrderId,CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + "  order by CustomerOrderNo", true, "--SELECT--");
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " order by OrderId", true, "--SELECT--");
        }
    } 
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerOrderNo_SelectedIndexChanged();
        // ddCategoryName.Focus();

        if (Session["varCompanyNo"].ToString() == "40" || Session["varCompanyNo"].ToString() == "41" || Session["varCompanyNo"].ToString() == "39")
        {
            //if (DGOrderDetail.Rows.Count == 0)
            //{
                string VarUnitID = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                    @"Select Top 1 OrderUnitID From OrderDetail Where OrderID = " + DDCustomerOrderNo.SelectedValue).ToString();

                DDUnit.SelectedValue = VarUnitID;
            //}
        }

        BindPackingOrderDetail();
    }
    private void DDCustomerOrderNo_SelectedIndexChanged()
    {
        Fill_Price();
    }
    protected void GetFinishedIdRateInvoiceQty()
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
                    //TxtTotalPcs.Text = lblBalanceQty.Text;

                    //*********************
                    //***********       

                    SqlParameter[] param = new SqlParameter[17];
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
                    param[9] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
                    param[10] = new SqlParameter("@Rate", SqlDbType.Float);
                    param[10].Direction = ParameterDirection.Output;
                    param[11] = new SqlParameter("@OrderQty", SqlDbType.Int);
                    param[11].Direction = ParameterDirection.Output;
                    param[12] = new SqlParameter("@PreInvoiceQty", SqlDbType.Int);
                    param[12].Direction = ParameterDirection.Output;
                    param[13] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                    param[13].Direction = ParameterDirection.Output;
                    param[14] = new SqlParameter("@ChkWithoutOrder", "0");
                    param[15] = new SqlParameter("@hnsampletype", hnsampletype.Value);
                    //param[16] = new SqlParameter("@RFrom", SqlDbType.Int);
                    //param[16].Direction = ParameterDirection.Output;
                    param[16] = new SqlParameter("@InvoiceId", ViewState["InvoiceID"]);

                    /// param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                    ///param[2].Direction = ParameterDirection.Output;
                    //**************
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_FinishedId_OrderRate_OrderQty_PreInvoiceQty", param);
                    TxtTotalQty.Text = param[11].Value.ToString();
                    TxtPrice.Text = param[10].Value.ToString();
                    TxtPreInvoiceQty.Text = param[12].Value.ToString();
                    //TxtRollNoFrom.Text = param[16].Value.ToString();
                    ItemFinishedId = Convert.ToInt32(param[13].Value.ToString());
                    ////LblErrorMessage.Text = param[2].Value.ToString();
                    //TxtInvoiceQty.Text = TxtTotalPcs.Text;
                    //Tran.Commit();                    
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
    private void SizeSelectedIndexChange(string SizeName, int SizeID)
    {
        if (Session["varCompanyId"].ToString() == "30")
        {
            string DDSizeValue = SizeName;
            //string stringBeforeChar = DDSizeValue.Substring(0, DDSizeValue.IndexOf("["));

            if (SizeName!="")
            {
                string[] splitString = DDSizeValue.Split('x');

                string Width = splitString[0].Trim();
                string Length = splitString[1].Trim();

                TxtWidth.Text = Width;
                TxtLength.Text = Length;
                Check_Length_Width_Format();
                //TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
            }
        }
        else
        {
            string Width = "WidthFt", Length = "LengthFt";
            if (DDUnit.SelectedValue == "1")
            {
                Width = "WidthMtr";
                Length = "LengthMtr";
            }

            string str = "";
            if (ChkBoxSelectedSizeId != "0")
            {
                str = "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + @" Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then AreaMtr Else AreaFt End Area from Size Where SizeId=" + ChkBoxSelectedSizeId;
            }
            else
            {

                str = "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + " Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then AreaMtr Else AreaFt End Area from Size Where SizeId=" + SizeID + " And MasterCompanyId=" + Session["varCompanyId"] + "";
            }
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                TxtLength.Text = Ds.Tables[0].Rows[0]["Length"].ToString();
                TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
            }
        }

        if (Session["varCompanyNo"].ToString() == "16" || Session["varCompanyNo"].ToString() == "40" || Session["varCompanyNo"].ToString() == "41" || Session["varCompanyNo"].ToString() == "39")
        {
            GetFinishedIdRateInvoiceQty();
        }

        //Fill_Price();

    }
    protected void Chkboxitem_CheckedChanged(object sender, EventArgs e)
    {
        //ddSize_SelectedIndexChanged(sender, e);

        CheckBox chkboxitem = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkboxitem.Parent.Parent;
        Label lblSizeId = (Label)row.FindControl("lblSizeId");
        Label lblSIZEname = (Label)row.FindControl("lblSIZEname");

        if (chkboxitem != null)
        {
            if (chkboxitem.Checked == true)
            {
                ChkBoxSelectedSizeId = lblSizeId.Text;
                //ddSize_SelectedIndexChanged(sender, e);
                SizeSelectedIndexChange(lblSIZEname.Text, Convert.ToInt32(lblSizeId.Text));
               
               
            }
            else
            {
                TxtWidth.Text = "";
                TxtLength.Text = "";
                TxtPrice.Text = "";
                TxtArea.Text = "";
                TxtTotalQty.Text = "";
                ChkBoxSelectedSizeId = "0";
                //TxtTotalPcs.Text = "0";
                //TxtBales.Text = "0";
                //TxtRollNoTo.Text = "0";

            }
        }
        BtnSave.Focus();

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
    
   
    private void Fill_Price()
    {
        
       
    }   
    
  
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "You Are Successfully LoggedOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ShapeSelectedChange();

        ////if (Session["VarCompanyId"].ToString() == "30")
        ////{
        ////    ChangeRate();
        ////}
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
    private void Save_Referce()
    {       
        DDUnit.Enabled = false;
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtProdCode.Text = "";
        TxtArea.Text = "";
        ////TxtPrice.Text = "";
        ////TxtTotalPcs.Text = "";
        //TxtPackQty.Text = "";
        TxtRemarks.Text = "";
        hnid.Value = "0";
        //hnpackingid.Value = "0";
       
        TxtTotalQty.Text = "0";
       
        hnsampletype.Value = "1";
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
            string strsql = @"Select IND.InvoiceDetailId,IND.InvoiceId,VF.Category_Name Category,VF.Item_Name ItemName,IND.Quality,IND.Design,IND.Color,ShapeName, 
                            IND.Width+'x'+IND.Length as Size,IND.Qty,IND.Price Rate,IND.Area,
                            Case When " + VarCalType + @"=0 Then Area*Price Else IND.Qty*Price End Amount ,isnull(OM.customerorderNo,'')as CustomerorderNo
                            From InvoiceMaster INM JOIN InvoiceDetail IND ON INM.InvoiceId=IND.InvoiceId
                            inner join V_FinishedItemDetail VF on IND.ItemFinishedId=VF.Item_Finished_id 
                            left join ordermaster OM on IND.Orderid=OM.orderid 
                            Where INM.InvoiceId=" + ViewState["InvoiceID"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "  order by InvoiceDetailId desc";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            //txttotalareagrid.Text = "";
            //txttotalpcsgrid.Text = "";
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txttotalpcsgrid.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
            //    txttotalareagrid.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(Area)", "")), 4).ToString();
            //}

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceNewByGrid.aspx");
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
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
        if (DGOrderDetail.Rows.Count > 0)
        {
            DGOrderDetail.Visible = true;
        }
        else
        {
            DGOrderDetail.Visible = false;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            int Qty = 0, TempOrderId = 0, TempOrderDetailId = 0;
            for (int i = 0; i < GVPackingOrderDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)GVPackingOrderDetail.Rows[i].FindControl("Chkboxitem"));
                TextBox TxtInvoiceQty = ((TextBox)GVPackingOrderDetail.Rows[i].FindControl("TxtInvoiceQty"));
                Label lblOrderDetailId = ((Label)GVPackingOrderDetail.Rows[i].FindControl("lblOrderDetailId"));

                if (TxtInvoiceQty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                    TxtInvoiceQty.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToDecimal(TxtInvoiceQty.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                    TxtInvoiceQty.Focus();
                    return;
                }

                if (Chkboxitem.Checked == true && (TxtInvoiceQty.Text != "") && DDConsignee.SelectedIndex > 0 && DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Qty = Convert.ToInt32(TxtInvoiceQty.Text);
                    TempOrderDetailId = Convert.ToInt32(lblOrderDetailId.Text);
                }

            }
            string VarInvoiceYear = "";

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[38];
                arr[0] = new SqlParameter("@InvoiceID", SqlDbType.Int);
                arr[1] = new SqlParameter("@InvoiceNo", SqlDbType.VarChar, 50);
                arr[2] = new SqlParameter("@ConsignorId", SqlDbType.Int);
                arr[3] = new SqlParameter("@ConsigneeId", SqlDbType.Int);
                arr[4] = new SqlParameter("@InvoiceDate", SqlDbType.DateTime);
                arr[5] = new SqlParameter("@SupplyDate", SqlDbType.DateTime);
                arr[6] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                arr[7] = new SqlParameter("@UnitId", SqlDbType.Int);
                arr[8] = new SqlParameter("@BilledTo", SqlDbType.VarChar, 500);
                arr[9] = new SqlParameter("@ShippedTo", SqlDbType.VarChar, 500);
                arr[10] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
                arr[11] = new SqlParameter("@UserId", SqlDbType.Int);
                arr[12] = new SqlParameter("@InvoiceYear", SqlDbType.Int);

                arr[13] = new SqlParameter("@InvoiceDetailId", SqlDbType.Int);
                arr[14] = new SqlParameter("@ItemFinishedId", SqlDbType.Int);
                arr[15] = new SqlParameter("@Width", SqlDbType.VarChar, 20);
                arr[16] = new SqlParameter("@Length", SqlDbType.VarChar, 20);
                arr[17] = new SqlParameter("@Area", SqlDbType.Float);
                arr[18] = new SqlParameter("@Price", SqlDbType.Float);
                arr[19] = new SqlParameter("@Qty", SqlDbType.Int);
                arr[20] = new SqlParameter("@Amount", SqlDbType.Float);
                arr[21] = new SqlParameter("@OrderId", SqlDbType.Int);
                arr[22] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                arr[23] = new SqlParameter("@CalTypeAmt", SqlDbType.Int);
                arr[24] = new SqlParameter("@GSTTYPE", SqlDbType.Int);
                arr[25] = new SqlParameter("@CGST", SqlDbType.Float);
                arr[26] = new SqlParameter("@SGST", SqlDbType.Float);
                arr[27] = new SqlParameter("@IGST", SqlDbType.Float);
                arr[28] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 30);
                arr[29] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 50);
                arr[30] = new SqlParameter("@Remark", SqlDbType.VarChar, 300);
                arr[31] = new SqlParameter("@Quality", SqlDbType.VarChar, 50);
                arr[32] = new SqlParameter("@Design", SqlDbType.VarChar, 50);
                arr[33] = new SqlParameter("@Color", SqlDbType.VarChar, 50);
                arr[34] = new SqlParameter("@CQSRNO", SqlDbType.Int);
                arr[35] = new SqlParameter("@CDSRNO", SqlDbType.Int);
                arr[36] = new SqlParameter("@CCSRNO", SqlDbType.Int);
                arr[37] = new SqlParameter("@msg", SqlDbType.VarChar, 200);


                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = ViewState["InvoiceID"];
                arr[1].Direction = ParameterDirection.InputOutput;
                arr[1].Value = TxtInvoiceNo.Text;
                arr[2].Value = DDCompanyName.SelectedValue;
                arr[3].Value = DDCustomerCode.SelectedValue;
                arr[4].Value = TxtDate.Text;
                arr[5].Value = TxtDate.Text;
                arr[6].Value = DDCurrency.SelectedValue;
                arr[7].Value = DDUnit.SelectedValue;
                arr[8].Value = txtBilledTo.Text;
                arr[9].Value = txtShippedTo.Text;
                arr[10].Value = Session["varCompanyId"];
                arr[11].Value = Session["varuserid"];
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
                arr[12].Value = VarInvoiceYear;
                arr[13].Value = 0;
                arr[14].Value = ItemFinishedId;
                arr[15].Value = TxtWidth.Text;
                arr[16].Value = TxtLength.Text;
                arr[17].Value = Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(Qty);
                arr[18].Value = TxtPrice.Text;
                arr[19].Value = Qty;
                if (RDAreaWise.Checked == true)
                {
                    arr[20].Value = (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(Qty)) * Convert.ToDouble(TxtPrice.Text);
                }
                else
                {
                    arr[20].Value = (Convert.ToDouble(Qty) * Convert.ToDouble(TxtPrice.Text));
                }
                arr[21].Value = DDCustomerOrderNo.SelectedValue;
                arr[22].Value = TempOrderDetailId;
                arr[23].Value = RDAreaWise.Checked == true ? 0 : 1;
                arr[24].Value = 0;
                arr[25].Value = txtCGST.Text == "" ? "0" : txtCGST.Text;
                arr[26].Value = txtSGST.Text == "" ? "0" : txtSGST.Text;
                arr[27].Value = txtIGST.Text == "" ? "0" : txtIGST.Text;
                arr[28].Value = txtVehicleNo.Text;
                arr[29].Value = txtEWayBillBo.Text;
                arr[30].Value = TxtRemarks.Text;
                arr[31].Value = QualityName;
                arr[32].Value = DesignName;
                //_arrpara[31].Value = ddColor.SelectedItem.Text;
                arr[33].Value = ColorName;
                arr[34].Value = QualityId;
                arr[35].Value = DesignId;
                arr[36].Value = ColorId;
                arr[37].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_SAVEINVOICEDETAILBYGRID]", arr);
                //ViewState["MaterialIssueID"] = arr[0].Value;
                hnInvoiceID.Value = arr[0].Value.ToString();
                ViewState["InvoiceID"] = hnInvoiceID.Value;
             
                tran.Commit();
                if (arr[37].Value.ToString() != "")
                {
                    LblErrorMessage.Text = arr[37].Value.ToString();
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[37].Value.ToString() + "');", true);
                }

                BtnSave.Text = "Save";
                TxtInvoiceNo.Text = Convert.ToString(arr[1].Value);              
                Fill_Grid();
                Save_Referce();

                if (Session["VarCompanyNo"].ToString() == "40" || Session["VarCompanyNo"].ToString() == "41"  || Session["VarCompanyNo"].ToString() == "39")
                {
                    BindPackingOrderDetail();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        

        //CHECKVALIDCONTROL();
        //if (LblErrorMessage.Text == "")
        //{
        //    //int BalanceQty = (Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text));
        //    //if (Convert.ToInt32(TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text) > BalanceQty)
        //    //{
        //    //    TxtPackQty.Text = "0";
        //    //    TxtTotalPcs.Text = "0";
        //    //    TxtBales.Text = "0";
        //    //    TxtRollNoTo.Text = "0";
        //    //    TxtTotalPcs.Focus();
        //    //    LblErrorMessage.Text = "Pack Pcs Qty Can't be greater then balance qty " + BalanceQty + "";
        //    //    //ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Pack Pcs Qty Can't be greater then balance qty '" + BalanceQty + ");", true);
        //    //    return;
        //    //}
            
        //        ViewState["INVOICEDETAILID"] = 0;
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        con.Open();
        //        SqlTransaction Tran = con.BeginTransaction();
        //        try
        //        {
        //            int num = 0;
        //            string VarInvoiceYear = "";
        //            if (Convert.ToInt32(ViewState["INVOICEID"]) == 0)
        //            {
        //                num = 1;
        //            }
        //            if (num == 1)
        //            {
        //                SqlParameter[] _arrpara1 = new SqlParameter[11];

        //                _arrpara1[0] = new SqlParameter("@PackingId", SqlDbType.Int);
        //                _arrpara1[1] = new SqlParameter("@ConsignorId", SqlDbType.Int);
        //                _arrpara1[2] = new SqlParameter("@ConsigneeId", SqlDbType.Int);
        //                _arrpara1[3] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
        //                _arrpara1[4] = new SqlParameter("@CurrencyId", SqlDbType.Int);
        //                _arrpara1[5] = new SqlParameter("@UnitId", SqlDbType.Int);
        //                _arrpara1[6] = new SqlParameter("@varuserid", SqlDbType.Int);
        //                _arrpara1[7] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
        //                _arrpara1[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);
        //                _arrpara1[9] = new SqlParameter("@InvoiceYear", SqlDbType.Int);
        //                _arrpara1[10] = new SqlParameter("@Msg", SqlDbType.NVarChar, 50);


        //                if (ddInvoiceNo.Visible == true)
        //                {
        //                    _arrpara1[0].Value = ddInvoiceNo.SelectedValue;
        //                }
        //                else
        //                {
        //                    _arrpara1[0].Value = 0;
        //                }
        //                _arrpara1[0].Direction = ParameterDirection.InputOutput;
        //                _arrpara1[1].Value = DDCompanyName.SelectedValue;
        //                _arrpara1[2].Value = DDCustomerCode.SelectedValue;
        //                _arrpara1[3].Value = TxtDate.Text;
        //                _arrpara1[4].Value = DDCurrency.SelectedValue;
        //                _arrpara1[5].Value = DDUnit.SelectedValue;
        //                _arrpara1[6].Value = Session["varuserid"];
        //                _arrpara1[7].Value = TxtInvoiceNo.Text;
        //                _arrpara1[8].Value = Session["varCompanyId"];
        //                string VarInvoiceMonth = DateTime.Now.ToString("MM");
        //                if (Convert.ToInt32(VarInvoiceMonth) >= 04)
        //                {
        //                    VarInvoiceYear = DateTime.Now.ToString("yyyy");
        //                }
        //                else
        //                {
        //                    VarInvoiceYear = DateTime.Now.ToString("yyyy");
        //                    VarInvoiceYear = (Convert.ToInt32(VarInvoiceYear) - 1).ToString();
        //                }
        //                _arrpara1[9].Value = VarInvoiceYear;
        //                _arrpara1[10].Direction = ParameterDirection.Output;


        //                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKING_AND_INVOICE", _arrpara1);
        //                ViewState["PackingID"] = _arrpara1[0].Value;
        //                if (_arrpara1[10].Value.ToString() != "")  //Check For Duplicate InvoiceNo
        //                {
        //                    LblErrorMessage.Visible = true;
        //                    LblErrorMessage.Text = _arrpara1[10].Value.ToString();
        //                    Tran.Rollback();
        //                    return;
        //                }
        //                LblErrorMessage.Text = "";
        //                BtnSave.Text = "Save";
        //            }
        //            SqlParameter[] _arrpara = new SqlParameter[59];
        //            _arrpara[0] = new SqlParameter("@PackingId", SqlDbType.Int);
        //            _arrpara[1] = new SqlParameter("@RollNo", SqlDbType.Int);
        //            _arrpara[2] = new SqlParameter("@PcsFrom", SqlDbType.Int);
        //            _arrpara[3] = new SqlParameter("@PcsTo", SqlDbType.Int);
        //            _arrpara[4] = new SqlParameter("@ArticleNo", SqlDbType.NVarChar, 20);
        //            _arrpara[5] = new SqlParameter("@Extra1", SqlDbType.NVarChar, 20);
        //            _arrpara[6] = new SqlParameter("@Extra2", SqlDbType.NVarChar, 20);
        //            _arrpara[7] = new SqlParameter("@FinishedID", SqlDbType.Int);
        //            _arrpara[8] = new SqlParameter("@Width", SqlDbType.NVarChar, 10);
        //            _arrpara[9] = new SqlParameter("@Length", SqlDbType.NVarChar, 10);
        //            _arrpara[10] = new SqlParameter("@Pcs", SqlDbType.Int);
        //            _arrpara[11] = new SqlParameter("@Area", SqlDbType.Float);
        //            _arrpara[12] = new SqlParameter("@Price", SqlDbType.Float);
        //            _arrpara[13] = new SqlParameter("@SrNo", SqlDbType.Int);
        //            _arrpara[14] = new SqlParameter("@OrderId", SqlDbType.Int);
        //            _arrpara[15] = new SqlParameter("@StockNo", SqlDbType.Int);
        //            _arrpara[16] = new SqlParameter("@TStockNo", SqlDbType.NVarChar, 8000);
        //            _arrpara[17] = new SqlParameter("@Id", SqlDbType.Int);
        //            _arrpara[18] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
        //            _arrpara[19] = new SqlParameter("@RollFrom", SqlDbType.Int);
        //            _arrpara[20] = new SqlParameter("@RollTo", SqlDbType.Int);
        //            _arrpara[21] = new SqlParameter("@RPcs", SqlDbType.Int);
        //            _arrpara[22] = new SqlParameter("@TotalPcs", SqlDbType.Int);
        //            _arrpara[23] = new SqlParameter("@TotalRoll", SqlDbType.Int);
        //            _arrpara[24] = new SqlParameter("@CompStockNo", SqlDbType.Int);
        //            _arrpara[25] = new SqlParameter("@CustStockNo", SqlDbType.Int);
        //            _arrpara[26] = new SqlParameter("@Pack", SqlDbType.Int);
        //            _arrpara[27] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 50);
        //            _arrpara[28] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
        //            _arrpara[29] = new SqlParameter("@Quality", SqlDbType.NVarChar, 50);
        //            _arrpara[30] = new SqlParameter("@Design", SqlDbType.NVarChar, 50);
        //            _arrpara[31] = new SqlParameter("@Color", SqlDbType.NVarChar, 50);
        //            _arrpara[32] = new SqlParameter("@CQSRNO", SqlDbType.Int);
        //            _arrpara[33] = new SqlParameter("@CDSRNO", SqlDbType.Int);
        //            _arrpara[34] = new SqlParameter("@CCSRNO", SqlDbType.Int);
        //            _arrpara[35] = new SqlParameter("@CalTypeAmt", SqlDbType.Int);
        //            _arrpara[36] = new SqlParameter("@MulipleRollFlag", SqlDbType.Int);
        //            _arrpara[37] = new SqlParameter("@PakingDetailID", SqlDbType.Int);
        //            _arrpara[38] = new SqlParameter("@QualityCodeID", SqlDbType.Int);
        //            _arrpara[39] = new SqlParameter("@Sub_Quality", SqlDbType.NVarChar, 450);
        //            _arrpara[40] = new SqlParameter("@NetWt", SqlDbType.Float);
        //            _arrpara[41] = new SqlParameter("@GrossWt", SqlDbType.Float);
        //            _arrpara[42] = new SqlParameter("@PurchaseCode", SqlDbType.NVarChar, 250);
        //            _arrpara[43] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar, 250);
        //            _arrpara[44] = new SqlParameter("@UCCNumber", SqlDbType.VarChar, 50);
        //            _arrpara[45] = new SqlParameter("@RUGID", SqlDbType.VarChar, 50);
        //            _arrpara[46] = new SqlParameter("@Withbuyercode", SqlDbType.TinyInt);
        //            _arrpara[47] = new SqlParameter("@BaleL", SqlDbType.VarChar, 10);
        //            _arrpara[48] = new SqlParameter("@BaleW", SqlDbType.VarChar, 10);
        //            _arrpara[49] = new SqlParameter("@BaleH", SqlDbType.VarChar, 10);
        //            _arrpara[50] = new SqlParameter("@CBM", SqlDbType.Float);
        //            _arrpara[51] = new SqlParameter("@SinglePcsNetWt", SqlDbType.Float);
        //            _arrpara[52] = new SqlParameter("@SinglePcsGrossWt", SqlDbType.Float);
        //            _arrpara[53] = new SqlParameter("@RatePerPcs", SqlDbType.Float);
        //            _arrpara[54] = new SqlParameter("@StyleNo", SqlDbType.VarChar, 20);
        //            _arrpara[55] = new SqlParameter("@FromTrackingNo", SqlDbType.VarChar, 25);
        //            _arrpara[56] = new SqlParameter("@ToTrackingNo", SqlDbType.VarChar, 25);
        //            _arrpara[57] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20);
        //            _arrpara[58] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 25);

        //            //Select PackingId,RollNo,PcsFrom,PcsTo,ArticleNo,Extra1,Extra2,FinishedId,Width,Length,Pcs,Area,Price,SrNo,OrderId,StockNo,TStockNo,Id,TPackingNo,
        //            //RollFrom,RollTo,RPcs,TotalPcs,TotalRoll,CompStockNo,CustStockNo,Pack,Remarks From PackingInformation
        //            _arrpara[0].Value = ViewState["PackingID"];
        //            _arrpara[1].Value = TxtRollNoFrom.Text;
        //            _arrpara[2].Value = TxtBales.Text;
        //            _arrpara[3].Value = 0;
        //            _arrpara[4].Value = "";
        //            _arrpara[5].Value = "";
        //            _arrpara[6].Value = "";

        //            _arrpara[8].Value = TxtWidth.Text;
        //            _arrpara[9].Value = TxtLength.Text;
        //            _arrpara[10].Value = TxtTotalPcs.Text;
        //            _arrpara[11].Value = Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtTotalPcs.Text);
        //            _arrpara[12].Value = TxtPrice.Text;
        //            _arrpara[13].Value = TxtSrNo.Text == "" ? "0" : TxtSrNo.Text;
        //            _arrpara[14].Value = ChkForWithoutOrder.Checked == true ? "0" : DDCustomerOrderNo.SelectedValue;
        //            _arrpara[15].Value = 0;
        //            if (hnid.Value == "0")
        //                _arrpara[17].Value = ViewState["PACKINGDETAILID"];

        //            else
        //                _arrpara[17].Value = hnid.Value;
        //            _arrpara[18].Value = TxtInvoiceNo.Text;
        //            _arrpara[19].Value = TxtRollNoFrom.Text;
        //            _arrpara[20].Value = TxtRollNoTo.Text;
        //            _arrpara[21].Value = TxtPcsPerRoll.Text;
        //            _arrpara[22].Value = TxtTotalPcs.Text;
        //            _arrpara[23].Value = TxtBales.Text;
        //            _arrpara[24].Value = 0;
        //            _arrpara[25].Value = 0;
        //            _arrpara[26].Value = 0;
        //            _arrpara[27].Value = TxtRemarks.Text;
        //            _arrpara[28].Value = TxtDate.Text;

        //            if (ChkForWithoutOrder.Checked == true || chksamplepack.Checked == true)
        //            {
        //                _arrpara[29].Value = ddQuality.SelectedItem.Text;
        //                _arrpara[30].Value = ddDesign.SelectedItem.Text;
        //                _arrpara[31].Value = ddColor.SelectedItem == null ? "" : ddColor.SelectedItem.Text;
        //                _arrpara[32].Value = ddQuality.SelectedValue;
        //                _arrpara[33].Value = ddDesign.SelectedValue;
        //                _arrpara[34].Value = ddColor.SelectedValue;
        //            }
        //            else
        //            {

        //                _arrpara[29].Value = QualityName;
        //                _arrpara[30].Value = DesignName;
        //                //_arrpara[31].Value = ddColor.SelectedItem.Text;
        //                _arrpara[31].Value = ColorName;
        //                _arrpara[32].Value = QualityId;
        //                _arrpara[33].Value = DesignId;
        //                _arrpara[34].Value = ColorId;
        //            }

        //            _arrpara[35].Value = RDAreaWise.Checked == true ? 0 : 1;
        //            _arrpara[36].Value = ChkForMulipleRolls.Checked == true ? 1 : 0;
        //            _arrpara[37].Direction = ParameterDirection.Output;
        //            //_arrpara[38].Value = DDSubQuality.SelectedValue;
        //            //_arrpara[39].Value = DDSubQuality.SelectedItem.Text;
        //            _arrpara[38].Value = 0;
        //            _arrpara[39].Value = "";
        //            _arrpara[40].Value = 0;
        //            _arrpara[41].Value = 0;
        //            _arrpara[42].Value = Txtpurchasecode.Text;
        //            _arrpara[43].Value = TxtBuyer.Text;
        //            _arrpara[44].Value = txtuccnumber.Text;
        //            _arrpara[45].Value = txtrugid.Text;
        //            _arrpara[46].Value = variable.Withbuyercode;

        //            _arrpara[47].Value = txtlengthBale.Text;
        //            _arrpara[48].Value = txtwidthBale.Text;
        //            _arrpara[49].Value = txtheightbale.Text;
        //            _arrpara[50].Value = txtcbmbale.Text == "" ? "0" : txtcbmbale.Text;
        //            _arrpara[51].Value = txtSinglePcsNetWt.Text == "" ? "0" : txtSinglePcsNetWt.Text;
        //            _arrpara[52].Value = txtSinglePcsGrossWt.Text == "" ? "0" : txtSinglePcsGrossWt.Text;
        //            _arrpara[53].Value = TxtRatePerPcs.Text == "" ? "0" : TxtRatePerPcs.Text;
        //            _arrpara[54].Value = txtStyleNo.Text;
        //            _arrpara[55].Value = txtFromTrackingNo.Text;
        //            _arrpara[56].Value = txtToTrackingNo.Text;
        //            _arrpara[57].Value = txtVehicleNo.Text;
        //            _arrpara[58].Value = txtEWayBillBo.Text;



        //            if (ChkForWithoutOrder.Checked == true || chksamplepack.Checked == true)
        //            {
        //                int VarQuality = 0; int VarDesign = 0; int VarColor = 0;
        //                #region
        //                //if (TxtStockNo.Text != "" && TxtStockNo.Text != "0")
        //                //{
        //                //    if (ddQuality.Visible == true)
        //                //    {
        //                //        VarQuality = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.QualityId from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
        //                //    }

        //                //    if (ddDesign.Visible == true)
        //                //    {
        //                //        VarDesign = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.DesignID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
        //                //    }

        //                //    if (ddColor.Visible == true)
        //                //    {
        //                //        VarColor = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.colorID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
        //                //    }
        //                //}
        //                //else
        //                //{
        //                #endregion
        //                VarQuality = Convert.ToInt32(ddQuality.SelectedValue) > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
        //                VarDesign = Convert.ToInt32(ddDesign.SelectedValue) > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
        //                VarColor = ddColor.Visible == true && Convert.ToInt32(ddColor.SelectedValue) > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0;
        //                //}
        //                int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
        //                int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
        //                int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;

        //                if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        //                {
        //                    _arrpara[7].Value = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //                }
        //                else
        //                {
        //                    //_arrpara[7].Value = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"]));
        //                    _arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"])));
        //                }
        //            }
        //            else
        //            {
        //                _arrpara[7].Value = ItemFinishedId;
        //            }



        //            //_arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"])));
        //            string stockno11 = "";
        //            //if (ChkForMulipleRolls.Checked == true)
        //            //{
        //            for (int i = 0; i < DGStock.Rows.Count; i++)
        //            {
        //                GridViewRow row = DGStock.Rows[i];
        //                if (((CheckBox)row.FindControl("Chkbox")).Checked == true)
        //                {
        //                    if (stockno11 == "")
        //                    {

        //                        stockno11 = DGStock.DataKeys[i].Value.ToString();
        //                    }
        //                    else
        //                    {
        //                        stockno11 = stockno11 + ',' + DGStock.DataKeys[i].Value.ToString();
        //                    }

        //                }
        //            }
        //            _arrpara[16].Value = stockno11;
        //            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
        //            ViewState["PACKINGDETAILID"] = _arrpara[37].Value;
        //            //}
        //            //else
        //            //{
        //            //    _arrpara[16].Value = TxtStockNo.Text.ToUpper();
        //            //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
        //            //}
        //            Tran.Commit();
        //            Fill_Grid();
        //            Save_Referce();
        //            DGStock.DataSource = null;
        //            DGStock.DataBind();
        //            TxtStockNo.Focus();

        //            if ((ChkForWithoutOrder.Checked == false && Session["VarCompanyNo"].ToString() == "16") || ChkForWithoutOrder.Checked == false && Session["VarCompanyNo"].ToString() == "40")
        //            {
        //                BindPackingOrderDetail();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
        //            Tran.Rollback();
        //            LblErrorMessage.Visible = true;
        //            LblErrorMessage.Text = ex.Message;
        //            if (DGOrderDetail.Rows.Count == 0)
        //            {
        //                ViewState["PackingID"] = 0;
        //            }
        //        }
        //        finally
        //        {
        //            con.Close();
        //            con.Dispose();
        //        }
            
        //    //else { LblErrorMessage.Text = "Total Quantity can't be greater than available quantity"; }
        //}
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
        //if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
        {
            goto a;
        }
        
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
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }    
   
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        
        TRSearchInvoiceNo.Visible = false;
        if (ChkForEdit.Checked == true)
        {
            TRSearchInvoiceNo.Visible = true;
            TDDDInvoiceNo.Visible = true;
            ddInvoiceNo.Items.Clear();          
            CustomerCodeSelectedIndexChange();           
            Save_Referce();
            ViewState["INVOICEID"] = 0;            
        }
        else
        {
            TRSearchInvoiceNo.Visible = false;
            
            TDDDInvoiceNo.Visible = false;
            ddInvoiceNo.Items.Clear();          
            Save_Referce();
            TxtInvoiceNo.Text = "";
            TxtDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            DGOrderDetail.Visible = false;
            hnid.Value = "0";
           hnInvoiceID.Value = "0";
            ViewState["InvoiceID"] = 0;
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
        string strsql = "";
        strsql = "select CalTypeAmt from InvoiceDetail where InvoiceId=" + ddInvoiceNo.SelectedValue + @"";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //VarCalType = Convert.ToInt16(ds.Tables[0].Rows[0]["caltypeamt"]);
            if (Convert.ToInt16(ds.Tables[0].Rows[0]["caltypeamt"]) == 0)
            {
                RDAreaWise.Checked = true;
            }
            else
            {
                RDPcsWise.Checked = true;
            }

        }

        string str = @"Select INM.InvoiceId,INM.InvoiceNo,Replace(Convert(VarChar(11),INM.InvoiceDate,106), ' ','-') InvoiceDate,INM.CurrencyId,INM.UnitId,IND.CGST,
                    IND.SGST,IND.IGST,IND.VehicleNo,IND.EWayBillNo 
                    From InvoiceMaster INM(Nolock) JOIN INVOICEDETAIL IND(NoLock) ON INM.InvoiceId=IND.InvoiceId    
                    Where INM.InvoiceId = " + ddInvoiceNo.SelectedValue + " And INM.MasterCompanyId=" + Session["varCompanyId"] + @" ";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
            TxtInvoiceNo.Text = Ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            ViewState["InvoiceID"] = Ds.Tables[0].Rows[0]["InvoiceId"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["InvoiceDate"].ToString();

            txtCGST.Text = Ds.Tables[0].Rows[0]["CGST"].ToString();
            txtSGST.Text = Ds.Tables[0].Rows[0]["SGST"].ToString();
            txtIGST.Text = Ds.Tables[0].Rows[0]["IGST"].ToString();
            txtVehicleNo.Text = Ds.Tables[0].Rows[0]["VehicleNo"].ToString();
            txtEWayBillBo.Text = Ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            txtCGST.Enabled = false;
            txtSGST.Enabled = false;
            txtIGST.Enabled = false;
            txtVehicleNo.Enabled = false;
            txtEWayBillBo.Enabled = false;
            RDAreaWise.Enabled = false;
            RDPcsWise.Enabled = false;


            Fill_Grid();


            //ChkForChangeQDCCheckedChange();
            ////TRcarpetSet.Visible = true;
            //////From Roll

        }
        

       
    }
   
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);

        //    for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
        //    {
        //        if (Session["varcompanyId"].ToString() == "30")
        //        {
        //            if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
        //            {
        //                DGOrderDetail.Columns[i].Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
        //            {
        //                DGOrderDetail.Columns[i].Visible = false;
        //            }
        //        }
        //    }
        //}
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId from V_FinishedItemDetail v ,orderdetail od Where OD.Item_Finished_Id=V.Item_Finished_Id and od.orderid=" + DDCustomerOrderNo.SelectedValue + " and ProductCode='" + TxtProdCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
               Fill_Price();
           
        }
        else
        {
            LblErrorMessage.Text = "Product Code Does Not Exist";
            LblErrorMessage.Visible = true;
        }
    }
    protected void TxtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        //string str = "";
        //if (ChkForEdit.Checked == true)
        //{
        //    str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And PackingId<>" + ddInvoiceNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
        //}
        //else
        //{
        //    str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And MasterCompanyId=" + Session["varCompanyId"] + ""));
        //}
        //if (str != "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Opn1", "alert('Invoice Number " + str + " Already Exists...');", true);
        //    TxtInvoiceNo.Text = "";
        //}
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
            array[0] = new SqlParameter("@InvoiceId", SqlDbType.Int);
            array[1] = new SqlParameter("@InvoiceDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("lblInvoiceId")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteInvoiceDetail", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Delete", "alert('" + array[2].Value + "');", true);
            Fill_Grid();
            if (DGOrderDetail.Rows.Count == 0)
            {
                DDUnit.Enabled = true;
            }
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
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@InvoiceId", SqlDbType.Int);
            array[1] = new SqlParameter("@InvoiceDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);            
            array[3] = new SqlParameter("@Rate", SqlDbType.Float);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("lblInvoiceId")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;            
            array[3].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtRate")).Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpDateInvoiceDetail]", array);
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
        //LblErrorMessage.Visible = false;
        //LblErrorMessage.Text = "";
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    SqlParameter[] param = new SqlParameter[4];
        //    param[0] = new SqlParameter("@Invoiceid", ddInvoiceNo.SelectedValue);
        //    param[1] = new SqlParameter("@InvoiceNo", SqlDbType.VarChar, 100);
        //    param[1].Direction = ParameterDirection.InputOutput;
        //    param[1].Value = TxtInvoiceNo.Text;
        //    param[2] = new SqlParameter("@Invoicedate", SqlDbType.VarChar, 50);
        //    param[2].Direction = ParameterDirection.InputOutput;
        //    param[2].Value = TxtDate.Text;
        //    param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        //    param[3].Direction = ParameterDirection.Output;
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEINVOICENODATE", param);
        //    Tran.Commit();
        //    LblErrorMessage.Visible = true;
        //    LblErrorMessage.Text = param[3].Value.ToString();

        //}
        //catch (Exception ex)
        //{
        //    Tran.Commit();
        //    LblErrorMessage.Visible = true;
        //    LblErrorMessage.Text = ex.Message;
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    protected void DGOrderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //DGOrderDetail.PageIndex = e.NewPageIndex;
        //Fill_Grid();
    }
    protected void TxtSearchInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtSearchInvoiceNo.Text != "")
        {
            try
            {
                //string str = @"select consignorId,cosigneeId,InvoiceYear,InvoiceId,TInvoiceNo From INVOICE With (Nolock) Where Consignorid = " + Session["CurrentWorkingCompanyID"] + " And Tinvoiceno='" + TxtSearchInvoiceNo.Text + "'";

                string str = @"select consignorId,ConsigneeId,InvoiceYear,InvoiceId,InvoiceNo From InvoiceMaster With (Nolock) Where Consignorid =" + Session["CurrentWorkingCompanyID"] + " And InvoiceNo='" + TxtSearchInvoiceNo.Text + "'";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DDCustomerCode.Items.FindByValue(ds.Tables[0].Rows[0]["ConsigneeId"].ToString()) != null)
                    {
                        DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["ConsigneeId"].ToString();
                        CustomerCodeSelectedIndexChange();
                    }
                    if (ddInvoiceNo.Items.FindByValue(ds.Tables[0].Rows[0]["InvoiceId"].ToString()) != null)
                    {
                        ddInvoiceNo.SelectedValue = ds.Tables[0].Rows[0]["InvoiceId"].ToString();
                        ddInvoiceNo_SelectedIndexChanged(sender, new EventArgs());
                    }
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
    //protected void ChangeRate()
    //{
    //    string OrderUnitId = "", PackingUnitid = "";
    //    string str = @"Select Distinct OM.OrderId,OD.OrderUnitid,OD.ordercaltype From OrderMaster(NoLock) OM JOIN OrderDetail (NoLock) OD ON OM.OrderId=OD.OrderId Where OM.OrderId=" + DDCustomerOrderNo.SelectedValue + "";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        OrderUnitId = ds.Tables[0].Rows[0]["OrderUnitid"].ToString();
    //    }
    //    if (DDUnit.SelectedIndex > 0)
    //    {
    //        PackingUnitid = DDUnit.SelectedValue;
    //    }
    //    if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "1")
    //    {
    //        if ((RDAreaWise.Checked))
    //        {
    //            TxtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) / Convert.ToDecimal(TxtArea.Text), 2));
    //        }
    //    }
    //    if (OrderUnitId != "" && PackingUnitid != "")
    //    {
    //        if (OrderUnitId == "2" && PackingUnitid == "1")
    //        {
    //            TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) / 10.764), 2));

    //            TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
    //        }
    //        if (OrderUnitId == "1" && PackingUnitid == "2")
    //        {
    //            TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) * 10.764), 2));

    //            TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
    //        }
    //    }
    //}

    protected void GVPackingOrderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVPackingOrderDetail.PageIndex = e.NewPageIndex;
        BindPackingOrderDetail();
    }
    private DataSet BindPackingOrderDetail()
    {
        DataSet DS = new DataSet();

        string str = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GetInvoiceOrderDetailByGrid", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerCodeId", DDCustomerCode.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerOrderId", DDCustomerOrderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@hnsampletype", hnsampletype.Value);
        //cmd.Parameters.AddWithValue("@ChkWithoutOrder", ChkForWithoutOrder.Checked == false ? "0" : "1");
        cmd.Parameters.AddWithValue("@UnitId", DDUnit.SelectedValue);        


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
        //DataSet ddd = new DataSet();
        //ddd = BindPackingOrderDetail();
        //DataTable dt = ddd.Tables[0];
        //string sortingDirection = string.Empty;
        //if (dir == SortDirection.Ascending)
        //{
        //    dir = SortDirection.Descending;
        //    sortingDirection = "Desc";
        //}
        //else
        //{
        //    dir = SortDirection.Ascending;
        //    sortingDirection = "Asc";
        //}
        //DataView sortedView = new DataView(dt);
        //sortedView.Sort = e.SortExpression + " " + sortingDirection;
        ////Session["objects"] = sortedView;
        //GVPackingOrderDetail.DataSource = sortedView;
        //GVPackingOrderDetail.DataBind();
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

            cmd.Parameters.AddWithValue("@InvoiceId", ViewState["InvoiceID"]);
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

                    sht.Cell("A6").Value = "KRIPA RUGS";
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


                    sht.Cell("A7").Value = "INVOICE NO:" + " " + ds.Tables[0].Rows[0]["InvoiceNo"];
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
    protected void InvoiceDetailReportInPDF()
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

            cmd.Parameters.AddWithValue("@InvoiceId", ViewState["InvoiceID"]);
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
                Session["rptFileName"] = "Reports/RptInvoiceDetailReport.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptInvoiceDetailReport.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
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
        if (Session["VarCompanyNo"].ToString() == "41")
        {
            InvoiceDetailReportInPDF();
        }
        else
        {
            InvoiceDetailReport();
        }
    }
}