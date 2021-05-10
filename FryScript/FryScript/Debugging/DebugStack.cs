using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FryScript.Debugging
{

    public class DebugStack
    {
        private int _idCounter;

        private readonly ConcurrentStack<DebugStackFrame> _stackFrames = new ConcurrentStack<DebugStackFrame>(new[] { new DebugStackFrame() });

        public void Push()
        {
            _stackFrames.Push(new DebugStackFrame());
        }

        public void Pop()
        {
            _stackFrames.TryPop(out _);
        }

        public void SetStackFrame(int line, Uri uri, DebugVariable[] variables)
        {
            _stackFrames.TryPop(out _);
            _stackFrames.Push(new DebugStackFrame
            {
                Line = line,
                Uri = uri,
                Variables = variables,
                Id = ++_idCounter
            });
        }

        public DebugStackFrame[] GetStackFrames()
        {
            return _stackFrames.ToArray();
        }

        public DebugStackFrame GetStackFrame(int id)
        {
            var stackFrame = _stackFrames.SingleOrDefault(s => s.Id == id);

            return stackFrame;
        }


        public DebugVariable[] GetVariables(int id)
        {
            return _stackFrames.SingleOrDefault(s => s.Id == id).Variables ?? new DebugVariable[0];
        }
    }
}
