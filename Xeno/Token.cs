
namespace Xeno;

internal class LexToken
{

    public LexToken(LexTokenType type, string? value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }
    public LexTokenType Type { get; set; }
    public string? Value { get; set; }
    public int Position { get; set; }
}

internal enum LexTokenType
{
    Header,
    DECPrefix,
    Identifier,
    Symbol,
    Comment,
    Operator,
    Whitespace,
    Mark, // Added for literal prefixes
    FunctionCall, // Function calls treated as identifiers
    Stringer, // For string/Char literals
    Coron, // For semicolon and other specific symbols
    Unknown
}
