export class Player
{
    playerId;
    userName;
    availableCards = [];
    ws;
    
    constructor(buffer)
    {
        let leng = buffer.readUInt8();
        this.playerId = buffer.readString(leng);

        leng = buffer.readUInt8();
        this.userName = buffer.readString(leng);

        leng = buffer.readUInt8();
        this.availableCards = [];
        for (let i = 0; i < leng; i++)
            this.availableCards.push(buffer.readUInt8());
    }

    InjectWs(ws)
    {
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

        length = this.availableCards.length;
        buffer.writeUInt8(length);

        for (let i = 0; i < length; i++)
            buffer.writeUInt8(this.availableCards[i]);
    }

    toString = function() {
        return `[Player] id: ${this.playerId}, userName: ${this.userName}`;
    }
}