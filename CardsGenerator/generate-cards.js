import { CardsGenerator } from './CardsGenerator.js';

// This script generates and saves the card decks to JSON files.
// It will create the files if they don't exist, or load them if they do.

console.log("Checking for and generating card decks...");

// Trigger the generation/loading of the mask deck
CardsGenerator.getMaskDeck();

// Trigger the generation/loading of the player deck
CardsGenerator.getPlayerDeck();

console.log("Decks are ready in the /CardsData directory.");


