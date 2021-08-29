import {CardList} from "./card-list";

export class HintsList {
  sets: CardList[];

  constructor() {
    this.sets = [];
  }

  addSet(cardlist: CardList): void {
    this.sets.push(cardlist);
  }
}
