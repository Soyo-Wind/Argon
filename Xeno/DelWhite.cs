using System.Runtime.CompilerServices;

namespace Xeno;

class DelWhite
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string RemoveWhitespace(string input)
    {
        return input.Replace("\r", "")
                    .Replace("\n", "")
                    .Replace("\t", "")
                    .Replace(" ", "");
    }
}