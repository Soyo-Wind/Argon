
using System.Text;

namespace ll1;

class NEx
{
    public static string numval(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr))
            throw new ArgumentException("空の式です");
        if (expr[0] != '(')
            return expr; // 数値だけならそのまま返す
        int pos = 0;
        return parse(expr, ref pos);
    }

    // 再帰でパース
    private static string parse(string expr, ref int pos)
    {
        // スペースとカッコをスキップ
        for (; pos < expr.Length && (expr[pos] == ' ' || expr[pos] == '('); pos++) ;

        // 演算子を抽出
        StringBuilder opBuilder = new();
        while (pos < expr.Length && expr[pos] != ' ' && expr[pos] != '(' && expr[pos] != ')')
        {
            opBuilder.Append(expr[pos]);
            pos++;
        }

        string op = opBuilder.ToString();

        string csOp = op switch
        {
            "+" => "+",
            "-" => "-",
            "*" => "*",
            "/" => "/",
            "mod" => "%",
            "and" => "&",
            "or" => "|",
            "not" => "!",
            "xor" => "^",
            "eand" => "&&",
            "eor" => "||",
            "eq" => "==",
            "neq" => "!=",
            ">" => ">",
            "<" => "<",
            ">=" => ">=",
            "=<" => "<=",
            _ => throw new ArgumentException("未対応演算子: " + op),
        };

        // オペランド処理
        List<string> operands = new();
        while (true)
        {
            // スペースや閉じカッコをスキップ
            while (pos < expr.Length && (expr[pos] == ' ')) pos++;
            if (pos >= expr.Length || expr[pos] == ')')
            {
                ++pos; // 閉じカッコ消費
                break;
            }

            if (expr[pos] == '(')
            {
                ++pos; // 開きカッコ消費
                operands.Add(parse(expr, ref pos));
            }
            else
            {
                StringBuilder numBuilder = new StringBuilder();
                while (pos < expr.Length &&
                       expr[pos] != ' ' &&
                       expr[pos] != ')')
                {
                    numBuilder.Append(expr[pos]);
                    pos++;
                }
                operands.Add(numBuilder.ToString());
            }
        }

        // オペランドを中置記法で連結
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < operands.Count; i++)
        {
            builder.Append(operands[i]);
            if (i < operands.Count - 1)
            {
                builder.Append(" " + csOp + " ");
            }
        }
        // 複雑な式なら括弧を付与
        if (operands.Count > 1)
            return "(" + builder.ToString() + ")";
        else
            return builder.ToString();
    }
}