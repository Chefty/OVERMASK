namespace Engine
{
    public class Card
    {
        public int CardId { get; private set; }
        public byte[] Data { get; private set; }

        public Card(int cardId, byte[] data)
        {
            CardId = cardId;
            Data = data;
        }
    }
}