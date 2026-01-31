export class ConnectDto
{
    connectionId;
    userName;

    constructor(buffer){
        let leng = buffer.readUInt8();
        this.connectionId = buffer.readString(leng);
        leng = buffer.readUInt8();
        this.userName = buffer.readString(leng);
    }
}