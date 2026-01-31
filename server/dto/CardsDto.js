import * as fs from 'fs';
import { Card } from '../Card.js';

const MASK_DECK_FILE = './CardsData/mask-deck.json';
const PLAYER_DECK_FILE = './CardsData/player-deck.json';

export class CardsDto {
    maskDeck = [];
    playerDeck = [];

    constructor(maskDeck, playerDeck) {
        this.maskDeck = maskDeck;
        this.playerDeck = playerDeck;
    }

    /**
     * Loads card decks from JSON files and returns a new CardsDto instance.
     * @returns {CardsDto}
     */
    static fromFiles() {
        if (!fs.existsSync(MASK_DECK_FILE) || !fs.existsSync(PLAYER_DECK_FILE)) {
            throw new Error("Card deck JSON files not found. Please run 'node server/generate-cards.js' first.");
        }

        const maskData = fs.readFileSync(MASK_DECK_FILE, 'utf-8');
        const playerData = fs.readFileSync(PLAYER_DECK_FILE, 'utf-8');

        const maskDeck = JSON.parse(maskData).map(p => new Card(p.grid));
        const playerDeck = JSON.parse(playerData).map(p => new Card(p.grid));

        return new CardsDto(maskDeck, playerDeck);
    }
}
