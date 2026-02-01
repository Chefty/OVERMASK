using System.Collections.Generic;
using client.dto;
using UnityEngine.Events;

namespace Engine
{
    public class Player
    {
        public bool IsLocalPlayer { get; }
        public string Name { get; }
        public string PlayerId { get; }
        public int Score { get; private set; }
        public PlayerFaction Faction { get; private set; }
        public List<byte> AvailableCards { get; private set; } = new();
        
        public readonly UnityEvent<List<byte>> OnDrawInitialHand = new UnityEvent<List<byte>>();
        public readonly UnityEvent<byte> OnGetNewCard = new UnityEvent<byte>();
        
        public Player(bool isLocalPlayer, PlayerDto player)
        {
            IsLocalPlayer = isLocalPlayer;
            Name = player.UserName;
            PlayerId = player.PlayerId;
            Faction = (PlayerFaction)player.Faction;
            AvailableCards = new List<byte>(player.AvailableCards);
        }

        public void OnRoundEnded(PlayerEndRoundDto playerEndRound)
        {
            Score = playerEndRound.PlayerScore;
            AddCard(playerEndRound.NewCardId);
        }

        public void RemoveCard(byte cardId)
        {
            AvailableCards.Remove(cardId);
        }

        public void AddCards(byte[] cardIds)
        {
            AvailableCards.AddRange(cardIds);
            OnDrawInitialHand.Invoke(AvailableCards);
        }

        public void AddCard(byte cardId)
        {
            AvailableCards.Add(cardId);
            OnGetNewCard.Invoke(cardId);
        }
    }
}