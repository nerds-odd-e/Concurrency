using Microsoft.Concurrency.TestTools.UnitTesting;
using Microsoft.Concurrency.TestTools.UnitTesting.Chess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: ChessInstrumentAssembly("System")]
[assembly: ChessInstrumentAssembly("mscorlib")]
[assembly: ChessInstrumentAssembly("Concurrency")]

namespace Concurrency.Chess
{
    [TestFixture]
    public class TestNonThreadSafeCollections
    {
        private static List<int> list;
        private static int listNode;
        private static Dictionary<int, string> dic;

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentEnumerateReadAndUpdateWithList()
        {
            list = new List<int>(new[] { 1, 2, 3, 4 });
            listNode = list[2];

            Thread t = new Thread(
                () =>
                {
                    var enumerater = list.GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        int current = enumerater.Current;
                    }
                });
            t.Start();

            list.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentLoopReadAndUpdateWithList()
        {
            list = new List<int>(new[] { 1, 2, 3, 4 });
            listNode = list[2];

            Thread t = new Thread(
                () =>
                {
                    foreach (var v in list)
                    {
                        Console.WriteLine("Read list with value " + v);
                    }
                });
            t.Start();

            // Updater thread
            list.Remove(listNode);
            Debug.WriteLine("Node 3 removed");
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentAddWithList()
        {
            list = new List<int>();

            Thread t = new Thread(
                () =>
                {
                    list.Add(10);
                });
            t.Start();

            list.Add(10);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentReadAndRemoveWithDictionary()
        {
            var dic = new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } };

            Thread t = new Thread(
                () =>
                {
                    try
                    {
                        var value = dic[10];
                    }
                    catch (Exception e)
                    {
                    }
                });
            t.Start();

            try
            {
                dic.Remove(10);
            }
            catch (Exception e)
            {
            }
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentLoopReadAndRemoveWithDictionary()
        {
            dic = new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } };
            Thread t = new Thread(
                () =>
                {
                    foreach (var v in dic)
                    {
                        var str = "Read dic with key " + v.Key + " was: " + v.Value;
                    }
                });
            t.Start();

            try
            {
                dic.Remove(10);
            }
            catch (Exception e)
            {
            }
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentReadAndWriteDifferentKeyWithDictionary()
        {
            var dic = new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } };

            Thread t = new Thread(
                () =>
                {
                    var task10 = dic[10];
                });
            t.Start();

            // Updater thread
            dic[11] = "task11";
            t.Join();
        }

    }
}
