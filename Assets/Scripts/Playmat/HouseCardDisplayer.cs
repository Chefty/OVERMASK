using Engine;
using UnityEngine;

public class HouseCardDisplayer : MonoBehaviour
{
    public void OnShowHouseCard(Card card)
    {
        var cardData = card as ICardData;
        CardGenContext context = new CardGenContext();
        context.Data = cardData;
        var root = PlaymatView.Instance.houseSlot;
        context.Faction = PlayerFaction.House;
        context.Parent = root.transform;
        CardGenService.Instance.GenCard(context);
    }
}
