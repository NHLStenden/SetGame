export class Card {
  fill: string;
  color: string;
  shape: string;
  nrOfShapes: number;
  id: string;
  html: string;

  constructor(fill: string, color: string, shape: string, nrOfShapes: number) {
    this.fill = fill;
    this.color = color;
    this.shape = shape;
    this.nrOfShapes = nrOfShapes;
    this.id = Card.createHTMLidFromCardProperties(this.shape, this.color, this.fill, this.nrOfShapes);
    this.html = Card.createHTMLidFromCardProperties(shape, color, fill, nrOfShapes);
  }

  /**
   * Creates an HTML id for use on the board, based on info supplied by a card object.
   * @param card
   * @return {string}
   */
  static createHTMLidFromCardProperties(shape: string, color: string, fill: string, nrOfShapes: number){
    return `${shape}|${color}|${fill}|${nrOfShapes}`;
  }

  static createHTMLIdFromExistingCard(card: Card) {
    return Card.createHTMLidFromCardProperties(card.shape, card.color, card.fill, card.nrOfShapes);
  }

}
