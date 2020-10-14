using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FryScript.UnitTests.Ast
{
    public class AstNodeTestBase<T> where T : AstNode, new()
    {
        public T Node { get; private set; }

        public Scope Scope { get; set; } = new Scope();

        public AstNode.AstNodeTransformer NodeTransformer { get; private set; }

        public AstNode.GetChildExpressionVisitor ChildExpressionVisitor { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Node = new T();
          
            NodeTransformer = Substitute.For<AstNode.AstNodeTransformer>();
            ChildExpressionVisitor = Substitute.For<AstNode.GetChildExpressionVisitor>();

            Node.NodeTransformer = NodeTransformer;
            Node.ChildExpressionVisitor = ChildExpressionVisitor;

            OnTestInitialize();
        }

        public virtual void OnTestInitialize()
        {

        }
    }
}