import {MatchStartDto} from "./dto/MatchStartDto.js";
import {RoundStartDto} from "./dto/RoundStartDto.js";

export class Room
{
    connections = [];
    roomId;
    dtoService;

    ready = 0;
    currentPlayer = 0;
    
    constructor(roomId, dtoService)
    {
        this.roomId = roomId;
        this.dtoService = dtoService;
    }

    SetConnectionReady(connection) {
        console.log(`[Room ${this.roomId}] Use ${connection.userName} is ready.`);

        if (++this.ready === 2)
            this.ChangeRound();
    }
    
    EndRound()
    {
        console.log(`[Room ${this.roomId}] End round.`);
        this.currentPlayer++;
        this.ChangeRound();
    }
    
    ChangeRound()
    {
        this.moved = false;
        this.BroadcastDto("StartRound", new RoundStartDto(this.GetCurrentConnection()));
    }
    
    MovePiece(connection, dto)
    {
        if(this.GetCurrentConnection().connectionId !== connection.connectionId)
        {
            console.log(`[Room ${this.roomId}] User ${connection.userName} is trying to make a move on opponents turn. Ignoring.`);
            return;
        }
        
        this.moved = true;
        this.BroadcastDto("MovePiece", dto);
    }
    
    AddConnection(connection)
    {
        this.connections.push(connection);
        console.log(`[Room ${this.roomId}] Add user > roomId: ${this.roomId}, connection: ${connection}`);
        
        if(this.connections.length === 2)
        {
            console.log(`[Room ${this.roomId}] Room is full. Starting a new match.`);
            
            this.dtoService.Send("OpponentFound", this.connections[0].ws, new MatchStartDto(this.connections[0], this.connections[1]));
            this.dtoService.Send("OpponentFound", this.connections[1].ws, new MatchStartDto(this.connections[1], this.connections[0]));
        }
    }

    RemoveConnection(connection)
    {
        console.log(`[Room ${this.roomId}] Remove user > roomId: ${this.roomId}, connection: ${connection}`);
        this.connections.splice(this.connections.indexOf(connection), 1);
        if(this.connections.length > 0)
            this.dtoService.Send("OpponentDisconnected", this.connections[0].ws, null);
    }
    
    BroadcastDto(type, dto)
    {
        this.dtoService.Send(type, this.connections[0].ws, dto);
        this.dtoService.Send(type, this.connections[1].ws, dto);

        console.log(`[Room ${this.roomId}] Broadcast > ${dto.toString()}.`);
    }
    
    GetCurrentConnection()
    {
        return this.connections[this.currentPlayer % 2];
    }
}