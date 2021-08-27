"use strict"

import  {HttpService} from './HttpService.js';

const colors = ["red", "green", "purple"];
const fills = ["solid", "hollow", "striped"];
const shapes = ["diamond", "pill", "wave"]
const nrOfShapes = [1, 2, 3];
let cardsOnTheBoard;
let hintsUsed = 0 ;
let nrOfAttempts = 0 ;

let httpService;
let idDeck = -1;

window.onload = () => {
    httpService = new HttpService('http://localhost:5000');
    setup();
}

/**
 * Show how many cards are left in the set.
 */
function showCardsLeftInFullSet(){
    const div = document.getElementById("cardsleft");
    if (div) {
        // TODO: show real amount (not received from back-end yet)
        div.innerHTML = `Cards left: <span>0</span>`;
    }
}

function showHintsUsed() {
    const div = document.getElementById("hintsused");
    if (div) {
        div.innerHTML = `Hints used:<span>${hintsUsed}</span>`;
    }
}

function showNrOfTurns() {
    const div = document.getElementById("nrOfTurns");
    if (div) {
        div.innerHTML = `Number of attempts:<span>${nrOfAttempts}</span>`;
    }
}
function GetRandomItems(nrOfItemsToTake) {
    const requests = [];
    for (let i = 0; i < nrOfItemsToTake; i++) {
        const oneRequest = httpService.getOneCardFromDeck(idDeck);
        requests.push(oneRequest);
    }
    return Promise.all(requests);
} // GetRandomItems

/**
 * Setup everything.
 */
function setup() {
    // Setupboard returns a promise because data get from HTTP (async)
    setupBoard().then(value => {
        manageBoard();
        setupHinting();
        setupButtons();

        showNrOfTurns();
        showCardsLeftInFullSet();
        showHintsUsed();
    });
}// setup

function setupBoard() {
    cardsOnTheBoard = [];
    return new Promise((resolve, reject) => {
        httpService.getNewDeck().then(
            (data) => {
                idDeck = data.deckId;
                addNewRandomCards(12).then(value => {
                    ProcessCardsFromBackend(cardsOnTheBoard);
                    resolve();
                });
            },
            (error) => {
                console.error(error.statusText);
            }
        );
    },
        (error) => { reject(error)
    });
}// setupBoard

/**
 * Processes the cards retrieved from the backend so that an HTML property is added to each
 * card that can be added later.
 * @param cards
 */
function ProcessCardsFromBackend(cards){
    let cardNr = 1;
    for (const card of cards) {
        let shapes = '';
        for (let i = 0; i < card.nrOfShapes; i++) {
            const shapeTemplate = `
                <svg 
                        viewBox="0 0 150 120" 
                        width="150px" height="120px">
                        <use xlink:href="#svg_defs"/>
                        <use xlink:href="#card_${card.shape}" class="shape ${card.fill} ${card.color}"/>
                </svg>`;
            shapes += shapeTemplate;
        }// for items on card
        const uniqueID = createHTMLidFromCardProperties(card);
        const cardTemplate = `<div id="${uniqueID}" class="card ${card.shape}">${shapes}</div>`;
        card.html = cardTemplate;
        card.id = createHTMLidFromCardProperties(card);
        cardNr++;
    }
}//ProcessCardsFromBackend

function addCards(cards) {
    // get the container from the HTML
    const container = document.getElementById("shapes");
    for (const card of cards) {
        cardsOnTheBoard.push(card);
        container.innerHTML += card.html;
    }
}

function manageBoard() {

    const container = document.getElementById("shapes");
    container.addEventListener('click', handleClickEvent);

}// manageBoard

