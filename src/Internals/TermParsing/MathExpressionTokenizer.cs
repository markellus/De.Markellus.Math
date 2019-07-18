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
using System.IO;
using System.Linq;
using System.Text;
using De.Markellus.Maths.Internals.TermParsing.Filters;
using De.Markellus.Maths.KnowledgeBase.Algebra.Token;

namespace De.Markellus.Maths.Internals.TermParsing
{
    /// <summary>
    /// Der MathExpressionTokenizer teilt eine mathematische Funktion, die als ein String vorliegt, in logische Blöcke auf.
    /// </summary>
    internal static class MathExpressionTokenizer
    {
        /// <summary>
        /// Ein Wörterbuch mit logischen Blöcken.
        /// </summary>
        private static readonly Dictionary<string, Token> _dictToken;

        /// <summary>
        /// Eine Liste mit Filtern
        /// </summary>
        private static readonly List<IPostprocessTokenizerFilter> _listFilters;

        /// <summary>
        /// Erstellt eine statische Default-Instanz dieser Klasse.
        /// </summary>
        static MathExpressionTokenizer()
        {
            _dictToken = new Dictionary<string, Token>();
            _listFilters = new List<IPostprocessTokenizerFilter>();
            TokenKnowledgeBase.LoadKnowledge();

            AddPostprocessFilter(new TokenGroupFilter());
            AddPostprocessFilter(new DecimalFilter());
            AddPostprocessFilter(new AssumeMultiplyFilter());
            AddPostprocessFilter(new WhitespaceFilter());
            AddPostprocessFilter(new SignedNumberFilter());
            AddPostprocessFilter(new FunctionArgumentFilter());
        }

        /// <summary>
        /// Fügt der Erkennungsroutine einen neuen logischen Block hinzu.
        /// </summary>
        /// <param name="tt">Der Block-Typ</param>
        /// <param name="strToken">Der zu erkennende Inhalt des Blocks</param>
        /// <param name="associativity"></param>
        /// <param name="precedence"></param>
        public static void RegisterToken(TokenType tt, string strToken,
            TokenAssociativity associativity = TokenAssociativity.NoneAssociative,
            TokenPrecedence precedence = TokenPrecedence.Undefined)
        {
            _dictToken.Add(strToken, new TokenizerToken(tt, strToken, associativity, precedence));
        }

        /// <summary>
        /// Ruft einen Token ab, der zuvor mittels der Funktion <see cref="RegisterToken"/> registriert wurde.
        /// </summary>
        /// <param name="strToken">Der Inhalt des abzurufenden Tokens</param>
        /// <returns>Ein Token, der dem übergebenen Inhalt entspricht, oder null, wenn ein Token mit diesem Inhalt nicht existiert.</returns>
        public static Token GetToken(string strToken)
        {
            if (_dictToken.ContainsKey(strToken))
            {
                return _dictToken[strToken].CreateCopy();
            }

            return null;
        }

        /// <summary>
        /// Ruft einen Token ab, der zuvor mittels der Funktion <see cref="RegisterToken"/> registriert wurde.
        /// </summary>
        /// <param name="type">Der Typ des abzurufenden Tokens. </param>
        /// <returns>Der erste gefundene Token, der dem übergebenen Typ entspricht, oder null, wenn ein Token von diesem Typ nicht existiert.</returns>
        public static Token GetToken(TokenType type)
        {
            return _dictToken.FirstOrDefault(t => t.Value.Type == type).Value;
        }

        /// <summary>
        /// Erstellt einen Token mit neuem Wert anhand der übergebenen Vorlage.
        /// </summary>
        /// <param name="token">Der Token, der als Vorlage dienen soll.</param>
        /// <param name="strValue">Der Wert des neuen Tokens</param>
        /// <returns>Ein neuer Token.</returns>
        public static Token GetModifiedToken(Token token, string strValue)
        {
            return new TokenizerToken(token.Type, strValue, token.Associativity, token.Precedence);
        }

