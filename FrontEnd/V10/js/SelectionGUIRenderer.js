export class SelectionGUIRenderer {
    htmlID;

    constructor(/*string*/ htmlID) {
        this.htmlID = htmlID;
    }

    /**
     * Draws the information on the current selection on the screen
     * @param selectionInfo
     */
    render(/* SetCheckResult */ selectionInfo) {
        this.renderOneItemBoolean('colors', selectionInfo.allDiffColors, selectionInfo.allSameColors);
        this.renderOneItemBoolean('shapes', selectionInfo.allDiffShapes, selectionInfo.allDiffShapes);
        this.renderOneItemBoolean('fills', selectionInfo.allDiffFills,selectionInfo.allSameFills);
        this.renderOneItemBoolean('nrOfshapes', selectionInfo.allDiffNrOfShapes, selectionInfo.allSameNrOfShapes);
    }//render

    /**
     * Render one item based on two boolean values (same & different)
     * @param selector
     * @param value1
     * @param value2
     */
    renderOneItemBoolean(/*string*/ selector, /* bool*/ value1,  /* bool*/ value2) {
        this.renderOneItem(selector, value1 ?  "✅" : "-" , value2 ?  "✅" : "-" );
    }// renderOneItemBoolean

    renderOneItem(/*string*/ selector, /*string*/ text1, /*string*/ text2){
        const dataCells = document.querySelectorAll(`#${this.htmlID} .${selector} td`);
        dataCells[1].innerHTML = text1;
        dataCells[2].innerHTML = text2;
    }// renderOneItem

    clear() {
        this.renderOneItem('colors', '', '');
        this.renderOneItem('shapes', '', '');
        this.renderOneItem('fills', '', '');
        this.renderOneItem('nrOfshapes', '', '');
    }// clear

}