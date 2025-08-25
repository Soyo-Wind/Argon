
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
        string input = @"%(main)=rin[<""asdf"">]/";
        VEx.codex(input.Replace("\r", ""));
        cscode += "}";
        Console.WriteLine(cscode);
    }

    internal static void addCode(string code)
    {
        cscode += code + "\n";
    }

}