using System.Diagnostics.CodeAnalysis;
// Copyright © 2018 ceCosmos, Brazil. All rights reserved.
// Project: ProtoStar
// Author: Johni Michels

using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace ProtoStar.Collections
{
    public class Group<TKey, TValue> :
        IGrouping<TKey, TValue>
    {
        public TKey Key { get; private set; }
        
        private IEnumerable<TValue> Values { get; set; }

        public IEnumerator<TValue> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

        public Group(TKey key, IEnumerable<TValue> values)
        {
            Key = key;
            Values = values;
        }

    }
}
