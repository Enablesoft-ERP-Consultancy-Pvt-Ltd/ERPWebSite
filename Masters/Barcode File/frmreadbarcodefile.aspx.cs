using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Barcode_File_frmreadbarcodefile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(Server.MapPath("~/Scanfile")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Scanfile"));
        }
        //Upload and save the file
        string csvPath = Server.MapPath("~/Scanfile/") + Path.GetFileName(fileupload.PostedFile.FileName);
        fileupload.SaveAs(csvPath);

        //Create a DataTable.

        DataTable dt = new DataTable();
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("Unit", typeof(string));
        dt.Columns.Add("EntryType", typeof(string));
        dt.Columns.Add("JobName", typeof(string));
        dt.Columns.Add("EMpcode", typeof(string));
        //dt.Columns.Add("Empcode1", typeof(string));
        dt.Columns.Add("SizeW", typeof(string));
        dt.Columns.Add("SizeL", typeof(string));
        dt.Columns.Add("Barcodescan", typeof(string));
        dt.Columns.Add("Defects", typeof(string));
        dt.Columns.Add("CheckBy", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("IssueDate", typeof(string));
        dt.Columns.Add("ReqDate", typeof(string));


        dt.Columns["Remarks"].DefaultValue = "";
        dt.Columns["CheckBy"].DefaultValue = "";
        dt.Columns["Defects"].DefaultValue = "";
        dt.Columns["IssueDate"].DefaultValue = System.DateTime.Now.ToString("dd-MMM-yyyy");
        dt.Columns["ReqDate"].DefaultValue = System.DateTime.Now.ToString("dd-MMM-yyyy");

        //Read the contents of CSV file.
        string csvData = File.ReadAllText(csvPath);
        string EntryType = "";
        //Execute a loop over the rows.
        foreach (string row in csvData.Split('\n'))
        {
            if (!string.IsNullOrEmpty(row) && row != "\r")
            {
                dt.Rows.Add();
                int i = 0;

                //Execute a loop over the columns.
                foreach (string cell in row.Split('\t'))
                {
                    if (cell.ToUpper() == "ISSUE")
                    {
                        EntryType = "ISSUE";
                    }
                    if (EntryType == "ISSUE")
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                        if (i == 7)
                        {
                            i = 10;
                        }
                        i++;
                    }
                    else
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                        i++;
                    }

                }
            }
        }
        //************Insert into Sql
        var saveflag = 1;
        if (dt.Rows.Count > 0)
        {
            //**********DELETE TEMP DATA
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE FROM TEMP_SCANDATAMSG WHERE USERID=" + Session["varuserid"] + "");
            //**********
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                DataTable dtdistinct = dt.DefaultView.ToTable(true, "EntryType");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "ENTRYTYPE";
                DataTable dtdistinct1 = dv1.ToTable();

                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "EntryType='" + dr["EntryType"] + "'";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());
                    //********EXECUTE PROC
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@dt", ds1.Tables[0]);
                    param[1] = new SqlParameter("@userid", Session["varuserid"]);
                    param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
                    param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[3].Direction = ParameterDirection.Output;
                    param[4] = new SqlParameter("@EntryType", dr["EntryType"]);
                    //*****
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SAVESCANDATA", param);
                    lblmsg.Text = param[3].Value.ToString();
                    //**********
                }
                Tran.Commit();

            }
            catch (Exception ex)
            {
                saveflag = 0;
                lblmsg.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            //*********
            ////*********GET DATA NOT DETAIL
            if (saveflag == 1)
            {
                string str = "SELECT StockNo,JobName,Msg,Issue_Receive FROM TEMP_SCANDATAMSG WHERE USERID=" + Session["varuserid"];
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Export to excel
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                     "attachment;filename=ScanDataDetail" + DateTime.Now + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    GridView1.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    lblmsg.Text = "FILE UPLOADED SUCCESSFULLY.";
                }

            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('No records exists in a File.')", true);
        }

    }
}