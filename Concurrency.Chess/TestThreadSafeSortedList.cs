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
    [ChessInstrumentAssembly("nunit.framework", Exclude = true)]
    public class TestThreadSafeSortedList
    {
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAdd()
        {
            var cSortedList = new ThreadSafeSortedList<int, string>();

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(1, "value1");
                });
            t.Start();

            cSortedList.Add(2, "value2");
            t.Join();
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
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndGetByKey()
        {
            var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            var value = cSortedList[10];
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddAndSetByKey()
        {
            var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    cSortedList.Add(42, "magic");
                });
            t.Start();

            cSortedList[12306] = "train";
            t.Join();
        }

        //[Test]
        //[DataRaceTestMethod]
        //[RegressionTestExpectedResult(TestResultType.Passed)]
        //public void TestPassedConcurrentAddAndValues()
        //{
        //    var cSortedList = new ThreadSafeSortedList<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

        //    Thread t = new Thread(
        //        () =>
        //        {
        //            cSortedList.Add(42, "magic");
        //        });
        //    t.Start();

        //    var values = cSortedList.Values;
        //    var v = values[0];
        //    t.Join();
        //}

    }
}
