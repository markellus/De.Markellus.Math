﻿/* 
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

using De.Markellus.Maths.Core.TermEngine.TermParsing.Filters;

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    public static class MathExpressionTokenizerFactory
    {
        /// <summary>
        /// Erstellt einen neuen Tokenizer.
        /// </summary>
        /// <param name="bRegisterDefaultTokens">true, wenn eine Vorauswahl an bekannten Token registriert werden soll, ansonsten false.</param>
        /// <param name="bAddDefaultPostProcessFilters">true, wenn die Standard-PostProcessing-Filter genutzt werden sollen, ansonsten false.</param>
        /// <returns></returns>
        public static MathExpressionTokenizer Create(bool bRegisterDefaultTokens = true, bool bAddDefaultPostProcessFilters = true)
        {
            MathExpressionTokenizer tokenizer = new MathExpressionTokenizer();

            if (bRegisterDefaultTokens)
            {
                // Numerische Werte
                tokenizer.RegisterToken(TokenType.Number, "0");
                tokenizer.RegisterToken(TokenType.Number, "1");
                tokenizer.RegisterToken(TokenType.Number, "2");
                tokenizer.RegisterToken(TokenType.Number, "3");
                tokenizer.RegisterToken(TokenType.Number, "4");
                tokenizer.RegisterToken(TokenType.Number, "5");
                tokenizer.RegisterToken(TokenType.Number, "6");
                tokenizer.RegisterToken(TokenType.Number, "7");
                tokenizer.RegisterToken(TokenType.Number, "8");
                tokenizer.RegisterToken(TokenType.Number, "9");

                // Konstanten
                tokenizer.RegisterToken(TokenType.Constant, "π");
                tokenizer.RegisterToken(TokenType.Constant, "PI");

                // Operatoren
                tokenizer.RegisterToken(TokenType.Operator, "+", TokenAssociativity.LeftAssociative, TokenPrecedence.Addition);
                tokenizer.RegisterToken(TokenType.Operator, "-", TokenAssociativity.LeftAssociative, TokenPrecedence.Subtraction);
                tokenizer.RegisterToken(TokenType.Operator, "*", TokenAssociativity.LeftAssociative, TokenPrecedence.Multiplication);
                tokenizer.RegisterToken(TokenType.Operator, "/", TokenAssociativity.LeftAssociative, TokenPrecedence.Division);
                tokenizer.RegisterToken(TokenType.Operator, "^", TokenAssociativity.RightAssociative, TokenPrecedence.Exponentiation);
                tokenizer.RegisterToken(TokenType.Operator, "=", TokenAssociativity.NoneAssociative, TokenPrecedence.Equality);

                // Klammern
                tokenizer.RegisterToken(TokenType.Parenthesis, "(");
                tokenizer.RegisterToken(TokenType.Parenthesis, ")");

                // Funktionen
                tokenizer.RegisterToken(TokenType.Function, "sqrt");
                tokenizer.RegisterToken(TokenType.Function, "sin");
                tokenizer.RegisterToken(TokenType.Function, "cos");
                tokenizer.RegisterToken(TokenType.Function, "tan");
                tokenizer.RegisterToken(TokenType.Function, "ln");
                tokenizer.RegisterToken(TokenType.Function, "exp");
                tokenizer.RegisterToken(TokenType.Function, "max");

                // Leerzeichen / Platzhalter
                tokenizer.RegisterToken(TokenType.WhiteSpace, " ");
                tokenizer.RegisterToken(TokenType.WhiteSpace, "\t");

                // Trennzeichen
                tokenizer.RegisterToken(TokenType.ArgumentSeparator, ",");
                tokenizer.RegisterToken(TokenType.DecimalSeparator, ".");
            }

            if (bAddDefaultPostProcessFilters)
            {
                tokenizer.AddPostprocessFilter(new TokenGroupFilter());
                tokenizer.AddPostprocessFilter(new DecimalFilter());
                tokenizer.AddPostprocessFilter(new AssumeMultiplyFilter());
                tokenizer.AddPostprocessFilter(new WhitespaceFilter());
                tokenizer.AddPostprocessFilter(new SignedNumberFilter());
            }

            return tokenizer;
        }
    }
}