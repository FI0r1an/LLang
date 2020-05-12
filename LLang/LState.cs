using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    struct LState
    {
        public int Index;
        public int Column;
        public int Row;
        public readonly string Source;
        public LState(string source)
        {
            Source = source;
            Index = 0;
            Column = Row = 1;
        }
        public char Current() => (Index < Source.Length) ? Source[Index] : '\0';
        public char Lookahead() => (Index + 1 < Source.Length) ? Source[Index + 1] : '\0';
        public char Next()
        {
            var old = Current();
            Index++;
            Column++;
            return old;
        }
    }
}