function handleClickEvent(evt) {

    const clickedElement = isClickedElementACard(evt);

    // check if a div.shape element was clicked
    if (clickedElement !== undefined){

        const selectedCardClasses = clickedElement.classList;
        const cardsSelected       = document.querySelectorAll('div.card.selected');

        if (selectedCardClasses.contains('selected')) {
            clickedElement.classList.remove('selected');
        }
        else{
            if (cardsSelected.length < 3 ) {
                clickedElement.classList.toggle('selected');
            }
        }

        const cardsNewSelected = document.querySelectorAll('div.card.selected');

        if (cardsNewSelected.length === 3 ) {
            nrOfAttempts++;
            showNrOfTurns();
            if (isSelectionASet()){
                console.log('Een set gevonden!');

                RemoveSelectedCards();

                // new amount of cards needs to 12; if there are more, do not add
                const cardsToAdd = 12 - cardsOnTheBoard.length;

                if (cardsToAdd > 0 ) {
                    addNewRandomCards(cardsToAdd).then();
                }

                clearTableWithSelectionInfo();
                clearHintsAsCards();
            }
        }
        else{
            clearTableWithSelectionInfo();
        }

    }
    else{
        console.log('Clicked something else');
        clearTableWithSelectionInfo();
    }
}// handleClickEvent

/**
 * Investigates the item clicked whether it is a card
 * @param evt
 */
