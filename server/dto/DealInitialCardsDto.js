export class DealInitialCardsDto
{
    cards;
    
    constructor(cards){
        this.cards = cards;
    }

    writeToBuffer(buffer)
    {
        let length = this.cards.length;
        buffer.writeUInt8(length);
        for (let i = 0; i < length; i++) {
            buffer.writeUInt8(this.cards[i]);
        }
    }

    toString()
    {
        return `[DealInitialCardsDto] cards: ${this.cards.length}`;
    }
}