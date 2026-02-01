using DG.Tweening;
using Engine;
using UnityEngine;

public class HouseCardDisplayer : MonoBehaviour
{
    private const float TIME_TO_SHOW = 0.5f;
    
    public CardView MaskCardView { get; private set; }
    
    private void Start()
    {
        Game.Instance.Round.OnDrawMaskCard.AddListener(OnDrawMaskCard);
    }

    private void OnDrawMaskCard(byte cardId)
    {
        var cardData = CardsService.Instance.GetMaskCardWithId(cardId) as ICardData;
        var root = transform;
        var context = new CardGenContext
        {
            Data = cardData,
            Faction = PlayerFaction.House,
            Parent = root.transform
        };
        MaskCardView = CardGenService.Instance.GenCard(context);
        MaskCardView.transform.localScale = Vector3.zero;
        MaskCardView.transform.DOScale(1.52104f, TIME_TO_SHOW).SetEase(Ease.OutQuart);
    }
}
