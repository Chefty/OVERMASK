export class ConnectToRoomWithCodeDto
{
    roomCode;
    reason;
    
    constructor(roomCode, reason){
        this.roomCode = roomCode;
        this.reason = reason;
    }

    writeToBuffer(buffer)
    {
        buffer.writeInt32BE(this.roomCode);
        let length = this.reason.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.reason);
    }
}