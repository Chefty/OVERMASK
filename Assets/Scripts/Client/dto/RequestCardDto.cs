namespace client.dto
{
    public class RequestCardDto : IDto
    {
        public byte MaskCardId { get; private set; }
        
        public void WriteToStream(CustomMemoryStream ms) { }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            MaskCardId = ms.ReadByte();
        }
    }
}