using System.IO;
using System.Xml.Linq;

namespace TdsCleaner
{
    public class TdsItem
    {
        public string ProjectPath { get; set; }

        public string FilePath { get; set; }

        public XElement ItemNode { get; set; }

        public TdsItem(TdsProject project, XElement itemNode)
        {
            ItemNode = itemNode;
            ProjectPath = ItemNode.Attribute("Include").Value;
            FilePath = Path.Combine(project.BaseDirectory, TdsUtil.DecodePath(ProjectPath));
        }
    }
}