using System;
using System.Xml;
using Newtonsoft.Json;

namespace Parchmentum.XmlConverter;

public static class XmlConverter
{
    public static string ToJson(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var root = doc.DocumentElement ?? throw new Exception("Missing root element");

        root.SetAttribute("xmlns:json", "http://james.newtonking.com/projects/json");

        TurnNestedObjectIntoArray(root, root["tags"]);
        TurnNestedObjectIntoArray(root, root["comments"]);

        return JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.Indented, true);
    }

    private static void TurnNestedObjectIntoArray(XmlElement root, XmlElement? element)
    {
        if (element == null)
            return;

        var isSingleNode = element.ChildNodes.Count == 1;

        root.RemoveChild(element);

        foreach (XmlElement child in element.ChildNodes)
        {
            XmlElement newChild;

            if (child.HasChildNodes)
                newChild = Clone(child, element.Name);
            else
            {
                newChild = root.OwnerDocument.CreateElement(element.Name);
                newChild.InnerText = child.InnerText;
            }

            if (isSingleNode)
            {
                var attribute = root.OwnerDocument.CreateAttribute("json", "Array", "http://james.newtonking.com/projects/json");
                attribute.InnerText = "true";
                newChild.Attributes.Append(attribute);
            }

            root.AppendChild(newChild);
        }
    }

    private static XmlElement Clone(XmlElement node, string newName)
    {
        var newNode = node.OwnerDocument.CreateElement(newName);

        // since I know only comments in the source xml have attributes, take this opportunity to convert them to elements
        foreach (XmlAttribute attribute in node.Attributes)
        {
            var newAttribute = node.OwnerDocument.CreateElement(attribute.Name);
            newAttribute.InnerText = attribute.InnerText;
            newNode.AppendChild(newAttribute);
        }

        foreach (XmlNode childNode in node.ChildNodes)
            newNode.AppendChild(childNode.CloneNode(true));

        return newNode;
    }
}
