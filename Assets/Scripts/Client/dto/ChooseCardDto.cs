namespace client.dto
{
    public class ChooseCardDto : IDto
    {
        public byte CardId { get; }

        public ChooseCardDto(byte cardId)
        {
            CardId = cardId;
        }

        public void WriteToStream(CustomMemoryStream ms)
        {
            ms.WriteByte(CardId);
        }

        public void ReadFromStream(CustomMemoryStream ms) { }
    }
}