import { Player } from "./Player.js";

export class PlayerService
{
    playerMap = new Map();

    CreatePlayer(ws, playerDto)
    {
        let player =  new Player(playerDto.playerId,  playerDto.userName, ws);
        this.playerMap.set(ws, player);
        console.log("[PlayerService] Add player > " + player);
        return player;
    }

    GetPlayer(ws)
    {
        return this.playerMap.get(ws);
    }

    DeletePlayer(ws)
    {
        let player = this.playerMap.get(ws);
        this.playerMap.delete(ws);
        console.log("[PlayerService] Remove player > " + player);
        return player;
    }
}