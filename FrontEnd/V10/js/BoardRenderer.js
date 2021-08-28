import {GUICard} from "./GUICard.js";

export class BoardRenderer {
    /* CardList */ cardsOnBoard;
    /* string */ htmlID;

    constructor(htmlID) {
        this.htmlID = htmlID;
    }

    setCards(/* CardList */ cards) {
        this.cardsOnBoard = cards;
    }

    render() {
        const container = document.getElementById(this.htmlID);
        while (container.childElementCount !==0 ) {
            container.firstChild.remove();
        }
        this.cardsOnBoard.forEach((card, index, array) => {
            const guiCard = new GUICard(card);
            container.innerHTML += guiCard.html;
        });
    }

    get cardsSelected() {
        return document.querySelectorAll('div.card.selected');
    }

    clickCard(/*DOMNode */ clickedElement) {
        const selectedCardClasses = clickedElement.classList;
        const cardsSelected       = this.cardsSelected;

        if (selectedCardClasses.contains('selected')) {
            clickedElement.classList.remove('selected');
        }
        else{
            if (cardsSelected.length < 3 ) {
                clickedElement.classList.toggle('selected');
            }
        }
    }// clickCard

    /**
     * Remove the cards that are selected on the board. Also remove them from the internal administration
     */
    removeSelectedCards(){
        const cardsNewSelected = this.cardsSelected;

        // remove cards in the set
        for (const card of cardsNewSelected) {
            card.parentElement.removeChild(card);
        }
    }// removeSelectedCards
}