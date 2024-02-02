using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Masters_Campany_frmuploadocs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDDoctype, "select ID,DocumentName From DocumentMaster order by ID", true, "--Plz Select--");
            Fillemployee();
        }
    }
    protected void Fillemployee()
    {
        lblempname.Text = "";
        lblempcode.Text = "";
        if (Request.QueryString["srno"]!=null)
        {
            string str = @"select empname,empcode From Empinfo Where EMpid="+Request.QueryString["srno"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,str);
            if (ds.Tables[0].Rows.Count>0)
            {
                lblempname.Text = ds.Tables[0].Rows[0]["empname"].ToString();
                lblempcode.Text = ds.Tables[0].Rows[0]["empcode"].ToString();
            }

        }
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {
        int Empid = 0;
        if (Request.QueryString["srno"] != null)
        {
            Empid = Convert.ToInt32(Request.QueryString["srno"]);
        }
        lblmsg.Text = "";
        if (Empid > 0)
        {

            Boolean saveflag = false;
            if (!Directory.Exists(Server.MapPath("~/Hrdocs")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Hrdocs"));
            }
            //***************
            if (Docuploads.FileName != "")
            {
                string filename = Path.GetFileName(Docuploads.PostedFile.FileName);
                string[] split = filename.Split('.');
                string fileextension = split[1].ToString();
                string savename = UtilityModule.validateFilename(Empid.ToString() + "#" + DDDoctype.SelectedItem.Text);
                string targetpath = Server.MapPath("~\\Hrdocs\\" + savename);
                string docname = savename + "." + fileextension;
                Docuploads.SaveAs(targetpath);
                //********update in  Table

                string str = "select isnull(submitdocsname,'') as Docsname From Empinfo where empid=" + Empid;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!ds.Tables[0].Rows[0]["Docsname"].ToString().ToUpper().Contains(docname.ToUpper()))
                    {
                        docname = ds.Tables[0].Rows[0]["Docsname"].ToString() == "" ? docname : ds.Tables[0].Rows[0]["Docsname"].ToString() + "," + docname;
                        str = "update empinfo set submitdocsname='" + docname + "' where empid=" + Empid;
                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                        saveflag = true;
                    }
                }
                else
                {
                    str = "update empinfo set submitdocsname='" + docname + "' where empid=" + Empid;
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    saveflag = true;
                }
                //*******
                if (saveflag)
                {
                    lblmsg.Text = "File upload successfully....";
                }
                else
                {
                    lblmsg.Text = "File does not upload because of doc type already uploaded.";
                }

            }
        }
    }
    protected void btndownload_Click(object sender, EventArgs e)
    {
        //******Pending work on filename .....
        if (Request.QueryString["srno"] != null)
        {
            string empid = Request.QueryString["srno"];
            string str = UtilityModule.validateFilename(empid + "#" + DDDoctype.SelectedItem.Text);
            string[] file = str.Split('.');
            string[] name = str.Split('#');
            string filePath = Server.MapPath("~//Hrdocs//" + file[0]);
            if (File.Exists(filePath))
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + name[1] + ".jpg");
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(filePath);
                Response.End();

            }
            else
            {
                lblmsg.Text = "Document does not exists...";
            }

        }
    }
}