using System;
using System.Collections.Generic;
using System.Text;
using DupFinder.Infrastructure.Serialization.Interfaces;

namespace DupFinder.Infrastructure.Serialization.Implementation
{
    public class XmlOutputSerializer<T> : IOutputSerializer<T>
    {
        public void Write(T value)
        {
            throw new NotImplementedException();
        }
    }
}
