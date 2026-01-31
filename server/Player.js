export class Player
{
    playerId;
    userName;
    ws;

    score;
    currentCardId;

    constructor(playerId, userName, ws)
    {
        this.playerId = playerId;
        this.userName = userName;
        this.ws = ws;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerId);

        length = this.userName.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.userName);
    }

    toString = function() {
        return `[Player] id: ${this.playerId}, userName: ${this.userName}`;
    }
}