using System;
using client.dto;

namespace Engine
{
    public class CardsService
    {
        public static CardsService Instance => instance ??= new CardsService();
        private static CardsService instance;

        public Card[] MaskCards { get; private set; }
        public Card[] PlayerCards { get; private set; }
        
        public void InjectCardsData(CardsDataDto csd)
        {
            MaskCards = new Card[csd.MaskCards.Length];
            for (var i = 0; i < csd.MaskCards.Length; i++)
            {
                var b = Convert.ToByte(i);
                MaskCards[i] = new Card(b, csd.MaskCards[i]);
            }

            PlayerCards = new Card[csd.PlayerCards.Length];
            for (var i = 0; i < csd.PlayerCards.Length; i++)
            {
                var b = Convert.ToByte(i);
                PlayerCards[i] = new Card(b, csd.PlayerCards[i]);
            }
        }

        public Card GetPlayerCardWithId(int cardId)
        {
            if (cardId < 0 || cardId >= PlayerCards.Length)
                return null;
            return PlayerCards[cardId];
        }

        public Card GetMaskCardWithId(int cardId)
        {
            if (cardId < 0 || cardId >= MaskCards.Length)
                return null;
            return MaskCards[cardId];
        }
    }
}