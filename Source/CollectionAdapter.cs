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
        #region Public Constructors

        public CollectionAdapter(Func<IEnumerable<T>> iterator, Action<T> addCallback, Predicate<T> removeCallback)
        {
            AddCallback = addCallback;
            Iterator = iterator;
            RemoveCallback = removeCallback;
        }

        public CollectionAdapter(Func<IEnumerable<T>> iterator)
        {
            Iterator = iterator;
        }

        #endregion Public Constructors

        #region Private Properties

        private Action<T> AddCallback { get; set; }
        private Func<IEnumerable<T>> Iterator { get; set; }
        private Predicate<T> RemoveCallback { get; set; }

        #endregion Private Properties

        #region Public Properties

        public bool IsReadOnly => AddCallback==null;
        public int Count => Iterator().Count();

        #endregion Public Properties

        #region Public Methods

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

        public bool Contains(T item) => Iterator().Contains(item);

        [ExcludeFromCodeCoverage]
        public void CopyTo(T[] array, int arrayIndex) => Iterator().ToList().CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => Iterator().GetEnumerator();

        public bool Remove(T item) => RemoveCallback(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Public Methods
    }
}