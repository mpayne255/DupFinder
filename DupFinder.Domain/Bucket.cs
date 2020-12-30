using System.Collections.Concurrent;

namespace DupFinder.Domain
{
    public class Bucket
    {
        public string BucketID { get; set; }
        public ConcurrentBag<FileCandidate> Duplicates { get; set; }
    }
}
