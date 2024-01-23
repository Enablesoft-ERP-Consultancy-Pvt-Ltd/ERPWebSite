using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
public partial class Masters_Order_FrmOrderAddProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DataSet ds;
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CI.CompanyId = CA.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                    SELECT Customerid, CustomerCode From CustomerInfo(Nolock) Where Customercode <> '' And MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode";

            ds = SqlHelper.ExecuteDataset(str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select--");
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                DDCustomerCodeSelectedIndex();
            }
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerCodeSelectedIndex();
    }
    protected void DDCustomerCodeSelectedIndex()
    {
        UtilityModule.ConditionalComboFill(ref DDCustOrderNo, @"Select OrderId, CustomerOrderNo From OrderMaster(Nolock) 
                            Where CompanyID = " + DDCompanyName.SelectedValue + " And CustomerID = " + DDCustomerCode.SelectedValue + " Order By CustomerOrderNo", true, "--SELECT--");
    }
    
    protected void DDCustOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //if (Lblmessage.Text == "" && lblvalidMessage.Text == "")
        //{
        //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //    con.Open();
        //    SqlTransaction Tran = con.BeginTransaction();
        //    try
        //    {
        //        if (ChkEditOrder.Checked == true)
        //        {
        //            Session["order_id"] = DDCustOrderNo.SelectedValue;
        //        }
        //        SqlParameter[] _arrpara = new SqlParameter[19];
        //        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        //        _arrpara[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
        //        _arrpara[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
        //        _arrpara[3] = new SqlParameter("@CustomerOrderNo", SqlDbType.NVarChar);
        //        _arrpara[4] = new SqlParameter("@LocalOrder", SqlDbType.NVarChar);
        //        _arrpara[5] = new SqlParameter("@OrderDate", SqlDbType.SmallDateTime);
        //        _arrpara[6] = new SqlParameter("@DispatchDate", SqlDbType.SmallDateTime);
        //        _arrpara[7] = new SqlParameter("@DueDate", SqlDbType.SmallDateTime);
        //        _arrpara[8] = new SqlParameter("@Id", SqlDbType.Int);
        //        _arrpara[9] = new SqlParameter("@ordercategory", SqlDbType.Int);
        //        _arrpara[10] = new SqlParameter("@Custorderdate", SqlDbType.SmallDateTime);

        //        _arrpara[11] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
        //        _arrpara[12] = new SqlParameter("@Qty", SqlDbType.Float);
        //        _arrpara[13] = new SqlParameter("@Rate", SqlDbType.Float);
        //        _arrpara[14] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
        //        _arrpara[15] = new SqlParameter("@DeliveryDate", SqlDbType.SmallDateTime);
        //        _arrpara[16] = new SqlParameter("@VarCurrentFlag", SqlDbType.Int);
        //        _arrpara[17] = new SqlParameter("@VarFinishedid", SqlDbType.Int);
        //        _arrpara[18] = new SqlParameter("@VarNewOrderDetailId", SqlDbType.Int);

        //        _arrpara[1].Value = DDCustomerCode.SelectedValue;
        //        _arrpara[2].Value = DDCompanyName.SelectedValue;
        //        _arrpara[3].Value = TxtCustOrderNo.Text.ToUpper();
        //        _arrpara[4].Value = TxtLocalOrderNo.Text.ToUpper();
        //        _arrpara[5].Value = TxtOrderDate.Text;
        //        _arrpara[6].Value = TxtDeliveryDate.Text;
        //        _arrpara[7].Value = DateTime.Now;
        //        _arrpara[8].Direction = ParameterDirection.Output;
        //        _arrpara[9].Value = ddordertype.SelectedValue;
        //        _arrpara[10].Value = Txtcustorderdt.Text;
        //        _arrpara[16].Value = CHKFORCURRENTCONSUMPTION.Checked == true ? 1 : 0;
        //        _arrpara[17].Value = ParameterDirection.Output;
        //        _arrpara[18].Value = ParameterDirection.Output;

        //        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        //        {
        //            if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chk1")).Checked == true)
        //            {
        //                _arrpara[0].Value = Session["order_id"];
        //                _arrpara[11].Value = DGOrderDetail.DataKeys[i].Value;
        //                _arrpara[12].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Qty")).Text; //Qty
        //                _arrpara[13].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Rate")).Text;//Rate
        //                _arrpara[14].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("Remark")).Text;
        //                _arrpara[15].Value = ((TextBox)DGOrderDetail.Rows[i].FindControl("DispatchDate")).Text;
        //                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_EnterOrder_For_Existing_Order", _arrpara);
        //                Session["order_id"] = _arrpara[8].Value;
        //                if (CHKFORCURRENTCONSUMPTION.Checked == true)
        //                {
        //                    UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[17].Value), Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[18].Value), 1, CHKFORCURRENTCONSUMPTION.Checked == true ? 1 : 0);
        //                }
        //            }
        //        }
        //        TxtCustOrderNo.Enabled = false;
        //        TxtLocalOrderNo.Enabled = false;
        //        Tran.Commit();
        //        Fill_Grid();
        //        DGOrderDetail.DataSource = null;
        //        DGOrderDetail.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Tran.Rollback();
        //        lblvalidMessage.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
    }
    protected void Fill_Grid()
    {
        DGOrderDetail.DataSource = getOrderDetail();
        DGOrderDetail.DataBind();
    }
    private DataSet getOrderDetail()
    {
        DataSet ds = null;
        string strsql = @"Select OM.OrderID, VF.CATEGORY_ID CategoryID, VF.ITEM_ID ItemID, VF.QualityID, VF.DesignID, VF.ColorID, 
                    VF.CATEGORY_NAME CategoryName, VF.ITEM_NAME ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, Sum(OD.QtyRequired) OQty 
                    From OrderMaster OM(Nolock) 
                    JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.Item_Finished_ID = OD.Item_Finished_ID 
                    Where OM.OrderId = " + DDCustOrderNo.SelectedValue + @" 
                    Group By OM.OrderID, VF.CATEGORY_ID, VF.ITEM_ID, VF.QualityID, VF.DesignID, VF.ColorID, VF.CATEGORY_NAME, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName 
                    Order By VF.CATEGORY_NAME, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName ";

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        return ds;
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
}