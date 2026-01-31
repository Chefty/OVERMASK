import {EndRoundDto} from "./dto/EndRoundDto.js";
import {GameStartDto} from "./dto/GameStartDto.js";
import {PlayerEndRoundDto} from "./dto/PlayerEndRoundDto.js";
import {RequestCardDto} from "./dto/RequestCardDto.js";
import {Dealer} from "./Dealer.js";
import { DealInitialCardsDto } from "./dto/DealInitialCardsDto.js";

export class Room
{
    INITIAL_CARD_AMOUNT = 3;

    players = [];
    roomId;
    dtoService;
    cardsService;
    dealer;

    dealtInitialCards = false;

    ready = 0;

    constructor(roomId, dtoService, cardsService)
    {
        this.roomId = roomId;
        this.dtoService = dtoService;
        this.cardsService = cardsService;
        this.dealer = new Dealer(cardsService);
    }

    SetPlayerReady(player) {
        console.log(`[Room ${this.roomId}] User ${player.userName} is ready.`);

        if (++this.ready === 2)
            this.ChangeRound();
    }
    
    ChangeRound()
    {
        if(!this.dealtInitialCards)
            this.DealInitialCards();

        this.ready = 0;
        this.moved = false;
        this.ResetPlayersCards();
        this.BroadcastDto("RequestCard", new RequestCardDto(this.GetRandomMaskCard()));
    }

    DealInitialCards()
    {
        this.dealtInitialCards = true;

        var player1Cards = [];
        var player2Cards = [];

        for (let i = 0; i < this.INITIAL_CARD_AMOUNT; i++) {
            player1Cards.push(this.GetRandomPlayerCard());
            player2Cards.push(this.GetRandomPlayerCard());
        }

        this.dtoService.Send("DealInitialCards", this.players[0].ws, new DealInitialCardsDto(player1Cards));
        this.dtoService.Send("DealInitialCards", this.players[1].ws, new DealInitialCardsDto(player2Cards));
    }
    
    PlayerChoseCard(player, chooseCardDto)
    {
        this.dealer.SetPlayerCard(player, chooseCardDto.cardId);
        if(this.players[0].currentCardId !== -1 && this.players[1].currentCardId !== -1)
            this.EndRound();
    }

    EndRound()
    {
        this.dealer.EndOfRound();

        var playerOnBottom = this.dealer.GetLeadingPlayer();
        var player1NewCard = this.GetRandomPlayerCard();
        var player1EndRound = new PlayerEndRoundDto(this.players[0].playerId, this.dealer.GetPlayer1Card(), this.dealer.GetPlayer1Score(), player1NewCard, playerOnBottom);

        var player2NewCard = this.GetRandomPlayerCard();
        var player2EndRound = new PlayerEndRoundDto(this.players[1].playerId, this.dealer.GetPlayer2Card(), this.dealer.GetPlayer2Score(), player2NewCard, playerOnBottom);

        var endRoundDto = new EndRoundDto(player1EndRound, player2EndRound);
        this.BroadcastDto("EndRound", endRoundDto);
    }

    GetRandomMaskCard()
    {
        return this.dealer.DrawMaskCard();
    }

    GetRandomPlayerCard()
    {
        return this.dealer.DrawPlayerCard();
    }

    AddPlayer(player)
    {
        this.players.push(player);
        console.log(`[Room ${this.roomId}] Add user > roomId: ${this.roomId}, player: ${player}`);
        
        if(this.players.length === 2)
        {
            console.log(`[Room ${this.roomId}] Room is full. Starting a new match.`);
            
            this.dtoService.Send("OpponentFound", this.players[0].ws, new GameStartDto(this.players[0], this.players[1], this.cardsService));
            this.dtoService.Send("OpponentFound", this.players[1].ws, new GameStartDto(this.players[1], this.players[0], this.cardsService));
        }
    }

    RemovePlayer(player)
    {
        console.log(`[Room ${this.roomId}] Remove user > roomId: ${this.roomId}, player: ${player}`);
        this.players.splice(this.players.indexOf(player), 1);
        if(this.players.length > 0)
            this.dtoService.Send("OpponentDisconnected", this.players[0].ws, null);
    }
    
    BroadcastDto(type, dto)
    {
        this.dtoService.Send(type, this.players[0].ws, dto);
        this.dtoService.Send(type, this.players[1].ws, dto);

        console.log(`[Room ${this.roomId}] Broadcast > ${dto.toString()}.`);
    }

    ResetPlayersCards()
    {
        this.players[0].currentCardId = -1;
        this.players[1].currentCardId = -1;
    }
}