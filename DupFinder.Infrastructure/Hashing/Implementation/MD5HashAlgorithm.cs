using System;
using System.IO;
using System.Security.Cryptography;
using DupFinder.Infrastructure.Hashing.Interfaces;

namespace DupFinder.Infrastructure.Hashing.Implementation
{
    public class MD5HashAlgorithm : IHashAlgorithm
    {
        public string Calculate(Stream inputStream)
        {
            using (var hash = MD5.Create())
            {
                byte[] checksum = hash.ComputeHash(inputStream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }
    }
}
