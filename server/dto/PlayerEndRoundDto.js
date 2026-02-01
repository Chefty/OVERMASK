export class PlayerEndRoundDto
{
    playerId;
    color;
    playerCardId;
    playerScore;
    newCardId;
    
    constructor(playerId, color, playerCardId, playerScore, newCardId){
        this.playerId = playerId;
        this.color = color;
        this.playerCardId = playerCardId;
        this.playerScore = playerScore;
        this.newCardId = newCardId;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerId);
        
        buffer.writeUInt8(this.color);
        
        buffer.writeUInt8(this.playerCardId);
        buffer.writeUInt8(this.playerScore);
        buffer.writeUInt8(this.newCardId);
    }
}