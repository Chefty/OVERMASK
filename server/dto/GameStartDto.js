export class GameStartDto
{
    player;
    opponent;
    cardService;
    
    constructor(player, opponent, cardService){
        this.player = player;
        this.opponent = opponent;
        this.cardService = cardService;
    }

    writeToBuffer(buffer)
    {
       this.player.writeToBuffer(buffer);
       this.opponent.writeToBuffer(buffer);
       this.cardService.writeToBuffer(buffer);
    }
}