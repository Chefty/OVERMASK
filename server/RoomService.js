import { ConnectToRoomWithCodeDto } from "./dto/ConnectToRoomWithCodeDto.js";
import { Room } from "./Room.js";
import { RoomConnectionResult } from "./RoomConnectionResult.js";

export class RoomService
{
    rooms = [];
    playerToRoom = new Map();
    roomId = 0;
    dtoService;
    cardsService;

    constructor (cardsService) {
        this.cardsService = cardsService;
    }
    
    InjectDtoService(dtoService)
    {
        this.dtoService = dtoService
    }
    
    GetAvailableRoom(roomCode)
    {
        return this.rooms.find(room => room.roomCode === roomCode && room.players.length === 1);
    }

    CreateRoom(roomId, roomCode)
    {
        let room = new Room(roomId, roomCode, this.dtoService, this.cardsService);
        this.rooms.push(room);
        console.log(`[RoomService] Create room > id: ${roomId}`);
        return room;
    }

    GetRoom(player)
    {
        return this.playerToRoom.get(player);
    }

    AddUserToRoom(room, player)
    {
        room.AddPlayer(player);
        this.playerToRoom.set(player, room);
    }
    
    AddPlayer(player, roomCode)
    {
        let room = this.GetAvailableRoom(roomCode);
        if(!room)
            room = this.CreateRoom((this.roomId++).toString(), roomCode);
        
        this.AddUserToRoom(room, player);
    }

    TryJoinRoomCode(player, roomCode)
    {
        for (let i = 0; i < this.rooms.length; i++) {
            const room = this.rooms[i];
            if(room.roomCode === roomCode)
            {
                if(room.players.length > 1)
                    return new RoomConnectionResult(false, roomCode, `Room ${roomCode.toString().padStart(4, '0')} is full.`);
                this.AddPlayer(player, roomCode);
                return new RoomConnectionResult(true, roomCode, "");
            }
        }

        return new RoomConnectionResult(false, roomCode, `Room with code ${roomCode.toString().padStart(4, '0')} was not found.`)
    }

    RemovePlayer(player)
    {
        let room = this.GetRoom(player);
        if(!room)
            return;
        
        room.RemovePlayer(player);
        this.rooms.splice(this.rooms.indexOf(room), 1);
        this.playerToRoom.delete(player);
        this.playerToRoom.delete(room.players[0]);
        console.log(`[RoomService] Removed room > id: ${room.roomId}`);
    }

    GetRandomRoomCode()
    {
        var code = Math.floor(Math.random() * 9999) + 1;
        while(true)
        {
            var found = false;
            for (let i = 0; i < this.rooms.length; i++) {
                const room = this.rooms[i];
                if(room.roomCode === code)
                    found = true;
            }

            if(found)
                var code = Math.floor(Math.random() * 10000);
            else
                break;
        }

        return code;
    }
}