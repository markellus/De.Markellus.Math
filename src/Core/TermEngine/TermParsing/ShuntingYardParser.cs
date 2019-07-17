/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// In dieser Klasse befinden sich Methoden, um mathematische Terme von der Infix-Notation in die umgedrehte polnische
    /// Notation umzuschreiben. Dafür wird der Shunting Yard-Algorithmus von Edsger W. Dijkstra genutzt.
    /// Sehe auch: https://de.wikipedia.org/wiki/Shunting-yard-Algorithmus 
    /// </summary>
    public static class ShuntingYardParser
    {
        /// <summary>
        /// Wandelt einen mathematischen Term in der Infix-Notation in die umgedrehte polnische Notation um.
        /// </summary>
        /// <param name="strInput">Der mathematische Term in der Infix-Notation als String</param>
        /// <param name="tokenizer">Ein Tokenizer, der zum Parsen des Terms verwendet werden kann. Null, wenn der Standard-Tokenizer verwendet werden soll.</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        public static IEnumerable<Token> InfixToRpn(string strInput, MathExpressionTokenizer tokenizer = null)
        {
            return InfixToRpn(new StringReader(strInput), tokenizer);
        }

        /// <summary>
        /// Wandelt einen mathematischen Term in der Infix-Notation in die umgedrehte polnische Notation um.
        /// </summary>
        /// <param name="reader">Ein Text-Stream, der den mathematischen Term in der Infix-Notation enthält</param>
        /// <param name="tokenizer">Ein Tokenizer, der zum Parsen des Terms verwendet werden kann. Null, wenn der Standard-Tokenizer verwendet werden soll.</param>
        /// <returns>Der mathematische Term in umgedrehter polnischer Notation, aufgeteilt in Token.</returns>
        public static IEnumerable<Token> InfixToRpn(TextReader reader, MathExpressionTokenizer tokenizer = null)
        {
            if (tokenizer == null)
            {
                tokenizer = MathExpressionTokenizer.Default;
            }

            return InfixToRpn(tokenizer.Tokenize(reader, true));
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
            bool bOk = true;

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
                        break;
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
