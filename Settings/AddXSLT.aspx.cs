
using IExpro.Core.Common;
using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using IExpro.Core.Models.Document;

using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Configuration;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml;
using System.Text;

public partial class Settings_AddXSLT : System.Web.UI.Page
{
    ICommonService CommSrv;
    int ClientId;
    IDocumentService DocSrv;

    public Settings_AddXSLT()
    {
        this.CommSrv = new CommonService(new UnitOfWork());
        this.DocSrv = new DocumentService(new UnitOfWork());

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }


        if (!IsPostBack)
        {
            ClientId = Convert.ToInt32(Session["varcompanyNo"].ToString());
            ddlCustomer.DataSource = this.CommSrv.GetCustomerList(ClientId);
            ddlCustomer.DataTextField = "ItemName";
            ddlCustomer.DataValueField = "ItemId";
            ddlCustomer.DataBind();

            ddlDocument.DataSource = this.CommSrv.GetDocTypeList(ClientId);
            ddlDocument.DataTextField = "ItemName";
            ddlDocument.DataValueField = "ItemId";
            ddlDocument.DataBind();

            rptDoc.DataSource = this.DocSrv.GetDocumentList();
            rptDoc.DataBind();


        }
    }

    protected void rptDoc_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "View")
            {
                int _docId = int.Parse(((HiddenField)e.Item.FindControl("hdnDocId")).Value);
                var xsltText = this.DocSrv.GetDocument(_docId);
                var xmlText = XElement.Load(Server.MapPath("~/App_Data/XML/InvoiceData.xml"));
                string signaturePath = CommonHelper.GetURI() + "/Images/signature/client-" + ClientId + ".png";
                xmlText.Descendants("InvoiceItem").SingleOrDefault().Add(new XElement("signature", signaturePath));
                XsltArgumentList arguments = new XsltArgumentList();
                arguments.AddExtensionObject("pda:MyUtils", new MathHelper());
                var htmlOutput = XmlHelper.XmlWriterFunction(xmlText.ToString(), arguments, xsltText);
                lblText.Text = htmlOutput;



                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.Charset = "UTF-8";
                //HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.doc", HttpUtility.UrlEncode("Invoice-" + DateTime.Now.ToShortDateString(), System.Text.Encoding.UTF8)));
                ////HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode("Invoice-" + DateTime.Now.ToShortDateString(), System.Text.Encoding.UTF8)));
                //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                ////HttpContext.Current.Response.ContentType = "application/ms-excel";
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
                //HttpContext.Current.Response.Write(htmlOutput.ToString());
                //HttpContext.Current.Response.End();
                //HttpContext.Current.Response.Close();


            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        DocumentModel doc = new DocumentModel();
        doc.DocType = Convert.ToInt32(ddlDocument.SelectedItem.Value);
        doc.UserId = Convert.ToInt32(ddlCustomer.SelectedItem.Value);
        doc.Title = ddlDocument.SelectedItem.Value + "for Customer" + ddlCustomer.SelectedItem.Text;
        doc.CompanyId = Convert.ToInt32(Session["varCompanyId"]);
        doc.CreatedBy = Convert.ToInt32(Session["varuserid"]);
        doc.CreatedOn = DateTime.Now;
        doc.Content = txtContent.Text;

        if (flpContent.HasFiles)
        {
            foreach (HttpPostedFile postedFile in flpContent.PostedFiles)
            {
                if (postedFile.ContentLength > 0) //if file not empty
                {
                    string contentType = postedFile.ContentType;
                    using (Stream fs = flpContent.PostedFile.InputStream)
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        }
                    }
                }
            }
        }

        this.DocSrv.AddDocument(doc);



    }












}
























