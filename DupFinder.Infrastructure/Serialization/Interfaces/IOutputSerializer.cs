using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DupFinder.Infrastructure.Serialization.Interfaces
{
    public interface IOutputSerializer<T>
    {
        void Write(StreamWriter streamWriter, T value);
    }
}
