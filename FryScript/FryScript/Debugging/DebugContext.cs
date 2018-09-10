namespace FryScript.Debugging
{
    public struct DebugContext
    {
        public DebugEvent DebugEvent;

        public readonly DebugVariable[] Variables;

        public readonly string Name;

        public readonly int Line, Column, Length;

        public DebugContext(DebugEvent debugEvent, string name, int line, int column, int length, DebugVariable[] variables)
        {
            DebugEvent = debugEvent;
            Name = name;
            Line = line;
            Column = column;
            Length = length;
            Variables = variables;
        }

        public static DebugContext Create(DebugEvent debugEvent, string name, int line, int column, int length, DebugVariable[] variables = null)
        {
            return new DebugContext(debugEvent, name, line, column, length, variables);
        }
    }
}
