
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Xeno;

internal static class Lexer
{
    internal static readonly Regex tokenRegex = new Regex(@$"
        (?<Header>▻[a-zA-Z0-9_\.]+;)|
        (?<DECPrefix>[{Owen.DECPrefix}§])|
        (?<FunctionCall>[{Owen.SigPrefix}{Owen.VerSigPrefix}])|
        (?<Operator>(\+\+|--|\+|-|<|>|==|=<|>=|!|!=|\*|\/|%|\?|:|∦|⩗|\||\^))|
        (?<Symbol>=>|[₪,\\{Owen.BlockPrefix}{Owen.BlockSufffix}{Owen.SwitchPrefix}{Owen.SigmaPrefix}{Owen.SigmaSuffix}=_])|
        (?<Coron>;)|
        (?<Stringer>"".+?"")|
        (?<Mark>[{Owen.IntLitelalPrefix}{Owen.LongLiteralPrefix}{Owen.ByteLiteralPrefix}{Owen.StringLiteralPrefix}{Owen.FloatLiteralPrefix}{Owen.DoubleLiteralPrefix}{Owen.BoolLiteralPrefix}{Owen.DecimalLiteralPrefix}])|
        (?<Comment>#.*$)|
        (?<Whitespace>[\s\t\r\n]+))|
        (?<Identifier>.+?)",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
    public static List<LexToken> Tokenize(string code)
    {
        var tokens = new List<LexToken>();
        var matches = tokenRegex.Matches(code);

        foreach (Match match in matches)
        {
            if (match.Groups["Header"].Success)
                tokens.Add(new LexToken(LexTokenType.Header, match.Value, match.Index));
            else if (match.Groups["DECPrefix"].Success)
                tokens.Add(new LexToken(LexTokenType.DECPrefix, match.Value, match.Index));
            else if (match.Groups["FunctionCall"].Success)
                tokens.Add(new LexToken(LexTokenType.FunctionCall, match.Value, match.Index)); // Function calls treated as identifiers
            else if (match.Groups["Operator"].Success)
                tokens.Add(new LexToken(LexTokenType.Operator, match.Value, match.Index)); // Whitespace ignored
            else if (match.Groups["Symbol"].Success)
                tokens.Add(new LexToken(LexTokenType.Symbol, match.Value, match.Index));
            else if (match.Groups["Mark"].Success)
                tokens.Add(new LexToken(LexTokenType.Mark, match.Value, match.Index));
            else if (match.Groups["Identifier"].Success)
                tokens.Add(new LexToken(LexTokenType.Identifier, match.Value, match.Index));
            else if (match.Groups["Comment"].Success)
                tokens.Add(new LexToken(LexTokenType.Comment, match.Value, match.Index));
            else if (match.Groups["Whitespace"].Success)
                tokens.Add(new LexToken(LexTokenType.Whitespace, match.Value, match.Index)); // Whitespace ignored
            else if (match.Groups["Stringer"].Success)
                tokens.Add(new LexToken(LexTokenType.Stringer, match.Value, match.Index)); // String literals
            else if (match.Groups["Coron"].Success)
                tokens.Add(new LexToken(LexTokenType.Symbol, match.Value, match.Index)); // Semicolon treated as a symbol
        }

        // Identifier連結処理
        var mergedTokens = new List<LexToken>();
        int i = 0;
        while (i < tokens.Count)
        {
            if (tokens[i].Type == LexTokenType.Identifier)
            {
                int startPos = tokens[i].Position;
                string mergedValue = tokens[i].Value ?? "";
                int j = i + 1;
                while (j < tokens.Count && tokens[j].Type == LexTokenType.Identifier && tokens[j].Position == tokens[j - 1].Position + (tokens[j - 1].Value ?? "").Length)
                {
                    mergedValue += tokens[j].Value;
                    j++;
                }
                mergedTokens.Add(new LexToken(LexTokenType.Identifier, mergedValue, startPos));
                i = j;
            }
            else
            {
                mergedTokens.Add(tokens[i]);
                i++;
            }
        }

        return mergedTokens;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void LexError(List<LexToken> tokens)
    {
        if (tokens.FindAll(t => t.Type == LexTokenType.Header).Count() > 1)
        {
            Owen.warns.Add((1, "MultipleHeaders", "複数のヘッダーが検出されました。"));
        }
    }
}