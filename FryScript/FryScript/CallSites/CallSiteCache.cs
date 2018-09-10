using FryScript.Binders;
using System;
using System.Runtime.CompilerServices;

namespace FryScript.CallSites
{

    public class CallSiteCache
    {
        private readonly ItemCache<string, CallSite<Func<CallSite, object, object>>> _getMember = new ItemCache<string, CallSite<Func<CallSite, object, object>>>(1000);
        private readonly ItemCache<string, CallSite<Func<CallSite, object, object, object>>> _setMember = new ItemCache<string, CallSite<Func<CallSite, object, object, object>>>(1000);
        private readonly ItemCache<string, CallSite<Func<CallSite, object, object>>> _hasMember = new ItemCache<string, CallSite<Func<CallSite, object, object>>>(1000);

        public static CallSiteCache Current = new CallSiteCache();

        public object GetMember(string name, object instance)
        {
            if (!_getMember.TryGetValue(name, out CallSite<Func<CallSite, object, object>> callSite))
            {
                var binder = BinderCache.Current.GetMemberBinder(name);
                callSite =
                    (CallSite<Func<CallSite, object, object>>)
                        CallSite.Create(typeof(Func<CallSite, object, object>), binder);
                _getMember.Add(name, callSite);
            }

            return callSite.Target(callSite, instance);
        }

        public object SetMember(string name, object instance, object value)
        {
            if (!_setMember.TryGetValue(name, out CallSite<Func<CallSite, object, object, object>> callSite))
            {
                var binder = BinderCache.Current.SetMemberBinder(name);
                callSite =
                    (CallSite<Func<CallSite, object, object, object>>)
                        CallSite.Create(typeof(Func<CallSite, object, object, object>), binder);

                _setMember.Add(name, callSite);
            }

            return callSite.Target(callSite, instance, value);
        }

        public bool HasMember(string name, object instance)
        {
            if (!_hasMember.TryGetValue(name, out CallSite<Func<CallSite, object, object>> callSite))
            {
                var binder = BinderCache.Current.HasOperationBinder(name);
                callSite = (CallSite<Func<CallSite, object, object>>)
                    CallSite.Create(typeof(Func<CallSite, object, object>), binder);

                _hasMember.Add(name, callSite);
            }

            return (bool)callSite.Target(callSite, instance);
        }
    }
}
