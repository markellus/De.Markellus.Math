/* 
 * This file is part of De.Markellus.Math (https://github.com/markellus/De.Markellus.Math).
 * Copyright (c) 2019 Marcel Bulla.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// Der MathExpressionTokenizer teilt eine mathematische Funktion, die als ein String vorliegt, in logische Blöcke auf.
    /// </summary>
    public class MathExpressionTokenizer
    {
        /// <summary>
        /// Eine Default-Instanz dieser Klasse, die für allgemeine Zwecke verwendet werden kann.
        /// </summary>
        public static MathExpressionTokenizer Default { get; }

        /// <summary>
        /// Ein Wörterbuch mit logischen Blöcken.
        /// </summary>
        private readonly Dictionary<string, Token> _dictToken;

        /// <summary>
        /// Eine Liste mit Filtern
        /// </summary>
        private readonly List<IPostprocessTokenizerFilter> _listFilters;

        /// <summary>
        /// Erstellt eine statische Default-Instanz dieser Klasse.
        /// </summary>
        static MathExpressionTokenizer()
        {
            Default = new MathExpressionTokenizer();
        }

        /// <summary>
        /// Erstellt einen neuen Tokenizer.
        /// Der Konstruktor sollte nicht direkt aufgerufen werden.
        /// Zur Erstellung eines neuen Tokenizers ist die <see cref="MathExpressionTokenizerFactory"/> gedacht.
        /// </summary>
        public MathExpressionTokenizer()
        {
            _dictToken = new Dictionary<string, Token>();
            _listFilters = new List<IPostprocessTokenizerFilter>();
        }

        /// <summary>
        /// Fügt der Erkennungsroutine einen neuen logischen Block hinzu.
        /// </summary>
        /// <param name="tt">Der Block-Typ</param>
        /// <param name="strToken">Der zu erkennende Inhalt des Blocks</param>
        /// <param name="associativity"></param>
        /// <param name="precedence"></param>
        public void RegisterToken(TokenType tt, string strToken, TokenAssociativity associativity = TokenAssociativity.NoneAssociative, TokenPrecedence precedence = TokenPrecedence.Undefined)
        {
            _dictToken.Add(strToken, new Token(tt, strToken, associativity, precedence));
        }

        /// <summary>
        /// Entfernt einen neuen logischen Block aus dem Speicher der Erkennungsroutine.
        /// </summary>
        /// <param name="strToken">Der zu nicht mehr zu erkennende Inhalt des Blocks</param>
        public void UnregisterToken(string strToken)
        {
            _dictToken.Remove(strToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public void AddPostprocessFilter(IPostprocessTokenizerFilter filter)
        {
            _listFilters.Add(filter);
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="strInput">Der String mit der mathematischen Funktion</param>
        /// <param name="bPostprocess">True, wenn die Ausgabe gefiltert werden soll. Filtern bedeutet, das beispielweise einzelne Zahlen
        /// die hintereinander stehen zusammengefasst werden (z.B. ein Token mit "10" anstelle von zwei Token mit "1" und "0", sowie die
        /// automatische Erkennung von Fehlenden Operatoren (z.B. 5x wird zu 5*x)</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public IEnumerable<Token> Tokenize(string strInput, bool bPostprocess = false)
        {
            return Tokenize(new StringReader(strInput), bPostprocess);
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</param>
        /// <param name="bPostprocess">True, wenn die Ausgabe gefiltert werden soll. Filtern bedeutet, das beispielweise einzelne Zahlen
        /// die hintereinander stehen zusammengefasst werden (z.B. ein Token mit "10" anstelle von zwei Token mit "1" und "0", sowie die
        /// automatische Erkennung von Fehlenden Operatoren (z.B. 5x wird zu 5*x)</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public IEnumerable<Token> Tokenize(TextReader reader, bool bPostprocess = false)
        {
            IEnumerable<Token> tokens = TokenizeInternal(reader);

            if (bPostprocess)
            {
                foreach (IPostprocessTokenizerFilter filter in _listFilters)
                {
                    tokens = filter.PostProcessTokens(tokens, this);
                }
            }

            return tokens;
        }

        internal Token GetRegisteredToken(string strToken)
        {
            if (_dictToken.ContainsKey(strToken))
            {
                return _dictToken[strToken].CreateCopy();
            }

            return null;
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        private IEnumerable<Token> TokenizeInternal(TextReader reader)
        {
            StringBuilder sbNextToken = new StringBuilder(10);

            int iNext;
            Token lastToken = new Token(TokenType.Unknown, "");

            while ((iNext = reader.Peek()) != -1)
            {
                char c = (char) iNext;

                sbNextToken.Append(c);

                if (_dictToken.ContainsKey(sbNextToken.ToString()))
                {
                    lastToken = _dictToken[sbNextToken.ToString()];
                    reader.Read();
                }
                else if (lastToken.Type != TokenType.Unknown)
                {
                    sbNextToken.Remove(sbNextToken.Length - 1, 1);
                    yield return new Token(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity, lastToken.Precedence);
                    lastToken = new Token(TokenType.Unknown, "");
                    sbNextToken.Clear();
                }
                else
                {
                    reader.Read();
                }
            }

            yield return new Token(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity);
        }

        #region Tests

        public static void Test()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "2"),
            };
            var arrResult = Default.Tokenize("1 + 1 = 2").ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 1");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("10 - 20").ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 2");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("10 -2 0").ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 3");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Parenthesis, ")"),
            };
            arrResult = Default.Tokenize("sqrt(9*9) = (9 )").ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 4");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
            };
            arrResult = Default.Tokenize("sqrt(9*9) = (9 )", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 5");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "10"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("10 -2 0", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 6");
            }

            // --------------------------------------------------------------

            Default.RegisterToken(TokenType.Variable, "x");
            Default.RegisterToken(TokenType.Variable, "y");

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "253"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Variable, "x"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Function, "tan"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Variable, "y"),
            };
            arrResult = Default.Tokenize("253 * x-(tan(5 5) 3) / y", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 7");
            }

            Default.UnregisterToken("x");
            Default.UnregisterToken("y");

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("1.2042 -2 0", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 8");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "-1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("-1.2042 -2 0", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 9");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = Default.Tokenize("+1.2042 -2 0", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 10");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "-2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "-0"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "-12"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
            };
            arrResult = Default.Tokenize("+1.2042 --2 \t\t* -0 +-12\t -   + 2", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 11");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Parenthesis, "("), 
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Parenthesis, ")"),
            };
            arrResult = Default.Tokenize("(3 + 4)(5 - 6)", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 12");
            }

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "8"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "9"),
            };
            arrResult = Default.Tokenize("1 + 2 - 3*4 + 5^6^7*8 - 9", true).ToArray();
            var arrResultT = Default.Tokenize("1 + 2 - 3 4 + 5^6^7*8 - 9", true).ToArray();
            if (!arrResult.SequenceEqual(arrExpected))
            {
                throw new SystemException("MathExpressionTokenizer::Test 13");
            }
            if (!arrResultT.SequenceEqual(arrResult))
            {
                throw new SystemException("MathExpressionTokenizer::Test 14");
            }
        }

        #endregion
    }
}
