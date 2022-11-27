using System.Collections.Generic;

namespace ThirdParty
{
    public static class DictionaryDeconstruct
    {
        public static void Deconstruct<TKey, TVal>(this KeyValuePair<TKey, TVal> kvp, out TKey key, out TVal value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}

