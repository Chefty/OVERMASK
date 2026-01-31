using System;
using System.IO;
using System.Text;

namespace client
{
    public class CustomMemoryStream : IDisposable
    {
        private static readonly Encoding LocalEncoding = Encoding.UTF8;
        private readonly MemoryStream memoryStream;

        public CustomMemoryStream()
        {
            memoryStream = new MemoryStream();
        }

        public CustomMemoryStream(byte[] buffer)
        {
            memoryStream = new MemoryStream(buffer);
        }

        public void Dispose()
        {
            memoryStream.Dispose();
        }

        public void WriteString(string value)
        {
            var length = (byte)value.Length;
            var bytes = Encoding.ASCII.GetBytes(value);

            memoryStream.WriteByte(length);
            memoryStream.Write(bytes, 0, bytes.Length);
        }

        public void WriteStream(CustomMemoryStream ms)
        {
            var data = ms.ToArray();
            var length = (byte)data.Length;

            memoryStream.WriteByte(length);
            memoryStream.Write(data, 0, data.Length);
        }

        public void WriteByte(byte value)
        {
            memoryStream.WriteByte(value);
        }

        public byte ReadByte()
        {
            return (byte)memoryStream.ReadByte();
        }

        public string ReadString()
        {
            var len = ReadByte();
            var buffer = new byte[len];
            memoryStream.Read(buffer, 0, len);
            return LocalEncoding.GetString(buffer);
        }

        public byte[] ToArray()
        {
            return memoryStream.ToArray();
        }
    }
}