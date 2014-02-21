using System;
using System.Collections.Generic;


namespace NorwegianBlueReporter
{
    class AppOptions
    {


        public Dictionary<Type, string> InputFileNames { get; private set; }
        public string OutputFileName { get; private set; }
        public string AttachmentsDirectory { get; private set; }
        public string MarkdownNotesFileName { get; private set; }

        public AppOptions(Dictionary<Type, string> inputFileNames, string outputFileName, string attachmentsDirectory, string markdownNotesFileName)
        {
            InputFileNames = inputFileNames;
            OutputFileName = outputFileName;
            AttachmentsDirectory = attachmentsDirectory;
            MarkdownNotesFileName = markdownNotesFileName;
        }
    }
}
