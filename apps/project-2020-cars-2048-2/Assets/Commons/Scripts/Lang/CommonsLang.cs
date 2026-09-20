using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons.Lang
{
    public class CommonsLang
    {
    }

    // https://msdn.microsoft.com/fr-fr/library/system.diagnostics.contracts.contract.requires(v=vs.110).aspx
    public class Contract
    {
        public static void Requires<T>(bool condition) where T : Exception, new()
        {
            if (!condition)
                throw new T();
        }

        public static void RequiresNotNull(object obj)
        {
            Requires<ArgumentNullException>(obj != null);
        }
    }

    public class Strings
    {
        // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Strings.html)

        public static bool IsNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }
        public static bool IsNotNullOrEmpty(string str)
        {
            return !IsNullOrEmpty(str);
        }

        // https://commons.apache.org/proper/commons-lang/apidocs/org/apache/commons/lang3/StringUtils.html

        public static bool IsBlank(string str)
        {
            return IsNullOrEmpty(str == null ? null : ReplaceAll(str, " ", ""));
        }
        public static bool IsNotBlank(string str)
        {
            return !IsBlank(str);
        }

        public static string ReplaceAll(string str, string oldChar, string newChar)
        {
            return Regex.Replace(str, @"\s+", "");
        }
    }

    public class Arrays
    {
        public static T[] Add<T>(T[] array, T element)
        {
            T[] res = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                res[i] = array[i];
            res[array.Length] = element;
            return res;
        }
        public static bool InBound(string[] array, int i)
        {
            return i < array.Length && i > -1;
        }

        public static bool OutOfBound(string[] array, int i)
        {
            return !InBound(array, i);
        }
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Joiner.html
    public class Joiner
    {
        string separator;

        public static Joiner On(string separator)
        {
            Joiner j = new Joiner();
            j.separator = separator;
            return j;
        }

        public string Join(string[] array)
        {
            string line = "";
            bool first = true;
            foreach (string x in array)
            {
                if (!first)
                    line += separator;
                line += x;
                first = false;
            }
            return line;
        }
    }

}
