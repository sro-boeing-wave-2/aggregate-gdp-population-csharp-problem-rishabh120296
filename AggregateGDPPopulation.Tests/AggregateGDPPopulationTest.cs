using System;
using System.Diagnostics;
using Xunit;
using AggregateGDPPopulation;
using System.IO;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task Test1Async()
        {
            await Class1.Aggregate();
            string file1 = File.ReadAllText("../../../../AggregateGDPPopulation.Tests/expected-output.json");
            string file2 = File.ReadAllText(@"../../../../AggregateGDPPopulation.Tests/output.json");
            Assert.Equal(file1, file2);
        }
    }
}
