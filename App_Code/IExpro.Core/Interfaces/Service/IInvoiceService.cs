using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Service
{
    public interface IInvoiceService
    {
        string GetInvoiceDetail(int _clientId, int _invoiceId, short docType, int userId, short userType);
    }
}
