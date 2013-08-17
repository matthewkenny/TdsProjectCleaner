using System;
using CommandLine;
using CommandLine.Text;

namespace TdsCleaner
{
    public class Options
    {
        [Option('i', Required = true, HelpText = "Input project file")]
        public string InputProjectFile { get; set; }

        [Option('o', DefaultValue = null, Required = false, HelpText = "Output project file")]
        public string OutputProjectFile { get; set; }

        [Option('r', "remove-references", DefaultValue = false, HelpText = "If present, will remove any item references in the project that are missing files.")]
        public bool RemoveBrokenItemReferences { get; set; }

        [Option('f', "remove-files", DefaultValue = false, HelpText = "If present, will remove any item files from the disk that are not referenced in the project.")]
        public bool RemoveUnusedItemFiles { get; set; }

        [Option('s', "sort", DefaultValue = false, HelpText = "If present, sorts the project file so that items are listed alphabetically.")]
        public bool SortFile { get; set; }

        [Option('t', DefaultValue = false, HelpText = "If present, will not actually make any changes.")]
        public bool TestRun { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}