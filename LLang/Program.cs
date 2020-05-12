using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = File.ReadAllText("test.txt");
            var sw = new Stopwatch();
            sw.Start();
            LState ls = new LState(source);
            Lexer lex = new Lexer(ls);
            while (lex.NotEnd())
            {
                var tk = lex.NextToken();
                Console.WriteLine(tk);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.ReadLine();
        }
    }
}
