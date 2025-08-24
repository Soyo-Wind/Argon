# vid 型 (VoID)

[eval](./eval関数.md) を使って評価します.

定義：

```
vid[<{戻り値}>:<{引数}>:<処理>]
```

>[!TIP]
>戻り値をnilに設定すると、戻り値を返さないvid型になります.
>引数も同じようにnilに設定すると引数なしと判断されます.

例：

```
%hello = vid[<str>:<nil>:<retn["hello"]>]
eval[%hello] ; "hello"
```
