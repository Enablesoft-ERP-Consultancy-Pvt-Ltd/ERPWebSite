using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;


public partial class Masters_ReportForms_frmorderwisecapacitydistrpt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDcategory, "select Category_id, Category_Name from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varCompanyId"] + "", true, "---select---");
            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from session order by Year desc", true, "----select----");
            UtilityModule.ConditonalChkBoxListFill(ref checkmonth, "select month_id,Month_Name from Monthtable");
         
            DDyear.SelectedValue = DateTime.Now.Year.ToString();
        }
    }


    protected void cmdorders_Click(object sender, EventArgs e)
    {
         string str = null;
        for (int i = 0; i < checkmonth.Items.Count; i++)
        {
            if (checkmonth.Items[i].Selected)
            {
                str = str == null ? checkmonth.Items[i].Value : str + "," + checkmonth.Items[i].Value;
            }
        }
        string str1 = "";
        str1 = @"select distinct Om.orderid,Om.customerorderno from ordermaster OM inner join vendorallocation va 
               on om.orderid=va.orderid inner join v_finisheditemdetail vf on va.item_finished_id=vf.item_finished_id where 1=1";
        if (DDcategory.SelectedIndex > 0)
        {
            str1 = str1 + "  and vf.category_id=" + DDcategory.SelectedValue;
        }
        if (str!=null)
        {
            str1 = str1 + "  and month(va.allocationdate) in(" + str + ") and year(allocationdate)='" +DDyear.SelectedValue + "'";
        }
        UtilityModule.ConditonalChkBoxListFill(ref Chkorderno, str1);
    
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        string monthid = null;
        string Monthname = null;
        for (int i = 0; i < checkmonth.Items.Count; i++)
        {
            if (checkmonth.Items[i].Selected)
            {
                monthid = monthid == null ? checkmonth.Items[i].Value : monthid + "," + checkmonth.Items[i].Value;
                Monthname = Monthname == null ? checkmonth.Items[i].Text : Monthname + "," + checkmonth.Items[i].Text;
            }
        }
        string Orderid = null;
        for (int i = 0; i < Chkorderno.Items.Count; i++)
        {
            if (checkmonth.Items[i].Selected)
            {
                Orderid = Orderid == null ? Chkorderno.Items[i].Value : Orderid + "," + Chkorderno.Items[i].Value;
            }
        }

        string str1 = "select *,'" + (Monthname==null?"ALL":Monthname) + "' as Months from  v_OrderwiseAllocation where 1=1";
        if (DDcategory.SelectedIndex > 0)
        {
            str1 = str1 + " and category_id=" + DDcategory.SelectedValue;
        }
        if (monthid != null)
        {
            str1 = str1 + " and month_id in(" +monthid + ")";
        }
        if (Orderid != null)
        {
            str1 = str1 + " and orderid in(" + Orderid + ")";
        }
        str1 = str1 + " and year='" + DDyear.SelectedValue + "'";
        DataSet ds=SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING,CommandType.Text,str1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptOrderwiseallocation.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderwiseallocation.rpt.xsd";
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