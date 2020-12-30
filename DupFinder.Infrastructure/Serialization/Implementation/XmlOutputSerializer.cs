using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DupFinder.Infrastructure.Serialization.Interfaces;

namespace DupFinder.Infrastructure.Serialization.Implementation
{
    public class XmlOutputSerializer<T> : IOutputSerializer<T>
    {
        // TODO: this won't currently work with the Bucket class due to serialization error
        private XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public void Write(StreamWriter streamWriter, T value)
        {
            _serializer.Serialize(streamWriter, value);
        }
    }
}
