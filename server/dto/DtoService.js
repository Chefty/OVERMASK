import {ConnectDto} from "./ConnectDto.js";
import {ChooseCardDto} from "./ChooseCardDto.js";
import StreamBuffer from 'streambuf';

export class DtoService {
    constructor(playerService, roomService, cardsService) {
        this.playerService = playerService;
        this.roomService = roomService;
        this.cardsService = cardsService;

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
            case "ChooseCard":
                var ccdto = new ChooseCardDto(buffer);
                this.ChooseCard(ws, ccdto);
                break;
            case "EndRound":
                this.EndRound(ws);
                break;
        }
    }

    Connect(ws, dto) {
        this.playerService.CreatePlayer(ws, dto);
    }

    CreateOrJoinRoom(ws) {
        let player = this.playerService.GetPlayer(ws);
        this.roomService.AddPlayer(player);
    }

    ReadyToPlay(ws) {
        let player = this.playerService.GetPlayer(ws);
        let room = this.roomService.GetRoom(player);
        room.SetPlayerReady(player);
    }

    ChooseCard(ws, dto) {
        let player = this.playerService.GetPlayer(ws);
        let room = this.roomService.GetRoom(player);
        room.PlayerChoseCard(player, dto)
    }

    EndRound(ws) {
        let player = this.playerService.GetPlayer(ws);
        let room = this.roomService.GetRoom(player);
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