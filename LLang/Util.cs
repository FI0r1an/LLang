using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    static class Util
    {
        public static bool IsSpace(char c) => c == ' ' || c == '\t';
        public static bool IsLine(char c) => c == '\r' || c == '\n';
        public static bool IsWhitespace(char c) => IsSpace(c) || IsLine(c);
        public static bool IsAlpha(char c) =>
            (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        public static bool IsDigit(char c) => c >= '0' && c <= '9';
        public static bool IsNumber(char c) => IsDigit(c) || (c == '.' || c == '-');
    }
}
