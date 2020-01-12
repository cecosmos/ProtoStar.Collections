using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ProtoStar.Collections
{
    public class ObservableHashSet<T> :
        INotifyCollectionChanged,
        IReadOnlyCollection<T>,
        ISet<T>
    {

        private readonly HashSet<T> Base;

        public ObservableHashSet() { Base = new HashSet<T>(); }
        public ObservableHashSet(IEnumerable<T> collection) { Base = new HashSet<T>(collection); }
        public ObservableHashSet(IEqualityComparer<T> comparer) { Base = new HashSet<T>(comparer); }
        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) { Base = new HashSet<T>(collection, comparer); }

        public int Count => Base.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChange(NotifyCollectionChangedEventArgs eventArgs)=>
            CollectionChanged?.Invoke(this, eventArgs);
        

        public bool Add(T item)
        {
            if(Base.Add(item))
            {
                OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
                return true;
            }
            return false;
        } 

        public void Clear()
        {
            if (Base.Any())
            {
                Base.Clear();
                OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool Contains(T item) => Base.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Base.CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<T> other)
        {
            var intermediate = new HashSet<T>(Base,Base.Comparer);
            Base.ExceptWith(other);
            intermediate.IntersectWith(other);
            if (intermediate.Any()) OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, intermediate));
        }

        public IEnumerator<T> GetEnumerator() => Base.GetEnumerator();

        public void IntersectWith(IEnumerable<T> other)
        {
            var intermediate = new HashSet<T>(Base, Base.Comparer);
            Base.IntersectWith(other);
            intermediate.ExceptWith(Base);
            if (intermediate.Any()) OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, intermediate));
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) => Base.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => Base.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other) => Base.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => Base.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other) => Base.Overlaps(other);

        public bool Remove(T item)
        {
            if(Base.Remove(item))
            {
                OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }));
            }
            return false;
        }

        public bool SetEquals(IEnumerable<T> other) => Base.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            var removed = new HashSet<T>(Base, Base.Comparer);
            removed.IntersectWith(other);
            var added = new HashSet<T>(other, Base.Comparer);
            added.ExceptWith(removed);
            Base.SymmetricExceptWith(other);
            if (removed.Any()) OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
            if (added.Any()) OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));
        }

        public void UnionWith(IEnumerable<T> other)
        {
            var added = new HashSet<T>(other, Base.Comparer);
            added.ExceptWith(Base);
            Base.UnionWith(other);
            if (added.Any()) OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));
        }

        void ICollection<T>.Add(T item)=>Add(item);
        

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
