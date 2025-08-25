
using System.Runtime.CompilerServices;
using System.Text;

namespace ll1;

class VEx
{
    public static void codex(string[] lines)
    {
        foreach (string line in lines)
        {
            StringBuilder name = new();
            int pos = 0;
            SkipSpace(line, ref pos);
            if (pos >= line.Length)
                continue;
            if (line[pos] == '%')
            {
                List<StringBuilder> values = new();
                VariableType type;
                string _ = "";
                SkipSpace(line, ref pos);
                for (; pos < line.Length && line[pos] != '='; name.Append(line[pos]), pos = pos >= line.Length ?
                throw new Exception("構文違い(変数)") : pos + 1) ;
                if (line[pos] != '=')
                    throw new Exception("構文違い(=がない)");
                for (; pos < line.Length && line[pos] != '['; _ += line[pos], pos = pos >= line.Length ?
                throw new Exception("構文違い(変数)") : pos + 1) ;
                if (line[pos] != '[')
                    throw new Exception("構文違い([がない)");
                type = _[1..] switch
                {
                    "rin" => VariableType.rin,
                    "vid" => VariableType.vid,
                    "tnil" => VariableType.tnil,
                    "bin" => VariableType.bin,
                    "teg" => VariableType.teg,
                    "cim" => VariableType.cim,
                    "flo" => VariableType.flo,
                    _ => throw new Exception("不明な型"),
                };
                if (type == VariableType.vid)
                {
                    for (; pos < line.Length && line[pos] != ']';)
                    {
                        StringBuilder val = new();
                        SkipSpace(line, ref pos);

                        if (line[pos] == '<')
                        {
                            pos++;
                            for (; pos < line.Length && line[pos] != '>'; val.Append(line[pos]), pos = pos >= line.Length ?
                            throw new Exception("構文違い(値)") : pos + 1) ;
                            if (line[pos] != '>')
                                throw new Exception("構文違い(>がない)");
                            pos++;
                        }
                        values.Add(val);
                        SkipSpace(line, ref pos);
                        if (line[pos] == ',')
                            pos++;
                    }
                    if (line[pos] != ']')
                        throw new Exception("構文違い(]がない)");
                }
                else
                {
                    StringBuilder val = new();
                    SkipSpace(line, ref pos);
                    if (line[pos] != '[')
                        throw new Exception("構文違い([がない)");
                    else
                    {
                        pos++;
                        SkipSpace(line, ref pos);
                        if (line[pos] == '<')
                        {
                            pos++;
                            for (; pos < line.Length && line[pos] != '>'; val.Append(line[pos]), pos = pos >= line.Length ?
                            throw new Exception("構文違い(値)") : pos + 1) ;
                            if (line[pos] != '>')
                                throw new Exception("構文違い(>がない)");
                            pos++;
                        }
                        else throw new Exception("構文違い(<がない)");
                        values.Add(val);
                        SkipSpace(line, ref pos);
                        if (line[pos] == ',')
                            pos++;
                    }
                }
                Owen.addCode(type switch
                {
                    VariableType.rin => $"string {name} = {values[0]};",
                    VariableType.vid => $"internal static {(values[0].ToString() == "nil"?"void":
                    values[0].ToString() switch
                    {
                        "rin" => "string",
                        "tnil" => "bool",
                        "bin" => "BigInteger",
                        "teg" => "int",
                        "cim" => "decimal",
                        "flo" => "float",
                        _ => throw new Exception("不明な型"),
                    })} {name}(){{ {string.Join(", ", values)} }};",
                    VariableType.tnil => $"bool {name} = {values[0]};",
                    VariableType.bin => $"BigInteger {name} = {values[0]};",
                    VariableType.teg => $"int {name} = {values[0]};",
                    VariableType.cim => $"decimal {name} = '{values[0]}';",
                    VariableType.flo => $"float {name} = {values[0]}f;",
                    _ => throw new Exception("不明な型"),
                });
            }
        }
    }
    
    private static void SkipSpace(string expr, ref int pos)
    {
        for (; pos < expr.Length && expr[pos] == ' '; pos++) ;
    }
}