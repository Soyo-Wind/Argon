
using System.Text;

namespace ll1;

class Owen
{
    private static string cscode =
@"using System;
using System.Numerics;
namespace main;
class Owen{
";
    public static void Main(string[] args)
    {
        string input = @"%(main)=rin[<""asdf"">]";
        string[] lines = split(input.Replace("\r", ""));
        VEx.codex(lines);
        Console.WriteLine(cscode);
    }

    internal static void addCode(string code)
    {
        cscode += code + "\n";
    }
    
    private static string[] split(string expr)
    {
        List<string> parts = new();
        StringBuilder current = new(expr);
        for (int pos = 0; pos < expr.Length; pos++)
        {
            if (expr[pos] == '<')
            {
                for (int depth = 1; depth > 0; pos++)
                {
                    if (pos >= expr.Length)
                        throw new Exception("構文違い(<>が閉じていない)");
                    if (expr[pos] == '<')
                        depth++;
                    else if (expr[pos] == '>')
                        depth--;
                }
            }
            else if (expr[pos] == '"')
            {
                for (int depth = 1; expr[depth] != '"'; pos++) ;
            }
            else if (expr[pos] == '/')
            {
                parts.Add(current.ToString(0, pos).Trim());
                current.Remove(0, pos + 1);
            }
        }
        return parts.ToArray();
    }
}