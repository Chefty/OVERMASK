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

        public string ReadString()
        {
            var len = ReadByte();
            var buffer = new byte[len];
            memoryStream.Read(buffer, 0, len);
            return LocalEncoding.GetString(buffer);
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

        public void WriteInt(int value)
        {
            memoryStream.WriteByte((byte)(value >> 24));
            memoryStream.WriteByte((byte)(value >> 16));
            memoryStream.WriteByte((byte)(value >> 8));
            memoryStream.WriteByte((byte)value);
        }
        
        public int ReadInt()
        {
            int b1 = ReadByte();
            int b2 = ReadByte();
            int b3 = ReadByte();
            int b4 = ReadByte();

            return (b1 << 24) | (b2 << 16) | (b3 << 8) | b4;
        }

        public byte[] ToArray()
        {
            return memoryStream.ToArray();
        }
    }
}