using System.Collections.Generic;
using client;
using client.dto;
using UnityEngine.Events;

namespace Engine
{
    public class Round
    {
        public Player LocalPlayer { get; private set; }
        public Player OpponentPlayer { get; private set; }
        
        public byte CurrentMaskCardId { get; private set; }
        
        public UnityEvent<byte> OnDrawMaskCard = new UnityEvent<byte>(); 
        public UnityEvent OnCardRequested = new UnityEvent(); 
        public UnityEvent<EndRoundDto> OnRoundEnded = new UnityEvent<EndRoundDto>(); 
        
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
            
            Client.Instance.OnCardRequested.AddListener(InternalOnCardRequested);
            Client.Instance.OnDealInitialCardsDto.AddListener(OnDealInitialCards);
            Client.Instance.OnRoundEnded.AddListener(InternalOnRoundEnded);
        }

        private void OnDealInitialCards(DealInitialCardsDto arg0)
        {
            LocalPlayer.AddCards(arg0.CardIds);
        }

        private void InternalOnCardRequested(RequestCardDto requestCardDto)
        {
            CurrentMaskCardId = requestCardDto.MaskCardId;
            OnDrawMaskCard.Invoke(CurrentMaskCardId);
            OnCardRequested.Invoke();
        }

        private void InternalOnRoundEnded(EndRoundDto endRoundDto)
        {
            GetPlayerBy(endRoundDto.Player1EndRound.PlayerId).OnRoundEnded(endRoundDto.Player1EndRound);
            GetPlayerBy(endRoundDto.Player2EndRound.PlayerId).OnRoundEnded(endRoundDto.Player2EndRound);
            PlaymatView.Instance.UpdateScores(endRoundDto.Player1EndRound.PlayerScore, endRoundDto.Player2EndRound.PlayerScore);
            OnRoundEnded.Invoke(endRoundDto);
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