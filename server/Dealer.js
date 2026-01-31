import { shuffleArray } from '../CardsGenerator/utils.js';
import {CardDto} from "./dto/CardDto.js";

export class Dealer {
    #playerDeckIndices;
    #maskDeckIndices;
    #originalPlayerDeck;
    #originalMaskDeck;
    
    #currentStack;
    #currentPlayer1CardId;
    #currentPlayer2CardId;
    #maskCardId;
    
    #player1Score = 0;
    #player2Score = 0;
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
        this.#player1Score = 0;
        this.#player2Score = 0;
        this.#playerLeading = 1;
        this.#currentStack = null;
        this.#currentPlayer1CardId = null;
        this.#currentPlayer2CardId = null;
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
    
    EndOfRound()
    {
        const player1Card = this.#originalPlayerDeck[this.#currentPlayer1CardId];
        const player2Card = this.#originalPlayerDeck[this.#currentPlayer2CardId];
        const maskCard = this.#originalMaskDeck[this.#maskCardId];
        
        let bottomCard, middleCard;
        if (this.#playerLeading === 1) {
            bottomCard = player1Card;
            middleCard = player2Card;
        } else {
            bottomCard = player2Card;
            middleCard = player1Card;
        }
        
        const stackedGrid = new Array(16);
        for (let i = 0; i < 16; i++) {
            stackedGrid[i] = bottomCard.grid[i];
            
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
        for (let i = 0; i < 16; i++) {
            if (stackedGrid[i] === CardDto.RED) {
                redCount++;
            } else if (stackedGrid[i] === CardDto.BLUE) {
                blueCount++;
            }
        }
        
        this.#player1Score += redCount;
        this.#player2Score += blueCount;

        if (this.#player1Score > this.#player2Score) {
            this.#playerLeading = 1;
        } else if (this.#player2Score > this.#player1Score) {
            this.#playerLeading = 2;
        }
        
        else {
            this.#playerLeading = Math.random() < 0.5 ? 1 : 2;
        }
    }
    
    GetCurrentStack() {
        return this.#currentStack;
    }
    
    SetPlayerCard(player, cardId) {
        if (player === 1) {
            this.#currentPlayer1CardId = cardId;
        } else if (player === 2) {
            this.#currentPlayer2CardId = cardId;
        }
    }

    GetPlayer1Card() {
        return this.#currentPlayer1CardId;
    }
    
    GetPlayer2Card() {
        return this.#currentPlayer2CardId;
    }
    
    GetPlayer1Score() {
        return this.#player1Score;
    }
    
    GetPlayer2Score() {
        return this.#player2Score;
    }
    
    GetLeadingPlayer() {
        return this.#playerLeading;
    }
}