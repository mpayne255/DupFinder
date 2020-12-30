using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DupFinder.Domain
{
    public class Bucket
    {
        public string BucketID { get; set; }
        public ConcurrentBag<FileCandidate> Duplicates { get; set; }
    }
}
