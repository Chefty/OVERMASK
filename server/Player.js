export class Player
{
    connectionId;
    userName;
    ws;

    constructor(connectionId, userName, ws)
    {
        this.connectionId = connectionId;
        this.userName = userName;
        this.ws = ws;
    }

    writeToBuffer(buffer)
    {
        let length = this.connectionId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.connectionId);

        length = this.userName.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.userName);
    }

    toString = function() {
        return `[Connection] id: ${this.connectionId}, userName: ${this.userName}`;
    }
}