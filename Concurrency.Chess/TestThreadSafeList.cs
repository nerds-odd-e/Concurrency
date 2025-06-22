using Microsoft.Concurrency.TestTools.UnitTesting;
using Microsoft.Concurrency.TestTools.UnitTesting.Chess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency.Chess
{
    [TestFixture]
    [ChessInstrumentAssembly("mscorlib")]
    [ChessInstrumentAssembly("nunit.framework", Exclude = true)]
    public class TestThreadSafeList
    {
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadAndUpdateWithThreadSafeList()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var enumerater = cList.GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        int current = enumerater.Current;
                    }
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndUpdateWithThreadSafeList()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    foreach (var v in cList)
                    {
                        object v1 = v;
                    }
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

    }
}
