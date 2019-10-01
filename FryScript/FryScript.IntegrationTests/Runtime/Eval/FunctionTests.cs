using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FunctionTests : IntegrationTestBase
    {
        [TestMethod]
        public void Function_With_No_Parameters()
        {
            var result = Eval("() => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_One_Parameter()
        {
            var result = Eval("p1 => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Two_Parameters()
        {
            var result = Eval("(p1, p2) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Three_Parameters()
        {
            var result = Eval("(p1, p2, p3) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Four_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Five_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Six_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Seven_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Eight_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Nine_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Ten_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_Eleven_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        public void Function_With_Twelve_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        public void Function_With_Thirteen_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        public void Function_With_Fourteen_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        public void Function_With_Fifteen_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        public void Function_With_Sixteen_Parameters()
        {
            var result = Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }


        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Function_With_Seventeen_Parameters()
        {
            Eval("(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => {};");
        }

        [TestMethod]
        public void Function_Return_Value()
        {
            var result = Eval("() => false;");

            Assert.IsFalse(result());
        }

        [TestMethod]
        public void Function_Body()
        {
            var result = Eval("() => { 100; };");

            Assert.AreEqual(100, result());
        }

        [TestMethod]
        public void Function_Body_Returns_Last_Expression()
        {
            var result = Eval("() => { var x = 100; x;};");

            Assert.AreEqual(100, result());
        }

        [TestMethod]
        public void Function_Body_Return_Statement_With_Value()
        {
            var result = Eval("() => { return 100;};");

            Assert.AreEqual(100, result());
        }

        [TestMethod]
        public void Function_Body_Return_Statement_With_No_Value()
        {
            var result = Eval("() => { return; };");

            Assert.IsNull(result());
        }

        [TestMethod]
        public void Function_With_One_Argument()
        {
            var result = Eval("a => a;");

            Assert.AreEqual(1, result(1));
        }

        [TestMethod]
        public void Function_With_Two_Arguments()
        {
            var result = Eval("(a, b) => a + b;");

            Assert.AreEqual(2, result(1, 1));
        }

        [TestMethod]
        public void Function_With_Three_Arguments()
        {
            var result = Eval("(a, b, c) => a + b + c;");

            Assert.AreEqual(3, result(1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Four_Arguments()
        {
            var result = Eval("(a, b, c, d) => a + b + c + d;");

            Assert.AreEqual(4, result(1, 1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Five_Arguments()
        {
            var result = Eval("(a, b, c, d, e) => a + b + c + d + e;");

            Assert.AreEqual(5, result(1, 1, 1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Six_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f) => a + b + c + d + e + f;");

            Assert.AreEqual(6, result(1, 1, 1, 1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Seven_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g) => a + b + c + d + e + f + g;");

            Assert.AreEqual(7, result(1, 1, 1, 1, 1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Eight_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;");

            Assert.AreEqual(8, result(1, 1, 1, 1, 1, 1, 1, 1));
        }

        [TestMethod]
        public void Function_With_Nine_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;");

            Assert.AreEqual(9, result(1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Ten_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j;");

            Assert.AreEqual(10, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Eleven_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k;");

            Assert.AreEqual(11, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Twelve_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l;");

            Assert.AreEqual(12, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Thirteen_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m;");

            Assert.AreEqual(13, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Fourteen_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n;");

            Assert.AreEqual(14, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Fifteen_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o;");

            Assert.AreEqual(15, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }

        [TestMethod]
        public void Function_With_Sixteen_Arguments()
        {
            var result = Eval("(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p;");

            Assert.AreEqual(16, result(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));            
        }
    }
}