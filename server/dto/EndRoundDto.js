export class EndRoundDto
{
    player1EndRound;
    player2EndRound;
    playerIdOnBottom;
    
    constructor(player1EndRound, player2EndRound, playerIdOnBottom){
        this.player1EndRound = player1EndRound;
        this.player2EndRound = player2EndRound;
        this.playerIdOnBottom = playerIdOnBottom;
    }

    writeToBuffer(buffer)
    {
       this.player1EndRound.writeToBuffer(buffer);
       this.player2EndRound.writeToBuffer(buffer);
       buffer.writeUInt8(playerIdOnBottom);
    }
}