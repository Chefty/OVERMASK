import { shuffleArray } from '../CardsGenerator/utils.js';
import {CardDto} from "./dto/CardDto.js";
import { Player } from './Player.js';

export class Dealer {
    #playerDeckIndices;
    #maskDeckIndices;
    #originalPlayerDeck;
    #originalMaskDeck;
    
    #currentStack;
    #currentRedCardId;
    #currentBlueCardId;
    #maskCardId;
    
    #redScore = 0;
    #blueScore = 0;
    #playerLeading;
    
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
        this.#redScore = 0;
        this.#blueScore = 0;
        this.#playerLeading = 1;
        // Initialize with a grey card (16 cells with value 3)
        var cadSize = this.#originalPlayerDeck[0].grid.length;
        this.#currentStack = new Uint8Array(cadSize).fill(CardDto.GREY);
        this.#currentRedCardId = -1;
        this.#currentBlueCardId = -1;
        this.#maskCardId = null;
    }

    DrawPlayerCard() {
        if (this.#playerDeckIndices.length > 0) {
            return this.#playerDeckIndices.pop();
        }
        return null;
    }
    
    DrawMaskCard() {
        if (this.#maskDeckIndices.length > 0) {
            this.#maskCardId = this.#maskDeckIndices.pop();
            return this.#maskCardId;
        }
        return null;
    }

    EndOfRound() {
        const redCard = this.#originalPlayerDeck[this.#currentRedCardId];
        const blueCard = this.#originalPlayerDeck[this.#currentBlueCardId];
        const maskCard = this.#originalMaskDeck[this.#maskCardId];

        let bottomCard, middleCard;
        if (this.#playerLeading === Player.RED) {
            bottomCard = redCard;
            middleCard = blueCard;
        } else {
            bottomCard = blueCard;
            middleCard = redCard;
        }

        const cardSize = bottomCard.grid.length;
        const stackedGrid = new Uint8Array(cardSize);

        // Start with the previous stack
        for (let i = 0; i < cardSize; i++) {
            stackedGrid[i] = this.#currentStack[i];

            if (bottomCard.grid[i] !== CardDto.EMPTY) {
                stackedGrid[i] = bottomCard.grid[i];
            }

            if (middleCard.grid[i] !== CardDto.EMPTY) {
                stackedGrid[i] = middleCard.grid[i];
            }

            if (maskCard.grid[i] !== CardDto.EMPTY) {
                stackedGrid[i] = maskCard.grid[i];
            }
        }

        this.#currentStack = stackedGrid;

        let redCount = 0;
        let blueCount = 0;
        for (let i = 0; i < cardSize; i++) {
            if (stackedGrid[i] === CardDto.RED) {
                redCount++;
            } else if (stackedGrid[i] === CardDto.BLUE) {
                blueCount++;
            }
        }

        this.#redScore += redCount;
        this.#blueScore += blueCount;

        this.#playerLeading = this.GetLeadingPlayer();
    }

    GetLeadingPlayer()
    {
        if (this.#redScore > this.#blueScore) {
            return Player.RED;
        } else if (this.#blueScore > this.#redScore) {
            return Player.BLUE;
        }
        else {
            return Math.random() < 0.5 ? Player.RED : Player.BLUE;
        }
    }
    
    GetCurrentStack() {
        return this.#currentStack;
    }
    
    SetPlayerCard(player, cardId) {
        if (player.color === Player.RED)
            this.#currentRedCardId = cardId;
        else
            this.#currentBlueCardId = cardId;
    }

    GetPlayerScore(player)
    {
        if(player.color == Player.BLUE)
            return this.#blueScore;
        return this.#redScore;
    }

    GetColorScore(color)
    {
        if(color === Player.BLUE)
            return this.#blueScore;
        return this.#redScore;     
    }

    GetPlayerCard(player)
    {
        if(player.color == Player.BLUE)
            return this.#currentBlueCardId;
        return this.#currentRedCardId;
    }
    
    GetLeadingPlayer() {
        return this.#playerLeading;
    }
}