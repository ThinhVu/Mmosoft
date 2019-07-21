using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mmosoft.Syntax
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this Object o)
        {
            return o == null;
        }
    }
}


namespace Mmosoft.Syntax.PrivateStore
{
    public static class ObjectExtensions
    {
        private static Dictionary<Object, Dictionary<string, object>> _dynamicStore
            = new Dictionary<object, Dictionary<string, object>>();

        // Dynamic extensions
        public static T Get<T>(this Object o, string key)
        {
            if (key.IsNull()) throw new ArgumentNullException();
            if (o.GetType().IsPrimitive) throw new ArgumentException("Primitive are not allow for private store");
            if (!_dynamicStore.ContainsKey(o)) return default(T);
            if (!_dynamicStore[o].ContainsKey(key)) return default(T);
            return (T)_dynamicStore[o][key];
        }
        public static void Set<T>(this Object o, string key, T value)
        {
            if (key.IsNull()) throw new ArgumentNullException();
            if (o.GetType().IsPrimitive) throw new ArgumentException("Primitive are not allow for private store");
            if (!_dynamicStore.ContainsKey(o))
                _dynamicStore[o] = new Dictionary<string, object>();
            _dynamicStore[o][key] = value;
        }
    }
}