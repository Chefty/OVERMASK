using TMPro;
using UnityEngine;

public class PlaymatView : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
    public PlayerSlot playerSlot;
    public OpponentSlot opponentSlot;
    public HouseSlot houseSlot;
    public CombatSlot combatSlot;
}
