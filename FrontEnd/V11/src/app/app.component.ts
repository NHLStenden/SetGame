import {Component, OnInit} from '@angular/core';
import {BoardControllerService} from "./services/board-controller.service";
import {HintFinder} from "./models/hint-finder";
import {CardList} from "./models/card-list";
import {HintsList} from "./models/hints-list";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements  OnInit{
  title = 'V11 - Angular';

  public hintsFinder: HintFinder;
  public hints: HintsList;

  constructor(private controller: BoardControllerService) {
    this.hintsFinder = new HintFinder();
    this.hints = new HintsList();
  }

  ngOnInit(): void {
  }

  onClickAddCard(): void {
    this.controller.addCardsToBoard(1);
  }

  onClickShowHints(): void {
    this.hints = this.hintsFinder.getHintsFromCards(this.controller.cards);
  }

}
