using System.Text.RegularExpressions;

namespace Xeno;

enum tt
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
    Comment,
    DECPrefix,
    SigPrefix
}

class Program
{
    private static void NotACT(string raw, string p)
    {
        string code = raw;
        int index = 0;
        while (true)
        {
            // SigPrefix + 名前 + SigStart「⨋for≪」を検索
            int sigStartPos = code.IndexOf($"⨋{p}≪", index);
            if (sigStartPos < 0) break;

            int bracketCount = 0;
            int pos = sigStartPos + "⨋for≪".Length;
            bool closed = false;

            while (pos < code.Length)
            {
                char c = code[pos];

                if (c == '≪') bracketCount++;
                else if (c == '≫')
                {
                    if (bracketCount == 0)
                    {
                        // 対応する閉じ括弧発見。Sigma終端はここ+１まで
                        pos++;
                        closed = true;
                        break;
                    }
                    else
                    {
                        bracketCount--;
                    }
                }
                pos++;
            }

            if (!closed)
            {
                // 括弧対応エラー：閉じが無い
                Console.WriteLine("括弧対応エラー: 対応する ≫ が見つかりません。");
                break;
            }

            // ;を含めてSigma終了検出
            int semicolonPos = code.IndexOf(';', pos);
            if (semicolonPos < 0)
            {
                Console.WriteLine("セミコロンが見つかりません。");
                break;
            }

            int tokenEnd = semicolonPos + 1;
            string sigmaToken = code.Substring(sigStartPos, tokenEnd - sigStartPos);

            // Sigmaトークンを処理・保存
            tokens.Add(([tt.SigPrefix], sigmaToken));

            index = tokenEnd;
        }
    }

    private static readonly string classHeader = "▻",
                                StringLiteralPrefix = "⫗",
                                CharLiteralPrefix = "⨝",
                                IntLitelalPrefix = "∈",
                                LongLiteralPrefix = "∋",
                                ByteLiteralPrefix = "⊀",
                                FloatLiteralPrefix = "ʕ",
                                DoubleLiteralPrefix = "ð",
                                BoolLiteralPrefix = "⨀",
                                DecimalLiteralPrefix = "ʖ",
                                SwitchPrefix = "⨬",
                                BlockPrefix = "⩿",
                                BlockSufffix = "⪀",
                                SigmaPrefix = "≪",
                                SigmaSuffix = "≫",
                                SigPrefix = "⨋",
                                SigStart = "≪",
                                SigEnd = "≫",
                                DECPrefix = "◈";

    private static List<(int code, string errortype, string message)> warns = new();
    private static List<(tt[] type, string token)> tokens = new();

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

        // トークン化
        var tokens = Lexer.Tokenize(input);
        // 出力
       Console.WriteLine($"=== トークン一覧({tokens.Count}) ===");
       foreach (var t in tokens)
       {
           Console.WriteLine($"[{t.Type}] : [{t.Value}] at {t.Position}");
       }

