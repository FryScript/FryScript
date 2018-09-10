using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FryScript
{
    public class MemberIndex : IEnumerable<MemberIndex>
    {
        public static readonly MemberIndex Root = new MemberIndex(17, 17);

        private readonly string _name;

        internal readonly int CurrentHash;
        internal readonly int PreviousHash;
        internal readonly MemberIndex PreviousIndex = null;
        internal readonly int Index = -1;

        public string Name => _name;

        public IEnumerable<string> Keys => this.Select(m => m._name);

        public int this[string name] => this.Single(m => m._name == name).Index;

        public MemberIndex(int currentHash, int previousHash)
        {
            CurrentHash = currentHash;
            PreviousHash = previousHash;
        }

        public MemberIndex(int currentHash, MemberIndex previousIndex, string name, int index)
        {
            CurrentHash = currentHash;
            PreviousIndex = previousIndex;
            PreviousHash = previousIndex.CurrentHash;
            _name = name;
            Index = index;
        }

        public bool TryGetValue(string name, out int index)
        {
            var memberIndex = this.SingleOrDefault(m => m._name == name);

            if(memberIndex == null)
            {
                index = 0;
                return false;
            }

            index = memberIndex.Index;

            return true;
        }

        public bool ContainsKey(string name)
        {
            return this.Any(m => m._name == name);
        }

        public IEnumerator<MemberIndex> GetEnumerator()
        {
            var current = this;

            while(current != Root)
            {
                yield return current;

                current = current.PreviousIndex;
            }
        }

        public MemberIndex Mutate(string name)
        {
            if (ContainsKey(name))
                return this;

            return new MemberIndex(GetMemberIndexHash(name), this, name, Index + 1);
        }

        public override int GetHashCode()
        {
            return CurrentHash;
        }

        public override bool Equals(object obj)
        {
            var memberIndex = obj as MemberIndex;

            return memberIndex != null && CurrentHash == memberIndex.CurrentHash;
        }

        public int GetMemberIndexHash(string name)
        {
            return ((CurrentHash << 5) + CurrentHash) ^ name.GetHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
