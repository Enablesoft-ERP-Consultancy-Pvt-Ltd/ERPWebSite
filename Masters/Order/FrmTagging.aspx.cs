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
public partial class Masters_Order_FrmTagging : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDLInCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDLInCompanyName, @"select CI.CompanyId,CompanyName From CompanyInfo CI(Nolock),Company_Authentication CA(Nolock) 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "--Select--");

            if (DDLInCompanyName.Items.Count > 0)
            {
                DDLInCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDLInCompanyName.Enabled = false;
            }

            if (Convert.ToInt32(Session["varcompanyno"]) == 7)
            {
                ChkForConsumption.Visible = false;
                lbldeldate.Text = "Stock At PH";
                ChkForEdit.Checked = true;
                BtnForProductionItem.Visible = false;
            }
            CompanyNameSelectedIndexChanged();
            if (DDLCustomerCode.Items.Count > 0)
            {
                DDLCustomerCode.SelectedIndex = 1;
            }
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            DDLCustomerCode.Focus();
            EditCheckedChanged();
            if (Convert.ToInt32(Session["varcompanyno"]) == 7)
            {
                //DDLCustomerCode.SelectedIndex = 1;
                if (Request.QueryString["orderid"] != null)
                {
                    DDLOrderNo.SelectedValue = Request.QueryString["orderid"];
                    GetOrderDetail();
                    Fill_Grid();
                }
            }
            if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1" && variable.VarGENERATESTOCKNOONTAGGING == "1")
            {
                btninterprodstockno.Visible = true;
            }
            //**************
            //if (variable.Carpetcompany == "1")
            //{
            //    btnproductionsummary.Visible = true;
            //}
        }
    }
    protected void DDLInCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        string Str = "";
        if (DDLInCompanyName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                Str = @"SELECT distinct C.CustomerId,(companyName+'     '+C.CustomerCode) CustomerCode 
                    FROM OrderMaster OM(Nolock) 
                    INNER JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                    INNER JOIN Customerinfo C(Nolock) ON OM.CustomerId=C.CustomerId 
                    Where Om.status=0 and OM.Companyid=" + DDLInCompanyName.SelectedValue + " And C.MasterCompanyId=" + Session["varCompanyId"];
                if (Session["usertype"].ToString() != "1")
                {
                    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Name_id From Process_Name_Master(Nolock) Where process_name_id in(1,16,35) Order By Process_Name_ID");
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            Str = Str + " And OM.OrderId Not in (Select OrderId From PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_ID"] + "(Nolock))";
                        }
                    }
                }

            }
            else
            {

//                if (Convert.ToInt32(Session["varcompanyno"]) == 44)
//                {
//                    Str = @"SELECT distinct C.CustomerId,(companyName+'     '+C.CustomerCode) CustomerCode  
//                    FROM OrderMaster OM(Nolock) 
//                    INNER JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
//                    INNER JOIN Customerinfo C(Nolock) ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varCompanyId"] + @" 
//                    Where  om.status=0 And OM.Companyid=" + DDLInCompanyName.SelectedValue + "";
//                }
//                else
//                {
                    Str = @"SELECT distinct C.CustomerId,(companyName+'     '+C.CustomerCode) CustomerCode  
                    FROM OrderMaster OM(Nolock) 
                    INNER JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                    INNER JOIN Customerinfo C(Nolock) ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varCompanyId"] + @" 
                    Where (OD.TAG_FLAG is null OR OD.TAG_FLAG=0) and om.status=0 And OM.Companyid=" + DDLInCompanyName.SelectedValue + "";
               // }
            }
            Str = Str + "  order by  companyName+'     '+C.CustomerCode ";
        }
        UtilityModule.ConditionalComboFill(ref DDLCustomerCode, Str, true, "Select CustomerCode");
    }
    protected void DDLCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGOrderDetail.DataSource = null;
        DGOrderDetail.DataBind();
        EditCheckedChanged();
    }
    protected void DDLOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOrderDetail();
        Fill_Grid();
    }
    private void GetOrderDetail()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"Select customerorderno,orderid,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,IsNull(replace(convert(varchar(11),prodreqdate,106), ' ','-'),0)  as prodreqdate,replace(convert(varchar(11),duedate,106), ' ','-') as duedate,remarks,replace(convert(varchar(11),DispatchDate,106), ' ','-') as DispatchDate from ordermaster where orderid=" + DDLOrderNo.SelectedValue;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtOrderDate.Text = ds.Tables[0].Rows[0]["orderdate"].ToString();
                TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
                TxtReqDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
                BtnForProductionItem.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmTagging.aspx");
        }
    }
    private void Fill_Grid()
    {
        if (DDLOrderNo.Items.Count > 0)
        {
            DGOrderDetail.DataSource = GetDetail();
            DGOrderDetail.DataBind();
        }
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            fillqty();
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            para[1] = new SqlParameter("@EditFlag", SqlDbType.Int);
            para[2] = new SqlParameter("@withbuyercode", SqlDbType.Int);
            para[3] = new SqlParameter("@VarNewQualitySize", SqlDbType.Int);

            para[0].Value = DDLOrderNo.SelectedValue;
            para[1].Value = ChkForEdit.Checked == true ? 1 : 0;
            para[2].Value = variable.Withbuyercode;
            para[3].Value = variable.VarNewQualitySize;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_Tag_Stock", para);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmTagging.aspx");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
        return ds;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
        #region Old code
        //Saveold();
        #endregion old code
    }
    protected void Savedetail()
    {
        lblErrorMessage.Text = "";
        string msg = "";
        int savecnt = 0;
        Check_Qty();
        if (lblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
                {
                    CheckBox Chkboxitem = ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkboxitem"));
                    if (Chkboxitem.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("Pro_Update_Tag_Stock", con, Tran);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 3000;

                        if (lblErrorMessage.Text == "")
                        {
                            string strOrderid = DGOrderDetail.DataKeys[i].Value.ToString();
                            TextBox txtTagStock1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtTagStock");
                            TextBox txtProd_Qty_Req1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req");
                            TextBox TxtSupplierQty = (TextBox)DGOrderDetail.Rows[i].FindControl("txtSOQTY");
                            Label lblcqid = (Label)DGOrderDetail.Rows[i].FindControl("lblcqid");
                            Label lbldsrno = (Label)DGOrderDetail.Rows[i].FindControl("lbldsrno");
                            Label lblcsrno = (Label)DGOrderDetail.Rows[i].FindControl("lblcsrno");
                            TextBox txtInt_prod_qty_req = (TextBox)DGOrderDetail.Rows[i].FindControl("txtInt_prod_qty_req");
                            int InternalProdqty = Convert.ToInt32(txtInt_prod_qty_req.Text == "" ? "0" : txtInt_prod_qty_req.Text);
                            Label lblpreprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreprodassignedqty");
                            Label lblpreinternalprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreinternalprodassignedqty");
                            Label lbloqty = (Label)DGOrderDetail.Rows[i].FindControl("lbloqty");
                            Label lblextraqty = (Label)DGOrderDetail.Rows[i].FindControl("lblextraqty");
                            Label lblsqty = (Label)DGOrderDetail.Rows[i].FindControl("lblsqty");
                            TextBox txtProd_Weaving_Rate = (TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Weaving_Rate");
                            TextBox txtInt_Weaving_Rate = (TextBox)DGOrderDetail.Rows[i].FindControl("txtInt_Weaving_Rate");

                            decimal Prod_Weaving_Rate = Convert.ToDecimal(txtProd_Weaving_Rate.Text == "" ? "0" : txtProd_Weaving_Rate.Text);
                            decimal Int_Weaving_Rate = Convert.ToDecimal(txtInt_Weaving_Rate.Text == "" ? "0" : txtInt_Weaving_Rate.Text);

                            if (Convert.ToInt32(txtTagStock1.Text) >= 0 && Convert.ToInt32(txtProd_Qty_Req1.Text) >= 0 && Convert.ToInt32(TxtSupplierQty.Text) >= 0 && (Convert.ToInt32(txtTagStock1.Text) >= 0 || Convert.ToInt32(txtProd_Qty_Req1.Text) >= 0 || Convert.ToInt32(TxtSupplierQty.Text) >= 0 && InternalProdqty >= 0))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", Convert.ToInt32(strOrderid.Split('|')[0]));
                                cmd.Parameters.AddWithValue("@ITEM_FINISHED_ID", Convert.ToInt32(strOrderid.Split('|')[1]));
                                cmd.Parameters.AddWithValue("@TagStock", txtTagStock1.Text);
                                cmd.Parameters.AddWithValue("@PreProdAssignedQty", lblpreprodassignedqty.Text); //DGOrderDetail.Rows[i].Cells[4].Text;
                                cmd.Parameters.AddWithValue("@Prod_Qty_Req", txtProd_Qty_Req1.Text);
                                cmd.Parameters.AddWithValue("@SupplierQty", TxtSupplierQty.Text);
                                cmd.Parameters.AddWithValue("@CQID", lblcqid.Text);
                                cmd.Parameters.AddWithValue("@Dsrno", lbldsrno.Text);
                                cmd.Parameters.AddWithValue("@csrno", lblcsrno.Text);
                                cmd.Parameters.AddWithValue("@Withbuyercode", variable.Withbuyercode);
                                cmd.Parameters.AddWithValue("@Internalprodqty", InternalProdqty);
                                cmd.Parameters.AddWithValue("@PREINTERNALPRODASSIGNEDQTY", lblpreinternalprodassignedqty.Text == "" ? "0" : lblpreinternalprodassignedqty.Text);
                                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                                cmd.Parameters.AddWithValue("@MastercompanyId", Session["varcompanyId"]);
                                cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
                                cmd.Parameters.AddWithValue("Prod_Weaving_Rate", Prod_Weaving_Rate);
                                cmd.Parameters.AddWithValue("Int_Weaving_Rate", Int_Weaving_Rate);

                                int qtyReq = Convert.ToInt32(lbloqty.Text);
                                int qtyextra = Convert.ToInt32(string.IsNullOrEmpty(lblextraqty.Text)?"0":lblextraqty.Text);
                                qtyReq = qtyReq + qtyextra;
                                int stock = Convert.ToInt32(lblsqty.Text);
                                int preinternalprodassignedqty = Convert.ToInt32(lblpreinternalprodassignedqty.Text == "" ? "0" : lblpreinternalprodassignedqty.Text);

                                if (stock >= Convert.ToInt32(txtTagStock1.Text) && (Convert.ToInt32(txtTagStock1.Text) + Convert.ToInt32(lblpreprodassignedqty.Text) + Convert.ToInt32(txtProd_Qty_Req1.Text) + Convert.ToInt32(TxtSupplierQty.Text) + InternalProdqty + preinternalprodassignedqty) <= qtyReq)
                                {
                                    int VarConsumptionFlag = ChkForConsumption.Checked == true ? 1 : 0;

                                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update OrderMaster Set ProdReqDate='" + TxtReqDate.Text + "',ConsmpFlag=" + VarConsumptionFlag + " Where Orderid=" + DDLOrderNo.SelectedValue + "");

                                    cmd.ExecuteNonQuery();

                                    //MessageSave();
                                    savecnt = savecnt + 1;
                                    if (cmd.Parameters["@msg"].Value.ToString() != "")
                                    {
                                        msg = msg + "," + cmd.Parameters["@msg"].Value.ToString() + " in Row No. " + (i + 1) + " \\n ";
                                    }
                                }
                                else
                                {
                                    lblErrorMessage.Visible = true;
                                    lblErrorMessage.Text = "Invalid Entry in Row Number " + (i + 1);
                                    txtTagStock1.Focus();
                                    i = DGOrderDetail.Rows.Count;
                                }
                            }
                        }
                    }
                }
                Tran.Commit();
                //********UPDATE STATUS
                UtilityModule.updatestatus(Convert.ToInt16(Session["varcompanyid"]), Convert.ToInt16(Session["varuserid"]), "JOBASSIGNS", Convert.ToInt32(DDLOrderNo.SelectedValue), "TAGGING UPDATED.");
                //******
                if (savecnt > 0)
                {
                    if (msg != "")
                    {
                        msg = msg + " Rest of rows Saved successfully.";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn12", "alert('" + msg + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Record(s) has been saved successfully!');", true);
                    }
                }
                if (ChkForEdit.Checked == true)
                {
                    EditCheckedChanged();
                    DDLOrderNo.SelectedIndex = 0;
                }
                //**************
                #region Send Message
                string Number = "", From = "";
                switch (Session["varcompanyNo"].ToString())
                {
                    case "16":
                        if (ChkForEdit.Checked == false)
                        {
                            if (DDLOrderNo.SelectedIndex > 0)
                            {
                                string str = "select Om.orderid,case When " + Session["varcompanyNo"] + "=6 Then CI.Customercode Else CI.CompanyName End as CustomerName,OM.customerorderno,replace(CONVERT(nvarchar(11),OM.dispatchdate,106),' ','-') as dispatchdate,Sum(QtyRequired) as Qty,Round(Sum(OD.Amount),2) as Amount,dbo.F_GetOrderItem(OM.orderid) as Product,isnull(c.CurrencyName,0) as CurrencyName from ordermaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId inner join Customerinfo ci on OM.customerid=CI.customerid left join currencyinfo c on c.currencyid=od.CurrencyId where OM.orderid=" + DDLOrderNo.SelectedValue + " group by CI.customercode,CI.CompanyName,OM.customerorderno,OM.DispatchDate,OM.orderid,c.CurrencyName";
                                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    switch (Session["varcompanyNo"].ToString())
                                    {
                                        case "16":
                                            //Number = "8447281984,9891009944";
                                            Number = "9839057003,9839380000";
                                            From = "";
                                            break;
                                    }
                                    string message = "Dear sir,\nThe Following Order entered in the Erp\nCustomer:" + ds.Tables[0].Rows[0]["CustomerName"] + "\nProduct:" + ds.Tables[0].Rows[0]["Product"] + "\nQuantity:" + ds.Tables[0].Rows[0]["Qty"] + "\nAmount:" + ds.Tables[0].Rows[0]["Amount"] + " " + ds.Tables[0].Rows[0]["Currencyname"] + "\nDeliveryDate:" + ds.Tables[0].Rows[0]["DispatchDate"] + ".";
                                    UtilityModule.SendMessage(Number, message, Convert.ToInt16(Session["varcompanyNo"]), From: From);
                                }

                            }
                        }
                        break;
                }
                #endregion Send Message
                if (ChkForEdit.Checked == false)
                {
                    Fill_Grid();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmTagging.aspx");
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = ex.Message;
                Logs.WriteErrorLog(ex.Message);
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void MessageSave()
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('Record(s) has been saved successfully!');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    private void Check_Qty()
    {
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            if (lblErrorMessage.Text == "")
            {
                string strOrderid = DGOrderDetail.DataKeys[i].Value.ToString();
                TextBox txtTagStock1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtTagStock");
                TextBox txtProd_Qty_Req1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req");
                TextBox TxtSupplierQty = (TextBox)DGOrderDetail.Rows[i].FindControl("txtSOQTY");
                TextBox txtInt_prod_qty_req = (TextBox)DGOrderDetail.Rows[i].FindControl("txtInt_prod_qty_req");
                Label lblpreprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreprodassignedqty");
                Label lblpreinternalprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreinternalprodassignedqty");
                Label lbloqty = (Label)DGOrderDetail.Rows[i].FindControl("lbloqty");
                Label lblextraqty = (Label)DGOrderDetail.Rows[i].FindControl("lblextraqty");
                Label lblsqty = (Label)DGOrderDetail.Rows[i].FindControl("lblsqty");

                if (Convert.ToInt32(Session["varcompanyno"]) == 16)
                {
                    Label lblProdWeavingRate = (Label)DGOrderDetail.Rows[i].FindControl("lblProdWeavingRate");
                    Label lblIntWeavingRate = (Label)DGOrderDetail.Rows[i].FindControl("lblIntWeavingRate");
                    TextBox txtProd_Weaving_Rate = (TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Weaving_Rate");
                    TextBox txtInt_Weaving_Rate = (TextBox)DGOrderDetail.Rows[i].FindControl("txtInt_Weaving_Rate");

                    decimal Prod_Weaving_Rate = Convert.ToDecimal(txtProd_Weaving_Rate.Text == "" ? "0" : txtProd_Weaving_Rate.Text);
                    decimal Int_Weaving_Rate = Convert.ToDecimal(txtInt_Weaving_Rate.Text == "" ? "0" : txtInt_Weaving_Rate.Text);
                    decimal Max_Prod_Weaving_Rate = Convert.ToDecimal(lblProdWeavingRate.Text == "" ? "0" : lblProdWeavingRate.Text);
                    decimal Max_Int_Weaving_Rate = Convert.ToDecimal(lblIntWeavingRate.Text == "" ? "0" : lblIntWeavingRate.Text);

                    if (Prod_Weaving_Rate > Max_Prod_Weaving_Rate)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Production Weaving rate can not be greater then " + Max_Prod_Weaving_Rate + " in Row number " + (i + 1);
                        txtProd_Weaving_Rate.Focus();
                        i = DGOrderDetail.Rows.Count;
                        return;
                    }

                    if (Int_Weaving_Rate > Max_Int_Weaving_Rate)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Int Weaving rate can not be greater then " + Max_Int_Weaving_Rate + " in Row number " + (i + 1);
                        txtInt_Weaving_Rate.Focus();
                        i = DGOrderDetail.Rows.Count;
                        return;
                    }
                }
                SqlParameter[] arrPara = new SqlParameter[6];
                arrPara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                arrPara[1] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                arrPara[2] = new SqlParameter("@TagStock", SqlDbType.Float);
                arrPara[3] = new SqlParameter("@PreProdAssignedQty", SqlDbType.Float);
                arrPara[4] = new SqlParameter("@Prod_Qty_Req", SqlDbType.Float);
                arrPara[5] = new SqlParameter("@SupplierQty", SqlDbType.Float);

                arrPara[0].Value = Convert.ToInt32(strOrderid.Split('|')[0]);
                arrPara[1].Value = Convert.ToInt32(strOrderid.Split('|')[1]);
                arrPara[2].Value = txtTagStock1.Text;
                arrPara[3].Value = lblpreprodassignedqty.Text; //DGOrderDetail.Rows[i].Cells[4].Text;
                arrPara[4].Value = txtProd_Qty_Req1.Text != "" ? txtProd_Qty_Req1.Text : "0";
                arrPara[5].Value = TxtSupplierQty.Text;

                int qtyReq = Convert.ToInt32(lbloqty.Text);
                int qtyextra = Convert.ToInt32(string.IsNullOrEmpty(lblextraqty.Text) ? "0" : lblextraqty.Text);
                qtyReq = qtyReq + qtyextra;
                int stock = Convert.ToInt32(lblsqty.Text);
                int internalprodqty = Convert.ToInt32(txtInt_prod_qty_req.Text == "" ? "0" : txtInt_prod_qty_req.Text);
                int preinternalprodassignedqty = Convert.ToInt32(lblpreinternalprodassignedqty.Text == "" ? "0" : lblpreinternalprodassignedqty.Text);

                if (Convert.ToInt32(txtTagStock1.Text) > stock || qtyReq < (Convert.ToInt32(arrPara[2].Value) + Convert.ToInt32(arrPara[3].Value) + Convert.ToInt32(arrPara[4].Value) + Convert.ToInt32(arrPara[5].Value) + internalprodqty + preinternalprodassignedqty))
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Invalid Entry in Row number " + (i + 1);
                    txtProd_Qty_Req1.Focus();
                    i = DGOrderDetail.Rows.Count;
                }
            }
        }
    }
    protected void BtnForProductionItem_Click(object sender, EventArgs e)
    {
        if (variable.Carpetcompany == "1")
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@orderid", DDLOrderNo.SelectedValue);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETTAGGINGPRODUCTIONITEM", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["varCompanyNo"].ToString() == "42")
                {
                    Session["rptFileName"] = "~\\Reports\\RptProductionTaggingDirectStockAndOutSide.rpt";
                }
                else if (Session["varCompanyNo"].ToString() == "44")
                {
                    Session["rptFileName"] = "~\\Reports\\rptproductiontagging_agni.rpt";
                }
                else if (Session["varCompanyNo"].ToString() == "43")
                {
                    Session["rptFileName"] = "~\\Reports\\RptProductionTaggingCI.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptproductiontagging.rpt";
                }
                
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptproductiontagging.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altr", "alert('No records found');", true);
            }

        }
        else
        {
            if (DDLOrderNo.SelectedIndex > 0)
            {
                Session["ReportPath"] = "Reports/ProductionReportForTagging.rpt";
                Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + DDLOrderNo.SelectedValue + "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
        EditCheckedChanged();
    }
    private void EditCheckedChanged()
    {
        string Str = "";
        if (DDLInCompanyName.SelectedIndex > 0 && DDLInCompanyName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                Str = @"SELECT Distinct OM.OrderId,case when " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @"=1 THen OM.LocalOrder+ ' | ' + OM.CustomerOrderNo Else OM.customerorderno End as OrderNo 
                    FROM OrderMaster OM(Nolock) 
                    INNER JOIN Customerinfo C(Nolock) ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Where OM.Companyid=" + DDLInCompanyName.SelectedValue + " And om.status=0 and Om.ORDERFROMSAMPLE=0  and OM.Customerid=" + DDLCustomerCode.SelectedValue;

                if (Session["usertype"].ToString() != "1")
                {
                    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Name_ID From Process_Name_Master(Nolock) Where Process_Name_id in(1,16,35) Order By Process_Name_ID");
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            Str = Str + " And OM.OrderId Not in (Select OrderId From PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_ID"] + "(Nolock))";
                        }
                    }
                }
            }
            else
            {
                Str = @"SELECT Distinct OM.OrderId,case when " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @"=1 THen OM.LocalOrder+ ' | ' + OM.CustomerOrderNo Else OM.customerorderno End as OrderNo 
                FROM OrderDetail OD(Nolock) 
                INNER JOIN OrderMaster OM(Nolock) ON OD.OrderId=OM.OrderId 
                INNER JOIN Customerinfo C(Nolock) ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varCompanyId"] + @"
                Where (OD.TAG_FLAG is null OR OD.TAG_FLAG=0) And Om.status=0 and Om.ORDERFROMSAMPLE=0 and OM.Companyid=" + DDLInCompanyName.SelectedValue + " And OM.Customerid=" + DDLCustomerCode.SelectedValue;
            }
            Str = Str + "  Order by  orderno";
            UtilityModule.ConditionalComboFill(ref DDLOrderNo, Str, true, "--Select--");
            Fill_Grid();
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
    private void fillqty()
    {
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            string prodqty = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req")).Text;
            if (((TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req")).Text != "0")
            {
                ((TextBox)DGOrderDetail.Rows[i].FindControl("txtSOQTY")).Text = prodqty;
                ((TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req")).Text = "0";
            }
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
        if (Session["varcompanyno"].ToString() == "7")
        {
            e.Row.Cells[3].Visible = false;
        }
    }
    protected void btnproductionsummary_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@orderid", DDLOrderNo.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetOrderLagat_Weaver_Dyer", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataView dv = new DataView(ds.Tables[2]);
                    dv.RowFilter = "Item_id=" + dr["item_id"] + " ANd QualityId=" + dr["qualityid"] + " and DesignId=" + dr["designid"] + " and colorid=" + dr["colorid"] + "";
                    DataTable dt = dv.ToTable();
                    foreach (DataRow dr2 in dt.Rows)
                    {
                        //FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\") + dr2["OrderDetailId"] + "orderdetail.gif");
                        //if (TheFile.Exists)
                        //{
                        //    string img = "";
                        //    img = Server.MapPath("~\\ImageDraftorder\\") + dr2["OrderDetailId"] + "orderdetail.gif";
                        //    Byte[] img_Byte = File.ReadAllBytes(img);
                        //    dr["Image"] = img_Byte;
                        //    break;
                        //}
                        if (Convert.ToString(dr2["Photo"]) != "")
                        {
                            FileInfo TheFile = new FileInfo(Server.MapPath(dr2["photo"].ToString()));
                            if (TheFile.Exists)
                            {
                                string img = dr2["Photo"].ToString();
                                img = Server.MapPath(img);
                                Byte[] img_Byte = File.ReadAllBytes(img);
                                dr["Image"] = img_Byte;
                                break;
                            }
                        }
                    }
                }

                Session["dsFileName"] = "~\\ReportSchema\\RptorderLagatweaverDyer.xsd";
                Session["rptFileName"] = "Reports/RptorderLagatweaverDyer.rpt";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }
    protected void DGOrderDetail_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
            {
                if (DGOrderDetail.Columns[i].HeaderText.ToUpper() == "INTERNAL PROD QTY REQ." || DGOrderDetail.Columns[i].HeaderText.ToUpper() == "PRE INTERNAL PROD QTY.")
                {
                    if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                    {
                        DGOrderDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }
                }
                if (Convert.ToInt32(Session["varcompanyno"]) != 16)
                {
                    if (DGOrderDetail.Columns[i].HeaderText.ToUpper() == "PROD WEAVING RATE" || DGOrderDetail.Columns[i].HeaderText.ToUpper() == "INT WEAVING RATE")
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void btninterprodstockno_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@orderid", DDLOrderNo.SelectedValue);
            param[1] = new SqlParameter("@mastercompanyid", Session["varcompanyid"].ToString());

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALPRODSTOCKNO", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //************

                sht.Range("A1:J1").Merge();
                sht.Range("A1").SetValue("INTERNAL PROD. STOCK NO. DETAIL");
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("A2:J2").Merge();
                sht.Range("A2").SetValue("CUSTOMER CODE : " + ds.Tables[0].Rows[0]["customercode"].ToString());
                sht.Range("A2:J2").Style.Font.Bold = true;
                sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("A3:J3").Merge();
                sht.Range("A3").SetValue("ORDER NO : " + ds.Tables[0].Rows[0]["customerorderno"].ToString());
                sht.Range("A3:J3").Style.Font.Bold = true;
                sht.Range("A3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Headings
                sht.Range("A4:J4").Style.Font.Bold = true;
                sht.Range("A4").SetValue("Item Name");
                sht.Range("B4").SetValue("Quality");
                sht.Range("C4").SetValue("Desingn");
                sht.Range("D4").SetValue("Color");
                sht.Range("E4").SetValue("Shape Name");
                sht.Range("F4").SetValue("Size");
                sht.Range("G4").SetValue("Stock No.");
                sht.Range("H4").SetValue("OnLoom Status");
                sht.Range("I4").SetValue("Bazar Status");
                sht.Range("J4").SetValue("BUCKET ISSUE");

                int row = 5;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Quality"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Design"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["color"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["shapename"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["size"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ONLoomStatus"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Bazarstatus"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Materialstatus"]);
                    row = row + 1;
                }


                sht.Columns(1, 12).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("InternalTaggingStockno_" + DateTime.Now + "." + Fileextension);
                String Path = Server.MapPath("~/Tempexcel/" + filename);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records found...');", true);
            }


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = true;
        }
    }
    protected void Saveold()
    {
        lblErrorMessage.Text = "";
        string msg = "";
        int savecnt = 0;

        Check_Qty();

        if (lblErrorMessage.Text == "")
        {
            for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                if (lblErrorMessage.Text == "")
                {
                    string strOrderid = DGOrderDetail.DataKeys[i].Value.ToString();
                    TextBox txtTagStock1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtTagStock");
                    TextBox txtProd_Qty_Req1 = (TextBox)DGOrderDetail.Rows[i].FindControl("txtProd_Qty_Req");
                    TextBox TxtSupplierQty = (TextBox)DGOrderDetail.Rows[i].FindControl("txtSOQTY");
                    Label lblcqid = (Label)DGOrderDetail.Rows[i].FindControl("lblcqid");
                    Label lbldsrno = (Label)DGOrderDetail.Rows[i].FindControl("lbldsrno");
                    Label lblcsrno = (Label)DGOrderDetail.Rows[i].FindControl("lblcsrno");
                    TextBox txtInt_prod_qty_req = (TextBox)DGOrderDetail.Rows[i].FindControl("txtInt_prod_qty_req");
                    int InternalProdqty = Convert.ToInt32(txtInt_prod_qty_req.Text == "" ? "0" : txtInt_prod_qty_req.Text);
                    Label lblpreprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreprodassignedqty");
                    Label lblpreinternalprodassignedqty = (Label)DGOrderDetail.Rows[i].FindControl("lblpreinternalprodassignedqty");
                    Label lbloqty = (Label)DGOrderDetail.Rows[i].FindControl("lbloqty");
                    Label lblsqty = (Label)DGOrderDetail.Rows[i].FindControl("lblsqty");

                    if (Convert.ToInt32(txtTagStock1.Text) >= 0 && Convert.ToInt32(txtProd_Qty_Req1.Text) >= 0 && Convert.ToInt32(TxtSupplierQty.Text) >= 0 && (Convert.ToInt32(txtTagStock1.Text) >= 0 || Convert.ToInt32(txtProd_Qty_Req1.Text) >= 0 || Convert.ToInt32(TxtSupplierQty.Text) >= 0 && InternalProdqty >= 0))
                    {


                        SqlParameter[] arrPara = new SqlParameter[15];
                        arrPara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                        arrPara[1] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                        arrPara[2] = new SqlParameter("@TagStock", SqlDbType.Int);
                        arrPara[3] = new SqlParameter("@PreProdAssignedQty", SqlDbType.Int);
                        arrPara[4] = new SqlParameter("@Prod_Qty_Req", SqlDbType.Int);
                        arrPara[5] = new SqlParameter("@SupplierQty", SqlDbType.Int);
                        arrPara[6] = new SqlParameter("@CQID", SqlDbType.Int);
                        arrPara[7] = new SqlParameter("@Dsrno", SqlDbType.Int);
                        arrPara[8] = new SqlParameter("@csrno", SqlDbType.Int);
                        arrPara[9] = new SqlParameter("@Withbuyercode", SqlDbType.Int);
                        arrPara[10] = new SqlParameter("@Internalprodqty", SqlDbType.Int);
                        arrPara[11] = new SqlParameter("@PREINTERNALPRODASSIGNEDQTY", SqlDbType.Int);
                        arrPara[12] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                        arrPara[13] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
                        arrPara[14] = new SqlParameter("@userid", Session["varuserid"]);

                        arrPara[0].Value = Convert.ToInt32(strOrderid.Split('|')[0]);
                        arrPara[1].Value = Convert.ToInt32(strOrderid.Split('|')[1]);
                        arrPara[2].Value = txtTagStock1.Text;
                        arrPara[3].Value = lblpreprodassignedqty.Text; //DGOrderDetail.Rows[i].Cells[4].Text;
                        arrPara[4].Value = txtProd_Qty_Req1.Text;
                        arrPara[5].Value = TxtSupplierQty.Text;
                        arrPara[6].Value = lblcqid.Text;
                        arrPara[7].Value = lbldsrno.Text;
                        arrPara[8].Value = lblcsrno.Text;
                        arrPara[9].Value = variable.Withbuyercode;
                        arrPara[10].Value = InternalProdqty;
                        arrPara[11].Value = lblpreinternalprodassignedqty.Text == "" ? "0" : lblpreinternalprodassignedqty.Text;
                        arrPara[12].Direction = ParameterDirection.Output;

                        int qtyReq = Convert.ToInt32(lbloqty.Text);
                        int stock = Convert.ToInt32(lblsqty.Text);
                        int preinternalprodassignedqty = Convert.ToInt32(lblpreinternalprodassignedqty.Text == "" ? "0" : lblpreinternalprodassignedqty.Text);

                        if (stock >= Convert.ToInt32(txtTagStock1.Text) && (Convert.ToInt32(arrPara[2].Value) + Convert.ToInt32(arrPara[3].Value) + Convert.ToInt32(arrPara[4].Value) + Convert.ToInt32(arrPara[5].Value) + InternalProdqty + preinternalprodassignedqty) <= qtyReq)
                        {
                            try
                            {
                                int VarConsumptionFlag = ChkForConsumption.Checked == true ? 1 : 0;
                                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update OrderMaster Set ProdReqDate='" + TxtReqDate.Text + "',ConsmpFlag=" + VarConsumptionFlag + " Where Orderid=" + DDLOrderNo.SelectedValue + "");
                                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Update_Tag_Stock", arrPara);
                                //MessageSave();
                                savecnt = savecnt + 1;
                                if (arrPara[12].Value.ToString() != "")
                                {
                                    msg = msg + "," + arrPara[12].Value.ToString() + " in Row No. " + (i + 1) + " \\n ";

                                }

                            }
                            catch (Exception ex)
                            {
                                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmTagging.aspx");
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = ex.Message;
                                Logs.WriteErrorLog(ex.Message);
                            }

                        }
                        else
                        {
                            lblErrorMessage.Visible = true;
                            lblErrorMessage.Text = "Invalid Entry in Row Number " + (i + 1);
                            txtTagStock1.Focus();
                            i = DGOrderDetail.Rows.Count;
                        }
                    }
                }
            }
            //********UPDATE STATUS
            UtilityModule.updatestatus(Convert.ToInt16(Session["varcompanyid"]), Convert.ToInt16(Session["varuserid"]), "JOBASSIGNS", Convert.ToInt32(DDLOrderNo.SelectedValue), "TAGGING UPDATED.");
            //******
            if (savecnt > 0)
            {
                if (msg != "")
                {
                    msg = msg + " Rest of rows Saved successfully.";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn12", "alert('" + msg + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Record(s) has been saved successfully!');", true);
                }
            }
            if (ChkForEdit.Checked == true)
            {
                EditCheckedChanged();
                DDLOrderNo.SelectedIndex = 0;
            }
            //**************
            #region Send Message
            string Number = "", From = "";
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    if (ChkForEdit.Checked == false)
                    {
                        if (DDLOrderNo.SelectedIndex > 0)
                        {
                            string str = "select Om.orderid,case When " + Session["varcompanyNo"] + "=6 Then CI.Customercode Else CI.CompanyName End as CustomerName,OM.customerorderno,replace(CONVERT(nvarchar(11),OM.dispatchdate,106),' ','-') as dispatchdate,Sum(QtyRequired) as Qty,Round(Sum(OD.Amount),2) as Amount,dbo.F_GetOrderItem(OM.orderid) as Product,isnull(c.CurrencyName,0) as CurrencyName from ordermaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId inner join Customerinfo ci on OM.customerid=CI.customerid left join currencyinfo c on c.currencyid=od.CurrencyId where OM.orderid=" + DDLOrderNo.SelectedValue + " group by CI.customercode,CI.CompanyName,OM.customerorderno,OM.DispatchDate,OM.orderid,c.CurrencyName";
                            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                switch (Session["varcompanyNo"].ToString())
                                {
                                    case "16":
                                        //Number = "8447281984,9891009944";
                                        Number = "9839057003,9839380000";
                                        From = "";
                                        break;
                                }
                                string message = "Dear sir,\nThe Following Order entered in the Erp\nCustomer:" + ds.Tables[0].Rows[0]["CustomerName"] + "\nProduct:" + ds.Tables[0].Rows[0]["Product"] + "\nQuantity:" + ds.Tables[0].Rows[0]["Qty"] + "\nAmount:" + ds.Tables[0].Rows[0]["Amount"] + " " + ds.Tables[0].Rows[0]["Currencyname"] + "\nDeliveryDate:" + ds.Tables[0].Rows[0]["DispatchDate"] + ".";
                                UtilityModule.SendMessage(Number, message, Convert.ToInt16(Session["varcompanyNo"]), From: From);
                            }

                        }
                    }
                    break;
            }
            #endregion Send Message
            Fill_Grid();
        }
    }
    protected void BtnDelStockNo_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("Pro_DeleteInternalStockNo", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@OrderId", DDLOrderNo.SelectedValue);
            cmd.Parameters.AddWithValue("@TStockNo", TxtStockNo.Text);
            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MastercompanyId", Session["varcompanyId"]);

            cmd.ExecuteNonQuery();
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn12", "alert('" + cmd.Parameters["@Msg"].Value + "');", true);
            if (cmd.Parameters["@Msg"].Value.ToString() == "Data successfully deleted")
            {
                TxtStockNo.Text = "";
                Fill_Grid();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmTagging.aspx/BtnDelStockNo_Click");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}