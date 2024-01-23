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

using System.Diagnostics;
//using IronPdf;
//using ImageMagick;

public partial class _Default : System.Web.UI.Page
{
    ReportDocument RD = new ReportDocument();
    DataSet ds = new DataSet();
    public static string Export = "Y";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }

        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "SetBrowserMode();", true);

        string rptFile = Server.MapPath(Session["rptFileName"].ToString());
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
                    String tmpfolder = "TempFiles";
                    String OUTtmpfolder = "TemFiles";
                    //tmpfoldername = tmpfoldername;
                    string repfilename = tmpfolder + "/" + file;
                    var tmpfoldername = HttpContext.Current.Server.MapPath(tmpfolder);
                    var outtmpfoldername = HttpContext.Current.Server.MapPath(OUTtmpfolder);
                    strFileName = Path.Combine(tmpfoldername, file);
                    var outFile = DateTime.Now.Ticks.ToString() + ".jpg";
                    var stroutFileName = Path.Combine(outtmpfoldername, outFile);

                    RD.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                    RD.Close();
                    RD.Dispose();
                    RD = null;
                    GC.Collect();
                    CrystalReportViewer1.Dispose();
                    CrystalReportViewer1 = null;
                    var appPath = HttpContext.Current.Server.MapPath("~/bin");
                    try
                        {

                            ProcessStartInfo StartInfo = new ProcessStartInfo();
                            StartInfo.FileName=appPath + "\\magick\\convert.exe";
                                //WorkingDirectory = @"C:\Program Files\ImageMagick-6.9.0-Q16\",
                            StartInfo.Arguments = "convert -density 150 " + strFileName + " " + stroutFileName + "";
                                StartInfo.UseShellExecute = false;

                                StartInfo.RedirectStandardOutput = true;
                                StartInfo.RedirectStandardError = true;
                                StartInfo.CreateNoWindow = true;
                               using (Process proc = System.Diagnostics.Process.Start(StartInfo))
        {

            // This needs to be before WaitForExit() to prevent deadlocks, for details: 
            // http://msdn.microsoft.com/en-us/library/system.diagnostics.process.standardoutput%28v=VS.80%29.aspx
            proc.StandardError.ReadToEnd();

            // Wait for exit
            proc.WaitForExit();
         //   Console.Read();


        }
                        
                      //  var test = proc.StartInfo.Arguments.ToString();
                      //  proc.Start(test);

                      //proc.WaitForExit();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message.ToString());
                    }
                    //var Renderer = new IronPdf.ChromePdfRenderer();

                    //PdfDocument Pdf = new PdfDocument(strFileName);

                    //System.Drawing.Bitmap[] pageImages = Pdf.ToBitmap();

                    //Pdf.RasterizeToImageFiles(tmpfoldername + @"thumbnail_*.jpg");
                 
                 //   Response.Redirect(OUTtmpfolder + "/" + outFile + "?r_nd" + DateTime.Now.Ticks);
                    string filepath = OUTtmpfolder + "/" + outFile;
                    Response.ContentType = "image/jpg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filepath + "\"");
                    Response.TransmitFile(Server.MapPath(filepath));
                    Response.End(); 
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