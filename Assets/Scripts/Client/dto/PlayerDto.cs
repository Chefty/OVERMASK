using System;

namespace client.dto
{
    [Serializable]
    public class PlayerDto : IDto
    {
        public string ConnectionId { get; private set; }
        public string UserName { get; private set; }
        public byte[] AvailableCards { get; private set; }
        
        public PlayerDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }

        public PlayerDto(string userName)
        {
            ConnectionId = Guid.NewGuid().ToString();
            UserName = userName;
        }

        public void WriteToStream(CustomMemoryStream ms)
        {
            ms.WriteString(ConnectionId);
            ms.WriteString(UserName);
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            ConnectionId = ms.ReadString();
            UserName = ms.ReadString();
            var cardsLenght = ms.ReadByte();
            AvailableCards = new byte[cardsLenght];
            for (var i = 0; i < cardsLenght; i++)
                AvailableCards[i] = ms.ReadByte();
        }
    }
}