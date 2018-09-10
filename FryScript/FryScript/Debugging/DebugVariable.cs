namespace FryScript.Debugging
{
    public struct DebugVariable
    {
        public readonly string Name;
        public readonly object Value;

        public DebugVariable(string name, object value)

        {
            Name = name;
            Value = value;
        }

        public static DebugVariable Create(string name, object value)
        {
            return new DebugVariable(name, value);
        }

        public override string ToString()
        {
            return $"{Name} : {Value}";
        }
    }
}
