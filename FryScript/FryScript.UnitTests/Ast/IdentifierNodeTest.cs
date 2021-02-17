using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class IdentifierNodeTest : AstNodeTestBase<IdentifierNode>
    {
        [TestMethod]
        public void GetExpression_Calls_GetIdentifier()
        {
            var expectedIdentifierExpr = Expression.Constant(new object());
            Node.Configure().GetIdentifier(Scope).Returns(expectedIdentifierExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedIdentifierExpr, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetIdentifier_Null_Scope()
        {
            Node.GetIdentifier(null);
        }

        [TestMethod]
        public void GetIdentifier_Scope_Has_Member()
        {
            var expectedIdentifierExpr = Scope.AddMember("member", Node);

            Node.Configure().ValueString.Returns("member");

            var result = Node.GetIdentifier(Scope);

            Assert.AreEqual(expectedIdentifierExpr, result);
        }

        [TestMethod]
        public void Get_Identifier_Scope_Does_Not_Have_Member_Uses_This()
        {
            var expectedIdentifierExpr = Scope.AddMember(Keywords.This, Node);

            Node.Configure().ValueString.Returns("member");

            var result = Node.GetIdentifier(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(1, result.Arguments.Count);
            Assert.AreEqual(expectedIdentifierExpr, result.Arguments[0]);

            var binder = result.Binder as ScriptGetMemberBinder;
            Assert.AreEqual("member", binder.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Scope()
        {
            Node.SetIdentifier(null, Expression.Constant(new object()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Value()
        {
            Node.SetIdentifier(Scope, null);
        }

        [TestMethod]
        public void SetIdentifier_Scope_Has_Member()
        {
            var valueExpr = Expression.Constant(new object());

            var expectedParamExpr = Scope.AddMember("member", Node);

            Node.Configure().ValueString.Returns("member");

            var result = Node.SetIdentifier(Scope, valueExpr) as BinaryExpression;

            Assert.AreEqual(ExpressionType.Assign, result.NodeType);
            Assert.AreEqual(expectedParamExpr, result.Left);
            Assert.AreEqual(valueExpr, result.Right);
        }

        [TestMethod]
        public void SetIdentifier_Scope_Does_Not_Have_Member_Uses_This()
        {
            var valueExpr = Expression.Constant(new object());

            var expectedParamExpr = Scope.AddMember(Keywords.This, Node);

            Node.Configure().ValueString.Returns("member");

            var result = Node.SetIdentifier(Scope, valueExpr) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedParamExpr, result.Arguments[0]);
            Assert.AreEqual(valueExpr, result.Arguments[1]);

            var binder = result.Binder as ScriptSetMemberBinder;

            Assert.AreEqual("member", binder.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateIdentifier_Null_Scope()
        {
            Node.CreateIdentifier(null);
        }

        [TestMethod]
        public void CreateIdentifier_Eval_Mode_Does_Not_Add_Member()
        {
            Node.StubCompilerContext(isEvalMode: true);

            Scope = Scope.New(Node);

            Node.CreateIdentifier(Scope);

            Assert.AreEqual(0, Scope.GetAllowedMembers(true).Count());
        }

        [TestMethod]
        public void CreateIdentifier_Adds_Member()
        {
            Node.StubCompilerContext();

            Node.Configure().ValueString.Returns("member");

            Node.CreateIdentifier(Scope);

            Assert.IsTrue(Scope.HasMember("member"));
        }
    }
}
