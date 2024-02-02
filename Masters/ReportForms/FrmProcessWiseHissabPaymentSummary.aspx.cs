using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Text;

public partial class Masters_ReportForms_FrmProcessWiseHissabPaymentSummary : System.Web.UI.Page
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
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");

            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarFinishingNewModuleWise == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDcompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EmpName", true, "--Select--");
        }

        if (Session["VarCompanyNo"].ToString() == "42")
        {
            if (DDProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                ChkForFinalFolio.Visible = true;
            }
            else
            {
                ChkForFinalFolio.Visible = false;
                ChkForFinalFolio.Checked = false;
            }
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (ChkForFinalFolio.Checked == true)
        {
            WeavingHissabPaymentSummaryWithAdvance();
        }
        else
        {
            ProcessWiseHissabPaymentSummary();
        }        
       
    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    protected void ProcessWiseHissabPaymentSummary()
    {
        try
        {
            string str = "";

            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and PHP.PartyId=" + DDEmpName.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                str = str + " and PHP.DATE>='" + txtFromdate.Text + "' and PHP.DATE<='" + txttodate.Text + "'";
            }

           
            //*****************
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);           
            param[2] = new SqlParameter("@FromDate", txtFromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@UserId", Session["VarUserId"]);
            param[6] = new SqlParameter("@where", str);
            param[7] = new SqlParameter("@DateFlag", ChkForDate.Checked==true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PROCESSWISEHISSABPAYMENTSUMMARY", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptProcessWiseHissabPayment.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessWiseHissabPayment.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void WeavingHissabPaymentSummaryWithAdvance()
    {
        try
        {
            string str = "";

            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and PHP.PartyId=" + DDEmpName.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                str = str + " and PHP.DATE>='" + txtFromdate.Text + "' and PHP.DATE<='" + txttodate.Text + "'";
            }


            //*****************
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@FromDate", txtFromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@UserId", Session["VarUserId"]);
            param[6] = new SqlParameter("@where", str);
            param[7] = new SqlParameter("@DateFlag", ChkForDate.Checked == true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVINGHISSABPAYMENTSUMMARY_WithAdvanceAmtFolioWise", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptWeavingHissabPaymentWithAdvance.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeavingHissabPaymentWithAdvance.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}
