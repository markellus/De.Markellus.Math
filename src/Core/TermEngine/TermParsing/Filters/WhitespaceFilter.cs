/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Collections.Generic;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing.Filters
{
    /// <summary>
    /// Dieser Filter entfernt leere Token.
    /// </summary>
    public class WhitespaceFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens, MathExpressionTokenizer tokenizer)
        {
            foreach (Token t in tokens)
            {
                if (t.Type != TokenType.WhiteSpace)
                {
                    yield return t;
                }
            }
        }
    }
}
