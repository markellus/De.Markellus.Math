/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Collections.Generic;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing.Filters
{
    /// <summary>
    /// Dieser Filter gruppiert zusammenpassende Token zu einem einzelnen Token zwecks einfacher Weiterverarbeitung.
    /// </summary>
    public class TokenGroupFilter : IPostprocessTokenizerFilter
    {
        /// <summary>
        /// Filtert einen mathematischen Ausdruck oder Term.
        /// </summary>
        /// <param name="tokens">Ein Token-Iterator mit den einzelnen Teilen des mathematischen Ausdruckes</param>
        /// <param name="tokenizer">Der <see cref="MathExpressionTokenizer"/>, der den Term zuvor geparst hat.</param>
        /// <returns>Ein neuer Token-Iterator, der es ermöglicht über den gefilterten Term zu iterieren.</returns>
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens, MathExpressionTokenizer tokenizer)
        {
            //Mehrere aufeinanderfolgende Leerzeichen sind redundant
            tokens = GroupByType(tokens, TokenType.WhiteSpace);
            //Gruppierung von einzelnen Ziffern zu einer Zahl
            tokens = GroupByType(tokens, TokenType.Number);
            return tokens;
        }

        /// <summary>
        /// Gruppiert aufeinanderfolgende Token.
        /// </summary>
        /// <param name="tokens">Potenziell zu gruppierende Token</param>
        /// <param name="tt">Typ der zu gruppierenden Token</param>
        /// <returns>Gefilterte Liste, in der aufeinanderfolgende Token des angegebenen Typs zu einem Token zusammengefasst worden sind.</returns>
        private IEnumerable<Token> GroupByType(IEnumerable<Token> tokens, TokenType tt)
        {
            Token lastToken = Token.CreateDefault();

            foreach (Token t in tokens)
            {
                if (t.Type == tt && lastToken.Type == tt)
                {
                    lastToken = new Token(t.Type, lastToken.Value + t.Value, t.Associativity, t.Precedence);
                }
                else if (lastToken.Type == TokenType.Unknown)
                {
                    lastToken = t;
                }
                else
                {
                    yield return lastToken;
                    lastToken = t;
                }
            }

            yield return lastToken;
        }
    }
}
