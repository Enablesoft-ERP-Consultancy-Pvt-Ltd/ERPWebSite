using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_DepartmentJobAssign : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDLInCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDLInCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
            if (DDLInCompanyName.Items.Count > 0)
            {
                DDLInCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDLInCompanyName.Enabled = false;
                CompanyNameSelectedIndexChanged();
            }
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            DDLCustomerCode.Focus();
            if (DDLCustomerCode.Items.Count > 0)
            {
                DDLCustomerCode.SelectedIndex = 1;
                ddlcustomercode_seletedchage();
            }
        }
    }
    protected void DDLInCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        string str = @"SELECT distinct C.CustomerId,(companyName +'     '+C.CustomerCode)CustomerCode  
        FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId INNER JOIN Customerinfo C ON 
        OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Where (OD.TAG_FLAG IS Null OR OD.TAG_FLAG=0) And OM.Companyid=" + DDLInCompanyName.SelectedValue + "";
        UtilityModule.ConditionalComboFill(ref DDLCustomerCode, str, true, "Select CustomerCode");
    }
    protected void DDLCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcustomercode_seletedchage();
    }
    private void ddlcustomercode_seletedchage()
    {
        UtilityModule.ConditionalComboFill(ref DDLOrderNo, @"SELECT Distinct OM.OrderId,OM.LocalOrder+ ' / ' +OM.CustomerOrderNo FROM OrderDetail OD INNER JOIN
        OrderMaster OM ON OD.OrderId=OM.OrderId INNER JOIN Customerinfo C ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
        Where (OD.TAG_FLAG IS Null OR OD.TAG_FLAG=0) And OM.Companyid=" + DDLInCompanyName.SelectedValue + " And OM.Customerid=" + DDLCustomerCode.SelectedValue, true, "--Select--");
    }
    protected void DDLOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOrderDetail();
        Fill_Grid();
        Session["ReportPath"] = "Reports/ProductionReportForTagging.rpt";
        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + DDLOrderNo.SelectedValue + "";
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
    }
    private void GetOrderDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"Select customerorderno,orderid,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,IsNull(replace(convert(varchar(11),prodreqdate,106), ' ','-'),0)  as prodreqdate,replace(convert(varchar(11),duedate,106), ' ','-') as duedate,remarks,replace(convert(varchar(11),DispatchDate,106), ' ','-') as DispatchDate from ordermaster where orderid=" + DDLOrderNo.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtOrderDate.Text = ds.Tables[0].Rows[0]["orderdate"].ToString();
                TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DispatchDate"].ToString();
                TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BtnForProductionItem.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/DepartmentJobAssign.aspx");
            //Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            para[1] = new SqlParameter("@EditFlag", SqlDbType.Int);
            para[0].Value = DDLOrderNo.SelectedValue;
            para[1].Value = 0;
            //SqlParameter para = new SqlParameter("@OrderId", SqlDbType.Int);
            //para.Value = DDLOrderNo.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_Get_Tag_Stock", para);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/DepartmentJobAssign.aspx");
            // Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;
    }
    protected void DGOrderDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string strOrderid = DGOrderDetail.DataKeys[e.RowIndex].Value.ToString();
        TextBox txtTagStock1 = (TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtTagStock");
        TextBox txtPreProdAssignedQty1 = (TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtPreProdAssignedQty");
        TextBox txtProd_Qty_Req1 = (TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtProd_Qty_Req");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlParameter[] arrPara = new SqlParameter[5];
        arrPara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        arrPara[1] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
        arrPara[2] = new SqlParameter("@TagStock", SqlDbType.Float);
        arrPara[3] = new SqlParameter("@PreProdAssignedQty", SqlDbType.Float);
        arrPara[4] = new SqlParameter("@Prod_Qty_Req", SqlDbType.Float);
        arrPara[0].Value = Convert.ToInt32(strOrderid.Split('|')[0]);
        arrPara[1].Value = Convert.ToInt32(strOrderid.Split('|')[1]);
        arrPara[2].Value = txtTagStock1.Text;
        arrPara[3].Value = txtPreProdAssignedQty1.Text;
        arrPara[4].Value = txtProd_Qty_Req1.Text;
        int qtyReq = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select Sum(QtyRequired) from OrderDetail where OrderId=" + arrPara[0].Value + " and ITEM_FINISHED_ID=" + arrPara[1].Value));
        int stock = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, @"select Sum(IsNull(S.QtyInHand,0)-isnull(S.QtyAssigned,0)) Avialable_stock
                    FROM  OrderDetail OD Left outer JOIN Stock S ON OD.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID
                    Where OD.OrderId=" + arrPara[0].Value + " And OD.ITEM_FINISHED_ID=" + arrPara[1].Value));
        if (stock >= Convert.ToInt32(txtTagStock1.Text) && Convert.ToInt32(txtTagStock1.Text) + Convert.ToInt32(txtPreProdAssignedQty1.Text) + Convert.ToInt32(txtProd_Qty_Req1.Text) <= qtyReq)
        {
            try
            {
                con.Open();//Select ProdReqDate from OrderMaster
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Update OrderMaster Set ProdReqDate='" + TxtReqDate.Text + "' Where Orderid=" + DDLOrderNo.SelectedValue + "");
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Update_Tag_Stock", arrPara);
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/DepartmentJobAssign.aspx");
                Logs.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            DGOrderDetail.EditIndex = -1;
            Fill_Grid();
            ErrorMessage.Visible = false;
        }
        else
        {
            ErrorMessage.Visible = true;
            lblErrorMessage.Text = "Invalid Entry";
            txtTagStock1.Focus();
        }
    }
    protected void DGOrderDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderDetail.EditIndex = -1;
        Fill_Grid();
    }
    protected void DGOrderDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderDetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
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
}