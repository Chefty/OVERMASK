using TMPro;
using UnityEngine;

public class PlaymatView : MonoBehaviour
{
    public static PlaymatView Instance;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private TMP_Text opponentScore;
    [SerializeField] private TMP_Text roundText;
    public PlayerSlot playerSlot;
    public OpponentSlot opponentSlot;
    public HouseSlot houseSlot;
    public CombatSlot combatSlot;
    public OpponentCardDisplayer opponentCardDisplayer;
    public HouseCardDisplayer houseCardDisplayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerName.text = Game.Instance.Round.LocalPlayer.Name;
        opponentName.text = Game.Instance.Round.OpponentPlayer.Name;
    }
}
