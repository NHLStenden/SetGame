import {CardList} from "./CardList.js";
import {HttpService} from "./HttpService.js";
import {BoardRenderer} from "./BoardRenderer.js";
import {SelectionList} from "./SelectionList.js";

export class Board {

    /* CardList */ cardsOnBoard;
    /* Number */ idDeck;
    /* HttpService */ httpService;
    /* string */ htmlID;
    /* BoardRenderer */ boardRenderer;
    /* function */ callbackSelectionChanged;
    /* function */ callbackSetFound;

    constructor(htmlID) {
        this.httpService = new HttpService('http://localhost:5000');
        this.cardsOnBoard = new CardList();
        this.htmlID = htmlID;
        this.boardRenderer = new BoardRenderer('shapes');
        this.callbackSelectionChanged = [];
        this.callbackSetFound = [];
    }// constructor

    setup() /* promise */ {
        this.setupClickHandler()

        const promise = new Promise((resolve, reject) => {
            this.getNewDeck().then(
                () => {
                    this.addCardsToBoard(12);
                    resolve();
                },
                (error) => reject(error)
            );

        });

        return promise;
    }// setup

    addCallbackSelectionChanged(/* function */ func) /* void */ {
        this.callbackSelectionChanged.push(func);
    }// addCallbackSelectionChanged

    addCallbackSetFound(/*function */ func) /* void */ {
        this.callbackSetFound.push(func);
    }// addCallbackSetFound

    callCallbackFunctions(/* function[] */ funcs, ...params) {
        funcs.forEach(func => func(...params));
    }// callCallbackFunctions

    addCardsToBoard(nrOfItems) {
        const promises= this.getCardsFromBackend(nrOfItems);
        promises.then(
            (cards) => {
                this.cardsOnBoard.addMultiple(cards);
                this.boardRenderer.setCards(this.cardsOnBoard);
                this.boardRenderer.render();
            },
            (error) => reject(error)
        );
    }// addCardsToBoard

    setupClickHandler(){
        const container = document.getElementById(this.htmlID);
        container.addEventListener('click', (evt) => this.handleClickEvent(evt));
    }// setupClickHandler

    handleClickEvent(evt) {

        // check if a div.shape element was clicked
        const clickedElement = this.isClickedElementACard(evt);

        if (clickedElement !== undefined){
            this.handleSelectionChange(clickedElement);
        }
        else{
            console.log('Clicked something else');
        }
    }// handleClickEvent

    handleSelectionChange(/* DOMNode */ clickedElement) {
        this.boardRenderer.clickCard(clickedElement);

        const cardsNewSelected = this.boardRenderer.cardsSelected;
        const selectionCheckResult  = this.isSelectionASet(cardsNewSelected);
        this.callCallbackFunctions(this.callbackSelectionChanged, cardsNewSelected, selectionCheckResult);

        if (cardsNewSelected.length === 3 ) {
            if (selectionCheckResult.isItASet){
                this.callCallbackFunctions(this.callbackSetFound, cardsNewSelected);

                this.cardsOnBoard.removeCards(cardsNewSelected);

                this.boardRenderer.removeSelectedCards();

                // new amount of cards needs to 12; if there are more, do not add
                const cardsToAdd = 12 - this.cardsOnBoard.length;

                if (cardsToAdd > 0 ) {
                    this.addCardsToBoard(cardsToAdd);
                }
            }
        }
    }// handleSelectionChange

    get cardsSelected() {
        return this.boardRenderer.cardsSelected;
    }

    get cards() {
        return this.cardsOnBoard.cards;
    }


    /**
     * Check if the selected cards on the board form a set. The array supplied is a list of DOMNodes containing a valid
     * id-attribute to search in the 'cardsOnBoard' list
     * @returns {SetCheckResult}
     */
    isSelectionASet(/* DOMNode[] */ cards) /* SetCheckResult */{
        const selections = new SelectionList();

        // collect info on selected HTML-div's containing a graphical representation of a card.
        for (const card of cards) {
            selections.addCardFromDeck(this.cardsOnBoard, card.id);
        }
        return selections.checkIfSelectionIsASet();
    }// isSelectionASet

    /**
     * Investigates the item clicked whether it is a card
     * @param evt
     */
    isClickedElementACard(evt) {
        let element = evt.target;

        while (element.id !== 'shapes') {
            if (element.tagName === 'DIV' && element.classList.contains('card')) {
                return element;
            }
            else {
                element = element.parentElement;
            }

        }
        if (element.id === 'shapes') {
            return undefined;
        }
    }


    /**
     * Get one new deck; for now discard the cards and only keep the id for future calls
     */
    getNewDeck() /* void */ {
        const that = this;
        return new Promise((resolve, reject) => {
            this.httpService.getNewDeck().then(
                (/* ?? */ deck) => {
                    that.idDeck = deck.deckId;
                    resolve();
                },
                (error) => {
                    reject(error)
                }
            )
        });
    }//getNewDeck

    getCardsFromBackend(nrOfItemsToTake) /* Promise */{
        const requests = [];
        for (let i = 0; i < nrOfItemsToTake; i++) {
            const oneRequest = this.httpService.getOneCardFromDeck(this.idDeck);
            requests.push(oneRequest);
        }
        return Promise.all(requests);
    }//getCards
}