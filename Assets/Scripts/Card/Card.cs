namespace Engine
{
    public class Card : ICardData
    {
        public byte CardId { get; private set; }
        public CardCellDefinition[][] ArrayData => CardDataConverter.ToArray(Data);
        public byte[] Data { get; private set; }

        public Card(byte cardId, byte[] data)
        {
            CardId = cardId;
            Data = data;
        }
    }
}