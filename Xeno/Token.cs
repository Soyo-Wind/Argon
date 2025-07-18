using System.Numerics;
namespace Xeno;

internal class Sentence
{
}

internal class String : Expression
{
    public String(string value) : base(value)
    {
    }
}

internal class Num : Expression
{
    public Num(object value) : base(value)
    {
    }
}

internal class Char : Expression
{
    public char Value;
    public Char(char value) : base(value)
    {
    }
}

internal class Bool : Expression
{
    public bool Value;
    public Bool(bool value) : base(value)
    {
    }
}

internal class Formula
{
    public object Value;

    public Formula(object value)
    {
        Value = value;
    }
}

internal class Biter
{
    public object Value;

    public Biter(object value)
    {
        Value = value;
    }
}

internal class Floater : Expression
{
    public Floater(object value) : base(value){}
}

internal class Changer
{
    public object Value;

    public Changer(object value)
    {
        Value = value;
    }
}

public enum VarType
{
    Bool = '⨀',
    Byte = '⊀',
    Int = '∈',
    Long = '∋',
    Bigint = '∝',
    Float = 'ʕ',
    Double = 'ð',
    Decimal = 'ʖ',
    Char = '⨝',
    Str = '⫗'
}

internal class ADD
{
    public object Value;

    public ADD(object left, object right, VarType type)
    {
        Value = type switch
        {
            VarType.Byte => (byte)left + (byte)right,
            VarType.Int => (int)left + (int)right,
            VarType.Long => (long)left + (long)right,
            VarType.Bigint => (BigInteger)left + (BigInteger)right, // Assuming Bigint is treated as long
            VarType.Float => (float)left + (float)right,
            VarType.Double => (double)left + (double)right,
            VarType.Decimal => (decimal)left + (decimal)right,
            VarType.Char => (string)left + right,
            VarType.Str => left + (string)right,
            _ => throw new InvalidOperationException("Unsupported VarType for addition")
        };
    }
}

internal class MINUS
{
    public object Value;

    public MINUS(object left, object right, VarType type)
    {
        Value = type switch
        {
            VarType.Byte => (byte)left - (byte)right,
            VarType.Int => (int)left - (int)right,
            VarType.Long => (long)left - (long)right,
            VarType.Bigint => (BigInteger)left - (BigInteger)right,
            VarType.Float => (float)left - (float)right,
            VarType.Double => (double)left - (double)right,
            VarType.Decimal => (decimal)left - (decimal)right,
            _ => throw new InvalidOperationException("Unsupported VarType for subtraction")
        };
    }
}

internal class MUL
{
    public object Value;

    public MUL(object left, object right, VarType type)
    {
        Value = type switch
        {
            VarType.Byte => (byte)left * (byte)right,
            VarType.Int => (int)left * (int)right,
            VarType.Long => (long)left * (long)right,
            VarType.Bigint => (BigInteger)left * (BigInteger)right,
            VarType.Float => (float)left * (float)right,
            VarType.Double => (double)left * (double)right,
            VarType.Decimal => (decimal)left * (decimal)right,
            VarType.Char => Tools.mults((string)left,(long)right),
            VarType.Str => Tools.mults((string)left , (long)right),
            _ => throw new InvalidOperationException("Unsupported VarType for multiplication")
        };
    }
}

internal class DIV
{
    public object Value;

    public DIV(object left, object right, VarType type)
    {
        Value = type switch
        {
            //VarType.Byte => (byte)left / (byte)right,
            //VarType.Int => (int)left / (int)right,
            //VarType.Long => (long)left / (long)right,
            //VarType.Bigint => (BigInteger)left / (BigInteger)right,
            VarType.Float => (float)left / (float)right,
            VarType.Double => (double)left / (double)right,
            VarType.Decimal => (decimal)left / (decimal)right,
            _ => throw new InvalidOperationException("Unsupported VarType for division")
        };
    }
}

public class Expression
{
    public object Value;
    public Expression(object value)
    {
        Value = value;
    }
}
