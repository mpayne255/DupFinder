using System.IO;

namespace Infrastructure.Interfaces
{
    public interface IHashAlgorithm
    {
        string Calculate(Stream inputStream);
    }
}
