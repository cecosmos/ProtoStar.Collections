// Copyright © 2018 ceCosmos, Brazil. All rights reserved.
// Project: ProtoStar
// Author: Johni Michels

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ProtoStar.Collections
{
    public class CollectionAdapter<T> : 
        ICollection<T>, 
        IReadOnlyCollection<T>
    {

        public CollectionAdapter(Func<IEnumerable<T>> iterator, Action<T> addCallback, Predicate<T> removeCallback)
        {
            AddCallback = addCallback;
            Iterator = iterator;
            RemoveCallback = removeCallback;
        }

        public CollectionAdapter(Func<IEnumerable<T>> iterator)
        {
            this.Iterator = iterator;
        }


        private Action<T> AddCallback { get; set; }
        private Func<IEnumerable<T>> Iterator { get; set; }
        private Predicate<T> RemoveCallback { get; set; }

        public bool IsReadOnly => (AddCallback==null) && (RemoveCallback==null);
        public int Count 
        {
            get 
            {
                switch (this.Iterator())
                {
                    case ICollection<T> collection:
                        return collection.Count;
                    case IReadOnlyCollection<T> collection:
                        return collection.Count;                    
                    default:
                        return this.Iterator().Count(); 
                }
            }
        }


        public void Add(T item)
        {
            AddCallback(item);
        }

        public void Clear()
        {
            foreach (T item in Iterator().ToList())
            {
                RemoveCallback(item);
            }
        }

        public bool Contains(T item) => this.Iterator().Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => this.Iterator().ToList().CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => this.Iterator().GetEnumerator();

        public bool Remove(T item) => this.RemoveCallback(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    }
}