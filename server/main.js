import { WebSocketServer } from "ws";
import * as http from "http";
import { DtoService } from "./dto/DtoService.js";
import { ConnectionService } from "./ConnectionService.js";
import { RoomService } from "./RoomService.js";
import StreamBuffer from "streambuf";
import getPort from "get-port";

(async () => {
  const PORT = await getPort({ port: [8080, 8081, 8082, 3000] }); // Try these ports

  const server = http.createServer((req, res) => {
    res.writeHead(200, { "Content-Type": "text/plain" });
    res.end("WebSocket server is running");
  });

  const ws = new WebSocketServer({ server });
  let connectionService = new ConnectionService();
  let roomService = new RoomService();
  let dtoService = new DtoService(connectionService, roomService);

  ws.on("connection", (ws) => {
    ws.on("message", (message) => {
      let buffer = StreamBuffer.from(message);
      dtoService.Parse(ws, buffer);
    });

    ws.on("close", () => {
      let connection = connectionService.DeleteConnection(ws);
      roomService.RemoveConnection(connection);
    });
  });

  server.listen(PORT, () => {
    console.log(`WebSocket server is listening on port ${PORT}`);
  });
})();
