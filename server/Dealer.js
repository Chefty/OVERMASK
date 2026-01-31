import { CardsDto } from './dto/CardsDto.js';
import { shuffleArray } from '../CardsGenerator/utils.js';

export class Dealer {
    #playerDeck;
    #maskDeck;
    #originalPlayerDeck;
    #originalMaskDeck;

    constructor(cardsDto) {
        this.#originalPlayerDeck = [...cardsDto.playerDeck];
        this.#originalMaskDeck = [...cardsDto.maskDeck];
        this.reset();
    }

    /**
     * Resets the decks to their original state and shuffles them for a new game.
     */
    reset() {
        this.#playerDeck = shuffleArray([...this.#originalPlayerDeck]);
        this.#maskDeck = shuffleArray([...this.#originalMaskDeck]);
    }

    drawPlayerCard() {
        if (this.#playerDeck.length > 0) {
            return this.#playerDeck.pop();
        }
        return null;
    }

    drawMaskCard() {
        if (this.#maskDeck.length > 0) {
            return this.#maskDeck.pop();
        }
        return null;
    }

    remainingPlayerCards() {
        return this.#playerDeck.length;
    }

    remainingMaskCards() {
        return this.#maskDeck.length;
    }
}