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

public partial class Masters_ReportForms_FrmFinishingProcess_Issue_Receive_ByFolioNo : System.Web.UI.Page
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
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where ProcessType=1 and MasterCompanyId=" + Session["varCompanyId"] + @" and Process_Name<>'WEAVING' Order By PROCESS_NAME";

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
        UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId inner join Process_Name_Master PNM on EMP.Processid=PNM.Process_Name_id Where PNM.Process_Name='Weaving' order by EmpName", true, "--Select--");

        ////UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
          
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueChallanNo();

    }
    protected void FillIssueChallanNo()
    {
        string str = "";
        str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From PROCESS_ISSUE_MASTER_1 PIM inner join EmpInfo ei on pim.Empid=ei.EmpId
                WHERE PIm.Companyid=" + DDCompanyName.SelectedValue + " and PIM.Empid=" + DDEmpName.SelectedValue + "  and PIM.Status<>'Canceled'";

        str = str + " UNION ";

        str = str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From Process_issue_Master_1 pim inner join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=1
            and pim.Empid=0 Where PIm.Companyid=" + DDCompanyName.SelectedValue + " and EMP.Empid=" + DDEmpName.SelectedValue + " and PIM.Status<>'Canceled' ";
        str = str + " order by issueorderid1";


//        str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue+ @" PIM inner join EmpInfo ei on pim.Empid=ei.EmpId
//                WHERE PIm.Companyid=" + DDCompanyName.SelectedValue + " and PIM.Empid=" + DDEmpName.SelectedValue + "  and PIM.Status<>'Canceled'";      

//        str = str + " UNION ";

//        str = str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From Process_issue_Master_" + DDProcessName.SelectedValue + @" pim inner join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=" + DDProcessName.SelectedValue + @"
//            and pim.Empid=0 Where PIm.Companyid=" + DDCompanyName.SelectedValue + " and EMP.Empid=" + DDEmpName.SelectedValue + " and PIM.Status<>'Canceled' ";      
//        str = str + " order by issueorderid1";

        UtilityModule.ConditionalComboFill(ref DDIssueChallanNo, str, true, "--Plz Select--");
    }
   
   
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            FinishingProcessReport();
        }
        else if(DDProcessName.SelectedIndex==0 && DDIssueChallanNo.SelectedIndex > 0)
        {
            AllFinishingProcessReportByFolioNo();
        }

               
    }
    protected void FinishingProcessReport()
    {

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_FINISHINGISSUERECEIVESUMMARY_ByFolioNo_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@ToProcessId", DDProcessName.SelectedValue);
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

            Session["rptFileName"] = "Reports/RptFinishingProcessIssRecSummaryByFolioNo.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessIssRecSummaryByFolioNo.xsd";
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
    protected void AllFinishingProcessReportByFolioNo()
    {

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_ALLFINISHINGPROCESSRECSUMMARY_BYFOLIONO_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@ToProcessId", "0");
        cmd.Parameters.AddWithValue("@issueorderid", DDIssueChallanNo.SelectedValue);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        cmd.Parameters.AddWithValue("@UserId", Session["varUserId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        // ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverFolio", param);


        if (ds.Tables[0].Rows.Count > 0)
        {

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("AllProcessSummaryByFolioNo_");

            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(83);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


            sht.PageSetup.Margins.Top = 1.21;
            sht.PageSetup.Margins.Left = 0.47;
            sht.PageSetup.Margins.Right = 0.36;
            sht.PageSetup.Margins.Bottom = 0.19;
            sht.PageSetup.Margins.Header = 1.20;
            sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();


            //Export to excel
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;

            string filename = "AllProcessSummaryByFolioNo_";
            filename = filename + (DDIssueChallanNo.SelectedIndex > 0 ? DDIssueChallanNo.SelectedItem.Text : "")+"_"+ DateTime.Now;           
            filename = filename + ".xls";

            Response.AddHeader("content-disposition",
             "attachment;filename=" + filename);
            Response.Charset = "";

            //Response.AddHeader("content-disposition",
            // "attachment;filename=AllProcessSummaryByFolioNo" + DateTime.Now + ".xls");
            //Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);



            int columncount = GridView1.Rows[0].Cells.Count;

            ////Change the Header Row back to white color
            //GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
            ////Applying stlye to gridview header cells          

            if (GridView1.Rows.Count > 0)
            {
                

                //for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
                //{
                //    //GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                //    GridView1.HeaderRow.Cells[i].Style.Add("width", "120px");
                //}       
               

                string StrHeaderText = "";
                for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
                {
                    if (GridView1.HeaderRow.Cells[i].Text == "RowType")
                    {
                        StrHeaderText = GridView1.HeaderRow.Cells[i].Text;

                        GridView1.HeaderRow.Cells[i].Visible = false;
                        //GridView1.Columns[i].Visible = false;
                    }
                }

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    ////GridView1.Rows[i].Cells[1].Style.Add("NumberFormat", "d-mmm-yy");
                    ////GridView1.Rows[i].Cells[1].Style.Add("FormatString", "{0:dd.MM.yyyy}");                      

                    string strTemp = GridView1.Rows[i].Cells[0].Text;
                    if (strTemp == "ORDER")
                    {
                        //GridView1.Rows[i].Attributes.Add("class", "textmode");                       
                        GridView1.Rows[i].Font.Bold=true;
                        GridView1.Rows[i].BackColor =System.Drawing.Color.LightBlue;
                    } 
                   
                }

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridView1.Rows[i].Cells[columncount - 1].Visible = false;                    
                }
            }

            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();      
         
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
                str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_1 Where ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and status<>'canceled' and empid>0";
                str = str + " UNION ";
                str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_1 PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=1 and pm.empid=0 and pm.ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and pm.status<>'canceled'";
            }
            else
            {
                str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_1 Where issueorderid=" + txtIssueChallanNo.Text.Trim() + " and status<>'canceled' and empid>0";
                str = str + " UNION ";
                str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_1 PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=1 and pm.empid=0 and pm.issueorderid=" + txtIssueChallanNo.Text.Trim() + " and pm.status<>'canceled'";
            }

            ////if (variable.VarCompanyWiseChallanNoGenerated == "1")
            ////{
            ////    str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" Where ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and status<>'canceled' and empid>0";
            ////    str = str + " UNION ";
            ////    str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=" + DDProcessName.SelectedValue + @" and pm.empid=0 and pm.ChallanNo='" + txtIssueChallanNo.Text.Trim() + "' and pm.status<>'canceled'";
            ////}
            ////else
            ////{
            ////    str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" Where issueorderid=" + txtIssueChallanNo.Text.Trim() + " and status<>'canceled' and empid>0";
            ////    str = str + " UNION ";
            ////    str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_" + DDProcessName.SelectedValue + @" PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=" + DDProcessName.SelectedValue + @" and pm.empid=0 and pm.issueorderid=" + txtIssueChallanNo.Text.Trim() + " and pm.status<>'canceled'";
            ////}


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId inner join Process_Name_Master PNM on EMP.Processid=PNM.Process_Name_id Where PNM.Process_Name='Weaving' order by EmpName", true, "--Select--");

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
                lblmsg.Text = "Please Enter Proper Folio Challan No.";
            }
        //}
        //else
        //{
        //    lblmsg.Text = "Please Select ProcessName Then Enter Folio ChallanNo.";
        //}

        
    }
    
    
}