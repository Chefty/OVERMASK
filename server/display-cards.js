import { CardsDto } from './dto/CardsDto.js';
import { Card } from './Card.js';

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
            case Card.EMPTY:
                symbol = ' '; // Empty
                break;
            case Card.RED:
                symbol = 'R'; // Red
                break;
            case Card.BLUE:
                symbol = 'B'; // Blue
                break;
            case Card.GRAY:
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

try {
    const cards = CardsDto.fromFiles();
    printDeck("Mask Deck", cards.maskDeck);
    printDeck("Player Deck", cards.playerDeck);
} catch (error) {
    console.error(error.message);
}

