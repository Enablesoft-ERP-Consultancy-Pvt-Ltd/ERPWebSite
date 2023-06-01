using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Common
{
    public interface IDataContext
    {
        IQueryable<TEntity> GetSet<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity entity) where TEntity : class;
        void DetachAll();
        void Detach<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        TEntity Create<TEntity>(TEntity entity = null) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Save();
        void Dispose();




        
    }
}
