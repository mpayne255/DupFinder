using System.IO;
using DupFinder.Infrastructure.Serialization.Interfaces;
using Newtonsoft.Json;

namespace DupFinder.Infrastructure.Serialization.Implementation
{
    public class JsonOutputSerializer<T> : IOutputSerializer<T>
    {
        private JsonSerializer _serializer = new JsonSerializer() { Formatting = Formatting.Indented };

        public void Write(StreamWriter streamWriter, T value)
        {
            _serializer.Serialize(streamWriter, value, typeof(T));
        }
    }
}
