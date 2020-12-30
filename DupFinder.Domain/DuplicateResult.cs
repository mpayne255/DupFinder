using System.Collections.Generic;
using System.Text;

namespace DupFinder.Domain
{
    public class DuplicateResult
    {
        public IEnumerable<Bucket> Buckets { get; set; }

        public override string ToString()
        {
            if(Buckets == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();

            foreach(var bucket in Buckets)
            {
                stringBuilder.Append(bucket.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
