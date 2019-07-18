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

using System.Collections.Generic;

namespace De.Markellus.Maths.Internals.TermParsing.Filters
{
    /// <summary>
    /// Dieser Filter erkennt Vorzeichen-Token und gruppiert diese mit dem dazugehörigen Zahl-Token.
    /// </summary>
    public class SignedNumberFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens)
        {
            bool bIsStart = true;
            Queue<Token> lastTokens = new Queue<Token>(3);

            foreach (Token t in tokens)
            {
                lastTokens.Enqueue(t);

                if (lastTokens.Count == 2 && bIsStart)
                {
                    bIsStart = false;

                    Token t1 = lastTokens.Dequeue();
                    Token t2 = lastTokens.Dequeue();

                    if ((t1.Type == TokenType.Operator && (t1.Value == "-" || t1.Value == "+")) &&
                        t2.Type == TokenType.Number)
                    {
                        yield return MathExpressionTokenizer.GetModifiedToken(t2, (t1.Value != "+" ? t1.Value : "") + t2.Value);
                    }
                    else
                    {
                        if (t1.Type == TokenType.Parenthesis && t1.Value == "(")
                        {
                            lastTokens.Enqueue(t1);
                        }
                        else
                        {
                            yield return t1;
                        }
                        lastTokens.Enqueue(t2);
                    }
                }

                if (lastTokens.Count == 3)
                {
                    Token t1 = lastTokens.Dequeue();
                    Token t2 = lastTokens.Dequeue();
                    Token t3 = lastTokens.Dequeue();

                    if ((t1.Type == TokenType.Operator || (t1.Type == TokenType.Parenthesis && t1.Value == "(")) &&
                        (t2.Type == TokenType.Operator && (t2.Value == "-" || t2.Value == "+")) &&
                        t3.Type == TokenType.Number)
                    {
                        yield return t1;
                        yield return MathExpressionTokenizer.GetModifiedToken(t3, (t2.Value != "+" ? t2.Value : "") + t3.Value);
                    }
                    else

                    {
                        yield return t1;
                        lastTokens.Enqueue(t2);
                        lastTokens.Enqueue(t3);
                    }
                }
            }

            while (lastTokens.Count > 0)
            {
                yield return lastTokens.Dequeue();
            }
        }
    }
}
