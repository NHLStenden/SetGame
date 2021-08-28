import {Card} from "./Card.js";

export class GUICard  {
    /* Card */ card;
    /* string */ html;

    constructor(/* Card */ card) {
        this.card = card;
        this.html = this.createHTMLFromCard();
    }

    createHTMLFromCard() {
        let shapes = '';
        for (let i = 0; i < this.card.nrOfShapes; i++) {
            const shapeTemplate = `
                <svg 
                        viewBox="0 0 150 120" 
                        width="150px" height="120px">
                        <use xlink:href="#svg_defs"/>
                        <use xlink:href="#card_${this.card.shape}" class="shape ${this.card.fill} ${this.card.color}"/>
                </svg>`;
            shapes += shapeTemplate;
        }// for items on card
        const cardTemplate = `<div id="${this.card.id}" class="card ${this.card.shape}">${shapes}</div>`;

        return cardTemplate;
    }

}