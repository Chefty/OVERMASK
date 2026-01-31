namespace client.dto
{
    public class CardsDataDto : IDto
    {
        public byte[][] MaskCards { get; private set; }
        public byte[][] PlayerCards { get; private set; }

        public CardsDataDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }
        
        public void WriteToStream(CustomMemoryStream ms) {}

        public void ReadFromStream(CustomMemoryStream ms)
        {
            var length = ms.ReadByte();
            MaskCards = new byte[length][];

            for (var i = 0; i < length; i++)
            {
                var innerLength = ms.ReadByte();
                MaskCards[i] = new byte[innerLength];
                for (var j = 0; j < innerLength; j++)
                    MaskCards[i][j] = ms.ReadByte();
            }
            
            length = ms.ReadByte();
            PlayerCards = new byte[length][];

            for (var i = 0; i < length; i++)
            {
                var innerLength = ms.ReadByte();
                PlayerCards[i] = new byte[innerLength];
                for (var j = 0; j < innerLength; j++)
                    PlayerCards[i][j] = ms.ReadByte();
            }
        }
    }
}