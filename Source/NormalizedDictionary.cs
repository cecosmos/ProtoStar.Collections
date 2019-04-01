// Copyright © 2018 ceCosmos, Brazil. All rights reserved.
// Project: ProtoStar
// Author: Johni Michels

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProtoStar.Collections
{
    public class NormalizedDictionary<T> : 
        IDictionary<T, double>, 
        IReadOnlyDictionary<T, double>
    {
        #region Private Fields

        private readonly Dictionary<T, double> baseDictionary;

        #endregion Private Fields

        #region Public Constructors

        public NormalizedDictionary() =>
            baseDictionary = new Dictionary<T, double>();

        public NormalizedDictionary(IEqualityComparer<T> comparer) =>
            baseDictionary = new Dictionary<T, double>(comparer);

        #endregion Public Constructors

        #region Public Properties

        public bool IsReadOnly => false;
        public int Count => baseDictionary.Count;
        public ICollection<T> Keys => baseDictionary.Keys;
        IEnumerable<T> IReadOnlyDictionary<T, double>.Keys => baseDictionary.Keys;
        public ICollection<double> Values => baseDictionary.Values.Select((item) => item / baseDictionary.Values.Sum()).ToArray();
        IEnumerable<double> IReadOnlyDictionary<T, double>.Values => Values;

        #endregion Public Properties

        #region Public Indexers

        public double this[T key] { get => baseDictionary[key] / baseDictionary.Values.Sum(); set => baseDictionary[key] = value; }

        #endregion Public Indexers

        #region Private Methods

        private void Add(KeyValuePair<T, double> item) => Add(item.Key, item.Value);

        #endregion Private Methods

        #region Public Methods

        public void Add(T key, double value) => baseDictionary.Add(key, value);

        public void Clear() => baseDictionary.Clear();

        public bool Contains(KeyValuePair<T, double> item) => baseDictionary.Contains(item);

        public bool ContainsKey(T key) => baseDictionary.ContainsKey(key);

        void ICollection<KeyValuePair<T,double>>.CopyTo(KeyValuePair<T, double>[] array, int arrayIndex) => 
            this.ToList().CopyTo(array,arrayIndex);

        public IEnumerator<KeyValuePair<T, double>> GetEnumerator() => Keys.Select((item) => new KeyValuePair<T, double>(item, this[item])).GetEnumerator();

        public bool Remove(T key) => baseDictionary.Remove(key);

        public bool Remove(KeyValuePair<T, double> item) => baseDictionary.Remove(item.Key);

        public bool TryGetValue(T key, out double value)
        {
            var result = baseDictionary.TryGetValue(key, out var intermediate);
            if (result) value = intermediate / baseDictionary.Values.Sum();
            else value = default;
            return result;
        }

        void ICollection<KeyValuePair<T, double>>.Add(KeyValuePair<T, double> item) => Add(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Public Methods
    }
}