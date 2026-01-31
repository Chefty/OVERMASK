import * as fs from 'fs';
import { CardDto } from './dto/CardDto.js';

const MASK_DECK_FILE = './CardsData/mask-deck.json';
const PLAYER_DECK_FILE = './CardsData/player-deck.json';

export class CardsService {
    maskDeck = [];
    playerDeck = [];

    constructor(maskDeck, playerDeck) {
        this.maskDeck = maskDeck;
        this.playerDeck = playerDeck;
    }

    writeToBuffer(buffer)
    {
        let length = this.maskDeck.length;
        buffer.writeUInt8(length);
        for (let i = 0; i < length; i++)
            this.maskDeck[i].writeToBuffer(buffer);

        length = this.playerDeck.length;
        buffer.writeUInt8(length);
        for (let i = 0; i < length; i++)
            this.playerDeck[i].writeToBuffer(buffer);
    }

    /**
     * Loads card decks from JSON files and returns a new CardsDto instance.
     * @returns {CardsService}
     */
    static fromFiles() {
        if (!fs.existsSync(MASK_DECK_FILE) || !fs.existsSync(PLAYER_DECK_FILE)) {
            throw new Error("Card deck JSON files not found. Please run 'node server/generate-cards.js' first.");
        }

        const maskData = fs.readFileSync(MASK_DECK_FILE, 'utf-8');
        const playerData = fs.readFileSync(PLAYER_DECK_FILE, 'utf-8');

        const maskDeck = JSON.parse(maskData).map(p => new CardDto(p.grid));
        const playerDeck = JSON.parse(playerData).map(p => new CardDto(p.grid));

        return new CardsService(maskDeck, playerDeck);
    }
}
