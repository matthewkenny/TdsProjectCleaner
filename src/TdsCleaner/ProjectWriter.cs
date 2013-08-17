using System.IO;

namespace TdsCleaner
{
    public static class ProjectWriter
    {
        public static void Process(Options options, Project project)
        {
            if (!options.TestRun)
            {
                project.Document.Save(options.OutputProjectFile);

                foreach (var file in project.FilesToBeDeleted)
                {
                    File.Delete(file);
                }

                foreach (var directory in project.DirectoriesToBeDeleted)
                {
                    Directory.Delete(directory);
                }
            }
        }
    }
}