        /// <summary>
        /// Erstellt einen Token, der eine Variable abbildet.
        /// </summary>
        /// <param name="strVariable">Die Stringdarstellung der Variable.</param>
        /// <returns>Ein neuer Token.</returns>
        public static Token GetVariableToken(string strVariable)
        {
            return new TokenizerToken(TokenType.Variable, strVariable);
        }

        /// <summary>
        /// Fügt einen Postprocessing-Filter hinzu.
        /// </summary>
        /// <param name="filter">Ein Filter, der das<see cref="IPostprocessTokenizerFilter"/>-Interface implementiert.</param>
        private static void AddPostprocessFilter(IPostprocessTokenizerFilter filter)
        {
            _listFilters.Add(filter);
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="strInput">Der String mit der mathematischen Funktion</param>
        /// <param name="bPostprocess">True, wenn die Ausgabe gefiltert werden soll. Filtern bedeutet, das beispielweise einzelne Zahlen
        /// die hintereinander stehen zusammengefasst werden (z.B. ein Token mit "10" anstelle von zwei Token mit "1" und "0", sowie die
        /// automatische Erkennung von Fehlenden Operatoren (z.B. 5x wird zu 5*x)</param>
        /// <param name="arrVariables">Liste mit Variablen, die in der mathematischen Funktion enthalten sind.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public static IEnumerable<Token> Tokenize(string strInput, bool bPostprocess = false, params string[] arrVariables)
        {
            return Tokenize(new StringReader(strInput), bPostprocess, arrVariables);
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <param name="bPostprocess">True, wenn die Ausgabe gefiltert werden soll. Filtern bedeutet, das beispielweise einzelne Zahlen
        /// die hintereinander stehen zusammengefasst werden (z.B. ein Token mit "10" anstelle von zwei Token mit "1" und "0", sowie die
        /// automatische Erkennung von Fehlenden Operatoren (z.B. 5x wird zu 5*x)</param>
        /// <param name="arrVariables">Liste mit Variablen, die in der mathematischen Funktion enthalten sind.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public static IEnumerable<Token> Tokenize(TextReader reader, bool bPostprocess = false, params string[] arrVariables)
        {
            IEnumerable<Token> tokens = TokenizeInternal(reader, arrVariables);

            if (bPostprocess)
            {
                foreach (IPostprocessTokenizerFilter filter in _listFilters)
                {
                    tokens = filter.PostProcessTokens(tokens);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <param name="arrVariables">Liste mit Variablen, die in der mathematischen Funktion enthalten sind.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        private static IEnumerable<Token> TokenizeInternal(TextReader reader, params string[] arrVariables)
        {
            StringBuilder sbNextToken = new StringBuilder(10);

            int iNext;
            Token lastToken = new TokenizerToken(TokenType.Unknown, "");

            while ((iNext = reader.Peek()) != -1)
            {
                char c = (char) iNext;

                sbNextToken.Append(c);

                if (_dictToken.ContainsKey(sbNextToken.ToString()))
                {
                    lastToken = _dictToken[sbNextToken.ToString()];
                    reader.Read();
                }
                else if(arrVariables.Contains(sbNextToken.ToString()))
                {
                    lastToken = GetVariableToken(sbNextToken.ToString());
                    reader.Read();
                }
                else if (lastToken.Type != TokenType.Unknown)
                {
                    sbNextToken.Remove(sbNextToken.Length - 1, 1);
                    yield return new TokenizerToken(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity,
                        lastToken.Precedence);
                    lastToken = new TokenizerToken(TokenType.Unknown, "");
                    sbNextToken.Clear();
                }
                else
                {
                    reader.Read();
                }
            }

            yield return new TokenizerToken(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity);
        }

        private class TokenizerToken : Token
        {
            public TokenizerToken(TokenType type, string value,
                TokenAssociativity associativity = TokenAssociativity.NoneAssociative,
                TokenPrecedence precedence = TokenPrecedence.Undefined) : base(type, value, associativity, precedence)
            {
            }
        }
    }
}
