import { WebSocketServer } from "ws";
import * as http from "http";
import { DtoService } from "./dto/DtoService.js";
import { PlayerService } from "./PlayerService.js";
import { RoomService } from "./RoomService.js";
import StreamBuffer from "streambuf";
import { CardsService } from "./CardsService.js";

const PORT = process.env.PORT || 8080; // Railway sets this

const server = http.createServer((req, res) => {
  res.writeHead(200, { "Content-Type": "text/plain" });
  res.end("WebSocket server is running");
});

const ws = new WebSocketServer({ server });
let cardsService = CardsService.fromFiles();
let playerService = new PlayerService();
let roomService = new RoomService(cardsService);
let dtoService = new DtoService(playerService, roomService, cardsService);

ws.on("connection", (ws) => {
  console.log("Client connected"); // Add logging

  ws.on("message", (message) => {
    let buffer = StreamBuffer.from(message);
    dtoService.Parse(ws, buffer);
  });

  ws.on("close", () => {
    console.log("Client disconnected"); // Add logging
    let player = playerService.DeletePlayer(ws);
    roomService.RemovePlayer(player);
  });
});

server.listen(PORT, () => {
  console.log(`WebSocket server is listening on port ${PORT}`);
});
