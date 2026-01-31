export class CardDto {
    static EMPTY = 0x00;
    static RED = 0x01;
    static BLUE = 0x02;
    static GRAY = 0x03;

    /**
     * The 16-element array representing the 4x4 grid of the card.
     * @type {number[]}
     */
    grid;

    /**
     * @param {number[]} grid A 16-element array representing the card's grid.
     */
    constructor(grid) {
        this.grid = grid;
    }

    writeToBuffer(buffer)
    {
        let length = this.grid.length;
        buffer.writeUInt8(length);
        for (let i = 0; i < length; i++) {
            buffer.writeUInt8(this.grid[i]);
        }
    }
}