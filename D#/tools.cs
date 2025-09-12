using System.Text;

class Tools
{
    // 生成されるC++コードのバッファ
    internal static StringBuilder cpplang = new(@"
int main() {
int QUITCODE = 0;
");
    internal static StringBuilder includes = new("#include <bits/stdc++.h>\nusing namespace std;\n");
    internal static StringBuilder funcDefs = new();
    internal static readonly string finlang = @"goto QUITLABEL;
QUITLABEL: return QUITCODE;
}
";
    internal static readonly HashSet<string> ValidHeaders = new HashSet<string>
    {
        "iostream", "vector", "string", "cmath", "algorithm", "array", "deque", "list",
    };

    // 定義済み関数の管理
    internal static readonly HashSet<string> DefinedFunctions = new HashSet<string>();

    // ブロックの整合性チェック用
    internal static int MainOpenBraces = 0; // mainの初期"{"
    internal static int FuncOpenBraces = 0;

    internal static readonly Dictionary<string, string> TypeMap = new()
    {
        { "rin", "string" },
        { "teg", "int" },
        { "cim", "double" },
        { "tnil", "bool" },
        {"lis","vector"}
    };

    internal static int CountIndent(string line)
    {
        int spaceCount = 0;
        foreach (char c in line)
        {
            if (c == ' ') spaceCount++;
            else if (c == '\t') spaceCount += 4; // タブもスペース4つ相当
            else break;
        }
        return spaceCount / 4;
    }


    internal static string Ctype(string type, int lineNumber)
    {
        if (string.IsNullOrEmpty(type)) throw new Exception($"Invalid type at line {lineNumber}");

        string[] parts = type.Split(':');
        string baseType = parts[^1]; // 最後の部分がベース型
        if (!TypeMap.TryGetValue(baseType, out var ctype))
            throw new Exception($"Unknown base type: {baseType} at line {lineNumber}");

        // ネストされたlisの数だけvectorを積む
        for (int j = parts.Length - 2; j >= 0; j--)
        {
            if (parts[j] != "lis")
                throw new Exception($"Invalid type format: {type} at line {lineNumber}. Expect 'lis:' repeated followed by base type.");
            ctype = $"vector<{ctype}>";
        }
        return ctype;
    }
}