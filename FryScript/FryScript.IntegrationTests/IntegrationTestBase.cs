using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        public ScriptRuntime ScriptRuntime { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ScriptRuntime = new ScriptRuntime();
        }

        public dynamic Eval(string script)
        {
            return ScriptRuntime.Eval(script);
        }

        public dynamic Get(string name)
        {
            return ScriptRuntime.Get(name);
        }
    }
}
