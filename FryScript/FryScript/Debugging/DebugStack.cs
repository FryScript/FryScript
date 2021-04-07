using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FryScript.Debugging
{
    public struct DebugStackFrame
    {
        public int Line;
        public Uri Uri;
        public DebugVariable[] Variables;
        public int Id;
    }

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
            return _stackFrames.SingleOrDefault(s => s.Id == id);
        }


        public DebugVariable[] GetVariables(int id)
        {
            return _stackFrames.SingleOrDefault(s => s.Id == id).Variables ?? new DebugVariable[0];
        }
    }

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

            if (!_debugHook.IsDebugging)
                return;

            if (!Active)
                return;

            _debugHook.BreakpointHit(Line, Uri, Id);
        }
    }

    public class DebugHook2
    {
        private readonly Dictionary<Uri, int[]> _deferredBreakpoints = new Dictionary<Uri, int[]>();

        private readonly HashSet<Uri> _loadedSources = new HashSet<Uri>();

        private readonly Dictionary<Uri, List<Breakpoint>> _breakpoints = new Dictionary<Uri, List<Breakpoint>>();

        public bool IsDebugging = false;

        public event Action<int, Uri, int> OnBreakpointHit;

        public event Action<Uri> OnSourceLoaded;

        public DebugStack Stack = new DebugStack();

        public Breakpoint NewBreakpoint(Uri uri, int line)
        {
            var breakpoint = new Breakpoint(line, uri, this);

            if (!_breakpoints.TryGetValue(uri, out var breakpoints))
            {
                _breakpoints[uri] = breakpoints = new List<Breakpoint>();
            }

            breakpoints.Add(breakpoint);

            return breakpoint;
        }

        public void Push()
        {
            Stack.Push();
        }

        public void Pop()
        {
            Stack.Pop();
        }

        public Breakpoint[] ActivateBreakpoints(Uri uri, int[] lines)
        {
            if (!_breakpoints.TryGetValue(uri, out var breakpoints))
            {
                _deferredBreakpoints[uri] = lines;
                return new Breakpoint[0];
            }

            breakpoints.ForEach(b => b.Active = false);

            var toReturn = new List<Breakpoint>();

            foreach (var line in lines)
            {
                breakpoints.Where(b => b.Line == line).ToList().ForEach(b =>
                {
                    b.Active = true;
                    toReturn.Add(b);
                });
            }

            return toReturn.ToArray();
        }

        public void BreakpointHit(int line, Uri uri, int id)
        {
            OnBreakpointHit?.Invoke(line, uri, id);
        }

        public void SourceLoaded(Uri uri)
        {
            if (!_loadedSources.Contains(uri))
                _loadedSources.Add(uri);

            if(_deferredBreakpoints.ContainsKey(uri))
            {
                var lines = _deferredBreakpoints[uri];
                ActivateBreakpoints(uri, lines);
                _deferredBreakpoints.Remove(uri);
            }

            OnSourceLoaded?.Invoke(uri);
        }

        public string[] GetLoadedSources()
        {
            return _loadedSources.Select(s => s.LocalPath).ToArray();
        }

        //public void Continue()
        //{
        //    foreach(var kvp in _breakpoints)
        //    {
        //        foreach(var breakpoint in kvp.Value)
        //        {
        //            breakpoint.
        //        }
        //    }
        //}
    }
}
