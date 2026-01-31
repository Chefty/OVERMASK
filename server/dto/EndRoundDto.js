export class EndRoundDto
{
    player1EndRound;
    player2EndRound;
    
    constructor(player1EndRound, player2EndRound){
        this.player1EndRound = player1EndRound;
        this.player2EndRound = player2EndRound;
    }

    writeToBuffer(buffer)
    {
       this.player1EndRound.writeToBuffer(buffer);
       this.player2EndRound.writeToBuffer(buffer);
    }
}