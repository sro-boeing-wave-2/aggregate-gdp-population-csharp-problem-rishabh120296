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
        public void Test1()
        {
            Class1.Aggregate();
            string file1 = File.ReadAllText("../../../../AggregateGDPPopulation.Tests/expected-output.json");
            string file2 = File.ReadAllText(@"../../../../AggregateGDPPopulation.Tests/output.json");
            Assert.Equal(file1, file2);
        }
    }
}
