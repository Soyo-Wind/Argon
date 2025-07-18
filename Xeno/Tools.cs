namespace Xeno;

internal class Tools
{
    internal static object mults(string left, long right)
    {
        for (long i = 0; i < right; i++)
        {
            left += left;
        }
        return left;
    }

    internal static string aaa(string code)
    {
        string[][] changers = {[VarType.Bool.ToString(), "bool "],
            [VarType.Char.ToString(), "char "],
            [VarType.Str.ToString(), "string "],
            [VarType.Float.ToString(), "float "],
            [VarType.Double.ToString(), "double "],
            [VarType.Decimal.ToString(), "decimal "],
            [VarType.Int.ToString(), "int "],
            [VarType.Long.ToString(), "long "]};
        foreach (var changer in changers)
        {
            code = code.Replace(changer[0], changer[1]);
        }
        return code;
    }
}