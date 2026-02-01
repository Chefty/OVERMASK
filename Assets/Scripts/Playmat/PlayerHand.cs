using System.Collections.Generic;
using DG.Tweening;
using Engine;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private const float CARD_DISTANCE = 3f;
    private const float CARD_UPDATE_TIME = 0.3f;
    
    [SerializeField] private Transform parent;
    
    private List<CardView> cards = new List<CardView>();
    
    private void Start()
    {
        Game.Instance.Round.LocalPlayer.OnDrawInitialHand.AddListener(OnDrawInitialHand);
        Game.Instance.Round.LocalPlayer.OnGetNewCard.AddListener(OnGetNewCard);
    }

    private void OnDrawInitialHand(List<byte> cards)
    {
        var slot = 0;
        foreach (var card in cards)
        {
            var cardData = CardsService.Instance.GetPlayerCardWithId(card) as ICardData;
            AddCard(cardData, false);
        }

        UpdateCardsPosition();
    }
    
    private void OnGetNewCard(byte card)
    {
        var cardData = CardsService.Instance.GetPlayerCardWithId(card) as ICardData;
        AddCard(cardData);
    }

    public void AddCard(ICardData card, bool animate = true)
    {
        var context = new CardGenContext
        {
            Data = card,
            Parent = parent,
            Faction = PlayerFaction.Player
        };
        var cardView = CardGenService.Instance.GenCard(context);
        cards.Add(cardView);

        if(animate)
            UpdateCardsPosition();
    }

    public void RemoveCard(CardView card)
    {
        cards.Remove(card);
        UpdateCardsPosition();
    }

    private void UpdateCardsPosition()
    {
        var cardAmount = cards.Count;
        var initialX = -((cardAmount - 1) * CARD_DISTANCE);
        for (int i = 0; i < cardAmount; i++)
        {
            var card = cards[i];
            var posX = initialX + (i * CARD_DISTANCE * 2);
            card.transform.DOLocalMoveX(posX, CARD_UPDATE_TIME).SetEase(Ease.OutQuart);
        }
    }

    // [ContextMenu(nameof(TryFillCard))]
    // public void TryFillCard()
    // {
    //     for (int i = 0; i < CardSlots.Count; i++)
    //     {
    //         var slot = CardSlots[i];
    //         var cardData = CardsService.Instance.GetPlayerCardWithId(i) as ICardData;
    //         CardGenContext context = new CardGenContext();
    //         context.CardId = cardData.CardId;
    //         context.Data = cardData;
    //         context.Parent = CardSlots[i];
    //         context.Faction = PlayerFaction.Player;
    //         CardGenService.Instance.GenCard(context);
    //     }
    // }
}
