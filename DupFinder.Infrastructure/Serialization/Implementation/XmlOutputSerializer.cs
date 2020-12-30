using System.IO;
using DupFinder.Infrastructure.Serialization.Interfaces;
using XSerializer;

namespace DupFinder.Infrastructure.Serialization.Implementation
{
    public class XmlOutputSerializer<T> : IOutputSerializer<T>
    {
        private XmlSerializer<T> _serializer = new XmlSerializer<T>(options => options.Indent());

        public void Write(StreamWriter streamWriter, T value)
        {
            _serializer.Serialize(streamWriter, value);
        }
    }
}
