using System;
using System.IO;
using System.Linq;

namespace TdsCleaner
{
    public static class ProjectCleaner
    {
        public static void Process(Options options, TdsProject project)
        {
            if (options.RemoveBrokenItemReferences)
            {
                RemoveBrokenReferences(project);
            }

            if (options.RemoveUnusedItemFiles)
            {
                RemoveUnusedFiles(project);
            }

            RemoveEmptyDirectories(project.BaseDirectory, new DirectoryInfo(project.BaseItemDirectory));

            NormalizeIcons(project);
        }

        /// <summary>
        /// Converts all the Icon elements so that they use a standardised format.
        /// </summary>
        /// <param name="project">Project reference</param>
        private static void NormalizeIcons(TdsProject project)
        {
            long counter = 0;

            foreach (var iconElement in project.Items.Select(item => item.ItemNode.Element(TdsProject.Namespaces.MsBuild + "Icon")))
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
        /// <param name="project">Project reference</param>
        private static void RemoveBrokenReferences(TdsProject project)
        {
            var items = project.Items.ToArray();
            foreach (var item in items)
            {
                if (!File.Exists(item.FilePath))
                {
                    project.Items.Remove(item);
                    item.ItemNode.Remove();

                    Log.Info("ProjectCleaner", "Removed broken reference from project: '{0}'", item.ProjectPath);
                }
            }
        }

        /// <summary>
        /// Removes files on the disk which no longer have corresponding <c>Item</c>
        /// elements in the project file.
        /// </summary>
        /// <param name="project">Project reference</param>
        private static void RemoveUnusedFiles(TdsProject project)
        {
            foreach (var file in Directory.GetFiles(project.BaseItemDirectory, "*.item", SearchOption.AllDirectories))
            {
                if (!project.Items.Any(item => item.FilePath.Equals(file, StringComparison.OrdinalIgnoreCase)))
                {
                    project.FilesToBeDeleted.Add(file);

                    Log.Info("ProjectCleaner", "Deleted unreferenced item file: '{0}'", file.Replace(project.BaseItemDirectory, string.Empty));
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

                string relativePath = directory.FullName.Replace(projectRoot, string.Empty);
                Log.Info("ProjectCleaner", "Deleted empty directory: '{0}'", relativePath);
            }
        }
    }
}