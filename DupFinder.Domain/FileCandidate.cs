using System;
using System.IO;
using Infrastructure.Interfaces;

namespace DupFinder.Domain
{
    public class FileCandidate : IComparable<FileCandidate>
    {
        private readonly int BufferSize = 2 * 1024 * 1024;
        private readonly IHashAlgorithm _hashAlgorithm;

        private string _fileHash;
        public string FileHash
        {
            get
            {
                if(_fileHash == null)
                {
                    _fileHash = GetSignature(FullName);
                }
                return _fileHash;
            }
            set
            {
                _fileHash = value;
            }
        }
        public string FullName { get; set; }
        public long Size { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public FileCandidate(string fullName, IHashAlgorithm hashAlgorithm)
        {
            FullName = fullName;
            _hashAlgorithm = hashAlgorithm;

            Initialize();
        }

        public int CompareTo(FileCandidate other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            if (Size != other.Size)
            {
                return Size.CompareTo(other.Size);
            }

            return FileHash.CompareTo(GetSignature(other.FullName));
        }

        private void Initialize()
        {
            var file = new FileInfo(FullName);
            Size = file.Length;
            LastModifiedDate = file.LastWriteTimeUtc;
        }

        protected virtual string GetSignature(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.None))
            {
                var hash = _hashAlgorithm.Calculate(stream);
                return hash;
            }
        }
    }
}
