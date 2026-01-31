import { Room } from "./Room.js";

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
    
    GetAvailableRoom()
    {
        return this.rooms.find(room => room.players.length === 1);
    }

    CreateRoom(roomId)
    {
        let room = new Room(roomId, this.dtoService, this.cardsService);
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
    
    AddPlayer(player)
    {
        let room = this.GetAvailableRoom();
        if(!room)
            room = this.CreateRoom((this.roomId++).toString());
        
        this.AddUserToRoom(room, player);
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
}