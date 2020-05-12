using System.Collections.Generic;

namespace LLang
{
    enum TokenType
    {
        Nope,
        Name,
        String,
        Number,
        // Binary
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Pow,
        Shr,
        Shl,
        // Binary Boolean
        Eq,
        Ne,
        Ge,
        Gt,
        Le,
        Lt,
        And,
        Or,
        // Unary
        Inc,
        Dec,
        Not,
        // Other
        Assign,
        LeftBracket,
        RightBracket,
        LeftIndexBracket,
        RightIndexBracket,
        LeftTableBracket,
        RightTableBracket,
        IndexMember,
        Split,
        // Keyword
        If,
        Else,
        Elif,
        End,
        For,
        Foreach,
        In,
        While,
        Do,
        Repeat,
        Until,
        Break,
        Function,
        Return
    }
    static class Keywords
    {
        static public List<string> Keyword = new List<string>()
        {
            "+", "-", "*", "/", "%", "^", ">>", "<<",
            "==", "!=", ">=", ">", "<=", "<", "&&", "||",
            "++", "--", "!",
            "=", "(", ")", "[", "]", "{", "}", ".", ",",
            "if", "else", "elif", "end", "for", "foreach", "in", "while", "do",
            "repeat", "until", "break", "function", "return"
        };
    }
}
