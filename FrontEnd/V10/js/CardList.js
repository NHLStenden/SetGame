export class CardList {
    _cards;

    constructor() {
        this._cards = [];
    }

    get length() {
        return this._cards.length;
    }

    get cards() {
        return this._cards;
    }

    add(/*Card*/ card) /* void */ {
        if (card !== undefined) {
            this._cards.push(card);
        }

    }

    addMultiple(/* Card[] */ cards) /* void */ {
        cards.forEach(card => this.add(card));
    }

    findIndex(/*Card*/ card) /*Number*/ {
        return this._cards.findIndex (c => c.id === card.id);
    }

    findByID(/*string*/ id) /* Card */ {
        return this._cards.find (c => c.id === id);
    }

    findIndexByID(/*string*/ id) /* Card */ {
        return this._cards.findIndex(c => c.id === id);
    }

    forEach(/* function(card, index, array) */ func) /* void */ {
        this._cards.forEach((card, index, array) => {
            func(card, index, array);
        })
    }

    removeCards(/* DOMNodeList */ domNodeCards){
        for(const domNodeCard of domNodeCards) {
            const idx = this.findIndexByID(domNodeCard.id);
            if (idx !== -1) {
                this._cards.splice(idx, 1);
            }
        }
    }// removeCards
}