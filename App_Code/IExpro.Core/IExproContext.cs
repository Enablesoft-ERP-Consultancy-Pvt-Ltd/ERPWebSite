using IExpro.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Entity
{

    public partial class IExproContext : DbContext, IDataContext
    {
        public IQueryable<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {

            this.Set<TEntity>().Attach(entity);
            base.Entry<TEntity>(entity).State = EntityState.Modified;
        }


        public void DetachAll()
        {

            foreach (DbEntityEntry dbEntityEntry in ChangeTracker.Entries())
            {

                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            if (this.Set<TEntity>().Local.Contains(entity))
                ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public TEntity Create<TEntity>(TEntity entity = null) where TEntity : class
        {
            if (entity != null)
            {
                this.Set<TEntity>().Attach(entity);
                base.Entry<TEntity>(entity).State = EntityState.Added;
            }
            else
                entity = this.Set<TEntity>().Create();

            return entity;
        }




        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = this.Entry<TEntity>(entity);

            if (entry.State == EntityState.Detached)
            {
                this.Set<TEntity>().Attach(entity);
                entry.State = EntityState.Modified;
            }
        }




        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Attach(entity);
            base.Entry<TEntity>(entity).State = EntityState.Deleted;
            //this.Set<TEntity>().Remove(entity);
        }
        public void Save()
        {
            base.SaveChanges();
        }

        public void Dispose()
        {
            base.Dispose();
        }

        public void Refersh<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry<TEntity>(entity).Reload();
        }

        public void RefreshAll()
        {
            foreach (var entity in ChangeTracker.Entries())
            {
                entity.Reload();
            }



        }







    }
}
