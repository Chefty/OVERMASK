using TMPro;
using UnityEngine;
using DG.Tweening;

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
    public HouseCardDisplayer houseCardDisplayer;

    private int roundNumber = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        blueName.text = Game.Instance.Round.LocalPlayer.Name;
        redName.text = Game.Instance.Round.OpponentPlayer.Name;
    }
    
    public void UpdateScores(int playerScoreValue, int opponentScoreValue)
    {
        blueScore.text = playerScoreValue.ToString();
        redScore.text = opponentScoreValue.ToString();
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
