using System.IO;

namespace DupFinder.Infrastructure.Hashing.Interfaces
{
    public interface IHashAlgorithm
    {
        string Calculate(Stream inputStream);
    }
}
