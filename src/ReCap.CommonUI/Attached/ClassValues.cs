using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCap.CommonUI
{
    public sealed class ClassValues
        : Dictionary<string, bool>
    {
        static IEnumerable<KeyValuePair<string, bool>> Combine(string[] names, bool[] values)
        {
            int namesCount = names.Length;
            int valuesCount = names.Length;
            int count = Math.Min(namesCount, valuesCount);
            for (int i = 0; i < count; i++)
            {
                yield return new(names[i], values[i]);
            }
        }


        static T[] SafeMakeArray<T>(IEnumerable<T> enumerable)
            => enumerable != null
                ? enumerable.ToArray()
                : Array.Empty<T>();


        public ClassValues(IEnumerable<string> names, IEnumerable<bool> values)
            : this(SafeMakeArray(names), SafeMakeArray(values))
        {}
        public ClassValues(string[] names, bool[] values)
            : this(Combine(names, values))
        {}
        public ClassValues(IEnumerable<KeyValuePair<string, bool>> enumerable)
            : this()
        {
            if (enumerable == null)
                return;

            foreach (var pair in enumerable)
            {
                Add(pair.Key, pair.Value);
            }
        }


        public ClassValues()
            : base()
        {}




        public override string ToString()
        {
            string result = base.ToString();
            if (Count > 0)
                result += "\n" + string.Join("\n", FormatPairsInternal());

            return result;
        }



        IEnumerable<string> FormatPairsInternal()
        {
            var keys = Keys;
            int maxLength = int.MinValue;
            foreach (string key in keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                maxLength = Math.Max(maxLength, key.Length);
            }
            return FormatPairsInternal(maxLength);
        }
        IEnumerable<string> FormatPairsInternal(int maxLength)
        {
            foreach (var pair in this)
            {
                yield return FormatPairInternal(pair, maxLength);
            }
        }
        string FormatPairInternal(KeyValuePair<string, bool> pair, int maxLength)
        {
            string result = $"{pair.Key}:";
            while (maxLength >= result.Length)
            {
                result += " ";
            }
            result += pair.Value;
            return result;
        }
    }
}