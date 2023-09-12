﻿using System;
using System.Collections.Generic;
using System.Linq;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;


namespace IExpro.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        protected IDataContext entities = null;

        public UnitOfWork(IDataContext Context)
        {
            entities = Context;
        }

        public UnitOfWork()
        {
            entities = new IExproContext();
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        IAccountRepository _AccontRepo;
        public IAccountRepository AccontRepo
        {
            get
            {
                if (_AccontRepo != null)
                    return _AccontRepo;
                else
                    return _AccontRepo = new AccountRepository(entities);

            }

        }
        IInvoiceRepository _InvoiceRepo;
        public IInvoiceRepository InvoiceRepo
        {
            get
            {
                if (_InvoiceRepo != null)
                    return _InvoiceRepo;
                else
                    return _InvoiceRepo = new InvoiceRepository(entities);

            }

        }
        INavigationRepository _NavRepository;
        public INavigationRepository NavRepo
        {
            get
            {
                if (_NavRepository != null)
                    return _NavRepository;
                else
                    return _NavRepository = new NavigationRepository(entities);

            }

        }
        ICommonRepository _CommRepo;
        public ICommonRepository CommRepo
        {
            get
            {
                if (_CommRepo != null)
                    return _CommRepo;
                else
                    return _CommRepo = new CommonRepository(entities);

            }

        }


        IOrderRepository _OrdRepo;
        public IOrderRepository OrdRepo
        {
            get
            {
                if (_OrdRepo != null)
                    return _OrdRepo;
                else
                    return _OrdRepo = new OrderRepository(entities);

            }

        }


        IDocumentRepository _DocRepo;
        public IDocumentRepository DocRepo
        {
            get
            {
                if (_DocRepo != null)
                    return _DocRepo;
                else
                    return _DocRepo = new DocumentRepository(entities);

            }

        }


        IProcessRepository _ProcRepo;
        public IProcessRepository ProcRepo
        {
            get
            {
                if (_ProcRepo != null)
                    return _ProcRepo;
                else
                    return _ProcRepo = new IProcessRepository(entities);

            }

        }

        























        public void SaveChanges()
        {
            entities.Save();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }






    }

}
