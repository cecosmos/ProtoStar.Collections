using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProtoStar.Collections.Tests
{
    
    public class CollectionAdapterTest
    {
        [Fact]
        public void AddRemoveElement()
        {
            IList<int> baseCollection = new List<int>(new[] { 2, 3, 4 });
            bool hitAdd = false;
            bool hitRemove = false;
            var forwardedCollection = new CollectionAdapter<int>(
                () => baseCollection,
                x => { hitAdd = true; baseCollection.Add(x); },                
                x => { hitRemove = true; return baseCollection.Remove(x); });

            Assert.Equal(baseCollection, forwardedCollection);
            Assert.False(hitAdd);
            Assert.False(hitRemove);
            
            forwardedCollection.Add(5);
            Assert.Equal(baseCollection, forwardedCollection);
            Assert.True(hitAdd);
            Assert.False(hitRemove);
            
            hitRemove = false;
            hitAdd = false;
            forwardedCollection.Remove(2);
            
            Assert.Equal(baseCollection, forwardedCollection);
            Assert.False(hitAdd);
            Assert.True(hitRemove);
        }

        [Fact]
        public void AddElement()
        {
            IList<int> baseCollection = new List<int>(new[] { 2, 3, 4 });
            bool hitAdd = false;
            bool hitRemove = false;
            var forwardedCollection = new CollectionAdapter<int>(
                () => baseCollection,
                x => { hitAdd = true; baseCollection.Add(x); },                
                x => { hitRemove = true; return baseCollection.Remove(x); });

            Assert.Equal(baseCollection, forwardedCollection);
            Assert.False(hitAdd);
            Assert.False(hitRemove);

            forwardedCollection.Add(5);
            Assert.Equal(baseCollection, forwardedCollection);
            Assert.True(hitAdd);
            Assert.False(hitRemove);
        }

        [Fact]
        public void RemoveElement()
        {
            IList<int> baseCollection = new List<int>(new[] { 2, 3, 4 });
            bool hitAdd = false;
            bool hitRemove = false;
            var forwardedCollection = new CollectionAdapter<int>(
                () => baseCollection,
                x => { hitAdd = true; baseCollection.Add(x); },                
                x => { hitRemove = true; return baseCollection.Remove(x); });

            forwardedCollection.Remove(2);

            Assert.Equal(baseCollection, forwardedCollection);
            Assert.False(hitAdd);
            Assert.True(hitRemove);
        }

        [Fact]
        public void IsReadOnlyOnNullCallbacks()
        {
            IList<int> baseCollection =  System.Linq.Enumerable.Range(0,10).ToList();
            var col = new CollectionAdapter<int>(()=>baseCollection);
            Assert.True(col.IsReadOnly);
            col = new CollectionAdapter<int>(()=>baseCollection,baseCollection.Add,baseCollection.Remove);
            Assert.False(col.IsReadOnly);
        }

        [Fact]
        public void CountMatchesSource()
        {
            IList<int> baseCollection =  System.Linq.Enumerable.Range(0,10).ToList();
            var col = new CollectionAdapter<int>(()=>baseCollection);
            Assert.Equal(baseCollection.Count,col.Count);
        }

        [Fact]
        public void ClearCleansSource()
        {
            IList<int> baseCollection =  System.Linq.Enumerable.Range(0,10).ToList();        
            var col = new CollectionAdapter<int>(()=>baseCollection,baseCollection.Add,baseCollection.Remove);
            col.Clear();
            Assert.Empty(baseCollection);
            Assert.Empty(col);
        }

        [Fact]
        public void ContainsEnsureSourcePresence()
        {
            IList<int> baseCollection =  System.Linq.Enumerable.Range(0,10).ToList();        
            var col = new CollectionAdapter<int>(()=>baseCollection,baseCollection.Add,baseCollection.Remove);
            Assert.True(col.Contains(3));
            Assert.False(col.Contains(10));
        }
    }
}
