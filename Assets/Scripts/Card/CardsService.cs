using client.dto;

namespace Engine
{
    public class CardsService
    {
        public static CardsService Instance => instance ??= new CardsService();
        private static CardsService instance;

        public byte[][] MaskCards { get; private set; }
        public byte[][] PlayerCards { get; private set; }
        
        public void InjectCardsData(CardsDataDto csd)
        {
            MaskCards = csd.MaskCards;
            PlayerCards = csd.PlayerCards;
        }
    }
}