using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProtoStar.Collections
{
    public class MultiValueDictionary<TKey, TValue> :
        IMultiValueDictionary<TKey, TValue>
    {

        public MultiValueDictionary()
        {
            Source = new Dictionary<TKey, List<TValue>>();
        }

        public MultiValueDictionary(IEqualityComparer<TKey> equalityComparer)
        {
            Source = new Dictionary<TKey, List<TValue>>(equalityComparer);
        }

        public ICollection<TValue> this[TKey key] => 
            new CollectionAdapter<TValue>(
                ()=> Source.TryGetValue(key, out var result) ? result: Enumerable.Empty<TValue>(),
                value=> Add(key,value),
                value=> Remove(key,value));

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] => Source[key];

        public int Count => Source.Count;

        public IEnumerable<TKey> Keys => Source.Keys;

        public IEnumerable<ICollection<TValue>> Values => Source.Values;

        private Dictionary<TKey, List<TValue>> Source { get; set; }

        public void Add(TKey key, TValue value)
        {
            if (Source.TryGetValue(key, out var targetCollection)) targetCollection.Add(value);
            else Source.Add(key, new List<TValue>() { value });
        }

        public bool Contains(TKey key) => Source.ContainsKey(key);

        public bool ContainsKey(TKey key) => Contains(key);

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() => Source.Select(kv => new Group<TKey, TValue>(kv.Key, kv.Value)).GetEnumerator();

        public bool Remove(TKey key, TValue value) =>
            (Source.TryGetValue(key, out var targetCollection)) ?
            targetCollection.Remove(value) :
            false;

        public bool Remove(TKey key) => Source.Remove(key);

        public bool TryGetValue(TKey key, out ICollection<TValue> value)
        {
            if(Source.TryGetValue(key, out var targetCollection))
            {
                value = targetCollection;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
