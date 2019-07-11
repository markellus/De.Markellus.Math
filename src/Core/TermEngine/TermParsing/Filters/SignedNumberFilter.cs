﻿/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Collections.Generic;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing.Filters
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
        public IEnumerable<Token> PostProcessTokens(IEnumerable<Token> tokens, MathExpressionTokenizer tokenizer)
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
                        yield return new Token(TokenType.Number, (t1.Value != "+" ? t1.Value : "") + t2.Value,
                            t2.Associativity, t2.Precedence);
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
                        yield return new Token(TokenType.Number, (t2.Value != "+" ? t2.Value : "") + t3.Value,
                            t3.Associativity, t3.Precedence);
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
