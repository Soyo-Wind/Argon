# Sample Program

## FizzBuzz

```
▻FizzBuzz.main;
⨋for≪◈∈i = ∈1;, ∈i <= ∈100,
	⨋stut≪∈i%15⨬⫗ ⩿
		∈₪ == ∈0 => "FizzBuzz",
		 ∈₪ % ∈3 == ∈0 => "Fizz",
		  ∈₪ % ∈5 == ∈0 => "Buzz",
		   ∈_ => ∈i
		⪀
	≫;
≫;
```

### minify

```
⨋for≪◈∈i=∈0;,∈i<∈99,⨋stut≪(∈++i%∈3<∈1?"Fizz":⫗"")+(∈i%∈5<∈1?"Buzz":∈i%∈3<∈1?"":∈i)≫;≫;
```
