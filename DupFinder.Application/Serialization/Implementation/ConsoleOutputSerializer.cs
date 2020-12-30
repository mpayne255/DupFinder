using System;
using System.Collections.Generic;
using System.Text;
using DupFinder.Application.Serialization.Interfaces;
using DupFinder.Domain;

namespace DupFinder.Application.Serialization.Implementation
{
    public class ConsoleOutputSerializer : IOutputSerializer
    {
        public void Write(Bucket bucket)
        {
            throw new NotImplementedException();
        }
    }
}
