using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Multi_Value_Dictionary {
    public class Operations : KeyValuePair<string, List<string>>, IOperations<string> {

        public IDictionary<string, List<string>> Dictionary() {
            return KeyValueDictionary;
        }

        public bool Add(string key, string value) {
            if (KeyExists(key)) {
                if (ValueExists(key, value)) return false;
                KeyValueDictionary[key].Add(value);
            } else KeyValueDictionary.Add(key, new List<string> { value });

            return true;
        }

        public IEnumerable<string> Keys() => KeyValueDictionary.Keys;

        public IEnumerable<string> Members(string key) => KeyValueDictionary[key];

        public void Remove(string key, [Optional] string value) {
            KeyValueDictionary[key].Remove(value);
            if (!Members(key).Any()) KeyValueDictionary.Remove(key);
        }

        public void RemoveAll(string key) => KeyValueDictionary.Remove(key);

        public void Clear() => KeyValueDictionary.Clear();

        public bool KeyExists(string key) => KeyValueDictionary.ContainsKey(key);

        public bool ValueExists(string key, string value) =>
                   ((key == null || KeyExists(key)) && KeyValueDictionary[key ?? string.Empty].Contains(value));
        
        public IEnumerable<IEnumerable<string>> AllMembers() => KeyValueDictionary.Values;

        public string Items() {
            var i = 0;
            var sb = new StringBuilder();
            foreach (var key in Keys()) Members(key).ToList().ForEach(v => sb.Append($"{++i}) {key}: {v} {Environment.NewLine}"));

            return sb.ToString();
        }

        public Operations(IDictionary<string, List<string>> keyValuePairs) : base(keyValuePairs) { }
    }
}
