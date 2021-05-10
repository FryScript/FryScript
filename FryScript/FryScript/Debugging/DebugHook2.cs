using System;
using System.Collections.Generic;
using System.Linq;

namespace FryScript.Debugging
{
    public class DebugHook2
    {
        private readonly Dictionary<Uri, int[]> _deferredBreakpoints = new Dictionary<Uri, int[]>();

        private readonly HashSet<Uri> _loadedSources = new HashSet<Uri>();

        private readonly Dictionary<Uri, List<Breakpoint>> _breakpoints = new Dictionary<Uri, List<Breakpoint>>();

        private int _stackMarker = 0;

        public DebuggerState DebuggerState = DebuggerState.Disconnected;

        public event Action<int, Uri, int> OnBreakpointHit;

        public event Action<Uri> OnSourceLoaded;

        public readonly DebugStack Stack = new DebugStack();

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
            DebuggerState = DebuggerState.Stopped;
            OnBreakpointHit?.Invoke(line, uri, id);
        }

        public void SourceLoaded(Uri uri)
        {
            if (!_loadedSources.Contains(uri))
                _loadedSources.Add(uri);

            if (_deferredBreakpoints.ContainsKey(uri))
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

        public void BeginStepOver()
        {
            _stackMarker = Stack.GetStackFrames().Length;
            DebuggerState = DebuggerState.StepOver;
        }

        public bool HasSteppedOver()
        {
            return Stack.GetStackFrames().Length <= _stackMarker && DebuggerState == DebuggerState.StepOver;
        }

        public void BeginStepIn()
        {
            DebuggerState = DebuggerState.StepIn;
        }

        public bool HasSteppedIn()
        {
            return DebuggerState == DebuggerState.StepIn;
        }

        public void BeginStepOut()
        {
            _stackMarker = Stack.GetStackFrames().Length;
            DebuggerState = DebuggerState.StepOut;
        }

        public bool HasSteppedOut()
        {
            return Stack.GetStackFrames().Length < _stackMarker && DebuggerState == DebuggerState.StepOut;
        }
    }
}
