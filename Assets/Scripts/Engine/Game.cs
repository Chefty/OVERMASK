using System.Collections;
using client;
using client.dto;
using Engine;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public Round Round { get; private set; }
    public int CurrentRoundNumber { get; set; } = 1;

    private void Awake()
    {
        Instance = this;
        
        var matchDto = Client.Instance?.GameDto;
        var localPlayer = new Player(true, matchDto.Player);
        var opponentPlayer = new Player(false, matchDto.Opponent);
        
        Round = new Round(localPlayer, opponentPlayer);
            
        Client.Instance!.OnOpponentDisconnected.AddListener(OnOpponentDisconnected);
    }

    private IEnumerator Start()
    {
        CurrentRoundNumber = 1;
        yield return new WaitForSeconds(0.5f);
        Client.Instance.SendMessage(new MessageDto("ReadyToPlay"));
    }
    
    public void StartNewRound()
    {
        Client.Instance.SendMessage(new MessageDto("ReadyToPlay"));
        PlaymatView.Instance.UpdateRoundText();
    }
    
    private void OnOpponentDisconnected()
    {
        //Show panel
    }
}
