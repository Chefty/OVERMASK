import { Connection } from "./Connection.js";

export class ConnectionService
{
    connectionMap = new Map();

    CreateConnection(ws, connectionDto)
    {
        let connection =  new Connection(connectionDto.connectionId,  connectionDto.userName, ws);
        this.connectionMap.set(ws, connection);
        console.log("[ConnectionService] Add connection > " + connection);
        return connection;
    }

    GetConnection(ws)
    {
        return this.connectionMap.get(ws);
    }

    DeleteConnection(ws)
    {
        let connection = this.connectionMap.get(ws);
        this.connectionMap.delete(ws);
        console.log("[ConnectionService] Remove connection > " + connection);
        return connection;
    }
}