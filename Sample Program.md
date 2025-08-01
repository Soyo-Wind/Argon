#Touser 
# Sample Program

## FizzBuzz

```
▻FizzBuzz.main;
⨋for≪◈∈i = ∈1;, ∈i <= ∈100,
	⨋stut≪∈i%15⨬⫗ ⩿
		∈₪ == ∈0 => "FizzBuzz\n",
		 ∈₪ % ∈3 == ∈0 => "Fizz\n",
		  ∈₪ % ∈5 == ∈0 => "Buzz\n",
		   ∈_ => ∈i
		⪀
	≫;
≫;
```

### minify

```
⨋for≪◈∈i=∈0;,∈i<∈99,⨋stut≪(∈++i%∈3<∈1?"Fizz\n":⫗"")+(∈i%∈5<∈1?"Buzz\n":∈i%∈3<∈1?⫗"":∈i)≫;≫;
```

## Fibonacci

```
▻Fibonacci.main;

⨋for≪◈∈i=∈0;,∈i<∈31,⨋stut≪∈⨊Fibonacci≪∈i++≫≫;≫;

◈⨊∈Fibonacci≪∈n≫⩿
	⨋retn≪∈n<∈2?∈n:∈⨊Fibonacci≪∈n-∈1≫+∈⨊Fibonacci≪∈n-∈2≫≫;
⪀
```

### minify

```
⨋for≪◈∈i=∈0;,∈i<∈31,⨋stut≪∈⨊F≪∈i++≫≫;≫;◈⨊∈F≪∈n≫⩿⨋retn≪∈n<∈2?∈n:∈⨊F≪∈n-∈1≫+∈⨊F≪∈n-∈2≫≫;⪀
```
