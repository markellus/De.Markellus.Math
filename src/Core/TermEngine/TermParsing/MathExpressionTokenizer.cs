﻿/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// Der MathExpressionTokenizer teilt eine mathematische Funktion, die als ein String vorliegt, in logische Blöcke auf.
    /// </summary>
    public class MathExpressionTokenizer
    {
        /// <summary>
        /// Eine Default-Instanz dieser Klasse, die für allgemeine Zwecke verwendet werden kann.
        /// </summary>
        public static MathExpressionTokenizer Default { get; }

        /// <summary>
        /// Ein Wörterbuch mit logischen Blöcken.
        /// </summary>
        private readonly Dictionary<string, Token> _dictToken;

        /// <summary>
        /// Eine Liste mit Filtern
        /// </summary>
        private readonly List<IPostprocessTokenizerFilter> _listFilters;

        /// <summary>
        /// Erstellt eine statische Default-Instanz dieser Klasse.
        /// </summary>
        static MathExpressionTokenizer()
        {
            Default = MathExpressionTokenizerFactory.Create(true, true);
        }

        /// <summary>
        /// Erstellt einen neuen Tokenizer.
        /// Der Konstruktor sollte nicht direkt aufgerufen werden.
        /// Zur Erstellung eines neuen Tokenizers ist die <see cref="MathExpressionTokenizerFactory"/> gedacht.
        /// </summary>
        public MathExpressionTokenizer()
        {
            _dictToken = new Dictionary<string, Token>();
            _listFilters = new List<IPostprocessTokenizerFilter>();
        }

        /// <summary>
        /// Fügt der Erkennungsroutine einen neuen logischen Block hinzu.
        /// </summary>
        /// <param name="tt">Der Block-Typ</param>
        /// <param name="strToken">Der zu erkennende Inhalt des Blocks</param>
        /// <param name="associativity"></param>
        /// <param name="precedence"></param>
        public void RegisterToken(TokenType tt, string strToken, TokenAssociativity associativity = TokenAssociativity.NoneAssociative, TokenPrecedence precedence = TokenPrecedence.Undefined)
        {
            _dictToken.Add(strToken, new Token(tt, strToken, associativity, precedence));
        }

        /// <summary>
        /// Entfernt einen neuen logischen Block aus dem Speicher der Erkennungsroutine.
        /// </summary>
        /// <param name="strToken">Der zu nicht mehr zu erkennende Inhalt des Blocks</param>
        public void UnregisterToken(string strToken)
        {
            _dictToken.Remove(strToken);
        }

        /// <summary>
        /// Fügt einen Postprocessing-Filter hinzu.
        /// </summary>
        /// <param name="filter">Ein Filter, der das<see cref="IPostprocessTokenizerFilter"/>-Interface implementiert.</param>
        public void AddPostprocessFilter(IPostprocessTokenizerFilter filter)
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
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public IEnumerable<Token> Tokenize(string strInput, bool bPostprocess = false)
        {
            return Tokenize(new StringReader(strInput), bPostprocess);
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</param>
        /// <param name="bPostprocess">True, wenn die Ausgabe gefiltert werden soll. Filtern bedeutet, das beispielweise einzelne Zahlen
        /// die hintereinander stehen zusammengefasst werden (z.B. ein Token mit "10" anstelle von zwei Token mit "1" und "0", sowie die
        /// automatische Erkennung von Fehlenden Operatoren (z.B. 5x wird zu 5*x)</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        public IEnumerable<Token> Tokenize(TextReader reader, bool bPostprocess = false)
        {
            IEnumerable<Token> tokens = TokenizeInternal(reader);

            if (bPostprocess)
            {
                foreach (IPostprocessTokenizerFilter filter in _listFilters)
                {
                    tokens = filter.PostProcessTokens(tokens, this);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Ruft einen Token ab, der zuvor mittels der Funktion <see cref="RegisterToken"/> registriert wurde.
        /// </summary>
        /// <param name="strToken">Der Inhalt des abzurufenden Tokens</param>
        /// <returns>Ein Tpoken, der dem übergebenen Inhalt entspricht, oder null, wenn ein Token mit diesem Inhalt nicht existiert.</returns>
        internal Token GetRegisteredToken(string strToken)
        {
            if (_dictToken.ContainsKey(strToken))
            {
                return _dictToken[strToken].CreateCopy();
            }

            return null;
        }

        /// <summary>
        /// Wandelt einen String, der eine mathematische Funktion enthält, in einzelne logische Blöcke um, über die iteriert werden kann.
        /// </summary>
        /// <param name="reader">Ein Stream, aus dem die Funktion ausgelesen wird.</param>
        /// <returns>Ein Iterator mit den logischen Blöcken der Funktion</returns>
        private IEnumerable<Token> TokenizeInternal(TextReader reader)
        {
            StringBuilder sbNextToken = new StringBuilder(10);

            int iNext;
            Token lastToken = new Token(TokenType.Unknown, "");

            while ((iNext = reader.Peek()) != -1)
            {
                char c = (char) iNext;

                sbNextToken.Append(c);

                if (_dictToken.ContainsKey(sbNextToken.ToString()))
                {
                    lastToken = _dictToken[sbNextToken.ToString()];
                    reader.Read();
                }
                else if (lastToken.Type != TokenType.Unknown)
                {
                    sbNextToken.Remove(sbNextToken.Length - 1, 1);
                    yield return new Token(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity, lastToken.Precedence);
                    lastToken = new Token(TokenType.Unknown, "");
                    sbNextToken.Clear();
                }
                else
                {
                    reader.Read();
                }
            }

            yield return new Token(lastToken.Type, sbNextToken.ToString(), lastToken.Associativity);
        }
    }
}