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
        public void Function_With_One_Parameter_No_Parens()
        {
            var result = Eval("p1 => {};");

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
        }

        [TestMethod]
        public void Function_With_One_Parameter()
        {
            var result = Eval("(p1) => {};");

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
            Eval("func = () => false;");

            Assert.IsFalse(Eval("func();"));
        }

        [TestMethod]
        public void Function_Body()
        {
            Eval("func = () => { 100; };");

            Assert.AreEqual(100, Eval("func();"));
        }

        [TestMethod]
        public void Function_Body_Returns_Last_Expression()
        {
            Eval("func = () => { var x = 100; x;};");

            Assert.AreEqual(100, Eval("func();"));
        }

        [TestMethod]
        public void Function_Body_Return_Statement_With_Value()
        {
            Eval("func = () => { return 100;};");

            Assert.AreEqual(100, Eval("func();"));
        }

        [TestMethod]
        public void Function_Body_Return_Statement_With_No_Value()
        {
            Eval("func = () => { return; };");

            Assert.IsNull(Eval("func();"));
        }

        [TestMethod]
        public void Function_With_One_Argument()
        {
            Eval("func = a => a;");

            Assert.AreEqual(1, Eval("func(1);"));
        }

        [TestMethod]
        public void Function_With_Two_Arguments()
        {
            Eval("func = (a, b) => a + b;");

            Assert.AreEqual(2, Eval("func(1, 1);"));
        }

        [TestMethod]
        public void Function_With_Three_Arguments()
        {
            Eval("func = (a, b, c) => a + b + c;");

            Assert.AreEqual(3, Eval("func(1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Four_Arguments()
        {
            Eval("func = (a, b, c, d) => a + b + c + d;");

            Assert.AreEqual(4, Eval("func(1, 1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Five_Arguments()
        {
            Eval("func = (a, b, c, d, e) => a + b + c + d + e;");

            Assert.AreEqual(5, Eval("func(1, 1, 1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Six_Arguments()
        {
            Eval("func = (a, b, c, d, e, f) => a + b + c + d + e + f;");

            Assert.AreEqual(6, Eval("func(1, 1, 1, 1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Seven_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g) => a + b + c + d + e + f + g;");

            Assert.AreEqual(7, Eval("func(1, 1, 1, 1, 1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Eight_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;");

            Assert.AreEqual(8, Eval("func(1, 1, 1, 1, 1, 1, 1, 1);"));
        }

        [TestMethod]
        public void Function_With_Nine_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;");

            Assert.AreEqual(9, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Ten_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j;");

            Assert.AreEqual(10, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Eleven_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k;");

            Assert.AreEqual(11, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Twelve_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l;");

            Assert.AreEqual(12, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Thirteen_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m;");

            Assert.AreEqual(13, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Fourteen_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n;");

            Assert.AreEqual(14, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Fifteen_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o;");

            Assert.AreEqual(15, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_With_Sixteen_Arguments()
        {
            Eval("func = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p;");

            Assert.AreEqual(16, Eval("func(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);"));            
        }

        [TestMethod]
        public void Function_Params()
        {
            Eval("func = params => params[0] + params[1] + params[2];");

            Assert.AreEqual(6, Eval("func(1,2,3);"));
        }

        [TestMethod]
        public void Function_Invoke_Member()
        {
            Eval("this.func = () => \"test\";");

            Assert.AreEqual("test", Eval("this.func();"));
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Return_Invalid_Context()
        {
            Eval("return;");
        }
    }
}