using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace FintTsPersistenceTests
{
    /// <summary>
    /// TODO: replace this with http://nuget.org/packages/FakeDbSet
    /// </summary>
    /// <remarks>http://www.nogginbox.co.uk/blog/mocking-entity-framework-data-context</remarks>
    public class FakeDbSet<T> : IDbSet<T>
        where T : class
    {
        readonly HashSet<T> data;
        readonly IQueryable query;

        public FakeDbSet()
        {
            data = new HashSet<T>();
            query = data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public T Add(T item)
        {
            data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            data.Add(item);
            return item;
        }

        public void Detach(T item)
        {
            data.Remove(item);
        }

        public virtual T Create()
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Create");
        }

        public virtual TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Create");
        }

        public ObservableCollection<T> Local { get; set; }

        Type IQueryable.ElementType
        {
            get { return query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
