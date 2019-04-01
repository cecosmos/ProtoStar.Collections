using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProtoStar.Collections.Tests
{
    public class NormalizedDictionaryTest
    {
        [Fact]
        public void ItemReturnsNormalized()
        {
            var dict = new NormalizedDictionary<int>()
            {
                {1,1.0 },
                {2,2.0 },
                {3,3.0 },
                {4,4.0 }
            };

            Assert.Equal(1.0 / 10.0, dict[1]);
            Assert.Equal(2.0 / 10.0, dict[2]);
            Assert.Equal(3.0 / 10.0, dict[3]);
            Assert.Equal(4.0 / 10.0, dict[4]);
        }

        [Fact]
        public void TryGetReturnsNormalized()
        {
            var dict = new NormalizedDictionary<int>()
            {
                {1,1.0 },
                {2,2.0 },
                {3,3.0 },
                {4,4.0 }
            };

            if(dict.TryGetValue(1,out var r1)) Assert.Equal(1.0/10.0,r1);
            if(dict.TryGetValue(2,out var r2)) Assert.Equal(2.0/10.0,r2);
            if(dict.TryGetValue(3,out var r3)) Assert.Equal(3.0/10.0,r3);
            if(dict.TryGetValue(4,out var r4)) Assert.Equal(4.0/10.0,r4);

        }


    }
}
