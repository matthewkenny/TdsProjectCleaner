using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TdsCleaner
{
    public static class ProjectCleaner
    {
        public static void Process(Options options, Project project)
        {
            var projectRoot = Path.GetDirectoryName(options.InputProjectFile) ?? string.Empty;

            if (options.RemoveBrokenItemReferences)
            {
                RemoveBrokenReferences(projectRoot, project);
            }

            if (options.RemoveUnusedItemFiles)
            {
                RemoveUnusedFiles(projectRoot, project);
            }

            RemoveEmptyDirectories(projectRoot, new DirectoryInfo(Path.Combine(projectRoot, "sitecore")));

            NormalizeIcons(project);
        }

        /// <summary>
        /// Converts all the Icon elements so that they use a standardised format.
        /// </summary>
        /// <param name="project"></param>
        private static void NormalizeIcons(Project project)
        {
            long counter = 0;

            foreach (var iconElement in project.ItemNodes.Select(itemNode => itemNode.Element(Project.Namespaces.MsBuild + "Icon")))
            {
                if (iconElement.Value.StartsWith("/temp/IconCache/"))
                {
                    counter++;
                    iconElement.Value = iconElement.Value.Replace("/temp/IconCache/", "/~/icon/") + ".aspx";
                }
            }

            Log.Info("ProjectCleaner", "Normalised {0:N0} icon entries.", counter);
        }

        /// <summary>
        /// Removes <c>Item</c> elements in the project that point to items which
        /// no longer exist on the disk.
        /// </summary>
        /// <param name="projectRoot"></param>
        /// <param name="project"></param>
        private static void RemoveBrokenReferences(string projectRoot, Project project)
        {
            foreach (var itemNode in project.ItemNodes)
            {
                var path = itemNode.Attribute("Include").Value;
                var fullPath = Path.Combine(projectRoot, path);

                if (!File.Exists(fullPath))
                {
                    project.Items.Remove(path);
                    itemNode.Remove();

                    Log.Info("ProjectCleaner", "Removed broken reference from project: '{0}'", path);
                }
            }
        }

        /// <summary>
        /// Removes files on the disk which no longer have corresponding <c>Item</c>
        /// elements in the project file.
        /// </summary>
        /// <param name="projectRoot"></param>
        /// <param name="project"></param>
        private static void RemoveUnusedFiles(string projectRoot, Project project)
        {
            foreach (var file in Directory.GetFiles(projectRoot, "*.item", SearchOption.AllDirectories))
            {
                string relativePath = Project.Decode(file.Replace(projectRoot, ""));
                if (!project.Items.Contains(relativePath.TrimStart('\\')))
                {
                    project.FilesToBeDeleted.Add(file);

                    Log.Info("ProjectCleaner", "Deleted unreferenced item file: '{0}'", relativePath);
                }
            }
        }

        /// <summary>
        /// Deletes a directory if it contains no files or subdirectories.
        /// </summary>
        /// <param name="directory"></param>
        private static void RemoveEmptyDirectories(string projectRoot, DirectoryInfo directory)
        {
            foreach (var directoryInfo in directory.GetDirectories())
            {
                RemoveEmptyDirectories(projectRoot, directoryInfo);
            }

            if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
            {
                directory.Delete();

                string relativePath = directory.FullName.Replace(projectRoot, "");
                Log.Info("ProjectCleaner", "Deleted empty directory: '{0}'", relativePath);
            }
        }
    }
}