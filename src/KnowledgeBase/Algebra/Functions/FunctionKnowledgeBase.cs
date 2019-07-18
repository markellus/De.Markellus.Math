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
using System.IO;
using System.Xml;
using De.Markellus.Maths.Internals.TermParsing;
using De.Markellus.Maths.Internals.TermParsing.Filters;

namespace De.Markellus.Maths.KnowledgeBase.Algebra.Functions
{
    /// <summary>
    /// Wissensbasis für mathematische Funktionen
    /// </summary>
    internal static class FunctionKnowledgeBase
    {
        private const string DIR = "./KnowledgeBase/Algebra/Functions";

        /// <summary>
        /// Läd und erstellt die Wissensbasis.
        /// </summary>
        public static void LoadKnowledge()
        {
            if (!Directory.Exists(DIR))
            {
                return;
            }

            foreach (string strFile in Directory.GetFiles(DIR))
            {
                LoadKnowledgeBaseFromFile(strFile);
            }
        }

        /// <summary>
        /// Ergänzt die Wissensbasis anhand einer Datenbank.
        /// </summary>
        /// <param name="strFile">Die Datei, welche die Datenbank enthält.</param>
        public static void LoadKnowledgeBaseFromFile(string strFile)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strFile);
                XmlNode root = doc.DocumentElement;

                if (root?.Name != "knowledgebase")
                {
                    return;
                }

                XmlNode xmlRules = root.SelectSingleNode("functions");

                if (xmlRules == null || !xmlRules.HasChildNodes ||
                    !(xmlRules.SelectNodes("function") is XmlNodeList listFunctions))
                {
                    return;
                }

                foreach (XmlNode node in listFunctions)
                {
                    if (!(node.SelectSingleNode("arguments")?.SelectNodes("argument") is XmlNodeList listArguments))
                    {
                        continue;
                    }
                    XmlNode xmlName = node.SelectSingleNode("name");

                    if (xmlName == null)
                    {
                        continue;
                    }

                    List<DefaultFunctionArgument> listDefaultArguments = new List<DefaultFunctionArgument>();

                    foreach (XmlNode nodeArgument in listArguments)
                    {
                        if (!(nodeArgument.SelectSingleNode("hasdefaultvalue") is XmlNode xmlHasDefaultArgument) ||
                            !bool.TryParse(xmlHasDefaultArgument.InnerText, out bool bHasDefaultArgument))
                        {
                            continue;
                        }

                        if (bHasDefaultArgument &&
                            nodeArgument.SelectSingleNode("defaultvalue") is XmlNode xmlDefaultArgument)
                        {
                            listDefaultArguments.Add(new DefaultFunctionArgument
                            {
                                HasDefaultValue = true,
                                DefaultValue = MathExpressionTokenizer.GetToken(xmlDefaultArgument.InnerText)
                            });
                        }
                        else
                        {
                            listDefaultArguments.Add(new DefaultFunctionArgument
                            {
                                HasDefaultValue = false
                            });
                        }
                    }

                    FunctionArgumentFilter.RegisterFunction(xmlName.InnerText, listDefaultArguments);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Datenklasse, bildet ein Argument ab
        /// </summary>
        internal class DefaultFunctionArgument
        {
            /// <summary>
            /// true, wenn dieses Argument einen Default-Wert hat, ansonsten false.
            /// </summary>
            public bool HasDefaultValue { get; set; }

            /// <summary>
            /// Der Default-Wert, falls definiert, ansonsten null.
            /// </summary>
            public Internals.TermParsing.Token DefaultValue { get; set; }
        }
    }
}
