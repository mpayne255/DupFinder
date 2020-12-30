using System;
using System.Collections.Generic;
using System.Text;
using DupFinder.Domain;

namespace DupFinder.Application.Serialization.Interfaces
{
    public interface IOutputSerializer
    {
        void Write(Bucket bucket);
    }
}
