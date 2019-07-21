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
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace De.Markellus.Maths.Internals.TermParsing
{
    /// <summary>
    /// In dieser Klasse befinden sich Methoden, um mathematische Terme von der Infix-Notation in die umgedrehte polnische
    /// Notation umzuschreiben. Dafür wird der Shunting Yard-Algorithmus von Edsger W. Dijkstra genutzt.
    /// Sehe auch: https://de.wikipedia.org/wiki/Shunting-yard-Algorithmus 
    /// </summary>
    internal static class ShuntingYardParser
    {
        /// <summary>
        /// Wandelt einen mathematischen Term in der Infix-Notation in die umgedrehte polnische Notation um.
        /// </summary>
        /// <param name="strInput">Der mathematische Term in der Infix-Notation als String</param>
        /// <param name="arrVariables">Liste mit Variablen, die in der mathematischen Funktion enthalten sind.</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        public static IEnumerable<Token> InfixToRpn(string strInput, params string[] arrVariables)
        {
            return InfixToRpn(new StringReader(strInput), arrVariables);
        }

        /// <summary>
        /// Wandelt einen mathematischen Term in der Infix-Notation in die umgedrehte polnische Notation um.
        /// </summary>
        /// <param name="reader">Ein Text-Stream, der den mathematischen Term in der Infix-Notation enthält</param>
        /// <param name="arrVariables">Liste mit Variablen, die in der mathematischen Funktion enthalten sind.</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        public static IEnumerable<Token> InfixToRpn(TextReader reader, params string[] arrVariables)
        {
            return InfixToRpn(MathExpressionTokenizer.Tokenize(reader, true, arrVariables));
        }

        /// <summary>
        /// Wandelt einen mathematischen Term in der Infix-Notation in die umgedrehte polnische Notation um.
        /// </summary>
        /// <param name="infixToken">Der mathematische Term in der Infix-Notation</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        public static IEnumerable<Token> InfixToRpn(IEnumerable<Token> infixToken)
        {
            return ShuntingYard(infixToken);
        }

        /// <summary>
        /// Shunting Yard-Parser
        /// </summary>
        /// <param name="infixToken">Der mathematische Term in der Infix-Notation</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        private static IEnumerable<Token> ShuntingYard(IEnumerable<Token> infixToken)
        {
            var stack = new Stack<Token>();
            bool bOk;

            //SOLANGE Tokens verfügbar sind:
            //Token einlesen.
            foreach (Token token in infixToken)
            {
                switch (token.Type)
                {
                    //WENN Token IST - Zahl:
                    case TokenType.Number:
                    case TokenType.Variable:
                    case TokenType.Constant:
                        //Token ZU Ausgabe.
                        yield return token;
                        break;
                    //WENN Token IST-Funktion:
                    case TokenType.Function:
                        //Token ZU Stack.
                        stack.Push(token);
                        break;
                    //WENN Token IST-Argumenttrennzeichen:
                    case TokenType.ArgumentSeparator:
                        bOk = true;
                        while (bOk)
                        {
                            //FEHLER-BEI Stack IST-LEER:
                            //GRUND (1) Ein falsch platziertes Argumenttrennzeichen.
                            //GRUND (2) Der schließenden Klammer geht keine öffnende voraus.
                            if (stack.Count == 0)
                            {
                                throw new FormatException(
                                    $"Syntax error: Unexpected token of type {nameof(TokenType.ArgumentSeparator)}");
                            }

                            //BIS Stack-Spitze IST öffnende-Klammer:
                            if (stack.Peek().Value != "(")
                            {
                                //Stack-Spitze ZU Ausgabe.
                                yield return stack.Pop();
                            }
                            else
                            {
                                bOk = false;
                            }
                        }
                        break;
                    //WENN Token IST-Operator
                    case TokenType.Operator:
                        //SOLANGE Stack IST-NICHT-LEER UND
                        //        Token IST-linksassoziativ UND
                        //        Präzedenz von Token IST-KLEINER-GLEICH Präzedenz von Stack-Spitze
                        while (stack.Count != 0 &&
                               //Stack-Spitze IST Operator UND
                               stack.Peek().Type == TokenType.Operator &&
                               //Token IST-linksassoziativ UND
                               token.Associativity == TokenAssociativity.LeftAssociative &&
                               //Präzedenz von Token IST-KLEINER-GLEICH Präzedenz von Stack-Spitze
                               token.Precedence <= stack.Peek().Precedence)
                        {
                            //Stack-Spitze ZU Ausgabe.
                            yield return stack.Pop();
                        }

                        //Token ZU Stack.
                        stack.Push(token);
                        break;
                    case TokenType.Parenthesis:
                        //WENN Token IST öffnende-Klammer:
                        if (token.Value == "(")
                            //Token ZU Stack.
                            stack.Push(token);
                        //WENN Token IST schließende-Klammer:
                        else if (token.Value == ")")
                        {
                            bOk = true;
                            while (bOk)
                            {
                                //FEHLER-BEI Stack IST-LEER:
                                //GRUND (1) Der schließenden Klammer geht keine öffnende voraus.
                                if (stack.Count == 0)
                                {
                                    throw new FormatException(
                                        "Syntax error: Closing parenthesis without preceding open parenthesis detected");
                                }

                                //BIS Stack-Spitze IST öffnende-Klammer:
                                if (stack.Peek().Value != "(")
                                {
                                    //Stack-Spitze ZU Ausgabe.
                                    yield return stack.Pop();
                                }
                                else
                                {
                                    bOk = false;
                                }
                            }

                            //Stack-Spitze (öffnende-Klammer) entfernen
                            stack.Pop();
                            //WENN Stack-Spitze IST-Funktion:
                            if (stack.Any() && stack.Peek().Type == TokenType.Function)
                                //Stack-Spitze ZU Ausgabe.
                                yield return stack.Pop();
                        }
                        break;
                    case TokenType.Unknown:
                        throw new FormatException(
                            $"Unable to parse the input, starting here: \"{token.Value}\"\r\nHave you forgotten to define a variable or function?");
                    default:
                        Debugger.Break();
                        throw new Exception("Wrong token");
                }
            }

            //BIS Stack IST-LEER:
            while (stack.Any())
            {
                //FEHLER-BEI Stack-Spitze IST öffnende-Klammer:
                //GRUND (1) Es gibt mehr öffnende als schließende Klammern.
                if (stack.Peek().Type == TokenType.Parenthesis && stack.Peek().Value == "(")
                {
                    throw new FormatException(
                        "Syntax error: open parenthesis without following closing parenthesis detected");
                }

                //Stack-Spitze ZU Ausgabe.
                yield return stack.Pop();
            }
        }
    }
}
