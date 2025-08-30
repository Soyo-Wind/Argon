# rin 型 (stRINg)

[expr](./expr.md) を使って評価します.

定義：

```
rin[<値(ダブルクォーテーションで囲む)>]
```

>[!TIP]
> Return文字などを表示する場合は、[エスケープシーケンス](エスケープシーケンス.md)を使います(Enterやベルなど).

例：

```
%hello = rin[<"hello">]
expr[%hello] ; "hello"
```

#型
