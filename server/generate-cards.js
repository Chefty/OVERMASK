import { CardsGenerator } from './CardsGenerator.js';
import { CardsDto } from './dto/CardsDto.js';

// Get the decks from the generator
const maskDeck = CardsGenerator.getMaskDeck();
const playerDeck = CardsGenerator.getPlayerDeck();

// Create a DTO to pass the data
const cardsDto = new CardsDto(maskDeck, playerDeck);

console.log("--- Mask Deck ---");
console.log(cardsDto.maskDeck);

console.log("\n--- Player Deck ---");
console.log(cardsDto.playerDeck);

console.log("\nDecks have been generated and saved to .json files in the server directory.");
console.log("Run this script again to load the decks from the files.");

