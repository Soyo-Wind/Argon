# if式

ある条件によって処理を分けるには[if文](https://programming.pc-note.net/csharp/if.html)や[switch文](https://programming.pc-note.net/csharp/switch.html)を使用します.

```
static void Main(string[] args) {     string str1 = "abc";     string str2 = "abcde";     bool bo = str1.Length > str2.Length;      int num = bo ? str1.Length : str2.Length;      Console.WriteLine("str1とstr2の最大サイズは " + num);      Console.ReadLine(); }
```

str1とstr2の最大サイズは 5

7行目の`**?**`と`**:**`が条件演算子です。  
この二つはセットで使用されます。  
`+`や`=`などの演算子は式を二つ(右辺と左辺、つまり二項)を必要とするのに対し、この条件演算子は式を三つ必要とするので**三項演算子**とも呼ばれます。  
C#では三項演算子はこの条件演算子しかないため、どちらで呼んでも通じます。

WC`val = x ? a : b;`

三項条件演算子は、「条件式x」が真ならば「式a」を、偽ならば「式b」を実行し、その評価値を返します。

上記のサンプルコードはわかりやすくするためbool型変数を使用していますが、以下のように条件式に直接書いても同じです。

WC`static void Main(string[] args) {     string str1 = "abc";     string str2 = "abcde";      int num = str1.Length > str2.Length ? str1.Length : str2.Length;      Console.WriteLine("str1とstr2の最大サイズは " + num);      Console.ReadLine(); }`

### if文との違い

条件演算子はすべてif文で同等の処理に書き換えることができますが、if文はすべて条件演算子で書き換えられるというわけではありません。  
条件演算子は必ず何か値を返す必要があります。  
つまり戻り値がvoid型のメソッド(式)を実行することはできません。

WC`string str1 = "abc"; string str2 = "abcde";  //エラー int num = str1.Length > str2.Length ?     Console.WriteLine("str1の方が大きい") :     Console.WriteLine("str2の方が大きいか等しい");  //代入しなくてもエラー str1.Length > str2.Length ?     Console.WriteLine("str1の方が大きい") :     Console.WriteLine("str2の方が大きいか等しい");`

条件演算子は正確に言えば「条件分岐」ではなく、「条件によって返す値を変える」という処理を行います。  
そのような場合にif文よりも簡潔な記述ができる手段がある、という程度の認識で良いでしょう。

### 条件演算子の入れ子

条件演算子は以下のように一行に連続して記述することも可能です。

WC`static void Main(string[] args) {     string str1 = "abc";     string str2 = "abcde";     string str3 = "ab";      int num =         str1.Length > str2.Length ?             str1.Length > str3.Length ?                 str1.Length : str3.Length :             str2.Length > str3.Length ?                 str2.Length : str3.Length;      Console.ReadLine(); }`

途中で改行をしているので複数行に見えますが、プログラムコード上は一行の処理です。