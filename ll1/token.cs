
using System.Dynamic;

namespace ll1;

class varEx
{
    internal varEx(string name, VariableType type, string[] values)
    {
        Name = name;
        Type = type;
        Values = values;
    }

    internal varEx get() => this;
    string Name;
    VariableType Type;
    string[] Values;
}

enum VariableType
{
    rin,vid,tnil,bin,teg,cim,flo
}