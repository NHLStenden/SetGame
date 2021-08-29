import {Component, Input, OnInit} from '@angular/core';
import {Card} from "../../models/card";

@Component({
  selector: 'app-guicard',
  templateUrl: './guicard.component.html',
  styleUrls: ['./guicard.component.css' , '../card-styling.css']
})
export class GUICardComponent implements OnInit {
  @Input() card: Card;
  @Input() shapeWidth: string = '';
  @Input() shapeHeight: string = '';


  public shapeURL: string;
  public shapes: number[];

  constructor() {
    this.card = new Card('', '', '',3);
    this.shapeURL = '';
    this.shapes = [];
  }

  ngOnInit(): void {
    console.log(this.card);
    this.shapes = [].constructor(this.card.nrOfShapes);
    this.shapeURL = '#card_' + this.card.shape;
  }

}
