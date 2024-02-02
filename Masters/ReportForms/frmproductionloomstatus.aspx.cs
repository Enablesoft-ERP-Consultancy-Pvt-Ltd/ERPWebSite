using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_ReportForms_frmproductionloomstatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                           
                           select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
    protected void FillFolioNo()
    {
        string str = @"select PIM.IssueOrderId,PIM.IssueOrderId from Process_issue_master_1 PIM
                            Where  PIM.CompanyId=" + DDcompany.SelectedValue + @" and PIM.status<>'Canceled'";
        if (DDProdunit.SelectedIndex > 0)
        {
            str = str + " and  PIM.Units=" + DDProdunit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and PIM.LoomId=" + DDLoomNo.SelectedValue;
        }
        if (DDstatus.SelectedIndex > 0)
        {
            str = str + " and PIM.Status='" + DDstatus.SelectedValue + "'";
        }
        str = str + " order by PIM.IssueOrderId";

        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string where = "";
        int Dateflag = 0;
        if (chkdate.Checked == true)
        {
            Dateflag = 1;
        }
        where = " Where PM.companyid=" + DDcompany.SelectedValue;
        if (DDProdunit.SelectedIndex > 0)
        {
            where = where + " and PM.Units=" + DDProdunit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            where = where + " and PM.Loomid=" + DDLoomNo.SelectedValue;
        }
        if (chkdate.Checked == true)
        {
            where = where + " and PM.assigndate>='" + txtfromdate.Text + "' and PM.assigndate<='" + txttodate.Text + "'";
        }
        if (DDFolioNo.SelectedIndex > 0)
        {
            where = where + " and PM.issueorderid=" + DDFolioNo.SelectedValue;
        }
        if (DDstatus.SelectedIndex>0)
        {
            where = where + " and PM.status='" + DDstatus.SelectedValue + "'";
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@where", where);
        param[1] = new SqlParameter("@dateflag", Dateflag);
        param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@ToDate", txttodate.Text);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_rtpgetproductionLoomstatus", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rptproductionloomstatus.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptproductionloomstatus.xsd";
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

    protected void DDstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
}