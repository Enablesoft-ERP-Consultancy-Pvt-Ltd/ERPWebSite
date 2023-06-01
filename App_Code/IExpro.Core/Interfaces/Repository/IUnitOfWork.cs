
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

        /// <summary>
        /// This region  is  created by user Rakesh
        /// </summary>    
        #region Rakesh
        IAccountRepository AccontRepo { get; }
        IInvoiceRepository InvoiceRepo { get; }

        INavigationRepository NavRepository { get; }
        #endregion

        void SaveChanges();



    }


   
}
