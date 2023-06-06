using IExpro.Core.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    protected string getXmlString(string from, string to)
    {
        string xmlString = string.Empty;

        SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        SqlParameter[] param = new SqlParameter[4];

        param[0] = new SqlParameter("@from", SqlDbType.VarChar);
        param[0].Direction = ParameterDirection.Input;
        param[0].Value = from;

        param[1] = new SqlParameter("@to", SqlDbType.VarChar);
        param[1].Direction = ParameterDirection.Input;
        param[1].Value = to;


        param[2] = new SqlParameter("@flag", SqlDbType.Int);
        param[2].Direction = ParameterDirection.Input;
        param[2].Value = Convert.ToInt32(ddlYarnType.SelectedValue);


        param[3] = new SqlParameter("@CompanyId", SqlDbType.VarChar);
        param[3].Direction = ParameterDirection.Input;
        param[3].Value = Convert.ToInt32(Session["CurrentWorkingCompanyID"]);



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

        string xsltName = string.Empty;
        switch (Convert.ToInt32(ddlYarnType.SelectedValue))
        {
            case 1:
                xsltName = "YarnInspection.xslt";
                break;
            case 2:
                xsltName = "DyedYarnInspection.xslt";
                break;
            case 3:
                xsltName = "FabricInspection.xslt";
                break;
            case 4:
                xsltName = "CartonInspection.xslt";
                break;

            case 5:
                xsltName = "PapertubeInspection.xslt";
                break;
            case 6:
                xsltName = "PolybagInspection.xslt";
                break;
        }


        string _from = txtFrom.Text.Trim();
        string _to = txtTo.Text.Trim();
        // this is being read from the same folder as this page is in.(only for demo purpose)
        // In real applications this xml might be coming from some external source or database.
        string xmlString = this.getXmlString(_from, _to);

        if (!string.IsNullOrEmpty(xmlString))
        {
            lblMessage.Text = "Data for Date Range from " + _from + "to " + _to;
            // Define the contents of the XML control
            Xml1.DocumentContent = xmlString;
            XsltArgumentList arguments = new XsltArgumentList();

            Xml1.TransformArgumentList = arguments;
            // Specify the XSL file to be used for transformation.
            Xml1.TransformSource = Server.MapPath("~/Content/XSLT/" + xsltName);
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


            string xsltName = string.Empty;
            string filename = string.Empty;
            switch (Convert.ToInt32(ddlYarnType.SelectedValue))
            {
                case 1:
                    xsltName = "YarnInspection.xslt";
                    filename = "YarnInspection.xls";
                    break;
                case 2:
                    xsltName = "DyedYarnInspection.xslt";
                    filename = "DyedYarnInspection.xls";
                    break;
                case 3:
                    xsltName = "FabricInspection.xslt";
                    filename = "FabricInspection.xls";
                    break;
                case 4:
                    xsltName = "CartonInspection.xslt";
                    filename = "CartonInspection.xls";
                    break;

                case 5:
                    xsltName = "PapertubeInspection.xslt";
                    filename = "PapertubeInspection.xls";
                    break;
                case 6:
                    xsltName = "PolybagInspection.xslt";
                    filename = "PolybagInspection.xls";
                    break;
            }

            // this is being read from the same folder as this page is in.(only for demo purpose)
            // In real applications this xml might be coming from some external source or database.
            string xmlString = this.getXmlString(_from, _to);

            if (!string.IsNullOrEmpty(xmlString))
            {
                lblMessage.Text = "Data for Date Range from " + _from + "to " + _to + "have been downloaded";
                string xsltText = Server.MapPath("~/Content/XSLT/" + xsltName);
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "";
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");

                XsltArgumentList arguments = new XsltArgumentList();
                // Creating XSLCompiled object
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(xsltText.ToString());
                using (StringWriter results = new StringWriter())
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString.ToString())))
                    {
                        transform.Transform(reader, arguments, results);
                    }
                    response.Write(results.ToString());
                    response.Flush(); // Sends all currently buffered output to the client.
                    response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
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