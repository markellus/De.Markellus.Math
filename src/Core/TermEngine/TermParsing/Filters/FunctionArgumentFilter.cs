/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Collections.Generic;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing.Filters
{
    /// <summary>
    /// Fügt Default-Parameter in Funktionen ein, wenn diese nicht explizit angegeben wurden.
    /// </summary>
    public class FunctionArgumentFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Wörterbuch mit Parameter-Definitionen
        /// </summary>
        private static readonly Dictionary<string, List<DefaultFunctionArgument>> _dicFunctionArgumentCount;

        /// <summary>
        /// Initialisierung der Parameter-Definitionen
        /// </summary>
        static FunctionArgumentFilter()
        {
            _dicFunctionArgumentCount = new Dictionary<string, List<DefaultFunctionArgument>>
            {
                {
                    //sqrt: Zweites Argument ist die 2te Wurzel, wenn nicht anders angegeben
                    "sqrt", new List<DefaultFunctionArgument>
                    {
                        new DefaultFunctionArgument
                        {
                            HasDefaultValue = false
                        },
                        new DefaultFunctionArgument
                        {
                            HasDefaultValue = true,
                            DefaultValue = new Token(TokenType.Number, "2")
                        },
                    }
                }
            };
        }

        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens, MathExpressionTokenizer tokenizer)
        {
            IEnumerator<Token> tokenEnum = tokens.GetEnumerator();

            while (tokenEnum.MoveNext())
            {
                Token t = tokenEnum.Current;
                yield return t;

                if (t?.Type == TokenType.Function && _dicFunctionArgumentCount.ContainsKey(t.Value))
                {
                    foreach (Token tInner in PostProcessTokensInner(tokenEnum))
                    {
                        yield return tInner;
                    }
                }
            }

        }

        /// <summary>
        /// Rekursives Einfügen von Default-Parametern
        /// </summary>
        /// <param name="tokenEnum">Ein Token-Enumerator mit den einzelnen Teilen desmathematischen Ausdruckes.
        /// Nach Abschluss dieser Funktion zeigt Dieser auf das Zeichen nach der Funktion.</param>
        /// <returns>Ein Token-Iterator mit den Token der Funktion ab der Position des übergebenen Enumerators</returns>
        public IEnumerable<Token> PostProcessTokensInner(IEnumerator<Token> tokenEnum)
        {
            bool bInsideFunction = true;
            int iOpenParenthesis = 0;
            int iCurrentArg = 1;
            List<DefaultFunctionArgument> listCurrent =
                _dicFunctionArgumentCount[tokenEnum?.Current?.Value ?? throw new InvalidOperationException()];

            while (bInsideFunction && tokenEnum.MoveNext())
            {
                Token t = tokenEnum.Current;

                if (t?.Type == TokenType.Function && _dicFunctionArgumentCount.ContainsKey(t.Value))
                {
                    yield return t;

                    foreach (Token tInner in PostProcessTokensInner(tokenEnum))
                    {
                        yield return tInner;
                    }
                }
                else if (t?.Type == TokenType.Parenthesis && t.Value == "(")
                {
                    yield return t;
                    iOpenParenthesis++;
                }
                else if (t?.Type == TokenType.Parenthesis && t.Value == ")")
                {
                    if (--iOpenParenthesis == 0 && iCurrentArg != listCurrent.Count)
                    {
                        foreach (Token arg in InsertMissingArguments(iCurrentArg, listCurrent))
                        {
                            yield return arg;
                        }
                    }

                    yield return t;
                    bInsideFunction = false;
                }
                else if (t?.Type == TokenType.ArgumentSeparator)
                {
                    yield return t;
                    iCurrentArg++;
                }
                else
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        /// Gibt die fehlenden Argument-Token einer Funktion zurück.
        /// </summary>
        /// <param name="iCurrentArg">Position, ab der Argumente fehlen</param>
        /// <param name="listDefaultArgs">Liste mit den Argument-Definitionen</param>
        /// <returns></returns>
        private IEnumerable<Token> InsertMissingArguments(int iCurrentArg, List<DefaultFunctionArgument> listDefaultArgs)
        {
            while (iCurrentArg != listDefaultArgs.Count)
            {
                if (listDefaultArgs[iCurrentArg].HasDefaultValue)
                {
                    yield return new Token(TokenType.ArgumentSeparator, ",");
                    yield return listDefaultArgs[iCurrentArg].DefaultValue;
                    iCurrentArg++;
                }
                else
                {
                    throw new FormatException();//TODO
                }
            }
        }

        /// <summary>
        /// Datenklasse, bildet ein Argument ab
        /// </summary>
        internal class DefaultFunctionArgument
        {
            /// <summary>
            /// true, wenn dieses Argument einen Default-Wert hat, ansonsten false.
            /// </summary>
            public bool HasDefaultValue { get; set; }

            /// <summary>
            /// Der Default-Wert, falls definiert, ansonsten null.
            /// </summary>
            public Token DefaultValue { get; set; }
        }
    }
}
