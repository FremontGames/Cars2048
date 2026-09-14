using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commons.Tests
{
    class GameTest
    {
        public static string[][] ReadFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[][] res = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var regex = new Regex("  *");
                string[] words = regex.Split(line);
                res[i] = new string[words.Length];
                for (int j = 0; j < words.Length; j++)
                    res[i][j] = words[j];
            }
            return res;
        }

    }

    class GameAssert
    {
        public static void AreEqual(string[] expected, string[] actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                if (!"?".Equals(expected[i]))
                    Assert.AreEqual(expected[i], actual[i]);
        }

        public static void AreEqual(string[][] expected, string[][] actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                AreEqual(expected[i], actual[i]);
        }
    }
}
