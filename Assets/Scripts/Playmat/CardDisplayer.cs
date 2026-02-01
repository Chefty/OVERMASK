using System;
using DG.Tweening;
using Engine;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    public static readonly float TIME_TO_SHOW = 0.5f;
    
    public CardView CardView { get; private set; }

    public void DisplayCard(Card card, PlayerFaction faction)
    {
        var cardData = card as ICardData;
        var root = transform;
        var context = new CardGenContext
        {
            Data = cardData,
            Faction = faction,
            Parent = root.transform
        };
        CardView = CardGenService.Instance.GenCard(context);
        CardView.transform.localScale = Vector3.zero;
        CardView.transform.DOScale(1f, TIME_TO_SHOW).SetEase(Ease.OutQuart);
    }

    public void ForceCardView(CardView cardView)
    {
        CardView = cardView;
    }
}
