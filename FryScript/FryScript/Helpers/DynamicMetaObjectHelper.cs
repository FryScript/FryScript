using FryScript.HostInterop;
using System;
using System.Dynamic;

namespace FryScript.Helpers
{

    public static class DynamicMetaObjectHelper
    {
        public static DynamicMetaObject GetDynamicMetaObject(DynamicMetaObject target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            if (target.Value is IDynamicMetaObjectProvider)
                return target;

            DynamicMetaObject result;
            if((result = TypeProvider.Current.GetMetaObject(target.Expression, target.Value)) == null)
                throw new ArgumentException(string.Format("Unable to get a {0} instance for value {1}",
                    typeof(DynamicMetaObject).FullName,
                    target.LimitType.FullName));

            return result;
        }
    }
}