function isClickedElementACard(evt) {
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
 * Remove the cards that are selected on the board. Also remove them from the internal administration
 * @constructor
 */
function RemoveSelectedCards(){
    const cardsNewSelected = document.querySelectorAll('div.card.selected');

    // remove cards in the set
    for (const card of cardsNewSelected) {
        card.parentElement.removeChild(card);

        const idx = findCardIndexFromHTMLid(card.id);

        if (idx !== -1 ){
            cardsOnTheBoard.splice(idx, 1);
        }
    }
}// RemoveSelectedCards

/**
 * Checks if two cards are the same by comparing all the four properties
 * @param card1
 * @param card2
 */
function areCardsTheSame(card1, card2) {
    return card1.fill === card2.fill &&
        card1.nrOfShapes === card2.nrOfShapes &&
        card1.color === card2.color &&
        card1.shape === card1.share;
}

/**
 * Creates an HTML id for use on the board, based on info supplied by a card object.
 * @param card
 * @return {string}
 */
function createHTMLidFromCardProperties(card){
    return `${card.shape}|${card.color}|${card.fill}|${card.nrOfShapes}`;
}

/**
 * Tries to find a card in the array cardsOnTheBoard based on an HTML id (constructed by the function
 * createHTMLidFromCardProperties()
 * @param id
 * @return {Card|undefined} undefined if not found, a Card if a match is found.
 */
function findCardFromHTMLid(id) {
    return cardsOnTheBoard.find(card => createHTMLidFromCardProperties(card) === id);
}

/**
 * Tries to find the index of a Card in the array cardsOnTheBoard based on an HTML id (constructed by the function
 * createHTMLidFromCardProperties()
 * @param id
 * @return {int} -1 if not found, otherwise an index in the array cardsOnTheBoard
 */
function findCardIndexFromHTMLid(id) {
    return cardsOnTheBoard.findIndex(card => createHTMLidFromCardProperties(card) === id);
}



/**
 * Find a number of new random items and add them to the DOM.
 * The general CLICK-eventhandler on the parent (div.container) can handle the clicks
 * @returns Promise ; use .then() to wait for the results
 */
function addNewRandomCards(nrOfNewCards) {
    return new Promise((resolve, reject) => {
        GetRandomItems(nrOfNewCards).then(
            (cards) => {
                ProcessCardsFromBackend(cards);
                addCards(cards);
                showCardsLeftInFullSet();
                resolve();
            },
            (error) => {
                console.log(error);
            });
    });
}// AddNewRandomCards

/**
 * Check if the selected cards on the board form a set.
 * @returns {boolean|boolean}
 */
function isSelectionASet() {
    const selectedCardsGUI = document.querySelectorAll('div.card.selected');
    const cardsInfo = [];

    // collect info on selected HTML-div's containing a graphical representation of a card.
    for (const card of selectedCardsGUI) {
        const cardInfo = findCardFromHTMLid(card.id);
        if (cardInfo !== undefined) {
            cardsInfo.push(cardInfo);
        }
    }
    return isCardsetASet(cardsInfo, true);
}

function isCardsetASet(cardsInfo, showInTable) {

    const setColor      = new Map();
    const setShape      = new Map();
    const setFill       = new Map();
    const setNrOfShapes = new Map();

    // collect info on selected cards
    for(const cardInfo of cardsInfo) {
        // Register COLOR as new or update count
        if (!setColor.has(cardInfo.color)) {
            setColor.set(cardInfo.color, 1);
        }
        else {
            const curValue = setColor.get(cardInfo.color);
            setColor.set(cardInfo.color, curValue + 1);
        }

        // Register FILL as new or update count
        if (!setFill.has(cardInfo.fill)) {
            setFill.set(cardInfo.fill, 1);
        }
        else{
            const curValue = setFill.get(cardInfo.fill);
            setFill.set(cardInfo.fill, curValue + 1);
        }

        // Register SHAPE as new or update count
        if (!setShape.has(cardInfo.shape)) {
            setShape.set(cardInfo.shape, 1);
        }
        else{
            const curValue = setShape.get(cardInfo.shape);
            setShape.set(cardInfo.shape, curValue + 1);
        }

        // Register NROFSHAPES as new or update count
        if (!setNrOfShapes.has(cardInfo.nrOfShapes)) {
            setNrOfShapes.set(cardInfo.nrOfShapes, 1);
        }
        else{
            const curValue = setNrOfShapes.get(cardInfo.nrOfShapes);
            setNrOfShapes.set(cardInfo.nrOfShapes, curValue + 1);
        }
    }

    const allDiffColors      = setColor.size === 3;
    const allDiffShapes      = setShape.size === 3;
    const allDiffFills       = setFill.size === 3;
    const allDiffNrOfShapes  = setNrOfShapes.size === 3;

    const allSameColors     = setColor.size === 1;
    const allSameShapes     = setShape.size === 1;
    const allSameFills      = setFill.size === 1;
    const allSameNrOfShapes = setNrOfShapes.size === 1;

    const itIsASet = (allDiffColors || allSameColors) &&
        (allDiffFills || allSameFills) &&
        (allDiffNrOfShapes || allSameNrOfShapes) &&
        (allDiffShapes || allSameShapes);

    if (showInTable){
        fillTableWithSelectionInfo(allDiffColors, allDiffShapes, allDiffFills,allDiffNrOfShapes,
            allSameColors, allSameShapes, allSameFills, allSameNrOfShapes);
    }

    return itIsASet;
}// isSelectionASet

function clearTableWithSelectionInfo(){
    const tds = document.querySelectorAll("div.selectionInfo td.indicator");
    tds.forEach(element => element.innerHTML = '');
}

function fillTableWithSelectionInfo(allDiffColors, allDiffShapes, allDiffFills,allDiffNrOfShapes,
                                    allSameColors, allSameShapes, allSameFills, allSameNrOfShapes) {

    const trColors    = document.querySelectorAll("div.selectionInfo .colors td");
    const trShapes    = document.querySelectorAll("div.selectionInfo .shapes td");
    const trFills     = document.querySelectorAll("div.selectionInfo .fills td");
    const trNrOfItems = document.querySelectorAll("div.selectionInfo .nrOfshapes td");

    trColors[1].innerHTML = allDiffColors ? "✅" : "-";
    trColors[2].innerHTML = allSameColors ? "✅" : "-";

    trShapes[1].innerHTML = allDiffShapes ? "✅" : "-";
    trShapes[2].innerHTML = allSameShapes ? "✅" : "-";

    trFills[1].innerHTML = allDiffFills ? "✅" : "-";
    trFills[2].innerHTML = allSameFills ? "✅" : "-";

    trNrOfItems[1].innerHTML = allDiffNrOfShapes ? "✅" : "-";
    trNrOfItems[2].innerHTML = allSameNrOfShapes ? "✅" : "-";

}


function findAllSets(cards) {
    const setsFound = findAllSet(cards, []);
    return setsFound;
}

function setupHinting() {
    const btn = document.getElementById("showHint");
    if (btn) {
        btn.addEventListener('click', function (evt) {
            const setsFound = findAllSets(cardsOnTheBoard);
            showHintsAsCards(setsFound);
            hintsUsed++;
            showHintsUsed();
        });
    }
}

function setupButtons() {
    document.getElementById("addCard").addEventListener('click', function(evt) {
        addNewRandomCards(1);
    });
}

function showHintsInTable(hints) {
    const hintsTableBody = document.querySelector("#hints tbody");
    hintsTableBody.innerHTML = '';

    let i = 1;
    for(const hint of hints){
        let parts = '';
        for (const card of hint) {
            const oneLine = `<td class="vshape">${card.shape}</td>
                             <td class="vcolor">${card.color}</td>
                             <td class="vfill">${card.fill}</td>
                             <td class="vnrOfShapes">${card.nrOfShapes}</td>`;
            const newLine = `<tr><td>${i}</td>${oneLine}</tr>`;
            hintsTableBody.innerHTML += newLine;
        }
        i++;

    }
}// showHintsInTable

function clearHintsAsCards() {
    const hintsShapeDiv = document.querySelector("#hintShapes");
    hintsShapeDiv.innerHTML = '';
}


function showHintsAsCards(hints) {
    clearHintsAsCards();
    const hintsShapeDiv = document.querySelector("#hintShapes");
    for(const hint of hints){
        let hintCard = '';

        for(const card of hint.sort((a,b) => a.nrOfShapes - b.nrOfShapes)){
            let cardShapes = '';
            for (let s = 0; s < card.nrOfShapes; s++) {
                const oneShape = `<div>
                    <svg viewBox="0 0 150 120" width="50px" height="40px">
                        <use xlink:href="#svg_defs"></use>
                        <use xlink:href="#card_${card.shape}" class="shape ${card.fill} ${card.color}"></use>
                    </svg>
                </div>`;
                cardShapes += oneShape;
            }

            hintCard += `<div class="hintcard">${cardShapes}</div>`;
        }
        hintsShapeDiv.innerHTML += `<div class="oneHint">${hintCard}</div>` ;
    }
}// showHintsAsCards

/**
 * Find all possible sets by using backtracking.
 * @param cards the set of cards to investigate
 * @param currentSet the current set under investigation
 * @return {[]|*[]} an array comprising of valid sets
 */
function findAllSet(cards, currentSet){

    // for this level, clear the sets found
    const setsFound = [];

    // make a collection of cards not currently in the set 'currentSet' so we know which cards remain
    // to be investigated
    const otherCards = cards.filter(c => currentSet.find(c2 => c2.id === c.id) === undefined);

    // do we have a set of 3 cards? if not, keep collecting; if yes, check if it is a set.
    if (currentSet.length < 3) {

        // check if there are other cards left to be investigated
        if (otherCards.length >0){
            // cycle through all the 'other cards' using a for-loop on this level.
            for (let i = 0; i < otherCards.length-1; i++) {
                // get a new card
                const newcard = otherCards[i];

                // if we remove this card, is there anything left to check?
                // because all the cards "on the left of the card taken" do not need to be investigated
                // again
                let remainingToCheck;
                if (otherCards.length > 1) {
                    // construct a set of cards to be investigated in the next recursive call by
                    // making a copy of a slice of the cards "on the right of the taken card".
                    remainingToCheck = otherCards.slice(i+1);
                }
                else{
                    remainingToCheck = [];
                }
                // start recursing the remaining cards. first extend the current set with the new
                // card taken, and construct a new set for the next level.
                const setsFoundBelow = findAllSet(remainingToCheck, [...currentSet, newcard]);

                // add the sets found on the lower levels to the sets found.
                setsFound.push(...setsFoundBelow);
            }
        }
    }
    else {
        if (isCardsetASet(currentSet, false)) {
            // return values must always be an array, so surround the result in brackets [] to construct
            // an array with one value.
            return [currentSet];
        }
    }

    return setsFound;
}// findAllSet



