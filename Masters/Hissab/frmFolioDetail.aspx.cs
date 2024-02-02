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

public partial class Masters_Hissab_frmFolioDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName";

            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                ChkHindiFormat.Visible = true;
                ChkFolioMaterialDetail.Visible = true;
                ChkFolioMaterialDetailNew.Visible = true;
                str = str + @" select Distinct EI.Empid,EI.Empname + case when isnull(Ei.empcode,'')<>'' then ' ['+Ei.empcode+']' else '' ENd as Empname 
                From Empinfo EI 
                inner Join Empprocess EP on EI.Empid=EP.empid 
                inner join V_PRODUCTIONPROCESSIDFORHAFIZIA PNM on EP.Processid=PNM.Process_Name_id  order by empname ";
            }
            else
            {
                ChkHindiFormat.Visible = false;
                ChkFolioMaterialDetail.Visible = false;
                ChkFolioMaterialDetailNew.Visible = false;
                str = str + @" select EI.Empid,EI.Empname + case when isnull(Ei.empcode,'')<>'' then ' ['+Ei.empcode+']' else '' ENd as Empname 
                From Empinfo EI inner Join Empprocess EP on EI.Empid=EP.empid 
                inner join Process_Name_Master PNM on EP.Processid=PNM.Process_Name_id 
                Where PNM.Process_Name='Weaving' order by Ei.empname ";
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDweaver, ds, 1, true, "--Plz Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            if ((Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16) || (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28))
            {
                ChkPoufFolio.Visible = true;
                ChkForDepartmentReport.Visible = true;
            }
        }
    }
    protected void DDweaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (Session["varcompanyNo"].ToString())
        {
            case "9":
                FillFolioNoHafizia();
                break;
            default:
                FillFolioNo();
                break;
        }
    }
    protected void FillFolioNoHafizia()
    {
        string str = "";
        str = @"select Distinct vim.issueorderid,vim.issueorderid as issueorderid1 From VIEW_PROCESS_ISSUE_MASTER VIM inner join EmpInfo ei on vim.Empid=ei.EmpId
                inner join V_PRODUCTIONPROCESSIDFORHAFIZIA VPH on VIm.PROCESSID=vph.PROCESS_NAME_ID
                WHERE vim.Companyid=" + DDCompanyName.SelectedValue + " and vim.Empid=" + DDweaver.SelectedValue + "  and vim.Status<>'Canceled'";
        if (chkfinalfolio.Checked == true)
        {
            str = str + " and IsNull(vim.FOlioStatus, 0)=1";
        }
        else
        {
            str = str + " and IsNull(vim.FOlioStatus, 0)=0";
        }
        str = str + " order by issueorderid1";

        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");

    }
    protected void FillFolioNo()
    {
        string str = "";
        if (ChkPoufFolio.Checked == false)
        {
            str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 
            From PROCESS_ISSUE_MASTER_1 PIM inner join EmpInfo ei on pim.Empid=ei.EmpId
            WHERE PIm.Companyid=" + DDCompanyName.SelectedValue + " and PIM.Empid=" + DDweaver.SelectedValue + "  and PIM.Status<>'Canceled'";
            if (chkfinalfolio.Checked == true)
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0)=1";
            }
            else
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0)=0";
            }

            str = str + " UNION ";

            str = str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 
            From Process_issue_Master_1 pim inner join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=1
            And pim.Empid=0 Where PIm.Companyid=" + DDCompanyName.SelectedValue + " and EMP.Empid=" + DDweaver.SelectedValue + " and PIM.Status<>'Canceled' ";
            if (chkfinalfolio.Checked == true)
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0)=1";
            }
            else
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0)=0";
            }
            str = str + " order by issueorderid1";
        }
        else
        {
            str = str + @" Select Distinct pim.IssueOrderId, isnull(PIM.ChallanNo, Pim.IssueOrderId) Issueorderid1 
            From HomeFurnishingOrderMaster pim(Nolock) 
            inner join Employee_HomeFurnishingOrderMaster emp(Nolock) on pim.issueorderid = emp.issueorderid and emp.ProcessId = pim.ProcessId 
            Where PIm.Companyid = " + DDCompanyName.SelectedValue + " And EMP.Empid = " + DDweaver.SelectedValue + " and PIM.Status <> 'Canceled' ";
            if (chkfinalfolio.Checked == true)
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0) = 1";
            }
            else
            {
                str = str + " and IsNull(PIm.FOlioStatus, 0) = 0";
            }
            str = str + " order by issueorderid1";
        }

        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        WeaverFolio();
    }
    protected void WeaverFolio()
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
        DataSet ds = new DataSet();
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            if (ChkHindiFormat.Checked == true)
            {
                FolioHindiReportHafizia();
                return;
            }
            else if (ChkFolioMaterialDetail.Checked == true)
            {
                FolioMaterialDetailReportHafizia();
                return;
            }
            else if (ChkFolioMaterialDetailNew.Checked == true)
            {
                FolioMaterialDetailReportHafiziaNew();
                return;
            }
            else
            {
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_WEAVERFOLIO_HAFIZIA]", param);
            }
        }
        else
        {
            int FolioType = 0;
            if (ChkPoufFolio.Checked == true)
            {
                FolioType = 1;
            }

            if (ChkForDepartmentReport.Checked == true)
            {
                FolioType = 2;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_WeaverFolio", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;
            if (FolioType == 2)
            {
                cmd.Parameters.AddWithValue("@issueorderid", txtfoliono.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.AddWithValue("@FolioType", FolioType);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            // ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverFolio", param);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "15":
                    Session["rptFileName"] = "Reports/rptweaverfolio_EMHD.rpt";
                    break;
                case "9":
                    Session["rptFileName"] = "Reports/Rptweaverfolio_Hafizia.rpt";
                    break;
                case "27":
                    Session["rptFileName"] = "Reports/Rptweaverfolio_Antique.rpt";
                    break;
                case "30":
                    Session["rptFileName"] = "Reports/Rptweaverfolio_Samara.rpt";
                    break;
                case "38":
                    if (ChkWithoutBarCode.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/RptWeaverFolioDetailWithoutBarCodeVikramKM.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/Rptweaverfolio_Vikram.rpt";
                    }
                    break;
                case "42":
                    if (ChkWithoutBarCode.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/RptWeaverFolioDetailWithoutBarCode.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/Rptweaverfolio_VikramMirzapur.rpt";
                    }
                    break;
                case "43":
                    if (ChkWithoutBarCode.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/RptWeaverFolioDetailWithoutBarCode.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/Rptweaverfolio_CarpetInternational.rpt";
                    }
                    break;
                default:
                    if (ChkForDepartmentReport.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/rptDepartmentIssRecDetail.rpt";
                    }
                    else if (ChkWithoutBarCode.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/RptWeaverFolioDetailWithoutBarCode.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/rptweaverfolio.rpt";
                    }
                    break;
            }
            Session["dsFileName"] = "~\\ReportSchema\\rptweaverfolio.xsd";
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
    protected void txtfoliono_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";        
        string str = "";

        switch (Session["varcompanyNo"].ToString())
        {
            case "27":
            case "42":
                if (Session["UserType"].ToString() == "1")
                {
                    btnfinalfolio.Visible = true;
                }
                else
                {
                    btnfinalfolio.Visible = false;
                }
                break;
            default:
                btnfinalfolio.Visible = true;
                break;
        }

        switch (Session["varcompanyNo"].ToString())
        {
            case "9":
                str = "select vim.empid,vim.issueorderid,vim.companyId,isnull(vim.foliostatus,0) as FolioStatus From VIEW_PROCESS_ISSUE_MASTER VIM inner join V_PRODUCTIONPROCESSIDFORHAFIZIA VPH on VIm.PROCESSID=vph.PROCESS_NAME_ID Where vim.issueorderid=" + txtfoliono.Text.Trim() + " and status<>'canceled'";
                break;
            default:
                if (ChkPoufFolio.Checked == false)
                {
                    if (variable.VarCompanyWiseChallanNoGenerated == "1")
                    {
                        str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_1 Where ChallanNo='" + txtfoliono.Text.Trim() + "' and status<>'canceled' and empid>0";
                        str = str + " UNION ";
                        str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_1 PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=1 and pm.empid=0 and pm.ChallanNo='" + txtfoliono.Text.Trim() + "' and pm.status<>'canceled'";
                    }
                    else
                    {
                        str = @"select Empid,issueorderid,Companyid,isnull(FOLIOSTATUS,0) as FolioStatus From Process_issue_master_1 Where issueorderid=" + txtfoliono.Text.Trim() + " and status<>'canceled' and empid>0";
                        str = str + " UNION ";
                        str = str + "  select EMp.Empid,PM.issueorderid,PM.Companyid,isnull(PM.FOLIOSTATUS,0) as FOlioStatus From Process_issue_master_1 PM inner join Employee_processorderNo emp on pm.issueorderid=Emp.issueorderid and emp.processid=1 and pm.empid=0 and pm.issueorderid=" + txtfoliono.Text.Trim() + " and pm.status<>'canceled'";
                    }
                }
                else
                {
                    str = @"select Distinct b.EmpID, a.ISSUEORDERID, a.Companyid, IsNull(a.FOLIOSTATUS, 0) FolioStatus 
                        From HomeFurnishingOrderMaster a
						JOIN Employee_HomeFurnishingOrderMaster b ON b.IssueOrderID = a.ISSUEORDERID 
                        Where a.CHALLANNO = '" + txtfoliono.Text.Trim() + "' And a.STATUS <> 'canceled'";
                }
                break;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FolioStatus"].ToString() == "1")
            {
                btnfinalfolio.Visible = false;
            }
            chkfinalfolio.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["FolioStatus"]);

            if (DDCompanyName.Items.FindByValue(ds.Tables[0].Rows[0]["Companyid"].ToString()) != null)
            {
                DDCompanyName.SelectedValue = ds.Tables[0].Rows[0]["Companyid"].ToString();
            }
            if (DDweaver.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
            {
                DDweaver.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDweaver_SelectedIndexChanged(sender, new EventArgs());
            }
            if (DDFolioNo.Items.FindByValue(ds.Tables[0].Rows[0]["issueorderid"].ToString()) != null)
            {
                DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
            }
        }
        else
        {
            lblmsg.Text = "Please enter proper Folio No.";
        }
    }
    protected void btnfinalfolio_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        int FolioType = 0;
        if (ChkPoufFolio.Checked == true)
        {
            FolioType = 1;
        }
        
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@FolioType", FolioType);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEFINALFOLIO", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }


    protected void chkfinalfolio_CheckedChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
    protected void ChkHindiFormat_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkHindiFormat.Checked == true)
        {
            ChkFolioMaterialDetail.Checked = false;
            ChkFolioMaterialDetailNew.Checked = false;
        }
    }
    protected void ChkFolioMaterialDetail_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkFolioMaterialDetail.Checked == true)
        {
            ChkHindiFormat.Checked = false;
            ChkFolioMaterialDetailNew.Checked = false;
        }
    }
    protected void ChkFolioMaterialDetailNew_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkFolioMaterialDetailNew.Checked == true)
        {
            ChkHindiFormat.Checked = false;
            ChkFolioMaterialDetail.Checked = false;
        }
    }
    protected void FolioHindiReportHafizia()
    {
        DataSet ds = new DataSet();
        if (DDFolioNo.SelectedIndex > 0)
        {
            lblmsg.Text = "";
            try
            {

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_HAFIZIA_HINDI_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
                //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);


                //SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);


                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_HAFIZIA_HINDI_REPORT", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("FolioReport");
                    //int row = 0;

                    sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    sht.PageSetup.AdjustTo(85);
                    sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

                    //
                    sht.PageSetup.Margins.Top = 0.41;
                    sht.PageSetup.Margins.Left = 0.20;
                    sht.PageSetup.Margins.Right = 0.10;
                    sht.PageSetup.Margins.Bottom = 0.19;
                    sht.PageSetup.Margins.Header = 1.50;
                    sht.PageSetup.Margins.Footer = 0.3;
                    sht.PageSetup.SetScaleHFWithDocument();

                    sht.ColumnWidth = 5.15;

                    sht.Row(1).Height = 30;
                    sht.Row(2).Height = 30;
                    sht.Row(3).Height = 30;
                    sht.Row(4).Height = 30;
                    sht.Row(5).Height = 30;
                    sht.Row(6).Height = 30;
                    sht.Row(7).Height = 30;
                    sht.Row(8).Height = 30;
                    sht.Row(9).Height = 30;
                    sht.Row(10).Height = 3;
                    sht.Row(11).Height = 18;
                    sht.Row(12).Height = 18;
                    sht.Row(13).Height = 18;
                    sht.Row(14).Height = 18;
                    sht.Row(15).Height = 18;
                    sht.Row(16).Height = 18;
                    sht.Row(17).Height = 18;
                    sht.Row(18).Height = 18;
                    sht.Row(19).Height = 20;

                    sht.Range("A1:AB1").Merge();
                    sht.Range("A1").Value = "ORDER - HANDLOOM DURRIE (JOB WORK)";

                    sht.Range("A1:AB1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:AB1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:AB1").Style.Alignment.SetWrapText();
                    sht.Range("A1:AB1").Style.Font.FontName = "Calibri";
                    sht.Range("A1:AB1").Style.Font.FontSize = 18;
                    sht.Range("A1:AB1").Style.Font.Bold = true;
                    //*******Header

                    sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
                    sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                    sht.Range("A2:H2").Style.Font.FontSize = 14;
                    sht.Range("A2:H2").Style.Font.SetBold();
                    sht.Range("A2:H2").Merge();
                    sht.Range("A2:H2").Style.Alignment.SetWrapText();
                    sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I2").Value = "Weaver Name & Address";
                    sht.Range("I2:K2").Style.Font.FontName = "Calibri";
                    sht.Range("I2:K2").Style.Font.FontSize = 11;
                    sht.Range("I2:K2").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("I2:K2").Merge();
                    sht.Range("I2:K2").Style.Alignment.SetWrapText();
                    sht.Range("I2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I2:K2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L2").Value = ds.Tables[0].Rows[0]["EmpName"] + " " + ds.Tables[0].Rows[0]["Address"];
                    sht.Range("L2:R2").Style.Font.FontName = "Calibri";
                    sht.Range("L2:R2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("L2:R2").Merge();
                    sht.Range("L2:R2").Style.Alignment.SetWrapText();
                    sht.Range("L2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S2").Value = "Pan/GST No";
                    sht.Range("S2:T2").Style.Font.FontName = "Calibri";
                    sht.Range("S2:T2").Style.Font.FontSize = 11;
                    sht.Range("S2:T2").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("S2:T2").Merge();
                    sht.Range("S2:T2").Style.Alignment.SetWrapText();
                    sht.Range("S2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S2:T2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S2:T2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
                    sht.Range("U2:X2").Style.Font.FontName = "Calibri";
                    sht.Range("U2:X2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("U2:X2").Merge();
                    sht.Range("U2:X2").Style.Alignment.SetWrapText();
                    sht.Range("U2:X2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U2:X2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y2").Value = "Sr.No";
                    sht.Range("Y2:Z2").Style.Font.FontName = "Calibri";
                    sht.Range("Y2:Z2").Style.Font.FontSize = 11;
                    sht.Range("Y2:Z2").Style.Font.SetBold();
                    sht.Range("Y2:Z2").Merge();
                    sht.Range("Y2:Z2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y2:Z2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y2:Z2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA2").Value = ds.Tables[0].Rows[0]["LocalOrder"];
                    sht.Range("AA2:AB2").Style.Font.FontName = "Calibri";
                    sht.Range("AA2:AB2").Style.Font.FontSize = 11;
                    //sht.Range("Q3:R3").Style.Font.SetBold();
                    sht.Range("AA2:AB2").Merge();
                    sht.Range("AA2:AB2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA2:AB2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"] + " " + ds.Tables[0].Rows[0]["COMPADDR3"];
                    sht.Range("A3:H4").Style.Font.FontName = "Calibri";
                    sht.Range("A3:H4").Style.Font.FontSize = 11;
                    sht.Range("A3:H4").Style.Font.SetBold();
                    sht.Range("A3:H4").Merge();
                    sht.Range("A3:H4").Style.Alignment.SetWrapText();
                    sht.Range("A3:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A3:H4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A5").Value = "GSTIN : " + ds.Tables[0].Rows[0]["CompanyGstNo"] + "                                     " + " PhoneNo : " + ds.Tables[0].Rows[0]["CompanyPhoneNo"];
                    sht.Range("A5:H5").Style.Font.FontName = "Calibri";
                    sht.Range("A5:H5").Style.Font.FontSize = 11;
                    sht.Range("A5:H5").Style.Font.SetBold();
                    sht.Range("A5:H5").Merge();
                    sht.Range("A5:H5").Style.Alignment.SetWrapText();
                    sht.Range("A5:H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A5:H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I3").Value = "Deliver To";
                    sht.Range("I3:K3").Style.Font.FontName = "Calibri";
                    sht.Range("I3:K3").Style.Font.FontSize = 11;
                    sht.Range("I3:K3").Style.Font.SetBold();
                    sht.Range("I3:K3").Merge();
                    sht.Range("I3:K3").Style.Alignment.SetWrapText();
                    sht.Range("I3:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I3:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I3:K3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L3").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
                    sht.Range("L3:R3").Style.Font.FontName = "Calibri";
                    sht.Range("L3:R3").Style.Font.FontSize = 11;
                    sht.Range("L3:R3").Merge();
                    sht.Range("L3:R3").Style.Alignment.SetWrapText();
                    sht.Range("L3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L3:R3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S3").Value = "Pan No";
                    sht.Range("S3:T3").Style.Font.FontName = "Calibri";
                    sht.Range("S3:T3").Style.Font.FontSize = 11;
                    sht.Range("S3:T3").Style.Font.SetBold();
                    sht.Range("S3:T3").Merge();
                    sht.Range("S3:T3").Style.Alignment.SetWrapText();
                    sht.Range("S3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S3:T3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S3:T3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U3").Value = ds.Tables[0].Rows[0]["Emp2PanNo"];
                    sht.Range("U3:X3").Style.Font.FontName = "Calibri";
                    sht.Range("U3:X3").Style.Font.FontSize = 11;
                    sht.Range("U3:X3").Merge();
                    sht.Range("U3:X3").Style.Alignment.SetWrapText();
                    sht.Range("U3:X3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U3:X3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y3").Value = "Folio No.";
                    sht.Range("Y3:Z3").Style.Font.FontName = "Calibri";
                    sht.Range("Y3:Z3").Style.Font.FontSize = 11;
                    sht.Range("Y3:Z3").Style.Font.SetBold();
                    sht.Range("Y3:Z3").Merge();
                    sht.Range("Y3:Z3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y3:Z3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y3:Z3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA3").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
                    sht.Range("AA3:AB3").Style.Font.FontName = "Calibri";
                    sht.Range("AA3:AB3").Style.Font.FontSize = 11;
                    sht.Range("AA3:AB3").Merge();
                    sht.Range("AA3:AB3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA3:AB3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I4").Value = "Address";
                    sht.Range("I4:K5").Style.Font.FontName = "Calibri";
                    sht.Range("I4:K5").Style.Font.FontSize = 11;
                    sht.Range("I4:K5").Style.Font.SetBold();
                    sht.Range("I4:K5").Merge();
                    sht.Range("I4:K5").Style.Alignment.SetWrapText();
                    sht.Range("I4:K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I4:K5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I4:K5").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L4").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
                    sht.Range("L4:R5").Style.Font.FontName = "Calibri";
                    sht.Range("L4:R5").Style.Font.FontSize = 11;
                    sht.Range("L4:R5").Merge();
                    sht.Range("L4:R5").Style.Alignment.SetWrapText();
                    sht.Range("L4:R5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L4:R5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S4").Value = "Aadhar No";
                    sht.Range("S4:T4").Style.Font.FontName = "Calibri";
                    sht.Range("S4:T4").Style.Font.FontSize = 11;
                    sht.Range("S4:T4").Style.Font.SetBold();
                    sht.Range("S4:T4").Merge();
                    sht.Range("S4:T4").Style.Alignment.SetWrapText();
                    sht.Range("S4:T4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S4:T4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S4:T4").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U4").Value = ds.Tables[0].Rows[0]["Emp2AadharNo"];
                    sht.Range("U4:X4").Style.Font.FontName = "Calibri";
                    sht.Range("U4:X4").Style.Font.FontSize = 11;
                    sht.Range("U4:X4").Style.NumberFormat.NumberFormatId = 1;
                    sht.Range("U4:X4").Merge();
                    sht.Range("U4:X4").Style.Alignment.SetWrapText();
                    sht.Range("U4:X4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U4:X4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y4").Value = "Order Date";
                    sht.Range("Y4:Z4").Style.Font.FontName = "Calibri";
                    sht.Range("Y4:Z4").Style.Font.FontSize = 11;
                    sht.Range("Y4:Z4").Style.Font.SetBold();
                    sht.Range("Y4:Z4").Merge();
                    sht.Range("Y4:Z4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y4:Z4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y4:Z4").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA4").Value = ds.Tables[0].Rows[0]["AssignDate"];
                    sht.Range("AA4:AB4").Style.Font.FontName = "Calibri";
                    sht.Range("AA4:AB4").Style.Font.FontSize = 11;
                    sht.Range("AA4:AB4").Merge();
                    sht.Range("AA4:AB4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA4:AB4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S5").Value = "Mobile No";
                    sht.Range("S5:T5").Style.Font.FontName = "Calibri";
                    sht.Range("S5:T5").Style.Font.FontSize = 11;
                    sht.Range("S5:T5").Style.Font.SetBold();
                    sht.Range("S5:T5").Merge();
                    sht.Range("S5:T5").Style.Alignment.SetWrapText();
                    sht.Range("S5:T5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S5:T5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S5:T5").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U5").Value = ds.Tables[0].Rows[0]["Emp2MobileNo"];
                    sht.Range("U5:X5").Style.Font.FontName = "Calibri";
                    sht.Range("U5:X5").Style.Font.FontSize = 11;
                    sht.Range("U5:X5").Merge();
                    sht.Range("U5:X5").Style.Alignment.SetWrapText();
                    sht.Range("U5:X5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U5:X5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y5").Value = "Delivery Date";
                    sht.Range("Y5:Z5").Style.Font.FontName = "Calibri";
                    sht.Range("Y5:Z5").Style.Font.FontSize = 11;
                    sht.Range("Y5:Z5").Style.Font.SetBold();
                    sht.Range("Y5:Z5").Merge();
                    sht.Range("Y5:Z5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y5:Z5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y5:Z5").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA5").Value = ds.Tables[0].Rows[0]["REQBYDATE"];
                    sht.Range("AA5:AB5").Style.Font.FontName = "Calibri";
                    sht.Range("AA5:AB5").Style.Font.FontSize = 11;
                    sht.Range("AA5:AB5").Merge();
                    sht.Range("AA5:AB5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA5:AB5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A6").Value = "Quality (Weight)";
                    sht.Range("A6:B6").Style.Font.FontName = "Calibri";
                    sht.Range("A6:B6").Style.Font.FontSize = 11;
                    sht.Range("A6:B6").Style.Font.SetBold();
                    sht.Range("A6:B6").Merge();
                    sht.Range("A6:B6").Style.Alignment.SetWrapText();
                    sht.Range("A6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A6:B6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("A7").Value = ds.Tables[0].Rows[0]["QualityGrmPerMeterMinus"];
                    sht.Range("A7:B8").Style.Font.FontName = "Calibri";
                    sht.Range("A7:B8").Style.Font.FontSize = 11;
                    //sht.Range("B6:D6").Style.Font.SetBold();
                    sht.Range("A7:B8").Merge();
                    sht.Range("A7:B8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A7:B8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("C6").Value = "Quality (Product)";
                    sht.Range("C6:E6").Style.Font.FontName = "Calibri";
                    sht.Range("C6:E6").Style.Font.FontSize = 11;
                    sht.Range("C6:E6").Style.Font.SetBold();
                    sht.Range("C6:E6").Merge();
                    sht.Range("C6:E6").Style.Alignment.SetWrapText();
                    sht.Range("C6:E6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C6:E6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C6:E6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("C7").Value = ds.Tables[0].Rows[0]["QUALITYNAME"];
                    sht.Range("C7:E8").Style.Font.FontName = "Calibri";
                    sht.Range("C7:E8").Style.Font.FontSize = 11;
                    //sht.Range("B6:D6").Style.Font.SetBold();
                    sht.Range("C7:E8").Merge();
                    sht.Range("C7:E8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C7:E8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("F6").Value = "Date";
                    sht.Range("F6:H6").Style.Font.FontName = "Calibri";
                    sht.Range("F6:H6").Style.Font.FontSize = 11;
                    sht.Range("F6:H6").Style.Font.SetBold();
                    sht.Range("F6:H6").Merge();
                    sht.Range("F6:H6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F6:H6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F6:H6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("F7").Value = "Slip No.";
                    sht.Range("F7:H7").Style.Font.FontName = "Calibri";
                    sht.Range("F7:H7").Style.Font.FontSize = 11;
                    sht.Range("F7:H7").Style.Font.SetBold();
                    sht.Range("F7:H7").Merge();
                    sht.Range("F7:H7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F7:H7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F7:H7").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("F8").Value = "Weight ";
                    sht.Range("F8:H8").Style.Font.FontName = "Calibri";
                    sht.Range("F8:H8").Style.Font.FontSize = 11;
                    sht.Range("F8:H8").Style.Font.SetBold();
                    sht.Range("F8:H8").Merge();
                    sht.Range("F8:H8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F8:H8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F8:H8").Style.Fill.BackgroundColor = XLColor.LightGray;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row2 = 6;
                        int noofrows2 = 0;
                        int i2 = 0;
                        int Dynamiccol2 = 8;
                        int Dynamiccolstart2 = Dynamiccol2 + 1;
                        int Dynamiccolend2;
                        int Totalcol2;

                        DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "ReceiveDate");
                        noofrows2 = dtdistinct2.Rows.Count;

                        for (i2 = 0; i2 < noofrows2; i2++)
                        {
                            Dynamiccol2 = Dynamiccol2 + 1;
                            sht.Range(sht.Cell(row2, Dynamiccol2), sht.Cell(row2, Dynamiccol2 + 1)).Merge();

                            sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            Dynamiccol2 = Dynamiccol2 + 1;
                        }
                        Dynamiccolend2 = Dynamiccolstart2 + noofrows2;

                        for (i2 = noofrows2; i2 < 10; i2++)
                        {
                            Dynamiccol2 = Dynamiccol2 + 1;
                            sht.Range(sht.Cell(row2, Dynamiccol2), sht.Cell(row2, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Range(sht.Cell(row2 + 1, Dynamiccol2), sht.Cell(row2 + 1, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetWrapText();
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Range(sht.Cell(row2 + 2, Dynamiccol2), sht.Cell(row2 + 2, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetWrapText();
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            Dynamiccol2 = Dynamiccol2 + 1;
                        }

                        row2 = row2 + 1;
                        int Col = Dynamiccolstart2;

                        for (int k = Dynamiccolstart2; k < Dynamiccolend2; k++)
                        {
                            var Date = sht.Cell(6, Col).Value;
                            decimal WeightQty = 0;
                            decimal Penality = 0;
                            string ProcessRecId = "";

                            DataRow[] foundRows;
                            foundRows = ds.Tables[1].Select("ReceiveDate='" + Date + "' ");
                            if (foundRows.Length > 0)
                            {
                                foreach (DataRow row3 in foundRows)
                                {
                                    ProcessRecId = row3["ProcessRecId"].ToString();
                                    break;
                                }

                                WeightQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(Weight)", "ReceiveDate='" + Date + "' "));
                                Penality = Convert.ToDecimal(ds.Tables[1].Compute("sum(Penality)", "ReceiveDate='" + Date + "' "));
                            }
                            sht.Cell(row2, Col).SetValue(ProcessRecId);
                            sht.Range(sht.Cell(row2, Col), sht.Cell(row2, Col + 1)).Merge();
                            sht.Cell(row2, Col).Style.Alignment.SetWrapText();
                            sht.Cell(row2, Col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2, Col).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Cell(8, Col).Value = WeightQty;
                            sht.Range(sht.Cell(8, Col), sht.Cell(8, Col + 1)).Merge();

                            sht.Cell(8, Col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(8, Col).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            Col = Col + 2;
                            //sht.Cell(10, k).Value = Penality;
                        }
                    }
                    else
                    {
                        int noofrows2 = 0;
                        int Dynamiccol2 = 8;
                        int row2 = 6;
                        for (int i2 = noofrows2; i2 < 10; i2++)
                        {
                            Dynamiccol2 = Dynamiccol2 + 1;
                            sht.Range(sht.Cell(row2, Dynamiccol2), sht.Cell(row2, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Range(sht.Cell(row2 + 1, Dynamiccol2), sht.Cell(row2 + 1, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetWrapText();
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2 + 1, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Range(sht.Cell(row2 + 2, Dynamiccol2), sht.Cell(row2 + 2, Dynamiccol2 + 1)).Merge();
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetWrapText();
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2 + 2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            Dynamiccol2 = Dynamiccol2 + 1;
                        }
                    }

                    sht.Range("A9").Value = "Design";
                    sht.Range("A9:B9").Style.Font.FontName = "Calibri";
                    sht.Range("A9:B9").Style.Font.FontSize = 11;
                    sht.Range("A9:B9").Style.Font.SetBold();
                    sht.Range("A9:B9").Merge();
                    sht.Range("A9:B9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A9:B9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A9:B9").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("C9").Value = "Color ";
                    sht.Range("C9:D9").Style.Font.FontName = "Calibri";
                    sht.Range("C9:D9").Style.Font.FontSize = 11;
                    sht.Range("C9:D9").Style.Font.SetBold();
                    sht.Range("C9:D9").Merge();
                    sht.Range("C9:D9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C9:D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C9:D9").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("E9").Value = "Size(" + ds.Tables[0].Rows[0]["UnitName"] + ")";
                    sht.Range("E9:G9").Style.Font.FontName = "Calibri";
                    sht.Range("E9:G9").Style.Font.FontSize = 11;
                    sht.Range("E9:G9").Style.Font.SetBold();
                    sht.Range("E9:G9").Merge();
                    sht.Range("E9:G9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E9:G9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E9:G9").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("H9").Value = "Qty";
                    sht.Range("H9").Style.Font.FontName = "Calibri";
                    sht.Range("H9").Style.Font.FontSize = 11;
                    sht.Range("H9").Style.Font.SetBold();
                    sht.Range("H9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("H9").Style.Fill.BackgroundColor = XLColor.LightGray;

                    int row = 9;
                    int column = 4;
                    int noofrows = 0;
                    int i = 0;
                    int Dynamiccol = 8;
                    int Dynamiccolstart = Dynamiccol + 1;
                    int Dynamiccolend;
                    int Totalcol;
                    decimal Area = 0;
                    decimal TotalArea = 0;

                    DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "ReceiveDate");
                    noofrows = dtdistinct.Rows.Count;

                    for (i = 0; i < noofrows; i++)
                    {
                        Dynamiccol = Dynamiccol + 1;
                        sht.Range(sht.Cell(row, Dynamiccol), sht.Cell(row, Dynamiccol + 1)).Merge();

                        sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["ReceiveDate"].ToString();
                        sht.Cell(row, Dynamiccol).Style.Font.Bold = true;
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        Dynamiccol = Dynamiccol + 1;
                    }
                    Dynamiccolend = Dynamiccol;
                    Totalcol = Dynamiccolend + 1;

                    sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol + 1)).Merge();
                    sht.Cell(row, Totalcol).Value = "Total ";
                    sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol).Style.Font.FontSize = 11;
                    sht.Cell(row, Totalcol).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    for (i = noofrows + 1; i <= 10; i++)
                    {
                        Dynamiccol = Dynamiccol + 1;
                        sht.Range(sht.Cell(row, Dynamiccol), sht.Cell(row, Dynamiccol + 1)).Merge();
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        Dynamiccol = Dynamiccol + 1;
                    }
                    row = row + 2;

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["DesignName"]);
                        sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Calibri";
                        sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                        sht.Range("A" + row + ":B" + row).Style.Font.SetBold();
                        sht.Range("A" + row + ":B" + row).Merge();
                        sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("A" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["ColorName"]);
                        sht.Range("C" + row + ":D" + row).Style.Font.FontName = "Calibri";
                        sht.Range("C" + row + ":D" + row).Style.Font.FontSize = 11;
                        sht.Range("C" + row + ":D" + row).Style.Font.SetBold();
                        sht.Range("C" + row + ":D" + row).Merge();
                        sht.Range("C" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("C" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["Size"]);
                        sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Calibri";
                        sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row + ":G" + row).Style.Font.SetBold();
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["OrderQty"]);
                        sht.Range("H" + row).Style.Font.FontName = "Calibri";
                        sht.Range("H" + row).Style.Font.FontSize = 11;
                        sht.Range("H" + row).Style.Font.SetBold();
                        sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("H" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        Area = Convert.ToDecimal(ds.Tables[0].Rows[j]["Area"]);
                        TotalArea = TotalArea + Area;

                        decimal TotalSumOneRow = 0;
                        for (int k = Dynamiccolstart; k <= Dynamiccolend; k = k + 2)
                        {
                            var Date = sht.Cell(9, k).Value;
                            decimal RecQty = 0;

                            DataRow[] foundRows;
                            foundRows = ds.Tables[1].Select("ReceiveDate='" + Date + "' and Item_Finished_Id='" + ds.Tables[0].Rows[j]["Item_Finished_Id"] + "' ");
                            if (foundRows.Length > 0)
                            {
                                RecQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(Pcs)", "ReceiveDate='" + Date + "' and Item_Finished_Id='" + ds.Tables[0].Rows[j]["Item_Finished_Id"] + "' "));
                            }
                            //IssRecConQty = IssQty + RecQty + ConsQty;
                            TotalSumOneRow = TotalSumOneRow + RecQty;
                            sht.Range(sht.Cell(row, k), sht.Cell(row, k + 1)).Merge();
                            sht.Cell(row, k).Value = RecQty;
                            sht.Cell(row, k).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, k).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        }
                        Totalcol = Dynamiccolend + 1;
                        sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol + 1)).Merge();
                        sht.Cell(row, Totalcol).Value = TotalSumOneRow;
                        sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        for (i = Totalcol + 2; i <= 28; i = i + 2)
                        {
                            sht.Range(sht.Cell(row, i), sht.Cell(row, i + 1)).Merge();
                            sht.Cell(row, i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        }
                        row = row + 1;
                    }
                    for (i = row; i <= 17; i++)
                    {
                        sht.Range(sht.Cell(row, 1), sht.Cell(row, 2)).Merge();
                        sht.Range(sht.Cell(row, 3), sht.Cell(row, 4)).Merge();
                        sht.Range(sht.Cell(row, 5), sht.Cell(row, 7)).Merge();
                        for (int Col = 9; Col < 28; Col = Col + 2)
                        {
                            sht.Range(sht.Cell(row, Col), sht.Cell(row, Col + 1)).Merge();
                            sht.Cell(row, Col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, Col).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        }
                        row = row + 1;
                    }

                    sht.Range("A" + row).Value = "Total";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("C" + row).Value = "Area";
                    sht.Range("C" + row + ":D" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":D" + row).Style.Font.FontSize = 11;
                    sht.Range("C" + row + ":D" + row).Style.Font.SetBold();
                    sht.Range("C" + row + ":D" + row).Merge();
                    sht.Range("C" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E" + row).Value = TotalArea;
                    sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Calibri";
                    sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 11;
                    sht.Range("E" + row + ":G" + row).Style.Font.SetBold();
                    sht.Range("E" + row + ":G" + row).Merge();
                    sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("H" + row).FormulaA1 = "=SUM(H11" + ":$H$" + (row - 1) + ")";
                    sht.Range("H" + row).Style.Font.FontName = "Calibri";
                    sht.Range("H" + row).Style.Font.FontSize = 11;
                    sht.Range("H" + row).Style.Font.SetBold();
                    sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I" + row).FormulaA1 = "=SUM(I11" + ":$I$" + (row - 1) + ")";
                    sht.Range("I" + row + ":J" + row).Style.Font.FontName = "Calibri";
                    sht.Range("I" + row + ":J" + row).Style.Font.FontSize = 11;
                    sht.Range("I" + row + ":J" + row).Style.Font.SetBold();
                    sht.Range("I" + row + ":J" + row).Merge();
                    sht.Range("I" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("K" + row).FormulaA1 = "=SUM(K11" + ":$K$" + (row - 1) + ")";
                    sht.Range("K" + row + ":L" + row).Style.Font.FontName = "Calibri";
                    sht.Range("K" + row + ":L" + row).Style.Font.FontSize = 11;
                    sht.Range("K" + row + ":L" + row).Style.Font.SetBold();
                    sht.Range("K" + row + ":L" + row).Merge();
                    sht.Range("K" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K" + row + ":L" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("M" + row).FormulaA1 = "=SUM(M11" + ":$M$" + (row - 1) + ")";
                    sht.Range("M" + row + ":N" + row).Style.Font.FontName = "Calibri";
                    sht.Range("M" + row + ":N" + row).Style.Font.FontSize = 11;
                    sht.Range("M" + row + ":N" + row).Style.Font.SetBold();
                    sht.Range("M" + row + ":N" + row).Merge();
                    sht.Range("M" + row + ":N" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M" + row + ":N" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("O" + row).FormulaA1 = "=SUM(O11" + ":$O$" + (row - 1) + ")";
                    sht.Range("O" + row + ":P" + row).Style.Font.FontName = "Calibri";
                    sht.Range("O" + row + ":P" + row).Style.Font.FontSize = 11;
                    sht.Range("O" + row + ":P" + row).Style.Font.SetBold();
                    sht.Range("O" + row + ":P" + row).Merge();
                    sht.Range("O" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("O" + row + ":P" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Q" + row).FormulaA1 = "=SUM(Q11" + ":$Q$" + (row - 1) + ")";
                    sht.Range("Q" + row + ":R" + row).Style.Font.FontName = "Calibri";
                    sht.Range("Q" + row + ":R" + row).Style.Font.FontSize = 11;
                    sht.Range("Q" + row + ":R" + row).Style.Font.SetBold();
                    sht.Range("Q" + row + ":R" + row).Merge();
                    sht.Range("Q" + row + ":R" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Q" + row + ":R" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S" + row).FormulaA1 = "=SUM(S11" + ":$S$" + (row - 1) + ")";
                    sht.Range("S" + row + ":T" + row).Style.Font.FontName = "Calibri";
                    sht.Range("S" + row + ":T" + row).Style.Font.FontSize = 11;
                    sht.Range("S" + row + ":T" + row).Style.Font.SetBold();
                    sht.Range("S" + row + ":T" + row).Merge();
                    sht.Range("S" + row + ":T" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S" + row + ":T" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("U" + row).FormulaA1 = "=SUM(U11" + ":$U$" + (row - 1) + ")";
                    sht.Range("U" + row + ":V" + row).Style.Font.FontName = "Calibri";
                    sht.Range("U" + row + ":V" + row).Style.Font.FontSize = 11;
                    sht.Range("U" + row + ":V" + row).Style.Font.SetBold();
                    sht.Range("U" + row + ":V" + row).Merge();
                    sht.Range("U" + row + ":V" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U" + row + ":V" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("W" + row).FormulaA1 = "=SUM(W11" + ":$W$" + (row - 1) + ")";
                    sht.Range("W" + row + ":X" + row).Style.Font.FontName = "Calibri";
                    sht.Range("W" + row + ":X" + row).Style.Font.FontSize = 11;
                    sht.Range("W" + row + ":X" + row).Style.Font.SetBold();
                    sht.Range("W" + row + ":X" + row).Merge();
                    sht.Range("W" + row + ":X" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("W" + row + ":X" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y" + row).FormulaA1 = "=SUM(Y11" + ":$Y$" + (row - 1) + ")";
                    sht.Range("Y" + row + ":Z" + row).Style.Font.FontName = "Calibri";
                    sht.Range("Y" + row + ":Z" + row).Style.Font.FontSize = 11;
                    sht.Range("Y" + row + ":Z" + row).Style.Font.SetBold();
                    sht.Range("Y" + row + ":Z" + row).Merge();
                    sht.Range("Y" + row + ":Z" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y" + row + ":Z" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("AA" + row).FormulaA1 = "=SUM(AA11" + ":$AA$" + (row - 1) + ")";
                    sht.Range("AA" + row + ":AB" + row).Style.Font.FontName = "Calibri";
                    sht.Range("AA" + row + ":AB" + row).Style.Font.FontSize = 11;
                    sht.Range("AA" + row + ":AB" + row).Style.Font.SetBold();
                    sht.Range("AA" + row + ":AB" + row).Merge();
                    sht.Range("AA" + row + ":AB" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA" + row + ":AB" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    row = row + 1;

                    sht.Range("A19").Value = "Issued By (" + ds.Tables[0].Rows[0]["UserName"] + ")";
                    sht.Range("A19:D27").Style.Font.FontName = "Calibri";
                    sht.Range("A19:D27").Style.Font.FontSize = 11;
                    sht.Range("A19:D27").Style.Font.SetBold();
                    sht.Range("A19:D27").Merge();
                    sht.Range("A19:D27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A19:D27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("E19").Value = "Received By";
                    sht.Range("E19:H27").Style.Font.FontName = "Calibri";
                    sht.Range("E19:H27").Style.Font.FontSize = 11;
                    sht.Range("E19:H27").Style.Font.SetBold();
                    sht.Range("E19:H27").Merge();
                    sht.Range("E19:H27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E19:H27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("I19").Value = "WEAVING /- SQ MTR FULL";
                    sht.Range("I19:L19").Style.Font.FontSize = 11;
                    sht.Range("I19:L19").Style.Font.SetBold();
                    sht.Range("I19:L19").Merge();
                    sht.Range("I19:L19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I19:L19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("M19").Value = "SIZE";
                    sht.Range("M19:N19").Style.Font.FontSize = 11;
                    sht.Range("M19:N19").Style.Font.SetBold();
                    sht.Range("M19:N19").Merge();
                    sht.Range("M19:N19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M19:N19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("O19").Value = "WEAVING /- PC";
                    sht.Range("O19:R19").Style.Font.FontSize = 11;
                    sht.Range("O19:R19").Style.Font.SetBold();
                    sht.Range("O19:R19").Merge();
                    sht.Range("O19:R19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("O19:R19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("S19").Value = "AFTER WEAVING";
                    sht.Range("S19:V19").Style.Font.FontName = "Calibri";
                    sht.Range("S19:V19").Style.Font.FontSize = 11;
                    sht.Range("S19:V19").Style.Font.SetBold();
                    sht.Range("S19:V19").Merge();
                    sht.Range("S19:V19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S19:V19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("S20:V27").Style.Font.FontName = "Calibri";
                    sht.Range("S20:V27").Style.Font.FontSize = 11;
                    sht.Range("S20:V27").Style.Font.SetBold();
                    sht.Range("S20:V27").Merge();
                    sht.Range("S20:V27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S20:V27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("W19:AB25").Style.Font.FontName = "Calibri";
                    sht.Range("W19:AB25").Style.Font.FontSize = 11;
                    sht.Range("W19:AB25").Style.Font.SetBold();
                    sht.Range("W19:AB25").Merge();
                    sht.Range("W19:AB25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("W19:AB25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("W26").Value = "Approved By (Signature)";
                    sht.Range("W26:AB26").Style.Font.FontSize = 11;
                    sht.Range("W26:AB26").Style.Font.SetBold();
                    sht.Range("W26:AB26").Merge();
                    sht.Range("W26:AB26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("W26:AB26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("W27").Value = "Hafiz Mohd. Akram";
                    sht.Range("W27:AB27").Style.Font.FontSize = 11;
                    sht.Range("W27:AB27").Style.Font.SetBold();
                    sht.Range("W27:AB27").Merge();
                    sht.Range("W27:AB27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("W27:AB27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    row = row + 1;
                    int noofrowsize = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtdistinct2 = ds.Tables[0].DefaultView.ToTable(true, "Size", "WeavingSQFull", "WeavingOnePcsAmount");
                        noofrowsize = dtdistinct2.Rows.Count;
                        if (noofrowsize > 8)
                        {
                            noofrowsize = 8;
                        }
                        for (int i2 = 0; i2 < noofrowsize; i2++)
                        {
                            sht.Range(sht.Cell(row, 9), sht.Cell(row, 12)).Merge();
                            sht.Range(sht.Cell(row, 13), sht.Cell(row, 14)).Merge();
                            sht.Range(sht.Cell(row, 15), sht.Cell(row, 18)).Merge();

                            sht.Cell(row, 9).Value = dtdistinct2.Rows[i2]["WeavingSQFull"].ToString();
                            sht.Cell(row, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Cell(row, 13).Value = dtdistinct2.Rows[i2]["Size"].ToString();
                            sht.Cell(row, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Cell(row, 15).Value = dtdistinct2.Rows[i2]["WeavingOnePcsAmount"].ToString();
                            sht.Cell(row, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            row = row + 1;
                        }
                    }
                    for (int i2 = row; i2 <= 27; i2++)
                    {
                        sht.Range(sht.Cell(row, 9), sht.Cell(row, 12)).Merge();
                        sht.Range(sht.Cell(row, 13), sht.Cell(row, 14)).Merge();
                        sht.Range(sht.Cell(row, 15), sht.Cell(row, 18)).Merge();
                        sht.Cell(row, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                    }

                    sht.Range("A28").Value = "Note : -(1) ऑर्डर शीट पर Signature करने से पहले बुनाई + कमीसन (Rate) देख ले और समझ ले, उसके बाद Signature करे, Signature होने के बाद  न Rate बढ़ेगी और न ही ऑर्डर वापस लिया जाएगा और दिए गए टाइम पर ऑर्डर पूरा करना होगा |  ";
                    sht.Range("A28:AB29").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A28:AB29").Style.Font.FontSize = 11;
                    sht.Range("A28:AB29").Style.Font.SetBold();
                    sht.Range("A28:AB29").Merge();
                    sht.Range("A28:AB29").Style.Alignment.SetWrapText();
                    sht.Range("A28:AB29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A28:AB29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

                    sht.Range("A30").Value = "(2) प्रोडक्शन के 6 -7 पीस  आ  जाने के बाद 6 से 12 इंच का एक टुकड़ा बनाकर दे जिससे चेक होने के बाद बाकी का मटेरियल डाई पे जावेगा  ";
                    sht.Range("A30:AB31").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A30:AB31").Style.Font.FontSize = 12;
                    sht.Range("A30:AB31").Style.Font.SetBold();
                    sht.Range("A30:AB31").Merge();
                    sht.Range("A30:AB31").Style.Alignment.SetWrapText();
                    sht.Range("A30:AB31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A30:AB31").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    //////*************
                    // sht.Columns(1, 20).AdjustToContents();
                    //********************
                    //***********BOrders
                    using (var a = sht.Range("A1" + ":AB" + (row - 1)))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("WeaverFolioReportHindi_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }

        }

    }
    protected void FolioMaterialDetailReportHafizia()
    {
        DataSet ds = new DataSet();
        if (DDFolioNo.SelectedIndex > 0)
        {
            lblmsg.Text = "";
            try
            {

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
                //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);

                //SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("FolioMaterialReport");
                    //int row = 0;

                    sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    sht.PageSetup.AdjustTo(85);
                    sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

                    //
                    sht.PageSetup.Margins.Top = 1.21;
                    sht.PageSetup.Margins.Left = 0.47;
                    sht.PageSetup.Margins.Right = 0.36;
                    sht.PageSetup.Margins.Bottom = 0.19;
                    sht.PageSetup.Margins.Header = 2.20;
                    sht.PageSetup.Margins.Footer = 0.3;
                    sht.PageSetup.SetScaleHFWithDocument();

                    //sht.Column("A").Width = 22.78;
                    //sht.Column("B").Width = 18.33;
                    //sht.Column("C").Width = 11.67;
                    //sht.Column("D").Width = 8.22;
                    //sht.Column("E").Width = 10.67;
                    //sht.Column("F").Width = 10.33;
                    //sht.Column("G").Width = 10.33;
                    //sht.Column("H").Width = 10.33;
                    //sht.Column("I").Width = 10.33;
                    //sht.Column("J").Width = 10.33;
                    //sht.Column("K").Width = 10.33;
                    //sht.Column("L").Width = 10.33;
                    //sht.Column("M").Width = 10.33;
                    //sht.Column("N").Width = 10.33;
                    //sht.Column("O").Width = 10.33;
                    //sht.Column("P").Width = 10.33;
                    //sht.Column("Q").Width = 10.33;
                    //sht.Column("R").Width = 10.33;

                    sht.Column("A").Width = 15.67;
                    sht.Column("B").Width = 15.22;
                    sht.Column("C").Width = 7.78;
                    sht.Column("D").Width = 8.22;
                    sht.Column("E").Width = 9.22;
                    sht.Column("F").Width = 10.89;
                    sht.Column("G").Width = 11.22;
                    sht.Column("H").Width = 9.89;
                    sht.Column("I").Width = 9.89;
                    sht.Column("J").Width = 14.33;
                    sht.Column("K").Width = 9.89;
                    sht.Column("L").Width = 9.89;
                    sht.Column("M").Width = 9.89;
                    sht.Column("N").Width = 9.89;


                    sht.Row(1).Height = 35;
                    sht.Row(2).Height = 30;
                    sht.Row(3).Height = 30;
                    sht.Row(4).Height = 30;
                    sht.Row(5).Height = 30;
                    sht.Row(6).Height = 30;
                    sht.Row(7).Height = 30;
                    sht.Row(8).Height = 30;
                    sht.Row(9).Height = 30;
                    sht.Row(10).Height = 30;
                    sht.Row(11).Height = 30;

                    // sht.Row(1).Height = 13.80;

                    sht.Range("A1:N1").Merge();
                    sht.Range("A1").Value = "RAW MATERIAL ISSUE (JOB WORK)";

                    sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:N1").Style.Alignment.SetWrapText();
                    sht.Range("A1:N1").Style.Font.FontName = "Calibri";
                    sht.Range("A1:N1").Style.Font.FontSize = 18;
                    sht.Range("A1:N1").Style.Font.Bold = true;
                    //*******Header

                    sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
                    sht.Range("A2:B2").Style.Font.FontName = "Calibri";
                    sht.Range("A2:B2").Style.Font.FontSize = 14;
                    sht.Range("A2:B2").Style.Font.SetBold();
                    sht.Range("A2:B2").Merge();
                    sht.Range("A2:B2").Style.Alignment.SetWrapText();
                    sht.Range("A2:B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A2:B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"];
                    sht.Range("A3:B4").Style.Font.FontName = "Calibri";
                    sht.Range("A3:B4").Style.Font.FontSize = 11;
                    sht.Range("A3:B4").Style.Font.SetBold();
                    sht.Range("A3:B4").Merge();
                    sht.Range("A3:B4").Style.Alignment.SetWrapText();
                    sht.Range("A3:B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A3:B4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A5").Value = "GSTIN";
                    sht.Range("A5").Style.Font.FontName = "Calibri";
                    sht.Range("A5").Style.Font.FontSize = 10;
                    sht.Range("A5").Style.Font.SetBold();

                    sht.Range("B5").Value = ds.Tables[0].Rows[0]["CompanyGstNo"];
                    sht.Range("B5:B5").Style.Font.FontName = "Calibri";
                    sht.Range("B5:B5").Style.Font.FontSize = 10;
                    //sht.Range("B5:D5").Style.Font.SetBold();
                    sht.Range("B5:B5").Merge();
                    sht.Range("B5:B5").Style.Alignment.SetWrapText();
                    sht.Range("B5:B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("A6").Value = "Phone No";
                    sht.Range("A6").Style.Font.FontName = "Calibri";
                    sht.Range("A6").Style.Font.FontSize = 10;
                    sht.Range("A6").Style.Font.SetBold();
                    sht.Range("A6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("B6").Value = ds.Tables[0].Rows[0]["CompanyPhoneNo"];
                    sht.Range("B6:B6").Style.Font.FontName = "Calibri";
                    sht.Range("B6:B6").Style.Font.FontSize = 10;
                    //sht.Range("B5:D5").Style.Font.SetBold();
                    sht.Range("B6:B6").Merge();
                    sht.Range("B6:B6").Style.Alignment.SetWrapText();
                    sht.Range("B6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    sht.Range("C2").Value = "Weaver Name & Address";
                    sht.Range("C2:D3").Style.Font.FontName = "Calibri";
                    sht.Range("C2:D3").Style.Font.FontSize = 11;
                    sht.Range("C2:D3").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("C2:D3").Merge();
                    sht.Range("C2:D3").Style.Alignment.SetWrapText();
                    sht.Range("C2:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C2:D3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    sht.Range("E2").Value = ds.Tables[0].Rows[0]["EmpName"];
                    sht.Range("E2:G2").Style.Font.FontName = "Calibri";
                    sht.Range("E2:G2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("E2:G2").Merge();
                    sht.Range("E2:G2").Style.Alignment.SetWrapText();
                    sht.Range("E2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //sht.Range("E2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E3").Value = ds.Tables[0].Rows[0]["Address"];
                    sht.Range("E3:G3").Style.Font.FontName = "Calibri";
                    sht.Range("E3:G3").Style.Font.FontSize = 11;
                    //sht.Range("E2:N5").Style.Font.SetBold();
                    sht.Range("E3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("E3:G3").Merge();
                    sht.Range("E2:G3").Style.Alignment.SetWrapText();

                    sht.Range("C4").Value = "Deliver To";
                    sht.Range("C4:D4").Style.Font.FontName = "Calibri";
                    sht.Range("C4:D4").Style.Font.FontSize = 11;
                    sht.Range("C4:D4").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("C4:D4").Merge();
                    sht.Range("C4:D4").Style.Alignment.SetWrapText();
                    sht.Range("C4:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C4:D4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E4").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
                    sht.Range("E4:G4").Style.Font.FontName = "Calibri";
                    sht.Range("E4:G4").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("E4:G4").Merge();
                    sht.Range("E4:G4").Style.Alignment.SetWrapText();
                    sht.Range("E4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E4:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("C5").Value = "Deliver To Address";
                    sht.Range("C5:D6").Style.Font.FontName = "Calibri";
                    sht.Range("C5:D6").Style.Font.FontSize = 11;
                    sht.Range("C5:D6").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("C5:D6").Merge();
                    sht.Range("C5:D6").Style.Alignment.SetWrapText();
                    sht.Range("C5:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C5:D6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E5").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
                    sht.Range("E5:G6").Style.Font.FontName = "Calibri";
                    sht.Range("E5:G6").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("E5:G6").Merge();
                    sht.Range("E5:G6").Style.Alignment.SetWrapText();
                    sht.Range("E5:G6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E5:G6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("H2").Value = "Pan No";
                    sht.Range("H2:I2").Style.Font.FontName = "Calibri";
                    sht.Range("H2:I2").Style.Font.FontSize = 11;
                    sht.Range("H2:I2").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("H2:I2").Merge();
                    sht.Range("H2:I2").Style.Alignment.SetWrapText();
                    sht.Range("H2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("J2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
                    sht.Range("J2:J2").Style.Font.FontName = "Calibri";
                    sht.Range("J2:J2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("J2:J2").Merge();
                    sht.Range("J2:J2").Style.Alignment.SetWrapText();
                    sht.Range("J2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("J2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("H3").Value = "Aadhar No";
                    sht.Range("H3:I3").Style.Font.FontName = "Calibri";
                    sht.Range("H3:I3").Style.Font.FontSize = 11;
                    sht.Range("H3:I3").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("H3:I3").Merge();
                    sht.Range("H3:I3").Style.Alignment.SetWrapText();
                    sht.Range("H3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H3:I3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("J3:J3").Style.NumberFormat.Format = "@";
                    sht.Range("J3").Value = ds.Tables[0].Rows[0]["EmpAadharNo"];
                    sht.Range("J3:J3").Style.Font.FontName = "Calibri";
                    sht.Range("J3:J3").Style.Font.FontSize = 11;
                    sht.Range("J3:J3").Style.NumberFormat.NumberFormatId = 1;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("J3:J3").Merge();
                    sht.Range("J3:J3").Style.Alignment.SetWrapText();
                    sht.Range("J3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("J3:J3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    sht.Range("H4").Value = "Mobile No";
                    sht.Range("H4:I4").Style.Font.FontName = "Calibri";
                    sht.Range("H4:I4").Style.Font.FontSize = 11;
                    sht.Range("H4:I4").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("H4:I4").Merge();
                    sht.Range("H4:I4").Style.Alignment.SetWrapText();
                    sht.Range("H4:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H4:I4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("J4").Value = ds.Tables[0].Rows[0]["EmpMobileNo"];
                    sht.Range("J4:J4").Style.Font.FontName = "Calibri";
                    sht.Range("J4:J4").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("J4:J4").Merge();
                    sht.Range("J4:J4").Style.Alignment.SetWrapText();
                    sht.Range("J4:J4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("J4:J4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    sht.Range("K2:L2").Value = "FOLIO NO.";
                    sht.Range("K2:L2").Style.Font.FontName = "Calibri";
                    sht.Range("K2:L2").Style.Font.FontSize = 11;
                    sht.Range("K2:L2").Style.Font.SetBold();
                    sht.Range("K2:L2").Merge();
                    sht.Range("K2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("M2").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
                    sht.Range("M2:N2").Style.Font.FontName = "Calibri";
                    sht.Range("M2:N2").Style.Font.FontSize = 11;
                    //sht.Range("Q2:R2").Style.Font.SetBold();
                    sht.Range("M2:N2").Merge();
                    sht.Range("M2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("K3").Value = "SR.NO";
                    sht.Range("K3:L3").Style.Font.FontName = "Calibri";
                    sht.Range("K3:L3").Style.Font.FontSize = 11;
                    sht.Range("K3:L3").Style.Font.SetBold();
                    sht.Range("K3:L3").Merge();
                    sht.Range("K3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("M3").Value = ds.Tables[0].Rows[0]["LocalOrder"];
                    sht.Range("M3:N3").Style.Font.FontName = "Calibri";
                    sht.Range("M3:N3").Style.Font.FontSize = 11;
                    //sht.Range("Q3:R3").Style.Font.SetBold();
                    sht.Range("M3:N3").Merge();
                    sht.Range("M3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M3:N3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("K4").Value = "ORDER DATE";
                    sht.Range("K4:L4").Style.Font.FontName = "Calibri";
                    sht.Range("K4:L4").Style.Font.FontSize = 11;
                    sht.Range("K4:L4").Style.Font.SetBold();
                    sht.Range("K4:L4").Merge();
                    sht.Range("K4:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K4:L4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("M4").Value = ds.Tables[0].Rows[0]["ASSIGNDATE"];
                    sht.Range("M4:N4").Style.Font.FontName = "Calibri";
                    sht.Range("M4:N4").Style.Font.FontSize = 11;
                    //sht.Range("Q5:R5").Style.Font.SetBold();
                    sht.Range("M4:N4").Merge();
                    sht.Range("M4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    sht.Range("K5").Value = "DELIVERY DATE";
                    sht.Range("K5:L5").Style.Font.FontName = "Calibri";
                    sht.Range("K5:L5").Style.Font.FontSize = 11;
                    sht.Range("K5:L5").Style.Font.SetBold();
                    sht.Range("K5:L5").Merge();
                    sht.Range("K5:L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K5:L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("M5").Value = ds.Tables[0].Rows[0]["REQBYDATE"];
                    sht.Range("M5:N5").Style.Font.FontName = "Calibri";
                    sht.Range("M5:N5").Style.Font.FontSize = 11;
                    //sht.Range("Q6:R6").Style.Font.SetBold();
                    sht.Range("M5:N5").Merge();
                    sht.Range("M5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("M5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    sht.Range("A7").Value = "Material Detail";
                    sht.Range("A7:D10").Style.Font.FontName = "Calibri";
                    sht.Range("A7:D10").Style.Font.FontSize = 11;
                    //sht.Range("B6:D6").Style.Font.SetBold();
                    sht.Range("A7:D10").Merge();
                    sht.Range("A7:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A7:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E7").Value = "Date";
                    sht.Range("E7").Style.Font.FontName = "Calibri";
                    sht.Range("E7").Style.Font.FontSize = 11;
                    sht.Range("E7").Style.Font.SetBold();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row2 = 7;
                        int noofrows2 = 0;
                        int i2 = 0;
                        int Dynamiccol2 = 5;
                        int Dynamiccolstart2 = Dynamiccol2 + 1;
                        int Dynamiccolend2;
                        int Totalcol2;

                        DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "Date");
                        noofrows2 = dtdistinct2.Rows.Count;

                        for (i2 = 0; i2 < noofrows2; i2++)
                        {
                            Dynamiccol2 = Dynamiccol2 + 1;
                            sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["Date"].ToString();
                        }
                        Dynamiccolend2 = Dynamiccol2;
                        //Totalcol2 = Dynamiccolend2 + 1;
                        //sht.Cell(row, Totalcol).Value = "Total";
                        //sht.Cell(row, Totalcol).Style.Font.Bold = true;

                        row2 = row2 + 1;

                        for (int k = Dynamiccolstart2; k <= Dynamiccolend2; k++)
                        {
                            var itemname = sht.Cell(7, k).Value;
                            string ChallanNo = "";

                            DataRow[] foundRows;
                            foundRows = ds.Tables[1].Select("Date='" + itemname + "' ");
                            if (foundRows.Length > 0)
                            {
                                foreach (DataRow row3 in foundRows)
                                {
                                    ChallanNo = row3["ChallanNo"].ToString();
                                    break;
                                }
                            }
                            sht.Cell(row2, k).SetValue(ChallanNo);
                            sht.Cell(row2, k).Style.Alignment.SetWrapText();
                            //sht.Cell(row2, k).Style.NumberFormat.NumberFormatId = 1;                            
                            //sht.Cell(row2, k).Style.Alignment.WrapText = true;
                            //sht.Cell(row2, k).Comment.Style.Alignment.SetAutomaticSize();                            

                        }
                    }

                    sht.Range("E8").Value = "Slip No";
                    sht.Range("E8").Style.Font.FontName = "Calibri";
                    sht.Range("E8").Style.Font.FontSize = 11;
                    sht.Range("E8").Style.Font.SetBold();

                    sht.Range("E9").Value = "Lagat ";
                    sht.Range("E9:E10").Style.Font.FontName = "Calibri";
                    sht.Range("E9:E10").Style.Font.FontSize = 11;
                    sht.Range("E9:E10").Style.Font.SetBold();
                    sht.Range("E9:E10").Merge();
                    sht.Range("E9").Style.Alignment.SetWrapText();
                    sht.Range("E9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Row(9).Height = 30;


                    sht.Range("A11").Value = "Yarn Detail & Count Ply ";
                    sht.Range("A11").Style.Font.FontName = "Calibri";
                    sht.Range("A11").Style.Font.FontSize = 11;
                    sht.Range("A11").Style.Font.SetBold();
                    sht.Range("A11").Merge();
                    sht.Range("A11").Style.Alignment.SetWrapText();
                    sht.Range("A11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("B11").Value = "Shade Color";
                    sht.Range("B11").Style.Font.FontName = "Calibri";
                    sht.Range("B11").Style.Font.FontSize = 11;
                    sht.Range("B11").Style.Font.SetBold();
                    sht.Range("B11").Merge();
                    sht.Range("B11").Style.Alignment.SetWrapText();
                    sht.Range("B11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("C11").Value = "Lot No";
                    sht.Range("C11").Style.Font.FontName = "Calibri";
                    sht.Range("C11").Style.Font.FontSize = 11;
                    sht.Range("C11").Style.Font.SetBold();
                    sht.Range("C11").Merge();
                    sht.Range("C11").Style.Alignment.SetWrapText();
                    sht.Range("C11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("D11").Value = "WT 10%";
                    sht.Range("D11").Style.Font.FontName = "Calibri";
                    sht.Range("D11").Style.Font.FontSize = 11;
                    sht.Range("D11").Style.Font.SetBold();
                    sht.Range("D11").Merge();
                    sht.Range("D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    int row = 11;
                    int noofrows = 0;
                    int i = 0;
                    int Dynamiccol = 5;
                    int Dynamiccolstart = Dynamiccol + 1;
                    int Dynamiccolend;
                    int Totalcol;

                    DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "Date", "Type");
                    //DataView dv3 = new DataView(dtdistinct);
                    //dv3.RowFilter = "Type='ISSUE'";
                    //dv3.Sort = "Date";
                    //DataSet ds3 = new DataSet();
                    //ds3.Tables.Add(dv3.ToTable());
                    noofrows = dtdistinct.Rows.Count;

                    for (i = 0; i < noofrows; i++)
                    {
                        Dynamiccol = Dynamiccol + 1;
                        sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["Date"].ToString();
                        sht.Cell(row, Dynamiccol).Style.Font.Bold = true;
                        sht.Cell(row, Dynamiccol).Style.Font.FontName = "Calibri";
                        sht.Cell(row, Dynamiccol).Style.Font.FontSize = 11;
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }
                    Dynamiccolend = Dynamiccol;
                    Totalcol = Dynamiccolend + 1;
                    sht.Cell(row, Totalcol).Value = "Total Issue";
                    sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol).Style.Font.FontSize = 11;
                    sht.Cell(row, Totalcol).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Cell(row, Totalcol + 1).Value = "Total Receive";
                    sht.Cell(row, Totalcol + 1).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol + 1).Style.Font.FontSize = 11;
                    sht.Cell(row, Totalcol + 1).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Row(row).Height = 30;
                    row = row + 1;

                    DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                    DataView dv1 = new DataView(dtdistinctDateType);
                    dv1.Sort = "Item_Name";
                    DataTable dtdistinctDateType1 = dv1.ToTable();

                    foreach (DataRow dr in dtdistinctDateType1.Rows)
                    {
                        decimal RecQty = 0;

                        //sht.Range("A" + row + ":D" + row).Merge();
                        //sht.Range("A" + row + ":D" + row).SetValue(dr["Item_Name"] + " " + dr["QualityName"] + " " + dr["DesignName"] + " " + dr["ShadeColorName"]);

                        sht.Range("A" + row).SetValue(dr["Item_Name"] + " " + dr["QualityName"]);
                        sht.Range("A" + row).Style.Alignment.SetWrapText();
                        sht.Range("B" + row).SetValue(dr["ShadeColorName"]);
                        sht.Range("C" + row).SetValue("");
                        sht.Range("D" + row).SetValue("");
                        sht.Range("E" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Compute("sum(Qty)", "Item_Name='" + dr["Item_Name"] + "' and QualityName='" + dr["QualityName"] + "' and DesignName='" + dr["DesignName"] + "' and ShadeColorName='" + dr["ShadeColorName"] + "' ")));

                        sht.Range("A" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("A" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        DataTable dtdistinctDateType2 = ds.Tables[1].DefaultView.ToTable(true, "Date", "Type");
                        DataView dv2 = new DataView(dtdistinctDateType2);
                        dv2.Sort = "Date";
                        DataTable dtdistinctDateType3 = dv2.ToTable();
                        foreach (DataRow dr2 in dtdistinctDateType3.Rows)
                        {
                            //sht.Range("F" + row).SetValue(dr2["Type"]);

                            decimal TotalSumOneRow = 0;
                            for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                            {
                                var itemname = sht.Cell(11, k).Value;
                                decimal IssQty = 0;

                                decimal IssRecConQty = 0;

                                DataRow[] foundRows;
                                foundRows = ds.Tables[1].Select("Date='" + itemname + "' and Type='" + dr2["Type"] + "' and FinishedId='" + dr["FinishedId"] + "' ");
                                if (foundRows.Length > 0)
                                {
                                    if (dr2["Type"].ToString() == "ISSUE")
                                    {
                                        IssQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(IssueQty)", "Date='" + itemname + "' and Type='" + dr2["Type"] + "' and FinishedId='" + dr["FinishedId"] + "' "));
                                    }
                                    else if (dr2["Type"].ToString() == "RECEIVE")
                                    {
                                        RecQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(RecQty)", "Type='RECEIVE' and FinishedId='" + dr["FinishedId"] + "' "));
                                    }
                                }

                                IssRecConQty = IssQty;
                                TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                                sht.Cell(row, k).Value = IssRecConQty;
                                sht.Cell(row, k).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                sht.Cell(row, k).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                            }
                            Totalcol = Dynamiccolend + 1;
                            sht.Cell(row, Totalcol).Value = TotalSumOneRow;
                            sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                            sht.Cell(row, Totalcol + 1).Value = RecQty;
                            sht.Cell(row, Totalcol + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row, Totalcol + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        }
                        sht.Row(row).Height = 30;
                        row = row + 1;

                    }
                    sht.Row(row).Height = 30;
                    row = row + 1;
                    //sht.Range("A" + row).SetValue("Total");                   


                    sht.Range("A" + row + ":C" + (row + 1)).Merge();
                    sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontSize = 12;
                    //sht.Range("B6:D6").Style.Font.SetBold();                   
                    sht.Range("A" + row + ":C" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A" + row).Value = "";

                    sht.Range("D" + row + ":J" + (row + 1)).Merge();
                    sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontSize = 12;
                    //sht.Range("B6:D6").Style.Font.SetBold();                   
                    sht.Range("D" + row + ":J" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D" + row).Value = "";

                    sht.Range("K" + row + ":N" + (row + 1)).Merge();
                    sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontName = "Calibri";
                    sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontSize = 12;
                    //sht.Range("B6:D6").Style.Font.SetBold();                   
                    sht.Range("K" + row + ":N" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("K" + row).Value = "";

                    sht.Row(row).Height = 30;
                    row = row + 2;

                    sht.Range("A" + row + ":C" + row).Value = "Prepared By";
                    sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 12;
                    //sht.Range("B6:D6").Style.Font.SetBold();
                    sht.Range("A" + row + ":C" + row).Merge();
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("K" + row + ":N" + row).Value = "Checked By";
                    sht.Range("K" + row + ":N" + row).Style.Font.FontName = "Calibri";
                    sht.Range("K" + row + ":N" + row).Style.Font.FontSize = 12;
                    //sht.Range("B6:D6").Style.Font.SetBold();
                    sht.Range("K" + row + ":N" + row).Merge();
                    sht.Range("K" + row + ":N" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Row(row).Height = 30;
                    row = row + 1;

                    //////*************
                    //sht.Columns(1, 20).AdjustToContents();
                    //********************
                    //***********BOrders
                    using (var a = sht.Range("A1" + ":N" + (row - 1)))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("WeaverFolioMaterialDetailReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }

        }

    }
    protected void BtnPrintSummaryFolio_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_FolioReceiveDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 30000;

        cmd.Parameters.AddWithValue("@IssueOrderID", DDFolioNo.SelectedValue);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/RptFolioReceiveDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptFolioReceiveDetail.xsd";
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
    protected void FolioMaterialDetailReportHafiziaNew()
    {
        DataSet ds = new DataSet();
        if (DDFolioNo.SelectedIndex > 0)
        {
            lblmsg.Text = "";
            try
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORTNEW", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
                //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);

                //SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("FolioRawMaterialReport");
                    //int row = 11;

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

                    sht.ColumnWidth = 5.20;

                    sht.Row(1).Height = 29;
                    sht.Row(2).Height = 29;
                    sht.Row(3).Height = 29;
                    sht.Row(4).Height = 29;
                    sht.Row(5).Height = 29;
                    sht.Row(6).Height = 15;
                    sht.Row(7).Height = 33;
                    sht.Row(8).Height = 18;
                    sht.Row(9).Height = 18;
                    sht.Row(10).Height = 18;
                    sht.Row(11).Height = 18;
                    sht.Row(12).Height = 18;

                    sht.Range("A1:AB1").Merge();
                    sht.Range("A1").Value = "RAW MATERIAL ISSUE (JOB WORK)";

                    sht.Range("A1:AB1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:AB1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:AB1").Style.Alignment.SetWrapText();
                    sht.Range("A1:AB1").Style.Font.FontName = "Calibri";
                    sht.Range("A1:AB1").Style.Font.FontSize = 18;
                    sht.Range("A1:AB1").Style.Font.Bold = true;
                    //*******Header

                    sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
                    sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                    sht.Range("A2:H2").Style.Font.FontSize = 14;
                    sht.Range("A2:H2").Style.Font.SetBold();
                    sht.Range("A2:H2").Merge();
                    sht.Range("A2:H2").Style.Alignment.SetWrapText();
                    sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I2").Value = "Weaver Name & Address";
                    sht.Range("I2:K2").Style.Font.FontName = "Calibri";
                    sht.Range("I2:K2").Style.Font.FontSize = 11;
                    sht.Range("I2:K2").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("I2:K2").Merge();
                    sht.Range("I2:K2").Style.Alignment.SetWrapText();
                    sht.Range("I2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I2:K2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L2").Value = ds.Tables[0].Rows[0]["EmpName"] + " " + ds.Tables[0].Rows[0]["Address"];
                    sht.Range("L2:R2").Style.Font.FontName = "Calibri";
                    sht.Range("L2:R2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("L2:R2").Merge();
                    sht.Range("L2:R2").Style.Alignment.SetWrapText();
                    sht.Range("L2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L2:R2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    sht.Range("S2").Value = "Pan/GST No";
                    sht.Range("S2:T2").Style.Font.FontName = "Calibri";
                    sht.Range("S2:T2").Style.Font.FontSize = 11;
                    sht.Range("S2:T2").Style.Font.SetBold();
                    //sht.Range("A2").Merge();
                    sht.Range("S2:T2").Merge();
                    sht.Range("S2:T2").Style.Alignment.SetWrapText();
                    sht.Range("S2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S2:T2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S2:T2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
                    sht.Range("U2:X2").Style.Font.FontName = "Calibri";
                    sht.Range("U2:X2").Style.Font.FontSize = 11;
                    //sht.Range("B2:D2").Style.Font.SetBold();
                    sht.Range("U2:X2").Merge();
                    sht.Range("U2:X2").Style.Alignment.SetWrapText();
                    sht.Range("U2:X2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U2:X2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y2").Value = "Sr.No";
                    sht.Range("Y2:Z2").Style.Font.FontName = "Calibri";
                    sht.Range("Y2:Z2").Style.Font.FontSize = 11;
                    sht.Range("Y2:Z2").Style.Font.SetBold();
                    sht.Range("Y2:Z2").Merge();
                    sht.Range("Y2:Z2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y2:Z2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y2:Z2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA2").Value = ds.Tables[0].Rows[0]["LocalOrder"];
                    sht.Range("AA2:AB2").Style.Font.FontName = "Calibri";
                    sht.Range("AA2:AB2").Style.Font.FontSize = 11;
                    //sht.Range("Q3:R3").Style.Font.SetBold();
                    sht.Range("AA2:AB2").Merge();
                    sht.Range("AA2:AB2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA2:AB2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"] + " " + ds.Tables[0].Rows[0]["COMPADDR3"];
                    sht.Range("A3:H4").Style.Font.FontName = "Calibri";
                    sht.Range("A3:H4").Style.Font.FontSize = 11;
                    sht.Range("A3:H4").Style.Font.SetBold();
                    sht.Range("A3:H4").Merge();
                    sht.Range("A3:H4").Style.Alignment.SetWrapText();
                    sht.Range("A3:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A3:H4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A5").Value = "GSTIN : " + ds.Tables[0].Rows[0]["CompanyGstNo"] + "                                     " + " PhoneNo : " + ds.Tables[0].Rows[0]["CompanyPhoneNo"];
                    sht.Range("A5:H5").Style.Font.FontName = "Calibri";
                    sht.Range("A5:H5").Style.Font.FontSize = 11;
                    sht.Range("A5:H5").Style.Font.SetBold();
                    sht.Range("A5:H5").Merge();
                    sht.Range("A5:H5").Style.Alignment.SetWrapText();
                    sht.Range("A5:H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A5:H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A6").Value = "DESIGN";
                    sht.Range("A6:B6").Style.Font.FontName = "Calibri";
                    sht.Range("A6:B6").Style.Font.FontSize = 11;
                    sht.Range("A6:B6").Style.Font.SetBold();
                    sht.Range("A6:B6").Merge();
                    sht.Range("A6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A6:B6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("C6").Value = ds.Tables[0].Rows[0]["FinishedItemDesingName"];
                    sht.Range("C6:D6").Style.Font.FontName = "Calibri";
                    sht.Range("C6:D6").Style.Font.FontSize = 11;
                    sht.Range("C6:D6").Merge();
                    sht.Range("C6:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C6:D6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E6").Value = "COLOR";
                    sht.Range("E6:F6").Style.Font.FontName = "Calibri";
                    sht.Range("E6:F6").Style.Font.FontSize = 11;
                    sht.Range("E6:F6").Style.Font.SetBold();
                    sht.Range("E6:F6").Merge();
                    sht.Range("E6:F6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E6:F6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E6:F6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("G6").Value = ds.Tables[0].Rows[0]["FinishedItemColorName"];
                    sht.Range("G6:H6").Style.Font.FontName = "Calibri";
                    sht.Range("G6:H6").Style.Font.FontSize = 11;
                    sht.Range("G6:H6").Merge();
                    sht.Range("G6:H6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G6:H6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I3").Value = "Deliver To";
                    sht.Range("I3:K3").Style.Font.FontName = "Calibri";
                    sht.Range("I3:K3").Style.Font.FontSize = 11;
                    sht.Range("I3:K3").Style.Font.SetBold();
                    sht.Range("I3:K3").Merge();
                    sht.Range("I3:K3").Style.Alignment.SetWrapText();
                    sht.Range("I3:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I3:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I3:K3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L3").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
                    sht.Range("L3:R3").Style.Font.FontName = "Calibri";
                    sht.Range("L3:R3").Style.Font.FontSize = 11;
                    sht.Range("L3:R3").Merge();
                    sht.Range("L3:R3").Style.Alignment.SetWrapText();
                    sht.Range("L3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L3:R3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S3").Value = "Pan No";
                    sht.Range("S3:T3").Style.Font.FontName = "Calibri";
                    sht.Range("S3:T3").Style.Font.FontSize = 11;
                    sht.Range("S3:T3").Style.Font.SetBold();
                    sht.Range("S3:T3").Merge();
                    sht.Range("S3:T3").Style.Alignment.SetWrapText();
                    sht.Range("S3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S3:T3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S3:T3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U3").Value = ds.Tables[0].Rows[0]["Emp2PanNo"];
                    sht.Range("U3:X3").Style.Font.FontName = "Calibri";
                    sht.Range("U3:X3").Style.Font.FontSize = 11;
                    sht.Range("U3:X3").Merge();
                    sht.Range("U3:X3").Style.Alignment.SetWrapText();
                    sht.Range("U3:X3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U3:X3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y3").Value = "Folio No.";
                    sht.Range("Y3:Z3").Style.Font.FontName = "Calibri";
                    sht.Range("Y3:Z3").Style.Font.FontSize = 11;
                    sht.Range("Y3:Z3").Style.Font.SetBold();
                    sht.Range("Y3:Z3").Merge();
                    sht.Range("Y3:Z3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y3:Z3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y3:Z3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA3").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
                    sht.Range("AA3:AB3").Style.Font.FontName = "Calibri";
                    sht.Range("AA3:AB3").Style.Font.FontSize = 11;
                    sht.Range("AA3:AB3").Merge();
                    sht.Range("AA3:AB3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA3:AB3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("I4").Value = "Address";
                    sht.Range("I4:K6").Style.Font.FontName = "Calibri";
                    sht.Range("I4:K6").Style.Font.FontSize = 11;
                    sht.Range("I4:K6").Style.Font.SetBold();
                    sht.Range("I4:K6").Merge();
                    sht.Range("I4:K6").Style.Alignment.SetWrapText();
                    sht.Range("I4:K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I4:K6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("I4:K6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("L4").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
                    sht.Range("L4:R6").Style.Font.FontName = "Calibri";
                    sht.Range("L4:R6").Style.Font.FontSize = 11;
                    sht.Range("L4:R6").Merge();
                    sht.Range("L4:R6").Style.Alignment.SetWrapText();
                    sht.Range("L4:R6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L4:R6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S4").Value = "Aadhar No";
                    sht.Range("S4:T4").Style.Font.FontName = "Calibri";
                    sht.Range("S4:T4").Style.Font.FontSize = 11;
                    sht.Range("S4:T4").Style.Font.SetBold();
                    sht.Range("S4:T4").Merge();
                    sht.Range("S4:T4").Style.Alignment.SetWrapText();
                    sht.Range("S4:T4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S4:T4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S4:T4").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U4:X4").Style.NumberFormat.Format = "@";
                    sht.Range("U4").Value = ds.Tables[0].Rows[0]["Emp2AadharNo"];
                    sht.Range("U4:X4").Style.Font.FontName = "Calibri";
                    sht.Range("U4:X4").Style.Font.FontSize = 11;
                    sht.Range("U4:X4").Style.NumberFormat.NumberFormatId = 1;
                    sht.Range("U4:X4").Merge();
                    sht.Range("U4:X4").Style.Alignment.SetWrapText();
                    sht.Range("U4:X4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U4:X4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y4").Value = "Order Date";
                    sht.Range("Y4:Z4").Style.Font.FontName = "Calibri";
                    sht.Range("Y4:Z4").Style.Font.FontSize = 11;
                    sht.Range("Y4:Z4").Style.Font.SetBold();
                    sht.Range("Y4:Z4").Merge();
                    sht.Range("Y4:Z4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y4:Z4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y4:Z4").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA4").Value = ds.Tables[0].Rows[0]["ASSIGNDATE"];
                    sht.Range("AA4:AB4").Style.Font.FontName = "Calibri";
                    sht.Range("AA4:AB4").Style.Font.FontSize = 11;
                    sht.Range("AA4:AB4").Merge();
                    sht.Range("AA4:AB4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA4:AB4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("S5").Value = "Mobile No";
                    sht.Range("S5:T6").Style.Font.FontName = "Calibri";
                    sht.Range("S5:T6").Style.Font.FontSize = 11;
                    sht.Range("S5:T6").Style.Font.SetBold();
                    sht.Range("S5:T6").Merge();
                    sht.Range("S5:T6").Style.Alignment.SetWrapText();
                    sht.Range("S5:T6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("S5:T6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("S5:T6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("U5").Value = ds.Tables[0].Rows[0]["Emp2MobileNo"];
                    sht.Range("U5:X6").Style.Font.FontName = "Calibri";
                    sht.Range("U5:X6").Style.Font.FontSize = 11;
                    sht.Range("U5:X6").Merge();
                    sht.Range("U5:X6").Style.Alignment.SetWrapText();
                    sht.Range("U5:X6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("U5:X6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("Y5").Value = "Delivery Date";
                    sht.Range("Y5:Z6").Style.Font.FontName = "Calibri";
                    sht.Range("Y5:Z6").Style.Font.FontSize = 11;
                    sht.Range("Y5:Z6").Style.Font.SetBold();
                    sht.Range("Y5:Z6").Merge();
                    sht.Range("Y5:Z6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("Y5:Z6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("Y5:Z6").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("AA5").Value = ds.Tables[0].Rows[0]["REQBYDATE"];
                    sht.Range("AA5:AB6").Style.Font.FontName = "Calibri";
                    sht.Range("AA5:AB6").Style.Font.FontSize = 11;
                    sht.Range("AA5:AB6").Merge();
                    sht.Range("AA5:AB6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("AA5:AB6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("A7").Value = "RAW MATERIAL & COUNT";
                    sht.Range("A7:D7").Style.Font.FontName = "Calibri";
                    sht.Range("A7:D7").Style.Font.FontSize = 11;
                    sht.Range("A7:D7").Style.Font.SetBold();
                    sht.Range("A7:D7").Merge();
                    sht.Range("A7:D7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A7:D7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A7:D7").Style.Fill.BackgroundColor = XLColor.LightGray;

                    int Dynamiccol6 = 4;
                    int Dynamiccolstart6 = Dynamiccol6 + 1;
                    int Dynamiccolend6 = 0;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row2 = 7;
                        int noofrows2 = 0;
                        int noofrows22 = 0;
                        int i2 = 0;
                        int Dynamiccol2 = 4;
                        int Dynamiccolstart2 = Dynamiccol2 + 1;
                        int Dynamiccolend2;
                        int Totalcol2;
                        string ItemName = "";

                        DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        noofrows2 = dtdistinct2.Rows.Count;

                        if (noofrows2 < 11)
                        {
                            noofrows22 = 11;
                        }
                        else
                        {
                            noofrows22 = noofrows2;
                        }


                        for (i2 = 0; i2 < noofrows22; i2++)
                        {
                            ItemName = "";
                            if (i2 < noofrows2)
                            {
                                ItemName = ds.Tables[1].Rows[i2]["Item_Name"] + " " + ds.Tables[1].Rows[i2]["QualityName"];
                            }
                            Dynamiccol2 = Dynamiccol2 + 1;
                            sht.Range(sht.Cell(row2, Dynamiccol2), sht.Cell(row2, Dynamiccol2 + 1)).Merge();

                            sht.Cell(row2, Dynamiccol2).Value = ItemName;
                            sht.Cell(row2, Dynamiccol2).Style.Font.Bold = true;
                            sht.Cell(row2, Dynamiccol2).Style.Font.FontName = "Calibri";
                            sht.Cell(row2, Dynamiccol2).Style.Font.FontSize = 9;
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            sht.Cell(row2, Dynamiccol2).Style.Alignment.WrapText = true;
                            //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
                            Dynamiccol2 = Dynamiccol2 + 1;

                            //sht.Range("AA5:AB6").Merge();

                        }
                        Dynamiccolend2 = Dynamiccol2;

                        Totalcol2 = Dynamiccolend2 + 1;

                        sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, Totalcol2 + 1)).Merge();

                        sht.Cell(row2, Totalcol2).Value = "TOTAL";
                        sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
                        sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
                        sht.Cell(row2, Totalcol2).Style.Font.FontSize = 9;
                        sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;

                        Dynamiccolend6 = Totalcol2;
                    }

                    sht.Range("A8").Value = "COLOUR";
                    sht.Range("A8:D8").Style.Font.FontName = "Calibri";
                    sht.Range("A8:D8").Style.Font.FontSize = 11;
                    sht.Range("A8:D8").Style.Font.SetBold();
                    sht.Range("A8:D8").Merge();
                    sht.Range("A8:D8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A8:D8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A8:D8").Style.Fill.BackgroundColor = XLColor.LightGray;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row3 = 8;
                        int noofrows3 = 0;
                        int noofrows33 = 0;
                        int i3 = 0;
                        int Dynamiccol3 = 4;
                        int Dynamiccolstart3 = Dynamiccol3 + 1;
                        int Dynamiccolend3;
                        int Totalcol3;
                        string ShadeColorName = "";

                        DataTable dtdistinct3 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        noofrows3 = dtdistinct3.Rows.Count;

                        if (noofrows3 < 12)
                        {
                            noofrows33 = 12;
                        }
                        else
                        {
                            noofrows33 = noofrows3;
                        }

                        for (i3 = 0; i3 < noofrows33; i3++)
                        {
                            ShadeColorName = "";
                            if (i3 < noofrows3)
                            {
                                ShadeColorName = ds.Tables[1].Rows[i3]["ShadeColorName"].ToString();
                            }


                            Dynamiccol3 = Dynamiccol3 + 1;
                            sht.Range(sht.Cell(row3, Dynamiccol3), sht.Cell(row3, Dynamiccol3 + 1)).Merge();

                            sht.Cell(row3, Dynamiccol3).Value = ShadeColorName;
                            sht.Cell(row3, Dynamiccol3).Style.Font.Bold = true;
                            sht.Cell(row3, Dynamiccol3).Style.Font.FontName = "Calibri";
                            sht.Cell(row3, Dynamiccol3).Style.Font.FontSize = 9;
                            sht.Cell(row3, Dynamiccol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row3, Dynamiccol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            sht.Cell(row3, Dynamiccol3).Style.Alignment.WrapText = true;
                            Dynamiccol3 = Dynamiccol3 + 1;
                        }

                        Dynamiccolend3 = Dynamiccol3;

                        Totalcol3 = Dynamiccolend3 + 1;

                    }

                    sht.Range("A9").Value = "EXTRA YARN ISSUE";
                    sht.Range("A9:D9").Style.Font.FontName = "Calibri";
                    sht.Range("A9:D9").Style.Font.FontSize = 11;
                    sht.Range("A9:D9").Style.Font.SetBold();
                    sht.Range("A9:D9").Merge();
                    sht.Range("A9:D9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A9:D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A9:D9").Style.Fill.BackgroundColor = XLColor.LightGray;


                    sht.Range("A10").Value = "LOT NO.";
                    sht.Range("A10:D10").Style.Font.FontName = "Calibri";
                    sht.Range("A10:D10").Style.Font.FontSize = 11;
                    sht.Range("A10:D10").Style.Font.SetBold();
                    sht.Range("A10:D10").Merge();
                    sht.Range("A10:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A10:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A10:D10").Style.Fill.BackgroundColor = XLColor.LightGray;

                    int i = 0;
                    int DynamiccolNew = 4;
                    for (i = 0; i < 12; i++)
                    {
                        DynamiccolNew = DynamiccolNew + 1;
                        sht.Range(sht.Cell(9, DynamiccolNew), sht.Cell(9, DynamiccolNew + 1)).Merge();
                        sht.Range(sht.Cell(10, DynamiccolNew), sht.Cell(10, DynamiccolNew + 1)).Merge();
                        DynamiccolNew = DynamiccolNew + 1;
                    }

                    sht.Range("A11").Value = "(1) WEIGHT YARN 90%";
                    sht.Range("A11:D11").Style.Font.FontName = "Calibri";
                    sht.Range("A11:D11").Style.Font.FontSize = 11;
                    sht.Range("A11:D11").Style.Font.SetBold();
                    sht.Range("A11:D11").Merge();
                    sht.Range("A11:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A11:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A11:D11").Style.Fill.BackgroundColor = XLColor.LightGray;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row4 = 11;
                        int noofrows4 = 0;
                        int noofrows44 = 0;
                        int i4 = 0;
                        int Dynamiccol4 = 4;
                        int Dynamiccolstart4 = Dynamiccol4 + 1;
                        int Dynamiccolend4;
                        int Totalcol4;

                        //DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        //DataView dv1 = new DataView(dtdistinctDateType);
                        //dv1.Sort = "Item_Name";
                        //DataTable dtdistinctDateType1 = dv1.ToTable();

                        DataTable dtdistinct4 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        noofrows4 = dtdistinct4.Rows.Count;

                        if (noofrows4 < 11)
                        {
                            noofrows44 = 11;
                        }
                        else
                        {
                            noofrows44 = noofrows4;
                        }

                        decimal TotalSumOneRow = 0;
                        for (i4 = 0; i4 < noofrows44; i4++)
                        {
                            decimal IssRecConQty = 0;
                            if (i4 < noofrows4)
                            {
                                IssRecConQty = Convert.ToDecimal(ds.Tables[1].Rows[i4]["RawMaterial90PerQty"]);
                            }

                            Dynamiccol4 = Dynamiccol4 + 1;
                            sht.Range(sht.Cell(row4, Dynamiccol4), sht.Cell(row4, Dynamiccol4 + 1)).Merge();

                            sht.Cell(row4, Dynamiccol4).Value = IssRecConQty == 0 ? "" : IssRecConQty.ToString();
                            sht.Cell(row4, Dynamiccol4).Style.Font.Bold = true;
                            sht.Cell(row4, Dynamiccol4).Style.Font.FontName = "Calibri";
                            sht.Cell(row4, Dynamiccol4).Style.Font.FontSize = 10;
                            sht.Cell(row4, Dynamiccol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row4, Dynamiccol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            sht.Cell(row4, Dynamiccol4).Style.Alignment.WrapText = true;
                            //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();

                            Dynamiccol4 = Dynamiccol4 + 1;
                            TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                        }
                        Dynamiccolend4 = Dynamiccol4;

                        Totalcol4 = Dynamiccolend4 + 1;
                        sht.Range(sht.Cell(row4, Totalcol4), sht.Cell(row4, Totalcol4 + 1)).Merge();

                        sht.Cell(row4, Totalcol4).Value = TotalSumOneRow;
                        sht.Cell(row4, Totalcol4).Style.Font.Bold = true;
                        sht.Cell(row4, Totalcol4).Style.Font.FontName = "Calibri";
                        sht.Cell(row4, Totalcol4).Style.Font.FontSize = 10;
                        sht.Cell(row4, Totalcol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row4, Totalcol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row4, Totalcol4).Style.Alignment.WrapText = true;
                    }

                    sht.Range("A12").Value = "(2) WEIGHT YARN 10%";
                    sht.Range("A12:D12").Style.Font.FontName = "Calibri";
                    sht.Range("A12:D12").Style.Font.FontSize = 11;
                    sht.Range("A12:D12").Style.Font.SetBold();
                    sht.Range("A12:D12").Merge();
                    sht.Range("A12:D12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A12:D12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A12:D12").Style.Fill.BackgroundColor = XLColor.LightGray;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int row5 = 12;
                        int noofrows5 = 0;
                        int noofrows55 = 0;
                        int i5 = 0;
                        int Dynamiccol5 = 4;
                        int Dynamiccolstart5 = Dynamiccol5 + 1;
                        int Dynamiccolend5;
                        int Totalcol5;

                        //DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        //DataView dv1 = new DataView(dtdistinctDateType);
                        //dv1.Sort = "Item_Name";
                        //DataTable dtdistinctDateType1 = dv1.ToTable();

                        DataTable dtdistinct5 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                        noofrows5 = dtdistinct5.Rows.Count;

                        if (noofrows5 < 11)
                        {
                            noofrows55 = 11;
                        }
                        else
                        {
                            noofrows55 = noofrows5;
                        }

                        decimal TotalSumOneRow = 0;
                        for (i5 = 0; i5 < noofrows55; i5++)
                        {
                            decimal IssRecConQty = 0;

                            if (i5 < noofrows5)
                            {
                                IssRecConQty = Convert.ToDecimal(ds.Tables[1].Rows[i5]["RawMaterial10PerQty"]);
                            }
                            Dynamiccol5 = Dynamiccol5 + 1;
                            sht.Range(sht.Cell(row5, Dynamiccol5), sht.Cell(row5, Dynamiccol5 + 1)).Merge();

                            sht.Cell(row5, Dynamiccol5).Value = IssRecConQty == 0 ? "" : IssRecConQty.ToString();
                            sht.Cell(row5, Dynamiccol5).Style.Font.Bold = true;
                            sht.Cell(row5, Dynamiccol5).Style.Font.FontName = "Calibri";
                            sht.Cell(row5, Dynamiccol5).Style.Font.FontSize = 10;
                            sht.Cell(row5, Dynamiccol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(row5, Dynamiccol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            sht.Cell(row5, Dynamiccol5).Style.Alignment.WrapText = true;
                            //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();


                            TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                            Dynamiccol5 = Dynamiccol5 + 1;

                        }
                        Dynamiccolend5 = Dynamiccol5;

                        Totalcol5 = Dynamiccolend5 + 1;
                        sht.Range(sht.Cell(row5, Totalcol5), sht.Cell(row5, Totalcol5 + 1)).Merge();

                        sht.Cell(row5, Totalcol5).Value = TotalSumOneRow;
                        sht.Cell(row5, Totalcol5).Style.Font.Bold = true;
                        sht.Cell(row5, Totalcol5).Style.Font.FontName = "Calibri";
                        sht.Cell(row5, Totalcol5).Style.Font.FontSize = 10;
                        sht.Cell(row5, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row5, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row5, Totalcol5).Style.Alignment.WrapText = true;

                    }

                    sht.Range("A13").Value = "DATE";
                    sht.Range("A13:B13").Style.Font.FontName = "Calibri";
                    sht.Range("A13:B13").Style.Font.FontSize = 11;
                    sht.Range("A13:B13").Style.Font.SetBold();
                    sht.Range("A13:B13").Merge();
                    sht.Range("A13:B13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A13:B13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A13:B13").Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("C13").Value = "SLIP NO";
                    sht.Range("C13:D13").Style.Font.FontName = "Calibri";
                    sht.Range("C13:D13").Style.Font.FontSize = 11;
                    sht.Range("C13:D13").Style.Font.SetBold();
                    sht.Range("C13:D13").Merge();
                    sht.Range("C13:D13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C13:D13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C13:D13").Style.Fill.BackgroundColor = XLColor.LightGray;


                    int row = 13;
                    for (int m = Dynamiccolstart6; m <= Dynamiccolend6; m = m + 2)
                    //for (int m = 5; m < 17; m++)
                    {
                        var NintyPerQty = sht.Cell(11, m).Value;
                        var TenPerQty = sht.Cell(12, m).Value;
                        decimal IssueQty = 0;
                        IssueQty = Convert.ToDecimal(NintyPerQty == "" ? 0 : NintyPerQty) + Convert.ToDecimal(TenPerQty == "" ? 0 : TenPerQty);

                        sht.Range(sht.Cell(row, m), sht.Cell(row, m + 1)).Merge();

                        sht.Cell(row, m).SetValue(IssueQty == 0 ? "" : IssueQty.ToString());
                        sht.Cell(row, m).Style.Font.Bold = true;
                        sht.Cell(row, m).Style.Font.FontName = "Calibri";
                        sht.Cell(row, m).Style.Font.FontSize = 11;
                        sht.Cell(row, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    }

                    row = row + 1;
                    sht.Row(row).Height = 3;

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        int noofrows7 = 0;                       
                        int i7 = 0;
                        int Dynamiccol7 = 4;
                        int Dynamiccolstart7 = Dynamiccol7 + 1;
                        int i8 = 0;
                        int noofrows8 = 0;

                        DataTable dtdistinct7 = ds.Tables[2].DefaultView.ToTable(true, "Date", "ChallanNo");
                        noofrows7 = dtdistinct7.Rows.Count;

                        row = row + 1;

                        for (i7 = 0; i7 < noofrows7; i7++)
                        {
                            sht.Row(row).Height = 26.25;
                            sht.Range(sht.Cell(row, 1), sht.Cell(row, 2)).Merge();

                            sht.Range("A" + row).Value = dtdistinct7.Rows[i7]["Date"];
                            sht.Range("A" + row).Style.Font.FontName = "Calibri";
                            sht.Range("A" + row).Style.Font.FontSize = 11;
                            sht.Range("A" + row).Style.Font.SetBold();
                            //sht.Range("A" + row).Merge();
                            sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            sht.Range("C" + row).Style.NumberFormat.Format = "@";
                            sht.Range(sht.Cell(row, 3), sht.Cell(row, 4)).Merge();
                            sht.Range("C" + row).Value = dtdistinct7.Rows[i7]["ChallanNo"];
                            sht.Range("C" + row).Style.Font.FontName = "Calibri";
                            sht.Range("C" + row).Style.Font.FontSize = 11;
                            sht.Range("C" + row).Style.Font.SetBold();
                            //sht.Range("C" + row).Merge();
                            sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            decimal TotalSumOneRow = 0;
                            for (int m = Dynamiccolstart6; m <= Dynamiccolend6; m = m + 2)
                            {
                                var itemname = sht.Cell(7, m).Value;
                                var ShadeColorName = sht.Cell(8, m).Value;

                                decimal WeightQty = 0;
                                decimal Penality = 0;
                                decimal IssueQty = 0;
                                var ItemNameMatch = "";

                                DataTable dtdistinct8 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
                                noofrows8 = dtdistinct8.Rows.Count;

                                for (i8 = 0; i8 < noofrows8; i8++)
                                {
                                    ItemNameMatch = dtdistinct8.Rows[i8]["Item_Name"] + " " + dtdistinct8.Rows[i8]["QualityName"];
                                    int result = string.Compare(itemname.ToString(), ItemNameMatch.ToString());

                                    if (result == 0)
                                    {
                                        DataRow[] foundRows;
                                        foundRows = ds.Tables[2].Select("Item_Name='" + dtdistinct8.Rows[i8]["Item_Name"] + "' and QualityName='" + dtdistinct8.Rows[i8]["QualityName"] + "' and DesignName='" + dtdistinct8.Rows[i8]["DesignName"] + "' and ShadeColorName='" + ShadeColorName + "' and Date='" + dtdistinct7.Rows[i7]["Date"] + "' and ChallanNo='" + dtdistinct7.Rows[i7]["ChallanNo"] + "' ");
                                        if (foundRows.Length > 0)
                                        {
                                            foreach (DataRow row4 in foundRows)
                                            {
                                                IssueQty = Convert.ToDecimal(row4["IssueQty"].ToString());
                                                break;
                                            }
                                        }
                                    }


                                }
                                sht.Range(sht.Cell(row, m), sht.Cell(row, m + 1)).Merge();
                                sht.Cell(row, m).SetValue(IssueQty == 0 ? "" : IssueQty.ToString());
                                sht.Cell(row, m).Style.Font.Bold = true;
                                sht.Cell(row, m).Style.Font.FontName = "Calibri";
                                sht.Cell(row, m).Style.Font.FontSize = 11;
                                sht.Cell(row, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                sht.Cell(row, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                TotalSumOneRow = TotalSumOneRow + IssueQty;

                                sht.Cell(row, Dynamiccolend6).SetValue(TotalSumOneRow);
                                sht.Range(sht.Cell(row, Dynamiccolend6), sht.Cell(row, Dynamiccolend6 + 1)).Merge();
                            }



                            row = row + 1;
                        }
                    }

                    for (int r = row; r <= 26; r++)
                    {
                        sht.Row(r).Height = 26.25;

                        for (int col = 1; col <= 28; col = col + 2)
                        {
                            sht.Range(sht.Cell(r, col), sht.Cell(r, col + 1)).Merge();
                        }

                        row = row + 1;
                    }

                    sht.Range(sht.Cell((row - 1), 3), sht.Cell((row - 1), 4)).Merge();

                    sht.Cell((row - 1), 3).SetValue("Total");
                    sht.Cell((row - 1), 3).Style.Font.Bold = true;
                    sht.Cell((row - 1), 3).Style.Font.FontName = "Calibri";
                    sht.Cell((row - 1), 3).Style.Font.FontSize = 11;
                    sht.Cell((row - 1), 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell((row - 1), 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    int sumrowstart = 15;
                    decimal TotalSumOneRowWise = 0;
                    for (int t = Dynamiccolstart6; t <= Dynamiccolend6; t = t + 2)
                    //for (int m = 5; m < 17; m++)
                    {
                        TotalSumOneRowWise = 0;
                        for (int s = sumrowstart; s <= 26; s++)
                        {
                            var OneColumnValue = sht.Cell(s, t).Value;
                            TotalSumOneRowWise = TotalSumOneRowWise + Convert.ToDecimal(OneColumnValue == "" ? 0 : OneColumnValue);
                        }
                        sumrowstart = 15;

                        sht.Range(sht.Cell((row - 1), t), sht.Cell((row - 1), t + 1)).Merge();

                        sht.Cell((row - 1), t).SetValue(TotalSumOneRowWise);
                        sht.Cell((row - 1), t).Style.Font.Bold = true;
                        sht.Cell((row - 1), t).Style.Font.FontName = "Calibri";
                        sht.Cell((row - 1), t).Style.Font.FontSize = 11;
                        sht.Cell((row - 1), t).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell((row - 1), t).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    }


                    ////////*************
                    ////sht.Columns(1, 20).AdjustToContents();
                    ////********************
                    ////***********BOrders
                    ///using (var a = sht.Range("A1" + ":AB" + (row - 1)))
                    //using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, (Dynamiccolend6 + 1))))
                    //{
                    using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, (Dynamiccolend6 + 1))))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("WeaverFolioMaterialDetailReportNew_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }

        }

    }

    //protected void FolioMaterialDetailReportHafiziaNew()
    //{
    //    DataSet ds = new DataSet();
    //    if (DDFolioNo.SelectedIndex > 0)
    //    {
    //        lblmsg.Text = "";
    //        try
    //        {
    //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //            if (con.State == ConnectionState.Closed)
    //            {
    //                con.Open();
    //            }
    //            SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORTNEW", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.CommandTimeout = 1000;

    //            cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
    //            //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
    //            SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //            cmd.ExecuteNonQuery();
    //            ad.Fill(ds);

    //            //SqlParameter[] param = new SqlParameter[1];
    //            //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

    //            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", param);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
    //                {
    //                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
    //                }
    //                string Path = "";
    //                var xapp = new XLWorkbook();
    //                var sht = xapp.Worksheets.Add("FolioRawMaterialReport");
    //                //int row = 11;

    //                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
    //                sht.PageSetup.AdjustTo(83);
    //                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


    //                sht.PageSetup.Margins.Top = 1.21;
    //                sht.PageSetup.Margins.Left = 0.47;
    //                sht.PageSetup.Margins.Right = 0.36;
    //                sht.PageSetup.Margins.Bottom = 0.19;
    //                sht.PageSetup.Margins.Header = 1.20;
    //                sht.PageSetup.Margins.Footer = 0.3;
    //                sht.PageSetup.SetScaleHFWithDocument();

    //                sht.ColumnWidth = 5.20;

    //                sht.Row(1).Height = 29;
    //                sht.Row(2).Height = 29;
    //                sht.Row(3).Height = 29;
    //                sht.Row(4).Height = 29;
    //                sht.Row(5).Height = 29;
    //                sht.Row(6).Height = 15;
    //                sht.Row(7).Height = 33;
    //                sht.Row(8).Height = 18;
    //                sht.Row(9).Height = 18;
    //                sht.Row(10).Height = 18;
    //                sht.Row(11).Height = 18;
    //                sht.Row(12).Height = 18;

    //                sht.Range("A1:AB1").Merge();
    //                sht.Range("A1").Value = "RAW MATERIAL ISSUE (JOB WORK)";

    //                sht.Range("A1:AB1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A1:AB1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //                sht.Range("A1:AB1").Style.Alignment.SetWrapText();
    //                sht.Range("A1:AB1").Style.Font.FontName = "Calibri";
    //                sht.Range("A1:AB1").Style.Font.FontSize = 18;
    //                sht.Range("A1:AB1").Style.Font.Bold = true;
    //                //*******Header

    //                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
    //                sht.Range("A2:H2").Style.Font.FontName = "Calibri";
    //                sht.Range("A2:H2").Style.Font.FontSize = 14;
    //                sht.Range("A2:H2").Style.Font.SetBold();
    //                sht.Range("A2:H2").Merge();
    //                sht.Range("A2:H2").Style.Alignment.SetWrapText();
    //                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("I2").Value = "Weaver Name & Address";
    //                sht.Range("I2:K2").Style.Font.FontName = "Calibri";
    //                sht.Range("I2:K2").Style.Font.FontSize = 11;
    //                sht.Range("I2:K2").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("I2:K2").Merge();
    //                sht.Range("I2:K2").Style.Alignment.SetWrapText();
    //                sht.Range("I2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("I2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("I2:K2").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("L2").Value = ds.Tables[0].Rows[0]["EmpName"] + " " + ds.Tables[0].Rows[0]["Address"];
    //                sht.Range("L2:R2").Style.Font.FontName = "Calibri";
    //                sht.Range("L2:R2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("L2:R2").Merge();
    //                sht.Range("L2:R2").Style.Alignment.SetWrapText();
    //                sht.Range("L2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("L2:R2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

    //                sht.Range("S2").Value = "Pan/GST No";
    //                sht.Range("S2:T2").Style.Font.FontName = "Calibri";
    //                sht.Range("S2:T2").Style.Font.FontSize = 11;
    //                sht.Range("S2:T2").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("S2:T2").Merge();
    //                sht.Range("S2:T2").Style.Alignment.SetWrapText();
    //                sht.Range("S2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("S2:T2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("S2:T2").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("U2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
    //                sht.Range("U2:X2").Style.Font.FontName = "Calibri";
    //                sht.Range("U2:X2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("U2:X2").Merge();
    //                sht.Range("U2:X2").Style.Alignment.SetWrapText();
    //                sht.Range("U2:X2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("U2:X2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("Y2").Value = "Sr.No";
    //                sht.Range("Y2:Z2").Style.Font.FontName = "Calibri";
    //                sht.Range("Y2:Z2").Style.Font.FontSize = 11;
    //                sht.Range("Y2:Z2").Style.Font.SetBold();
    //                sht.Range("Y2:Z2").Merge();
    //                sht.Range("Y2:Z2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("Y2:Z2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("Y2:Z2").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("AA2").Value = ds.Tables[0].Rows[0]["LocalOrder"];
    //                sht.Range("AA2:AB2").Style.Font.FontName = "Calibri";
    //                sht.Range("AA2:AB2").Style.Font.FontSize = 11;
    //                //sht.Range("Q3:R3").Style.Font.SetBold();
    //                sht.Range("AA2:AB2").Merge();
    //                sht.Range("AA2:AB2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("AA2:AB2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"] + " " + ds.Tables[0].Rows[0]["COMPADDR3"];
    //                sht.Range("A3:H4").Style.Font.FontName = "Calibri";
    //                sht.Range("A3:H4").Style.Font.FontSize = 11;
    //                sht.Range("A3:H4").Style.Font.SetBold();
    //                sht.Range("A3:H4").Merge();
    //                sht.Range("A3:H4").Style.Alignment.SetWrapText();
    //                sht.Range("A3:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A3:H4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("A5").Value = "GSTIN : " + ds.Tables[0].Rows[0]["CompanyGstNo"] + "                                     " + " PhoneNo : " + ds.Tables[0].Rows[0]["CompanyPhoneNo"];
    //                sht.Range("A5:H5").Style.Font.FontName = "Calibri";
    //                sht.Range("A5:H5").Style.Font.FontSize = 11;
    //                sht.Range("A5:H5").Style.Font.SetBold();
    //                sht.Range("A5:H5").Merge();
    //                sht.Range("A5:H5").Style.Alignment.SetWrapText();
    //                sht.Range("A5:H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A5:H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("A6").Value = "DESIGN";
    //                sht.Range("A6:B6").Style.Font.FontName = "Calibri";
    //                sht.Range("A6:B6").Style.Font.FontSize = 11;
    //                sht.Range("A6:B6").Style.Font.SetBold();
    //                sht.Range("A6:B6").Merge();
    //                sht.Range("A6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A6:B6").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("C6").Value = ds.Tables[0].Rows[0]["FinishedItemDesingName"];
    //                sht.Range("C6:D6").Style.Font.FontName = "Calibri";
    //                sht.Range("C6:D6").Style.Font.FontSize = 11;
    //                sht.Range("C6:D6").Merge();
    //                sht.Range("C6:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C6:D6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E6").Value = "COLOR";
    //                sht.Range("E6:F6").Style.Font.FontName = "Calibri";
    //                sht.Range("E6:F6").Style.Font.FontSize = 11;
    //                sht.Range("E6:F6").Style.Font.SetBold();
    //                sht.Range("E6:F6").Merge();
    //                sht.Range("E6:F6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E6:F6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("E6:F6").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("G6").Value = ds.Tables[0].Rows[0]["FinishedItemColorName"];
    //                sht.Range("G6:H6").Style.Font.FontName = "Calibri";
    //                sht.Range("G6:H6").Style.Font.FontSize = 11;
    //                sht.Range("G6:H6").Merge();
    //                sht.Range("G6:H6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("G6:H6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("I3").Value = "Deliver To";
    //                sht.Range("I3:K3").Style.Font.FontName = "Calibri";
    //                sht.Range("I3:K3").Style.Font.FontSize = 11;
    //                sht.Range("I3:K3").Style.Font.SetBold();
    //                sht.Range("I3:K3").Merge();
    //                sht.Range("I3:K3").Style.Alignment.SetWrapText();
    //                sht.Range("I3:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("I3:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("I3:K3").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("L3").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
    //                sht.Range("L3:R3").Style.Font.FontName = "Calibri";
    //                sht.Range("L3:R3").Style.Font.FontSize = 11;
    //                sht.Range("L3:R3").Merge();
    //                sht.Range("L3:R3").Style.Alignment.SetWrapText();
    //                sht.Range("L3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("L3:R3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("S3").Value = "Pan No";
    //                sht.Range("S3:T3").Style.Font.FontName = "Calibri";
    //                sht.Range("S3:T3").Style.Font.FontSize = 11;
    //                sht.Range("S3:T3").Style.Font.SetBold();
    //                sht.Range("S3:T3").Merge();
    //                sht.Range("S3:T3").Style.Alignment.SetWrapText();
    //                sht.Range("S3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("S3:T3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("S3:T3").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("U3").Value = ds.Tables[0].Rows[0]["Emp2PanNo"];
    //                sht.Range("U3:X3").Style.Font.FontName = "Calibri";
    //                sht.Range("U3:X3").Style.Font.FontSize = 11;
    //                sht.Range("U3:X3").Merge();
    //                sht.Range("U3:X3").Style.Alignment.SetWrapText();
    //                sht.Range("U3:X3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("U3:X3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("Y3").Value = "Folio No.";
    //                sht.Range("Y3:Z3").Style.Font.FontName = "Calibri";
    //                sht.Range("Y3:Z3").Style.Font.FontSize = 11;
    //                sht.Range("Y3:Z3").Style.Font.SetBold();
    //                sht.Range("Y3:Z3").Merge();
    //                sht.Range("Y3:Z3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("Y3:Z3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("Y3:Z3").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("AA3").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
    //                sht.Range("AA3:AB3").Style.Font.FontName = "Calibri";
    //                sht.Range("AA3:AB3").Style.Font.FontSize = 11;
    //                sht.Range("AA3:AB3").Merge();
    //                sht.Range("AA3:AB3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("AA3:AB3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("I4").Value = "Address";
    //                sht.Range("I4:K6").Style.Font.FontName = "Calibri";
    //                sht.Range("I4:K6").Style.Font.FontSize = 11;
    //                sht.Range("I4:K6").Style.Font.SetBold();
    //                sht.Range("I4:K6").Merge();
    //                sht.Range("I4:K6").Style.Alignment.SetWrapText();
    //                sht.Range("I4:K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("I4:K6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("I4:K6").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("L4").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
    //                sht.Range("L4:R6").Style.Font.FontName = "Calibri";
    //                sht.Range("L4:R6").Style.Font.FontSize = 11;
    //                sht.Range("L4:R6").Merge();
    //                sht.Range("L4:R6").Style.Alignment.SetWrapText();
    //                sht.Range("L4:R6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("L4:R6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("S4").Value = "Aadhar No";
    //                sht.Range("S4:T4").Style.Font.FontName = "Calibri";
    //                sht.Range("S4:T4").Style.Font.FontSize = 11;
    //                sht.Range("S4:T4").Style.Font.SetBold();
    //                sht.Range("S4:T4").Merge();
    //                sht.Range("S4:T4").Style.Alignment.SetWrapText();
    //                sht.Range("S4:T4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("S4:T4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("S4:T4").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("U4:X4").Style.NumberFormat.Format = "@";
    //                sht.Range("U4").Value = ds.Tables[0].Rows[0]["Emp2AadharNo"];
    //                sht.Range("U4:X4").Style.Font.FontName = "Calibri";
    //                sht.Range("U4:X4").Style.Font.FontSize = 11;
    //                sht.Range("U4:X4").Style.NumberFormat.NumberFormatId = 1;
    //                sht.Range("U4:X4").Merge();
    //                sht.Range("U4:X4").Style.Alignment.SetWrapText();
    //                sht.Range("U4:X4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("U4:X4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("Y4").Value = "Order Date";
    //                sht.Range("Y4:Z4").Style.Font.FontName = "Calibri";
    //                sht.Range("Y4:Z4").Style.Font.FontSize = 11;
    //                sht.Range("Y4:Z4").Style.Font.SetBold();
    //                sht.Range("Y4:Z4").Merge();
    //                sht.Range("Y4:Z4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("Y4:Z4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("Y4:Z4").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("AA4").Value = ds.Tables[0].Rows[0]["ASSIGNDATE"];
    //                sht.Range("AA4:AB4").Style.Font.FontName = "Calibri";
    //                sht.Range("AA4:AB4").Style.Font.FontSize = 11;
    //                sht.Range("AA4:AB4").Merge();
    //                sht.Range("AA4:AB4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("AA4:AB4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("S5").Value = "Mobile No";
    //                sht.Range("S5:T6").Style.Font.FontName = "Calibri";
    //                sht.Range("S5:T6").Style.Font.FontSize = 11;
    //                sht.Range("S5:T6").Style.Font.SetBold();
    //                sht.Range("S5:T6").Merge();
    //                sht.Range("S5:T6").Style.Alignment.SetWrapText();
    //                sht.Range("S5:T6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("S5:T6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("S5:T6").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("U5").Value = ds.Tables[0].Rows[0]["Emp2MobileNo"];
    //                sht.Range("U5:X6").Style.Font.FontName = "Calibri";
    //                sht.Range("U5:X6").Style.Font.FontSize = 11;
    //                sht.Range("U5:X6").Merge();
    //                sht.Range("U5:X6").Style.Alignment.SetWrapText();
    //                sht.Range("U5:X6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("U5:X6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("Y5").Value = "Delivery Date";
    //                sht.Range("Y5:Z6").Style.Font.FontName = "Calibri";
    //                sht.Range("Y5:Z6").Style.Font.FontSize = 11;
    //                sht.Range("Y5:Z6").Style.Font.SetBold();
    //                sht.Range("Y5:Z6").Merge();
    //                sht.Range("Y5:Z6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("Y5:Z6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("Y5:Z6").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("AA5").Value = ds.Tables[0].Rows[0]["REQBYDATE"];
    //                sht.Range("AA5:AB6").Style.Font.FontName = "Calibri";
    //                sht.Range("AA5:AB6").Style.Font.FontSize = 11;
    //                sht.Range("AA5:AB6").Merge();
    //                sht.Range("AA5:AB6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("AA5:AB6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("A7").Value = "RAW MATERIAL & COUNT";
    //                sht.Range("A7:D7").Style.Font.FontName = "Calibri";
    //                sht.Range("A7:D7").Style.Font.FontSize = 11;
    //                sht.Range("A7:D7").Style.Font.SetBold();
    //                sht.Range("A7:D7").Merge();
    //                sht.Range("A7:D7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A7:D7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A7:D7").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                int Dynamiccol6 = 4;
    //                int Dynamiccolstart6 = Dynamiccol6 + 1;
    //                int Dynamiccolend6 = 0;

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row2 = 7;
    //                    int noofrows2 = 0;
    //                    int i2 = 0;
    //                    int Dynamiccol2 = 4;
    //                    int Dynamiccolstart2 = Dynamiccol2 + 1;
    //                    int Dynamiccolend2;
    //                    int Totalcol2;
    //                    string ItemName = "";

    //                    DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    noofrows2 = dtdistinct2.Rows.Count;

    //                    for (i2 = 0; i2 < 11; i2++)
    //                    {
    //                        ItemName = "";
    //                        if (i2 < noofrows2)
    //                        {
    //                            ItemName = ds.Tables[1].Rows[i2]["Item_Name"] + " " + ds.Tables[1].Rows[i2]["QualityName"];
    //                        }
    //                        Dynamiccol2 = Dynamiccol2 + 1;
    //                        sht.Range(sht.Cell(row2, Dynamiccol2), sht.Cell(row2, Dynamiccol2 + 1)).Merge();

    //                        sht.Cell(row2, Dynamiccol2).Value = ItemName;
    //                        sht.Cell(row2, Dynamiccol2).Style.Font.Bold = true;
    //                        sht.Cell(row2, Dynamiccol2).Style.Font.FontName = "Calibri";
    //                        sht.Cell(row2, Dynamiccol2).Style.Font.FontSize = 9;
    //                        sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                        sht.Cell(row2, Dynamiccol2).Style.Alignment.WrapText = true;
    //                        //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
    //                        Dynamiccol2 = Dynamiccol2 + 1;

    //                        //sht.Range("AA5:AB6").Merge();

    //                    }
    //                    Dynamiccolend2 = Dynamiccol2;

    //                    Totalcol2 = Dynamiccolend2 + 1;

    //                    sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, Totalcol2 + 1)).Merge();

    //                    sht.Cell(row2, Totalcol2).Value = "TOTAL";
    //                    sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
    //                    sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
    //                    sht.Cell(row2, Totalcol2).Style.Font.FontSize = 9;
    //                    sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                    sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;

    //                    Dynamiccolend6 = Totalcol2;
    //                }

    //                sht.Range("A8").Value = "COLOUR";
    //                sht.Range("A8:D8").Style.Font.FontName = "Calibri";
    //                sht.Range("A8:D8").Style.Font.FontSize = 11;
    //                sht.Range("A8:D8").Style.Font.SetBold();
    //                sht.Range("A8:D8").Merge();
    //                sht.Range("A8:D8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A8:D8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A8:D8").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row3 = 8;
    //                    int noofrows3 = 0;
    //                    int i3 = 0;
    //                    int Dynamiccol3 = 4;
    //                    int Dynamiccolstart3 = Dynamiccol3 + 1;
    //                    int Dynamiccolend3;
    //                    int Totalcol3;
    //                    string ShadeColorName = "";

    //                    DataTable dtdistinct3 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    noofrows3 = dtdistinct3.Rows.Count;

    //                    for (i3 = 0; i3 < 12; i3++)
    //                    {
    //                        ShadeColorName = "";
    //                        if (i3 < noofrows3)
    //                        {
    //                            ShadeColorName = ds.Tables[1].Rows[i3]["ShadeColorName"].ToString();
    //                        }


    //                        Dynamiccol3 = Dynamiccol3 + 1;
    //                        sht.Range(sht.Cell(row3, Dynamiccol3), sht.Cell(row3, Dynamiccol3 + 1)).Merge();

    //                        sht.Cell(row3, Dynamiccol3).Value = ShadeColorName;
    //                        sht.Cell(row3, Dynamiccol3).Style.Font.Bold = true;
    //                        sht.Cell(row3, Dynamiccol3).Style.Font.FontName = "Calibri";
    //                        sht.Cell(row3, Dynamiccol3).Style.Font.FontSize = 9;
    //                        sht.Cell(row3, Dynamiccol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row3, Dynamiccol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                        sht.Cell(row3, Dynamiccol3).Style.Alignment.WrapText = true;
    //                        Dynamiccol3 = Dynamiccol3 + 1;
    //                    }

    //                    Dynamiccolend3 = Dynamiccol3;

    //                    Totalcol3 = Dynamiccolend3 + 1;

    //                }

    //                sht.Range("A9").Value = "EXTRA YARN ISSUE";
    //                sht.Range("A9:D9").Style.Font.FontName = "Calibri";
    //                sht.Range("A9:D9").Style.Font.FontSize = 11;
    //                sht.Range("A9:D9").Style.Font.SetBold();
    //                sht.Range("A9:D9").Merge();
    //                sht.Range("A9:D9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A9:D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A9:D9").Style.Fill.BackgroundColor = XLColor.LightGray;


    //                sht.Range("A10").Value = "LOT NO.";
    //                sht.Range("A10:D10").Style.Font.FontName = "Calibri";
    //                sht.Range("A10:D10").Style.Font.FontSize = 11;
    //                sht.Range("A10:D10").Style.Font.SetBold();
    //                sht.Range("A10:D10").Merge();
    //                sht.Range("A10:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A10:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A10:D10").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                int i = 0;
    //                int DynamiccolNew = 4;
    //                for (i = 0; i < 12; i++)
    //                {
    //                    DynamiccolNew = DynamiccolNew + 1;
    //                    sht.Range(sht.Cell(9, DynamiccolNew), sht.Cell(9, DynamiccolNew + 1)).Merge();
    //                    sht.Range(sht.Cell(10, DynamiccolNew), sht.Cell(10, DynamiccolNew + 1)).Merge();
    //                    DynamiccolNew = DynamiccolNew + 1;
    //                }

    //                sht.Range("A11").Value = "(1) WEIGHT YARN 90%";
    //                sht.Range("A11:D11").Style.Font.FontName = "Calibri";
    //                sht.Range("A11:D11").Style.Font.FontSize = 11;
    //                sht.Range("A11:D11").Style.Font.SetBold();
    //                sht.Range("A11:D11").Merge();
    //                sht.Range("A11:D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A11:D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A11:D11").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row4 = 11;
    //                    int noofrows4 = 0;
    //                    int i4 = 0;
    //                    int Dynamiccol4 = 4;
    //                    int Dynamiccolstart4 = Dynamiccol4 + 1;
    //                    int Dynamiccolend4;
    //                    int Totalcol4;

    //                    //DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    //DataView dv1 = new DataView(dtdistinctDateType);
    //                    //dv1.Sort = "Item_Name";
    //                    //DataTable dtdistinctDateType1 = dv1.ToTable();

    //                    DataTable dtdistinct4 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    noofrows4 = dtdistinct4.Rows.Count;

    //                    decimal TotalSumOneRow = 0;
    //                    for (i4 = 0; i4 < 11; i4++)
    //                    {
    //                        decimal IssRecConQty = 0;
    //                        if (i4 < noofrows4)
    //                        {
    //                            IssRecConQty = Convert.ToDecimal(ds.Tables[1].Rows[i4]["RawMaterial90PerQty"]);
    //                        }

    //                        Dynamiccol4 = Dynamiccol4 + 1;
    //                        sht.Range(sht.Cell(row4, Dynamiccol4), sht.Cell(row4, Dynamiccol4 + 1)).Merge();

    //                        sht.Cell(row4, Dynamiccol4).Value = IssRecConQty == 0 ? "" : IssRecConQty.ToString();
    //                        sht.Cell(row4, Dynamiccol4).Style.Font.Bold = true;
    //                        sht.Cell(row4, Dynamiccol4).Style.Font.FontName = "Calibri";
    //                        sht.Cell(row4, Dynamiccol4).Style.Font.FontSize = 10;
    //                        sht.Cell(row4, Dynamiccol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row4, Dynamiccol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                        sht.Cell(row4, Dynamiccol4).Style.Alignment.WrapText = true;
    //                        //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();

    //                        Dynamiccol4 = Dynamiccol4 + 1;
    //                        TotalSumOneRow = TotalSumOneRow + IssRecConQty;
    //                    }
    //                    Dynamiccolend4 = Dynamiccol4;

    //                    Totalcol4 = Dynamiccolend4 + 1;
    //                    sht.Range(sht.Cell(row4, Totalcol4), sht.Cell(row4, Totalcol4 + 1)).Merge();

    //                    sht.Cell(row4, Totalcol4).Value = TotalSumOneRow;
    //                    sht.Cell(row4, Totalcol4).Style.Font.Bold = true;
    //                    sht.Cell(row4, Totalcol4).Style.Font.FontName = "Calibri";
    //                    sht.Cell(row4, Totalcol4).Style.Font.FontSize = 10;
    //                    sht.Cell(row4, Totalcol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row4, Totalcol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                    sht.Cell(row4, Totalcol4).Style.Alignment.WrapText = true;
    //                }

    //                sht.Range("A12").Value = "(2) WEIGHT YARN 10%";
    //                sht.Range("A12:D12").Style.Font.FontName = "Calibri";
    //                sht.Range("A12:D12").Style.Font.FontSize = 11;
    //                sht.Range("A12:D12").Style.Font.SetBold();
    //                sht.Range("A12:D12").Merge();
    //                sht.Range("A12:D12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A12:D12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A12:D12").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row5 = 12;
    //                    int noofrows5 = 0;
    //                    int i5 = 0;
    //                    int Dynamiccol5 = 4;
    //                    int Dynamiccolstart5 = Dynamiccol5 + 1;
    //                    int Dynamiccolend5;
    //                    int Totalcol5;

    //                    //DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    //DataView dv1 = new DataView(dtdistinctDateType);
    //                    //dv1.Sort = "Item_Name";
    //                    //DataTable dtdistinctDateType1 = dv1.ToTable();

    //                    DataTable dtdistinct5 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                    noofrows5 = dtdistinct5.Rows.Count;

    //                    decimal TotalSumOneRow = 0;
    //                    for (i5 = 0; i5 < 11; i5++)
    //                    {
    //                        decimal IssRecConQty = 0;

    //                        if (i5 < noofrows5)
    //                        {
    //                            IssRecConQty = Convert.ToDecimal(ds.Tables[1].Rows[i5]["RawMaterial10PerQty"]);
    //                        }
    //                        Dynamiccol5 = Dynamiccol5 + 1;
    //                        sht.Range(sht.Cell(row5, Dynamiccol5), sht.Cell(row5, Dynamiccol5 + 1)).Merge();

    //                        sht.Cell(row5, Dynamiccol5).Value = IssRecConQty == 0 ? "" : IssRecConQty.ToString();
    //                        sht.Cell(row5, Dynamiccol5).Style.Font.Bold = true;
    //                        sht.Cell(row5, Dynamiccol5).Style.Font.FontName = "Calibri";
    //                        sht.Cell(row5, Dynamiccol5).Style.Font.FontSize = 10;
    //                        sht.Cell(row5, Dynamiccol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row5, Dynamiccol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                        sht.Cell(row5, Dynamiccol5).Style.Alignment.WrapText = true;
    //                        //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();


    //                        TotalSumOneRow = TotalSumOneRow + IssRecConQty;
    //                        Dynamiccol5 = Dynamiccol5 + 1;

    //                    }
    //                    Dynamiccolend5 = Dynamiccol5;

    //                    Totalcol5 = Dynamiccolend5 + 1;
    //                    sht.Range(sht.Cell(row5, Totalcol5), sht.Cell(row5, Totalcol5 + 1)).Merge();

    //                    sht.Cell(row5, Totalcol5).Value = TotalSumOneRow;
    //                    sht.Cell(row5, Totalcol5).Style.Font.Bold = true;
    //                    sht.Cell(row5, Totalcol5).Style.Font.FontName = "Calibri";
    //                    sht.Cell(row5, Totalcol5).Style.Font.FontSize = 10;
    //                    sht.Cell(row5, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row5, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                    sht.Cell(row5, Totalcol5).Style.Alignment.WrapText = true;

    //                }

    //                sht.Range("A13").Value = "DATE";
    //                sht.Range("A13:B13").Style.Font.FontName = "Calibri";
    //                sht.Range("A13:B13").Style.Font.FontSize = 11;
    //                sht.Range("A13:B13").Style.Font.SetBold();
    //                sht.Range("A13:B13").Merge();
    //                sht.Range("A13:B13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A13:B13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A13:B13").Style.Fill.BackgroundColor = XLColor.LightGray;

    //                sht.Range("C13").Value = "SLIP NO";
    //                sht.Range("C13:D13").Style.Font.FontName = "Calibri";
    //                sht.Range("C13:D13").Style.Font.FontSize = 11;
    //                sht.Range("C13:D13").Style.Font.SetBold();
    //                sht.Range("C13:D13").Merge();
    //                sht.Range("C13:D13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C13:D13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("C13:D13").Style.Fill.BackgroundColor = XLColor.LightGray;


    //                int row = 13;
    //                for (int m = Dynamiccolstart6; m <= Dynamiccolend6; m = m + 2)
    //                //for (int m = 5; m < 17; m++)
    //                {
    //                    var NintyPerQty = sht.Cell(11, m).Value;
    //                    var TenPerQty = sht.Cell(12, m).Value;
    //                    decimal IssueQty = 0;
    //                    IssueQty = Convert.ToDecimal(NintyPerQty == "" ? 0 : NintyPerQty) + Convert.ToDecimal(TenPerQty == "" ? 0 : TenPerQty);

    //                    sht.Range(sht.Cell(row, m), sht.Cell(row, m + 1)).Merge();

    //                    sht.Cell(row, m).SetValue(IssueQty == 0 ? "" : IssueQty.ToString());
    //                    sht.Cell(row, m).Style.Font.Bold = true;
    //                    sht.Cell(row, m).Style.Font.FontName = "Calibri";
    //                    sht.Cell(row, m).Style.Font.FontSize = 11;
    //                    sht.Cell(row, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                }

    //                row = row + 1;
    //                sht.Row(row).Height = 3;

    //                if (ds.Tables[2].Rows.Count > 0)
    //                {
    //                    int noofrows7 = 0;
    //                    int i7 = 0;
    //                    int Dynamiccol7 = 4;
    //                    int Dynamiccolstart7 = Dynamiccol7 + 1;
    //                    int i8 = 0;
    //                    int noofrows8 = 0;

    //                    DataTable dtdistinct7 = ds.Tables[2].DefaultView.ToTable(true, "Date", "ChallanNo");
    //                    noofrows7 = dtdistinct7.Rows.Count;

    //                    row = row + 1;

    //                    for (i7 = 0; i7 < noofrows7; i7++)
    //                    {
    //                        sht.Row(row).Height = 26.25;
    //                        sht.Range(sht.Cell(row, 1), sht.Cell(row, 2)).Merge();

    //                        sht.Range("A" + row).Value = dtdistinct7.Rows[i7]["Date"];
    //                        sht.Range("A" + row).Style.Font.FontName = "Calibri";
    //                        sht.Range("A" + row).Style.Font.FontSize = 11;
    //                        sht.Range("A" + row).Style.Font.SetBold();
    //                        //sht.Range("A" + row).Merge();
    //                        sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                        sht.Range("C" + row).Style.NumberFormat.Format = "@";
    //                        sht.Range(sht.Cell(row, 3), sht.Cell(row, 4)).Merge();
    //                        sht.Range("C" + row).Value = dtdistinct7.Rows[i7]["ChallanNo"];
    //                        sht.Range("C" + row).Style.Font.FontName = "Calibri";
    //                        sht.Range("C" + row).Style.Font.FontSize = 11;
    //                        sht.Range("C" + row).Style.Font.SetBold();
    //                        //sht.Range("C" + row).Merge();
    //                        sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Range("C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                        decimal TotalSumOneRow = 0;
    //                        for (int m = Dynamiccolstart6; m <= Dynamiccolend6; m = m + 2)
    //                        {
    //                            var itemname = sht.Cell(7, m).Value;
    //                            var ShadeColorName = sht.Cell(8, m).Value;

    //                            decimal WeightQty = 0;
    //                            decimal Penality = 0;
    //                            decimal IssueQty = 0;
    //                            var ItemNameMatch = "";

    //                            DataTable dtdistinct8 = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                            noofrows8 = dtdistinct8.Rows.Count;

    //                            for (i8 = 0; i8 < noofrows8; i8++)
    //                            {
    //                                ItemNameMatch = dtdistinct8.Rows[i8]["Item_Name"] + " " + dtdistinct8.Rows[i8]["QualityName"];
    //                                int result = string.Compare(itemname.ToString(), ItemNameMatch.ToString());

    //                                if (result == 0)
    //                                {
    //                                    DataRow[] foundRows;
    //                                    foundRows = ds.Tables[2].Select("Item_Name='" + dtdistinct8.Rows[i8]["Item_Name"] + "' and QualityName='" + dtdistinct8.Rows[i8]["QualityName"] + "' and DesignName='" + dtdistinct8.Rows[i8]["DesignName"] + "' and ShadeColorName='" + ShadeColorName + "' and Date='" + dtdistinct7.Rows[i7]["Date"] + "' and ChallanNo='" + dtdistinct7.Rows[i7]["ChallanNo"] + "' ");
    //                                    if (foundRows.Length > 0)
    //                                    {
    //                                        foreach (DataRow row4 in foundRows)
    //                                        {
    //                                            IssueQty = Convert.ToDecimal(row4["IssueQty"].ToString());
    //                                            break;
    //                                        }
    //                                    }
    //                                }


    //                            }
    //                            sht.Range(sht.Cell(row, m), sht.Cell(row, m + 1)).Merge();
    //                            sht.Cell(row, m).SetValue(IssueQty == 0 ? "" : IssueQty.ToString());
    //                            sht.Cell(row, m).Style.Font.Bold = true;
    //                            sht.Cell(row, m).Style.Font.FontName = "Calibri";
    //                            sht.Cell(row, m).Style.Font.FontSize = 11;
    //                            sht.Cell(row, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                            sht.Cell(row, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                            TotalSumOneRow = TotalSumOneRow + IssueQty;

    //                            sht.Cell(row, Dynamiccolend6).SetValue(TotalSumOneRow);
    //                            sht.Range(sht.Cell(row, Dynamiccolend6), sht.Cell(row, Dynamiccolend6 + 1)).Merge();
    //                        }



    //                        row = row + 1;
    //                    }
    //                }

    //                for (int r = row; r <= 26; r++)
    //                {
    //                    sht.Row(r).Height = 26.25;

    //                    for (int col = 1; col <= 28; col = col + 2)
    //                    {
    //                        sht.Range(sht.Cell(r, col), sht.Cell(r, col + 1)).Merge();
    //                    }

    //                    row = row + 1;
    //                }

    //                sht.Range(sht.Cell((row - 1), 3), sht.Cell((row - 1), 4)).Merge();

    //                sht.Cell((row - 1), 3).SetValue("Total");
    //                sht.Cell((row - 1), 3).Style.Font.Bold = true;
    //                sht.Cell((row - 1), 3).Style.Font.FontName = "Calibri";
    //                sht.Cell((row - 1), 3).Style.Font.FontSize = 11;
    //                sht.Cell((row - 1), 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Cell((row - 1), 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                int sumrowstart = 15;
    //                decimal TotalSumOneRowWise = 0;
    //                for (int t = Dynamiccolstart6; t <= Dynamiccolend6; t = t + 2)
    //                //for (int m = 5; m < 17; m++)
    //                {
    //                    TotalSumOneRowWise = 0;
    //                    for (int s = sumrowstart; s <= 26; s++)
    //                    {
    //                        var OneColumnValue = sht.Cell(s, t).Value;
    //                        TotalSumOneRowWise = TotalSumOneRowWise + Convert.ToDecimal(OneColumnValue == "" ? 0 : OneColumnValue);
    //                    }
    //                    sumrowstart = 15;

    //                    sht.Range(sht.Cell((row - 1), t), sht.Cell((row - 1), t + 1)).Merge();

    //                    sht.Cell((row - 1), t).SetValue(TotalSumOneRowWise);
    //                    sht.Cell((row - 1), t).Style.Font.Bold = true;
    //                    sht.Cell((row - 1), t).Style.Font.FontName = "Calibri";
    //                    sht.Cell((row - 1), t).Style.Font.FontSize = 11;
    //                    sht.Cell((row - 1), t).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell((row - 1), t).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                }


    //                ////////*************
    //                ////sht.Columns(1, 20).AdjustToContents();
    //                ////********************
    //                ////***********BOrders
    //                using (var a = sht.Range("A1" + ":AB" + (row - 1)))
    //                {
    //                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                }

    //                string Fileextension = "xlsx";
    //                string filename = UtilityModule.validateFilename("WeaverFolioMaterialDetailReportNew_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
    //                Path = Server.MapPath("~/Tempexcel/" + filename);
    //                xapp.SaveAs(Path);
    //                xapp.Dispose();
    //                //Download File
    //                Response.ClearContent();
    //                Response.ClearHeaders();
    //                // Response.Clear();
    //                Response.ContentType = "application/vnd.ms-excel";
    //                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //                Response.WriteFile(Path);
    //                // File.Delete(Path);
    //                Response.End();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            lblmsg.Text = ex.Message;
    //        }

    //    }

    //}
    protected void BtnStockNoStatus_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        DataSet DS;

        param[0] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
        param[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        param[1].Direction = ParameterDirection.Output;

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_StockNoStatus", param);
        if (param[1].Value != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[1].Value + "');", true);
            return;
        }
        if (DS.Tables[0].Rows.Count > 0)
        {
            Session["rptFilename"] = "Reports/RptStockNoStatus.rpt";
            Session["dsFilename"] = "~\\ReportSchema\\RptRptStockNoStatus.xsd";
            Session["GetDataset"] = DS;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void ChkForDepartmentReport_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDepartmentReport.Checked == true)
        {

        }
    }
}