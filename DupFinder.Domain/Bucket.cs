using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace DupFinder.Domain
{
    public class Bucket
    {
        public string BucketID { get; set; }
        public ConcurrentBag<FileCandidate> Duplicates { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder($"+------------------------------------------------------------------------------{Environment.NewLine}");

            foreach (var duplicate in Duplicates.OrderBy(d => d.FullName))
            {
                stringBuilder.AppendLine(duplicate.FullName);
            }

            return stringBuilder.ToString();
        }
    }
}
