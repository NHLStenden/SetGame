export class StatisticsKeeper {
  nrOfAttempts: number;
  nrOfCardsLeft: number;
  nrOfSetsFound: number;
  nrOfHintsUsed: number;

  constructor() {
    this.nrOfAttempts  = 0;
    this.nrOfCardsLeft = 0;
    this.nrOfSetsFound = 0;
    this.nrOfHintsUsed = 0;
  }
}
