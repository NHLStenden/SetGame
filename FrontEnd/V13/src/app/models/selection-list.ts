import {CardList} from "./card-list";
import {SetCheckResult} from "./set-check-result";

export class SelectionList extends CardList {

  _setCheckResult: SetCheckResult;

  constructor() {
    super();
    this._setCheckResult = new SetCheckResult();
  }

  checkIfSelectionIsASet(): SetCheckResult {

    const setColors     = new Map();
    const setShapes     = new Map();
    const setFills      = new Map();
    const setNrOfShapes = new Map();

    // collect info on selected cards
    for (const cardInfo of this._cards) {
      // Register COLOR as new or update count
      if (!setColors.has(cardInfo.color)) {
        setColors.set(cardInfo.color, 1);
      } else {
        const curValue = setColors.get(cardInfo.color);
        setColors.set(cardInfo.color, curValue + 1);
      }

      // Register FILL as new or update count
      if (!setFills.has(cardInfo.fill)) {
        setFills.set(cardInfo.fill, 1);
      } else {
        const curValue = setFills.get(cardInfo.fill);
        setFills.set(cardInfo.fill, curValue + 1);
      }

      // Register SHAPE as new or update count
      if (!setShapes.has(cardInfo.shape)) {
        setShapes.set(cardInfo.shape, 1);
      } else {
        const curValue = setShapes.get(cardInfo.shape);
        setShapes.set(cardInfo.shape, curValue + 1);
      }

      // Register NROFSHAPES as new or update count
      if (!setNrOfShapes.has(cardInfo.nrOfShapes)) {
        setNrOfShapes.set(cardInfo.nrOfShapes, 1);
      } else {
        const curValue = setNrOfShapes.get(cardInfo.nrOfShapes);
        setNrOfShapes.set(cardInfo.nrOfShapes, curValue + 1);
      }
    }

    this._setCheckResult.allDiffColors     = setColors.size === 3;
    this._setCheckResult.allDiffShapes     = setShapes.size === 3;
    this._setCheckResult.allDiffFills      = setFills.size === 3;
    this._setCheckResult.allDiffNrOfShapes = setNrOfShapes.size === 3;

    this._setCheckResult.allSameColors     = setColors.size === 1;
    this._setCheckResult.allSameShapes     = setShapes.size === 1;
    this._setCheckResult.allSameFills      = setFills.size === 1;
    this._setCheckResult.allSameNrOfShapes = setNrOfShapes.size === 1;

    return this._setCheckResult;
  }// isItASet

  get setCheckResult() {
    return this._setCheckResult;
  }

}
