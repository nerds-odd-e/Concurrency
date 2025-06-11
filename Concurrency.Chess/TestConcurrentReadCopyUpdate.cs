using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Concurrency.TestTools.UnitTesting;
using Microsoft.Concurrency.TestTools.UnitTesting.Chess;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

//**********************************************************************************************
// Instruct Chess to instrument the System.dll assembly so we can catch data races in CLR classes 
// such as LinkedList{T}
//**********************************************************************************************
[assembly: ChessInstrumentAssembly("System")]
[assembly: ChessInstrumentAssembly("mscorlib")]
[assembly: ChessInstrumentAssembly("Concurrency")]

namespace Concurrency.Chess
{
    /// <summary>
    /// Test the <see cref="ReadCopyUpdateList{T}"/> in a concurrent environment.
    /// </summary>
    [TestFixture]
    public class TestConcurrentReadCopyUpdate
    {
        private static LinkedList<int> llist;
        private static LinkedListNode<int> lnode3;
        private static ReadCopyUpdateList<int> rculist;
        private static ReadCopyUpdateListNode<int> rcunode3;
        private static List<int> list;
        private static int listNode;
        private static Dictionary<int, string> dic;

        /// <summary>
        /// Test we can read while updating.
        /// </summary>
        [Test]
        [DataRaceTestMethod]
        public void TestPassedConcurrentReadAndUpdate()
        {
            rculist = new ReadCopyUpdateList<int>();
            rculist.AddValues(1, 2, 3, 4);
            rcunode3 = rculist.Find(3);
            Assert.IsNotNull(rcunode3);

            Parallel.Invoke(
                () =>
                    {
                        // Reader thread
                        ReadCopyUpdateListNode<int> node = rculist.First;
                        int count = 0;
                        while (node != null)
                        {
                            node = node.Next;
                            count++;
                        }
                        Debug.WriteLine("Read completed - count was: " + count);
                    },
                () =>
                    {
                        // Updater thread
                        rculist.Remove(rcunode3);
                        Debug.WriteLine("Node 3 removed");
                    });
        }

        /// <summary>
        /// Test we can read while updating using a standard linked list.
        /// 
        /// If ran under Chess (comment out the Ignore attribute) this test fails with a "DATARACE" error.
        /// This is because the "next" pointers in the <see cref="LinkedListNode{T}"/> class are not volitile
        /// and therefore updates made by one thread are not neccessarily immediately seen by other threads.
        /// 
        /// Compare this with the <see cref="TestConcurrentReadAndUpdate"/> test that uses <see cref="ReadCopyUpdateList{T}"/>.
        /// Both tests perform the same operation but <see cref="ReadCopyUpdateList{T}"/> does not suffer from the "DATARACE" error.
        /// </summary>
        //[Microsoft.Concurrency.TestTools.UnitTesting.Ignore]
        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentEnumerateReadAndUpdateWithLinkedList()
        {
            llist = new LinkedList<int>(new[] { 1, 2, 3, 4 });
            lnode3 = llist.Find(3);
            Assert.IsNotNull(lnode3);

            Parallel.Invoke(
                () =>
                {
                    // Reader thread
                    LinkedListNode<int> node = llist.First;
                    int count = 0;
                    while (node != null)
                    {
                        node = node.Next;
                        count++;
                    }
                    Debug.WriteLine("Read completed - count was: " + count);
                },
                () =>
                {
                    // Updater thread
                    llist.Remove(lnode3);
                    Debug.WriteLine("Node 3 removed");
                });
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentLoopReadAndUpdateWithLinkedList()
        {
            llist = new LinkedList<int>(new[] { 1, 2, 3, 4 });
            lnode3 = llist.Find(3);
            Assert.IsNotNull(lnode3);

            Thread t = new Thread(
                () =>
                {
                    foreach (var v in llist)
                    {
                        Console.WriteLine("Read list with value " + v);
                    }
                });
            t.Start();

            // Updater thread
            llist.Remove(lnode3);
            Debug.WriteLine("Node 3 removed");
            t.Join();
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRaceConcurrentEnumerateReadAndUpdateWithList()
        {
            list = new List<int>(new[] { 1, 2, 3, 4 });
            listNode = list[2];
            Assert.IsNotNull(listNode);

            Thread t = new Thread(
                () =>
                {
                    var enumerater = list.GetEnumerator();
                    while (enumerater.MoveNext())
                    {
                        Console.WriteLine("Read list wth value " + enumerater.Current);
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
        public void TestDataRaceConcurrentLoopReadAndUpdateWithList()
        {
            list = new List<int>(new[] { 1, 2, 3, 4 });
            listNode = list[2];
            Assert.IsNotNull(listNode);

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
