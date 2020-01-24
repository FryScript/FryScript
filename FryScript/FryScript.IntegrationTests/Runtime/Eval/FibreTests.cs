using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FibreTests : IntegrationTestBase
    {
        [TestMethod]
        public void Fibre_With_No_Parameters()
        {
            var result = Eval("fibre () => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_One_Parameter_No_Parens()
        {
            var result = Eval("fibre p1 => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));

        }

        [TestMethod]
        public void Fibre_With_One_Parameter()
        {
            var result = Eval("fibre (p1) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));

        }


        [TestMethod]
        public void Fibre_With_Two_Parameters()
        {
            var result = Eval("fibre (p1, p2) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));

        }

        [TestMethod]
        public void Fibre_With_Three_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Four_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Five_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Six_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Seven_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Eight_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Nine_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Ten_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Eleven_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Twelve_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Thirteen_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Fourteen_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Fifteen_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }

        [TestMethod]
        public void Fibre_With_Sixteen_Parameters()
        {
            var result = Eval("fibre (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
        }
    }
}