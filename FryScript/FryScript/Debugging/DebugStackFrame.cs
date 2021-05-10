using System;

namespace FryScript.Debugging
{
    public struct DebugStackFrame
    {
        public int Line;
        public Uri Uri;
        public DebugVariable[] Variables;
        public int Id;
    }
}
