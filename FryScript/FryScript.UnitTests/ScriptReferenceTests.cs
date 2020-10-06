using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptReferenceTests
    {
        private dynamic _reference;
        private IScriptObject _referencedObject;
        private MetaScriptObjectBase _metaReferencedObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _referencedObject = Substitute.For<IScriptObject>();
            _metaReferencedObject = Substitute.For<MetaScriptObjectBase>(Expression.Constant(true), BindingRestrictions.Empty, new object());
            _referencedObject.GetMetaObject(Arg.Any<Expression>()).Returns(_metaReferencedObject);
            _reference = new ScriptObjectReference(() => _referencedObject);
        }

        [TestMethod]
        public void Forwards_Get_Member()
        {
            _metaReferencedObject.BindGetMember(Arg.Any<GetMemberBinder>()).Returns(GetMetaObject("get member"));
            
            var result = _reference.test;

            Assert.AreEqual("get member", result);
        }

        [TestMethod]
        public void Forwards_Set_Member()
        {
            _metaReferencedObject.BindSetMember(Arg.Any<SetMemberBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("set value"));

            var result = _reference.test = "set member";

            Assert.AreEqual("set member", result);
        }

        [TestMethod]
        public void Forwards_Get_Index()
        {
            _metaReferencedObject.BindGetIndex(Arg.Any<GetIndexBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("get index"));

            var result = _reference["test"];

            Assert.AreEqual("get index", result);
        }

        [TestMethod]
        public void Forwards_Set_Index()
        {
            _metaReferencedObject.BindSetIndex(Arg.Any<SetIndexBinder>(), Arg.Any<DynamicMetaObject[]>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("set index"));

            var result = _reference["test"] = "set index";

            Assert.AreEqual("set index", result);
        }

        [TestMethod]
        public void Forwards_Invoke_Member()
        {
            _metaReferencedObject.BindInvokeMember(Arg.Any<InvokeMemberBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("invoke member"));

            var result = _reference.test();

            Assert.AreEqual("invoke member", result);
        }

        [TestMethod]
        public void Forwards_Invoke()
        {
            _metaReferencedObject.BindInvoke(Arg.Any<InvokeBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("invoke"));

            var result = _reference();

            Assert.AreEqual("invoke", result);
        }

        [TestMethod]
        public void Forwards_Binary_Operation()
        {
            _metaReferencedObject.BindBinaryOperation(Arg.Any<BinaryOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("binary operation"));

            var result = _reference + 1;

            Assert.AreEqual("binary operation", result);
        }

        [TestMethod]
        public void Forwards_Convert()
        {
            _metaReferencedObject.BindConvert(Arg.Any<ConvertBinder>())
                .Returns(GetMetaObject("convert"));

            var result = (string)_reference;

            Assert.AreEqual("convert", result);
        }

        [TestMethod]
        public void Forwards_Create_Instance()
        {
            _metaReferencedObject.BindCreateInstance(Arg.Any<CreateInstanceBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("create instance"));

            var binder = new ScriptCreateInstanceBinder(0);
            var callsite = CallSite.Create(typeof(Func<CallSite, object, object>), binder) as CallSite<Func<CallSite, object, object>>;

            var result = callsite.Target(callsite, _reference);

            Assert.AreEqual("create instance", result);
        }

        [TestMethod]
        public void Forwards_Is_Operation()
        {
            _metaReferencedObject.BindIsOperation(Arg.Any<ScriptIsOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("is operation"));

            var binder = new ScriptIsOperationBinder();
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object, object>), binder) as CallSite<Func<CallSite, object, object, object>>;

            var result = callSite.Target(callSite, _reference, new object());

            Assert.AreEqual("is operation", result);
        }

        [TestMethod]
        public void Forwards_Has_Operation()
        {
            _metaReferencedObject.BindHasOperation(Arg.Any<ScriptHasOperationBinder>())
                .Returns(GetMetaObject("has operation"));

            var binder = new ScriptHasOperationBinder("test");
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object>), binder) as CallSite<Func<CallSite, object, object>>;

            var result = callSite.Target(callSite, _reference);

            Assert.AreEqual("has operation", result);
        }

        [TestMethod]
        public void Forwards_Extends_Operation()
        {
            _metaReferencedObject.BindExtendsOperation(Arg.Any<ScriptExtendsOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("extends operation"));
            
            var binder = new ScriptExtendsOperationBinder();
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object, object>), binder) as CallSite<Func<CallSite, object, object, object>>;

            var result = callSite.Target(callSite, _reference, new object());

            Assert.AreEqual("extends operation", result);
        }
        
        private static DynamicMetaObject GetMetaObject(object value)
        {
            return new DynamicMetaObject(Expression.Constant(value), BindingRestrictions.GetExpressionRestriction(Expression.Constant(true)));
        }
    }
}
