import { ConnectToRoomWithCodeDto } from "./dto/ConnectToRoomWithCodeDto.js";

export class RoomConnectionResult
{
    result;
    failedDto;
    reason;

    constructor(result, roomCode, reason)
    {
        this.result = result;

        if(!result)
            this.failedDto = new ConnectToRoomWithCodeDto(roomCode, reason);
    }
}