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

public partial class Masters_Payroll_frmMisspunchDetail : System.Web.UI.Page
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
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "Select Comp Name");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
        }
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_GETDATAFORMISSPUNCHDETAIL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            Dgdetail.DataSource = ds.Tables[0];
            Dgdetail.DataBind();

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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //*************Datatable
        DataTable dt = new DataTable();
        dt.Columns.Add("empid", typeof(int));
        dt.Columns.Add("empcode", typeof(string));
        dt.Columns.Add("Manualdate", typeof(DateTime));
        dt.Columns.Add("intime", typeof(string));
        dt.Columns.Add("outtime", typeof(string));
        dt.Columns.Add("Insecond", typeof(string));
        dt.Columns.Add("Outsecond", typeof(string));
        dt.Columns.Add("shiftintime", typeof(string));
        dt.Columns.Add("shiftouttime", typeof(string));

        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            Label lblempid = (Label)Dgdetail.Rows[i].FindControl("lblempid");
            Label lblempcode = (Label)Dgdetail.Rows[i].FindControl("lblempcode");
            Label lbldate = (Label)Dgdetail.Rows[i].FindControl("lbldate");
            TextBox txtintime = (TextBox)Dgdetail.Rows[i].FindControl("txtintime");
            TextBox txtouttime = (TextBox)Dgdetail.Rows[i].FindControl("txtouttime");
            Label lblinsecond = (Label)Dgdetail.Rows[i].FindControl("lblinsecond");
            Label lbloutsecond = (Label)Dgdetail.Rows[i].FindControl("lbloutsecond");
            Label lblshiftintime = (Label)Dgdetail.Rows[i].FindControl("lblshiftintime");
            Label lblshiftouttime = (Label)Dgdetail.Rows[i].FindControl("lblshiftouttime");

            DataRow dr = dt.NewRow();
            dr["empid"] = lblempid.Text;
            dr["empcode"] = lblempcode.Text;
            dr["Manualdate"] = lbldate.Text;
            dr["intime"] = txtintime.Text;
            dr["outtime"] = txtouttime.Text;
            dr["Insecond"] = lblinsecond.Text;
            dr["Outsecond"] = lbloutsecond.Text;
            dr["shiftintime"] = lblshiftintime.Text;
            dr["shiftouttime"] = lblshiftouttime.Text;

            dt.Rows.Add(dr);

        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altval", "alert('No Data fill to Save in data grid.')", true);
            return;
        }
        //**************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVEMANUALATTENDANCE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300000;

            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "')", true);
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
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
    private void ExcelReportData()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_GETDATAFORMISSPUNCHDETAILExcelReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@empcode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);           

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");

                sht.Range("A1:J1").Merge();
                sht.Range("A1").Value = "MISS PUNCH DETAILS";                
                sht.Range("A3:J3").Merge();
                sht.Range("A3").Value = "Date From :  " + txtfromdate.Text + " " + "To" + " " + txttodate.Text;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:J3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:J3").Style.Alignment.SetWrapText();
                sht.Range("A1:J3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:J3").Style.Font.FontSize = 10;
                sht.Range("A1:J3").Style.Font.Bold = true;
                //*******Header
                sht.Range("A4").Value = "SR No";
                sht.Range("B4").Value = "Emp Code";
                sht.Range("C4").Value = "Emp Name";
                sht.Range("D4").Value = "Designation";
                sht.Range("E4").Value = "Date";
                sht.Range("F4").Value = "Day Name";
                sht.Range("G4").Value = "Shift In Time";
                sht.Range("H4").Value = "Shift Out Time";
                sht.Range("I4").Value = "In Time (hh:mm)";
                sht.Range("A4:J4").Style.Font.Bold = true;

                int row = 4;               
                int i = 0;                
                int rowfrom = 5;
                int srno = 0;

                row = row + 1;

                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    srno = srno + 1;
                    sht.Range("A" + row).SetValue(srno);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Designation"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DaysName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShiftInTime"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ShiftOutTime"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["InTime"]);                    

                    row = row + 1;                    
                }
                //sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;                

                row = row + 1;

                ////*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":J" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("MissPunchDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        finally
        {
            con.Close();
            con.Dispose();
        }
        
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        ExcelReportData();
    }
}