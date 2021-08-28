export class Card {
    fill;
    color;
    shape;
    nrOfShapes;
    id;
    html;

    constructor(fill, color, shape, nrOfShapes) {
        this.fill = fill;
        this.color = color;
        this.shape = shape;
        this.nrOfShapes = nrOfShapes;
        this.id = Card.createHTMLidFromCardProperties(this.shape, this.color, this.fill, this.nrOfShapes);
    }

    /**
     * Creates an HTML id for use on the board, based on info supplied by a card object.
     * @param card
     * @return {string}
     */
    static createHTMLidFromCardProperties(shape, color, fill, nrOfShapes){
        return `${shape}|${color}|${fill}|${nrOfShapes}`;
    }

    static createHTMLIdFromExistingCard(card) {
        return Card.createHTMLidFromCardProperties(card.shape, card.color, card.fill, card.nrOfShapes);
    }

}