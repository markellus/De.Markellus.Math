/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Collections.Generic;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// Interface für einen Filter, mit dem ein mathematischer Ausdruck oder Term nachbearbeitet werden kann.
    /// </summary>
    public interface IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens, MathExpressionTokenizer tokenizer);
    }
}
