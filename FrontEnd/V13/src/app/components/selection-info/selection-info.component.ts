import {Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {BoardControllerService} from "../../services/board-controller.service";
import {SelectionList} from "../../models/selection-list";
import {SetCheckResult} from "../../models/set-check-result";

@Component({
  selector: 'app-selection-info',
  templateUrl: './selection-info.component.html',
  styleUrls: ['./selection-info.component.css']
})
export class SelectionInfoComponent implements OnInit {

  checkResult: SetCheckResult | undefined;


  constructor(private controller: BoardControllerService) {
  }

  ngOnInit(): void {
    this.controller.checkedForSelection$.subscribe((results) => {
      console.log('SelectionInfoComponent::Selection changed');
      this.checkResult = results;
    });
  }
}
