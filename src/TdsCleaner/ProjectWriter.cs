using System.IO;

namespace TdsCleaner
{
    public static class ProjectWriter
    {
        public static void Process(Options options, TdsProject project)
        {
            Log.Info("ProjectWriter", "{0} items in project.", project.Items.Count);

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