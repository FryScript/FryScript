using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class TupleTests : IntegrationTestBase
    {
        [TestMethod]
        public void Tuple()
        {
            var result = (ScriptTuple)Eval("{1,2,3};");

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
        }

        [TestMethod]
        public void Two_Variable_Declarations()
        {
            Eval("var {t1, t2};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
        }

        [TestMethod]
        public void Three_Variable_Declarations()
        {
            Eval("var {t1, t2, t3};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
        }

        [TestMethod]
        public void Four_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
        }

        [TestMethod]
        public void Five_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
        }

        [TestMethod]
        public void Six_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5, t6};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
            Assert.IsNull(Eval("t6;"));
        }

        [TestMethod]
        public void Seven_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
            Assert.IsNull(Eval("t6;"));
            Assert.IsNull(Eval("t7;"));
        }

        [TestMethod]
        public void Eight_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
            Assert.IsNull(Eval("t6;"));
            Assert.IsNull(Eval("t7;"));
            Assert.IsNull(Eval("t8;"));
        }

        [TestMethod]
        public void Nine_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8, t9};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
            Assert.IsNull(Eval("t6;"));
            Assert.IsNull(Eval("t7;"));
            Assert.IsNull(Eval("t8;"));
            Assert.IsNull(Eval("t9;"));
        }

        [TestMethod]
        public void Ten_Variable_Declarations()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8, t9, t10};");

            Assert.IsNull(Eval("t1;"));
            Assert.IsNull(Eval("t2;"));
            Assert.IsNull(Eval("t3;"));
            Assert.IsNull(Eval("t4;"));
            Assert.IsNull(Eval("t5;"));
            Assert.IsNull(Eval("t6;"));
            Assert.IsNull(Eval("t7;"));
            Assert.IsNull(Eval("t8;"));
            Assert.IsNull(Eval("t9;"));
            Assert.IsNull(Eval("t10;"));
        }

        [TestMethod]
        public void Unpack_Two_Declaration_Tuple()
        {
            Eval("var {t1, t2} = {1, 2};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
        }

        [TestMethod]
        public void Unpack_Three_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3} = {1, 2, 3};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
        }

        [TestMethod]
        public void Unpack_Four_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4} = {1, 2, 3, 4};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
        }

        [TestMethod]
        public void Unpack_Five_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5} = {1, 2, 3, 4, 5};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
        }

        [TestMethod]
        public void Unpack_Six_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5, t6} = {1, 2, 3, 4, 5, 6};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
        }

        [TestMethod]
        public void Unpack_Seven_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7} = {1, 2, 3, 4, 5, 6, 7};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
        }

        [TestMethod]
        public void Unpack_Eight_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8} = {1, 2, 3, 4, 5, 6, 7, 8};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
        }

        [TestMethod]
        public void Unpack_Nine_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8, t9} = {1, 2, 3, 4, 5, 6, 7, 8, 9};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
            Assert.AreEqual(9, Eval("t9;"));
        }

        [TestMethod]
        public void Unpack_Ten_Declaration_Tuple()
        {
            Eval("var {t1, t2, t3, t4, t5, t6, t7, t8, t9, t10} = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
            Assert.AreEqual(9, Eval("t9;"));
            Assert.AreEqual(10, Eval("t10;"));
        }

        [TestMethod]
        public void Unpack_Two_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");

            Eval("{t1, t2} = {1, 2};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
        }

        [TestMethod]
        public void Unpack_Three_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");

            Eval("{t1, t2, t3} = {1, 2, 3};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
        }

        [TestMethod]
        public void Unpack_Four_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");

            Eval("{t1, t2, t3, t4} = {1, 2, 3, 4};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
        }

        [TestMethod]
        public void Unpack_Five_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");

            Eval("{t1, t2, t3, t4, t5} = {1, 2, 3, 4, 5};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
        }

        [TestMethod]
        public void Unpack_Six_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");
            Eval("var t6;");

            Eval("{t1, t2, t3, t4, t5, t6} = {1, 2, 3, 4, 5, 6};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
            Assert.AreEqual(6, Eval(("t6;")));
        }

        [TestMethod]
        public void Unpack_Seven_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");
            Eval("var t6;");
            Eval("var t7;");

            Eval("{t1, t2, t3, t4, t5, t6, t7} = {1, 2, 3, 4, 5, 6, 7};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
            Assert.AreEqual(6, Eval(("t6;")));
            Assert.AreEqual(7, Eval(("t7;")));
        }

        [TestMethod]
        public void Unpack_Eight_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");
            Eval("var t6;");
            Eval("var t7;");
            Eval("var t8;");

            Eval("{t1, t2, t3, t4, t5, t6, t7, t8} = {1, 2, 3, 4, 5, 6, 7, 8};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
            Assert.AreEqual(6, Eval(("t6;")));
            Assert.AreEqual(7, Eval(("t7;")));
            Assert.AreEqual(8, Eval(("t8;")));
        }

        [TestMethod]
        public void Unpack_Nine_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");
            Eval("var t6;");
            Eval("var t7;");
            Eval("var t8;");
            Eval("var t9;");

            Eval("{t1, t2, t3, t4, t5, t6, t7, t8, t9} = {1, 2, 3, 4, 5, 6, 7, 8, 9};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
            Assert.AreEqual(6, Eval(("t6;")));
            Assert.AreEqual(7, Eval(("t7;")));
            Assert.AreEqual(8, Eval(("t8;")));
            Assert.AreEqual(9, Eval(("t9;")));
        }

        [TestMethod]
        public void Unpack_Ten_Existing_Variables()
        {
            Eval("var t1;");
            Eval("var t2;");
            Eval("var t3;");
            Eval("var t4;");
            Eval("var t5;");
            Eval("var t6;");
            Eval("var t7;");
            Eval("var t8;");
            Eval("var t9;");
            Eval("var t10;");

            Eval("{t1, t2, t3, t4, t5, t6, t7, t8, t9, t10} = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};");

            Assert.AreEqual(1, Eval(("t1;")));
            Assert.AreEqual(2, Eval(("t2;")));
            Assert.AreEqual(3, Eval(("t3;")));
            Assert.AreEqual(4, Eval(("t4;")));
            Assert.AreEqual(5, Eval(("t5;")));
            Assert.AreEqual(6, Eval(("t6;")));
            Assert.AreEqual(7, Eval(("t7;")));
            Assert.AreEqual(8, Eval(("t8;")));
            Assert.AreEqual(9, Eval(("t9;")));
            Assert.AreEqual(10, Eval(("t10;")));
        }

        [TestMethod]
        public void Unpack_Two_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0};");

            Eval("{obj.t1, obj.t2} = {1, 2};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
        }

        [TestMethod]
        public void Unpack_Three_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0};");

            Eval("{obj.t1, obj.t2, obj.t3} = {1, 2, 3};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
        }

        [TestMethod]
        public void Unpack_Four_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4} = {1, 2, 3, 4};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
        }

        [TestMethod]
        public void Unpack_Five_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5} = {1, 2, 3, 4, 5};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
        }

        [TestMethod]
        public void Unpack_Six_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0, t6:0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5, obj.t6} = {1, 2, 3, 4, 5, 6};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
            Assert.AreEqual(6, Eval("obj.t6;"));
        }

        [TestMethod]
        public void Unpack_Seven_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0, t6:0, t7:0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5, obj.t6, obj.t7} = {1, 2, 3, 4, 5, 6, 7};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
            Assert.AreEqual(6, Eval("obj.t6;"));
            Assert.AreEqual(7, Eval("obj.t7;"));
        }

        [TestMethod]
        public void Unpack_Eight_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0, t6:0, t7:0, t8:0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5, obj.t6, obj.t7, obj.t8} = {1, 2, 3, 4, 5, 6, 7, 8};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
            Assert.AreEqual(6, Eval("obj.t6;"));
            Assert.AreEqual(7, Eval("obj.t7;"));
            Assert.AreEqual(8, Eval("obj.t8;"));
        }

        [TestMethod]
        public void Unpack_Nine_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0, t6:0, t7:0, t8:0, t9: 0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5, obj.t6, obj.t7, obj.t8, obj.t9} = {1, 2, 3, 4, 5, 6, 7, 8, 9};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
            Assert.AreEqual(6, Eval("obj.t6;"));
            Assert.AreEqual(7, Eval("obj.t7;"));
            Assert.AreEqual(8, Eval("obj.t8;"));
            Assert.AreEqual(9, Eval("obj.t9;"));
        }

        [TestMethod]
        public void Unpack_Ten_Object_Member()
        {
            Eval("var obj = {t1:0, t2:0, t3:0, t4:0, t5:0, t6:0, t7:0, t8:0, t9: 0, t10: 0};");

            Eval("{obj.t1, obj.t2, obj.t3, obj.t4, obj.t5, obj.t6, obj.t7, obj.t8, obj.t9, obj.t10} = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};");

            Assert.AreEqual(1, Eval("obj.t1;"));
            Assert.AreEqual(2, Eval("obj.t2;"));
            Assert.AreEqual(3, Eval("obj.t3;"));
            Assert.AreEqual(4, Eval("obj.t4;"));
            Assert.AreEqual(5, Eval("obj.t5;"));
            Assert.AreEqual(6, Eval("obj.t6;"));
            Assert.AreEqual(7, Eval("obj.t7;"));
            Assert.AreEqual(8, Eval("obj.t8;"));
            Assert.AreEqual(9, Eval("obj.t9;"));
            Assert.AreEqual(10, Eval("obj.t10;"));
        }

        [TestMethod]
        public void Unpack_Declaration_Non_Tuple()
        {
            Eval("var {x, y} = 10;");

            Assert.AreEqual(10, Eval("x;"));
            Assert.IsNull(Eval("y;"));
        }

        [TestMethod]
        public void Unpack_Existing_Non_Tuple()
        {
            Eval("var x;");
            Eval("var y;");

            Eval("{x, y} = 10;");

            Assert.AreEqual(10, Eval("x;"));
            Assert.IsNull(Eval("y;"));
        }

        [TestMethod]
        public void Unpack_Object_Member_Non_Tuple()
        {
            Eval("var obj = {x: 0, y: 0};");

            Eval("{obj.x, obj.y} = 10;");

            Assert.AreEqual(10, Eval("obj.x;"));
            Assert.IsNull(Eval("obj.y;"));
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Unpack_Declaration_Non_Identifier()
        {
            Eval("var {1,2} = {3, 4};");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Unpack_Assign_Non_Identifier()
        {
            Eval("{1,2} = {3, 4};");
        }

        [TestMethod]
        public void Unpack_Two_As_Declaration()
        {
            Eval("{1, 2} as {t1, t2};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
        }

        [TestMethod]
        public void Unpack_Three_As_Declaration()
        {
            Eval("{1, 2, 3} as {t1, t2, t3};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
        }

        [TestMethod]
        public void Unpack_Four_As_Declaration()
        {
            Eval("{1, 2, 3, 4} as {t1, t2, t3, t4};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
        }

        [TestMethod]
        public void Unpack_Five_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5} as {t1, t2, t3, t4, t5};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
        }

        [TestMethod]
        public void Unpack_Six_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5, 6} as {t1, t2, t3, t4, t5, t6};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
        }

        [TestMethod]
        public void Unpack_Seven_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5, 6, 7} as {t1, t2, t3, t4, t5, t6, t7};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
        }

        [TestMethod]
        public void Unpack_Eight_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5, 6, 7, 8} as {t1, t2, t3, t4, t5, t6, t7, t8};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
        }

        [TestMethod]
        public void Unpack_Nine_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5, 6, 7, 8, 9} as {t1, t2, t3, t4, t5, t6, t7, t8, t9};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
            Assert.AreEqual(9, Eval("t9;"));
        }

        [TestMethod]
        public void Unpack_Ten_As_Declaration()
        {
            Eval("{1, 2, 3, 4, 5, 6, 7, 8, 9, 10} as {t1, t2, t3, t4, t5, t6, t7, t8, t9, t10};");

            Assert.AreEqual(1, Eval("t1;"));
            Assert.AreEqual(2, Eval("t2;"));
            Assert.AreEqual(3, Eval("t3;"));
            Assert.AreEqual(4, Eval("t4;"));
            Assert.AreEqual(5, Eval("t5;"));
            Assert.AreEqual(6, Eval("t6;"));
            Assert.AreEqual(7, Eval("t7;"));
            Assert.AreEqual(8, Eval("t8;"));
            Assert.AreEqual(9, Eval("t9;"));
            Assert.AreEqual(10, Eval("t10;"));
        }

        [TestMethod]
        public void Unpack_Out_Position_One()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {out, t2, t3, t4, t5, t6, t7, t8, t9, t10};
            ");

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Two()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, out, t3, t4, t5, t6, t7, t8, t9, t10};
            ");

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Three()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, out, t4, t5, t6, t7, t8, t9, t10};
            ");

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Four()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, out, t5, t6, t7, t8, t9, t10};
            ");

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Five()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, out, t6, t7, t8, t9, t10};
            ");

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Six()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, t5, out, t7, t8, t9, t10};
            ");

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Seven()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, t5, t6, out, t8, t9, t10};
            ");

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Eight()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, t5, t6, t7, out, t9, t10};
            ");

            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Nine()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, t5, t6, t7, t8, out, t10};
            ");

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void Unpack_Out_Position_Ten()
        {
            var result = Eval(@"
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 10} 
            as 
            {t1, t2, t3, t4, t5, t6, t7, t8, t9, out};
            ");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Upack_Mutiple_Out_Params()
        {
            Eval("{1, 2} as {out, out};");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Tuple_Out_With_No_As_Context()
        {
            Eval("{out, 1};");
        }
    }
}