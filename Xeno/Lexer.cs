
using System.Text.RegularExpressions;

namespace Xeno;
internal static class Lexer
{
    internal static readonly Regex tokenRegex = new Regex(@$"
        (?<Header>▻[a-zA-Z0-9_\.]+;)|
        (?<DECPrefix>[{Owen.DECPrefix}§])|
        (?<FunctionCall>[{Owen.SigPrefix}{Owen.VerSigPrefix}])|
        (?<Operator>(\+\+|--|\+|-|<|>|==|=<|>=|!|!=|\*|\/|%|\?|:|∦|⩗|\||\^))|
        (?<Symbol>=>|[₪;,\\{Owen.BlockPrefix}{Owen.BlockSufffix}{Owen.SwitchPrefix}{Owen.SigmaPrefix}{Owen.SigmaSuffix}=_])|
        (?<Mark>[""{Owen.IntLitelalPrefix}{Owen.LongLiteralPrefix}{Owen.ByteLiteralPrefix}{Owen.StringLiteralPrefix}{Owen.FloatLiteralPrefix}{Owen.DoubleLiteralPrefix}{Owen.BoolLiteralPrefix}{Owen.DecimalLiteralPrefix}])|
        (?<Comment>#.+$)|
        (?<Whitespace>[\s\t\r\n]+))|
        (?<Identifier>.+?)",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
    public static List<Token> Tokenize(string code)
    {
        var tokens = new List<Token>();
        var matches = tokenRegex.Matches(code);

        foreach (Match match in matches)
        {
            if (match.Groups["Header"].Success)
                tokens.Add(new Token(TokenType.Header, match.Value, match.Index));
            else if (match.Groups["DECPrefix"].Success)
                tokens.Add(new Token(TokenType.DECPrefix, match.Value, match.Index));
            else if (match.Groups["FunctionCall"].Success)
                tokens.Add(new Token(TokenType.FunctionCall, match.Value, match.Index)); // Function calls treated as identifiers
            else if (match.Groups["Operator"].Success)
                tokens.Add(new Token(TokenType.Operator, match.Value, match.Index)); // Whitespace ignored
            else if (match.Groups["Symbol"].Success)
                tokens.Add(new Token(TokenType.Symbol, match.Value, match.Index));
            else if (match.Groups["Mark"].Success)
                tokens.Add(new Token(TokenType.Mark, match.Value, match.Index));
            else if (match.Groups["Identifier"].Success)
                tokens.Add(new Token(TokenType.Identifier, match.Value, match.Index));
            else if (match.Groups["Comment"].Success)
                tokens.Add(new Token(TokenType.Comment, match.Value, match.Index));
            else if (match.Groups["Whitespace"].Success)
                tokens.Add(new Token(TokenType.Whitespace, match.Value, match.Index)); // Whitespace ignored
            else
                tokens.Add(new Token(TokenType.Unknown, match.Value, match.Index));
        }

        return tokens;
    }
}