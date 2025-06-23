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
        public void TestPassedConcurrentEnumerateReadAndUpdate()
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
        public void TestPassedConcurrentLoopReadAndUpdate()
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

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentCapacityAndUpdate()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var c = cList.Capacity;
                });
            t.Start();

            cList.Capacity = 10;
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddRangeAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.AddRange(new List<int> { 10, 20 });
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentInsertRangeAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.InsertRange(2, new List<int> { 10, 20 });
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveRange()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.RemoveRange(1, 2);
                });
            t.Start();

            cList.Add(42);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveAll()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.RemoveAll(e => e == 2 || e == 4);
                });
            t.Start();

            cList.Add(42);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfWithIndexAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var index = cList.IndexOf(3, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfWithIndexAndCountAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var index = cList.IndexOf(3, 1, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var index = cList.LastIndexOf(3);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfWithIndexAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var index = cList.LastIndexOf(3, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfWithIndexAndCountAndRemove()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    var index = cList.LastIndexOf(3, 1, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverse()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Reverse();
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverseWithIndexAndCount()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Reverse(1, 2);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSort()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort();
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparator()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort(Comparer<int>.Default);
            t.Join();
        }

    }
}
