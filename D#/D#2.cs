
using System.Text;

class DS2
{
    // 生成されるC++コードのバッファ（パフォーマンス向上のためStringBuilder使用）
    private static StringBuilder cpplang = new(@"using namespace std;
int main() {
    int QUITCODE = 0;
");
    private static StringBuilder includes = new("#include <bits/stdc++.h>\n");
    private static readonly string finlang = @"goto QUITLABEL;
    QUITLABEL:return QUITCODE;
}
";

    private static List<string> IncludedHeaders = new();

    private static bool inFunction = false; // 関数定義内かどうかを追跡（将来の拡張用）
    private static readonly Stack<string> blockStack = new Stack<string>(); // ブロックの種類を追跡

    // D#の型をC++の型にマッピングする辞書
    private static readonly Dictionary<string, string> TypeMap = new()
    {
        { "rin", "string" },
        { "teg", "int" },
        { "cim", "double" },
        { "tnil", "bool" }
    };

    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            // D#コードをファイルから読み込み
            string code = File.ReadAllText(@"m.ds");
            string[] lines = code.Split('\n');
            int currentIndent = 0; // 現在のインデントレベル（スペース4つ=1レベル）
            bool inConditionalBlock = false; // if/elif/else/elブロックの追跡

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Replace("\r", "").TrimEnd(); // 改行コードの正規化
                if (string.IsNullOrWhiteSpace(line)) continue;

                // インデントをカウント（スペース4つ=1レベル）
                int indentCount = CountIndent(line);

                string trimmedLine = line.TrimStart();
                string[] tokens = Split(trimmedLine, i + 1);

                // トークンが空の場合はスキップ
                if (tokens.Length == 0) continue;

                // 条件分岐以外の構文でチェーンを終了（インデント0の場合）
                if (tokens[0] != "if" && tokens[0] != "elif" && tokens[0] != "else" && tokens[0] != "el" && indentCount == 0)
                {
                    inConditionalBlock = false;
                }

                // インデント減少時にブロックを閉じる
                while (indentCount < currentIndent)
                {
                    if (inFunction && currentIndent == 1)
                    {
                        inFunction = false; // 関数定義の終了（将来用）
                    }
                    if (blockStack.Count > 0) blockStack.Pop();
                    cpplang.Append($"{Indent(--currentIndent)}}}\n");
                }

                // インデントチェック
                if (indentCount != currentIndent)
                {
                    throw new Exception($"Invalid indent at line {i + 1}: Expected {currentIndent * 4} spaces, found {indentCount * 4} spaces.");
                }

                // 構文変換
                ConvertToCpp(tokens, trimmedLine, currentIndent, ref inConditionalBlock, i + 1);

                // if/elif/else/el/for/whileならインデント増加
                if (tokens[0] == "if" || tokens[0] == "elif" || tokens[0] == "else" || tokens[0] == "el" ||
                    tokens[0] == "for" || tokens[0] == "while")
                {
                    blockStack.Push(tokens[0]);
                    currentIndent++;
                    if (tokens[0] == "if" || tokens[0] == "elif") inConditionalBlock = true;
                }
            }

            // 残りのブロックを閉じる
            while (currentIndent > 0)
            {
                if (inFunction && currentIndent == 1)
                {
                    inFunction = false;
                }
                if (blockStack.Count > 0) blockStack.Pop();
                cpplang.Append($"{Indent(--currentIndent)}}}\n");
            }

