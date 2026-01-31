export class ConnectDto
{
    playerId;
    userName;

    constructor(buffer){
        let leng = buffer.readUInt8();
        this.playerId = buffer.readString(leng);
        leng = buffer.readUInt8();
        this.userName = buffer.readString(leng);
    }
}