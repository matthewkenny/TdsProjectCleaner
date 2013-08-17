using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace TdsCleaner
{
    class Program
    {
        public static Options Options { get; set; }

        public static Project Project { get; set; }


        static void Main(string[] args)
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

            Project = new Project();
            ProjectLoader.Process(Options, Project);
            ProjectCleaner.Process(Options, Project);
            ProjectSorter.Process(Options, Project);
            ProjectWriter.Process(Options, Project);
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
