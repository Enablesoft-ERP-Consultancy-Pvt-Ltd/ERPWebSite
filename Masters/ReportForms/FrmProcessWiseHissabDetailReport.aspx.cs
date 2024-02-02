using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_ReportForms_FrmProcessWiseHissabDetailReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDcompany, "select CI.CompanyId,ci.CompanyName From Companyinfo CI inner join Company_Authentication CA on CI.CompanyId=CA.CompanyId where CA.UserId=" + Session["varuserid"] + "", false, "");
            //txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            BindProcessName();
        }
    }
    private void BindProcessName()
    {
        string Str = "";
        Str = "Select Process_Name_Id, Process_Name from PROCESS_NAME_MASTER(Nolock) Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and Process_Name<>'Weaving' Order By Process_Name";

        UtilityModule.ConditionalComboFill(ref DDProcessName, Str, true, "--SELECT--");
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string Str = "";
            if (variable.VarFinishingNewModuleWise == "1")
            {
                if (DDProcessName.SelectedValue == "1")
                {
                    Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),EMpInfo EI Where CompanyId=" + DDcompany.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and EI.Blacklist=0 Order By EmpName";
                }
                else
                {
                    Str = @"select Distinct EI.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From Employee_ProcessOrderNo EMP(NoLock) inner join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(NoLock) on EMP.IssueOrderId=PIM.IssueOrderId
                            inner join EmpInfo EI(NoLock) on EMP.Empid=EI.EmpId Where  EMP.ProcessId=" + DDProcessName.SelectedValue + " and PIM.Companyid= " + DDcompany.SelectedValue + " and EI.Blacklist=0 order by Empname";
                }

                //                    switch (Session["varMasterCompanyIDForERP"].ToString())
                //                    {
                //                        case "28":
                //                        case "22":
                //                        case "21":
                //                            Str = @"SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM(NoLock) INNER JOIN EMPINFO EI(NoLock) ON PIM.EMPID=EI.EMPID WHERE PIM.Companyid=" + DDcompany.SelectedValue + @" and  EI.Blacklist=0
                //                                UNION
                //                                SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM EMPLOYEE_PROCESSORDERNO EMP(NoLock) INNER JOIN PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM(NoLock)  ON EMP.ISSUEORDERID=PIM.ISSUEORDERID AND EMP.PROCESSID=" + DDProcessName.SelectedValue + @"
                //                                INNER JOIN EMPINFO EI(NoLock) ON EMP.EMPID=EI.EMPID WHere PIm.companyid=" + DDcompany.SelectedValue + " and EI.Blacklist=0 ORDER BY EMPNAME";

                //                            break;
                //                        default:
                //                            if (DDProcessName.SelectedValue == "1")
                //                            {
                //                                Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),EMpInfo EI Where CompanyId=" + DDcompany.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and EI.Blacklist=0 Order By EmpName";
                //                            }
                //                            else
                //                            {
                //                                Str = @"select Distinct EI.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From Employee_ProcessOrderNo EMP(NoLock) inner join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(NoLock) on EMP.IssueOrderId=PIM.IssueOrderId
                //                            inner join EmpInfo EI(NoLock) on EMP.Empid=EI.EmpId Where  EMP.ProcessId=" + DDProcessName.SelectedValue + " and PIM.Companyid= " + DDcompany.SelectedValue + " and EI.Blacklist=0 order by Empname";
                //                            }
                //                            break;
                //                    }
            }
            else
            {
                Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),EMpInfo EI(NoLock) Where CompanyId=" + DDcompany.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and EI.Blacklist=0 Order By EmpName";
            }

            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");

        }

    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChanged();
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        str = @"select distinct PM.id, PM.HissabNo From PROCESS_HISSAB PM(NoLock) Where PM.CompanyId=" + DDcompany.SelectedValue + " and  PM.ProcessId=" + DDProcessName.SelectedValue + " and PM.Empid=" + DDEmployerName.SelectedValue + "";

        UtilityModule.ConditionalComboFill(ref DDHissabSlipNo, str, true, "--SELECT--");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GetProcessWiseHissabDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyId", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@Empid", DDEmployerName.SelectedValue);
            cmd.Parameters.AddWithValue("@HissabSlipNoId", DDHissabSlipNo.SelectedValue);
            cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            //cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            //cmd.Parameters.AddWithValue("@TODate", TxtToDate.Text);       

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptProcessWiseHissabDetailReportVikramKM.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessWiseHissabDetailReportVikramKM.xsd";

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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert(' " + ex.Message + "')", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }        
    }
}
