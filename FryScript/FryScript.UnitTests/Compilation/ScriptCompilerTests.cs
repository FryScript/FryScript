using System;
using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;

namespace FryScript.UnitTests.Compilation
{
    [TestClass]
    public class ScriptCompilerTests
    {
        private ScriptCompiler _compiler;
        private IScriptParser _parser;
        private IScriptParser _expressionParser;

        private CompilerContext _context;
        private IRootNode _rootNode;

        [TestInitialize]
        public void TestInitialize()
        {
            _parser = Substitute.For<IScriptParser>();
            _expressionParser = Substitute.For<IScriptParser>();

            _compiler = new ScriptCompiler(_parser, _expressionParser);

            _context = new CompilerContext(Substitute.For<IScriptRuntime>(), new Uri("test:///name"));

            _rootNode = Substitute.For<IRootNode>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Parser()
        {
            _compiler = new ScriptCompiler(null, _expressionParser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Expression_Parser()
        {
            _compiler = new ScriptCompiler(_parser, null);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Invalid_Source(string source)
        {
            _compiler.Compile(source, "name", _context);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Invalid_Name(string name)
        {
            _compiler.Compile("source", name, _context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Null_Context()
        {
            _compiler.Compile("source", "name", null);
        }

        [TestMethod]
        public void Compile_Success()
        {
            _parser.Parse("source", "name", _context).Returns(_rootNode);

            var expectedFunc = new Func<IScriptObject, object>(o => o);
            _rootNode.Compile(Arg.Any<Scope>()).Returns((CallInfo c) => expectedFunc);

            var result = _compiler.Compile("source", "name", _context);

            Assert.AreEqual(expectedFunc, result);
            Assert.AreEqual(_expressionParser, _context.ExpressionParser);
        }
    }
}
