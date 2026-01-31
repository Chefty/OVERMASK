export class MatchStartDto
{
    player;
    opponent;
    
    constructor(player, opponent){
        this.player = player;
        this.opponent = opponent;
    }

    writeToBuffer(buffer)
    {
       this.player.writeToBuffer(buffer);
       this.opponent.writeToBuffer(buffer);
    }
}