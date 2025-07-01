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
    [ChessInstrumentAssembly("System")]
    [ChessInstrumentAssembly("System.Core")]
    [ChessInstrumentAssembly("mscorlib")]
    [ChessInstrumentAssembly("nunit.framework", Exclude = true)]
    public class TestThreadSafeSortedList
    {
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAdd()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>();
            TestPassedConcurrentAddAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>();
            TestPassedConcurrentAddAs(cSortedList);
        }

        private void TestPassedConcurrentAddAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.Add(2, "value2");
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithIDictionary()
        {
            var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList.Add(12306, "train");
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByKey()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndGetByKeyAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByKeyCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndGetByKeyAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndGetByKeyAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var value = cSortedList[10];
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual("task10", value);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSetByKey()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndSetByKeyAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSetByKeyCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndSetByKeyAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndSetByKeyAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList[12306] = "train";
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndValues()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndValuesAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndValuesCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndValuesAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndValuesAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            ICollection<string> values = cSortedList.Values;
            foreach (var value in values)
            {
                var v = value;
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(values.Count == 3 || values.Count == 4);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndKeys()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndKeysAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndKeysCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndKeysAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndKeysAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            ICollection<int> values = cSortedList.Keys;
            foreach (var value in values)
            {
                var v = value;
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(values.Count == 3 || values.Count == 4);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContainsKey()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndContainsKeyAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContainsKeyCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndContainsKeyAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndContainsKeyAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var contains = cSortedList.ContainsKey(10);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(contains);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemove()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndRemoveAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndRemoveAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndRemoveAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var removed = cSortedList.Remove(10);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(removed);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTryGetValue()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndTryGetValueAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTryGetValueCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndTryGetValueAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndTryGetValueAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList.TryGetValue(10, out string value);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual("task10", value);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCount()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndCountAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCountCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndCountAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndCountAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var count = cSortedList.Count;
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(count == 3 || count == 4);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIsReadOnly()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndIsReadOnlyAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIsReadOnlyCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndIsReadOnlyAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndIsReadOnlyAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var isReadOnly = cSortedList.IsReadOnly;
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsFalse(isReadOnly);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddKeyValuePair()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>();
            TestPassedConcurrentAddKeyValuePairAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddKeyValuePairCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>();
            TestPassedConcurrentAddKeyValuePairAs(cSortedList);
        }

        private void TestPassedConcurrentAddKeyValuePairAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(new KeyValuePair<int, string>(1, "value1"));
                });
            t.Start();

            cSortedList.Add(new KeyValuePair<int, string>(2, "value2"));
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndClear()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndClearAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndClearCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndClearAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndClearAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList.Clear();
            t.Join();
            NUnit.Framework.Assert.IsTrue(cSortedList.Count == 0 || cSortedList.Count == 1);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContains()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndContainsAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContainsCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndContainsAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndContainsAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var contains = cSortedList.Contains(new KeyValuePair<int, string>(10, "task10"));
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(contains);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithIndex()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndCopyToWithIndexAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndCopyToWithIndexCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndCopyToWithIndexAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndCopyToWithIndexAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var array = new KeyValuePair<int, string>[cSortedList.Count];
            try
            {
                cSortedList.CopyTo(array, 0);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(array.Length == 4 || array.Length == 3);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveKeyValuePair()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndRemoveKeyValuePairAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveKeyValuePairCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndRemoveKeyValuePairAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndRemoveKeyValuePairAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var removed = cSortedList.Remove(new KeyValuePair<int, string>(10, "task10"));
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(removed);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndEnumerator()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndEnumeratorAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndEnumeratorCastAsIDictionary()
        {
            IDictionary<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndEnumeratorAs(cSortedList);
        }

        private void TestPassedConcurrentAddAndEnumeratorAs<T>(T cSortedList) where T : IDictionary<int, string>
        {
            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            foreach (var keyValue in cSortedList)
            {
                var key = keyValue.Key;
                var value = keyValue.Value;
            }
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadViaIEnumerableAndAdd()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    System.Collections.IEnumerator enumerater = ((System.Collections.IEnumerable)cSortedList).GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        int key = ((KeyValuePair<int, string>)enumerater.Current).Key;
                        string value = ((KeyValuePair<int, string>)enumerater.Current).Value;
                    }
                });
            t.Start();

            cSortedList.Add(42, "magic");
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithCapacity()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(42);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.Add(2, "value2");
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithComparer()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(Comparer<int>.Default);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.Add(2, "value2");
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithCapacityAndComparer()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(42, Comparer<int>.Default);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.Add(2, "value2");
            t.Join();
            NUnit.Framework.Assert.AreEqual(2, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConstructorWithIDictionaryAndComparer()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(
                new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } }, Comparer<int>.Default);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList.Add(123, "train");
            t.Join();
            NUnit.Framework.Assert.AreEqual(5, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndValuesReturnsIList()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            IList<string> values = cSortedList.Values;
            var value = values[2];
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual("task20", value);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndKeysReturnsIList()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            IList<int> keys = cSortedList.Keys;
            var key = keys[1];
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual(10, key);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentCapacityReadAndUpdate()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    var c = cSortedList.Capacity;
                });
            t.Start();

            cSortedList.Capacity = 10;
            t.Join();
            NUnit.Framework.Assert.AreEqual(10, cSortedList.Capacity);
            NUnit.Framework.Assert.AreEqual(3, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetComparer()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(Comparer<int>.Default);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            var comparer = cSortedList.Comparer;
            t.Join();
            NUnit.Framework.Assert.AreEqual(1, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual(Comparer<int>.Default, comparer);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndContainsValue()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var contains = cSortedList.ContainsValue("task20");
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(contains);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIndexOfKey()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            int index = cSortedList.IndexOfKey(10);
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual(1, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndIndexOfValue()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            int index = cSortedList.IndexOfValue("task10");
            t.Join();
            NUnit.Framework.Assert.AreEqual(4, cSortedList.Count);
            NUnit.Framework.Assert.AreEqual(1, index);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndRemoveAt()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList.RemoveAt(2);
            t.Join();
            NUnit.Framework.Assert.AreEqual(3, cSortedList.Count);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndTrimExcess()
        {
            ThreadSafeSortedList<int, string> cSortedList = new ThreadSafeSortedList<int, string>(10);

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.TrimExcess();
            t.Join();
            NUnit.Framework.Assert.AreEqual(1, cSortedList.Count);
            NUnit.Framework.Assert.IsTrue(cSortedList.Capacity == 4 || cSortedList.Capacity == 1);
        }

    }
}
