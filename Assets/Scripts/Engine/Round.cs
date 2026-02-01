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
        public UnityEvent<PlayerFaction, PlayerFaction> OnStackUpdated = new UnityEvent<PlayerFaction, PlayerFaction>();
        
        private readonly Dictionary<string, Player> connectionIdToPlayer;
        private byte? localPlayerCardId;
        private byte? opponentPlayerCardId;

        public Round(Player playerOne, Player playerTwo)
        {
            connectionIdToPlayer = new Dictionary<string, Player>()
            {
                { playerOne.PlayerId, playerOne },
                { playerTwo.PlayerId, playerTwo }
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
            var player1EndRound = endRoundDto.Player1EndRound;
            var player2EndRound = endRoundDto.Player2EndRound;
        
            var player1IsLocal = player1EndRound.PlayerId == LocalPlayer.PlayerId;
            localPlayerCardId = player1IsLocal ? player1EndRound.PlayerCardId : player2EndRound.PlayerCardId;
            opponentPlayerCardId = player1IsLocal ? player2EndRound.PlayerCardId : player1EndRound.PlayerCardId;
        
            GetPlayerBy(player1EndRound.PlayerId).OnRoundEnded(player1EndRound);
            GetPlayerBy(player2EndRound.PlayerId).OnRoundEnded(player2EndRound);
            
            PlaymatView.Instance.UpdateScores(
                GetScoreForFaction(endRoundDto, PlayerFaction.Blue),
                GetScoreForFaction(endRoundDto, PlayerFaction.Red));
            
            OnRoundEnded.Invoke(endRoundDto);
            
            PimDeWitte.UnityMainThreadDispatcher.UnityMainThreadDispatcher.Instance.WaitAndCall(() =>
            {
                BuildStack(endRoundDto.PlayerIdOnBottom);
                localPlayerCardId = null;
                opponentPlayerCardId = null;
            }, CardDisplayer.TIME_TO_SHOW + .2f);
        }

        private byte GetScoreForFaction(EndRoundDto endRoundDto, PlayerFaction playerFaction)
        {
            var userIdForFaction = Game.Instance.Round.GetPlayerBy(playerFaction).PlayerId;
            if(endRoundDto.Player1EndRound.PlayerId == userIdForFaction)
                return endRoundDto.Player1EndRound.PlayerScore;
            return endRoundDto.Player2EndRound.PlayerScore;
        }

        public void ChooseCard(byte cardId)
        {
            LocalPlayer.RemoveCard(cardId);
            localPlayerCardId = cardId;
            Client.Instance.SendMessage(new MessageDto("ChooseCard", new ChooseCardDto(cardId)));
        }
        
        private void BuildStack(string playerIdOnBottom)
        {
            if (!localPlayerCardId.HasValue || !opponentPlayerCardId.HasValue)
                return;
            
            // Determine which faction is on bottom based on server's playerIdOnBottom
            var localPlayerOnBottom = playerIdOnBottom == LocalPlayer.PlayerId;
            var bottomFaction = localPlayerOnBottom ? LocalPlayer.Faction : OpponentPlayer.Faction;
            var topFaction = localPlayerOnBottom ? OpponentPlayer.Faction : LocalPlayer.Faction;
        
            OnStackUpdated.Invoke(bottomFaction, topFaction);
        }

        public Player GetPlayerBy(string connectionId)
        {
            return connectionIdToPlayer[connectionId];
        }
        
        public Player GetPlayerBy(PlayerFaction faction)
        {
            if (LocalPlayer.Faction == faction)
                return LocalPlayer;
            return OpponentPlayer;
        }
    }
}