using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;

public partial class FrmDataBaseBackUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnDataBaseBackUp_Click(object sender, EventArgs e)
    {
        //Enter destination directory where backup file stored
        String[] strDrives = Directory.GetLogicalDrives();
        int a = strDrives.Count();
        string destdir = "";
        if (strDrives.Count() == 1)
        {
            destdir = "" + strDrives[0] + "DataBaseBackUp";
        }
        else
        {
            destdir = "" + strDrives[1] + "DataBaseBackUp";
        }
        string destdirNew = destdir;
        string VarDataBaseName = ConfigurationManager.AppSettings["VarBaseName"];
        //Check that directory already there otherwise create 
        if (!System.IO.Directory.Exists(destdir))
        {
            System.IO.Directory.CreateDirectory("" + destdir + "");
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Left(CompanyName,4) From Master_Company");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            destdir = destdir + "\\" + Ds.Tables[0].Rows[0][0] + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".bak";
        }
        try
        {
            string Str = "backup database  " + VarDataBaseName + " to disk='" + destdir + "'";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Backup database successfully and goes to path " + destdirNew + "..!');", true);
            LblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
    }
}