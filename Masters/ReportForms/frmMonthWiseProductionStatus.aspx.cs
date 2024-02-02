using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmMonthWiseProductionStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                         select shapeid,shapeName from Shape order by ShapeId
                         select year,year as year1 from session order by year desc
                         Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CustomerId";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 1, true, "--Plz Select Shape--");
            UtilityModule.ConditionalComboFillWithDS(ref DDFyear, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDToyear, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 3, true, "--Plz Select Customer--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                DDCompany_SelectedIndexChanged(sender, e);
            }
            ds.Dispose();
            RDProduction.Checked = true;
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select DesignId,DesignName from Design Order by DesignName
                     select ColorId,ColorName from Color Order by ColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Plz Select Design--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "--Plz Select Color--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDShape.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, Sizeft AS SIZENAME FROM SIZE WHERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY SIZEID", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, Sizeft AS SIZENAME FROM SIZE Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY SIZEID", true, "ALL");
        }
    }
    protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkmtr.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, Sizemtr AS SIZENAME FROM SIZE WHERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY SIZEID", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, Sizeft AS SIZENAME FROM SIZE WHERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY SIZEID", true, "ALL");
        }
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {

        lblMessage.Text = "";
        try
        {
            if (RDCustmonthlyStatus.Checked == true)
            {
                CustomerMonthlyOrderStatus();
            }
            else
            {
                MonthlyProduction_orderStatus();
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {

        }


    }

    protected void MonthlyProduction_orderStatus()
    {
        DataSet ds = new DataSet();
        SqlParameter[] array = new SqlParameter[8];
        array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[1] = new SqlParameter("@FMonth", SqlDbType.VarChar, 5);
        array[2] = new SqlParameter("@Fyear", SqlDbType.VarChar, 6);
        array[3] = new SqlParameter("@TMonth", SqlDbType.VarChar, 5);
        array[4] = new SqlParameter("@TYear", SqlDbType.VarChar, 6);
        array[5] = new SqlParameter("@UserId", SqlDbType.Int);
        array[6] = new SqlParameter("@Where", SqlDbType.VarChar, 1000);
        array[7] = new SqlParameter("@ReportType", SqlDbType.Int); // 0 For Production Status,1 For OrderStatus


        array[0].Value = 1;  //fix for weaving
        array[1].Value = DDFMonth.SelectedItem.Text;
        array[2].Value = DDFyear.SelectedItem.Text;
        array[3].Value = DDToMonth.SelectedItem.Text;
        array[4].Value = DDToyear.SelectedItem.Text;
        array[5].Value = Session["varuserid"];
        string str = "";

        str = " CompanyId=" + DDCompany.SelectedValue;

        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And vf.ColorId=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And vf.sizeid=" + DDSize.SelectedValue;
        }
        array[6].Value = str;

        array[7].Value = RDProduction.Checked == true ? 0 : 1;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionStatus", array);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (RDProduction.Checked == true)
            {
                Session["dsFileName"] = "~\\ReportSchema\\RptMonthlyProductionStatus.xsd";
                Session["rptFileName"] = "Reports/RptMonthlyProductionStatus.rpt";
                Session["GetDataset"] = ds;
            }
            else
            {
                //RptMonthWise_OrderDetail
                Session["dsFileName"] = "~\\ReportSchema\\RptMonthWise_OrderDetail.xsd";
                Session["rptFileName"] = "Reports/RptMonthWise_OrderDetail.rpt";
                Session["GetDataset"] = ds;

            }
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
    protected void CustomerMonthlyOrderStatus()
    {
        DataSet ds = new DataSet();
        SqlParameter[] array = new SqlParameter[8];
        array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
        array[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
        array[2] = new SqlParameter("@OrderId", SqlDbType.Int);
        array[3] = new SqlParameter("@FromMonth", SqlDbType.VarChar, 5);
        array[4] = new SqlParameter("@Fromyear", SqlDbType.VarChar, 6);
        array[5] = new SqlParameter("@ToMonth", SqlDbType.VarChar, 5);
        array[6] = new SqlParameter("@Toyear", SqlDbType.VarChar, 6);
        array[7] = new SqlParameter("@UserId", SqlDbType.Int);



        array[0].Value = DDCompany.SelectedValue;  //fix for weaving
        array[1].Value = DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0";
        array[2].Value = DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0";
        array[3].Value = DDFMonth.SelectedItem.Text;
        array[4].Value = DDFyear.SelectedItem.Text;
        array[5].Value = DDToMonth.SelectedItem.Text;
        array[6].Value = DDToyear.SelectedItem.Text;
        array[7].Value = Session["varuserid"];

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_getCustomerMonthlyStatus]", array);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptMonthlyCustomerStatus.xsd";
            Session["rptFileName"] = "Reports/RptMonthlyCustomerStatus.rpt";
            Session["GetDataset"] = ds;

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
    protected void RDCustmonthlyStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (RDCustmonthlyStatus.Checked == true)
        {
            TRDDCustName.Visible = true;
            TRDDOrder.Visible = true;
        }
    }
    protected void RDProduction_CheckedChanged(object sender, EventArgs e)
    {
        if (RDProduction.Checked == true)
        {
            TRDDCustName.Visible = false;
            TRDDOrder.Visible = false;
        }
    }
    protected void RDOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (RDOrder.Checked == true)
        {
            TRDDCustName.Visible = false;
            TRDDOrder.Visible = false;
        }
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str =@"Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM Where  OM.Customerid=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue;
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            Str =@"Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM Where OM.Status = 0 And OM.Customerid=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref DDOrder, Str, true, "--Select--");
    }
}