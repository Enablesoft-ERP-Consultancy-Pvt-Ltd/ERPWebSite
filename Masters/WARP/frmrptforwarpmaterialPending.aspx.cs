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

public partial class Masters_WARP_frmrptforwarpmaterialPending : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                            Select Ei.EmpId,Ei.EmpName+case when Ei.empcode<>'' Then '['+Ei.empcode+']' Else '' End as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId 
                                And D.DepartmentName='WARPING' and Ei.mastercompanyid=" + Session["varcompanyid"] + " order by EmpName";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.NewChkBoxListFillWithDs(ref chkemp, ds, 1);
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string empid = "";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chkemp.Items.Count; i++)
        {
            if (chkemp.Items[i].Selected)
            {
                sb.Append(chkemp.Items[i].Value + ",");
            }
        }
        empid = sb.ToString().TrimEnd(',');
        //
        if (empid == "")
        {
            for (int i = 0; i < chkemp.Items.Count; i++)
            {

                sb.Append(chkemp.Items[i].Value + ",");

            }
            empid = sb.ToString().TrimEnd(',');
        }
        //Date flag
        int Dateflag = 0;
        if (chkdate.Checked == true)
        {
            Dateflag = 1;
        }
        string str = "select *," + Dateflag + " as Dateflag,'" + txtfromdate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate from V_WarptmaterialPending where companyid=" + DDcompany.SelectedValue + " and issueqty>issuedQty";
        if (empid != "")
        {
            str = str + " and empid in(" + empid + ")";
        }
        if (chkdate.Checked == true)
        {
            str = str + " and issuedate>='" + txtfromdate.Text + "'   and issuedate<='" + txttodate.Text + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chksummary.Checked == true)
            {
                Session["rptFileName"] = "~\\Reports\\rptyarnopeningrecDetailSummary.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\rptwarpmaterialpending.rpt";
            }
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptwarpmaterialpending.xsd";
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
}