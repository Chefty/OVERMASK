using TMPro;
using UnityEngine;

public class PlaymatView : MonoBehaviour
{
    public static PlaymatView Instance;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
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
}
