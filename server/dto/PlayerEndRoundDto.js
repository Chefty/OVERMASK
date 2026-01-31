export class PlayerEndRoundDto
{
    playerId;
    playerCardId;
    playerScore;
    newCardId;
    
    constructor(playerId, playerCardId, playerScore, newCardId, playerIdOnBottom){
        this.playerId = playerId;
        this.playerCardId = playerCardId;
        this.playerScore = playerScore;
        this.newCardId = newCardId;
        this.playerIdOnBottom = playerIdOnBottom;
    }

    writeToBuffer(buffer)
    {
        let length = this.playerId.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerId);
        
        buffer.writeUInt8(playerCardId);
        buffer.writeUInt8(playerScore);
        buffer.writeUInt8(newCardId);

        length = this.playerIdOnBottom.length;
        buffer.writeUInt8(length);
        buffer.writeString(this.playerIdOnBottom);
    }
}