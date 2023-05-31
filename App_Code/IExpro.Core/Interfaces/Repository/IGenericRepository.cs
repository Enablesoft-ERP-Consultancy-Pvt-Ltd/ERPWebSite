

using System.Collections.Generic;
using System;
using System.Linq.Expressions;


namespace IExpro.Core.Interfaces.Repository
{

    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void DetachAll();
        void Detach(T entity);
        void Update(T entity);
        void Remove(T entity);
        T Get(Func<T, bool> predicate);
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        void GenericManyToManyUpdate(IEnumerable<T> ListGetFromView, IEnumerable<T> ListGetFromDB);

        void UpdateWithDetach(T entity);

        //Gaurav Pr
        IEnumerable<U> GetBy<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> columns);
        T GetORDefault(Func<T, bool> predicate);
        void GenericManyToManyUpdate2(IEnumerable<T> ListGetFromView, IEnumerable<T> ListGetFromDB, out IEnumerable<T> NewlyAdded, out IEnumerable<T> Deleted);

        // bool Exists(Func<T, bool> predicate);

        //IEnumerable<T> GetAll2(Expression<Func<T, bool>> predicate = null);
        //void AddRange(IEnumerable<T> entity);
        //void RemoveRange(IEnumerable<T> entity);

        //IEnumerable<HierarchyNode<T>> AsChildHierarchy(IEnumerable<HierarchyNode<T>> allTeams, T parentId);






    }
}
