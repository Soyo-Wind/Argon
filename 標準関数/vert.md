# vert 関数 (conVERT)

型変換

定義：
```
$vert({型(obj)},{値})
```

型変換できるもの：

| ↓変換前・変換後→ | cim | ject | lst | rin | teg | tnil | lst(rin) |
| --------- | --- | ---- | --- | --- | --- | ---- | -------- |
| cim       | /   | O    | X   | O   | X   | X    | O        |
| ject      | X   | /    | X   | X   | X   | X    | X        |
| lst       | X   | O    | /   | X   | X   | X    | ?        |
| rin       | X   | O    | X   | /   | X   | X    | O        |
| teg       | ?   | O    | X   | O   | /   | X    | O        |
| tnil      | O   | O    | O   | O   | O   | /    | O        |
| lst(rin)  | X   | O    | X   | O   | X   | X    | /        |

#関数 
