using System.Collections.Generic;
using Engine;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private List<Transform> CardSlots = new List<Transform>();


    [ContextMenu(nameof(TryFillCard))]
    public void TryFillCard()
    {
        for (int i = 0; i < CardSlots.Count; i++)
        {
            var slot = CardSlots[i];
            var cardData = CardsService.Instance.GetPlayerCardWithId(i) as ICardData;
            CardGenContext context = new CardGenContext();
            context.CardId = cardData.CardId;
            context.Data = cardData;
            context.Parent = CardSlots[i];
            context.Faction = PlayerFaction.Player;
            CardGenService.Instance.GenCard(context);
        }
    }
}
