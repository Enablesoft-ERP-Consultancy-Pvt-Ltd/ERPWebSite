
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        IAccountRepository AccontRepo { get; }
        IInvoiceRepository InvoiceRepo { get; }
        INavigationRepository NavRepo { get; }
        ICommonRepository CommRepo { get; }
        IOrderRepository OrdRepo { get; }
        IDocumentRepository DocRepo { get; }
        IProcessRepository ProcRepo { get; }
        void SaveChanges();



    }



}
