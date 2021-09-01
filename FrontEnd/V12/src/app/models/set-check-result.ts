export class SetCheckResult {
  allDiffColors: boolean;
  allDiffShapes: boolean;
  allDiffFills: boolean;
  allDiffNrOfShapes: boolean;
  allSameColors: boolean;
  allSameShapes: boolean;
  allSameFills: boolean;
  allSameNrOfShapes: boolean;

  constructor() {
    this.allDiffColors = false;
    this.allDiffShapes = false;
    this.allDiffFills = false;
    this.allDiffNrOfShapes = false;
    this.allSameColors = false;
    this.allSameShapes = false;
    this.allSameFills = false;
    this.allSameNrOfShapes = false;
  }

  get isItASet(): boolean {
    return (this.allDiffColors || this.allSameColors) &&
      (this.allDiffFills || this.allSameFills) &&
      (this.allDiffNrOfShapes || this.allSameNrOfShapes) &&
      (this.allDiffShapes || this.allSameShapes);
  }
}
