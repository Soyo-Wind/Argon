#Touser 
# Sample Program

## FizzBuzz

``` scheme
(field (<int>i='(1))
	(while (< (car i) 100) 
		(field (<int>x=(% i 15))
			(stut (+
				(if (= x 0) "FizzBuzz"
				(if (= (% x 3) 0) "Fizz"
				(if (= (% x 5) 0) "Buzz" i))) "\n")))))
```

