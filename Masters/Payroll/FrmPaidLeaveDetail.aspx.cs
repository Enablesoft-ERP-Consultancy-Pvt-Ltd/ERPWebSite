using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Payroll_FrmPaidLeaveDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDleavetype, "SELECT LEAVEID,NAME + space(5) + CODE FROM HR_LEAVETYPE(Nolock) ", true, "--Plz Select--");
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            DDleavetype.SelectedValue = "3";
        }
    }
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        fillemployee();
    }

    protected void fillemployee()
    {
        try
        {
            txtempname.Text = "";
            hnempid.Value = "0";
            string str = @"SELECT EI.EMPID,EI.EMPNAME FROM EMPINFO EI With (nolock) INNER JOIN HR_EMPLOYEEINFORMATION HEI With (nolock) ON EI.EMPID=HEI.EMPID
                         AND EI.EMPCODE='" + txtempcode.Text + "' AND HEI.RESIGNSTATUS=0";

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnempid.Value = ds.Tables[0].Rows[0]["empid"].ToString();
                txtempname.Text = ds.Tables[0].Rows[0]["empname"].ToString();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Emp. Code does not exists in ERP or Status is Resigned !!!')", true);
                txtempcode.Text = "";
            }
            TxtNoofLeave.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('" + ex.Message + "')", true);
            throw;
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = 0;
            param[1] = new SqlParameter("@empid", hnempid.Value);
            param[2] = new SqlParameter("@LeaveTypeid", DDleavetype.SelectedValue);
            param[3] = new SqlParameter("@Date", txtdate.Text);
            param[4] = new SqlParameter("@NOOFLEAVE", TxtNoofLeave.Text);
            param[5] = new SqlParameter("@REMARK", TxtRemark.Text);
            param[6] = new SqlParameter("@UserID", Session["varuserid"]);
            param[7] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVEPAIDLEAVEDETAIL", param);
            Tran.Commit();
            if (param[8].Value.ToString() != "")
            {
                lblmsg.Text = param[8].Value.ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Data Successfully Saved !!!')", true);
                Refreshcontrols();
            }

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
    private void Refreshcontrols()
    {
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtempcode.Text = "";
        txtempname.Text = "";
        hnempid.Value = "0";
        TxtNoofLeave.Text = "";
        TxtRemark.Text = "";
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GET_PAIDLEAVEDETAIL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@EmpCode", txtempcode.Text);
            cmd.Parameters.AddWithValue("@LeaveTypeID", DDleavetype.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PAIDLEAVEDETAIL");

                sht.Range("A1:E1").Style.Font.FontSize = 11;
                sht.Range("A1:E1").Style.Font.Bold = true;
                sht.Range("A1:D1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Row(1).Height = 18.00;
                //
                sht.Range("A1").SetValue("CODE");
                sht.Range("B1").SetValue("NAME");
                sht.Range("C1").SetValue("DATE");
                sht.Range("D1").SetValue("LEAVE TYPE");
                sht.Range("E1").SetValue("No OF LEAVE");
                using (var a = sht.Range("A1"+ ":E1"))
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
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;

                    sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["EMPCODE"]);
                    sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["DATE"]);
                    sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["LEAVETYPENAME"]);
                    sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["NOOFLEAVE"]);
                    
                    using (var a = sht.Range("A" + Row + ":E" + Row))
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
                string filename = UtilityModule.validateFilename("PAIDLEAVEDETAIL:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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
}