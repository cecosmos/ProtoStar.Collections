using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace ProtoStar.Collections.Tests
{
    public class CollectionExtensionsTest
    {

        private static readonly IEnumerable<int> BaseOddNumbersSource = new int[] { 1, 3, 5, 7, 9 };
        private static readonly IEnumerable<int> BaseEvenNumbersSource = new int[] { 2, 4, 6, 8, 10 };
        private static readonly Predicate<int> BasePredicate = (x) => x > 2;

        public static IEnumerable<object[]> Data =>
            new List<object[]>()
            {
                new object[] {BaseOddNumbersSource,BasePredicate, true, 3 },
                new object[] {BaseEvenNumbersSource, BasePredicate, true,4},
                new object[] {BaseOddNumbersSource, new Predicate<int>(x => x>10), false,0}                
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void TryFindMatches(IEnumerable<int> source, Predicate<int> predicate, bool shouldFind, int expectedMatchValue)
        {
            Assert.Equal(shouldFind, source.TryFind(predicate, out var match));
            if (shouldFind) Assert.Equal(expectedMatchValue, match);
        }

        [Fact]
        public void Interleaving()
        {
            var resulting = BaseOddNumbersSource.Interleave(BaseEvenNumbersSource);
            Assert.Equal(new []{1,2,3,4}, resulting.Take(4));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TakeWhileInclusion(bool shouldInclude)
        {
            var lessThenFive = BaseEvenNumbersSource.TakeWhile(x=>x<5,shouldInclude);
            Assert.Equal(
                BaseEvenNumbersSource.TakeWhile(x=>x<5).Count() + (shouldInclude?1:0),
                lessThenFive.Count());
        }

        [Fact]
        public void CreateNormalizedDictionary()
        {
            var dict = BaseOddNumbersSource.ToNormalizedDictionary(odd=> odd, odd=> odd*2);
            Assert.NotEmpty(dict);
        }

        [Fact]
        public void CreateNormalizedDictionaryArgLess()
        {
            var dict = BaseOddNumbersSource.ToDictionary(odd=> odd, odd=> odd*2.0);
            var normalizedDictionary = dict.ToNormalizedDictionary();
            Assert.NotEmpty(normalizedDictionary);
        }
    }
}
