export class RoundStartDto
{
    playerColor;

    constructor(playerColor){
        this.playerColor = playerColor;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerColor.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerColor);
    }
    
    toString()
    {
        return `[RoundStartDto] playerColor: ${this.playerColor}`;
    }
}