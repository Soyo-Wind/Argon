
using System.Text.RegularExpressions;

enum TokenType
{
    Header,
    FrontPare,
    BackPare,
    FrontBlock,
    BackBlock,
    ForLoop,
    EachLoop,
    IntLiteral,
    LongLiteral,
    ByteLiteral,
    StringLiteral,
    CharLiteral,
    FloatLiteral,
    DoubleLiteral,
    BoolLiteral,
    DecimalLiteral,
    Comment
}

class Program
{
    static string? HeaderValue = null;

    private static readonly string classHeader = "▻", StringLiteralPrefix = "⫗", CharLiteralPrefix = "⨝";

    private static List<(int code, string errortype, string message)> warns = new();
    private static List<(TokenType type, string token)> tokens = new();

    static void Main()
    {
        string input = @"
            ▻FizzBuzz.main;
⨋for≪◈∈i = ∈1;, ∈i <= ∈100,
	⨋stut≪∈i%15⨬⫗ ⩿
		∈₪ == ∈0 => ""FizzBuzz"",
		 ∈₪ % ∈3 == ∈0 => ""Fizz"",
		  ∈₪ % ∈5 == ∈0 => ""Buzz"",
		   ∈_ => ∈i
		⪀
	≫;
≫;
        ";

        // 行分割して先頭のheaderを探す
        string[] lines = input.Split(["\r\n", "\r", "\n"], 0);
        string headerPattern = @$"^\s*{classHeader}([^;]+);.*$";
        List<(int idx, string value)> headerLines = new();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            Match m = Regex.Match(line, headerPattern);
            if (m.Success)
            {
                headerLines.Add((i, m.Groups[1].Value.Trim()));
            }
        }

        List<string> headerError = new();

        // 複数出現 or 最初以外 → エラー
        if (headerLines.Count > 1)
        {
            warns.Add((1, "HeaderError", "複数のヘッダが見つかりました。"));
        }
        else if (headerLines.Count == 1)
        {
            // 最初の非空白行か？
            int firstNonEmptyLine = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    firstNonEmptyLine = i;
                    break;
                }
            }
            if (headerLines[0].idx != firstNonEmptyLine)
            {
                warns.Add((1, "HeaderError", "ヘッダが最初の非空白行ではありません。"));
            }
            HeaderValue = headerLines[0].value;
        }

        // 「▻???;」行を全て削除してから解析
        foreach (var hl in headerLines)
        {
            lines[hl.idx] = ""; // 空行に
        }
        input = string.Join("\n", lines);

        string commentPattern = @"//.*?$|/\*[\s\S]*?\*/";
        string literalPattern = @$"(?:{StringLiteralPrefix}""([^""]*)"")|(?:{CharLiteralPrefix}""([^""]*)"")|""([^""]*)""";

        List<string>
        stringLiterals = new(),
        charLiterals = new();

        // コメント抽出＆除去
        string codeWithoutComments = input;
        foreach (Match m in Regex.Matches(input, commentPattern, RegexOptions.Multiline))
        {
            codeWithoutComments = codeWithoutComments.Replace(m.Value, "");
        }

        // リテラル抽出と分類
        foreach (Match m in Regex.Matches(codeWithoutComments, literalPattern))
        {
            if (m.Groups[1].Success) // ⫗"文字列"
            {
                stringLiterals.Add(m.Groups[1].Value);
                tokens.Add((TokenType.StringLiteral, m.Groups[1].Value));
            }
            else if (m.Groups[2].Success) // ⨝""文字""
            {
                string val = m.Groups[2].Value;
                if (val.Length == 1)
                {
                    charLiterals.Add(val);
                    tokens.Add((TokenType.CharLiteral, val));
                }
                else
                    warns.Add((2, "CharLiteralError", $"{charLiterals}\"{val}\" は1文字ではありません"));
            }
            else if (m.Groups[3].Success) // "..."
            {
                string val = m.Groups[3].Value;
                if (val.Length >= 2)
                {
                    stringLiterals.Add(val);
                    tokens.Add((TokenType.StringLiteral, val));
                }
                else
                    warns.Add((3, "StringLiteralError", $"\"{val}\" は2文字未満、かつ特別な接頭辞も無し"));
            }
        }

        // 出力
        if (!string.IsNullOrEmpty(HeaderValue) && headerError.Count == 0)
        {
            Console.WriteLine($"=== HeaderValue (class変数) ===\n{HeaderValue}\n");
        }

        Console.WriteLine($"=== トークン一覧({tokens.Count}) ===");
        foreach (var (type, token) in tokens)
        {
            Console.WriteLine($"[[{Enum.GetName(type)}] : [{token}]]");
        }

        Console.WriteLine("\n=== WARNー一覧 ===");
        foreach (var (code, errortype, message) in warns)
        {
            Console.WriteLine($"[{code}] {errortype}: {message}");
        }

        if (headerError.Count > 0)
        {
            Console.WriteLine("\n=== ヘッダエラー ===");
            foreach (var e in headerError) Console.WriteLine(e);
        }
    }
}
