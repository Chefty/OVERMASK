import { shuffleArray, isValid } from './dto/utils.js';
import { Card } from './Card.js';
import * as fs from 'fs';

const MASK_DECK_FILE = './CardsData/mask-deck.json';
const PLAYER_DECK_FILE = './CardsData/player-deck.json';
const PLAYER_CARDS_COUNT = 24;
const MASK_CARDS_COUNT = 10;
const PLAYER_PERCENTAGE_RED = 0.25;
const PLAYER_PERCENTAGE_BLUE = 0.25;
const MASK_PERCENTAGE_GRAY = 0.5;
const TOTAL_CELLS = 16;

export class CardsGenerator {
    static generateMaskDeck() {
        const deck = [];
        const grayCellsCount = Math.floor(TOTAL_CELLS * MASK_PERCENTAGE_GRAY);
        const emptyCellsCount = TOTAL_CELLS - grayCellsCount;

        while (deck.length < MASK_CARDS_COUNT) {
            let cells = new Array(emptyCellsCount).fill(Card.EMPTY).concat(new Array(grayCellsCount).fill(Card.GRAY));
            cells = shuffleArray(cells);
            const card = new Uint8Array(cells);
if (isValid(card, Card.GRAY)) {
                deck.push(new Card(Array.from(card)));
            }
        }
        return deck;
    }

    static generatePlayerDeck() {
        const deck = [];
        const redCellsCount = Math.floor(TOTAL_CELLS * PLAYER_PERCENTAGE_RED);
        const blueCellsCount = Math.floor(TOTAL_CELLS * PLAYER_PERCENTAGE_BLUE);
        const emptyCellsCount = TOTAL_CELLS - redCellsCount - blueCellsCount;

        while (deck.length < PLAYER_CARDS_COUNT) {
            let cells = new Array(emptyCellsCount).fill(Card.EMPTY)
                .concat(new Array(redCellsCount).fill(Card.RED))
                .concat(new Array(blueCellsCount).fill(Card.BLUE));
            
            const card = new Uint8Array(shuffleArray(cells));

            if (isValid(card, Card.RED) && isValid(card, Card.BLUE)) {
                deck.push(new Card(Array.from(card)));
            }
        }
        
        return shuffleArray(deck);
    }

    static getMaskDeck() {
        if (fs.existsSync(MASK_DECK_FILE)) {
            const data = fs.readFileSync(MASK_DECK_FILE, 'utf-8');
            const plainCards = JSON.parse(data);
            return plainCards.map(p => new Card(p.cells));
        } else {
            const deck = this.generateMaskDeck();
            fs.writeFileSync(MASK_DECK_FILE, JSON.stringify(deck, null, 2));
            return deck;
        }
    }

    static getPlayerDeck() {
        if (fs.existsSync(PLAYER_DECK_FILE)) {
            const data = fs.readFileSync(PLAYER_DECK_FILE, 'utf-8');
            const plainCards = JSON.parse(data);
            return plainCards.map(p => new Card(p.cells));
        } else {
            const deck = this.generatePlayerDeck();
            fs.writeFileSync(PLAYER_DECK_FILE, JSON.stringify(deck, null, 2));
            return deck;
        }
    }
}
