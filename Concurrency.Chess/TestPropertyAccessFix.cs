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
    [ChessInstrumentAssembly("nunit.framework", Exclude = true)]
    public class TestPropertyAccessFix
    {

        public class NonThreadSafePropertyAccess
        {

            private string _strPro;

            public string StrPro
            {
                get
                {
                    return _strPro;
                }
                set { _strPro = value; }
            }
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.DataRace)]
        public void TestDataRacePropertyReadAndWrite()
        {
            var obj = new NonThreadSafePropertyAccess();
            obj.StrPro = "default";

            Thread t = new Thread(
                () =>
                {
                    obj.StrPro = "test";
                });
            t.Start();

            var str = obj.StrPro;
            t.Join();
        }

        public class ThreadSafePropertyAccess
        {
            private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

            private string _strPro;

            public string StrPro
            {
                get
                {
                    locker.EnterReadLock();
                    try
                    {
                        return _strPro;
                    }
                    finally
                    {
                        locker.ExitReadLock();
                    }
                }
                set
                {
                    locker.EnterWriteLock();
                    try
                    {
                        _strPro = value;
                    }
                    finally
                    {
                        locker.ExitWriteLock();
                    }
                }
            }
        }

        [Test]
        [DataRaceTestMethod]
        [RegressionTestExpectedResult(TestResultType.Passed)]
        public void TestPassedPropertyReadAndWrite()
        {
            var obj = new ThreadSafePropertyAccess();
            obj.StrPro = "default";

            Thread t = new Thread(
                () =>
                {
                    obj.StrPro = "test";
                });
            t.Start();

            var str = obj.StrPro;
            t.Join();
        }

    }
}
