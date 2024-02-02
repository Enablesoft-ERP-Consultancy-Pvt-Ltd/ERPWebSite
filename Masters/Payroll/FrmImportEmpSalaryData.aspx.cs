using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Drawing;
using System.IO;
using System.Text.RegularExpressions;

public partial class Masters_Payroll_FrmImportEmpSalaryData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txtcompdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                                       select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessUnit, ds, 1, true, "--Plz Select--");
            if (DDProcessUnit.Items.Count > 0)
            {
                DDProcessUnit.SelectedIndex = 1;
            }
        }
    }
    protected void fillReceiveNo()
    {
        //        string str = @"select distinct AE.ReceiveNo,AE.ReceiveNo+' / '+cast(left(datename(month,dateadd(month, DATEPART(MONTH, AE.AttendanceDate) - 1, 0)),3) as varchar)+'-'+cast(DATEPART(YEAR, AE.AttendanceDate) as varchar) as ReceiveName 
        //                      from AttendanceEmp AE where AE.AttendanceDate>='" + txtfromdate.Text + "'";
        string str = @"select distinct ESR.ReceiptNo, ESR.ReceiptNo+ ' / '+cast(left(datename(month,dateadd(month, ESR.EmpSalaryMonth - 1, 0)),3) as varchar)+'-'+cast(ESR.EmpSalaryYear as varchar) as ReceiveName 
                       from EmpSalaryReportData ESR where UnitId='" + DDCompanyUnit.SelectedValue + "'";
        UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
    }
    protected void bindCompanyUnit()
    {
        string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                                       select UnitsId,UnitName from Units order by UnitName";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //CommanFunction.FillComboWithDS(DDCompany, ds, 0);
        UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "");
        UtilityModule.ConditionalComboFillWithDS(ref DDCompanyUnit, ds, 1, true, "--Plz Select--");
        //if (DDCompanyUnit.Items.Count > 0)
        //{
        //    DDCompanyUnit.SelectedIndex = 1;
        //}
    }
    protected void DDCompanyUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillReceiveNo();
    }
    protected void chkDelete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDelete.Checked == true)
        {
            bindCompanyUnit();
            TDreceiveno.Visible = true;
            TDDeleteBtn.Visible = true;
            TDCompanyName.Visible = true;
            TDCompanyUnit.Visible = true;
        }
        else
        {
            TDreceiveno.Visible = false;
            TDDeleteBtn.Visible = false;
            TDCompanyName.Visible = false;
            TDCompanyUnit.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDreceiveNo, "", true, "--Plz Select--");
            UtilityModule.ConditionalComboFill(ref DDCompanyUnit, "", true, "--Plz Select--");
        }
    }
    //protected void txtfromdate_TextChanged(object sender, EventArgs e)
    //{
    //    fillReceiveNo();
    //}

    protected void btnimport_Click(object sender, EventArgs e)
    {
        string StrE2 = "";
        string PDate = "";
        string PMonth = "";
        string PYear = "";

        string StrA3 = "";
        string SEmpCode = "";
        string JNo = "";
        double rate = 0.00;

        string Attendate = "";

        double R = 0.00;
        double L = 0.00;
        double MinWages = 0.00;
        double MinimumWages2 = 0.00;
        int TotalMin = 0;
        double TotalEarnWages = 0.00;


        lblmsg.Text = "";
        //********************************
        if (fileupload.HasFile)
        {
            //***********check File type
            if (Path.GetExtension(fileupload.FileName) != ".xlsx" && Path.GetExtension(fileupload.FileName) != ".xls")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "al4", "alert('Please select valid .xls or .xlsx file')", true);
                return;
            }
            //***********
            DataTable dt = new DataTable();
            dt.Columns.Add("PFNo", typeof(string));
            dt.Columns.Add("JNo", typeof(string));
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("EmpName", typeof(string));
            dt.Columns.Add("FatherName", typeof(string));
            dt.Columns.Add("WorkingDays", typeof(int));
            dt.Columns.Add("WorkingHours", typeof(int));
            dt.Columns.Add("WorkingMin", typeof(int));
            dt.Columns.Add("TotalHourConToMin", typeof(int));
            dt.Columns.Add("TotalMin", typeof(int));
            dt.Columns.Add("MinWagesRatePerMin", typeof(decimal));
            dt.Columns.Add("MinimumWages", typeof(decimal));
            dt.Columns.Add("Pcs", typeof(decimal));
            dt.Columns.Add("RatePerPcs", typeof(decimal));
            dt.Columns.Add("EarnWages", typeof(decimal));
            dt.Columns.Add("TotalEarnWages", typeof(decimal));
            dt.Columns.Add("ProductionWagesPerMin", typeof(decimal));
            dt.Columns.Add("ProductionOnAmt", typeof(decimal));
            dt.Columns.Add("MinWages", typeof(decimal));
            dt.Columns.Add("CompletePcs", typeof(decimal));
            dt.Columns.Add("EarnWages2", typeof(decimal));
            dt.Columns.Add("TotalEarnWages2", typeof(decimal));
            dt.Columns.Add("ProductionWagesPerMin2", typeof(decimal));
            dt.Columns.Add("ProductionOnAmt2", typeof(decimal));
            dt.Columns.Add("MinimumWages2", typeof(decimal));
            dt.Columns.Add("TotalGrossWages", typeof(decimal));
            dt.Columns.Add("EmpId", typeof(int));
            dt.Columns.Add("EmpSalaryMonth", typeof(int));
            dt.Columns.Add("EmpSalaryYear", typeof(int));




            if (!Directory.Exists(Server.MapPath("~/WeavingEmpSalarySheet/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/WeavingEmpSalarySheet/"));
            }

            string Fileextension = Path.GetExtension(fileupload.FileName);

            string filename1 = UtilityModule.validateFilename("WeavingEmpSalarySheet_" + DateTime.Now + "." + Fileextension);
            fileupload.SaveAs(Server.MapPath("~/WeavingEmpSalarySheet/" + filename1));
            string filename = Server.MapPath("~/WeavingEmpSalarySheet/" + filename1);

            using (var document = SpreadsheetDocument.Open(filename, true))
            {
                try
                {
                    WorkbookPart wbPart = document.WorkbookPart;
                    Sheet sheet = (Sheet)wbPart.Workbook.Sheets.FirstChild;
                    WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);

                    //string PDate = Regex.Match(wsp.Readcell("E2").Trim(), @"\d+").Value;
                    StrE2 = wsp.Readcell("A1").Trim();
                    if (StrE2 != "")
                    {
                        string s = StrE2.ToString();
                        string[] parts = s.Split('-');

                        PMonth = parts[0].Substring(parts[0].Length - 3);
                        PYear = parts[1];
                        PMonth = Convert.ToString(DateTime.Parse(PMonth + " 01, 1900").Month);
                        //DateTime dt2 = new DateTime(1, Int32.Parse(PMonth), 1);
                        //PMonth = dt2.ToString();
                        // Attendate = mm + "-" + PYear;
                    }


                    int count = 0;
                    for (int rNo = 3; rNo < 5000; rNo++)
                    {

                        if (wsp.Readcell("AA" + rNo).Trim() == "")
                        {
                            //lblmsg.Text = "Excel sheet has no data to import";
                            break;
                        }
                        else
                        {
                            count = count + 1;
                            StrA3 = wsp.Readcell("D" + rNo).Trim();
                            if (StrA3.Trim() != "")
                            {


                                if (wsp.Readcell("B" + (rNo)).Trim() == "Singal")
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["PFNo"] = wsp.Readcell("A" + (rNo)).Trim();
                                    dr["JNo"] = wsp.Readcell("B" + (rNo)).Trim();

                                    dr["SLNo"] = wsp.Readcell("C" + (rNo)).Trim();
                                    dr["EmpName"] = wsp.Readcell("D" + (rNo)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo)).Trim();
                                    //dr["FatherName"] = "NULL";
                                    dr["WorkingDays"] = wsp.Readcell("F" + (rNo)).Trim();
                                    dr["WorkingHours"] = wsp.Readcell("G" + (rNo)).Trim();
                                    dr["WorkingMin"] = wsp.Readcell("H" + (rNo)).Trim();
                                    dr["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    dr["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    dr["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo)).Trim();
                                    dr["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());

                                    dr["Pcs"] = wsp.Readcell("M" + (rNo)).Trim();
                                    dr["RatePerPcs"] = wsp.Readcell("N" + (rNo)).Trim();
                                    dr["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    if (wsp.Readcell("G" + (rNo)).Trim() == "0" && wsp.Readcell("H" + (rNo)).Trim() == "00")
                                    {
                                        dr["ProductionWagesPerMin"] = 0;
                                        dr["ProductionOnAmt"] = 0;
                                        R = 0;
                                        L = 0;
                                    }
                                    else
                                    {
                                        dr["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())));
                                        dr["ProductionOnAmt"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                        R = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                        L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());
                                    }

                                   

                                   

                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;

                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);

                                    }
                                    dr["MinWages"] = MinWages;

                                    dr["CompletePcs"] = wsp.Readcell("T" + (rNo)).Trim();

                                    dr["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());

                                    if (wsp.Readcell("G" + (rNo)).Trim() == "0" && wsp.Readcell("H" + (rNo)).Trim() == "00")
                                    {
                                        dr["ProductionWagesPerMin2"] = 0;
                                        dr["ProductionOnAmt2"] = 0;
                                        dr["MinimumWages2"] = MinimumWages2;
                                        dr["TotalGrossWages"] = 0;
                                    }
                                    else
                                    {
                                        dr["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())));
                                        dr["ProductionOnAmt2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                        
                                        dr["TotalGrossWages"] = Convert.ToDouble(Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()))) - Convert.ToDouble(MinimumWages2);
                                    }

                                   
                                   
                                  

                                    dr["EmpId"] = wsp.Readcell("AA" + (rNo)).Trim();
                                    dr["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr);
                                }
                                else if (wsp.Readcell("B" + (rNo)).Trim() != JNo)
                                {
                                    JNo = wsp.Readcell("B" + (rNo)).Trim();
                                    rate = Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());

                                    DataRow dr = dt.NewRow();
                                    dr["PFNo"] = wsp.Readcell("A" + (rNo)).Trim();

                                    dr["JNo"] = wsp.Readcell("B" + (rNo)).Trim();


                                    dr["SLNo"] = wsp.Readcell("C" + (rNo)).Trim();
                                    dr["EmpName"] = wsp.Readcell("D" + (rNo)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo)).Trim();
                                    //dr["FatherName"] = "NULL";
                                    dr["WorkingDays"] = wsp.Readcell("F" + (rNo)).Trim();
                                    dr["WorkingHours"] = wsp.Readcell("G" + (rNo)).Trim();
                                    dr["WorkingMin"] = wsp.Readcell("H" + (rNo)).Trim();
                                    dr["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    //dr["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    TotalMin = (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) + (Convert.ToInt32(wsp.Readcell("G" + (rNo + 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo + 1)).Trim()));
                                    dr["TotalMin"] = TotalMin;
                                    //dr["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    dr["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo)).Trim();
                                    dr["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());

                                    dr["Pcs"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim());
                                    dr["RatePerPcs"] = wsp.Readcell("N" + (rNo)).Trim();
                                    dr["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt"] = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));


                                    R = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());
                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;
                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);

                                    }
                                    dr["MinWages"] = MinWages;

                                    dr["CompletePcs"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim());

                                    dr["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());

                                    dr["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt2"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));


                                    dr["MinimumWages2"] = MinimumWages2;
                                    dr["TotalGrossWages"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) - Convert.ToDouble(MinimumWages2);


                                    dr["EmpId"] = wsp.Readcell("AA" + (rNo)).Trim();
                                    dr["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr);


                                }
                                else if (wsp.Readcell("B" + (rNo)).Trim() == JNo && Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) == rate)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["PFNo"] = wsp.Readcell("A" + (rNo)).Trim();

                                    dr["JNo"] = wsp.Readcell("B" + (rNo - 1)).Trim();


                                    dr["SLNo"] = wsp.Readcell("C" + (rNo)).Trim();
                                    dr["EmpName"] = wsp.Readcell("D" + (rNo)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo)).Trim();
                                    //dr["FatherName"] = "NULL";
                                    dr["WorkingDays"] = wsp.Readcell("F" + (rNo)).Trim();
                                    dr["WorkingHours"] = wsp.Readcell("G" + (rNo)).Trim();
                                    dr["WorkingMin"] = wsp.Readcell("H" + (rNo)).Trim();
                                    dr["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    //dr["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    TotalMin = (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) + (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    dr["TotalMin"] = TotalMin;
                                    dr["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo)).Trim();
                                    dr["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());


                                    dr["Pcs"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim());
                                    dr["RatePerPcs"] = wsp.Readcell("N" + (rNo - 1)).Trim();
                                    dr["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    dr["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    //dr["ProductionWagesPerMin"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr["ProductionOnAmt"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    dr["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt"] = (Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));



                                    R = (Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());
                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;

                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);

                                    }
                                    dr["MinWages"] = MinWages;

                                    dr["CompletePcs"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim());

                                    dr["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    dr["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());

                                    dr["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt2"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));

                                    dr["MinimumWages2"] = MinimumWages2;
                                    dr["TotalGrossWages"] = (Convert.ToDouble(Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()))) - Convert.ToDouble(MinimumWages2);


                                    dr["EmpId"] = wsp.Readcell("AA" + (rNo)).Trim();
                                    dr["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr);

                                }
                                else if (wsp.Readcell("B" + (rNo)).Trim() == JNo && Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) != rate)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["PFNo"] = wsp.Readcell("A" + (rNo)).Trim();

                                    dr["JNo"] = wsp.Readcell("B" + (rNo)).Trim();

                                    dr["SLNo"] = wsp.Readcell("C" + (rNo)).Trim();
                                    dr["EmpName"] = wsp.Readcell("D" + (rNo)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo)).Trim();
                                    //dr["FatherName"] = "NULL";
                                    dr["WorkingDays"] = wsp.Readcell("F" + (rNo)).Trim();
                                    dr["WorkingHours"] = wsp.Readcell("G" + (rNo)).Trim();
                                    dr["WorkingMin"] = wsp.Readcell("H" + (rNo)).Trim();
                                    dr["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    //dr["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    TotalMin = (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) + (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    dr["TotalMin"] = TotalMin;
                                    dr["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo)).Trim();
                                    dr["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());

                                    dr["Pcs"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim());
                                    dr["RatePerPcs"] = wsp.Readcell("N" + (rNo)).Trim();
                                    dr["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    //dr["ProductionWagesPerMin"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr["ProductionOnAmt"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));

                                    dr["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt"] = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));

                                    //R = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    R = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());
                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;
                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);
                                    }
                                    dr["MinWages"] = MinWages;
                                    dr["CompletePcs"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim());
                                    dr["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());

                                    dr["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr["ProductionOnAmt2"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    dr["MinimumWages2"] = MinimumWages2;
                                    dr["TotalGrossWages"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) - Convert.ToDouble(MinimumWages2);

                                    //dr["ProductionWagesPerMin2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr["ProductionOnAmt2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr["MinimumWages2"] = MinimumWages2;                                    
                                    //dr["TotalGrossWages"] = Convert.ToDouble(Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()))) - Convert.ToDouble(MinimumWages2);

                                    dr["EmpId"] = wsp.Readcell("AA" + (rNo)).Trim();
                                    dr["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr);


                                    DataRow dr2 = dt.NewRow();
                                    dr2["PFNo"] = wsp.Readcell("A" + (rNo - 1)).Trim();

                                    dr2["JNo"] = wsp.Readcell("B" + (rNo - 1)).Trim();

                                    dr2["SLNo"] = wsp.Readcell("C" + (rNo - 1)).Trim();
                                    dr2["EmpName"] = wsp.Readcell("D" + (rNo - 1)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo - 1)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo - 1)).Trim();
                                    //dr2["FatherName"] = "NULL";
                                    dr2["WorkingDays"] = wsp.Readcell("F" + (rNo - 1)).Trim();
                                    dr2["WorkingHours"] = wsp.Readcell("G" + (rNo - 1)).Trim();
                                    dr2["WorkingMin"] = wsp.Readcell("H" + (rNo - 1)).Trim();
                                    dr2["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim());
                                    //dr2["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim());
                                    TotalMin = (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) + (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    dr2["TotalMin"] = TotalMin;
                                    dr2["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo - 1)).Trim();
                                    dr2["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo - 1)).Trim());

                                    dr2["Pcs"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim());
                                    dr2["RatePerPcs"] = wsp.Readcell("N" + (rNo)).Trim();
                                    dr2["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr2["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    //dr2["ProductionWagesPerMin"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()));
                                    //dr2["ProductionOnAmt"] = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()));

                                    dr2["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr2["ProductionOnAmt"] = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));

                                    // R = Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()));
                                    R = (Convert.ToDouble(wsp.Readcell("M" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo - 1)).Trim());
                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;
                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);
                                    }
                                    dr2["MinWages"] = MinWages;
                                    dr2["CompletePcs"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim());
                                    dr2["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());
                                    dr2["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim());

                                    dr2["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin);
                                    dr2["ProductionOnAmt2"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    dr2["MinimumWages2"] = MinimumWages2;
                                    dr2["TotalGrossWages"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim())) - Convert.ToDouble(MinimumWages2);

                                    //dr2["ProductionWagesPerMin2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()));
                                    //dr2["ProductionOnAmt2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()));
                                    //dr2["MinimumWages2"] = MinimumWages2;
                                    //dr2["TotalGrossWages"] = Convert.ToDouble(Convert.ToDouble(wsp.Readcell("T" + (rNo)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo-1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo-1)).Trim()))) - Convert.ToDouble(MinimumWages2);

                                    dr2["EmpId"] = wsp.Readcell("AA" + (rNo - 1)).Trim();
                                    dr2["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr2["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr2);

                                    DataRow dr3 = dt.NewRow();
                                    dr3["PFNo"] = wsp.Readcell("A" + (rNo)).Trim();

                                    dr3["JNo"] = wsp.Readcell("B" + (rNo)).Trim();

                                    dr3["SLNo"] = wsp.Readcell("C" + (rNo)).Trim();
                                    dr3["EmpName"] = wsp.Readcell("D" + (rNo)).Trim();
                                    dr["FatherName"] = wsp.Readcell("E" + (rNo)).Trim() == "" ? "NULL" : wsp.Readcell("E" + (rNo)).Trim();
                                    //dr3["FatherName"] = "NULL";
                                    dr3["WorkingDays"] = wsp.Readcell("F" + (rNo)).Trim();
                                    dr3["WorkingHours"] = wsp.Readcell("G" + (rNo)).Trim();
                                    dr3["WorkingMin"] = wsp.Readcell("H" + (rNo)).Trim();
                                    dr3["TotalHourConToMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    //dr3["TotalMin"] = Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim());
                                    TotalMin = (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) + (Convert.ToInt32(wsp.Readcell("G" + (rNo - 1)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo - 1)).Trim()));
                                    dr3["TotalMin"] = TotalMin;
                                    dr3["MinWagesRatePerMin"] = wsp.Readcell("K" + (rNo)).Trim();
                                    dr3["MinimumWages"] = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());

                                    dr3["Pcs"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim());
                                    dr3["RatePerPcs"] = wsp.Readcell("N" + (rNo - 1)).Trim();
                                    dr3["EarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    dr3["TotalEarnWages"] = Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    //dr3["ProductionWagesPerMin"] = Convert.ToDouble(wsp.Readcell("M" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr3["ProductionOnAmt"] = Convert.ToDouble(wsp.Readcell("M" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));

                                    dr3["ProductionWagesPerMin"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin);
                                    dr3["ProductionOnAmt"] = (Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));

                                    //R = Convert.ToDouble(wsp.Readcell("M" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    R = (Convert.ToDouble(wsp.Readcell("M" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    L = Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * Convert.ToDouble(wsp.Readcell("K" + (rNo)).Trim());
                                    if (R > L)
                                    {
                                        MinWages = 0;
                                        MinimumWages2 = 0;
                                    }
                                    else
                                    {
                                        MinWages = Convert.ToDouble(R - L);
                                        MinimumWages2 = Convert.ToDouble(R - L);
                                    }
                                    dr3["MinWages"] = MinWages;
                                    dr3["CompletePcs"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim());
                                    dr3["EarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());
                                    dr3["TotalEarnWages2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim());

                                    dr3["ProductionWagesPerMin2"] = string.Format("{0:0,0.000000}", Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin);
                                    dr3["ProductionOnAmt2"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    dr3["MinimumWages2"] = MinimumWages2;
                                    dr3["TotalGrossWages"] = (Convert.ToDouble(wsp.Readcell("T" + (rNo - 1)).Trim()) * Convert.ToDouble(wsp.Readcell("N" + (rNo - 1)).Trim()) / TotalMin) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) - Convert.ToDouble(MinimumWages2);

                                    //dr3["ProductionWagesPerMin2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr3["ProductionOnAmt2"] = Convert.ToDouble(wsp.Readcell("T" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / Convert.ToDouble(Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()));
                                    //dr3["MinimumWages2"] = MinimumWages2;
                                    //dr3["TotalGrossWages"] = Convert.ToDouble(Convert.ToDouble(wsp.Readcell("T" + (rNo-1)).Trim()) / 2 * Convert.ToDouble(wsp.Readcell("N" + (rNo-1)).Trim()) / (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim())) * (Convert.ToInt32(wsp.Readcell("G" + (rNo)).Trim()) * 60 + Convert.ToInt32(wsp.Readcell("H" + (rNo)).Trim()))) - Convert.ToDouble(MinimumWages2);

                                    dr3["EmpId"] = wsp.Readcell("AA" + (rNo)).Trim();
                                    dr3["EmpSalaryMonth"] = Convert.ToInt32(PMonth);
                                    dr3["EmpSalaryYear"] = PYear;
                                    dt.Rows.Add(dr3);

                                }

                            }

                            //rNo = rNo + 1;

                        }
                    }

                    if (count == 0)
                    {
                        lblmsg.Text = "Excel sheet has no data to import.";
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            SqlTransaction Tran = con.BeginTransaction();
                            try
                            {

                                SqlParameter[] param = new SqlParameter[4];
                                param[0] = new SqlParameter("@dtrecords", dt);
                                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                                param[1].Direction = ParameterDirection.Output;
                                param[2] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                                param[3] = new SqlParameter("@UnitId", DDProcessUnit.SelectedIndex > 0 ? DDProcessUnit.SelectedValue : "0");
                                //*************************

                                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveEmpSalaryReportData", param);
                                lblmsg.Text = param[1].Value.ToString();
                                Tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                Tran.Rollback();
                                lblmsg.Text = ex.Message;

                            }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    lblmsg.Text = ex.Message;
                }
                finally
                {
                    document.Close();
                    document.Dispose();
                    //File.Delete(filename);
                }
            }
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (DDreceiveNo.SelectedIndex > 0)
        {
            lblmsg.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@ReceiptNo", DDreceiveNo.SelectedValue);
                param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                param[2] = new SqlParameter("@UserId", Session["varuserid"]);
                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                //*************************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteEmpSalaryData", param);
                lblmsg.Text = param[3].Value.ToString();
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmsg.Text = ex.Message;

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            lblmsg.Text = "Please Select ReceiptNo";
        }
    }

}