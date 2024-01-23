using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Barcode_File_frmonlineloominspection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (!Directory.Exists(Server.MapPath("~/Scanfile")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Scanfile"));
        }
        //Upload and save the file
        string csvPath = Server.MapPath("~/Scanfile/") + Path.GetFileName(fileupload.PostedFile.FileName);
        fileupload.SaveAs(csvPath);

        //Create a Datatable
        DataTable dt = new DataTable();
        dt.Columns.Add("Username", typeof(string));
        dt.Columns.Add("TStockNo", typeof(string));
        dt.Columns.Add("Defects", typeof(string));
        dt.Columns.Add("Remark", typeof(string));
        dt.Columns.Add("Progress", typeof(string));
        dt.Columns.Add("Scandate", typeof(string));

        //*******Read the contents of csv file
        string csvData = File.ReadAllText(csvPath);
        //**********execute a loop
        foreach (string row in csvData.Split('\n'))
        {
            if (!string.IsNullOrEmpty(row) && row != "\r")
            {
                dt.Rows.Add();
                int i = 0;
                //Execute a loop over the columns.
                foreach (string cell in row.Split('\t'))
                {
                    dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                    i++;
                }
            }
        }
        var saveflag = 1;
        if (dt.Rows.Count>0)
        {
           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State==ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param=new SqlParameter[4];
                param[0] = new SqlParameter("@dt", dt);
                param[1] = new SqlParameter("@userid", Session["varuserid"]);
                param[2] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
                param[3] = new SqlParameter("@msg",SqlDbType.VarChar,100);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEONLINEINSPECTIONDATA", param);
                lblmsg.Text = param[3].Value.ToString();
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
                con.Dispose();
                con.Close();
            }
            //*********
            ////*********GET DATA NOT DETAIL
            if (saveflag == 1)
            {
                string str = "SELECT StockNo,Msg FROM TEMP_SCANDATAMSG WHERE USERID=" + Session["varuserid"];
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
                     "attachment;filename=ScanLoomDataDetail" + DateTime.Now + ".xls");
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