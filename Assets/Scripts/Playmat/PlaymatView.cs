using TMPro;
using UnityEngine;
using client;
using DG.Tweening;

public class PlaymatView : MonoBehaviour
{
    public static PlaymatView Instance;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private TMP_Text opponentScore;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text readyText;
    public PlayerSlot playerSlot;
    public OpponentSlot opponentSlot;
    public HouseSlot houseSlot;
    public CombatSlot combatSlot;
    public OpponentCardDisplayer opponentCardDisplayer;
    public HouseCardDisplayer houseCardDisplayer;

    private int roundNumber = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerName.text = Game.Instance.Round.LocalPlayer.Name;
        opponentName.text = Game.Instance.Round.OpponentPlayer.Name;
    }
    
    public void UpdateScores(int playerScoreValue, int opponentScoreValue)
    {
        playerScore.text = playerScoreValue.ToString();
        opponentScore.text = opponentScoreValue.ToString();
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
