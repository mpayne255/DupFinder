using System.IO;

namespace DupFinder.Hashing.Interfaces
{
    public interface IHashAlgorithm
    {
        string Calculate(Stream inputStream);
    }
}
