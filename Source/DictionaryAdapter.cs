// Copyright © 2018 ceCosmos, Brazil. All rights reserved.
// Project: ProtoStar
// Author: Johni Michels

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using ProtoStar.Core;

namespace ProtoStar.Collections
{
    public class DictionaryAdapter<TKey, TValue> : 
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey,TValue>
    {
        private readonly ProtoStar.Core.TryFunc<TKey, TValue> GetCallback;
        private readonly Action<TKey, TValue> AddOrSetCallback;
        private readonly Predicate<TKey> RemoveCallback;
        private readonly Func<IEnumerable<TKey>> KeyEnumerable;

        public DictionaryAdapter(
            ProtoStar.Core.TryFunc<TKey, TValue> getCallback,
            Func<IEnumerable<TKey>> keyEnumerable,
            Action<TKey, TValue> addOrSetCallback,
            Predicate<TKey> removeCallback)
        {
            GetCallback = getCallback;
            AddOrSetCallback = addOrSetCallback;
            RemoveCallback = removeCallback;
            KeyEnumerable = keyEnumerable;
        }

        public DictionaryAdapter(ProtoStar.Core.TryFunc<TKey,TValue> getCallback,Func<IEnumerable<TKey>> keyEnumerable)
        {
            GetCallback = getCallback;
            KeyEnumerable = keyEnumerable;
        }

        public TValue this[TKey key]
        {
            get
            {
                if(GetCallback(key, out var result)) return result;
                throw new KeyNotFoundException();
            }
            set => AddOrSetCallback(key,value);
        }

        public ICollection<TKey> Keys => new CollectionAdapter<TKey>(KeyEnumerable);

        public ICollection<TValue> Values => new CollectionAdapter<TValue>(()=>KeyEnumerable().Select(k=> this[k]));

        [ExcludeFromCodeCoverage]
        public int Count => KeyEnumerable().Count();

        public bool IsReadOnly => AddOrSetCallback==null;

        [ExcludeFromCodeCoverage]
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        [ExcludeFromCodeCoverage]
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        [ExcludeFromCodeCoverage]
        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => Count;

        [ExcludeFromCodeCoverage]
        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => this[key];

        public void Add(TKey key, TValue value)
        {
            if(GetCallback(key, out var currentValue))
            { throw new ArgumentException("An element with the same key already exists in the Dictionary<TKey,TValue>", nameof(key)) ; }
            AddOrSetCallback(key, value);
        }


        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)=>
            Add(item.Key,item.Value);        

        public void Clear() => 
            Keys.ToList().ForEach(k => Remove(k));        

        [ExcludeFromCodeCoverage]
        public bool Contains(KeyValuePair<TKey, TValue> item)=>
            GetCallback(item.Key, out var value)&&value.Equals(item.Value);

        public bool ContainsKey(TKey key) => 
            GetCallback(key, out var value);

        [ExcludeFromCodeCoverage]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)=>
            this.AsEnumerable().Select((kv, i) => { array[i + arrayIndex] = kv; return kv; }).ToList();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            Keys.Select(k => new KeyValuePair<TKey, TValue>(k, this[k])).GetEnumerator();

        public bool Remove(TKey key)=>RemoveCallback(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public bool TryGetValue(TKey key, out TValue value) => GetCallback(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
