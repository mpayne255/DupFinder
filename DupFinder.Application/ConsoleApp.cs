using System;
using System.Diagnostics;
using System.IO;
using DupFinder.Application.Services.Interfaces;
using DupFinder.Domain;
using DupFinder.Infrastructure.Serialization.Interfaces;

namespace DupFinder.Application
{
    public class ConsoleApp
    {
        private Configuration _configuration;
        private IDuplicateService _duplicateService;
        private IOutputSerializer<DuplicateResult> _outputSerializer;
        private StreamWriter _writer;

        public ConsoleApp(IOutputSerializer<DuplicateResult> serializer, Configuration configuration, IDuplicateService duplicateService)
        {
            _outputSerializer = serializer;
            _duplicateService = duplicateService;
            _configuration = configuration;

            _writer = GetStreamWriter();
        }

        public void Run(string[] args)
        {
            if (_configuration.ShowUsage)
            {
                ShowUsage();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            var duplicateResult = _duplicateService.GetDuplicates();

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time(ms): {stopwatch.ElapsedMilliseconds}");

            _outputSerializer.Write(_writer, duplicateResult);

            _writer.Flush();
        }

        private StreamWriter GetStreamWriter()
        {
            switch(_configuration.OutputMode)
            {
                default:
                case Domain.Enums.OutputMode.Console:
                    return new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                case Domain.Enums.OutputMode.Json:
                case Domain.Enums.OutputMode.Xml:
                    return new StreamWriter(_configuration.OutputTarget);
            }
        }

        public void ShowUsage()
        {
            Console.WriteLine("DupFinder [options] <directory1, directory2, ..., directoryN>");
            Console.WriteLine("Find duplicate files or folders in the specified directories");
            Console.WriteLine("Options:");
            Console.WriteLine("   --mode, -m             file (default) or directory (not implemented)");
            Console.WriteLine("   --output-mode, -om     xml or json, omit for console output");
            Console.WriteLine("   --output, -o           full path to the output file, omit for console output");
            Console.WriteLine("   --include-empty -i     include empty files/directories (excluded by default)");
        }
    }
}
