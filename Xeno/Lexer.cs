
using System.Text.RegularExpressions;

namespace Xeno;
internal static class Lexer
{
    private static readonly Regex tokenRegex = new Regex(@"
        (?<Header>▻[a-zA-Z0-9_\.]+;)|
        (?<DECPrefix>[◈§⫗∈∋⊀ʕð⨀ʖ])|
        (?<Identifier>[a-zA-Z_][a-zA-Z0-9_]*)|
        (?<IntLiteral>\d+(\.\d+)?)|
        (?<StringLiteral>⫗""[^""]*"")|
        (?<Symbol>[;,\(\)⩿⪀⨬≪≫])|
        (?<Comment>//.*?$|/\*[\s\S]*?\*/)",
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
            else if (match.Groups["Identifier"].Success)
                tokens.Add(new Token(TokenType.Identifier, match.Value, match.Index));
            else if (match.Groups["IntLiteral"].Success)
                tokens.Add(new Token(TokenType.IntLiteral, match.Value, match.Index));
            else if (match.Groups["StringLiteral"].Success)
                tokens.Add(new Token(TokenType.StringLiteral, match.Value, match.Index));
            else if (match.Groups["Symbol"].Success)
                tokens.Add(new Token(TokenType.Symbol, match.Value, match.Index));
            else if (match.Groups["Comment"].Success)
            {
                // コメントは無視、あるいは必要ならトークン化
            }
            else
                tokens.Add(new Token(TokenType.Unknown, match.Value, match.Index));
        }

        return tokens;
    }
}
