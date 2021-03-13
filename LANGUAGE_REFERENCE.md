# Introduction

The following document provides information about the available features of Fry Script.

# Primitive types
Fry Script supports the following primitive types.

## Int32
A 32 bit integer. Can be used in scripts via numeric literals and also imported for the purposes of type comparison.

```
// import the int 32 type into a script
@import "int" as int;

// numeric literals examples
100;
500;
```

## Float
A single-precision floating point number. Can be used in scripts via float literals defined using a decimal point and also imported for the purposes of type comparison.

```
// import the float type into a script
@import "float" as float;

// float literal examples
1.5;
99.9;
```

## Boolean
A true or false value. Can be used in scripts via boolean literals ```true``` and ```false``` and also imported for the purposes of type comparison.

```
// import the boolean type into a script
@import "boolean" as bool;

// boolean literal examples
true;
false;
```

## String
A string of characters. Can be used in scripts via string literals represented by a collection of words enclosed in double quotes, and also imported for the purposes of type comparison;

```
// import the string type into a script
@import "string" as string;

// string literal examples
"i am a string";
"yes you are!"
```

## Object
A grouping of related data and functionality. Can be used in scripts via object literals that contain named properties, and also imported for the purposes of type comparison. An object literal defines properties as a collection of key value pairs seperated be a colon, with each pair seperated by a comma.

```
// import the object type into a script
@import "object" as object;

// object literal examples

// empty object
{};

// object with properties
{
    name: "The obeject",
    power: 100
};
```

## Array
A collection of items. Can be used in scripts via array literals and also imported for the purposes of type comparison.

```
// import the array type into a script
@import "array" as array;

// array literal examples

// empty array
[];

// array with items
[1, 2, 3, 4];

```

## Function
A block of reusable code than can be executed, functions can accept arguments that can be operated on by the function. Can be used in scripts via function literals and also imported for the purposes of type comparison.

```
// import the function type into a script
@import "function" as function;

// function literal examples.

// empty function
() => {};

// empty functiom with a single argument
arg => {};

// empty function with two arguments
(arg1, arg2) => {};

// function with a single argument and a return value
arg1 => arg1 + 1;

// function with body block with two arguments and a return value
(arg1, arg2) => {
    return arg1 + arg2;
};
```

## Fibre
A block of reusable code that can be executed, fibres can accept arguments that can be operated on by the fibre. Cab be used in scripts via fibre literals and also imported for the purposes of type comparison.
```
// import the fibre type into a script
@import "fibre" as f;

// fibre literal examples

// empty fibre
fibre () => {};

// empty fibre with a single argument
fibre arg => {};

// fibre with body block with one argument and a yield return value
fibre arg => {
    yield return arg;
};
```
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
* %= modulo assignment.

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

## Logical operators

The following logical operators are supported. They are used with a left and right operand, both operands are typically boolean values.

* && logical and.
* || logical or.
* ! logial not.

```
// && will be true when both the left and right operands are true
condition1 = true;
condition2 = true;

// equates to true
condition1 && condition2;

// || will be true if either the left or right operands are true
condition1 = false;
condition2 = true;

// equates to true
condition1 || condition2;

// equates to false
condition1 = true;
!condition1;
```

As well as working against boolean types the logical operators will also treat ```null``` as being false.

```
// equates false
null && true;

// equates to true
null || true;
```

## Object interrogation operators

Fry Script provides the following operators for querying the type and properties of an object.

* has. 
* is.

```
@import "object" as object;

var obj = {
    name: "the object"
};

// has operator examples

// equates to true
obj has name;

// equate to false
obj has age;
```