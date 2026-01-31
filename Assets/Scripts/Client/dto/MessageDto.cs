using System;

namespace client.dto
{
    [Serializable]
    public class MessageDto : IDto
    {
        private readonly IDto data;
        private string type;

        public MessageDto(string type, IDto data = null)
        {
            this.type = type;
            this.data = data;
        }

        public void WriteToStream(CustomMemoryStream ms)
        {
            ms.WriteString(type);
            data?.WriteToStream(ms);
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            type = ms.ReadString();
        }
    }
}