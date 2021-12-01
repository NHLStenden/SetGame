import {Component, Input, OnInit} from '@angular/core';
import {HintsList} from "../../models/hints-list";
import {HintFinder} from "../../models/hint-finder";
import {BoardControllerService} from "../../services/board-controller.service";
import {HttpService} from "../../services/http.service";

@Component({
  selector: 'app-hints',
  templateUrl: './hints.component.html',
  styleUrls: ['./hints.component.css']
})
export class HintsComponent implements OnInit {

  public hints: HintsList;
  public hintsFinder: HintFinder;

  constructor(private controller: BoardControllerService, private http: HttpService) {
    this.hintsFinder = new HintFinder(this.http);
    this.hints = new HintsList();
  }

  ngOnInit(): void {
    // this.hintsFinder = new HintFinder(this.http);
  }

  onClickShowHints(): void {
    this.hintsFinder.getHintsFromCards(this.controller.cards, this.controller.idDeck).subscribe((hints) => {
        this.hints = hints;
      }
    );
  }

}
