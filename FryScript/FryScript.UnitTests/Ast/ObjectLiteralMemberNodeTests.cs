using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ObjectLiteralMemberNodeTests : AstNodeTestBase<ObjectLiteralMemberNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Node()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Sets_Literal_Member()
        {
            var nameNode = Node<AstNode>.WithValueString("member");

            var expectedValueExpr = Expression.Constant(new object());
            var valueNode = Node<AstNode>.Empty;
            valueNode.GetExpression(expectedValueExpr, Scope);

            var expectedParamExpr = Scope.AddMember("literal", Node);
            Scope.SetData(ScopeData.ObjectLiteralContext, expectedParamExpr);

            Node.SetChildren(nameNode, valueNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedParamExpr, result.Arguments[0]);
            Assert.AreEqual(expectedValueExpr, result.Arguments[1]);

            var binder = result.Binder as ScriptSetMemberBinder;

            Assert.AreEqual("member", binder.Name);
        }
    }
}
