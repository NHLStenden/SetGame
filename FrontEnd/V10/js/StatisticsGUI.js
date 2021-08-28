import {StatisticsKeeper} from "./StatisticsKeeper.js";

export class StatisticsGUI {
    /* StatisticsKeeper */ stats;

    constructor(htmlID) {
        this.stats = new StatisticsKeeper();
        this.render();
    }

    increaseNrOfAttempts() {
        this.stats.nrOfAttempts++;
        this.render();
    }

    increaseNrOfSetsFound() {
        this.stats.nrOfSetsFound++;
        this.render();
    }

    increaseNrOfHintsUsed() {
        this.stats.nrOfHintsUsed++;
        this.render();
    }

    setNrOfCardsLeft(/* Number */ nr) {
        this.stats.nrOfCardsLeft = nr;
        this.render();
    }

    render() /*void*/ {
        this.renderOneItem('cardsleft',     this.stats.nrOfCardsLeft, 'There are', 'cards left');
        this.renderOneItem('hintsused',     this.stats.nrOfHintsUsed, 'You used', 'hints');
        this.renderOneItem('nrOfTurns',     this.stats.nrOfAttempts,  'You did', 'attempts');
        this.renderOneItem('nrOfSetsFound', this.stats.nrOfSetsFound, 'You found', 'sets');
    }

    renderOneItem(/* string*/ htmlID, /* any*/ value, /* string */ prefix = '', /* string */ suffix= '') {
        const div = document.getElementById(htmlID);
        if (div) {
            div.innerHTML = `<span class="prefix">${prefix}</span> <span class="value">${value}</span> <span class="suffix">${suffix}</span>`;
        }
    }
}