﻿using System.Collections.Generic;
using System.Linq;

namespace ProtoStar.Collections
{
    public interface IMultiValueDictionary<TKey,TValue>:
        ILookup<TKey,TValue>
    {
        void Add(TKey key, TValue value);
        bool Remove(TKey key, TValue value);
        bool Remove(TKey key);
        new ICollection<TValue> this[TKey key] { get;}
    }
}
