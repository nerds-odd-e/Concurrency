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
            var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
            TestPassedConcurrentAddAndGetByKeyAs(cSortedList);
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByKeyCastAsIDictionary()
        {
            var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });
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

    }
}
