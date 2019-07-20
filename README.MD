# Markellus Advanced Math Library

This library provides a set of advanced mathematical functions.


## General

Most functionality can be easily accessed over static methods.


## Implemented functions

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

### Statistics
* arithmetic mean
* geometric mean
* modal mean
* five point summary


## System Requirements
* .NET Standard 2.1 compatible system
* 64 Bit CPU


## License

Copyright (c) 2019 Marcel Bulla.

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 3.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See ** LICENSE.MD ** for more details.