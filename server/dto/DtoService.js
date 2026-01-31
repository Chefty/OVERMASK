import {ConnectDto} from "./ConnectDto.js";
import StreamBuffer from 'streambuf';

export class DtoService {
    constructor(connectionService, roomService) {
        this.connectionService = connectionService;
        this.roomService = roomService;

        this.roomService.InjectDtoService(this);
    }

    Parse(ws, buffer) {
        let leng = buffer.readUInt8();
        let type = buffer.readString(leng);

        switch (type) {
            case "Connection":
                this.Connect(ws, new ConnectDto(buffer));
                break;
            case "CreateOrJoinRoom":
                this.CreateOrJoinRoom(ws);
                break;
            case "ReadyToPlay":
                this.ReadyToPlay(ws);
                break;
            case "EndRound":
                this.EndRound(ws);
                break;
        }
    }

    Connect(ws, dto) {
        this.connectionService.CreateConnection(ws, dto);
    }

    CreateOrJoinRoom(ws) {
        let connection = this.connectionService.GetConnection(ws);
        this.roomService.AddConnection(connection);
    }

    ReadyToPlay(ws) {
        let connection = this.connectionService.GetConnection(ws);
        let room = this.roomService.GetRoom(connection);
        room.SetConnectionReady(connection);
    }

    EndRound(ws) {
        let connection = this.connectionService.GetConnection(ws);
        let room = this.roomService.GetRoom(connection);
        room.EndRound();
    }

    Send(type, ws, dto) {
        let buffer = new StreamBuffer(Buffer.alloc(4096));

        buffer.writeUInt8(type.length);
        buffer.writeString(type);

        if (dto)
            dto.writeToBuffer(buffer);

        let noTrailBuffer = this.RemoveBufferTrail(buffer.buffer);
        ws.send(noTrailBuffer);
    }

    RemoveBufferTrail(buffer) {
        let lastIndex = buffer.length - 1;
        while (lastIndex >= 0 && buffer[lastIndex] === 0x00) {
            lastIndex--;
        }
        return buffer.slice(0, lastIndex + 10);
    }
}