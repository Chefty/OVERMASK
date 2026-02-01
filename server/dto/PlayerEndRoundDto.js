export class PlayerEndRoundDto
{
    playerId;
    playerCardId;
    playerScore;
    newCardId;
    
    constructor(playerId, playerCardId, playerScore, newCardId){
        this.playerId = playerId;
        this.playerCardId = playerCardId;
        this.playerScore = playerScore;
        this.newCardId = newCardId;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerId);
        
        buffer.writeUInt8(this.playerCardId);
        buffer.writeUInt8(this.playerScore);
        buffer.writeUInt8(this.newCardId);
    }
}