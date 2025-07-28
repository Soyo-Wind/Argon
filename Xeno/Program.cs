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
        string input = @"▻Fibonacci.main;

⨋for≪◈∈i=∈0;,∈i<∈31,⨋stut≪∈⨊Fibonacci≪∈i++≫≫;≫;

◈⨊∈Fibonacci≪∈n≫⩿
	⨋retn≪n<2?n:F(n-1)+F(n-2)≫;
⪀
        ";

        // トークン化
        tokens = Lexer.Tokenize(input);
        // 出力
        Console.WriteLine($"=== トークン一覧({tokens.Count}) ===");
        foreach (var t in tokens)
        {
            Console.WriteLine($"[{t.Type}] : [{t.Value}] at {t.Position}");
        }
        Console.WriteLine(input == tokens.Select(t => t.Value).Aggregate((a, b) => a + b) ? "一致" : "不一致");
        foreach (var t in tokens)
        {
            if (t.Type == TokenType.Unknown)
            {
                warns.Add((t.Position, "UnknownToken", $"不明なトークン: {t.Value} at {t.Position}"));
            }

            // 色分けして出力
            Console.ForegroundColor = t.Type switch
            {
                TokenType.Header => ConsoleColor.Cyan,
                TokenType.DECPrefix => ConsoleColor.Magenta,
                TokenType.Identifier => ConsoleColor.Green,
                TokenType.IntegerLiteral => ConsoleColor.Yellow,
                TokenType.StringLiteral => ConsoleColor.Blue,
                TokenType.Symbol => ConsoleColor.Gray,
                TokenType.Comment => ConsoleColor.DarkGray,
                TokenType.Operator => ConsoleColor.DarkYellow,
                TokenType.Whitespace => ConsoleColor.White, // Whitespace is not colored
                _ => ConsoleColor.Red
            };
            
            if (t.Type == TokenType.Unknown)warns.Add((t.Position, "UnknownToken", $"不明なトークン: {t.Value} at {t.Position}"));
            Console.Write(t.Value);
            Console.ResetColor();
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
