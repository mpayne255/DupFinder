using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DupFinder.Domain;
using DupFinder.Infrastructure.Hashing.Interfaces;
using DupFinder.Infrastructure.Serialization.Interfaces;

namespace DupFinder.Application
{
    public class ConsoleApp
    {
        private ConcurrentDictionary<string, Bucket> _allItems;
        private Configuration _configuration;
        private IHashAlgorithm _hashAlgorithm;
        private IOutputSerializer<Bucket> _outputSerializer;

        public ConsoleApp(IHashAlgorithm hashAlgorithm, IOutputSerializer<Bucket> serializer, Configuration configuration)
        {
            _hashAlgorithm = hashAlgorithm;
            _outputSerializer = serializer;
            _configuration = configuration;
        }

        public void Run(string[] args)
        {
            _allItems = new ConcurrentDictionary<string, Bucket>();

            if (_configuration == null)
            {
                ShowUsage();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            var results = GetDuplicates();

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time(ms): {stopwatch.ElapsedMilliseconds}");

            foreach (var result in results)
            {
                _outputSerializer.Write(result);
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

        public IEnumerable<Bucket> GetDuplicates()
        {
            _configuration.Directories.AsParallel().ForAll(path =>
            {
                FindRecursive(path);
            });

            var duplicates = _allItems.Where(kvp => kvp.Value.Duplicates.Count > 1)
                .Select(kvp => kvp.Value);
            return duplicates;
        }

        // TODO: Replace direct file system calls with a new component/interface
        protected void FindRecursive(string filePath)
        {
            var dir = new DirectoryInfo(filePath);
            var results = dir.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);

            results.AsParallel().ForAll(fsInfo =>
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
            });
        }
    }
}
