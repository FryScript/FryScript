using FryScript.Ast;
using FryScript.Debugging;
using FryScript.Parsing;
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
        internal Func<ScriptObject, object> ProtoCtor;
        internal ScriptObjectReference ProtoReference;
        internal List<ImportInfo> ImportInfos = new List<ImportInfo>(); 
        
        public string Name { get { return _name; } }

        public ScriptEngine ScriptEngine { get { return _scriptEngine; } }

        public bool HasDebugHook => _scriptEngine?.DebugHook != null;

        public DebugHook DebugHook => _scriptEngine?.DebugHook;

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

        public void Extend(ScriptObject scriptObject)
        {
            scriptObject = scriptObject ?? throw new ArgumentNullException(nameof(scriptObject));

            Extends.Add(scriptObject.ScriptType);

            foreach (var extend in scriptObject.Extends)
            {
                Extends.Add(extend);
            }
        }
    }
}
