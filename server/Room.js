import { EndRoundDto } from "./dto/EndRoundDto.js";
import {GameStartDto} from "./dto/GameStartDto.js";
import { PlayerEndRoundDto } from "./dto/PlayerEndRoundDto.js";
import { RequestCardDto } from "./dto/RequestCardDto.js";

export class Room
{
    players = [];
    roomId;
    dtoService;

    ready = 0;
    currentMaskCardId = 0;
    
    constructor(roomId, dtoService)
    {
        this.roomId = roomId;
        this.dtoService = dtoService;
    }

    SetPlayerReady(player) {
        console.log(`[Room ${this.roomId}] Use ${player.userName} is ready.`);

        if (++this.ready === 2)
            this.ChangeRound();
    }
    
    ChangeRound()
    {
        this.ready = 0;
        this.moved = false;
        this.ResetPlayersCards();
        this.currentMaskCardId = this.GetRandomMaskCard();
        this.BroadcastDto("RequestCard", new RequestCardDto(currentMaskCardId));
    }
    
    PlayerChoseCard(player, chooseCardDto)
    {
        player.currentCardId = chooseCardDto.cardId;
        if(this.players[0].currentCardId !== -1 && this.players[1].currentCardId !== -1)
            this.EndRound();
    }

    EndRound()
    {
        var playerOnBottom = this.players[0].score > this.players[1].score ? this.players[1].playerId : this.players[0].playerId;
        var player1NewCard = this.GetRandomPlayerCard();
        var player1EndRound = new PlayerEndRoundDto(this.players[0].playerId, this.players[0].currentCardId, this.players[0].score, player1NewCard, playerOnBottom);

        var player2NewCard = this.GetRandomPlayerCard();
        var player2EndRound = new PlayerEndRoundDto(this.players[1].playerId, this.players[1].currentCardId, this.players[1].score, player2NewCard, playerOnBottom);

        var endRoundDto = new EndRoundDto(player1EndRound, player2EndRound);
        this.BroadcastDto("EndRound", endRoundDto);
    }

    GetRandomMaskCard()
    {
        //TODO: get a random mask card id
        return 0;
    }

    GetRandomPlayerCard()
    {
        //TODO: get a random player card id
        return 0;
    }
    
    AddPlayer(player)
    {
        this.players.push(player);
        console.log(`[Room ${this.roomId}] Add user > roomId: ${this.roomId}, player: ${player}`);
        
        if(this.players.length === 2)
        {
            console.log(`[Room ${this.roomId}] Room is full. Starting a new match.`);
            
            this.dtoService.Send("OpponentFound", this.players[0].ws, new GameStartDto(this.players[0], this.players[1]));
            this.dtoService.Send("OpponentFound", this.players[1].ws, new GameStartDto(this.players[1], this.players[0]));
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