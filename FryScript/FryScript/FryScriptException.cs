using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript
{
    using System.Collections;
    using Ast;
    using Helpers;
    using CallSites;
    public class FryScriptException : Exception
    {
        public string Name { get; set; }

        public int? Line { get; set; }

        public int? Column { get; set; }

        public object ScriptData { get; set; }

        public FryScriptException(string message)
            : base(message)
        {
        }

        public FryScriptException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FryScriptException(string message, Exception innerException, string name, int line, int column)
            : base(message, innerException)
        {
            Name = name;
            Line = line;
            Column = column;
        }

        public static object FormatException(Exception ex, string name, int line, int column)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            var fryEx = ex as FryScriptException;

            if (fryEx == null)
                fryEx = ExceptionHelper.NativeInteropException(ex, name, line, column);

            if (!fryEx.Line.HasValue)
                fryEx.Line = line;

            if (!fryEx.Column.HasValue)
                fryEx.Column = column;

            return fryEx;
        }

        public static object Throw(object data, Exception ex, string name, int line, int column)
        {
            var stringData = data as string;

            if (stringData != null)
                return new FryScriptException(stringData, ex, name, line, column);

            string message = data != null && CallSiteCache.Current.HasMember("message", data)
                ? (data as dynamic).message
                : string.Empty;

            throw new FryScriptException(message, ex, name, line, column)
            {
                ScriptData = data
            };
        }

        public static object GetCatchObject(Exception ex)
        {
            var fryEx = ex as FryScriptException;

            if(fryEx == null || fryEx.ScriptData == null)
                return ex.Message ?? string.Empty;

            return fryEx.ScriptData;
        }
    }
}
