using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class FibreBlockNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ChildNodes = ChildNodes
                .Concat(new[] { new YieldStatementNode { ChildNodes = new AstNode[] { null, null } } })
                .ToArray();



            //if (ChildNodes.Length == 0)
            //    ChildNodes = new[] { Transform<StatementNode>(Transform<YieldStatementNode>(null, null)) };

            //var childNode = ChildNodes.Last();

            //if (childNode.GetType() == typeof(ExpressionNode))
            //{
            //    ChildNodes[ChildNodes.Length - 1] = Transform<StatementNode>(Transform<YieldStatementNode>(null, null, childNode));
            //}
            //else
            //{
            //    TransformBlockStatement(childNode);
            //}

            return GetChildExpression(scope);
        }

        private void TransformBlockStatement(AstNode blockStatement)
        {
            if (blockStatement.ChildNodes.Length == 0)
            {
                blockStatement.ChildNodes = new[] { Transform<StatementNode>(Transform<YieldStatementNode>(null, null)) };
                return;
            }

            var lastStatementNode = blockStatement.ChildNodes.First().ChildNodes.Last();
            var innerStatementNode = lastStatementNode.ChildNodes.First();

            if (innerStatementNode.GetType() == typeof(ExpressionNode))
            {
                lastStatementNode.ChildNodes[lastStatementNode.ChildNodes.Length - 1] = Transform<YieldStatementNode>(null, null, innerStatementNode);
            }

            else if (innerStatementNode.GetType() == typeof(YieldStatementNode))
            {
                var yieldStatement = (YieldStatementNode)innerStatementNode;

                if (yieldStatement.IsYieldReturn() == true)
                    return;

                yieldStatement.ChildNodes = new AstNode[] { null }.Concat(yieldStatement.ChildNodes).ToArray();
            }

            else
            {
                var yieldNode = new YieldStatementNode
                {
                    ChildNodes = new AstNode[3]
                };

                innerStatementNode.ChildNodes = innerStatementNode.ChildNodes.Concat(new[] { yieldNode }).ToArray();
            }
        }
    }
}
