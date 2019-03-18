using System;

namespace FryScript
{
    public interface IScriptObjectBuilder
    {
        Uri Uri { get; }

        IScriptObject Build();
    }
}