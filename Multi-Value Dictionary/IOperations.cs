using System;
using System.Collections.Generic;

namespace Multi_Value_Dictionary {
    public interface IOperations<T> where T : IConvertible {
        bool Add(T key, T value);
        IEnumerable<T> Keys();
        IEnumerable<T> Members(T key);
        void Remove(T key, T value);
        void RemoveAll(T key);
        void Clear();
        bool KeyExists(T key);
        bool ValueExists(T key, T value);
        IEnumerable<IEnumerable<T>> AllMembers();
        T Items();
        IDictionary<T, List<T>> Dictionary();
    }
}