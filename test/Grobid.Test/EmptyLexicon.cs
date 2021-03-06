﻿using System;

using Grobid.NET;

namespace Grobid.Test
{
    public class EmptyLexicon : ILexicon
    {
        public static EmptyLexicon Instance = new EmptyLexicon();

        private EmptyLexicon()
        {
        }

        public bool IsFirstName(string name)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(name, "christopher") == 0;
        }

        public bool IsLastName(string name)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(name, "boumenot") == 0;
        }

        public bool HasWord(string word)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(word, "word") == 0;
        }

        public bool inDictionary(string word)
        {
            return this.HasWord(word);
        }

        public java.util.List inCityNames(java.util.List tokens)
        {
            return new java.util.LinkedList();
        }

        public java.util.List inCityNames(string text)
        {
            return new java.util.LinkedList();
        }

        public string getcountryCode(string str)
        {
            return string.Empty;
        }
    }
}
