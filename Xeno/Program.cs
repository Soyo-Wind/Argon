namespace Xeno;


class Owen
{
    internal static readonly string classHeader = "▻",
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
                                VerSigPrefix = "⨊",
                                DECPrefix = "◈";

    private static List<(int code, string errortype, string message)> warns = new();
    private static List<Token> tokens = new();

    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            Console.WriteLine(Lexer.tokenRegex.ToString()
            .Replace("\"", "\"\"")
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace("\t",""));
            return;
        }
        string input = @"▻FizzBuzz.main;
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
        tokens = Lexer.Tokenize(input);
        // 出力
        Console.WriteLine($"=== トークンカウント({tokens.Count}) ===");
        Console.WriteLine(input == tokens.Select(t => t.Value).Aggregate((a, b) => a + b) ? "=== 一致 ===" : "=== 不一致 ===");
        foreach (var t in tokens)
        {
            if (t.Type == TokenType.Unknown)
            {
                warns.Add((t.Position, "UnknownToken", $"不明なトークン: {t.Value} at {t.Position}"));
            }

            // 色分けして出力
            Console.ForegroundColor = t.Type switch
            {
                TokenType.Header => ConsoleColor.White,
                TokenType.DECPrefix => ConsoleColor.Magenta,
                TokenType.Identifier => ConsoleColor.Green,
                TokenType.Symbol => ConsoleColor.Blue,
                TokenType.Comment or TokenType.Whitespace => ConsoleColor.Gray,
                TokenType.Operator => ConsoleColor.Yellow,
                TokenType.Mark or TokenType.FunctionCall => ConsoleColor.Cyan,
                _ => ConsoleColor.Red
            };
            
            if (t.Type == TokenType.Unknown)warns.Add((t.Position, "UnknownToken", $"不明なトークン: {t.Value} at {t.Position}"));
            Console.Write(t.Value);
            Console.ResetColor();
        }

        if (tokens.FindAll(t => t.Type == TokenType.Header).Count > 1)
        {
            warns.Add((0, "MultipleHeaders", "複数のヘッダーが検出されました。"));
        }

        if (warns.Count > 0)
        {
            Console.WriteLine($"=== 警告一覧({warns.Count}) ===");
            foreach (var warn in warns)
            {
                Console.WriteLine($"[{warn.code}] [{warn.errortype}] : {warn.message}");
            }
        }
    }
}
