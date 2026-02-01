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
        this.#currentPlayer1CardId = -1;
        this.#currentPlayer2CardId = -1;
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
        
        let cardSize = bottomCard.grid.length;
        const stackedGrid = new Array(cardSize);
        for (let i = 0; i < cardSize; i++) {
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
        for (let i = 0; i < cardSize; i++) {
            if (stackedGrid[i] === CardDto.RED) {
                redCount++;
            } else if (stackedGrid[i] === CardDto.BLUE) {
                blueCount++;
            }
        }
        
        this.#player1Score += redCount;
        this.#player2Score += blueCount;

        this.#playerLeading = this.GetLeadingPlayer();
    }

    GetLeadingPlayer()
    {
        if (this.#player1Score > this.#player2Score) {
            return 0;
        } else if (this.#player2Score > this.#player1Score) {
            return 1;
        }
        else {
            return Math.random() < 0.5 ? 0 : 1;
        }
    }
    
    GetCurrentStack() {
        return this.#currentStack;
    }
    
    SetPlayerCard(player, cardId) {
        if (player === 0) {
            this.#currentPlayer1CardId = cardId;
        } else if (player === 1) {
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