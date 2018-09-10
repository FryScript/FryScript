using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FryScript.HostInterop;
using System.Reflection;
using System.Linq;

namespace FryScript.UnitTests.HostInterop
{
    public interface INonScriptableType
    {

    }

    public class NonScriptableType
    {
    }

    [ScriptableType("[test]")]
    public class ScriptableType
    {
    }

    [ScriptableType("[test]")]
    public class VirtualScriptableType
    {
        [ScriptableMethod("method")]
        public virtual string Method(object a, int b, string c)
        {
            return null;
        }
    }

    [ScriptableType("[test]")]
    public class VirtualVoidScriptableType
    {
        [ScriptableMethod("method")]
        public virtual void Method(object a, int b, string c)
        {
        }
    }

    [ScriptableType("[test]")]
    public abstract class AbstractScriptableType
    {
        [ScriptableMethod("method")]
        public abstract void Method(object a, int b, string c);
    }

    [TestClass]
    public class TypeFactoryTests
    {
        private TypeFactory _typeFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _typeFactory = new TypeFactory();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateScriptableTypeNullType()
        {
            _typeFactory.CreateScriptableType(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateScriptableTypeMissingScriptableTypeAttribute()
        {
            _typeFactory.CreateScriptableType(typeof(NonScriptableType));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateScriptableTypeInterfaceType()
        {
            _typeFactory.CreateScriptableType(typeof(INonScriptableType));
        }

        [TestMethod]
        public void CreateScriptableTypeAutoImplementsIScriptable()
        {
            var type = _typeFactory.CreateScriptableType(typeof(ScriptableType));

            Assert.IsTrue(type.GetTypeInfo().ImplementedInterfaces.Any(i => i == typeof(IScriptable)));
        }

        [TestMethod]
        public void CreateScriptableTypeOverridesVirtualMethod()
        {
            var type = _typeFactory.CreateScriptableType(typeof(VirtualScriptableType));

            var baseMethod = typeof(VirtualScriptableType).GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(VirtualScriptableType.Method));

            var overrideMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(VirtualScriptableType.Method));

            var proxyMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == $"<FryScript><{nameof(VirtualScriptableType.Method)}>_proxy");

            AssertMethods(baseMethod, overrideMethod);
            AssertMethods(baseMethod, proxyMethod);
            Assert.IsNotNull(proxyMethod.GetCustomAttribute<RuntimeOverrideAttribute>());
        }

        [TestMethod]
        public void CreateScriptableTypeOverridesVirtualMethodVoidReturnType()
        {
            var type = _typeFactory.CreateScriptableType(typeof(VirtualVoidScriptableType));

            var baseMethod = typeof(VirtualVoidScriptableType).GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(VirtualVoidScriptableType.Method));

            var overrideMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(VirtualVoidScriptableType.Method));

            var proxyMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == $"<FryScript><{nameof(VirtualVoidScriptableType.Method)}>_proxy");

            AssertMethods(baseMethod, overrideMethod);
            AssertMethods(baseMethod, proxyMethod);
            Assert.IsNotNull(proxyMethod.GetCustomAttribute<RuntimeOverrideAttribute>());
        }

        [TestMethod]
        public void CreateScriptableTypeImplementsAbstractMethod()
        {
            var type = _typeFactory.CreateScriptableType(typeof(AbstractScriptableType));

            var baseMethod = typeof(AbstractScriptableType).GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(AbstractScriptableType.Method));

            var overrideMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == nameof(AbstractScriptableType.Method));

            var proxyMethod = type.GetTypeInfo().GetRuntimeMethods().Single(m => m.Name == $"<FryScript><{nameof(AbstractScriptableType.Method)}>_proxy");

            AssertMethods(baseMethod, overrideMethod);
            AssertMethods(baseMethod, proxyMethod);
            Assert.IsNotNull(proxyMethod.GetCustomAttribute<RuntimeOverrideAttribute>());
        }

        private static void AssertMethods(MethodInfo baseMethod, MethodInfo proxyMethod)
        {
            Assert.AreEqual(baseMethod.ReturnType, proxyMethod.ReturnType);

            var baseParams = baseMethod.GetParameters();
            var overrideParams = proxyMethod.GetParameters();

            Assert.AreEqual(baseParams.Length, overrideParams.Length);

            for (var i = 0; i < baseParams.Length; i++)
            {
                Assert.AreEqual(baseParams[i].Name, overrideParams[i].Name);
                Assert.AreEqual(baseParams[i].ParameterType, overrideParams[i].ParameterType);
            }
        }
    }
}
