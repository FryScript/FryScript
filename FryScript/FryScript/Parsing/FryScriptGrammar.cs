using FryScript.Ast;
using Irony.Parsing;

namespace FryScript.Parsing
{
    [Language("FryScript", "v0.0.1", "Fry script")]
    public class FryScriptGrammar : Grammar
    {
        public FryScriptGrammar()
        {
            var literal = new NonTerminal(NodeNames.Literal);
            var booleanLiteral = new NonTerminal(NodeNames.BooleanLiteral, typeof (BooleanLiteralNode));
            var numberLiteral = new NumberLiteral(NodeNames.NumberLiteral, NumberOptions.AllowSign);
            var stringLiteral = new StringLiteral(NodeNames.StringLiteral, "\"", StringOptions.AllowsLineBreak, typeof(StringLiteralNode));
            var nullLiteral = new NonTerminal(NodeNames.NullLiteral, typeof(NullNode));
            var @object = new NonTerminal(NodeNames.ObjectLiteralExpression, typeof (ObjectLiteralExpressionNode));
            var objectMembers = new NonTerminal(NodeNames.ObjectMembers, typeof (ObjectLiteralMembersNode));
            var objectMember = new NonTerminal(NodeNames.ObjectMember, typeof (ObjectLiteralMemberNode));
            var arrayExpression = new NonTerminal(NodeNames.ArrayExpression, typeof (ArrayExpressionNode));
            var arrayItems = new NonTerminal(NodeNames.ArrayItems);
            var identifier = new IdentifierTerminal(NodeNames.Identifier);
            var @this = new NonTerminal(NodeNames.This, typeof (ThisNode));
            var @base = new NonTerminal(NodeNames.Base, typeof (BaseNode));
            //var proto = new NonTerminal(NodeNames.Proto, typeof(ProtoNode));
            var script = new NonTerminal(NodeNames.Script, typeof (ScriptNode));
            var scriptHeaders = new NonTerminal(NodeNames.ScriptHeaders, typeof (ScriptHeadersNode));
            var scriptHeader = new NonTerminal(NodeNames.ScriptHeader);
            var scriptExtend = new NonTerminal(NodeNames.ScriptExtend, typeof (ScriptExtendNode));
            var scriptImport = new NonTerminal(NodeNames.ScriptImport, typeof (ScriptImportNode));
            var scriptImportFrom = new NonTerminal(NodeNames.ScriptImportFrom, typeof(ScriptImportFromNode));
            var importAlias = new NonTerminal(NodeNames.ImportAlias, typeof(ImportAliasNode));
            var importAliasList = new NonTerminal(NodeNames.ImportAliasList, typeof(ImportAliasListNode));
            //var scriptProto = new NonTerminal(NodeNames.ScriptProto, typeof(ScriptProtoNode));
            var statements = new NonTerminal(NodeNames.Statements, typeof (StatementsNode));
            var statement = new NonTerminal(NodeNames.Statement, typeof(StatementNode));
            var semiStatement = new NonTerminal(NodeNames.SemiStatement);
            var returnStatement = new NonTerminal(NodeNames.ReturnStatement, typeof(ReturnStatementNode));
            var emptyStatement = new NonTerminal(NodeNames.EmptyStatement, typeof (EmptyStatement));
            var functionExtendStatement = new NonTerminal(NodeNames.FunctionExtendStatement, typeof(FunctionExtendStatementNode));
            var variableDeclaration = new NonTerminal(NodeNames.VariableDeclaration, typeof (VariableDeclarationNode));
            var expression = new NonTerminal(NodeNames.Expression, typeof(ExpressionNode));
            var assignExpression = new NonTerminal(NodeNames.AssignExpression, typeof (AssignExpressionNode));
            var assignOperator = new NonTerminal("BinaryOperator");
            var index = new NonTerminal(NodeNames.Index, typeof (IndexNode));
            var factor = new NonTerminal("Factor");
            var identifierExpression = new NonTerminal(NodeNames.IdentifierExpression, typeof (IdentifierExpressionNode));
            var function = new NonTerminal(NodeNames.FunctionExpression, typeof(FunctionExpressionNode));
            var functionParameters = new NonTerminal(NodeNames.FunctionParameters, typeof(FunctionParametersNode));
            var @params = new NonTerminal(NodeNames.Params, typeof (ParamsNode));
            var parameterNames = new NonTerminal(NodeNames.ParameterNames, typeof(ParameterNamesNode));
            var functionBlock = new NonTerminal(NodeNames.FunctionBlock, typeof(FunctionBlockNode));
            var fibreExpression = new NonTerminal(NodeNames.FibreExpression, typeof(FibreExpressionNode));
            var yieldStatement = new NonTerminal(NodeNames.YieldStatement, typeof(YieldStatementNode));
            var booleanExpression = new NonTerminal(NodeNames.BooleanExpression, typeof(BooleanExpressionNode));
            var relationalExpression = new NonTerminal(NodeNames.RelationalExpression, typeof(RelationalExpressionNode));
            var addExpression = new NonTerminal(NodeNames.AddExpression, typeof(AddExpressionNode));
            var addOperator = new NonTerminal(NodeNames.AddOperator, typeof(TokenNode));
            var multiplyExpression = new NonTerminal(NodeNames.MultiplyExpression, typeof (MultiplyExpressionNode));
            var multiplyOperator = new NonTerminal(NodeNames.MultiplyOperator, typeof(TokenNode));

            var booleanOperator = new NonTerminal(NodeNames.BooleanOperator, typeof(TokenNode));
            var relationalOperator = new NonTerminal(NodeNames.RelationalOperator, typeof(TokenNode));

            var invokeMemberExpression = new NonTerminal(NodeNames.InvokeMemberExpression, typeof(InvokeMemberExpressionNode));
            var invokeExpression = new NonTerminal(NodeNames.InvokeExpression, typeof(InvokeExpressionNode));
            var invokeArgs = new NonTerminal(NodeNames.InvokeArgs);
            var parenExpression = new NonTerminal(NodeNames.ParenExpression, typeof(ParenExpressionNode));

            var blockStatement = new NonTerminal(NodeNames.BlockStatement, typeof(BlockStatementNode));
            var ifStatement = new NonTerminal(NodeNames.IfStatement, typeof(IfStatementNode));
            var forStatement = new NonTerminal(NodeNames.ForStatement, typeof(ForStatementNode));
            var whileStatement = new NonTerminal(NodeNames.WhileStatement, typeof(WhileStatementNode));
            var forEachStatement = new NonTerminal(NodeNames.ForEachStatement, typeof (ForEachStatementNode));
            var tryCatchFinallyStatement = new NonTerminal(NodeNames.TryCatchFinallyStatement, typeof(TryCatchFinallyStatementNode));
            var tryStatement = new NonTerminal(NodeNames.TryStatement, typeof(TryStatementNode));
            var catchStatement = new NonTerminal(NodeNames.CatchStatement, typeof(CatchStatementNode));
            var finallyStatement = new NonTerminal(NodeNames.FinallyStatement, typeof(FinallyStatementNode));
            var throwExpression = new NonTerminal(NodeNames.ThrowExpression, typeof(ThrowExpressionNode));

            var ternaryExpression = new NonTerminal(NodeNames.TernaryExpression, typeof(TernaryExpressionNode));
            var conditionalAssignExpression = new NonTerminal(NodeNames.ConditionalAssignExpression, typeof(ConditionalAssignExpressionNode));
            var conditionalAssignOperator = new NonTerminal(NodeNames.ConditionalAssignOperator, typeof(TokenNode));
            var binaryExpression = new NonTerminal(NodeNames.BinaryExpression, typeof(BinaryExpressionNode));
            var binaryOperator = new NonTerminal(NodeNames.BinaryOperator, typeof(TokenNode));
            var unaryExpression = new NonTerminal(NodeNames.UnaryExpression);
            var unarySuffixExpression = new NonTerminal(NodeNames.UnarySuffixExpression, typeof(UnarySuffixExpressionNode));
            var unaryPrefixExpression = new NonTerminal(NodeNames.UnaryPrefixExpression, typeof(UnaryPrefixExpressionNode));
            var unaryOperator = new NonTerminal(NodeNames.UnaryOperator, typeof(TokenNode));
            var notExpression = new NonTerminal(NodeNames.NotExpression, typeof(NotExpressionNode));
            var notOperator = new NonTerminal(NodeNames.NotOperator, typeof(TokenNode));
            var awaitExpression = new NonTerminal(NodeNames.AwaitExpression, typeof(AwaitExpressionNode));
            var assignTupleExpression = new NonTerminal(NodeNames.AssignTupleExpression, typeof(AssignTupleExpressionNode));
            var tupleNames = new NonTerminal(NodeNames.TupleNames, typeof(TupleNamesNode));
            var tupleName = new NonTerminal(NodeNames.TupleName, typeof(DefaultNode));
            var tupleDeclaration = new NonTerminal(NodeNames.TupleDeclaration, typeof(TupleDeclarationNode));
            var tupleOut = new NonTerminal(NodeNames.TupleOut, typeof(TupleOut));

            var breakStatement = new NonTerminal(NodeNames.BreakStatement, typeof (BreakStatementNode));
            var continueStatement = new NonTerminal(NodeNames.ContinueStatement, typeof (ContinueStatementNode));

            var newExpression = new NonTerminal(NodeNames.NewExpression, typeof(NewExpressionNode));
            var isExpression = new NonTerminal(NodeNames.IsExpression, typeof(IsExpressionNode));
            var extendsExpression = new NonTerminal(NodeNames.ExtendsExpression, typeof(ExtendsExpressionNode));
            var hasExpression = new NonTerminal(NodeNames.HasExpression, typeof(HasExpressionNode));
            var asExpression = new NonTerminal(NodeNames.AsExpressionNode, typeof(AsExpressionNode));

            script.Rule = scriptHeaders + statements;

            scriptHeaders.Rule = MakeStarRule(scriptHeaders, scriptHeader);

            scriptHeader.Rule = scriptExtend
                                | scriptImport
                                | scriptImportFrom;
                                //| scriptProto;
            importAlias.Rule = identifier + Keywords.As + identifier
                | identifier;

            importAliasList.Rule = MakePlusRule(importAliasList, ToTerm(","), importAlias);

            scriptExtend.Rule = ToTerm(Keywords.ScriptExtend) + stringLiteral + ";";
            scriptImport.Rule = ToTerm(Keywords.ScriptImport) + stringLiteral + Keywords.As + identifier + ";";
            scriptImportFrom.Rule = ToTerm(Keywords.ScriptImport) + importAliasList + Keywords.From + stringLiteral + ";";
            //scriptProto.Rule = ToTerm(Keywords.ScriptProto) + blockStatement;

            statements.Rule = MakeStarRule(statements, statement);

            statement.Rule = semiStatement + ToTerm(";")
                             | blockStatement
                             | forStatement
                             | ifStatement
                             | whileStatement
                             | forEachStatement
                             | tryCatchFinallyStatement;

            blockStatement.Rule = ToTerm("{") + PreferShiftHere() + "}" + ReduceHere()
                                  | ToTerm("{") + statements + "}";

            ifStatement.Rule = ToTerm(Keywords.If) + parenExpression + statement
                | Keywords.If + parenExpression + statement +  PreferShiftHere()  + Keywords.Else + statement;

            forStatement.Rule = ToTerm(Keywords.For) + "(" + semiStatement + ";" + semiStatement + ";" + semiStatement + ")"  + statement;

            whileStatement.Rule = ToTerm(Keywords.While) + parenExpression + statement;

            forEachStatement.Rule = ToTerm(Keywords.ForEach) + "(" + Keywords.Var + identifier + Keywords.In + expression + ")" + statement;

            tryCatchFinallyStatement.Rule = tryStatement + catchStatement
                | tryStatement + finallyStatement
                | tryStatement + catchStatement + finallyStatement;

            tryStatement.Rule = ToTerm(Keywords.Try) + blockStatement;
            catchStatement.Rule = ToTerm(Keywords.Catch) + identifier + blockStatement;
            finallyStatement.Rule = ToTerm(Keywords.Finally) + blockStatement;

            semiStatement.Rule = expression
                                 | variableDeclaration
                                 | tupleDeclaration
                                 | returnStatement
                                 | breakStatement
                                 | continueStatement
                                 | functionExtendStatement
                                 | emptyStatement
                                 | yieldStatement;

            returnStatement.Rule = ToTerm(Keywords.Return) + expression
                | Keywords.Return;

            breakStatement.Rule = ToTerm(Keywords.Break);

            continueStatement.Rule = ToTerm(Keywords.Continue);

            functionExtendStatement.Rule = identifierExpression + Keywords.FunctionExtend + function
                | identifierExpression + Keywords.FunctionExtend + fibreExpression;

            emptyStatement.Rule = Empty;

            throwExpression.Rule = ToTerm(Keywords.Throw)
                | ToTerm(Keywords.Throw) + expression;

            yieldStatement.Rule = ToTerm(Keywords.Yield) + expression
                | ToTerm(Keywords.Yield)
                | ToTerm(Keywords.Yield) + ToTerm(Keywords.Return)
                | ToTerm(Keywords.Yield) + ToTerm(Keywords.Return) + expression;

            variableDeclaration.Rule = ToTerm(Keywords.Var) + identifier + assignOperator + expression
                                       | Keywords.Var + identifier;

            tupleDeclaration.Rule = ToTerm(Keywords.Var) + "{" + tupleNames + "}" + assignOperator + expression
                | ToTerm(Keywords.Var) + "{" + tupleNames + "}";

            assignTupleExpression.Rule = "{" + tupleNames + "}"
                | "{" + tupleNames + "}" + assignOperator + expression;

            tupleNames.Rule = tupleNames + "," + tupleName
                | tupleName + PreferShiftHere() + "," + tupleName;

            tupleName.Rule = expression
                | tupleOut;

            tupleOut.Rule = ToTerm(Keywords.Out);

            expression.Rule = assignExpression
                | conditionalAssignExpression
                | assignTupleExpression
                | booleanExpression
                | ternaryExpression;

            assignExpression.Rule = identifierExpression + assignOperator + expression;
            assignOperator.Rule = ToTerm(Operators.Assign);

            ternaryExpression.Rule = expression + PreferShiftHere() + "?" + expression + ":" + expression
               | expression + PreferShiftHere() + "?" + ":" + expression;

            booleanExpression.Rule = booleanExpression + booleanOperator + relationalExpression
                                     | relationalExpression;
            booleanOperator.Rule = PreferShiftHere() + ToTerm(Operators.And)
                                   | PreferShiftHere() + Operators.Or;

            relationalExpression.Rule = relationalExpression + relationalOperator + isExpression
                                     | asExpression;

            relationalOperator.Rule = PreferShiftHere() + ToTerm(Operators.Equal)
                                      | PreferShiftHere() + Operators.NotEqual
                                      | PreferShiftHere() + Operators.GreaterThan
                                      | PreferShiftHere() + Operators.LessThan
                                      | PreferShiftHere() + Operators.GreaterThanOrEqual
                                      | PreferShiftHere() + Operators.LessThanOrEqual;

            asExpression.Rule = isExpression + PreferShiftHere() + ToTerm(Keywords.As)  + identifier
                | isExpression + ToTerm(Keywords.As) + "{" + tupleNames + "}"
                | "{" + tupleNames + "}" + PreferShiftHere() + ToTerm(Keywords.As) + identifier
                | "{" + tupleNames + "}" + PreferShiftHere() + ToTerm(Keywords.As) + "{" + tupleNames + "}"
                | isExpression;

            isExpression.Rule = factor + ToTerm(Keywords.Is) + factor
                                | extendsExpression;

            extendsExpression.Rule = factor + ToTerm(Keywords.Extends) + factor
                                     | hasExpression;

            hasExpression.Rule = factor + ToTerm(Keywords.Has) + identifier
                                 | binaryExpression;

            binaryExpression.Rule = identifierExpression + binaryOperator + addExpression
                | addExpression;
            binaryOperator.Rule = PreferShiftHere() + ToTerm(Operators.IncrementAssign)
                                  | PreferShiftHere() + Operators.DecrementAssign
                                  | PreferShiftHere() + Operators.MultiplyAssign
                                  | PreferShiftHere() + Operators.DivideAssign
                                  | PreferShiftHere() + Operators.ModuloAssign;

            conditionalAssignExpression.Rule = identifierExpression + conditionalAssignOperator + expression;
            conditionalAssignOperator.Rule = ToTerm(Operators.ConditionalAssign);

            addExpression.Rule = addExpression + addOperator + multiplyExpression
                                 | multiplyExpression;
            addOperator.Rule = PreferShiftHere() + ToTerm(Operators.Add) 
                | PreferShiftHere() + Operators.Subtract;

            multiplyExpression.Rule = multiplyExpression + multiplyOperator + unaryExpression
                | unaryExpression;
            multiplyOperator.Rule = PreferShiftHere() + ToTerm(Operators.Multiply) 
                | PreferShiftHere() + Operators.Divide 
                | PreferShiftHere() + Operators.Modulo;

            unaryExpression.Rule = unarySuffixExpression
                        | unaryPrefixExpression
                        | notExpression
                        | newExpression
                        | arrayExpression
                        | @object
                        | function
                        | fibreExpression
                        | throwExpression
                        | awaitExpression
                        | factor; 

            unaryPrefixExpression.Rule = unaryOperator + identifierExpression;
            unarySuffixExpression.Rule = identifierExpression + unaryOperator;
            unaryOperator.Rule = ToTerm(Operators.Increment) | Operators.Decrement;

            notExpression.Rule = notOperator + factor;
            notOperator.Rule = ToTerm(Operators.Not);

            newExpression.Rule = ToTerm(Keywords.New) + invokeExpression;

            function.Rule = functionParameters + ToTerm("=>") + functionBlock;

            functionParameters.Rule = ToTerm("(") + ")"
                                      | "(" + parameterNames + ")"
                                      | identifier
                                      | ToTerm("(") + @params + PreferShiftHere() + ")"
                                      | @params;

            parameterNames.Rule = parameterNames + "," + identifier
                                  | identifier + ReduceHere();

            functionBlock.Rule = blockStatement
                | expression;

            fibreExpression.Rule = ToTerm(Keywords.Fibre) + function;

            awaitExpression.Rule = ToTerm(Keywords.Await) + factor + ReduceHere();

            factor.Rule = invokeMemberExpression
                          | invokeExpression
                          | identifierExpression
                          | literal
                          | parenExpression
                          | @this
                          | @params
                          | @base;
                          //| @proto;

            parenExpression.Rule = ToTerm("(") + expression + PreferShiftHere() + ")" + PreferShiftHere();

            @object.Rule = ToTerm("{") + objectMembers + "}"
                | "{" + PreferShiftHere() + "}";

            objectMembers.Rule = MakePlusRule(objectMembers, ToTerm(","), objectMember);
            objectMember.Rule = identifier + ToTerm(":") + expression;

            arrayExpression.Rule = ToTerm("[") + arrayItems + "]";
            arrayItems.Rule = MakeStarRule(arrayItems, ToTerm(","), expression);
            invokeMemberExpression.Rule = factor + "." + identifier + PreferShiftHere() + "(" + invokeArgs + ")";

            invokeExpression.Rule = factor + "(" + invokeArgs + ")";

            invokeArgs.Rule = MakeStarRule(invokeArgs, ToTerm(","), expression);

            identifierExpression.Rule = identifier
                                        | factor + "." + identifier
                                        | factor + index;

            index.Rule = "[" + expression + "]";

            literal.Rule = numberLiteral
                           | stringLiteral
                           | booleanLiteral
                           | nullLiteral;

            booleanLiteral.Rule = ToTerm(Keywords.True)
                                  | ToTerm(Keywords.False);

            nullLiteral.Rule = ToTerm(Keywords.Null);

            @this.Rule = ToTerm(Keywords.This);
            @params.Rule = ToTerm(Keywords.Params);
            @base.Rule = ToTerm(Keywords.Base);
            //proto.Rule = ToTerm(Keywords.Proto);

            Root = script;

            NonGrammarTerminals.Add(new CommentTerminal("Comment", "//", "\r\n", "\n"));
            NonGrammarTerminals.Add(new CommentTerminal("Block comment", "/*", "*/"));

            MarkReservedWords(Keywords.This, Keywords.ScriptExtend, Keywords.ScriptImport, Keywords.As, Keywords.Var, Keywords.Null, Keywords.Params, Keywords.Return, Keywords.If, Keywords.Else, Keywords.For, Keywords.New, Keywords.While, Keywords.ForEach, Keywords.In, Keywords.FunctionExtend, Keywords.Base, /*Keywords.Proto*/ Keywords.Has, Keywords.Try, Keywords.Catch, Keywords.Finally, Keywords.Throw, Keywords.Fibre, Keywords.Yield, /*Keywords.Begin,*/ Keywords.Yield, Keywords.Await, Keywords.From, Keywords.Out);
            MarkTransient(scriptHeader, semiStatement, literal, factor, unaryExpression, tupleName);
            MarkPunctuation(";", ":", ".", ",", "[", "]", "{", "}", "(", ")", "=>", "@", "?");

            SnippetRoots.Add(expression);
        }
    }
}
