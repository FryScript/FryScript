using FryScript.Ast;
using FryScript.Debugging;
using FryScript.Parsing;
using FryScript.ScriptProviders;
using Irony.Ast;
using System;
using System.Collections.Generic;

namespace FryScript.Compilation
{
    public class CompilerContext : AstContext
    {
        private readonly string _name;

        internal HashSet<string> Extends = new HashSet<string>();
        //internal Func<ScriptObject, object> ProtoCtor;
        //internal ScriptObjectReference ProtoReference;
        internal List<ImportInfo> ImportInfos = new List<ImportInfo>();

        public string Name { get { return _name; } }

        public Uri Uri { get; }

        public IScriptRuntime ScriptRuntime { get; }

        public Type ScriptType { get; set; }

        public IScriptParser ExpressionParser { get; set; }

        public bool HasDebugHook => DebugHook != null;

        public DebugHook DebugHook => ScriptRuntime.DebugHook;

        public bool IsEvalMode { get; }

        public bool DetailedExceptions { get; }

        public Scope Scope { get; }

        public IRootNode RootNode { get; set; }

        public CompilerContext(IScriptRuntime scriptRuntime, Uri uri, bool evalMode = false)
            : base(FryScriptLanguageData.LanguageData)
        {
            ScriptRuntime = scriptRuntime ?? throw new ArgumentNullException(nameof(scriptRuntime));
            Uri = uri;
            IsEvalMode = evalMode;

            DefaultNodeType = typeof(DefaultNode);
            DefaultLiteralNodeType = typeof(LiteralNode);
            DefaultIdentifierNodeType = typeof(IdentifierNode);
            ScriptType = typeof(ScriptObject);
            Scope = new Scope();
        }
    }
}
