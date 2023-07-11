using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

public partial class ViewReport : System.Web.UI.Page
{
    ReportDocument RD = new ReportDocument();
    DataSet ds = new DataSet();
    public static string Export = "Y";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }

        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "SetBrowserMode();", true);
        
        string rptFile = Server.MapPath(Session["rptFileName"].ToString());
        //Written by Jitendra for file name in QP
        if (Request.QueryString["file"] != null)
        {
            rptFile = Server.MapPath(Request.QueryString["file"].ToString());
        }
        RD.Load(rptFile);
        if (rptFile == Server.MapPath("rptFile"))
        {
            RD.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
        }
        LblReportName.Text = Session["rptFileName"].ToString();
        ds = (DataSet)Session["GetDataset"];
        string dsFile = Session["dsFileName"].ToString();
        string dsPath = Server.MapPath(dsFile);
        ds.WriteXmlSchema(dsPath);
        RD.SetDataSource(ds);
        CrystalReportViewer1.ReportSource = RD;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.DataBind();
        //Get Query string value for export
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
        //End*************************
        //Export
        if (Export == "N")
        {
            switch (variable.ReportWithpdf)
            {
                case "1":
                    string strFileName = "";
                    string file = "";
                    string Report = rptFile.Substring(rptFile.LastIndexOf("\\") + 1).Replace(".rpt", "");
                    file = Report + ".pdf";
                    String tmpfoldername = "TempFiles";
                    //tmpfoldername = tmpfoldername;
                    string repfilename = @tmpfoldername + "/" + file;
                    strFileName = HttpContext.Current.Server.MapPath(tmpfoldername);
                    strFileName = Path.Combine(strFileName, file);
                    RD.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                    RD.Close();
                    RD.Dispose();
                    RD = null;
                    GC.Collect();
                    CrystalReportViewer1.Dispose();
                    CrystalReportViewer1 = null;                    
                    Response.Redirect(repfilename + "?r_nd" + DateTime.Now.Ticks);
                    break;
                default:
                    break;
            }

        }
        //***********New Changes on 20-Dec-2016       
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
                    if (RD != null && RD.IsLoaded != null)
                    {
                        RD.Close();
                        RD.Dispose();
                        RD = null;
                        GC.Collect();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}