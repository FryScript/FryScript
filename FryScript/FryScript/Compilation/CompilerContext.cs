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
        private readonly ScriptEngine _scriptEngine;
        private readonly string _name;

        internal HashSet<string> Extends = new HashSet<string>();
        //internal Func<ScriptObject, object> ProtoCtor;
        //internal ScriptObjectReference ProtoReference;
        internal List<ImportInfo> ImportInfos = new List<ImportInfo>(); 
        
        public string Name { get { return _name; } }

        public Uri Uri { get; }

        public ScriptEngine ScriptEngine { get { return _scriptEngine; } }

        public IScriptRuntime ScriptRuntime { get; }

        public Type ScriptType { get; set; }

        public IScriptParser ExpressionParser { get; set; }

        public bool HasDebugHook => _scriptEngine?.DebugHook != null;

        public DebugHook DebugHook => _scriptEngine?.DebugHook;

        public bool IsEvalMode { get; }

        public CompilerContext(ScriptEngine scriptEngine, string name)
            : base(FryScriptLanguageData.LanguageData)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _scriptEngine = scriptEngine ?? throw new ArgumentNullException(nameof(scriptEngine));
            _name = name;

            DefaultNodeType = typeof(DefaultNode);
            DefaultLiteralNodeType = typeof(LiteralNode);
            DefaultIdentifierNodeType = typeof(IdentifierNode);
        }

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
        }

        //public void Extend(ScriptObject scriptObject)
        //{
        //    scriptObject = scriptObject ?? throw new ArgumentNullException(nameof(scriptObject));

        //    Extends.Add(scriptObject.ScriptType);

        //    foreach (var extend in scriptObject.Extends)
        //    {
        //        Extends.Add(extend);
        //    }
        //}
    }
}
