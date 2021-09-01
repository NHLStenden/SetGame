import {Component, Input, OnInit} from '@angular/core';
import {Card} from "../../models/card";
import {BoardControllerService} from "../../services/board-controller.service";

@Component({
  selector: 'app-guicard',
  templateUrl: './guicard.component.html',
  styleUrls: ['./guicard.component.css' , '../card-styling.css']
})
export class GUICardComponent implements OnInit {
  @Input() card: Card;
  @Input() shapeWidth: string = '';
  @Input() shapeHeight: string = '';

  isSelected: boolean;


  public shapeURL: string;
  public shapes: number[];

  constructor(private controller: BoardControllerService) {
    this.card = new Card('', '', '',3);
    this.shapeURL = '';
    this.shapes = [];
    this.isSelected = false;
  }

  ngOnInit(): void {
    console.log(this.card);
    this.shapes = [].constructor(this.card.nrOfShapes);
    this.shapeURL = '#card_' + this.card.shape;
  }

  selectCard(event: MouseEvent) {
    let wasSelected = false;
    if (this.isSelected) {
      this.controller.unselectCard(this.card);
    }
    else{
      wasSelected = this.controller.selectCard(this.card);
    }
    // toggle
    this.isSelected = wasSelected;

  }

}
