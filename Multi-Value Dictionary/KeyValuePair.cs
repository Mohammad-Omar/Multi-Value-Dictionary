using System;
using System.Collections.Generic;

namespace Multi_Value_Dictionary {
    public class KeyValuePair<T, T1> where T : IConvertible
        where T1 : IEnumerable<T> {
        public KeyValuePair(IDictionary<T, T1> keyValuePairs) => KeyValueDictionary = keyValuePairs;

        protected IDictionary<T, T1> KeyValueDictionary { get; } = new Dictionary<T, T1>();
    }
}