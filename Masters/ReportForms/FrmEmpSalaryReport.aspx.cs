using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmEmpSalaryReport : System.Web.UI.Page
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
                           select UnitsId,UnitName from Units order by UnitName
                            select Month_Id,Month_Name from MonthTable 
                           select Year,Year from YearData";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessUnit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 3, true, "--Plz Select--");

            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            if (DDProcessUnit.Items.Count > 0)
            {
                DDProcessUnit.SelectedIndex = 1;
            }

            if (DDMonth.Items.Count > 0)
            {
                DDMonth.SelectedValue = currentMonth;
            }
            if (DDYear.Items.Count > 0)
            {
                DDYear.SelectedValue = currentYear;
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        Finishinghissab_ExcelExport();
    }

    private void Finishinghissab_ExcelExport()
    {
        lblMessage.Text = "";
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        string strCondition = "And PRM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions     

        if (DDProcessUnit.SelectedIndex > 0)
        {
            strCondition = strCondition + " And PRM.UnitId=" + DDProcessUnit.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();

        //End Conditions
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@FromDate", DDMonth.SelectedValue);
        param[1] = new SqlParameter("@Todate", DDYear.SelectedValue);
        param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        param[2].Direction = ParameterDirection.Output;
        param[3] = new SqlParameter("@Where", strCondition);

        ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "GetWeavingEmpSalaryReportData", param);
        tran.Commit();
        lblMessage.Text = param[2].Value.ToString();
        //DataView dv = new DataView(ds1.Tables[0]);
        //dv.Sort = "EmpCode";
        //ds.Tables.Add(dv.ToTable());

        if (lblMessage.Text == "")
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string Path = "";
                string label = "WEAVING SALARY";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("WeavingEmpSalary");
                //var sht2 = xapp.Worksheets.Add("Hide Unhide");
                //*************
                sht.Range("A1:Z1").Merge();
                sht.Range("A1:Z1").Style.Font.FontSize = 20;
                sht.Range("A1:Z1").Style.Font.Bold = true;
                sht.Range("A1:Z1").Style.Font.FontName = "Arial";
                sht.Range("A1:Z1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Z1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                //DateTime dt =DateTime.ParseExact(TxtFromDate.Text, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //string month = dt.ToString("MMM").ToUpper();
                //string Year = dt.Year.ToString();

                DateTime dt2 = new DateTime(1, Int32.Parse(DDMonth.SelectedValue), 1);
                string mm = dt2.ToString("MMM");
                string month = mm;
                string Year = DDYear.SelectedValue;

                label = label + " " + month + '-' + Year;

                // label = label + "-" + ("From Date " + TxtFromDate.Text + "  ToDate " + TxtToDate.Text + "");
                sht.Range("A1").SetValue(DDCompany.SelectedItem.Text + " " + label);
                sht.Row(1).Height = 21.75;
                //Header
                sht.Range("A2:Z2").Style.Font.FontSize = 10;
                sht.Range("A2:Z2").Style.Font.Bold = true;
                sht.Range("A2:Z2").Style.Font.FontName = "Arial";
                sht.Range("A2:Z2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:Z2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Z2").Style.Alignment.SetWrapText();

                sht.Range("A2:Z2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:Z2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:Z2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:Z2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // sht.Row(2).Height = 18.00;
                //
                sht.Range("A2").SetValue("PF No");
                sht.Range("B2").SetValue("J/No");
                //sht.Column(2).Width = 4.71;
                sht.Range("C2").SetValue("SLNo");
                //sht.Column(3).Width = 4.29;
                sht.Range("D2").SetValue("Emp Name");
                sht.Column(4).Width = 17.29;
                sht.Range("E2").SetValue("Father Name");
                sht.Column(5).Width = 19.71;
                sht.Range("F2").SetValue("Working Days");
                sht.Column(6).Width = 4.86;

                sht.Range("G2").SetValue("Working Hours");
                sht.Range("H2").SetValue("Min");
                sht.Range("I2").SetValue("Total Hour Con.To Min");
                sht.Range("J2").SetValue("Total Min");
                sht.Range("K2").SetValue("Minimum Wage Rate Per Min");
                sht.Range("L2").SetValue("Minimim Wages");
                sht.Range("M2").SetValue("Pcs");
                sht.Range("N2").SetValue("Rate Per Pcs");
                sht.Range("O2").SetValue("Earn Wages");
                sht.Range("P2").SetValue("Total Earn Wages");
                sht.Range("Q2").SetValue("Production Wage Per Min");
                sht.Range("R2").SetValue("Production Amt");
                sht.Range("S2").SetValue("Min Wages");
                sht.Range("T2").SetValue("Complete Pcs");
                sht.Range("U2").SetValue("Earn Wages");
                sht.Range("V2").SetValue("Total Earn Wage");
                sht.Range("W2").SetValue("Production Wage Per Min");
                sht.Range("X2").SetValue("Production Amt");
                sht.Range("Y2").SetValue("Minimum Wage");
                sht.Range("Z2").SetValue("Total Gross Wages");
                sht.Range("AA2").SetValue("EmpId");




                int Row = 3;
                int Row2 = 0;
                int SLNo = 0;
                double ProductionAmt = 0, MinimumWage = 0, MinimumWage2 = 0, ProductionAmt2 = 0, TempRate = 0;
                string JNo = "";

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + Row + ":AA" + Row).Style.Font.FontSize = 10;
                    sht.Range("A" + Row + ":AA" + Row).Style.Font.FontName = "Arial";


                    //sht.Range("A" + Row + ":N" + Row).Style.Font.FontSize = 11;
                    //sht.Range("J" + Row + ":N" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //sht.Range("A" + Row + ':' + "Z" + Row).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    sht.Range("A" + Row + ":AA" + Row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + Row + ":AA" + Row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + Row + ":AA" + Row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + Row + ":AA" + Row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                    sht.Range("AA" + Row).SetValue(ds1.Tables[0].Rows[i]["EmpId"]);
                    sht.Columns("AA").Hide();

                    sht.Range("A" + Row).SetValue(ds1.Tables[0].Rows[i]["EMPACNo"]);

                    SLNo = SLNo + 1;
                    sht.Range("C" + Row).SetValue(SLNo);
                    sht.Range("D" + Row).SetValue(ds1.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("E" + Row).SetValue(ds1.Tables[0].Rows[i]["FatherName"]);
                    sht.Range("F" + Row).SetValue(ds1.Tables[0].Rows[i]["WorkingDays"]);
                    // textBox.Text = d.ToString("F2").Split('.')[1];
                    sht.Range("G" + Row).SetValue(ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[0]);
                    sht.Range("H" + Row).SetValue(ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[1]);
                    sht.Range("I" + Row).FormulaA1 = "=G" + Row + '*' + 60 + '+' + ("$H$" + Row + "");

                    ////**********Start Get MinWagesRatePerMin 
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        sht.Range("K" + Row).SetValue(ds1.Tables[1].Rows[0]["MinWagesRatePerMin"]);
                    }
                    else
                    {
                        sht.Range("K" + Row).SetValue(0);
                    }

                    ////**********End Get MinWagesRatePerMin 

                    sht.Range("L" + Row).FormulaA1 = "=I" + Row + '*' + "$K$" + Row + "";

                    sht.Range("A" + Row + ':' + "C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row + ':' + "C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F" + Row + ':' + "Z" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + Row + ':' + "Z" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + Row + ':' + "Z" + Row).Style.Alignment.SetWrapText();

                    if (ds1.Tables[0].Rows[i]["JNo"].ToString() == "Singal")
                    {
                        sht.Range("B" + Row).SetValue(ds1.Tables[0].Rows[i]["JNo"]);

                        sht.Range("J" + Row).FormulaA1 = "=G" + Row + '*' + 60 + '+' + ("$H$" + Row + "");

                        sht.Range("M" + Row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                        sht.Range("N" + Row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);

                        sht.Range("O" + Row).FormulaA1 = "=M" + Row + '*' + "$N$" + Row + "";
                        sht.Range("P" + Row).SetValue(Math.Round(Convert.ToDouble(sht.Evaluate("M" + Row + '*' + "N" + Row)), 2, MidpointRounding.AwayFromZero));
                        sht.Range("P" + Row).FormulaA1 = "=M" + Row + '*' + "$N$" + Row + "";
                        if (ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[0] == "0" && ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[1] == "00")
                        {
                            sht.Range("Q" + Row).SetValue(0);
                        }
                        else
                        {
                            sht.Range("Q" + Row).FormulaA1 = "=O" + Row + '/' + "$J$" + Row + "";
                        }
                        sht.Range("R" + Row).FormulaA1 = "=Q" + Row + '*' + "$I$" + Row + "";

                        sht.Range("S" + Row).FormulaA1 = "=IF(R" + Row + '>' + "$L$" + Row + ",0,R" + Row + '-' + "$L$" + Row + ")";

                        sht.Range("T" + Row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);

                        sht.Range("U" + Row).FormulaA1 = "=T" + Row + '*' + "$N$" + Row + "";
                        sht.Range("V" + Row).SetValue(Math.Round(Convert.ToDouble(sht.Evaluate("T" + Row + '*' + "N" + Row)), 2, MidpointRounding.AwayFromZero));
                        sht.Range("V" + Row).FormulaA1 = "=T" + Row + '*' + "$N$" + Row + "";

                        if (ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[0] == "0" && ds1.Tables[0].Rows[i]["WorkingHours"].ToString().Split('.')[1] == "00")
                        {
                            sht.Range("W" + Row).SetValue(0);
                        }
                        else
                        {
                            sht.Range("W" + Row).FormulaA1 = "=U" + Row + '/' + "$J$" + Row + "";
                        }


                        sht.Range("X" + Row).FormulaA1 = "=W" + Row + '*' + "$I$" + Row + "";
                        ProductionAmt2 = Convert.ToDouble(sht.Evaluate("X" + Row));

                        sht.Range("Y" + Row).FormulaA1 = "=IF(R" + Row + '>' + "$L$" + Row + ",0,R" + Row + '-' + "$L$" + Row + ")";
                        sht.Range("Z" + Row).FormulaA1 = "=X" + Row + '-' + "$Y$" + Row + "";
                    }
                    else if (ds1.Tables[0].Rows[i]["JNo"].ToString() != JNo || ds1.Tables[0].Rows[i]["JNo"].ToString() == JNo)
                    {
                        sht.Range("M" + Row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                        sht.Range("N" + Row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                        sht.Range("O" + Row).FormulaA1 = "=M" + Row + '*' + "$N$" + Row + "";
                        sht.Range("T" + Row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                        sht.Range("U" + Row).FormulaA1 = "=T" + Row + '*' + "$N$" + Row + "";

                        if (ds1.Tables[0].Rows[i]["JNo"].ToString() == JNo)
                        {
                            Row2 = Row - 1;

                            sht.Range("B" + Row2 + ':' + "B" + Row).Merge();
                            sht.Range("J" + Row2 + ':' + "J" + Row).Merge();
                            if (ds1.Tables[0].Rows[i]["JNo"].ToString() == JNo && Convert.ToDouble(ds1.Tables[0].Rows[i]["Rate"].ToString()) == TempRate)
                            {
                                sht.Range("M" + Row2 + ':' + "M" + Row).Merge();
                                sht.Range("N" + Row2 + ':' + "N" + Row).Merge();
                                sht.Range("O" + Row2 + ':' + "O" + Row).Merge();
                                sht.Range("T" + Row2 + ':' + "T" + Row).Merge();
                                sht.Range("U" + Row2 + ':' + "U" + Row).Merge();
                            }

                            sht.Range("P" + Row2 + ':' + "P" + Row).Merge();
                            sht.Range("Q" + Row2 + ':' + "Q" + Row).Merge();
                            sht.Range("V" + Row2 + ':' + "V" + Row).Merge();

                            sht.Range("W" + Row2 + ':' + "W" + Row).Merge();

                            sht.Range("J" + Row2).FormulaA1 = "=I" + Row2 + '+' + "$I$" + Row + "";

                            if (ds1.Tables[0].Rows[i]["JNo"].ToString() == JNo && Convert.ToDouble(ds1.Tables[0].Rows[i]["Rate"].ToString()) == TempRate)
                            {

                                sht.Range("P" + Row2).FormulaA1 = "=M" + Row2 + '*' + "$N$" + Row2 + "";
                                //sht.Range("P" + Row2).Style.NumberFormat.Format = "#,##0.00";                               

                                //sht.Range("P" + Row2).SetValue(sht.Evaluate("O" + Row2));                                
                                //sht.Range("P" + Row2).SetValue(Math.Round(Convert.ToDouble(sht.Evaluate("P" + Row2)), 2, MidpointRounding.AwayFromZero));
                                //sht.Range("V" + Row2).SetValue(sht.Evaluate("U" + Row2));
                                sht.Range("V" + Row2).FormulaA1 = "=T" + Row2 + '*' + "$N$" + Row2 + "";
                            }
                            else
                            {
                                sht.Range("P" + Row2).FormulaA1 = "=O" + Row2 + '+' + "$O$" + Row + "";
                                //sht.Range("P" + Row2).SetValue(Math.Round(Convert.ToDouble(sht.Evaluate("P" + Row2)), 2, MidpointRounding.AwayFromZero));

                                sht.Range("V" + Row2).FormulaA1 = "=U" + Row2 + '+' + "$U$" + Row + "";
                                //sht.Range("V" + Row2).SetValue(Math.Round(Convert.ToDouble(sht.Evaluate("V" + Row2)), 2, MidpointRounding.AwayFromZero));
                            }

                            sht.Range("Q" + Row2).FormulaA1 = "=P" + Row2 + '/' + "$J$" + Row2 + "";
                            sht.Range("W" + Row2).FormulaA1 = "=V" + Row2 + '/' + "$J$" + Row2 + "";
                            sht.Range("R" + Row2).FormulaA1 = "=Q" + Row2 + '*' + "$I$" + Row2 + "";
                            sht.Range("S" + Row2).FormulaA1 = "=IF(R" + Row2 + '>' + "$L$" + Row2 + ",0,R" + Row2 + '-' + "$L$" + Row2 + ")";
                            sht.Range("X" + Row2).FormulaA1 = "=W" + Row2 + '*' + "$I$" + Row2 + "";
                            sht.Range("Y" + Row2).FormulaA1 = "=IF(R" + Row2 + '>' + "$L$" + Row2 + ",0,R" + Row2 + '-' + "$L$" + Row2 + ")";
                            sht.Range("Z" + Row2).FormulaA1 = "=X" + Row2 + '-' + "$Y$" + Row2 + "";

                        }

                        JNo = ds1.Tables[0].Rows[i]["JNo"].ToString();
                        TempRate = Convert.ToDouble(ds1.Tables[0].Rows[i]["Rate"].ToString());

                        sht.Range("B" + Row).SetValue(ds1.Tables[0].Rows[i]["JNo"]);

                        sht.Range("J" + Row).FormulaA1 = "=I" + Row + '+' + "$I$" + Row2 + "";
                        sht.Range("Q" + Row).FormulaA1 = "=P" + Row + '/' + "$J$" + Row + "";
                        sht.Range("R" + Row).FormulaA1 = "=Q" + Row2 + '*' + "$I$" + Row + "";
                        sht.Range("S" + Row).FormulaA1 = "=IF(R" + Row + '>' + "$L$" + Row + ",0,R" + Row + '-' + "$L$" + Row + ")";
                        sht.Range("W" + Row).FormulaA1 = "=V" + Row + '/' + "$J$" + Row + "";

                        sht.Range("X" + Row).FormulaA1 = "=W" + Row2 + '*' + "$I$" + Row + "";
                        ProductionAmt2 = Convert.ToDouble(sht.Evaluate("X" + Row));

                        sht.Range("Y" + Row).FormulaA1 = "=IF(R" + Row + '>' + "$L$" + Row + ",0,R" + Row + '-' + "$L$" + Row + ")";
                        sht.Range("Z" + Row).FormulaA1 = "=X" + Row + '-' + "$Y$" + Row + "";
                    }

                    Row = Row + 1;
                }
                //********          

                ////**********
                //sht.Columns(1, 20).AdjustToContents();
                //**************Save
                //******SAVE FILE
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingEmpSalaryReport_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }



    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDMonth) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDYear) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
    }
}