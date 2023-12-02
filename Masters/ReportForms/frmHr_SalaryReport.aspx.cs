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
public partial class Masters_ReportForms_frmHr_SalaryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock) 
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order by CI.CompanyName 
                            Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varCompanyId"] + @" 
                            Order By D.DepartmentName 
                            Select Designationid,Designation From HR_Designationmaster order by Designation
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @"
                            Select SUBDEPTID, SUBDEPT From HR_SUBDEPT(Nolock) Order By SUBDEPT ";

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
            UtilityModule.ConditionalComboFillWithDS(ref DDSubDepartment, ds, 4, true, "Select");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtAsOnDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Convert.ToInt16(Request.QueryString["ReportType"]) == 1)
            {
                DDreporttype.Items.Add(new ListItem("Minimum Wages Report(Pcs Wise)", "3"));
                DDreporttype.Items.Add(new ListItem("Salary Report Value", "2"));
                DDreporttype.Items.Add(new ListItem("Employee Report", "4"));
                DDreporttype.Items.Add(new ListItem("Advance Report", "5"));
                DDreporttype.Items.Add(new ListItem("Grautity Report", "6"));
                DDreporttype.Items.Add(new ListItem("EARN LEAVE DETAIL", "7"));
                DDreporttype.Items.Add(new ListItem("BONUS DETAIL", "8"));
                DDreporttype.Items.Add(new ListItem("FULL AND FINAL HISSAB", "9"));
                DDreporttype.Items.Add(new ListItem("FORM C", "10"));
            }
            else
            {
                DDreporttype.Items.Add(new ListItem("New Joining Report", "1"));
            }
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
        lstdept.Items.Remove(lstdept.SelectedItem);
        //lstdept.Items.Clear();
    }
    protected void btndeletedesignation_Click(object sender, EventArgs e)
    {
        lstdesignation.Items.Remove(lstdesignation.SelectedItem);
        //lstdesignation.Items.Clear();
    }
    protected void Btnpreview_Click(object sender, EventArgs e)
    {
        switch (DDreporttype.SelectedValue)
        {
            case "1":
                Newjoiningreport();
                break;
            case "2":
                Currentsalaryreport();
                break;
            case "3":
                Minimumwages();
                break;
            case "4":
                EmployeeMaster();
                break;
            case "5":
                Advancereport();
                break;
            case "6":
                GratuityReport();
                break;
            case "7":
                EARNLEAVEDETAIL();
                break;
            case "8":
                BONUSDETAIL();
                break;
            case "9":
                FULLANDFINALHISSABDETAIL();
                break;
            case "10":
                BONUSDETAILFORMC();
                break;
            default:
                break;
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
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@SubDepartmentID", DDSubDepartment.SelectedValue);

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
                sht.Range("E" + Hrow).SetValue("SubDepartment");
                sht.Range("F" + Hrow).SetValue("Section");
                sht.Range("G" + Hrow).SetValue("Cadre");
                sht.Range("H" + Hrow).SetValue("Doj");
                sht.Range("I" + Hrow).SetValue("Basic");
                
                //*********Dynamic Cell
                int cellend = 9;
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
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SubDepartment"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["category"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["cadre"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["dateofjoining"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["basicpay"]);
                    
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
    protected void Currentsalaryreport()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETCURRENTSALARYDATA]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@RESIGNSTATUS", chkresignstatus.Checked == true ? "1" : "0");
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@SubDepartmentID", DDSubDepartment.SelectedValue);

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
                var sht = xapp.Worksheets.Add("Currentsalaryreport");

                string status = "All Running Employees.";
                if (chkresignstatus.Checked == true)
                {
                    status = "All Retirement/Resignation Employees.";
                }
                sht.Range("A1").SetValue("CURRENT SALARY REPORT");
                sht.Range("A2").SetValue(status);

                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Department");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("SubDepartment");
                sht.Range("F" + Hrow).SetValue("Section");
                sht.Range("G" + Hrow).SetValue("Cadre");
                sht.Range("H" + Hrow).SetValue("DOJ");
                sht.Range("I" + Hrow).SetValue("DOL");
                sht.Range("J" + Hrow).SetValue("Wages Cal.");
                sht.Range("K" + Hrow).SetValue("E.P.F");
                sht.Range("L" + Hrow).SetValue("E.S.I");
                sht.Range("M" + Hrow).SetValue("Basic");
                
                //*********Dynamic Cell
                int cellend = 13;
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
                        if (dr["Parametername"] != "")
                        {
                            Dynamiccell = Dynamiccell + 1;
                            sht.Cell(Hrow, Dynamiccell).Value = dr["Parametername"];
                        }
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
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SubDepartment"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["category"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["cadre"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["dateofjoining"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["RESIGNDATE"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["WAGESCALCULATION"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["EPF"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ESI"]);
                    

                    decimal basic_pay = 0;
                    decimal alwtotal = 0;
                    if (Dynamiccell > 0)
                    {
                        DataTable dtTable = ds.Tables[1];
                        DataRow[] drs = dtTable.Select("empid='" + ds.Tables[0].Rows[i]["empid"] + "'");
                        if (drs.Length > 0)
                        {
                            foreach (DataRow dr in drs)
                            {
                                //basic_pay = Convert.ToDecimal(dr["basic_pay"]);
                                sht.Range("M" + row).SetValue(dr["basic_pay"]);
                                sht.Cell(row, totalcell).SetValue(dr["Grosssal"]);
                            }

                        }
                        // sht.Range("L" + row).SetValue(basic_pay);
                        for (int k = Dynamiccell_start; k <= Dynamiccell_end; k++)
                        {
                            var Item_name = sht.Cell(Hrow, k).Value;
                            if (Item_name != "")
                            {
                                var alw_amount = ds.Tables[1].Compute("sum(Alw_amount)", "empid=" + ds.Tables[0].Rows[i]["empid"] + " and  parametername='" + Item_name + "'");
                                sht.Cell(row, k).SetValue(alw_amount == DBNull.Value ? 0 : alw_amount);
                                //alwtotal = alwtotal + Convert.ToDecimal(alw_amount == DBNull.Value ? 0 : alw_amount);
                            }

                        }
                    }
                    //sht.Cell(row, totalcell).SetValue(ds.Tables[0].Rows[i]["grosssal"]);
                    //sht.Cell(row, totalcell).SetValue(basic_pay + alwtotal);
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
                string filename = UtilityModule.validateFilename("CurrentSalaryReport" + DateTime.Now + "." + Fileextension);
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
    protected void Minimumwages()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETMINIMUMWAGES_PCSWISE]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

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
            cmd.Parameters.AddWithValue("@SubDepartmentID", DDSubDepartment.SelectedValue);

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
                var sht = xapp.Worksheets.Add("MinimumWages");

                sht.Range("A1:K1").Merge();
                sht.Range("A2:K2").Merge();
                sht.Range("A3:K3").Merge();
                sht.Range("A4:K4").Merge();
                sht.Range("A1:K4").Style.Font.SetBold();
                sht.Range("A1:K4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"]);
                sht.Range("A2").SetValue(ds.Tables[0].Rows[0]["CompanyAddress"]);
                sht.Range("A3").SetValue("MINIMUM WAGES REPORT");
                sht.Range("A4").SetValue("FROM - " + txtfromdate.Text + "  TO - " + txttodate.Text);
                
                int Hrow = 5;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Department");
                sht.Range("D" + Hrow).SetValue("Designation");
                sht.Range("E" + Hrow).SetValue("Sub Department");
                sht.Range("F" + Hrow).SetValue("Basic");
                sht.Range("G" + Hrow).SetValue("Days");
                sht.Range("H" + Hrow).SetValue("PerDay");
                sht.Range("I" + Hrow).SetValue("EarAmt");
                sht.Range("J" + Hrow).SetValue("ProdAmt");
                sht.Range("K" + Hrow).SetValue("Min");
                sht.Range("H" + Hrow + ":K" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + Hrow + ":K" + Hrow).Style.Font.SetBold();

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Department"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SubDepartment"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Basicpay"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Daycount"]);
                    sht.Range("H" + row).Style.NumberFormat.Format = "0.00";
                    sht.Range("K" + row).Style.NumberFormat.Format = "0.00";
                    sht.Range("H" + row).FormulaA1 = "=F" + row + "/26";
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Basicearnamt"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Productionamt"]);
                    sht.Range("K" + row).FormulaA1 = "=J" + row + "-I" + row + "";
                    row = row + 1;
                }
                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range("A1" + ":K" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Minimumwages" + DateTime.Now + "." + Fileextension);
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
    protected void EmployeeMaster()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETEMPLOYEEDETAIL]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@RESIGNSTATUS", chkresignstatus.Checked == true ? "1" : "0");
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            cmd.Parameters.AddWithValue("@SubDepartmentID", DDSubDepartment.SelectedValue);

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
                var sht = xapp.Worksheets.Add("Employeedetail");

                string status = "All Running Employees.";
                if (chkresignstatus.Checked == true)
                {
                    status = "All Retirement/Resignation Employees.";
                }
                sht.Range("A1").SetValue("EMPLOYEE MASTER REPORT");
                sht.Range("A2").SetValue(status);

                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Father's Name");
                sht.Range("D" + Hrow).SetValue("Department");
                sht.Range("E" + Hrow).SetValue("Designation");
                sht.Range("F" + Hrow).SetValue("Sub Department");

                sht.Range("G" + Hrow).SetValue("Category");
                sht.Range("H" + Hrow).SetValue("Cadre");
                sht.Range("I" + Hrow).SetValue("PayrollType");
                sht.Range("J" + Hrow).SetValue("DOJ");
                sht.Range("K" + Hrow).SetValue("DOB");
                sht.Range("L" + Hrow).SetValue("Age");
                sht.Range("M" + Hrow).SetValue("Sex");
                sht.Range("N" + Hrow).SetValue("Nationality");
                sht.Range("O" + Hrow).SetValue("Religion");
                sht.Range("P" + Hrow).SetValue("Maritalstatus");
                sht.Range("Q" + Hrow).SetValue("Relative");
                sht.Range("R" + Hrow).SetValue("Relation");
                sht.Range("S" + Hrow).SetValue("Relativeage");
                sht.Range("T" + Hrow).SetValue("Present address");
                sht.Range("U" + Hrow).SetValue("Permanent address");
                sht.Range("V" + Hrow).SetValue("Qualification");
                sht.Range("W" + Hrow).SetValue("Technical Qualification");
                sht.Range("X" + Hrow).SetValue("Languages");
                sht.Range("Y" + Hrow).SetValue("Experience");
                sht.Range("Z" + Hrow).SetValue("Mother's Name");
                sht.Range("AA" + Hrow).SetValue("Doc Type");
                sht.Range("AB" + Hrow).SetValue("Doc No.");
                sht.Range("AC" + Hrow).SetValue("P.F");
                sht.Range("AD" + Hrow).SetValue("P.F No.");
                sht.Range("AE" + Hrow).SetValue("UAN No.");
                sht.Range("AF" + Hrow).SetValue("E.S.I");
                sht.Range("AG" + Hrow).SetValue("ESI No.");
                sht.Range("AH" + Hrow).SetValue("Bank Holder Name");
                sht.Range("AI" + Hrow).SetValue("Bank Name");
                sht.Range("AJ" + Hrow).SetValue("A/C No.");
                sht.Range("AK" + Hrow).SetValue("IFSC Code");
                sht.Range("AL" + Hrow).SetValue("Branch Name");
                sht.Range("AM" + Hrow).SetValue("DOL");
                sht.Range("AN" + Hrow).SetValue("Ph Number");                
                sht.Range("AO" + Hrow).SetValue("Minimum Wages");
                sht.Range("AP" + Hrow).SetValue("Employee Group");
                sht.Range("AQ" + Hrow).SetValue("Payroll Type");
                sht.Range("AR" + Hrow).SetValue("Basic");
               
                //*********Dynamic Cell
                int cellend = 44;
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
                        if (dr["Parametername"] != "")
                        {
                            Dynamiccell = Dynamiccell + 1;
                            sht.Cell(Hrow, Dynamiccell).Value = dr["Parametername"];
                        }
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
                sht.Cell(Hrow, totalcell).Value = "Gross Salary";

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

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["fathername"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Dept"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SubDepartment"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Category"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["cadre"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["payrolltype"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["doj"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Dob"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Age"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Gender"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Nationality"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Religion"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["maritalstatus"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["relativename"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["relation"]);
                    sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["relativeage"]);
                    sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["presentaddress"]);
                    sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["permanentaddress"]);
                    sht.Range("V" + row).SetValue(ds.Tables[0].Rows[i]["Qualification"]);
                    sht.Range("W" + row).SetValue(ds.Tables[0].Rows[i]["TECHNICAL"]);
                    sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["languageknown"]);
                    sht.Range("Y" + row).SetValue(ds.Tables[0].Rows[i]["TOTALEXP"]);
                    sht.Range("Z" + row).SetValue(ds.Tables[0].Rows[i]["Mothername"]);
                    sht.Range("AA" + row).SetValue(ds.Tables[0].Rows[i]["Doctype"]);
                    sht.Range("AB" + row).SetValue(ds.Tables[0].Rows[i]["Docno"]);
                    sht.Range("AC" + row).SetValue(ds.Tables[0].Rows[i]["pf"]);
                    sht.Range("AD" + row).SetValue(ds.Tables[0].Rows[i]["pfno"]);
                    sht.Range("AE" + row).SetValue(ds.Tables[0].Rows[i]["UanNo"]);
                    sht.Range("AF" + row).SetValue(ds.Tables[0].Rows[i]["Esi"]);
                    sht.Range("AG" + row).SetValue(ds.Tables[0].Rows[i]["EsiNo"]);
                    sht.Range("AH" + row).SetValue(ds.Tables[0].Rows[i]["Accountholdername"]);
                    sht.Range("AI" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                    sht.Range("AJ" + row).SetValue(ds.Tables[0].Rows[i]["Bankacno"]);
                    sht.Range("AK" + row).SetValue(ds.Tables[0].Rows[i]["IFsccode"]);
                    sht.Range("AL" + row).SetValue(ds.Tables[0].Rows[i]["Branch"]);
                    sht.Range("AM" + row).SetValue(ds.Tables[0].Rows[i]["dol"]);
                    sht.Range("AN" + row).SetValue(ds.Tables[0].Rows[i]["Phno_present"]);
                    sht.Range("AO" + row).SetValue(ds.Tables[0].Rows[i]["MINIMUMWAGESBASIC"]);
                    sht.Range("AP" + row).SetValue(ds.Tables[0].Rows[i]["GroupName"]);                    
                    sht.Range("AQ" + row).SetValue(ds.Tables[0].Rows[i]["PayRollTypeName"]);
                    

                    decimal basic_pay = 0;
                    decimal alwtotal = 0;
                    if (Dynamiccell > 0)
                    {
                        DataTable dtTable = ds.Tables[1];
                        DataRow[] drs = dtTable.Select("empid='" + ds.Tables[0].Rows[i]["empid"] + "'");
                        if (drs.Length > 0)
                        {
                            foreach (DataRow dr in drs)
                            {
                                //basic_pay = Convert.ToDecimal(dr["basic_pay"]);
                                sht.Range("AR" + row).SetValue(dr["basic_pay"]);
                                sht.Cell(row, totalcell).SetValue(dr["Grosssal"]);
                            }

                        }
                        // sht.Range("L" + row).SetValue(basic_pay);
                        for (int k = Dynamiccell_start; k <= Dynamiccell_end; k++)
                        {
                            var Item_name = sht.Cell(Hrow, k).Value;
                            if (Item_name != "")
                            {
                                var alw_amount = ds.Tables[1].Compute("sum(Alw_amount)", "empid=" + ds.Tables[0].Rows[i]["empid"] + " and  parametername='" + Item_name + "'");
                                sht.Cell(row, k).SetValue(alw_amount == DBNull.Value ? 0 : alw_amount);
                                //alwtotal = alwtotal + Convert.ToDecimal(alw_amount == DBNull.Value ? 0 : alw_amount);
                            }

                        }
                    }
                    //sht.Cell(row, totalcell).SetValue(ds.Tables[0].Rows[i]["grosssal"]);
                    //sht.Cell(row, totalcell).SetValue(basic_pay + alwtotal);
                    row = row + 1;
                }


                //************
                sht.Columns(1, 50).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, totalcell)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Employeemasterreport" + DateTime.Now + "." + Fileextension);
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
    protected void Advancereport()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETADVANCEDATA]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@Resignstatus", chkresignstatus.Checked == true ? "1" : "0");
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
                var sht = xapp.Worksheets.Add("Advancereport");

                sht.Range("A1:Q1").Merge();
                sht.Range("A2:Q2").Merge();
                sht.Range("A1:Q2").Style.Font.SetBold();
                sht.Range("A1:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("A1").SetValue("ADVANCE REPORT");
                sht.Range("A2").SetValue("FROM - " + txtfromdate.Text + "  TO - " + txttodate.Text);

                int Hrow = 3;

                sht.Range("A" + Hrow).SetValue("Sr No.");
                sht.Range("B" + Hrow).SetValue("Entry Date");
                sht.Range("B" + Hrow).SetValue("Card No.");
                sht.Range("C" + Hrow).SetValue("Name");
                sht.Range("D" + Hrow).SetValue("Department");
                sht.Range("E" + Hrow).SetValue("Cr/Dr");
                sht.Range("F" + Hrow).SetValue("Type");
                sht.Range("G" + Hrow).SetValue("Amount");
                sht.Range("H" + Hrow).SetValue("NoofInstallment");
                sht.Range("I" + Hrow).SetValue("Installment Amt.");
                sht.Range("J" + Hrow).SetValue("Deduction period");
                sht.Range("K" + Hrow).SetValue("Deduction Year");
                sht.Range("L" + Hrow).SetValue("Deduction Month");
                sht.Range("M" + Hrow).SetValue("Payment Mode");
                sht.Range("N" + Hrow).SetValue("Cheque No.");
                sht.Range("O" + Hrow).SetValue("Bank Name");
                sht.Range("P" + Hrow).SetValue("Remark");

                sht.Range("H" + Hrow + ":J" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + Hrow + ":P" + Hrow).Style.Font.SetBold();
                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sht.Range("A" + row).SetValue(i + 1);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Adv_loandate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["cr_dr"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["type"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["AMount"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["noofinstallment"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["installmentamount"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Deductionperiod"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Deductionyear"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Month_name"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["paymentmode"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["ChequeNo"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }
                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range("A1" + ":P" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Advancereport" + DateTime.Now + "." + Fileextension);
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
    protected void GratuityReport()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETGRAUTITYSTATEMENT_REPORT]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@AsOndate", txtAsOnDate.Text);
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
                Session["dsFileName"] = "~\\ReportSchema\\RptHR_GratuityStatement.xsd";
                Session["rptFileName"] = "Reports/RptHR_GratuityStatement.rpt";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void EARNLEAVEDETAIL()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETEARNLEAVEDETAIL]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFilename"] = "Reports/RPT_HR_EARNLEAVEDETAIL.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_EARNLEAVEDETAIL.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsalary", "alert('No Records found for this combination !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void FULLANDFINALHISSABDETAIL()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_FULLANDFINALHISSAB]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFilename"] = "Reports/RptHr_FullAndFinalHissab.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_FULLANDFINALHISSAB.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsalary", "alert('No Records found for this combination !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BONUSDETAILFORMC()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETFORMCDETAIL]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("FORMC");

                int Hrow = 1;
                sht.Range("A" + Hrow).SetValue("Payment of Bonus Act");
                sht.Range("A" + Hrow, "Q" + Hrow).Merge();
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 14;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 2;
                sht.Range("A" + Hrow).SetValue("FORM C");
                sht.Range("A" + Hrow, "Q" + Hrow).Merge();
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 14;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 3;
                sht.Range("A" + Hrow).SetValue("See rule 4(b)");
                sht.Range("A" + Hrow, "Q" + Hrow).Merge();
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 7;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 4;
                sht.Range("A" + Hrow).SetValue("BONUS PAID TO EMPLOYEES FOR THE ACCOUNTING YEAR ENDING ON THE (" + ds.Tables[0].Rows[0]["FromDate"] + " To " + ds.Tables[0].Rows[0]["ToDate"] + ")");
                sht.Range("A" + Hrow, "Q" + Hrow).Merge();
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 10;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 5;
                sht.Range("A" + Hrow).SetValue("Name of the establishment - " + ds.Tables[0].Rows[0]["CompanyName"] + "");
                sht.Range("A" + Hrow, "C" + Hrow).Merge();
                sht.Range("A" + Hrow, "C" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 10;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 6;
                sht.Range("A" + Hrow).SetValue("No. of working days in the year " + ds.Tables[0].Rows[0]["WorkingDaysinYear"] + "");
                sht.Range("A" + Hrow, "C" + Hrow).Merge();
                sht.Range("A" + Hrow, "C" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.FontSize = 10;
                sht.Range("A" + Hrow, "Q" + Hrow).Style.Font.SetBold();

                Hrow = 8;
                sht.Range("I" + Hrow, "M" + Hrow).Merge();
                sht.Range("I" + Hrow, "M" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + Hrow).SetValue("Deductions");
                sht.Range("A" + Hrow, "M" + Hrow).Style.Font.SetBold();

                Hrow = 9;

                sht.Column("A").Width = 8.50;
                sht.Column("B").Width = 28.00;
                sht.Column("C").Width = 30;
                sht.Column("D").Width = 10.0;
                sht.Column("E").Width = 30;
                sht.Column("F").Width = 8.50;
                sht.Column("G").Width = 8.50;
                sht.Column("H").Width = 8.50;
                sht.Column("I").Width = 8.50;
                sht.Column("J").Width = 8.50;
                sht.Column("K").Width = 8.50;
                sht.Column("L").Width = 10.60;
                sht.Column("M").Width = 8.50;
                sht.Column("N").Width = 8.50;
                sht.Column("O").Width = 8.50;
                sht.Column("P").Width = 8.50;
                sht.Column("Q").Width = 10;

                sht.Range("A" + Hrow).SetValue("Sl. No./EMP CODE");
                sht.Range("B" + Hrow).SetValue("Name of the employee");
                sht.Range("C" + Hrow).SetValue("Father's name");
                sht.Range("D" + Hrow).SetValue("Whether he has completed 15 years of age at the beginning of the accounting year");
                sht.Range("E" + Hrow).SetValue("Designation");
                sht.Range("F" + Hrow).SetValue("No. of days worked in the year");
                sht.Range("G" + Hrow).SetValue("Total salary or wage in respect of the accounting year");
                sht.Range("H" + Hrow).SetValue("Amount of bonus payable under section 10 or section 11, as the case may be");
                sht.Range("I" + Hrow).SetValue("Puja bonus or other customary bonus paid during the accounting year");
                sht.Range("J" + Hrow).SetValue("Interim bonus or bonus paid in advance");
                sht.Range("K" + Hrow).SetValue("Amount of Income-tax deduced");
                sht.Range("L" + Hrow).SetValue("Deduction on account of financial loss, if any, caused by misconduct of the employee");
                sht.Range("M" + Hrow).SetValue("Total sum deducted under Columns 9, 10, 10A and 11");
                sht.Range("N" + Hrow).SetValue("Net amount payable (Column 8 minus Column 12)");
                sht.Range("O" + Hrow).SetValue("Amount actually paid");
                sht.Range("P" + Hrow).SetValue("Date on which paid");
                sht.Range("Q" + Hrow).SetValue("Signature/Thumb impression of the employee");

                sht.Range("A" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("B" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("C" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("D" + Hrow).Style.Alignment.WrapText = true;
                sht.Range("E" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("F" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("G" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("H" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("I" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("J" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("K" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("L" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("M" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("N" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("O" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("P" + Hrow).Style.Alignment.SetWrapText();
                sht.Range("Q" + Hrow).Style.Alignment.SetWrapText();

                sht.Range("A" + Hrow).Style.Font.SetBold();
                sht.Range("B" + Hrow).Style.Font.SetBold();
                sht.Range("C" + Hrow).Style.Font.SetBold();
                sht.Range("D" + Hrow).Style.Font.SetBold();
                sht.Range("E" + Hrow).Style.Font.SetBold();
                sht.Range("F" + Hrow).Style.Font.SetBold();
                sht.Range("G" + Hrow).Style.Font.SetBold();
                sht.Range("H" + Hrow).Style.Font.SetBold();
                sht.Range("I" + Hrow).Style.Font.SetBold();
                sht.Range("J" + Hrow).Style.Font.SetBold();
                sht.Range("K" + Hrow).Style.Font.SetBold();
                sht.Range("L" + Hrow).Style.Font.SetBold();
                sht.Range("M" + Hrow).Style.Font.SetBold();
                sht.Range("N" + Hrow).Style.Font.SetBold();
                sht.Range("O" + Hrow).Style.Font.SetBold();
                sht.Range("P" + Hrow).Style.Font.SetBold();
                sht.Range("Q" + Hrow).Style.Font.SetBold();

                Hrow = 10;

                sht.Range("A" + Hrow).SetValue("1");
                sht.Range("B" + Hrow).SetValue("2");
                sht.Range("C" + Hrow).SetValue("3");
                sht.Range("D" + Hrow).SetValue("4");
                sht.Range("E" + Hrow).SetValue("5");
                sht.Range("F" + Hrow).SetValue("6");
                sht.Range("G" + Hrow).SetValue("7");
                sht.Range("H" + Hrow).SetValue("8");
                sht.Range("I" + Hrow).SetValue("9");
                sht.Range("J" + Hrow).SetValue("10");
                sht.Range("K" + Hrow).SetValue("10A");
                sht.Range("L" + Hrow).SetValue("11");
                sht.Range("M" + Hrow).SetValue("12");
                sht.Range("N" + Hrow).SetValue("13");
                sht.Range("O" + Hrow).SetValue("14");
                sht.Range("P" + Hrow).SetValue("15");
                sht.Range("Q" + Hrow).SetValue("16");

                sht.Range("A" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("K" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("M" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("O" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("P" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("A" + Hrow).Style.Font.SetBold();
                sht.Range("B" + Hrow).Style.Font.SetBold();
                sht.Range("C" + Hrow).Style.Font.SetBold();
                sht.Range("D" + Hrow).Style.Font.SetBold();
                sht.Range("E" + Hrow).Style.Font.SetBold();
                sht.Range("F" + Hrow).Style.Font.SetBold();
                sht.Range("G" + Hrow).Style.Font.SetBold();
                sht.Range("H" + Hrow).Style.Font.SetBold();
                sht.Range("I" + Hrow).Style.Font.SetBold();
                sht.Range("J" + Hrow).Style.Font.SetBold();
                sht.Range("K" + Hrow).Style.Font.SetBold();
                sht.Range("L" + Hrow).Style.Font.SetBold();
                sht.Range("M" + Hrow).Style.Font.SetBold();
                sht.Range("N" + Hrow).Style.Font.SetBold();
                sht.Range("O" + Hrow).Style.Font.SetBold();
                sht.Range("P" + Hrow).Style.Font.SetBold();
                sht.Range("Q" + Hrow).Style.Font.SetBold();

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["FatherName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["NoOFDays"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["BonusMinRate"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Bonus"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["EmpStatus"]);

                    sht.Range("A" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("B" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("C" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("E" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("F" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("G" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("O" + Hrow).Style.Alignment.SetWrapText();
                    sht.Range("P" + Hrow).Style.Alignment.SetWrapText();
                    row = row + 1;
                }


                //************
                //sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range(sht.Cell(8, 1), sht.Cell(row, 17)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("BonusReport" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsalary", "alert('No Records found for this combination !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BONUSDETAIL()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_GETBONUSDETAIL]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("BonusReport");

                string status = "BONUS" + txtfromdate.Text + " TO" + txttodate.Text;
                sht.Range("A1").SetValue(status);

                sht.Range("A1", "R1").Merge();
                sht.Range("A1", "R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                int Hrow = 2;

                sht.Range("A" + Hrow).SetValue("Card No.");
                sht.Range("B" + Hrow).SetValue("Name");
                sht.Range("C" + Hrow).SetValue("Father Name");
                sht.Range("D" + Hrow).SetValue("Category Name");
                sht.Range("E" + Hrow).SetValue("Type");
                sht.Range("F" + Hrow).SetValue("Apr");
                sht.Range("G" + Hrow).SetValue("May");
                sht.Range("H" + Hrow).SetValue("Jun");
                sht.Range("I" + Hrow).SetValue("Jul");
                sht.Range("J" + Hrow).SetValue("Aug");
                sht.Range("K" + Hrow).SetValue("Sep");
                sht.Range("L" + Hrow).SetValue("Oct");
                sht.Range("M" + Hrow).SetValue("Nov");
                sht.Range("N" + Hrow).SetValue("Dec");
                sht.Range("O" + Hrow).SetValue("Jan");
                sht.Range("P" + Hrow).SetValue("Feb");
                sht.Range("Q" + Hrow).SetValue("Mar");
                sht.Range("R" + Hrow).SetValue("Total");

                int row = Hrow + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["FatherName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["TYPE"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Apr"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["May"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Jun"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Jul"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Aug"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Sep"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Oct"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Nov"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Dec"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Jan"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Feb"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["Mar"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["Total"]);
                    row = row + 1;
                }


                //************
                sht.Columns(1, 25).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, 18)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("BonusReport" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsalary", "alert('No Records found for this combination !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //protected void DDreporttype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    switch (DDreporttype.SelectedValue)
    //    {
    //        case "2":
    //            trtodate.Visible = false;
    //            Trfrom.Visible = false;
    //            break;
    //        default:
    //            trtodate.Visible = true;
    //            Trfrom.Visible = true;
    //            break;
    //    }
    //}
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
}