export class SetCheckResult {
    /* bool */ allDiffColors;
    /* bool */ allDiffShapes;
    /* bool */ allDiffFills;
    /* bool */ allDiffNrOfShapes;
    /* bool */ allSameColors;
    /* bool */ allSameShapes;
    /* bool */ allSameFills;
    /* bool */ allSameNrOfShapes;

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

    get isItASet() /* bool */ {
        return (this.allDiffColors || this.allSameColors) &&
            (this.allDiffFills || this.allSameFills) &&
            (this.allDiffNrOfShapes || this.allSameNrOfShapes) &&
            (this.allDiffShapes || this.allSameShapes);
    }
}