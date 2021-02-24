# Introduction

The following document provides information the available features of Fry Script.

# Operators

## Mathematical operators

Fry Script supports the following mathematical operators. These operators are supported for expressions containing numerical types. Using them in expressions with non numerical types will cause an error to be thrown.

* \+ addition.
* \- subtraction.
* \* multiplication.
* \/ division.
* % modulo.

```
// evaulates to 3
1 + 2 ;

// evaluates to 4.25
1.5 + 2.75;
```

All of the mathematical operators have corresponding assignment operators.

* \+= addition assignment.
* \-= subtraction assignment.
* \*= multiplication assignment.
* \/= division assignment.

These operators must be used with an indentifier as the left hand operand.

```
// declare variable x
var x = 6;

// divides x by 2 and assigns the new value back to x
x /= 2;
```

Fry Script also supports the following operators for incrementing or decrementing a numerical value by 1.

* ++ increment.
* -- decrement.

The order these operators are applied to an operand will influence their behaviour. If they are used before the operand they will modify the value and the return it. If they are used after they will return the value and then modify it.

```
// declare variable x
var x = 0;

// increment via prefix
// this expression will return 1 and the value of x will be 1;
++x;

// declare variable y
var y = 0;

//increment via suffix
// this expression will return 0 and the value of y will become 1;
y++;
```

## Conditional operators

Fry Script supports the following conditional operators. They are used with a left and right operand, if the operands match the condition the result will be ```true``` otherwise it will be ```false```.

* == equality.
* != inequality.
* \> greater than.
* \< less than.
* \>= greater than or equal.
* \<= less than or equal.

```
// equates to true
2 == 2;

// equates to false
2 == 3;

// Equates to false
8 <= 10;
```