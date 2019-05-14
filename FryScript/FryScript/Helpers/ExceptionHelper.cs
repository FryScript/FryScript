using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FryScript.Helpers
{
    public static class ExceptionHelper
    {
        public static FryScriptException MemberUndefined(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            throw new FryScriptException(string.Format("Member \"{0}\" is undefined", name.ToDisplayString()));
        }

        public static FryScriptException InvalidIndexes()
        {
            throw new FryScriptException("Invalid number of indexes");
        }

        public static FryScriptException InvalidIndex(object value)
        {
            value = value ?? throw new ArgumentNullException(nameof(value));

            throw new FryScriptException(string.Format("Invalid index value {0}", value.ToDisplayString()));
        }

        public static FryScriptException InvalidConvert(Type from, Type to)
        {
            from = from ?? throw new ArgumentNullException(nameof(from));
            to = to ?? throw new ArgumentNullException(nameof(to));

            throw new FryScriptException(string.Format("Unable to convert type {0} to type {1}", from.Name.ToDisplayString(), to.Name.ToDisplayString()));
        }

        public static FryScriptException InvalidConvert(Type from, Type to, object value)
        {
            from = from ?? throw new ArgumentNullException(nameof(from));
            to = to ?? throw new ArgumentNullException(nameof(to));
            value = value ?? throw new ArgumentNullException(nameof(value));

            throw new FryScriptException(string.Format("Unable to convert type {0} with value {1} to type {2}", from.Name.ToDisplayString(), value, to.Name.ToDisplayString()));
        }

        public static FryScriptException NonBinaryOperation(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support binary operations", type.Name));
        }

        public static FryScriptException NonCreateInstance(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support creating instances", type.Name));
        }

        public static FryScriptException NonConvertible(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support convert operations", type.Name));
        }

        public static FryScriptException NonInvokable(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} is not an invokable type", type.Name.ToDisplayString()));
        }

        public static FryScriptException NonBeginable(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException($"Type {type.Name.ToDisplayString()} is not a beginable type");
        }

        public static FryScriptException NonResumable(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException($"Type {type.Name.ToDisplayString()} is not a resumable type");
        }

        public static FryScriptException NonAwaitable(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException($"Type {type.Name.ToDisplayString()} is not an awaitable type");
        }

        public static FryScriptException FibreContextCompleted()
        {
            throw new FryScriptException("Cannot resume a completed fibre context");
        }

        public static FryScriptException NonSetIndex(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support setting indexes", type.Name.ToDisplayString()));
        }

        public static FryScriptException NonSetMember(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support setting members", type.Name.ToDisplayString()));
        }

        public static FryScriptException NonUnaryOperation(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support binary operations", type.Name.ToDisplayString()));
        }

        public static FryScriptException NonDeleteIndex(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support deleting indexes", type.Name));
        }

        public static FryScriptException NonDeleteMember(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support deleting members", type.Name));
        }

        public static FryScriptException NonGetIndex(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support getting indexes", type.Name));
        }

        public static FryScriptException NonGetMember(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support getting members", type.Name));
        }

        public static FryScriptException NonInvokeMember(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Type {0} does not support invoking members", type.Name));
        }

        public static FryScriptException IndexOutOfBounds(int index)
        {
            throw new FryScriptException(string.Format("Index {0} is out of bounds", index));
        }

        public static FryScriptException InvalidCreateInstance(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            throw new FryScriptException(string.Format("Type {0} does not support creating new instance", name));
        }

        public static FryScriptException InvalidExtendTarget()
        {
            throw new FryScriptException("Left side of extend is not a function");
        }

        public static FryScriptException InvalidBinaryOperation(ExpressionType operation, Type leftType, Type rightType)
        {
            leftType = leftType ?? throw new ArgumentNullException(nameof(leftType));
            rightType = rightType ?? throw new ArgumentNullException(nameof(rightType));

            var operatorName = Enum.GetName(typeof(ExpressionType), operation);

            throw new FryScriptException(
                string.Format(
                    "Cannot perform binary operation \"{0}\" on types {1} and {2} no operator is defined",
                    operatorName,
                    leftType.FullName, rightType.FullName));
        }

        public static FryScriptException InvalidIsOperation(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Cannot perform is operation on type {0} no operator is defined", type.GetType().FullName));
        }

        public static FryScriptException InvalidExtendsOperation(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Cannot perform extends operation on type {0} no operator is defined", type.GetType().FullName));
        }

        public static FryScriptException InvalidHasOperation(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new FryScriptException(string.Format("Cannot perform has operation on type {0} no operator is defined", type.GetType().FullName));
        }

        public static FryScriptException NativeInteropException(Exception ex, string name, int line, int position)
        {
            ex = ex ?? throw new ArgumentNullException(nameof(ex));
            name = name ?? throw new ArgumentNullException(nameof(name));

            return new FryScriptException("Native interop exception see inner exception for details", ex, name, line, position);
        }

        public static CompilerException InvalidContext(string keyword, AstNode astNode)
        {
            keyword = keyword ?? throw new ArgumentNullException(nameof(keyword));
            astNode = astNode ?? throw new ArgumentNullException(nameof(astNode));

            throw CompilerException.FromAst(string.Format("{0} is invalid in the current context", keyword), astNode);
        }

        public static CompilerException CircularDependency(CircularDependencyException ex, AstNode astNode)
        {
            ex = ex ?? throw new ArgumentNullException(nameof(ex));
            astNode = astNode ?? throw new ArgumentNullException(nameof(astNode));

            var sb = new StringBuilder();

            foreach (var keyName in ex.KeyNames)
            {
                sb.Append(keyName.ToDisplayString()).Append(" -> ");
            }

            throw CompilerException.FromAst(string.Format("Circular dependency detected: {0} {1}", sb, ex.CurrentKey), astNode);
        }

        public static CompilerException CircularDependency(string curKey, IEnumerable<string> keyNames)
        {
            curKey = curKey ?? throw new ArgumentNullException(nameof(curKey));
            keyNames = keyNames ?? throw new ArgumentNullException(nameof(keyNames));

            var sb = new StringBuilder();

            foreach (var keyName in keyNames)
            {
                sb.Append(keyName.ToDisplayString()).Append(" -> ");
            }

            throw CompilerException.FromAst(string.Format("Circular dependency detected: {0} {1}", sb, curKey), null);
        }

        public static CompilerException MultipleOutVars(AstNode astNode)
        {
            throw CompilerException.FromAst("As variable expressions can only have one out variable", astNode);
        }

        public static CompilerException UnexpectedOut(AstNode astNode)
        {
            throw CompilerException.FromAst("Cannot use out here", astNode);
        }

        public static CompilerException ExtendUnavailable(AstNode astNode)
        {
            throw CompilerException.FromAst("@extend is unavailable", astNode);
        }
    }
}
