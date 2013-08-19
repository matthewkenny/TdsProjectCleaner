using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TdsCleaner
{
    public static class ProjectSorter
    {
        public static void Process(Options options, TdsProject project)
        {
            project.ItemRoot.ReplaceWith(Reorder(project.ItemRoot));
        }

        private static XElement Reorder(XElement root)
        {
            return new XElement(root.Name,
                                from el in root.Elements()
                                orderby GetOrderKey(el)
                                select Reorder(el),
                                root.Attributes(),
                                root.Nodes().Where(n => n.NodeType == XmlNodeType.Text || n.NodeType == XmlNodeType.Whitespace));
        }

        private static string GetOrderKey(XElement element)
        {
            if (element.Name.LocalName == "SitecoreItem")
            {
                return element.Attribute("Include").Value;
            }

            if (element.Name.LocalName == "Icon")
            {
                return "a";
            }

            if (element.Name.LocalName == "ChildItemSynchronization")
            {
                return "d";
            }

            if (element.Name.LocalName == "ItemDeployment")
            {
                return "g";
            }

            if (element.Name.LocalName == "SitecoreName")
            {
                return "j";
            }

            if (element.Name.LocalName == "ExcludeItemFrom")
            {
                return "n";
            }

            return element.Name.LocalName;
        }
    }
}