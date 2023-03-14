using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using System.Text;
public partial class Masters_ReportForms_frmHr_Esipfreport : System.Web.UI.Page
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
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName  
                    Select Distinct D.DepartmentId, D.DepartmentName 
                    From Department D(Nolock)
                    JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                    JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                    Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varCompanyId"] + @" 
                    Order By D.DepartmentName 
                    SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE order by Month_Id
                    SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA order by Year
                    select Designationid,Designation From HR_Designationmaster order by Designation
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--ALL--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDyear, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 4, true, "--Plz Select--");

            DDMonth.SelectedValue = System.DateTime.Now.Month.ToString();
            DDyear.SelectedValue = System.DateTime.Now.Year.ToString();
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 5, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
        }
    }
    protected void Btnpreview_Click(object sender, EventArgs e)
    {
        switch (DDreporttype.SelectedValue)
        {
            case "1":
                ESIFORMAT();
                break;
            case "2":
                PFFORMAT();
                break;
            default:
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No report found for this report type.')", true);
                break;
        }
    }
    protected void ESIFORMAT()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_ESIFORMAT]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@Wagescalculation", 0);
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

            if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ESIFORMAT");

            sht.Range("A1:E1").Merge();
            sht.Range("A1").SetValue("ESI FOR THE MONTH OF " + DDMonth.SelectedItem.Text + "," + DDyear.SelectedValue + "");
            sht.Range("A2:E2").Merge();
            sht.Range("A2").SetValue("Payroll Type -");
            sht.Range("A1:E2").Style.Font.SetBold();
            sht.Range("A1:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            int Hrow = 3;
            sht.Range("A" + Hrow).SetValue("EMP CODE");
            sht.Range("B" + Hrow).SetValue("IP NUMBER");
            sht.Range("C" + Hrow).SetValue("IP NAME");
            sht.Range("D" + Hrow).SetValue("No of Days for which wages paid/payable during the month");
            sht.Range("E" + Hrow).SetValue("Total Monthly Wages");

            sht.Range("A" + Hrow + ":E" + Hrow).Style.Font.SetBold();


            int row = Hrow + 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Esi_InsuranceNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["totalworkingdays"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Grosssal"]);
                row = row + 1;
            }
            //******SAVE FILE
            sht.Columns(1, 15).AdjustToContents();
            sht.Column("D").Width = 27.29;
            sht.Range("D" + Hrow).Style.Alignment.WrapText = true;
            sht.Range("D" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            using (var a = sht.Range("A1" + ":E" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //****************************
            string Path = "";
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ESIFORMAT(" + DDMonth.SelectedItem.Text + ")" + DateTime.Now + "." + Fileextension);
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
    protected void PFFORMAT()
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
            SqlCommand cmd = new SqlCommand("[PRO_HR_PFFORMAT]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@Wagescalculation", 0);
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

            if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PFFORMAT");

            sht.Range("A1:L1").Merge();
            sht.Range("A1").SetValue("M/S " + DDCompanyName.SelectedItem.Text);
            sht.Range("A2:L2").Merge();
            sht.Range("A2").SetValue("" + DDMonth.SelectedItem.Text + "," + DDyear.SelectedValue + "");
            sht.Range("A1:L2").Style.Font.SetBold();
            sht.Range("A1:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            int Hrow = 3;
            sht.Range("A" + Hrow).SetValue("Sr No.");
            sht.Range("B" + Hrow).SetValue("EMP CODE");
            sht.Range("C" + Hrow).SetValue("P.F UAN No.");
            sht.Range("D" + Hrow).SetValue("Name of the Employee");
            sht.Range("E" + Hrow).SetValue("Gross Wages");
            sht.Range("F" + Hrow).SetValue("P.F. Wages");
            sht.Range("G" + Hrow).SetValue("EPS Rules Wages");
            sht.Range("H" + Hrow).SetValue("A/C No. 1 EPF 3.67%");
            sht.Range("I" + Hrow).SetValue("A/C No. 10 EPS 8.33%");
            sht.Range("J" + Hrow).SetValue("A/C No. 1 PF 12%");
            sht.Range("K" + Hrow).SetValue("NCP");
            sht.Range("L" + Hrow).SetValue("Grand Total");

            sht.Columns("E").Width = 11.29;
            sht.Columns("F").Width = 14.57;
            sht.Columns("G").Width = 11;
            sht.Columns("H").Width = 10;
            sht.Columns("I").Width = 9.86;
            sht.Columns("J").Width = 9.57;
            sht.Columns("K").Width = 9.57;
            sht.Columns("L").Width = 11.00;

            sht.Range("A" + Hrow + ":L" + Hrow).Style.Font.SetBold();
            sht.Range("A" + Hrow + ":L" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + Hrow + ":L" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E" + Hrow + ":L" + Hrow).Style.Alignment.SetWrapText();



            int row = Hrow + 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(i + 1);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["PF_UanNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["grosssal"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["PFwages"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["PFwages"]);
                sht.Range("H" + row).FormulaA1 = "=J" + row + "-I" + row;
                sht.Range("I" + row).FormulaA1 = "=ROUND((G" + row + "*8.33%),)";

                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Earnamt"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["absent_value"]);
                sht.Range("L" + row).FormulaA1 = "=E" + row + "-J" + row;

                row = row + 1;
            }
            //**************'
            sht.Range("E" + row).FormulaA1 = "=SUM(E" + (Hrow + 1) + ":E" + (row - 1) + ")";
            sht.Range("F" + row).FormulaA1 = "=SUM(F" + (Hrow + 1) + ":F" + (row - 1) + ")";
            sht.Range("G" + row).FormulaA1 = "=SUM(G" + (Hrow + 1) + ":G" + (row - 1) + ")";
            sht.Range("H" + row).FormulaA1 = "=SUM(H" + (Hrow + 1) + ":H" + (row - 1) + ")";
            sht.Range("I" + row).FormulaA1 = "=SUM(I" + (Hrow + 1) + ":I" + (row - 1) + ")";
            sht.Range("J" + row).FormulaA1 = "=SUM(J" + (Hrow + 1) + ":J" + (row - 1) + ")";
            sht.Range("K" + row).FormulaA1 = "=SUM(K" + (Hrow + 1) + ":K" + (row - 1) + ")";
            sht.Range("L" + row).FormulaA1 = "=SUM(L" + (Hrow + 1) + ":L" + (row - 1) + ")";
            sht.Range("E" + row + ":L" + row).Style.Font.SetBold();
            //*********A/C No. Detail
            int ACNorow = row + 1;
            int acnorow_start = ACNorow;

            sht.Range("D" + ACNorow).SetValue("A/C No. 1");
            sht.Range("E" + ACNorow + ":F" + ACNorow).Merge();
            sht.Range("E" + ACNorow).SetValue("Employer + Employee Share");
            sht.Range("G" + ACNorow).SetValue("3.67%+12%");
            sht.Range("H" + ACNorow).FormulaA1 = "=J" + row + "+H" + row;

            ACNorow = ACNorow + 1;

            sht.Range("D" + ACNorow).SetValue("A/C No. 10");
            sht.Range("E" + ACNorow + ":F" + ACNorow).Merge();
            sht.Range("E" + ACNorow).SetValue("EPS");
            sht.Range("G" + ACNorow).SetValue("8.33%");
            sht.Range("H" + ACNorow).FormulaA1 = "=I" + row;

            ACNorow = ACNorow + 1;

            sht.Range("D" + ACNorow).SetValue("A/C No. 2");
            sht.Range("E" + ACNorow + ":F" + ACNorow).Merge();
            sht.Range("E" + ACNorow).SetValue("ADMIN CHARGES");
            sht.Range("G" + ACNorow).SetValue("0.50%");
            sht.Range("H" + ACNorow).FormulaA1 = "=ROUND((F" + row + "*G" + ACNorow + "),)";

            ACNorow = ACNorow + 1;

            sht.Range("D" + ACNorow).SetValue("A/C No. 21");
            sht.Range("E" + ACNorow + ":F" + ACNorow).Merge();
            sht.Range("E" + ACNorow).SetValue("EDLI");
            sht.Range("G" + ACNorow).SetValue("0.50%");
            sht.Range("H" + ACNorow).FormulaA1 = "=ROUND((F" + row + "*G" + ACNorow + "),)";

            ACNorow = ACNorow + 1;

            sht.Range("D" + ACNorow).SetValue("A/C No. 22");
            sht.Range("E" + ACNorow + ":F" + ACNorow).Merge();
            sht.Range("E" + ACNorow).SetValue("RAI");
            sht.Range("G" + ACNorow).SetValue("0.01%");
            sht.Range("H" + ACNorow).FormulaA1 = "=ROUND((F" + row + "*G" + ACNorow + "),)";

            ACNorow = ACNorow + 1;
            sht.Range("D" + ACNorow + ":G" + ACNorow).Merge();
            sht.Range("D" + ACNorow).SetValue("TOTAL AMOUNT");

            sht.Range("H" + ACNorow).FormulaA1 = "=SUM(H" + acnorow_start + ":H" + (ACNorow - 1) + ")";

            sht.Range("D" + acnorow_start + ":F" + ACNorow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D" + acnorow_start + ":H" + ACNorow).Style.Font.SetBold();
            sht.Range("G" + acnorow_start + ":G" + ACNorow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            using (var a = sht.Range("D" + acnorow_start + ":H" + ACNorow))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //******SAVE FILE
            sht.Columns(1, 4).AdjustToContents();


            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //****************************
            string Path = "";
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PFFORMAT(" + DDMonth.SelectedItem.Text + ")" + DateTime.Now + "." + Fileextension);
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
    protected void Form12_Monthwise(DataSet ds, string shiftstart, string Shiftend, string Lunchstart, string Lunchend)
    {
        if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("FORM12_MONTHWISE");
        int MHrow = 1;
        int Hrow = 0;
        int row = 0;
        //******************MAIN HEADERS
        sht.Range("I" + MHrow + ":BR" + MHrow).Merge();
        sht.Range("I" + MHrow).SetValue("प्रपत्र  सं. १२");
        sht.Range("I" + MHrow + ":BR" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("I" + MHrow + ":BR" + MHrow).Merge();
        sht.Range("I" + MHrow).SetValue("(नियम  ७८ )");
        sht.Range("I" + MHrow + ":BR" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("I" + MHrow + ":BR" + MHrow).Merge();
        sht.Range("I" + MHrow).SetValue("वयस्क कार्यकर्ताओ की पंजी जो     अधिनियम की धारा ६२ के  अधीन निर्धारित की गई है");
        sht.Range("I" + MHrow + ":BR" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        //कारखाने या विभाग का नाम  महीना वर्ष
        sht.Range("I" + MHrow + ":BR" + MHrow).Merge();
        sht.Range("I" + MHrow).SetValue("कारखाने या विभाग का नाम  " + DDCompanyName.SelectedItem.Text + "  महीना  " + DDMonth.SelectedItem.Text + " वर्ष  " + DDyear.SelectedValue + "");
        sht.Range("I" + MHrow + ":BR" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //कार्य प्रारम्भ करने का समय
        MHrow = MHrow + 1;
        sht.Row(MHrow).Height = 33;
        sht.Range("U" + MHrow).SetValue("कार्य प्रारम्भ करने का समय");
        sht.Range("U" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("U" + MHrow + ":Y" + MHrow + "").Merge();
        sht.Range("U" + MHrow + ":Y" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("U" + MHrow + ":Y" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //विभाग
        sht.Range("Z" + MHrow).SetValue("विभाग");
        sht.Range("Z" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("Z" + MHrow + ":AF" + MHrow + "").Merge();
        sht.Range("Z" + MHrow + ":AF" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("Z" + MHrow + ":AF" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //अवधि
        sht.Range("AG" + MHrow).SetValue("अवधि");
        sht.Range("AG" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("AG" + MHrow + ":AM" + MHrow + "").Merge();
        sht.Range("AG" + MHrow + ":AM" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("AG" + MHrow + ":AM" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //सोमवार से शुक्रवार तक
        MHrow = MHrow + 1;
        sht.Range("K" + MHrow).SetValue("सोमवार से शुक्रवार तक");
        sht.Range("K" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("K" + MHrow + ":T" + MHrow + "").Merge();

        sht.Range("U" + MHrow + ":Y" + (MHrow + 3) + "").Merge();
        sht.Range("U" + MHrow).SetValue(shiftstart);
        sht.Range("U" + MHrow + ":Y" + (MHrow + 3)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("U" + MHrow + ":Y" + (MHrow + 3)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("Z" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("Z" + MHrow).SetValue("से");
        sht.Range("AC" + MHrow + ":AF" + MHrow + "").Merge();
        sht.Range("AC" + MHrow).SetValue("तक");
        sht.Range("Z" + MHrow + ":AF" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("AG" + MHrow + ":AI" + MHrow + "").Merge();
        sht.Range("AG" + MHrow).SetValue("से");
        sht.Range("AJ" + MHrow + ":AM" + MHrow + "").Merge();
        sht.Range("AJ" + MHrow).SetValue("तक");


        sht.Range("AN" + MHrow + ":BL" + MHrow + "").Merge();
        sht.Range("AN" + MHrow).SetValue("समाप्ति (Completion) का समय");

        sht.Range("AG" + MHrow + ":BL" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //शनिवार
        MHrow = MHrow + 1;
        sht.Range("K" + MHrow).SetValue("शनिवार");
        sht.Range("K" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("K" + MHrow + ":T" + MHrow + "").Merge();

        sht.Range("Z" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("Z" + MHrow).SetValue(shiftstart);
        sht.Range("AC" + MHrow + ":AF" + MHrow + "").Merge();
        sht.Range("AC" + MHrow).SetValue(Lunchstart);
        sht.Range("Z" + MHrow + ":AF" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("AG" + MHrow + ":AI" + MHrow + "").Merge();
        sht.Range("AG" + MHrow).SetValue(Lunchend);
        sht.Range("AJ" + MHrow + ":AM" + MHrow + "").Merge();
        sht.Range("AJ" + MHrow).SetValue(Shiftend);


        sht.Range("AN" + MHrow + ":BL" + MHrow + "").Merge();
        sht.Range("AN" + MHrow).SetValue(Shiftend);

        sht.Range("AG" + MHrow + ":BL" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //रविवार
        MHrow = MHrow + 1;
        sht.Range("K" + MHrow).SetValue("रविवार");
        sht.Range("K" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("K" + MHrow + ":T" + MHrow + "").Merge();
        //रिले के आंक्शन  (रोटेशन) की पद्धत्ति
        MHrow = MHrow + 1;
        sht.Range("K" + MHrow).SetValue("रिले के आंक्शन  (रोटेशन) की पद्धत्ति");
        sht.Range("K" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("K" + MHrow + ":T" + MHrow + "").Merge();
        //*********Headers
        //sht.Range("A" + Hrow + ":K" + Hrow).Style.Font.FontName = "Arial Unicode MS";
        Hrow = MHrow + 2;
        sht.Range("A" + Hrow + ":F" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A" + Hrow + ":F" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("A" + Hrow).SetValue("क्रम  संख्या");
        sht.Range("A" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("A").Width = 08.43;

        sht.Range("B" + Hrow).SetValue("कर्मचारी का कोड़");
        sht.Range("B" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("B").Width = 10.46;

        sht.Range("C" + Hrow).SetValue("वयस्क  कार्यकर्त्ता का नाम");
        sht.Column("C").Width = 30.00;

        sht.Range("D" + Hrow).SetValue("पिता का नाम");
        sht.Column("D").Width = 30.00;

        sht.Range("E" + Hrow).SetValue("कार्य का स्वरुप");
        sht.Column("E").Width = 15.00;
        sht.Range("E" + Hrow).Style.Alignment.SetTextRotation(90);

        sht.Range("F" + Hrow).SetValue("विभाग");
        sht.Column("F").Width = 15.00;
        sht.Range("F" + Hrow).Style.Alignment.SetTextRotation(90);

        sht.Range("G" + (Hrow - 1) + ":H" + (Hrow - 1)).Merge();
        sht.Range("G" + (Hrow - 1)).SetValue("प्रपत्र ११ के अनुसार");
        sht.Range("G" + (Hrow - 1) + ":H" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("G" + Hrow).SetValue("समूह या रिले");
        sht.Range("H" + Hrow).SetValue("पाली");

        sht.Range("G" + Hrow + ":H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("G" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("G" + Hrow + ":H" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        //****Days
        sht.Range("I" + Hrow + ":J" + Hrow).Merge();
        sht.Range("I" + Hrow).SetValue("पहला");
        sht.Range("I" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("I").Width = 2.43;
        sht.Column("J").Width = 2.43;

        sht.Range("K" + Hrow + ":L" + Hrow).Merge();
        sht.Range("K" + Hrow).SetValue("दूसरा");
        sht.Range("K" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("K").Width = 2.43;
        sht.Column("L").Width = 2.43;

        sht.Range("M" + Hrow + ":N" + Hrow).Merge();
        sht.Range("M" + Hrow).SetValue("तीसरा ");
        sht.Range("M" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("M").Width = 2.43;
        sht.Column("N").Width = 2.43;

        sht.Range("O" + Hrow + ":P" + Hrow).Merge();
        sht.Range("O" + Hrow).SetValue("चौथा ");
        sht.Range("O" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("O").Width = 2.43;
        sht.Column("P").Width = 2.43;

        sht.Range("Q" + Hrow + ":R" + Hrow).Merge();
        sht.Range("Q" + Hrow).SetValue("पाँचवा");
        sht.Range("Q" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("Q").Width = 2.43;
        sht.Column("R").Width = 2.43;

        sht.Range("S" + Hrow + ":T" + Hrow).Merge();
        sht.Range("S" + Hrow).SetValue("छठवॉं");
        sht.Range("S" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("S").Width = 2.43;
        sht.Column("T").Width = 2.43;

        sht.Range("U" + Hrow + ":V" + Hrow).Merge();
        sht.Range("U" + Hrow).SetValue("सातवॉं");
        sht.Range("U" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("U").Width = 2.43;
        sht.Column("V").Width = 2.43;

        sht.Range("W" + Hrow + ":X" + Hrow).Merge();
        sht.Range("W" + Hrow).SetValue("आठवॉ");
        sht.Range("W" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("W").Width = 2.43;
        sht.Column("X").Width = 2.43;

        sht.Range("Y" + Hrow + ":Z" + Hrow).Merge();
        sht.Range("Y" + Hrow).SetValue("नौवॉं");
        sht.Range("Y" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("Y").Width = 2.43;
        sht.Column("Z").Width = 2.43;

        sht.Range("AA" + Hrow + ":AB" + Hrow).Merge();
        sht.Range("AA" + Hrow).SetValue("दसवॉं");
        sht.Range("AA" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AA").Width = 2.43;
        sht.Column("AB").Width = 2.43;

        sht.Range("AC" + Hrow + ":AD" + Hrow).Merge();
        sht.Range("AC" + Hrow).SetValue("ग्यारहवॉं");
        sht.Range("AC" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AC").Width = 2.43;
        sht.Column("AD").Width = 2.43;

        sht.Range("AE" + Hrow + ":AF" + Hrow).Merge();
        sht.Range("AE" + Hrow).SetValue("बारहवॉं");
        sht.Range("AE" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AE").Width = 2.43;
        sht.Column("AF").Width = 2.43;

        sht.Range("AG" + Hrow + ":AH" + Hrow).Merge();
        sht.Range("AG" + Hrow).SetValue("तेरहवॉं");
        sht.Range("AG" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AG").Width = 2.43;
        sht.Column("AH").Width = 2.43;

        sht.Range("AI" + Hrow + ":AJ" + Hrow).Merge();
        sht.Range("AI" + Hrow).SetValue("चौहवॉ");
        sht.Range("AI" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AI").Width = 2.43;
        sht.Column("AJ").Width = 2.43;

        sht.Range("AK" + Hrow + ":AL" + Hrow).Merge();
        sht.Range("AK" + Hrow).SetValue("पन्द्रहवॉ");
        sht.Range("AK" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AK").Width = 2.43;
        sht.Column("AL").Width = 2.43;

        sht.Range("AM" + Hrow + ":AN" + Hrow).Merge();
        sht.Range("AM" + Hrow).SetValue("सोलहवॉ");
        sht.Range("AM" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AM").Width = 2.43;
        sht.Column("AN").Width = 2.43;

        sht.Range("AO" + Hrow + ":AP" + Hrow).Merge();
        sht.Range("AO" + Hrow).SetValue("सत्रहवॉ");
        sht.Range("AO" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AO").Width = 2.43;
        sht.Column("AP").Width = 2.43;

        sht.Range("AQ" + Hrow + ":AR" + Hrow).Merge();
        sht.Range("AQ" + Hrow).SetValue("अठरहवॉ");
        sht.Range("AQ" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AQ").Width = 2.43;
        sht.Column("AR").Width = 2.43;

        sht.Range("AS" + Hrow + ":AT" + Hrow).Merge();
        sht.Range("AS" + Hrow).SetValue("उन्निसवॉ");
        sht.Range("AS" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AS").Width = 2.43;
        sht.Column("AT").Width = 2.43;

        sht.Range("AU" + Hrow + ":AV" + Hrow).Merge();
        sht.Range("AU" + Hrow).SetValue("बीसवाँ");
        sht.Range("AU" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AU").Width = 2.43;
        sht.Column("AV").Width = 2.43;

        sht.Range("AW" + Hrow + ":AX" + Hrow).Merge();
        sht.Range("AW" + Hrow).SetValue("इक्कीसवॉं");
        sht.Range("AW" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AW").Width = 2.43;
        sht.Column("AX").Width = 2.43;

        sht.Range("AY" + Hrow + ":AZ" + Hrow).Merge();
        sht.Range("AY" + Hrow).SetValue("बाईसवॉं");
        sht.Range("AY" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("AY").Width = 2.43;
        sht.Column("AZ").Width = 2.43;

        sht.Range("BA" + Hrow + ":BB" + Hrow).Merge();
        sht.Range("BA" + Hrow).SetValue("तेइसवॉं");
        sht.Range("BA" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BA").Width = 2.43;
        sht.Column("BB").Width = 2.43;

        sht.Range("BC" + Hrow + ":BD" + Hrow).Merge();
        sht.Range("BC" + Hrow).SetValue("चौबीसवॉं");
        sht.Range("BC" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BC").Width = 2.43;
        sht.Column("BD").Width = 2.43;


        sht.Range("BE" + Hrow + ":BF" + Hrow).Merge();
        sht.Range("BE" + Hrow).SetValue("पच्चीसवॉं");
        sht.Range("BE" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BE").Width = 2.43;
        sht.Column("BF").Width = 2.43;

        sht.Range("BG" + Hrow + ":BH" + Hrow).Merge();
        sht.Range("BG" + Hrow).SetValue("छब्बीसवॉं");
        sht.Range("BG" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BG").Width = 2.43;
        sht.Column("BH").Width = 2.43;

        sht.Range("BI" + Hrow + ":BJ" + Hrow).Merge();
        sht.Range("BI" + Hrow).SetValue("सत्ताइसवॉं");
        sht.Range("BI" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BI").Width = 2.43;
        sht.Column("BJ").Width = 2.43;

        sht.Range("BK" + Hrow + ":BL" + Hrow).Merge();
        sht.Range("BK" + Hrow).SetValue("अठइसवॉं");
        sht.Range("BK" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BK").Width = 2.43;
        sht.Column("BL").Width = 2.43;

        sht.Range("BM" + Hrow + ":BN" + Hrow).Merge();
        sht.Range("BM" + Hrow).SetValue("उन्तीसवॉं");
        sht.Range("BM" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BM").Width = 2.43;
        sht.Column("BN").Width = 2.43;

        sht.Range("BO" + Hrow + ":BP" + Hrow).Merge();
        sht.Range("BO" + Hrow).SetValue("तीसवॉं");
        sht.Range("BO" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BO").Width = 2.43;
        sht.Column("BP").Width = 2.43;

        sht.Range("BQ" + Hrow + ":BR" + Hrow).Merge();
        sht.Range("BQ" + Hrow).SetValue("इक्तीसवॉं");
        sht.Range("BQ" + Hrow).Style.Alignment.SetTextRotation(90);
        sht.Column("BQ").Width = 2.43;
        sht.Column("BR").Width = 2.43;


        sht.Range("I" + Hrow + ":BR" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


        sht.Range("BS" + Hrow).SetValue("उन दिनों की कुल संख्या जिनमे कार्य किया गया");
        sht.Column("BS").Width = 10.86;
        sht.Range("BS" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BS" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BS" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("BT" + Hrow).SetValue("आधारित मजदूरी की दर");
        sht.Column("BT").Width = 10.86;
        sht.Range("BT" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BT" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BT" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //अर्जित धनराशि

        sht.Range("BU" + Hrow).SetValue("अर्जित धनराशि");
        sht.Column("BU").Width = 10.86;
        sht.Range("BU" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BU" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BU" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //भत्तो  की दर यदि कोई हो
        sht.Range("BV" + (Hrow - 1) + ":CC" + (Hrow - 1)).Merge();
        sht.Range("BV" + (Hrow - 1)).SetValue("भत्तो  की दर यदि कोई हो");
        sht.Range("BV" + (Hrow - 1) + ":CC" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("BV" + Hrow + ":CC" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BV" + Hrow + ":CC" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("BV" + Hrow).SetValue("HRA");
        sht.Column("BV").Width = 9.43;
        sht.Range("BW" + Hrow).SetValue("EARN");
        sht.Column("BW").Width = 9.43;

        sht.Range("BX" + Hrow).SetValue("CONV.");
        sht.Column("BX").Width = 9.43;
        sht.Range("BY" + Hrow).SetValue("EARN");
        sht.Column("BY").Width = 9.43;

        sht.Range("BZ" + Hrow).SetValue("EDU ALW");
        sht.Column("BZ").Width = 9.43;
        sht.Range("CA" + Hrow).SetValue("EARN");
        sht.Column("CA").Width = 9.43;

        sht.Range("CB" + Hrow).SetValue("SPL ALW");
        sht.Column("CB").Width = 9.43;
        sht.Range("CC" + Hrow).SetValue("EARN");
        sht.Column("CC").Width = 9.43;

        //कुल अर्जित धन राशि
        sht.Range("CD" + Hrow + ":CF" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("CD" + Hrow + ":CF" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("CD" + Hrow + ":CF" + Hrow).Style.Alignment.SetWrapText();


        sht.Range("CD" + Hrow).SetValue("कुल अर्जित धन राशि");
        sht.Column("CD").Width = 9.43;

        sht.Range("CE" + Hrow).SetValue("अतिरिक्त काल (Over Time) के कुल घंटे");
        sht.Column("CE").Width = 10.86;

        sht.Range("CF" + Hrow).SetValue("अतिरिक्त काल (Over Time) की दर");
        sht.Column("CF").Width = 10.86;

        //कटौतियां यदि कोई हो
        sht.Range("CG" + (Hrow - 1) + ":CI" + (Hrow - 1)).Merge();
        sht.Range("CG" + (Hrow - 1)).SetValue("कटौतियां यदि कोई हो");
        sht.Range("CG" + (Hrow - 1) + ":CI" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("CG" + Hrow + ":CU" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("CG" + Hrow + ":CU" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("CG" + Hrow + ":CU" + Hrow).Style.Alignment.SetWrapText();

        sht.Range("CG" + Hrow).SetValue("भविष्य निधि के लेख में (P.F)");
        sht.Column("CG").Width = 9.43;

        sht.Range("CH" + Hrow).SetValue("कर्मचारी राज्य बिमा (E.S.I)");
        sht.Column("CH").Width = 9.43;

        sht.Range("CI" + Hrow).SetValue("अग्रिम दिए गए धन के लेख में");
        sht.Column("CI").Width = 9.43;

        sht.Range("CJ" + Hrow).SetValue("कुल कटौती (Total Deduction)");
        sht.Column("CJ").Width = 9.43;

        sht.Range("CK" + Hrow).SetValue("अर्थ दण्ड");
        sht.Column("CK").Width = 9.43;

        sht.Range("CL" + Hrow).SetValue("वास्तविक मजदूरी जो भुगतान की गई");
        sht.Column("CL").Width = 10.86;

        sht.Range("CM" + Hrow).SetValue("उन साप्ताहिक तातीलों Holidays की कुल संख्या जिसकी कार्यकर्ता की हानि हुई");
        sht.Column("CM").Width = 18.57;

        sht.Range("CN" + Hrow).SetValue("दिनांक, जिसकी क्षति पूरक तातील या तातीलों Holidays की जाएगी");
        sht.Column("CN").Width = 13.71;

        sht.Range("CO" + Hrow).SetValue("");
        sht.Column("CO").Width = 9.43;

        sht.Range("CP" + Hrow).SetValue("अभ्युक्तियॉं या इस बात का संकेत कि भुगतान कर दिये गये है दिनांको सहित");
        sht.Column("CP").Width = 18.57;

        sht.Range("CQ" + Hrow).SetValue("Account Name");
        sht.Column("CQ").Width = 30.0;
        sht.Range("CR" + Hrow).SetValue("Bank Name");
        sht.Column("CR").Width = 30.0;
        sht.Range("CS" + Hrow).SetValue("Account No.");
        sht.Column("CS").Width = 30.0;
        sht.Range("CT" + Hrow).SetValue("Ifsc Code");
        sht.Column("CT").Width = 30.0;
        sht.Range("CU" + Hrow).SetValue("Payable Amt.");
        sht.Column("CU").Width = 30.0;

        //*********Sr No
        Hrow = Hrow + 1;
        sht.Range("A" + Hrow + ":CU" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A" + Hrow).SetValue("१");
        sht.Range("B" + Hrow).SetValue("२");
        sht.Range("C" + Hrow).SetValue("३");
        sht.Range("D" + Hrow).SetValue("४");
        sht.Range("E" + Hrow).SetValue("५");
        sht.Range("F" + Hrow).SetValue("६");
        sht.Range("G" + Hrow).SetValue("७");
        sht.Range("H" + Hrow).SetValue("८");

        sht.Range("I" + Hrow + ":J" + Hrow).Merge();
        sht.Range("I" + Hrow).SetValue("९");

        sht.Range("K" + Hrow + ":L" + Hrow).Merge();
        sht.Range("K" + Hrow).SetValue("१०");

        sht.Range("M" + Hrow + ":N" + Hrow).Merge();
        sht.Range("M" + Hrow).SetValue("११");

        sht.Range("O" + Hrow + ":P" + Hrow).Merge();
        sht.Range("O" + Hrow).SetValue("१२");

        sht.Range("Q" + Hrow + ":R" + Hrow).Merge();
        sht.Range("Q" + Hrow).SetValue("१३");

        sht.Range("S" + Hrow + ":T" + Hrow).Merge();
        sht.Range("S" + Hrow).SetValue("१४");

        sht.Range("U" + Hrow + ":V" + Hrow).Merge();
        sht.Range("U" + Hrow).SetValue("१५");

        sht.Range("W" + Hrow + ":X" + Hrow).Merge();
        sht.Range("W" + Hrow).SetValue("१६");

        sht.Range("Y" + Hrow + ":Z" + Hrow).Merge();
        sht.Range("Y" + Hrow).SetValue("१७");

        sht.Range("AA" + Hrow + ":AB" + Hrow).Merge();
        sht.Range("AA" + Hrow).SetValue("१८");

        sht.Range("AC" + Hrow + ":AD" + Hrow).Merge();
        sht.Range("AC" + Hrow).SetValue("१९");

        sht.Range("AE" + Hrow + ":AF" + Hrow).Merge();
        sht.Range("AE" + Hrow).SetValue("२०");

        sht.Range("AG" + Hrow + ":AH" + Hrow).Merge();
        sht.Range("AG" + Hrow).SetValue("२१");

        sht.Range("AI" + Hrow + ":AJ" + Hrow).Merge();
        sht.Range("AI" + Hrow).SetValue("२२");

        sht.Range("AK" + Hrow + ":AL" + Hrow).Merge();
        sht.Range("AK" + Hrow).SetValue("२३");

        sht.Range("AM" + Hrow + ":AN" + Hrow).Merge();
        sht.Range("AM" + Hrow).SetValue("२४");

        sht.Range("AO" + Hrow + ":AP" + Hrow).Merge();
        sht.Range("AO" + Hrow).SetValue("२५");

        sht.Range("AQ" + Hrow + ":AR" + Hrow).Merge();
        sht.Range("AQ" + Hrow).SetValue("२६");

        sht.Range("AS" + Hrow + ":AT" + Hrow).Merge();
        sht.Range("AS" + Hrow).SetValue("२७");

        sht.Range("AU" + Hrow + ":AV" + Hrow).Merge();
        sht.Range("AU" + Hrow).SetValue("२८");

        sht.Range("AW" + Hrow + ":AX" + Hrow).Merge();
        sht.Range("AW" + Hrow).SetValue("२९");

        sht.Range("AY" + Hrow + ":AZ" + Hrow).Merge();
        sht.Range("AY" + Hrow).SetValue("३०");

        sht.Range("BA" + Hrow + ":BB" + Hrow).Merge();
        sht.Range("BA" + Hrow).SetValue("३१");

        sht.Range("BC" + Hrow + ":BD" + Hrow).Merge();
        sht.Range("BC" + Hrow).SetValue("३२");

        sht.Range("BE" + Hrow + ":BF" + Hrow).Merge();
        sht.Range("BE" + Hrow).SetValue("३३");

        sht.Range("BG" + Hrow + ":BH" + Hrow).Merge();
        sht.Range("BG" + Hrow).SetValue("३४");

        sht.Range("BI" + Hrow + ":BJ" + Hrow).Merge();
        sht.Range("BI" + Hrow).SetValue("३५");

        sht.Range("BK" + Hrow + ":BL" + Hrow).Merge();
        sht.Range("BK" + Hrow).SetValue("३६");

        sht.Range("BM" + Hrow + ":BN" + Hrow).Merge();
        sht.Range("BM" + Hrow).SetValue("३७");

        sht.Range("BO" + Hrow + ":BP" + Hrow).Merge();
        sht.Range("BO" + Hrow).SetValue("३८");

        sht.Range("BQ" + Hrow + ":BR" + Hrow).Merge();
        sht.Range("BQ" + Hrow).SetValue("३९");

        sht.Range("BS" + Hrow).SetValue("४०");
        sht.Range("BT" + Hrow).SetValue("४१");
        sht.Range("BU" + Hrow).SetValue("४२");
        sht.Range("BV" + Hrow).SetValue("४३");
        sht.Range("BW" + Hrow).SetValue("४४");
        sht.Range("BX" + Hrow).SetValue("४५");
        sht.Range("BY" + Hrow).SetValue("४६");
        sht.Range("BZ" + Hrow).SetValue("४७");
        sht.Range("CA" + Hrow).SetValue("४८");
        sht.Range("CB" + Hrow).SetValue("४९");
        sht.Range("CC" + Hrow).SetValue("५०");
        sht.Range("CD" + Hrow).SetValue("५१");
        sht.Range("CE" + Hrow).SetValue("५२");
        sht.Range("CF" + Hrow).SetValue("५३");
        sht.Range("CG" + Hrow).SetValue("५४");
        sht.Range("CH" + Hrow).SetValue("५५");
        sht.Range("CI" + Hrow).SetValue("५६");
        sht.Range("CJ" + Hrow).SetValue("५७");
        sht.Range("CK" + Hrow).SetValue("५८");
        sht.Range("CL" + Hrow).SetValue("५९");
        sht.Range("CM" + Hrow).SetValue("६०");
        sht.Range("CN" + Hrow).SetValue("६१");
        sht.Range("CO" + Hrow).SetValue("६२");
        sht.Range("CP" + Hrow).SetValue("६३");
        sht.Range("CQ" + Hrow).SetValue("६४");
        sht.Range("CR" + Hrow).SetValue("६५");
        sht.Range("CS" + Hrow).SetValue("६६");
        sht.Range("CT" + Hrow).SetValue("६७");
        sht.Range("CU" + Hrow).SetValue("६८");
        //Freeze Row and cell
        sht.SheetView.Freeze((Hrow), 1);

        //BR
        row = Hrow + 1;

        //****
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(i + 1);
            sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
            sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Empname"]);
            sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Fathername"]);
            sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
            sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
            sht.Range("G" + row).SetValue("");
            sht.Range("H" + row).SetValue("");

            //***********DAY PRESENT STATUS
            DataView dvstatus = new DataView(ds.Tables[1]);
            dvstatus.RowFilter = "empid=" + ds.Tables[0].Rows[i]["empid"];
            dvstatus.Sort = "Date";
            DataSet dsstatus = new DataSet();
            dsstatus.Tables.Add(dvstatus.ToTable());
            int Attcell_Isthalf = 9, Attcell_IIndhalf = 10;

            for (int j = 0; j < dsstatus.Tables[0].Rows.Count; j++)
            {
                if ((dsstatus.Tables[0].Rows[j]["ist_half"].ToString() == "WO" && dsstatus.Tables[0].Rows[j]["iind_half"].ToString() == "WO") || (dsstatus.Tables[0].Rows[j]["ist_half"].ToString() == "HD" && dsstatus.Tables[0].Rows[j]["iind_half"].ToString() == "HD"))
                {
                    sht.Range(sht.Cell(row, Attcell_Isthalf), sht.Cell(row, Attcell_IIndhalf)).Merge();
                    sht.Cell(row, Attcell_Isthalf).SetValue(dsstatus.Tables[0].Rows[j]["ist_half"]);
                    sht.Range(sht.Cell(row, Attcell_Isthalf), sht.Cell(row, Attcell_IIndhalf)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }
                else
                {
                    sht.Cell(row, Attcell_Isthalf).SetValue(dsstatus.Tables[0].Rows[j]["ist_half"]);
                    sht.Cell(row, Attcell_IIndhalf).SetValue(dsstatus.Tables[0].Rows[j]["iind_half"]);
                }

                Attcell_Isthalf = Attcell_Isthalf + 2;
                Attcell_IIndhalf = Attcell_IIndhalf + 2;
            }
            //***********
            //***********Amount Details
            DataView dvamount = new DataView(ds.Tables[2]);
            dvamount.RowFilter = "empid=" + ds.Tables[0].Rows[i]["empid"];
            DataSet dsamt = new DataSet();
            dsamt.Tables.Add(dvamount.ToTable());
            for (int k = 0; k < dsamt.Tables[0].Rows.Count; k++)
            {
                sht.Range("BS" + row).SetValue(dsamt.Tables[0].Rows[0]["Totalworkingdays"]);
                sht.Range("BT" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicpay"]);
                sht.Range("BU" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicearnamt"]);

                //HRA
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "HRA")
                {
                    sht.Range("BV" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("BW" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //Conveyance
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "CONVEYANCE")
                {
                    sht.Range("BX" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("BY" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //EDU_ALW
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "EDU_ALW")
                {
                    sht.Range("BZ" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("CA" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //SPL_ALW
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "SPL_ALW")
                {
                    sht.Range("CB" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("CC" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }

                sht.Range("CD" + row).FormulaA1 = "=BU" + row + "+BW" + row + "+BY" + row + "+CA" + row + "+CC" + row + "";
                //E.P.F
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.P.F")
                {
                    sht.Range("CG" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.S.I")
                {
                    sht.Range("CH" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                //Advance
                sht.Range("CI" + row).SetValue(Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Advanceamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Loanamt"]));
                sht.Range("CJ" + row).FormulaA1 = "=CG" + row + "+CH" + row + "+CI" + row + "";
                sht.Range("CL" + row).FormulaA1 = "=CD" + row + "-CJ" + row + "";

                //sht.Range("CQ" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("CQ" + row).SetValue(ds.Tables[0].Rows[i]["accountholdername"]);
                sht.Range("CR" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                sht.Range("CS" + row).SetValue(ds.Tables[0].Rows[i]["Bankacno"]);
                sht.Range("CT" + row).SetValue(ds.Tables[0].Rows[i]["ifsccode"]);
                sht.Range("CU" + row).FormulaA1 = "=CL" + row + "";
            }


            row = row + 1;
        }
        //**********
        //******SAVE FILE
        string Path = "";
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("Form12(MonthWise)" + DateTime.Now + "." + Fileextension);
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
        //lstdept.Items.Remove(lstdept.SelectedItem);
        lstdept.Items.Clear();
    }
    protected void btndeletedesignation_Click(object sender, EventArgs e)
    {
        //lstdesignation.Items.Remove(lstdesignation.SelectedItem);
        lstdesignation.Items.Clear();
    }

}