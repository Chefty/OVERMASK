export class GameOverDto
{
    redScore;
    blueScore;
    
    constructor(redScore, blueScore){
        this.redScore = redScore;
        this.blueScore = blueScore;
    }

    writeToBuffer(buffer)
    {
        buffer.writeUInt8(this.redScore);
        buffer.writeUInt8(this.blueScore);
    }
}