export class JoinRoomWithCodeDto
{
    roomCode;

    constructor(buffer)
    {
        this.roomCode = buffer.readInt32BE();
    }
}