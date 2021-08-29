import {Card} from "./card";

export class CardList {
  _cards: Card[];

  constructor() {
    this._cards = [];
  }

  get length() {
    return this._cards.length;
  }

  get cards() {
    return this._cards;
  }

  add(card: Card): void{
    if (card !== undefined) {
      this._cards.push(card);
    }

  }

  addMultiple(cards: Card[]) /* void */ {
    cards.forEach(card => this.add(card));
  }

  findIndex(card: Card): number {
    return this._cards.findIndex (c => c.id === card.id);
  }

  findByID(id: string): Card | undefined {
    return this._cards.find(c => c.id === id);
  }

  findIndexByID(id: string): number {
    return this._cards.findIndex(c => c.id === id);
  }

  forEach(callback: (arg: any, index: number, array: Card[]) => void) :void {
    this._cards.forEach((card, index, array) => {
      callback(card, index, array);
    })
  }// forEach

  removeCards(domNodeCards: HTMLElement[]){
    for(const domNodeCard of domNodeCards) {
      const idx = this.findIndexByID(domNodeCard.id);
      if (idx !== -1) {
        this._cards.splice(idx, 1);
      }
    }
  }// removeCards
}
