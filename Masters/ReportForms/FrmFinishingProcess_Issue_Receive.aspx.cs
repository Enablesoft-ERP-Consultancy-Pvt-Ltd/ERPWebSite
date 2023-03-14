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

public partial class Masters_ReportForms_FrmFinishingProcess_Issue_Receive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where ProcessType=1 and MasterCompanyId=" + Session["varCompanyId"] + @" Order By PROCESS_NAME";

            //if (Session["varcompanyId"].ToString() == "9")
            //{
            //    ChkHindiFormat.Visible = true;
            //    ChkFolioMaterialDetail.Visible = true;
            //    ChkFolioMaterialDetailNew.Visible = true;
            //    str = str + " select Distinct EI.Empid,EI.Empname + case when isnull(Ei.empcode,'')<>'' then ' ['+Ei.empcode+']' else '' ENd as Empname From Empinfo EI inner Join Empprocess EP on EI.Empid=EP.empid inner join V_PRODUCTIONPROCESSIDFORHAFIZIA PNM on EP.Processid=PNM.Process_Name_id  order by empname ";
            //}
            //else
            //{
            //    ChkHindiFormat.Visible = false;
            //    ChkFolioMaterialDetail.Visible = false;
            //    ChkFolioMaterialDetailNew.Visible = false;
            //    str = str + " select EI.Empid,EI.Empname + case when isnull(Ei.empcode,'')<>'' then ' ['+Ei.empcode+']' else '' ENd as Empname From Empinfo EI inner Join Empprocess EP on EI.Empid=EP.empid inner join Process_Name_Master PNM on EP.Processid=PNM.Process_Name_id Where PNM.Process_Name='Weaving' order by Ei.empname ";
            //}
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDEmpName.SelectedIndex > 0)
        {
            DDEmpName.SelectedIndex = 0;
        }
        if (DDIssueChallanNo.SelectedIndex > 0)
        {
            DDIssueChallanNo.SelectedIndex = 0;
        }
        txtIssueChallanNo.Text = "";
        UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
          
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueChallanNo();

    }
    protected void FillIssueChallanNo()
    {
        string str = "";
        str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue+ @" PIM inner join EmpInfo ei on pim.Empid=ei.EmpId
                WHERE PIm.Companyid=" + DDCompanyName.SelectedValue + " and PIM.Empid=" + DDEmpName.SelectedValue + "  and PIM.Status<>'Canceled'";      

        str = str + " UNION ";

        str = str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From Process_issue_Master_" + DDProcessName.SelectedValue + @" pim inner join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=" + DDProcessName.SelectedValue + @"
            and pim.Empid=0 Where PIm.Companyid=" + DDCompanyName.SelectedValue + " and EMP.Empid=" + DDEmpName.SelectedValue + " and PIM.Status<>'Canceled' ";      
        str = str + " order by issueorderid1";

        UtilityModule.ConditionalComboFill(ref DDIssueChallanNo, str, true, "--Plz Select--");
    }
   
   
    protected void btnprint_Click(object sender, EventArgs e)
    {
        FinishingProcessReport();        
    }
    protected void FinishingProcessReport()
    {

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_FINISHINGISSUERECEIVESUMMARY_ISSUECHALLANNO_WISE_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@issueorderid", DDIssueChallanNo.SelectedValue);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        cmd.Parameters.AddWithValue("@UserId", Session["varUserId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        // ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverFolio", param);


        if (ds.Tables[0].Rows.Count > 0)
        {
            ////switch (Session["varcompanyId"].ToString())
            ////{
            ////    case "15":
            ////        Session["rptFileName"] = "Reports/rptweaverfolio_EMHD.rpt";
            ////        break;
            ////    case "9":
            ////        Session["rptFileName"] = "Reports/Rptweaverfolio_Hafizia.rpt";
            ////        break;
            ////    case "27":
            ////        Session["rptFileName"] = "Reports/Rptweaverfolio_Antique.rpt";
            ////        break;
            ////    case "30":
            ////        Session["rptFileName"] = "Reports/Rptweaverfolio_Samara.rpt";
            ////        break;
            ////    default:
            ////        Session["rptFileName"] = "Reports/rptweaverfolio.rpt";
            ////        break;
            ////}

            Session["rptFileName"] = "Reports/RptFinishingProcessIssRecSummaryIssueChallanWise.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessIssRecSummaryIssueChallanWise.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    //protected void WeaverFolio()
    //{
       
    //    DataSet ds = new DataSet();       
        
    //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //        if (con.State == ConnectionState.Closed)
    //        {
    //            con.Open();
    //        }
    //        SqlCommand cmd = new SqlCommand("Pro_WeaverFolio", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandTimeout = 300;

    //        cmd.Parameters.AddWithValue("@issueorderid", DDIssueChallanNo.SelectedValue);
    //        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
    //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //        cmd.ExecuteNonQuery();
    //        ad.Fill(ds);
    //        // ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverFolio", param);
       

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        switch (Session["varcompanyId"].ToString())
    //        {
    //            case "15":
    //                Session["rptFileName"] = "Reports/rptweaverfolio_EMHD.rpt";
    //                break;
    //            case "9":
    //                Session["rptFileName"] = "Reports/Rptweaverfolio_Hafizia.rpt";
    //                break;
    //            case "27":
    //                Session["rptFileName"] = "Reports/Rptweaverfolio_Antique.rpt";
    //                break;
    //            case "30":
    //                Session["rptFileName"] = "Reports/Rptweaverfolio_Samara.rpt";
    //                break;
    //            default:
    //                Session["rptFileName"] = "Reports/rptweaverfolio.rpt";
    //                break;
    //        }
    //        Session["dsFileName"] = "~\\ReportSchema\\rptweaverfolio.xsd";
    //        Session["GetDataset"] = ds;
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
    //    }
    //}
    protected void txtIssueChallanNo_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //if (DDProcessName.SelectedIndex > 0)
        //{            
            string str = "";

            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" Where ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and status<>'canceled' and empid>0";
                str = str + " UNION ";
                str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=" + DDProcessName.SelectedValue + @" and pm.empid=0 and pm.ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and pm.status<>'canceled'";
            }
            else
            {
                str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" Where issueorderid=" + txtIssueChallanNo.Text.Trim() + " and status<>'canceled' and empid>0";
                str = str + " UNION ";
                str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=" + DDProcessName.SelectedValue + @" and pm.empid=0 and pm.issueorderid=" + txtIssueChallanNo.Text.Trim() + " and pm.status<>'canceled'";
            }


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDCompanyName.Items.FindByValue(ds.Tables[0].Rows[0]["Companyid"].ToString()) != null)
                {
                    DDCompanyName.SelectedValue = ds.Tables[0].Rows[0]["Companyid"].ToString();
                }
                if (DDEmpName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
                {
                    DDEmpName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    DDEmpName_SelectedIndexChanged(sender, new EventArgs());
                }
                if (DDIssueChallanNo.Items.FindByValue(ds.Tables[0].Rows[0]["issueorderid"].ToString()) != null)
                {
                    DDIssueChallanNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
                }
            }
            else
            {
                lblmsg.Text = "Please Enter Proper Issue Challan No.";
            }
        //}
        //else
        //{
        //    lblmsg.Text = "Please Select ProcessName Then Enter Issue ChallanNo.";
        //}

        
    }
    
    
}