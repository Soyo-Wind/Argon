# mar 型 (MAcRo)

[Lisp式](./lisp式.md) のマクロを作れる

定義：

```
mar[<{戻り値}>:<{引数}>:<処理>]
```

例：

```
%hello2 = mar[<rin>:<nil>:<"hello"*2>]
```