export class StatisticsKeeper {
    /* Number */ nrOfAttempts;
    /* Number */ nrOfCardsLeft;
    /* Number */ nrOfSetsFound;
    /* Number */ nrOfHintsUsed;

    constructor() {
        this.nrOfAttempts  = 0;
        this.nrOfCardsLeft = 0;
        this.nrOfSetsFound = 0;
        this.nrOfHintsUsed = 0;
    }
}