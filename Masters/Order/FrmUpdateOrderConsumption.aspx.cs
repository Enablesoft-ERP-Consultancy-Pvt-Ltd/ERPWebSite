using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Order_FrmUpdateOrderConsumption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string Str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName
                        SELECT Customerid, CompanyName + SPACE(5) + Customercode CompanyName 
                        From customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddcompany, ds, 0, false, "");
            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddcustomercode, ds, 1, true, "Select customer Code");
        }
    }
    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddOrderNo, "Select OrderId, CustomerOrderNo From OrderMaster(Nolock) Where Status = 0 and Customerid = " + ddcustomercode.SelectedValue + " And companyId = " + ddcompany.SelectedValue + " Order By OrderID Desc", true, "--SELECT--");
    }
    protected void btnupdateallconsmp_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select OM.OrderID, OD.OrderDetailID, OD.Item_Finished_ID 
                    From OrderMaster OM(Nolock) 
                    JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                    Where OM.Status = 0 And OM.CompanyId = " + ddcompany.SelectedValue + " And OM.CustomerId = " + ddcustomercode.SelectedValue + @" And
                    OM.OrderID = " + ddOrderNo.SelectedValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(ds.Tables[0].Rows[i]["Item_Finished_ID"]), Convert.ToInt32(ds.Tables[0].Rows[i]["OrderID"]), Convert.ToInt32(ds.Tables[0].Rows[i]["OrderDetailID"]), VARUPDATE_FLAG: 1, UPDATECURRENTCONSUMPTION: 1, effectivedate: DateTime.Now.ToString("dd-MMM-yyyy"));
                }
            }
            //Update status
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Exec Pro_Updatestatus " + Session["varMasterCompanyIDForERP"] + "," + Session["Varuserid"] + ",'Order_Consumption_Detail'," + ddOrderNo.SelectedValue + ",'Consumption Updated'");
            //
            ScriptManager.RegisterStartupScript(Page, GetType(), "a", "alert('Consumption Updated successfully...')", true);
            ddOrderNo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}