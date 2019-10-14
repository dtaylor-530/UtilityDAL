using System.Linq;
using Xunit;

namespace UtilityDAL.Test
{
    public class Tea
    {
        [Fact]
        public void Test3()
        {
            var xxx = new UtilityDAL.Teatime.Repository<xxStructure>("../../../");

            var arr = Enumerable.Range(0, 10).Select(_ => new xxStructure { Ticks = _ }).ToArray();

            xxx.Clear("Repo");

            xxx.To(arr, "Repo");

            var yy = xxx.From("Repo");

            Assert.Equal(10, arr.Length);
        }

        private struct xxStructure
        {
            public long Ticks { get; set; }
        }
    }
}