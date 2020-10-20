using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ExtendsExpressionNodeTests : AstNodeTestBase<ExtendsExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Single_Child_Gets_Child_Expression()
        {
            TestSingleChildNode();
        }

        [TestMethod]
        public void GetExpression_Binds_Dynamic_Operation()
        {
            var instance = Node<AstNode>.Empty;
            var value = Node<AstNode>.Empty;

            var instanceExpr = Expression.Constant(new object());
            var valueExpr = Expression.Constant(new object());

            instance.GetExpression(instanceExpr, Scope);
            value.GetExpression(valueExpr, Scope);

            Node.SetChildren(instance, null, value);

            var result = Node.GetExpression(Scope) as DynamicExpression;
            
            Assert.IsInstanceOfType(result.Binder, typeof(ScriptExtendsOperationBinder));
            Assert.AreEqual(instanceExpr, result.Arguments[0]);
            Assert.AreEqual(valueExpr, result.Arguments[1]);
        }
    }
}
