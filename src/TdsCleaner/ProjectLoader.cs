using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TdsCleaner
{
    public static class ProjectLoader
    {
        public static void Process(Options options, Project project)
        {
            project.Document = XDocument.Load(options.InputProjectFile);

            foreach (var itemNode in project.ItemNodes)
            {
                var path = Project.Decode(itemNode.Attribute("Include").Value);
                if (!project.Items.Contains(path))
                {
                    project.Items.Add(path);
                }
                else
                {
                    itemNode.Remove();
                    Log.Info("ProjectLoader", "Removed duplicate item '{0}'", path);
                }
            }
        }
    }
}