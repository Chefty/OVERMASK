namespace client.dto
{
    public class DealInitialCardsDto : IDto
    {
        public byte[] CardIds { get; private set; }
            
        public void WriteToStream(CustomMemoryStream ms)
        {
            
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            var length = ms.ReadByte();
            CardIds = new byte[length];
            for (var i = 0; i < length; i++)
                CardIds[i] = ms.ReadByte();
        }
    }
}