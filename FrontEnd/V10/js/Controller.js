import {SelectionGUIRenderer} from "./SelectionGUIRenderer.js";
import { StatisticsGUI } from './StatisticsGUI.js';
import {Board} from "./Board.js";
import {HintGUI} from "./HintGUI.js";
import {HintFinder} from "./HintFinder.js";

export class Controller {
    /* Board */ board;
    /* StatisticsGUI */ statisticsGUI;
    /* HintGUI */ hintGUI;
    /* HintFinder */ hintFinder;

    constructor() {
        this.board = new Board('shapes');
        this.selectionGUIRenderer = new SelectionGUIRenderer('selectionInfo')
        this.statisticsGUI = new StatisticsGUI('stats');
        this.hintGUI = new HintGUI('hintShapes');
        this.hintFinder = new HintFinder();

        document.getElementById('addCard')
            .addEventListener('click', (evt) => this.addNewCard(evt));

        document.getElementById('showHint')
            .addEventListener('click', (evt) => this.showHint(evt));

        this.board.addCallbackSelectionChanged((this.callbackSelectionChanged).bind(this));
        this.board.addCallbackSetFound((this.callbackSetFound).bind(this));

        this.board.setup().then(() => {
                console.log('Done setup');
            },
            (error) => {
                console.log(error);
            }
        );
    }// constructor

    addNewCard(/* MouseEvent*/ evt) {
        this.board.addCardsToBoard(1);
    }// addNewCard

    showHint(/* MouseEvent */ evt) {
        const hints = this.hintFinder.getHintsFromCards(this.board.cards);
        this.hintGUI.showHintsAsCards(hints);
        this.statisticsGUI.increaseNrOfHintsUsed();
    }// showHint

    /**
     *
     * @param {SelectionList} cardsInSelection
     * @param {SetCheckResult} checkResult
     */
    callbackSelectionChanged(/*SelectionList */ cardsInSelection, /* SetCheckResult */ checkResult) /*void*/ {
        if (cardsInSelection.length === 3){
            this.statisticsGUI.increaseNrOfAttempts();
            this.selectionGUIRenderer.render(checkResult);
        }
        else {
            this.selectionGUIRenderer.clear();
        }
        this.statisticsGUI.render();
    }// callbackSelectionChanged

    /**
     *
     * @param {SelectionList} cardsInSelection
     */
    callbackSetFound(/*SelectionList */ cardsInSelection) /*void*/ {
        this.statisticsGUI.increaseNrOfSetsFound();
        this.hintGUI.clear();
        this.selectionGUIRenderer.clear();
    }// callbackSetFound

}