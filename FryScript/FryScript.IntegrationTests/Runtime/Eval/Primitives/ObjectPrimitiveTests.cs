using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class ObjectPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Object()
        {
            Eval("@import \"object\" as object;");

            var result = Eval("new object();");

            Assert.IsInstanceOfType(result, typeof(ScriptObject));
        }

        [TestMethod]
        public void Object_Literal_One_Member()
        {
            var result = Eval("{m1: 1};");

            Assert.AreEqual(1, result.m1);
        }

        [TestMethod]
        public void Object_Literal_Two_Members()
        {
            var result = Eval("{m1: 1, m2: 2};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
        }

        [TestMethod]
        public void Object_Literal_Three_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
        }

        [TestMethod]
        public void Object_Literal_Four_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
        }

        [TestMethod]
        public void Object_Literal_Five_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
        }

        [TestMethod]
        public void Object_Literal_Six_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5, m6: 6};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
            Assert.AreEqual(6, result.m6);
        }


        [TestMethod]
        public void Object_Literal_Seven_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5, m6: 6, m7: 7};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
            Assert.AreEqual(6, result.m6);
            Assert.AreEqual(7, result.m7);
        }

        [TestMethod]
        public void Object_Literal_Eight_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5, m6: 6, m7: 7, m8: 8};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
            Assert.AreEqual(6, result.m6);
            Assert.AreEqual(7, result.m7);
            Assert.AreEqual(8, result.m8);
        }

        [TestMethod]
        public void Object_Literal_Nine_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5, m6: 6, m7: 7, m8: 8, m9: 9};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
            Assert.AreEqual(6, result.m6);
            Assert.AreEqual(7, result.m7);
            Assert.AreEqual(8, result.m8);
            Assert.AreEqual(9, result.m9);
        }

        [TestMethod]
        public void Object_Literal_Ten_Members()
        {
            var result = Eval("{m1: 1, m2: 2, m3: 3, m4: 4, m5: 5, m6: 6, m7: 7, m8: 8, m9: 9, m10: 10};");

            Assert.AreEqual(1, result.m1);
            Assert.AreEqual(2, result.m2);
            Assert.AreEqual(3, result.m3);
            Assert.AreEqual(4, result.m4);
            Assert.AreEqual(5, result.m5);
            Assert.AreEqual(6, result.m6);
            Assert.AreEqual(7, result.m7);
            Assert.AreEqual(8, result.m8);
            Assert.AreEqual(9, result.m9);
            Assert.AreEqual(10, result.m10);
        }

        [TestMethod]
        public void Object_Literal_ToString()
        {
            var result = Eval("({}).toString();");

            Assert.AreEqual("object", result);
        }
    }
}