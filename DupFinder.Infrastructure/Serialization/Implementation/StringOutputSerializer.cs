using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DupFinder.Infrastructure.Serialization.Interfaces;

namespace DupFinder.Infrastructure.Serialization.Implementation
{
    public class StringOutputSerializer<T> : IOutputSerializer<T> 
    {
        public void Write(StreamWriter streamWriter, T value)
        {
            streamWriter.WriteLine(value.ToString());
        }
    }
}
