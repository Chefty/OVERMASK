using System;

namespace client.dto
{
    [Serializable]
    public class PlayerDto : IDto
    {
        public string PlayerId { get; private set; }
        public string UserName { get; private set; }
        public byte Faction { get; private set; } = 0;
        public byte[] AvailableCards { get; private set; } = Array.Empty<byte>();
        
        public PlayerDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }

        public PlayerDto(string userName)
        {
            PlayerId = Guid.NewGuid().ToString();
            UserName = userName;
            Faction = 0;
        }

        public void WriteToStream(CustomMemoryStream ms)
        {
            ms.WriteString(PlayerId);
            ms.WriteString(UserName);
            ms.WriteByte(Faction);
            ms.WriteByte(byte.Parse(AvailableCards.Length.ToString()));
            for (var i = 0; i < AvailableCards.Length; i++)
                ms.WriteByte(AvailableCards[i]);
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            PlayerId = ms.ReadString();
            UserName = ms.ReadString();
            Faction = ms.ReadByte();
            var cardsLenght = ms.ReadByte();
            AvailableCards = new byte[cardsLenght];
            for (var i = 0; i < cardsLenght; i++)
                AvailableCards[i] = ms.ReadByte();
        }
    }
}