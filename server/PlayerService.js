export class PlayerService
{
    playerMap = new Map();

    CreatePlayer(ws, player)
    {
        player.InjectWs(ws);
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