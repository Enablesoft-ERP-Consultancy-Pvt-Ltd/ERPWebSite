using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.OleDb;

public partial class Masters_Payroll_frmUpdateEMPLOYEESTATUTORYDETAILFromEXCELL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
           
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        // CHECK IF A FILE HAS BEEN SELECTED.
        if ((FileUpload.HasFile))
        {
            lblConfirm.Text = "1";
            if (!Convert.IsDBNull(FileUpload.PostedFile) &
                FileUpload.PostedFile.ContentLength > 0)
            {
                lblConfirm.Text = "2";
                //FIRST, SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
                FileUpload.SaveAs(Server.MapPath(".") + "\\" + FileUpload.FileName);
                lblConfirm.Text = "3";
                SqlBulkCopy oSqlBulk = null;
                lblConfirm.Text = "4";
                // SET A CONNECTION WITH THE EXCEL FILE.
                OleDbConnection myExcelConn = new OleDbConnection
                    ("Provider=Microsoft.ACE.OLEDB.12.0; " +
                        "Data Source=" + Server.MapPath(".") + "\\" + FileUpload.FileName +
                        ";Extended Properties=Excel 12.0;");
                lblConfirm.Text = "5";
                try
                {
                    myExcelConn.Open();
                    lblConfirm.Text = "6";
                    // GET DATA FROM EXCEL SHEET.
                    OleDbCommand objOleDB =
                        new OleDbCommand("SELECT *FROM [Sheet1$]", myExcelConn);

                    lblConfirm.Text = "7";
                    // READ THE DATA EXTRACTED FROM THE EXCEL FILE.
                    OleDbDataReader objBulkReader = null;
                    objBulkReader = objOleDB.ExecuteReader();

                    // SET THE CONNECTION STRING.
                    string sCon = ErpGlobal.DBCONNECTIONSTRING;

                    using (SqlConnection con = new SqlConnection(sCon))
                    {
                        con.Open();

                        // FINALLY, LOAD DATA INTO THE DATABASE TABLE.
                        oSqlBulk = new SqlBulkCopy(con);
                        oSqlBulk.DestinationTableName = "HR_TEMP_UPDATE_EMPLOYEESTATUTORYDETAIL"; // TABLE NAME.
                        oSqlBulk.WriteToServer(objBulkReader);
                    }
                    
                    //*********************
                    //SqlHelper.ExecuteNonQuery(sCon, CommandType.StoredProcedure, "PRO_HR_Update_HR_EMPLOYEESTATUTORYDetail");

                    lblConfirm.Text = "DATA IMPORTED SUCCESSFULLY.";
                    lblConfirm.Attributes.Add("style", "color:green");

                }
                catch (Exception ex)
                {
                    lblConfirm1.Text = ex.Message;
                    lblConfirm.Attributes.Add("style", "color:red");
                }
                finally
                {
                    // CLEAR.
                    oSqlBulk.Close();
                    oSqlBulk = null;
                    myExcelConn.Close();
                    myExcelConn = null;
                }
            }
        }
    }
}