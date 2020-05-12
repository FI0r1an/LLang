using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LLang
{
    class Lexer
    {
        private LState lState;
        public Lexer(LState ls)
        {
            lState = ls;
        }
        private char Next() => lState.Next();
        private char Current() => lState.Current();
        private char LookAhead() => lState.Lookahead();
        public bool NotEnd() => (lState.Index < lState.Source.Length);
        private void Assert(bool b, string msg)
        {
            if (!b) throw new Exception($"{msg}: At Column {lState.Column}, Row {lState.Row}");
        }
        private void SkipWhitespace()
        {
            while (Util.IsWhitespace(Current()))
            {
                var old = Next();
                if (Util.IsLine(old))
                {
                    var cur = Current();
                    if (Util.IsLine(cur) && cur != old) Next();
                    lState.Column = 1;
                    lState.Row++;
                }
            }
        }
        private bool IsComment()
        {
            var old = Current();
            if (old == '/')
            {
                var cur = LookAhead();
                return (cur == old || cur == '*');
            }
            return false;
        }
        private void SkipComment()
        {
            Next();
            var sign = Next();
            if (sign == '*')
            {
                while (Current() != '*' && LookAhead() != '/')
                {
                    SkipWhitespace();
                    if (Current() == '*' && LookAhead() == '/') break;
                    Next();
                }
                Next(); Next();
                return;
            }
            while (!Util.IsLine(Current())) Next();
        }
        private Token GetNumberToken()
        {
            var str = "";
            while (Util.IsNumber(Current())) str += Next();
            Assert(double.TryParse(str, out double num), $"Can't convert {str} to double");
            return new Token(TokenType.Number, num, lState);
        }
        private Token GetStringToken()
        {
            var str = "";
            var sign = Next();
            while (Current() != sign)
            {
                Assert(NotEnd(), $"Missing {sign}");
                str += Next();
            }
            Next();
            return new Token(TokenType.String, str, lState);
        }
        private Token GetNameToken()
        {
            var str = "";
            while (Util.IsAlpha(Current()))
            {
                str += Next();
            }
            var idx = Keywords.Keyword.IndexOf(str);
            if (idx >= 0)
            {
                Assert(idx + 4 < 47, "Out of range");
                return new Token((TokenType)(idx + 4), lState);
            }
            else return new Token(TokenType.Name, str, lState);
        }
        private Token GetOperatorToken()
        {
            var cur = Current();
            var all = cur.ToString() + LookAhead();
            int tkt;
            var idx1 = Keywords.Keyword.IndexOf(cur.ToString());
            var idx2 = Keywords.Keyword.IndexOf(all);
            if (idx2 == -1) Next();
            else Next(); Next();
            tkt = (idx2 == -1) ? idx1 : idx2;
            return new Token((TokenType)(tkt + 4), lState);
            #region Useless
            /*switch (cur)
            {
                case '+':
                    if (next == cur) { tkt = TokenType.Inc; Next(); }
                    else { tkt = TokenType.Add; }
                    break;
                case '-':
                    if (next == cur) { tkt = TokenType.Dec; Next(); }
                    else { tkt = TokenType.Sub; }
                    break;
                case '*':
                    tkt = TokenType.Mul;
                    break;
                case '/':
                    tkt = TokenType.Div;
                    break;
                case '%':
                    tkt = TokenType.Mod;
                    break;
                case '>':
                    if (next == cur) { tkt = TokenType.Shr; Next(); }
                    else if (next == '=') { tkt = TokenType.Ge; Next(); }
                    else { tkt = TokenType.Gt; }
                    break;
                case '<':
                    if (next == cur) { tkt = TokenType.Shl; Next(); }
                    else if (next == '=') { tkt = TokenType.Le; Next(); }
                    else { tkt = TokenType.Lt; }
                    break;
                case '=':
                    if (next == cur) { tkt = TokenType.Eq; Next(); }
                    else tkt = TokenType.Assign; Next();
                    break;
                case '&':
                    if (next == cur) tkt = TokenType.And; Next();
                    break;
                case '|':
                    if (next == cur) tkt = TokenType.Or; Next();
                    break;
                case '!':
                    if (next == '=') { tkt = TokenType.Ne; Next(); }
                    else tkt = TokenType.Not;
                    break;
                case '(':
                    tkt = TokenType.LeftBracket;
                    break;
                case ')':
                    tkt = TokenType.RightBracket;
                    break;
                case '[':
                    tkt = TokenType.LeftIndexBracket;
                    break;
                case ']':
                    tkt = TokenType.RightIndexBracket;
                    break;
                case '{':
                    tkt = TokenType.LeftTableBracket;
                    break;
                case '}':
                    tkt = TokenType.RightTableBracket;
                    break;
                case ',':
                    tkt = TokenType.Split;
                    break;
                case '.':
                    tkt = TokenType.IndexMember;
                    break;
                default:
                    throw new Exception("Wrong");
            }
            Next();*/
            #endregion
        }
        public Token NextToken()
        {
            for (; ; )
            {
                char cur = Current(), next = LookAhead();
                switch (cur)
                {
                    case '\n':
                    case '\r':
                    case ' ':
                    case '\t':
                        SkipWhitespace();
                        break;
                    case '/':
                        if (IsComment()) { SkipComment(); break; }
                        goto default;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                    case '+':
                    case '-':
                        if ((cur == '+' || cur == '.' || cur == '-') && Util.IsDigit(next))
                            return GetNumberToken();
                        else if (Util.IsDigit(cur))
                            return GetNumberToken();
                        else
                            goto default;
                    case '\"':
                    case '\'':
                        return GetStringToken();
                    default:
                        if (Util.IsAlpha(cur))
                        {
                            return GetNameToken();
                        }
                        else return GetOperatorToken();
                        throw new Exception("Wrong");
                }
            }
        }
        public Token LookAheadToken()
        {
            int idx = lState.Index, col = lState.Column, row = lState.Row;
            NextToken();
            var tk = NextToken();
            lState.Index = idx;
            lState.Column = col;
            lState.Row = row;
            return tk;
        }
        public Token CurrentToken()
        {
            int idx = lState.Index, col = lState.Column, row = lState.Row;
            var tk = NextToken();
            lState.Index = idx;
            lState.Column = col;
            lState.Row = row;
            return tk;
        }
    }
}
