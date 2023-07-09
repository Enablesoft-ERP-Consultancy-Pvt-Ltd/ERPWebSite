using IExpro.Core.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace IExpro.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private IUnitOfWork IU { get; set; }
        private IInvoiceRepository InvoiceRepo { get; set; }

        public InvoiceService(IUnitOfWork _IU)
        {
            this.IU = _IU;
            this.InvoiceRepo = IU.InvoiceRepo;
        }

        public string GetInvoiceDetail(int _clientId, int _invoiceId, short docType, int userId, short userType)
        {
            try
            {
                string signaturePath = CommonHelper.GetURI() + "/Images/signature/client-" + _clientId + ".png";
                var xmlText = this.InvoiceRepo.GetInvoiceDetail(_invoiceId);

                userId = Convert.ToInt32(xmlText.Descendants("CustomerId").SingleOrDefault().Value);
                xmlText.Descendants("InvoiceItem").SingleOrDefault().Add(new XElement("signature", signaturePath));
                string xsltText = this.InvoiceRepo.GetXSLTDetail(_clientId, docType, userId, userType);
                XsltArgumentList arguments = new XsltArgumentList();
                arguments.AddExtensionObject("pda:MyUtils", new MathHelper());
                return XmlHelper.XmlWriterFunction(xmlText.ToString(), arguments, xsltText);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
