# 🎭 OVERMASK

> A strategic card-stacking duel where masks reveal the battlefield.

**Global Game Jam 2026** • Theme: "Mask"

---

## 🎮 Game Concept

Two players, Red and Blue, compete by stacking colored cards while mask cards reveal the final battlefield.
Each round, players strategically place their cards—but only the visible cells count toward victory as the stack builds higher and higher throughout the game.

**Stack wisely. Every layer matters.**

---

## ✨ Features

- **🎴 Strategic Card Play** – Choose your cards carefully each round
- **🎭 Mask Mechanics** – Visual masks determine what's visible in the final stack
- **📊 Cumulative Scoring** – Build the combat stack round by round
- **🌐 Online Multiplayer** – Real-time turn-based WebSocket battles÷
- **🎨 Faction System** – Red vs Blue color-based competition

---

## 🛠️ Tech Stack

- **Client**: Unity (C#) with DOTween animations
- **Server**: Node.js with WebSocket
- **Hosting**: Railway (production deployment)
- **Networking**: Custom binary protocol for efficient real-time play

---

## 🚀 Getting Started

### Play Online
Connect to the hosted server and challenge other players in real-time!

### Run Locally

**Server:**
```bash
cd server
npm install
node main.js
```

**Client:**
1. Open the Unity project
2. Set the connection URL in `LobbyManager.cs`
3. Play in the Unity Editor or build

---

## 🎯 How to Play

1. **Enter your name** and connect to the server
2. **Wait for an opponent** to join your room
3. **Each round:**
   - A mask card is revealed
   - Both players secretly choose a card
   - Cards stack on top of previous rounds
   - Visible colored cells are counted and added to each players score
4. **After 5 rounds**, the player with the highest score wins!

---

## 📦 Project Structure

```
GGJ-2026/
├── Assets/           # Unity game assets and scripts
│   ├── Scripts/      # C# game logic
│   └── Scenes/       # Game scenes
├── server/           # Node.js WebSocket server
│   ├── main.js       # Server entry point
│   └── Dealer.js     # Game logic & card stacking
└── CardsGenerator/   # Card generation utilities
```

---

## 🏆 Made with ❤️ for Global Game Jam 2026

**Theme:** Mask  
**Duration:** 48 hours

### 👥 Contributors

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/Chefty">
        <img src="https://github.com/Chefty.png" width="100px;" alt="Fabien Cheftel"/>
        <br />
        <sub><b>Fabien Cheftel</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/Mukarillo">
        <img src="https://github.com/Mukarillo.png" width="100px;" alt="Mukarillo"/>
        <br />
        <sub><b>Mukarillo</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/victorfresh">
        <img src="https://github.com/victorfresh.png" width="100px;" alt="XiaoWei Victor Qian"/>
        <br />
        <sub><b>XiaoWei Victor Qian</b></sub>
      </a>
    </td>
  </tr>
</table>

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
