using System;

namespace FryScript.HostInterop
{
    public interface ITypeFactory
    {
        Type CreateScriptableType(Type type);
    }
}