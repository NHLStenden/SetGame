import {Component, Input, OnInit} from '@angular/core';
import {HintsList} from "../../models/hints-list";

@Component({
  selector: 'app-hints',
  templateUrl: './hints.component.html',
  styleUrls: ['./hints.component.css']
})
export class HintsComponent implements OnInit {

  @Input() hints: HintsList;

  constructor() {
    this.hints = new HintsList();
  }

  ngOnInit(): void {
  }

}
