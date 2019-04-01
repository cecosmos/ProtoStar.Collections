using Xunit;
using System;
using System.Linq;

namespace ProtoStar.Collections.Tests
{
    public class MultiValueDictionaryTest
    {
        [Fact]
        public void CanAddAndRemoveItems()
        {
            var multRange = Enumerable.Range(0,10);
            var multTable = multRange.
                SelectMany(l=> multRange.Select(r=> (Index: l, Value: l*r)));
            var multDict = new MultiValueDictionary<int,int>();
            foreach(var item in multTable)
            {
                multDict.Add(item.Index,item.Value);                
            }

            Assert.True(multDict.Count==10);
            multDict.Remove(1,1);
            Assert.True(multDict.Count==10);
            Assert.DoesNotContain(1,multDict[1]);
            multDict.Remove(2);
            Assert.True(multDict.Count==9);
        }
    }
}