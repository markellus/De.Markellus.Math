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
using De.Markellus.Maths.KnowledgeBase.Algebra.Functions;

namespace De.Markellus.Maths.Internals.TermParsing.Filters
{
    /// <summary>
    /// Fügt Default-Parameter in Funktionen ein, wenn diese nicht explizit angegeben wurden.
    /// </summary>
    public class FunctionArgumentFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Wörterbuch mit Parameter-Definitionen
        /// </summary>
        private static readonly Dictionary<string, List<FunctionKnowledgeBase.DefaultFunctionArgument>> _dicFunctionArgumentCount;

        /// <summary>
        /// Initialisierung der Parameter-Definitionen
        /// </summary>
        static FunctionArgumentFilter()
        {
            _dicFunctionArgumentCount = new Dictionary<string, List<FunctionKnowledgeBase.DefaultFunctionArgument>>();
            FunctionKnowledgeBase.LoadKnowledge();
        }

        /// <summary>
        /// Registriert eine neue Parameter-Definition.
        /// </summary>
        /// <param name="strName">Der Name der Funktion</param>
        /// <param name="listArguments">Liste mit Parameter-Definitionen</param>
        internal static void RegisterFunction(string strName, List<FunctionKnowledgeBase.DefaultFunctionArgument> listArguments)
        {
            _dicFunctionArgumentCount.Add(strName, listArguments);
        }

        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens)
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
            List<FunctionKnowledgeBase.DefaultFunctionArgument> listCurrent =
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
        private IEnumerable<Token> InsertMissingArguments(int iCurrentArg, List<FunctionKnowledgeBase.DefaultFunctionArgument> listDefaultArgs)
        {
            while (iCurrentArg != listDefaultArgs.Count)
            {
                if (listDefaultArgs[iCurrentArg].HasDefaultValue)
                {
                    yield return MathExpressionTokenizer.GetToken(TokenType.ArgumentSeparator);
                    yield return listDefaultArgs[iCurrentArg].DefaultValue;
                    iCurrentArg++;
                }
                else
                {
                    throw new FormatException();//TODO
                }
            }
        }

        
    }
}
