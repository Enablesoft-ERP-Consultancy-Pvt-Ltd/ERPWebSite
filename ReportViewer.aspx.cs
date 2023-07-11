using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class ReportViewer : System.Web.UI.Page
{
    ReportDocument cryRpt = new ReportDocument();
    public static string Export = "Y";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            
            Response.Redirect("~/Login.aspx");
        }
        if (Session["ReportPath"] != "")
        {
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "SetBrowserMode();", true);

            string function = Session["CommanFormula"].ToString();
            string path = Session["ReportPath"].ToString();
            LblFormula.Visible = true;
            LblPath.Visible = true;
            LblPath.Text = path;
            string varxyz = ConfigurationManager.AppSettings["varPasswordName"];
            string UserName = ConfigurationManager.AppSettings["VaruserName"];
            FileInfo ReportPath = new FileInfo(Server.MapPath(path));

            if (ReportPath.Exists)
            {
                
                cryRpt.Load(Server.MapPath(path));
                TableLogOnInfos tblLogonInfos = new TableLogOnInfos();
                TableLogOnInfo TblLogonInfo = new TableLogOnInfo();

                ConnectionInfo ConInfo = new ConnectionInfo();
                Tables Tbls;                
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                ConInfo.ServerName = con.DataSource;
                ConInfo.DatabaseName = con.Database;
                ConInfo.UserID = UserName;
                ConInfo.Password = varxyz;

                Tbls = cryRpt.Database.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table tbl1 in Tbls)
                {
                    AssignTableConnection(tbl1, ConInfo);
                }
                cryRpt.DataDefinition.FormulaFields.ToString();
                cryRpt.RecordSelectionFormula = function;
                //cryRpt.SetDatabaseLogon(UserName, varxyz, con.DataSource, con.Database);
                CrystalReportViewer1.ReportSource = cryRpt;
                CrystalReportViewer1.RefreshReport();
                //*******************
                if (Request.QueryString["Export"] != null)
                {
                    Export = Request.QueryString["Export"].ToString();
                }
                else
                {
                    switch (variable.ReportWithpdf)
                    {
                        case "1":
                            Export = "N";
                            break;
                        default:
                            Export = "Y";
                            break;
                    }
                }
                //*****************
                //Export
                if (Export == "N")
                {
                    switch (variable.ReportWithpdf)
                    {
                        case "1":
                            string strFileName = "";
                            string file = "";
                            string Report = path.Substring(path.LastIndexOf("\\") + 1).Replace(".rpt", "");
                            Report = Report.Substring(path.LastIndexOf("/") + 1);
                            file = Report + ".pdf";
                            String tmpfoldername = "TempFiles";
                            //tmpfoldername = tmpfoldername;
                            string repfilename = @tmpfoldername + "/" + file;
                            strFileName = HttpContext.Current.Server.MapPath(tmpfoldername);
                            strFileName = Path.Combine(strFileName, file);
                            cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            cryRpt.Close();
                            cryRpt.Dispose();
                            cryRpt = null;
                            GC.Collect();
                            CrystalReportViewer1.Dispose();
                            CrystalReportViewer1 = null;
                            Response.Redirect(repfilename + "?r_nd" + DateTime.Now.Ticks);
                            break;
                        default:
                            break;
                    }

                }
            }
            else
            {
                Response.Write("Report Path Not Exists");
            }
        }
    }
    private void AssignTableConnection(CrystalDecisions.CrystalReports.Engine.Table table, ConnectionInfo connection)
    {
        // Cache the logon info block
        TableLogOnInfo logOnInfo = table.LogOnInfo;

        connection.Type = logOnInfo.ConnectionInfo.Type;

        // Set the connection
        logOnInfo.ConnectionInfo = connection;

        // Apply the connection to the table!
        //table.ApplyLogOnInfo(logOnInfo);//commented out Aug 2nd 2010 MPP for subreports seems that this actually overrides the command so if you have a different command in the subreport it uses the command from the main report

        table.LogOnInfo.ConnectionInfo.DatabaseName = connection.DatabaseName;
        table.LogOnInfo.ConnectionInfo.ServerName = connection.ServerName;
        table.LogOnInfo.ConnectionInfo.UserID = connection.UserID;
        table.LogOnInfo.ConnectionInfo.Password = connection.Password;
        table.LogOnInfo.ConnectionInfo.Type = connection.Type;

        logOnInfo = table.LogOnInfo;
        logOnInfo.ConnectionInfo = connection;
        table.ApplyLogOnInfo(logOnInfo);
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        if (Export == "Y")
        {
            switch (variable.ReportWithpdf)
            {
                case "0":
                    CrystalReportViewer1.Dispose();
                    CrystalReportViewer1 = null;
                    if (cryRpt != null && cryRpt.IsLoaded != null)
                    {
                        cryRpt.Close();
                        cryRpt.Dispose();
                        cryRpt = null;
                        GC.Collect();
                    }
                    break;

                default:
                    break;
            }
        }
        //CrystalReportViewer1.Dispose();
        //CrystalReportViewer1 = null;
        //if (cryRpt != null && cryRpt.IsLoaded != null)
        //{
        //    cryRpt.Close();
        //    cryRpt.Dispose();
        //    cryRpt = null;
        //    GC.Collect();
        //}

    }
}
