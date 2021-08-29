import { Component, OnInit } from '@angular/core';
import {BoardControllerService} from "../../services/board-controller.service";
import {CardList} from "../../models/card-list";

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  public cards: CardList;

  constructor(private controller: BoardControllerService) {
    console.log('Board init');
    this.cards = new CardList();

  }

  ngOnInit(): void {
    this.cards = this.controller.cards;
  }

}
