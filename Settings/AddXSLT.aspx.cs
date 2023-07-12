
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
using IExpro.Web.Pages;

public partial class Settings_AddXSLT : BasePage
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

            BindDocument();

        }
    }


    protected void BindDocument()
    {
        rptDoc.DataSource = this.DocSrv.GetDocumentList();
        rptDoc.DataBind();
    }

    protected void rptDoc_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            int _docId = int.Parse(((HiddenField)e.Item.FindControl("hdnDocId")).Value);
            if (e.CommandName == "View")
            {

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
            else if (e.CommandName == "Delete")
            {
                var result = this.DocSrv.DeleteDocument(_docId);
                if (result > 0)
                {
                    BindDocument();

                }

            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        //.
        string fileName = ddlCustomer.SelectedItem.Text.Escape().Replace('.', '_').Replace('/', '_').Replace(' ', '_') + '_' + ddlDocument.SelectedItem.Value.Escape() + ".xslt";
        string filePath = Path.Combine(Server.MapPath("~/App_Data/XSLT/"), fileName);
        DocumentModel doc = new DocumentModel();
        doc.DocType = Convert.ToInt32(ddlDocument.SelectedItem.Value);
        doc.UserId = Convert.ToInt32(ddlCustomer.SelectedItem.Value);
        doc.UserType = (byte)UserType.Customer;
        doc.Title = ddlDocument.SelectedItem.Text + " for Customer " + ddlCustomer.SelectedItem.Text;
        doc.CompanyId = Convert.ToInt32(Session["varCompanyId"]);
        doc.CreatedBy = Convert.ToInt32(Session["varuserid"]);
        doc.CreatedOn = DateTime.Now;
        doc.Content = txtContent.Text;

        if (flpContent.HasFiles)
        {
            flpContent.SaveAs(filePath);
            if (File.Exists(filePath))
            {
                doc.Content = File.ReadAllText(filePath);
                File.Delete(filePath);
            }



        }

        var result = this.DocSrv.AddDocument(doc);
        if (result > 0)
        {
            BindDocument();

        }


    }












}
























