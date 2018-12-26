using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AdventOfCode2018
{
    public static class Helper
    {
        public static string ReadEmbeddedFile(Assembly assembly, string filePath)
        {
            var name = $"{assembly.GetName().Name}.{filePath}";
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(name);
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string Join<T>(this T[,] obj)
        {
            return obj.Join(null);
        }

        public static string Join<T>(this T[,] obj, string seperator)
        {
            var isSeperator = !string.IsNullOrEmpty(seperator);

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < obj.GetUpperBound(0) + 1; i++)
            for (var j = 0; j < obj.GetUpperBound(1) + 1; j++)
            {
                stringBuilder.Append(obj[i, j]);
                if (isSeperator)
                {
                    stringBuilder.Append(seperator);
                }
            }

            return stringBuilder.ToString();
        }

        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}