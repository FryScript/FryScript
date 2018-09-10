using System;
using System.Collections;

namespace FryScript.Helpers
{
    public static class EnumerableHelper
    {
        public static IEnumerator GetEnumerator(object obj)
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));

            if (obj is IEnumerable enumerable)
                return enumerable.GetEnumerator();

            return ToEnumerable(obj).GetEnumerator();
        }

        private static IEnumerable ToEnumerable(object obj)
        {
            yield return obj;
        }
    }
}
