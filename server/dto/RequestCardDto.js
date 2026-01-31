export class RequestCardDto
{
    maskCardId;
    
    constructor(maskCardId){
        this.maskCardId = maskCardId;
    }

    writeToBuffer(buffer)
    {
        buffer.writeUInt8(maskCardId);
    }

    toString()
    {
        return `[RequestCardDto] maskCardId: ${this.maskCardId}`;
    }
}