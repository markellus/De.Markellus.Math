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
using System.IO;
using System.Xml;
using De.Markellus.Maths.Internals.TermParsing;
using De.Markellus.Maths.Internals.TermProcessing;

namespace De.Markellus.Maths.KnowledgeBase.Algebra.Nodes
{
    /// <summary>
    /// Wissensbasis für das Umwandeln von mathemnatischen Termen in eine Baumstruktur
    /// </summary>
    internal static class NodeKnowledgeBase
    {
        private const string DIR = "./KnowledgeBase/Algebra/Nodes";

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

                XmlNode xmlRules = root.SelectSingleNode("nodes");

                if (xmlRules == null || !xmlRules.HasChildNodes ||
                    !(xmlRules.SelectNodes("node") is XmlNodeList listTokens))
                {
                    return;
                }

                foreach (XmlNode node in listTokens)
                {
                    XmlNode xmlType = node.SelectSingleNode("type");
                    XmlNode xmlValue = node.SelectSingleNode("value");
                    XmlNode xmlIsDynamicValue = node.SelectSingleNode("dynamicvalue");
                    XmlNode xmlChildren = node.SelectSingleNode("children");
                    XmlNode xmlChildrenCount = xmlChildren?.SelectSingleNode("count");


                    if (xmlType != null && xmlValue != null && xmlIsDynamicValue != null && xmlChildrenCount != null &&
                        Enum.TryParse(typeof(TokenType), xmlType.InnerText, out object tt) &&
                        bool.TryParse(xmlIsDynamicValue.InnerText, out bool bDnamic) &&
                        int.TryParse(xmlChildrenCount.InnerText, out int iChildrenCount))
                    {
                        NodeFactory.AddTemplate(new NodeTemplate((TokenType)tt, xmlValue.InnerText, bDnamic, iChildrenCount));
                    }
                    else
                    {
                        Console.WriteLine($"Invalid knowledge base entry: {node.InnerXml}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}