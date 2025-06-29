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
    [ChessInstrumentAssembly("System.Core")]
    [ChessInstrumentAssembly("nunit.framework", Exclude = true)]
    public class TestThreadSafeList
    {
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadAndUpdate()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentEnumerateReadAndUpdateAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadAndUpdateCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentEnumerateReadAndUpdateAs(cList);
        }

        private void TestPassedConcurrentEnumerateReadAndUpdateAs<T>(T cList) where T : IList<int>
        {
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

            var removed = cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.IsTrue(removed);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadViaIEnumerableAndAdd()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    IEnumerator enumerater = ((IEnumerable)cList).GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        int current = (int)enumerater.Current;
                    }
                });
            t.Start();

            cList.Add(42);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndUpdate()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentLoopReadAndUpdateAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndUpdateCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentLoopReadAndUpdateAs(cList);
        }

        private void TestPassedConcurrentLoopReadAndUpdateAs<T>(T cList) where T : IList<int>
        {
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

            var removed = cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.IsTrue(removed);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentCapacityAndUpdate()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    var c = cList.Capacity;
                });
            t.Start();

            cList.Capacity = 10;
            t.Join();
            NUnit.Framework.Assert.AreEqual(10, cList.Capacity);
            NUnit.Framework.Assert.AreEqual(4, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIsReadOnly()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndIsReadOnlyAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIsReadOnlyCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndIsReadOnlyAs(cList);
        }

        private void TestPassedConcurrentAddAndIsReadOnlyAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var isReadOnly = cList.IsReadOnly;
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsFalse(isReadOnly);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndCountAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCountCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndCountAs(cList);
        }

        private void TestPassedConcurrentAddAndCountAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var count = cList.Count;
            t.Join();
            NUnit.Framework.Assert.IsTrue(count == 4 || count == 5);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            TestPassedConcurrentAddAndGetByIndexAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByIndexCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            TestPassedConcurrentAddAndGetByIndexAs(cList);
        }

        private void TestPassedConcurrentAddAndGetByIndexAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var v = cList[1];
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, v);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSetByIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            TestPassedConcurrentAddAndSetByIndexAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSetByIndexCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            TestPassedConcurrentAddAndSetByIndexAs(cList);
        }

        private void TestPassedConcurrentAddAndSetByIndexAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList[2] = 123;
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(123, cList[2]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndClear()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndClearAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndClearCastAsIList()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndClearAs(cList);
        }

        private void TestPassedConcurrentAddAndClearAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Clear();
            t.Join();
            NUnit.Framework.Assert.IsTrue(cList.Count == 0 || cList.Count == 1);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContains()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndContainsAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContainsCastAsIList()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndContainsAs(cList);
        }

        private void TestPassedConcurrentAddAndContainsAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var contains = cList.Contains(2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(contains);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddRangeAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.AddRange(new List<int> { 10, 20 });
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentInsertAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentInsertAndRemoveAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentInsertAndRemoveCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentInsertAndRemoveAs(cList);
        }

        private void TestPassedConcurrentInsertAndRemoveAs<T>(T cList) where T : IList<int>
        {
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Insert(2, 42);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentInsertRangeAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.InsertRange(2, new List<int> { 10, 20 });
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndRemoveAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndRemoveAs(cList);
        }

        private void TestPassedConcurrentAddAndRemoveAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Remove(3);
                });
            t.Start();

            cList.Add(42);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveAt()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndRemoveAtAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveAtCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndRemoveAtAs(cList);
        }

        private void TestPassedConcurrentAddAndRemoveAtAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.RemoveAt(1);
                });
            t.Start();

            cList.Add(42);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveRange()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.RemoveRange(1, 2);
                });
            t.Start();

            cList.Add(42);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveAll()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.RemoveAll(e => e == 2 || e == 4);
                });
            t.Start();

            cList.Add(42);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentIndexOfAndRemoveAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfAndRemoveCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentIndexOfAndRemoveAs(cList);
        }

        private void TestPassedConcurrentIndexOfAndRemoveAs<T>(T cList) where T : IList<int>
        {
            var listNode = cList[2];

            int index = -1;
            Thread t = new Thread(
                () =>
                {
                    index = cList.IndexOf(4);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.IsTrue(index == 2 || index == 3);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfWithIndexAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            int index = -1;
            Thread t = new Thread(
                () =>
                {
                    index = cList.IndexOf(4, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.IsTrue(index == 2 || index == 3);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentIndexOfWithIndexAndCountAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            int index = 42;
            Thread t = new Thread(
                () =>
                {
                    index = cList.IndexOf(4, 1, 1);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.AreEqual(-1, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            int index = -1;
            Thread t = new Thread(
                () =>
                {
                    index = cList.LastIndexOf(4);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.IsTrue(index == 2 || index == 3);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfWithIndexAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            int index = -1;
            Thread t = new Thread(
                () =>
                {
                    index = cList.LastIndexOf(1, 2);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.AreEqual(0, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLastIndexOfWithIndexAndCountAndRemove()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            int index = -1;
            Thread t = new Thread(
                () =>
                {
                    index = cList.LastIndexOf(1, 2, 3);
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cList.Count);
            NUnit.Framework.Assert.AreEqual(0, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverse()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Reverse();
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(cList[0] == 4 || cList[0] == 42);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverseCastAsIListViaLinq()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var reversed = cList.Reverse();  // not excuted until loop over it
            /*
            It is a bit tricky why we need to catch this exception. If you look at the definition of the Reverse method, 
            you can see that it takes an IEnumerable<T> as a parameter. 
            And we know that in our case, the IEnumerable<T> is a copy of the original list anyway. So, anyway, it should throw no exception here.
            However, if you look at the source code of LINQ (specifically, .NET Framework 4.5.1) and see how it implements the Reverse method, 
            you can see that it will try to cast the IEnumerable as ICollection first. 
            If it succeeds, it will use the CopyTo method instead of looping over with IEnumerable.
            With this implementation, it’s still thread-safe. However, it will throw an ArgumentException when the original list's Count changes 
            after GetEnumerator but before CopyTo.
            To avoid such an exception, you need to lock the ThreadSafeList instance (using it as an IList) 
            when concurrently looping over the returned IEnumerable of Reverse (or other LINQ methods) and list modifications. See the next test for detail
            */
            try
            {
                foreach (var item in reversed)
                {
                    var v = item;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverseCastAsIListViaLinqNoException()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    lock (cList)
                    {
                        cList.Add(42);
                    }
                });
            t.Start();

            var reversed = cList.Reverse();
            lock (cList)
            {
                foreach (var item in reversed)
                {
                    var v = item;
                }
            }

            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndReverseWithIndexAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Reverse(1, 2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, cList[2]);
            NUnit.Framework.Assert.AreEqual(3, cList[1]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSort()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort();
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(1, cList[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparator()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort(Comparer<int>.Default);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(1, cList[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparison()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

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
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(1, cList[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSortWithComparisonWithStartIndexAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Sort(1, 2, Comparer<int>.Default);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, cList[1]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTrimExcess()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.TrimExcess();
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(cList.Capacity == 8 || cList.Capacity == 5);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearch()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.BinarySearch(listNode);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearchWithComparer()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.BinarySearch(listNode, Comparer<int>.Default);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndBinarySearchWithStartIndexAndCountAndComparer()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            var listNode = cList[2];

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.BinarySearch(1, 2, listNode, Comparer<int>.Default);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFind()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var element = cList.Find(e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(3, element);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindAll()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var allElements = cList.FindAll(e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(1, allElements.Count);
            NUnit.Framework.Assert.AreEqual(3, allElements[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedThreadSafeOfListReturnedByFindAll()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            var found = cList.FindAll(e => e == 3);

            Thread t = new Thread(
                () =>
                {
                    found.Add(12306);
                });
            t.Start();

            found.Add(777);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, found.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.FindIndex(e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndexWithStartIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.FindIndex(1, e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindIndexWithStartIndexAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.FindIndex(1, 3, e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLast()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            int element = cList.FindLast(e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(3, element);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            int index = cList.FindLastIndex(e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndexWithStartIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.FindLastIndex(1, e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(-1, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndFindLastIndexWithStartIndexAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var index = cList.FindLastIndex(3, 3, e => e == 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndExists()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var exists = cList.Exists(e => e == 2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(exists);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTrueForAll()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var trueForAll = cList.TrueForAll(e => e == 2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsFalse(trueForAll);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndForEach()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.ForEach(e => Console.WriteLine(e));
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndConvertAll()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var convertAll = cList.ConvertAll<string>(e => e.ToString());
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(convertAll.Count == 5 || convertAll.Count == 4);
            NUnit.Framework.Assert.AreEqual("1", convertAll[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndAsReadOnly()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Remove(4);
                });
            t.Start();

            var result = cList.AsReadOnly();
            foreach (var e in result)
            {
                var e1 = e;
            }
            t.Join();
            NUnit.Framework.Assert.IsTrue(result.Count == 3 || result.Count == 4);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructor()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>();

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(1);
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithCapacity()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(10);

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            cList.Add(1);
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyTo()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(result);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithArrayIndex()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndCopyToWithArrayIndexAs(cList);
        }

        private void TestPassedConcurrentAddAndCopyToWithArrayIndexAs<T>(T cList) where T : IList<int>
        {
            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(result, 3);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(1, result[3]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithArrayIndexCastAsIList()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });
            TestPassedConcurrentAddAndCopyToWithArrayIndexAs(cList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithBothIndexAndCount()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var result = new int[10];
            cList.CopyTo(1, result, 0, 2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, result[0]);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetRange()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var getRange = cList.GetRange(1, 2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.AreEqual(2, getRange.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedThreadSafeOfListReturnedByGetRange()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            var found = cList.GetRange(1, 2);

            Thread t = new Thread(
                () =>
                {
                    found.Add(12306);
                });
            t.Start();

            found.Add(777);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, found.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndToArray()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var array = cList.ToArray();
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(array.Length == 5 || array.Length == 4);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndToArrayCastAsIListViaLinq()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();
            int[] a = null;
            try
            {
                a = cList.ToArray();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(a == null || a.Length == 4 || a.Length == 5);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndToArrayCastAsIListViaLinqNoException()
        {
            IList<int> cList = new ThreadSafeList<int>(new List<int> { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    lock (cList)
                    {
                        cList.Add(42);
                    }
                });
            t.Start();
            int[] a = null;
            lock (cList)
            {
                a = cList.ToArray();
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cList.Count);
            NUnit.Framework.Assert.IsTrue(a == null || a.Length == 4 || a.Length == 5);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedSyncRoot()
        {
            ThreadSafeList<int> cList = new ThreadSafeList<int>();

            Thread t = new Thread(
                () =>
                {
                    cList.Add(42);
                });
            t.Start();

            var syncRoot = cList.SyncRoot;
            t.Join();
            NUnit.Framework.Assert.AreEqual(1, cList.Count);
            NUnit.Framework.Assert.IsNotNull(syncRoot);
        }

    }
}