            // C++コードを完成させ、ファイルに出力
            includes.Append(cpplang+finlang);
            cpplang = includes;
            File.WriteAllText("main.cpp", cpplang.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // インデントを生成（スペース4つ×レベル）
    private static string Indent(int level) => new string(' ', level * 4);

    // インデントレベルをカウント
    private static int CountIndent(string line)
    {
        int count = 0;
        foreach (char c in line)
        {
            if (c != ' ') break;
            count++;
        }
        return count / 4; // スペース4つ=1インデントレベル
    }

    // D#の型をC++の型に変換
    private static string Ctype(string type, int lineNumber)
    {
        if (TypeMap.TryGetValue(type, out var ctype))
            return ctype;
        throw new Exception($"Unknown type: {type} at line {lineNumber}");
    }
    
    private static readonly HashSet<string> ValidHeaders = new HashSet<string>
    {
        "iostream", "vector", "string", "cmath", "algorithm", "array", "deque", "list",
        "forward_list", "set", "map", "unordered_set", "unordered_map", "stack", "queue",
        "span", "flat_map", "flat_set", "mdspan", "fstream", "sstream", "iomanip", "ios",
        "iosfwd", "ostream", "istream", "print", "format", "string_view", "regex", "charconv",
        "complex", "numeric", "random", "numbers", "bitset", "chrono", "ctime", "thread",
        "mutex", "condition_variable", "future", "atomic", "shared_mutex", "latch", "barrier",
        "semaphore", "stdexcept", "exception", "system_error", "cassert", "cctype", "cerrno",
        "cfenv", "cfloat", "cinttypes", "climits", "clocale", "csetjmp", "csignal", "cstdarg",
        "cstddef", "cstdint", "cstdio", "cstdlib", "cstring", "ctgmath", "cuchar", "cwchar",
        "cwctype", "filesystem", "locale", "codecvt", "type_traits", "typeindex", "typeinfo",
        "ratio", "new", "memory", "memory_resource", "coroutine", "concepts", "ranges",
        "spanstream", "functional", "iterator", "utility", "tuple", "optional", "variant",
        "any", "compare", "bit", "source_location", "version"
    };

    // トークン分割（文字列リテラルと括弧を考慮）
    private static string[] Split(string a, int lineNumber)
    {
        a = a.TrimEnd() + " ";
        List<string> strings = new();
        int start = 0;
        bool inString = false, inChar = false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] == ' ' && !inString)
            {
                if (i > start)
                    strings.Add(a[start..i]);
                start = i + 1;
            }
            else if (a[i] == '"')
            {
                if (inString)
                {
                    strings.Add(a[start..(i + 1)]);
                    start = i + 1;
                    inString = false;
                }
                else
                {
                    inString = true;
                    start = i;
                }
            }
            else if (a[i] == '\'')
            {
                if (inChar)
                {
                    strings.Add(a[start..(i + 1)]);
                    start = i + 1;
                    inChar = false;
                }
                else
                {
                    inChar = true;
                    start = i;
                }
            }
            else if (!inString && a[i] == '(')
            {
                int depth = 1;
                int startParen = i;
                i++;
                while (i < a.Length && depth > 0)
                {
                    if (a[i] == '(') depth++;
                    else if (a[i] == ')') depth--;
                    i++;
                }
                if (depth == 0)
                {
                    string innerExpr = a[startParen..i];
                    strings.Add(innerExpr);
                    start = i;
                }
                else
                {
                    throw new Exception($"Unmatched parenthesis at line {lineNumber}");
                }
            }
        }

        if (inString)
            throw new Exception($"Unclosed string literal at line {lineNumber}");

        if (start < a.Length - 1)
            strings.Add(a[start..(a.Length - 1)]);

        return strings.ToArray();
    }

    // D#構文をC++に変換
    private static void ConvertToCpp(string[] tokens, string line, int indentLevel, ref bool inConditionalBlock, int lineNumber)
    {
        string ev = tokens[0];
        try
        {
            if (ev == "def")
            {
                if (tokens.Length != 4) throw new Exception($"Invalid def syntax at line {lineNumber}: Expected 'def <type> <name> <value>'");
                string type = tokens[1];
                string name = tokens[2];
                string value = tokens[3];
                string ctype = Ctype(type, lineNumber);
                cpplang.Append($"{Indent(indentLevel)}{ctype} {name} = {value};\n");
            }
            else if (ev == "deflis")
            {
                if (tokens.Length != 4) throw new Exception($"Invalid deflis syntax at line {lineNumber}: Expected 'deflis <type> <name> <value>'");
                string type = tokens[1];
                string name = tokens[2];
                string value = tokens[3];
                string ctype = Ctype(type, lineNumber);
                cpplang.Append($"{Indent(indentLevel)}vector<{ctype}> {name}{value};\n");
            }
            else if (ev == "q")
            {
                if (tokens.Length != 3) throw new Exception($"Invalid q syntax at line {lineNumber}: Expected 'q <name> <value>'");
                string name = tokens[1];
                string value = tokens[2];
                cpplang.Append($"{Indent(indentLevel)}{name} = {value};\n");
            }
            else if (ev == "add")
            {
                if (tokens.Length != 3) throw new Exception($"Invalid add syntax at line {lineNumber}: Expected 'add <name> <value>'");
                string name = tokens[1];
                string value = tokens[2];
                cpplang.Append($"{Indent(indentLevel)}{name}.push_back({value});\n");

            }
            else if (ev == "del")
            {
                if (tokens.Length != 3) throw new Exception($"Invalid del syntax at line {lineNumber}: Expected 'del <name> <position>'");
                string name = tokens[1];
                string pos = tokens[2];
                cpplang.Append($"{Indent(indentLevel)}{name}.erase({pos});\n");
            }
            else if (ev == "ins")
            {
                if (tokens.Length != 4) throw new Exception($"Invalid ins syntax at line {lineNumber}: Expected 'ins <name> <position> <value>'");
                string name = tokens[1];
                string pos = tokens[2];
                string value = tokens[3];
                cpplang.Append($"{Indent(indentLevel)}{name}.insert({pos},{value});\n");
            }
            else if (ev == "outln")
            {
                if (tokens.Length < 2) throw new Exception($"Invalid outln syntax at line {lineNumber}: Expected 'outln <value>'");
                string value = string.Join(" ", tokens[1..]); // 複数トークンの場合に対応
                cpplang.Append($"{Indent(indentLevel)}cout << {value} << endl;\n");
            }
            else if (ev == "out")
            {
                if (tokens.Length < 2) throw new Exception($"Invalid out syntax at line {lineNumber}: Expected 'out <value>'");
                string value = string.Join(" ", tokens[1..]);
                cpplang.Append($"{Indent(indentLevel)}cout << {value};\n");
            }
            else if (ev == "in")
            {
                if (tokens.Length != 2) throw new Exception($"Invalid in syntax at line {lineNumber}: Expected 'in <variable>'");
                string name = tokens[1];
                cpplang.Append($"{Indent(indentLevel)}cin >> {name};\n");
            }
            else if (ev == "if")
            {
                if (tokens.Length < 2) throw new Exception($"Invalid if syntax at line {lineNumber}: Expected 'if <condition>'");
                string condition = string.Join(" ", tokens[1..]);
                cpplang.Append($"{Indent(indentLevel)}if ({condition}) {{\n");
            }
            else if (ev == "elif")
            {
                if (!inConditionalBlock) throw new Exception($"elif without preceding if at line {lineNumber}: {line}");
                if (tokens.Length < 2) throw new Exception($"Invalid elif syntax at line {lineNumber}: Expected 'elif <condition>'");
                string condition = string.Join(" ", tokens[1..]);
                cpplang.Append($"{Indent(indentLevel)}else if ({condition}) {{\n");
            }
            else if (ev == "else" || ev == "el")
            {
                if (!inConditionalBlock) throw new Exception($"else/el without preceding if/elif at line {lineNumber}: {line}");
                if (tokens.Length > 1) throw new Exception($"Invalid else/el syntax at line {lineNumber}: Expected 'else' or 'el'");
                cpplang.Append($"{Indent(indentLevel)}else {{\n");
            }
            else if (ev == "for")
            {
                if (tokens.Length != 5) throw new Exception($"Invalid for syntax at line {lineNumber}: Expected 'for <var> <start> <condition> <increment>'");
                string varName = tokens[1];
                string start = tokens[2];
                string condition = tokens[3];
                string increment = tokens[4];
                cpplang.Append($"{Indent(indentLevel)}for (int {varName} = {start}; {condition}; {varName} +={increment}) {{\n");
            }
            else if (ev == "while")
            {
                if (tokens.Length < 2) throw new Exception($"Invalid while syntax at line {lineNumber}: Expected 'while <condition>'");
                string condition = string.Join(" ", tokens[1..]);
                cpplang.Append($"{Indent(indentLevel)}while ({condition}) {{\n");
            }
            else if (ev == "quit")
            {
                if (tokens.Length != 2) throw new Exception($"Invalid quit syntax at line {lineNumber}: Expected 'quit <code>'");
                string quitcode = tokens[1];
                cpplang.Append($"{Indent(indentLevel)}QUITCODE = {quitcode};\n{Indent(indentLevel)}goto QUITLABEL;\n");
            }
            else if (ev == "use")
            {
                if (tokens.Length != 2) throw new Exception($"Invalid use syntax at line {lineNumber}: Expected 'use <header>'");
                string header = tokens[1];
                if (ValidHeaders.Contains(header) || IncludedHeaders.Contains(header))
                    throw new Exception($"Invalid header: {header} at line {lineNumber}");
                IncludedHeaders.Add(header);
                includes.Append($"#include <{header}>\n");
            }
            else
            {
                throw new Exception($"Unknown command: {ev} at line {lineNumber}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error at line {lineNumber}: {line}\n{ex.Message}");
        }
    }
}