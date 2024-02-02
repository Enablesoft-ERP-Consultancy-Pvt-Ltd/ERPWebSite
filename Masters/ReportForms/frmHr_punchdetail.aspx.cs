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
public partial class Masters_ReportForms_frmHr_punchdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock) 
                        JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order by CI.CompanyName 
                        Select Distinct D.DepartmentId, D.DepartmentName 
                        From Department D(Nolock)
                        JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                        JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                        Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                        Order By D.DepartmentName 
                        Select Designationid,Designation From HR_Designationmaster order by Designation 
                        Select ID, BranchName 
                        From BRANCHMASTER BM(nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                        Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                        SELECT DIVISIONID, DIVISION FROM HR_DIVISIONMASTER(Nolock) ORDER BY DISPSEQNO,DIVISION ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--ALL--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 2, true, "--Plz Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 3, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref dddivision, ds, 4, true, "--Plz Select--");

            DDreporttype.Items.Add(new ListItem("Punch Detail", "1"));
            DDreporttype.Items.Add(new ListItem("Attendance Report", "2"));
            DDreporttype.Items.Add(new ListItem("Long Absenteeism Report", "3"));
            DDreporttype.Items.Add(new ListItem("Performance Summary Report", "4"));
            DDreporttype.Items.Add(new ListItem("Daily Man Power Report", "5"));
            DDreporttype.Items.Add(new ListItem("New Joining Report", "6"));
            DDreporttype.Items.Add(new ListItem("Leaving Report", "7"));
            DDreporttype.Items.Add(new ListItem("Attendance Register", "8"));
            DDreporttype.Items.Add(new ListItem("Late Arrival Report", "9"));
            DDreporttype.Items.Add(new ListItem("Early Outing Report", "15"));
            DDreporttype.Items.Add(new ListItem("Department Wise Present Report", "10"));
            DDreporttype.Items.Add(new ListItem("Department Present Report", "11"));
            DDreporttype.Items.Add(new ListItem("Total Head Count", "12"));

            int UserType=1;
            
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16 && Convert.ToInt16(Session["varuserId"]) == 85)
            {
                UserType = 0;
            }
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16 && Convert.ToInt16(Session["varuserId"]) == 139)
            {
                UserType = 0;
            }
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16 && UserType == 1)
            {
                DDreporttype.Items.Add(new ListItem("Actual Performance Summary Report", "13"));
                DDreporttype.Items.Add(new ListItem("Actual Performance Detail Report", "14"));
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void Btnpreview_Click(object sender, EventArgs e)
    {
        switch (DDreporttype.SelectedValue)
        {
            case "1":
                Punchdetail();
                break;
            case "2":
                Attendance();
                break;
            case "3":
                LongAbsenteeismreport();
                break;
            case "4":
                Performancesummaryreport();
                break;
            case "5":
                DailymanpowerReport();
                break;
            //case "6":
            //    Newjoiningreport();
            //    break;
            case "7":
                Leavingreport();
                break;
            case "8":
                Attendanceregister();
                break;
            case "9":
                LateArrivalreport();
                break;
            case "10":
                DepartmentWisePresentReport();
                break;
            case "11":
                DepartmentPresentReport();
                break;
            case "12":
                TotalHeadCount();
                break;
            case "13":
                ActualPerformancesummaryreport();
                break;
            case "14":
                ActualPerformanceDetailReport();
                break;
            case "15":
                EarlyOutingReport();
                break;
            default:
                break;
        }
    }
    protected void Punchdetail()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETPUNCHDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                Session["rptFilename"] = "Reports/rpt_Hr_punchdetails.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_punchdetails.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Attendance()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETATTENDANCEREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                Session["rptFilename"] = "Reports/rpt_Hr_Attendancereport.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_Attendancereport.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void LongAbsenteeismreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_LONGABSENTEEISMREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("LongAbsenteeismReport");

                sht.Range("A1:G1").Merge();
                sht.Range("A2:G2").Merge();
                sht.Range("A3:G3").Merge();

                sht.Range("A1:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:G3").Style.Font.FontName = "Arial";
                sht.Range("A1:G3").Style.Font.Bold = true;
                sht.Range("A1:G2").Style.Font.FontSize = 15;
                sht.Range("A3:G3").Style.Font.FontSize = 13;
                sht.Range("A3:G3").Style.Font.SetUnderline();

                sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["companyname"]);
                sht.Range("A2").SetValue("LONG ABSENTEEISM REPORT");
                sht.Range("A3").SetValue("Report Date : " + txtfromdate.Text);

                //************Headers
                int Hrow = 4;
                sht.Range("A" + Hrow).SetValue("Sr No.");
                sht.Range("B" + Hrow).SetValue("Card No.");
                sht.Range("C" + Hrow).SetValue("Employee Name");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Department");
                sht.Range("F" + Hrow).SetValue("Absent since");
                sht.Range("G" + Hrow).SetValue("No of days");

                sht.Range("A" + Hrow + ":G" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":G" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":G" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":G" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":G" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(i + 1);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["absentsince"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Noofdays"]);
                    row = row + 1;
                }

                //******SAVE FILE
                sht.Columns(1, 10).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Longabsenteeismreport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Performancesummaryreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETATTENDANCEPERFORMENCEREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text); if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Performancereport");

                sht.Range("A1:I1").Merge();
                sht.Range("A2:I2").Merge();


                sht.Range("A1:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:I3").Style.Font.FontName = "Arial";
                sht.Range("A1:I3").Style.Font.Bold = true;
                sht.Range("A1:I2").Style.Font.FontSize = 15;


                sht.Range("A1").SetValue("PERFORMANCE REPORT");
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + " TO - " + txttodate.Text);


                //************Headers
                int Hrow = 3;
                sht.Range("A" + Hrow).SetValue("Date");
                sht.Range("B" + Hrow).SetValue("Card No.");
                sht.Range("C" + Hrow).SetValue("Employee Name");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Department");
                sht.Range("F" + Hrow).SetValue("In Time");
                sht.Range("G" + Hrow).SetValue("Out Time");
                sht.Range("H" + Hrow).SetValue("Hour");
                sht.Range("I" + Hrow).SetValue("Status");

                sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":I" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":I" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["InTime"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Outtime"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Workduration"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    row = row + 1;
                }

                //******SAVE FILE
                sht.Columns(1, 15).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Performancesummaryreport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DailymanpowerReport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETATTENDANCEREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                Session["rptFilename"] = "Reports/rpt_Hr_Dailymanpowerreport.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_Dailymanpowerreport.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Newjoiningreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETNEWJOININGDATA]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Newjoiningreport");


                sht.Range("A1").SetValue("NEW JOINING REPORT");
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + "  TO - " + txttodate.Text);

                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Department");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Section");
                sht.Range("F" + Hrow).SetValue("Cadre");
                sht.Range("G" + Hrow).SetValue("Doj");
                sht.Range("H" + Hrow).SetValue("Basic");
                //*********Dynamic Cell
                int cellend = 8;
                int Dynamiccell = 0;
                int Dynamiccell_start = 0;
                int Dynamiccell_end = 0;


                if (ds.Tables[1].Rows.Count > 0)
                {
                    Dynamiccell = cellend;
                    Dynamiccell_start = Dynamiccell + 1;

                    DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "Parametername");
                    DataView dv1 = new DataView(dtdistinct);
                    DataTable dtdistinct1 = dv1.ToTable();

                    foreach (DataRow dr in dtdistinct1.Rows)
                    {
                        Dynamiccell = Dynamiccell + 1;
                        sht.Cell(Hrow, Dynamiccell).Value = dr["Parametername"];
                    }
                    Dynamiccell_end = Dynamiccell;
                    //sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Merge();
                    //sht.Cell(3, Dynamiccell_start).SetValue("M.Balance");
                    //sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Style.Font.Bold = true;
                }
                int Lastcell = 0;
                if (Dynamiccell_end == 0)
                {
                    Lastcell = cellend;
                }
                else
                {
                    Lastcell = Dynamiccell_end;
                }
                int totalcell = Lastcell + 1;
                sht.Cell(Hrow, totalcell).Value = "Total Salary";

                sht.Range(sht.Cell(1, 1), sht.Cell(1, totalcell)).Merge();
                sht.Range(sht.Cell(2, 1), sht.Cell(2, totalcell)).Merge();
                sht.Range(sht.Cell(1, 1), sht.Cell(1, totalcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range(sht.Cell(2, 1), sht.Cell(2, totalcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range(sht.Cell(1, 1), sht.Cell(Hrow, totalcell)).Style.Font.FontName = "Arial Unicode MS";
                sht.Range(sht.Cell(1, 1), sht.Cell(Hrow, totalcell)).Style.Font.FontSize = 10;
                sht.Range(sht.Cell(1, 1), sht.Cell(Hrow, totalcell)).Style.Font.Bold = true;


                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range(sht.Cell(row, 1), sht.Cell(row, totalcell)).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range(sht.Cell(row, 1), sht.Cell(row, totalcell)).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EMpcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["category"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["cadre"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["dateofjoining"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["basicpay"]);
                    if (Dynamiccell > 0)
                    {
                        for (int k = Dynamiccell_start; k <= Dynamiccell_end; k++)
                        {
                            var Item_name = sht.Cell(Hrow, k).Value;
                            if (Item_name != "")
                            {
                                var alw_amount = ds.Tables[1].Compute("sum(Alw_amount)", "empid=" + ds.Tables[0].Rows[i]["empid"] + " and  parametername='" + Item_name + "'");
                                sht.Cell(row, k).SetValue(alw_amount == DBNull.Value ? 0 : alw_amount);
                            }

                        }
                    }
                    sht.Cell(row, totalcell).SetValue(ds.Tables[0].Rows[i]["grosssal"]);
                    row = row + 1;
                }


                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, totalcell)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("NewJoiningReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Leavingreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETLEAVINGREPORT]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Leavingreport");


                sht.Range("A1").SetValue("NEW LEAVING REPORT");
                sht.Range("A1:H1").Merge();
                sht.Range("A2:H2").Merge();
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + "  TO - " + txttodate.Text);
                sht.Range("A1:H2").Style.Font.SetBold();
                sht.Range("A1:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Department");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Section");
                sht.Range("F" + Hrow).SetValue("Cadre");
                sht.Range("G" + Hrow).SetValue("Doj");
                sht.Range("H" + Hrow).SetValue("Dol");

                sht.Range("A" + Hrow + ":H" + Hrow).Style.Font.SetBold();
                //*********Dynamic Cell
                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EMpcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["category"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["cadre"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["dateofjoining"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["RESIGNDATE"]);

                    row = row + 1;
                }


                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range("A1" + ":H" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("LeavingReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Attendanceregister()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("[Pro_HR_Getattendanceregister]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@Wagescalculation", DDwagescalculation.SelectedValue);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@DIVISION", dddivision.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (Designationid == "" && Departmentid == "" && txtempcode.Text == "")
                {
                    Session["rptFilename"] = "Reports/rpt_Hr_AttendanceregisterEmployeeDetail.rpt";
                    Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_AttendanceregisterEmployeeDetail.xsd";
                }
                else
                {
                    Session["rptFilename"] = "Reports/rpt_Hr_Attendanceregister.rpt";
                    Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_Attendanceregister.xsd";
                }
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void EarlyOutingReport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETEARLYGOINGREPORT]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Newjoiningreport");

                sht.Range("A1:I1").Merge();
                sht.Range("A2:I2").Merge();
                sht.Range("A1").SetValue("EARLY GOING REPORT - " + txtfromdate.Text);
                sht.Range("A2").SetValue("Company/Unit : " + DDCompanyName.SelectedItem.Text);
                sht.Range("A1:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:I2").Style.Font.SetBold();

                sht.Range("A3:C3").Merge();
                sht.Range("A3").SetValue("Total Early Arrivals : " + ds.Tables[0].Rows.Count);
                sht.Range("A3:C3").Style.Font.SetBold();

                int Hrow = 4;

                sht.Range("A" + Hrow).SetValue("S.No.");
                sht.Range("B" + Hrow).SetValue("Date");
                sht.Range("C" + Hrow).SetValue("Card No.");
                sht.Range("D" + Hrow).SetValue("Employee Name");
                sht.Range("E" + Hrow).SetValue("Designation");
                sht.Range("F" + Hrow).SetValue("Out");
                sht.Range("G" + Hrow).SetValue("Shift End");
                sht.Range("H" + Hrow).SetValue("Early Hrs.");
                sht.Range("I" + Hrow).SetValue("Remark");
                sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.SetBold();
                sht.Range("F" + Hrow + ":H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                
                int row = Hrow + 1;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row).SetValue(i + 1);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["DATEOFFICE"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["OUT2"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SHIFTENDTIME"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["EARLY_HRS"]);
                    sht.Range("I" + row).SetValue("");
                    row = row + 1;
                }
                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range("A1" + ":I" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("EarlyOutingReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void LateArrivalreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETLATEARRIVALREPORT]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Newjoiningreport");

                sht.Range("A1:I1").Merge();
                sht.Range("A2:I2").Merge();
                sht.Range("A1").SetValue("LATE ARRIVAL REPORT - " + txtfromdate.Text);
                sht.Range("A2").SetValue("Company/Unit : " + DDCompanyName.SelectedItem.Text);
                sht.Range("A1:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:I2").Style.Font.SetBold();


                sht.Range("A3:C3").Merge();
                sht.Range("A3").SetValue("Total Late Arrivals : " + ds.Tables[0].Rows.Count);
                sht.Range("A3:C3").Style.Font.SetBold();

                int Hrow = 4;

                sht.Range("A" + Hrow).SetValue("S.No.");
                sht.Range("B" + Hrow).SetValue("Date");
                sht.Range("C" + Hrow).SetValue("Card No.");
                sht.Range("D" + Hrow).SetValue("Employee Name");
                sht.Range("E" + Hrow).SetValue("Designation");
                sht.Range("F" + Hrow).SetValue("IN");
                sht.Range("G" + Hrow).SetValue("Shift Start");
                sht.Range("H" + Hrow).SetValue("Late Hrs.");
                sht.Range("I" + Hrow).SetValue("Remark");
                sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.SetBold();
                sht.Range("F" + Hrow + ":H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                int row = Hrow + 1;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row).SetValue(i + 1);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["DATEOFFICE"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Intime"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["shiftstart"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["late_hrs"]);
                    sht.Range("I" + row).SetValue("");
                    row = row + 1;
                }
                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range("A1" + ":I" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Latearrivalreport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DepartmentWisePresentReport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETDepartmentWisePresent", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@DEPARTMENTIDS", Departmentid);
            cmd.Parameters.AddWithValue("@DESIGNATIONIDS", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("DepartmentWisePresentReport");

                sht.Range("A1:E1").Merge();
                sht.Range("A2:E2").Merge();

                sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:E1").Style.Font.FontName = "Arial";
                sht.Range("A1:E1").Style.Font.Bold = true;
                sht.Range("A1:E1").Style.Font.FontSize = 15;
                sht.Range("A3:E3").Style.Font.FontSize = 11;

                sht.Range("A1").SetValue("Department wise Present Report");
                sht.Range("A2").SetValue("Report Date : " + txtfromdate.Text);

                //************Headers
                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Department");
                sht.Range("B" + Hrow).SetValue("Designation");
                sht.Range("C" + Hrow).SetValue("Total");
                sht.Range("D" + Hrow).SetValue("Present");
                sht.Range("E" + Hrow).SetValue("Absent");

                sht.Range("A" + Hrow + ":E" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":E" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":E" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":E" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Total"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["TotalPresent"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["TotalAbsent"]);
                    row = row + 1;
                }

                using (var a = sht.Range("B" + row + ":E" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("B" + row).SetValue("Total");
                sht.Range("C" + row).FormulaA1 = "=SUM(C4:C" + (row - 1) + ")";
                sht.Range("D" + row).FormulaA1 = "=SUM(D4:D" + (row - 1) + ")";
                sht.Range("E" + row).FormulaA1 = "=SUM(E4:E" + (row - 1) + ")";

                //******SAVE FILE
                sht.Columns(1, 6).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentWisePresentReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void TotalHeadCount()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETTOTALHEADCOUNT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                Session["rptFilename"] = "Reports/rpt_Hr_TotalHeadCountReport.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\rpt_Hr_TotalHeadCountReport.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DepartmentPresentReport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            //for (int i = 0; i < lstdesignation.Items.Count; i++)
            //{
            //    if (Designationid == "")
            //    {
            //        Designationid = lstdesignation.Items[i].Value;
            //    }
            //    else
            //    {
            //        Designationid = Designationid + "," + lstdesignation.Items[i].Value;
            //    }
            //}
            SqlCommand cmd = new SqlCommand("PRO_HR_GETDepartmentPresent", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@DEPARTMENTIDS", Departmentid);
            //cmd.Parameters.AddWithValue("@DESIGNATIONIDS", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("DepartmentWisePresentReport");

                sht.Range("A1:D1").Merge();
                sht.Range("A2:D2").Merge();

                sht.Range("A1:D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:D1").Style.Font.FontName = "Arial";
                sht.Range("A1:D1").Style.Font.Bold = true;
                sht.Range("A1:D1").Style.Font.FontSize = 15;
                sht.Range("A3:D3").Style.Font.FontSize = 11;

                sht.Range("A1").SetValue("Department Present Report");
                sht.Range("A2").SetValue("Report Date : " + txtfromdate.Text);

                //************Headers
                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Department");
                sht.Range("B" + Hrow).SetValue("Total");
                sht.Range("C" + Hrow).SetValue("Present");
                sht.Range("D" + Hrow).SetValue("Absent");

                sht.Range("A" + Hrow + ":D" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":D" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":D" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":D" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":D" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Total"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["TotalPresent"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["TotalAbsent"]);
                    row = row + 1;
                }

                using (var a = sht.Range("A" + row + ":D" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A" + row).SetValue("Total");
                sht.Range("B" + row).FormulaA1 = "=SUM(B4:B" + (row - 1) + ")";
                sht.Range("C" + row).FormulaA1 = "=SUM(C4:C" + (row - 1) + ")";
                sht.Range("D" + row).FormulaA1 = "=SUM(D4:D" + (row - 1) + ")";

                //******SAVE FILE
                sht.Columns(1, 6).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentPresentReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDept.SelectedIndex > 0)
        {
            if (lstdept.Items.FindByValue(DDDept.SelectedValue) == null)
            {

                lstdept.Items.Add(new ListItem(DDDept.SelectedItem.Text, DDDept.SelectedValue));
            }
        }
    }
    protected void DDDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDesignation.SelectedIndex > 0)
        {
            if (lstdesignation.Items.FindByValue(DDDesignation.SelectedValue) == null)
            {

                lstdesignation.Items.Add(new ListItem(DDDesignation.SelectedItem.Text, DDDesignation.SelectedValue));
            }
        }
    }
    protected void btnDeletedept_Click(object sender, EventArgs e)
    {
        //lstdept.Items.Clear();
        lstdept.Items.Remove(lstdept.SelectedItem);
    }
    protected void btndeletedesignation_Click(object sender, EventArgs e)
    {
        //lstdesignation.Items.Clear();
        lstdesignation.Items.Remove(lstdesignation.SelectedItem);
    }
    protected void DDreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        trdivision.Visible = false;
        switch (DDreporttype.SelectedValue)
        {
            case "4":
            case "6":
            case "7":
            case "9":
            case "13":
            case "14":
            case "15":
                trtodate.Visible = true;
                break;
            case "8":
                trdivision.Visible = true;
                trtodate.Visible = true;
                break;
            default:
                trtodate.Visible = false;
                break;
        }
    }
    protected void CHkAllDept_CheckedChanged(object sender, EventArgs e)
    {
        if (CHkAllDept.Checked == true)
        {
            for (int i = 1; i < DDDept.Items.Count; i++)
            {
                lstdept.Items.Add(new ListItem(DDDept.Items[i].Text, DDDept.Items[i].Value));
            }
        }
        else
        {
            lstdept.Items.Clear();
        }
    }
    protected void ChkAllDesignation_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkAllDesignation.Checked == true)
        {
            for (int i = 1; i < DDDesignation.Items.Count; i++)
            {
                lstdesignation.Items.Add(new ListItem(DDDesignation.Items[i].Text, DDDesignation.Items[i].Value));
            }
        }
        else
        {
            lstdesignation.Items.Clear();
        }
    }

    protected void ActualPerformancesummaryreport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETATTENDANCEPERFORMENCEREPORT_ActualDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Wagescalculation", DDwagescalculation.SelectedValue);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 0);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("ActualPerformanceSummaryReport");

                sht.Range("A1:K1").Merge();
                sht.Range("A2:K2").Merge();


                sht.Range("A1:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K3").Style.Font.FontName = "Arial";
                sht.Range("A1:K3").Style.Font.Bold = true;
                sht.Range("A1:K2").Style.Font.FontSize = 15;


                sht.Range("A1").SetValue("ACTUAL PERFORMANCE REPORT");
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + " TO - " + txttodate.Text);


                //************Headers
                int Hrow = 3;
                sht.Range("A" + Hrow).SetValue("Date");
                sht.Range("B" + Hrow).SetValue("Card No.");
                sht.Range("C" + Hrow).SetValue("Employee Name");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Department");
                sht.Range("F" + Hrow).SetValue("In Time");
                sht.Range("G" + Hrow).SetValue("Out Time");
                sht.Range("H" + Hrow).SetValue("Status");
                sht.Range("I" + Hrow).SetValue("Hour");
                sht.Range("J" + Hrow).SetValue("Actual Hour");
                sht.Range("K" + Hrow).SetValue("NoofDay");

                sht.Range("A" + Hrow + ":K" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":K" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":K" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":K" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":K" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["InTime"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Outtime"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Workduration"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["TotalHours"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["NOOFDAY"]);
                    sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("K" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                    row = row + 1;
                }

                //******SAVE FILE
                sht.Columns(1, 15).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ActualPerformanceSummaryReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ActualPerformanceDetailReport()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string Departmentid = "", Designationid = "";
            for (int i = 0; i < lstdept.Items.Count; i++)
            {
                if (Departmentid == "")
                {
                    Departmentid = lstdept.Items[i].Value;
                }
                else
                {
                    Departmentid = Departmentid + "," + lstdept.Items[i].Value;
                }
            }
            for (int i = 0; i < lstdesignation.Items.Count; i++)
            {
                if (Designationid == "")
                {
                    Designationid = lstdesignation.Items[i].Value;
                }
                else
                {
                    Designationid = Designationid + "," + lstdesignation.Items[i].Value;
                }
            }
            SqlCommand cmd = new SqlCommand("PRO_HR_GETATTENDANCEPERFORMENCEREPORT_ActualDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Wagescalculation", DDwagescalculation.SelectedValue);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 1);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("ActualPerformanceDetailReport");

                sht.Range("A1:H1").Merge();
                sht.Range("A2:H2").Merge();

                sht.Range("A1:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H3").Style.Font.FontName = "Arial";
                sht.Range("A1:H3").Style.Font.Bold = true;
                sht.Range("A1:H2").Style.Font.FontSize = 15;

                sht.Range("A1").SetValue("ACTUAL PERFORMANCE REPORT");
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + " TO - " + txttodate.Text);

                //************Headers
                int Hrow = 3;
                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Employee Name");
                sht.Range("C" + Hrow).SetValue("Designation");
                sht.Range("D" + Hrow).SetValue("Department");
                sht.Range("E" + Hrow).SetValue("Total Days");
                sht.Range("F" + Hrow).SetValue("Actaul Hour");
                sht.Range("G" + Hrow).SetValue("Working Hours");
                sht.Range("H" + Hrow).SetValue("Difference");

                sht.Range("A" + Hrow + ":H" + Hrow).Style.Font.FontName = "Arial";
                sht.Range("A" + Hrow + ":H" + Hrow).Style.Font.Bold = true;
                sht.Range("A" + Hrow + ":H" + Hrow).Style.Font.FontSize = 10;

                using (var a = sht.Range("A" + Hrow + ":H" + Hrow))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":H" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["TotalDay"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["WORKDURATION"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["TotalHours"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TotalPlusMinus"]);
                    sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    row = row + 1;
                }

                //******SAVE FILE
                sht.Columns(1, 15).AdjustToContents();

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ActualPerformanceSummaryReport" + DateTime.Now + "." + Fileextension);
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}