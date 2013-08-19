using System;
using System.IO;

namespace TdsCleaner
{
    internal class Program
    {
        public static Options Options { get; set; }

        public static TdsProject Project { get; set; }

        private static void Main(string[] args)
        {
            Options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, Options))
            {
                PrintUsage(false);
                return;
            }

            if (!File.Exists(Options.InputProjectFile))
            {
                Console.WriteLine("Input project file could not be found.");
                PrintUsage(true);
                return;
            }

            Options.InputProjectFile = Path.GetFullPath(Options.InputProjectFile);
            Options.OutputProjectFile = string.IsNullOrEmpty(Options.OutputProjectFile)
                                            ? Options.InputProjectFile
                                            : Path.GetFullPath(Options.OutputProjectFile);

            if (File.Exists(Options.OutputProjectFile) && Options.OutputProjectFile != Options.InputProjectFile)
            {
                File.Delete(Options.OutputProjectFile);
            }

            Project = new TdsProject();
            ProjectLoader.Process(Options, Project);
            ProjectCleaner.Process(Options, Project);
            ProjectSorter.Process(Options, Project);
            ProjectWriter.Process(Options, Project);

            Console.ReadKey(true);
        }

        private static void PrintUsage(bool outputUsage)
        {
            if (outputUsage)
            {
                CommandLine.Parser.Default.Settings.HelpWriter.WriteLine(Options.GetUsage());
            }

            Console.Write("Press an key to exit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}