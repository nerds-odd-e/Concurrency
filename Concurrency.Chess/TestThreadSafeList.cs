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

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparison()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort((a, b) =>
            {
                if (a > b) return 1;
                else if (a < b) return -1;
                else return 0;
            });
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparisonWithStartIndexAndCount()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort(1, 2, Comparer<int>.Default);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTrimExcess()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.TrimExcess();
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearch()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.BinarySearch(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearchWithComparer()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.BinarySearch(listNode, Comparer<int>.Default);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearchWithStartIndexAndCountAndComparer()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.BinarySearch(1, 2, listNode, Comparer<int>.Default);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFind()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Find(e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindAll()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindAll(e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedThreadSafeOfListReturnedByFindAll()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            var found = cList.FindAll(e => e == 3);

            Thread t = new Thread(
                () =>
                {
                    found.Add(12306);
                });
            t.Start();

            found.Add(777);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndex()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindIndex(e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndexWithStartIndex()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindIndex(1, e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndexWithStartIndexAndCount()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindIndex(1, 3, e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLast()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindLast(e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndex()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindLastIndex(e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndexWithStartIndex()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindLastIndex(1, e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndexWithStartIndexAndCount()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.FindLastIndex(3, 3, e => e == 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndExists()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Exists(e => e == 2);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTrueForAll()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.TrueForAll(e => e == 2);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndForEach()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.ForEach(e => Console.WriteLine(e));
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndConvertAll()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.ConvertAll<string>(e => e.ToString());
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndAsReadOnly()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = cList.AsReadOnly();
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructor()
        {
            var cList = new ThreadSafeList<int>();

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(1);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithCapacity()
        {
            var cList = new ThreadSafeList<int>(10);

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(1);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyTo()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(result);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithArrayIndex()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(result, 3);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithBothIndexAndCount()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(1, result, 0, 2);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetRange()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.GetRange(1, 2);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndToArray()
        {
            var cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.ToArray();
            t.Join();
        }

    }
}
