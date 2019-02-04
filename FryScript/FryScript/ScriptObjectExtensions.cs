using FryScript.CallSites;
using FryScript.HostInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FryScript
{
    public static class ScriptObjectExtensions
    {
        public static object SetMember(this IScriptObject source, int index, object value)
        {
            var memberData = source.MemberData ?? (source.MemberData = new object[16]);
            
            if (memberData.Length <= index)
                while (source.MemberData.Length <= index)
                    Array.Resize(ref memberData, memberData.Length + 16);

            return memberData[index] = value;
        }

        public static object SetIndex(this IScriptObject source, string name, object value)
        {
            return CallSiteCache.Current.SetMember(name, source, value);
        }

        public static object GetMember(this IScriptObject source, int index)
        {
            return source.MemberData[index];
        }

        public static object GetIndex(this IScriptObject source, string name)
        {
            return CallSiteCache.Current.GetMember(name, source);
        }

        public static bool IsValidSetMember(this IScriptObject source, MemberIndex memberIndex)
        {
            var curIndex = source.MemberIndex;
            if (curIndex == memberIndex)
                return true;

            if (curIndex.CurrentHash == memberIndex.PreviousHash)
            {
                source.MemberIndex = memberIndex;
                return true;
            }

            return false;
        }

        public static bool IsValidGetMember(this IScriptObject source, MemberIndex memberIndex)
        {
            return source.MemberIndex == memberIndex;
        }

        public static IEnumerable<string> GetMembers(this IScriptObject source)
        {
            var members = TypeProvider.Current.GetMemberNames(source.GetType());

            //if (HasTarget)
            //{
            //    members = members.Union(TypeProvider.Current.GetMemberNames(TargetType));
            //}

            members = members.Union(source.MemberIndex.Keys);

            return members;
        }
    }
}
