using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using DupFinder.Application.Services.Interfaces;
using DupFinder.Domain;
using DupFinder.Infrastructure.Hashing.Interfaces;

namespace DupFinder.Application.Services.Implementation
{
    public class DuplicateService : IDuplicateService
    {
        private Configuration _configuration;
        private ConcurrentDictionary<string, Bucket> _allItems = new ConcurrentDictionary<string, Bucket>();
        private IHashAlgorithm _hashAlgorithm;

        public DuplicateService(IHashAlgorithm hashAlgorithm, Configuration configuration)
        {
            _hashAlgorithm = hashAlgorithm;
            _configuration = configuration;
        }

        public DuplicateResult GetDuplicates()
        {
            _configuration.Directories.AsParallel().ForAll(path =>
            {
                FindRecursive(path);
            });

            var duplicateBuckets = _allItems.Where(kvp => kvp.Value.Duplicates.Count > 1)
                .Select(kvp => kvp.Value);
            var duplicateResult = new DuplicateResult { Buckets = duplicateBuckets };

            return duplicateResult;
        }

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
