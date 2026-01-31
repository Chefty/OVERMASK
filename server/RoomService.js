import { Room } from "./Room.js";

export class RoomService
{
    rooms = [];
    connectionToRoom = new Map();
    roomId = 0;
    dtoService;
    
    InjectDtoService(dtoService)
    {
        this.dtoService = dtoService
    }
    
    GetAvailableRoom()
    {
        return this.rooms.find(room => room.connections.length === 1);
    }

    CreateRoom(roomId)
    {
        let room = new Room(roomId, this.dtoService);
        this.rooms.push(room);
        console.log(`[RoomService] Create room > id: ${roomId}`);
        return room;
    }

    GetRoom(connection)
    {
        return this.connectionToRoom.get(connection);
    }

    AddUserToRoom(room, connection)
    {
        room.AddConnection(connection);
        this.connectionToRoom.set(connection, room);
    }
    
    AddConnection(connection)
    {
        let room = this.GetAvailableRoom();
        if(!room)
            room = this.CreateRoom((this.roomId++).toString());
        
        this.AddUserToRoom(room, connection);
    }

    RemoveConnection(connection)
    {
        let room = this.GetRoom(connection);
        if(!room)
            return;
        
        room.RemoveConnection(connection);
        this.rooms.splice(this.rooms.indexOf(room), 1);
        this.connectionToRoom.delete(connection);
        this.connectionToRoom.delete(room.connections[0]);
        console.log(`[RoomService] Removed room > id: ${room.roomId}`);
    }
}