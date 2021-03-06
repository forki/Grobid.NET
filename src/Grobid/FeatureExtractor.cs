﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Grobid.NET;

namespace Grobid
{
    public class FeatureExtractor
    {
        private static readonly Regex EmailAddress = new Regex("^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\]?)|(?:[a-zA-Z0-9-]+\\.)+(?:[a-zA-Z]){2,}\\.?)$", RegexOptions.Compiled);
        private static readonly Regex PunctuationRegex = new Regex("^[\\,\\:;\\?\\.]+$", RegexOptions.Compiled);

        private static readonly HashSet<string> Months = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec",
        };

        private readonly ILexicon lexicon;

        public FeatureExtractor(ILexicon lexicon)
        {
            this.lexicon = lexicon;
        }

        public string Prefix(string s, int length)
        {
            return s.Substring(0, Math.Min(s.Length, length));
        }

        public string Suffix(string s, int length)
        {
            int len = Math.Min(s.Length, length);
            int offset = s.Length - len;

            return s.Substring(offset, len);
        }

        public Capitalization Case(string s)
        {
            if (s.Length == 0 || s.All(Char.IsDigit))
            {
                return Capitalization.NOCAPS;
            }

            if (s.Length > 1 && Char.IsUpper(s[0]) && s.Skip(1).All(Char.IsLower))
            {
                return Capitalization.INITCAP;
            }

            if (s.Any(Char.IsLower))
            {
                return Capitalization.NOCAPS;
            }

            return Capitalization.ALLCAP;
        }

        public Digit Digit(string s)
        {
            var digitCount = s.Count(Char.IsDigit);
            return digitCount == 0
                       ? Grobid.Digit.NODIGIT
                       : digitCount == s.Length
                           ? Grobid.Digit.ALLDIGIT
                           : Grobid.Digit.CONTAINDIGIT;
        }

        public bool IsSingleChar(string s)
        {
            return s.Length == 1;
        }

        public bool IsMonth(string s)
        {
            return FeatureExtractor.Months.Contains(s);
        }

        public bool IsYear(string s)
        {
            return s.Length == 4 &&
                   s.All(Char.IsDigit) &&
                   (s[0] == '1' || s[0] == '2');
        }

        public bool IsEmailAddress(string s)
        {
            return FeatureExtractor.EmailAddress.IsMatch(s);
        }

        public bool HasHttp(string s)
        {
            return s.Contains("http");
        }

        public bool HasDash(string s)
        {
            return s.Contains("-");
        }

        public Punctuation Punctuation(string s)
        {
            var punc = Grobid.Punctuation.NOPUNCT;

            if (FeatureExtractor.PunctuationRegex.IsMatch(s))
            {
                punc = Grobid.Punctuation.PUNCT;
            }

            switch (s)
            {
                case "(":
                case "[":
                    punc = Grobid.Punctuation.OPENBRACKET;
                    break;
                case ")":
                case "]":
                    punc = Grobid.Punctuation.ENDBRACKET;
                    break;
                case ".":
                    punc = Grobid.Punctuation.DOT;
                    break;
                case ",":
                    punc = Grobid.Punctuation.COMMA;
                    break;
                case "-":
                    punc = Grobid.Punctuation.HYPHEN;
                    break;
                case "\"":
                case "'":
                case "`":
                    punc = Grobid.Punctuation.QUOTE;
                    break;
            }

            return punc;
        }

        public bool IsForename(string s)
        {
            return this.lexicon.IsFirstName(s);
        }

        public bool IsSurname(string s)
        {
            return this.lexicon.IsLastName(s);
        }

        public bool IsName(string s)
        {
            return this.IsForename(s) || this.IsSurname(s);
        }

        public bool IsDictionaryWord(string s)
        {
            return this.lexicon.HasWord(s);
        }

        public bool IsLocationName(string s)
        {
            return false; // TODO: implement me
        }
    }
}