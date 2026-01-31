using System.Collections.Generic;
using client;
using client.dto;

namespace Engine
{
    public class Round
    {
        public Player LocalPlayer { get; private set; }
        public Player OpponentPlayer { get; private set; }
        
        public byte CurrentMaskCardId { get; private set; }
        
        private readonly Dictionary<string, Player> connectionIdToPlayer;

        public Round(Player playerOne, Player playerTwo)
        {
            connectionIdToPlayer = new Dictionary<string, Player>()
            {
                { playerOne.ConnectionId, playerOne },
                { playerTwo.ConnectionId, playerTwo }
            };

            LocalPlayer = playerOne.IsLocalPlayer ? playerOne : playerTwo;
            OpponentPlayer = playerOne.IsLocalPlayer ? playerTwo : playerOne;
            
            Client.Instance.OnCardRequested.AddListener(OnCardRequested);
            Client.Instance.OnRoundEnded.AddListener(OnRoundEnded);
            
            Client.Instance.SendMessage(new MessageDto("ReadyToPlay"));
        }

        private void OnCardRequested(RequestCardDto requestCardDto)
        {
            CurrentMaskCardId = requestCardDto.MaskCardId;
        }

        private void OnRoundEnded(EndRoundDto endRoundDto)
        {
            GetPlayerBy(endRoundDto.Player1EndRound.ConnectionId).OnRoundEnded(endRoundDto.Player1EndRound);
            GetPlayerBy(endRoundDto.Player2EndRound.ConnectionId).OnRoundEnded(endRoundDto.Player2EndRound);
        }

        public void ChooseCard(byte cardId)
        {
            LocalPlayer.RemoveCard(cardId);
            Client.Instance.SendMessage(new MessageDto("ChooseCard", new ChooseCardDto(cardId)));
        }

        public Player GetPlayerBy(string connectionId)
        {
            return connectionIdToPlayer[connectionId];
        }
    }
}