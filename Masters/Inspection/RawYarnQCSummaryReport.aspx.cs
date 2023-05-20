using IExpro.Core.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

public partial class Master_Inspection_RawYarnQCSummaryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string getXmlString(string from, string to)
    {
        string xmlString = string.Empty;
     
        SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        SqlParameter[] param = new SqlParameter[3];

        param[0] = new SqlParameter("@from", SqlDbType.VarChar);
        param[0].Direction = ParameterDirection.Input;
        param[0].Value = from;

        param[1] = new SqlParameter("@to", SqlDbType.VarChar);
        param[1].Direction = ParameterDirection.Input;
        param[1].Value = to;


        param[2] = new SqlParameter("@flag", SqlDbType.Int);
        param[2].Direction = ParameterDirection.Input;
        param[2].Value = 1;



        using (var readerXl = SqlHelper.ExecuteXmlReader(conn, CommandType.StoredProcedure, "RawYarnInspectionProc", param))
        {
            while (readerXl.Read())
            {
                xmlString = readerXl.ReadOuterXml();
            }
        }

        return xmlString;


    }



    protected void btnPriview_Click(object sender, EventArgs e)
    {
        Xml1.Visible = true;
        ltlTutorial.Visible = false;

        string _from = txtFrom.Text.Trim();
        string _to = txtTo.Text.Trim();
        // this is being read from the same folder as this page is in.(only for demo purpose)
        // In real applications this xml might be coming from some external source or database.
        string xmlString = this.getXmlString(_from,_to);

        if (!string.IsNullOrEmpty(xmlString))
        {
            // Define the contents of the XML control
            Xml1.DocumentContent = xmlString;
            XsltArgumentList arguments = new XsltArgumentList();

            Xml1.TransformArgumentList = arguments;
            // Specify the XSL file to be used for transformation.
            Xml1.TransformSource = Server.MapPath("~/Content/XSLT/YarnInspection.xslt");
        }
        else
        {

            lblMessage.Text = "There is no data available in the selected date range !";
        }
    }

    protected void tblDownload_Click(object sender, EventArgs e)
    {


        try
        {


            string _from = txtFrom.Text.Trim();
            string _to = txtTo.Text.Trim();
            // this is being read from the same folder as this page is in.(only for demo purpose)
            // In real applications this xml might be coming from some external source or database.
            string xmlText = this.getXmlString(_from, _to);
            

            if(!string.IsNullOrEmpty(xmlText))
            {
                string xsltText = Server.MapPath("~/Content/XSLT/YarnInspection.xslt");
                XsltArgumentList arguments = new XsltArgumentList();
                var htmlOutput = XmlHelper.XmlWriterFunction(xmlText.ToString(), arguments, xsltText);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "UTF-8";


                // HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.doc", HttpUtility.UrlEncode("Invoice-" + DateTime.Now.ToShortDateString(), System.Text.Encoding.UTF8)));
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode("RawYarnInspection-" + DateTime.Now.ToShortDateString(), System.Text.Encoding.UTF8)));
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

                HttpContext.Current.Response.Write(htmlOutput.ToString());
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();

            }
            else
            {

                lblMessage.Text = "There is no data available in the selected date range !";
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}