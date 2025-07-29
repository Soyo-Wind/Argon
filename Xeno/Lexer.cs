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
        (?<Symbol>=>|[₪.(),\\{Owen.BlockPrefix}{Owen.BlockSufffix}{Owen.SwitchPrefix}{Owen.SigmaPrefix}{Owen.SigmaSuffix}=_])|
        (?<Coron>;)|
        (?<Stringer>"".*?"")|
        (?<Mark>[{Owen.IntLitelalPrefix}{Owen.LongLiteralPrefix}{Owen.ByteLiteralPrefix}{Owen.StringLiteralPrefix}{Owen.FloatLiteralPrefix}{Owen.DoubleLiteralPrefix}{Owen.BoolLiteralPrefix}{Owen.DecimalLiteralPrefix}])|
        (?<Comment>#.*$)|
        (?<Whitespace>[\s\t\r\n]+))|
        (?<Identifier>[a-zA-Z0-9]+)",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
    public static List<LexToken> Tokenize(string code)
    {
        var tokens = new List<LexToken>();
        var matches = tokenRegex.Matches(code);

        foreach (Match match in matches)
        {
            var type =
                match.Groups["Header"].Success        ? LexTokenType.Header :
                match.Groups["DECPrefix"].Success     ? LexTokenType.DECPrefix :
                match.Groups["FunctionCall"].Success  ? LexTokenType.FunctionCall :
                match.Groups["Operator"].Success      ? LexTokenType.Operator :
                match.Groups["Symbol"].Success        ? LexTokenType.Symbol :
                match.Groups["Mark"].Success          ? LexTokenType.Mark :
                match.Groups["Identifier"].Success    ? LexTokenType.Identifier :
                match.Groups["Comment"].Success       ? LexTokenType.Comment :
                match.Groups["Whitespace"].Success    ? LexTokenType.Whitespace :
                match.Groups["Stringer"].Success      ? LexTokenType.Stringer :
                match.Groups["Coron"].Success         ? LexTokenType.Symbol :
                LexTokenType.Unknown;

            tokens.Add(new LexToken(type, match.Value, match.Index));
        }
        return tokens;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void LexError(List<LexToken> tokens)
    {
        if (tokens.FindAll(t => t.Type == LexTokenType.Header).Count() > 1)
        {
            Owen.warns.Add((1, "MultipleHeaders", "複数のヘッダーが検出されました。"));
        }
        
        foreach (LexToken c in tokens.Where(t => t.Type == LexTokenType.Unknown).ToArray())
        {
            Owen.warns.Add((0, "UnknownToken", $"[不明なトークンが検出されました。\n{c.Value} at position {c.Position}]"));
        }
    }
}