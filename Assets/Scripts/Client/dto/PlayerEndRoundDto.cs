namespace client.dto
{
    public class PlayerEndRoundDto : IDto
    {
        public string PlayerId { get; private set; }
        public byte PlayerCardId { get; private set; }
        public byte PlayerScore { get; private set; }
        public byte NewCardId { get; private set; }
        public string PlayerIdOnBottom { get; private set; }
        
        public PlayerEndRoundDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }
        
        public void WriteToStream(CustomMemoryStream ms)
        {
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            PlayerId = ms.ReadString();
            PlayerCardId = ms.ReadByte();
            PlayerScore = ms.ReadByte();
            NewCardId = ms.ReadByte();
            PlayerIdOnBottom = ms.ReadString();
        }
    }
}