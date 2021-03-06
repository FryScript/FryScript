﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptTupleTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullItems()
        {
            new ScriptTuple(null);
        }

        [TestMethod]
        public void IndexGetSingleIndexTest()
        {
            var tuple = new ScriptTuple("test");
            Assert.AreEqual("test", tuple[0]);
        }

        [TestMethod]
        public void IndexGetMultipleIndexTest()
        {
            var tuple = new ScriptTuple("test", 100, true);
            Assert.AreEqual("test", tuple[0]);
            Assert.AreEqual(100, tuple[1]);
            Assert.AreEqual(true, tuple[2]);

        }

        [TestMethod]
        public void IndexGetPositiveIndexOutOfRange()
        {
            var tuple = new ScriptTuple("range test");
            Assert.IsNull(tuple[1]);
        }

        [TestMethod]
        public void WrapTupleDoesNotWrapTuple()
        {
            var tuple = new ScriptTuple(0, 1);

            var result = ScriptTuple.WrapTuple(tuple);

            Assert.AreEqual(tuple, result);
        }

        [TestMethod]
        public void WrapTupleWrapsNonTuple()
        {
            var nonTuple = new object();

            var result = ScriptTuple.WrapTuple(nonTuple);

            Assert.AreEqual(nonTuple, result[0]);
        }
    }
}
