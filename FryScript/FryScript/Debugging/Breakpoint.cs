using System;

namespace FryScript.Debugging
{
    public class Breakpoint
    {
        private static int _idCounter;

        private readonly DebugHook2 _debugHook;

        public readonly int Line;
        public Uri Uri;
        public bool Active;
        public readonly int Id;

        public Breakpoint(int line, Uri uri, DebugHook2 debugHook)
        {
            Line = line;
            Uri = uri;
            _debugHook = debugHook;
            Id = ++_idCounter;
        }

        public void Execute(DebugVariable[] variables)
        {
            _debugHook.Stack.SetStackFrame(Line, Uri, variables);

            var state = _debugHook.DebuggerState;

            if (state == DebuggerState.Disconnected)
                return;

            if (state == DebuggerState.StepOver && _debugHook.HasSteppedOver())
            {
                _debugHook.BreakpointHit(Line, Uri, Id);
            }
            else if(state == DebuggerState.StepIn && _debugHook.HasSteppedIn())
            {
                _debugHook.BreakpointHit(Line, Uri, Id);
            }
            else if(state == DebuggerState.StepOut && _debugHook.HasSteppedOut())
            {
                _debugHook.BreakpointHit(Line, Uri, Id);
            }
            else if (Active)
            {
                _debugHook.BreakpointHit(Line, Uri, Id);
            }
        }
    }
}
