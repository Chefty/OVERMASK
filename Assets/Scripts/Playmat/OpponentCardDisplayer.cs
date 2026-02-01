using Engine;
using UnityEngine;

public class OpponentCardDisplayer : MonoBehaviour
{
    public void OnShowOpponentCard(Card card)
    {
        var cardData = card as ICardData;
        CardGenContext context = new CardGenContext();
        context.Data = cardData;
        var root = PlaymatView.Instance.opponentSlot;
        context.Faction = PlayerFaction.Opponent;
        context.Parent = root.transform;
        CardGenService.Instance.GenCard(context);
    }
}
