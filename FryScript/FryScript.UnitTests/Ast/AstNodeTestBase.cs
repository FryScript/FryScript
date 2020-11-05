using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Debugging;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    public class AstNodeTestBase<T> where T : AstNode
    {
        public T Node { get; private set; }

        public Scope Scope { get; set; } = new Scope();

        [TestInitialize]
        public void TestInitialize()
        {
            Node = Substitute.ForPartsOf<T>();

            OnTestInitialize();
        }

        public virtual void OnTestInitialize()
        {

        }

        public void TestSingleChildNode()
        {
            Node.SetChildren(Node<AstNode>.Empty);

            var expr = Expression.Empty();

            Node.Configure().GetChildExpression(Scope).Returns(expr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expr, result);
        }

        public virtual void GetExpression_Null_Scope()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Node.GetExpression(null));
        }

        public virtual void GetExpression_Single_Child_Gets_Child_Expression()
        {
            TestSingleChildNode();
        }
    }
}