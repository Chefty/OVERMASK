import { shuffleArray } from '../CardsGenerator/utils.js';

export class Dealer {
    #playerDeckIndices;
    #maskDeckIndices;
    #originalPlayerDeck;
    #originalMaskDeck;

    constructor(cardsDto) {
        this.#originalPlayerDeck = [...cardsDto.playerDeck];
        this.#originalMaskDeck = [...cardsDto.maskDeck];
        this.Reset();
    }

    /**
     * Resets the decks to their original state and shuffles them for a new game.
     */
    Reset() {
        this.#playerDeckIndices = shuffleArray([...Array(this.#originalPlayerDeck.length).keys()]);
        this.#maskDeckIndices = shuffleArray([...Array(this.#originalMaskDeck.length).keys()]);
    }

    /**
     * Draws a card from the player deck and returns its index (ID).
     * @returns {number|null} The index of the drawn card, or null if the deck is empty.
     */
    DrawPlayerCard() {
        if (this.#playerDeckIndices.length > 0) {
            return this.#playerDeckIndices.pop();
        }
        return null;
    }

    /**
     * Draws a card from the mask deck and returns its index (ID).
     * @returns {number|null} The index of the drawn card, or null if the deck is empty.
     */
    DrawMaskCard() {
        if (this.#maskDeckIndices.length > 0) {
            return this.#maskDeckIndices.pop();
        }
        return null;
    }
}