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
public partial class Masters_ReportForms_frmHr_empformreports : System.Web.UI.Page
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
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                    Select Distinct D.DepartmentId, D.DepartmentName 
                    From Department D(Nolock)
                    JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                    JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                    Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By D.DepartmentName 
                    SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE order by Month_Id
                    SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA order by Year
                    Select Designationid,Designation From HR_Designationmaster order by Designation
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];
            
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--ALL--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDyear, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 4, true, "--Plz Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            DDMonth.SelectedValue = System.DateTime.Now.Month.ToString();
            DDyear.SelectedValue = System.DateTime.Now.Year.ToString();

            if (Convert.ToInt16(Request.QueryString["ForST"]) == 1) ////For ST Means Salary Transfer
            {
                DDreporttype.Items.Clear();
                DDreporttype.Items.Add(new ListItem("Salary Transfer OtherBank Emp", "6"));
                DDreporttype.Items.Add(new ListItem("Salary Transfer SameBank Emp", "7"));
                DDreporttype.Items.Add(new ListItem("Cash Salary", "8"));
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
                Form12_Excel();
                break;
            case "2":
                SalarySlip();
                break;
            case "3":
            case "11":
                Form14();
                break;
            case "4":
                Form12ForPayRoll_Excel();
                ///Form12_Excel();   Same PROCEDURE USED IN BOTH 
                break;
            case "5":
                Form12Register();
                ///Form12_Excel();   Same PROCEDURE USED IN BOTH 
                break;
            case "6":
                SalaryTransferEmpBankDetail();
                break;
            case "7":
                SalaryTransferEmpBankDetailSame();
                break;
            case "8":
                CashSalary();
                break;
            case "9":
                SalaryTransferEmpBankDetail();
                break;
            case "10":
                SalaryTransferEmpBankDetailSame();
                break;
            case "12":
                FormCLSL();
                break;
            case "13":
                BalanceCLSLEL();
                break;
            default:
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No report found for this report type.')", true);
                break;
        }
    }
    protected void BalanceCLSLEL()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_GETBALCLSLEL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFilename"] = "Reports/RptFormCLSL.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\RptFormCLSL.xsd";
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
    protected void FormCLSL()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_GETCLSL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFilename"] = "Reports/RptFormCLSL.rpt";
                Session["dsFilename"] = "~\\ReportSchema\\RptFormCLSL.xsd";
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
    protected void Form12Register()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_FORM12REGISTER", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.Add("@Shiftstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@shiftstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Shiftend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Shiftend"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchend"].Direction = ParameterDirection.Output;
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["emp_photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["emp_photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["emp_photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["image"] = img_Byte;
                        }
                    }
                }
                if (DDwagescalculation.SelectedValue == "1")
                {
                    if (DDBranchName.SelectedValue == "2")
                    {
                        Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_MONTHLYForChampoPanipat.rpt";
                    }
                    else
                    {
                        Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_MONTHLY.rpt";
                    }
                }
                else if (DDwagescalculation.SelectedValue == "2")
                {
                    Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_MONTHLY.rpt";
                }
                else if (DDwagescalculation.SelectedValue == "3")
                {
                    //Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_PERDAY.rpt";
                    if (DDBranchName.SelectedValue == "2")
                    {
                        Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_PERDAYForChampoPanipat.rpt";
                    }
                    else
                    {
                        Session["rptFilename"] = "Reports/RPT_FORM12REGISTER_PERDAY.rpt";
                    }
                }
                
                Session["dsFilename"] = "~\\ReportSchema\\RPTFORM12REGISTER.xsd";
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

            if (DDwagescalculation.SelectedValue == "3")  //PCS WISE
            {

            }
            else
            {

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
    protected void Form12_Excel()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_FORM12EXCEL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.Add("@Shiftstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@shiftstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Shiftend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Shiftend"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchend"].Direction = ParameterDirection.Output;
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            if (DDwagescalculation.SelectedValue == "3")  //PCS WISE
            {
                Form12_PCSwise(ds, cmd.Parameters["@shiftstart"].Value.ToString(), cmd.Parameters["@Shiftend"].Value.ToString(), cmd.Parameters["@Lunchstart"].Value.ToString(), cmd.Parameters["@Lunchend"].Value.ToString());
            }
            else
            {
                Form12_Monthwise(ds, cmd.Parameters["@shiftstart"].Value.ToString(), cmd.Parameters["@Shiftend"].Value.ToString(), cmd.Parameters["@Lunchstart"].Value.ToString(), cmd.Parameters["@Lunchend"].Value.ToString());
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

                //sht.Range("CD" + row).FormulaA1 = "=BU" + row + "+BW" + row + "+BY" + row + "+CA" + row + "+CC" + row + "";
                sht.Range("CD" + row).SetValue(dsamt.Tables[0].Rows[0]["GROSSSAL"]);

                sht.Range("CE" + row).SetValue(dsamt.Tables[0].Rows[0]["OTHOURMINUTES"]);
                sht.Range("CF" + row).SetValue(dsamt.Tables[0].Rows[0]["OTAMOUNT"]);
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
                sht.Range("CL" + row).FormulaA1 = "=CD" + row + "+CF" + row + "-CJ" + row + "";

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
    protected void Form12_PCSwise(DataSet ds, string shiftstart, string Shiftend, string Lunchstart, string Lunchend)
    {
        if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("FORM12_PCSWISE");
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
        sht.Range("BV" + Hrow).SetValue("भत्तो  की दर यदि कोई हो");
        sht.Column("BV").Width = 10.86;
        sht.Range("BV" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BV" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BV" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("BW" + Hrow).SetValue("प्रोडक्शन राशि (Production Amount)");
        sht.Column("BW").Width = 10.86;
        sht.Range("BW" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BW" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BW" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("BX" + Hrow).SetValue("(Incentive Amount) प्रोत्त्साहन राशि");
        sht.Column("BX").Width = 10.86;
        sht.Range("BX" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("BX" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BX" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //कुल अर्जित धन राशि
        sht.Range("BY" + Hrow + ":CA" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("BY" + Hrow + ":CA" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("BY" + Hrow + ":CA" + Hrow).Style.Alignment.SetWrapText();


        sht.Range("BY" + Hrow).SetValue("कुल अर्जित धन राशि");
        sht.Column("BY").Width = 9.43;

        sht.Range("BZ" + Hrow).SetValue("अतिरिक्त काल (Over Time) के कुल घंटे");
        sht.Column("BZ").Width = 10.86;

        sht.Range("CA" + Hrow).SetValue("अतिरिक्त काल (Over Time) की दर");
        sht.Column("CA").Width = 10.86;

        //कटौतियां यदि कोई हो
        sht.Range("CB" + (Hrow - 1) + ":CD" + (Hrow - 1)).Merge();
        sht.Range("CB" + (Hrow - 1)).SetValue("कटौतियां यदि कोई हो");
        sht.Range("CB" + (Hrow - 1) + ":CD" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("CB" + Hrow + ":CP" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("CB" + Hrow + ":CP" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("CB" + Hrow + ":CP" + Hrow).Style.Alignment.SetWrapText();

        sht.Range("CB" + Hrow).SetValue("भविष्य निधि के लेख में (P.F)");
        sht.Column("CB").Width = 9.43;

        sht.Range("CC" + Hrow).SetValue("कर्मचारी राज्य बिमा (E.S.I)");
        sht.Column("CC").Width = 9.43;

        sht.Range("CD" + Hrow).SetValue("खर्चा");
        sht.Column("CD").Width = 9.43;

        sht.Range("CE" + Hrow).SetValue("कुल कटौती (Total Deduction)");
        sht.Column("CE").Width = 9.43;

        sht.Range("CF" + Hrow).SetValue("अर्थ दण्ड");
        sht.Column("CF").Width = 9.43;

        sht.Range("CG" + Hrow).SetValue("वास्तविक मजदूरी जो भुगतान की गई");
        sht.Column("CG").Width = 10.86;

        sht.Range("CH" + Hrow).SetValue("उन साप्ताहिक तातीलों Holidays की कुल संख्या जिसकी कार्यकर्ता की हानि हुई");
        sht.Column("CH").Width = 18.57;

        sht.Range("CI" + Hrow).SetValue("दिनांक, जिसकी क्षति पूरक तातील या तातीलों Holidays की जाएगी");
        sht.Column("CI").Width = 13.71;

        sht.Range("CJ" + Hrow).SetValue("");
        sht.Column("CJ").Width = 9.43;

        sht.Range("CK" + Hrow).SetValue("अभ्युक्तियॉं या इस बात का संकेत कि भुगतान कर दिये गये है दिनांको सहित");
        sht.Column("CK").Width = 18.57;

        sht.Range("CL" + Hrow).SetValue("Account Name");
        sht.Column("CL").Width = 30.0;
        sht.Range("CM" + Hrow).SetValue("Bank Name");
        sht.Column("CM").Width = 30.0;
        sht.Range("CN" + Hrow).SetValue("Account No.");
        sht.Column("CN").Width = 30.0;
        sht.Range("CO" + Hrow).SetValue("Ifsc Code");
        sht.Column("CO").Width = 30.0;
        sht.Range("CP" + Hrow).SetValue("Payable Amt.");
        sht.Column("CP").Width = 30.0;

        //*********Sr No
        Hrow = Hrow + 1;
        sht.Range("A" + Hrow + ":CP" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
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
                sht.Range("BW" + row).SetValue(dsamt.Tables[0].Rows[0]["Productionamt"]);
                sht.Range("BX" + row).SetValue(dsamt.Tables[0].Rows[0]["Incentiveamt"]);

                ////HRA
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "HRA")
                //{
                //    sht.Range("BV" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("BW" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////Conveyance
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "CONVEYANCE")
                //{
                //    sht.Range("BX" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("BY" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////EDU_ALW
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "EDU_ALW")
                //{
                //    sht.Range("BZ" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("CA" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////SPL_ALW
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "SPL_ALW")
                //{
                //    sht.Range("CB" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("CC" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}

                //sht.Range("CD" + row).FormulaA1 = "=BU" + row + "+BW" + row + "+BY" + row + "+CA" + row + "+CC" + row + "";
                sht.Range("BY" + row).SetValue(dsamt.Tables[0].Rows[k]["Grosssal"]);
                sht.Range("BZ" + row).SetValue(dsamt.Tables[0].Rows[k]["OTHOURMINUTES"]);
                sht.Range("CA" + row).SetValue(dsamt.Tables[0].Rows[k]["OTAmount"]);
                //E.P.F
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.P.F")
                {
                    sht.Range("CB" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.S.I")
                {
                    sht.Range("CC" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                //Advance
                sht.Range("CD" + row).SetValue(Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Advanceamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Loanamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Kharchaamt"]));
                sht.Range("CE" + row).FormulaA1 = "=CB" + row + "+CC" + row + "+CD" + row + "";
                sht.Range("CG" + row).FormulaA1 = "=BY" + row + "+CA" + row + "-CE" + row + "";

                //sht.Range("CL" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("CL" + row).SetValue(ds.Tables[0].Rows[i]["accountholdername"]);
                sht.Range("CM" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                sht.Range("CN" + row).SetValue(ds.Tables[0].Rows[i]["Bankacno"]);
                sht.Range("CO" + row).SetValue(ds.Tables[0].Rows[i]["ifsccode"]);
                sht.Range("CP" + row).FormulaA1 = "=CG" + row + "";
            }


            row = row + 1;
        }
        //**********
        //******SAVE FILE
        string Path = "";
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("Form12(PcsWise)" + DateTime.Now + "." + Fileextension);
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
    protected void Form12ForPayRoll_Excel()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_FORM12EXCEL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.Add("@Shiftstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@shiftstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Shiftend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Shiftend"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchstart", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchstart"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Lunchend", SqlDbType.VarChar, 20);
            cmd.Parameters["@Lunchend"].Direction = ParameterDirection.Output;
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
                return;
            }
            if (DDwagescalculation.SelectedValue == "3")  //PCS WISE
            {
                Form12ForPayRoll_PCSwise(ds, cmd.Parameters["@shiftstart"].Value.ToString(), cmd.Parameters["@Shiftend"].Value.ToString(), cmd.Parameters["@Lunchstart"].Value.ToString(), cmd.Parameters["@Lunchend"].Value.ToString());
            }
            else
            {
                Form12ForPayRoll_Monthwise(ds, cmd.Parameters["@shiftstart"].Value.ToString(), cmd.Parameters["@Shiftend"].Value.ToString(), cmd.Parameters["@Lunchstart"].Value.ToString(), cmd.Parameters["@Lunchend"].Value.ToString());
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
    protected void Form12ForPayRoll_Monthwise(DataSet ds, string shiftstart, string Shiftend, string Lunchstart, string Lunchend)
    {
        if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("FORM12ForPayRoll_MONTHWISE");
        int MHrow = 1;
        int Hrow = 0;
        int row = 0;
        //******************MAIN HEADERS
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("प्रपत्र  सं. १२");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("(नियम  ७८ )");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("वयस्क कार्यकर्ताओ की पंजी जो     अधिनियम की धारा ६२ के  अधीन निर्धारित की गई है");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        //कारखाने या विभाग का नाम  महीना वर्ष
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("कारखाने या विभाग का नाम  " + DDCompanyName.SelectedItem.Text + "  महीना  " + DDMonth.SelectedItem.Text + " वर्ष  " + DDyear.SelectedValue + "");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //कार्य प्रारम्भ करने का समय
        MHrow = MHrow + 1;
        sht.Row(MHrow).Height = 33;
        sht.Range("P" + MHrow).SetValue("कार्य प्रारम्भ करने का समय");
        sht.Range("P" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("P" + MHrow + ":R" + MHrow + "").Merge();
        sht.Range("P" + MHrow + ":R" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("P" + MHrow + ":R" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //विभाग
        sht.Range("S" + MHrow).SetValue("विभाग");
        sht.Range("S" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("S" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //अवधि
        sht.Range("U" + MHrow).SetValue("अवधि");
        sht.Range("U" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("U" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("U" + MHrow + ":V" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("U" + MHrow + ":V" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //सोमवार से शुक्रवार तक
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("सोमवार से शुक्रवार तक");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();

        sht.Range("P" + MHrow + ":R" + (MHrow + 3) + "").Merge();
        sht.Range("P" + MHrow).SetValue(shiftstart);
        sht.Range("P" + MHrow + ":R" + (MHrow + 3)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("P" + MHrow + ":R" + (MHrow + 3)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("S" + MHrow + ":S" + MHrow + "").Merge();
        sht.Range("S" + MHrow).SetValue("से");
        sht.Range("T" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("T" + MHrow).SetValue("तक");
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("U" + MHrow + ":U" + MHrow + "").Merge();
        sht.Range("U" + MHrow).SetValue("से");
        sht.Range("V" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("V" + MHrow).SetValue("तक");


        sht.Range("W" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("W" + MHrow).SetValue("समाप्ति (Completion) का समय");

        sht.Range("W" + MHrow + ":AB" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //शनिवार
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("शनिवार");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();

        sht.Range("S" + MHrow + ":S" + MHrow + "").Merge();
        sht.Range("S" + MHrow).SetValue(shiftstart);
        sht.Range("T" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("T" + MHrow).SetValue(Lunchstart);
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("U" + MHrow + ":U" + MHrow + "").Merge();
        sht.Range("U" + MHrow).SetValue(Lunchend);
        sht.Range("V" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("V" + MHrow).SetValue(Shiftend);


        sht.Range("W" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("W" + MHrow).SetValue(Shiftend);

        sht.Range("W" + MHrow + ":AB" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //रविवार
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("रविवार");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();
        //रिले के आंक्शन  (रोटेशन) की पद्धत्ति
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("रिले के आंक्शन  (रोटेशन) की पद्धत्ति");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();
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

        sht.Range("B" + Hrow + ":F" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


        sht.Range("G" + Hrow).SetValue("उन दिनों की कुल संख्या जिनमे कार्य किया गया");
        sht.Column("G").Width = 10.86;
        sht.Range("G" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("G" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("G" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("H" + Hrow).SetValue("आधारित मजदूरी की दर");
        sht.Column("H").Width = 10.86;
        sht.Range("H" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("H" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //अर्जित धनराशि

        sht.Range("I" + Hrow).SetValue("अर्जित धनराशि");
        sht.Column("I").Width = 10.86;
        sht.Range("I" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("I" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("I" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //भत्तो  की दर यदि कोई हो
        sht.Range("J" + (Hrow - 1) + ":Q" + (Hrow - 1)).Merge();
        sht.Range("J" + (Hrow - 1)).SetValue("भत्तो  की दर यदि कोई हो");
        sht.Range("J" + (Hrow - 1) + ":Q" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("J" + Hrow + ":Q" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("J" + Hrow + ":Q" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("J" + Hrow).SetValue("HRA");
        sht.Column("J").Width = 9.43;
        sht.Range("K" + Hrow).SetValue("EARN");
        sht.Column("K").Width = 9.43;

        sht.Range("L" + Hrow).SetValue("CONV.");
        sht.Column("L").Width = 9.43;
        sht.Range("M" + Hrow).SetValue("EARN");
        sht.Column("M").Width = 9.43;

        sht.Range("N" + Hrow).SetValue("EDU ALW");
        sht.Column("N").Width = 9.43;
        sht.Range("O" + Hrow).SetValue("EARN");
        sht.Column("O").Width = 9.43;

        sht.Range("P" + Hrow).SetValue("SPL ALW");
        sht.Column("P").Width = 9.43;
        sht.Range("Q" + Hrow).SetValue("EARN");
        sht.Column("Q").Width = 9.43;

        //कुल अर्जित धन राशि
        sht.Range("R" + Hrow + ":T" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("R" + Hrow + ":T" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("R" + Hrow + ":T" + Hrow).Style.Alignment.SetWrapText();


        sht.Range("R" + Hrow).SetValue("कुल अर्जित धन राशि");
        sht.Column("R").Width = 9.43;

        sht.Range("S" + Hrow).SetValue("अतिरिक्त काल (Over Time) के कुल घंटे");
        sht.Column("S").Width = 10.86;

        sht.Range("T" + Hrow).SetValue("अतिरिक्त काल (Over Time) की दर");
        sht.Column("T").Width = 10.86;

        //कटौतियां यदि कोई हो
        sht.Range("U" + (Hrow - 1) + ":W" + (Hrow - 1)).Merge();
        sht.Range("U" + (Hrow - 1)).SetValue("कटौतियां यदि कोई हो");
        sht.Range("U" + (Hrow - 1) + ":W" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("U" + Hrow + ":CU" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("U" + Hrow + ":CU" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("U" + Hrow + ":CU" + Hrow).Style.Alignment.SetWrapText();

        sht.Range("U" + Hrow).SetValue("भविष्य निधि के लेख में (P.F)");
        sht.Column("U").Width = 9.43;

        sht.Range("V" + Hrow).SetValue("कर्मचारी राज्य बिमा (E.S.I)");
        sht.Column("V").Width = 9.43;

        sht.Range("W" + Hrow).SetValue("अग्रिम दिए गए धन के लेख में");
        sht.Column("W").Width = 9.43;

        sht.Range("X" + Hrow).SetValue("कुल कटौती (Total Deduction)");
        sht.Column("X").Width = 9.43;

        sht.Range("Y" + Hrow).SetValue("अर्थ दण्ड");
        sht.Column("Y").Width = 9.43;

        sht.Range("Z" + Hrow).SetValue("वास्तविक मजदूरी जो भुगतान की गई");
        sht.Column("Z").Width = 10.86;

        sht.Range("AA" + Hrow).SetValue("उन साप्ताहिक तातीलों Holidays की कुल संख्या जिसकी कार्यकर्ता की हानि हुई");
        sht.Column("AA").Width = 18.57;

        sht.Range("AB" + Hrow).SetValue("दिनांक, जिसकी क्षति पूरक तातील या तातीलों Holidays की जाएगी");
        sht.Column("AB").Width = 13.71;

        sht.Range("AC" + Hrow).SetValue("");
        sht.Column("AC").Width = 9.43;

        sht.Range("AD" + Hrow).SetValue("अभ्युक्तियॉं या इस बात का संकेत कि भुगतान कर दिये गये है दिनांको सहित");
        sht.Column("AD").Width = 18.57;

        sht.Range("AE" + Hrow).SetValue("Account Name");
        //sht.Column("AE").Width = 30.0;
        sht.Range("AF" + Hrow).SetValue("Bank Name");
        //sht.Column("AF").Width = 30.0;
        sht.Range("AG" + Hrow).SetValue("Account No.");
        //sht.Column("AG").Width = 30.0;
        sht.Range("AH" + Hrow).SetValue("Ifsc Code");
        //sht.Column("AH").Width = 30.0;
        sht.Range("AI" + Hrow).SetValue("Payable Amt.");
        //sht.Column("AI").Width = 30.0;

        //*********Sr No
        Hrow = Hrow + 1;
        sht.Range("A" + Hrow + ":AI" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A" + Hrow).SetValue("१");
        sht.Range("B" + Hrow).SetValue("२");
        sht.Range("C" + Hrow).SetValue("३");
        sht.Range("D" + Hrow).SetValue("४");
        sht.Range("E" + Hrow).SetValue("५");
        sht.Range("F" + Hrow).SetValue("६");
        sht.Range("G" + Hrow).SetValue("७");
        sht.Range("H" + Hrow).SetValue("८");

        sht.Range("I" + Hrow).SetValue("९");
        sht.Range("J" + Hrow).SetValue("१०");
        sht.Range("K" + Hrow).SetValue("११");
        sht.Range("L" + Hrow).SetValue("१२");
        sht.Range("M" + Hrow).SetValue("१३");
        sht.Range("N" + Hrow).SetValue("१४");
        sht.Range("O" + Hrow).SetValue("१५");
        sht.Range("P" + Hrow).SetValue("१६");
        sht.Range("Q" + Hrow).SetValue("१७");
        sht.Range("R" + Hrow).SetValue("१८");
        sht.Range("S" + Hrow).SetValue("१९");
        sht.Range("T" + Hrow).SetValue("२०");
        sht.Range("U" + Hrow).SetValue("२१");
        sht.Range("V" + Hrow).SetValue("२२");
        sht.Range("W" + Hrow).SetValue("२३");
        sht.Range("X" + Hrow).SetValue("२४");
        sht.Range("Y" + Hrow).SetValue("२५");
        sht.Range("Z" + Hrow).SetValue("२६");
        sht.Range("AA" + Hrow).SetValue("२७");
        sht.Range("AB" + Hrow).SetValue("२८");
        sht.Range("AC" + Hrow).SetValue("२९");
        sht.Range("AD" + Hrow).SetValue("३०");
        sht.Range("AE" + Hrow).SetValue("३१");
        sht.Range("AF" + Hrow).SetValue("३२");
        sht.Range("AG" + Hrow).SetValue("३३");
        sht.Range("AH" + Hrow).SetValue("३४");
        sht.Range("AI" + Hrow).SetValue("३५");

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

            /* //***********DAY PRESENT STATUS
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
             //***********/

            //***********Amount Details
            DataView dvamount = new DataView(ds.Tables[2]);
            dvamount.RowFilter = "empid=" + ds.Tables[0].Rows[i]["empid"];
            DataSet dsamt = new DataSet();
            dsamt.Tables.Add(dvamount.ToTable());
            for (int k = 0; k < dsamt.Tables[0].Rows.Count; k++)
            {
                sht.Range("G" + row).SetValue(dsamt.Tables[0].Rows[0]["Totalworkingdays"]);
                sht.Range("H" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicpay"]);
                sht.Range("I" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicearnamt"]);

                //HRA
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "HRA")
                {
                    sht.Range("J" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("K" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //Conveyance
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "CONVEYANCE")
                {
                    sht.Range("L" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("M" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //EDU_ALW
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "EDU_ALW")
                {
                    sht.Range("N" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("O" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }
                //SPL_ALW
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "SPL_ALW")
                {
                    sht.Range("P" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                    sht.Range("Q" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                }

                sht.Range("R" + row).FormulaA1 = "=I" + row + "+K" + row + "+M" + row + "+O" + row + "+Q" + row + "";
                sht.Range("S" + row).SetValue(dsamt.Tables[0].Rows[k]["OTHOURMINUTES"]);
                sht.Range("T" + row).SetValue(dsamt.Tables[0].Rows[k]["OTAMOUNT"]);

                //E.P.F
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.P.F")
                {
                    sht.Range("U" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.S.I")
                {
                    sht.Range("V" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                //Advance
                sht.Range("W" + row).SetValue(Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Advanceamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Loanamt"]));
                sht.Range("X" + row).FormulaA1 = "=U" + row + "+V" + row + "+W" + row + "";
                sht.Range("Z" + row).FormulaA1 = "=R" + row + "+T" + row + "-X" + row + "";

                //sht.Range("CQ" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("AE" + row).SetValue(ds.Tables[0].Rows[i]["accountholdername"]);
                sht.Range("AF" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                sht.Range("AG" + row).SetValue(ds.Tables[0].Rows[i]["Bankacno"]);
                sht.Range("AH" + row).SetValue(ds.Tables[0].Rows[i]["ifsccode"]);
                sht.Range("AI" + row).FormulaA1 = "=Z" + row + "";
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
    protected void Form12ForPayRoll_PCSwise(DataSet ds, string shiftstart, string Shiftend, string Lunchstart, string Lunchend)
    {
        if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
        }

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("FORM12ForPayRoll_PCSWISE");
        int MHrow = 1;
        int Hrow = 0;
        int row = 0;
        //******************MAIN HEADERS
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("प्रपत्र  सं. १२");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("(नियम  ७८ )");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("वयस्क कार्यकर्ताओ की पंजी जो     अधिनियम की धारा ६२ के  अधीन निर्धारित की गई है");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        MHrow = MHrow + 1;
        //कारखाने या विभाग का नाम  महीना वर्ष
        sht.Range("B" + MHrow + ":AI" + MHrow).Merge();
        sht.Range("B" + MHrow).SetValue("कारखाने या विभाग का नाम  " + DDCompanyName.SelectedItem.Text + "  महीना  " + DDMonth.SelectedItem.Text + " वर्ष  " + DDyear.SelectedValue + "");
        sht.Range("B" + MHrow + ":AI" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //कार्य प्रारम्भ करने का समय
        MHrow = MHrow + 1;
        sht.Row(MHrow).Height = 33;
        sht.Range("P" + MHrow).SetValue("कार्य प्रारम्भ करने का समय");
        sht.Range("P" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("P" + MHrow + ":R" + MHrow + "").Merge();
        sht.Range("P" + MHrow + ":R" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("P" + MHrow + ":R" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //विभाग
        sht.Range("S" + MHrow).SetValue("विभाग");
        sht.Range("S" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("S" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //अवधि
        sht.Range("U" + MHrow).SetValue("अवधि");
        sht.Range("U" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("U" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("U" + MHrow + ":V" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("U" + MHrow + ":V" + MHrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //सोमवार से शुक्रवार तक
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("सोमवार से शुक्रवार तक");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();

        sht.Range("P" + MHrow + ":R" + (MHrow + 3) + "").Merge();
        sht.Range("P" + MHrow).SetValue(shiftstart);
        sht.Range("P" + MHrow + ":R" + (MHrow + 3)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("P" + MHrow + ":R" + (MHrow + 3)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("S" + MHrow + ":S" + MHrow + "").Merge();
        sht.Range("S" + MHrow).SetValue("से");
        sht.Range("T" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("T" + MHrow).SetValue("तक");
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("U" + MHrow + ":U" + MHrow + "").Merge();
        sht.Range("U" + MHrow).SetValue("से");
        sht.Range("V" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("V" + MHrow).SetValue("तक");


        sht.Range("W" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("W" + MHrow).SetValue("समाप्ति (Completion) का समय");

        sht.Range("W" + MHrow + ":AB" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //शनिवार
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("शनिवार");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();

        sht.Range("S" + MHrow + ":S" + MHrow + "").Merge();
        sht.Range("S" + MHrow).SetValue(shiftstart);
        sht.Range("T" + MHrow + ":T" + MHrow + "").Merge();
        sht.Range("T" + MHrow).SetValue(Lunchstart);
        sht.Range("S" + MHrow + ":T" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("U" + MHrow + ":U" + MHrow + "").Merge();
        sht.Range("U" + MHrow).SetValue(Lunchend);
        sht.Range("V" + MHrow + ":V" + MHrow + "").Merge();
        sht.Range("V" + MHrow).SetValue(Shiftend);


        sht.Range("W" + MHrow + ":AB" + MHrow + "").Merge();
        sht.Range("W" + MHrow).SetValue(Shiftend);

        sht.Range("W" + MHrow + ":AB" + MHrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //रविवार
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("रविवार");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();
        //रिले के आंक्शन  (रोटेशन) की पद्धत्ति
        MHrow = MHrow + 1;
        sht.Range("J" + MHrow).SetValue("रिले के आंक्शन  (रोटेशन) की पद्धत्ति");
        sht.Range("J" + MHrow).Style.Alignment.SetWrapText();
        sht.Range("J" + MHrow + ":O" + MHrow + "").Merge();
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

        sht.Range("G" + Hrow).SetValue("उन दिनों की कुल संख्या जिनमे कार्य किया गया");
        sht.Column("G").Width = 10.86;
        sht.Range("G" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("G" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("G" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("H" + Hrow).SetValue("आधारित मजदूरी की दर");
        sht.Column("H").Width = 10.86;
        sht.Range("H" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("H" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("H" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //अर्जित धनराशि

        sht.Range("I" + Hrow).SetValue("अर्जित धनराशि");
        sht.Column("I").Width = 10.86;
        sht.Range("I" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("I" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("I" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //भत्तो  की दर यदि कोई हो
        sht.Range("J" + Hrow).SetValue("भत्तो  की दर यदि कोई हो");
        sht.Column("J").Width = 10.86;
        sht.Range("J" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("J" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("J" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("K" + Hrow).SetValue("प्रोडक्शन राशि (Production Amount)");
        sht.Column("K").Width = 10.86;
        sht.Range("K" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("K" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("K" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        sht.Range("L" + Hrow).SetValue("(Incentive Amount) प्रोत्त्साहन राशि");
        sht.Column("L").Width = 10.86;
        sht.Range("L" + Hrow).Style.Alignment.SetWrapText();
        sht.Range("L" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("L" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        //कुल अर्जित धन राशि
        sht.Range("M" + Hrow + ":CA" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("M" + Hrow + ":CA" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("M" + Hrow + ":CA" + Hrow).Style.Alignment.SetWrapText();


        sht.Range("M" + Hrow).SetValue("कुल अर्जित धन राशि");
        sht.Column("M").Width = 9.43;

        sht.Range("N" + Hrow).SetValue("अतिरिक्त काल (Over Time) के कुल घंटे");
        sht.Column("N").Width = 10.86;

        sht.Range("O" + Hrow).SetValue("अतिरिक्त काल (Over Time) की दर");
        sht.Column("O").Width = 10.86;

        //कटौतियां यदि कोई हो
        sht.Range("P" + (Hrow - 1) + ":CD" + (Hrow - 1)).Merge();
        sht.Range("P" + (Hrow - 1)).SetValue("कटौतियां यदि कोई हो");
        sht.Range("P" + (Hrow - 1) + ":CD" + (Hrow - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        sht.Range("P" + Hrow + ":CP" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("P" + Hrow + ":CP" + Hrow).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("P" + Hrow + ":CP" + Hrow).Style.Alignment.SetWrapText();

        sht.Range("P" + Hrow).SetValue("भविष्य निधि के लेख में (P.F)");
        sht.Column("P").Width = 9.43;

        sht.Range("Q" + Hrow).SetValue("कर्मचारी राज्य बिमा (E.S.I)");
        sht.Column("Q").Width = 9.43;

        sht.Range("R" + Hrow).SetValue("खर्चा");
        sht.Column("R").Width = 9.43;

        sht.Range("S" + Hrow).SetValue("कुल कटौती (Total Deduction)");
        sht.Column("S").Width = 9.43;

        sht.Range("T" + Hrow).SetValue("अर्थ दण्ड");
        sht.Column("T").Width = 9.43;

        sht.Range("U" + Hrow).SetValue("वास्तविक मजदूरी जो भुगतान की गई");
        sht.Column("U").Width = 10.86;

        sht.Range("V" + Hrow).SetValue("उन साप्ताहिक तातीलों Holidays की कुल संख्या जिसकी कार्यकर्ता की हानि हुई");
        sht.Column("V").Width = 18.57;

        sht.Range("W" + Hrow).SetValue("दिनांक, जिसकी क्षति पूरक तातील या तातीलों Holidays की जाएगी");
        sht.Column("W").Width = 13.71;

        sht.Range("X" + Hrow).SetValue("");
        sht.Column("X").Width = 9.43;

        sht.Range("Y" + Hrow).SetValue("अभ्युक्तियॉं या इस बात का संकेत कि भुगतान कर दिये गये है दिनांको सहित");
        sht.Column("Y").Width = 18.57;

        sht.Range("Z" + Hrow).SetValue("Account Name");
        //sht.Column("Z").Width = 30.0;
        sht.Range("AA" + Hrow).SetValue("Bank Name");
        //sht.Column("AA").Width = 30.0;
        sht.Range("AB" + Hrow).SetValue("Account No.");
        //sht.Column("AB").Width = 30.0;
        sht.Range("AC" + Hrow).SetValue("Ifsc Code");
        //sht.Column("AC").Width = 30.0;
        sht.Range("AD" + Hrow).SetValue("Payable Amt.");
        //sht.Column("AD").Width = 30.0;

        //*********Sr No
        Hrow = Hrow + 1;
        sht.Range("A" + Hrow + ":CP" + Hrow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A" + Hrow).SetValue("१");
        sht.Range("B" + Hrow).SetValue("२");
        sht.Range("C" + Hrow).SetValue("३");
        sht.Range("D" + Hrow).SetValue("४");
        sht.Range("E" + Hrow).SetValue("५");
        sht.Range("F" + Hrow).SetValue("६");
        sht.Range("G" + Hrow).SetValue("७");
        sht.Range("H" + Hrow).SetValue("८");
        sht.Range("I" + Hrow).SetValue("९");
        sht.Range("J" + Hrow).SetValue("१०");
        sht.Range("K" + Hrow).SetValue("११");
        sht.Range("L" + Hrow).SetValue("१२");
        sht.Range("M" + Hrow).SetValue("१३");
        sht.Range("N" + Hrow).SetValue("१४");
        sht.Range("O" + Hrow).SetValue("१५");
        sht.Range("P" + Hrow).SetValue("१६");
        sht.Range("Q" + Hrow).SetValue("१७");
        sht.Range("R" + Hrow).SetValue("१८");
        sht.Range("S" + Hrow).SetValue("१९");
        sht.Range("T" + Hrow).SetValue("२०");
        sht.Range("U" + Hrow).SetValue("२१");
        sht.Range("V" + Hrow).SetValue("२२");
        sht.Range("W" + Hrow).SetValue("२३");
        sht.Range("X" + Hrow).SetValue("२४");
        sht.Range("Y" + Hrow).SetValue("२५");
        sht.Range("Z" + Hrow).SetValue("२६");
        sht.Range("AA" + Hrow).SetValue("२७");
        sht.Range("AB" + Hrow).SetValue("२८");
        sht.Range("AC" + Hrow).SetValue("२९");
        sht.Range("AD" + Hrow).SetValue("३०");
        sht.Range("AE" + Hrow).SetValue("३१");
        sht.Range("AF" + Hrow).SetValue("३२");
        sht.Range("AG" + Hrow).SetValue("३३");

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

            //***********Amount Details
            DataView dvamount = new DataView(ds.Tables[2]);
            dvamount.RowFilter = "empid=" + ds.Tables[0].Rows[i]["empid"];
            DataSet dsamt = new DataSet();
            dsamt.Tables.Add(dvamount.ToTable());
            for (int k = 0; k < dsamt.Tables[0].Rows.Count; k++)
            {
                sht.Range("G" + row).SetValue(dsamt.Tables[0].Rows[0]["Totalworkingdays"]);
                sht.Range("H" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicpay"]);
                sht.Range("I" + row).SetValue(dsamt.Tables[0].Rows[0]["Basicearnamt"]);
                sht.Range("K" + row).SetValue(dsamt.Tables[0].Rows[0]["Productionamt"]);
                sht.Range("L" + row).SetValue(dsamt.Tables[0].Rows[0]["Incentiveamt"]);

                ////HRA
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "HRA")
                //{
                //    sht.Range("J" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("K" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////Conveyance
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "CONVEYANCE")
                //{
                //    sht.Range("L" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("M" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////EDU_ALW
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "EDU_ALW")
                //{
                //    sht.Range("N" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("O" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}
                ////SPL_ALW
                //if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "SPL_ALW")
                //{
                //    sht.Range("P" + row).SetValue(dsamt.Tables[0].Rows[k]["Actualamt"]);
                //    sht.Range("Q" + row).SetValue(dsamt.Tables[0].Rows[k]["EarnAmt"]);
                //}

                //sht.Range("R" + row).FormulaA1 = "=BU" + row + "+BW" + row + "+BY" + row + "+CA" + row + "+Q" + row + "";
                sht.Range("M" + row).SetValue(dsamt.Tables[0].Rows[k]["Grosssal"]);
                sht.Range("N" + row).SetValue(dsamt.Tables[0].Rows[k]["OTHOURMINUTES"]);
                sht.Range("O" + row).SetValue(dsamt.Tables[0].Rows[k]["OTAMOUNT"]);

                //E.P.F
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.P.F")
                {
                    sht.Range("P" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                if (dsamt.Tables[0].Rows[k]["parametername"].ToString().ToUpper() == "E.S.I")
                {
                    sht.Range("Q" + row).SetValue(dsamt.Tables[0].Rows[k]["Earnamt"]);
                }
                //Advance
                sht.Range("R" + row).SetValue(Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Advanceamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Loanamt"]) + Convert.ToDecimal(dsamt.Tables[0].Rows[k]["Kharchaamt"]));
                sht.Range("S" + row).FormulaA1 = "=P" + row + "+Q" + row + "+R" + row + "";
                sht.Range("U" + row).FormulaA1 = "=M" + row + "+O" + row + "-S" + row + "";

                //sht.Range("Z" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("Z" + row).SetValue(ds.Tables[0].Rows[i]["accountholdername"]);
                sht.Range("AA" + row).SetValue(ds.Tables[0].Rows[i]["Bankname"]);
                sht.Range("AB" + row).SetValue(ds.Tables[0].Rows[i]["Bankacno"]);
                sht.Range("AC" + row).SetValue(ds.Tables[0].Rows[i]["ifsccode"]);
                sht.Range("AD" + row).FormulaA1 = "=U" + row + "";
            }


            row = row + 1;
        }
        //**********
        //******SAVE FILE
        string Path = "";
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("Form12(PcsWise)" + DateTime.Now + "." + Fileextension);
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
    protected void SalarySlip()
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

            SqlCommand cmd = new SqlCommand("PRO_HR_SALARYSLIP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["emp_photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["emp_photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["emp_photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["image"] = img_Byte;
                        }
                    }
                }
                if (DDwagescalculation.SelectedValue == "3")
                {
                    if (DDBranchName.SelectedValue == "2")
                    {
                        Session["rptFilename"] = "Reports/rptsalaryslipPcswiseForBranchPanipat.rpt";
                    }
                    else
                    {
                        Session["rptFilename"] = "Reports/rptsalaryslipPcswise.rpt";
                    }
                }
                else
                {
                    if (DDBranchName.SelectedValue == "2")
                    {
                        Session["rptFilename"] = "Reports/rptsalaryslipmonthlyForBranchPanipat.rpt";
                    }
                    else
                    {
                        Session["rptFilename"] = "Reports/rptsalaryslipmonthly.rpt";
                    }
                }
                Session["dsFilename"] = "~\\ReportSchema\\rptsalaryslipmonthly.xsd";
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
    protected void Form14()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_FORM14", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDBranchName.SelectedValue == "2")
                {
                    Session["rptFilename"] = "Reports/Rptform14_MonthlyForChampoPanipat.rpt";
                }
                else
                {
                    if (DDwagescalculation.SelectedValue == "3")
                    {
                        Session["rptFilename"] = "Reports/RptForm14_Pcswise.rpt";
                    }
                    else
                    {
                        Session["rptFilename"] = "Reports/Rptform14_Monthly.rpt";
                    }
                }
                Session["dsFilename"] = "~\\ReportSchema\\Rptform14.xsd";
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
    protected void CashSalary()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SALARYTRANSFER_EMPBANKDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 2);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["CompanyLogo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["CompanyLogo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["image"] = img_Byte;
                        }
                    }
                }

                Session["rptFilename"] = "Reports/RPT_HR_SALARYTRANSFER_CASHSALARY.rpt";

                Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_CASHSALARY.xsd";
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
    protected void SalaryTransferEmpBankDetail()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SALARYTRANSFER_EMPBANKDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 0);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDreporttype.SelectedValue == "9")
                {
                    SalaryTransferNEFTOtherBankEmp(ds);
                }
                else
                {
                    ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(dr["CompanyLogo"]) != "")
                        {
                            FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                            if (TheFile.Exists)
                            {
                                string img = dr["CompanyLogo"].ToString();
                                img = Server.MapPath(img);
                                Byte[] img_Byte = File.ReadAllBytes(img);
                                dr["image"] = img_Byte;
                            }
                        }
                    }

                    Session["rptFilename"] = "Reports/RPT_HR_SALARYTRANSFER_EMPBANKDETAIL.rpt";

                    Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_EMPBANKDETAIL.xsd";
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
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
    protected void SalaryTransferNEFTOtherBankEmp(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("BankFormatOthers");

            sht.Range("A1:H1").Style.Font.FontSize = 11;
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B1:B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 18.00;
            //
            sht.Range("A1").SetValue("DEBIT ACCOUNT NUMBER");
            sht.Range("B1").SetValue("AMOUNT");
            sht.Range("C1").SetValue("IFSC CODE");
            sht.Range("D1").SetValue("TRANSACTIN PART");
            sht.Range("E1").SetValue("CREDIT ACCOUNT NUMBER");
            sht.Range("F1").SetValue("CUSTOMER NAME");
            sht.Range("G1").SetValue("ADDRESS");
            sht.Range("H1").SetValue("EMP CODE");

            using (var a = sht.Range("A1" + ":H1"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            int Row = 2;

            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("A" + Row + ":H" + Row).Style.Font.FontSize = 11;

                //       "NEFT FM " + ds.Tables[0].Rows[i]["CompanyName"]);

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["CompanyBankAccountNo"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["PayableAmt"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["EmpBankIFSCCode"]);
                sht.Range("D" + Row).SetValue("NEFT FM " + ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["EmpBankACNo"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["EmpBankBranch"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);

                using (var a = sht.Range("A" + Row + ":H" + Row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                
                Row = Row + 1;
            }

            sht.Columns(1, 15).AdjustToContents();

            if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BankFormatOthers:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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
    protected void SalaryTransferEmpBankDetailSame()
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SALARYTRANSFER_EMPBANKDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Departmentid", Departmentid);
            cmd.Parameters.AddWithValue("@Designationid", Designationid);
            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@Monthid", DDMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@Monthname", DDMonth.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Yearid", DDyear.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportType", 1);
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

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDreporttype.SelectedValue == "10")
                {
                    SalaryTransferNEFTSameBankEmp(ds);
                }
                else
                {
                    ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(dr["CompanyLogo"]) != "")
                        {
                            FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                            if (TheFile.Exists)
                            {
                                string img = dr["CompanyLogo"].ToString();
                                img = Server.MapPath(img);
                                Byte[] img_Byte = File.ReadAllBytes(img);
                                dr["image"] = img_Byte;
                            }
                        }
                    }

                    Session["rptFilename"] = "Reports/RPT_HR_SALARYTRANSFER_EMPBANKDETAILSAME.rpt";

                    Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_EMPBANKDETAILSAME.xsd";
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
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
    protected void SalaryTransferNEFTSameBankEmp(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("BankFormatOthers");

            sht.Range("A1:F1").Style.Font.FontSize = 11;
            sht.Range("A1:F1").Style.Font.Bold = true;
            sht.Range("A1:F1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 18.00;
            //
            sht.Range("A1").SetValue("ACCOUNT NUMBER");
            sht.Range("B1").SetValue("TR TYPE");
            sht.Range("C1").SetValue("TR REMARKS");
            sht.Range("D1").SetValue("REMARKS");
            sht.Range("E1").SetValue("AMOUNT");
            sht.Range("F1").SetValue("EMP CODE");
            using (var a = sht.Range("A1" + ":F1"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            int Row = 2;

            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("A" + Row + ":F" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["EmpBankACNo"]);
                sht.Range("B" + Row).SetValue("C");
                sht.Range("C" + Row).SetValue("NEFT FM " + ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["PayableAmt"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);

                using (var a = sht.Range("A" + Row + ":F" + Row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                Row = Row + 1;
            }

            sht.Columns(1, 15).AdjustToContents();

            if (!Directory.Exists(Server.MapPath("~/TempHrexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempHrexcel/"));
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BankFormatUBI:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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