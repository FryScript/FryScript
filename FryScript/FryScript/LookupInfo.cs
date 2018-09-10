using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript
{
    public struct LookupInfo
    {
        public readonly int Index;
        public readonly int PreviousHash;
        public readonly int CurrentHash;

        public LookupInfo(int index, int previousHash, int currentHash)
        {
            Index = index;
            PreviousHash = previousHash;
            CurrentHash = currentHash;
        }
    }
}
