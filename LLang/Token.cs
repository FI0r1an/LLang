using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    struct Token
    {
        public readonly TokenType Type;
        public readonly string StringValue;
        public readonly double NumberValue;
        public readonly int Row;
        public readonly int Column;
        public override string ToString() => $"<{Type} {StringValue} {NumberValue}>";
        public Token(TokenType type, string sv, LState ls)
        {
            Type = type;
            StringValue = sv;
            NumberValue = 0d;
            Row = ls.Row;
            Column = ls.Column;
        }
        public Token(TokenType type, LState ls)
        {
            Type = type;
            StringValue = "";
            NumberValue = 0d;
            Row = ls.Row;
            Column = ls.Column;
        }
        public Token(TokenType type, double nv, LState ls)
        {
            Type = type;
            StringValue = "";
            NumberValue = nv;
            Row = ls.Row;
            Column = ls.Column;
        }
    }
}
