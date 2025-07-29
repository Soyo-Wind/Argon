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

    internal static List<(int code, string errortype, string message)> warns = new();
    private static List<LexToken> tokens = new();

    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            Console.WriteLine(DelWhite.RemoveWhitespace(Lexer.tokenRegex.ToString()));
            return;
        }
        string input = @"▻FizzBuzz.main;
⨋for≪◈∈i = ∈1;, ∈i =< ∈100,
	⨋stut≪∈i%15⨬⫗ ⩿
		∈₪ == ∈0 => ""FizzBuzz"",
		 ∈₪ % ∈3 == ∈0 => ""Fizz"",
		  ∈₪ % ∈5 == ∈0 => ""Buzz"",
		   ∈_ => ∈i
		⪀
	≫;
≫;";

        // トークン化
        tokens = Lexer.Tokenize(input);

        // 出力
        Console.WriteLine($"=== トークンカウント({tokens.Count}) ===");
        foreach (var t in tokens)
        {

            // 色分けして出力
            Console.ForegroundColor = t.Type switch
            {
                LexTokenType.Header => ConsoleColor.White,
                LexTokenType.DECPrefix => ConsoleColor.Magenta,
                LexTokenType.Identifier => ConsoleColor.Gray,
                LexTokenType.Symbol or LexTokenType.Coron => ConsoleColor.Blue,
                LexTokenType.Comment or LexTokenType.Whitespace => ConsoleColor.DarkGreen,
                LexTokenType.Operator => ConsoleColor.Yellow,
                LexTokenType.Mark or LexTokenType.FunctionCall => ConsoleColor.Cyan,
                LexTokenType.Stringer => ConsoleColor.DarkRed,
                _ => ConsoleColor.Black // Unknown or other types
            };
            Console.Write(t.Value);
            Console.ResetColor();
        }

        Lexer.LexError(tokens);
        if (warns.Count > 0)
        {
            Console.WriteLine($"=== 警告一覧({warns.Count}) ===");
            foreach (var warn in warns)
            {
                Console.WriteLine($"[ RG{warn.code} ] [{warn.errortype}] : {warn.message}");
            }
        }
    }
}
