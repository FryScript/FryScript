using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class BuilderTests
    {
        [TestMethod]
        public void ScrpitObjectBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptObjectUri, Builder.ScriptObjectBuilder.Uri);
            Assert.AreEqual(null, Builder.ScriptObjectBuilder.Parent);
        }

        [TestMethod]
        public void ScrpitArrayBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptArrayUri, Builder.ScriptArrayBuilder.Uri);
            Assert.AreEqual(Builder.ScriptObjectBuilder, Builder.ScriptArrayBuilder.Parent);
        }

        [TestMethod]
        public void ScrpitErrorBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptErrorUri, Builder.ScriptErrorBuilder.Uri);
            Assert.AreEqual(Builder.ScriptObjectBuilder, Builder.ScriptErrorBuilder.Parent);
        }

        [TestMethod]
        public void ScrpitFibreBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptFibreUri, Builder.ScriptFibreBuilder.Uri);
            Assert.AreEqual(Builder.ScriptObjectBuilder, Builder.ScriptFibreBuilder.Parent);
        }

        [TestMethod]
        public void ScrpitFibreContextBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptFibreContextUri, Builder.ScriptFibreContextBuilder.Uri);
            Assert.AreEqual(Builder.ScriptObjectBuilder, Builder.ScriptFibreContextBuilder.Parent);
        }

        [TestMethod]
        public void ScrpitFunctionBuilder_Properties()
        {
            Assert.AreEqual(RuntimeUri.ScriptFunctionUri, Builder.ScriptFunctionBuilder.Uri);
            Assert.AreEqual(Builder.ScriptObjectBuilder, Builder.ScriptFunctionBuilder.Parent);
        }
    }
}