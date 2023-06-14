
using IExpro.Core.Entity;
using IExpro.Core.Models.Account;
using System.Xml.Linq;

namespace IExpro.Core.Interfaces.Repository
{
    public interface IInvoiceRepository : IGenericRepository<INVOICE>
    {      
        string GetXSLTDetail(int clientId, short docType, int userId, short userType);
        XElement GetInvoiceDetail (int _invoiceId);
     
    }
}
