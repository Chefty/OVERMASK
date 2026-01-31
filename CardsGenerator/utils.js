/**
 * Shuffles an array in place.
 * @param {Array} array The array to shuffle.
 * @returns {Array} The shuffled array.
 */
export function shuffleArray(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

/**
 * Checks if a 4x4 card grid has any "dead" rows or columns (more than 2 blocks adjacent).
 * @param {Uint8Array} card - The 16-byte array representing the card.
 * @param {number} blockValue - The byte value representing a block.
 * @returns {boolean} True if the card is valid, false otherwise.
 */
export function isValid(card, blockValue) {
    // Check rows
    for (let i = 0; i < 4; i++) {
        for (let j = 0; j < 2; j++) {
            if (card[i * 4 + j] === blockValue &&
                card[i * 4 + j + 1] === blockValue &&
                card[i * 4 + j + 2] === blockValue) {
                return false; // Found 3 adjacent blocks in a row
            }
        }
    }

    // Check columns
    for (let i = 0; i < 4; i++) {
        for (let j = 0; j < 2; j++) {
            if (card[j * 4 + i] === blockValue &&
                card[(j + 1) * 4 + i] === blockValue &&
                card[(j + 2) * 4 + i] === blockValue) {
                return false; // Found 3 adjacent blocks in a column
            }
        }
    }

    return true;
}
