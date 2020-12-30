using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DupFinder.Domain;
using Infrastructure.Interfaces;

namespace DupFinder.Application
{
    public class ConsoleApp
    {
        protected ConcurrentDictionary<string, Bucket> _allItems;
        protected Configuration _configuration;
        protected IHashAlgorithm _hashAlgorithm;

        public ConsoleApp(IHashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public void Run(string[] args)
        {
            _allItems = new ConcurrentDictionary<string, Bucket>();

            _configuration = GetConfiguration(args);

            if (_configuration == null)
            {
                ShowUsage();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            var results = GetDuplicates();

            stopwatch.Stop();

            // TODO: Replace output with use of IOutputSerializer

            Console.WriteLine($"Elapsed time(ms): {stopwatch.ElapsedMilliseconds}");

            foreach (var result in results)
            {
                Console.WriteLine("+-------");

                foreach (var fsInfo in result.OrderBy(fsInfo => fsInfo.FullName))
                {
                    Console.WriteLine(fsInfo.FullName);
                }
            }
        }

        public Configuration GetConfiguration(string[] args)
        {
            var config = Configuration.Build(args);
            return config;
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

        public IEnumerable<ConcurrentBag<FileCandidate>> GetDuplicates()
        {
            _configuration.Directories.AsParallel().ForAll(path =>
            {
                FindRecursive(path);
            });

            var duplicates = _allItems.Where(kvp => kvp.Value.Duplicates.Count > 1)
                .Select(kvp => kvp.Value.Duplicates);
            return duplicates;
        }

        // TODO: Replace direct file system calls with a new component/interface
        protected void FindRecursive(string filePath)
        {
            var dir = new DirectoryInfo(filePath);
            var results = dir.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);

            results.AsParallel().ForAll(fsInfo =>
            {
                try
                {
                    if (fsInfo.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        FindRecursive(fsInfo.FullName);
                    }
                    else
                    {
                        var info = new FileCandidate(fsInfo.FullName, _hashAlgorithm);
                        if (info.Size > 0 || (info.Size == 0 && _configuration.IncludeEmpty))
                        {
                            _allItems.AddOrUpdate(info.FileHash, newBucket =>
                                new Bucket 
                                { 
                                    BucketID = info.FileHash,
                                    Duplicates = new ConcurrentBag<FileCandidate>(new[] { info })
                                },
                                (sig, bucket) =>
                                {
                                    bucket.Duplicates.Add(info);
                                    return bucket;
                                }
                            );
                        }
                    }
                }
                catch
                {
                    // TODO: log error...
                }
            });
        }
    }
}
