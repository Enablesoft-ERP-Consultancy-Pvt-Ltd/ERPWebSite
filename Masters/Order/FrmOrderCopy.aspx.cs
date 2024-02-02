using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
public partial class Masters_Order_FrmOrderCopy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["order_id"] = 0;
            TxtCustOrderNo.Enabled = true;
            TxtLocalOrderNo.Enabled = true;
            UtilityModule.ConditionalComboFill(ref ddordertype, "select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory", true, "Select OrderCategory");
            ddordertype.SelectedValue = "1";
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo where Customercode<>'' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void TxtCustOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtCustOrderNo_Validate();
    }
    protected void TxtCustOrderNo_Validate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string CustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(CustomerOrderNo,0) asd from OrderMaster where CustomerOrderNo='" + TxtCustOrderNo.Text + "'"));
            if (CustOrderNo != "")
            {
                TxtCustOrderNo.Text = "";
                TxtLocalOrderNo.Text = "";
                TxtCustOrderNo.Focus();
                Lblmessage.Visible = true;
                Lblmessage.Text = "Customer Order Number Already Exist......";
            }
            else
            {
                string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'L %'").ToString();
                TxtLocalOrderNo.Text = "L " + Str;
                Lblmessage.Visible = false;
                Lblmessage.Text = "";
            }
        }
        catch (Exception)
        {
            Lblmessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {

            DDCustOrderNo.Visible = true;
            TxtCustOrderNo.Visible = false;
            TxtLocalOrderNo.Text = "";
            TxtOrderDate.Text = "";
            TxtDeliveryDate.Text = "";
            Txtcustorderdt.Text = "";
        }
        else
        {
            DDCustOrderNo.Visible = false;
            TxtCustOrderNo.Visible = true;
            TxtLocalOrderNo.Text = "";
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Txtcustorderdt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["order_id"] = 0;
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDCustOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDFromOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
        }
    }
    protected void DDFromOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"select OD.OrderDetailId,VF.ITEM_NAME AS ITEM_NAME,QualityName+'--' +DesignName+'--'+ColorName+'--'+ShapeName+'--'+
            CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else SizeFt End Description,OD.TotalArea Area,OD.QtyRequired Qty,OD.UnitRate Rate,
            Round(Od.Amount,2) Amount,OD.Remark,'" + TxtOrderDate.Text + @"' As DispatchDate,OrderCaltype From OrderDetail OD,V_FinishedItemDetail VF
            Where OD.Item_Finished_Id=VF.Item_Finished_Id And OrderId=" + DDFromOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"]+@"
            Order by QualityName ";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;
            int TotalQtyRequired = 0;
            double TotalAmount = 0, TotalArea = 0;
            for (int i = 0; i < n; i++)
            {
                TotalQtyRequired = TotalQtyRequired + Convert.ToInt32(ds.Tables[0].Rows[i]["Qty"]);
                TotalAmount = TotalAmount + Convert.ToDouble(ds.Tables[0].Rows[i]["Amount"]);
                TotalArea = TotalArea + (Convert.ToDouble(ds.Tables[0].Rows[i]["Area"]));
            }
            TxtOrderArea.Text = TotalArea.ToString();
            TxtTotalAmount.Text = TotalAmount.ToString();
            TxtTotalQtyRequired.Text = TotalQtyRequired.ToString();
        }
        catch
        {
            Lblmessage.Visible = true;
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
    private DataSet getOrderDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"select OD.OrderDetailId As DetailId,VF.ITEM_NAME AS ITEM_NAME,QualityName+'  '+DesignName+'  '+ColorName+'  '+ShapeName+'  '+
                            CASE WHEN OD.ORDERUnitId=1 Then SizeMtr Else SizeFt End Description,OD.TotalArea*OD.QtyRequired Area,OD.QtyRequired Qty,OD.UnitRate As Rate,
                            Round(Od.Amount,2) Amount From OrderDetail OD,V_FinishedItemDetail VF,ITEM_PARAMETER_MASTER IPM,Unit U,CurrencyInfo Ci
                            where OD.Item_Finished_Id=VF.Item_Finished_Id  And OD.Item_Finished_Id=IPM.Item_Finished_Id
                             And U.UnitId=OD.OrderUnitId And Ci.CurrencyId=Od.CurrencyId And OrderId=" + Session["order_id"] + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        return ds;
    }
    protected void DDCustOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = @"SELECT OM.Orderid,LocalOrder,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,replace(convert(varchar(11),OM.DispatchDate,106), ' ','-') as DispatchDate,OrderCategoryId,replace(convert(varchar(11),Custorderdate,106), ' ','-') as Custorderdate,replace(convert(varchar(11),duedate,106), ' ','-') as duedate From OrderMaster OM Left Outer Join OrderDetail OD ON OM.OrderId=OD.OrderId Where OM.OrderId=" + DDCustOrderNo.SelectedValue;
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["order_id"] = Convert.ToInt32(ds.Tables[0].Rows[0]["orderid"]);
            TxtLocalOrderNo.Text = ds.Tables[0].Rows[0]["LocalOrder"].ToString();
            TxtOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
            Txtcustorderdt.Text = ds.Tables[0].Rows[0]["Custorderdate"].ToString();
            UtilityModule.ConditionalComboFill(ref ddordertype, "select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory", true, "Select OrderCategory");
            ddordertype.SelectedValue = ds.Tables[0].Rows[0]["OrderCategoryId"].ToString();
        }
        UtilityModule.ConditionalComboFill(ref DDFromOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where customerid=" + DDCustomerCode.SelectedValue + " And OrderId<>" + DDCustOrderNo.SelectedValue, true, "--SELECT--");
        Fill_Grid();

    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        }
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
    protected void TextQtyChanged_Event(object sender, EventArgs e)
    {
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        if (((Label)DGOrderDetail.Rows[RowIndex].FindControl("OrderCaltype")).Text == "0") // For Area Wise
        {
            DGOrderDetail.Rows[RowIndex].Cells[6].Text = (Convert.ToDouble(DGOrderDetail.Rows[RowIndex].Cells[4].Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("Qty")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("Rate")).Text)).ToString();

        }
        else
        {
            DGOrderDetail.Rows[RowIndex].Cells[6].Text = (Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("Rate")).Text) * Convert.ToDouble(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("Qty")).Text)).ToString();
        }
    }
    protected void TxtDispatchDate_Changed(object sender, EventArgs e)
    {
        Lblmessage.Visible = false;
        Lblmessage.Text = "";
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        if (Convert.ToDateTime(((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("DispatchDate")).Text) < Convert.ToDateTime(TxtOrderDate.Text))
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Dispatch Date Can not be shorter than OrderDate";
            ((TextBox)DGOrderDetail.Rows[RowIndex].FindControl("DispatchDate")).Text = Txtcustorderdt.Text;
        }
    }
    protected void TxtOrderDate_TextChanged(object sender, EventArgs e)
    {
        Lblmessage.Visible = false;
        Lblmessage.Text = "";
        for (int i = 1; i < DGOrderDetail.Rows.Count; i++)
        {
            if (Convert.ToDateTime(TxtOrderDate.Text) < Convert.ToDateTime(((TextBox)DGOrderDetail.Rows[i].FindControl("DispatchDate")).Text) && ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chk1")).Checked == true)
            {
                Lblmessage.Visible = true;
                Lblmessage.Text = "Dispatch Date Can not be shorter than OrderDate";
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (Lblmessage.Text == "" && lblvalidMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (ChkEditOrder.Checked == true)
                {
                    Session["order_id"] = DDCustOrderNo.SelectedValue;
                }
                SqlParameter[] _arrpara = new SqlParameter[21];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@CustomerOrderNo", SqlDbType.NVarChar);
                _arrpara[4] = new SqlParameter("@LocalOrder", SqlDbType.NVarChar);
                _arrpara[5] = new SqlParameter("@OrderDate", SqlDbType.SmallDateTime);
                _arrpara[6] = new SqlParameter("@DispatchDate", SqlDbType.SmallDateTime);
                _arrpara[7] = new SqlParameter("@DueDate", SqlDbType.SmallDateTime);
                _arrpara[8] = new SqlParameter("@Id", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@ordercategory", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@Custorderdate", SqlDbType.SmallDateTime);

                _arrpara[11] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                _arrpara[12] = new SqlParameter("@Qty", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrpara[14] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
                _arrpara[15] = new SqlParameter("@DeliveryDate", SqlDbType.SmallDateTime);
                _arrpara[16] = new SqlParameter("@VarCurrentFlag", SqlDbType.Int);
                _arrpara[17] = new SqlParameter("@VarFinishedid", SqlDbType.Int);
                _arrpara[18] = new SqlParameter("@VarNewOrderDetailId", SqlDbType.Int);
                _arrpara[19] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrpara[20] = new SqlParameter("@RepeatOrderId", SqlDbType.Int);

                _arrpara[1].Value = DDCustomerCode.SelectedValue;
                _arrpara[2].Value = DDCompanyName.SelectedValue;
                _arrpara[3].Value = TxtCustOrderNo.Text.ToUpper();
                _arrpara[4].Value = TxtLocalOrderNo.Text.ToUpper();
                _arrpara[5].Value = TxtOrderDate.Text;
                _arrpara[6].Value = TxtDeliveryDate.Text;
                _arrpara[7].Value = DateTime.Now;
                _arrpara[8].Direction = ParameterDirection.Output;
                _arrpara[9].Value = ddordertype.SelectedValue;
                _arrpara[10].Value = Txtcustorderdt.Text;
                _arrpara[16].Value = CHKFORCURRENTCONSUMPTION.Checked == true ? 1 : 0;
                _arrpara[17].Value = ParameterDirection.Output;
                _arrpara[18].Value = ParameterDirection.Output;
                _arrpara[19].Value = Session["VarUserId"];
                _arrpara[20].Value = DDFromOrderNo.SelectedValue;

                for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
                {
                    if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chk1")).Checked == true)
                    {
                        _arrpara[0].Value = Session["order_id"];
                        _arrpara[11].Value = DGOrderDetail.DataKeys[i].Value;
                        _arrpara[12].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Qty")).Text; //Qty
                        _arrpara[13].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Rate")).Text;//Rate
                        _arrpara[14].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Remark")).Text;
                        _arrpara[15].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("DispatchDate")).Text;
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_EnterOrder_For_Existing_Order", _arrpara);
                        Session["order_id"] = _arrpara[8].Value;
                        if (CHKFORCURRENTCONSUMPTION.Checked == true)
                        {
                            UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[17].Value), Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[18].Value), 1, CHKFORCURRENTCONSUMPTION.Checked == true ? 1 : 0);
                        }
                    }
                }
                TxtCustOrderNo.Enabled = false;
                TxtLocalOrderNo.Enabled = false;
                Tran.Commit();
                Fill_Grid();
                DGOrderDetail.DataSource = null;
                DGOrderDetail.DataBind();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblvalidMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void refreshform()
    {
        TxtTotalAmount.Text = "";
        TxtTotalQtyRequired.Text = "";
        TxtOrderArea.Text = "";
        Session["OrderDetailId"] = 0;
    }
    private void CHECKVALIDCONTROL()
    {
        lblvalidMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (TxtCustOrderNo.Visible == true)
        {
            if (UtilityModule.VALIDTEXTBOX(TxtCustOrderNo) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLocalOrderNo) == false)
        {
            goto a;
        }

        else
        {
            goto B;
        }
    a:
        lblvalidMessage.Visible = true;
        UtilityModule.SHOWMSG(lblvalidMessage);
    B: ;
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        DDCustomerCode.SelectedIndex = -1;
        TxtCustOrderNo.Enabled = true;
        TxtLocalOrderNo.Enabled = true;
        TxtLocalOrderNo.Text = "";
        TxtCustOrderNo.Text = "";
        ChkEditOrder.Checked = false;
        ChkEditOrder_CheckedChanged(sender, e);
        GDOrderSummary.DataSource = null;
        GDOrderSummary.DataBind();
    }
    protected void GDOrderSummary_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet Ds = null;
        int VarDetailId;
        DataSet Ds1, Ds2, Ds3;
        lblvalidMessage.Text = "";
        VarDetailId = Convert.ToInt32(GDOrderSummary.DataKeys[e.RowIndex].Value);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            string Str = "Select * From OrderDetail Where OrderDetailId=" + VarDetailId + "";
            Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * From OrderDetail Where OrderDetailId=" + VarDetailId + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Ds1 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * From JobAssigns Where OrderId=" + Ds.Tables[0].Rows[0]["OrderId"] + " And Item_Finished_ID=" + Ds.Tables[0].Rows[0]["Item_Finished_ID"] + "");
                if (Ds1.Tables[0].Rows.Count > 0)
                {
                    Ds2 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select Process_Name_Id,Process_Name From Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Process_Name_Id");
                    if (Ds2.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds2.Tables[0].Rows.Count; i++)
                        {
                            Ds3 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * from PROCESS_ISSUE_DETAIL_" + Ds2.Tables[0].Rows[i]["Process_Name_Id"] + " Where OrderId=" + Ds.Tables[0].Rows[0]["OrderId"] + " And Item_Finished_ID=" + Ds.Tables[0].Rows[0]["Item_Finished_ID"] + "");
                            if (Ds3.Tables[0].Rows.Count > 0)
                            {
                                lblvalidMessage.Visible = true;
                                lblvalidMessage.Text = "AlReady Issue To " + Ds2.Tables[0].Rows[i]["Process_Name"] + " Process";
                                i = Ds2.Tables[0].Rows.Count;
                            }
                        }
                    }
                }
                if (lblvalidMessage.Text == "")
                {
                    //SqlParameter[] _arrpara = new SqlParameter[3];
                    //_arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                    //_arrpara[1] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
                    //_arrpara[2] = new SqlParameter("@VarRow", SqlDbType.Int);
                    //_arrpara[0].Value = Ds.Tables[0].Rows[0]["OrderId"];
                    //_arrpara[1].Value = Ds.Tables[0].Rows[0]["OrderDetailId"];
                    //_arrpara[2].Value = 1;
                    //if (GDOrderSummary.Rows.Count == 1)
                    //{
                    //    _arrpara[2].Value = 0;
                    //}
                    //SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteOrder_Row", _arrpara);
                    //tran.Commit();
                    //Fill_Grid();
                    //lblvalidMessage.Visible = true;
                    //lblvalidMessage.Text = "Successfully Deleted..";


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
                    _arrpara[3].Value = Session["varMasterCompanyIDForERP"];
                    _arrpara[4].Value = Session["varuserid"];
                    if (DGOrderDetail.Rows.Count == 1)
                    {
                        _arrpara[1].Value = 0;
                    }
                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteOrder_Row", _arrpara);                   
                    tran.Commit();
                    Fill_Grid();
                    lblvalidMessage.Visible = true;
                    lblvalidMessage.Text = _arrpara[2].Value.ToString();

                }
            }
        }
        catch (Exception ex)
        {
            lblvalidMessage.Text = ex.Message;
            lblvalidMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Fill_Grid()
    {
        GDOrderSummary.DataSource = getOrderDetail();
        GDOrderSummary.DataBind();
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["order_id"] = 0;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
}