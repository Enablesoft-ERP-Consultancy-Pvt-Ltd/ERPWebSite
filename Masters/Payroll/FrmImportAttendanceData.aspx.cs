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
using System.Data.OleDb;

public partial class Masters_Payroll_FrmImportAttendanceData : System.Web.UI.Page
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
        string str = @"select distinct AE.ReceiveNo,AE.ReceiveNo+' / '+cast(left(datename(month,dateadd(month, DATEPART(MONTH, AE.AttendanceDate) - 1, 0)),3) as varchar)+'-'+cast(DATEPART(YEAR, AE.AttendanceDate) as varchar) as ReceiveName 
                      from AttendanceEmp AE where UnitId='"+DDCompanyUnit.SelectedValue+"'";
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
            TDCompanyName.Visible = true;
            TDCompanyUnit.Visible = true;
            TDDeleteBtn.Visible = true;
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
        string StrE2="";
        string PDate="";
        string PMonth="";
        string PYear="";

        string StrA6="";
        string SEmpCode="";

        string Attendate = "";

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
            dt.Columns.Add("EmpCode", typeof(string));
            dt.Columns.Add("AttendanceDate", typeof(string));
            dt.Columns.Add("EmpValue", typeof(string));
            dt.Columns.Add("AttenMonth", typeof(int));
            dt.Columns.Add("AttenYear", typeof(int));


            if (!Directory.Exists(Server.MapPath("~/AttendanceSheet/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/AttendanceSheet/"));
            }
            fileupload.SaveAs(Server.MapPath("~/AttendanceSheet/" + fileupload.FileName.ToString()));
            string filename = Server.MapPath("~/AttendanceSheet/" + fileupload.FileName.ToString());

            string myexceldataquery = "select * from [Sheet1$]";

            //////create our connection strings   
            //string sexcelconnectionstring = string.Empty;
            //string extension = Path.GetExtension(fileupload.PostedFile.FileName);
            //switch (extension)
            //{
            //    case ".xls": //Excel 97-03
            //        sexcelconnectionstring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=YES'";
            //        break;
            //    case ".xlsx": //Excel 07 or higher
            //        sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'";
            //        break;
            //} 
          
            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'";
            
            string ssqlconnectionstring = ErpGlobal.DBCONNECTIONSTRING;
            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
            oledbconn.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            oledbconn.Close();

            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

            oledbcmd.CommandText = "SELECT * From [" + SheetName + "]";

            da.SelectCommand = oledbcmd;

            da.Fill(ds);

            //StrE2 = wsp.Readcell("E2").Trim();
            string colname = ds.Tables[0].Columns[4].ColumnName;
            StrE2 = ds.Tables[0].Rows[0][colname].ToString().Trim();
            if (StrE2 != "")
            {
                StrE2 = StrE2.Substring(0, 39);
                MatchCollection mc = Regex.Matches(StrE2, @"\d+");
                PDate = mc[0].Value;
                PMonth = mc[1].Value;
                PYear = mc[2].Value;
                DateTime dt2 = new DateTime(1, Int32.Parse(PMonth), 1);
                string mm = dt2.ToString("MMM");
                Attendate = mm + "-" + PYear;
            }

            int count = 0;

            for (int rNo = 4; rNo < ds.Tables[0].Rows.Count; rNo++)
            {
                if (ds.Tables[0].Rows[rNo].ToString().Trim() == "")
                {
                    break;
                }
                else
                {
                    count = count + 1;
                    StrA6 = ds.Tables[0].Rows[rNo]["F1"].ToString().Trim();

                    if (StrA6.Trim() != "")
                    {
                        string value = "";
                        for (int i = 2; i <= ds.Tables[0].Columns.Count; i++)
                        {
                            //string col = UtilityModule.GetDataSetColumnName(i,ds);
                            string col = ds.Tables[0].Columns[i-1].ColumnName;
                            value = value + "#";
                            if (col != "" && ds.Tables[0].Rows[rNo - 1][col].ToString().Trim() != "")
                            {
                                value = value + ds.Tables[0].Rows[rNo - 1][col].ToString().Trim(); //Date

                                value = value + "," + (ds.Tables[0].Rows[rNo + 2][col].ToString().Replace("-","").Trim()=="" ? "0" : ds.Tables[0].Rows[rNo + 2][col].ToString().Trim()); //IN-1
                                value = value + "," + (ds.Tables[0].Rows[rNo + 3][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 3][col].ToString().Trim()); //OUT-2
                                value = value + "," + (ds.Tables[0].Rows[rNo + 4][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 4][col].ToString().Trim()); //IN-2
                                value = value + "," + (ds.Tables[0].Rows[rNo + 5][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 5][col].ToString().Trim()); //OUT-1
                                value = value + "," + (ds.Tables[0].Rows[rNo + 6][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 6][col].ToString().Trim()); //Work-1
                                value = value + "," + (ds.Tables[0].Rows[rNo + 7][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 7][col].ToString().Trim()); //OT-1
                                value = value + "," + (ds.Tables[0].Rows[rNo + 8][col].ToString().Replace("-", "").Trim() == "" ? "0" : ds.Tables[0].Rows[rNo + 8][col].ToString().Trim()); //Status

                            }

                        }

                        DataRow dr = dt.NewRow();
                        dr["EmpCode"] = StrA6;
                        dr["AttendanceDate"] = Attendate;
                        dr["EmpValue"] = value.TrimStart('#');
                        dr["AttenMonth"] = PMonth;
                        dr["AttenYear"] = PYear;
                        dt.Rows.Add(dr);

                    }

                    rNo = rNo + 10;
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
                  
                   // SqlTransaction Tran = con.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Pro_SaveAttendanceData",con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;
                        cmd.Parameters.AddWithValue("@dtrecords", dt);
                        cmd.Parameters.Add("@msg", SqlDbType.VarChar,100);
                        cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
                        cmd.Parameters.AddWithValue("@UnitId", DDProcessUnit.SelectedIndex > 0 ? DDProcessUnit.SelectedValue : "0");

                        //SqlParameter[] param = new SqlParameter[4];
                        //param[0] = new SqlParameter("@dtrecords", dt);
                        //param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                        //param[1].Direction = ParameterDirection.Output;
                        //param[2] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                        //param[3] = new SqlParameter("@UnitId", DDProcessUnit.SelectedIndex > 0 ? DDProcessUnit.SelectedValue : "0");
                        ////*************************
                        //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveAttendanceData", param);
                        cmd.ExecuteNonQuery();
                        lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                       // Tran.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                       // Tran.Rollback();
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
                param[0] = new SqlParameter("@ReceiveNo", DDreceiveNo.SelectedValue);
                param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                param[2] = new SqlParameter("@UserId", Session["varuserid"]);
                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                //*************************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteAttendanceData", param);
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
            lblmsg.Text = "Please Select ReceiveNo";
        }
    }
   
}