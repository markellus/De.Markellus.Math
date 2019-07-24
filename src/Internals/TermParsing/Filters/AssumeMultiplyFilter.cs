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
using System.Linq;

namespace De.Markellus.Maths.Internals.TermParsing.Filters
{
    /// <summary>
    /// Untersucht einen mathematischen Term auf Stellen, an denen implizit ein Multiplikations-Operator angenommen wird,
    /// und fügt diesen als Token ein.
    /// Beispiele:
    ///     (1)  Aus (5 + x)(3 - y) wird (5 + x) * (3 - y)
    ///     (2)  Aus 5x wird 5 * x
    /// </summary>
    public class AssumeMultiplyFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens)
        {
            Queue<Token> lastTokens = new Queue<Token>(3);

            foreach (Token t in tokens)
            {
                lastTokens.Enqueue(t);

                if (lastTokens.Count == 2)
                {
                    Token t1 = lastTokens.Dequeue();
                    Token t2 = lastTokens.Dequeue();

                    if (t1.Type == TokenType.Parenthesis && t1.Value == ")" &&
                        t2.Type == TokenType.Parenthesis && t2.Value == "(")
                    {
                        yield return t1;
                        yield return MathExpressionTokenizer.GetToken("*");
                        yield return t2;
                    }
                    else if ((t1.Type == TokenType.Number || t1.Type == TokenType.Variable || t1.Type == TokenType.Constant || (t1.Type == TokenType.Parenthesis && t1.Value == ")") ) &&
                             (t2.Type == TokenType.Number || t2.Type == TokenType.Variable || t2.Type == TokenType.Constant || (t2.Type == TokenType.Parenthesis && t1.Value == "(") || t2.Type == TokenType.Function))
                    {
                        yield return t1;
                        yield return MathExpressionTokenizer.GetToken("*");
                        yield return t2;
                    }
                    else
                    {
                        yield return t1;
                        lastTokens.Enqueue(t2);
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
