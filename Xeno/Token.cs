
namespace Xeno;
internal class Token
{

    public Token(TokenType type, string? value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }
    public TokenType Type { get; set; }
    public string? Value { get; set; }
    public int Position { get; set; }
}
internal enum TokenType
{
    Header,
    DECPrefix,
    Identifier,
    Symbol,
    Comment,
    Unknown,
    Operator,
    Whitespace,
    Mark, // Added for literal prefixes
    FunctionCall, // Function calls treated as identifiers
}