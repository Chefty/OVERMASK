using System.Collections.Generic;
using client.dto;

namespace Engine
{
    public class Player
    {
        public bool IsLocalPlayer { get; }
        public string Name { get; }
        public string ConnectionId { get; }
        public int Score { get; private set; }
        public List<byte> AvailableCards { get; private set; }
        
        public Player(bool isLocalPlayer, PlayerDto player)
        {
            IsLocalPlayer = isLocalPlayer;
            Name = player.UserName;
            ConnectionId = player.ConnectionId;
            AvailableCards = new List<byte>(player.AvailableCards);
        }

        public void OnRoundEnded(PlayerEndRoundDto playerEndRound)
        {
            Score = playerEndRound.PlayerScore;
            AvailableCards.Add(playerEndRound.NewCardId);
        }

        public void RemoveCard(byte cardId)
        {
            AvailableCards.Remove(cardId);
        }
    }
}