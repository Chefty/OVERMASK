export class Player
{
    static RED = 0x01;
    static BLUE = 0x02;

    playerId;
    userName;
    availableCards = [];
    ws;
    color;
    
    constructor(buffer)
    {
        let leng = buffer.readUInt8();
        this.playerId = buffer.readString(leng);

        leng = buffer.readUInt8();
        this.userName = buffer.readString(leng);

        this.color = buffer.readUInt8();

        leng = buffer.readUInt8();
        this.availableCards = [];
        for (let i = 0; i < leng; i++)
            this.availableCards.push(buffer.readUInt8());
    }

    InjectWs(ws)
    {
        this.ws = ws;
    }

    SetColor(color)
    {
        this.color = color;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerId);

        length = this.userName.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.userName);

        buffer.writeUInt8(this.color);

        length = this.availableCards.length;
        buffer.writeUInt8(length);

        for (let i = 0; i < length; i++)
            buffer.writeUInt8(this.availableCards[i]);
    }

    toString = function() {
        return `[Player] id: ${this.playerId}, userName: ${this.userName}`;
    }
}