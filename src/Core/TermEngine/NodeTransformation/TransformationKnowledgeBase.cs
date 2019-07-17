using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation
{
    internal static class TransformationKnowledgeBase
    {
        private const string DIR = "./KnowledgeBase/TransformationRules";

        private static List<TransformationRule> _listTransformationRules;
        private static string[] _arrTransformationRuleWhitelist;

        static TransformationKnowledgeBase()
        {
            if (!Directory.Exists(DIR))
            {
                return;
            }

            _listTransformationRules = new List<TransformationRule>(0);
            foreach (string strFile in Directory.GetFiles(DIR))
            {
                LoadKnowledgeBaseFromFile(strFile);
            }
        }

        public static List<TransformationRule> GetTransformationRules()
        {
            if (_arrTransformationRuleWhitelist != null)
            {
                List <TransformationRule> listFiltered = new List<TransformationRule>(_arrTransformationRuleWhitelist.Length);
                foreach (TransformationRule rule in _listTransformationRules)
                {
                    foreach (string strFilter in _arrTransformationRuleWhitelist)
                    {
                        if (rule.InternalName.Contains(strFilter))
                        {
                            listFiltered.Add(rule);
                        }
                    }
                    
                }

                return listFiltered;
            }
            else
            {
                return _listTransformationRules;
            }
        }

        internal static void RemoveKnowledgeFilters()
        {
            _arrTransformationRuleWhitelist = null;
        }

        internal static void SuppressKnowledgeWithWhitelist(params string[] arrWhitelist)
        {
            _arrTransformationRuleWhitelist = arrWhitelist;
        }

        private static void LoadKnowledgeBaseFromFile(string strFile)
        {
            ValidationEventHandler eventHandler = new ValidationEventHandler((sender, e) =>
            {
                throw e.Exception;
            });
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("http://markellus.de/de.markellus.math", DIR + "/TransformationRules.xsd");
            settings.ValidationType = ValidationType.Schema;
            XmlReader reader = XmlReader.Create(strFile, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            doc.Validate(eventHandler);
            XmlNode root = doc.DocumentElement;

            if (root?.Name != "knowledgebase")
            {
                return;
            }

            XmlNode xmlMeta = root.SelectSingleNode("meta");

            XmlNode xmlRules = root.SelectSingleNode("rules");

            if (xmlRules != null && xmlRules.HasChildNodes && xmlRules.SelectNodes("rule") is XmlNodeList listRules)
            {
                foreach (XmlNode node in listRules)
                {
                    TransformationRule ruleIn = new TransformationRule(node, false);

                    if (!ruleIn.Valid)
                    {
                        Console.WriteLine($"Warning: Invalid rule: {ruleIn}");
                        continue;
                    }

                    _listTransformationRules.Add(ruleIn);

                    bool.TryParse(
                        node.SelectSingleNode("properties")?.SelectSingleNode("twoway")?.InnerText ?? "true",
                        out bool bTwoWay);

                    if (bTwoWay)
                    {
                        TransformationRule ruleOut = new TransformationRule(node, true);
                        _listTransformationRules.Add(ruleOut);
                    }
                }
            }
        }
    }
}
