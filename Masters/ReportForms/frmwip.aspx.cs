using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmwip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           select Customerid,Companyname+'/'+customercode as Customer from customerinfo where mastercompanyid=" + Session["varcompanyId"] + @" order by Customer
                           Select Process_Name_id,Process_Name from Process_Name_Master where MasterCompanyId=" + Session["varCompanyId"] + " order by Process_Name";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 0, false, "");
            UtilityModule.NewChkBoxListFillWithDs(ref chkcustomer, ds, 1);
            UtilityModule.NewChkBoxListFillWithDs(ref ChLProcess, ds, 2);
            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            DataTable dtcustomerid = new DataTable();
            dtcustomerid.Columns.Add("Customerid", typeof(int));
            //
            for (int i = 0; i < chkcustomer.Items.Count; i++)
            {
                if (chkcustomer.Items[i].Selected)
                {
                    DataRow dr = dtcustomerid.NewRow();
                    dr["customerid"] = chkcustomer.Items[i].Value;
                    dtcustomerid.Rows.Add(dr);
                }
            }
            //*****************
            DataTable dtorderid = new DataTable();
            dtorderid.Columns.Add("Orderid", typeof(int));
            //
            for (int i = 0; i < chkorderno.Items.Count; i++)
            {
                if (chkorderno.Items[i].Selected)
                {
                    DataRow dr = dtorderid.NewRow();
                    dr["orderid"] = chkorderno.Items[i].Value;
                    dtorderid.Rows.Add(dr);
                }
            }
            //Report
            if (RDWIP.Checked == true)
            {
                WIP(dtcustomerid, dtorderid);
            }
            else if (RDBOMDetail.Checked == true)
            {
                BOM_TNA(dtcustomerid, dtorderid);
            }
            else if (RDTNA.Checked == true)
            {
                BOM_TNA(dtcustomerid, dtorderid);
            }
            else if (RDWipsummary.Checked == true)
            {
                WIPSummary(dtcustomerid, dtorderid);
            }
        }
        catch (Exception ex)
        {

            lblmsg.Text = ex.Message;
        }

    }
    protected void WIP(DataTable dtcustomerid, DataTable dtorderid)
    {

        //**************
        string processid = "";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < ChLProcess.Items.Count; i++)
        {
            if (ChLProcess.Items[i].Selected)
            {
                sb.Append(ChLProcess.Items[i].Value + ",");
            }
        }
        processid = sb.ToString().TrimEnd(',');
        //
        if (processid == "")
        {
            for (int i = 0; i < ChLProcess.Items.Count; i++)
            {

                sb.Append(ChLProcess.Items[i].Value + ",");

            }
            processid = sb.ToString().TrimEnd(',');
        }
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@companyId", DDcompanyName.SelectedValue);
        param[1] = new SqlParameter("@dtcustomerid", dtcustomerid);
        param[2] = new SqlParameter("@dtorderid", dtorderid);
        param[3] = new SqlParameter("@mastercompanyId", Session["varcompanyId"]);
        param[4] = new SqlParameter("@processid", processid);
        //
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Proc_GetorderWIP", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptWIP.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptWIP.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
    protected void WIPSummary(DataTable dtcustomerid, DataTable dtorderid)
    {

        //**************
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyId", DDcompanyName.SelectedValue);
        param[1] = new SqlParameter("@dtcustomerid", dtcustomerid);
        param[2] = new SqlParameter("@dtorderid", dtorderid);
        param[3] = new SqlParameter("@mastercompanyId", Session["varcompanyId"]);
        //
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWipSummary", param);
        ds1.Tables[0].DefaultView.RowFilter = "Bomdays<0 or TNAdays<0";
        DataView dv = ds1.Tables[0].DefaultView;
        DataSet ds = new DataSet();
        ds.Tables.Add(dv.ToTable());
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptWIPsummary.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptWIPsummary.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
    protected void BOM_TNA(DataTable dtcustomerid, DataTable dtorderid)
    {

        //**************
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyId", DDcompanyName.SelectedValue);
        param[1] = new SqlParameter("@dtcustomerid", dtcustomerid);
        param[2] = new SqlParameter("@dtorderid", dtorderid);
        param[3] = new SqlParameter("@mastercompanyId", Session["varcompanyId"]);
        //
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetBOM_TNA", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (RDBOMDetail.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\rptBomDefineorderWise.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptBom_TNADefineorderWise.xsd";
            }
            else if (RDTNA.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\RptTnadefinedorderwise.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptBom_TNADefineorderWise.xsd";
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
    protected void btngetorders_Click(object sender, EventArgs e)
    {
        string customerid = null;
        //units authentication
        for (int j = 0; j < chkcustomer.Items.Count; j++)
        {
            if (chkcustomer.Items[j].Selected)
            {
                customerid = customerid == null ? chkcustomer.Items[j].Value : customerid + "," + chkcustomer.Items[j].Value;
            }
        }
        chkorderno.Items.Clear();
        if (customerid != null)
        {
            UtilityModule.ConditonalChkBoxListFill(ref chkorderno, "select OM.orderid,OM.LocalOrder+'#'+OM.customerorderno as customerorderno from ordermaster OM Where OM.customerid in(" + customerid + ") and OM.status=0 order by customerorderno");
        }
    }
}