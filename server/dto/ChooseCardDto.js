export class ChooseCardDto
{
    cardId;

    constructor(buffer)
    {
        this.cardId = buffer.readUInt8();
    }

    writeToBuffer(buffer)
    {
        buffer.writeUInt8(cardId);
    }
}