using System;

namespace FryScript
{
    public interface IScriptObjectBuilder
    {
        Uri Uri { get; }

        IScriptObjectBuilder Parent { get; }

        IScriptObject Build();

        IScriptObject Extend(IScriptObject obj);
    }
}