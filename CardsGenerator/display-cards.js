import { CardDto } from '../server/dto/CardDto.js';
import * as fs from 'fs';

const MASK_DECK_FILE = './CardsData/mask-deck.json';
const PLAYER_DECK_FILE = './CardsData/player-deck.json';

/**
 * Helper function to format and print a single card grid.
 * @param {Card} card - The card to print.
 */
function printCard(card) {
    let cardStr = '';
    for (let i = 0; i < 16; i++) {
        const cell = card.grid[i];
        let symbol = ' ';
        switch (cell) {
            case CardDto.EMPTY:
                symbol = ' '; // Empty
                break;
            case CardDto.RED:
                symbol = 'R'; // Red
                break;
            case CardDto.BLUE:
                symbol = 'B'; // Blue
                break;
            case CardDto.GRAY:
                symbol = 'X'; // Gray (Block)
                break;
        }
        cardStr += `[${symbol}]`;
        if ((i + 1) % 4 === 0) {
            cardStr += '\n';
        }
    }
    console.log(cardStr);
}

/**
 * Helper function to print a deck of cards.
 * @param {string} deckName - The name of the deck.
 * @param {Card[]} deck - The array of cards.
 */
function printDeck(deckName, deck) {
    console.log(`\n--- ${deckName} ---`);
    console.log(`Total cards: ${deck.length}`);
    deck.forEach((card, index) => {
        console.log(`\nCard ${index + 1}:`);
        printCard(card);
    });
}

// --- Load and Display Decks ---

if (fs.existsSync(MASK_DECK_FILE) && fs.existsSync(PLAYER_DECK_FILE)) {
    const maskData = fs.readFileSync(MASK_DECK_FILE, 'utf-8');
    const playerData = fs.readFileSync(PLAYER_DECK_FILE, 'utf-8');

    const maskDeck = JSON.parse(maskData).map(p => new CardDto(p.grid));
    const playerDeck = JSON.parse(playerData).map(p => new CardDto(p.grid));

    printDeck("Mask Deck", maskDeck);
    printDeck("Player Deck", playerDeck);
} else {
    console.log("Card deck JSON files not found. Please run 'node generate-cards.js' first.");
}

