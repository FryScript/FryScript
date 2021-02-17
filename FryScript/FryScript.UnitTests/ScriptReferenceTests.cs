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
        private dynamic _import;
        private IScriptObject _importedObject;
        private MetaScriptObjectBase metaImportedObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _importedObject = Substitute.For<IScriptObject>();
            metaImportedObject = Substitute.For<MetaScriptObjectBase>(Expression.Constant(true), BindingRestrictions.Empty, new object());
            _importedObject.GetMetaObject(Arg.Any<Expression>()).Returns(metaImportedObject);
            _import = new ScriptImport(() => _importedObject);
        }

        [TestMethod]
        public void Forwards_Get_Member()
        {
            metaImportedObject.BindGetMember(Arg.Any<GetMemberBinder>()).Returns(GetMetaObject("get member"));
            
            var result = _import.test;

            Assert.AreEqual("get member", result);
        }

        [TestMethod]
        public void Forwards_Set_Member()
        {
            metaImportedObject.BindSetMember(Arg.Any<SetMemberBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("set member"));

            var result = _import.test = "set member";

            Assert.AreEqual("set member", result);
        }

        [TestMethod]
        public void Forwards_Get_Index()
        {
            metaImportedObject.BindGetIndex(Arg.Any<GetIndexBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("get index"));

            var result = _import["test"];

            Assert.AreEqual("get index", result);
        }

        [TestMethod]
        public void Forwards_Set_Index()
        {
            metaImportedObject.BindSetIndex(Arg.Any<SetIndexBinder>(), Arg.Any<DynamicMetaObject[]>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("set index"));

            var result = _import["test"] = "set index";

            Assert.AreEqual("set index", result);
        }

        [TestMethod]
        public void Forwards_Invoke_Member()
        {
            metaImportedObject.BindInvokeMember(Arg.Any<InvokeMemberBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("invoke member"));

            var result = _import.test();

            Assert.AreEqual("invoke member", result);
        }

        [TestMethod]
        public void Forwards_Invoke()
        {
            metaImportedObject.BindInvoke(Arg.Any<InvokeBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("invoke"));

            var result = _import();

            Assert.AreEqual("invoke", result);
        }

        [TestMethod]
        public void Forwards_Binary_Operation()
        {
            metaImportedObject.BindBinaryOperation(Arg.Any<BinaryOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("binary operation"));

            var result = _import + 1;

            Assert.AreEqual("binary operation", result);
        }

        [TestMethod]
        public void Forwards_Convert()
        {
            metaImportedObject.BindConvert(Arg.Any<ConvertBinder>())
                .Returns(GetMetaObject("convert"));

            var result = (string)_import;

            Assert.AreEqual("convert", result);
        }

        [TestMethod]
        public void Forwards_Create_Instance()
        {
            metaImportedObject.BindCreateInstance(Arg.Any<CreateInstanceBinder>(), Arg.Any<DynamicMetaObject[]>())
                .Returns(GetMetaObject("create instance"));

            var binder = new ScriptCreateInstanceBinder(0);
            var callsite = CallSite.Create(typeof(Func<CallSite, object, object>), binder) as CallSite<Func<CallSite, object, object>>;

            var result = callsite.Target(callsite, _import);

            Assert.AreEqual("create instance", result);
        }

        [TestMethod]
        public void Forwards_Is_Operation()
        {
            metaImportedObject.BindIsOperation(Arg.Any<ScriptIsOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("is operation"));

            var binder = new ScriptIsOperationBinder();
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object, object>), binder) as CallSite<Func<CallSite, object, object, object>>;

            var result = callSite.Target(callSite, _import, new object());

            Assert.AreEqual("is operation", result);
        }

        [TestMethod]
        public void Forwards_Has_Operation()
        {
            metaImportedObject.BindHasOperation(Arg.Any<ScriptHasOperationBinder>())
                .Returns(GetMetaObject("has operation"));

            var binder = new ScriptHasOperationBinder("test");
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object>), binder) as CallSite<Func<CallSite, object, object>>;

            var result = callSite.Target(callSite, _import);

            Assert.AreEqual("has operation", result);
        }

        [TestMethod]
        public void Forwards_Extends_Operation()
        {
            metaImportedObject.BindExtendsOperation(Arg.Any<ScriptExtendsOperationBinder>(), Arg.Any<DynamicMetaObject>())
                .Returns(GetMetaObject("extends operation"));
            
            var binder = new ScriptExtendsOperationBinder();
            var callSite = CallSite.Create(typeof(Func<CallSite, object, object, object>), binder) as CallSite<Func<CallSite, object, object, object>>;

            var result = callSite.Target(callSite, _import, new object());

            Assert.AreEqual("extends operation", result);
        }
        
        private static DynamicMetaObject GetMetaObject(object value)
        {
            return new DynamicMetaObject(Expression.Constant(value), BindingRestrictions.GetExpressionRestriction(Expression.Constant(true)));
        }
    }
}
