using client.dto;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Engine;

public class PlaymatView : MonoBehaviour
{
    private const float STACK_LAYER_SPACING = 0.05f;
    
    public static PlaymatView Instance;
    
    [SerializeField] private TMP_Text blueName;
    [SerializeField] private TMP_Text redName;
    [SerializeField] private TMP_Text blueScore;
    [SerializeField] private TMP_Text redScore;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text readyText;
    public CombatSlot combatSlot;
    public CardDisplayer maskCardDisplayer;
    public CardDisplayer blueCardDisplayer;
    public CardDisplayer redCardDisplayer;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        blueName.text = Game.Instance.Round.GetPlayerBy(PlayerFaction.Blue).Name;
        redName.text = Game.Instance.Round.GetPlayerBy(PlayerFaction.Red).Name;
        
        Game.Instance.Round.OnRoundEnded.AddListener(OnRoundEnded);
        Game.Instance.Round.OnDrawMaskCard.AddListener(OnDrawMaskCard);
        Game.Instance.Round.OnStackUpdated.AddListener(UpdateStackVisuals);
    }

    private void OnDrawMaskCard(byte cardId)
    {
        maskCardDisplayer.DisplayCard(CardsService.Instance.GetMaskCardWithId(cardId), PlayerFaction.Mask);
    }

    private void OnRoundEnded(EndRoundDto endRoundDto)
    {
        var cardDisplayer = GetCardDisplayerForOpponent();
        if (endRoundDto.Player1EndRound.PlayerId == Game.Instance.Round.LocalPlayer.PlayerId)
            cardDisplayer.DisplayCard(CardsService.Instance.GetPlayerCardWithId(endRoundDto.Player2EndRound.PlayerCardId), Game.Instance.Round.OpponentPlayer.Faction);
        else
            cardDisplayer.DisplayCard(CardsService.Instance.GetPlayerCardWithId(endRoundDto.Player1EndRound.PlayerCardId), Game.Instance.Round.LocalPlayer.Faction);
    }

    public CardDisplayer GetCardDisplayerForFaction(PlayerFaction faction)
    {
        if (faction == PlayerFaction.Blue)
            return blueCardDisplayer;
        return redCardDisplayer;
    }
    
    private CardDisplayer GetCardDisplayerForOpponent()
    {
        if (Game.Instance.Round.LocalPlayer.Faction == PlayerFaction.Blue)
            return GetCardDisplayerForFaction(PlayerFaction.Red);
        return GetCardDisplayerForFaction(PlayerFaction.Blue);
    }

    public void UpdateScores(int blueScoreValue, int redScoreValue)
    {
        blueScore.text = blueScoreValue.ToString();
        redScore.text = redScoreValue.ToString();
    }
    
    public void UpdateRoundText()
    {
        roundText.text = "Round " + Game.Instance.CurrentRoundNumber++;
        roundText.DOFade(1, 0.5f).OnComplete(() =>
        {
            roundText.DOFade(0, 0.5f).SetDelay(1f);
        });
    }
    
    private void UpdateStackVisuals(PlayerFaction bottomFaction, PlayerFaction topFaction)
    {
        Debug.Log($"Stack built: Bottom={bottomFaction}, Top={topFaction}, Mask=House");
        
        // Get the CardDisplayers for each faction
        var bottomDisplayer = GetCardDisplayerForFaction(bottomFaction);
        var topDisplayer = GetCardDisplayerForFaction(topFaction);
        
        if (bottomDisplayer.CardView == null || 
            topDisplayer.CardView == null || 
            maskCardDisplayer.CardView == null)
        {
            Debug.LogError("Cannot build stack: One or more CardViews are missing");
            return;
        }
        
        AnimateCardToStack(bottomDisplayer, 0, 0f);
        AnimateCardToStack(topDisplayer, 1, 0.3f);
        var maskSequence = AnimateCardToStack(maskCardDisplayer, 2, 0.6f);
        maskSequence.OnComplete(() =>
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                Game.Instance.StartNewRound();
            });
        });
    }
    
    private Sequence AnimateCardToStack(CardDisplayer cardDisplayer, int stackLayer, float delay)
    {
        if (cardDisplayer.CardView == null)
        {
            Debug.LogWarning($"CardDisplayer has no CardView to animate");
            return null;
        }

        var cardView = cardDisplayer.CardView;
        var targetPos = combatSlot.transform.position + Vector3.up * (stackLayer * STACK_LAYER_SPACING);

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.Append(cardView.transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutQuad));
        sequence.Join(cardView.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutQuad));
        sequence.Join(cardView.transform.DOScale(Vector3.one * 1f, 0.5f).SetEase(Ease.OutQuad));
        sequence.Append(cardView.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f));
    
        return sequence;
    }
}
