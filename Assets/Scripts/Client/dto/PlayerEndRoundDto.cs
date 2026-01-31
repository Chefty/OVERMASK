namespace client.dto
{
    public class PlayerEndRoundDto : IDto
    {
        public string ConnectionId { get; private set; }
        public byte PlayerCardId { get; private set; }
        public byte PlayerScore { get; private set; }
        public byte NewCardId { get; private set; }
        
        public PlayerEndRoundDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }
        
        public void WriteToStream(CustomMemoryStream ms)
        {
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            ConnectionId = ms.ReadString();
            PlayerCardId = ms.ReadByte();
            PlayerScore = ms.ReadByte();
            NewCardId = ms.ReadByte();
        }
    }
}