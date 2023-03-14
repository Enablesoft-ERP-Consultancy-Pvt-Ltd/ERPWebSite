using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Net.Mail;
using System.Text;
/// <summary>
/// Summary description for SendpdfViaMail
/// </summary>
public class SendpdfViaMail
{

    public static void sendPdf(DataSet ds, string rptFilename = "", string dsFilename = "", string frommail = "", string Tomail = "", string Subject = "", string Body = "", int partyId = 0, int OrderId = 0, string Bcc = "", string CC = "")
    {
        string pdfFile = "";
        pdfFile = "C:\\Downloads\\" + OrderId + ".pdf";
        ReportDocument crystalreport = new ReportDocument();
        CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
        DataSet ds1 = new DataSet();
       

        crystalreport.Load(rptFilename);
        //crystalreport.Load(Server.MapPath("~/Reports");                 

        ds1 = ds;
        crystalreport.SetDataSource(ds1);
        CrystalReportViewer1.ReportSource = crystalreport;
        crystalreport.ExportToDisk(ExportFormatType.PortableDocFormat, pdfFile);
        //mail();
        //Mail Id Information
        MailMessage m = new MailMessage();
        System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient();
        m.From = new System.Net.Mail.MailAddress(frommail);
        m.To.Add(Tomail);
        if (Bcc != "")
        {
            m.Bcc.Add(Bcc);
        }
        if (CC != "")
        {
            m.CC.Add(CC);
        }
        if (Subject != "")
        {
            m.Subject = Subject;
        }
        if (Body != "")
        {
            m.Body = Body;
        }
        Attachment at = new Attachment(pdfFile);
        m.Attachments.Add(at);
        sc.Host = "smtp.gmail.com";
        sc.EnableSsl = true;
        //sc.UseDefaultCredentials = false;
        sc.Credentials = new System.Net.NetworkCredential("surendrapal848@gmail.com", "");
        sc.Send(m);
        m.Dispose();
        //
        CrystalReportViewer1.Dispose();
        CrystalReportViewer1 = null;
        if (crystalreport != null && crystalreport.IsLoaded != null)
        {
            crystalreport.Close();
            crystalreport.Dispose();
            crystalreport = null;
            GC.Collect();
        }

        File.Delete(pdfFile);
    }



}