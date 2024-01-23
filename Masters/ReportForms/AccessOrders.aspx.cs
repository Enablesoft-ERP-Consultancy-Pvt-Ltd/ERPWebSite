using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class AccessOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddlCompanyname, @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName", true, "------SELECT-----");

            UtilityModule.ConditionalComboFill(ref ddlCustCode, "SELECT customerid,Customercode from customerinfo ", true, "------SELECT-----");
            txtfrmdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        if (ddlCompanyname.Items.Count > 0)
        {
            ddlCompanyname.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
            ddlCompanyname.Enabled = false;
        }
    }
    protected void ddlCompanyname_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddlCustCode, "SELECT customerid,Customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerCode", true, "--SELECT--");
    }
    protected void chkdate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdate.Checked == true)
        {
            TRDate.Visible = true;
        }
        else
        {

            TRDate.Visible = false;
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        string str = @"select  CI.CompanyName,CI.CompAddr1,cI.CompAddr2,CI.CompAddr3,CI.CompTel,CC.CustomerCode,DOM.orderno as DraftOrderNo
                    ,DOM.OrderDate as DraftDate,case When OM.DRAFTORDERID IS null then 'Unconfirm' Else 'Confirm' End as Status from DRAFT_ORDER_MASTER DOM Left outer join OrderMaster OM
                    on DOM.OrderId=OM.DRAFTORDERID inner join CompanyInfo CI on DOM.CompanyId=CI.CompanyId
                    inner join customerinfo CC on DOM.CustomerId=CC.CustomerId Where CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddlCompanyname.SelectedIndex > 0)
        {
            str = str + " And CI.CompanyId=" + ddlCompanyname.SelectedValue;
        }
        if (ddlCustCode.SelectedIndex > 0)
        {
            str = str + " And CC.CustomerId=" + ddlCustCode.SelectedValue;
        }
        if (radioconfirm.Checked == true)
        {
            str = str + "  and isnull(OM.DraftOrderId,0)<>0";
        }
        else if (radioUnconfirm.Checked == true)
        {
            str = str + "  and isnull(OM.DraftOrderId,0)=0";
        }
        if (chkdate.Checked == true)
        {
            str = str + " and DOM.OrderDate>='" + txtfrmdate.Text + "' and DOM.orderDate<='" + txttodate.Text + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptConfirm_UnCorfirmDO.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptConfirm_UnCorfirmDO.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found...');", true);
        }

    }
}