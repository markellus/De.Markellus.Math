
# Markellus Advanced Math Library

This library provides a set of advanced mathematical functions.


## General

Most functionality can be easily accessed over static methods.


## Implemented Functionality

### The Math Environment [WIP]
* Working with mathematical expressions
```csharp
MathEnvironment env = new MathEnvironment();
env.DefineVariable("x", "1.23");
env.DefineVariable("y", "9");
Node term = env.AddTerm("x * sqrt(y) / 2");
Console.WriteLine(term.Simplify());
Console.WriteLine(term.Resolve());
``` 

### Input Parsing
* InputParser.ParseDoubleList(string[])
	* Converts a list of strings into a list of doubles.

* InputParser.TokenizeTerm()
	* Splits a mathematical term into tokens.
	* Detects the type of a token and provides additional information such as operator associativity and precedence
	* Example: InputParser.TokenizeTerm("100 + 5 * sqrt(9)) -> Token[] {100,+,5,*,sqrt,(,9,)}

* InputParse.TokenizeTermToRpn()
	* Splits a mathematical term into tokens in reverse polish notation with **Dijkstra's shunting yard algorithm**
	* Detects the type of a token and provides additional information such as operator associativity and precedence

### Arithmetics
* The real datatype
	* Provides a datatype for real numbers.
	* No precision loss as long as you have enough RAM to fit all decimals
	* Uses the [spigot exact real calculator](https://www.chiark.greenend.org.uk/~sgtatham/spigot/) with added support for multithreading and automatic detection of repeating decimals

### Statistics
* arithmetic mean
* geometric mean
* modal mean
* five point summary


## System Requirements
* .NET Standard 2.1 compatible system

### Additional requirements
* The usage of the real datatype requires a 64 Bit CPU and Microsoft Windows (for now)

## License

Copyright (c) 2019 Marcel Bulla.

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 3.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See ** LICENSE.MD ** for more details.

## Third Party Licenses

### [spigot exact real calculator](https://www.chiark.greenend.org.uk/~sgtatham/spigot/)
spigot is copyright 2007-2019 Simon Tatham. All rights reserved.

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.