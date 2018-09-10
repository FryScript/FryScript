//using FryScript.HostInterop;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FryScript.UnitTests.HostInterop
//{
//    [TestClass]
//    public class TypeDescriptor2Tests
//    {
//        private class Nested
//        {
//            public string Property { get; set; }
//        }

//        private class Test
//        {
//            public Nested NestedProperty => null;

//            public object Property => null;

//            public object this[string key1, int key2] => null;

//            public object Method() => null;
//        }

//        private TypeDescriptor2<Test> _typeDescriptor;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            _typeDescriptor = new TypeDescriptor2<Test>();
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void PropertyNullLambdaTest()
//        {
//            _typeDescriptor.Property<object>(null);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void PropertyNoMemberTest()
//        {
//            _typeDescriptor.Property(t => new object());
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void PropertyNonPropertyTest()
//        {
//            _typeDescriptor.Property(t => t.Method());
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void PropertyNestedMemberTest()
//        {
//            _typeDescriptor.Property(t => t.NestedProperty.Property);
//        }

//        [TestMethod]
//        public void PropertyTest()
//        {
//            _typeDescriptor.Property(t => t.Property);

//            Assert.IsTrue(_typeDescriptor.Properties.ContainsKey("property"));
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void IndexNullLambdaTest()
//        {
//            _typeDescriptor.Index<object>(null);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void IndexNonIndexTest()
//        {
//            _typeDescriptor.Index(t => new object());
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void IndexMethodTest()
//        {
//            _typeDescriptor.Index(t => t.Method());
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void IndexMultipleIndexTest()
//        {
//            _typeDescriptor.Index(t => t[Using<string>.Arg, Using<int>.Arg]);
//        }
//    }
//}
