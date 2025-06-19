using Microsoft.Concurrency.TestTools.UnitTesting;
using Microsoft.Concurrency.TestTools.UnitTesting.Chess;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: ChessInstrumentAssembly("System")]
//[assembly: ChessInstrumentAssembly("mscorlib")]
[assembly: ChessInstrumentAssembly("Concurrency")]

namespace Concurrency.Chess
{
    [TestFixture]
    public class TestThreadSafeCollections
    {
        private static ArrayList cList;
        private static int listNode;
        private static ConcurrentDictionary<int, string> dic;
        private static ConcurrentQueue<int> cQueue;

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnumerateReadAndUpdateWithSynchronizedArrayList()
        {
            cList = ArrayList.Synchronized(new ArrayList(new[] { 1, 2, 3, 4 }));
            listNode = (int)cList[2];

            Thread t = new Thread(
                () =>
                {
                    lock (cList.SyncRoot)
                    {
                        var enumerater = cList.GetEnumerator();
                        while (enumerater.MoveNext())
                        {
                            int current = (int)enumerater.Current;
                        }
                    }
                });
            t.Start();

            // Updater thread
            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndUpdateWithSynchronizedArrayList()
        {
            cList = ArrayList.Synchronized(new ArrayList(new[] { 1, 2, 3, 4 }));
            listNode = (int)cList[2];

            Thread t = new Thread(
                () =>
                {
                    lock (cList.SyncRoot)
                    {
                        foreach (var v in cList)
                        {
                            object v1 = v;
                        }
                    }
                });
            t.Start();

            cList.Remove(listNode);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentAddWithSynchronizedArrayList()
        {
            cList = ArrayList.Synchronized(new ArrayList());

            Thread t = new Thread(
                () =>
                {
                    cList.Add(10);
                });
            t.Start();

            cList.Add(10);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndDequeueWithConcurrentQueue()
        {
            cQueue = new ConcurrentQueue<int>(new int[] { 1, 2, 3, 4 });

            Thread t = new Thread(
                () =>
                {
                    foreach (var v in cQueue)
                    {
                        int v1 = v;
                    }
                });
            t.Start();

            cQueue.TryDequeue(out int result);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentEnqueueWithConcurrentQueue()
        {
            cQueue = new ConcurrentQueue<int>();

            Thread t = new Thread(
                () =>
                {
                    cQueue.Enqueue(10);
                });
            t.Start();

            cQueue.Enqueue(10);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentReadAndRemoveWithConcurrentDictionary()
        {
            dic = new ConcurrentDictionary<int, string>(new Dictionary<int, string>() { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

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
                dic.TryRemove(10, out string result);
            }
            catch (Exception e)
            {
            }
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentLoopReadAndRemoveWithConcurrentDictionary()
        {
            dic = new ConcurrentDictionary<int, string>(new Dictionary<int, string>() { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

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
                dic.TryRemove(10, out string result);
            }
            catch (Exception e)
            {
            }
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentReadAndWriteDifferentKeyWithConcurrentDictionary()
        {
            var dic = new ConcurrentDictionary<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    var task10 = dic[10];
                });
            t.Start();

            dic[11] = "task11";
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentReadAndContainsKeySameKeyWithConcurrentDictionary()
        {
            var dic = new ConcurrentDictionary<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    var task10 = dic[10];
                });
            t.Start();

            var result = dic.ContainsKey(10);
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedConcurrentReadAndContainsKeyDifferentKeyWithConcurrentDictionary()
        {
            var dic = new ConcurrentDictionary<int, string>(new Dictionary<int, string> { { 1, "task1" }, { 10, "task10" }, { 20, "task20" } });

            Thread t = new Thread(
                () =>
                {
                    var task10 = dic[10];
                });
            t.Start();

            var result = dic.ContainsKey(20);
            t.Join();
        }

    }
}