        /*NotACT(input,"stut");

        // 行分割して先頭のheaderを探す
        string[] lines = input.Split(';', StringSplitOptions.RemoveEmptyEntries);
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
            tokens.Add(([tt.Header], headerLines[0].value));
        }

        // 「▻???;」行を全て削除してから解析
        foreach (var hl in headerLines)
        {
            lines[hl.idx] = ""; // 空行に
        }
        input = string.Join("\n", lines);
*/
        //        string commentPattern = @"//.*?$|/\*[\s\S]*?\*/
        //        ";
        /*        string literalPattern = @$"
                    (?:{StringLiteralPrefix}""([^""]*)"")|
                    (?:{CharLiteralPrefix}""([^""]*)"")|
                    (?:{IntLitelalPrefix}(\d+))|
                    (?:{LongLiteralPrefix}(\d+))|
                    (?:{ByteLiteralPrefix}(\d+))|
                    (?:{FloatLiteralPrefix}([\d\.]+))|
                    (?:{DoubleLiteralPrefix}([\d\.]+))|
                    (?:{BoolLiteralPrefix}(true|false|1|0))|
                    (?:{DecimalLiteralPrefix}([\d\.]+))|
                    ""([^""]*)""                          
                ";
                string VariablePattern = @$"{DECPrefix}(
                    (?:{StringLiteralPrefix})|
                    (?:{CharLiteralPrefix})|
                    (?:{IntLitelalPrefix})|
                    (?:{LongLiteralPrefix})|
                    (?:{ByteLiteralPrefix})|
                    (?:{FloatLiteralPrefix})|
                    (?:{DoubleLiteralPrefix})|
                    (?:{BoolLiteralPrefix})|
                    (?:{DecimalLiteralPrefix}))
                    ([a-z]+)";
                string SigPattern = @$"{Regex.Escape(SigPrefix)}[a-z]+{Regex.Escape(SigStart)}.*?{Regex.Escape(SigEnd)};";

                literalPattern = Regex.Replace(literalPattern, @"\s+#.*", ""); // コメント除去
                literalPattern = Regex.Replace(literalPattern, @"\s+", "");    // 改行・空白除去
                VariablePattern = Regex.Replace(VariablePattern, @"\s+#.*", ""); // コメント除去
                VariablePattern = Regex.Replace(VariablePattern, @"\s+", "");    // 改行・空白除去

                List<string>
                stringLiterals = new(),
                charLiterals = new(),
                variableNames = new();

                // コメント抽出＆除去
                string codeWithoutComments = input;
                foreach (Match m in Regex.Matches(input, commentPattern, RegexOptions.Multiline))
                {
                    codeWithoutComments = codeWithoutComments.Replace(m.Value, "");
                }

                // リテラル抽出と分類
                foreach (Match m in Regex.Matches(codeWithoutComments, literalPattern,RegexOptions.Singleline))
                {
                    int groupIdx = -1;
                    for (int i = 1; i <= 10; i++)
                    {
                        if (m.Groups[i].Success)
                        {
                            groupIdx = i;
                            break;
                        }
                    }

                    switch (groupIdx)
                    {
                        case 1: // ⫗"文字列"
                            stringLiterals.Add(m.Groups[1].Value);
                            tokens.Add(([tt.StringLiteral], m.Groups[1].Value));
                            break;
                        case 2: // ⨝"文字"
                            {
                                string val = m.Groups[2].Value;
                                if (val.Length == 1)
                                {
                                    charLiterals.Add(val);
                                    tokens.Add(([tt.CharLiteral], val));
                                }
                                else
                                    warns.Add((2, "CharLiteralError", $"{charLiterals}\"{val}\" は1文字ではありません"));
                            }
                            break;
                        case > 2 and <= 9: // ∈123, ∋123, ⊀123, ʕ1.23, ð1.23, ⨀true/false, ʖ1.23
                            tokens.Add((groupIdx switch
                            {
                                3 => [tt.IntLiteral],
                                4 => [tt.LongLiteral],
                                5 => [tt.ByteLiteral],
                                6 => [tt.FloatLiteral],
                                7 => [tt.DoubleLiteral],
                                8 => [tt.BoolLiteral],
                                9 => [tt.DecimalLiteral],
                                _ => throw new InvalidOperationException("Unexpected group index")
                            }, m.Groups[groupIdx].Value));
                            break;
                        case 10: // "通常文字列"
                            {
                                string val = m.Groups[10].Value;
                                if (val.Length >= 2)
                                {
                                    stringLiterals.Add(val);
                                    tokens.Add(([tt.StringLiteral], val));
                                }
                                else
                                    warns.Add((3, "StringLiteralError", $"\"{val}\" は2文字未満、かつ特別な接頭辞も無し"));
                            }
                            break;
                        default:
                            warns.Add((4, "LiteralError", $"不明なリテラル形式: {m.Value}"));
                            break;
                    }
                }

                // 変数宣言抽出
                foreach (Match m in Regex.Matches(codeWithoutComments, VariablePattern, RegexOptions.Singleline))
                {
                    int groupIdx = -1;
                    for (int i = 1; i <= 10; i++)
                    {
                        if (m.Groups[i].Success)
                        {
                            groupIdx = i;
                            break;
                        }
                    }

                    tokens.Add((groupIdx switch
                    {
                        1 => [tt.DECPrefix,tt.StringLiteral],
                        2 => [tt.DECPrefix,tt.CharLiteral],
                        3 => [tt.DECPrefix,tt.IntLiteral],
                        4 => [tt.DECPrefix,tt.LongLiteral],
                        5 => [tt.DECPrefix,tt.ByteLiteral],
                        6 => [tt.DECPrefix,tt.FloatLiteral],
                        7 => [tt.DECPrefix,tt.DoubleLiteral],
                        8 => [tt.DECPrefix,tt.BoolLiteral],
                        9 => [tt.DECPrefix,tt.DecimalLiteral],
                        _ => throw new InvalidOperationException("Unexpected group index")
                    }, m.Groups[groupIdx].Value));
                }
                // シグネチャ抽出
                foreach (Match m in Regex.Matches(codeWithoutComments, SigPattern, RegexOptions.Singleline))
                {
                    string sig = m.Value;
                    tokens.Add(([tt.SigPrefix], sig));
                }
                // 出力
                Console.WriteLine($"=== トークン一覧({tokens.Count}) ===");
                foreach (var (type, token) in tokens)
                {
                    string typeNames = string.Join(",", type.Select(t => Enum.GetName(typeof(tt), t)));
                    Console.WriteLine($"[[{typeNames}] : [{token}]]");
                }

                if (warns.Count > 0)
                {
                    Console.WriteLine($"\n=== 警告一覧 ==={warns.Count}");
                    foreach (var (code, errortype, message) in warns)
                    {
                        Console.WriteLine($"[{code}] {errortype}: {message}");
                    }
                }*/
    }
}
