import { Component, OnInit } from '@angular/core';
import {BoardControllerService} from "../../services/board-controller.service";
import {CardList} from "../../models/card-list";
import {Card} from "../../models/card";

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  public cards: CardList;

  constructor(private controller: BoardControllerService) {
    console.log('Board init');
    this.cards = new CardList(); // keep compiler happy
  }

  ngOnInit(): void {
    this.cards = this.controller.cards;
  }

  onClickAddCard(): void {
    this.controller.addCardsToBoard(1);
  }

  selectCard(event: MouseEvent, card: Card) {
    let wasSelected = false;
    if (card.isSelected) {
      this.controller.unselectCard(card);
    }
    else{
      wasSelected = this.controller.selectCard(card);
    }
    // toggle
    card.isSelected = wasSelected;
  }

}
