namespace FryScript
{
    public class MemberIndexLookup
    {
        private readonly ItemCache<int, MemberIndex> _cache = new ItemCache<int, MemberIndex>(1000);
 
        public static MemberIndexLookup Current = new MemberIndexLookup();

        public MemberLookupInfo MutateMemberIndex(MemberIndex memberIndex, string name)
        {
            lock (_cache)
            {
                int index;
                if (memberIndex.TryGetValue(name, out index))
                    return new MemberLookupInfo
                    {
                        Index = index,
                        MemberIndex = memberIndex
                    };

                var nextIndexHash = memberIndex.GetMemberIndexHash(name);

                MemberIndex nextIndex;
                if (!_cache.TryGetValue(nextIndexHash, out nextIndex))
                {
                    _cache.Add(nextIndexHash, nextIndex = memberIndex.Mutate(name));
                }

                return new MemberLookupInfo
                {
                    Index = nextIndex[name],
                    MemberIndex = nextIndex
                };
            }
        }

        public bool TryGetMemberIndex(MemberIndex memberIndex, string name, out MemberLookupInfo memberLookupInfo)
        {
            lock (_cache)
            {
                int index;
                if (memberIndex.TryGetValue(name, out index))
                {
                    memberLookupInfo = new MemberLookupInfo
                    {
                        MemberIndex = memberIndex,
                        Index = index
                    };

                    return true;
                }

                memberLookupInfo = new MemberLookupInfo();
                return false;
            }
        }
    }
}
