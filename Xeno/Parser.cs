
namespace Xeno;

internal class Parser
{
    internal static string? className;
    internal Parser(List<LexToken> tokens)
    {
        List<LexToken> parsedTokens = new();

        className = (
            tokens.Where(t => t.Type == LexTokenType.Header)
                  .Select(t => t.Value)
                  .FirstOrDefault() ?? " main ")[1..];

        List<LexToken>[] sentence = tokens
            .Select((t, i) => new { Token = t, Index = i })
            .GroupBy(x => tokens.Take(x.Index + 1).Count(t => t.Type == LexTokenType.Coron))
            .Select(g => g.Select(x => x.Token).ToList())
            .Where(list => list.Count > 0)
            .ToArray();

        foreach (var s in sentence)
        {
        }
    }
}