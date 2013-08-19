using System.IO;
using System.Xml.Linq;

namespace TdsCleaner
{
    public static class ProjectLoader
    {
        public static void Process(Options options, TdsProject project)
        {
            project.Document = XDocument.Load(options.InputProjectFile);
            project.BaseDirectory = Path.GetDirectoryName(options.InputProjectFile);
            project.BaseItemDirectory = Path.Combine(project.BaseDirectory, "sitecore");

            Log.Info("ProjectLoader", "{0} items in project.", project.Items.Count);
        }
    }
}