using client.dto;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Engine;
using UnityEngine.Serialization;

public class PlaymatView : MonoBehaviour
{
    public static PlaymatView Instance;
    
    [SerializeField] private TMP_Text blueName;
    [SerializeField] private TMP_Text redName;
    [SerializeField] private TMP_Text blueScore;
    [SerializeField] private TMP_Text redScore;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text readyText;
    public PlayerSlot blueSlot;
    public OpponentSlot redSlot;
    public CombatSlot combatSlot;
    public CardDisplayer maskCardDisplayer;
    public CardDisplayer blueCardDisplayer;
    public CardDisplayer redCardDisplayer;

    private int roundNumber = 0;
    
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
    }

    private void OnDrawMaskCard(byte cardId)
    {
        maskCardDisplayer.DisplayCard(CardsService.Instance.GetMaskCardWithId(cardId));
    }

    private void OnRoundEnded(EndRoundDto endRoundDto)
    {
        var cardDisplayer = GetCardDisplayerForOpponent();
        if (endRoundDto.Player1EndRound.PlayerId == Game.Instance.Round.LocalPlayer.PlayerId)
            cardDisplayer.DisplayCard(CardsService.Instance.GetPlayerCardWithId(endRoundDto.Player2EndRound.PlayerCardId));
        else
            cardDisplayer.DisplayCard(CardsService.Instance.GetPlayerCardWithId(endRoundDto.Player1EndRound.PlayerCardId));
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
        roundText.text = "Round " + roundNumber++;
        roundText.DOFade(1, 0.5f).OnComplete(() =>
        {
            roundText.DOFade(0, 0.5f).SetDelay(1f);
        });
    }
}
