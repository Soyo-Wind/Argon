# teg 型 (inTEGer)

[eval](./eval関数.md) を使って評価します.

定義：

```
teg[<値>]
```

>[!TIP]
>戻り値をnilに設定すると、戻り値を返さないvid型になります.
>引数も同じようにnilに設定すると引数なしと判断されます.

例：

```
%one = teg[<1>]
eval[%one] ; 1 (teg)
```
