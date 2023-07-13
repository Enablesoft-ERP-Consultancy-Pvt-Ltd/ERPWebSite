using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Order_FRMVENDORMANAGEMENT : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtallocatedate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref ddcompany, @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName", true, "-----SELECT------");

            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref ddcustomer, "select CustomerId, CustomerName from customerinfo  where MasterCompanyId=" + Session["varCompanyId"] + " order by CustomerName", true, "-----SELECT------");
            UtilityModule.ConditionalComboFill(ref ddorderno, "select OrderId, CustomerOrderNo from ordermaster order by CustomerOrderNo", true, "-----SELECT------");
            UtilityModule.ConditionalComboFill(ref ddvendor, "select EmpId,EmpName from EmpInfo where MasterCompanyId=" + Session["varCompanyId"] + " order by EmpName", true, "-----SELECT------");
        }
    }

    protected void FillGrid()
    {
        string str = @"Select QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)
                        +SizeMtr+ Space(2)+ShadeColorName as ItemDescription,
                        OD.QtyRequired as OrderQty,(select isnull(sum(Quantity),0) from vendorallocation VA where VA.orderid=OD.orderid and 
                        VA.Item_finished_id=OD.Item_finished_id) as AlreadyAllocation, VF.item_finished_id 
                        From  V_FinishedItemDetail VF,
                        orderdetail OD Where VF.item_finished_id = od.item_finished_id AND OD.ORDERID=" + ddorderno.SelectedValue;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        DataSet ds = SqlHelper.ExecuteDataset(str);
        GDview.DataSource = ds;
        GDview.DataBind();
    }
    protected void GDview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int row = gvr.RowIndex;
                Label lblorderQty = ((Label)GDview.Rows[row].FindControl("lblorderQty"));
                Label lblitem_finished_id = ((Label)GDview.Rows[row].FindControl("lblitem_finished_id"));

                TextBox txtallocationqty = ((TextBox)GDview.Rows[row].FindControl("txtallocationqty"));
                TextBox txtcmt = ((TextBox)GDview.Rows[row].FindControl("txtcmt"));
                if (txtallocationqty.Text == "" || txtallocationqty.Text == "0")
                {
                    lblmsg.Text = "Plz Enter Allocation Qty......";
                    Tran.Commit();
                    return;
                }

                if (txtcmt.Text == "" || txtcmt.Text == "0")
                {
                    lblmsg.Text = "Plz Enter CMT......";
                    Tran.Commit();
                    return;
                }

                param[0] = new SqlParameter("@varuserid ", SqlDbType.Int);
                param[1] = new SqlParameter("@OrderId", SqlDbType.Int);
                param[2] = new SqlParameter("@VendorId ", SqlDbType.Int);
                param[3] = new SqlParameter("@Companyid ", SqlDbType.Int);
                param[4] = new SqlParameter("@Customerid ", SqlDbType.Int);
                param[5] = new SqlParameter("@AllocationDate ", SqlDbType.DateTime);
                param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                param[7] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                param[8] = new SqlParameter("@Quantity", SqlDbType.Int);
                param[9] = new SqlParameter("@item_finished_id", SqlDbType.Int);
                param[10] = new SqlParameter("@Ven_capacity", SqlDbType.Int);
                param[11] = new SqlParameter("@CMT", SqlDbType.Float);


                param[0].Value = Session["varuserid"].ToString();
                param[1].Value = ddorderno.SelectedIndex < 0 ? "0" : ddorderno.SelectedValue;
                param[2].Value = ddvendor.SelectedIndex < 0 ? "0" : ddvendor.SelectedValue;
                param[3].Value = ddcompany.SelectedIndex < 0 ? "0" : ddcompany.SelectedValue;
                param[4].Value = ddcustomer.SelectedIndex < 0 ? "0" : ddcustomer.SelectedValue;
                param[5].Value = txtallocatedate.Text;
                param[6].Direction = ParameterDirection.Output;
                param[7].Value = Session["varCompanyId"].ToString();
                param[8].Value = txtallocationqty.Text;
                param[9].Value = lblitem_finished_id.Text;
                param[10].Direction = ParameterDirection.Output;
                param[11].Value = txtcmt.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_VendorAllocation", param);
                lblmsg.Text = param[6].Value.ToString();
                lablcapacity.Text = param[10].Value.ToString();
                Tran.Commit();
                FillGrid();
                ddvendor_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmsg.Text = ex.Message;
                con.Close();
            }

        }
    }

    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void ddvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlParameter[] param = new SqlParameter[4];

        param[0] = new SqlParameter("@empid", SqlDbType.Int);
        param[1] = new SqlParameter("@Quantity", SqlDbType.Int);
        param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 50);
        param[3] = new SqlParameter("@Date", SqlDbType.DateTime);


        param[0].Value = ddvendor.SelectedIndex < 0 ? "0" : ddvendor.SelectedValue;
        param[1].Direction = ParameterDirection.Output;
        param[2].Direction = ParameterDirection.Output;
        param[3].Value = txtallocatedate.Text;
        SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "pro_GetVendorCapacity", param);
        lablcapacity.Text = param[1].Value.ToString();
    }
    protected void ddcompany_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}