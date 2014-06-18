using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Reporter
{
    public class AppOptions : DynamicObject
    {
        private readonly Dictionary<object, object> _configuration;

        public AppOptions(string[] args)
        {
            _configuration = YamlParser.GetConfiguration();

            // Parse command line arguments
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!string.IsNullOrEmpty(options.OutputFileName))
                {
                    Console.WriteLine("File saved as {0}", options.OutputFileName);
                    _configuration["OutputFileName"] = options.OutputFileName;
                }

                if (!string.IsNullOrEmpty(options.AttachmentsDirectory))
                {
                    Console.WriteLine("Attachments taken from {0}", options.AttachmentsDirectory);
                    _configuration["AttachmentsDirectory"] = options.AttachmentsDirectory;
                }

                if (!string.IsNullOrEmpty(options.Markdown))
                {
                    Console.WriteLine("Using markdown file {0}", options.Markdown);
                    _configuration["MarkdownNotesFileName"] = options.Markdown;
                }
            }
            else
            {
                throw new ArgumentException("Invalid command line arguments!");
            }

            // Parameter checking
            if (!_configuration.ContainsKey("AttachmentsDirectory"))
            {
                _configuration["AttachmentsDirectory"] = null;
            }

            // Parameter checking
            if (!_configuration.ContainsKey("OutputFileName") || string.IsNullOrEmpty((string)_configuration["OutputFileName"]))
            {
                throw new ArgumentException("Missing output filename");
            }


            if (!_configuration.ContainsKey("MarkdownNotesFileName"))
            {
                _configuration["MarkdownNotesFileName"] = null;
            }
            else
            {
                if (!string.IsNullOrEmpty((string)_configuration["MarkdownNotesFileName"]) && !File.Exists((string)_configuration["MarkdownNotesFileName"]))
                {
                    throw new FileNotFoundException("Markdown file was not found ...");
                }
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _configuration.TryGetValue(binder.Name, out result);
        }
    }
}
