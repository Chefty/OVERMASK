import {ConnectDto} from "./ConnectDto.js";
import {ChooseCardDto} from "./ChooseCardDto.js";
import StreamBuffer from 'streambuf';
import { Player } from "../Player.js";
import { ConnectToRoomWithCodeDto } from "./ConnectToRoomWithCodeDto.js";
import { JoinRoomWithCodeDto } from "./JoinRoomWithCodeDto.js";

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
                this.Connect(ws, new Player(buffer));
                break;
            case "CreateOrJoinRoom":
                this.CreateOrJoinRoom(ws);
                break;
            case "CreateRoomCode":
                this.CreateRoomWithCode(ws);
                break;
            case "JoinRoomCode":
                this.JoinRoomWithCode(ws, new JoinRoomWithCodeDto(buffer));
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
        this.roomService.AddPlayer(player, 0);
    }

    CreateRoomWithCode(ws)
    {
        let player = this.playerService.GetPlayer(ws);
        var randomCode = this.roomService.GetRandomRoomCode();
        this.roomService.AddPlayer(player, randomCode);
        this.Send("CreatedRoomWithCode", ws, new ConnectToRoomWithCodeDto(randomCode, "Success"));
    }

    JoinRoomWithCode(ws, joinRoomDto)
    {
        let player = this.playerService.GetPlayer(ws);
        let connectionResult = this.roomService.TryJoinRoomCode(player, joinRoomDto.roomCode);
        if(!connectionResult.result)
            this.Send("FailedToJoinRoomWithCode", ws, connectionResult.failedDto);
    }

    ReadyToPlay(ws) {
        let player = this.playerService.GetPlayer(ws);
        let room = this.roomService.GetRoom(player);
        if(player != null)
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