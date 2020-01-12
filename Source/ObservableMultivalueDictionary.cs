using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ProtoStar.Collections
{
    public class ObservableMultivalueDictionary<TKey, TValue> :
        IMultiValueDictionary<TKey, TValue>,
        INotifyCollectionChanged
    {
        #region Public Constructors

        public ObservableMultivalueDictionary() =>
            SourceDictionary = new MultiValueDictionary<TKey, TValue>();

        public ObservableMultivalueDictionary(IEqualityComparer<TKey> equalityComparer) =>
            SourceDictionary = new MultiValueDictionary<TKey, TValue>(equalityComparer);

        #endregion Public Constructors

        #region Public Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion Public Events

        #region Private Properties

        private MultiValueDictionary<TKey, TValue> SourceDictionary { get; set; }

        #endregion Private Properties

        #region Public Properties

        public int Count => SourceDictionary.Count;

        #endregion Public Properties

        #region Public Indexers

        public ICollection<TValue> this[TKey key] =>
            new CollectionAdapter<TValue>(
                () => SourceDictionary[key],
                (value) => Add(key, value),
                (value) => Remove(key, value));

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] => SourceDictionary[key];

        #endregion Public Indexers

        #region Private Methods

        private void OnCollectionChange(NotifyCollectionChangedEventArgs args) =>
            CollectionChanged?.Invoke(this, args);

        #endregion Private Methods



        #region Public Methods

        public void Add(TKey key, TValue value)
        {
            SourceDictionary.Add(key, value);
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new Group<TKey, TValue>(key, new[] { value })));
        }

        public bool Contains(TKey key) => SourceDictionary.ContainsKey(key);

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() => SourceDictionary.GetEnumerator();

        public bool Remove(TKey key, TValue value)
        {
            var result = SourceDictionary.Remove(key, value);
            if (result)
                OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new Group<TKey, TValue>(key, new[] { value })));
            return result;
        }

        public bool Remove(TKey key)
        {
            if (SourceDictionary.TryGetValue(key, out var targetItems))
            {
                SourceDictionary.Remove(key);
                OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new Group<TKey,TValue>(key, targetItems)));
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Public Methods
    }
}