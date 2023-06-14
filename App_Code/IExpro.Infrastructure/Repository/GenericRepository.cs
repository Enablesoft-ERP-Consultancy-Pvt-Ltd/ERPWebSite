

using IExpro.Core.Interfaces.Common;
using IExpro.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IExpro.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDataContext entities = null;

        public GenericRepository(IDataContext _entities)
        {
            entities = _entities;

        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {

                return entities.GetSet<T>().Where(predicate);

            }

            return entities.GetSet<T>().AsEnumerable();
        }

        public T Get(Func<T, bool> predicate)
        {
            return entities.GetSet<T>().FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            entities.Create<T>(entity);
        }

        public void Update(T entity)
        {

            //entities.Update<T>(entity);
            entities.Attach<T>(entity);
        }

        public void Remove(T entity)
        {
           
            entities.Delete<T>(entity);
        }

        public void Detach(T entity)
        {
            entities.Detach<T>(entity);

        }


        public void DetachAll()
        {
            entities.DetachAll();

        }

        public void UpdateWithDetach(T entity)
        {
            //entities.Detach<T>(entity);
            //entities.Attach<T>(entity);
            entities.Update<T>(entity);
        }

        public void GenericManyToManyUpdate(IEnumerable<T> ListGetFromView, IEnumerable<T> ListGetFromDB)
        {
            IEnumerable<T> NewlyAdded = ListGetFromView.Except(ListGetFromDB);
            foreach (var a in NewlyAdded)
            {
                this.Add(a);
            }
            IEnumerable<T> Deleted = ListGetFromDB.Except(ListGetFromView);
            foreach (var a in Deleted)
            {
                this.Remove(a);
            }
        }


        //Gaurav Pr
        public IEnumerable<U> GetBy<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> columns)
        {
            if (predicate != null)
            {
                return entities.GetSet<T>().Where<T>(predicate).Select<T, U>(columns);
            }

            return entities.GetSet<T>().Select<T, U>(columns);
        }

        //Gaurav Pr
        public T GetORDefault(Func<T, bool> predicate)
        {
            return entities.GetSet<T>().FirstOrDefault<T>(predicate);
        }
        //Gaurav Pr
        public void GenericManyToManyUpdate2(IEnumerable<T> ListGetFromView, IEnumerable<T> ListGetFromDB, out IEnumerable<T> NewlyAdded, out IEnumerable<T> Deleted)
        {
            NewlyAdded = ListGetFromView.Except(ListGetFromDB);

            Deleted = ListGetFromDB.Except(ListGetFromView);

        }





      






      

    }
}
