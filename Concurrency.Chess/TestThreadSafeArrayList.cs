using Microsoft.Concurrency.TestTools.UnitTesting;
using Microsoft.Concurrency.TestTools.UnitTesting.Chess;
using NUnit.Framework;
using System;
using System.Collections;
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
    public class TestThreadSafeArrayList
    {
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadAndUpdateWithThreadSafeArrayList()
        {
            var cList = new ThreadSafeArrayList(new ArrayList { 1, 2, 3, 4 });
            var listNode = (int)cList[2];

            Thread t = new Thread(
                () =>
                {
                    var enumerater = cList.GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        int current = (int)enumerater.Current;
                    }
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateRangeReadAndUpdateWithThreadSafeArrayList()
        {
            var cList = new ThreadSafeArrayList(new ArrayList { 1, 2, 3, 4 });
            var listNode = (int)cList[2];

            Thread t = new Thread(
                () =>
                {
                    var enumerater = cList.GetEnumerator(1, 2);
                    while (enumerater.MoveNext())
                    {
                        int current = (int)enumerater.Current;
                    }
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndUpdateWithThreadSafeArrayList()
        {
            var cList = new ThreadSafeArrayList(new ArrayList { 1, 2, 3, 4 });
            var listNode = (int)cList[2];

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

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructor()
        {
            var cList = new ThreadSafeArrayList();

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(77);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithCapacity()
        {
            var cList = new ThreadSafeArrayList(10);

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(77);
            t.Join();
        }

    }
}
