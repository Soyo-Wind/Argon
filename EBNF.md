```ebnf
<program>       ::= { <statement> }

<statement>     ::= <variable_declaration> "/"
                  | <expression> "/"
                  | <function_declaration> "/"
                  | <comment>

<variable_declaration> ::= <variable> "=" <typed_value>

<typed_value>   ::= <type> "[" "<" <value> ">" "]"
                  | <expression>

<function_declaration> ::= <variable> "=" "vid" "[" "<" <type> ">" ":" "<" <argument_list_or_nil> ">" ":" "<" <expression> ">" "]"

<argument_list_or_nil> ::= "nil" | <type> { "," <type> }

<expression>    ::= <s_expression> | <variable> | <literal>

<s_expression>  ::= "(" <symbol> { <expression> } ")"

<variable>      ::= "%" <identifier>

<type>          ::= "teg" | "rin" | "bin" | "tnil" | "flo" | "cim" | "vid"

<literal>       ::= <number> | <string> | "t" | "nil" | "null"

<identifier>    ::= <letter> { <letter> | <digit> | "_" }

<number>        ::= <digit> { <digit> }

<string>        ::= "\"" { <character_except_quote_or_escape> | <escape_sequence> } "\""

<symbol>        ::= <identifier>   ; 関数や演算子名

<comment>       ::= ";" { <any_character_except_newline> } <newline>
				   |";;" { <any_character_except_newline> } <newline>
				   |";;;" { <any_character_except_newline> } <newline>
				   |";;;;" { <any_character_except_newline> } <newline>

<letter>        ::= "A" | ... | "Z" | "a" | ... | "z"
<digit>         ::= "0" | ... | "9"
<newline>       ::= "\n" | "\r\n"

```